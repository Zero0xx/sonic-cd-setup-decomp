using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Cryptography
{
	// Token: 0x02000864 RID: 2148
	[ComVisible(true)]
	[Serializable]
	public class CryptographicUnexpectedOperationException : CryptographicException
	{
		// Token: 0x06004E6D RID: 20077 RVA: 0x0010FED9 File Offset: 0x0010EED9
		public CryptographicUnexpectedOperationException()
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x0010FEEC File Offset: 0x0010EEEC
		public CryptographicUnexpectedOperationException(string message) : base(message)
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x0010FF00 File Offset: 0x0010EF00
		public CryptographicUnexpectedOperationException(string format, string insert) : base(string.Format(CultureInfo.CurrentCulture, format, new object[]
		{
			insert
		}))
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x0010FF35 File Offset: 0x0010EF35
		public CryptographicUnexpectedOperationException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233295);
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x0010FF4A File Offset: 0x0010EF4A
		protected CryptographicUnexpectedOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
