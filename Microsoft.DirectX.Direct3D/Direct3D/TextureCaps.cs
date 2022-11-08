using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000046 RID: 70
	[MiscellaneousBits(1)]
	public struct TextureCaps
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00058870 File Offset: 0x00057C70
		internal TextureCaps()
		{
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00058854 File Offset: 0x00057C54
		internal TextureCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00058884 File Offset: 0x00057C84
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$19$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$20$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000589B0 File Offset: 0x00057DB0
		public bool SupportsPerspective
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000589CC File Offset: 0x00057DCC
		public bool SupportsPower2
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000589EC File Offset: 0x00057DEC
		public bool SupportsAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00058A0C File Offset: 0x00057E0C
		public bool SupportsSquareOnly
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00058A2C File Offset: 0x00057E2C
		public bool SupportsTextureRepeatNotScaledBySize
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00058A4C File Offset: 0x00057E4C
		public bool SupportsAlphaPalette
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00058A6C File Offset: 0x00057E6C
		public bool SupportsNonPower2Conditional
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00058A8C File Offset: 0x00057E8C
		public bool SupportsProjected
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 10 & 1U) != 0;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00058AAC File Offset: 0x00057EAC
		public bool SupportsCubeMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 11 & 1U) != 0;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00058ACC File Offset: 0x00057ECC
		public bool SupportsVolumeMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 13 & 1U) != 0;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00058AEC File Offset: 0x00057EEC
		public bool SupportsMipMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 14 & 1U) != 0;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00058B0C File Offset: 0x00057F0C
		public bool SupportsMipVolumeMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 15 & 1U) != 0;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00058B2C File Offset: 0x00057F2C
		public unsafe bool SupportsMipCubeMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(ref this.caps + 2) & 1) != 0;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00058B4C File Offset: 0x00057F4C
		public bool SupportsCubeMapPower2
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 17 & 1U) != 0;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00058B6C File Offset: 0x00057F6C
		public bool SupportsVolumeMapPower2
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 18 & 1U) != 0;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00058B8C File Offset: 0x00057F8C
		public bool SupportsNoProjectedBumpEnvironment
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 21 & 1U) != 0;
			}
		}

		// Token: 0x04000D5E RID: 3422
		private int caps;
	}
}
