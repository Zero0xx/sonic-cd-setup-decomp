using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000E5 RID: 229
	[MiscellaneousBits(1)]
	public struct ValidateDeviceParams
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x00061BDC File Offset: 0x00060FDC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$120$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$121$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00061D08 File Offset: 0x00061108
		public int Passes
		{
			get
			{
				return (int)this.dwPasses;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00061D20 File Offset: 0x00061120
		public ResultCode Result
		{
			get
			{
				return this.mResult;
			}
		}

		// Token: 0x04000F81 RID: 3969
		internal uint dwPasses;

		// Token: 0x04000F82 RID: 3970
		internal ResultCode mResult;
	}
}
