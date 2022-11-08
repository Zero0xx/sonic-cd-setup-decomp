using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000728 RID: 1832
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class MethodReturnMessageWrapper : InternalMessageWrapper, IMethodReturnMessage, IMethodMessage, IMessage
	{
		// Token: 0x060041B1 RID: 16817 RVA: 0x000DFAEC File Offset: 0x000DEAEC
		public MethodReturnMessageWrapper(IMethodReturnMessage msg) : base(msg)
		{
			this._msg = msg;
			this._args = this._msg.Args;
			this._returnValue = this._msg.ReturnValue;
			this._exception = this._msg.Exception;
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x000DFB3A File Offset: 0x000DEB3A
		// (set) Token: 0x060041B3 RID: 16819 RVA: 0x000DFB47 File Offset: 0x000DEB47
		public string Uri
		{
			get
			{
				return this._msg.Uri;
			}
			set
			{
				this._msg.Properties[Message.UriKey] = value;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x000DFB5F File Offset: 0x000DEB5F
		public virtual string MethodName
		{
			get
			{
				return this._msg.MethodName;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x060041B5 RID: 16821 RVA: 0x000DFB6C File Offset: 0x000DEB6C
		public virtual string TypeName
		{
			get
			{
				return this._msg.TypeName;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x000DFB79 File Offset: 0x000DEB79
		public virtual object MethodSignature
		{
			get
			{
				return this._msg.MethodSignature;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x000DFB86 File Offset: 0x000DEB86
		public virtual LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._msg.LogicalCallContext;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x000DFB93 File Offset: 0x000DEB93
		public virtual MethodBase MethodBase
		{
			get
			{
				return this._msg.MethodBase;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060041B9 RID: 16825 RVA: 0x000DFBA0 File Offset: 0x000DEBA0
		public virtual int ArgCount
		{
			get
			{
				if (this._args != null)
				{
					return this._args.Length;
				}
				return 0;
			}
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x000DFBB4 File Offset: 0x000DEBB4
		public virtual string GetArgName(int index)
		{
			return this._msg.GetArgName(index);
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x000DFBC2 File Offset: 0x000DEBC2
		public virtual object GetArg(int argNum)
		{
			return this._args[argNum];
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x000DFBCC File Offset: 0x000DEBCC
		// (set) Token: 0x060041BD RID: 16829 RVA: 0x000DFBD4 File Offset: 0x000DEBD4
		public virtual object[] Args
		{
			get
			{
				return this._args;
			}
			set
			{
				this._args = value;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x000DFBDD File Offset: 0x000DEBDD
		public virtual bool HasVarArgs
		{
			get
			{
				return this._msg.HasVarArgs;
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060041BF RID: 16831 RVA: 0x000DFBEA File Offset: 0x000DEBEA
		public virtual int OutArgCount
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

		// Token: 0x060041C0 RID: 16832 RVA: 0x000DFC0C File Offset: 0x000DEC0C
		public virtual object GetOutArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x000DFC2F File Offset: 0x000DEC2F
		public virtual string GetOutArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x000DFC52 File Offset: 0x000DEC52
		public virtual object[] OutArgs
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

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x000DFC74 File Offset: 0x000DEC74
		// (set) Token: 0x060041C4 RID: 16836 RVA: 0x000DFC7C File Offset: 0x000DEC7C
		public virtual Exception Exception
		{
			get
			{
				return this._exception;
			}
			set
			{
				this._exception = value;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060041C5 RID: 16837 RVA: 0x000DFC85 File Offset: 0x000DEC85
		// (set) Token: 0x060041C6 RID: 16838 RVA: 0x000DFC8D File Offset: 0x000DEC8D
		public virtual object ReturnValue
		{
			get
			{
				return this._returnValue;
			}
			set
			{
				this._returnValue = value;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x000DFC96 File Offset: 0x000DEC96
		public virtual IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					this._properties = new MethodReturnMessageWrapper.MRMWrapperDictionary(this, this._msg.Properties);
				}
				return this._properties;
			}
		}

		// Token: 0x040020FC RID: 8444
		private IMethodReturnMessage _msg;

		// Token: 0x040020FD RID: 8445
		private IDictionary _properties;

		// Token: 0x040020FE RID: 8446
		private ArgMapper _argMapper;

		// Token: 0x040020FF RID: 8447
		private object[] _args;

		// Token: 0x04002100 RID: 8448
		private object _returnValue;

		// Token: 0x04002101 RID: 8449
		private Exception _exception;

		// Token: 0x02000729 RID: 1833
		private class MRMWrapperDictionary : Hashtable
		{
			// Token: 0x060041C8 RID: 16840 RVA: 0x000DFCBD File Offset: 0x000DECBD
			public MRMWrapperDictionary(IMethodReturnMessage msg, IDictionary idict)
			{
				this._mrmsg = msg;
				this._idict = idict;
			}

			// Token: 0x17000B8F RID: 2959
			public override object this[object key]
			{
				get
				{
					string text = key as string;
					string a;
					if (text != null && (a = text) != null)
					{
						if (a == "__Uri")
						{
							return this._mrmsg.Uri;
						}
						if (a == "__MethodName")
						{
							return this._mrmsg.MethodName;
						}
						if (a == "__MethodSignature")
						{
							return this._mrmsg.MethodSignature;
						}
						if (a == "__TypeName")
						{
							return this._mrmsg.TypeName;
						}
						if (a == "__Return")
						{
							return this._mrmsg.ReturnValue;
						}
						if (a == "__OutArgs")
						{
							return this._mrmsg.OutArgs;
						}
					}
					return this._idict[key];
				}
				set
				{
					string text = key as string;
					if (text != null)
					{
						string a;
						if ((a = text) != null && (a == "__MethodName" || a == "__MethodSignature" || a == "__TypeName" || a == "__Return" || a == "__OutArgs"))
						{
							throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
						}
						this._idict[key] = value;
					}
				}
			}

			// Token: 0x04002102 RID: 8450
			private IMethodReturnMessage _mrmsg;

			// Token: 0x04002103 RID: 8451
			private IDictionary _idict;
		}
	}
}
