using System;

namespace System.Windows.Forms
{
	// Token: 0x02000723 RID: 1827
	public class VScrollProperties : ScrollProperties
	{
		// Token: 0x060060D0 RID: 24784 RVA: 0x0016278C File Offset: 0x0016178C
		public VScrollProperties(ScrollableControl container) : base(container)
		{
		}

		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x060060D1 RID: 24785 RVA: 0x00162798 File Offset: 0x00161798
		internal override int PageSize
		{
			get
			{
				return base.ParentControl.ClientRectangle.Height;
			}
		}

		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x060060D2 RID: 24786 RVA: 0x001627B8 File Offset: 0x001617B8
		internal override int Orientation
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x060060D3 RID: 24787 RVA: 0x001627BC File Offset: 0x001617BC
		internal override int HorizontalDisplayPosition
		{
			get
			{
				return base.ParentControl.DisplayRectangle.X;
			}
		}

		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x060060D4 RID: 24788 RVA: 0x001627DC File Offset: 0x001617DC
		internal override int VerticalDisplayPosition
		{
			get
			{
				return -this.value;
			}
		}
	}
}
