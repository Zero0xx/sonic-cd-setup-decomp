using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x0200041B RID: 1051
	internal class DefaultCertPolicy : ICertificatePolicy
	{
		// Token: 0x060020DB RID: 8411 RVA: 0x00081457 File Offset: 0x00080457
		public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest request, int problem)
		{
			return problem == 0;
		}
	}
}
