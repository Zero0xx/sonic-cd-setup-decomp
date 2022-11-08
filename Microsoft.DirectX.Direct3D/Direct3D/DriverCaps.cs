using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000033 RID: 51
	[MiscellaneousBits(1)]
	public struct DriverCaps
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00056E28 File Offset: 0x00056228
		internal DriverCaps()
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00056E00 File Offset: 0x00056200
		internal DriverCaps(int c, int c2, int c3)
		{
			this.caps = c;
			this.caps2 = c2;
			this.caps3 = c3;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00056E3C File Offset: 0x0005623C
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$1$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$2$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00056F68 File Offset: 0x00056368
		public bool ReadScanLine
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 17 & 1U) != 0;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00056F88 File Offset: 0x00056388
		public bool SupportsFullscreenGamma
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 17 & 1U) != 0;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00056FA8 File Offset: 0x000563A8
		public bool CanCalibrateGamma
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 20 & 1U) != 0;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00056FC8 File Offset: 0x000563C8
		public bool CanManageResource
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 28 & 1U) != 0;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00056FE8 File Offset: 0x000563E8
		public bool SupportsDynamicTextures
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 29 & 1U) != 0;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00057008 File Offset: 0x00056408
		public bool CanAutoGenerateMipMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 30 & 1U) != 0;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00057028 File Offset: 0x00056428
		public bool SupportsAlphaFullscreenFlipOrDiscard
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps3 >> 5 & 1U) != 0;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00057048 File Offset: 0x00056448
		public bool SupportsLinearToSrgbPresentation
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps3 >> 7 & 1U) != 0;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00057068 File Offset: 0x00056468
		public bool SupportsCopyToVideoMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps3 >> 8 & 1U) != 0;
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00057088 File Offset: 0x00056488
		public bool SupportsCopyToSystemMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps3 >> 9 & 1U) != 0;
			}
		}

		// Token: 0x04000D49 RID: 3401
		private int caps;

		// Token: 0x04000D4A RID: 3402
		private int caps2;

		// Token: 0x04000D4B RID: 3403
		private int caps3;
	}
}
