using System;
using System.Globalization;
using System.Text;

namespace System.Net
{
	// Token: 0x020004B4 RID: 1204
	internal class BasicClient : IAuthenticationModule
	{
		// Token: 0x06002542 RID: 9538 RVA: 0x00094B08 File Offset: 0x00093B08
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || credentials is SystemNetworkCredential)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null || httpWebRequest.ChallengedUri == null)
			{
				return null;
			}
			int num = AuthenticationManager.FindSubstringNotInQuotes(challenge, BasicClient.Signature);
			if (num < 0)
			{
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002543 RID: 9539 RVA: 0x00094B56 File Offset: 0x00093B56
		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x00094B5C File Offset: 0x00093B5C
		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || credentials is SystemNetworkCredential)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			return this.Lookup(httpWebRequest, credentials);
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002545 RID: 9541 RVA: 0x00094B8A File Offset: 0x00093B8A
		public string AuthenticationType
		{
			get
			{
				return "Basic";
			}
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x00094B94 File Offset: 0x00093B94
		private Authorization Lookup(HttpWebRequest httpWebRequest, ICredentials credentials)
		{
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.ChallengedUri, BasicClient.Signature);
			if (credential == null)
			{
				return null;
			}
			ICredentialPolicy credentialPolicy = AuthenticationManager.CredentialPolicy;
			if (credentialPolicy != null && !credentialPolicy.ShouldSendCredential(httpWebRequest.ChallengedUri, httpWebRequest, credential, this))
			{
				return null;
			}
			string text = credential.InternalGetUserName();
			string text2 = credential.InternalGetDomain();
			if (ValidationHelper.IsBlankString(text))
			{
				return null;
			}
			string rawString = ((!ValidationHelper.IsBlankString(text2)) ? (text2 + "\\") : "") + text + ":" + credential.InternalGetPassword();
			byte[] inArray = BasicClient.EncodingRightGetBytes(rawString);
			string token = "Basic " + Convert.ToBase64String(inArray);
			return new Authorization(token, true);
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x00094C40 File Offset: 0x00093C40
		internal static byte[] EncodingRightGetBytes(string rawString)
		{
			byte[] bytes = Encoding.Default.GetBytes(rawString);
			string @string = Encoding.Default.GetString(bytes);
			if (string.Compare(rawString, @string, StringComparison.Ordinal) != 0)
			{
				throw ExceptionHelper.MethodNotSupportedException;
			}
			return bytes;
		}

		// Token: 0x0400250F RID: 9487
		internal const string AuthType = "Basic";

		// Token: 0x04002510 RID: 9488
		internal static string Signature = "Basic".ToLower(CultureInfo.InvariantCulture);

		// Token: 0x04002511 RID: 9489
		internal static int SignatureSize = BasicClient.Signature.Length;
	}
}
