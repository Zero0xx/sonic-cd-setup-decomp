using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Threading
{
	// Token: 0x0200014C RID: 332
	[Serializable]
	internal sealed class DomainCompressedStack
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600121E RID: 4638 RVA: 0x000328F1 File Offset: 0x000318F1
		internal PermissionListSet PLS
		{
			get
			{
				return this.m_pls;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x000328F9 File Offset: 0x000318F9
		internal bool ConstructionHalted
		{
			get
			{
				return this.m_bHaltConstruction;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00032904 File Offset: 0x00031904
		private static DomainCompressedStack CreateManagedObject(IntPtr unmanagedDCS)
		{
			DomainCompressedStack domainCompressedStack = new DomainCompressedStack();
			domainCompressedStack.m_pls = PermissionListSet.CreateCompressedState(unmanagedDCS, out domainCompressedStack.m_bHaltConstruction);
			return domainCompressedStack;
		}

		// Token: 0x06001221 RID: 4641
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetDescCount(IntPtr dcs);

		// Token: 0x06001222 RID: 4642
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetDomainPermissionSets(IntPtr dcs, out PermissionSet granted, out PermissionSet refused);

		// Token: 0x06001223 RID: 4643
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetDescriptorInfo(IntPtr dcs, int index, out PermissionSet granted, out PermissionSet refused, out Assembly assembly, out FrameSecurityDescriptor fsd);

		// Token: 0x06001224 RID: 4644
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IgnoreDomain(IntPtr dcs);

		// Token: 0x04000635 RID: 1589
		private PermissionListSet m_pls;

		// Token: 0x04000636 RID: 1590
		private bool m_bHaltConstruction;
	}
}
