using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006C5 RID: 1733
	[ComVisible(true)]
	public class Context
	{
		// Token: 0x06003E72 RID: 15986 RVA: 0x000D63AC File Offset: 0x000D53AC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public Context() : this(0)
		{
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x000D63B8 File Offset: 0x000D53B8
		private Context(int flags)
		{
			this._ctxFlags = flags;
			if ((this._ctxFlags & 1) != 0)
			{
				this._ctxID = 0;
			}
			else
			{
				this._ctxID = Interlocked.Increment(ref Context._ctxIDCounter);
			}
			DomainSpecificRemotingData remotingData = Thread.GetDomain().RemotingData;
			if (remotingData != null)
			{
				IContextProperty[] appDomainContextProperties = remotingData.AppDomainContextProperties;
				if (appDomainContextProperties != null)
				{
					for (int i = 0; i < appDomainContextProperties.Length; i++)
					{
						this.SetProperty(appDomainContextProperties[i]);
					}
				}
			}
			if ((this._ctxFlags & 1) != 0)
			{
				this.Freeze();
			}
			this.SetupInternalContext((this._ctxFlags & 1) == 1);
		}

		// Token: 0x06003E74 RID: 15988
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetupInternalContext(bool bDefault);

		// Token: 0x06003E75 RID: 15989 RVA: 0x000D6448 File Offset: 0x000D5448
		~Context()
		{
			if (this._internalContext != IntPtr.Zero && (this._ctxFlags & 1) == 0)
			{
				this.CleanupInternalContext();
			}
		}

		// Token: 0x06003E76 RID: 15990
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CleanupInternalContext();

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06003E77 RID: 15991 RVA: 0x000D6490 File Offset: 0x000D5490
		public virtual int ContextID
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return this._ctxID;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06003E78 RID: 15992 RVA: 0x000D6498 File Offset: 0x000D5498
		internal virtual IntPtr InternalContextID
		{
			get
			{
				return this._internalContext;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06003E79 RID: 15993 RVA: 0x000D64A0 File Offset: 0x000D54A0
		internal virtual AppDomain AppDomain
		{
			get
			{
				return this._appDomain;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06003E7A RID: 15994 RVA: 0x000D64A8 File Offset: 0x000D54A8
		internal bool IsDefaultContext
		{
			get
			{
				return this._ctxID == 0;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06003E7B RID: 15995 RVA: 0x000D64B3 File Offset: 0x000D54B3
		public static Context DefaultContext
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return Thread.GetDomain().GetDefaultContext();
			}
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x000D64BF File Offset: 0x000D54BF
		internal static Context CreateDefaultContext()
		{
			return new Context(1);
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x000D64C8 File Offset: 0x000D54C8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual IContextProperty GetProperty(string name)
		{
			if (this._ctxProps == null || name == null)
			{
				return null;
			}
			IContextProperty result = null;
			for (int i = 0; i < this._numCtxProps; i++)
			{
				if (this._ctxProps[i].Name.Equals(name))
				{
					result = this._ctxProps[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x000D6518 File Offset: 0x000D5518
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual void SetProperty(IContextProperty prop)
		{
			if (prop == null || prop.Name == null)
			{
				throw new ArgumentNullException((prop == null) ? "prop" : "property name");
			}
			if ((this._ctxFlags & 2) != 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_AddContextFrozen"));
			}
			lock (this)
			{
				Context.CheckPropertyNameClash(prop.Name, this._ctxProps, this._numCtxProps);
				if (this._ctxProps == null || this._numCtxProps == this._ctxProps.Length)
				{
					this._ctxProps = Context.GrowPropertiesArray(this._ctxProps);
				}
				this._ctxProps[this._numCtxProps++] = prop;
			}
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x000D65DC File Offset: 0x000D55DC
		internal virtual void InternalFreeze()
		{
			this._ctxFlags |= 2;
			for (int i = 0; i < this._numCtxProps; i++)
			{
				this._ctxProps[i].Freeze(this);
			}
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x000D6618 File Offset: 0x000D5618
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual void Freeze()
		{
			lock (this)
			{
				if ((this._ctxFlags & 2) != 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ContextAlreadyFrozen"));
				}
				this.InternalFreeze();
			}
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x000D6668 File Offset: 0x000D5668
		internal virtual void SetThreadPoolAware()
		{
			this._ctxFlags |= 4;
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x000D6678 File Offset: 0x000D5678
		internal virtual bool IsThreadPoolAware
		{
			get
			{
				return (this._ctxFlags & 4) == 4;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x000D6688 File Offset: 0x000D5688
		public virtual IContextProperty[] ContextProperties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				if (this._ctxProps == null)
				{
					return null;
				}
				IContextProperty[] result;
				lock (this)
				{
					IContextProperty[] array = new IContextProperty[this._numCtxProps];
					Array.Copy(this._ctxProps, array, this._numCtxProps);
					result = array;
				}
				return result;
			}
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x000D66E4 File Offset: 0x000D56E4
		internal static void CheckPropertyNameClash(string name, IContextProperty[] props, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (props[i].Name.Equals(name))
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DuplicatePropertyName"));
				}
			}
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x000D6720 File Offset: 0x000D5720
		internal static IContextProperty[] GrowPropertiesArray(IContextProperty[] props)
		{
			int num = ((props != null) ? props.Length : 0) + 8;
			IContextProperty[] array = new IContextProperty[num];
			if (props != null)
			{
				Array.Copy(props, array, props.Length);
			}
			return array;
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x000D6750 File Offset: 0x000D5750
		internal virtual IMessageSink GetServerContextChain()
		{
			if (this._serverContextChain == null)
			{
				IMessageSink messageSink = ServerContextTerminatorSink.MessageSink;
				int numCtxProps = this._numCtxProps;
				while (numCtxProps-- > 0)
				{
					object obj = this._ctxProps[numCtxProps];
					IContributeServerContextSink contributeServerContextSink = obj as IContributeServerContextSink;
					if (contributeServerContextSink != null)
					{
						messageSink = contributeServerContextSink.GetServerContextSink(messageSink);
						if (messageSink == null)
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
						}
					}
				}
				lock (this)
				{
					if (this._serverContextChain == null)
					{
						this._serverContextChain = messageSink;
					}
				}
			}
			return this._serverContextChain;
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x000D67E4 File Offset: 0x000D57E4
		internal virtual IMessageSink GetClientContextChain()
		{
			if (this._clientContextChain == null)
			{
				IMessageSink messageSink = ClientContextTerminatorSink.MessageSink;
				for (int i = 0; i < this._numCtxProps; i++)
				{
					object obj = this._ctxProps[i];
					IContributeClientContextSink contributeClientContextSink = obj as IContributeClientContextSink;
					if (contributeClientContextSink != null)
					{
						messageSink = contributeClientContextSink.GetClientContextSink(messageSink);
						if (messageSink == null)
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
						}
					}
				}
				lock (this)
				{
					if (this._clientContextChain == null)
					{
						this._clientContextChain = messageSink;
					}
				}
			}
			return this._clientContextChain;
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x000D6878 File Offset: 0x000D5878
		internal virtual IMessageSink CreateServerObjectChain(MarshalByRefObject serverObj)
		{
			IMessageSink messageSink = new ServerObjectTerminatorSink(serverObj);
			int numCtxProps = this._numCtxProps;
			while (numCtxProps-- > 0)
			{
				object obj = this._ctxProps[numCtxProps];
				IContributeObjectSink contributeObjectSink = obj as IContributeObjectSink;
				if (contributeObjectSink != null)
				{
					messageSink = contributeObjectSink.GetObjectSink(serverObj, messageSink);
					if (messageSink == null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
					}
				}
			}
			return messageSink;
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x000D68D0 File Offset: 0x000D58D0
		internal virtual IMessageSink CreateEnvoyChain(MarshalByRefObject objectOrProxy)
		{
			IMessageSink messageSink = EnvoyTerminatorSink.MessageSink;
			for (int i = 0; i < this._numCtxProps; i++)
			{
				object obj = this._ctxProps[i];
				IContributeEnvoySink contributeEnvoySink = obj as IContributeEnvoySink;
				if (contributeEnvoySink != null)
				{
					messageSink = contributeEnvoySink.GetEnvoySink(objectOrProxy, messageSink);
					if (messageSink == null)
					{
						throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_BadProperty"));
					}
				}
			}
			return messageSink;
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x000D692C File Offset: 0x000D592C
		internal IMessage NotifyActivatorProperties(IMessage msg, bool bServerSide)
		{
			IMessage message = null;
			try
			{
				int numCtxProps = this._numCtxProps;
				while (numCtxProps-- != 0)
				{
					object obj = this._ctxProps[numCtxProps];
					IContextPropertyActivator contextPropertyActivator = obj as IContextPropertyActivator;
					if (contextPropertyActivator != null)
					{
						IConstructionCallMessage constructionCallMessage = msg as IConstructionCallMessage;
						if (constructionCallMessage != null)
						{
							if (!bServerSide)
							{
								contextPropertyActivator.CollectFromClientContext(constructionCallMessage);
							}
							else
							{
								contextPropertyActivator.DeliverClientContextToServerContext(constructionCallMessage);
							}
						}
						else if (bServerSide)
						{
							contextPropertyActivator.CollectFromServerContext((IConstructionReturnMessage)msg);
						}
						else
						{
							contextPropertyActivator.DeliverServerContextToClientContext((IConstructionReturnMessage)msg);
						}
					}
				}
			}
			catch (Exception e)
			{
				IMethodCallMessage mcm;
				if (msg is IConstructionCallMessage)
				{
					mcm = (IMethodCallMessage)msg;
				}
				else
				{
					mcm = new ErrorMessage();
				}
				message = new ReturnMessage(e, mcm);
				if (msg != null)
				{
					((ReturnMessage)message).SetLogicalCallContext((LogicalCallContext)msg.Properties[Message.CallContextKey]);
				}
			}
			return message;
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x000D6A04 File Offset: 0x000D5A04
		public override string ToString()
		{
			return "ContextID: " + this._ctxID;
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x000D6A1C File Offset: 0x000D5A1C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public void DoCallBack(CrossContextDelegate deleg)
		{
			if (deleg == null)
			{
				throw new ArgumentNullException("deleg");
			}
			if ((this._ctxFlags & 2) == 0)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Contexts_ContextNotFrozenForCallBack"));
			}
			Context currentContext = Thread.CurrentContext;
			if (currentContext == this)
			{
				deleg();
				return;
			}
			currentContext.DoCallBackGeneric(this.InternalContextID, deleg);
			GC.KeepAlive(this);
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x000D6A78 File Offset: 0x000D5A78
		internal static void DoCallBackFromEE(IntPtr targetCtxID, IntPtr privateData, int targetDomainID)
		{
			if (targetDomainID == 0)
			{
				CallBackHelper @object = new CallBackHelper(privateData, true, targetDomainID);
				CrossContextDelegate deleg = new CrossContextDelegate(@object.Func);
				Thread.CurrentContext.DoCallBackGeneric(targetCtxID, deleg);
				return;
			}
			TransitionCall msg = new TransitionCall(targetCtxID, privateData, targetDomainID);
			Message.PropagateCallContextFromThreadToMessage(msg);
			IMessage message = Thread.CurrentContext.GetClientContextChain().SyncProcessMessage(msg);
			Message.PropagateCallContextFromMessageToThread(message);
			IMethodReturnMessage methodReturnMessage = message as IMethodReturnMessage;
			if (methodReturnMessage != null && methodReturnMessage.Exception != null)
			{
				throw methodReturnMessage.Exception;
			}
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x000D6AF0 File Offset: 0x000D5AF0
		internal void DoCallBackGeneric(IntPtr targetCtxID, CrossContextDelegate deleg)
		{
			TransitionCall msg = new TransitionCall(targetCtxID, deleg);
			Message.PropagateCallContextFromThreadToMessage(msg);
			IMessage message = this.GetClientContextChain().SyncProcessMessage(msg);
			if (message != null)
			{
				Message.PropagateCallContextFromMessageToThread(message);
			}
			IMethodReturnMessage methodReturnMessage = message as IMethodReturnMessage;
			if (methodReturnMessage != null && methodReturnMessage.Exception != null)
			{
				throw methodReturnMessage.Exception;
			}
		}

		// Token: 0x06003E8F RID: 16015
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ExecuteCallBackInEE(IntPtr privateData);

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x000D6B3C File Offset: 0x000D5B3C
		private LocalDataStore MyLocalStore
		{
			get
			{
				if (this._localDataStore == null)
				{
					lock (Context._localDataStoreMgr)
					{
						if (this._localDataStore == null)
						{
							this._localDataStore = Context._localDataStoreMgr.CreateLocalDataStore();
						}
					}
				}
				return this._localDataStore;
			}
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x000D6B94 File Offset: 0x000D5B94
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static LocalDataStoreSlot AllocateDataSlot()
		{
			return Context._localDataStoreMgr.AllocateDataSlot();
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x000D6BA0 File Offset: 0x000D5BA0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			return Context._localDataStoreMgr.AllocateNamedDataSlot(name);
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x000D6BAD File Offset: 0x000D5BAD
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			return Context._localDataStoreMgr.GetNamedDataSlot(name);
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x000D6BBA File Offset: 0x000D5BBA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void FreeNamedDataSlot(string name)
		{
			Context._localDataStoreMgr.FreeNamedDataSlot(name);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x000D6BC7 File Offset: 0x000D5BC7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static void SetData(LocalDataStoreSlot slot, object data)
		{
			Thread.CurrentContext.MyLocalStore.SetData(slot, data);
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x000D6BDA File Offset: 0x000D5BDA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static object GetData(LocalDataStoreSlot slot)
		{
			return Thread.CurrentContext.MyLocalStore.GetData(slot);
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x000D6BEC File Offset: 0x000D5BEC
		private int ReserveSlot()
		{
			if (this._ctxStatics == null)
			{
				this._ctxStatics = new object[8];
				this._ctxStatics[0] = null;
				this._ctxStaticsFreeIndex = 1;
				this._ctxStaticsCurrentBucket = 0;
			}
			if (this._ctxStaticsFreeIndex == 8)
			{
				object[] array = new object[8];
				object[] array2 = this._ctxStatics;
				while (array2[0] != null)
				{
					array2 = (object[])array2[0];
				}
				array2[0] = array;
				this._ctxStaticsFreeIndex = 1;
				this._ctxStaticsCurrentBucket++;
			}
			return this._ctxStaticsFreeIndex++ | this._ctxStaticsCurrentBucket << 16;
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x000D6C80 File Offset: 0x000D5C80
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static bool RegisterDynamicProperty(IDynamicProperty prop, ContextBoundObject obj, Context ctx)
		{
			if (prop == null || prop.Name == null || !(prop is IContributeDynamicSink))
			{
				throw new ArgumentNullException("prop");
			}
			if (obj != null && ctx != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NonNullObjAndCtx"));
			}
			bool result;
			if (obj != null)
			{
				result = IdentityHolder.AddDynamicProperty(obj, prop);
			}
			else
			{
				result = Context.AddDynamicProperty(ctx, prop);
			}
			return result;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x000D6CDC File Offset: 0x000D5CDC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
		public static bool UnregisterDynamicProperty(string name, ContextBoundObject obj, Context ctx)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (obj != null && ctx != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NonNullObjAndCtx"));
			}
			bool result;
			if (obj != null)
			{
				result = IdentityHolder.RemoveDynamicProperty(obj, name);
			}
			else
			{
				result = Context.RemoveDynamicProperty(ctx, name);
			}
			return result;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x000D6D25 File Offset: 0x000D5D25
		internal static bool AddDynamicProperty(Context ctx, IDynamicProperty prop)
		{
			if (ctx != null)
			{
				return ctx.AddPerContextDynamicProperty(prop);
			}
			return Context.AddGlobalDynamicProperty(prop);
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x000D6D38 File Offset: 0x000D5D38
		private bool AddPerContextDynamicProperty(IDynamicProperty prop)
		{
			if (this._dphCtx == null)
			{
				DynamicPropertyHolder dphCtx = new DynamicPropertyHolder();
				lock (this)
				{
					if (this._dphCtx == null)
					{
						this._dphCtx = dphCtx;
					}
				}
			}
			return this._dphCtx.AddDynamicProperty(prop);
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x000D6D90 File Offset: 0x000D5D90
		private static bool AddGlobalDynamicProperty(IDynamicProperty prop)
		{
			return Context._dphGlobal.AddDynamicProperty(prop);
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x000D6D9D File Offset: 0x000D5D9D
		internal static bool RemoveDynamicProperty(Context ctx, string name)
		{
			if (ctx != null)
			{
				return ctx.RemovePerContextDynamicProperty(name);
			}
			return Context.RemoveGlobalDynamicProperty(name);
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x000D6DB0 File Offset: 0x000D5DB0
		private bool RemovePerContextDynamicProperty(string name)
		{
			if (this._dphCtx == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Contexts_NoProperty"), new object[]
				{
					name
				}));
			}
			return this._dphCtx.RemoveDynamicProperty(name);
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x000D6DF7 File Offset: 0x000D5DF7
		private static bool RemoveGlobalDynamicProperty(string name)
		{
			return Context._dphGlobal.RemoveDynamicProperty(name);
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003EA0 RID: 16032 RVA: 0x000D6E04 File Offset: 0x000D5E04
		internal virtual IDynamicProperty[] PerContextDynamicProperties
		{
			get
			{
				if (this._dphCtx == null)
				{
					return null;
				}
				return this._dphCtx.DynamicProperties;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x000D6E1B File Offset: 0x000D5E1B
		internal static ArrayWithSize GlobalDynamicSinks
		{
			get
			{
				return Context._dphGlobal.DynamicSinks;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003EA2 RID: 16034 RVA: 0x000D6E27 File Offset: 0x000D5E27
		internal virtual ArrayWithSize DynamicSinks
		{
			get
			{
				if (this._dphCtx == null)
				{
					return null;
				}
				return this._dphCtx.DynamicSinks;
			}
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x000D6E40 File Offset: 0x000D5E40
		internal virtual bool NotifyDynamicSinks(IMessage msg, bool bCliSide, bool bStart, bool bAsync, bool bNotifyGlobals)
		{
			bool result = false;
			if (bNotifyGlobals && Context._dphGlobal.DynamicProperties != null)
			{
				ArrayWithSize globalDynamicSinks = Context.GlobalDynamicSinks;
				if (globalDynamicSinks != null)
				{
					DynamicPropertyHolder.NotifyDynamicSinks(msg, globalDynamicSinks, bCliSide, bStart, bAsync);
					result = true;
				}
			}
			ArrayWithSize dynamicSinks = this.DynamicSinks;
			if (dynamicSinks != null)
			{
				DynamicPropertyHolder.NotifyDynamicSinks(msg, dynamicSinks, bCliSide, bStart, bAsync);
				result = true;
			}
			return result;
		}

		// Token: 0x04001FCD RID: 8141
		internal const int CTX_DEFAULT_CONTEXT = 1;

		// Token: 0x04001FCE RID: 8142
		internal const int CTX_FROZEN = 2;

		// Token: 0x04001FCF RID: 8143
		internal const int CTX_THREADPOOL_AWARE = 4;

		// Token: 0x04001FD0 RID: 8144
		private const int GROW_BY = 8;

		// Token: 0x04001FD1 RID: 8145
		private const int STATICS_BUCKET_SIZE = 8;

		// Token: 0x04001FD2 RID: 8146
		private IContextProperty[] _ctxProps;

		// Token: 0x04001FD3 RID: 8147
		private DynamicPropertyHolder _dphCtx;

		// Token: 0x04001FD4 RID: 8148
		private LocalDataStore _localDataStore;

		// Token: 0x04001FD5 RID: 8149
		private IMessageSink _serverContextChain;

		// Token: 0x04001FD6 RID: 8150
		private IMessageSink _clientContextChain;

		// Token: 0x04001FD7 RID: 8151
		private AppDomain _appDomain;

		// Token: 0x04001FD8 RID: 8152
		private object[] _ctxStatics;

		// Token: 0x04001FD9 RID: 8153
		private IntPtr _internalContext;

		// Token: 0x04001FDA RID: 8154
		private int _ctxID;

		// Token: 0x04001FDB RID: 8155
		private int _ctxFlags;

		// Token: 0x04001FDC RID: 8156
		private int _numCtxProps;

		// Token: 0x04001FDD RID: 8157
		private int _ctxStaticsCurrentBucket;

		// Token: 0x04001FDE RID: 8158
		private int _ctxStaticsFreeIndex;

		// Token: 0x04001FDF RID: 8159
		private static DynamicPropertyHolder _dphGlobal = new DynamicPropertyHolder();

		// Token: 0x04001FE0 RID: 8160
		private static LocalDataStoreMgr _localDataStoreMgr = new LocalDataStoreMgr();

		// Token: 0x04001FE1 RID: 8161
		private static int _ctxIDCounter = 0;
	}
}
