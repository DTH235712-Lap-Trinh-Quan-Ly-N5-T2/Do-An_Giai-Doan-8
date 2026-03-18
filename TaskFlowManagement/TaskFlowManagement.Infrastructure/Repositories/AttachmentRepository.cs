using Microsoft.EntityFrameworkCore;
using TaskFlowManagement.Core.Entities;
using TaskFlowManagement.Core.Interfaces;
using TaskFlowManagement.Infrastructure.Data;

namespace TaskFlowManagement.Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public AttachmentRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Attachment?> GetByIdAsync(int id)
        {
             using var ctx = await _contextFactory.CreateDbContextAsync();
             return await ctx.Attachments
                 .Include(a => a.UploadedBy)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Attachment>> GetAllAsync()
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            return await ctx.Attachments
                .Include(a => a.UploadedBy)
                .AsNoTracking()
                .OrderBy(a => a.UploadedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Attachment entity)
        {
             using var ctx = await _contextFactory.CreateDbContextAsync();
             await ctx.Attachments.AddAsync(entity);
             await ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attachment entity)
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            ctx.Attachments.Update(entity);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var ctx = await _contextFactory.CreateDbContextAsync();
            var attachment = await ctx.Attachments.FindAsync(id);
            if (attachment != null)
            {
                ctx.Attachments.Remove(attachment);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<List<Attachment>> GetByTaskIdAsync(int taskId)
        {
             using var ctx = await _contextFactory.CreateDbContextAsync();
             return await ctx.Attachments
                 .Include(a => a.UploadedBy)
                 .Where(a => a.TaskItemId == taskId)
                 .AsNoTracking()
                 .OrderByDescending(a => a.UploadedAt)
                 .ToListAsync();
        }
    }
}
