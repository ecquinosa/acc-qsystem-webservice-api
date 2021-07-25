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
    public class ChainController  :BaseController
    {
        private readonly IChainService _chainService;

        public ChainController(IChainService chainService)
        {
            _chainService = chainService;
        }
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllData([FromBody]requestVM request)
        {
            var result = await _chainService.GetAll(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Get")]
        public async Task<IActionResult> GetData([FromBody]requestVM request)
        {
            var result = await _chainService.Get(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]requestVM request)
        {
            var result = await _chainService.Create(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody]requestVM request)
        {
            var result = await _chainService.Update(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
    }


}
