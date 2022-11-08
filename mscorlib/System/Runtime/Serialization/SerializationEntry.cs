using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000379 RID: 889
	[ComVisible(true)]
	public struct SerializationEntry
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x00058296 File Offset: 0x00057296
		public object Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0005829E File Offset: 0x0005729E
		public string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x000582A6 File Offset: 0x000572A6
		public Type ObjectType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000582AE File Offset: 0x000572AE
		internal SerializationEntry(string entryName, object entryValue, Type entryType)
		{
			this.m_value = entryValue;
			this.m_name = entryName;
			this.m_type = entryType;
		}

		// Token: 0x04000EA5 RID: 3749
		private Type m_type;

		// Token: 0x04000EA6 RID: 3750
		private object m_value;

		// Token: 0x04000EA7 RID: 3751
		private string m_name;
	}
}
