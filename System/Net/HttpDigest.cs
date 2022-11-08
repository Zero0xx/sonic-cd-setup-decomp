using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Net
{
	// Token: 0x020004D5 RID: 1237
	internal static class HttpDigest
	{
		// Token: 0x06002677 RID: 9847 RVA: 0x0009CE3C File Offset: 0x0009BE3C
		static HttpDigest()
		{
			HttpDigest.ReadSuppressExtendedProtectionRegistryValue();
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0009CF68 File Offset: 0x0009BF68
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Lsa")]
		private static void ReadSuppressExtendedProtectionRegistryValue()
		{
			HttpDigest.suppressExtendedProtection = !ComNetOS.IsWin7;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa"))
				{
					try
					{
						if (registryKey.GetValueKind("SuppressExtendedProtection") == RegistryValueKind.DWord)
						{
							HttpDigest.suppressExtendedProtection = ((int)registryKey.GetValue("SuppressExtendedProtection") == 1);
						}
					}
					catch (UnauthorizedAccessException ex)
					{
						if (Logging.On)
						{
							Logging.PrintWarning(Logging.Web, typeof(HttpDigest), "ReadSuppressExtendedProtectionRegistryValue", ex.Message);
						}
					}
					catch (IOException ex2)
					{
						if (Logging.On)
						{
							Logging.PrintWarning(Logging.Web, typeof(HttpDigest), "ReadSuppressExtendedProtectionRegistryValue", ex2.Message);
						}
					}
				}
			}
			catch (SecurityException ex3)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, typeof(HttpDigest), "ReadSuppressExtendedProtectionRegistryValue", ex3.Message);
				}
			}
			catch (ObjectDisposedException ex4)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, typeof(HttpDigest), "ReadSuppressExtendedProtectionRegistryValue", ex4.Message);
				}
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0009D0B4 File Offset: 0x0009C0B4
		internal static HttpDigestChallenge Interpret(string challenge, int startingPoint, HttpWebRequest httpWebRequest)
		{
			HttpDigestChallenge httpDigestChallenge = new HttpDigestChallenge();
			httpDigestChallenge.SetFromRequest(httpWebRequest);
			startingPoint = ((startingPoint == -1) ? 0 : (startingPoint + DigestClient.SignatureSize));
			int num = startingPoint;
			for (;;)
			{
				int num2 = num;
				int num3 = AuthenticationManager.SplitNoQuotes(challenge, ref num2);
				if (num2 < 0)
				{
					goto IL_9E;
				}
				string text = challenge.Substring(num, num2 - num);
				if (string.Compare(text, "charset", StringComparison.OrdinalIgnoreCase) == 0)
				{
					string text2;
					if (num3 < 0)
					{
						text2 = HttpDigest.unquote(challenge.Substring(num2 + 1));
					}
					else
					{
						text2 = HttpDigest.unquote(challenge.Substring(num2 + 1, num3 - num2 - 1));
					}
					if (string.Compare(text2, "utf-8", StringComparison.OrdinalIgnoreCase) == 0)
					{
						break;
					}
				}
				if (num3 < 0)
				{
					goto IL_9E;
				}
				num = num3 + 1;
			}
			httpDigestChallenge.UTF8Charset = true;
			IL_9E:
			num = startingPoint;
			for (;;)
			{
				int num2 = num;
				int num3 = AuthenticationManager.SplitNoQuotes(challenge, ref num2);
				if (num2 < 0)
				{
					break;
				}
				string text = challenge.Substring(num, num2 - num);
				string text2;
				if (num3 < 0)
				{
					text2 = HttpDigest.unquote(challenge.Substring(num2 + 1));
				}
				else
				{
					text2 = HttpDigest.unquote(challenge.Substring(num2 + 1, num3 - num2 - 1));
				}
				if (httpDigestChallenge.UTF8Charset)
				{
					bool flag = true;
					for (int i = 0; i < text2.Length; i++)
					{
						if (text2[i] > '\u007f')
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						byte[] array = new byte[text2.Length];
						for (int j = 0; j < text2.Length; j++)
						{
							array[j] = (byte)text2[j];
						}
						text2 = Encoding.UTF8.GetString(array);
					}
				}
				bool flag2 = httpDigestChallenge.defineAttribute(text, text2);
				if (num3 < 0 || !flag2)
				{
					break;
				}
				num = num3 + 1;
			}
			return httpDigestChallenge;
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0009D248 File Offset: 0x0009C248
		private static string CharsetEncode(string rawString, HttpDigest.Charset charset)
		{
			if (charset == HttpDigest.Charset.UTF8 || charset == HttpDigest.Charset.ANSI)
			{
				byte[] array = (charset == HttpDigest.Charset.UTF8) ? Encoding.UTF8.GetBytes(rawString) : Encoding.Default.GetBytes(rawString);
				char[] array2 = new char[array.Length];
				array.CopyTo(array2, 0);
				rawString = new string(array2);
			}
			return rawString;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0009D294 File Offset: 0x0009C294
		private static HttpDigest.Charset DetectCharset(string rawString)
		{
			HttpDigest.Charset result = HttpDigest.Charset.ASCII;
			for (int i = 0; i < rawString.Length; i++)
			{
				if (rawString[i] > '\u007f')
				{
					byte[] bytes = Encoding.Default.GetBytes(rawString);
					string @string = Encoding.Default.GetString(bytes);
					result = ((string.Compare(rawString, @string, StringComparison.Ordinal) == 0) ? HttpDigest.Charset.ANSI : HttpDigest.Charset.UTF8);
					break;
				}
			}
			return result;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0009D2EC File Offset: 0x0009C2EC
		internal static Authorization Authenticate(HttpDigestChallenge digestChallenge, NetworkCredential NC, string spn, ChannelBinding binding)
		{
			string text = NC.InternalGetUserName();
			if (ValidationHelper.IsBlankString(text))
			{
				return null;
			}
			string text2 = NC.InternalGetPassword();
			bool flag = HttpDigest.IsUpgraded(digestChallenge.Nonce, binding);
			if (flag)
			{
				digestChallenge.ServiceName = spn;
				digestChallenge.ChannelBinding = HttpDigest.hashChannelBinding(binding, digestChallenge.MD5provider);
			}
			if (digestChallenge.QopPresent)
			{
				if (digestChallenge.ClientNonce == null || digestChallenge.Stale)
				{
					if (flag)
					{
						digestChallenge.ClientNonce = HttpDigest.createUpgradedNonce(digestChallenge);
					}
					else
					{
						digestChallenge.ClientNonce = HttpDigest.createNonce(32);
					}
					digestChallenge.NonceCount = 1;
				}
				else
				{
					digestChallenge.NonceCount++;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			HttpDigest.Charset charset = HttpDigest.DetectCharset(text);
			if (!digestChallenge.UTF8Charset && charset == HttpDigest.Charset.UTF8)
			{
				return null;
			}
			HttpDigest.Charset charset2 = HttpDigest.DetectCharset(text2);
			if (!digestChallenge.UTF8Charset && charset2 == HttpDigest.Charset.UTF8)
			{
				return null;
			}
			if (digestChallenge.UTF8Charset)
			{
				stringBuilder.Append(HttpDigest.pair("charset", "utf-8", false));
				stringBuilder.Append(",");
				if (charset == HttpDigest.Charset.UTF8)
				{
					text = HttpDigest.CharsetEncode(text, HttpDigest.Charset.UTF8);
					stringBuilder.Append(HttpDigest.pair("username", text, true));
					stringBuilder.Append(",");
				}
				else
				{
					stringBuilder.Append(HttpDigest.pair("username", HttpDigest.CharsetEncode(text, HttpDigest.Charset.UTF8), true));
					stringBuilder.Append(",");
					text = HttpDigest.CharsetEncode(text, charset);
				}
			}
			else
			{
				text = HttpDigest.CharsetEncode(text, charset);
				stringBuilder.Append(HttpDigest.pair("username", text, true));
				stringBuilder.Append(",");
			}
			text2 = HttpDigest.CharsetEncode(text2, charset2);
			stringBuilder.Append(HttpDigest.pair("realm", digestChallenge.Realm, true));
			stringBuilder.Append(",");
			stringBuilder.Append(HttpDigest.pair("nonce", digestChallenge.Nonce, true));
			stringBuilder.Append(",");
			stringBuilder.Append(HttpDigest.pair("uri", digestChallenge.Uri, true));
			if (digestChallenge.QopPresent)
			{
				if (digestChallenge.Algorithm != null)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(HttpDigest.pair("algorithm", digestChallenge.Algorithm, true));
				}
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("cnonce", digestChallenge.ClientNonce, true));
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("nc", digestChallenge.NonceCount.ToString("x8", NumberFormatInfo.InvariantInfo), false));
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("qop", "auth", true));
				if (flag)
				{
					stringBuilder.Append(",");
					stringBuilder.Append(HttpDigest.pair("hashed-dirs", "service-name,channel-binding", true));
					stringBuilder.Append(",");
					stringBuilder.Append(HttpDigest.pair("service-name", digestChallenge.ServiceName, true));
					stringBuilder.Append(",");
					stringBuilder.Append(HttpDigest.pair("channel-binding", digestChallenge.ChannelBinding, true));
				}
			}
			string text3 = HttpDigest.responseValue(digestChallenge, text, text2);
			if (text3 == null)
			{
				return null;
			}
			stringBuilder.Append(",");
			stringBuilder.Append(HttpDigest.pair("response", text3, true));
			if (digestChallenge.Opaque != null)
			{
				stringBuilder.Append(",");
				stringBuilder.Append(HttpDigest.pair("opaque", digestChallenge.Opaque, true));
			}
			return new Authorization("Digest " + stringBuilder.ToString(), false);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0009D66D File Offset: 0x0009C66D
		private static bool IsUpgraded(string nonce, ChannelBinding binding)
		{
			return (binding != null || !HttpDigest.suppressExtendedProtection) && AuthenticationManager.SspSupportsExtendedProtection && nonce.StartsWith("+Upgraded+", StringComparison.Ordinal);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0009D690 File Offset: 0x0009C690
		internal static string unquote(string quotedString)
		{
			return quotedString.Trim().Trim("\"".ToCharArray());
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0009D6A7 File Offset: 0x0009C6A7
		internal static string pair(string name, string value, bool quote)
		{
			if (quote)
			{
				return name + "=\"" + value + "\"";
			}
			return name + "=" + value;
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0009D6CC File Offset: 0x0009C6CC
		private static string responseValue(HttpDigestChallenge challenge, string username, string password)
		{
			string text = HttpDigest.computeSecret(challenge, username, password);
			if (text == null)
			{
				return null;
			}
			string text2 = challenge.Method + ":" + challenge.Uri;
			if (text2 == null)
			{
				return null;
			}
			string str = HttpDigest.hashString(text, challenge.MD5provider);
			string text3 = HttpDigest.hashString(text2, challenge.MD5provider);
			string str2 = challenge.Nonce + ":" + (challenge.QopPresent ? string.Concat(new string[]
			{
				challenge.NonceCount.ToString("x8", NumberFormatInfo.InvariantInfo),
				":",
				challenge.ClientNonce,
				":auth:",
				text3
			}) : text3);
			return HttpDigest.hashString(str + ":" + str2, challenge.MD5provider);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0009D79C File Offset: 0x0009C79C
		private static string computeSecret(HttpDigestChallenge challenge, string username, string password)
		{
			if (challenge.Algorithm == null || string.Compare(challenge.Algorithm, "md5", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return string.Concat(new string[]
				{
					username,
					":",
					challenge.Realm,
					":",
					password
				});
			}
			if (string.Compare(challenge.Algorithm, "md5-sess", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return string.Concat(new string[]
				{
					HttpDigest.hashString(string.Concat(new string[]
					{
						username,
						":",
						challenge.Realm,
						":",
						password
					}), challenge.MD5provider),
					":",
					challenge.Nonce,
					":",
					challenge.ClientNonce
				});
			}
			throw new NotSupportedException(SR.GetString("net_HashAlgorithmNotSupportedException", new object[]
			{
				challenge.Algorithm
			}));
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0009D894 File Offset: 0x0009C894
		private static byte[] formatChannelBindingForHash(ChannelBinding binding)
		{
			int value = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.InitiatorTypeOffset);
			int num = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.InitiatorLengthOffset);
			int value2 = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.AcceptorTypeOffset);
			int num2 = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.AcceptorLengthOffset);
			int num3 = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.ApplicationDataLengthOffset);
			byte[] array = new byte[HttpDigest.MinimumFormattedBindingLength + num + num2 + num3];
			BitConverter.GetBytes(value).CopyTo(array, 0);
			BitConverter.GetBytes(num).CopyTo(array, HttpDigest.SizeOfInt);
			int num4 = 2 * HttpDigest.SizeOfInt;
			if (num > 0)
			{
				int b = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.InitiatorOffsetOffset);
				Marshal.Copy(IntPtrHelper.Add(binding.DangerousGetHandle(), b), array, num4, num);
				num4 += num;
			}
			BitConverter.GetBytes(value2).CopyTo(array, num4);
			BitConverter.GetBytes(num2).CopyTo(array, num4 + HttpDigest.SizeOfInt);
			num4 += 2 * HttpDigest.SizeOfInt;
			if (num2 > 0)
			{
				int b2 = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.AcceptorOffsetOffset);
				Marshal.Copy(IntPtrHelper.Add(binding.DangerousGetHandle(), b2), array, num4, num2);
				num4 += num2;
			}
			BitConverter.GetBytes(num3).CopyTo(array, num4);
			num4 += HttpDigest.SizeOfInt;
			if (num3 > 0)
			{
				int b3 = Marshal.ReadInt32(binding.DangerousGetHandle(), HttpDigest.ApplicationDataOffsetOffset);
				Marshal.Copy(IntPtrHelper.Add(binding.DangerousGetHandle(), b3), array, num4, num3);
			}
			return array;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0009DA14 File Offset: 0x0009CA14
		private static string hashChannelBinding(ChannelBinding binding, MD5CryptoServiceProvider MD5provider)
		{
			if (binding == null)
			{
				return "00000000000000000000000000000000";
			}
			byte[] buffer = HttpDigest.formatChannelBindingForHash(binding);
			byte[] rawbytes = MD5provider.ComputeHash(buffer);
			return HttpDigest.hexEncode(rawbytes);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0009DA40 File Offset: 0x0009CA40
		private static string hashString(string myString, MD5CryptoServiceProvider MD5provider)
		{
			byte[] array = new byte[myString.Length];
			for (int i = 0; i < myString.Length; i++)
			{
				array[i] = (byte)myString[i];
			}
			byte[] rawbytes = MD5provider.ComputeHash(array);
			return HttpDigest.hexEncode(rawbytes);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0009DA88 File Offset: 0x0009CA88
		private static string hexEncode(byte[] rawbytes)
		{
			int num = rawbytes.Length;
			char[] array = new char[2 * num];
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				array[num2++] = Uri.HexLowerChars[rawbytes[i] >> 4];
				array[num2++] = Uri.HexLowerChars[(int)(rawbytes[i] & 15)];
				i++;
			}
			return new string(array);
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0009DADC File Offset: 0x0009CADC
		private static string createNonce(int length)
		{
			byte[] array = new byte[length];
			char[] array2 = new char[length];
			HttpDigest.RandomGenerator.GetBytes(array);
			for (int i = 0; i < length; i++)
			{
				array2[i] = Uri.HexLowerChars[(int)(array[i] & 15)];
			}
			return new string(array2);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0009DB28 File Offset: 0x0009CB28
		private static string createUpgradedNonce(HttpDigestChallenge digestChallenge)
		{
			string s = digestChallenge.ServiceName + ":" + digestChallenge.ChannelBinding;
			byte[] rawbytes = digestChallenge.MD5provider.ComputeHash(Encoding.ASCII.GetBytes(s));
			return "+Upgraded+v1" + HttpDigest.hexEncode(rawbytes) + HttpDigest.createNonce(32);
		}

		// Token: 0x040025FE RID: 9726
		internal const string DA_algorithm = "algorithm";

		// Token: 0x040025FF RID: 9727
		internal const string DA_cnonce = "cnonce";

		// Token: 0x04002600 RID: 9728
		internal const string DA_domain = "domain";

		// Token: 0x04002601 RID: 9729
		internal const string DA_nc = "nc";

		// Token: 0x04002602 RID: 9730
		internal const string DA_nonce = "nonce";

		// Token: 0x04002603 RID: 9731
		internal const string DA_opaque = "opaque";

		// Token: 0x04002604 RID: 9732
		internal const string DA_qop = "qop";

		// Token: 0x04002605 RID: 9733
		internal const string DA_realm = "realm";

		// Token: 0x04002606 RID: 9734
		internal const string DA_response = "response";

		// Token: 0x04002607 RID: 9735
		internal const string DA_stale = "stale";

		// Token: 0x04002608 RID: 9736
		internal const string DA_uri = "uri";

		// Token: 0x04002609 RID: 9737
		internal const string DA_username = "username";

		// Token: 0x0400260A RID: 9738
		internal const string DA_charset = "charset";

		// Token: 0x0400260B RID: 9739
		internal const string DA_cipher = "cipher";

		// Token: 0x0400260C RID: 9740
		internal const string DA_hasheddirs = "hashed-dirs";

		// Token: 0x0400260D RID: 9741
		internal const string DA_servicename = "service-name";

		// Token: 0x0400260E RID: 9742
		internal const string DA_channelbinding = "channel-binding";

		// Token: 0x0400260F RID: 9743
		internal const string SupportedQuality = "auth";

		// Token: 0x04002610 RID: 9744
		internal const string ValidSeparator = ", \"'\t\r\n";

		// Token: 0x04002611 RID: 9745
		internal const string HashedDirs = "service-name,channel-binding";

		// Token: 0x04002612 RID: 9746
		internal const string Upgraded = "+Upgraded+";

		// Token: 0x04002613 RID: 9747
		internal const string UpgradedV1 = "+Upgraded+v1";

		// Token: 0x04002614 RID: 9748
		internal const string ZeroChannelBindingHash = "00000000000000000000000000000000";

		// Token: 0x04002615 RID: 9749
		private const string suppressExtendedProtectionKey = "System\\CurrentControlSet\\Control\\Lsa";

		// Token: 0x04002616 RID: 9750
		private const string suppressExtendedProtectionKeyPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Lsa";

		// Token: 0x04002617 RID: 9751
		private const string suppressExtendedProtectionValueName = "SuppressExtendedProtection";

		// Token: 0x04002618 RID: 9752
		private static bool suppressExtendedProtection;

		// Token: 0x04002619 RID: 9753
		private static readonly RNGCryptoServiceProvider RandomGenerator = new RNGCryptoServiceProvider();

		// Token: 0x0400261A RID: 9754
		private static int InitiatorTypeOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "dwInitiatorAddrType");

		// Token: 0x0400261B RID: 9755
		private static int InitiatorLengthOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "cbInitiatorLength");

		// Token: 0x0400261C RID: 9756
		private static int InitiatorOffsetOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "dwInitiatorOffset");

		// Token: 0x0400261D RID: 9757
		private static int AcceptorTypeOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "dwAcceptorAddrType");

		// Token: 0x0400261E RID: 9758
		private static int AcceptorLengthOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "cbAcceptorLength");

		// Token: 0x0400261F RID: 9759
		private static int AcceptorOffsetOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "dwAcceptorOffset");

		// Token: 0x04002620 RID: 9760
		private static int ApplicationDataLengthOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "cbApplicationDataLength");

		// Token: 0x04002621 RID: 9761
		private static int ApplicationDataOffsetOffset = (int)Marshal.OffsetOf(typeof(SecChannelBindings), "dwApplicationDataOffset");

		// Token: 0x04002622 RID: 9762
		private static int SizeOfInt = Marshal.SizeOf(typeof(int));

		// Token: 0x04002623 RID: 9763
		private static int MinimumFormattedBindingLength = 5 * HttpDigest.SizeOfInt;

		// Token: 0x020004D6 RID: 1238
		private enum Charset
		{
			// Token: 0x04002625 RID: 9765
			ASCII,
			// Token: 0x04002626 RID: 9766
			ANSI,
			// Token: 0x04002627 RID: 9767
			UTF8
		}
	}
}
