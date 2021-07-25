using AutoMapper;
using com.allcard.common;
using com.allcard.institution.common;
using com.allcard.institution.common.ViewModel;
using com.allcard.institution.repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.services
{
    public interface IAuthenticateService
    {
        Task<responseVM> Authenticate(requestVM payload);
        Task<userProfileVM> GetUserProfile(Guid guid);
    }
    public class AuthenticateService : IAuthenticateService
    {

        public AuthenticateService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<QueConfiguration> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        protected string _audience;
        protected string _subject;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly QueConfiguration _appSettings;

        public async Task<responseVM> Authenticate(requestVM payload)
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
                    await ValidateAuthenticate(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;

                    var entity = JsonConvert.DeserializeObject<authenticateVM>(payload.Data.ToString());
                    var userDetails = await _unitOfWork.UserProfileRepository.GetAsync(entity.Username, entity.Password);
                    var userRole = await _unitOfWork.UserRoleRepository.GetByUser(userDetails.ID);
                    var role = await _unitOfWork.RoleRepository.GetAsync(userRole.RoleID);
                    var branch = await _unitOfWork.BranchRepository.GetAsync(userDetails.BranchID);
                    var store = await _unitOfWork.InstitutionRepository.Get(branch.InstitutionID);


                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                                new Claim("ucode", userDetails.GUID.ToString()),
                                new Claim("r", role.ID.ToString()),
                                 new Claim("role", role.Name),
                                new Claim("bcode", branch.GUID.ToString()),
                                new Claim("bc", branch.Name),
                                new Claim("s", store.Name),
                                new Claim("name", userDetails.DisplayName)
                        }),
                        Expires = DateTime.UtcNow.AddHours(int.Parse(_appSettings.TokenExpirationHours)),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);


                    response.Data = tokenHandler.WriteToken(token);
                    response.ResultCode = Constants.RESULT_CODE_SUCCESS;
                    response.ResultMessage = string.Format("{0} has been success!.", response.Audience);
                }
                catch (Exception ex)
                {

                    response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                    response.ResultMessage = string.Format("Error:{0} Inner:{1}", ex.Message, ex.StackTrace);
                }

                string message = string.Format("Response:{0} Message:{1}", response.ResultCode, response.ResultMessage);
                await _unitOfWork.AuditLogRepository.Log(message, Constants.MODULE_AUTHENTICATE, 1);

                return response;
            }
        }

        public async Task<responseVM> ValidateAuthenticate(requestVM payload, responseVM response)
        {
            var entity = new authenticateVM();
            try
            {
                entity = JsonConvert.DeserializeObject<authenticateVM>(payload.Data.ToString());

            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }
            var userDetails = await _unitOfWork.UserProfileRepository.GetAsync(entity.Username, entity.Password);

            if (userDetails == null)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("Login failed, Invalid username or password.", response.Audience);
            }
            else if (userDetails.Status == Constants.USER_STATUS_BLOCKED)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("User is blocked.", response.Audience);
            }


            return response;
        }

        public async Task<userProfileVM> GetUserProfile(Guid guid)
        {
            var user = await _unitOfWork.UserProfileRepository.GetByGuid(guid);
            return _mapper.Map<userProfileVM>(user);
        }
    }
}
