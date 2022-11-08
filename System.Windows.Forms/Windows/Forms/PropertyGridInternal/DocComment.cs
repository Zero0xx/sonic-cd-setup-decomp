using System;
using System.Drawing;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007AD RID: 1965
	internal class DocComment : PropertyGrid.SnappableControl
	{
		// Token: 0x06006838 RID: 26680 RVA: 0x0017D94C File Offset: 0x0017C94C
		internal DocComment(PropertyGrid owner) : base(owner)
		{
			base.SuspendLayout();
			this.m_labelTitle = new Label();
			this.m_labelTitle.UseMnemonic = false;
			this.m_labelTitle.Cursor = Cursors.Default;
			this.m_labelDesc = new Label();
			this.m_labelDesc.AutoEllipsis = true;
			this.m_labelDesc.Cursor = Cursors.Default;
			this.UpdateTextRenderingEngine();
			base.Controls.Add(this.m_labelTitle);
			base.Controls.Add(this.m_labelDesc);
			base.Size = new Size(0, 59);
			this.Text = SR.GetString("PBRSDocCommentPaneTitle");
			base.SetStyle(ControlStyles.Selectable, false);
			base.ResumeLayout(false);
		}

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06006839 RID: 26681 RVA: 0x0017DA1F File Offset: 0x0017CA1F
		// (set) Token: 0x0600683A RID: 26682 RVA: 0x0017DA34 File Offset: 0x0017CA34
		public virtual int Lines
		{
			get
			{
				this.UpdateUIWithFont();
				return base.Height / this.lineHeight;
			}
			set
			{
				this.UpdateUIWithFont();
				base.Size = new Size(base.Width, 1 + value * this.lineHeight);
			}
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x0017DA58 File Offset: 0x0017CA58
		public override int GetOptimalHeight(int width)
		{
			this.UpdateUIWithFont();
			int num = this.m_labelTitle.Size.Height;
			if (this.ownerGrid.IsHandleCreated && !base.IsHandleCreated)
			{
				base.CreateControl();
			}
			Graphics graphics = this.m_labelDesc.CreateGraphicsInternal();
			SizeF value = PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, graphics, this.m_labelTitle.Text, this.Font, width);
			Size size = Size.Ceiling(value);
			graphics.Dispose();
			num += size.Height * 2 + 2;
			return Math.Max(num + 4, 59);
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x0017DAEB File Offset: 0x0017CAEB
		internal virtual void LayoutWindow()
		{
		}

		// Token: 0x0600683D RID: 26685 RVA: 0x0017DAED File Offset: 0x0017CAED
		protected override void OnFontChanged(EventArgs e)
		{
			this.needUpdateUIWithFont = true;
			base.PerformLayout();
			base.OnFontChanged(e);
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x0017DB04 File Offset: 0x0017CB04
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.UpdateUIWithFont();
			Size clientSize = base.ClientSize;
			clientSize.Width = Math.Max(0, clientSize.Width - 6);
			clientSize.Height = Math.Max(0, clientSize.Height - 6);
			this.m_labelTitle.SetBounds(this.m_labelTitle.Top, this.m_labelTitle.Left, clientSize.Width, Math.Min(this.lineHeight, clientSize.Height), BoundsSpecified.Size);
			this.m_labelDesc.SetBounds(this.m_labelDesc.Top, this.m_labelDesc.Left, clientSize.Width, Math.Max(0, clientSize.Height - this.lineHeight - 1), BoundsSpecified.Size);
			this.m_labelDesc.Text = this.fullDesc;
			this.m_labelDesc.AccessibleName = this.fullDesc;
			base.OnLayout(e);
		}

		// Token: 0x0600683F RID: 26687 RVA: 0x0017DBF0 File Offset: 0x0017CBF0
		protected override void OnResize(EventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			if (!this.rect.IsEmpty && clientRectangle.Width > this.rect.Width)
			{
				Rectangle rc = new Rectangle(this.rect.Width - 1, 0, clientRectangle.Width - this.rect.Width + 1, this.rect.Height);
				base.Invalidate(rc);
			}
			this.rect = clientRectangle;
			base.OnResize(e);
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x0017DC6F File Offset: 0x0017CC6F
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.UpdateUIWithFont();
		}

		// Token: 0x06006841 RID: 26689 RVA: 0x0017DC80 File Offset: 0x0017CC80
		public virtual void SetComment(string title, string desc)
		{
			if (this.m_labelDesc.Text != title)
			{
				this.m_labelTitle.Text = title;
			}
			if (desc != this.fullDesc)
			{
				this.fullDesc = desc;
				this.m_labelDesc.Text = this.fullDesc;
				this.m_labelDesc.AccessibleName = this.fullDesc;
			}
		}

		// Token: 0x06006842 RID: 26690 RVA: 0x0017DCE4 File Offset: 0x0017CCE4
		public override int SnapHeightRequest(int cyNew)
		{
			this.UpdateUIWithFont();
			int num = Math.Max(2, cyNew / this.lineHeight);
			return 1 + num * this.lineHeight;
		}

		// Token: 0x06006843 RID: 26691 RVA: 0x0017DD10 File Offset: 0x0017CD10
		internal void UpdateTextRenderingEngine()
		{
			this.m_labelTitle.UseCompatibleTextRendering = this.ownerGrid.UseCompatibleTextRendering;
			this.m_labelDesc.UseCompatibleTextRendering = this.ownerGrid.UseCompatibleTextRendering;
		}

		// Token: 0x06006844 RID: 26692 RVA: 0x0017DD40 File Offset: 0x0017CD40
		private void UpdateUIWithFont()
		{
			if (base.IsHandleCreated && this.needUpdateUIWithFont)
			{
				try
				{
					this.m_labelTitle.Font = new Font(this.Font, FontStyle.Bold);
				}
				catch
				{
				}
				this.lineHeight = this.Font.Height + 2;
				this.m_labelTitle.Location = new Point(3, 3);
				this.m_labelDesc.Location = new Point(3, 3 + this.lineHeight);
				this.needUpdateUIWithFont = false;
				base.PerformLayout();
			}
		}

		// Token: 0x04003D60 RID: 15712
		protected const int CBORDER = 3;

		// Token: 0x04003D61 RID: 15713
		protected const int CXDEF = 0;

		// Token: 0x04003D62 RID: 15714
		protected const int CYDEF = 59;

		// Token: 0x04003D63 RID: 15715
		protected const int MIN_LINES = 2;

		// Token: 0x04003D64 RID: 15716
		private Label m_labelTitle;

		// Token: 0x04003D65 RID: 15717
		private Label m_labelDesc;

		// Token: 0x04003D66 RID: 15718
		private string fullDesc;

		// Token: 0x04003D67 RID: 15719
		protected int lineHeight;

		// Token: 0x04003D68 RID: 15720
		private bool needUpdateUIWithFont = true;

		// Token: 0x04003D69 RID: 15721
		internal Rectangle rect = Rectangle.Empty;
	}
}
