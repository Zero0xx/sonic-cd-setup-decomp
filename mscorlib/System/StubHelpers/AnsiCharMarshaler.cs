using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

namespace System.StubHelpers
{
	// Token: 0x02000118 RID: 280
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static class AnsiCharMarshaler
	{
		// Token: 0x06001028 RID: 4136 RVA: 0x0002DEEC File Offset: 0x0002CEEC
		internal static byte[] DoAnsiConversion(string str, bool fBestFit, bool fThrowOnUnmappableChar)
		{
			byte[] result = null;
			Encoding encoding = null;
			bool flag = false;
			if (fThrowOnUnmappableChar)
			{
				if (fBestFit)
				{
					flag = true;
				}
				else
				{
					encoding = Encoding.GetEncoding(0, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
				}
			}
			else if (fBestFit)
			{
				encoding = Encoding.Default;
			}
			else
			{
				encoding = Encoding.GetEncoding(0, EncoderFallback.ReplacementFallback, DecoderFallback.ReplacementFallback);
			}
			if (flag)
			{
				result = str.ConvertToAnsi_BestFit_Throw(Marshal.SystemMaxDBCSCharSize);
			}
			else
			{
				try
				{
					result = encoding.GetBytes(str);
				}
				catch (EncoderFallbackException innerException)
				{
					throw new ArgumentException(Environment.GetResourceString("Interop_Marshal_Unmappable_Char"), innerException);
				}
			}
			return result;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0002DF78 File Offset: 0x0002CF78
		internal static byte ConvertToNative(char managedChar, bool fBestFit, bool fThrowOnUnmappableChar)
		{
			byte[] array = AnsiCharMarshaler.DoAnsiConversion(managedChar.ToString(), fBestFit, fThrowOnUnmappableChar);
			return array[0];
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0002DF98 File Offset: 0x0002CF98
		internal static char ConvertToManaged(byte nativeChar)
		{
			byte[] bytes = new byte[]
			{
				nativeChar
			};
			string @string = Encoding.Default.GetString(bytes);
			return @string[0];
		}
	}
}
