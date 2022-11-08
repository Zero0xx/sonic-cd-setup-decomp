using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006AC RID: 1708
	[Serializable]
	internal class CallContextRemotingData : ICloneable
	{
		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x000D2E4C File Offset: 0x000D1E4C
		// (set) Token: 0x06003DB9 RID: 15801 RVA: 0x000D2E54 File Offset: 0x000D1E54
		internal string LogicalCallID
		{
			get
			{
				return this._logicalCallID;
			}
			set
			{
				this._logicalCallID = value;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x000D2E5D File Offset: 0x000D1E5D
		internal bool HasInfo
		{
			get
			{
				return this._logicalCallID != null;
			}
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x000D2E6C File Offset: 0x000D1E6C
		public object Clone()
		{
			return new CallContextRemotingData
			{
				LogicalCallID = this.LogicalCallID
			};
		}

		// Token: 0x04001F83 RID: 8067
		private string _logicalCallID;
	}
}
