using com.allcard.common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.allcard.institution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : BaseController
    {
        private readonly IRefCityMunicipalityServices _refCityMunicipalityServices;

        public CityController(IRefCityMunicipalityServices refCityMunicipalityServices)
        {
            _refCityMunicipalityServices = refCityMunicipalityServices;
        }


        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]requestVM request)
        {
            var result = await _refCityMunicipalityServices.AutoComplete(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody]requestVM request)
        {
            var result = await _refCityMunicipalityServices.GetAllowedCityMun(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
    }
}
