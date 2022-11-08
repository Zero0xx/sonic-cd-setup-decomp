using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x02000739 RID: 1849
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ProxyAttribute : Attribute, IContextAttribute
	{
		// Token: 0x06004239 RID: 16953 RVA: 0x000E140F File Offset: 0x000E040F
		public virtual MarshalByRefObject CreateInstance(Type serverType)
		{
			if (!serverType.IsContextful)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Activation_MBR_ProxyAttribute"));
			}
			if (serverType.IsAbstract)
			{
				throw new RemotingException(Environment.GetResourceString("Acc_CreateAbst"));
			}
			return this.CreateInstanceInternal(serverType);
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x000E1448 File Offset: 0x000E0448
		internal MarshalByRefObject CreateInstanceInternal(Type serverType)
		{
			return ActivationServices.CreateInstance(serverType);
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x000E1450 File Offset: 0x000E0450
		public virtual RealProxy CreateProxy(ObjRef objRef, Type serverType, object serverObject, Context serverContext)
		{
			RemotingProxy remotingProxy = new RemotingProxy(serverType);
			if (serverContext != null)
			{
				RealProxy.SetStubData(remotingProxy, serverContext.InternalContextID);
			}
			if (objRef != null && objRef.GetServerIdentity().IsAllocated)
			{
				remotingProxy.SetSrvInfo(objRef.GetServerIdentity(), objRef.GetDomainID());
			}
			remotingProxy.Initialized = true;
			if (!serverType.IsContextful && !serverType.IsMarshalByRef && serverContext != null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Activation_MBR_ProxyAttribute"));
			}
			return remotingProxy;
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x000E14CD File Offset: 0x000E04CD
		[ComVisible(true)]
		public bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return true;
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x000E14D0 File Offset: 0x000E04D0
		[ComVisible(true)]
		public void GetPropertiesForNewContext(IConstructionCallMessage msg)
		{
		}
	}
}
