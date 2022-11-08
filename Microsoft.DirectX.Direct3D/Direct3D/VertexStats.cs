using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000D0 RID: 208
	[MiscellaneousBits(1)]
	public struct VertexStats
	{
		// Token: 0x0600035B RID: 859 RVA: 0x0006095C File Offset: 0x0005FD5C
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$100$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$101$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00060A88 File Offset: 0x0005FE88
		public int NumberRenderedTriangles
		{
			get
			{
				return (int)this.dwNumRenderedTriangles;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00060AA0 File Offset: 0x0005FEA0
		public int NumberExtraClippingTriangles
		{
			get
			{
				return (int)this.dwNumExtraClippingTriangles;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00060AB8 File Offset: 0x0005FEB8
		public VertexStats()
		{
			ref VertexStats vertexStats& = ref this;
			initblk(ref vertexStats&, 0, 8);
		}

		// Token: 0x04000F51 RID: 3921
		internal uint dwNumRenderedTriangles;

		// Token: 0x04000F52 RID: 3922
		internal uint dwNumExtraClippingTriangles;
	}
}
