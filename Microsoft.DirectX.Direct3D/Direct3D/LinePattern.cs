using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000B5 RID: 181
	[MiscellaneousBits(1)]
	public struct LinePattern
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0005DDAC File Offset: 0x0005D1AC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$74$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$75$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0005DED8 File Offset: 0x0005D2D8
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0005DEF4 File Offset: 0x0005D2F4
		public short RepeatFactor
		{
			get
			{
				return (short)this.wRepeatFactor;
			}
			set
			{
				this.wRepeatFactor = (ushort)value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0005DF10 File Offset: 0x0005D310
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0005DF2C File Offset: 0x0005D32C
		public short Linepattern
		{
			get
			{
				return (short)this.wLinePattern;
			}
			set
			{
				this.wLinePattern = (ushort)value;
			}
		}

		// Token: 0x04000EFB RID: 3835
		internal ushort wRepeatFactor;

		// Token: 0x04000EFC RID: 3836
		internal ushort wLinePattern;
	}
}
