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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.services
{
    public interface IBranchScheduleService
    {
        Task<responseVM> GenerateSchedule(requestVM payload, bool IsSave, Guid userID);

        Task<responseVM> CancelSchedule(requestVM payload, bool IsSave, Guid userID);
        Task<responseVM> UpdateSchedule(requestVM payload, bool IsSave, Guid userID);

        Task<responseVM> GetAvailableSchedule(requestVM payload, bool IsSave);
        Task<responseVM> GetScheduleAvailableCount(requestVM payload, bool IsSave);


        Task<responseVM> FinishSchedule(requestVM payload, bool IsSave, Guid userID);
        Task<responseVM> GetAllSchedule(requestVM payload, bool IsSave, Guid userID);
        Task<responseVM> GetBoard(requestVM payload, bool IsSave);
      
    }
    public class BranchScheduleService : IBranchScheduleService
    {
        protected string _audience;
        protected string _subject;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly QueConfiguration _appSettings;

        public BranchScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<QueConfiguration> appSettings)
        {
            _audience = "Branch Schedule";
            _subject = "Branch Schedule";
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        public async Task<responseVM> GenerateSchedule(requestVM payload, bool IsSave, Guid userID)
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
                    await ValidateGenerate(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<generateScheduleVM>(payload.Data.ToString());
                    var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.BranchCode);

                    // To make sure same date ,start and end
                    entity.Start = new DateTime(entity.Date.Year, entity.Date.Month, entity.Date.Day, entity.Start.Hour, entity.Start.Minute, 000);
                    entity.End = new DateTime(entity.Date.Year, entity.Date.Month, entity.Date.Day, entity.End.Hour, entity.End.Minute, 000);


                    DateTime newStartTime = entity.Start;
                    DateTime newEndTime = entity.Start;
                    newEndTime = newEndTime.AddHours(entity.HoursCount).AddSeconds(-1);


                    DateTime nextStartHours = newStartTime;
                    while (entity.End > newStartTime)
                    {

                        nextStartHours = newStartTime.AddHours(entity.HoursCount);


                        // Insert Schedule.
                        var branchSchedule = new BranchSchedule();
                        branchSchedule.Date = entity.Date;
                        branchSchedule.MaxPersonCount = entity.PersonCount;
                        branchSchedule.StartTime = newStartTime;
                        branchSchedule.EndTime = newEndTime;
                        branchSchedule.IsSenior = entity.IsSenior;
                        branchSchedule.Status = Constants.STATUS_OPEN;
                        branchSchedule.StoreOpen = branch.StoreHoursOpen;
                        branchSchedule.StoreClose = branch.StoreHoursClose;
                        branchSchedule.InstitutionID = branch.InstitutionID;
                        branchSchedule.Remarks = string.Empty;
                        branchSchedule.BranchID = branch.ID;
                        branchSchedule.CreatedBy = user.ID;
                        branchSchedule.UpdatedBy = user.ID;
                        //await ValidateInsert(branchSchedule, response);

                        try
                        {
                            if (!await _unitOfWork.BranchScheduleRepository.IsConflictSchedule(branchSchedule))
                            {
                                await _unitOfWork.BranchScheduleRepository.AddAsyn(branchSchedule);
                                await _unitOfWork.Save();
                                // save it
                            }
                        }
                        catch (Exception ex)
                        {
                            // Just continue.
                            var error = ex;
                        }

                        // Compute next
                        newStartTime = nextStartHours;
                        newEndTime = nextStartHours.AddHours(entity.HoursCount).AddSeconds(-1);
                        // so end exactly when
                        if (newEndTime > entity.End)
                        {
                            newEndTime = entity.End.AddSeconds(-1);
                        }
                    }

                    response.Data = null;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Successfully created schedule!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_GENERATE_SCHEDULE, user.ID);

                return response;
            }
        }

        public async Task<responseVM> ValidateGenerate(requestVM payload, responseVM response)
        {
            var entity = new generateScheduleVM();
            try
            {
                entity = JsonConvert.DeserializeObject<generateScheduleVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }

            var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.BranchCode);
            if (branch == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid branch!.", response.Audience);
            }
            else if (entity.Date.Date < DateTime.Now.Date)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Previous date is not allowed.", response.Audience);
            }



            entity.Start = new DateTime(entity.Date.Year, entity.Date.Month, entity.Date.Day, entity.Start.Hour, entity.Start.Minute, 000);
            entity.End = new DateTime(entity.Date.Year, entity.Date.Month, entity.Date.Day, entity.End.Hour, entity.End.Minute, 000);

            bool HasScheduleConflict = false;

            DateTime newStartTime = entity.Start;
            DateTime newEndTime = entity.Start;
            newEndTime = newEndTime.AddHours(entity.HoursCount).AddSeconds(-1);
      
            DateTime nextStartHours = newStartTime;
            while (entity.End > newStartTime)
            {

                nextStartHours = newStartTime.AddHours(entity.HoursCount);

                var branchSchedule = new BranchSchedule();
                branchSchedule.Date = entity.Date;
                branchSchedule.StartTime = newStartTime;
                branchSchedule.EndTime = newEndTime;


                if (await _unitOfWork.BranchScheduleRepository.IsConflictSchedule(branchSchedule))
                {
                    HasScheduleConflict = true;
                    break;
                }

                // Compute next
                newStartTime = nextStartHours;
                newEndTime = nextStartHours.AddHours(entity.HoursCount).AddSeconds(-1);
                // so end exactly when
                if (newEndTime > entity.End)
                    newEndTime = entity.End;
            }

            if (HasScheduleConflict)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Schedule is existing.", response.Audience);
                
            }




            return response;
        }

        public async Task<responseVM> GetAvailableSchedule(requestVM payload, bool IsSave)
        {
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;

                try
                {
                    await ValidateGetAvaialable(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<getAvailableSchedule>(payload.Data.ToString());
                    var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.BranchCode);
                    var dateParam = entity.Date == null ? DateTime.Now.Date : entity.Date.Value;
                    var result = await _unitOfWork.BranchScheduleRepository.GetAvailableSchedule(branch.ID, dateParam);

                    var list = new List<branchScheduleVM>();
                    foreach (var item in result)
                    {
                        int verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(item.ID);
                        var sched = _mapper.Map<branchScheduleVM>(item);
                        sched.AvailableSlot = sched.MaxPersonCount - verifiedCount;
                        sched.VerifiedCount = verifiedCount;
                        list.Add(sched);
                    }
                    response.Data = list;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("{0} has been success!.", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

  
                return response;
            }
        }

        public async Task<responseVM> ValidateGetAvaialable(requestVM payload, responseVM response)
        {
            var entity = new getAvailableSchedule();
            try
            {
                entity = JsonConvert.DeserializeObject<getAvailableSchedule>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }

            var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.BranchCode);
            if (branch == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid branch!.", response.Audience);
            }
            else if (entity.Date != null)
            {

                if (entity.Date.Value.Date < DateTime.Now.Date)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Previous date is not allowed.", response.Audience);
                }
            }
            return response;
        }


        public async Task<responseVM> GetScheduleAvailableCount(requestVM payload, bool IsSave)
        {
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;

                try
                {

                    var entity = JsonConvert.DeserializeObject<getScheduleAvailableCount>(payload.Data.ToString());
                    var result = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);

                    var remaining = result.MaxPersonCount - await _unitOfWork.BranchScheduleMemberRepository.CountVerified(result.ID);
                    response.Data = remaining < 0 ? 0 : remaining;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("{0} has been success!.", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                return response;
            }
        }

        public async Task<responseVM> CancelSchedule(requestVM payload, bool IsSave, Guid userID)
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
                    await ValidateCancellation(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<cancelScheduleVM>(payload.Data.ToString());

                    if (entity.Date == null)
                    {
                        var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);
                        var branch = await _unitOfWork.BranchRepository.GetAsync(schedule.BranchID);
                        var institution = await _unitOfWork.InstitutionRepository.Get(branch.InstitutionID);

                        schedule.Status = Constants.STATUS_CANCELLED;
                        schedule.Remarks = string.Format("Store Messasge:{0}",entity.Message);
                        schedule.UpdatedBy = user.ID;
                        await _unitOfWork.BranchScheduleRepository.UpdateAsyn(schedule, schedule.ID);


                        var scheduleMembers = await _unitOfWork.BranchScheduleMemberRepository.GetByScheduleID(schedule.ID);
                        foreach (var memsched in scheduleMembers)
                        {
                            var description = string.Format("Control number:<b>{0}</b> <br/>{3} {1}-{2}.", memsched.RefNumber, schedule.StartTime.ToString("hh:mm tt"), schedule.EndTime.ToString("hh:mm tt"),schedule.Date.ToString("yyyy MMMM dd"));
                            var subject = string.Format("[{0} {1}] Reservation", institution.Name, branch.Name);
                            MailClient.SendCancellation(subject,memsched.Member.Email, branch.Name, description, entity.Message, _appSettings);
                        }

                    }
                    else
                    {
                        var schedules = await _unitOfWork.BranchScheduleRepository.GetAllByDate(entity.Date.Value);
                        var cancelCount = schedules.Where(a => a.Status == Constants.STATUS_OPEN || a.Status == Constants.STATUS_FULL);

                        foreach (var sched in cancelCount)
                        {
                            // allow only futre date & open full.
                            if ((sched.Status == Constants.STATUS_OPEN || sched.Status == Constants.STATUS_FULL))
                            {
                                var branch = await _unitOfWork.BranchRepository.GetAsync(sched.BranchID);
                                var institution = await _unitOfWork.InstitutionRepository.Get(branch.InstitutionID);
                                sched.Status = Constants.STATUS_CANCELLED;
                                sched.Remarks = string.Format("Store Messasge:{0}", entity.Message);
                                sched.UpdatedBy = user.ID;
                                await _unitOfWork.BranchScheduleRepository.UpdateAsyn(sched, sched.ID);

                                var scheduleMembers = await _unitOfWork.BranchScheduleMemberRepository.GetByScheduleID(sched.ID);
                                foreach (var memsched in scheduleMembers)
                                {
                                    var description = string.Format("Control number:<b>{0}</b> <br/>{3} {1}-{2}.", memsched.RefNumber, sched.StartTime.ToString("hh:mm tt"), sched.EndTime.ToString("hh:mm tt"), sched.Date);
                                    var subject = string.Format("[{0} {1}] Reservation", institution.Name, branch.Name);
                                    MailClient.SendCancellation(subject,memsched.Member.Email, branch.Name, description, entity.Message, _appSettings);
                                }
                            }
                        }
                    }



                    response.Data = null;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_CANCEL_SCHEDULE, user.ID);
                return response;
            }
        }

        public async Task<responseVM> ValidateCancellation(requestVM payload, responseVM response)
        {
            var entity = new cancelScheduleVM();
            try
            {
                entity = JsonConvert.DeserializeObject<cancelScheduleVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }

            if (entity.Date != null)
            {
                var openCount = await _unitOfWork.BranchScheduleRepository.GetCountByStatus(entity.Date.Value, Constants.STATUS_OPEN);
                if (entity.Date.Value.Date < DateTime.Now.Date)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Sorry you cannot cancel previous date.", response.Audience);
                }
                else if (openCount == 0)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("No schedule available.", response.Audience);
                }
                else if (entity.Message == null || entity.Message == "")
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Message is required!, This will be sent to reserved member(s).");
                }
            }
            else
            {
                var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);
                if (schedule == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule not found!.", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_FINISH)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Your schedule's done!.", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_CANCELLED)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule have been cancelled.", response.Audience);
                }
                else if (schedule.Date < DateTime.Now.Date)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Sorry, previous date cannot be cancelled.", response.Audience);
                }
                else if (entity.Message == null || entity.Message == "")
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Message is required!, This will be sent to reserved member(s).");
                }
                // schedule that already pass is not allowed to be cancelled.
                else if (schedule.EndTime < DateTime.Now)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Sorry, previous date cannot be cancelled.");
                }
            }


            return response;
        }

        public async Task<responseVM> GetAllSchedule(requestVM payload, bool IsSave, Guid userID)
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

                    await ValidateGetSchedule(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    var entity = JsonConvert.DeserializeObject<getBranchScheduleVM>(payload.Data.ToString());
                    var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.branchCode);


                    var dateFrom = new DateTime(entity.From.Value.Year, entity.From.Value.Month, entity.From.Value.Day, 0,0,0);
                    var dateTo = new DateTime(entity.To.Value.Year, entity.To.Value.Month, entity.To.Value.Day, 23, 59, 59);


                    var schedules = await _unitOfWork.BranchScheduleRepository.Search(entity.page -1, entity.row, branch.ID, dateFrom, dateTo);
                    var totalCount = await _unitOfWork.BranchScheduleRepository.SearchCount(branch.ID, dateFrom, dateTo);
                    var pageCount = totalCount == 0 ? 0 : await _unitOfWork.BranchScheduleRepository.SearchPageCount(branch.ID, entity.row, dateFrom, dateTo);

                    var newList = new List<branchScheduleVM>();
                    foreach (var sched in schedules)
                    {
                        var verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(sched.ID);
                        var branchSched = _mapper.Map<branchScheduleVM>(sched);
                        branchSched.AvailableSlot = branchSched.MaxPersonCount - verifiedCount;
                        branchSched.VerifiedCount = verifiedCount;
                        newList.Add(branchSched);
                    }

                    var result = new paginationVM
                    {
                        List = newList,
                        PageCount = pageCount,
                        TotalCount = totalCount
                    };

                    response.Data = result;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("{0} has been success!.", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }
                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_GET_SCHEDULE, user.ID);


                return response;
            }
        }

        public async Task<responseVM> ValidateGetSchedule(requestVM payload, responseVM response)
        {
            var entity = new getBranchScheduleVM();
            try
            {
                entity = JsonConvert.DeserializeObject<getBranchScheduleVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.branchCode);
            if (branch == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid branch!", response.Audience);
            }
            else if (entity.From == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("From date is required", response.Audience);
            }
            else if (entity.To == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("To date is required", response.Audience);
            }

            return response;
        }

        public async Task<responseVM> GetBoard(requestVM payload, bool IsSave)
        {
            using (responseVM response = new responseVM())
            {

                response.JTI = Guid.NewGuid();
                response.Expiration = Utilities.GetTimestamp(DateTime.UtcNow.AddHours(2));
                response.Date = Utilities.GetTimestamp(DateTime.UtcNow);
                response.Audience = _audience;
                response.Subject = _subject;
                response.ResultCode = Constants.RESULT_CODE_SUCCESS;

                try
                {

                    await ValidateGetBoard(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    var entity = JsonConvert.DeserializeObject<getbranchScheduleBoardVM>(payload.Data.ToString());
                    var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.BranchCode);
                    
                    var list = new List<branchScheduleBoardVM>();
                    var openSchedule = await _unitOfWork.BranchScheduleRepository.GetAllByDate(branch.ID,entity.Date,Constants.STATUS_OPEN);
                    foreach (var op in openSchedule)
                    {
                        var verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(op.ID);
                        var sched = new branchScheduleBoardVM
                        {
                            Status = op.Status,
                            StartTime = op.StartTime,
                            EndTime = op.EndTime,
                            TotalSlots = op.MaxPersonCount,
                            ScheduleCode = op.GUID,
                            AvailableSlot = op.MaxPersonCount - verifiedCount,
                            VerifiedCount = verifiedCount,
                            IsSenior = op.IsSenior
                        };
                        list.Add(sched);
                    }
                    var fullSchedule = await _unitOfWork.BranchScheduleRepository.GetAllByDate(branch.ID, entity.Date, Constants.STATUS_FULL);
                    foreach (var full in fullSchedule)
                    {
                        var verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(full.ID);
                        var sched = new branchScheduleBoardVM
                        {
                            Status = full.Status,
                            StartTime = full.StartTime,
                            EndTime = full.EndTime,
                            TotalSlots = full.MaxPersonCount,
                            ScheduleCode = full.GUID,
                            AvailableSlot = full.MaxPersonCount - verifiedCount,
                            VerifiedCount = verifiedCount,
                            IsSenior = full.IsSenior
                            
                        };
                        list.Add(sched);
                    }

                    response.Data = list;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("{0} has been success!.", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                return response;
            }
        }
        public async Task<responseVM> ValidateGetBoard(requestVM payload, responseVM response)
        {
            var entity = new getbranchScheduleBoardVM();
            try
            {
                entity = JsonConvert.DeserializeObject<getbranchScheduleBoardVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.BranchCode);
            if (branch == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid branch!", response.Audience);
            }
            return response;
        }

        public async Task<responseVM> UpdateSchedule(requestVM payload, bool IsSave, Guid userID)
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

                    await ValidateUpdateSchedule(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<updateScheduleVM>(payload.Data.ToString());
                    var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);
                    schedule.MaxPersonCount = entity.MaxPersonCount;
                    schedule.UpdatedBy = user.ID;
                    //schedule.IsSenior = entity.IsSenior;
                    //schedule.StartTime = entity.StartTime;
                    //schedule.EndTime = entity.EndTime;
                    //schedule.Date = entity.Date;
                    await _unitOfWork.BranchScheduleRepository.UpdateAsyn(schedule, schedule.ID);


                    response.Data = null;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }
                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_CANCEL_SCHEDULE, user.ID);
                return response;
            }
        }
        public async Task<responseVM> ValidateUpdateSchedule(requestVM payload, responseVM response)
        {
            var entity = new updateScheduleVM();
            try
            {
                entity = JsonConvert.DeserializeObject<updateScheduleVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }

            var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);
           

            if (schedule == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid schedule!", response.Audience);
            }
            else
            {
                schedule.MaxPersonCount = entity.MaxPersonCount;
                //schedule.IsSenior = entity.IsSenior;
                //schedule.StartTime = entity.StartTime;
                //schedule.EndTime = entity.EndTime;
                //schedule.Date = entity.Date;


                var verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(schedule.ID);

                var branch = await _unitOfWork.BranchRepository.GetAsync(schedule.BranchID);
                if (branch == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Invalid branch!", response.Audience);
                }
                //else if (await _unitOfWork.BranchScheduleRepository.IsConflictSchedule(schedule))
                //{
                //    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                //    response.ResultMessage = string.Format("Schedule conflict!", response.Audience);
                //}
                else if (schedule.Status == Constants.STATUS_FINISH)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Your schedule's done!", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_CANCELLED)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Schedule have been cancelled.", response.Audience);
                }
                else if (schedule.Date.Date < DateTime.Now.Date)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Previous date is not allowed.", response.Audience);
                }
                else if (verifiedCount > entity.MaxPersonCount)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Max person count cannot be lower than reserve member ({0}).", verifiedCount);
                }
            }
           
            return response;
        }

        public async Task<responseVM> FinishSchedule(requestVM payload, bool IsSave, Guid userID)
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

                    await ValidateFinishSchedule(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<finishScheduleVM>(payload.Data.ToString());
                    var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);
                    schedule.Status = Constants.STATUS_FINISH;
                    schedule.UpdatedBy = user.ID;
                    await _unitOfWork.BranchScheduleRepository.UpdateAsyn(schedule, schedule.ID);


                    response.Data = null;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("Success!", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }
                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_CANCEL_SCHEDULE, user.ID);
                return response;
            }
        }
        public async Task<responseVM> ValidateFinishSchedule(requestVM payload, responseVM response)
        {
            var entity = new finishScheduleVM();
            try
            {
                entity = JsonConvert.DeserializeObject<finishScheduleVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }

            var schedule = await _unitOfWork.BranchScheduleRepository.GetByGuid(entity.ScheduleCode);


            if (schedule == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid schedule!", response.Audience);
            }
            else
            {
                var branch = await _unitOfWork.BranchRepository.GetAsync(schedule.BranchID);
                if (branch == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Invalid branch!", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_FINISH)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("This schedule has already finished.", response.Audience);
                }
                else if (schedule.Status == Constants.STATUS_CANCELLED)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("This schedule has already cancelled.", response.Audience);
                }
                else if (schedule.Date.Date > DateTime.Now.Date)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Future date is not allowed.", response.Audience);
                }
            }

            return response;
        }


        public async Task<responseVM> LoadSchedule(requestVM payload, bool IsSave, Guid userID)
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

                    await ValidateGetSchedule(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    var entity = JsonConvert.DeserializeObject<getBranchScheduleVM>(payload.Data.ToString());
                    var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.branchCode);


                    var dateFrom = new DateTime(entity.From.Value.Year, entity.From.Value.Month, entity.From.Value.Day, 0, 0, 0);
                    var dateTo = new DateTime(entity.To.Value.Year, entity.To.Value.Month, entity.To.Value.Day, 23, 59, 59);


                    var schedules = await _unitOfWork.BranchScheduleRepository.Search(entity.page - 1, entity.row, branch.ID, dateFrom, dateTo);
                    var totalCount = await _unitOfWork.BranchScheduleRepository.SearchCount(branch.ID, dateFrom, dateTo);
                    var pageCount = totalCount == 0 ? 0 : await _unitOfWork.BranchScheduleRepository.SearchPageCount(branch.ID, entity.row, dateFrom, dateTo);

                    var newList = new List<branchScheduleVM>();
                    foreach (var sched in schedules)
                    {
                        var verifiedCount = await _unitOfWork.BranchScheduleMemberRepository.CountVerified(sched.ID);
                        var branchSched = _mapper.Map<branchScheduleVM>(sched);
                        branchSched.AvailableSlot = branchSched.MaxPersonCount - verifiedCount;
                        newList.Add(branchSched);
                    }

                    var result = new paginationVM
                    {
                        List = newList,
                        PageCount = pageCount,
                        TotalCount = totalCount
                    };

                    response.Data = result;
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("{0} has been success!.", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }
                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_GET_SCHEDULE, user.ID);


                return response;
            }
        }

        public async Task<responseVM> ValidateLoadSchedule(requestVM payload, responseVM response)
        {
            var entity = new getBranchScheduleVM();
            try
            {
                entity = JsonConvert.DeserializeObject<getBranchScheduleVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            var branch = await _unitOfWork.BranchRepository.GetByGuid(entity.branchCode);
            if (branch == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Invalid branch!", response.Audience);
            }
            else if (entity.From == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("From date is required", response.Audience);
            }
            else if (entity.To == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("To date is required", response.Audience);
            }

            return response;
        }

    }
}
