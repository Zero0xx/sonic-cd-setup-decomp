using System;
using System.Collections;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200071A RID: 1818
	internal class StackBasedReturnMessage : IMethodReturnMessage, IMethodMessage, IMessage, IInternalMessage
	{
		// Token: 0x060040E1 RID: 16609 RVA: 0x000DCE58 File Offset: 0x000DBE58
		internal StackBasedReturnMessage()
		{
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x000DCE60 File Offset: 0x000DBE60
		internal void InitFields(Message m)
		{
			this._m = m;
			if (this._h != null)
			{
				this._h.Clear();
			}
			if (this._d != null)
			{
				this._d.Clear();
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x000DCE8F File Offset: 0x000DBE8F
		public string Uri
		{
			get
			{
				return this._m.Uri;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x060040E4 RID: 16612 RVA: 0x000DCE9C File Offset: 0x000DBE9C
		public string MethodName
		{
			get
			{
				return this._m.MethodName;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x060040E5 RID: 16613 RVA: 0x000DCEA9 File Offset: 0x000DBEA9
		public string TypeName
		{
			get
			{
				return this._m.TypeName;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x060040E6 RID: 16614 RVA: 0x000DCEB6 File Offset: 0x000DBEB6
		public object MethodSignature
		{
			get
			{
				return this._m.MethodSignature;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x060040E7 RID: 16615 RVA: 0x000DCEC3 File Offset: 0x000DBEC3
		public MethodBase MethodBase
		{
			get
			{
				return this._m.MethodBase;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060040E8 RID: 16616 RVA: 0x000DCED0 File Offset: 0x000DBED0
		public bool HasVarArgs
		{
			get
			{
				return this._m.HasVarArgs;
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060040E9 RID: 16617 RVA: 0x000DCEDD File Offset: 0x000DBEDD
		public int ArgCount
		{
			get
			{
				return this._m.ArgCount;
			}
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x000DCEEA File Offset: 0x000DBEEA
		public object GetArg(int argNum)
		{
			return this._m.GetArg(argNum);
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x000DCEF8 File Offset: 0x000DBEF8
		public string GetArgName(int index)
		{
			return this._m.GetArgName(index);
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060040EC RID: 16620 RVA: 0x000DCF06 File Offset: 0x000DBF06
		public object[] Args
		{
			get
			{
				return this._m.Args;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060040ED RID: 16621 RVA: 0x000DCF13 File Offset: 0x000DBF13
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._m.GetLogicalCallContext();
			}
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x000DCF20 File Offset: 0x000DBF20
		internal LogicalCallContext GetLogicalCallContext()
		{
			return this._m.GetLogicalCallContext();
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x000DCF2D File Offset: 0x000DBF2D
		internal LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
		{
			return this._m.SetLogicalCallContext(callCtx);
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x060040F0 RID: 16624 RVA: 0x000DCF3B File Offset: 0x000DBF3B
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

		// Token: 0x060040F1 RID: 16625 RVA: 0x000DCF5D File Offset: 0x000DBF5D
		public object GetOutArg(int argNum)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArg(argNum);
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x000DCF80 File Offset: 0x000DBF80
		public string GetOutArgName(int index)
		{
			if (this._argMapper == null)
			{
				this._argMapper = new ArgMapper(this, true);
			}
			return this._argMapper.GetArgName(index);
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x060040F3 RID: 16627 RVA: 0x000DCFA3 File Offset: 0x000DBFA3
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

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060040F4 RID: 16628 RVA: 0x000DCFC5 File Offset: 0x000DBFC5
		public Exception Exception
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x060040F5 RID: 16629 RVA: 0x000DCFC8 File Offset: 0x000DBFC8
		public object ReturnValue
		{
			get
			{
				return this._m.GetReturnValue();
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x060040F6 RID: 16630 RVA: 0x000DCFD8 File Offset: 0x000DBFD8
		public IDictionary Properties
		{
			get
			{
				IDictionary d;
				lock (this)
				{
					if (this._h == null)
					{
						this._h = new Hashtable();
					}
					if (this._d == null)
					{
						this._d = new MRMDictionary(this, this._h);
					}
					d = this._d;
				}
				return d;
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x060040F7 RID: 16631 RVA: 0x000DD03C File Offset: 0x000DC03C
		// (set) Token: 0x060040F8 RID: 16632 RVA: 0x000DD03F File Offset: 0x000DC03F
		ServerIdentity IInternalMessage.ServerIdentityObject
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x060040F9 RID: 16633 RVA: 0x000DD041 File Offset: 0x000DC041
		// (set) Token: 0x060040FA RID: 16634 RVA: 0x000DD044 File Offset: 0x000DC044
		Identity IInternalMessage.IdentityObject
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x000DD046 File Offset: 0x000DC046
		void IInternalMessage.SetURI(string val)
		{
			this._m.Uri = val;
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x000DD054 File Offset: 0x000DC054
		void IInternalMessage.SetCallContext(LogicalCallContext newCallContext)
		{
			this._m.SetLogicalCallContext(newCallContext);
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x000DD063 File Offset: 0x000DC063
		bool IInternalMessage.HasProperties()
		{
			return this._h != null;
		}

		// Token: 0x040020B8 RID: 8376
		private Message _m;

		// Token: 0x040020B9 RID: 8377
		private Hashtable _h;

		// Token: 0x040020BA RID: 8378
		private MRMDictionary _d;

		// Token: 0x040020BB RID: 8379
		private ArgMapper _argMapper;
	}
}
