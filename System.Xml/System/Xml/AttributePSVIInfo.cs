using System;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020000AD RID: 173
	internal class AttributePSVIInfo
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x0002C52C File Offset: 0x0002B52C
		internal AttributePSVIInfo()
		{
			this.attributeSchemaInfo = new XmlSchemaInfo();
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0002C53F File Offset: 0x0002B53F
		internal void Reset()
		{
			this.typedAttributeValue = null;
			this.localName = string.Empty;
			this.namespaceUri = string.Empty;
			this.attributeSchemaInfo.Clear();
		}

		// Token: 0x0400084F RID: 2127
		internal string localName;

		// Token: 0x04000850 RID: 2128
		internal string namespaceUri;

		// Token: 0x04000851 RID: 2129
		internal object typedAttributeValue;

		// Token: 0x04000852 RID: 2130
		internal XmlSchemaInfo attributeSchemaInfo;
	}
}
