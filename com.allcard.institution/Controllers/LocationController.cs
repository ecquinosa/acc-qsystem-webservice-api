using com.allcard.common;
using com.allcard.institution.services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.allcard.institution.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllData([FromBody]requestVM request)
        {
            var result = await _locationService.GetAll(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("getlist")]
        public async Task<IActionResult> Getlist([FromBody]requestVM request)
        {
            var result = await _locationService.GetList(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Get")]
        public async Task<IActionResult> GetData([FromBody]requestVM request)
        {
            var result = await _locationService.Get(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("getdetails")]
        public async Task<IActionResult> GetDetails([FromBody]requestVM request)
        {
            var result = await _locationService.GetDetails(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]requestVM request)
        {
            var result = await _locationService.Create(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody]requestVM request)
        {
            var result = await _locationService.Update(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
    }
}
