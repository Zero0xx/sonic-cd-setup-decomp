using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000713 RID: 1811
	internal class ConstructorCallMessage : IConstructionCallMessage, IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x000DBD26 File Offset: 0x000DAD26
		private ConstructorCallMessage()
		{
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x000DBD2E File Offset: 0x000DAD2E
		internal ConstructorCallMessage(object[] callSiteActivationAttributes, object[] womAttr, object[] typeAttr, Type serverType)
		{
			this._activationType = serverType;
			this._activationTypeName = RemotingServices.GetDefaultQualifiedTypeName(this._activationType);
			this._callSiteActivationAttributes = callSiteActivationAttributes;
			this._womGlobalAttributes = womAttr;
			this._typeAttributes = typeAttr;
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x000DBD64 File Offset: 0x000DAD64
		public object GetThisPtr()
		{
			if (this._message != null)
			{
				return this._message.GetThisPtr();
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06004091 RID: 16529 RVA: 0x000DBD89 File Offset: 0x000DAD89
		public object[] CallSiteActivationAttributes
		{
			get
			{
				return this._callSiteActivationAttributes;
			}
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x000DBD91 File Offset: 0x000DAD91
		internal object[] GetWOMAttributes()
		{
			return this._womGlobalAttributes;
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x000DBD99 File Offset: 0x000DAD99
		internal object[] GetTypeAttributes()
		{
			return this._typeAttributes;
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06004094 RID: 16532 RVA: 0x000DBDA1 File Offset: 0x000DADA1
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

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x000DBDCB File Offset: 0x000DADCB
		public string ActivationTypeName
		{
			get
			{
				return this._activationTypeName;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x000DBDD3 File Offset: 0x000DADD3
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

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x000DBDEE File Offset: 0x000DADEE
		// (set) Token: 0x06004098 RID: 16536 RVA: 0x000DBE13 File Offset: 0x000DAE13
		public string Uri
		{
			get
			{
				if (this._message != null)
				{
					return this._message.Uri;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
			set
			{
				if (this._message != null)
				{
					this._message.Uri = value;
					return;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06004099 RID: 16537 RVA: 0x000DBE39 File Offset: 0x000DAE39
		public string MethodName
		{
			get
			{
				if (this._message != null)
				{
					return this._message.MethodName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x000DBE5E File Offset: 0x000DAE5E
		public string TypeName
		{
			get
			{
				if (this._message != null)
				{
					return this._message.TypeName;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x000DBE83 File Offset: 0x000DAE83
		public object MethodSignature
		{
			get
			{
				if (this._message != null)
				{
					return this._message.MethodSignature;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x000DBEA8 File Offset: 0x000DAEA8
		public MethodBase MethodBase
		{
			get
			{
				if (this._message != null)
				{
					return this._message.MethodBase;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x0600409D RID: 16541 RVA: 0x000DBECD File Offset: 0x000DAECD
		public int InArgCount
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.ArgCount;
			}
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x000DBEEF File Offset: 0x000DAEEF
		public object GetInArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x000DBF12 File Offset: 0x000DAF12
		public string GetInArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, false);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x060040A0 RID: 16544 RVA: 0x000DBF35 File Offset: 0x000DAF35
		public object[] InArgs
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, false);
				}
				return this._argMapper.Args;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x060040A1 RID: 16545 RVA: 0x000DBF57 File Offset: 0x000DAF57
		public int ArgCount
		{
			get
			{
				if (this._message != null)
				{
					return this._message.ArgCount;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x000DBF7C File Offset: 0x000DAF7C
		public object GetArg(int argNum)
		{
			if (this._message != null)
			{
				return this._message.GetArg(argNum);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x000DBFA2 File Offset: 0x000DAFA2
		public string GetArgName(int index)
		{
			if (this._message != null)
			{
				return this._message.GetArgName(index);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x060040A4 RID: 16548 RVA: 0x000DBFC8 File Offset: 0x000DAFC8
		public bool HasVarArgs
		{
			get
			{
				if (this._message != null)
				{
					return this._message.HasVarArgs;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x000DBFED File Offset: 0x000DAFED
		public object[] Args
		{
			get
			{
				if (this._message != null)
				{
					return this._message.Args;
				}
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x000DC014 File Offset: 0x000DB014
		public IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					object value = new CCMDictionary(this, new Hashtable());
					Interlocked.CompareExchange(ref this._properties, value, null);
				}
				return (IDictionary)this._properties;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x000DC04E File Offset: 0x000DB04E
		// (set) Token: 0x060040A8 RID: 16552 RVA: 0x000DC056 File Offset: 0x000DB056
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

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x000DC05F File Offset: 0x000DB05F
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x000DC067 File Offset: 0x000DB067
		// (set) Token: 0x060040AB RID: 16555 RVA: 0x000DC077 File Offset: 0x000DB077
		internal bool ActivateInContext
		{
			get
			{
				return (this._iFlags & 1) != 0;
			}
			set
			{
				this._iFlags = (value ? (this._iFlags | 1) : (this._iFlags & -2));
			}
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x000DC095 File Offset: 0x000DB095
		internal void SetFrame(MessageData msgData)
		{
			this._message = new Message();
			this._message.InitFields(msgData);
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x000DC0AE File Offset: 0x000DB0AE
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this._message != null)
			{
				return this._message.GetLogicalCallContext();
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x000DC0D3 File Offset: 0x000DB0D3
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			if (this._message != null)
			{
				return this._message.SetLogicalCallContext(ctx);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_InternalState"));
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x000DC0F9 File Offset: 0x000DB0F9
		internal Message GetMessage()
		{
			return this._message;
		}

		// Token: 0x0400209B RID: 8347
		private const int CCM_ACTIVATEINCONTEXT = 1;

		// Token: 0x0400209C RID: 8348
		private object[] _callSiteActivationAttributes;

		// Token: 0x0400209D RID: 8349
		private object[] _womGlobalAttributes;

		// Token: 0x0400209E RID: 8350
		private object[] _typeAttributes;

		// Token: 0x0400209F RID: 8351
		[NonSerialized]
		private Type _activationType;

		// Token: 0x040020A0 RID: 8352
		private string _activationTypeName;

		// Token: 0x040020A1 RID: 8353
		private IList _contextProperties;

		// Token: 0x040020A2 RID: 8354
		private int _iFlags;

		// Token: 0x040020A3 RID: 8355
		private Message _message;

		// Token: 0x040020A4 RID: 8356
		private object _properties;

		// Token: 0x040020A5 RID: 8357
		private ArgMapper _argMapper;

		// Token: 0x040020A6 RID: 8358
		private IActivator _activator;
	}
}
