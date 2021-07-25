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
    public interface IChainService : IBaseService
    {
        Task<responseVM> GetAll(requestVM payload);
    }
    public class ChainService : BaseService, IChainService
    {
        public ChainService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _audience = "Chain";
            _subject = "Chain Service";
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


                    var entity = JsonConvert.DeserializeObject<Chain>(payload.Data.ToString());
                    await _unitOfWork.ChainRepository.AddAsyn(entity);
                    if (IsSave)
                        await _unitOfWork.Save();
                    response.Data = _mapper.Map<chainVM>(entity);
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
                var entity = JsonConvert.DeserializeObject<Chain>(payload.Data.ToString());
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


                    var entity = JsonConvert.DeserializeObject<Chain>(payload.Data.ToString());
                    await _unitOfWork.ChainRepository.UpdateAsyn(entity, entity.ID);
                    if (IsSave)
                        await _unitOfWork.Save();

                    response.Data = _mapper.Map<chainVM>(entity);
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
            var entity = new Chain();

            try
            {
                entity = JsonConvert.DeserializeObject<Chain>(payload.Data.ToString());
            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            if (response.ResultCode == Constants.RESULT_CODE_SUCCESS)
            {
                var entityExist = await _unitOfWork.ChainRepository.GetAsync(entity.ID);
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
                    var entity = JsonConvert.DeserializeObject<chainGetVM>(payload.Data.ToString());

                    IList<Chain> data = new List<Chain>();
                    if (entity.ChainID != 0)
                    {
                        data.Add(await _unitOfWork.ChainRepository.GetAsync(entity.ChainID));
                    }
                    else if (entity.InstitutionID != null)
                    {
                        data = await _unitOfWork.ChainRepository.GetByInstitution(entity.InstitutionID);
                    }
                
                   
                    response.Data = _mapper.Map< IList<chainVM>>(data);

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

                    var data = await _unitOfWork.ChainRepository.GetAllAsyn();
                    response.Data = _mapper.Map<IList<chainVM>>(data);

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
