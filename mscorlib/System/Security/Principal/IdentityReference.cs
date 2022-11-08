using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x0200090F RID: 2319
	[ComVisible(false)]
	public abstract class IdentityReference
	{
		// Token: 0x0600540A RID: 21514 RVA: 0x0012EFE8 File Offset: 0x0012DFE8
		internal IdentityReference()
		{
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x0600540B RID: 21515
		public abstract string Value { get; }

		// Token: 0x0600540C RID: 21516
		public abstract bool IsValidTargetType(Type targetType);

		// Token: 0x0600540D RID: 21517
		public abstract IdentityReference Translate(Type targetType);

		// Token: 0x0600540E RID: 21518
		public abstract override bool Equals(object o);

		// Token: 0x0600540F RID: 21519
		public abstract override int GetHashCode();

		// Token: 0x06005410 RID: 21520
		public abstract override string ToString();

		// Token: 0x06005411 RID: 21521 RVA: 0x0012EFF0 File Offset: 0x0012DFF0
		public static bool operator ==(IdentityReference left, IdentityReference right)
		{
			return (left == null && right == null) || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x0012F018 File Offset: 0x0012E018
		public static bool operator !=(IdentityReference left, IdentityReference right)
		{
			return !(left == right);
		}
	}
}
