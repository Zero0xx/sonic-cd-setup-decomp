using System;

namespace System.Runtime.ConstrainedExecution
{
	// Token: 0x020004D7 RID: 1239
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Interface, Inherited = false)]
	public sealed class ReliabilityContractAttribute : Attribute
	{
		// Token: 0x0600312D RID: 12589 RVA: 0x000A8EFF File Offset: 0x000A7EFF
		public ReliabilityContractAttribute(Consistency consistencyGuarantee, Cer cer)
		{
			this._consistency = consistencyGuarantee;
			this._cer = cer;
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x000A8F15 File Offset: 0x000A7F15
		public Consistency ConsistencyGuarantee
		{
			get
			{
				return this._consistency;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x000A8F1D File Offset: 0x000A7F1D
		public Cer Cer
		{
			get
			{
				return this._cer;
			}
		}

		// Token: 0x040018E7 RID: 6375
		private Consistency _consistency;

		// Token: 0x040018E8 RID: 6376
		private Cer _cer;
	}
}
