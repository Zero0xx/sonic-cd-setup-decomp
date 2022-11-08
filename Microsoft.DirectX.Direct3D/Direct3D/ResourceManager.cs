using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000D6 RID: 214
	[MiscellaneousBits(1)]
	public struct ResourceManager
	{
		// Token: 0x06000375 RID: 885 RVA: 0x00060F54 File Offset: 0x00060354
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$106$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$107$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00061080 File Offset: 0x00060480
		public ResourceStats[] GetStatistics()
		{
			ResourceStats[] array = new ResourceStats[8];
			array.Initialize();
			ref ResourceStats resourceStats& = ref array[0];
			cpblk(ref resourceStats&, ref this.m_Internal, 352);
			return array;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000610B8 File Offset: 0x000604B8
		public ResourceManager()
		{
			initblk(ref this.m_Internal, 0, 352);
		}

		// Token: 0x04000F65 RID: 3941
		internal _D3DDEVINFO_RESOURCEMANAGER m_Internal;
	}
}
