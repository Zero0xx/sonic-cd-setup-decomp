using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000042 RID: 66
	[MiscellaneousBits(1)]
	public struct BlendCaps
	{
		// Token: 0x060000EE RID: 238 RVA: 0x0005833C File Offset: 0x0005773C
		internal BlendCaps()
		{
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00058320 File Offset: 0x00057720
		internal BlendCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00058350 File Offset: 0x00057750
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$15$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$16$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0005847C File Offset: 0x0005787C
		public bool SupportsZero
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00058498 File Offset: 0x00057898
		public bool SupportsOne
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x000584B8 File Offset: 0x000578B8
		public bool SupportsSourceColor
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x000584D8 File Offset: 0x000578D8
		public bool SupportsInverseSourceColor
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000584F8 File Offset: 0x000578F8
		public bool SupportsSourceAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00058518 File Offset: 0x00057918
		public bool SupportsInverseSourceAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00058538 File Offset: 0x00057938
		public bool SupportsDestinationAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00058558 File Offset: 0x00057958
		public bool SupportsInverseDestinationAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00058578 File Offset: 0x00057978
		public bool SupportsDestinationColor
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00058598 File Offset: 0x00057998
		public bool SupportsInverseDestinationColor
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000585B8 File Offset: 0x000579B8
		public bool SupportsSourceAlphaSat
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 10 & 1U) != 0;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000585D8 File Offset: 0x000579D8
		public bool SupportsBothSourceAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 11 & 1U) != 0;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000585F8 File Offset: 0x000579F8
		public bool SupportsBothInverseSourceAlpha
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 12 & 1U) != 0;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00058618 File Offset: 0x00057A18
		public bool SupportsBlendFactor
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 13 & 1U) != 0;
			}
		}

		// Token: 0x04000D5A RID: 3418
		private int caps;
	}
}
