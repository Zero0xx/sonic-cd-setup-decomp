using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000BA RID: 186
	[MiscellaneousBits(1)]
	public struct RasterStatus
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0005ECF8 File Offset: 0x0005E0F8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$78$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$79$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0005EE24 File Offset: 0x0005E224
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0005EE3C File Offset: 0x0005E23C
		public bool InVBlank
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_InVBlank;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.m_InVBlank = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0005EE58 File Offset: 0x0005E258
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0005EE70 File Offset: 0x0005E270
		public int ScanLine
		{
			get
			{
				return this.m_ScanLine;
			}
			set
			{
				this.m_ScanLine = value;
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0005EE8C File Offset: 0x0005E28C
		public RasterStatus()
		{
			ref RasterStatus rasterStatus& = ref this;
			initblk(ref rasterStatus&, 0, 8);
		}

		// Token: 0x04000F08 RID: 3848
		private bool m_InVBlank;

		// Token: 0x04000F09 RID: 3849
		private int m_ScanLine;
	}
}
