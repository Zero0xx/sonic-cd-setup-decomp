using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000CA RID: 202
	[MiscellaneousBits(1)]
	public struct VertexBufferDescription
	{
		// Token: 0x06000334 RID: 820 RVA: 0x000601CC File Offset: 0x0005F5CC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$94$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$95$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000602F8 File Offset: 0x0005F6F8
		// (set) Token: 0x06000336 RID: 822 RVA: 0x00060310 File Offset: 0x0005F710
		public Format Format
		{
			get
			{
				return this.m_Format;
			}
			set
			{
				this.m_Format = value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0006032C File Offset: 0x0005F72C
		// (set) Token: 0x06000338 RID: 824 RVA: 0x00060344 File Offset: 0x0005F744
		public ResourceType Type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00060360 File Offset: 0x0005F760
		// (set) Token: 0x0600033A RID: 826 RVA: 0x00060378 File Offset: 0x0005F778
		public Usage Usage
		{
			get
			{
				return this.m_Usage;
			}
			set
			{
				this.m_Usage = value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00060394 File Offset: 0x0005F794
		// (set) Token: 0x0600033C RID: 828 RVA: 0x000603AC File Offset: 0x0005F7AC
		public Pool Pool
		{
			get
			{
				return this.m_Pool;
			}
			set
			{
				this.m_Pool = value;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600033D RID: 829 RVA: 0x000603C8 File Offset: 0x0005F7C8
		// (set) Token: 0x0600033E RID: 830 RVA: 0x000603E0 File Offset: 0x0005F7E0
		public int Size
		{
			get
			{
				return this.m_Size;
			}
			set
			{
				this.m_Size = value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000603FC File Offset: 0x0005F7FC
		// (set) Token: 0x06000340 RID: 832 RVA: 0x00060414 File Offset: 0x0005F814
		public VertexFormats VertexFormat
		{
			get
			{
				return this.m_VertexFormat;
			}
			set
			{
				this.m_VertexFormat = value;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00060430 File Offset: 0x0005F830
		public VertexBufferDescription()
		{
			ref VertexBufferDescription vertexBufferDescription& = ref this;
			initblk(ref vertexBufferDescription&, 0, 24);
		}

		// Token: 0x04000F3C RID: 3900
		private Format m_Format;

		// Token: 0x04000F3D RID: 3901
		private ResourceType m_Type;

		// Token: 0x04000F3E RID: 3902
		private Usage m_Usage;

		// Token: 0x04000F3F RID: 3903
		private Pool m_Pool;

		// Token: 0x04000F40 RID: 3904
		private int m_Size;

		// Token: 0x04000F41 RID: 3905
		private VertexFormats m_VertexFormat;
	}
}
