using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace com.allcard.institution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController :BaseController
    {
        [HttpGet("logo")]
        public async Task<IActionResult> Get()
        {
            var file = Directory.GetCurrentDirectory() + "\\Resources\\Image\\logo.png";
            var image = System.IO.File.OpenRead(file);
            return File(image, "image/jpeg");
        }
    }
}
