using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.allcard.common;
using com.allcard.institution.common;
using com.allcard.institution.models;
using com.allcard.institution.repository;
using Newtonsoft.Json;

namespace com.allcard.institution.services
{
    public interface IBranchService : IBaseService
    {
        Task<responseVM> GetAll(requestVM payload);
    }
    public class BranchService : BaseService, IBranchService
    {
        public BranchService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _audience = "Branch";
            _subject = "Branch Service";
        }
        public override async Task<responseVM> Create(requestVM payload, bool IsSave)
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
                    await ValidateCreate(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<Branch>(payload.Data.ToString());
                    await _unitOfWork.BranchRepository.AddAsyn(entity);
                    if (IsSave)
                        await _unitOfWork.Save();
                    response.Data = _mapper.Map<branchVM>(entity);
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
        public async Task<responseVM> ValidateCreate(requestVM payload, responseVM response)
        {
            try
            {
                var entity = JsonConvert.DeserializeObject<Branch>(payload.Data.ToString());
            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }



            return response;
        }
        public override async Task<responseVM> Update(requestVM payload, bool IsSave)
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
                    await ValidateUpdate(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<Branch>(payload.Data.ToString());
                    await _unitOfWork.BranchRepository.UpdateAsyn(entity, entity.ID);
                    if (IsSave)
                        await _unitOfWork.Save();

                    response.Data = _mapper.Map<branchVM>(entity);
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
        public async Task<responseVM> ValidateUpdate(requestVM payload, responseVM response)
        {
            var entity = new Branch();

            try
            {
                entity = JsonConvert.DeserializeObject<Branch>(payload.Data.ToString());
            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            if (response.ResultCode == Constants.RESULT_CODE_SUCCESS)
            {
                var entityExist = await _unitOfWork.BranchRepository.GetAsync(entity.ID);
                if (entityExist == null)
                {
                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("{0} update failed data is not exist!.", response.Audience);
                }
            }


            return response;
        }
        public override async Task<responseVM> Get(requestVM payload)
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
                    var entity = JsonConvert.DeserializeObject<branchGetVM>(payload.Data.ToString());

                    IList<Branch> data = new List<Branch>();
                    if (entity.BranchID != 0)
                    {
                        data.Add(await _unitOfWork.BranchRepository.GetAsync(entity.BranchID));
                    }
                    else if (entity.MerchantID != 0)
                    {
                        data = await _unitOfWork.BranchRepository.GetByMerchant(entity.MerchantID);
                    }
                    else if (entity.InstitutionID != null)
                    {
                        data = await _unitOfWork.BranchRepository.GetByInstitution(entity.InstitutionID);
                    }


                    response.Data = _mapper.Map<IList<branchVM>>(data);

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
        public async Task<responseVM> GetAll(requestVM payload)
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

                    var data = await _unitOfWork.BranchRepository.GetAllAsyn();
                    response.Data = _mapper.Map<IList<branchVM>>(data);
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
    }
}
