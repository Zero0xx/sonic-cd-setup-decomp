using System;
using System.Xml.Schema;

namespace System.Xml.XPath
{
	// Token: 0x020000B7 RID: 183
	public abstract class XPathItem
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000A5D RID: 2653
		public abstract bool IsNode { get; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000A5E RID: 2654
		public abstract XmlSchemaType XmlType { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000A5F RID: 2655
		public abstract string Value { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000A60 RID: 2656
		public abstract object TypedValue { get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000A61 RID: 2657
		public abstract Type ValueType { get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000A62 RID: 2658
		public abstract bool ValueAsBoolean { get; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000A63 RID: 2659
		public abstract DateTime ValueAsDateTime { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000A64 RID: 2660
		public abstract double ValueAsDouble { get; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000A65 RID: 2661
		public abstract int ValueAsInt { get; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000A66 RID: 2662
		public abstract long ValueAsLong { get; }

		// Token: 0x06000A67 RID: 2663 RVA: 0x00030A49 File Offset: 0x0002FA49
		public virtual object ValueAs(Type returnType)
		{
			return this.ValueAs(returnType, null);
		}

		// Token: 0x06000A68 RID: 2664
		public abstract object ValueAs(Type returnType, IXmlNamespaceResolver nsResolver);
	}
}
