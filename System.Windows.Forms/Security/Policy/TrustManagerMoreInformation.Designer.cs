namespace System.Security.Policy
{
	// Token: 0x02000717 RID: 1815
	internal partial class TrustManagerMoreInformation : global::System.Windows.Forms.Form
	{
		// Token: 0x06006088 RID: 24712 RVA: 0x001617D2 File Offset: 0x001607D2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600608A RID: 24714 RVA: 0x0016199C File Offset: 0x0016099C
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::System.Security.Policy.TrustManagerMoreInformation));
			this.tableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.pictureBoxPublisher = new global::System.Windows.Forms.PictureBox();
			this.pictureBoxMachineAccess = new global::System.Windows.Forms.PictureBox();
			this.pictureBoxInstallation = new global::System.Windows.Forms.PictureBox();
			this.pictureBoxLocation = new global::System.Windows.Forms.PictureBox();
			this.lblPublisher = new global::System.Windows.Forms.Label();
			this.lblPublisherContent = new global::System.Windows.Forms.Label();
			this.lblMachineAccess = new global::System.Windows.Forms.Label();
			this.lblMachineAccessContent = new global::System.Windows.Forms.Label();
			this.lblInstallation = new global::System.Windows.Forms.Label();
			this.lblInstallationContent = new global::System.Windows.Forms.Label();
			this.lblLocation = new global::System.Windows.Forms.Label();
			this.lblLocationContent = new global::System.Windows.Forms.Label();
			this.btnClose = new global::System.Windows.Forms.Button();
			this.tableLayoutPanel.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxPublisher).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxMachineAccess).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxInstallation).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLocation).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
			this.tableLayoutPanel.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 389f));
			this.tableLayoutPanel.Controls.Add(this.pictureBoxPublisher, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.pictureBoxMachineAccess, 0, 2);
			this.tableLayoutPanel.Controls.Add(this.pictureBoxInstallation, 0, 4);
			this.tableLayoutPanel.Controls.Add(this.pictureBoxLocation, 0, 6);
			this.tableLayoutPanel.Controls.Add(this.lblPublisher, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.lblPublisherContent, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.lblMachineAccess, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.lblMachineAccessContent, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.lblInstallation, 1, 4);
			this.tableLayoutPanel.Controls.Add(this.lblInstallationContent, 1, 5);
			this.tableLayoutPanel.Controls.Add(this.lblLocation, 1, 6);
			this.tableLayoutPanel.Controls.Add(this.lblLocationContent, 1, 7);
			this.tableLayoutPanel.Controls.Add(this.btnClose, 1, 8);
			this.tableLayoutPanel.Margin = new global::System.Windows.Forms.Padding(12);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			componentResourceManager.ApplyResources(this.pictureBoxPublisher, "pictureBoxPublisher");
			this.pictureBoxPublisher.Margin = new global::System.Windows.Forms.Padding(0, 0, 3, 0);
			this.pictureBoxPublisher.Name = "pictureBoxPublisher";
			this.tableLayoutPanel.SetRowSpan(this.pictureBoxPublisher, 2);
			this.pictureBoxPublisher.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBoxMachineAccess, "pictureBoxMachineAccess");
			this.pictureBoxMachineAccess.Margin = new global::System.Windows.Forms.Padding(0, 10, 3, 0);
			this.pictureBoxMachineAccess.Name = "pictureBoxMachineAccess";
			this.tableLayoutPanel.SetRowSpan(this.pictureBoxMachineAccess, 2);
			this.pictureBoxMachineAccess.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBoxInstallation, "pictureBoxInstallation");
			this.pictureBoxInstallation.Margin = new global::System.Windows.Forms.Padding(0, 10, 3, 0);
			this.pictureBoxInstallation.Name = "pictureBoxInstallation";
			this.tableLayoutPanel.SetRowSpan(this.pictureBoxInstallation, 2);
			this.pictureBoxInstallation.TabStop = false;
			componentResourceManager.ApplyResources(this.pictureBoxLocation, "pictureBoxLocation");
			this.pictureBoxLocation.Margin = new global::System.Windows.Forms.Padding(0, 10, 3, 0);
			this.pictureBoxLocation.Name = "pictureBoxLocation";
			this.tableLayoutPanel.SetRowSpan(this.pictureBoxLocation, 2);
			this.pictureBoxLocation.TabStop = false;
			componentResourceManager.ApplyResources(this.lblPublisher, "lblPublisher");
			this.lblPublisher.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 0);
			this.lblPublisher.Name = "lblPublisher";
			componentResourceManager.ApplyResources(this.lblPublisherContent, "lblPublisherContent");
			this.lblPublisherContent.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 10);
			this.lblPublisherContent.Name = "lblPublisherContent";
			componentResourceManager.ApplyResources(this.lblMachineAccess, "lblMachineAccess");
			this.lblMachineAccess.Margin = new global::System.Windows.Forms.Padding(3, 10, 0, 0);
			this.lblMachineAccess.Name = "lblMachineAccess";
			componentResourceManager.ApplyResources(this.lblMachineAccessContent, "lblMachineAccessContent");
			this.lblMachineAccessContent.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 10);
			this.lblMachineAccessContent.Name = "lblMachineAccessContent";
			componentResourceManager.ApplyResources(this.lblInstallation, "lblInstallation");
			this.lblInstallation.Margin = new global::System.Windows.Forms.Padding(3, 10, 0, 0);
			this.lblInstallation.Name = "lblInstallation";
			componentResourceManager.ApplyResources(this.lblInstallationContent, "lblInstallationContent");
			this.lblInstallationContent.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 10);
			this.lblInstallationContent.Name = "lblInstallationContent";
			componentResourceManager.ApplyResources(this.lblLocation, "lblLocation");
			this.lblLocation.Margin = new global::System.Windows.Forms.Padding(3, 10, 0, 0);
			this.lblLocation.Name = "lblLocation";
			componentResourceManager.ApplyResources(this.lblLocationContent, "lblLocationContent");
			this.lblLocationContent.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 10);
			this.lblLocationContent.Name = "lblLocationContent";
			componentResourceManager.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnClose.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Margin = new global::System.Windows.Forms.Padding(0, 10, 0, 0);
			this.btnClose.MinimumSize = new global::System.Drawing.Size(75, 23);
			this.btnClose.Name = "btnClose";
			this.btnClose.Padding = new global::System.Windows.Forms.Padding(10, 0, 10, 0);
			this.tableLayoutPanel.SetColumnSpan(this.btnClose, 2);
			base.AcceptButton = this.btnClose;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.AutoSizeMode = global::System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = this.btnClose;
			base.Controls.Add(this.tableLayoutPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TrustManagerMoreInformation";
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxPublisher).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxMachineAccess).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxInstallation).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBoxLocation).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04003A7D RID: 14973
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04003A7E RID: 14974
		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel;

		// Token: 0x04003A7F RID: 14975
		private global::System.Windows.Forms.Label lblPublisher;

		// Token: 0x04003A80 RID: 14976
		private global::System.Windows.Forms.Label lblPublisherContent;

		// Token: 0x04003A81 RID: 14977
		private global::System.Windows.Forms.Label lblMachineAccess;

		// Token: 0x04003A82 RID: 14978
		private global::System.Windows.Forms.Label lblMachineAccessContent;

		// Token: 0x04003A83 RID: 14979
		private global::System.Windows.Forms.Label lblInstallation;

		// Token: 0x04003A84 RID: 14980
		private global::System.Windows.Forms.Label lblInstallationContent;

		// Token: 0x04003A85 RID: 14981
		private global::System.Windows.Forms.Label lblLocation;

		// Token: 0x04003A86 RID: 14982
		private global::System.Windows.Forms.Label lblLocationContent;

		// Token: 0x04003A87 RID: 14983
		private global::System.Windows.Forms.PictureBox pictureBoxPublisher;

		// Token: 0x04003A88 RID: 14984
		private global::System.Windows.Forms.PictureBox pictureBoxMachineAccess;

		// Token: 0x04003A89 RID: 14985
		private global::System.Windows.Forms.PictureBox pictureBoxLocation;

		// Token: 0x04003A8A RID: 14986
		private global::System.Windows.Forms.PictureBox pictureBoxInstallation;

		// Token: 0x04003A8B RID: 14987
		private global::System.Windows.Forms.Button btnClose;
	}
}
