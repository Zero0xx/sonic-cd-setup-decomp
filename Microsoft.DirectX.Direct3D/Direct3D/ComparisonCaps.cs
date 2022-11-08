using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000040 RID: 64
	[MiscellaneousBits(1)]
	public struct ComparisonCaps
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x000580C4 File Offset: 0x000574C4
		internal ComparisonCaps()
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000580A8 File Offset: 0x000574A8
		internal ComparisonCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000580D8 File Offset: 0x000574D8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$13$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$14$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00058204 File Offset: 0x00057604
		public bool SupportsNever
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00058220 File Offset: 0x00057620
		public bool SupportsLess
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00058240 File Offset: 0x00057640
		public bool SupportsEqual
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00058260 File Offset: 0x00057660
		public bool SupportsLessEqual
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00058280 File Offset: 0x00057680
		public bool SupportsGreater
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000582A0 File Offset: 0x000576A0
		public bool SupportsNotEqual
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000582C0 File Offset: 0x000576C0
		public bool SupportsGreaterEqual
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000582E0 File Offset: 0x000576E0
		public bool SupportsAlways
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x04000D58 RID: 3416
		private int caps;
	}
}
