using System;
using System.Threading;

namespace System.Net.Mime
{
	// Token: 0x020006AF RID: 1711
	internal class MultiAsyncResult : LazyAsyncResult
	{
		// Token: 0x060034E0 RID: 13536 RVA: 0x000E0B73 File Offset: 0x000DFB73
		internal MultiAsyncResult(object context, AsyncCallback callback, object state) : base(context, state, callback)
		{
			this.context = context;
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060034E1 RID: 13537 RVA: 0x000E0B85 File Offset: 0x000DFB85
		internal object Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x000E0B8D File Offset: 0x000DFB8D
		internal void Enter()
		{
			this.Increment();
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000E0B95 File Offset: 0x000DFB95
		internal void Leave()
		{
			this.Decrement();
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x000E0B9D File Offset: 0x000DFB9D
		internal void Leave(object result)
		{
			base.Result = result;
			this.Decrement();
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000E0BAC File Offset: 0x000DFBAC
		private void Decrement()
		{
			if (Interlocked.Decrement(ref this.outstanding) == -1)
			{
				base.InvokeCallback(base.Result);
			}
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000E0BC8 File Offset: 0x000DFBC8
		private void Increment()
		{
			Interlocked.Increment(ref this.outstanding);
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000E0BD6 File Offset: 0x000DFBD6
		internal void CompleteSequence()
		{
			this.Decrement();
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000E0BE0 File Offset: 0x000DFBE0
		internal static object End(IAsyncResult result)
		{
			MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result;
			multiAsyncResult.InternalWaitForCompletion();
			return multiAsyncResult.Result;
		}

		// Token: 0x0400308D RID: 12429
		private int outstanding;

		// Token: 0x0400308E RID: 12430
		private object context;
	}
}
