using System;
using System.Collections.Generic;

namespace TaskFlowManagement.Core.DTOs
{
    // =====================================================
    // DTO cho GĐ6: Dashboard & Thống kê
    // Cung cấp số liệu tổng quan cho màn hình Admin/Manager
    // =====================================================

    public class DashboardStatsDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int DueSoonTasks { get; set; }
        public List<StatusSummaryDto> StatusSummaries { get; set; } = new List<StatusSummaryDto>();
        public List<ProjectProgressDto> ProjectProgresses { get; set; } = new List<ProjectProgressDto>();
    }

    public class StatusSummaryDto
    {
        public string StatusName { get; set; } = string.Empty;
        public int Count { get; set; }
        public string ColorHex { get; set; } = string.Empty;
    }

    public class ProjectProgressDto
    {
        public string ProjectName { get; set; } = string.Empty;
        // ĐÃ FIX: Đổi sang decimal để đồng bộ độ chính xác với Budget
        public decimal ProgressPercentage { get; set; }
    }

    public class BudgetReportDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Remaining => Budget - TotalExpense;
        public decimal UsagePercentage => Budget > 0 ? Math.Round((TotalExpense / Budget) * 100m, 1) : 0m;
    }

    public class ProgressReportDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public decimal AvgProgress { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ProjectBudgetSummaryDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Remaining => Budget - TotalExpense;

        public decimal UsagePercent => Budget > 0
            ? Math.Round((TotalExpense / Budget) * 100m, 1)
            : 0m;

        public bool IsOverBudget => TotalExpense > Budget && Budget > 0;
    }
}