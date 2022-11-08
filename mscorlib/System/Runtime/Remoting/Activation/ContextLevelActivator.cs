using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006A1 RID: 1697
	[Serializable]
	internal class ContextLevelActivator : IActivator
	{
		// Token: 0x06003D5C RID: 15708 RVA: 0x000D2129 File Offset: 0x000D1129
		internal ContextLevelActivator()
		{
			this.m_NextActivator = null;
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x000D2138 File Offset: 0x000D1138
		internal ContextLevelActivator(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_NextActivator = (IActivator)info.GetValue("m_NextActivator", typeof(IActivator));
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003D5E RID: 15710 RVA: 0x000D216E File Offset: 0x000D116E
		// (set) Token: 0x06003D5F RID: 15711 RVA: 0x000D2176 File Offset: 0x000D1176
		public virtual IActivator NextActivator
		{
			get
			{
				return this.m_NextActivator;
			}
			set
			{
				this.m_NextActivator = value;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06003D60 RID: 15712 RVA: 0x000D217F File Offset: 0x000D117F
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.Context;
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x000D2182 File Offset: 0x000D1182
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.Activator = ctorMsg.Activator.NextActivator;
			return ActivationServices.DoCrossContextActivation(ctorMsg);
		}

		// Token: 0x04001F6A RID: 8042
		private IActivator m_NextActivator;
	}
}
