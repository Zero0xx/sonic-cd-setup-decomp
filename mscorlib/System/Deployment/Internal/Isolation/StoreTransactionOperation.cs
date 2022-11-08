using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200022F RID: 559
	internal struct StoreTransactionOperation
	{
		// Token: 0x040008FE RID: 2302
		[MarshalAs(UnmanagedType.U4)]
		public StoreTransactionOperationType Operation;

		// Token: 0x040008FF RID: 2303
		public StoreTransactionData Data;
	}
}
