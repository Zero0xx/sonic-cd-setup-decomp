using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007BD RID: 1981
	[SoapType(Embedded = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ServerFault
	{
		// Token: 0x06004695 RID: 18069 RVA: 0x000F0A15 File Offset: 0x000EFA15
		internal ServerFault(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x000F0A24 File Offset: 0x000EFA24
		public ServerFault(string exceptionType, string message, string stackTrace)
		{
			this.exceptionType = exceptionType;
			this.message = message;
			this.stackTrace = stackTrace;
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004697 RID: 18071 RVA: 0x000F0A41 File Offset: 0x000EFA41
		// (set) Token: 0x06004698 RID: 18072 RVA: 0x000F0A49 File Offset: 0x000EFA49
		public string ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
			set
			{
				this.exceptionType = value;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004699 RID: 18073 RVA: 0x000F0A52 File Offset: 0x000EFA52
		// (set) Token: 0x0600469A RID: 18074 RVA: 0x000F0A5A File Offset: 0x000EFA5A
		public string ExceptionMessage
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x0600469B RID: 18075 RVA: 0x000F0A63 File Offset: 0x000EFA63
		// (set) Token: 0x0600469C RID: 18076 RVA: 0x000F0A6B File Offset: 0x000EFA6B
		public string StackTrace
		{
			get
			{
				return this.stackTrace;
			}
			set
			{
				this.stackTrace = value;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600469D RID: 18077 RVA: 0x000F0A74 File Offset: 0x000EFA74
		internal Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x04002318 RID: 8984
		private string exceptionType;

		// Token: 0x04002319 RID: 8985
		private string message;

		// Token: 0x0400231A RID: 8986
		private string stackTrace;

		// Token: 0x0400231B RID: 8987
		private Exception exception;
	}
}
