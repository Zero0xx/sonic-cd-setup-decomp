using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x020003E7 RID: 999
	public interface ICertificatePolicy
	{
		// Token: 0x06002069 RID: 8297
		bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem);
	}
}
