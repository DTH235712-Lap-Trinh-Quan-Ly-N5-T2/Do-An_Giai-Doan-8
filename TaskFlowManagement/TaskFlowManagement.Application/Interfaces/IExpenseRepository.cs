using TaskFlowManagement.Core.Entities;

namespace TaskFlowManagement.Core.Interfaces
{
    /// <summary>
    /// Repository cho Expense – CRUD chi phí dự án (GĐ8: UC-23).
    /// Kế thừa IRepository&lt;Expense&gt; → có sẵn GetByIdAsync, GetAllAsync, AddAsync, UpdateAsync, DeleteAsync.
    /// </summary>
    public interface IExpenseRepository : IRepository<Expense>
    {
        /// <summary>
        /// Lấy danh sách chi phí của một dự án, kèm thông tin người tạo (CreatedBy).
        /// Sắp xếp theo ExpenseDate giảm dần (mới nhất lên đầu).
        /// </summary>
        Task<List<Expense>> GetByProjectAsync(int projectId);

        /// <summary>
        /// Tính tổng chi phí thực tế của một dự án trực tiếp trên DB (SUM aggregation).
        /// Trả về 0 nếu chưa có chi phí nào.
        /// Kết quả là decimal – KHÔNG dùng double/float.
        /// </summary>
        Task<decimal> GetTotalByProjectAsync(int projectId);
    }
}
