using System;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Threading
{
	// Token: 0x02000150 RID: 336
	internal struct ExecutionContextSwitcher : IDisposable
	{
		// Token: 0x0600122A RID: 4650 RVA: 0x00032934 File Offset: 0x00031934
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is ExecutionContextSwitcher))
			{
				return false;
			}
			ExecutionContextSwitcher executionContextSwitcher = (ExecutionContextSwitcher)obj;
			return this.prevEC == executionContextSwitcher.prevEC && this.currEC == executionContextSwitcher.currEC && this.scsw == executionContextSwitcher.scsw && this.sysw == executionContextSwitcher.sysw && this.hecsw == executionContextSwitcher.hecsw && this.thread == executionContextSwitcher.thread;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000329BB File Offset: 0x000319BB
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000329CE File Offset: 0x000319CE
		public static bool operator ==(ExecutionContextSwitcher c1, ExecutionContextSwitcher c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000329E3 File Offset: 0x000319E3
		public static bool operator !=(ExecutionContextSwitcher c1, ExecutionContextSwitcher c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000329FB File Offset: 0x000319FB
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00032A04 File Offset: 0x00031A04
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool UndoNoThrow()
		{
			try
			{
				this.Undo();
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00032A34 File Offset: 0x00031A34
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Undo()
		{
			if (this.thread == null)
			{
				return;
			}
			if (this.thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotUseSwitcherOtherThread"));
			}
			if (this.currEC != Thread.CurrentThread.GetExecutionContextNoCreate())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			this.scsw.Undo();
			try
			{
				HostExecutionContextSwitcher.Undo(this.hecsw);
			}
			finally
			{
				this.sysw.Undo();
			}
			Thread.CurrentThread.SetExecutionContext(this.prevEC);
			this.thread = null;
		}

		// Token: 0x0400063F RID: 1599
		internal ExecutionContext prevEC;

		// Token: 0x04000640 RID: 1600
		internal ExecutionContext currEC;

		// Token: 0x04000641 RID: 1601
		internal SecurityContextSwitcher scsw;

		// Token: 0x04000642 RID: 1602
		internal SynchronizationContextSwitcher sysw;

		// Token: 0x04000643 RID: 1603
		internal object hecsw;

		// Token: 0x04000644 RID: 1604
		internal Thread thread;
	}
}
