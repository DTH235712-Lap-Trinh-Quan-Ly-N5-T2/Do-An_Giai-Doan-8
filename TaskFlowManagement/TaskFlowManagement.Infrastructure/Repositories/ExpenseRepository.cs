using Microsoft.EntityFrameworkCore;
using TaskFlowManagement.Core.Entities;
using TaskFlowManagement.Core.Interfaces;
using TaskFlowManagement.Infrastructure.Data;

namespace TaskFlowManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repository triển khai IExpenseRepository phục vụ UC-23 (Giai đoạn 8).
    /// Sử dụng IDbContextFactory để đảm bảo thread-safety trong môi trường WinForms.
    /// </summary>
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public ExpenseRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            using var ctx = _contextFactory.CreateDbContext();
            return await ctx.Expenses
                .AsNoTracking()
                .Include(e => e.Project)
                .Include(e => e.CreatedBy)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Expense>> GetAllAsync()
        {
            using var ctx = _contextFactory.CreateDbContext();
            return await ctx.Expenses
                .AsNoTracking()
                .Include(e => e.Project)
                .Include(e => e.CreatedBy)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetByProjectAsync(int projectId)
        {
            using var ctx = _contextFactory.CreateDbContext();
            return await ctx.Expenses
                .AsNoTracking()
                .Include(e => e.Project)
                .Include(e => e.CreatedBy)
                .Where(e => e.ProjectId == projectId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalByProjectAsync(int projectId)
        {
            using var ctx = _contextFactory.CreateDbContext();
            // SUM aggregation trực tiếp trên Database
            return await ctx.Expenses
                .Where(e => e.ProjectId == projectId)
                .SumAsync(e => e.Amount);
        }

        public async Task AddAsync(Expense entity)
        {
            using var ctx = _contextFactory.CreateDbContext();
            entity.CreatedAt = DateTime.UtcNow;
            await ctx.Expenses.AddAsync(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense entity)
        {
            using var ctx = _contextFactory.CreateDbContext();
            ctx.Expenses.Update(entity);
            // Đảm bảo không ghi đè CreatedAt nếu không cần thiết 
            // (Thường entity được load trước khi update nên sẽ giữ được CreatedAt)
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var ctx = _contextFactory.CreateDbContext();
            await ctx.Expenses.Where(e => e.Id == id).ExecuteDeleteAsync();
        }
    }
}
