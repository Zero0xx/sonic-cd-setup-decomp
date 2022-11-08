using System;

namespace System.Xml.Schema
{
	// Token: 0x020000AE RID: 174
	public interface IXmlSchemaInfo
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000982 RID: 2434
		XmlSchemaValidity Validity { get; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000983 RID: 2435
		bool IsDefault { get; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000984 RID: 2436
		bool IsNil { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000985 RID: 2437
		XmlSchemaSimpleType MemberType { get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000986 RID: 2438
		XmlSchemaType SchemaType { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000987 RID: 2439
		XmlSchemaElement SchemaElement { get; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000988 RID: 2440
		XmlSchemaAttribute SchemaAttribute { get; }
	}
}
