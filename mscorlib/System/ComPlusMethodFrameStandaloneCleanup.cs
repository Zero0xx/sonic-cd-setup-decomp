using System;

namespace System
{
	// Token: 0x02000089 RID: 137
	internal struct ComPlusMethodFrameStandaloneCleanup
	{
		// Token: 0x04000289 RID: 649
		private UIntPtr _gsCookie;

		// Token: 0x0400028A RID: 650
		private IntPtr _CleanupWorkList;

		// Token: 0x0400028B RID: 651
		private IntPtr _cbArgs_FPRet;

		// Token: 0x0400028C RID: 652
		private IntPtr _MarkForHostCall;

		// Token: 0x0400028D RID: 653
		private IntPtr _FrameVPtr;

		// Token: 0x0400028E RID: 654
		private IntPtr _Next;

		// Token: 0x0400028F RID: 655
		private IntPtr _rsp;

		// Token: 0x04000290 RID: 656
		private IntPtr _rdi;

		// Token: 0x04000291 RID: 657
		private IntPtr _rsi;

		// Token: 0x04000292 RID: 658
		private IntPtr _rbx;

		// Token: 0x04000293 RID: 659
		private IntPtr _rbp;

		// Token: 0x04000294 RID: 660
		private IntPtr _r12;

		// Token: 0x04000295 RID: 661
		private IntPtr _r13;

		// Token: 0x04000296 RID: 662
		private IntPtr _r14;

		// Token: 0x04000297 RID: 663
		private IntPtr _r15;

		// Token: 0x04000298 RID: 664
		private IntPtr _Datum;

		// Token: 0x04000299 RID: 665
		private IntPtr _ReturnAddress;
	}
}
