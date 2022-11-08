using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019C RID: 412
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a504e5b0-8ccf-4cb4-9902-c9d1b9abd033")]
	[ComImport]
	internal interface ICMS
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600143B RID: 5179
		IDefinitionIdentity Identity { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600143C RID: 5180
		ISection FileSection { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600143D RID: 5181
		ISection CategoryMembershipSection { get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600143E RID: 5182
		ISection COMRedirectionSection { get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600143F RID: 5183
		ISection ProgIdRedirectionSection { get; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001440 RID: 5184
		ISection CLRSurrogateSection { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06001441 RID: 5185
		ISection AssemblyReferenceSection { get; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001442 RID: 5186
		ISection WindowClassSection { get; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001443 RID: 5187
		ISection StringSection { get; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001444 RID: 5188
		ISection EntryPointSection { get; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001445 RID: 5189
		ISection PermissionSetSection { get; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001446 RID: 5190
		ISectionEntry MetadataSectionEntry { get; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001447 RID: 5191
		ISection AssemblyRequestSection { get; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001448 RID: 5192
		ISection RegistryKeySection { get; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001449 RID: 5193
		ISection DirectorySection { get; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600144A RID: 5194
		ISection FileAssociationSection { get; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600144B RID: 5195
		ISection EventSection { get; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600144C RID: 5196
		ISection EventMapSection { get; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600144D RID: 5197
		ISection EventTagSection { get; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600144E RID: 5198
		ISection CounterSetSection { get; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600144F RID: 5199
		ISection CounterSection { get; }
	}
}
