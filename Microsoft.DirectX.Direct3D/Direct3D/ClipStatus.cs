using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000AF RID: 175
	[MiscellaneousBits(1)]
	public struct ClipStatus
	{
		// Token: 0x0600027B RID: 635 RVA: 0x0005D7EC File Offset: 0x0005CBEC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$70$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$71$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0005D918 File Offset: 0x0005CD18
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0005D930 File Offset: 0x0005CD30
		public ClipStatusFlags ClipUnion
		{
			get
			{
				return this.m_ClipUnion;
			}
			set
			{
				this.m_ClipUnion = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0005D94C File Offset: 0x0005CD4C
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0005D964 File Offset: 0x0005CD64
		public ClipStatusFlags ClipIntersection
		{
			get
			{
				return this.m_ClipIntersection;
			}
			set
			{
				this.m_ClipIntersection = value;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0005D980 File Offset: 0x0005CD80
		public ClipStatus()
		{
			ref ClipStatus clipStatus& = ref this;
			initblk(ref clipStatus&, 0, 8);
		}

		// Token: 0x04000EF2 RID: 3826
		private ClipStatusFlags m_ClipUnion;

		// Token: 0x04000EF3 RID: 3827
		private ClipStatusFlags m_ClipIntersection;
	}
}
