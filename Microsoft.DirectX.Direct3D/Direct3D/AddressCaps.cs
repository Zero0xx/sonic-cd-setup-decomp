using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x0200004B RID: 75
	[MiscellaneousBits(1)]
	public struct AddressCaps
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00058EE4 File Offset: 0x000582E4
		internal AddressCaps()
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00058EC8 File Offset: 0x000582C8
		internal AddressCaps(int c)
		{
			this.caps = c;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00058EF8 File Offset: 0x000582F8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$23$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$24$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00059024 File Offset: 0x00058424
		public bool SupportsWrap
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps & 1) != 0;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00059040 File Offset: 0x00058440
		public bool SupportsMirror
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 1 & 1U) != 0;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00059060 File Offset: 0x00058460
		public bool SupportsClamp
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 2 & 1U) != 0;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00059080 File Offset: 0x00058480
		public bool SupportsBorder
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 3 & 1U) != 0;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000590A0 File Offset: 0x000584A0
		public bool SupportsIndependentUV
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000590C0 File Offset: 0x000584C0
		public bool SupportsMirrorOnce
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x04000D69 RID: 3433
		private int caps;
	}
}
