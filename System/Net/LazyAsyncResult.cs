using System;
using System.Diagnostics;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003A0 RID: 928
	internal class LazyAsyncResult : IAsyncResult
	{
		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x0006F468 File Offset: 0x0006E468
		private static LazyAsyncResult.ThreadContext CurrentThreadContext
		{
			get
			{
				LazyAsyncResult.ThreadContext threadContext = LazyAsyncResult.t_ThreadContext;
				if (threadContext == null)
				{
					threadContext = new LazyAsyncResult.ThreadContext();
					LazyAsyncResult.t_ThreadContext = threadContext;
				}
				return threadContext;
			}
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x0006F48B File Offset: 0x0006E48B
		internal LazyAsyncResult(object myObject, object myState, AsyncCallback myCallBack)
		{
			this.m_AsyncObject = myObject;
			this.m_AsyncState = myState;
			this.m_AsyncCallback = myCallBack;
			this.m_Result = DBNull.Value;
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x0006F4B3 File Offset: 0x0006E4B3
		internal LazyAsyncResult(object myObject, object myState, AsyncCallback myCallBack, object result)
		{
			this.m_AsyncObject = myObject;
			this.m_AsyncState = myState;
			this.m_AsyncCallback = myCallBack;
			this.m_Result = result;
			this.m_IntCompleted = 1;
			if (this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(this);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x0006F4F3 File Offset: 0x0006E4F3
		internal object AsyncObject
		{
			get
			{
				return this.m_AsyncObject;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x0006F4FB File Offset: 0x0006E4FB
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x0006F503 File Offset: 0x0006E503
		// (set) Token: 0x06001D10 RID: 7440 RVA: 0x0006F50B File Offset: 0x0006E50B
		protected AsyncCallback AsyncCallback
		{
			get
			{
				return this.m_AsyncCallback;
			}
			set
			{
				this.m_AsyncCallback = value;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x0006F514 File Offset: 0x0006E514
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				this.m_UserEvent = true;
				if (this.m_IntCompleted == 0)
				{
					Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				while (manualResetEvent == null)
				{
					this.LazilyCreateEvent(out manualResetEvent);
				}
				return manualResetEvent;
			}
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x0006F560 File Offset: 0x0006E560
		private bool LazilyCreateEvent(out ManualResetEvent waitHandle)
		{
			waitHandle = new ManualResetEvent(false);
			bool result;
			try
			{
				if (Interlocked.CompareExchange(ref this.m_Event, waitHandle, null) == null)
				{
					if (this.InternalPeekCompleted)
					{
						waitHandle.Set();
					}
					result = true;
				}
				else
				{
					waitHandle.Close();
					waitHandle = (ManualResetEvent)this.m_Event;
					result = false;
				}
			}
			catch
			{
				this.m_Event = null;
				if (waitHandle != null)
				{
					waitHandle.Close();
				}
				throw;
			}
			return result;
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x0006F5D8 File Offset: 0x0006E5D8
		[Conditional("DEBUG")]
		protected void DebugProtectState(bool protect)
		{
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x0006F5DC File Offset: 0x0006E5DC
		public bool CompletedSynchronously
		{
			get
			{
				int num = this.m_IntCompleted;
				if (num == 0)
				{
					num = Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				return num > 0;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x0006F60C File Offset: 0x0006E60C
		public bool IsCompleted
		{
			get
			{
				int num = this.m_IntCompleted;
				if (num == 0)
				{
					num = Interlocked.CompareExchange(ref this.m_IntCompleted, int.MinValue, 0);
				}
				return (num & int.MaxValue) != 0;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x0006F642 File Offset: 0x0006E642
		internal bool InternalPeekCompleted
		{
			get
			{
				return (this.m_IntCompleted & int.MaxValue) != 0;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0006F656 File Offset: 0x0006E656
		// (set) Token: 0x06001D18 RID: 7448 RVA: 0x0006F66D File Offset: 0x0006E66D
		internal object Result
		{
			get
			{
				if (this.m_Result != DBNull.Value)
				{
					return this.m_Result;
				}
				return null;
			}
			set
			{
				this.m_Result = value;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0006F676 File Offset: 0x0006E676
		// (set) Token: 0x06001D1A RID: 7450 RVA: 0x0006F67E File Offset: 0x0006E67E
		internal bool EndCalled
		{
			get
			{
				return this.m_EndCalled;
			}
			set
			{
				this.m_EndCalled = value;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x0006F687 File Offset: 0x0006E687
		// (set) Token: 0x06001D1C RID: 7452 RVA: 0x0006F68F File Offset: 0x0006E68F
		internal int ErrorCode
		{
			get
			{
				return this.m_ErrorCode;
			}
			set
			{
				this.m_ErrorCode = value;
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x0006F698 File Offset: 0x0006E698
		protected void ProtectedInvokeCallback(object result, IntPtr userToken)
		{
			if (result == DBNull.Value)
			{
				throw new ArgumentNullException("result");
			}
			if ((this.m_IntCompleted & 2147483647) == 0 && (Interlocked.Increment(ref this.m_IntCompleted) & 2147483647) == 1)
			{
				if (this.m_Result == DBNull.Value)
				{
					this.m_Result = result;
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				if (manualResetEvent != null)
				{
					try
					{
						manualResetEvent.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				this.Complete(userToken);
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x0006F720 File Offset: 0x0006E720
		internal void InvokeCallback(object result)
		{
			this.ProtectedInvokeCallback(result, IntPtr.Zero);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x0006F72E File Offset: 0x0006E72E
		internal void InvokeCallback()
		{
			this.ProtectedInvokeCallback(null, IntPtr.Zero);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x0006F73C File Offset: 0x0006E73C
		protected virtual void Complete(IntPtr userToken)
		{
			bool flag = false;
			LazyAsyncResult.ThreadContext currentThreadContext = LazyAsyncResult.CurrentThreadContext;
			try
			{
				currentThreadContext.m_NestedIOCount++;
				if (this.m_AsyncCallback != null)
				{
					if (currentThreadContext.m_NestedIOCount >= 50)
					{
						ThreadPool.QueueUserWorkItem(new WaitCallback(this.WorkerThreadComplete));
						flag = true;
					}
					else
					{
						this.m_AsyncCallback(this);
					}
				}
			}
			finally
			{
				currentThreadContext.m_NestedIOCount--;
				if (!flag)
				{
					this.Cleanup();
				}
			}
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x0006F7C0 File Offset: 0x0006E7C0
		private void WorkerThreadComplete(object state)
		{
			try
			{
				this.m_AsyncCallback(this);
			}
			finally
			{
				this.Cleanup();
			}
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x0006F7F4 File Offset: 0x0006E7F4
		protected virtual void Cleanup()
		{
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x0006F7F6 File Offset: 0x0006E7F6
		internal object InternalWaitForCompletion()
		{
			return this.WaitForCompletion(true);
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x0006F800 File Offset: 0x0006E800
		private object WaitForCompletion(bool snap)
		{
			ManualResetEvent manualResetEvent = null;
			bool flag = false;
			if (!(snap ? this.IsCompleted : this.InternalPeekCompleted))
			{
				manualResetEvent = (ManualResetEvent)this.m_Event;
				if (manualResetEvent == null)
				{
					flag = this.LazilyCreateEvent(out manualResetEvent);
				}
			}
			if (manualResetEvent == null)
			{
				goto IL_77;
			}
			try
			{
				try
				{
					manualResetEvent.WaitOne(-1, false);
				}
				catch (ObjectDisposedException)
				{
				}
				goto IL_77;
			}
			finally
			{
				if (flag && !this.m_UserEvent)
				{
					ManualResetEvent manualResetEvent2 = (ManualResetEvent)this.m_Event;
					this.m_Event = null;
					if (!this.m_UserEvent)
					{
						manualResetEvent2.Close();
					}
				}
			}
			IL_71:
			Thread.SpinWait(1);
			IL_77:
			if (this.m_Result != DBNull.Value)
			{
				return this.m_Result;
			}
			goto IL_71;
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x0006F8B4 File Offset: 0x0006E8B4
		internal void InternalCleanup()
		{
			if ((this.m_IntCompleted & 2147483647) == 0 && (Interlocked.Increment(ref this.m_IntCompleted) & 2147483647) == 1)
			{
				this.m_Result = null;
				this.Cleanup();
			}
		}

		// Token: 0x04001D57 RID: 7511
		private const int c_HighBit = -2147483648;

		// Token: 0x04001D58 RID: 7512
		private const int c_ForceAsyncCount = 50;

		// Token: 0x04001D59 RID: 7513
		[ThreadStatic]
		private static LazyAsyncResult.ThreadContext t_ThreadContext;

		// Token: 0x04001D5A RID: 7514
		private object m_AsyncObject;

		// Token: 0x04001D5B RID: 7515
		private object m_AsyncState;

		// Token: 0x04001D5C RID: 7516
		private AsyncCallback m_AsyncCallback;

		// Token: 0x04001D5D RID: 7517
		private object m_Result;

		// Token: 0x04001D5E RID: 7518
		private int m_ErrorCode;

		// Token: 0x04001D5F RID: 7519
		private int m_IntCompleted;

		// Token: 0x04001D60 RID: 7520
		private bool m_EndCalled;

		// Token: 0x04001D61 RID: 7521
		private bool m_UserEvent;

		// Token: 0x04001D62 RID: 7522
		private object m_Event;

		// Token: 0x020003A1 RID: 929
		private class ThreadContext
		{
			// Token: 0x04001D63 RID: 7523
			internal int m_NestedIOCount;
		}
	}
}
