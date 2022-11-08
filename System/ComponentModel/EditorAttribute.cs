using System;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x020000DC RID: 220
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public sealed class EditorAttribute : Attribute
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x0001ABC5 File Offset: 0x00019BC5
		public EditorAttribute()
		{
			this.typeName = string.Empty;
			this.baseTypeName = string.Empty;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001ABE3 File Offset: 0x00019BE3
		public EditorAttribute(string typeName, string baseTypeName)
		{
			typeName.ToUpper(CultureInfo.InvariantCulture);
			this.typeName = typeName;
			this.baseTypeName = baseTypeName;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001AC05 File Offset: 0x00019C05
		public EditorAttribute(string typeName, Type baseType)
		{
			typeName.ToUpper(CultureInfo.InvariantCulture);
			this.typeName = typeName;
			this.baseTypeName = baseType.AssemblyQualifiedName;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001AC2C File Offset: 0x00019C2C
		public EditorAttribute(Type type, Type baseType)
		{
			this.typeName = type.AssemblyQualifiedName;
			this.baseTypeName = baseType.AssemblyQualifiedName;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0001AC4C File Offset: 0x00019C4C
		public string EditorBaseTypeName
		{
			get
			{
				return this.baseTypeName;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0001AC54 File Offset: 0x00019C54
		public string EditorTypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001AC5C File Offset: 0x00019C5C
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					string text = this.baseTypeName;
					int num = text.IndexOf(',');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					this.typeId = base.GetType().FullName + text;
				}
				return this.typeId;
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001ACAC File Offset: 0x00019CAC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			EditorAttribute editorAttribute = obj as EditorAttribute;
			return editorAttribute != null && editorAttribute.typeName == this.typeName && editorAttribute.baseTypeName == this.baseTypeName;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001ACEF File Offset: 0x00019CEF
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400095F RID: 2399
		private string baseTypeName;

		// Token: 0x04000960 RID: 2400
		private string typeName;

		// Token: 0x04000961 RID: 2401
		private string typeId;
	}
}
