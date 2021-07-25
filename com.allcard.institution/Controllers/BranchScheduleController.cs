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
    public class BranchScheduleController : BaseController
    {
        private readonly IBranchScheduleService _branchScheduleService;

        public BranchScheduleController(IBranchScheduleService branchScheduleService)
        {
            _branchScheduleService = branchScheduleService;
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> Create([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.GenerateSchedule(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("GetAvailable")]
        public async Task<IActionResult> GetAvailable([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.GetAvailableSchedule(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("RemainingCount")]
        public async Task<IActionResult> GetRemaining([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.GetScheduleAvailableCount(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }


        [HttpPost("Cancel")]
        public async Task<IActionResult> CancelSched([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.CancelSchedule(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("MySchedule")]
        public async Task<IActionResult> GetSchedule([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.GetAllSchedule(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("board")]
        public async Task<IActionResult> GetBoard([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.GetBoard(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateSched([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.UpdateSchedule(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Finish")]
        public async Task<IActionResult> FinishSched([FromBody]requestVM request)
        {
            var result = await _branchScheduleService.FinishSchedule(request, true, GetUserCode);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
    }
}
