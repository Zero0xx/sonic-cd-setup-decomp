using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B4 RID: 1972
	internal partial class GridErrorDlg : Form
	{
		// Token: 0x17001628 RID: 5672
		// (set) Token: 0x06006863 RID: 26723 RVA: 0x0017E26C File Offset: 0x0017D26C
		public string Details
		{
			set
			{
				this.details.Text = value;
			}
		}

		// Token: 0x17001629 RID: 5673
		// (set) Token: 0x06006864 RID: 26724 RVA: 0x0017E27A File Offset: 0x0017D27A
		public string Message
		{
			set
			{
				this.lblMessage.Text = value;
			}
		}

		// Token: 0x06006865 RID: 26725 RVA: 0x0017E288 File Offset: 0x0017D288
		public GridErrorDlg(PropertyGrid owner)
		{
			this.ownerGrid = owner;
			this.expandImage = new Bitmap(typeof(ThreadExceptionDialog), "down.bmp");
			this.expandImage.MakeTransparent();
			this.collapseImage = new Bitmap(typeof(ThreadExceptionDialog), "up.bmp");
			this.collapseImage.MakeTransparent();
			this.InitializeComponent();
			foreach (object obj in base.Controls)
			{
				Control control = (Control)obj;
				if (control.SupportsUseCompatibleTextRendering)
				{
					control.UseCompatibleTextRenderingInt = this.ownerGrid.UseCompatibleTextRendering;
				}
			}
			this.pictureBox.Image = SystemIcons.Warning.ToBitmap();
			this.detailsBtn.Text = " " + SR.GetString("ExDlgShowDetails");
			this.details.AccessibleName = SR.GetString("ExDlgDetailsText");
			this.okBtn.Text = SR.GetString("ExDlgOk");
			this.cancelBtn.Text = SR.GetString("ExDlgCancel");
			this.detailsBtn.Image = this.expandImage;
		}

		// Token: 0x06006866 RID: 26726 RVA: 0x0017E3D4 File Offset: 0x0017D3D4
		private void DetailsClick(object sender, EventArgs devent)
		{
			int num = this.details.Height + 8;
			if (this.details.Visible)
			{
				this.detailsBtn.Image = this.expandImage;
				base.Height -= num;
			}
			else
			{
				this.detailsBtn.Image = this.collapseImage;
				this.details.Width = this.overarchingTableLayoutPanel.Width - this.details.Margin.Horizontal;
				base.Height += num;
			}
			this.details.Visible = !this.details.Visible;
		}

		// Token: 0x06006868 RID: 26728 RVA: 0x0017EBDE File Offset: 0x0017DBDE
		private void OnButtonClick(object s, EventArgs e)
		{
			base.DialogResult = ((Button)s).DialogResult;
			base.Close();
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x0017EBF8 File Offset: 0x0017DBF8
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Visible)
			{
				using (Graphics graphics = base.CreateGraphics())
				{
					int num = (int)Math.Ceiling((double)PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, graphics, this.detailsBtn.Text, this.detailsBtn.Font).Width);
					num += this.detailsBtn.Image.Width;
					this.detailsBtn.Width = (int)Math.Ceiling((double)((float)num * (this.ownerGrid.UseCompatibleTextRendering ? 1.15f : 1.4f)));
					this.detailsBtn.Height = this.okBtn.Height;
				}
				if (this.details.Visible)
				{
					this.DetailsClick(this.details, EventArgs.Empty);
				}
			}
			this.okBtn.Focus();
		}

		// Token: 0x04003D79 RID: 15737
		private Bitmap expandImage;

		// Token: 0x04003D7A RID: 15738
		private Bitmap collapseImage;

		// Token: 0x04003D7B RID: 15739
		private PropertyGrid ownerGrid;
	}
}
