using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200003C RID: 60
	[MiscellaneousBits(1)]
	public struct LineCaps
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00057B14 File Offset: 0x00056F14
		internal LineCaps()
		{
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00057AF8 File Offset: 0x00056EF8
		internal LineCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00057B28 File Offset: 0x00056F28
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$9$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$10$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00057C54 File Offset: 0x00057054
		public bool SupportsTextureMapping
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00057C70 File Offset: 0x00057070
		public bool SupportsZBufferTest
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00057C90 File Offset: 0x00057090
		public bool SupportsBlend
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00057CB0 File Offset: 0x000570B0
		public bool SupportsAlphaCompare
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00057CD0 File Offset: 0x000570D0
		public bool SupportsFog
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00057CF0 File Offset: 0x000570F0
		public bool SupportsAntiAlias
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x04000D54 RID: 3412
		private int caps;
	}
}
