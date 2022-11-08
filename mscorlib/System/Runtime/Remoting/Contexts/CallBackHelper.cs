using System;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006C6 RID: 1734
	[Serializable]
	internal class CallBackHelper
	{
		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003EA5 RID: 16037 RVA: 0x000D6EA9 File Offset: 0x000D5EA9
		// (set) Token: 0x06003EA6 RID: 16038 RVA: 0x000D6EB6 File Offset: 0x000D5EB6
		internal bool IsEERequested
		{
			get
			{
				return (this._flags & 1) == 1;
			}
			set
			{
				if (value)
				{
					this._flags |= 1;
				}
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (set) Token: 0x06003EA7 RID: 16039 RVA: 0x000D6EC9 File Offset: 0x000D5EC9
		internal bool IsCrossDomain
		{
			set
			{
				if (value)
				{
					this._flags |= 256;
				}
			}
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x000D6EE0 File Offset: 0x000D5EE0
		internal CallBackHelper(IntPtr privateData, bool bFromEE, int targetDomainID)
		{
			this.IsEERequested = bFromEE;
			this.IsCrossDomain = (targetDomainID != 0);
			this._privateData = privateData;
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x000D6F03 File Offset: 0x000D5F03
		internal void Func()
		{
			if (this.IsEERequested)
			{
				Context.ExecuteCallBackInEE(this._privateData);
			}
		}

		// Token: 0x04001FE2 RID: 8162
		internal const int RequestedFromEE = 1;

		// Token: 0x04001FE3 RID: 8163
		internal const int XDomainTransition = 256;

		// Token: 0x04001FE4 RID: 8164
		private int _flags;

		// Token: 0x04001FE5 RID: 8165
		private IntPtr _privateData;
	}
}
