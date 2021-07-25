using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IChainRepository : IBaseRepository<Chain>
    {
        Task<IList<Chain>> GetByInstitution(Guid institution);
     }
    public class ChainRepository : BaseRepository<Chain>, IChainRepository
    {
        public ChainRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<IList<Chain>> GetByInstitution(Guid institution)
        {
            return await _context.Chain.Where(a => a.InstitutionID == institution).ToListAsync();
        }
    }
}
