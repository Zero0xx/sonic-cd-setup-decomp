using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000049 RID: 73
	[MiscellaneousBits(1)]
	public struct FilterCaps
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00058BE8 File Offset: 0x00057FE8
		internal FilterCaps()
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00058BCC File Offset: 0x00057FCC
		internal FilterCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00058BFC File Offset: 0x00057FFC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$21$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$22$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00058D28 File Offset: 0x00058128
		public bool SupportsMinifyPoint
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00058D48 File Offset: 0x00058148
		public bool SupportsMinifyLinear
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00058D68 File Offset: 0x00058168
		public bool SupportsMinifyAnisotropic
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 10 & 1U) != 0;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00058D88 File Offset: 0x00058188
		public unsafe bool SupportsMipMapPoint
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(ref this.caps + 2) & 1) != 0;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00058DA8 File Offset: 0x000581A8
		public bool SupportsMipMapLinear
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 17 & 1U) != 0;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00058DC8 File Offset: 0x000581C8
		public unsafe bool SupportsMagnifyPoint
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (*(ref this.caps + 3) & 1) != 0;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00058DE8 File Offset: 0x000581E8
		public bool SupportsMagnifyLinear
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 25 & 1U) != 0;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00058E08 File Offset: 0x00058208
		public bool SupportsMagnifyAnisotropic
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 26 & 1U) != 0;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00058E28 File Offset: 0x00058228
		public bool SupportsMagnifyPyramidalQuad
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 27 & 1U) != 0;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00058E48 File Offset: 0x00058248
		public bool SupportsMagnifyGaussianQuad
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 28 & 1U) != 0;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00058E68 File Offset: 0x00058268
		public bool SupportsMinifyPyramidalQuad
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 11 & 1U) != 0;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00058E88 File Offset: 0x00058288
		public bool SupportsMinifyGaussianQuad
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 12 & 1U) != 0;
			}
		}

		// Token: 0x04000D67 RID: 3431
		private int caps;
	}
}
