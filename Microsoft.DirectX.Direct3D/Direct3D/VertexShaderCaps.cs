using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200009F RID: 159
	[MiscellaneousBits(1)]
	public struct VertexShaderCaps
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x0005BD64 File Offset: 0x0005B164
		internal VertexShaderCaps()
		{
			this.m_Internal = null;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0005BD80 File Offset: 0x0005B180
		internal unsafe void SetHandle(_D3DVSHADERCAPS2_0* p)
		{
			this.m_Internal = p;
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0005BD9C File Offset: 0x0005B19C
		public unsafe bool SupportsPredication
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(int*)this.m_Internal & 1) != 0;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0005BDB8 File Offset: 0x0005B1B8
		public unsafe int DynamicFlowControlDepth
		{
			get
			{
				return *(int*)(this.m_Internal + 4 / sizeof(_D3DVSHADERCAPS2_0));
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0005BDD4 File Offset: 0x0005B1D4
		public unsafe int NumberTemps
		{
			get
			{
				return *(int*)(this.m_Internal + 8 / sizeof(_D3DVSHADERCAPS2_0));
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0005BDF0 File Offset: 0x0005B1F0
		public unsafe int StaticFlowControlDepth
		{
			get
			{
				return *(int*)(this.m_Internal + 12 / sizeof(_D3DVSHADERCAPS2_0));
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0005BE0C File Offset: 0x0005B20C
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$59$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$60$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000ECB RID: 3787
		internal unsafe _D3DVSHADERCAPS2_0* m_Internal;

		// Token: 0x04000ECC RID: 3788
		public const int MaxDynamicFlowControlDepth = 24;

		// Token: 0x04000ECD RID: 3789
		public const int MinDynamicFlowControlDepth = 0;

		// Token: 0x04000ECE RID: 3790
		public const int MaxNumberTemps = 32;

		// Token: 0x04000ECF RID: 3791
		public const int MinNumberTemps = 12;

		// Token: 0x04000ED0 RID: 3792
		public const int MaxStaticFlowControlDepth = 4;

		// Token: 0x04000ED1 RID: 3793
		public const int MinStaticFlowControlDepth = 1;
	}
}
