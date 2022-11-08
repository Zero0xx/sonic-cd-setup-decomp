using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050C RID: 1292
	[ComVisible(true)]
	[Serializable]
	public class COMException : ExternalException
	{
		// Token: 0x060031AE RID: 12718 RVA: 0x000A99D8 File Offset: 0x000A89D8
		public COMException() : base(Environment.GetResourceString("Arg_COMException"))
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000A99F5 File Offset: 0x000A89F5
		public COMException(string message) : base(message)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000A9A09 File Offset: 0x000A8A09
		public COMException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000A9A1E File Offset: 0x000A8A1E
		public COMException(string message, int errorCode) : base(message)
		{
			base.SetErrorCode(errorCode);
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000A9A2E File Offset: 0x000A8A2E
		protected COMException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000A9A38 File Offset: 0x000A8A38
		public override string ToString()
		{
			string message = this.Message;
			string str = base.GetType().ToString();
			string text = str + " (0x" + base.HResult.ToString("X8", CultureInfo.InvariantCulture) + ")";
			if (message != null && message.Length > 0)
			{
				text = text + ": " + message;
			}
			Exception innerException = base.InnerException;
			if (innerException != null)
			{
				text = text + " ---> " + innerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			return text;
		}
	}
}
