using com.allcard.institution.services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace com.allcard.institution.Controllers
{
    public class BaseController : ControllerBase
    {
        
        public Guid GetUserCode
        {
            get
            {
                var currentUser = HttpContext.User;
                IEnumerable<Claim> claims = currentUser.Claims;
                Guid value;
                if (Guid.TryParse(claims.Where(a => a.Type == "ucode")
                    .Select(a => a.Value)
                    .FirstOrDefault(), out value)) ;
                var result = value;
                return result;
            }
        }
        

        public int GetRole
        {
            get
            {
                var currentUser = HttpContext.User;
                IEnumerable<Claim> claims = currentUser.Claims;
                int value;
                if (int.TryParse(claims.Where(a => a.Type == "r")
                    .Select(a => a.Value)
                    .FirstOrDefault(), out value)) ;
                var result = value;

                return result;
            }
        }

        public Guid GetBranch
        {
            get
            {
                var currentUser = HttpContext.User;
                IEnumerable<Claim> claims = currentUser.Claims;
                Guid value;
                if (Guid.TryParse(claims.Where(a => a.Type == "bcode")
                    .Select(a => a.Value)
                    .FirstOrDefault(), out value)) ;
                var result = value;

                return result;
            }
        }

    }
}
