using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000172 RID: 370
	[ComVisible(true)]
	public sealed class RegisteredWaitHandle : MarshalByRefObject
	{
		// Token: 0x060013B9 RID: 5049 RVA: 0x000358DC File Offset: 0x000348DC
		internal RegisteredWaitHandle()
		{
			this.internalRegisteredWait = new RegisteredWaitHandleSafe();
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x000358EF File Offset: 0x000348EF
		internal void SetHandle(IntPtr handle)
		{
			this.internalRegisteredWait.SetHandle(handle);
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x000358FD File Offset: 0x000348FD
		internal void SetWaitObject(WaitHandle waitObject)
		{
			this.internalRegisteredWait.SetWaitObject(waitObject);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0003590B File Offset: 0x0003490B
		[ComVisible(true)]
		public bool Unregister(WaitHandle waitObject)
		{
			return this.internalRegisteredWait.Unregister(waitObject);
		}

		// Token: 0x040006BD RID: 1725
		private RegisteredWaitHandleSafe internalRegisteredWait;
	}
}
