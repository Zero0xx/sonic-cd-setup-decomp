using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000D2 RID: 210
	[MiscellaneousBits(1)]
	public struct VertexCache
	{
		// Token: 0x06000360 RID: 864 RVA: 0x00060B00 File Offset: 0x0005FF00
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$102$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$103$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00060C2C File Offset: 0x0006002C
		public int Pattern
		{
			get
			{
				return (int)this.dwPattern;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00060C44 File Offset: 0x00060044
		public int OptimizationMethod
		{
			get
			{
				return (int)this.dwOptMethod;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00060C5C File Offset: 0x0006005C
		public int CacheSize
		{
			get
			{
				return (int)this.dwCacheSize;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00060C74 File Offset: 0x00060074
		public int MagicNumber
		{
			get
			{
				return (int)this.dwMagicNumber;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00060C8C File Offset: 0x0006008C
		public VertexCache()
		{
			ref VertexCache vertexCache& = ref this;
			initblk(ref vertexCache&, 0, 16);
		}

		// Token: 0x04000F54 RID: 3924
		internal uint dwPattern;

		// Token: 0x04000F55 RID: 3925
		internal uint dwOptMethod;

		// Token: 0x04000F56 RID: 3926
		internal uint dwCacheSize;

		// Token: 0x04000F57 RID: 3927
		internal uint dwMagicNumber;
	}
}
