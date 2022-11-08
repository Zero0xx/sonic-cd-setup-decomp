using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000DD RID: 221
	[MiscellaneousBits(1)]
	public struct StageTimings
	{
		// Token: 0x06000388 RID: 904 RVA: 0x000614C8 File Offset: 0x000608C8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$112$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$113$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000389 RID: 905 RVA: 0x000615F4 File Offset: 0x000609F4
		public float MemoryProcessingPercent
		{
			get
			{
				return this.fMemoryProcessingPercent;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0006160C File Offset: 0x00060A0C
		public float ComputationProcessingPercent
		{
			get
			{
				return this.fComputationProcessingPercent;
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00061624 File Offset: 0x00060A24
		public StageTimings()
		{
			ref StageTimings stageTimings& = ref this;
			initblk(ref stageTimings&, 0, 8);
		}

		// Token: 0x04000F72 RID: 3954
		internal float fMemoryProcessingPercent;

		// Token: 0x04000F73 RID: 3955
		internal float fComputationProcessingPercent;
	}
}
