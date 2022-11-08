using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security
{
	// Token: 0x02000697 RID: 1687
	[ComVisible(true)]
	[Serializable]
	public class VerificationException : SystemException
	{
		// Token: 0x06003D15 RID: 15637 RVA: 0x000D10FF File Offset: 0x000D00FF
		public VerificationException() : base(Environment.GetResourceString("Verification_Exception"))
		{
			base.SetErrorCode(-2146233075);
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x000D111C File Offset: 0x000D011C
		public VerificationException(string message) : base(message)
		{
			base.SetErrorCode(-2146233075);
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x000D1130 File Offset: 0x000D0130
		public VerificationException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233075);
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x000D1145 File Offset: 0x000D0145
		protected VerificationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
