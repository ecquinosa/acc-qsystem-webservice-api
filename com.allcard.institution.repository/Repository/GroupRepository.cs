using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<IList<Group>> GetByChain(int chainID);
        Task<IList<Group>> GetByInstitution(Guid institutionID);
    }
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<IList<Group>> GetByChain(int chainID)
        {
            return await _context.Group.Where(a => a.ChainID == chainID).ToListAsync();
        }

        public async Task<IList<Group>> GetByInstitution(Guid institutionID)
        {
            return await _context.Group.Where(a => a.InstitutionID == institutionID).ToListAsync();
        }
    }
}
