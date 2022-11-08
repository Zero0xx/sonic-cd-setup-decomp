using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004D0 RID: 1232
	internal class CallbackClosure
	{
		// Token: 0x0600265A RID: 9818 RVA: 0x0009C080 File Offset: 0x0009B080
		internal CallbackClosure(ExecutionContext context, AsyncCallback callback)
		{
			if (callback != null)
			{
				this.savedCallback = callback;
				this.savedContext = context;
			}
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x0009C099 File Offset: 0x0009B099
		internal bool IsCompatible(AsyncCallback callback)
		{
			return callback != null && this.savedCallback != null && object.Equals(this.savedCallback, callback);
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600265C RID: 9820 RVA: 0x0009C0B9 File Offset: 0x0009B0B9
		internal AsyncCallback AsyncCallback
		{
			get
			{
				return this.savedCallback;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x0009C0C1 File Offset: 0x0009B0C1
		internal ExecutionContext Context
		{
			get
			{
				return this.savedContext;
			}
		}

		// Token: 0x040025E3 RID: 9699
		private AsyncCallback savedCallback;

		// Token: 0x040025E4 RID: 9700
		private ExecutionContext savedContext;
	}
}
