using AutoMapper;
using com.allcard.common;
using com.allcard.institution.common;
using com.allcard.institution.models;
using com.allcard.institution.repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.allcard.institution.services
{
    public interface IMerchantService : IBaseService
    {
        Task<responseVM> GetAll(requestVM payload);
    }
    public class MerchantService : BaseService, IMerchantService
    {
        public MerchantService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _audience = "Merchant";
            _subject = "Merchant Service";
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


                    var entity = JsonConvert.DeserializeObject<Merchant>(payload.Data.ToString());
                    await _unitOfWork.MerchantRepository.AddAsyn(entity);
                    if (IsSave)
                        await _unitOfWork.Save();
                    response.Data = _mapper.Map<merchantVM>(entity);
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
                var entity = JsonConvert.DeserializeObject<Merchant>(payload.Data.ToString());
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


                    var entity = JsonConvert.DeserializeObject<Merchant>(payload.Data.ToString());
                    await _unitOfWork.MerchantRepository.UpdateAsyn(entity, entity.ID);
                    if (IsSave)
                        await _unitOfWork.Save();

                    response.Data = _mapper.Map<merchantVM>(entity);
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
            var entity = new Merchant();

            try
            {
                entity = JsonConvert.DeserializeObject<Merchant>(payload.Data.ToString());
            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            if (response.ResultCode == Constants.RESULT_CODE_SUCCESS)
            {
                var entityExist = await _unitOfWork.MerchantRepository.GetAsync(entity.ID);
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
                    var entity = JsonConvert.DeserializeObject<merchantGetVM>(payload.Data.ToString());

                    IList<Merchant> data = new List<Merchant>();
                    if (entity.MerchantID != 0)
                    {
                        data.Add(await _unitOfWork.MerchantRepository.GetAsync(entity.MerchantID));
                    }
                    else if (entity.GroupID != 0)
                    {
                        data = await _unitOfWork.MerchantRepository.GetByGroup(entity.GroupID);
                    }
                    else if (entity.InstitutionID != null)
                    {
                        data = await _unitOfWork.MerchantRepository.GetByInstitutionID(entity.InstitutionID);
                    }


                    response.Data = _mapper.Map<IList<merchantVM>>(data);

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

                    var data = await _unitOfWork.MerchantRepository.GetAllAsyn();
                    response.Data = _mapper.Map<IList<merchantVM>>(data);

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
