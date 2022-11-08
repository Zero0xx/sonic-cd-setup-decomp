﻿using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x020000C0 RID: 192
	[ComVisible(true)]
	public interface IAsyncResult
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000AFB RID: 2811
		bool IsCompleted { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000AFC RID: 2812
		WaitHandle AsyncWaitHandle { get; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000AFD RID: 2813
		object AsyncState { get; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000AFE RID: 2814
		bool CompletedSynchronously { get; }
	}
}
