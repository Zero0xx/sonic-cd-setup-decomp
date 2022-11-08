using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006A3 RID: 1699
	internal class RemotePropertyHolderAttribute : IContextAttribute
	{
		// Token: 0x06003D67 RID: 15719 RVA: 0x000D21C9 File Offset: 0x000D11C9
		internal RemotePropertyHolderAttribute(IList cp)
		{
			this._cp = cp;
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x000D21D8 File Offset: 0x000D11D8
		[ComVisible(true)]
		public virtual bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return false;
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x000D21DC File Offset: 0x000D11DC
		[ComVisible(true)]
		public virtual void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			for (int i = 0; i < this._cp.Count; i++)
			{
				ctorMsg.ContextProperties.Add(this._cp[i]);
			}
		}

		// Token: 0x04001F6B RID: 8043
		private IList _cp;
	}
}
