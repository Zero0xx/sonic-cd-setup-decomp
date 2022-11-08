using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000053 RID: 83
	[MiscellaneousBits(1)]
	public struct VertexProcessingCaps
	{
		// Token: 0x06000168 RID: 360 RVA: 0x00059A50 File Offset: 0x00058E50
		internal VertexProcessingCaps()
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00059A34 File Offset: 0x00058E34
		internal VertexProcessingCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00059A64 File Offset: 0x00058E64
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$31$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$32$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00059B90 File Offset: 0x00058F90
		public bool SupportsTextureGeneration
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00059BAC File Offset: 0x00058FAC
		public bool SupportsMaterialSource
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00059BCC File Offset: 0x00058FCC
		public bool SupportsDirectionalLights
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00059BEC File Offset: 0x00058FEC
		public bool SupportsPositionalLights
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00059C0C File Offset: 0x0005900C
		public bool SupportsLocalViewer
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00059C2C File Offset: 0x0005902C
		public bool SupportsTweening
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00059C4C File Offset: 0x0005904C
		public bool SupportsTextureGenerationSphereMap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00059C6C File Offset: 0x0005906C
		public bool SupportsNoTextureGenerationNonLocalViewer
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x04000D71 RID: 3441
		private int caps;
	}
}
