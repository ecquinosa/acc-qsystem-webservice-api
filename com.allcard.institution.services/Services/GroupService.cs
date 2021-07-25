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
    public interface IGroupService : IBaseService
    {
        Task<responseVM> GetAll(requestVM payload);
    }
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _audience = "Group";
            _subject = "Group Service";
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


                    var entity = JsonConvert.DeserializeObject<Group>(payload.Data.ToString());
                    await _unitOfWork.GroupRepository.AddAsyn(entity);
                    if (IsSave)
                        await _unitOfWork.Save();
                    response.Data = _mapper.Map<groupVM>(entity);
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
                var entity = JsonConvert.DeserializeObject<Group>(payload.Data.ToString());
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


                    var entity = JsonConvert.DeserializeObject<Group>(payload.Data.ToString());
                    await _unitOfWork.GroupRepository.UpdateAsyn(entity, entity.ID);
                    if (IsSave)
                        await _unitOfWork.Save();

                    response.Data = _mapper.Map<groupVM>(entity);
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
            var entity = new Group();

            try
            {
                entity = JsonConvert.DeserializeObject<Group>(payload.Data.ToString());
            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            if (response.ResultCode == Constants.RESULT_CODE_SUCCESS)
            {
                var entityExist = await _unitOfWork.GroupRepository.GetAsync(entity.ID);
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
                    var entity = JsonConvert.DeserializeObject<groupGetVM>(payload.Data.ToString());

                    IList<Group> data = new List<Group>();
                    if (entity.GroupID != 0)
                    {
                        data.Add(await _unitOfWork.GroupRepository.GetAsync(entity.GroupID));
                    }
                    else if (entity.ChainID != 0)
                    {
                        data = await _unitOfWork.GroupRepository.GetByChain(entity.ChainID);
                    }
                    else if (entity.InstitutionID != null)
                    {
                        data = await _unitOfWork.GroupRepository.GetByInstitution(entity.InstitutionID);
                    }


                    response.Data = _mapper.Map<IList<groupVM>>(data);
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

                    var data = await _unitOfWork.GroupRepository.GetAllAsyn();
                    response.Data = _mapper.Map<IList<groupVM>>(data);

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
