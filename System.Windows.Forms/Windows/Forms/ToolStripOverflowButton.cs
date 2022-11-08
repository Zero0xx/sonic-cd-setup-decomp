using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	// Token: 0x020006BA RID: 1722
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.None)]
	public class ToolStripOverflowButton : ToolStripDropDownButton
	{
		// Token: 0x06005A33 RID: 23091 RVA: 0x0014795E File Offset: 0x0014695E
		internal ToolStripOverflowButton(ToolStrip parentToolStrip)
		{
			base.SupportsItemClick = false;
			this.parentToolStrip = parentToolStrip;
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x06005A34 RID: 23092 RVA: 0x00147974 File Offset: 0x00146974
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x06005A35 RID: 23093 RVA: 0x0014797B File Offset: 0x0014697B
		public override bool HasDropDownItems
		{
			get
			{
				return base.ParentInternal.OverflowItems.Count > 0;
			}
		}

		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x06005A36 RID: 23094 RVA: 0x00147990 File Offset: 0x00146990
		internal override bool OppositeDropDownAlign
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x06005A37 RID: 23095 RVA: 0x00147993 File Offset: 0x00146993
		internal ToolStrip ParentToolStrip
		{
			get
			{
				return this.parentToolStrip;
			}
		}

		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06005A38 RID: 23096 RVA: 0x0014799B File Offset: 0x0014699B
		// (set) Token: 0x06005A39 RID: 23097 RVA: 0x001479A3 File Offset: 0x001469A3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x06005A3A RID: 23098 RVA: 0x001479AC File Offset: 0x001469AC
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject(this);
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x001479B4 File Offset: 0x001469B4
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripOverflow(this);
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x001479BC File Offset: 0x001469BC
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size sz = constrainingSize;
			if (base.ParentInternal != null)
			{
				if (base.ParentInternal.Orientation == Orientation.Horizontal)
				{
					sz.Width = Math.Min(constrainingSize.Width, 16);
				}
				else
				{
					sz.Height = Math.Min(constrainingSize.Height, 16);
				}
			}
			return sz + this.Padding.Size;
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x00147A20 File Offset: 0x00146A20
		protected internal override void SetBounds(Rectangle bounds)
		{
			if (base.ParentInternal != null && base.ParentInternal.LayoutEngine is ToolStripSplitStackLayout)
			{
				if (base.ParentInternal.Orientation == Orientation.Horizontal)
				{
					bounds.Height = base.ParentInternal.Height;
					bounds.Y = 0;
				}
				else
				{
					bounds.Width = base.ParentInternal.Width;
					bounds.X = 0;
				}
			}
			base.SetBounds(bounds);
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x00147A94 File Offset: 0x00146A94
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.ParentInternal != null)
			{
				ToolStripRenderer renderer = base.ParentInternal.Renderer;
				renderer.DrawOverflowButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
			}
		}

		// Token: 0x040038B3 RID: 14515
		private ToolStrip parentToolStrip;

		// Token: 0x020006BB RID: 1723
		internal class ToolStripOverflowButtonAccessibleObject : ToolStripDropDownItemAccessibleObject
		{
			// Token: 0x06005A3F RID: 23103 RVA: 0x00147AC7 File Offset: 0x00146AC7
			public ToolStripOverflowButtonAccessibleObject(ToolStripOverflowButton owner) : base(owner)
			{
			}

			// Token: 0x170012C4 RID: 4804
			// (get) Token: 0x06005A40 RID: 23104 RVA: 0x00147AD0 File Offset: 0x00146AD0
			// (set) Token: 0x06005A41 RID: 23105 RVA: 0x00147B11 File Offset: 0x00146B11
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					if (string.IsNullOrEmpty(this.stockName))
					{
						this.stockName = SR.GetString("ToolStripOptions");
					}
					return this.stockName;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x040038B4 RID: 14516
			private string stockName;
		}
	}
}
