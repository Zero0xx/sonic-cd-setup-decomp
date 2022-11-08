using System;
using System.Collections.Generic;

namespace System.Xml
{
	// Token: 0x02000072 RID: 114
	internal class XmlCharCheckingReaderWithNS : XmlCharCheckingReader, IXmlNamespaceResolver
	{
		// Token: 0x060004D6 RID: 1238 RVA: 0x00015025 File Offset: 0x00014025
		internal XmlCharCheckingReaderWithNS(XmlReader reader, IXmlNamespaceResolver readerAsNSResolver, bool checkCharacters, bool ignoreWhitespace, bool ignoreComments, bool ignorePis, bool prohibitDtd) : base(reader, checkCharacters, ignoreWhitespace, ignoreComments, ignorePis, prohibitDtd)
		{
			this.readerAsNSResolver = readerAsNSResolver;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001503E File Offset: 0x0001403E
		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return this.readerAsNSResolver.GetNamespacesInScope(scope);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001504C File Offset: 0x0001404C
		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return this.readerAsNSResolver.LookupNamespace(prefix);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001505A File Offset: 0x0001405A
		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return this.readerAsNSResolver.LookupPrefix(namespaceName);
		}

		// Token: 0x040005F6 RID: 1526
		internal IXmlNamespaceResolver readerAsNSResolver;
	}
}
