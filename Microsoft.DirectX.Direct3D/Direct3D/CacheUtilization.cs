using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000E1 RID: 225
	[MiscellaneousBits(1)]
	public struct CacheUtilization
	{
		// Token: 0x06000395 RID: 917 RVA: 0x00061858 File Offset: 0x00060C58
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$116$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$117$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00061984 File Offset: 0x00060D84
		public float TextureCacheHitRate
		{
			get
			{
				return this.fTextureCacheHitRate;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000397 RID: 919 RVA: 0x0006199C File Offset: 0x00060D9C
		public float PostTransformVertexCacheHitRate
		{
			get
			{
				return this.fPostTransformVertexCacheHitRate;
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000619B4 File Offset: 0x00060DB4
		public CacheUtilization()
		{
			ref CacheUtilization cacheUtilization& = ref this;
			initblk(ref cacheUtilization&, 0, 8);
		}

		// Token: 0x04000F7B RID: 3963
		internal float fTextureCacheHitRate;

		// Token: 0x04000F7C RID: 3964
		internal float fPostTransformVertexCacheHitRate;
	}
}
