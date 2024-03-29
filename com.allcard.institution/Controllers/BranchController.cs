﻿using com.allcard.common;
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
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllData([FromBody]requestVM request)
        {
            var result = await _branchService.GetAll(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Get")]
        public async Task<IActionResult> GetData([FromBody]requestVM request)
        {
            var result = await _branchService.Get(request);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]requestVM request)
        {
            var result = await _branchService.Create(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody]requestVM request)
        {
            var result = await _branchService.Update(request, true);
            return StatusCode(int.Parse(result.ResultCode), result);
        }
    }
}
