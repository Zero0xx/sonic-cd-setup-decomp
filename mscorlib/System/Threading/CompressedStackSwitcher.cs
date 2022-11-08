using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	// Token: 0x02000147 RID: 327
	internal struct CompressedStackSwitcher : IDisposable
	{
		// Token: 0x060011E1 RID: 4577 RVA: 0x0003224C File Offset: 0x0003124C
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is CompressedStackSwitcher))
			{
				return false;
			}
			CompressedStackSwitcher compressedStackSwitcher = (CompressedStackSwitcher)obj;
			return this.curr_CS == compressedStackSwitcher.curr_CS && this.prev_CS == compressedStackSwitcher.prev_CS && this.prev_ADStack == compressedStackSwitcher.prev_ADStack;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0003229F File Offset: 0x0003129F
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x000322B2 File Offset: 0x000312B2
		public static bool operator ==(CompressedStackSwitcher c1, CompressedStackSwitcher c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x000322C7 File Offset: 0x000312C7
		public static bool operator !=(CompressedStackSwitcher c1, CompressedStackSwitcher c2)
		{
			return !c1.Equals(c2);
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000322DF File Offset: 0x000312DF
		void IDisposable.Dispose()
		{
			this.Undo();
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x000322E8 File Offset: 0x000312E8
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

		// Token: 0x060011E7 RID: 4583 RVA: 0x00032318 File Offset: 0x00031318
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Undo()
		{
			if (this.curr_CS == null && this.prev_CS == null)
			{
				return;
			}
			if (this.prev_ADStack != (IntPtr)0)
			{
				CompressedStack.RestoreAppDomainStack(this.prev_ADStack);
			}
			CompressedStack.SetCompressedStackThread(this.prev_CS);
			this.prev_CS = null;
			this.curr_CS = null;
			this.prev_ADStack = (IntPtr)0;
		}

		// Token: 0x04000625 RID: 1573
		internal CompressedStack curr_CS;

		// Token: 0x04000626 RID: 1574
		internal CompressedStack prev_CS;

		// Token: 0x04000627 RID: 1575
		internal IntPtr prev_ADStack;
	}
}
