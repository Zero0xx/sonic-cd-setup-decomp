using System;

namespace System.Net
{
	// Token: 0x020003DC RID: 988
	public enum HttpStatusCode
	{
		// Token: 0x04001F04 RID: 7940
		Continue = 100,
		// Token: 0x04001F05 RID: 7941
		SwitchingProtocols,
		// Token: 0x04001F06 RID: 7942
		OK = 200,
		// Token: 0x04001F07 RID: 7943
		Created,
		// Token: 0x04001F08 RID: 7944
		Accepted,
		// Token: 0x04001F09 RID: 7945
		NonAuthoritativeInformation,
		// Token: 0x04001F0A RID: 7946
		NoContent,
		// Token: 0x04001F0B RID: 7947
		ResetContent,
		// Token: 0x04001F0C RID: 7948
		PartialContent,
		// Token: 0x04001F0D RID: 7949
		MultipleChoices = 300,
		// Token: 0x04001F0E RID: 7950
		Ambiguous = 300,
		// Token: 0x04001F0F RID: 7951
		MovedPermanently,
		// Token: 0x04001F10 RID: 7952
		Moved = 301,
		// Token: 0x04001F11 RID: 7953
		Found,
		// Token: 0x04001F12 RID: 7954
		Redirect = 302,
		// Token: 0x04001F13 RID: 7955
		SeeOther,
		// Token: 0x04001F14 RID: 7956
		RedirectMethod = 303,
		// Token: 0x04001F15 RID: 7957
		NotModified,
		// Token: 0x04001F16 RID: 7958
		UseProxy,
		// Token: 0x04001F17 RID: 7959
		Unused,
		// Token: 0x04001F18 RID: 7960
		TemporaryRedirect,
		// Token: 0x04001F19 RID: 7961
		RedirectKeepVerb = 307,
		// Token: 0x04001F1A RID: 7962
		BadRequest = 400,
		// Token: 0x04001F1B RID: 7963
		Unauthorized,
		// Token: 0x04001F1C RID: 7964
		PaymentRequired,
		// Token: 0x04001F1D RID: 7965
		Forbidden,
		// Token: 0x04001F1E RID: 7966
		NotFound,
		// Token: 0x04001F1F RID: 7967
		MethodNotAllowed,
		// Token: 0x04001F20 RID: 7968
		NotAcceptable,
		// Token: 0x04001F21 RID: 7969
		ProxyAuthenticationRequired,
		// Token: 0x04001F22 RID: 7970
		RequestTimeout,
		// Token: 0x04001F23 RID: 7971
		Conflict,
		// Token: 0x04001F24 RID: 7972
		Gone,
		// Token: 0x04001F25 RID: 7973
		LengthRequired,
		// Token: 0x04001F26 RID: 7974
		PreconditionFailed,
		// Token: 0x04001F27 RID: 7975
		RequestEntityTooLarge,
		// Token: 0x04001F28 RID: 7976
		RequestUriTooLong,
		// Token: 0x04001F29 RID: 7977
		UnsupportedMediaType,
		// Token: 0x04001F2A RID: 7978
		RequestedRangeNotSatisfiable,
		// Token: 0x04001F2B RID: 7979
		ExpectationFailed,
		// Token: 0x04001F2C RID: 7980
		InternalServerError = 500,
		// Token: 0x04001F2D RID: 7981
		NotImplemented,
		// Token: 0x04001F2E RID: 7982
		BadGateway,
		// Token: 0x04001F2F RID: 7983
		ServiceUnavailable,
		// Token: 0x04001F30 RID: 7984
		GatewayTimeout,
		// Token: 0x04001F31 RID: 7985
		HttpVersionNotSupported
	}
}
