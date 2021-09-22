using AutoMapper;
using com.allcard.common;
using com.allcard.institution.common;
using com.allcard.institution.common.ViewModel;
using com.allcard.institution.models;
using com.allcard.institution.repository;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.services
{
    public interface IBranchScheduleMemberService
    {
        Task<responseVM> ReserveSlot(requestVM payload, bool IsSave, Guid userID);
        Task<responseVM> ConfirmOTP(requestVM payload, bool IsSave, Guid userID);
        Task<responseVM> ScanCode(requestVM payload, bool IsSave, Guid userID);

        Task<responseVM> ScanIn(requestVM payload, bool IsSave, Guid userID);
        Task<responseVM> ScanOut(requestVM payload, bool IsSave, Guid userID);
    }
    public class BranchScheduleMemberService : IBranchScheduleMemberService
    {
        protected string _audience;
        protected string _subject;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly QueConfiguration _appSettings;

        public BranchScheduleMemberService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<QueConfiguration> appSettings)
        {
            _audience = "Branch Schedule";
            _subject = "Branch Schedule";
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<responseVM> ReserveSlot(requestVM payload, bool IsSave, Guid userID)
        {
            // Save member
            // Generate OTP
            // Save Branch Schedule Member
            // Send SMS
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;

                var user = await _unitOfWork.UserProfileRepository.GetByGuid(userID);
                try
                {
                    await ValidateReserve(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    var entity = JsonConvert.DeserializeObject<reserveSlotVM>(payload.Data.ToString());
                    var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);
                    var memberAge = (DateTime.Now.Year - entity.BirthDate.Year);

                    var member = new Member();
                    member.CCANo = entity.CCANo;
                    member.CIF = entity.CIF;
                    member.FirstName = entity.FirstName;
                    member.MiddleName = entity.MiddleName;
                    member.LastName = entity.LastName;
                    member.Suffix = entity.Suffix;

                    string middleName = " ";
                    if (!string.IsNullOrEmpty(entity.MiddleName)) middleName += entity.MiddleName + " ";
                    member.FullName = string.Format("{0}{1}{2}{3}",entity.FirstName, middleName, entity.LastName,entity.Suffix).Trim();
                    member.MobileNumber = entity.MobileNumber;
                    member.Birthdate = entity.BirthDate;
                    //member.BirthPlace = entity.BirthPlace;
                    member.Email = entity.Email;
                    member.CreatedBy = user.ID;
                    member.UpdatedBy = user.ID;
                    await _unitOfWork.MemberRepository.AddAsyn(member);


                    var scheduleMember = new BranchScheduleMember();
                    scheduleMember.BranchScheduleID = schedule.ID;
                    scheduleMember.MemberID = member.ID;
                    scheduleMember.IsSenior = memberAge < _appSettings.SeniorAge ? false : true;
                    scheduleMember.IsVerified = false;
                    scheduleMember.OTP = Utilities.RandomString(6);
                    scheduleMember.OTPExpiry = DateTime.Now.AddMinutes(_appSettings.OTPExipryMinutes);
                    scheduleMember.RefNumber = await GenerateRefNum();
                    scheduleMember.CreatedBy = user.ID;
                    scheduleMember.UpdatedBy = user.ID;


                    await _unitOfWork.BranchScheduleMemberRepository.AddAsyn(scheduleMember);
                    await _unitOfWork.Save();
                    /// Send SMS.
                    var branch = await _unitOfWork.BranchRepository.GetAsync(schedule.BranchID);
                    var institution = await _unitOfWork.InstitutionRepository.Get(branch.InstitutionID);

                    var emailMessage = string.Format("Schedule:<b>{0} {1}-{2} @ {3}</b> <br/> Please click verify button to get your control number. ", schedule.Date.ToString("yyyy MMMM dd"), schedule.StartTime.ToString("hh:mm tt"),schedule.EndTime.ToString("hh:mm tt"), branch.Name);
                    var subject = string.Format("[{0} {1}] Reservation",institution.Name,branch.Name);
                    MailClient.SendConfirm(subject,member.Email,  scheduleMember.RefNumber, scheduleMember.OTP, entity.RedirectURL, emailMessage, _appSettings);

                    //string message = string.Format("Your One-Time PIN(OTP) is {0}, please enter within 5 minute(s)", scheduleMember.OTP);
                    //SMSClient.Send(entity.MobileNumber, message);
                    

                    var result = _mapper.Map<branchScheduleMemberVM>(scheduleMember);
                    result.RefNumber = string.Empty;

                    response.Data = result;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_RESERVE_SLOT, user.ID);

                return response;
            }
        }
        public async Task<responseVM> ValidateReserve(requestVM payload, responseVM response)
        {
            var entity = new reserveSlotVM();
            try
            {
                entity = JsonConvert.DeserializeObject<reserveSlotVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!", response.Audience);
            }


            var memberAge = (DateTime.Now.Year - entity.BirthDate.Year);
          


            var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);
            var emailSchedule = await _unitOfWork.BranchScheduleMemberRepository.GetEmailSchedule(entity.Email, schedule.Date);
            if (schedule == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Schedule does not exist!", response.Audience);
            }
            else if (schedule.Status == Constants.STATUS_FINISH)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Your schedule's done!", response.Audience);
            }
            else if (schedule.Status == Constants.STATUS_CANCELLED)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Schedule cancelled.", response.Audience);
            }
            else if (schedule.Status == Constants.STATUS_FULL)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Scheduled has been filled.", response.Audience);
            }
            else if (schedule.IsSenior && memberAge < _appSettings.SeniorAge)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Schedule is for senior citizen only.", response.Audience);
            }
            else if (schedule.Date.Date < DateTime.Now.Date)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Schedule has ended.", response.Audience);
            }
            //else if (await _unitOfWork.BranchScheduleMemberRepository.CountEmailSchedule(entity.Email, schedule.Date) >= 1)
            //{
            //    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
            //    response.ResultMessage = string.Format("Customer can reserve once a day only.", response.Audience);
            //}
            else if (emailSchedule!= null && emailSchedule.IsVerified)
            {
                var emailSched = await _unitOfWork.BranchScheduleRepository.GetAsync(emailSchedule.BranchScheduleID);
                if (emailSched.Status == Constants.STATUS_OPEN || emailSched.Status == Constants.STATUS_FULL || emailSched.Status == Constants.STATUS_FINISH)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("You can only reserve once a day.", response.Audience);
                }
            }
            else if (schedule.EndTime < DateTime.Now)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Schedule has ended.", response.Audience);
            }

            return response;
        }



        private async Task<string> GenerateRefNum()
        {
            var genrandom = Utilities.RandomString(8);

            if (await _unitOfWork.BranchScheduleMemberRepository.IsRefNumberExist(genrandom))
            {
               return await GenerateRefNum();
            }
            return genrandom;
        }
        public async Task<responseVM> ConfirmOTP(requestVM payload, bool IsSave, Guid userID)
        {
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                var user = await _unitOfWork.UserProfileRepository.GetByGuid(userID);

                try
                {
                    await ValidateOTP(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    var entity = JsonConvert.DeserializeObject<confirmOTPVM>(payload.Data.ToString());
                    //var schedulemember = await _unitOfWork.BranchScheduleMemberRepository.GetByGuid(entity.ReserveCode);
                    var schedulemember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);
                    var schedule = await _unitOfWork.BranchScheduleRepository.GetAsync(schedulemember.BranchScheduleID);
                    // Update And Verify


                    if (!schedulemember.IsVerified)
                    {
                        schedulemember.IsVerified = true;
                        schedulemember.VerifiedDate = DateTime.Now;
                        schedulemember.UpdatedBy = user.ID;
                        await _unitOfWork.BranchScheduleMemberRepository.UpdateAsyn(schedulemember, schedulemember.ID);
                    }

                    // Check if full change status.
                    var verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(schedule.ID);

                    if (verifiedCount >= schedule.MaxPersonCount)
                    {
                        schedule.Status = Constants.STATUS_FULL;
                        await _unitOfWork.BranchScheduleRepository.UpdateAsyn(schedule, schedule.ID);
                    }


                    response.Data = schedulemember.RefNumber;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_CONFIRM_OTP, user.ID);

                return response;
            }
        }

        public async Task<responseVM> ValidateOTP(requestVM payload, responseVM response)
        {
            var entity = new confirmOTPVM();
            try
            {
                entity = JsonConvert.DeserializeObject<confirmOTPVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!", response.Audience);
            }
            var schedulemember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);

            if (schedulemember == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid request verification failed.", response.Audience);
            }
            else
            {
                var schedule = await _unitOfWork.BranchScheduleRepository.GetAsync(schedulemember.BranchScheduleID);
                var verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(schedule.ID);
                var member = await _unitOfWork.MemberRepository.GetAsync(schedulemember.MemberID);
                var emailSchedule = await _unitOfWork.BranchScheduleMemberRepository.GetEmailSchedule(member.Email, schedule.Date);
                //if (schedulemember.IsVerified)
                //{
                //    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                //    response.ResultMessage = string.Format("Already verified!.", response.Audience);
                //}
                //else 
                if (!schedulemember.IsVerified && schedulemember.OTPExpiry <= DateTime.Now)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Verification expired!.", response.Audience);
                }
                else if (!schedulemember.IsVerified && schedulemember.OTP.ToUpper() != entity.OTP.ToUpper())
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Invalid One-Time PIN!", response.Audience);
                }
                else if (schedule == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule does not exist!", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_FINISH)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Your schedule's done!", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_CANCELLED)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule cancelled.", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_FULL)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Scheduled has been filled.", response.Audience);
                }
                else if (verifiedCount >= schedule.MaxPersonCount)
                {
                    schedule.Status = Constants.STATUS_FULL;
                    await _unitOfWork.BranchScheduleRepository.UpdateAsyn(schedule, schedule.ID);

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Scheduled has been filled.", response.Audience);
                }
                else if (schedule.Date.Date < DateTime.Now.Date)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule has ended.", response.Audience);
                }
                //else if (!schedulemember.IsVerified && await _unitOfWork.BranchScheduleMemberRepository.CountEmailSchedule(member.Email, schedule.Date) >= 1)
                //{
                //    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                //    response.ResultMessage = string.Format("Customer can reserve once a day only.", response.Audience);
                //}
                else if (emailSchedule != null && emailSchedule.IsVerified && emailSchedule.RefNumber != entity.ReserveCode)
                {
                    var emailSched = await _unitOfWork.BranchScheduleRepository.GetAsync(emailSchedule.BranchScheduleID);
                    if (emailSched.Status == Constants.STATUS_OPEN || emailSched.Status == Constants.STATUS_FULL || emailSched.Status == Constants.STATUS_FINISH)
                    {
                        response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                        response.ResultMessage = string.Format("You can only reserve once a day.", response.Audience);
                    }
                }
                else if (schedule.EndTime < DateTime.Now)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule has ended.", response.Audience);
                }

                // Validate if not status not full.
                // Validate Count if not yet full
            }





            return response;
        }

        public async Task<responseVM> ScanCode(requestVM payload, bool IsSave, Guid userID)
        {
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;

                var user =  await _unitOfWork.UserProfileRepository.GetByGuid(userID);

                try
                {
                    await ValidateScanCode(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;
                    var entity = JsonConvert.DeserializeObject<scanCodeVM>(payload.Data.ToString());

                    //var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByGuid(entity.ReserveCode);
                    var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);


                    var member = await _unitOfWork.MemberRepository.GetAsync(scheduleMember.MemberID);

                    var scanCodeResponse = new scanCodeResponseVM();
                    scanCodeResponse.Member = _mapper.Map<memberVM>(member);
                    scanCodeResponse.TimeIn = scheduleMember.TimeIn;
                    scanCodeResponse.TimeOut = scheduleMember.TimeOut;

                    response.Data = scanCodeResponse;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {
                    
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_SCAN_CODE, user.ID);

                return response;
            }
        }

        public async Task<responseVM> ValidateScanCode(requestVM payload, responseVM response)
        {
            var entity = new scanCodeVM();
            try
            {
                entity = JsonConvert.DeserializeObject<scanCodeVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!", response.Audience);
            }


            var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);
            if (scheduleMember == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid QR Code!", response.Audience);
            }
            else
            {
                var schedule = await _unitOfWork.BranchScheduleRepository.GetAsync(scheduleMember.BranchScheduleID);
                var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.BranchCode);

               

                var todayTime = DateTime.Now;
                if (!scheduleMember.IsVerified)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Member is not verified!", response.Audience);
                }
                else if (schedule == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule not found!", response.Audience);
                }
                else if (branch == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Branch not found!", response.Audience);
                }
                else if (schedule.IsSenior && !scheduleMember.IsSenior)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Member is not a senior citizen!", response.Audience);
                }
                else if (schedule.BranchID != branch.ID)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Branch is not match.", branch.Name);
                }
                //else if (todayTime < schedule.StartTime)
                //{
                //    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                //    response.ResultMessage = string.Format("Member schedule is not yet started. {0} to {1}.", schedule.StartTime, schedule.EndTime);
                //}
                //else if (todayTime > schedule.EndTime)
                //{
                //    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                //    response.ResultMessage = string.Format("Member schedule has already ended. {0} to {1}!.", schedule.StartTime, schedule.EndTime);
                //}

            }
            return response;
        }


        public async Task<responseVM> ScanIn(requestVM payload, bool IsSave, Guid userID)
        {
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                var user = await _unitOfWork.UserProfileRepository.GetByGuid(userID);
                try
                {
                    await ValidateScanCode(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    await ValidateIn(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<scanCodeVM>(payload.Data.ToString());
                    //var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByGuid(entity.ReserveCode);
                    var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);

                    scheduleMember.TimeIn = DateTime.Now;
                    scheduleMember.UpdatedBy = user.ID;

                    await _unitOfWork.BranchScheduleMemberRepository.UpdateAsyn(scheduleMember, scheduleMember.ID);


                    response.Data = scheduleMember.TimeIn;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }


                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_SCAN_IN, user.ID);


                return response;
            }
        }

        public async Task<responseVM> ValidateIn(requestVM payload, responseVM response)
        {
            var entity = JsonConvert.DeserializeObject<scanCodeVM>(payload.Data.ToString());
            var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);
            if (scheduleMember == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid control number.", scheduleMember.TimeIn);
            }
            else
            {
                var scheduler = await _unitOfWork.BranchScheduleRepository.GetAsync(scheduleMember.BranchScheduleID);
                if (scheduleMember.TimeIn != null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Member already time-in last {0}.", scheduleMember.TimeIn);
                }
                else if (scheduler.Status == Constants.STATUS_CANCELLED)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Sorry,the schedule was cancelled.", scheduleMember.TimeIn);
                }
                else if (scheduler.Status == Constants.STATUS_FINISH)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Your schedule's done!", scheduleMember.TimeIn);
                }
                else if (DateTime.Now < scheduler.StartTime)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule is not yet started. {0} {1} to {2}.", scheduler.Date.ToString("yyyy MMMM dd"),scheduler.StartTime.ToString("hh:mm tt"), scheduler.EndTime.ToString("hh:mm tt"));
                }
                else if (DateTime.Now > scheduler.EndTime)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule has ended.", scheduler.StartTime, scheduler.EndTime);
                }
            }
            return response;
        }


        public async Task<responseVM> ScanOut(requestVM payload, bool IsSave, Guid userID)
        {
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;

                var user = await _unitOfWork.UserProfileRepository.GetByGuid(userID);
                try
                {
                    await ValidateScanCode(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    await ValidateOut(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<scanCodeVM>(payload.Data.ToString());
                    var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);
                    scheduleMember.TimeOut = DateTime.Now;
                    scheduleMember.UpdatedBy = user.ID;

                    await _unitOfWork.BranchScheduleMemberRepository.UpdateAsyn(scheduleMember, scheduleMember.ID);


                    response.Data = scheduleMember.TimeOut;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_SCAN_OUT, user.ID);
                return response;
            }
        }

        public async Task<responseVM> ValidateOut(requestVM payload, responseVM response)
        {
            var entity = JsonConvert.DeserializeObject<scanCodeVM>(payload.Data.ToString());
            var scheduleMember = await _unitOfWork.BranchScheduleMemberRepository.GetByRefNum(entity.ReserveCode);
            var schedule = await _unitOfWork.BranchScheduleRepository.GetAsync(scheduleMember.BranchScheduleID);
            if (scheduleMember == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid control number.", scheduleMember.TimeIn);
            }
            else
            {
                if (scheduleMember.TimeIn == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Member dont have time-in details.");
                }
                else if (scheduleMember.TimeOut != null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Member has already time-out last {0}.", scheduleMember.TimeOut);
                }
                else if (DateTime.Now < schedule.StartTime)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule is not yet started. {0} to {1}.", schedule.StartTime, schedule.EndTime);
                }

            }
            return response;
        }

    }
}
