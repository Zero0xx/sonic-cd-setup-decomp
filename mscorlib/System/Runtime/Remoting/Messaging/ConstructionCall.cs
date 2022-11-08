using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200071D RID: 1821
	[ComVisible(true)]
	[CLSCompliant(false)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class ConstructionCall : MethodCall, IConstructionCallMessage, IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x0600412D RID: 16685 RVA: 0x000DDFC7 File Offset: 0x000DCFC7
		public ConstructionCall(Header[] headers) : base(headers)
		{
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x000DDFD0 File Offset: 0x000DCFD0
		public ConstructionCall(IMessage m) : base(m)
		{
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x000DDFD9 File Offset: 0x000DCFD9
		internal ConstructionCall(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x000DDFE4 File Offset: 0x000DCFE4
		internal override bool FillSpecialHeader(string key, object value)
		{
			if (key != null)
			{
				if (key.Equals("__ActivationType"))
				{
					this._activationType = null;
				}
				else if (key.Equals("__ContextProperties"))
				{
					this._contextProperties = (IList)value;
				}
				else if (key.Equals("__CallSiteActivationAttributes"))
				{
					this._callSiteActivationAttributes = (object[])value;
				}
				else if (key.Equals("__Activator"))
				{
					this._activator = (IActivator)value;
				}
				else
				{
					if (!key.Equals("__ActivationTypeName"))
					{
						return base.FillSpecialHeader(key, value);
					}
					this._activationTypeName = (string)value;
				}
			}
			return true;
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004131 RID: 16689 RVA: 0x000DE083 File Offset: 0x000DD083
		public object[] CallSiteActivationAttributes
		{
			get
			{
				return this._callSiteActivationAttributes;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06004132 RID: 16690 RVA: 0x000DE08B File Offset: 0x000DD08B
		public Type ActivationType
		{
			get
			{
				if (this._activationType == null && this._activationTypeName != null)
				{
					this._activationType = RemotingServices.InternalGetTypeFromQualifiedTypeName(this._activationTypeName, false);
				}
				return this._activationType;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06004133 RID: 16691 RVA: 0x000DE0B5 File Offset: 0x000DD0B5
		public string ActivationTypeName
		{
			get
			{
				return this._activationTypeName;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004134 RID: 16692 RVA: 0x000DE0BD File Offset: 0x000DD0BD
		public IList ContextProperties
		{
			get
			{
				if (this._contextProperties == null)
				{
					this._contextProperties = new ArrayList();
				}
				return this._contextProperties;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004135 RID: 16693 RVA: 0x000DE0D8 File Offset: 0x000DD0D8
		public override IDictionary Properties
		{
			get
			{
				IDictionary externalProperties;
				lock (this)
				{
					if (this.InternalProperties == null)
					{
						this.InternalProperties = new Hashtable();
					}
					if (this.ExternalProperties == null)
					{
						this.ExternalProperties = new CCMDictionary(this, this.InternalProperties);
					}
					externalProperties = this.ExternalProperties;
				}
				return externalProperties;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06004136 RID: 16694 RVA: 0x000DE13C File Offset: 0x000DD13C
		// (set) Token: 0x06004137 RID: 16695 RVA: 0x000DE144 File Offset: 0x000DD144
		public IActivator Activator
		{
			get
			{
				return this._activator;
			}
			set
			{
				this._activator = value;
			}
		}

		// Token: 0x040020CD RID: 8397
		internal Type _activationType;

		// Token: 0x040020CE RID: 8398
		internal string _activationTypeName;

		// Token: 0x040020CF RID: 8399
		internal IList _contextProperties;

		// Token: 0x040020D0 RID: 8400
		internal object[] _callSiteActivationAttributes;

		// Token: 0x040020D1 RID: 8401
		internal IActivator _activator;
	}
}
