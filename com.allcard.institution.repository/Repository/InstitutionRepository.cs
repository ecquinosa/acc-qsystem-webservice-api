using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IInstitutionRepository : IBaseRepository<Institution>
    {
        Task<Institution> Get(Guid guid);
    }
    public class InstitutionRepository : BaseRepository<Institution>, IInstitutionRepository
    {
        public InstitutionRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<Institution> Get(Guid guid)
        {
            return await _context.Institution.Where(a => a.GUID == guid).FirstOrDefaultAsync();
        }
    }
}
