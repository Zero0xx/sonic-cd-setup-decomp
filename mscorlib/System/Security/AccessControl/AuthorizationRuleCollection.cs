using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x02000909 RID: 2313
	public sealed class AuthorizationRuleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060053BE RID: 21438 RVA: 0x0012DF78 File Offset: 0x0012CF78
		internal AuthorizationRuleCollection()
		{
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x0012DF80 File Offset: 0x0012CF80
		internal void AddRule(AuthorizationRule rule)
		{
			base.InnerList.Add(rule);
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x0012DF8F File Offset: 0x0012CF8F
		public void CopyTo(AuthorizationRule[] rules, int index)
		{
			((ICollection)this).CopyTo(rules, index);
		}

		// Token: 0x17000E91 RID: 3729
		public AuthorizationRule this[int index]
		{
			get
			{
				return base.InnerList[index] as AuthorizationRule;
			}
		}
	}
}
