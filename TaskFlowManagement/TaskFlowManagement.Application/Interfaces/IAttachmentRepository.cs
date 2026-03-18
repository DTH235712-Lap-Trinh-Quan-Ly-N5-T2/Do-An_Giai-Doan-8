using TaskFlowManagement.Core.Entities;

namespace TaskFlowManagement.Core.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        Task<List<Attachment>> GetByTaskIdAsync(int taskId);
    }
}
