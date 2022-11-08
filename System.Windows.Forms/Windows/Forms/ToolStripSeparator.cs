using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	// Token: 0x020006DA RID: 1754
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	public class ToolStripSeparator : ToolStripItem
	{
		// Token: 0x06005C87 RID: 23687 RVA: 0x00150B36 File Offset: 0x0014FB36
		public ToolStripSeparator()
		{
			this.ForeColor = SystemColors.ControlDark;
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06005C88 RID: 23688 RVA: 0x00150B49 File Offset: 0x0014FB49
		// (set) Token: 0x06005C89 RID: 23689 RVA: 0x00150B51 File Offset: 0x0014FB51
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool AutoToolTip
		{
			get
			{
				return base.AutoToolTip;
			}
			set
			{
				base.AutoToolTip = value;
			}
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06005C8A RID: 23690 RVA: 0x00150B5A File Offset: 0x0014FB5A
		// (set) Token: 0x06005C8B RID: 23691 RVA: 0x00150B62 File Offset: 0x0014FB62
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06005C8C RID: 23692 RVA: 0x00150B6B File Offset: 0x0014FB6B
		// (set) Token: 0x06005C8D RID: 23693 RVA: 0x00150B73 File Offset: 0x0014FB73
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06005C8E RID: 23694 RVA: 0x00150B7C File Offset: 0x0014FB7C
		public override bool CanSelect
		{
			get
			{
				return base.DesignMode;
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06005C8F RID: 23695 RVA: 0x00150B84 File Offset: 0x0014FB84
		protected override Size DefaultSize
		{
			get
			{
				return new Size(6, 6);
			}
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06005C90 RID: 23696 RVA: 0x00150B8D File Offset: 0x0014FB8D
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06005C91 RID: 23697 RVA: 0x00150B94 File Offset: 0x0014FB94
		// (set) Token: 0x06005C92 RID: 23698 RVA: 0x00150B9C File Offset: 0x0014FB9C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool DoubleClickEnabled
		{
			get
			{
				return base.DoubleClickEnabled;
			}
			set
			{
				base.DoubleClickEnabled = value;
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06005C93 RID: 23699 RVA: 0x00150BA5 File Offset: 0x0014FBA5
		// (set) Token: 0x06005C94 RID: 23700 RVA: 0x00150BAD File Offset: 0x0014FBAD
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		// Token: 0x1400037E RID: 894
		// (add) Token: 0x06005C95 RID: 23701 RVA: 0x00150BB6 File Offset: 0x0014FBB6
		// (remove) Token: 0x06005C96 RID: 23702 RVA: 0x00150BBF File Offset: 0x0014FBBF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				base.EnabledChanged += value;
			}
			remove
			{
				base.EnabledChanged -= value;
			}
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06005C97 RID: 23703 RVA: 0x00150BC8 File Offset: 0x0014FBC8
		// (set) Token: 0x06005C98 RID: 23704 RVA: 0x00150BD0 File Offset: 0x0014FBD0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ToolStripItemDisplayStyle DisplayStyle
		{
			get
			{
				return base.DisplayStyle;
			}
			set
			{
				base.DisplayStyle = value;
			}
		}

		// Token: 0x1400037F RID: 895
		// (add) Token: 0x06005C99 RID: 23705 RVA: 0x00150BD9 File Offset: 0x0014FBD9
		// (remove) Token: 0x06005C9A RID: 23706 RVA: 0x00150BE2 File Offset: 0x0014FBE2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DisplayStyleChanged
		{
			add
			{
				base.DisplayStyleChanged += value;
			}
			remove
			{
				base.DisplayStyleChanged -= value;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06005C9B RID: 23707 RVA: 0x00150BEB File Offset: 0x0014FBEB
		// (set) Token: 0x06005C9C RID: 23708 RVA: 0x00150BF3 File Offset: 0x0014FBF3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06005C9D RID: 23709 RVA: 0x00150BFC File Offset: 0x0014FBFC
		// (set) Token: 0x06005C9E RID: 23710 RVA: 0x00150C04 File Offset: 0x0014FC04
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
			set
			{
				base.ImageAlign = value;
			}
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06005C9F RID: 23711 RVA: 0x00150C0D File Offset: 0x0014FC0D
		// (set) Token: 0x06005CA0 RID: 23712 RVA: 0x00150C15 File Offset: 0x0014FC15
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06005CA1 RID: 23713 RVA: 0x00150C1E File Offset: 0x0014FC1E
		// (set) Token: 0x06005CA2 RID: 23714 RVA: 0x00150C26 File Offset: 0x0014FC26
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public new int ImageIndex
		{
			get
			{
				return base.ImageIndex;
			}
			set
			{
				base.ImageIndex = value;
			}
		}

		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06005CA3 RID: 23715 RVA: 0x00150C2F File Offset: 0x0014FC2F
		// (set) Token: 0x06005CA4 RID: 23716 RVA: 0x00150C37 File Offset: 0x0014FC37
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ImageKey
		{
			get
			{
				return base.ImageKey;
			}
			set
			{
				base.ImageKey = value;
			}
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06005CA5 RID: 23717 RVA: 0x00150C40 File Offset: 0x0014FC40
		// (set) Token: 0x06005CA6 RID: 23718 RVA: 0x00150C48 File Offset: 0x0014FC48
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ImageTransparentColor
		{
			get
			{
				return base.ImageTransparentColor;
			}
			set
			{
				base.ImageTransparentColor = value;
			}
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06005CA7 RID: 23719 RVA: 0x00150C51 File Offset: 0x0014FC51
		// (set) Token: 0x06005CA8 RID: 23720 RVA: 0x00150C59 File Offset: 0x0014FC59
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ToolStripItemImageScaling ImageScaling
		{
			get
			{
				return base.ImageScaling;
			}
			set
			{
				base.ImageScaling = value;
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06005CA9 RID: 23721 RVA: 0x00150C64 File Offset: 0x0014FC64
		private bool IsVertical
		{
			get
			{
				ToolStrip toolStrip = base.ParentInternal;
				if (toolStrip == null)
				{
					toolStrip = base.Owner;
				}
				ToolStripDropDownMenu toolStripDropDownMenu = toolStrip as ToolStripDropDownMenu;
				if (toolStripDropDownMenu != null)
				{
					return false;
				}
				switch (toolStrip.LayoutStyle)
				{
				case ToolStripLayoutStyle.VerticalStackWithOverflow:
					return false;
				}
				return true;
			}
		}

		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x06005CAA RID: 23722 RVA: 0x00150CB2 File Offset: 0x0014FCB2
		// (set) Token: 0x06005CAB RID: 23723 RVA: 0x00150CBA File Offset: 0x0014FCBA
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x14000380 RID: 896
		// (add) Token: 0x06005CAC RID: 23724 RVA: 0x00150CC3 File Offset: 0x0014FCC3
		// (remove) Token: 0x06005CAD RID: 23725 RVA: 0x00150CCC File Offset: 0x0014FCCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x06005CAE RID: 23726 RVA: 0x00150CD5 File Offset: 0x0014FCD5
		// (set) Token: 0x06005CAF RID: 23727 RVA: 0x00150CDD File Offset: 0x0014FCDD
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06005CB0 RID: 23728 RVA: 0x00150CE6 File Offset: 0x0014FCE6
		// (set) Token: 0x06005CB1 RID: 23729 RVA: 0x00150CEE File Offset: 0x0014FCEE
		[DefaultValue(ToolStripTextDirection.Horizontal)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ToolStripTextDirection TextDirection
		{
			get
			{
				return base.TextDirection;
			}
			set
			{
				base.TextDirection = value;
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x06005CB2 RID: 23730 RVA: 0x00150CF7 File Offset: 0x0014FCF7
		// (set) Token: 0x06005CB3 RID: 23731 RVA: 0x00150CFF File Offset: 0x0014FCFF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new TextImageRelation TextImageRelation
		{
			get
			{
				return base.TextImageRelation;
			}
			set
			{
				base.TextImageRelation = value;
			}
		}

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x06005CB4 RID: 23732 RVA: 0x00150D08 File Offset: 0x0014FD08
		// (set) Token: 0x06005CB5 RID: 23733 RVA: 0x00150D10 File Offset: 0x0014FD10
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ToolTipText
		{
			get
			{
				return base.ToolTipText;
			}
			set
			{
				base.ToolTipText = value;
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06005CB6 RID: 23734 RVA: 0x00150D19 File Offset: 0x0014FD19
		// (set) Token: 0x06005CB7 RID: 23735 RVA: 0x00150D21 File Offset: 0x0014FD21
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool RightToLeftAutoMirrorImage
		{
			get
			{
				return base.RightToLeftAutoMirrorImage;
			}
			set
			{
				base.RightToLeftAutoMirrorImage = value;
			}
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x00150D2A File Offset: 0x0014FD2A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripSeparator.ToolStripSeparatorAccessibleObject(this);
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x00150D34 File Offset: 0x0014FD34
		public override Size GetPreferredSize(Size constrainingSize)
		{
			ToolStrip toolStrip = base.ParentInternal;
			if (toolStrip == null)
			{
				toolStrip = base.Owner;
			}
			if (toolStrip == null)
			{
				return new Size(6, 6);
			}
			ToolStripDropDownMenu toolStripDropDownMenu = toolStrip as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null)
			{
				return new Size(toolStrip.Width - (toolStrip.Padding.Horizontal - toolStripDropDownMenu.ImageMargin.Width), 6);
			}
			if (toolStrip.LayoutStyle != ToolStripLayoutStyle.HorizontalStackWithOverflow || toolStrip.LayoutStyle != ToolStripLayoutStyle.VerticalStackWithOverflow)
			{
				constrainingSize.Width = 23;
				constrainingSize.Height = 23;
			}
			if (this.IsVertical)
			{
				return new Size(6, constrainingSize.Height);
			}
			return new Size(constrainingSize.Width, 6);
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x00150DD9 File Offset: 0x0014FDD9
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null && base.ParentInternal != null)
			{
				base.Renderer.DrawSeparator(new ToolStripSeparatorRenderEventArgs(e.Graphics, this, this.IsVertical));
			}
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x00150E08 File Offset: 0x0014FE08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void OnFontChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripItem.EventFontChanged, e);
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x00150E16 File Offset: 0x0014FE16
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal override bool ShouldSerializeForeColor()
		{
			return this.ForeColor != SystemColors.ControlDark;
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x00150E28 File Offset: 0x0014FE28
		protected internal override void SetBounds(Rectangle rect)
		{
			ToolStripDropDownMenu toolStripDropDownMenu = base.Owner as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null && toolStripDropDownMenu != null)
			{
				rect.X = 2;
				rect.Width = toolStripDropDownMenu.Width - 4;
			}
			base.SetBounds(rect);
		}

		// Token: 0x04003922 RID: 14626
		private const int WINBAR_SEPARATORTHICKNESS = 6;

		// Token: 0x04003923 RID: 14627
		private const int WINBAR_SEPARATORHEIGHT = 23;

		// Token: 0x020006DB RID: 1755
		[ComVisible(true)]
		internal class ToolStripSeparatorAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x06005CBE RID: 23742 RVA: 0x00150E65 File Offset: 0x0014FE65
			public ToolStripSeparatorAccessibleObject(ToolStripSeparator ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x17001374 RID: 4980
			// (get) Token: 0x06005CBF RID: 23743 RVA: 0x00150E78 File Offset: 0x0014FE78
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = this.ownerItem.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Separator;
				}
			}

			// Token: 0x04003924 RID: 14628
			private ToolStripSeparator ownerItem;
		}
	}
}
