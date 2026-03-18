using TaskFlowManagement.Core.Entities;
using TaskFlowManagement.Core.Interfaces.Services;
using TaskFlowManagement.WinForms.Common;

namespace TaskFlowManagement.WinForms.Forms
{
    public partial class frmExpenseEdit : BaseForm
    {
        private readonly IExpenseService _expenseService;
        private readonly IProjectService _projectService;
        private readonly Expense? _editingExpense;

        public frmExpenseEdit(IExpenseService expenseService, IProjectService projectService, Expense? expense = null)
        {
            _expenseService = expenseService;
            _projectService = projectService;
            _editingExpense = expense;

            InitializeComponent();
            SetupUI();
            WireEvents();
        }

        private void SetupUI()
        {
            if (_editingExpense != null)
            {
                lblTitleForm.Text = "✏️  Sửa chi phí";
                this.Text = "Sửa chi phí";
                btnSave.Text = "💾  Cập nhật chi phí";
            }
            else
            {
                lblTitleForm.Text = "➕  Thêm chi phí mới";
                this.Text = "Thêm chi phí";
                btnSave.Text = "💾  Lưu chi phí";
                dtpDate.Value = DateTime.Today;
            }

            // Financial Precision: Thousands separator and zero decimals
            numAmount.ThousandsSeparator = true;
            numAmount.DecimalPlaces = 0;
            numAmount.Maximum = 1000000000; // 1 tỷ
        }

        private void WireEvents()
        {
            this.Load += async (s, e) => {
                await LoadProjectsAsync();
                if (_editingExpense != null) BindData();
            };

            btnSave.Click += async (s, e) => await SaveAsync();
            btnCancel.Click += (s, e) => this.Close();
        }

        private async Task LoadProjectsAsync()
        {
            try
            {
                var projects = await _projectService.GetProjectsForUserAsync(AppSession.UserId, AppSession.IsManager || AppSession.IsAdmin);
                
                cboProject.Items.Clear();
                foreach (var p in projects)
                {
                    cboProject.Items.Add(new ComboItem(p.Id, p.Name));
                }
                
                if (cboProject.Items.Count > 0) cboProject.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                lblError.Text = "Lỗi tải dự án: " + ex.Message;
            }
        }

        private void BindData()
        {
            if (_editingExpense == null) return;

            // Chọn dự án 
            for (int i = 0; i < cboProject.Items.Count; i++)
            {
                if ((cboProject.Items[i] as ComboItem)?.Id == _editingExpense.ProjectId)
                {
                    cboProject.SelectedIndex = i;
                    break;
                }
            }

            // Chọn loại
            cboType.SelectedItem = _editingExpense.ExpenseType;
            
            // Số tiền
            numAmount.Value = _editingExpense.Amount;
            
            // Ngày
            dtpDate.Value = _editingExpense.ExpenseDate.ToDateTime(TimeOnly.MinValue);
            
            // Ghi chú
            txtNote.Text = _editingExpense.Note;
        }

        private async Task SaveAsync()
        {
            // 1. Validate
            if (cboProject.SelectedItem == null) { ShowError("Vui lòng chọn dự án."); return; }
            if (cboType.SelectedItem == null) { ShowError("Vui lòng chọn loại chi phí."); return; }
            if (numAmount.Value <= 0) { ShowError("Số tiền phải lớn hơn 0."); return; }
            if (dtpDate.Value.Date > DateTime.Today) { ShowError("Ngày chi phí không được lớn hơn ngày hiện tại."); return; }

            lblError.Text = "";
            btnSave.Enabled = false;

            try
            {
                var expense = _editingExpense ?? new Expense();
                expense.ProjectId = (cboProject.SelectedItem as ComboItem)!.Id;
                expense.ExpenseType = cboType.SelectedItem.ToString()!;
                expense.Amount = numAmount.Value;
                expense.ExpenseDate = DateOnly.FromDateTime(dtpDate.Value);
                expense.Note = txtNote.Text.Trim();

                if (_editingExpense == null)
                {
                    // Audit Trail: Người tạo khoản chi (đảm bảo gán từ UI session)
                    expense.CreatedById = AppSession.UserId;

                    var (ok, msg) = await _expenseService.AddExpenseAsync(expense);
                    
                    this.InvokeIfRequired(() => {
                        if (ok) {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        } else {
                            ShowError(msg);
                        }
                    });
                }
                else
                {
                    var (ok, msg) = await _expenseService.UpdateExpenseAsync(expense);
                    
                    this.InvokeIfRequired(() => {
                        if (ok) {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        } else {
                            ShowError(msg);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                this.InvokeIfRequired(() => ShowError("Lỗi hệ thống: " + ex.Message));
            }
            finally
            {
                this.InvokeIfRequired(() => btnSave.Enabled = true);
            }
        }

        private void ShowError(string msg)
        {
            lblError.Text = "⚠️ " + msg;
            System.Media.SystemSounds.Beep.Play();
        }
    }
}
