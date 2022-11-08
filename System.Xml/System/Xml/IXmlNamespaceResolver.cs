using System;
using System.Collections.Generic;

namespace System.Xml
{
	// Token: 0x0200001E RID: 30
	public interface IXmlNamespaceResolver
	{
		// Token: 0x0600007F RID: 127
		IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope);

		// Token: 0x06000080 RID: 128
		string LookupNamespace(string prefix);

		// Token: 0x06000081 RID: 129
		string LookupPrefix(string namespaceName);
	}
}
