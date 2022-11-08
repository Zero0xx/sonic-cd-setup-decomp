using System;

namespace System.Threading
{
	// Token: 0x02000161 RID: 353
	internal class _IOCompletionCallback
	{
		// Token: 0x060012B2 RID: 4786 RVA: 0x00033DD1 File Offset: 0x00032DD1
		internal _IOCompletionCallback(IOCompletionCallback ioCompletionCallback, ref StackCrawlMark stackMark)
		{
			this._ioCompletionCallback = ioCompletionCallback;
			this._executionContext = ExecutionContext.Capture(ref stackMark);
			ExecutionContext.ClearSyncContext(this._executionContext);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00033DF8 File Offset: 0x00032DF8
		internal static void IOCompletionCallback_Context(object state)
		{
			_IOCompletionCallback iocompletionCallback = (_IOCompletionCallback)state;
			iocompletionCallback._ioCompletionCallback(iocompletionCallback._errorCode, iocompletionCallback._numBytes, iocompletionCallback._pOVERLAP);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00033E2C File Offset: 0x00032E2C
		internal unsafe static void PerformIOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* pOVERLAP)
		{
			do
			{
				Overlapped overlapped = OverlappedData.GetOverlappedFromNative(pOVERLAP).m_overlapped;
				_IOCompletionCallback iocbHelper = overlapped.iocbHelper;
				if (iocbHelper == null || iocbHelper._executionContext == null || iocbHelper._executionContext.IsDefaultFTContext())
				{
					IOCompletionCallback userCallback = overlapped.UserCallback;
					userCallback(errorCode, numBytes, pOVERLAP);
				}
				else
				{
					iocbHelper._errorCode = errorCode;
					iocbHelper._numBytes = numBytes;
					iocbHelper._pOVERLAP = pOVERLAP;
					ExecutionContext.Run(iocbHelper._executionContext.CreateCopy(), _IOCompletionCallback._ccb, iocbHelper);
				}
				OverlappedData.CheckVMForIOPacket(out pOVERLAP, out errorCode, out numBytes);
			}
			while (pOVERLAP != null);
		}

		// Token: 0x0400066D RID: 1645
		private IOCompletionCallback _ioCompletionCallback;

		// Token: 0x0400066E RID: 1646
		private ExecutionContext _executionContext;

		// Token: 0x0400066F RID: 1647
		private uint _errorCode;

		// Token: 0x04000670 RID: 1648
		private uint _numBytes;

		// Token: 0x04000671 RID: 1649
		private unsafe NativeOverlapped* _pOVERLAP;

		// Token: 0x04000672 RID: 1650
		internal static ContextCallback _ccb = new ContextCallback(_IOCompletionCallback.IOCompletionCallback_Context);
	}
}
