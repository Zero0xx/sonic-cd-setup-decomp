using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x02000151 RID: 337
	public struct AsyncFlowControl : IDisposable
	{
		// Token: 0x06001231 RID: 4657 RVA: 0x00032AD4 File Offset: 0x00031AD4
		internal void Setup(SecurityContextDisableFlow flags)
		{
			this.useEC = false;
			this._sc = Thread.CurrentThread.ExecutionContext.SecurityContext;
			this._sc._disableFlow = flags;
			this._thread = Thread.CurrentThread;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00032B09 File Offset: 0x00031B09
		internal void Setup()
		{
			this.useEC = true;
			this._ec = Thread.CurrentThread.ExecutionContext;
			this._ec.isFlowSuppressed = true;
			this._thread = Thread.CurrentThread;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00032B39 File Offset: 0x00031B39
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00032B44 File Offset: 0x00031B44
		public void Undo()
		{
			if (this._thread == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseAFCMultiple"));
			}
			if (this._thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseAFCOtherThread"));
			}
			if (this.useEC)
			{
				if (Thread.CurrentThread.ExecutionContext != this._ec)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AsyncFlowCtrlCtxMismatch"));
				}
				ExecutionContext.RestoreFlow();
			}
			else
			{
				if (Thread.CurrentThread.ExecutionContext.SecurityContext != this._sc)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AsyncFlowCtrlCtxMismatch"));
				}
				SecurityContext.RestoreFlow();
			}
			this._thread = null;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00032BEA File Offset: 0x00031BEA
		public override int GetHashCode()
		{
			if (this._thread != null)
			{
				return this._thread.GetHashCode();
			}
			return this.ToString().GetHashCode();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00032C11 File Offset: 0x00031C11
		public override bool Equals(object obj)
		{
			return obj is AsyncFlowControl && this.Equals((AsyncFlowControl)obj);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00032C29 File Offset: 0x00031C29
		public bool Equals(AsyncFlowControl obj)
		{
			return obj.useEC == this.useEC && obj._ec == this._ec && obj._sc == this._sc && obj._thread == this._thread;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00032C69 File Offset: 0x00031C69
		public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00032C73 File Offset: 0x00031C73
		public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b)
		{
			return !(a == b);
		}

		// Token: 0x04000645 RID: 1605
		private bool useEC;

		// Token: 0x04000646 RID: 1606
		private ExecutionContext _ec;

		// Token: 0x04000647 RID: 1607
		private SecurityContext _sc;

		// Token: 0x04000648 RID: 1608
		private Thread _thread;
	}
}
