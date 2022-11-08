using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006E6 RID: 1766
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripStatusLabel : ToolStripLabel
	{
		// Token: 0x06005D47 RID: 23879 RVA: 0x00153234 File Offset: 0x00152234
		public ToolStripStatusLabel()
		{
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x00153247 File Offset: 0x00152247
		public ToolStripStatusLabel(string text) : base(text, null, false, null)
		{
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x0015325E File Offset: 0x0015225E
		public ToolStripStatusLabel(Image image) : base(null, image, false, null)
		{
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x00153275 File Offset: 0x00152275
		public ToolStripStatusLabel(string text, Image image) : base(text, image, false, null)
		{
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x0015328C File Offset: 0x0015228C
		public ToolStripStatusLabel(string text, Image image, EventHandler onClick) : base(text, image, false, onClick, null)
		{
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x001532A4 File Offset: 0x001522A4
		public ToolStripStatusLabel(string text, Image image, EventHandler onClick, string name) : base(text, image, false, onClick, name)
		{
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x001532BD File Offset: 0x001522BD
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripStatusLabel.ToolStripStatusLabelLayout(this);
		}

		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06005D4E RID: 23886 RVA: 0x001532C5 File Offset: 0x001522C5
		// (set) Token: 0x06005D4F RID: 23887 RVA: 0x001532CD File Offset: 0x001522CD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new ToolStripItemAlignment Alignment
		{
			get
			{
				return base.Alignment;
			}
			set
			{
				base.Alignment = value;
			}
		}

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06005D50 RID: 23888 RVA: 0x001532D6 File Offset: 0x001522D6
		// (set) Token: 0x06005D51 RID: 23889 RVA: 0x001532E0 File Offset: 0x001522E0
		[DefaultValue(Border3DStyle.Flat)]
		[SRDescription("ToolStripStatusLabelBorderStyleDescr")]
		[SRCategory("CatAppearance")]
		public Border3DStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					8192,
					9,
					6,
					16394,
					5,
					4,
					1,
					10,
					8,
					2
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Border3DStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06005D52 RID: 23890 RVA: 0x00153363 File Offset: 0x00152363
		// (set) Token: 0x06005D53 RID: 23891 RVA: 0x0015336B File Offset: 0x0015236B
		[DefaultValue(ToolStripStatusLabelBorderSides.None)]
		[SRDescription("ToolStripStatusLabelBorderSidesDescr")]
		[SRCategory("CatAppearance")]
		public ToolStripStatusLabelBorderSides BorderSides
		{
			get
			{
				return this.borderSides;
			}
			set
			{
				if (this.borderSides != value)
				{
					this.borderSides = value;
					LayoutTransaction.DoLayout(base.Owner, this, PropertyNames.BorderStyle);
					base.Invalidate();
				}
			}
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06005D54 RID: 23892 RVA: 0x00153394 File Offset: 0x00152394
		protected internal override Padding DefaultMargin
		{
			get
			{
				return new Padding(0, 3, 0, 2);
			}
		}

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06005D55 RID: 23893 RVA: 0x0015339F File Offset: 0x0015239F
		// (set) Token: 0x06005D56 RID: 23894 RVA: 0x001533A7 File Offset: 0x001523A7
		[DefaultValue(false)]
		[SRDescription("ToolStripStatusLabelSpringDescr")]
		[SRCategory("CatAppearance")]
		public bool Spring
		{
			get
			{
				return this.spring;
			}
			set
			{
				if (this.spring != value)
				{
					this.spring = value;
					if (base.ParentInternal != null)
					{
						LayoutTransaction.DoLayout(base.ParentInternal, this, PropertyNames.Spring);
					}
				}
			}
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x001533D2 File Offset: 0x001523D2
		public override Size GetPreferredSize(Size constrainingSize)
		{
			if (this.BorderSides != ToolStripStatusLabelBorderSides.None)
			{
				return base.GetPreferredSize(constrainingSize) + new Size(4, 4);
			}
			return base.GetPreferredSize(constrainingSize);
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x001533F8 File Offset: 0x001523F8
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				renderer.DrawToolStripStatusLabelBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(e.Graphics, this, base.InternalLayout.ImageRectangle));
				}
				base.PaintText(e.Graphics);
			}
		}

		// Token: 0x04003948 RID: 14664
		private Border3DStyle borderStyle = Border3DStyle.Flat;

		// Token: 0x04003949 RID: 14665
		private ToolStripStatusLabelBorderSides borderSides;

		// Token: 0x0400394A RID: 14666
		private bool spring;

		// Token: 0x020006E7 RID: 1767
		private class ToolStripStatusLabelLayout : ToolStripItemInternalLayout
		{
			// Token: 0x06005D59 RID: 23897 RVA: 0x0015345A File Offset: 0x0015245A
			public ToolStripStatusLabelLayout(ToolStripStatusLabel owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005D5A RID: 23898 RVA: 0x0015346C File Offset: 0x0015246C
			protected override ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
			{
				ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = base.CommonLayoutOptions();
				toolStripItemLayoutOptions.borderSize = 0;
				return toolStripItemLayoutOptions;
			}

			// Token: 0x0400394B RID: 14667
			private ToolStripStatusLabel owner;
		}
	}
}
