using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace System.Security.Cryptography
{
	// Token: 0x02000863 RID: 2147
	[ComVisible(true)]
	[Serializable]
	public class CryptographicException : SystemException
	{
		// Token: 0x06004E66 RID: 20070 RVA: 0x0010FE14 File Offset: 0x0010EE14
		public CryptographicException() : base(Environment.GetResourceString("Arg_CryptographyException"))
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x0010FE31 File Offset: 0x0010EE31
		public CryptographicException(string message) : base(message)
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x0010FE48 File Offset: 0x0010EE48
		public CryptographicException(string format, string insert) : base(string.Format(CultureInfo.CurrentCulture, format, new object[]
		{
			insert
		}))
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x0010FE7D File Offset: 0x0010EE7D
		public CryptographicException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233296);
		}

		// Token: 0x06004E6A RID: 20074 RVA: 0x0010FE92 File Offset: 0x0010EE92
		public CryptographicException(int hr) : this(Win32Native.GetMessage(hr))
		{
			if (((long)hr & (long)((ulong)-2147483648)) != (long)((ulong)-2147483648))
			{
				hr = ((hr & 65535) | -2147024896);
			}
			base.SetErrorCode(hr);
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x0010FEC7 File Offset: 0x0010EEC7
		protected CryptographicException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x0010FED1 File Offset: 0x0010EED1
		private static void ThrowCryptogaphicException(int hr)
		{
			throw new CryptographicException(hr);
		}

		// Token: 0x04002892 RID: 10386
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04002893 RID: 10387
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04002894 RID: 10388
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
	}
}
