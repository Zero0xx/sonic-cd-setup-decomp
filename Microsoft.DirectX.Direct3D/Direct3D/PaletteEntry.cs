using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000C2 RID: 194
	[MiscellaneousBits(1)]
	public struct PaletteEntry
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0005F6A4 File Offset: 0x0005EAA4
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$86$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$87$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0005F7D0 File Offset: 0x0005EBD0
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0005F7E8 File Offset: 0x0005EBE8
		public byte Red
		{
			get
			{
				return this.m_Red;
			}
			set
			{
				this.m_Red = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0005F804 File Offset: 0x0005EC04
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0005F81C File Offset: 0x0005EC1C
		public byte Green
		{
			get
			{
				return this.m_Green;
			}
			set
			{
				this.m_Green = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0005F838 File Offset: 0x0005EC38
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0005F850 File Offset: 0x0005EC50
		public byte Blue
		{
			get
			{
				return this.m_Blue;
			}
			set
			{
				this.m_Blue = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0005F86C File Offset: 0x0005EC6C
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0005F884 File Offset: 0x0005EC84
		public byte Flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0005F8D0 File Offset: 0x0005ECD0
		public PaletteEntry()
		{
			this.m_Flags = 0;
			this.m_Blue = 0;
			this.m_Green = 0;
			this.m_Red = 0;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0005F8A0 File Offset: 0x0005ECA0
		public PaletteEntry(byte red, byte green, byte blue, byte flags)
		{
			this.m_Red = red;
			this.m_Green = green;
			this.m_Blue = blue;
			this.m_Flags = flags;
		}

		// Token: 0x04000F1F RID: 3871
		private byte m_Red;

		// Token: 0x04000F20 RID: 3872
		private byte m_Green;

		// Token: 0x04000F21 RID: 3873
		private byte m_Blue;

		// Token: 0x04000F22 RID: 3874
		private byte m_Flags;
	}
}
