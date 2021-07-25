using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IMerchantRepository : IBaseRepository<Merchant>
    {
        Task<IList<Merchant>> GetByGroup(int groupID);
        Task<IList<Merchant>> GetByInstitutionID(Guid institutionID);
    }
    public class MerchantRepository : BaseRepository<Merchant>, IMerchantRepository
    {
        public MerchantRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<IList<Merchant>> GetByGroup(int groupID)
        {
            return await _context.Merchant.Where(a => a.GroupID == groupID).ToListAsync();
        }

        public async Task<IList<Merchant>> GetByInstitutionID(Guid institutionID)
        {
            return await _context.Merchant.Where(a => a.InstitutionID == institutionID).ToListAsync(); 
        }
    }
}
