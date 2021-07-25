using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IBranchScheduleMemberRepository : IBaseRepository<BranchScheduleMember>
    {
        Task<bool> IsRefNumberExist(string refNum);
        Task<int> CountVerified(int scheduleID);

        Task<BranchScheduleMember> GetByGuid(Guid guid);

        Task<int> CountEmailSchedule(string email, DateTime date);
        Task<BranchScheduleMember> GetEmailSchedule(string email, DateTime date);

        Task<List<BranchScheduleMember>> GetByScheduleID(int scheduleID);

        Task<BranchScheduleMember> GetByRefNum(string refNum);

    }
    public class BranchScheduleMemberRepository : BaseRepository<BranchScheduleMember>, IBranchScheduleMemberRepository
    {
        public BranchScheduleMemberRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<int> CountVerified(int scheduleID)
        {
            return await _context.BranchScheduleMember.Where(a => a.BranchScheduleID == scheduleID && a.IsVerified).CountAsync();
        }

        public async Task<BranchScheduleMember> GetByGuid(Guid guid)
        {
            return await _context.BranchScheduleMember.Where(a => a.GUID == guid).FirstOrDefaultAsync();
        }

        public async Task<int> CountEmailSchedule(string email, DateTime date)
        {
            return await _context.BranchScheduleMember
                .Where(a => a.Member.Email == email &&
                a.BranchSchedule.Date.Date == date.Date &&
                a.IsVerified)
                .CountAsync();
        }

        public async Task<bool> IsRefNumberExist(string refNum)
        {
            return await _context.BranchScheduleMember.Where(a => a.RefNumber == refNum).AnyAsync();
        }

        public async Task<List<BranchScheduleMember>> GetByScheduleID(int scheduleID)
        {
            return await _context.BranchScheduleMember
                .Where(a => a.BranchScheduleID == scheduleID)
                .Include(a => a.Member)
                .ToListAsync();
        }

        public async Task<BranchScheduleMember> GetByRefNum(string refNum)
        {
            return await _context.BranchScheduleMember.Where(a => a.RefNumber == refNum).FirstOrDefaultAsync();
        }

        public async Task<BranchScheduleMember> GetEmailSchedule(string email, DateTime date)
        {
            return await _context.BranchScheduleMember
                .Where(a => a.Member.Email == email &&
                a.BranchSchedule.Date.Date == date.Date &&
                a.IsVerified)
                .FirstOrDefaultAsync();
        }
    }
}
