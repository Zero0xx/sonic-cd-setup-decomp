using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007B6 RID: 1974
	internal class HotCommands : PropertyGrid.SnappableControl
	{
		// Token: 0x06006875 RID: 26741 RVA: 0x0017F13D File Offset: 0x0017E13D
		internal HotCommands(PropertyGrid owner) : base(owner)
		{
			this.Text = "Command Pane";
		}

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06006876 RID: 26742 RVA: 0x0017F15F File Offset: 0x0017E15F
		// (set) Token: 0x06006877 RID: 26743 RVA: 0x0017F167 File Offset: 0x0017E167
		public virtual bool AllowVisible
		{
			get
			{
				return this.allowVisible;
			}
			set
			{
				if (this.allowVisible != value)
				{
					this.allowVisible = value;
					if (value && this.WouldBeVisible)
					{
						base.Visible = true;
						return;
					}
					base.Visible = false;
				}
			}
		}

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06006878 RID: 26744 RVA: 0x0017F194 File Offset: 0x0017E194
		public override Rectangle DisplayRectangle
		{
			get
			{
				Size clientSize = base.ClientSize;
				return new Rectangle(4, 4, clientSize.Width - 8, clientSize.Height - 8);
			}
		}

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x06006879 RID: 26745 RVA: 0x0017F1C4 File Offset: 0x0017E1C4
		public LinkLabel Label
		{
			get
			{
				if (this.label == null)
				{
					this.label = new LinkLabel();
					this.label.Dock = DockStyle.Fill;
					this.label.LinkBehavior = LinkBehavior.AlwaysUnderline;
					this.label.DisabledLinkColor = SystemColors.ControlDark;
					this.label.LinkClicked += this.LinkClicked;
					base.Controls.Add(this.label);
				}
				return this.label;
			}
		}

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x0600687A RID: 26746 RVA: 0x0017F23A File Offset: 0x0017E23A
		public virtual bool WouldBeVisible
		{
			get
			{
				return this.component != null;
			}
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x0017F248 File Offset: 0x0017E248
		public override int GetOptimalHeight(int width)
		{
			if (this.optimalHeight == -1)
			{
				int num = (int)(1.5 * (double)this.Font.Height);
				int num2 = 0;
				if (this.verbs != null)
				{
					num2 = this.verbs.Length;
				}
				this.optimalHeight = num2 * num + 8;
			}
			return this.optimalHeight;
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x0017F29A File Offset: 0x0017E29A
		public override int SnapHeightRequest(int request)
		{
			return request;
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x0017F2A0 File Offset: 0x0017E2A0
		private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				if (e.Link.Enabled)
				{
					((DesignerVerb)e.Link.LinkData).Invoke();
				}
			}
			catch (Exception ex)
			{
				RTLAwareMessageBox.Show(this, ex.Message, SR.GetString("PBRSErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
			}
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x0017F304 File Offset: 0x0017E304
		private void OnCommandChanged(object sender, EventArgs e)
		{
			this.SetupLabel();
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x0017F30C File Offset: 0x0017E30C
		protected override void OnGotFocus(EventArgs e)
		{
			this.Label.FocusInternal();
			this.Label.Invalidate();
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x0017F325 File Offset: 0x0017E325
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.optimalHeight = -1;
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x0017F338 File Offset: 0x0017E338
		internal void SetColors(Color background, Color normalText, Color link, Color activeLink, Color visitedLink, Color disabledLink)
		{
			this.Label.BackColor = background;
			this.Label.ForeColor = normalText;
			this.Label.LinkColor = link;
			this.Label.ActiveLinkColor = activeLink;
			this.Label.VisitedLinkColor = visitedLink;
			this.Label.DisabledLinkColor = disabledLink;
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x0017F390 File Offset: 0x0017E390
		public void Select(bool forward)
		{
			this.Label.FocusInternal();
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x0017F3A0 File Offset: 0x0017E3A0
		public virtual void SetVerbs(object component, DesignerVerb[] verbs)
		{
			if (this.verbs != null)
			{
				for (int i = 0; i < this.verbs.Length; i++)
				{
					this.verbs[i].CommandChanged -= this.OnCommandChanged;
				}
				this.component = null;
				this.verbs = null;
			}
			if (component == null || verbs == null || verbs.Length == 0)
			{
				base.Visible = false;
			}
			else
			{
				this.component = component;
				this.verbs = verbs;
				for (int j = 0; j < verbs.Length; j++)
				{
					verbs[j].CommandChanged += this.OnCommandChanged;
				}
				if (this.allowVisible)
				{
					base.Visible = true;
				}
				this.SetupLabel();
			}
			this.optimalHeight = -1;
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x0017F450 File Offset: 0x0017E450
		private void SetupLabel()
		{
			this.Label.Links.Clear();
			StringBuilder stringBuilder = new StringBuilder();
			Point[] array = new Point[this.verbs.Length];
			int num = 0;
			bool flag = true;
			for (int i = 0; i < this.verbs.Length; i++)
			{
				if (this.verbs[i].Visible && this.verbs[i].Supported)
				{
					if (!flag)
					{
						stringBuilder.Append(Application.CurrentCulture.TextInfo.ListSeparator);
						stringBuilder.Append(" ");
						num += 2;
					}
					string text = this.verbs[i].Text;
					array[i] = new Point(num, text.Length);
					stringBuilder.Append(text);
					num += text.Length;
					flag = false;
				}
			}
			this.Label.Text = stringBuilder.ToString();
			for (int j = 0; j < this.verbs.Length; j++)
			{
				if (this.verbs[j].Visible && this.verbs[j].Supported)
				{
					LinkLabel.Link link = this.Label.Links.Add(array[j].X, array[j].Y, this.verbs[j]);
					if (!this.verbs[j].Enabled)
					{
						link.Enabled = false;
					}
				}
			}
		}

		// Token: 0x04003D82 RID: 15746
		private object component;

		// Token: 0x04003D83 RID: 15747
		private DesignerVerb[] verbs;

		// Token: 0x04003D84 RID: 15748
		private LinkLabel label;

		// Token: 0x04003D85 RID: 15749
		private bool allowVisible = true;

		// Token: 0x04003D86 RID: 15750
		private int optimalHeight = -1;
	}
}
