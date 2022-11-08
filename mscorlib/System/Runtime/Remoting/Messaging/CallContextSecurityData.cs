using System;
using System.Security.Principal;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006AB RID: 1707
	[Serializable]
	internal class CallContextSecurityData : ICloneable
	{
		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x000D2E03 File Offset: 0x000D1E03
		// (set) Token: 0x06003DB4 RID: 15796 RVA: 0x000D2E0B File Offset: 0x000D1E0B
		internal IPrincipal Principal
		{
			get
			{
				return this._principal;
			}
			set
			{
				this._principal = value;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x000D2E14 File Offset: 0x000D1E14
		internal bool HasInfo
		{
			get
			{
				return null != this._principal;
			}
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x000D2E24 File Offset: 0x000D1E24
		public object Clone()
		{
			return new CallContextSecurityData
			{
				_principal = this._principal
			};
		}

		// Token: 0x04001F82 RID: 8066
		private IPrincipal _principal;
	}
}
