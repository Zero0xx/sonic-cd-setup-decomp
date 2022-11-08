using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000632 RID: 1586
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class HostProtectionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600392D RID: 14637 RVA: 0x000C131A File Offset: 0x000C031A
		public HostProtectionAttribute() : base(SecurityAction.LinkDemand)
		{
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x000C1323 File Offset: 0x000C0323
		public HostProtectionAttribute(SecurityAction action) : base(action)
		{
			if (action != SecurityAction.LinkDemand)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"));
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600392F RID: 14639 RVA: 0x000C1340 File Offset: 0x000C0340
		// (set) Token: 0x06003930 RID: 14640 RVA: 0x000C1348 File Offset: 0x000C0348
		public HostProtectionResource Resources
		{
			get
			{
				return this.m_resources;
			}
			set
			{
				this.m_resources = value;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06003931 RID: 14641 RVA: 0x000C1351 File Offset: 0x000C0351
		// (set) Token: 0x06003932 RID: 14642 RVA: 0x000C1361 File Offset: 0x000C0361
		public bool Synchronization
		{
			get
			{
				return (this.m_resources & HostProtectionResource.Synchronization) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.Synchronization) : (this.m_resources & ~HostProtectionResource.Synchronization));
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06003933 RID: 14643 RVA: 0x000C137F File Offset: 0x000C037F
		// (set) Token: 0x06003934 RID: 14644 RVA: 0x000C138F File Offset: 0x000C038F
		public bool SharedState
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SharedState) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SharedState) : (this.m_resources & ~HostProtectionResource.SharedState));
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003935 RID: 14645 RVA: 0x000C13AD File Offset: 0x000C03AD
		// (set) Token: 0x06003936 RID: 14646 RVA: 0x000C13BD File Offset: 0x000C03BD
		public bool ExternalProcessMgmt
		{
			get
			{
				return (this.m_resources & HostProtectionResource.ExternalProcessMgmt) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.ExternalProcessMgmt) : (this.m_resources & ~HostProtectionResource.ExternalProcessMgmt));
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06003937 RID: 14647 RVA: 0x000C13DB File Offset: 0x000C03DB
		// (set) Token: 0x06003938 RID: 14648 RVA: 0x000C13EB File Offset: 0x000C03EB
		public bool SelfAffectingProcessMgmt
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SelfAffectingProcessMgmt) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SelfAffectingProcessMgmt) : (this.m_resources & ~HostProtectionResource.SelfAffectingProcessMgmt));
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003939 RID: 14649 RVA: 0x000C1409 File Offset: 0x000C0409
		// (set) Token: 0x0600393A RID: 14650 RVA: 0x000C141A File Offset: 0x000C041A
		public bool ExternalThreading
		{
			get
			{
				return (this.m_resources & HostProtectionResource.ExternalThreading) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.ExternalThreading) : (this.m_resources & ~HostProtectionResource.ExternalThreading));
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600393B RID: 14651 RVA: 0x000C1439 File Offset: 0x000C0439
		// (set) Token: 0x0600393C RID: 14652 RVA: 0x000C144A File Offset: 0x000C044A
		public bool SelfAffectingThreading
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SelfAffectingThreading) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SelfAffectingThreading) : (this.m_resources & ~HostProtectionResource.SelfAffectingThreading));
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x000C1469 File Offset: 0x000C0469
		// (set) Token: 0x0600393E RID: 14654 RVA: 0x000C147A File Offset: 0x000C047A
		[ComVisible(true)]
		public bool SecurityInfrastructure
		{
			get
			{
				return (this.m_resources & HostProtectionResource.SecurityInfrastructure) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.SecurityInfrastructure) : (this.m_resources & ~HostProtectionResource.SecurityInfrastructure));
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x0600393F RID: 14655 RVA: 0x000C1499 File Offset: 0x000C0499
		// (set) Token: 0x06003940 RID: 14656 RVA: 0x000C14AD File Offset: 0x000C04AD
		public bool UI
		{
			get
			{
				return (this.m_resources & HostProtectionResource.UI) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.UI) : (this.m_resources & ~HostProtectionResource.UI));
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06003941 RID: 14657 RVA: 0x000C14D2 File Offset: 0x000C04D2
		// (set) Token: 0x06003942 RID: 14658 RVA: 0x000C14E6 File Offset: 0x000C04E6
		public bool MayLeakOnAbort
		{
			get
			{
				return (this.m_resources & HostProtectionResource.MayLeakOnAbort) != HostProtectionResource.None;
			}
			set
			{
				this.m_resources = (value ? (this.m_resources | HostProtectionResource.MayLeakOnAbort) : (this.m_resources & ~HostProtectionResource.MayLeakOnAbort));
			}
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x000C150B File Offset: 0x000C050B
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new HostProtectionPermission(PermissionState.Unrestricted);
			}
			return new HostProtectionPermission(this.m_resources);
		}

		// Token: 0x04001DAF RID: 7599
		private HostProtectionResource m_resources;
	}
}
