using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000A2 RID: 162
	[MiscellaneousBits(1)]
	public struct PixelShaderCaps
	{
		// Token: 0x060001ED RID: 493 RVA: 0x0005BF64 File Offset: 0x0005B364
		internal PixelShaderCaps()
		{
			this.m_Internal = null;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0005BF80 File Offset: 0x0005B380
		internal unsafe void SetHandle(_D3DPSHADERCAPS2_0* p)
		{
			this.m_Internal = p;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0005BF9C File Offset: 0x0005B39C
		public unsafe bool SupportsPredication
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)(*(int*)this.m_Internal) >> 2 & 1U) != 0;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0005BFBC File Offset: 0x0005B3BC
		public unsafe bool SupportsArbitrarySwizzle
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(int*)this.m_Internal & 1) != 0;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0005BFD8 File Offset: 0x0005B3D8
		public unsafe bool SupportsGradientInstructions
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)(*(int*)this.m_Internal) >> 1 & 1U) != 0;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0005BFF8 File Offset: 0x0005B3F8
		public unsafe bool SupportsNoDependentReadLimit
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)(*(int*)this.m_Internal) >> 3 & 1U) != 0;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0005C018 File Offset: 0x0005B418
		public unsafe bool SupportsNoTextureInstructionLimit
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)(*(int*)this.m_Internal) >> 4 & 1U) != 0;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0005C038 File Offset: 0x0005B438
		public unsafe int DynamicFlowControlDepth
		{
			get
			{
				return *(int*)(this.m_Internal + 4 / sizeof(_D3DPSHADERCAPS2_0));
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0005C054 File Offset: 0x0005B454
		public unsafe int NumberTemps
		{
			get
			{
				return *(int*)(this.m_Internal + 8 / sizeof(_D3DPSHADERCAPS2_0));
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0005C070 File Offset: 0x0005B470
		public unsafe int StaticFlowControlDepth
		{
			get
			{
				return *(int*)(this.m_Internal + 12 / sizeof(_D3DPSHADERCAPS2_0));
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0005C08C File Offset: 0x0005B48C
		public unsafe int NumberInstructionSlots
		{
			get
			{
				return *(int*)(this.m_Internal + 16 / sizeof(_D3DPSHADERCAPS2_0));
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0005C0A8 File Offset: 0x0005B4A8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$61$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$62$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000ED3 RID: 3795
		internal unsafe _D3DPSHADERCAPS2_0* m_Internal;

		// Token: 0x04000ED4 RID: 3796
		public const int MaxDynamicFlowControlDepth = 24;

		// Token: 0x04000ED5 RID: 3797
		public const int MinDynamicFlowControlDepth = 0;

		// Token: 0x04000ED6 RID: 3798
		public const int MaxNumberTemps = 32;

		// Token: 0x04000ED7 RID: 3799
		public const int MinNumberTemps = 12;

		// Token: 0x04000ED8 RID: 3800
		public const int MaxStaticFlowControlDepth = 4;

		// Token: 0x04000ED9 RID: 3801
		public const int MinStaticFlowControlDepth = 0;

		// Token: 0x04000EDA RID: 3802
		public const int MaxNumberInstructionSlots = 512;

		// Token: 0x04000EDB RID: 3803
		public const int MinNumberInstructionSlots = 96;
	}
}
