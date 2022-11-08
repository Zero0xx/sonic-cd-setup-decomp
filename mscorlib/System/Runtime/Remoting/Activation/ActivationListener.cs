using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200069F RID: 1695
	internal class ActivationListener : MarshalByRefObject, IActivator
	{
		// Token: 0x06003D50 RID: 15696 RVA: 0x000D1FE8 File Offset: 0x000D0FE8
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06003D51 RID: 15697 RVA: 0x000D1FEB File Offset: 0x000D0FEB
		// (set) Token: 0x06003D52 RID: 15698 RVA: 0x000D1FEE File Offset: 0x000D0FEE
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

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x000D1FF5 File Offset: 0x000D0FF5
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.AppDomain;
			}
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x000D1FFC File Offset: 0x000D0FFC
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null || RemotingServices.IsTransparentProxy(ctorMsg))
			{
				throw new ArgumentNullException("ctorMsg");
			}
			ctorMsg.Properties["Permission"] = "allowed";
			string activationTypeName = ctorMsg.ActivationTypeName;
			if (!RemotingConfigHandler.IsActivationAllowed(activationTypeName))
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Activation_PermissionDenied"), new object[]
				{
					ctorMsg.ActivationTypeName
				}));
			}
			if (ctorMsg.ActivationType == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[]
				{
					ctorMsg.ActivationTypeName
				}));
			}
			return ActivationServices.GetActivator().Activate(ctorMsg);
		}
	}
}
