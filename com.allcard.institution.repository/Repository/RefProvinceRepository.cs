using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IRefProvinceRepository : IBaseRepository<RefProvince>
    {
        Task<RefProvince> GetByProvinceCode(string provinceCode);

        Task<IList<RefProvince>> GetByRegion(List<string> regionCode);
    }
    public class RefProvinceRepository : BaseRepository<RefProvince>, IRefProvinceRepository
    {
        public RefProvinceRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<RefProvince> GetByProvinceCode(string provinceCode)
        {
            return await _context.RefProvince.Where(a => a.PSGCProvinceCode == provinceCode)
                 .FirstOrDefaultAsync();
        }

        public async Task<IList<RefProvince>> GetByRegion(List<string> regionCode)
        {
             return await _context.RefProvince.Where(a => regionCode.Contains(a.PSGCRegionCode))
                 .ToListAsync();
        }
    }
}
