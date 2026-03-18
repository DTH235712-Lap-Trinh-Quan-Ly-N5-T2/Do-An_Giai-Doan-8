using System.IO;
using System.Reflection;
using TaskFlowManagement.Core.Entities;
using TaskFlowManagement.Core.Interfaces.Services;
using TaskFlowManagement.WinForms.Common;

namespace TaskFlowManagement.WinForms.Forms
{
    /// <summary>
    /// Form Thêm mới / Sửa công việc (TaskItem).
    ///   taskId == null → chế độ Thêm mới
    ///   taskId có giá trị → chế độ Sửa
    ///
    /// DROPDOWN ASSIGNEE:
    ///   Load toàn bộ user active qua GetAllActiveUsersAsync() — không lọc theo ProjectMember.
    ///   Lý do: seed data có thể thiếu member trong ProjectMembers → lọc sẽ làm dropdown rỗng.
    ///   Admin/Manager tự chịu trách nhiệm chọn đúng người.
    ///
    /// PHÂN QUYỀN COMBOBOX (chế độ Sửa):
    ///   | Role                      | cboStatus | cboPriority | cboAssignee |
    ///   |---------------------------|-----------|-------------|-------------|
    ///   | Admin / Manager           |   ✅      |    ✅       |    ✅       |
    ///   | Developer (assignee)      |   ✅      |    ❌       |    ✅       |
    ///   | Developer (không assignee)|   ❌      |    ❌       |    ❌       |
    ///
    /// SO SÁNH ROLE: OrdinalIgnoreCase — DB seed "Admin"/"Manager"/"Developer"
    /// </summary>
    public partial class frmTaskEdit : BaseForm
    {
        // ── Dependencies & State ──────────────────────────────────────────────
        private readonly ITaskService _taskService = null!;
        private readonly IProjectService _projectService = null!;
        private readonly IUserService _userService = null!;

        private readonly int? _taskId;
        private TaskItem? _editingTask;

        private List<Priority> _priorities = new();
        private List<Status> _statuses = new();
        private List<Category> _categories = new();
        private List<Project> _projects = new();
        private List<User> _users = new();

        // ── Constructors ──────────────────────────────────────────────────────

        [Obsolete("Dùng constructor DI. Constructor này chỉ dành cho VS Designer.")]
        public frmTaskEdit()
        {
            InitializeComponent();
        }

        public frmTaskEdit(
            ITaskService taskService,
            IProjectService projectService,
            IUserService userService,
            int? taskId)
#pragma warning disable CS0618
            : this()
#pragma warning restore CS0618
        {
            _taskService = taskService;
            _projectService = projectService;
            _userService = userService;
            _taskId = taskId;
        }

        // ── Form Load ─────────────────────────────────────────────────────────

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var isNew = !_taskId.HasValue;
            var title = isNew
                ? "➕  Thêm công việc mới"
                : $"✏️  Sửa công việc (ID: {_taskId!.Value})";

            this.Text = title;
            lblHeader.Text = title;

            await LoadLookupsAsync();

            if (_taskId.HasValue)
                await LoadTaskForEditAsync(_taskId.Value);
            else
                SetDefaultsForNewTask();

            // DoubleBuffer cho FlowLayoutPanel chống flickering khi cuộn / Resize
            SetDoubleBuffered(pnlCommentsList);

            // phím Enter gửi bình luận (Ctrl+Enter xuống dòng)
            txtNewComment.KeyDown += txtNewComment_KeyDown;
        }

        /// <summary>Bật DoubleBuffered cho bất kỳ Control nào qua Reflection (WinForms ẩn thuộc tính này).</summary>
        private static void SetDoubleBuffered(Control ctrl)
        {
            var prop = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(ctrl, true);
        }

        private void txtNewComment_KeyDown(object? sender, KeyEventArgs e)
        {
            // Enter đơn → gửi; Ctrl+Enter → xuống dòng thực sự
            if (e.KeyCode == Keys.Enter && !e.Control)
            {
                e.SuppressKeyPress = true;
                btnSendComment.PerformClick();
            }
        }

        // ── Load Lookups ──────────────────────────────────────────────────────

        private async Task LoadLookupsAsync()
        {
            try
            {
                var tPriorities = _taskService.GetAllPrioritiesAsync();
                var tStatuses = _taskService.GetAllStatusesAsync();
                var tCategories = _taskService.GetAllCategoriesAsync();
                var tProjects = _projectService.GetProjectsForUserAsync(
                                      AppSession.UserId, AppSession.IsManager);
                var tUsers = _userService.GetAllActiveUsersAsync();

                await Task.WhenAll(tPriorities, tStatuses, tCategories, tProjects, tUsers);

                _priorities = tPriorities.Result;
                _statuses = tStatuses.Result;
                _categories = tCategories.Result;
                _projects = tProjects.Result;
                _users = tUsers.Result;

                cboPriority.Items.Clear();
                cboPriority.Items.Add(new ComboItem(0, "— Chọn mức ưu tiên —"));
                foreach (var p in _priorities.OrderBy(p => p.Level))
                    cboPriority.Items.Add(new ComboItem(p.Id, p.Name));
                cboPriority.SelectedIndex = 0;

                cboStatus.Items.Clear();
                cboStatus.Items.Add(new ComboItem(0, "— Chọn trạng thái —"));
                foreach (var s in _statuses.OrderBy(s => s.DisplayOrder))
                    cboStatus.Items.Add(new ComboItem(s.Id, s.Name));
                cboStatus.SelectedIndex = 0;

                cboCategory.Items.Clear();
                cboCategory.Items.Add(new ComboItem(0, "— Chọn phân loại —"));
                foreach (var c in _categories.OrderBy(c => c.Name))
                    cboCategory.Items.Add(new ComboItem(c.Id, c.Name));
                cboCategory.SelectedIndex = 0;

                cboProject.Items.Clear();
                cboProject.Items.Add(new ComboItem(0, "— Chọn dự án —"));
                foreach (var proj in _projects)
                    cboProject.Items.Add(new ComboItem(proj.Id, proj.Name));
                cboProject.SelectedIndex = 0;

                cboAssignee.Items.Clear();
                cboAssignee.Items.Add(new ComboItem(0, "— Chưa gán —"));
                foreach (var u in _users.OrderBy(u => u.FullName))
                    cboAssignee.Items.Add(new ComboItem(u.Id, u.FullName));
                cboAssignee.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu dropdown:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Load for Edit ─────────────────────────────────────────────────────

        private async Task LoadTaskForEditAsync(int taskId)
        {
            _editingTask = await _taskService.GetByIdAsync(taskId);

            if (_editingTask == null)
            {
                MessageBox.Show("Không tìm thấy công việc này. Có thể đã bị xóa.",
                    "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            txtTitle.Text = _editingTask.Title;
            txtDescription.Text = _editingTask.Description ?? string.Empty;
            numProgress.Value = _editingTask.ProgressPercent;
            numEstimatedHours.Value = (decimal)(_editingTask.EstimatedHours ?? 0);

            if (_editingTask.DueDate.HasValue)
            {
                chkHasDueDate.Checked = true;
                dtpDueDate.Value = _editingTask.DueDate.Value.ToLocalTime();
                dtpDueDate.Enabled = true;
            }
            else
            {
                chkHasDueDate.Checked = false;
                dtpDueDate.Enabled = false;
            }

            SelectComboById(cboProject, _editingTask.ProjectId);
            SelectComboById(cboPriority, _editingTask.PriorityId);
            SelectComboById(cboStatus, _editingTask.StatusId);
            SelectComboById(cboCategory, _editingTask.CategoryId);
            SelectComboById(cboAssignee, _editingTask.AssignedToId ?? 0);

            // Phân quyền chỉnh sửa
            bool isManager = AppSession.Roles.Any(r =>
                r.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                r.Equals("Manager", StringComparison.OrdinalIgnoreCase));

            bool isAssignee = _editingTask.AssignedToId == AppSession.UserId;

            cboStatus.Enabled = isManager || isAssignee;
            cboPriority.Enabled = isManager;
            cboAssignee.Enabled = isManager || isAssignee;

            await LoadCommentsAsync();
            await LoadAttachmentsAsync();
        }

        private void SetDefaultsForNewTask()
        {
            chkHasDueDate.Checked = false;
            dtpDueDate.Enabled = false;
            dtpDueDate.Value = DateTime.Now.AddDays(7);

            bool isManagerOrAbove = AppSession.Roles.Any(r =>
                r.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                r.Equals("Manager", StringComparison.OrdinalIgnoreCase));

            cboStatus.Enabled = true;
            cboPriority.Enabled = isManagerOrAbove;
            cboAssignee.Enabled = true;
        }

        // ── Events ────────────────────────────────────────────────────────────

        private void chkHasDueDate_CheckedChanged(object sender, EventArgs e)
            => dtpDueDate.Enabled = chkHasDueDate.Checked;

        private void numProgress_ValueChanged(object sender, EventArgs e)
            => chkIsCompleted.Checked = numProgress.Value == 100;

        // ── Save ──────────────────────────────────────────────────────────────

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            btnSave.Enabled = false;
            btnSave.Text = "⏳  Đang lưu...";

            try
            {
                var task = BuildTaskFromForm();
                var (ok, msg) = _taskId.HasValue
                    ? await _taskService.UpdateTaskAsync(task)
                    : await _taskService.CreateTaskAsync(task);

                if (ok)
                {
                    MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(msg, "Không thể lưu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không mong đợi:\n" + ex.Message,
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "💾  Lưu";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ── Validate ──────────────────────────────────────────────────────────

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            { MessageBox.Show("Vui lòng nhập tiêu đề công việc.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTitle.Focus(); return false; }

            if (txtTitle.Text.Trim().Length > 200)
            { MessageBox.Show("Tiêu đề không được dài hơn 200 ký tự.", "Tiêu đề quá dài", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTitle.Focus(); return false; }

            if (GetComboId(cboProject) <= 0)
            { MessageBox.Show("Vui lòng chọn dự án cho công việc.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cboProject.Focus(); return false; }

            if (GetComboId(cboPriority) <= 0)
            { MessageBox.Show("Vui lòng chọn mức độ ưu tiên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cboPriority.Focus(); return false; }

            if (GetComboId(cboStatus) <= 0)
            { MessageBox.Show("Vui lòng chọn trạng thái công việc.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cboStatus.Focus(); return false; }

            return true;
        }

        // ── Build Entity ──────────────────────────────────────────────────────

        private TaskItem BuildTaskFromForm()
        {
            var task = _editingTask ?? new TaskItem();

            task.Title = txtTitle.Text.Trim();
            task.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();

            task.ProjectId = GetComboId(cboProject);
            task.PriorityId = GetComboId(cboPriority);
            task.StatusId = GetComboId(cboStatus);
            task.CategoryId = GetComboId(cboCategory);

            var assigneeId = GetComboId(cboAssignee);
            task.AssignedToId = assigneeId > 0 ? assigneeId : null;

            task.ProgressPercent = (byte)numProgress.Value;
            task.IsCompleted = chkIsCompleted.Checked;
            task.EstimatedHours = numEstimatedHours.Value > 0 ? (decimal?)numEstimatedHours.Value : null;
            task.DueDate = chkHasDueDate.Checked ? dtpDueDate.Value.ToUniversalTime() : null;

            if (!_taskId.HasValue)
            {
                task.CreatedById = AppSession.UserId;
                task.CreatedAt = DateTime.UtcNow;
            }

            return task;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static int GetComboId(ComboBox cbo)
            => cbo.SelectedItem is ComboItem ci ? ci.Id : 0;

        private static void SelectComboById(ComboBox cbo, int id)
        {
            if (id <= 0) return;
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if (cbo.Items[i] is ComboItem ci && ci.Id == id)
                { cbo.SelectedIndex = i; return; }
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // GIAI ĐOẠN 7: COMMENT & ATTACHMENT LOGIC
        // ═══════════════════════════════════════════════════════════════════

        private void tabControlMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab == tabComments)
            {
                _ = LoadCommentsAsync();
            }
            else if (tabControlMain.SelectedTab == tabAttachments)
            {
                _ = LoadAttachmentsAsync();
            }
        }

        private async Task LoadCommentsAsync()
        {
            if (!_taskId.HasValue) return;
            
            try 
            {
                var comments = await _taskService.GetCommentsAsync(_taskId.Value);
                
                this.InvokeIfRequired(() => {
                    pnlCommentsList.Controls.Clear();
                    
                    if (comments == null || comments.Count == 0)
                    {
                        var lblEmpty = new Label
                        {
                            Text = "Chưa có bình luận nào cho công việc này. Hãy là người đầu tiên thảo luận!",
                            Font = UIHelper.FontBase,
                            ForeColor = UIHelper.ColorMuted,
                            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Top,
                            Height = 100,
                            AutoSize = false,
                            Tag = "empty"  // đánh dấu để xác định khi gửi tin mới
                        };
                        pnlCommentsList.Controls.Add(lblEmpty);
                    }
                    else 
                    {
                        foreach (var c in comments)
                        {
                            AddCommentToUI(c);
                        }
                    }
                    
                    // Cuộn xuống dưới cùng
                    ScrollToBottom();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScrollToBottom()
        {
            if (pnlCommentsList.VerticalScroll.Visible)
            {
                pnlCommentsList.VerticalScroll.Value = pnlCommentsList.VerticalScroll.Maximum;
            }
            pnlCommentsList.PerformLayout();
        }

        private void pnlCommentsList_Resize(object? sender, EventArgs e)
        {
            pnlCommentsList.SuspendLayout();
            int available = pnlCommentsList.ClientSize.Width - 10;
            foreach (Control ctrl in pnlCommentsList.Controls)
            {
                if (ctrl is Panel pnl && pnl.Tag is Comment)
                {
                    RebuildCommentPanel(pnl, available);
                }
                else if (ctrl is Label emptyLbl)
                {
                    emptyLbl.Width = available;
                }
            }
            pnlCommentsList.ResumeLayout(true);
            pnlCommentsList.PerformLayout();
        }

        /// <summary>
        /// Tính lại layout (location + height) cho một Panel bình luận
        /// dựa trên availableWidth. Gọi khi Add lần đầu và khi Resize.
        /// </summary>
        private void RebuildCommentPanel(Panel pnl, int availableWidth)
        {
            const int innerPad = 10;
            const int lineGap  = 6;

            pnl.Width = availableWidth;

            int textWidth = availableWidth - innerPad * 2;
            if (textWidth < 10) textWidth = 10;

            if (pnl.Controls.Count < 2) return;
            var lblUser    = pnl.Controls[0] as Label;
            var lblContent = pnl.Controls[1] as Label;
            if (lblUser == null || lblContent == null) return;

            // Tính chiều cao thực tế từng label qua MeasureString
            using var g = pnl.CreateGraphics();
            int userH    = (int)Math.Ceiling(g.MeasureString(lblUser.Text,    lblUser.Font,    textWidth).Height);
            int contentH = (int)Math.Ceiling(g.MeasureString(lblContent.Text, lblContent.Font, textWidth).Height);

            lblUser.Location = new Point(innerPad, innerPad);
            lblUser.Size     = new Size(textWidth, userH);

            lblContent.Location = new Point(innerPad, innerPad + userH + lineGap);
            lblContent.Size     = new Size(textWidth, contentH);

            pnl.Height = innerPad + userH + lineGap + contentH + innerPad;
        }

        private void AddCommentToUI(Comment c)
        {
            int available = pnlCommentsList.ClientSize.Width - 10;

            // Panel khung trắng — Height sẽ được tính chính xác bởi RebuildCommentPanel
            var pnl = new Panel
            {
                Width     = available,
                Height    = 60,             // tạm thời
                Margin    = new Padding(0, 0, 0, 6),
                BackColor = System.Drawing.Color.White,
                Tag       = c               // nhận dạng trong Resize handler
            };
            // Không Dock, không Anchor — FlowLayoutPanel quản lý vị trí dọc

            var lblUser = new Label
            {
                Text      = $"{c.User?.FullName ?? "Unknown"} \u2022 {c.CreatedAt.ToLocalTime():g}",
                Font      = new System.Drawing.Font(UIHelper.FontBase.FontFamily, 8.5f, System.Drawing.FontStyle.Bold),
                ForeColor = UIHelper.ColorMuted,
                AutoSize  = false   // chiều cao do RebuildCommentPanel tính
            };

            var lblContent = new Label
            {
                Text      = c.Content,
                Font      = UIHelper.FontBase,
                ForeColor = UIHelper.ColorDark,
                AutoSize  = false
            };

            // Thứ tự Add quan trọng: [0] = lblUser (trên), [1] = lblContent (dưới)
            pnl.Controls.Add(lblUser);
            pnl.Controls.Add(lblContent);

            // Tính vị trí và chiều cao chính xác bằng MeasureString
            RebuildCommentPanel(pnl, available);

            pnlCommentsList.Controls.Add(pnl);
        }


        private async void btnSendComment_Click(object? sender, EventArgs e)
        {
            if (!_taskId.HasValue)
            {
                MessageBox.Show("Vui lòng lưu công việc trước khi bình luận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            var content = txtNewComment.Text.Trim();
            if (string.IsNullOrEmpty(content)) return;

            btnSendComment.Enabled = false;
            var (ok, msg, newComment) = await _taskService.AddCommentAsync(_taskId.Value, content, AppSession.UserId, AppSession.Roles);
            
            if (ok && newComment != null)
            {
                txtNewComment.Clear();

                // Xóa Empty-State label nếu có
                if (pnlCommentsList.Controls.Count == 1
                    && pnlCommentsList.Controls[0] is Label emptyLbl
                    && emptyLbl.Tag is string tag && tag == "empty")
                {
                    pnlCommentsList.Controls.Clear();
                }

                AddCommentToUI(newComment);
                ScrollToBottom();
            }
            else
            {
                MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnSendComment.Enabled = true;
        }

        private async Task LoadAttachmentsAsync()
        {
            if (!_taskId.HasValue) return;
            var attachments = await _taskService.GetAttachmentsAsync(_taskId.Value);
            lvwAttachments.Items.Clear();
            foreach (var a in attachments)
            {
                var item = new ListViewItem(a.FileName);
                item.SubItems.Add($"{a.FileSizeBytes / 1024} KB");
                item.SubItems.Add(a.UploadedAt.ToLocalTime().ToString("g"));
                item.SubItems.Add(a.UploadedBy?.FullName ?? "Unknown");
                item.Tag = a;
                lvwAttachments.Items.Add(item);
            }
        }

        private async void btnChooseFile_Click(object? sender, EventArgs e)
        {
            if (!_taskId.HasValue)
            {
                MessageBox.Show("Vui lòng lưu công việc trước khi đính kèm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Tất cả file|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string? err = ValidateFileForUpload(ofd.FileName);
                if (err != null) { MessageBox.Show(err, "Không thể đính kèm", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                await UploadFileAsync(ofd.FileName);
            }
        }

        // Danh sách đuôi file bị chặn vì mục tiêu bảo mật
        private static readonly HashSet<string> BlockedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".exe", ".bat", ".msi", ".cmd", ".ps1", ".vbs", ".js", ".reg" };
        private const long MaxFileSizeBytes = 10L * 1024 * 1024; // 10 MB

        private static string? ValidateFileForUpload(string filePath)
        {
            var info = new FileInfo(filePath);
            if (!info.Exists) return "File không tồn tại.";
            if (info.Length > MaxFileSizeBytes)
                return $"File quá lớn (tối đa 10 MB). File này có kích thước {info.Length / 1024 / 1024:F1} MB.";
            if (BlockedExtensions.Contains(info.Extension))
                return $"Không được phép đính kèm file có đuôi '{info.Extension}' vì lý do bảo mật.";
            return null;
        }

        private void lvwAttachments_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private async void lvwAttachments_DragDrop(object? sender, DragEventArgs e)
        {
            if (!_taskId.HasValue)
            {
                MessageBox.Show("Vui lòng lưu công việc trước khi đính kèm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var files = (string[])e.Data!.GetData(DataFormats.FileDrop)!;
            if (files.Length > 0)
            {
                string? err = ValidateFileForUpload(files[0]);
                if (err != null) { MessageBox.Show(err, "Không thể đính kèm", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                await UploadFileAsync(files[0]);
            }
        }

        private async Task UploadFileAsync(string filePath)
        {
            btnChooseFile.Enabled = false;
            btnChooseFile.Text = "Đang tải...";

            var (ok, msg, newAttachment) = await _taskService.UploadAttachmentAsync(_taskId!.Value, filePath, AppSession.UserId, AppSession.Roles);
            
            if (ok && newAttachment != null)
            {
                var item = new ListViewItem(newAttachment.FileName);
                item.SubItems.Add($"{newAttachment.FileSizeBytes / 1024} KB");
                item.SubItems.Add(newAttachment.UploadedAt.ToLocalTime().ToString("g"));
                item.SubItems.Add(newAttachment.UploadedBy?.FullName ?? "Unknown");
                item.Tag = newAttachment;
                lvwAttachments.Items.Insert(0, item);
            }
            else
            {
                MessageBox.Show(msg, "Lỗi tải file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnChooseFile.Enabled = true;
            btnChooseFile.Text = "Chọn file...";
        }

        private void lvwAttachments_DoubleClick(object? sender, EventArgs e)
        {
            if (lvwAttachments.SelectedItems.Count == 0) return;
            var item = lvwAttachments.SelectedItems[0];
            if (item.Tag is Attachment a)
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, a.FilePath);
                if (File.Exists(fullPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = fullPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("File không còn tồn tại trên hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async void lvwAttachments_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lvwAttachments.SelectedItems.Count > 0)
            {
                var item = lvwAttachments.SelectedItems[0];
                if (item.Tag is Attachment a)
                {
                    if (MessageBox.Show($"Bạn có chắc muốn xóa file {a.FileName}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var (ok, msg) = await _taskService.DeleteAttachmentAsync(a.Id, AppSession.UserId, AppSession.Roles);
                        if (ok)
                        {
                            lvwAttachments.Items.Remove(item);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
