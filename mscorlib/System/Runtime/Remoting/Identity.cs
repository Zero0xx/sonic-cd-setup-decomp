using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x020006FD RID: 1789
	internal class Identity
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003F99 RID: 16281 RVA: 0x000D8C52 File Offset: 0x000D7C52
		internal static string ProcessIDGuid
		{
			get
			{
				return SharedStatics.Remoting_Identity_IDGuid;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003F9A RID: 16282 RVA: 0x000D8C59 File Offset: 0x000D7C59
		internal static string AppDomainUniqueId
		{
			get
			{
				if (Identity.s_configuredAppDomainGuid != null)
				{
					return Identity.s_configuredAppDomainGuid;
				}
				return Identity.s_originalAppDomainGuid;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003F9B RID: 16283 RVA: 0x000D8C6D File Offset: 0x000D7C6D
		internal static string IDGuidString
		{
			get
			{
				return Identity.s_IDGuidString;
			}
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x000D8C74 File Offset: 0x000D7C74
		internal static string RemoveAppNameOrAppGuidIfNecessary(string uri)
		{
			if (uri == null || uri.Length <= 1 || uri[0] != '/')
			{
				return uri;
			}
			string text;
			if (Identity.s_configuredAppDomainGuidString != null)
			{
				text = Identity.s_configuredAppDomainGuidString;
				if (uri.Length > text.Length && Identity.StringStartsWith(uri, text))
				{
					return uri.Substring(text.Length);
				}
			}
			text = Identity.s_originalAppDomainGuidString;
			if (uri.Length > text.Length && Identity.StringStartsWith(uri, text))
			{
				return uri.Substring(text.Length);
			}
			string applicationName = RemotingConfiguration.ApplicationName;
			if (applicationName != null && uri.Length > applicationName.Length + 2 && string.Compare(uri, 1, applicationName, 0, applicationName.Length, true, CultureInfo.InvariantCulture) == 0 && uri[applicationName.Length + 1] == '/')
			{
				return uri.Substring(applicationName.Length + 2);
			}
			uri = uri.Substring(1);
			return uri;
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x000D8D50 File Offset: 0x000D7D50
		private static bool StringStartsWith(string s1, string prefix)
		{
			return s1.Length >= prefix.Length && string.CompareOrdinal(s1, 0, prefix, 0, prefix.Length) == 0;
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003F9E RID: 16286 RVA: 0x000D8D74 File Offset: 0x000D7D74
		internal static string ProcessGuid
		{
			get
			{
				return Identity.ProcessIDGuid;
			}
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x000D8D7B File Offset: 0x000D7D7B
		private static int GetNextSeqNum()
		{
			return SharedStatics.Remoting_Identity_GetNextSeqNum();
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x000D8D84 File Offset: 0x000D7D84
		private static byte[] GetRandomBytes()
		{
			byte[] array = new byte[18];
			Identity.s_rng.GetBytes(array);
			return array;
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x000D8DA5 File Offset: 0x000D7DA5
		internal Identity(string objURI, string URL)
		{
			if (URL != null)
			{
				this._flags |= 256;
				this._URL = URL;
			}
			this.SetOrCreateURI(objURI, true);
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x000D8DD1 File Offset: 0x000D7DD1
		internal Identity(bool bContextBound)
		{
			if (bContextBound)
			{
				this._flags |= 16;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x000D8DEB File Offset: 0x000D7DEB
		internal bool IsContextBound
		{
			get
			{
				return (this._flags & 16) == 16;
			}
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x000D8DFA File Offset: 0x000D7DFA
		internal bool IsWellKnown()
		{
			return (this._flags & 256) == 256;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x000D8E10 File Offset: 0x000D7E10
		internal void SetInIDTable()
		{
			int flags;
			int value;
			do
			{
				flags = this._flags;
				value = (this._flags | 4);
			}
			while (flags != Interlocked.CompareExchange(ref this._flags, value, flags));
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x000D8E40 File Offset: 0x000D7E40
		internal void ResetInIDTable(bool bResetURI)
		{
			int flags;
			int value;
			do
			{
				flags = this._flags;
				value = (this._flags & -5);
			}
			while (flags != Interlocked.CompareExchange(ref this._flags, value, flags));
			if (bResetURI)
			{
				((ObjRef)this._objRef).URI = null;
				this._ObjURI = null;
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x000D8E89 File Offset: 0x000D7E89
		internal bool IsInIDTable()
		{
			return (this._flags & 4) == 4;
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x000D8E98 File Offset: 0x000D7E98
		internal void SetFullyConnected()
		{
			int flags;
			int value;
			do
			{
				flags = this._flags;
				value = (this._flags & -4);
			}
			while (flags != Interlocked.CompareExchange(ref this._flags, value, flags));
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x000D8EC6 File Offset: 0x000D7EC6
		internal bool IsFullyDisconnected()
		{
			return (this._flags & 1) == 1;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x000D8ED3 File Offset: 0x000D7ED3
		internal bool IsRemoteDisconnected()
		{
			return (this._flags & 2) == 2;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x000D8EE0 File Offset: 0x000D7EE0
		internal bool IsDisconnected()
		{
			return this.IsFullyDisconnected() || this.IsRemoteDisconnected();
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003FAC RID: 16300 RVA: 0x000D8EF2 File Offset: 0x000D7EF2
		internal string URI
		{
			get
			{
				if (this.IsWellKnown())
				{
					return this._URL;
				}
				return this._ObjURI;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06003FAD RID: 16301 RVA: 0x000D8F09 File Offset: 0x000D7F09
		internal string ObjURI
		{
			get
			{
				return this._ObjURI;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003FAE RID: 16302 RVA: 0x000D8F11 File Offset: 0x000D7F11
		internal MarshalByRefObject TPOrObject
		{
			get
			{
				return (MarshalByRefObject)this._tpOrObject;
			}
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x000D8F1E File Offset: 0x000D7F1E
		internal object RaceSetTransparentProxy(object tpObj)
		{
			if (this._tpOrObject == null)
			{
				Interlocked.CompareExchange(ref this._tpOrObject, tpObj, null);
			}
			return this._tpOrObject;
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003FB0 RID: 16304 RVA: 0x000D8F3C File Offset: 0x000D7F3C
		internal ObjRef ObjectRef
		{
			get
			{
				return (ObjRef)this._objRef;
			}
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x000D8F49 File Offset: 0x000D7F49
		internal ObjRef RaceSetObjRef(ObjRef objRefGiven)
		{
			if (this._objRef == null)
			{
				Interlocked.CompareExchange(ref this._objRef, objRefGiven, null);
			}
			return (ObjRef)this._objRef;
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06003FB2 RID: 16306 RVA: 0x000D8F6C File Offset: 0x000D7F6C
		internal IMessageSink ChannelSink
		{
			get
			{
				return (IMessageSink)this._channelSink;
			}
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x000D8F79 File Offset: 0x000D7F79
		internal IMessageSink RaceSetChannelSink(IMessageSink channelSink)
		{
			if (this._channelSink == null)
			{
				Interlocked.CompareExchange(ref this._channelSink, channelSink, null);
			}
			return (IMessageSink)this._channelSink;
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06003FB4 RID: 16308 RVA: 0x000D8F9C File Offset: 0x000D7F9C
		internal IMessageSink EnvoyChain
		{
			get
			{
				return (IMessageSink)this._envoyChain;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x000D8FA9 File Offset: 0x000D7FA9
		// (set) Token: 0x06003FB6 RID: 16310 RVA: 0x000D8FB1 File Offset: 0x000D7FB1
		internal Lease Lease
		{
			get
			{
				return this._lease;
			}
			set
			{
				this._lease = value;
			}
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x000D8FBA File Offset: 0x000D7FBA
		internal IMessageSink RaceSetEnvoyChain(IMessageSink envoyChain)
		{
			if (this._envoyChain == null)
			{
				Interlocked.CompareExchange(ref this._envoyChain, envoyChain, null);
			}
			return (IMessageSink)this._envoyChain;
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x000D8FDD File Offset: 0x000D7FDD
		internal void SetOrCreateURI(string uri)
		{
			this.SetOrCreateURI(uri, false);
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x000D8FE8 File Offset: 0x000D7FE8
		internal void SetOrCreateURI(string uri, bool bIdCtor)
		{
			if (!bIdCtor && this._ObjURI != null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_SetObjectUriForMarshal__UriExists"));
			}
			if (uri == null)
			{
				string text = Convert.ToBase64String(Identity.GetRandomBytes());
				this._ObjURI = string.Concat(new object[]
				{
					Identity.IDGuidString,
					text.Replace('/', '_'),
					"_",
					Identity.GetNextSeqNum(),
					".rem"
				}).ToLower(CultureInfo.InvariantCulture);
				return;
			}
			if (this is ServerIdentity)
			{
				this._ObjURI = Identity.IDGuidString + uri;
				return;
			}
			this._ObjURI = uri;
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x000D908F File Offset: 0x000D808F
		internal static string GetNewLogicalCallID()
		{
			return Identity.IDGuidString + Identity.GetNextSeqNum();
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x000D90A5 File Offset: 0x000D80A5
		[Conditional("_DEBUG")]
		internal virtual void AssertValid()
		{
			if (this.URI != null)
			{
				IdentityHolder.ResolveIdentity(this.URI);
			}
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x000D90BC File Offset: 0x000D80BC
		internal bool AddProxySideDynamicProperty(IDynamicProperty prop)
		{
			bool result;
			lock (this)
			{
				if (this._dph == null)
				{
					DynamicPropertyHolder dph = new DynamicPropertyHolder();
					lock (this)
					{
						if (this._dph == null)
						{
							this._dph = dph;
						}
					}
				}
				result = this._dph.AddDynamicProperty(prop);
			}
			return result;
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x000D9134 File Offset: 0x000D8134
		internal bool RemoveProxySideDynamicProperty(string name)
		{
			bool result;
			lock (this)
			{
				if (this._dph == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Contexts_NoProperty"), new object[]
					{
						name
					}));
				}
				result = this._dph.RemoveDynamicProperty(name);
			}
			return result;
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06003FBE RID: 16318 RVA: 0x000D91A0 File Offset: 0x000D81A0
		internal ArrayWithSize ProxySideDynamicSinks
		{
			get
			{
				if (this._dph == null)
				{
					return null;
				}
				return this._dph.DynamicSinks;
			}
		}

		// Token: 0x04002022 RID: 8226
		protected const int IDFLG_DISCONNECTED_FULL = 1;

		// Token: 0x04002023 RID: 8227
		protected const int IDFLG_DISCONNECTED_REM = 2;

		// Token: 0x04002024 RID: 8228
		protected const int IDFLG_IN_IDTABLE = 4;

		// Token: 0x04002025 RID: 8229
		protected const int IDFLG_CONTEXT_BOUND = 16;

		// Token: 0x04002026 RID: 8230
		protected const int IDFLG_WELLKNOWN = 256;

		// Token: 0x04002027 RID: 8231
		protected const int IDFLG_SERVER_SINGLECALL = 512;

		// Token: 0x04002028 RID: 8232
		protected const int IDFLG_SERVER_SINGLETON = 1024;

		// Token: 0x04002029 RID: 8233
		private static string s_originalAppDomainGuid = Guid.NewGuid().ToString().Replace('-', '_');

		// Token: 0x0400202A RID: 8234
		private static string s_configuredAppDomainGuid = null;

		// Token: 0x0400202B RID: 8235
		private static string s_originalAppDomainGuidString = "/" + Identity.s_originalAppDomainGuid.ToLower(CultureInfo.InvariantCulture) + "/";

		// Token: 0x0400202C RID: 8236
		private static string s_configuredAppDomainGuidString = null;

		// Token: 0x0400202D RID: 8237
		private static string s_IDGuidString = "/" + Identity.s_originalAppDomainGuid.ToLower(CultureInfo.InvariantCulture) + "/";

		// Token: 0x0400202E RID: 8238
		private static RNGCryptoServiceProvider s_rng = new RNGCryptoServiceProvider();

		// Token: 0x0400202F RID: 8239
		internal int _flags;

		// Token: 0x04002030 RID: 8240
		internal object _tpOrObject;

		// Token: 0x04002031 RID: 8241
		protected string _ObjURI;

		// Token: 0x04002032 RID: 8242
		protected string _URL;

		// Token: 0x04002033 RID: 8243
		internal object _objRef;

		// Token: 0x04002034 RID: 8244
		internal object _channelSink;

		// Token: 0x04002035 RID: 8245
		internal object _envoyChain;

		// Token: 0x04002036 RID: 8246
		internal DynamicPropertyHolder _dph;

		// Token: 0x04002037 RID: 8247
		internal Lease _lease;
	}
}
