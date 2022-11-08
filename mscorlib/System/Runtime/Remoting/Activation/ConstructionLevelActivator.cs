using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006A2 RID: 1698
	[Serializable]
	internal class ConstructionLevelActivator : IActivator
	{
		// Token: 0x06003D62 RID: 15714 RVA: 0x000D219B File Offset: 0x000D119B
		internal ConstructionLevelActivator()
		{
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x000D21A3 File Offset: 0x000D11A3
		// (set) Token: 0x06003D64 RID: 15716 RVA: 0x000D21A6 File Offset: 0x000D11A6
		public virtual IActivator NextActivator
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06003D65 RID: 15717 RVA: 0x000D21AD File Offset: 0x000D11AD
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.Construction;
			}
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x000D21B0 File Offset: 0x000D11B0
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.Activator = ctorMsg.Activator.NextActivator;
			return ActivationServices.DoServerContextActivation(ctorMsg);
		}
	}
}
