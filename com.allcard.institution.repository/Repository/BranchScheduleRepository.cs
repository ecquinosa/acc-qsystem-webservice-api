using com.allcard.common;
using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{
    public interface IBranchScheduleRepository : IBaseRepository<BranchSchedule>
    {
        Task<bool> IsConflictSchedule(BranchSchedule entity);

        Task<IList<BranchSchedule>> GetAvailableSchedule(int branchID, DateTime? date);


        Task<IList<BranchSchedule>> GetAllByDate(DateTime date);

        Task<IList<BranchSchedule>> GetAllByDate(int branchID, DateTime date, string status);

        Task<int> GetCountByStatus(DateTime date, string status);

        Task<IList<BranchSchedule>> GetAllSchedule(int branchID);

        Task<BranchSchedule> GetByGuid(Guid guid);

        Task<IList<BranchSchedule>> Search(int skip, int row, int branchID, DateTime from, DateTime to);
        Task<int> SearchCount(int branchID, DateTime from, DateTime to);
        Task<int> SearchPageCount(int branchID, int row, DateTime from, DateTime to);


    }
    public class BranchScheduleRepository : BaseRepository<BranchSchedule>, IBranchScheduleRepository
    {
        public BranchScheduleRepository(InstitutionContext context) : base(context)
        {
        }

        public async Task<IList<BranchSchedule>> GetAllByDate(DateTime date)
        {
            return await _context.BranchSchedule.Where(a => a.Date == date).ToListAsync();
        }

        public async Task<IList<BranchSchedule>> GetAllByDate(int branchID, DateTime date, string status)
        {
            return await _context.BranchSchedule
                .Where(a => a.BranchID == branchID &&
                a.Status == status &&
                a.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IList<BranchSchedule>> GetAllSchedule(int branchID)
        {
            return await _context.BranchSchedule.Where(a => a.BranchID == branchID).ToListAsync();
        }

        public async Task<IList<BranchSchedule>> GetAvailableSchedule(int branchID, DateTime? date)
        {
            return await _context.BranchSchedule
                .Where(a => a.Status == Constants.STATUS_OPEN &&
                //(a.Date == date || date == null) &&
                a.Date == date  &&
                a.BranchID == branchID)
                .OrderBy(a => a.Date)
                .ToListAsync();

            //return await _context.BranchSchedule
            //    .Where(a => a.Status == Constants.STATUS_OPEN &&
            //    (a.Date == date || date == null) &&
            //    a.Date.Date >= DateTime.Now.Date &&
            //    a.BranchID == branchID)
            //    .OrderBy(a => a.Date)
            //    .ToListAsync();
        }

        public async Task<BranchSchedule> GetByGuid(Guid guid)
        {
            return await _context.BranchSchedule.Where(a => a.GUID == guid).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountByStatus(DateTime date, string status)
        {
            return await _context.BranchSchedule.Where(a => a.Date == date && a.Status == status).CountAsync();
        }

        public async Task<bool> IsConflictSchedule(BranchSchedule entity)
        {
            //(StartDate1 <= EndDate2) and (EndDate1 >= StartDate2)
            return await _context.BranchSchedule
                .Where(x => x.StartTime <= entity.EndTime &&
                x.EndTime >= entity.StartTime && 
                x.Status == Constants.STATUS_OPEN &&
                x.ID != entity.ID) // only added
                .AnyAsync();
        }

        #region Pagination
        public async Task<IList<BranchSchedule>> Search(int page, int row, int branchID, DateTime from, DateTime to)
        {
            return await _context.BranchSchedule
                .Where(
                a => a.BranchID == branchID &&
                a.Date >= from && a.Date <= to
                )
                //.OrderByDescending(a => a.Date
                .OrderByDescending(a => a.Date)
                .Skip(page * row)
                .Take(row)
                .ToListAsync();
        }

        public async Task<int> SearchCount(int branchID, DateTime from, DateTime to)
        {
            return await _context.BranchSchedule
                 .Where(
                 a => a.BranchID == branchID &&
                   a.Date >= from && a.Date <= to
                 )
                 .CountAsync();
        }
        public async Task<int> SearchPageCount(int branchID, int row, DateTime from, DateTime to)
        {
            var total = await _context.BranchSchedule
                 .Where(
                 a => a.BranchID == branchID &&
                  a.Date >= from && a.Date <= to
                 )
                 .CountAsync();

            double result = (double)total / row;
            return (int)Math.Ceiling(result);
        }

        #endregion

    }
}
