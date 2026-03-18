using TaskFlowManagement.Core.DTOs;
using TaskFlowManagement.Core.Entities;

namespace TaskFlowManagement.Core.Interfaces.Services
{
    /// <summary>
    /// Service nghiệp vụ quản lý Chi phí dự án (GĐ8: UC-23).
    /// Tất cả phương thức đều là async để không chặn UI thread.
    /// </summary>
    public interface IExpenseService
    {
        // ── Danh sách ─────────────────────────────────────────────────────

        /// <summary>
        /// Lấy danh sách chi phí của một dự án (kèm CreatedBy).
        /// Sắp xếp theo ExpenseDate giảm dần.
        /// </summary>
        Task<List<Expense>> GetByProjectAsync(int projectId);

        // ── CRUD ──────────────────────────────────────────────────────────

        /// <summary>
        /// Thêm khoản chi phí mới.
        /// - Validation: Amount phải > 0; ExpenseDate không được lớn hơn ngày hiện tại.
        /// - Audit: CreatedById được gán tự động từ AppSession.UserId.
        /// - CreatedAt được gán tự động DateTime.UtcNow.
        /// </summary>
        Task<(bool Success, string Message)> AddExpenseAsync(Expense expense);

        /// <summary>
        /// Cập nhật thông tin khoản chi phí đã có.
        /// Validation tương tự AddExpenseAsync (Amount > 0, ExpenseDate hợp lệ).
        /// </summary>
        Task<(bool Success, string Message)> UpdateExpenseAsync(Expense expense);

        /// <summary>
        /// Xóa khoản chi phí theo ID (hard delete).
        /// Trả về lỗi nếu không tìm thấy bản ghi.
        /// </summary>
        Task<(bool Success, string Message)> DeleteExpenseAsync(int expenseId);

        // ── Budget Summary ─────────────────────────────────────────────────

        /// <summary>
        /// Trả về tóm tắt ngân sách của một dự án:
        /// Budget (từ Project), TotalExpense (SUM từ Expense), Remaining, UsagePercent.
        /// Tất cả tính toán tiền tệ dùng decimal – KHÔNG dùng double/float.
        /// Trả về null nếu dự án không tồn tại.
        /// </summary>
        Task<ProjectBudgetSummaryDto?> GetProjectBudgetSummaryAsync(int projectId);
    }
}
