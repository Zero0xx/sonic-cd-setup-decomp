using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000CC RID: 204
	[MiscellaneousBits(1)]
	public struct IndexBufferDescription
	{
		// Token: 0x06000343 RID: 835 RVA: 0x00060478 File Offset: 0x0005F878
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$96$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$97$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000344 RID: 836 RVA: 0x000605A4 File Offset: 0x0005F9A4
		public bool Is16BitIndices
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_Format == (Format)101;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000345 RID: 837 RVA: 0x000605C0 File Offset: 0x0005F9C0
		public ResourceType Type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000605D8 File Offset: 0x0005F9D8
		public Usage Usage
		{
			get
			{
				return this.m_Usage;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000347 RID: 839 RVA: 0x000605F0 File Offset: 0x0005F9F0
		public Pool Pool
		{
			get
			{
				return this.m_Pool;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00060608 File Offset: 0x0005FA08
		public int Size
		{
			get
			{
				return this.m_Size;
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00060620 File Offset: 0x0005FA20
		public IndexBufferDescription()
		{
			ref IndexBufferDescription indexBufferDescription& = ref this;
			initblk(ref indexBufferDescription&, 0, 20);
		}

		// Token: 0x04000F43 RID: 3907
		private Format m_Format;

		// Token: 0x04000F44 RID: 3908
		private ResourceType m_Type;

		// Token: 0x04000F45 RID: 3909
		private Usage m_Usage;

		// Token: 0x04000F46 RID: 3910
		private Pool m_Pool;

		// Token: 0x04000F47 RID: 3911
		private int m_Size;
	}
}
