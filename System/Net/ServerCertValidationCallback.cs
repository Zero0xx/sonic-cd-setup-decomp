using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000438 RID: 1080
	internal class ServerCertValidationCallback
	{
		// Token: 0x060021B3 RID: 8627 RVA: 0x00085760 File Offset: 0x00084760
		internal ServerCertValidationCallback(RemoteCertificateValidationCallback validationCallback)
		{
			this.m_ValidationCallback = validationCallback;
			this.m_Context = ExecutionContext.Capture();
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x0008577A File Offset: 0x0008477A
		internal RemoteCertificateValidationCallback ValidationCallback
		{
			get
			{
				return this.m_ValidationCallback;
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00085784 File Offset: 0x00084784
		internal void Callback(object state)
		{
			ServerCertValidationCallback.CallbackContext callbackContext = (ServerCertValidationCallback.CallbackContext)state;
			callbackContext.result = this.m_ValidationCallback(callbackContext.request, callbackContext.certificate, callbackContext.chain, callbackContext.sslPolicyErrors);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000857C4 File Offset: 0x000847C4
		internal bool Invoke(object request, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (this.m_Context == null)
			{
				return this.m_ValidationCallback(request, certificate, chain, sslPolicyErrors);
			}
			ExecutionContext executionContext = this.m_Context.CreateCopy();
			ServerCertValidationCallback.CallbackContext callbackContext = new ServerCertValidationCallback.CallbackContext(request, certificate, chain, sslPolicyErrors);
			ExecutionContext.Run(executionContext, new ContextCallback(this.Callback), callbackContext);
			return callbackContext.result;
		}

		// Token: 0x040021C6 RID: 8646
		private RemoteCertificateValidationCallback m_ValidationCallback;

		// Token: 0x040021C7 RID: 8647
		private ExecutionContext m_Context;

		// Token: 0x02000439 RID: 1081
		private class CallbackContext
		{
			// Token: 0x060021B7 RID: 8631 RVA: 0x0008581A File Offset: 0x0008481A
			internal CallbackContext(object request, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.request = request;
				this.certificate = certificate;
				this.chain = chain;
				this.sslPolicyErrors = sslPolicyErrors;
			}

			// Token: 0x040021C8 RID: 8648
			internal readonly object request;

			// Token: 0x040021C9 RID: 8649
			internal readonly X509Certificate certificate;

			// Token: 0x040021CA RID: 8650
			internal readonly X509Chain chain;

			// Token: 0x040021CB RID: 8651
			internal readonly SslPolicyErrors sslPolicyErrors;

			// Token: 0x040021CC RID: 8652
			internal bool result;
		}
	}
}
