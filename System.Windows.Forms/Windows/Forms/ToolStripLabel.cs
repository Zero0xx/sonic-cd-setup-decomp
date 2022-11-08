using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	// Token: 0x020006AB RID: 1707
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class ToolStripLabel : ToolStripItem
	{
		// Token: 0x0600596B RID: 22891 RVA: 0x00144945 File Offset: 0x00143945
		public ToolStripLabel()
		{
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x0014496E File Offset: 0x0014396E
		public ToolStripLabel(string text) : base(text, null, null)
		{
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x0014499A File Offset: 0x0014399A
		public ToolStripLabel(Image image) : base(null, image, null)
		{
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x001449C6 File Offset: 0x001439C6
		public ToolStripLabel(string text, Image image) : base(text, image, null)
		{
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x001449F2 File Offset: 0x001439F2
		public ToolStripLabel(string text, Image image, bool isLink) : this(text, image, isLink, null)
		{
		}

		// Token: 0x06005970 RID: 22896 RVA: 0x001449FE File Offset: 0x001439FE
		public ToolStripLabel(string text, Image image, bool isLink, EventHandler onClick) : this(text, image, isLink, onClick, null)
		{
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x00144A0C File Offset: 0x00143A0C
		public ToolStripLabel(string text, Image image, bool isLink, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.IsLink = isLink;
		}

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06005972 RID: 22898 RVA: 0x00144A42 File Offset: 0x00143A42
		public override bool CanSelect
		{
			get
			{
				return this.IsLink || base.DesignMode;
			}
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06005973 RID: 22899 RVA: 0x00144A54 File Offset: 0x00143A54
		// (set) Token: 0x06005974 RID: 22900 RVA: 0x00144A5C File Offset: 0x00143A5C
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ToolStripLabelIsLinkDescr")]
		public bool IsLink
		{
			get
			{
				return this.isLink;
			}
			set
			{
				if (this.isLink != value)
				{
					this.isLink = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06005975 RID: 22901 RVA: 0x00144A74 File Offset: 0x00143A74
		// (set) Token: 0x06005976 RID: 22902 RVA: 0x00144A90 File Offset: 0x00143A90
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLabelActiveLinkColorDescr")]
		public Color ActiveLinkColor
		{
			get
			{
				if (this.activeLinkColor.IsEmpty)
				{
					return this.IEActiveLinkColor;
				}
				return this.activeLinkColor;
			}
			set
			{
				if (this.activeLinkColor != value)
				{
					this.activeLinkColor = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06005977 RID: 22903 RVA: 0x00144AAD File Offset: 0x00143AAD
		private Color IELinkColor
		{
			get
			{
				return LinkUtilities.IELinkColor;
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06005978 RID: 22904 RVA: 0x00144AB4 File Offset: 0x00143AB4
		private Color IEActiveLinkColor
		{
			get
			{
				return LinkUtilities.IEActiveLinkColor;
			}
		}

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06005979 RID: 22905 RVA: 0x00144ABB File Offset: 0x00143ABB
		private Color IEVisitedLinkColor
		{
			get
			{
				return LinkUtilities.IEVisitedLinkColor;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x0600597A RID: 22906 RVA: 0x00144AC2 File Offset: 0x00143AC2
		// (set) Token: 0x0600597B RID: 22907 RVA: 0x00144ACC File Offset: 0x00143ACC
		[SRDescription("ToolStripLabelLinkBehaviorDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(LinkBehavior.SystemDefault)]
		public LinkBehavior LinkBehavior
		{
			get
			{
				return this.linkBehavior;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("LinkBehavior", (int)value, typeof(LinkBehavior));
				}
				if (this.linkBehavior != value)
				{
					this.linkBehavior = value;
					this.InvalidateLinkFonts();
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x0600597C RID: 22908 RVA: 0x00144B1B File Offset: 0x00143B1B
		// (set) Token: 0x0600597D RID: 22909 RVA: 0x00144B37 File Offset: 0x00143B37
		[SRDescription("ToolStripLabelLinkColorDescr")]
		[SRCategory("CatAppearance")]
		public Color LinkColor
		{
			get
			{
				if (this.linkColor.IsEmpty)
				{
					return this.IELinkColor;
				}
				return this.linkColor;
			}
			set
			{
				if (this.linkColor != value)
				{
					this.linkColor = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x0600597E RID: 22910 RVA: 0x00144B54 File Offset: 0x00143B54
		// (set) Token: 0x0600597F RID: 22911 RVA: 0x00144B5C File Offset: 0x00143B5C
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLabelLinkVisitedDescr")]
		public bool LinkVisited
		{
			get
			{
				return this.linkVisited;
			}
			set
			{
				if (this.linkVisited != value)
				{
					this.linkVisited = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06005980 RID: 22912 RVA: 0x00144B74 File Offset: 0x00143B74
		// (set) Token: 0x06005981 RID: 22913 RVA: 0x00144B90 File Offset: 0x00143B90
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLabelVisitedLinkColorDescr")]
		public Color VisitedLinkColor
		{
			get
			{
				if (this.visitedLinkColor.IsEmpty)
				{
					return this.IEVisitedLinkColor;
				}
				return this.visitedLinkColor;
			}
			set
			{
				if (this.visitedLinkColor != value)
				{
					this.visitedLinkColor = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x00144BB0 File Offset: 0x00143BB0
		private void InvalidateLinkFonts()
		{
			if (this.linkFont != null)
			{
				this.linkFont.Dispose();
			}
			if (this.hoverLinkFont != null && this.hoverLinkFont != this.linkFont)
			{
				this.hoverLinkFont.Dispose();
			}
			this.linkFont = null;
			this.hoverLinkFont = null;
		}

		// Token: 0x06005983 RID: 22915 RVA: 0x00144BFF File Offset: 0x00143BFF
		protected override void OnFontChanged(EventArgs e)
		{
			this.InvalidateLinkFonts();
			base.OnFontChanged(e);
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x00144C10 File Offset: 0x00143C10
		protected override void OnMouseEnter(EventArgs e)
		{
			if (this.IsLink)
			{
				ToolStrip parent = base.Parent;
				if (parent != null)
				{
					this.lastCursor = parent.Cursor;
					parent.Cursor = Cursors.Hand;
				}
			}
			base.OnMouseEnter(e);
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x00144C50 File Offset: 0x00143C50
		protected override void OnMouseLeave(EventArgs e)
		{
			if (this.IsLink)
			{
				ToolStrip parent = base.Parent;
				if (parent != null)
				{
					parent.Cursor = this.lastCursor;
				}
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x00144C82 File Offset: 0x00143C82
		private void ResetActiveLinkColor()
		{
			this.ActiveLinkColor = this.IEActiveLinkColor;
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x00144C90 File Offset: 0x00143C90
		private void ResetLinkColor()
		{
			this.LinkColor = this.IELinkColor;
		}

		// Token: 0x06005988 RID: 22920 RVA: 0x00144C9E File Offset: 0x00143C9E
		private void ResetVisitedLinkColor()
		{
			this.VisitedLinkColor = this.IEVisitedLinkColor;
		}

		// Token: 0x06005989 RID: 22921 RVA: 0x00144CAC File Offset: 0x00143CAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeActiveLinkColor()
		{
			return !this.activeLinkColor.IsEmpty;
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x00144CBC File Offset: 0x00143CBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeLinkColor()
		{
			return !this.linkColor.IsEmpty;
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x00144CCC File Offset: 0x00143CCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeVisitedLinkColor()
		{
			return !this.visitedLinkColor.IsEmpty;
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x00144CDC File Offset: 0x00143CDC
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripLabel.ToolStripLabelLayout(this);
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x00144CE4 File Offset: 0x00143CE4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripLabel.ToolStripLabelAccessibleObject(this);
		}

		// Token: 0x0600598E RID: 22926 RVA: 0x00144CEC File Offset: 0x00143CEC
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				renderer.DrawLabelBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(e.Graphics, this, base.InternalLayout.ImageRectangle));
				}
				this.PaintText(e.Graphics);
			}
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x00144D50 File Offset: 0x00143D50
		internal void PaintText(Graphics g)
		{
			ToolStripRenderer renderer = base.Renderer;
			if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
			{
				Font font = this.Font;
				Color textColor = this.ForeColor;
				if (this.IsLink)
				{
					LinkUtilities.EnsureLinkFonts(font, this.LinkBehavior, ref this.linkFont, ref this.hoverLinkFont);
					if (this.Pressed)
					{
						font = this.hoverLinkFont;
						textColor = this.ActiveLinkColor;
					}
					else if (this.Selected)
					{
						font = this.hoverLinkFont;
						textColor = (this.LinkVisited ? this.VisitedLinkColor : this.LinkColor);
					}
					else
					{
						font = this.linkFont;
						textColor = (this.LinkVisited ? this.VisitedLinkColor : this.LinkColor);
					}
				}
				Rectangle textRectangle = base.InternalLayout.TextRectangle;
				renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(g, this, this.Text, textRectangle, textColor, font, base.InternalLayout.TextFormat));
			}
		}

		// Token: 0x06005990 RID: 22928 RVA: 0x00144E2B File Offset: 0x00143E2B
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.ParentInternal != null)
			{
				if (!this.CanSelect)
				{
					base.ParentInternal.SetFocusUnsafe();
					base.ParentInternal.SelectNextToolStripItem(this, true);
				}
				else
				{
					base.FireEvent(ToolStripItemEventType.Click);
				}
				return true;
			}
			return false;
		}

		// Token: 0x04003870 RID: 14448
		private LinkBehavior linkBehavior;

		// Token: 0x04003871 RID: 14449
		private bool isLink;

		// Token: 0x04003872 RID: 14450
		private bool linkVisited;

		// Token: 0x04003873 RID: 14451
		private Color linkColor = Color.Empty;

		// Token: 0x04003874 RID: 14452
		private Color activeLinkColor = Color.Empty;

		// Token: 0x04003875 RID: 14453
		private Color visitedLinkColor = Color.Empty;

		// Token: 0x04003876 RID: 14454
		private Font hoverLinkFont;

		// Token: 0x04003877 RID: 14455
		private Font linkFont;

		// Token: 0x04003878 RID: 14456
		private Cursor lastCursor;

		// Token: 0x020006AC RID: 1708
		[ComVisible(true)]
		internal class ToolStripLabelAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x06005991 RID: 22929 RVA: 0x00144E62 File Offset: 0x00143E62
			public ToolStripLabelAccessibleObject(ToolStripLabel ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x17001290 RID: 4752
			// (get) Token: 0x06005992 RID: 22930 RVA: 0x00144E72 File Offset: 0x00143E72
			public override string DefaultAction
			{
				get
				{
					if (this.ownerItem.IsLink)
					{
						return SR.GetString("AccessibleActionClick");
					}
					return string.Empty;
				}
			}

			// Token: 0x06005993 RID: 22931 RVA: 0x00144E91 File Offset: 0x00143E91
			public override void DoDefaultAction()
			{
				if (this.ownerItem.IsLink)
				{
					base.DoDefaultAction();
				}
			}

			// Token: 0x17001291 RID: 4753
			// (get) Token: 0x06005994 RID: 22932 RVA: 0x00144EA8 File Offset: 0x00143EA8
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					if (!this.ownerItem.IsLink)
					{
						return AccessibleRole.StaticText;
					}
					return AccessibleRole.Link;
				}
			}

			// Token: 0x17001292 RID: 4754
			// (get) Token: 0x06005995 RID: 22933 RVA: 0x00144ED9 File Offset: 0x00143ED9
			public override AccessibleStates State
			{
				get
				{
					return base.State | AccessibleStates.ReadOnly;
				}
			}

			// Token: 0x04003879 RID: 14457
			private ToolStripLabel ownerItem;
		}

		// Token: 0x020006AD RID: 1709
		private class ToolStripLabelLayout : ToolStripItemInternalLayout
		{
			// Token: 0x06005996 RID: 22934 RVA: 0x00144EE4 File Offset: 0x00143EE4
			public ToolStripLabelLayout(ToolStripLabel owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005997 RID: 22935 RVA: 0x00144EF4 File Offset: 0x00143EF4
			protected override ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
			{
				ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = base.CommonLayoutOptions();
				toolStripItemLayoutOptions.borderSize = 0;
				return toolStripItemLayoutOptions;
			}

			// Token: 0x0400387A RID: 14458
			private ToolStripLabel owner;
		}
	}
}
