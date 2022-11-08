using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008CB RID: 2251
	public abstract class KnownAce : GenericAce
	{
		// Token: 0x060051E9 RID: 20969 RVA: 0x00126BB0 File Offset: 0x00125BB0
		internal KnownAce(AceType type, AceFlags flags, int accessMask, SecurityIdentifier securityIdentifier) : base(type, flags)
		{
			if (securityIdentifier == null)
			{
				throw new ArgumentNullException("securityIdentifier");
			}
			this.AccessMask = accessMask;
			this.SecurityIdentifier = securityIdentifier;
		}

		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x060051EA RID: 20970 RVA: 0x00126BDE File Offset: 0x00125BDE
		// (set) Token: 0x060051EB RID: 20971 RVA: 0x00126BE6 File Offset: 0x00125BE6
		public int AccessMask
		{
			get
			{
				return this._accessMask;
			}
			set
			{
				this._accessMask = value;
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x060051EC RID: 20972 RVA: 0x00126BEF File Offset: 0x00125BEF
		// (set) Token: 0x060051ED RID: 20973 RVA: 0x00126BF7 File Offset: 0x00125BF7
		public SecurityIdentifier SecurityIdentifier
		{
			get
			{
				return this._sid;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._sid = value;
			}
		}

		// Token: 0x04002A5C RID: 10844
		internal const int AccessMaskLength = 4;

		// Token: 0x04002A5D RID: 10845
		private int _accessMask;

		// Token: 0x04002A5E RID: 10846
		private SecurityIdentifier _sid;
	}
}
