namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B4 RID: 1972
	internal partial class GridErrorDlg : global::System.Windows.Forms.Form
	{
		// Token: 0x06006867 RID: 26727 RVA: 0x0017E480 File Offset: 0x0017D480
		private void InitializeComponent()
		{
			this.detailsBtn = new global::System.Windows.Forms.Button();
			this.overarchingTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.buttonTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.okBtn = new global::System.Windows.Forms.Button();
			this.cancelBtn = new global::System.Windows.Forms.Button();
			this.pictureLabelTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.lblMessage = new global::System.Windows.Forms.Label();
			this.pictureBox = new global::System.Windows.Forms.PictureBox();
			this.details = new global::System.Windows.Forms.TextBox();
			this.overarchingTableLayoutPanel.SuspendLayout();
			this.buttonTableLayoutPanel.SuspendLayout();
			this.pictureLabelTableLayoutPanel.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
			base.SuspendLayout();
			this.lblMessage.Location = new global::System.Drawing.Point(73, 30);
			this.lblMessage.Margin = new global::System.Windows.Forms.Padding(3, 30, 3, 0);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new global::System.Drawing.Size(208, 43);
			this.lblMessage.TabIndex = 0;
			this.pictureBox.Location = new global::System.Drawing.Point(3, 3);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new global::System.Drawing.Size(64, 64);
			this.pictureBox.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox.TabIndex = 5;
			this.pictureBox.TabStop = false;
			this.detailsBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.detailsBtn.Location = new global::System.Drawing.Point(3, 3);
			this.detailsBtn.Margin = new global::System.Windows.Forms.Padding(12, 3, 29, 3);
			this.detailsBtn.Name = "detailsBtn";
			this.detailsBtn.Size = new global::System.Drawing.Size(100, 23);
			this.detailsBtn.TabIndex = 4;
			this.detailsBtn.Click += new global::System.EventHandler(this.DetailsClick);
			this.overarchingTableLayoutPanel.AutoSize = true;
			this.overarchingTableLayoutPanel.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.overarchingTableLayoutPanel.ColumnCount = 1;
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.overarchingTableLayoutPanel.Controls.Add(this.buttonTableLayoutPanel, 0, 1);
			this.overarchingTableLayoutPanel.Controls.Add(this.pictureLabelTableLayoutPanel, 0, 0);
			this.overarchingTableLayoutPanel.Location = new global::System.Drawing.Point(1, 0);
			this.overarchingTableLayoutPanel.MinimumSize = new global::System.Drawing.Size(279, 50);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			this.overarchingTableLayoutPanel.RowCount = 2;
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Absolute, 20f));
			this.overarchingTableLayoutPanel.Size = new global::System.Drawing.Size(290, 108);
			this.overarchingTableLayoutPanel.TabIndex = 6;
			this.buttonTableLayoutPanel.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.buttonTableLayoutPanel.AutoSize = true;
			this.buttonTableLayoutPanel.ColumnCount = 3;
			this.overarchingTableLayoutPanel.SetColumnSpan(this.buttonTableLayoutPanel, 2);
			this.buttonTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			this.buttonTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.buttonTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.buttonTableLayoutPanel.Controls.Add(this.cancelBtn, 2, 0);
			this.buttonTableLayoutPanel.Controls.Add(this.okBtn, 1, 0);
			this.buttonTableLayoutPanel.Controls.Add(this.detailsBtn, 0, 0);
			this.buttonTableLayoutPanel.Location = new global::System.Drawing.Point(0, 79);
			this.buttonTableLayoutPanel.Name = "buttonTableLayoutPanel";
			this.buttonTableLayoutPanel.RowCount = 1;
			this.buttonTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.buttonTableLayoutPanel.Size = new global::System.Drawing.Size(290, 29);
			this.buttonTableLayoutPanel.TabIndex = 8;
			this.okBtn.AutoSize = true;
			this.okBtn.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.okBtn.Location = new global::System.Drawing.Point(131, 3);
			this.okBtn.Name = "okBtn";
			this.okBtn.Size = new global::System.Drawing.Size(75, 23);
			this.okBtn.TabIndex = 1;
			this.okBtn.Click += new global::System.EventHandler(this.OnButtonClick);
			this.cancelBtn.AutoSize = true;
			this.cancelBtn.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelBtn.Location = new global::System.Drawing.Point(212, 3);
			this.cancelBtn.Margin = new global::System.Windows.Forms.Padding(0, 3, 3, 3);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new global::System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 2;
			this.cancelBtn.Click += new global::System.EventHandler(this.OnButtonClick);
			this.pictureLabelTableLayoutPanel.ColumnCount = 2;
			this.pictureLabelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.pictureLabelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			this.pictureLabelTableLayoutPanel.Controls.Add(this.lblMessage, 1, 0);
			this.pictureLabelTableLayoutPanel.Controls.Add(this.pictureBox, 0, 0);
			this.pictureLabelTableLayoutPanel.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.pictureLabelTableLayoutPanel.Location = new global::System.Drawing.Point(3, 3);
			this.pictureLabelTableLayoutPanel.Name = "pictureLabelTableLayoutPanel";
			this.pictureLabelTableLayoutPanel.RowCount = 1;
			this.pictureLabelTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Absolute, 70f));
			this.pictureLabelTableLayoutPanel.Size = new global::System.Drawing.Size(284, 73);
			this.pictureLabelTableLayoutPanel.TabIndex = 4;
			this.details.Location = new global::System.Drawing.Point(4, 114);
			this.details.Multiline = true;
			this.details.Name = "details";
			this.details.ReadOnly = true;
			this.details.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this.details.Size = new global::System.Drawing.Size(273, 100);
			this.details.TabIndex = 3;
			this.details.TabStop = false;
			this.details.Visible = false;
			base.AcceptButton = this.okBtn;
			this.AutoSize = true;
			base.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = this.cancelBtn;
			base.ClientSize = new global::System.Drawing.Size(299, 113);
			base.Controls.Add(this.details);
			base.Controls.Add(this.overarchingTableLayoutPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Form1";
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			this.buttonTableLayoutPanel.ResumeLayout(false);
			this.buttonTableLayoutPanel.PerformLayout();
			this.pictureLabelTableLayoutPanel.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003D70 RID: 15728
		private global::System.Windows.Forms.TableLayoutPanel overarchingTableLayoutPanel;

		// Token: 0x04003D71 RID: 15729
		private global::System.Windows.Forms.TableLayoutPanel buttonTableLayoutPanel;

		// Token: 0x04003D72 RID: 15730
		private global::System.Windows.Forms.PictureBox pictureBox;

		// Token: 0x04003D73 RID: 15731
		private global::System.Windows.Forms.Label lblMessage;

		// Token: 0x04003D74 RID: 15732
		private global::System.Windows.Forms.Button detailsBtn;

		// Token: 0x04003D75 RID: 15733
		private global::System.Windows.Forms.Button cancelBtn;

		// Token: 0x04003D76 RID: 15734
		private global::System.Windows.Forms.Button okBtn;

		// Token: 0x04003D77 RID: 15735
		private global::System.Windows.Forms.TableLayoutPanel pictureLabelTableLayoutPanel;

		// Token: 0x04003D78 RID: 15736
		private global::System.Windows.Forms.TextBox details;
	}
}
