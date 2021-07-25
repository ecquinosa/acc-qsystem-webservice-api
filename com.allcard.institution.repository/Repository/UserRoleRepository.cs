using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<UserRole> GetByUser(int userID);
    }
    public class UserRoleRepository : BaseRepository<UserRole> , IUserRoleRepository
    {
        public UserRoleRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<UserRole> GetByUser(int userID)
        {
            return await _context.UserRole.Where(a => a.UserProfileID == userID).OrderByDescending(a=>a.ID).FirstOrDefaultAsync();
        }
    }
}
