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
    public class MerchantController : BaseController
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService  merchantService)
        {
            _merchantService = merchantService;
        }
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllData([FromBody]requestVM request)
        {
            var result = await _merchantService.GetAll(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Get")]
        public async Task<IActionResult> GetData([FromBody]requestVM request)
        {
            var result = await _merchantService.Get(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]requestVM request)
        {
            var result = await _merchantService.Create(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody]requestVM request)
        {
            var result = await _merchantService.Update(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
    }
}
