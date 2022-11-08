using System;
using System.Collections;
using System.Runtime.Remoting.Activation;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000712 RID: 1810
	internal class ConstructorReturnMessage : ReturnMessage, IConstructionReturnMessage, IMethodReturnMessage, IMethodMessage, IMessage
	{
		// Token: 0x06004089 RID: 16521 RVA: 0x000DBC9B File Offset: 0x000DAC9B
		public ConstructorReturnMessage(MarshalByRefObject o, object[] outArgs, int outArgsCount, LogicalCallContext callCtx, IConstructionCallMessage ccm) : base(o, outArgs, outArgsCount, callCtx, ccm)
		{
			this._o = o;
			this._iFlags = 1;
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x000DBCB8 File Offset: 0x000DACB8
		public ConstructorReturnMessage(Exception e, IConstructionCallMessage ccm) : base(e, ccm)
		{
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x0600408B RID: 16523 RVA: 0x000DBCC2 File Offset: 0x000DACC2
		public override object ReturnValue
		{
			get
			{
				if (this._iFlags == 1)
				{
					return RemotingServices.MarshalInternal(this._o, null, null);
				}
				return base.ReturnValue;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x0600408C RID: 16524 RVA: 0x000DBCE4 File Offset: 0x000DACE4
		public override IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					object value = new CRMDictionary(this, new Hashtable());
					Interlocked.CompareExchange(ref this._properties, value, null);
				}
				return (IDictionary)this._properties;
			}
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x000DBD1E File Offset: 0x000DAD1E
		internal object GetObject()
		{
			return this._o;
		}

		// Token: 0x04002098 RID: 8344
		private const int Intercept = 1;

		// Token: 0x04002099 RID: 8345
		private MarshalByRefObject _o;

		// Token: 0x0400209A RID: 8346
		private int _iFlags;
	}
}
