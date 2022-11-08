using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000159 RID: 345
	internal class IUnknownSafeHandle : SafeHandle
	{
		// Token: 0x0600127B RID: 4731 RVA: 0x0003337C File Offset: 0x0003237C
		public IUnknownSafeHandle() : base(IntPtr.Zero, true)
		{
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x0003338A File Offset: 0x0003238A
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0003339C File Offset: 0x0003239C
		protected override bool ReleaseHandle()
		{
			HostExecutionContextManager.ReleaseHostSecurityContext(this.handle);
			return true;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000333AC File Offset: 0x000323AC
		internal object Clone()
		{
			IUnknownSafeHandle unknownSafeHandle = new IUnknownSafeHandle();
			if (!this.IsInvalid)
			{
				HostExecutionContextManager.CloneHostSecurityContext(this, unknownSafeHandle);
			}
			return unknownSafeHandle;
		}
	}
}
