using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	// Token: 0x02000143 RID: 323
	internal struct SynchronizationContextSwitcher : IDisposable
	{
		// Token: 0x060011C8 RID: 4552 RVA: 0x00031FE4 File Offset: 0x00030FE4
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is SynchronizationContextSwitcher))
			{
				return false;
			}
			SynchronizationContextSwitcher synchronizationContextSwitcher = (SynchronizationContextSwitcher)obj;
			return this.savedSC == synchronizationContextSwitcher.savedSC && this.currSC == synchronizationContextSwitcher.currSC && this._ec == synchronizationContextSwitcher._ec;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00032034 File Offset: 0x00031034
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00032047 File Offset: 0x00031047
		public static bool operator ==(SynchronizationContextSwitcher c1, SynchronizationContextSwitcher c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0003205C File Offset: 0x0003105C
		public static bool operator !=(SynchronizationContextSwitcher c1, SynchronizationContextSwitcher c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00032074 File Offset: 0x00031074
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0003207C File Offset: 0x0003107C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal bool UndoNoThrow()
		{
			if (this._ec == null)
			{
				return true;
			}
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

		// Token: 0x060011CE RID: 4558 RVA: 0x000320B4 File Offset: 0x000310B4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Undo()
		{
			if (this._ec == null)
			{
				return;
			}
			ExecutionContext executionContextNoCreate = Thread.CurrentThread.GetExecutionContextNoCreate();
			if (this._ec != executionContextNoCreate)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			if (this.currSC != this._ec.SynchronizationContext)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SwitcherCtxMismatch"));
			}
			executionContextNoCreate.SynchronizationContext = this.savedSC;
			this._ec = null;
		}

		// Token: 0x0400061E RID: 1566
		internal SynchronizationContext savedSC;

		// Token: 0x0400061F RID: 1567
		internal SynchronizationContext currSC;

		// Token: 0x04000620 RID: 1568
		internal ExecutionContext _ec;
	}
}
