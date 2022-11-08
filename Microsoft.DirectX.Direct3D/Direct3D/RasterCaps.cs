using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200003E RID: 62
	[MiscellaneousBits(1)]
	public struct RasterCaps
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00057D4C File Offset: 0x0005714C
		internal RasterCaps()
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00057D30 File Offset: 0x00057130
		internal RasterCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00057D60 File Offset: 0x00057160
		public unsafe override string ToString()
		{
			object obj = this;
			Type type = obj.GetType();
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = type.GetProperties();
			int num = 0;
			if (0 < properties.Length)
			{
				do
				{
					MethodInfo getMethod = properties[num].GetGetMethod();
					if (getMethod != null && !getMethod.IsStatic)
					{
						object obj2 = getMethod.Invoke(obj, null);
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$11$];
						array[0] = properties[num].Name;
						string text;
						if (obj2 != null)
						{
							text = obj2.ToString();
						}
						else
						{
							text = string.Empty;
						}
						array[1] = text;
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array));
					}
					num++;
				}
				while (num < properties.Length);
			}
			FieldInfo[] fields = type.GetFields();
			int num2 = 0;
			if (0 < fields.Length)
			{
				do
				{
					object value = fields[num2].GetValue(obj);
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$12$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00057E8C File Offset: 0x0005728C
		public bool SupportsDither
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00057EA8 File Offset: 0x000572A8
		public bool SupportsZBufferTest
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00057EC8 File Offset: 0x000572C8
		public bool SupportsFogVertex
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00057EE8 File Offset: 0x000572E8
		public bool SupportsFogTable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00057F08 File Offset: 0x00057308
		public bool SupportsMipMapLevelOfDetailBias
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 13 & 1U) != 0;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00057F28 File Offset: 0x00057328
		public bool SupportsZBufferLessHsr
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 15 & 1U) != 0;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00057F48 File Offset: 0x00057348
		public unsafe bool SupportsFogRange
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(ref this.caps + 2) & 1) != 0;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00057F68 File Offset: 0x00057368
		public bool SupportsAnisotropy
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 17 & 1U) != 0;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00057F88 File Offset: 0x00057388
		public bool SupportsWBuffer
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 18 & 1U) != 0;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00057FA8 File Offset: 0x000573A8
		public bool SupportsWFog
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 20 & 1U) != 0;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00057FC8 File Offset: 0x000573C8
		public bool SupportsZFog
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 21 & 1U) != 0;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00057FE8 File Offset: 0x000573E8
		public bool SupportsColorPerspective
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 22 & 1U) != 0;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00058008 File Offset: 0x00057408
		public unsafe bool SupportsScissorTest
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (*(ref this.caps + 3) & 1) != 0;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00058028 File Offset: 0x00057428
		public bool SupportsSlopeScaleDepthBias
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 25 & 1U) != 0;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00058048 File Offset: 0x00057448
		public bool SupportsDepthBias
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 26 & 1U) != 0;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00058068 File Offset: 0x00057468
		public bool SupportsMultisampleToggle
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 27 & 1U) != 0;
			}
		}

		// Token: 0x04000D56 RID: 3414
		private int caps;
	}
}
