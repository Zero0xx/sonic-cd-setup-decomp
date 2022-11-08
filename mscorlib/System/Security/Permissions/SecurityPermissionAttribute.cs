using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000642 RID: 1602
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039BD RID: 14781 RVA: 0x000C2541 File Offset: 0x000C1541
		public SecurityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x000C254A File Offset: 0x000C154A
		// (set) Token: 0x060039BF RID: 14783 RVA: 0x000C2552 File Offset: 0x000C1552
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.m_flag;
			}
			set
			{
				this.m_flag = value;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x000C255B File Offset: 0x000C155B
		// (set) Token: 0x060039C1 RID: 14785 RVA: 0x000C256B File Offset: 0x000C156B
		public bool Assertion
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.Assertion) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.Assertion) : (this.m_flag & ~SecurityPermissionFlag.Assertion));
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060039C2 RID: 14786 RVA: 0x000C2589 File Offset: 0x000C1589
		// (set) Token: 0x060039C3 RID: 14787 RVA: 0x000C2599 File Offset: 0x000C1599
		public bool UnmanagedCode
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.UnmanagedCode) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.UnmanagedCode) : (this.m_flag & ~SecurityPermissionFlag.UnmanagedCode));
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x000C25B7 File Offset: 0x000C15B7
		// (set) Token: 0x060039C5 RID: 14789 RVA: 0x000C25C7 File Offset: 0x000C15C7
		public bool SkipVerification
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.SkipVerification) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.SkipVerification) : (this.m_flag & ~SecurityPermissionFlag.SkipVerification));
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x000C25E5 File Offset: 0x000C15E5
		// (set) Token: 0x060039C7 RID: 14791 RVA: 0x000C25F5 File Offset: 0x000C15F5
		public bool Execution
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.Execution) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.Execution) : (this.m_flag & ~SecurityPermissionFlag.Execution));
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x000C2613 File Offset: 0x000C1613
		// (set) Token: 0x060039C9 RID: 14793 RVA: 0x000C2624 File Offset: 0x000C1624
		public bool ControlThread
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlThread) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlThread) : (this.m_flag & ~SecurityPermissionFlag.ControlThread));
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x000C2643 File Offset: 0x000C1643
		// (set) Token: 0x060039CB RID: 14795 RVA: 0x000C2654 File Offset: 0x000C1654
		public bool ControlEvidence
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlEvidence) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlEvidence) : (this.m_flag & ~SecurityPermissionFlag.ControlEvidence));
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x000C2673 File Offset: 0x000C1673
		// (set) Token: 0x060039CD RID: 14797 RVA: 0x000C2684 File Offset: 0x000C1684
		public bool ControlPolicy
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlPolicy) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlPolicy) : (this.m_flag & ~SecurityPermissionFlag.ControlPolicy));
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x000C26A3 File Offset: 0x000C16A3
		// (set) Token: 0x060039CF RID: 14799 RVA: 0x000C26B7 File Offset: 0x000C16B7
		public bool SerializationFormatter
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.SerializationFormatter) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.SerializationFormatter) : (this.m_flag & ~SecurityPermissionFlag.SerializationFormatter));
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060039D0 RID: 14800 RVA: 0x000C26DC File Offset: 0x000C16DC
		// (set) Token: 0x060039D1 RID: 14801 RVA: 0x000C26F0 File Offset: 0x000C16F0
		public bool ControlDomainPolicy
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlDomainPolicy) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlDomainPolicy) : (this.m_flag & ~SecurityPermissionFlag.ControlDomainPolicy));
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x060039D2 RID: 14802 RVA: 0x000C2715 File Offset: 0x000C1715
		// (set) Token: 0x060039D3 RID: 14803 RVA: 0x000C2729 File Offset: 0x000C1729
		public bool ControlPrincipal
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlPrincipal) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlPrincipal) : (this.m_flag & ~SecurityPermissionFlag.ControlPrincipal));
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060039D4 RID: 14804 RVA: 0x000C274E File Offset: 0x000C174E
		// (set) Token: 0x060039D5 RID: 14805 RVA: 0x000C2762 File Offset: 0x000C1762
		public bool ControlAppDomain
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.ControlAppDomain) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.ControlAppDomain) : (this.m_flag & ~SecurityPermissionFlag.ControlAppDomain));
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060039D6 RID: 14806 RVA: 0x000C2787 File Offset: 0x000C1787
		// (set) Token: 0x060039D7 RID: 14807 RVA: 0x000C279B File Offset: 0x000C179B
		public bool RemotingConfiguration
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.RemotingConfiguration) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.RemotingConfiguration) : (this.m_flag & ~SecurityPermissionFlag.RemotingConfiguration));
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060039D8 RID: 14808 RVA: 0x000C27C0 File Offset: 0x000C17C0
		// (set) Token: 0x060039D9 RID: 14809 RVA: 0x000C27D4 File Offset: 0x000C17D4
		[ComVisible(true)]
		public bool Infrastructure
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.Infrastructure) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.Infrastructure) : (this.m_flag & ~SecurityPermissionFlag.Infrastructure));
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060039DA RID: 14810 RVA: 0x000C27F9 File Offset: 0x000C17F9
		// (set) Token: 0x060039DB RID: 14811 RVA: 0x000C280D File Offset: 0x000C180D
		public bool BindingRedirects
		{
			get
			{
				return (this.m_flag & SecurityPermissionFlag.BindingRedirects) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				this.m_flag = (value ? (this.m_flag | SecurityPermissionFlag.BindingRedirects) : (this.m_flag & ~SecurityPermissionFlag.BindingRedirects));
			}
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x000C2832 File Offset: 0x000C1832
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			return new SecurityPermission(this.m_flag);
		}

		// Token: 0x04001E12 RID: 7698
		private SecurityPermissionFlag m_flag;
	}
}
