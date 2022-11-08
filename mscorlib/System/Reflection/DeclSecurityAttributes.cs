using System;

namespace System.Reflection
{
	// Token: 0x0200031B RID: 795
	[Flags]
	[Serializable]
	internal enum DeclSecurityAttributes
	{
		// Token: 0x04000BD5 RID: 3029
		ActionMask = 31,
		// Token: 0x04000BD6 RID: 3030
		ActionNil = 0,
		// Token: 0x04000BD7 RID: 3031
		Request = 1,
		// Token: 0x04000BD8 RID: 3032
		Demand = 2,
		// Token: 0x04000BD9 RID: 3033
		Assert = 3,
		// Token: 0x04000BDA RID: 3034
		Deny = 4,
		// Token: 0x04000BDB RID: 3035
		PermitOnly = 5,
		// Token: 0x04000BDC RID: 3036
		LinktimeCheck = 6,
		// Token: 0x04000BDD RID: 3037
		InheritanceCheck = 7,
		// Token: 0x04000BDE RID: 3038
		RequestMinimum = 8,
		// Token: 0x04000BDF RID: 3039
		RequestOptional = 9,
		// Token: 0x04000BE0 RID: 3040
		RequestRefuse = 10,
		// Token: 0x04000BE1 RID: 3041
		PrejitGrant = 11,
		// Token: 0x04000BE2 RID: 3042
		PrejitDenied = 12,
		// Token: 0x04000BE3 RID: 3043
		NonCasDemand = 13,
		// Token: 0x04000BE4 RID: 3044
		NonCasLinkDemand = 14,
		// Token: 0x04000BE5 RID: 3045
		NonCasInheritance = 15,
		// Token: 0x04000BE6 RID: 3046
		MaximumValue = 15
	}
}
