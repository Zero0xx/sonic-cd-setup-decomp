using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000044 RID: 68
	[MiscellaneousBits(1)]
	public struct ShadeCaps
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00058674 File Offset: 0x00057A74
		internal ShadeCaps()
		{
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00058658 File Offset: 0x00057A58
		internal ShadeCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00058688 File Offset: 0x00057A88
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$17$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$18$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000587B4 File Offset: 0x00057BB4
		public bool SupportsColorGouraudRgb
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000587D4 File Offset: 0x00057BD4
		public bool SupportsSpecularGouraudRgb
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000587F4 File Offset: 0x00057BF4
		public bool SupportsAlphaGouraudBlend
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 14 & 1U) != 0;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00058814 File Offset: 0x00057C14
		public bool SupportsFogGouraud
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 19 & 1U) != 0;
			}
		}

		// Token: 0x04000D5C RID: 3420
		private int caps;
	}
}
