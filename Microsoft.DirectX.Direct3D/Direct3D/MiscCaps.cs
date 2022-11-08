using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200003A RID: 58
	[MiscellaneousBits(1)]
	public struct MiscCaps
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00057778 File Offset: 0x00056B78
		internal MiscCaps()
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0005775C File Offset: 0x00056B5C
		internal MiscCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0005778C File Offset: 0x00056B8C
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$7$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$8$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000578B8 File Offset: 0x00056CB8
		public bool SupportsMaskZ
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000578D8 File Offset: 0x00056CD8
		public bool SupportsCullNone
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000578F8 File Offset: 0x00056CF8
		public bool SupportsCullClockwise
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00057918 File Offset: 0x00056D18
		public bool SupportsCullCounterClockwise
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00057938 File Offset: 0x00056D38
		public bool SupportsColorWrite
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00057958 File Offset: 0x00056D58
		public bool SupportsClipPlaneScaledPoints
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00057978 File Offset: 0x00056D78
		public bool SupportsClipTransformedVertices
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00057998 File Offset: 0x00056D98
		public bool SupportsTextureStageStateArgumentTemp
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 10 & 1U) != 0;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000579B8 File Offset: 0x00056DB8
		public bool SupportsBlendOperation
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 11 & 1U) != 0;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000579D8 File Offset: 0x00056DD8
		public bool IsNullReference
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 12 & 1U) != 0;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000579F8 File Offset: 0x00056DF8
		public bool SupportsIndependentWriteMasks
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 14 & 1U) != 0;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00057A18 File Offset: 0x00056E18
		public bool SupportsPerStageConstant
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 15 & 1U) != 0;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00057A38 File Offset: 0x00056E38
		public unsafe bool SupportsFogAndSpecularAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(ref this.caps + 2) & 1) != 0;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00057A58 File Offset: 0x00056E58
		public bool SupportsSeparateAlphaBlend
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 17 & 1U) != 0;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00057A78 File Offset: 0x00056E78
		public bool SupportsMultipleRenderTargetsIndependentBitDepths
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 18 & 1U) != 0;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00057A98 File Offset: 0x00056E98
		public bool SupportsMultipleRenderTargetsPostPixelShaderBlending
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 19 & 1U) != 0;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00057AB8 File Offset: 0x00056EB8
		public bool HasFogVertexClamped
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 20 & 1U) != 0;
			}
		}

		// Token: 0x04000D52 RID: 3410
		private int caps;
	}
}
