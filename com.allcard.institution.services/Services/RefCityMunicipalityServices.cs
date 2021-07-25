using AutoMapper;
using com.allcard.common;
using com.allcard.institution.common;
using com.allcard.institution.models;
using com.allcard.institution.repository;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution
{
    public interface IRefCityMunicipalityServices
    {
        Task<responseVM> AutoComplete(requestVM payload);
        Task<responseVM> GetAllowedCityMun(requestVM payload);
    }
    public class RefCityMunicipalityServices : IRefCityMunicipalityServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _audience;
        private string _subject;
        private readonly QueConfiguration _appSettings;

        public RefCityMunicipalityServices(IUnitOfWork unitOfWork, IMapper mapper, IOptions<QueConfiguration> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _audience = "City Mun";
            _subject = "City Mun";
            _appSettings = appSettings.Value;
        }

        public async Task<responseVM> AutoComplete(requestVM payload)
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
                    await ValidateAutoComplet(payload, response);
                    if (response.ResultCode != Constants.RESULT_CODE_SUCCESS)
                        return response;


                    var entity = JsonConvert.DeserializeObject<getCitMunicipality>(payload.Data.ToString());
                    var result = await _unitOfWork.RefCityMunicipalityRepository.Search(entity.Value.ToUpper());

                    var list = new List<refCityMunicipalityVM>();
                    foreach (var city in result)
                    {
                        var prov = await _unitOfWork.RefProvinceRepository.GetByProvinceCode(city.ProvinceCode);
                        var c = new refCityMunicipalityVM()
                        {
                            Description = city.Description,
                            CityMunicipalityCode = city.CityMunicipalityCode,
                            ProvinceCode = city.ProvinceCode,
                             ProvinceDescription = prov.PSGCProvinceDescription
                        };
                        list.Add(c);
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

        private async Task<responseVM> ValidateAutoComplet(requestVM payload, responseVM response)
        {
            
            try
            {
                var entity = JsonConvert.DeserializeObject<getCitMunicipality>(payload.Data.ToString());
            }
            catch (Exception)
            {
                response.ResultCode = Constants.RESULT_CODE_SERVER_ERROR;
                response.ResultMessage = string.Format("{0} invalid data object!.", response.Audience);
            }

            return response;
        }


        public async Task<responseVM> GetAllowedCityMun(requestVM payload)
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

                    var regionConfig = _appSettings.AllowRegions.Split(",").ToList();

                    var provincesConfig = await _unitOfWork.RefProvinceRepository.GetByRegion(regionConfig);
                    var provinces = new List<string>();
                    foreach (var prov in provincesConfig)
                    {
                        provinces.Add(prov.PSGCProvinceCode);
                    }
                    var result = await _unitOfWork.RefCityMunicipalityRepository.GetByProvinces(provinces);

                    var list = new List<refCityMunicipalityVM>();
                    foreach (var city in result)
                    {
                        var prov = await _unitOfWork.RefProvinceRepository.GetByProvinceCode(city.ProvinceCode);
                        var c = new refCityMunicipalityVM()
                        {
                            Description = city.Description,
                            CityMunicipalityCode = city.CityMunicipalityCode,
                            ProvinceCode = city.ProvinceCode,
                            ProvinceDescription = prov.PSGCProvinceDescription
                        };
                        list.Add(c);
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

    }
}
