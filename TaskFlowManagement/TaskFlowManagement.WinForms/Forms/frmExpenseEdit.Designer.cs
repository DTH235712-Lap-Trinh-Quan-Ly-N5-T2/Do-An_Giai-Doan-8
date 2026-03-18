using TaskFlowManagement.WinForms.Common;

namespace TaskFlowManagement.WinForms.Forms
{
    partial class frmExpenseEdit
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
            lblTitleForm = new Label();
            panelAccentLine = new Panel();
            panelBody = new Panel();
            lblProject = new Label();
            cboProject = new ComboBox();
            lblType = new Label();
            cboType = new ComboBox();
            lblAmount = new Label();
            numAmount = new NumericUpDown();
            lblDate = new Label();
            dtpDate = new DateTimePicker();
            lblNote = new Label();
            txtNote = new TextBox();
            panelFooter = new Panel();
            panelFooterLine = new Panel();
            lblError = new Label();
            btnSave = new Button();
            btnCancel = new Button();

            panelHeader.SuspendLayout();
            panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
            panelFooter.SuspendLayout();
            this.SuspendLayout();

            // panelHeader
            panelHeader.BackColor = UIHelper.ColorHeaderBg;
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 64;
            panelHeader.Controls.Add(lblTitleForm);
            panelHeader.Controls.Add(panelAccentLine);

            lblTitleForm.AutoSize = false;
            lblTitleForm.Dock = DockStyle.Fill;
            lblTitleForm.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitleForm.ForeColor = UIHelper.ColorHeaderFg;
            lblTitleForm.Padding = new Padding(18, 0, 0, 4);
            lblTitleForm.Text = "➕  Thêm chi phí mới";
            lblTitleForm.TextAlign = ContentAlignment.MiddleLeft;

            panelAccentLine.BackColor = UIHelper.ColorPrimary;
            panelAccentLine.Dock = DockStyle.Bottom;
            panelAccentLine.Height = 4;

            // panelBody
            panelBody.BackColor = UIHelper.ColorBackground;
            panelBody.Dock = DockStyle.Fill;
            panelBody.Padding = new Padding(20);
            panelBody.Controls.AddRange(new Control[] {
                lblProject, cboProject, lblType, cboType,
                lblAmount, numAmount, lblDate, dtpDate,
                lblNote, txtNote
            });

            int y = 20, gap = 60, lx = 20, tw = 360;

            SetLabel(lblProject, "DỰ ÁN *", lx, y);
            StyleCombo(cboProject, lx, y + 20, tw, 1);

            y += gap;
            SetLabel(lblType, "LOẠI CHI PHÍ *", lx, y);
            StyleCombo(cboType, lx, y + 20, tw, 2);
            cboType.Items.AddRange(new object[] { "Nhân công", "Phần mềm", "Hạ tầng", "Khác" });

            y += gap;
            SetLabel(lblAmount, "SỐ TIỀN (VNĐ) *", lx, y);
            numAmount.Location = new Point(lx, y + 20);
            numAmount.Size = new Size( tw, 30);
            numAmount.Maximum = 999999999999;
            numAmount.ThousandsSeparator = true;
            numAmount.Font = UIHelper.FontBase;
            numAmount.TabIndex = 3;

            y += gap;
            SetLabel(lblDate, "NGÀY PHÁT SINH", lx, y);
            dtpDate.Location = new Point(lx, y + 20);
            dtpDate.Size = new Size(tw, 30);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Font = UIHelper.FontBase;
            dtpDate.TabIndex = 4;

            y += gap;
            SetLabel(lblNote, "GHI CHÚ", lx, y);
            txtNote.Location = new Point(lx, y + 20);
            txtNote.Size = new Size(tw, 80);
            txtNote.Multiline = true;
            txtNote.Font = UIHelper.FontBase;
            txtNote.BorderStyle = BorderStyle.FixedSingle;
            txtNote.PlaceholderText = "Nhập ghi chú (nếu có)...";
            txtNote.TabIndex = 5;

            // panelFooter
            panelFooter.BackColor = Color.White;
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Height = 80;
            panelFooter.Controls.AddRange(new Control[] { panelFooterLine, lblError, btnSave, btnCancel });

            panelFooterLine.BackColor = UIHelper.ColorBorderLight;
            panelFooterLine.Dock = DockStyle.Top;
            panelFooterLine.Height = 1;

            lblError.ForeColor = UIHelper.ColorDanger;
            lblError.Font = UIHelper.FontSmall;
            lblError.Location = new Point(20, 6);
            lblError.Size = new Size(400, 20);
            lblError.Text = "";

            UIHelper.StyleButton(btnSave, UIHelper.ButtonVariant.Primary);
            btnSave.Location = new Point(20, 28);
            btnSave.Size = new Size(250, 40);
            btnSave.Text = "💾  Lưu chi phí";

            UIHelper.StyleButton(btnCancel, UIHelper.ButtonVariant.Secondary);
            btnCancel.Location = new Point(280, 28);
            btnCancel.Size = new Size(100, 40);
            btnCancel.Text = "Hủy";

            // Form
            this.ClientSize = new Size(420, 520);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExpenseEdit";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chi tiết chi phí";

            this.Controls.Add(panelBody);
            this.Controls.Add(panelFooter);
            this.Controls.Add(panelHeader);

            panelHeader.ResumeLayout(false);
            panelBody.ResumeLayout(false);
            panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
            panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private static void SetLabel(Label lbl, string text, int x, int y)
        {
            lbl.AutoSize = true;
            lbl.Font = UIHelper.FontLabel;
            lbl.Location = new Point(x, y);
            lbl.Text = text;
        }

        private static void StyleCombo(ComboBox cbo, int x, int y, int w, int tabIdx)
        {
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo.Font = UIHelper.FontBase;
            cbo.Location = new Point(x, y);
            cbo.Size = new Size(w, 30);
            cbo.TabIndex = tabIdx;
        }

        private Panel panelHeader, panelAccentLine, panelBody, panelFooter, panelFooterLine;
        private Label lblTitleForm, lblError, lblProject, lblType, lblAmount, lblDate, lblNote;
        private ComboBox cboProject, cboType;
        private NumericUpDown numAmount;
        private DateTimePicker dtpDate;
        private TextBox txtNote;
        private Button btnSave, btnCancel;
    }
}
