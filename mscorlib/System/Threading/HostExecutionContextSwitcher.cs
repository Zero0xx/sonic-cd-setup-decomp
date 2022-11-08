using System;
using System.Runtime.ConstrainedExecution;

namespace System.Threading
{
	// Token: 0x02000157 RID: 343
	internal class HostExecutionContextSwitcher
	{
		// Token: 0x06001274 RID: 4724 RVA: 0x00033300 File Offset: 0x00032300
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void Undo(object switcherObject)
		{
			if (switcherObject == null)
			{
				return;
			}
			HostExecutionContextManager currentHostExecutionContextManager = HostExecutionContextManager.GetCurrentHostExecutionContextManager();
			if (currentHostExecutionContextManager != null)
			{
				currentHostExecutionContextManager.Revert(switcherObject);
			}
		}

		// Token: 0x04000657 RID: 1623
		internal ExecutionContext executionContext;

		// Token: 0x04000658 RID: 1624
		internal HostExecutionContext previousHostContext;

		// Token: 0x04000659 RID: 1625
		internal HostExecutionContext currentHostContext;
	}
}
