using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x02000771 RID: 1905
	internal class ServerIdentity : Identity
	{
		// Token: 0x060043EF RID: 17391 RVA: 0x000E8788 File Offset: 0x000E7788
		internal Type GetLastCalledType(string newTypeName)
		{
			ServerIdentity.LastCalledType lastCalledType = this._lastCalledType;
			if (lastCalledType == null)
			{
				return null;
			}
			string typeName = lastCalledType.typeName;
			Type type = lastCalledType.type;
			if (typeName == null || type == null)
			{
				return null;
			}
			if (typeName.Equals(newTypeName))
			{
				return type;
			}
			return null;
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x000E87C4 File Offset: 0x000E77C4
		internal void SetLastCalledType(string newTypeName, Type newType)
		{
			this._lastCalledType = new ServerIdentity.LastCalledType
			{
				typeName = newTypeName,
				type = newType
			};
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x000E87EC File Offset: 0x000E77EC
		internal void SetHandle()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				if (!this._srvIdentityHandle.IsAllocated)
				{
					this._srvIdentityHandle = new GCHandle(this, GCHandleType.Normal);
				}
				else
				{
					this._srvIdentityHandle.Target = this;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x000E884C File Offset: 0x000E784C
		internal void ResetHandle()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this._srvIdentityHandle.Target = null;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x000E8890 File Offset: 0x000E7890
		internal GCHandle GetHandle()
		{
			return this._srvIdentityHandle;
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x000E8898 File Offset: 0x000E7898
		internal ServerIdentity(MarshalByRefObject obj, Context serverCtx) : base(obj is ContextBoundObject)
		{
			if (obj != null)
			{
				if (!RemotingServices.IsTransparentProxy(obj))
				{
					this._srvType = obj.GetType();
				}
				else
				{
					RealProxy realProxy = RemotingServices.GetRealProxy(obj);
					this._srvType = realProxy.GetProxiedType();
				}
			}
			this._srvCtx = serverCtx;
			this._serverObjectChain = null;
			this._stackBuilderSink = null;
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x000E88F5 File Offset: 0x000E78F5
		internal ServerIdentity(MarshalByRefObject obj, Context serverCtx, string uri) : this(obj, serverCtx)
		{
			base.SetOrCreateURI(uri, true);
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060043F6 RID: 17398 RVA: 0x000E8907 File Offset: 0x000E7907
		internal Context ServerContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._srvCtx;
			}
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x000E890F File Offset: 0x000E790F
		internal void SetSingleCallObjectMode()
		{
			this._flags |= 512;
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x000E8923 File Offset: 0x000E7923
		internal void SetSingletonObjectMode()
		{
			this._flags |= 1024;
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x000E8937 File Offset: 0x000E7937
		internal bool IsSingleCall()
		{
			return (this._flags & 512) != 0;
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x000E894B File Offset: 0x000E794B
		internal bool IsSingleton()
		{
			return (this._flags & 1024) != 0;
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x000E8960 File Offset: 0x000E7960
		internal IMessageSink GetServerObjectChain(out MarshalByRefObject obj)
		{
			obj = null;
			if (!this.IsSingleCall())
			{
				if (this._serverObjectChain == null)
				{
					bool flag = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.ReliableEnter(this, ref flag);
						if (this._serverObjectChain == null)
						{
							MarshalByRefObject tporObject = base.TPOrObject;
							this._serverObjectChain = this._srvCtx.CreateServerObjectChain(tporObject);
						}
					}
					finally
					{
						if (flag)
						{
							Monitor.Exit(this);
						}
					}
				}
				return this._serverObjectChain;
			}
			MarshalByRefObject marshalByRefObject;
			IMessageSink messageSink;
			if (this._tpOrObject != null && this._firstCallDispatched == 0 && Interlocked.CompareExchange(ref this._firstCallDispatched, 1, 0) == 0)
			{
				marshalByRefObject = (MarshalByRefObject)this._tpOrObject;
				messageSink = this._serverObjectChain;
				if (messageSink == null)
				{
					messageSink = this._srvCtx.CreateServerObjectChain(marshalByRefObject);
				}
			}
			else
			{
				marshalByRefObject = (MarshalByRefObject)Activator.CreateInstance(this._srvType, true);
				string objectUri = RemotingServices.GetObjectUri(marshalByRefObject);
				if (objectUri != null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_WellKnown_CtorCantMarshal"), new object[]
					{
						base.URI
					}));
				}
				if (!RemotingServices.IsTransparentProxy(marshalByRefObject))
				{
					marshalByRefObject.__RaceSetServerIdentity(this);
				}
				else
				{
					RealProxy realProxy = RemotingServices.GetRealProxy(marshalByRefObject);
					realProxy.IdentityObject = this;
				}
				messageSink = this._srvCtx.CreateServerObjectChain(marshalByRefObject);
			}
			obj = marshalByRefObject;
			return messageSink;
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060043FC RID: 17404 RVA: 0x000E8AA0 File Offset: 0x000E7AA0
		// (set) Token: 0x060043FD RID: 17405 RVA: 0x000E8AA8 File Offset: 0x000E7AA8
		internal Type ServerType
		{
			get
			{
				return this._srvType;
			}
			set
			{
				this._srvType = value;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060043FE RID: 17406 RVA: 0x000E8AB1 File Offset: 0x000E7AB1
		// (set) Token: 0x060043FF RID: 17407 RVA: 0x000E8AB9 File Offset: 0x000E7AB9
		internal bool MarshaledAsSpecificType
		{
			get
			{
				return this._bMarshaledAsSpecificType;
			}
			set
			{
				this._bMarshaledAsSpecificType = value;
			}
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x000E8AC4 File Offset: 0x000E7AC4
		internal IMessageSink RaceSetServerObjectChain(IMessageSink serverObjectChain)
		{
			if (this._serverObjectChain == null)
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(this, ref flag);
					if (this._serverObjectChain == null)
					{
						this._serverObjectChain = serverObjectChain;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
			return this._serverObjectChain;
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x000E8B1C File Offset: 0x000E7B1C
		internal bool AddServerSideDynamicProperty(IDynamicProperty prop)
		{
			if (this._dphSrv == null)
			{
				DynamicPropertyHolder dphSrv = new DynamicPropertyHolder();
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(this, ref flag);
					if (this._dphSrv == null)
					{
						this._dphSrv = dphSrv;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
			return this._dphSrv.AddDynamicProperty(prop);
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x000E8B80 File Offset: 0x000E7B80
		internal bool RemoveServerSideDynamicProperty(string name)
		{
			if (this._dphSrv == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_PropNotFound"));
			}
			return this._dphSrv.RemoveDynamicProperty(name);
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06004403 RID: 17411 RVA: 0x000E8BA6 File Offset: 0x000E7BA6
		internal ArrayWithSize ServerSideDynamicSinks
		{
			get
			{
				if (this._dphSrv == null)
				{
					return null;
				}
				return this._dphSrv.DynamicSinks;
			}
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x000E8BBD File Offset: 0x000E7BBD
		internal override void AssertValid()
		{
			if (base.TPOrObject != null)
			{
				RemotingServices.IsTransparentProxy(base.TPOrObject);
			}
		}

		// Token: 0x04002206 RID: 8710
		internal Context _srvCtx;

		// Token: 0x04002207 RID: 8711
		internal IMessageSink _serverObjectChain;

		// Token: 0x04002208 RID: 8712
		internal StackBuilderSink _stackBuilderSink;

		// Token: 0x04002209 RID: 8713
		internal DynamicPropertyHolder _dphSrv;

		// Token: 0x0400220A RID: 8714
		internal Type _srvType;

		// Token: 0x0400220B RID: 8715
		private ServerIdentity.LastCalledType _lastCalledType;

		// Token: 0x0400220C RID: 8716
		internal bool _bMarshaledAsSpecificType;

		// Token: 0x0400220D RID: 8717
		internal int _firstCallDispatched;

		// Token: 0x0400220E RID: 8718
		internal GCHandle _srvIdentityHandle;

		// Token: 0x02000772 RID: 1906
		private class LastCalledType
		{
			// Token: 0x0400220F RID: 8719
			public string typeName;

			// Token: 0x04002210 RID: 8720
			public Type type;
		}
	}
}
