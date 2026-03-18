using TaskFlowManagement.WinForms.Common;

namespace TaskFlowManagement.WinForms.Forms
{
    partial class frmExpenses
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelHeader = new Panel();
            lblHeader = new Label();
            panelFilter = new Panel();
            lblProjectFilter = new Label();
            cboProject = new ComboBox();
            lblTypeFilter = new Label();
            cboExpenseType = new ComboBox();
            btnRefresh = new Button();
            panelSummary = new Panel();
            pnlSummaryCard = new Panel();
            lblUsagePct = new Label();
            lblRemainingVal = new Label();
            lblTotalExpenseVal = new Label();
            lblBudgetVal = new Label();
            lblRemainingTitle = new Label();
            lblTotalExpenseTitle = new Label();
            lblBudgetTitle = new Label();
            panelToolbar = new Panel();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnDetail = new Button();
            lblCount = new Label();
            dgvExpenses = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colProject = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colNote = new DataGridViewTextBoxColumn();
            colCreatedBy = new DataGridViewTextBoxColumn();
            panelStatus = new Panel();
            lblStatus = new Label();

            panelHeader.SuspendLayout();
            panelFilter.SuspendLayout();
            panelSummary.SuspendLayout();
            pnlSummaryCard.SuspendLayout();
            panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvExpenses).BeginInit();
            this.SuspendLayout();

            // panelHeader
            panelHeader.BackColor = UIHelper.ColorHeaderBg;
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 58;
            panelHeader.Controls.Add(lblHeader);

            lblHeader.AutoSize = false;
            lblHeader.Font = UIHelper.FontHeaderLarge;
            lblHeader.ForeColor = UIHelper.ColorHeaderFg;
            lblHeader.Location = new Point(20, 14);
            lblHeader.Size = new Size(700, 30);
            lblHeader.Text = "💸  Quản lý Chi phí & Ngân sách";

            // panelFilter
            panelFilter.BackColor = UIHelper.ColorBackground;
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Height = 46;
            panelFilter.Controls.AddRange(new Control[] { lblProjectFilter, cboProject, lblTypeFilter, cboExpenseType, btnRefresh });

            lblProjectFilter.AutoSize = true;
            lblProjectFilter.Font = UIHelper.FontLabel;
            lblProjectFilter.Location = new Point(14, 15);
            lblProjectFilter.Text = "Dự án:";

            cboProject.Location = new Point(65, 11);
            cboProject.Size = new Size(220, 26);
            UIHelper.StyleFilterCombo(cboProject);

            lblTypeFilter.AutoSize = true;
            lblTypeFilter.Font = UIHelper.FontLabel;
            lblTypeFilter.Location = new Point(300, 15);
            lblTypeFilter.Text = "Loại:";

            cboExpenseType.Location = new Point(340, 11);
            cboExpenseType.Size = new Size(160, 26);
            UIHelper.StyleFilterCombo(cboExpenseType);
            cboExpenseType.Items.AddRange(new object[] { "— Tất cả —", "Nhân công", "Phần mềm", "Hạ tầng", "Khác" });
            cboExpenseType.SelectedIndex = 0;

            btnRefresh.Location = new Point(515, 11);
            btnRefresh.Size = new Size(110, 26);
            UIHelper.StyleButton(btnRefresh, UIHelper.ButtonVariant.Secondary);
            btnRefresh.Text = "🔄 Làm mới";

            // panelSummary (Budget Card)
            panelSummary.BackColor = UIHelper.ColorBackground;
            panelSummary.Dock = DockStyle.Top;
            panelSummary.Height = 100;
            panelSummary.Controls.Add(pnlSummaryCard);

            pnlSummaryCard.BackColor = UIHelper.ColorSurface;
            pnlSummaryCard.Location = new Point(14, 10);
            pnlSummaryCard.Size = new Size(970, 80);
            pnlSummaryCard.BorderStyle = BorderStyle.FixedSingle;
            pnlSummaryCard.Controls.AddRange(new Control[] { 
                lblUsagePct, lblRemainingVal, lblTotalExpenseVal, lblBudgetVal, 
                lblRemainingTitle, lblTotalExpenseTitle, lblBudgetTitle 
            });

            lblBudgetTitle.Font = UIHelper.FontLabel;
            lblBudgetTitle.ForeColor = UIHelper.ColorMuted;
            lblBudgetTitle.Location = new Point(20, 15);
            lblBudgetTitle.Text = "NGÂN SÁCH";
            lblBudgetTitle.Size = new Size(150, 20);

            lblBudgetVal.Font = UIHelper.FontHeaderLarge;
            lblBudgetVal.ForeColor = UIHelper.ColorPrimary;
            lblBudgetVal.Location = new Point(20, 35);
            lblBudgetVal.Text = "0 ₫";
            lblBudgetVal.Size = new Size(200, 30);

            lblTotalExpenseTitle.Font = UIHelper.FontLabel;
            lblTotalExpenseTitle.ForeColor = UIHelper.ColorMuted;
            lblTotalExpenseTitle.Location = new Point(250, 15);
            lblTotalExpenseTitle.Text = "TỔNG CHI PHÍ";
            lblTotalExpenseTitle.Size = new Size(150, 20);

            lblTotalExpenseVal.Font = UIHelper.FontHeaderLarge;
            lblTotalExpenseVal.ForeColor = UIHelper.ColorHeaderBg;
            lblTotalExpenseVal.Location = new Point(250, 35);
            lblTotalExpenseVal.Text = "0 ₫";
            lblTotalExpenseVal.Size = new Size(200, 30);

            lblRemainingTitle.Font = UIHelper.FontLabel;
            lblRemainingTitle.ForeColor = UIHelper.ColorMuted;
            lblRemainingTitle.Location = new Point(500, 15);
            lblRemainingTitle.Text = "CÒN LẠI";
            lblRemainingTitle.Size = new Size(150, 20);

            lblRemainingVal.Font = UIHelper.FontHeaderLarge;
            lblRemainingVal.ForeColor = UIHelper.ColorSuccess;
            lblRemainingVal.Location = new Point(500, 35);
            lblRemainingVal.Text = "0 ₫";
            lblRemainingVal.Size = new Size(200, 30);

            lblUsagePct.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblUsagePct.ForeColor = UIHelper.ColorMuted;
            lblUsagePct.Location = new Point(780, 15);
            lblUsagePct.Text = "0%";
            lblUsagePct.Size = new Size(160, 50);
            lblUsagePct.TextAlign = ContentAlignment.MiddleRight;

            // panelToolbar
            panelToolbar.BackColor = UIHelper.ColorSurface;
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Height = 52;
            panelToolbar.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnDetail, lblCount });

            int bx = 14, by = 9, bg = 6, bh = 34;
            UIHelper.StyleToolButton(btnAdd, "➕ Thêm chi phí", UIHelper.ButtonVariant.Primary, bx, by, 140, bh); bx += 140 + bg;
            UIHelper.StyleToolButton(btnEdit, "✏️ Sửa", UIHelper.ButtonVariant.Success, bx, by, 80, bh); bx += 80 + bg;
            UIHelper.StyleToolButton(btnDelete, "🗑️ Xóa", UIHelper.ButtonVariant.Danger, bx, by, 80, bh); bx += 80 + bg;
            UIHelper.StyleToolButton(btnDetail, "📋 Chi tiết", UIHelper.ButtonVariant.Slate, bx, by, 100, bh); bx += 100 + bg;

            lblCount.AutoSize = false;
            lblCount.Font = UIHelper.FontSmall;
            lblCount.ForeColor = UIHelper.ColorMuted;
            lblCount.Location = new Point(bx, by);
            lblCount.Size = new Size(200, bh);
            lblCount.TextAlign = ContentAlignment.MiddleLeft;

            // dgvExpenses
            UIHelper.StyleDataGridView(dgvExpenses);
            UIHelper.ApplyAlternateRowColors(dgvExpenses);
            dgvExpenses.Dock = DockStyle.Fill;

            colId.Name = "colId"; colId.Visible = false;
            colProject.Name = "colProject"; colProject.HeaderText = "Dự án"; colProject.Width = 180;
            colType.Name = "colType"; colType.HeaderText = "Loại chi phí"; colType.Width = 140;
            colAmount.Name = "colAmount"; colAmount.HeaderText = "Số tiền"; colAmount.Width = 140;
            colDate.Name = "colDate"; colDate.HeaderText = "Ngày"; colDate.Width = 100;
            colNote.Name = "colNote"; colNote.HeaderText = "Ghi chú"; colNote.Width = 250;
            colCreatedBy.Name = "colCreatedBy"; colCreatedBy.HeaderText = "Người tạo"; colCreatedBy.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvExpenses.Columns.AddRange(new DataGridViewColumn[] { colId, colProject, colType, colAmount, colDate, colNote, colCreatedBy });

            // Status Bar
            (panelStatus, lblStatus) = UIHelper.CreateStatusBar();

            // Form properties
            this.ClientSize = new Size(1000, 700);
            this.Name = "frmExpenses";
            this.Text = "Quản lý Chi phí & Ngân sách";
            this.StartPosition = FormStartPosition.CenterParent;

            this.Controls.Add(dgvExpenses);
            this.Controls.Add(panelStatus);
            this.Controls.Add(panelToolbar);
            this.Controls.Add(panelSummary);
            this.Controls.Add(panelFilter);
            this.Controls.Add(panelHeader);

            panelHeader.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            panelSummary.ResumeLayout(false);
            pnlSummaryCard.ResumeLayout(false);
            panelToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvExpenses).EndInit();
            this.ResumeLayout(false);
        }

        private Panel panelHeader, panelFilter, panelSummary, pnlSummaryCard, panelToolbar, panelStatus;
        private Label lblHeader, lblProjectFilter, lblTypeFilter, lblBudgetTitle, lblBudgetVal, lblTotalExpenseTitle, lblTotalExpenseVal, lblRemainingTitle, lblRemainingVal, lblUsagePct, lblCount, lblStatus;
        private ComboBox cboProject, cboExpenseType;
        private Button btnRefresh, btnAdd, btnEdit, btnDelete, btnDetail;
        private DataGridView dgvExpenses;
        private DataGridViewTextBoxColumn colId, colProject, colType, colAmount, colDate, colNote, colCreatedBy;
    }
}
