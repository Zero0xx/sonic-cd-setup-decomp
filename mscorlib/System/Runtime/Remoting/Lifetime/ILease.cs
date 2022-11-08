using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x02000702 RID: 1794
	[ComVisible(true)]
	public interface ILease
	{
		// Token: 0x06003FDC RID: 16348
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Register(ISponsor obj, TimeSpan renewalTime);

		// Token: 0x06003FDD RID: 16349
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Register(ISponsor obj);

		// Token: 0x06003FDE RID: 16350
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Unregister(ISponsor obj);

		// Token: 0x06003FDF RID: 16351
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		TimeSpan Renew(TimeSpan renewalTime);

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06003FE0 RID: 16352
		// (set) Token: 0x06003FE1 RID: 16353
		TimeSpan RenewOnCallTime { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06003FE2 RID: 16354
		// (set) Token: 0x06003FE3 RID: 16355
		TimeSpan SponsorshipTimeout { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06003FE4 RID: 16356
		// (set) Token: 0x06003FE5 RID: 16357
		TimeSpan InitialLeaseTime { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] set; }

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06003FE6 RID: 16358
		TimeSpan CurrentLeaseTime { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003FE7 RID: 16359
		LeaseState CurrentState { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)] get; }
	}
}
