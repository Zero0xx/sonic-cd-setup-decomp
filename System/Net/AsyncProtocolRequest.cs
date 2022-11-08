using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200059A RID: 1434
	internal class AsyncProtocolRequest
	{
		// Token: 0x06002C26 RID: 11302 RVA: 0x000BDEAA File Offset: 0x000BCEAA
		public AsyncProtocolRequest(LazyAsyncResult userAsyncResult)
		{
			this.UserAsyncResult = userAsyncResult;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000BDEB9 File Offset: 0x000BCEB9
		public void SetNextRequest(byte[] buffer, int offset, int count, AsyncProtocolCallback callback)
		{
			if (this._CompletionStatus != 0)
			{
				throw new InternalException();
			}
			this.Buffer = buffer;
			this.Offset = offset;
			this.Count = count;
			this._Callback = callback;
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x000BDEE6 File Offset: 0x000BCEE6
		internal object AsyncObject
		{
			get
			{
				return this.UserAsyncResult.AsyncObject;
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000BDEF4 File Offset: 0x000BCEF4
		internal void CompleteRequest(int result)
		{
			this.Result = result;
			int num = Interlocked.Exchange(ref this._CompletionStatus, 1);
			if (num == 1)
			{
				throw new InternalException();
			}
			if (num == 2)
			{
				this._CompletionStatus = 0;
				this._Callback(this);
			}
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x000BDF38 File Offset: 0x000BCF38
		public bool MustCompleteSynchronously
		{
			get
			{
				int num = Interlocked.Exchange(ref this._CompletionStatus, 2);
				if (num == 2)
				{
					throw new InternalException();
				}
				if (num == 1)
				{
					this._CompletionStatus = 0;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000BDF6A File Offset: 0x000BCF6A
		internal void CompleteWithError(Exception e)
		{
			this.UserAsyncResult.InvokeCallback(e);
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000BDF78 File Offset: 0x000BCF78
		internal void CompleteUser()
		{
			this.UserAsyncResult.InvokeCallback();
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000BDF85 File Offset: 0x000BCF85
		internal void CompleteUser(object userResult)
		{
			this.UserAsyncResult.InvokeCallback(userResult);
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x000BDF93 File Offset: 0x000BCF93
		internal bool IsUserCompleted
		{
			get
			{
				return this.UserAsyncResult.InternalPeekCompleted;
			}
		}

		// Token: 0x04002A00 RID: 10752
		private const int StatusNotStarted = 0;

		// Token: 0x04002A01 RID: 10753
		private const int StatusCompleted = 1;

		// Token: 0x04002A02 RID: 10754
		private const int StatusCheckedOnSyncCompletion = 2;

		// Token: 0x04002A03 RID: 10755
		private AsyncProtocolCallback _Callback;

		// Token: 0x04002A04 RID: 10756
		private int _CompletionStatus;

		// Token: 0x04002A05 RID: 10757
		public LazyAsyncResult UserAsyncResult;

		// Token: 0x04002A06 RID: 10758
		public int Result;

		// Token: 0x04002A07 RID: 10759
		public object AsyncState;

		// Token: 0x04002A08 RID: 10760
		public byte[] Buffer;

		// Token: 0x04002A09 RID: 10761
		public int Offset;

		// Token: 0x04002A0A RID: 10762
		public int Count;
	}
}
