using com.allcard.common;
using com.allcard.institution.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.allcard.institution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchScheduleMemberController : BaseController
    {
        private readonly IBranchScheduleMemberService _branchScheduleMemberService;

        public BranchScheduleMemberController(IBranchScheduleMemberService branchScheduleMemberService) 
        {
            _branchScheduleMemberService = branchScheduleMemberService;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> Create([FromBody]requestVM request)
        {
            var result = await _branchScheduleMemberService.ReserveSlot(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("otpconfirm")]
        public async Task<IActionResult> ConfirmOTP([FromBody]requestVM request)
        {
            var result = await _branchScheduleMemberService.ConfirmOTP(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("scan")]
        public async Task<IActionResult> Scan([FromBody]requestVM request)
        {
            var result = await _branchScheduleMemberService.ScanCode(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("in")]
        public async Task<IActionResult> ScanIn([FromBody]requestVM request)
        {
            var result = await _branchScheduleMemberService.ScanIn(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("out")]
        public async Task<IActionResult> ScanOut([FromBody]requestVM request)
        {
            var result = await _branchScheduleMemberService.ScanOut(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
    }
}
