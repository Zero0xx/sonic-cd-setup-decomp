using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200004D RID: 77
	[MiscellaneousBits(1)]
	public struct StencilCaps
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0005911C File Offset: 0x0005851C
		internal StencilCaps()
		{
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00059100 File Offset: 0x00058500
		internal StencilCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00059130 File Offset: 0x00058530
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$25$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$26$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0005925C File Offset: 0x0005865C
		public bool SupportsKeep
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00059278 File Offset: 0x00058678
		public bool SupportsZero
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00059298 File Offset: 0x00058698
		public bool SupportsReplace
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000592B8 File Offset: 0x000586B8
		public bool SupportsIncrementSaturation
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000592D8 File Offset: 0x000586D8
		public bool SupportsDecrementSaturation
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000592F8 File Offset: 0x000586F8
		public bool SupportsInvert
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00059318 File Offset: 0x00058718
		public bool SupportsIncrement
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00059338 File Offset: 0x00058738
		public bool SupportsDecrement
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00059358 File Offset: 0x00058758
		public bool SupportsTwoSided
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x04000D6B RID: 3435
		private int caps;
	}
}
