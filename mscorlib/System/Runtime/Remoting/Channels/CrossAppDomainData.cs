using System;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D0 RID: 1744
	[Serializable]
	internal class CrossAppDomainData
	{
		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003EE3 RID: 16099 RVA: 0x000D7802 File Offset: 0x000D6802
		internal virtual IntPtr ContextID
		{
			get
			{
				return new IntPtr((long)this._ContextID);
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003EE4 RID: 16100 RVA: 0x000D7814 File Offset: 0x000D6814
		internal virtual int DomainID
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._DomainID;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003EE5 RID: 16101 RVA: 0x000D781C File Offset: 0x000D681C
		internal virtual string ProcessGuid
		{
			get
			{
				return this._processGuid;
			}
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x000D7824 File Offset: 0x000D6824
		internal CrossAppDomainData(IntPtr ctxId, int domainID, string processGuid)
		{
			this._DomainID = domainID;
			this._processGuid = processGuid;
			this._ContextID = ctxId.ToInt64();
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x000D7858 File Offset: 0x000D6858
		internal bool IsFromThisProcess()
		{
			return Identity.ProcessGuid.Equals(this._processGuid);
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x000D786A File Offset: 0x000D686A
		internal bool IsFromThisAppDomain()
		{
			return this.IsFromThisProcess() && Thread.GetDomain().GetId() == this._DomainID;
		}

		// Token: 0x04001FF6 RID: 8182
		private object _ContextID = 0;

		// Token: 0x04001FF7 RID: 8183
		private int _DomainID;

		// Token: 0x04001FF8 RID: 8184
		private string _processGuid;
	}
}
