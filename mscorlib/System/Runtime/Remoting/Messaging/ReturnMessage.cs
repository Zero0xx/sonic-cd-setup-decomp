using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000711 RID: 1809
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ReturnMessage : IMethodReturnMessage, IMethodMessage, IMessage
	{
		// Token: 0x06004070 RID: 16496 RVA: 0x000DB8FC File Offset: 0x000DA8FC
		public ReturnMessage(object ret, object[] outArgs, int outArgsCount, LogicalCallContext callCtx, IMethodCallMessage mcm)
		{
			this._ret = ret;
			this._outArgs = outArgs;
			this._outArgsCount = outArgsCount;
			if (callCtx != null)
			{
				this._callContext = callCtx;
			}
			else
			{
				this._callContext = CallContext.GetLogicalCallContext();
			}
			if (mcm != null)
			{
				this._URI = mcm.Uri;
				this._methodName = mcm.MethodName;
				this._methodSignature = null;
				this._typeName = mcm.TypeName;
				this._hasVarArgs = mcm.HasVarArgs;
				this._methodBase = mcm.MethodBase;
			}
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x000DB98C File Offset: 0x000DA98C
		public ReturnMessage(Exception e, IMethodCallMessage mcm)
		{
			this._e = (ReturnMessage.IsCustomErrorEnabled() ? new RemotingException(Environment.GetResourceString("Remoting_InternalError")) : e);
			this._callContext = CallContext.GetLogicalCallContext();
			if (mcm != null)
			{
				this._URI = mcm.Uri;
				this._methodName = mcm.MethodName;
				this._methodSignature = null;
				this._typeName = mcm.TypeName;
				this._hasVarArgs = mcm.HasVarArgs;
				this._methodBase = mcm.MethodBase;
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x000DBA0F File Offset: 0x000DAA0F
		// (set) Token: 0x06004073 RID: 16499 RVA: 0x000DBA17 File Offset: 0x000DAA17
		public string Uri
		{
			get
			{
				return this._URI;
			}
			set
			{
				this._URI = value;
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x000DBA20 File Offset: 0x000DAA20
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x000DBA28 File Offset: 0x000DAA28
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06004076 RID: 16502 RVA: 0x000DBA30 File Offset: 0x000DAA30
		public object MethodSignature
		{
			get
			{
				if (this._methodSignature == null && this._methodBase != null)
				{
					this._methodSignature = Message.GenerateMethodSignature(this._methodBase);
				}
				return this._methodSignature;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x000DBA59 File Offset: 0x000DAA59
		public MethodBase MethodBase
		{
			get
			{
				return this._methodBase;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06004078 RID: 16504 RVA: 0x000DBA61 File Offset: 0x000DAA61
		public bool HasVarArgs
		{
			get
			{
				return this._hasVarArgs;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06004079 RID: 16505 RVA: 0x000DBA69 File Offset: 0x000DAA69
		public int ArgCount
		{
			get
			{
				if (this._outArgs == null)
				{
					return this._outArgsCount;
				}
				return this._outArgs.Length;
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x000DBA84 File Offset: 0x000DAA84
		public object GetArg(int argNum)
		{
			if (this._outArgs == null)
			{
				if (argNum < 0 || argNum >= this._outArgsCount)
				{
					throw new ArgumentOutOfRangeException("argNum");
				}
				return null;
			}
			else
			{
				if (argNum < 0 || argNum >= this._outArgs.Length)
				{
					throw new ArgumentOutOfRangeException("argNum");
				}
				return this._outArgs[argNum];
			}
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x000DBAD8 File Offset: 0x000DAAD8
		public string GetArgName(int index)
		{
			if (this._outArgs == null)
			{
				if (index < 0 || index >= this._outArgsCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
			}
			else if (index < 0 || index >= this._outArgs.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (this._methodBase != null)
			{
				RemotingMethodCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(this._methodBase);
				return reflectionCachedData.Parameters[index].Name;
			}
			return "__param" + index;
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x0600407C RID: 16508 RVA: 0x000DBB51 File Offset: 0x000DAB51
		public object[] Args
		{
			get
			{
				if (this._outArgs == null)
				{
					return new object[this._outArgsCount];
				}
				return this._outArgs;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x0600407D RID: 16509 RVA: 0x000DBB6D File Offset: 0x000DAB6D
		public int OutArgCount
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, true);
				}
				return this._argMapper.ArgCount;
			}
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x000DBB8F File Offset: 0x000DAB8F
		public object GetOutArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x000DBBB2 File Offset: 0x000DABB2
		public string GetOutArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06004080 RID: 16512 RVA: 0x000DBBD5 File Offset: 0x000DABD5
		public object[] OutArgs
		{
			get
			{
				if (this._argMapper == null)
				{
					this._argMapper = new ArgMapper(this, true);
				}
				return this._argMapper.Args;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06004081 RID: 16513 RVA: 0x000DBBF7 File Offset: 0x000DABF7
		public Exception Exception
		{
			get
			{
				return this._e;
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06004082 RID: 16514 RVA: 0x000DBBFF File Offset: 0x000DABFF
		public virtual object ReturnValue
		{
			get
			{
				return this._ret;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06004083 RID: 16515 RVA: 0x000DBC07 File Offset: 0x000DAC07
		public virtual IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					this._properties = new MRMDictionary(this, null);
				}
				return (MRMDictionary)this._properties;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x000DBC29 File Offset: 0x000DAC29
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.GetLogicalCallContext();
			}
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x000DBC31 File Offset: 0x000DAC31
		internal LogicalCallContext GetLogicalCallContext()
		{
			if (this._callContext == null)
			{
				this._callContext = new LogicalCallContext();
			}
			return this._callContext;
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x000DBC4C File Offset: 0x000DAC4C
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext ctx)
		{
			LogicalCallContext callContext = this._callContext;
			this._callContext = ctx;
			return callContext;
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x000DBC68 File Offset: 0x000DAC68
		internal bool HasProperties()
		{
			return this._properties != null;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x000DBC78 File Offset: 0x000DAC78
		internal static bool IsCustomErrorEnabled()
		{
			object data = CallContext.GetData("__CustomErrorsEnabled");
			return data != null && (bool)data;
		}

		// Token: 0x0400208B RID: 8331
		internal object _ret;

		// Token: 0x0400208C RID: 8332
		internal object _properties;

		// Token: 0x0400208D RID: 8333
		internal string _URI;

		// Token: 0x0400208E RID: 8334
		internal Exception _e;

		// Token: 0x0400208F RID: 8335
		internal object[] _outArgs;

		// Token: 0x04002090 RID: 8336
		internal int _outArgsCount;

		// Token: 0x04002091 RID: 8337
		internal string _methodName;

		// Token: 0x04002092 RID: 8338
		internal string _typeName;

		// Token: 0x04002093 RID: 8339
		internal Type[] _methodSignature;

		// Token: 0x04002094 RID: 8340
		internal bool _hasVarArgs;

		// Token: 0x04002095 RID: 8341
		internal LogicalCallContext _callContext;

		// Token: 0x04002096 RID: 8342
		internal ArgMapper _argMapper;

		// Token: 0x04002097 RID: 8343
		internal MethodBase _methodBase;
	}
}
