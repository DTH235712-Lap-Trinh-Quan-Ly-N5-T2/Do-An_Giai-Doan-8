using TaskFlowManagement.Core.Entities;
using TaskFlowManagement.Core.DTOs;
using TaskFlowManagement.Core.Interfaces.Services;
using TaskFlowManagement.WinForms.Common;

namespace TaskFlowManagement.WinForms.Forms
{
    public partial class frmExpenses : BaseForm
    {
        private readonly IExpenseService _expenseService;
        private readonly IProjectService _projectService;

        private List<Expense> _allExpenses = new();
        private Expense? _selectedExpense = null;

        public frmExpenses(IExpenseService expenseService, IProjectService projectService)
        {
            _expenseService = expenseService;
            _projectService = projectService;
            InitializeComponent();
            SetupUI();
            SetupGrid();
            SetupPermissions();
            WireEvents();
        }

        private void SetupUI()
        {
            // 1. Cấu trúc tổng thể
            this.BackColor = UIHelper.ColorBackground;
            panelHeader.BackColor = UIHelper.ColorHeaderBg;
            lblHeader.ForeColor = UIHelper.ColorHeaderFg;
            lblHeader.Font = UIHelper.FontHeaderLarge;

            // 2. Thiết lập DataGridView (Main Content)
            UIHelper.StyleDataGridView(dgvExpenses);
            UIHelper.ApplyAlternateRowColors(dgvExpenses);
            dgvExpenses.Dock = DockStyle.Fill;
            colNote.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colAmount.DefaultCellStyle.Format = "N0";

            // 3. Toolbar & Filter Section
            panelFilter.Height = 50;
            panelToolbar.Height = 54;
            panelToolbar.BackColor = UIHelper.ColorSurface;

            // Tinh chỉnh khoảng cách chống đè chữ (Fix Overlap)
            cboProject.Left = lblProjectFilter.Right + 10;
            lblTypeFilter.Left = cboProject.Right + 20;
            cboExpenseType.Left = lblTypeFilter.Right + 10;
            btnRefresh.Left = cboExpenseType.Right + 15;
            
            UIHelper.StyleButton(btnRefresh, UIHelper.ButtonVariant.Secondary);
            UIHelper.StyleToolButton(btnAdd, "➕ Thêm chi phí", UIHelper.ButtonVariant.Primary, btnAdd.Left, btnAdd.Top, 140, 34);
            UIHelper.StyleToolButton(btnEdit, "✏️ Sửa", UIHelper.ButtonVariant.Success, btnEdit.Left, btnEdit.Top, 80, 34);
            UIHelper.StyleToolButton(btnDelete, "🗑️ Xóa", UIHelper.ButtonVariant.Danger, btnDelete.Left, btnDelete.Top, 80, 34);
            UIHelper.StyleToolButton(btnDetail, "📋 Chi tiết", UIHelper.ButtonVariant.Slate, btnDetail.Left, btnDetail.Top, 100, 34);

            // 4. Dàn đều Summary Cards (Responsive Top Section)
            // Thay thế pnlSummaryCard hiện tại bằng TableLayoutPanel
            var tlpSummary = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent
            };

            // Thiết lập tỷ lệ 25% cho mỗi cột
            for (int i = 0; i < 4; i++)
            {
                tlpSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }

            // Di chuyển các label vào các "Card" panel con của TableLayoutPanel
            tlpSummary.Controls.Add(CreateStatCard(lblBudgetTitle, lblBudgetVal), 0, 0);
            tlpSummary.Controls.Add(CreateStatCard(lblTotalExpenseTitle, lblTotalExpenseVal), 1, 0);
            tlpSummary.Controls.Add(CreateStatCard(lblRemainingTitle, lblRemainingVal), 2, 0);
            tlpSummary.Controls.Add(CreateUsageCard(lblUsagePct), 3, 0);

            // Cấu hình panelSummary để chứa tlpSummary với padding 10px
            panelSummary.Height = 110;
            panelSummary.Padding = new Padding(10, 10, 10, 0);
            panelSummary.Controls.Clear();
            panelSummary.Controls.Add(tlpSummary);

            // Đảm bảo Padding đồng bộ cho toàn bộ form
            panelFilter.Padding = new Padding(10, 0, 10, 0);
            panelToolbar.Padding = new Padding(10, 0, 10, 0);
        }

        private Panel CreateStatCard(Label title, Label val)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UIHelper.ColorSurface,
                Margin = new Padding(4),
                BorderStyle = BorderStyle.None
            };
            
            title.Dock = DockStyle.Top;
            title.Height = 25;
            title.TextAlign = ContentAlignment.BottomLeft;
            title.Font = UIHelper.FontLabel;
            title.ForeColor = UIHelper.ColorMuted;
            title.Padding = new Padding(10, 0, 0, 0);

            val.Dock = DockStyle.Fill;
            val.TextAlign = ContentAlignment.MiddleLeft;
            val.Font = UIHelper.FontHeaderLarge;
            val.Padding = new Padding(10, 0, 0, 10);

            pnl.Controls.Add(val);
            pnl.Controls.Add(title);
            return pnl;
        }

        private Panel CreateUsageCard(Label pct)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UIHelper.ColorSurface,
                Margin = new Padding(4),
                BorderStyle = BorderStyle.None
            };

            var title = new Label
            {
                Text = "TỶ LỆ SỬ DỤNG",
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.BottomLeft,
                Font = UIHelper.FontLabel,
                ForeColor = UIHelper.ColorMuted,
                Padding = new Padding(10, 0, 0, 0)
            };

            pct.Dock = DockStyle.Fill;
            pct.TextAlign = ContentAlignment.MiddleRight;
            pct.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            pct.Padding = new Padding(0, 0, 15, 0);

            pnl.Controls.Add(pct);
            pnl.Controls.Add(title);
            return pnl;
        }

        private void SetupGrid()
        {
            dgvExpenses.AutoGenerateColumns = false;
            colId.DataPropertyName = "Id";
            colProject.DataPropertyName = "ProjectName";
            colType.DataPropertyName = "ExpenseType";
            colAmount.DataPropertyName = "Amount";
            colDate.DataPropertyName = "ExpenseDateDisplay";
            colNote.DataPropertyName = "Note";
            colCreatedBy.DataPropertyName = "CreatorName";
        }

        private void SetupPermissions()
        {
            // Chỉ Manager/Admin mới được thêm/sửa/xóa chi phí
            bool canEdit = AppSession.IsManager || AppSession.IsAdmin;
            btnAdd.Visible = canEdit;
            btnEdit.Visible = canEdit;
            btnDelete.Visible = canEdit;
        }

        private void WireEvents()
        {
            this.Load += async (s, e) => {
                await LoadProjectsAsync();
                await LoadExpensesAsync();
            };

            cboProject.SelectedIndexChanged += async (s, e) => await LoadExpensesAsync();
            cboExpenseType.SelectedIndexChanged += (s, e) => ApplyFilter();
            btnRefresh.Click += async (s, e) => await LoadExpensesAsync();

            dgvExpenses.SelectionChanged += (s, e) => {
                if (dgvExpenses.CurrentRow != null && dgvExpenses.CurrentRow.Selected) {
                    if (dgvExpenses.CurrentRow.Cells["colId"].Value is int id) {
                        _selectedExpense = _allExpenses.FirstOrDefault(x => x.Id == id);
                    }
                } else {
                    _selectedExpense = null;
                }
                UpdateButtons();
            };

            btnAdd.Click += async (s, e) => await OpenEditForm(null);
            btnEdit.Click += async (s, e) => await OpenEditForm(_selectedExpense);
            btnDelete.Click += async (s, e) => await DeleteExpenseAsync();
            btnDetail.Click += (s, e) => ShowDetail();

            dgvExpenses.CellFormatting += dgvExpenses_CellFormatting;
        }

        private void UpdateButtons()
        {
            bool hasSelection = _selectedExpense != null;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnDetail.Enabled = hasSelection;
        }

        private async Task LoadProjectsAsync()
        {
            try
            {
                var projects = await _projectService.GetProjectsForUserAsync(AppSession.UserId, AppSession.IsManager || AppSession.IsAdmin);
                
                cboProject.Items.Clear();
                cboProject.Items.Add(new ComboItem(0, "-- Tất cả dự án --"));
                
                foreach (var p in projects)
                {
                    cboProject.Items.Add(new ComboItem(p.Id, p.Name));
                }
                
                cboProject.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dự án: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExpensesAsync()
        {
            SetStatus("⏳ Đang tải dữ liệu...");
            try
            {
                int projectId = (cboProject.SelectedItem as ComboItem)?.Id ?? 0;
                
                if (projectId > 0)
                {
                    _allExpenses = await _expenseService.GetByProjectAsync(projectId);
                    var summary = await _expenseService.GetProjectBudgetSummaryAsync(projectId);
                    UpdateSummaryCard(summary);
                }
                else
                {
                    // Nếu chọn "Tất cả", ta có thể load hết (Admin) hoặc chỉ load các dự án mà user là member
                    // Ở đây để đơn giản ta load những chi phí thuộc các dự án user có quyền xem
                    var projects = await _projectService.GetProjectsForUserAsync(AppSession.UserId, AppSession.IsManager || AppSession.IsAdmin);
                    _allExpenses = new List<Expense>();
                    foreach(var p in projects)
                    {
                        var pExpenses = await _expenseService.GetByProjectAsync(p.Id);
                        _allExpenses.AddRange(pExpenses);
                    }
                    _allExpenses = _allExpenses.OrderByDescending(x => x.ExpenseDate).ToList();
                    UpdateSummaryCard(null); // Không hiện card cho "Tất cả" hoặc tính tổng hợp? 
                                             // Theo plan: hiện card khi chọn project đơn lẻ.
                }

                ApplyFilter();
                SetStatus($"✅ Đã tải {_allExpenses.Count} bản ghi.");
            }
            catch (Exception ex)
            {
                SetStatus("❌ Lỗi tải dữ liệu.");
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummaryCard(ProjectBudgetSummaryDto? summary)
        {
            if (summary == null)
            {
                lblBudgetVal.Text = "—";
                lblTotalExpenseVal.Text = "—";
                lblRemainingVal.Text = "—";
                lblUsagePct.Text = "0%";
                lblUsagePct.ForeColor = UIHelper.ColorMuted;
                return;
            }

            lblBudgetVal.Text = summary.Budget.ToString("N0") + " ₫";
            lblTotalExpenseVal.Text = summary.TotalExpense.ToString("N0") + " ₫";
            lblRemainingVal.Text = summary.Remaining.ToString("N0") + " ₫";
            lblUsagePct.Text = summary.UsagePercent.ToString("N0") + "%";

            // Màu sắc cảnh báo
            if (summary.IsOverBudget)
            {
                lblTotalExpenseVal.ForeColor = UIHelper.ColorDanger;
                lblRemainingVal.ForeColor = UIHelper.ColorDanger;
                lblUsagePct.ForeColor = UIHelper.ColorDanger;
            }
            else if (summary.UsagePercent > 80)
            {
                lblTotalExpenseVal.ForeColor = UIHelper.ColorWarning;
                lblRemainingVal.ForeColor = UIHelper.ColorWarning;
                lblUsagePct.ForeColor = UIHelper.ColorWarning;
            }
            else
            {
                lblTotalExpenseVal.ForeColor = UIHelper.ColorHeaderBg;
                lblRemainingVal.ForeColor = UIHelper.ColorSuccess;
                lblUsagePct.ForeColor = UIHelper.ColorMuted;
            }
        }

        private void ApplyFilter()
        {
            string typeFilter = cboExpenseType.SelectedItem?.ToString() ?? "— Tất cả —";
            
            var filtered = _allExpenses.Where(x => 
                typeFilter == "— Tất cả —" || x.ExpenseType == typeFilter
            ).ToList();

            BindGrid(filtered);
        }

        private void BindGrid(List<Expense> list)
        {
            var gridData = list.Select(e => new
            {
                e.Id,
                ProjectName = e.Project?.Name ?? "—",
                e.ExpenseType,
                e.Amount, // Để CellFormatting tự định dạng
                ExpenseDateDisplay = e.ExpenseDate.ToString("dd/MM/yyyy"),
                Note = e.Note ?? "—",
                CreatorName = e.CreatedBy?.FullName ?? "Hệ thống"
            }).ToList();

            dgvExpenses.DataSource = gridData;
            lblCount.Text = $"Tổng số: {list.Count} khoản chi.";
        }

        private async Task OpenEditForm(Expense? expense)
        {
            // Chúng ta sẽ triển khai frmExpenseEdit sau
            using var dlg = new frmExpenseEdit(_expenseService, _projectService, expense);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                await LoadExpensesAsync();
                // Trigger dashboard update
                // (Giả sử có event TaskDataChanged như plan) 
                // Ở đây ta cứ load lại là được.
            }
        }

        private async Task DeleteExpenseAsync()
        {
            if (_selectedExpense == null) return;

            if (MessageBox.Show($"Xác nhận xóa khoản chi {(_selectedExpense.Amount.ToString("N0"))} ₫ cho dự án {_selectedExpense.Project?.Name}?", 
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var (ok, msg) = await _expenseService.DeleteExpenseAsync(_selectedExpense.Id);
                if (ok) await LoadExpensesAsync();
                else MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDetail()
        {
            if (_selectedExpense == null) return;
            
            string info = $"Dự án: {_selectedExpense.Project?.Name}\n" +
                          $"Loại: {_selectedExpense.ExpenseType}\n" +
                          $"Số tiền: {_selectedExpense.Amount:N0} ₫\n" +
                          $"Ngày: {_selectedExpense.ExpenseDate:dd/MM/yyyy}\n" +
                          $"Người tạo: {_selectedExpense.CreatedBy?.FullName}\n" +
                          $"Ghi chú: {_selectedExpense.Note ?? "—"}";
            
            MessageBox.Show(info, "Chi tiết chi phí", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvExpenses_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.RowIndex < 0) return;

            // Định dạng cột Số tiền
            if (dgvExpenses.Columns[e.ColumnIndex].Name == "colAmount")
            {
                if (e.Value is decimal amount)
                {
                    e.Value = amount.ToString("N0") + " ₫";
                    e.FormattingApplied = true;
                }
            }
        }

        private void SetStatus(string msg) => lblStatus.Text = msg;
    }
}
