using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006A0 RID: 1696
	internal class AppDomainLevelActivator : IActivator
	{
		// Token: 0x06003D56 RID: 15702 RVA: 0x000D20B6 File Offset: 0x000D10B6
		internal AppDomainLevelActivator(string remActivatorURL)
		{
			this.m_RemActivatorURL = remActivatorURL;
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x000D20C5 File Offset: 0x000D10C5
		internal AppDomainLevelActivator(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_NextActivator = (IActivator)info.GetValue("m_NextActivator", typeof(IActivator));
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06003D58 RID: 15704 RVA: 0x000D20FB File Offset: 0x000D10FB
		// (set) Token: 0x06003D59 RID: 15705 RVA: 0x000D2103 File Offset: 0x000D1103
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

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06003D5A RID: 15706 RVA: 0x000D210C File Offset: 0x000D110C
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.AppDomain;
			}
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x000D2110 File Offset: 0x000D1110
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			ctorMsg.Activator = this.m_NextActivator;
			return ActivationServices.GetActivator().Activate(ctorMsg);
		}

		// Token: 0x04001F68 RID: 8040
		private IActivator m_NextActivator;

		// Token: 0x04001F69 RID: 8041
		private string m_RemActivatorURL;
	}
}
