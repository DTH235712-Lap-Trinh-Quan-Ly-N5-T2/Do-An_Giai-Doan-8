using System.Collections.Generic;

namespace TaskFlowManagement.Core.DTOs
{
    // =====================================================
    // DTO cho GĐ6: Dashboard & Thống kê
    // Cung cấp số liệu tổng quan cho màn hình Admin/Manager
    // =====================================================

    /// <summary>
    /// Thống kê tổng quan cho Dashboard. 
    /// Bao gồm các con số tóm tắt và danh sách chi tiết (Biểu đồ).
    /// </summary>
    public class DashboardStatsDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        
        /// <summary>Task đã quá hạn (DueDate < Now & !IsCompleted)</summary>
        public int OverdueTasks { get; set; }
        
        /// <summary>Task sắp đến hạn (trong vòng 7 ngày tới & !IsCompleted)</summary>
        public int DueSoonTasks { get; set; }

        /// <summary>Thống kê số lượng task theo từng nhóm trạng thái để vẽ PieChart</summary>
        public List<StatusSummaryDto> StatusSummaries { get; set; } = new List<StatusSummaryDto>();

        /// <summary>Thống kê tiến độ các dự án để vẽ BarChart</summary>
        public List<ProjectProgressDto> ProjectProgresses { get; set; } = new List<ProjectProgressDto>();
    }

    /// <summary>
    /// Số liệu đếm task theo trạng thái. Dùng để vẽ Pie Chart.
    /// </summary>
    public class StatusSummaryDto
    {
        public string StatusName { get; set; } = string.Empty;
        public int Count { get; set; }
        public string ColorHex { get; set; } = string.Empty;
    }

    /// <summary>
    /// Đo lường tiến độ tổng thể của Project. Dùng để vẽ Bar Chart.
    /// </summary>
    public class ProjectProgressDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public double ProgressPercentage { get; set; }
    }

    /// <summary>
    /// Báo cáo ngân sách dự án (GĐ6)
    /// </summary>
    public class BudgetReportDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Remaining => Budget - TotalExpense;
        public double UsagePercentage => Budget > 0 ? (double)Math.Round((TotalExpense / Budget) * 100, 1) : (TotalExpense > 0 ? 100 : 0);
    }

    /// <summary>
    /// Báo cáo chi tiết tiến độ dự án (GĐ6)
    /// </summary>
    public class ProgressReportDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double AvgProgress { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // =====================================================
    // DTO cho GĐ8: Quản lý Chi phí – UC-23
    // Cung cấp tóm tắt ngân sách cho từng dự án cụ thể
    // =====================================================

    /// <summary>
    /// Tóm tắt ngân sách + chi phí thực tế của một dự án (GĐ8).
    /// Dùng decimal cho MỌI trường tiền tệ – tuyệt đối KHÔNG dùng double/float.
    /// </summary>
    public class ProjectBudgetSummaryDto
    {
        public int     ProjectId    { get; set; }
        public string  ProjectName  { get; set; } = string.Empty;

        /// <summary>Ngân sách định mức của dự án (từ Project.Budget).</summary>
        public decimal Budget       { get; set; }

        /// <summary>Tổng chi phí thực tế (SUM(Expense.Amount) từ DB).</summary>
        public decimal TotalExpense { get; set; }

        /// <summary>Số tiền còn lại = Budget - TotalExpense (có thể âm nếu vượt quỹ).</summary>
        public decimal Remaining    => Budget - TotalExpense;

        /// <summary>
        /// Tỉ lệ sử dụng ngân sách (%).
        /// Trả về 100 nếu Budget = 0 nhưng có chi phí; trả về 0 nếu cả hai đều = 0.
        /// </summary>
        public double UsagePercent => Budget > 0
            ? (double)Math.Round((TotalExpense / Budget) * 100m, 1)
            : (TotalExpense > 0 ? 100.0 : 0.0);

        /// <summary>True nếu chi phí vượt ngân sách định mức.</summary>
        public bool IsOverBudget => TotalExpense > Budget && Budget > 0;
    }
}
