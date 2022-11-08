using System;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x02000099 RID: 153
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class AsyncOperation
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x00016E04 File Offset: 0x00015E04
		private AsyncOperation(object userSuppliedState, SynchronizationContext syncContext)
		{
			this.userSuppliedState = userSuppliedState;
			this.syncContext = syncContext;
			this.alreadyCompleted = false;
			this.syncContext.OperationStarted();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00016E2C File Offset: 0x00015E2C
		~AsyncOperation()
		{
			if (!this.alreadyCompleted && this.syncContext != null)
			{
				this.syncContext.OperationCompleted();
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00016E70 File Offset: 0x00015E70
		public object UserSuppliedState
		{
			get
			{
				return this.userSuppliedState;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00016E78 File Offset: 0x00015E78
		public SynchronizationContext SynchronizationContext
		{
			get
			{
				return this.syncContext;
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00016E80 File Offset: 0x00015E80
		public void Post(SendOrPostCallback d, object arg)
		{
			this.VerifyNotCompleted();
			this.VerifyDelegateNotNull(d);
			this.syncContext.Post(d, arg);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00016E9C File Offset: 0x00015E9C
		public void PostOperationCompleted(SendOrPostCallback d, object arg)
		{
			this.Post(d, arg);
			this.OperationCompletedCore();
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00016EAC File Offset: 0x00015EAC
		public void OperationCompleted()
		{
			this.VerifyNotCompleted();
			this.OperationCompletedCore();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00016EBC File Offset: 0x00015EBC
		private void OperationCompletedCore()
		{
			try
			{
				this.syncContext.OperationCompleted();
			}
			finally
			{
				this.alreadyCompleted = true;
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00016EF4 File Offset: 0x00015EF4
		private void VerifyNotCompleted()
		{
			if (this.alreadyCompleted)
			{
				throw new InvalidOperationException(SR.GetString("Async_OperationAlreadyCompleted"));
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00016F0E File Offset: 0x00015F0E
		private void VerifyDelegateNotNull(SendOrPostCallback d)
		{
			if (d == null)
			{
				throw new ArgumentNullException(SR.GetString("Async_NullDelegate"), "d");
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00016F28 File Offset: 0x00015F28
		internal static AsyncOperation CreateOperation(object userSuppliedState, SynchronizationContext syncContext)
		{
			return new AsyncOperation(userSuppliedState, syncContext);
		}

		// Token: 0x040008D1 RID: 2257
		private SynchronizationContext syncContext;

		// Token: 0x040008D2 RID: 2258
		private object userSuppliedState;

		// Token: 0x040008D3 RID: 2259
		private bool alreadyCompleted;
	}
}
