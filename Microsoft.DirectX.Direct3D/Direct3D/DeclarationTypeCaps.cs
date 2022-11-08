using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000055 RID: 85
	[MiscellaneousBits(1)]
	public struct DeclarationTypeCaps
	{
		// Token: 0x06000174 RID: 372 RVA: 0x00059CC8 File Offset: 0x000590C8
		internal DeclarationTypeCaps()
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00059CAC File Offset: 0x000590AC
		internal DeclarationTypeCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00059CDC File Offset: 0x000590DC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$33$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$34$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00059E08 File Offset: 0x00059208
		public bool SupportsUByte4
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00059E24 File Offset: 0x00059224
		public bool SupportsUByte4N
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00059E44 File Offset: 0x00059244
		public bool SupportsShort2N
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00059E64 File Offset: 0x00059264
		public bool SupportsShort4N
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00059E84 File Offset: 0x00059284
		public bool SupportsUShort2N
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00059EA4 File Offset: 0x000592A4
		public bool SupportsUShort4N
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00059EC4 File Offset: 0x000592C4
		public bool SupportsUDec3
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00059EE4 File Offset: 0x000592E4
		public bool SupportsDec3N
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00059F04 File Offset: 0x00059304
		public bool SupportsFloat16Two
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00059F24 File Offset: 0x00059324
		public bool SupportsFloat16Four
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x04000D73 RID: 3443
		private int caps;
	}
}
