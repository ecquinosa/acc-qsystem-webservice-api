using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface ILocationRepository : IBaseRepository<Location>
    {
        Task<IList<Location>> GetByBranch(int branchID);
        Task<IList<Location>> GetByInstitution(Guid institutionID);
        Task<Location> GetAsync(Guid locationID);

    }
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<Location> GetAsync(Guid locationID)
        {
            return await _context.Location.Where(a => a.GUID == locationID).FirstOrDefaultAsync();
        }

        public async  Task<IList<Location>> GetByBranch(int branchID)
        {
            return await _context.Location.Where(a => a.BranchID == branchID).ToListAsync();
        }

        public async Task<IList<Location>> GetByInstitution(Guid institutionID)
        {
            return await _context.Location.Where(a => a.InstitutionID == institutionID).ToListAsync();
        }
    }
}
