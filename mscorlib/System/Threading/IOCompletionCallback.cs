using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000177 RID: 375
	// (Invoke) Token: 0x060013D1 RID: 5073
	[ComVisible(true)]
	[CLSCompliant(false)]
	public unsafe delegate void IOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* pOVERLAP);
}
