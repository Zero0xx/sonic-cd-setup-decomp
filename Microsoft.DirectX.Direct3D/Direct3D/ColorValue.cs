using System;
using System.Drawing;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000B7 RID: 183
	[MiscellaneousBits(1)]
	public struct ColorValue
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0005E00C File Offset: 0x0005D40C
		public ColorValue(int r, int g, int b)
		{
			this.mr = (float)r * 0.003921569f;
			this.mg = (float)g * 0.003921569f;
			this.mb = (float)b * 0.003921569f;
			this.ma = 1f;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0005DFC0 File Offset: 0x0005D3C0
		public ColorValue(int r, int g, int b, int a)
		{
			this.mr = (float)r * 0.003921569f;
			this.mg = (float)g * 0.003921569f;
			this.mb = (float)b * 0.003921569f;
			this.ma = (float)a * 0.003921569f;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0005DF8C File Offset: 0x0005D38C
		public ColorValue(float r, float g, float b)
		{
			this.mr = r;
			this.mg = g;
			this.mb = b;
			this.ma = 1f;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0005DF5C File Offset: 0x0005D35C
		public ColorValue(float r, float g, float b, float a)
		{
			this.mr = r;
			this.mg = g;
			this.mb = b;
			this.ma = a;
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0005E054 File Offset: 0x0005D454
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0005E06C File Offset: 0x0005D46C
		public float Red
		{
			get
			{
				return this.mr;
			}
			set
			{
				this.mr = value;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0005E088 File Offset: 0x0005D488
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0005E0A0 File Offset: 0x0005D4A0
		public float Blue
		{
			get
			{
				return this.mb;
			}
			set
			{
				this.mb = value;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0005E0BC File Offset: 0x0005D4BC
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0005E0D4 File Offset: 0x0005D4D4
		public float Green
		{
			get
			{
				return this.mg;
			}
			set
			{
				this.mg = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0005E0F0 File Offset: 0x0005D4F0
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0005E108 File Offset: 0x0005D508
		public float Alpha
		{
			get
			{
				return this.ma;
			}
			set
			{
				this.ma = value;
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0005E124 File Offset: 0x0005D524
		public int ToArgb()
		{
			return (int)(((((uint)((double)(this.mr * 255f)) & 255U) | (uint)((double)(this.ma * 255f)) << 8) << 8 | ((uint)((double)(this.mg * 255f)) & 255U)) << 8 | ((uint)((double)(this.mb * 255f)) & 255U));
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0005E18C File Offset: 0x0005D58C
		public static ColorValue FromArgb(int color)
		{
			return new ColorValue
			{
				mr = (float)((byte)(color >> 16)) * 0.003921569f,
				mg = (float)((byte)(color >> 8)) * 0.003921569f,
				mb = (float)((byte)color) * 0.003921569f,
				ma = (float)((byte)(color >> 24)) * 0.003921569f
			};
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0005E1F0 File Offset: 0x0005D5F0
		public static ColorValue FromColor(Color color)
		{
			return ColorValue.FromArgb(color.ToArgb());
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0005E210 File Offset: 0x0005D610
		[CLSCompliant(false)]
		public unsafe static ColorValue FromD3DColor(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE* color)
		{
			return new ColorValue
			{
				mr = *(float*)color,
				mg = *(float*)(color + 4 / sizeof(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE)),
				mb = *(float*)(color + 8 / sizeof(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE)),
				ma = *(float*)(color + 12 / sizeof(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE))
			};
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0005E258 File Offset: 0x0005D658
		[CLSCompliant(false)]
		public unsafe void FillD3DColor(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE* pColor)
		{
			*(float*)pColor = this.mr;
			*(float*)(pColor + 4 / sizeof(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE)) = this.mg;
			*(float*)(pColor + 8 / sizeof(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE)) = this.mb;
			*(float*)(pColor + 12 / sizeof(Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE)) = this.ma;
		}

		// Token: 0x04000EFE RID: 3838
		internal float mr;

		// Token: 0x04000EFF RID: 3839
		internal float mg;

		// Token: 0x04000F00 RID: 3840
		internal float mb;

		// Token: 0x04000F01 RID: 3841
		internal float ma;
	}
}
