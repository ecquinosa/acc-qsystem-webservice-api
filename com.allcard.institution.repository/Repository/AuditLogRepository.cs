using com.allcard.institution.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IAuditLogRepository : IBaseRepository<AuditLog>
    {
        Task<bool> Log(string message, string module, int userID);
    }
    public class AuditLogRepository : BaseRepository<AuditLog> , IAuditLogRepository
    {
        public AuditLogRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<bool> Log(string message, string module, int userID)
        {
            var log = new AuditLog();
            log.Module = module;
            log.Description = message;
            log.CreatedBy = userID;
            log.UpdatedBy = userID;
            await _context.AuditLog.AddAsync(log);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
