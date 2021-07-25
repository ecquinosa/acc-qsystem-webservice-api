using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IUserProfileRepository : IBaseRepository<UsersProfile>
    {
        Task<UsersProfile> GetAsync(string username, string password);
        Task<UsersProfile> GetByGuid(Guid guid);
    }
    public class UserProfileRepository : BaseRepository<UsersProfile> ,IUserProfileRepository
    {
        public UserProfileRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<UsersProfile> GetAsync(string username, string password)
        {
            return await _context.UsersProfile.Where(a=>a.Username == username && a.Password == password).FirstOrDefaultAsync();
        }

        public async Task<UsersProfile> GetByGuid(Guid guid)
        {
            return await _context.UsersProfile.Where(a => a.GUID == guid).FirstOrDefaultAsync();
        }
    }
}
