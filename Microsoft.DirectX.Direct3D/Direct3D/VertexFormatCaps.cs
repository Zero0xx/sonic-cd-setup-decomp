using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000051 RID: 81
	[MiscellaneousBits(1)]
	public struct VertexFormatCaps
	{
		// Token: 0x06000161 RID: 353 RVA: 0x0005986C File Offset: 0x00058C6C
		internal VertexFormatCaps()
		{
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00059850 File Offset: 0x00058C50
		internal VertexFormatCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00059880 File Offset: 0x00058C80
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$29$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$30$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000164 RID: 356 RVA: 0x000599AC File Offset: 0x00058DAC
		public bool SupportsTextureCoordinateCountMask
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (this.caps & 65535) != 0;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000599D4 File Offset: 0x00058DD4
		public bool SupportsDoNotStripElements
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 19 & 1U) != 0;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000599F4 File Offset: 0x00058DF4
		public bool SupportsPointSize
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 20 & 1U) != 0;
			}
		}

		// Token: 0x04000D6F RID: 3439
		private int caps;
	}
}
