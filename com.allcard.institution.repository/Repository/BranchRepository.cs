using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {
        Task<IList<Branch>> GetByMerchant(int merchantID);
        Task<IList<Branch>> GetByInstitution(Guid institution);

        Task<Branch> GetByGuid(Guid branchGuid);
    }
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        public BranchRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<Branch> GetByGuid(Guid branchGuid)
        {
            return await _context.Branch.Where(a => a.GUID == branchGuid).FirstOrDefaultAsync();
        }

        public async Task<IList<Branch>> GetByInstitution(Guid institution)
        {
            return await _context.Branch
                .Where(a => a.InstitutionID == institution)
                .Include(a=>a.Merchant)
                .ThenInclude(a=>a.Group)
                .ThenInclude(a=>a.Chain)
                .ToListAsync();
        }

        public async Task<IList<Branch>> GetByMerchant(int merchantID)
        {
            return await _context.Branch.Where(a => a.MerchantID == merchantID).ToListAsync() ;
        }
    }
}
