using System;

namespace System.ComponentModel
{
	// Token: 0x0200009D RID: 157
	[AttributeUsage(AttributeTargets.Property)]
	public class AttributeProviderAttribute : Attribute
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0001741E File Offset: 0x0001641E
		public AttributeProviderAttribute(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this._typeName = typeName;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001743B File Offset: 0x0001643B
		public AttributeProviderAttribute(string typeName, string propertyName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			this._typeName = typeName;
			this._propertyName = propertyName;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001746D File Offset: 0x0001646D
		public AttributeProviderAttribute(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001748F File Offset: 0x0001648F
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00017497 File Offset: 0x00016497
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x040008DD RID: 2269
		private string _typeName;

		// Token: 0x040008DE RID: 2270
		private string _propertyName;
	}
}
