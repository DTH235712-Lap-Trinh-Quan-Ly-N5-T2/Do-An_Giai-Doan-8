using Microsoft.EntityFrameworkCore;
using TaskFlowManagement.Core.Entities;
using TaskFlowManagement.Core.Interfaces;
using TaskFlowManagement.Infrastructure.Data;

namespace TaskFlowManagement.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public CommentRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            return await ctx.Comments
                .Include(c => c.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            return await ctx.Comments
                .Include(c => c.User)
                .AsNoTracking()
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Comment entity)
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            await ctx.Comments.AddAsync(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment entity)
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            ctx.Comments.Update(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            var comment = await ctx.Comments.FindAsync(id);
            if (comment != null)
            {
                ctx.Comments.Remove(comment);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<List<Comment>> GetByTaskIdAsync(int taskId)
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            return await ctx.Comments
                .Include(c => c.User)
                .Where(c => c.TaskItemId == taskId)
                .AsNoTracking()
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
