using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200004F RID: 79
	[MiscellaneousBits(1)]
	public struct TextureOperationCaps
	{
		// Token: 0x06000143 RID: 323 RVA: 0x000593B4 File Offset: 0x000587B4
		internal TextureOperationCaps()
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00059398 File Offset: 0x00058798
		internal TextureOperationCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000593C8 File Offset: 0x000587C8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$27$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$28$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000594F4 File Offset: 0x000588F4
		public bool SupportsDisable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00059510 File Offset: 0x00058910
		public bool SupportsSelectArgument1
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00059530 File Offset: 0x00058930
		public bool SupportsSelectArgument2
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00059550 File Offset: 0x00058950
		public bool SupportsModulate
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00059570 File Offset: 0x00058970
		public bool SupportsModulate2X
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00059590 File Offset: 0x00058990
		public bool SupportsModulate4X
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000595B0 File Offset: 0x000589B0
		public bool SupportsAdd
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000595D0 File Offset: 0x000589D0
		public bool SupportsAddSigned
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000595F0 File Offset: 0x000589F0
		public bool SupportsAddSigned2X
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00059610 File Offset: 0x00058A10
		public bool SupportsSubtract
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00059630 File Offset: 0x00058A30
		public bool SupportsAddSmooth
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 10 & 1U) != 0;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00059650 File Offset: 0x00058A50
		public bool SupportsBlendDiffuseAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 11 & 1U) != 0;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00059670 File Offset: 0x00058A70
		public bool SupportsBlendTextureAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 12 & 1U) != 0;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00059690 File Offset: 0x00058A90
		public bool SupportsBlendFactorAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 13 & 1U) != 0;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000596B0 File Offset: 0x00058AB0
		public bool SupportsBlendTextureAlphaPM
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 14 & 1U) != 0;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000596D0 File Offset: 0x00058AD0
		public bool SupportsBlendCurrentAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 15 & 1U) != 0;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000596F0 File Offset: 0x00058AF0
		public unsafe bool SupportsPreModulate
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(ref this.caps + 2) & 1) != 0;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00059710 File Offset: 0x00058B10
		public bool SupportsModulateAlphaAddColor
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 17 & 1U) != 0;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00059730 File Offset: 0x00058B30
		public bool SupportsModulateColorAddAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 18 & 1U) != 0;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00059750 File Offset: 0x00058B50
		public bool SupportsModulateInvAlphaAddColor
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 19 & 1U) != 0;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00059770 File Offset: 0x00058B70
		public bool SupportsModulateInvColorAddAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 20 & 1U) != 0;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00059790 File Offset: 0x00058B90
		public bool SupportsBumpEnvironmentMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 21 & 1U) != 0;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000597B0 File Offset: 0x00058BB0
		public bool SupportsBumpEnvironmentMapLuminance
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 22 & 1U) != 0;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000597D0 File Offset: 0x00058BD0
		public bool SupportsDotProduct3
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 23 & 1U) != 0;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000597F0 File Offset: 0x00058BF0
		public unsafe bool SupportsMultiplyAdd
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (*(ref this.caps + 3) & 1) != 0;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00059810 File Offset: 0x00058C10
		public bool SupportsLerp
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 25 & 1U) != 0;
			}
		}

		// Token: 0x04000D6D RID: 3437
		private int caps;
	}
}
