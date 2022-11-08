using System;

namespace System.Reflection
{
	// Token: 0x0200030F RID: 783
	[Flags]
	public enum GenericParameterAttributes
	{
		// Token: 0x04000B63 RID: 2915
		None = 0,
		// Token: 0x04000B64 RID: 2916
		VarianceMask = 3,
		// Token: 0x04000B65 RID: 2917
		Covariant = 1,
		// Token: 0x04000B66 RID: 2918
		Contravariant = 2,
		// Token: 0x04000B67 RID: 2919
		SpecialConstraintMask = 28,
		// Token: 0x04000B68 RID: 2920
		ReferenceTypeConstraint = 4,
		// Token: 0x04000B69 RID: 2921
		NotNullableValueTypeConstraint = 8,
		// Token: 0x04000B6A RID: 2922
		DefaultConstructorConstraint = 16
	}
}
