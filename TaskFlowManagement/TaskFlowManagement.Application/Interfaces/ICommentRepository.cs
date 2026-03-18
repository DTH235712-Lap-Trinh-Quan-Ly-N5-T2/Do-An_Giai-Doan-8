using TaskFlowManagement.Core.Entities;

namespace TaskFlowManagement.Core.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<List<Comment>> GetByTaskIdAsync(int taskId);
    }
}
