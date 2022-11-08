using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000436 RID: 1078
	internal class CertPolicyValidationCallback
	{
		// Token: 0x060021AC RID: 8620 RVA: 0x00085649 File Offset: 0x00084649
		internal CertPolicyValidationCallback()
		{
			this.m_CertificatePolicy = new DefaultCertPolicy();
			this.m_Context = null;
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00085663 File Offset: 0x00084663
		internal CertPolicyValidationCallback(ICertificatePolicy certificatePolicy)
		{
			this.m_CertificatePolicy = certificatePolicy;
			this.m_Context = ExecutionContext.Capture();
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x0008567D File Offset: 0x0008467D
		internal ICertificatePolicy CertificatePolicy
		{
			get
			{
				return this.m_CertificatePolicy;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x00085685 File Offset: 0x00084685
		internal bool UsesDefault
		{
			get
			{
				return this.m_Context == null;
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x00085690 File Offset: 0x00084690
		internal void Callback(object state)
		{
			CertPolicyValidationCallback.CallbackContext callbackContext = (CertPolicyValidationCallback.CallbackContext)state;
			callbackContext.result = callbackContext.policyWrapper.CheckErrors(callbackContext.hostName, callbackContext.certificate, callbackContext.chain, callbackContext.sslPolicyErrors);
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000856D0 File Offset: 0x000846D0
		internal bool Invoke(string hostName, ServicePoint servicePoint, X509Certificate certificate, WebRequest request, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			PolicyWrapper policyWrapper = new PolicyWrapper(this.m_CertificatePolicy, servicePoint, request);
			if (this.m_Context == null)
			{
				return policyWrapper.CheckErrors(hostName, certificate, chain, sslPolicyErrors);
			}
			ExecutionContext executionContext = this.m_Context.CreateCopy();
			CertPolicyValidationCallback.CallbackContext callbackContext = new CertPolicyValidationCallback.CallbackContext(policyWrapper, hostName, certificate, chain, sslPolicyErrors);
			ExecutionContext.Run(executionContext, new ContextCallback(this.Callback), callbackContext);
			return callbackContext.result;
		}

		// Token: 0x040021BE RID: 8638
		private ICertificatePolicy m_CertificatePolicy;

		// Token: 0x040021BF RID: 8639
		private ExecutionContext m_Context;

		// Token: 0x02000437 RID: 1079
		private class CallbackContext
		{
			// Token: 0x060021B2 RID: 8626 RVA: 0x00085733 File Offset: 0x00084733
			internal CallbackContext(PolicyWrapper policyWrapper, string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.policyWrapper = policyWrapper;
				this.hostName = hostName;
				this.certificate = certificate;
				this.chain = chain;
				this.sslPolicyErrors = sslPolicyErrors;
			}

			// Token: 0x040021C0 RID: 8640
			internal readonly PolicyWrapper policyWrapper;

			// Token: 0x040021C1 RID: 8641
			internal readonly string hostName;

			// Token: 0x040021C2 RID: 8642
			internal readonly X509Certificate certificate;

			// Token: 0x040021C3 RID: 8643
			internal readonly X509Chain chain;

			// Token: 0x040021C4 RID: 8644
			internal readonly SslPolicyErrors sslPolicyErrors;

			// Token: 0x040021C5 RID: 8645
			internal bool result;
		}
	}
}
