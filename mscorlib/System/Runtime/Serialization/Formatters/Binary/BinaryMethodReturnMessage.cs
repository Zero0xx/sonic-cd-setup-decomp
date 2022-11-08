using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007FD RID: 2045
	[Serializable]
	internal class BinaryMethodReturnMessage
	{
		// Token: 0x0600485A RID: 18522 RVA: 0x000FAD1C File Offset: 0x000F9D1C
		internal BinaryMethodReturnMessage(object returnValue, object[] args, Exception e, LogicalCallContext callContext, object[] properties)
		{
			this._returnValue = returnValue;
			if (args == null)
			{
				args = new object[0];
			}
			this._outargs = args;
			this._args = args;
			this._exception = e;
			if (callContext == null)
			{
				this._logicalCallContext = new LogicalCallContext();
			}
			else
			{
				this._logicalCallContext = callContext;
			}
			this._properties = properties;
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x0600485B RID: 18523 RVA: 0x000FAD77 File Offset: 0x000F9D77
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600485C RID: 18524 RVA: 0x000FAD7F File Offset: 0x000F9D7F
		public object ReturnValue
		{
			get
			{
				return this._returnValue;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600485D RID: 18525 RVA: 0x000FAD87 File Offset: 0x000F9D87
		public object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600485E RID: 18526 RVA: 0x000FAD8F File Offset: 0x000F9D8F
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this._logicalCallContext;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600485F RID: 18527 RVA: 0x000FAD97 File Offset: 0x000F9D97
		public bool HasProperties
		{
			get
			{
				return this._properties != null;
			}
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x000FADA8 File Offset: 0x000F9DA8
		internal void PopulateMessageProperties(IDictionary dict)
		{
			foreach (DictionaryEntry dictionaryEntry in this._properties)
			{
				dict[dictionaryEntry.Key] = dictionaryEntry.Value;
			}
		}

		// Token: 0x0400254B RID: 9547
		private object[] _outargs;

		// Token: 0x0400254C RID: 9548
		private Exception _exception;

		// Token: 0x0400254D RID: 9549
		private object _returnValue;

		// Token: 0x0400254E RID: 9550
		private object[] _args;

		// Token: 0x0400254F RID: 9551
		private LogicalCallContext _logicalCallContext;

		// Token: 0x04002550 RID: 9552
		private object[] _properties;
	}
}
