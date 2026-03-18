using TaskFlowManagement.WinForms.Common;

namespace TaskFlowManagement.WinForms.Forms
{
    partial class frmTaskEdit   // BaseForm declared in frmTaskEdit.cs
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Instantiation ─────────────────────────────────────────────────
            panelHeader = new Panel();
            panelAccentLine = new Panel();
            lblHeader = new Label();

            tabControlMain = new TabControl();
            tabGeneral = new TabPage();
            tabComments = new TabPage();
            tabAttachments = new TabPage();

            panelLeft = new Panel();
            panelRight = new Panel();
            panelButtons = new Panel();

            lblTitle = new Label();
            txtTitle = new TextBox();
            lblProject = new Label();
            cboProject = new ComboBox();
            lblAssignee = new Label();
            cboAssignee = new ComboBox();
            lblDescription = new Label();
            txtDescription = new RichTextBox();
            lblEstimated = new Label();
            numEstimatedHours = new NumericUpDown();

            lblPriority = new Label();
            cboPriority = new ComboBox();
            lblStatus = new Label();
            cboStatus = new ComboBox();
            lblCategory = new Label();
            cboCategory = new ComboBox();
            lblProgress = new Label();
            numProgress = new NumericUpDown();
            chkIsCompleted = new CheckBox();
            lblDueDate = new Label();
            chkHasDueDate = new CheckBox();
            dtpDueDate = new DateTimePicker();

            btnSave = new Button();
            btnCancel = new Button();

            panelHeader.SuspendLayout();
            tabControlMain.SuspendLayout();
            tabGeneral.SuspendLayout();
            tabComments.SuspendLayout();
            tabAttachments.SuspendLayout();
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numProgress).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numEstimatedHours).BeginInit();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════
            // panelHeader — Dark banner (THÊM MỚI)
            // ════════════════════════════════════════════════════
            panelHeader.BackColor = UIHelper.ColorHeaderBg;
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 58;
            panelHeader.Name = "panelHeader";
            panelHeader.Controls.Add(lblHeader);
            panelHeader.Controls.Add(panelAccentLine);

            panelAccentLine.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            panelAccentLine.Dock = DockStyle.Bottom;
            panelAccentLine.Height = 4;
            panelAccentLine.Name = "panelAccentLine";

            lblHeader.AutoSize = false;
            lblHeader.Dock = DockStyle.Fill;
            lblHeader.Font = UIHelper.FontHeaderLarge;
            lblHeader.ForeColor = UIHelper.ColorHeaderFg;
            lblHeader.Name = "lblHeader";
            lblHeader.Padding = new Padding(18, 0, 0, 4);
            lblHeader.Text = "📋  Công việc";
            lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ════════════════════════════════════════════════════
            // panelButtons — Dock.Bottom (buttons Lưu / Hủy)
            // ════════════════════════════════════════════════════
            panelButtons.BackColor = UIHelper.ColorSurface;
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Height = 56;
            panelButtons.Name = "panelButtons";

            // btnSave — Primary (SỬA: hard-code Blue → UIHelper)
            UIHelper.StyleButton(btnSave, UIHelper.ButtonVariant.Primary);
            btnSave.Location = new System.Drawing.Point(596, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(110, 34);
            btnSave.Text = "💾  Lưu";
            btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnSave.Click += btnSave_Click;

            // btnCancel — Secondary (BUG FIX: trước dùng màu đỏ cho Cancel → sai UX; đỏ = nguy hiểm, không phải hủy)
            UIHelper.StyleButton(btnCancel, UIHelper.ButtonVariant.Secondary);
            btnCancel.Location = new System.Drawing.Point(716, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(80, 34);
            btnCancel.Text = "✖  Hủy";
            btnCancel.Font = UIHelper.FontBase;
            btnCancel.Click += btnCancel_Click;

            panelButtons.Controls.AddRange(new Control[] { btnSave, btnCancel });

            // ════════════════════════════════════════════════════
            // tabControlMain — Thay thế panelContent, Dock.Fill
            // ════════════════════════════════════════════════════
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Font = UIHelper.FontBase;
            tabControlMain.Name = "tabControlMain";
            tabControlMain.Controls.Add(tabGeneral);
            tabControlMain.Controls.Add(tabComments);
            tabControlMain.Controls.Add(tabAttachments);
            tabControlMain.SelectedIndexChanged += tabControlMain_SelectedIndexChanged;

            // ── tabGeneral ────────────────────────────────────────────────────
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Text = "Chi tiết công việc";
            tabGeneral.BackColor = UIHelper.ColorBackground;
            tabGeneral.Controls.Add(panelLeft);
            tabGeneral.Controls.Add(panelRight);

            // ── tabComments ───────────────────────────────────────────────────
            tabComments.Name = "tabComments";
            tabComments.Text = "Thảo luận";
            tabComments.BackColor = UIHelper.ColorBackground;
            
            pnlCommentsList = new FlowLayoutPanel();
            pnlCommentsList.Dock = DockStyle.Fill;
            pnlCommentsList.AutoScroll = true;
            pnlCommentsList.Padding = new Padding(10);
            pnlCommentsList.FlowDirection = FlowDirection.TopDown;
            pnlCommentsList.WrapContents = false;
            pnlCommentsList.BackColor = UIHelper.ColorBackground;
            pnlCommentsList.Resize += pnlCommentsList_Resize;

            var pnlCommentInput = new Panel();
            pnlCommentInput.Dock = DockStyle.Bottom;
            pnlCommentInput.Height = 60;
            pnlCommentInput.Padding = new Padding(10);
            
            txtNewComment = new TextBox();
            txtNewComment.Multiline = true;
            txtNewComment.Dock = DockStyle.Fill;
            txtNewComment.Font = UIHelper.FontBase;
            
            btnSendComment = new Button();
            btnSendComment.Dock = DockStyle.Right;
            btnSendComment.Width = 80;
            btnSendComment.Text = "Gửi";
            btnSendComment.Font = UIHelper.FontBase;
            UIHelper.StyleButton(btnSendComment, UIHelper.ButtonVariant.Primary);
            btnSendComment.Click += btnSendComment_Click;

            pnlCommentInput.Controls.Add(txtNewComment);
            pnlCommentInput.Controls.Add(btnSendComment);
            
            tabComments.Controls.Add(pnlCommentsList);
            tabComments.Controls.Add(pnlCommentInput);

            // ── tabAttachments ────────────────────────────────────────────────
            tabAttachments.Name = "tabAttachments";
            tabAttachments.Text = "Đính kèm";
            tabAttachments.BackColor = UIHelper.ColorBackground;
            
            lvwAttachments = new ListView();
            lvwAttachments.Dock = DockStyle.Fill;
            lvwAttachments.View = View.Details;
            lvwAttachments.FullRowSelect = true;
            lvwAttachments.AllowDrop = true;
            lvwAttachments.Columns.Add("Tên file", 300);
            lvwAttachments.Columns.Add("Kích thước", 100);
            lvwAttachments.Columns.Add("Ngày tải", 150);
            lvwAttachments.Columns.Add("Người tải", 150);
            lvwAttachments.Font = UIHelper.FontBase;
            lvwAttachments.DoubleClick += lvwAttachments_DoubleClick;
            lvwAttachments.DragEnter += lvwAttachments_DragEnter;
            lvwAttachments.DragDrop += lvwAttachments_DragDrop;
            lvwAttachments.KeyDown += lvwAttachments_KeyDown;
            
            var pnlAttachmentTop = new Panel();
            pnlAttachmentTop.Dock = DockStyle.Top;
            pnlAttachmentTop.Height = 44;
            pnlAttachmentTop.Padding = new Padding(10, 5, 0, 5);

            btnChooseFile = new Button();
            btnChooseFile.Text = "Chọn file...";
            btnChooseFile.Width = 100;
            btnChooseFile.Dock = DockStyle.Left;
            UIHelper.StyleButton(btnChooseFile, UIHelper.ButtonVariant.Secondary);
            btnChooseFile.Click += btnChooseFile_Click;
            
            var lblAttachmentHint = new Label();
            lblAttachmentHint.Text = "  Hoặc kéo thả file vào danh sách bên dưới. Lưu ý: có thể nhấn Delete để xoá file.";
            lblAttachmentHint.Dock = DockStyle.Fill;
            lblAttachmentHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            pnlAttachmentTop.Controls.Add(lblAttachmentHint);
            pnlAttachmentTop.Controls.Add(btnChooseFile);

            tabAttachments.Controls.Add(lvwAttachments);
            tabAttachments.Controls.Add(pnlAttachmentTop);

            // ════════════════════════════════════════════════════
            // panelLeft — Cột trái (tiêu đề, dự án, assignee, mô tả, giờ)
            // Giữ absolute position vì form FixedDialog
            // ════════════════════════════════════════════════════
            panelLeft.BackColor = System.Drawing.Color.Transparent;
            panelLeft.Location = new System.Drawing.Point(12, 12);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new System.Drawing.Size(484, 510);

            // ── Tiêu đề công việc ─────────────────────────────────────────────
            SetFieldLabel(lblTitle, "TIÊU ĐỀ CÔNG VIỆC *", 0, 0);
            txtTitle.BorderStyle = BorderStyle.FixedSingle;
            txtTitle.BackColor = System.Drawing.Color.White;
            txtTitle.Font = UIHelper.FontBase;
            txtTitle.Location = new System.Drawing.Point(0, 20);
            txtTitle.MaxLength = 200;
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "Nhập tiêu đề công việc...";
            txtTitle.Size = new System.Drawing.Size(480, 28);

            // ── Dự án ─────────────────────────────────────────────────────────
            SetFieldLabel(lblProject, "DỰ ÁN *", 0, 60);
            cboProject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProject.Font = UIHelper.FontBase;
            cboProject.Location = new System.Drawing.Point(0, 80);
            cboProject.Name = "cboProject";
            cboProject.Size = new System.Drawing.Size(480, 28);

            // ── Người thực hiện ───────────────────────────────────────────────
            SetFieldLabel(lblAssignee, "NGƯỜI THỰC HIỆN", 0, 120);
            cboAssignee.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAssignee.Font = UIHelper.FontBase;
            cboAssignee.Location = new System.Drawing.Point(0, 140);
            cboAssignee.Name = "cboAssignee";
            cboAssignee.Size = new System.Drawing.Size(480, 28);

            // ── Mô tả ─────────────────────────────────────────────────────────
            SetFieldLabel(lblDescription, "MÔ TẢ CHI TIẾT", 0, 180);
            txtDescription.BackColor = System.Drawing.Color.White;
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.Font = UIHelper.FontBase;
            txtDescription.Location = new System.Drawing.Point(0, 200);
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtDescription.Size = new System.Drawing.Size(480, 140);

            // ── Số giờ ước tính ───────────────────────────────────────────────
            SetFieldLabel(lblEstimated, "GIỜ ƯỚC TÍNH", 0, 354);
            numEstimatedHours.BackColor = System.Drawing.Color.White;
            numEstimatedHours.BorderStyle = BorderStyle.FixedSingle;
            numEstimatedHours.DecimalPlaces = 1;
            numEstimatedHours.Font = UIHelper.FontBase;
            numEstimatedHours.Location = new System.Drawing.Point(0, 374);
            numEstimatedHours.Maximum = 9999;
            numEstimatedHours.Minimum = 0;
            numEstimatedHours.Name = "numEstimatedHours";
            numEstimatedHours.Size = new System.Drawing.Size(120, 28);
            numEstimatedHours.Value = 0;

            panelLeft.Controls.AddRange(new Control[]
            {
                lblTitle, txtTitle, lblProject, cboProject,
                lblAssignee, cboAssignee, lblDescription, txtDescription,
                lblEstimated, numEstimatedHours,
            });

            // ════════════════════════════════════════════════════
            // panelRight — Cột phải (priority, status, category, progress, duedate)
            // ════════════════════════════════════════════════════
            panelRight.BackColor = System.Drawing.Color.Transparent;
            panelRight.Location = new System.Drawing.Point(510, 12);
            panelRight.Name = "panelRight";
            panelRight.Size = new System.Drawing.Size(268, 510);

            // ── Mức ưu tiên ───────────────────────────────────────────────────
            SetFieldLabel(lblPriority, "MỨC ƯU TIÊN *", 0, 0);
            cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPriority.Font = UIHelper.FontBase;
            cboPriority.Location = new System.Drawing.Point(0, 20);
            cboPriority.Name = "cboPriority";
            cboPriority.Size = new System.Drawing.Size(260, 28);

            // ── Trạng thái ────────────────────────────────────────────────────
            SetFieldLabel(lblStatus, "TRẠNG THÁI *", 0, 60);
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Font = UIHelper.FontBase;
            cboStatus.Location = new System.Drawing.Point(0, 80);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new System.Drawing.Size(260, 28);

            // ── Phân loại ─────────────────────────────────────────────────────
            SetFieldLabel(lblCategory, "PHÂN LOẠI (CATEGORY)", 0, 120);
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Font = UIHelper.FontBase;
            cboCategory.Location = new System.Drawing.Point(0, 140);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new System.Drawing.Size(260, 28);

            // ── Tiến độ ───────────────────────────────────────────────────────
            SetFieldLabel(lblProgress, "TIẾN ĐỘ (%)", 0, 180);
            numProgress.BackColor = System.Drawing.Color.White;
            numProgress.Font = UIHelper.FontBase;
            numProgress.Location = new System.Drawing.Point(0, 200);
            numProgress.Maximum = 100;
            numProgress.Minimum = 0;
            numProgress.Name = "numProgress";
            numProgress.Size = new System.Drawing.Size(100, 28);
            numProgress.Value = 0;
            numProgress.ValueChanged += numProgress_ValueChanged;

            chkIsCompleted.Font = UIHelper.FontBase;
            chkIsCompleted.ForeColor = UIHelper.ColorDark;
            chkIsCompleted.Location = new System.Drawing.Point(110, 202);
            chkIsCompleted.Name = "chkIsCompleted";
            chkIsCompleted.Size = new System.Drawing.Size(150, 24);
            chkIsCompleted.Text = "Đã hoàn thành";

            // ── Ngày hạn chót ─────────────────────────────────────────────────
            SetFieldLabel(lblDueDate, "NGÀY HẠN CHÓT", 0, 244);

            chkHasDueDate.Font = UIHelper.FontBase;
            chkHasDueDate.ForeColor = UIHelper.ColorMuted;
            chkHasDueDate.Location = new System.Drawing.Point(0, 266);
            chkHasDueDate.Name = "chkHasDueDate";
            chkHasDueDate.Size = new System.Drawing.Size(160, 24);
            chkHasDueDate.Text = "Có ngày hạn chót";
            chkHasDueDate.CheckedChanged += chkHasDueDate_CheckedChanged;

            dtpDueDate.Enabled = false;
            dtpDueDate.Font = UIHelper.FontBase;
            dtpDueDate.Format = DateTimePickerFormat.Short;
            dtpDueDate.Location = new System.Drawing.Point(0, 294);
            dtpDueDate.Name = "dtpDueDate";
            dtpDueDate.Size = new System.Drawing.Size(260, 28);

            panelRight.Controls.AddRange(new Control[]
            {
                lblPriority, cboPriority, lblStatus, cboStatus,
                lblCategory, cboCategory, lblProgress, numProgress, chkIsCompleted,
                lblDueDate, chkHasDueDate, dtpDueDate,
            });

            // ════════════════════════════════════════════════════
            // Form
            // ════════════════════════════════════════════════════
            this.Text = "📋  Công việc";
            this.Size = new System.Drawing.Size(820, 680);
            this.MinimumSize = new System.Drawing.Size(820, 640);
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "frmTaskEdit";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = UIHelper.FontBase;

            // Thứ tự Add: Fill → Bottom → Top
            this.Controls.Add(tabControlMain); // DockStyle.Fill
            this.Controls.Add(panelButtons);   // DockStyle.Bottom
            this.Controls.Add(panelHeader);    // DockStyle.Top

            panelHeader.ResumeLayout(false);
            tabControlMain.ResumeLayout(false);
            tabGeneral.ResumeLayout(false);
            tabComments.ResumeLayout(false);
            tabAttachments.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numProgress).EndInit();
            ((System.ComponentModel.ISupportInitialize)numEstimatedHours).EndInit();
            this.ResumeLayout(false);
        }

        /// <summary>Label field chuẩn — UPPERCASE, màu muted, font semi-bold.</summary>
        private static void SetFieldLabel(Label lbl, string text, int x, int y)
        {
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lbl.ForeColor = UIHelper.ColorDark;
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Text = text;
        }

        // ── Field declarations ────────────────────────────────────────────────
        private Panel panelHeader, panelAccentLine;
        private Label lblHeader;
        private TabControl tabControlMain;
        private TabPage tabGeneral, tabComments, tabAttachments;
        private Panel panelLeft, panelRight, panelButtons;
        private FlowLayoutPanel pnlCommentsList;
        private TextBox txtNewComment;
        private Button btnSendComment;
        private ListView lvwAttachments;
        private Button btnChooseFile;
        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblDescription;
        private RichTextBox txtDescription;
        private Label lblProject;
        private ComboBox cboProject;
        private Label lblAssignee;
        private ComboBox cboAssignee;
        private Label lblPriority;
        private ComboBox cboPriority;
        private Label lblStatus;
        private ComboBox cboStatus;
        private Label lblCategory;
        private ComboBox cboCategory;
        private Label lblProgress;
        private NumericUpDown numProgress;
        private CheckBox chkIsCompleted;
        private Label lblEstimated;
        private NumericUpDown numEstimatedHours;
        private Label lblDueDate;
        private CheckBox chkHasDueDate;
        private DateTimePicker dtpDueDate;
        private Button btnSave, btnCancel;
    }
}