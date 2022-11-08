using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace System.Security.Policy
{
	// Token: 0x02000711 RID: 1809
	internal class TrustManagerPromptUIThread
	{
		// Token: 0x06006060 RID: 24672 RVA: 0x0016007C File Offset: 0x0015F07C
		public TrustManagerPromptUIThread(string appName, string defaultBrowserExePath, string supportUrl, string deploymentUrl, string publisherName, X509Certificate2 certificate, TrustManagerPromptOptions options)
		{
			this.m_appName = appName;
			this.m_defaultBrowserExePath = defaultBrowserExePath;
			this.m_supportUrl = supportUrl;
			this.m_deploymentUrl = deploymentUrl;
			this.m_publisherName = publisherName;
			this.m_certificate = certificate;
			this.m_options = options;
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x001600CC File Offset: 0x0015F0CC
		public DialogResult ShowDialog()
		{
			Thread thread = new Thread(new ThreadStart(this.ShowDialogWork));
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			return this.m_ret;
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x00160104 File Offset: 0x0015F104
		private void ShowDialogWork()
		{
			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				using (TrustManagerPromptUI trustManagerPromptUI = new TrustManagerPromptUI(this.m_appName, this.m_defaultBrowserExePath, this.m_supportUrl, this.m_deploymentUrl, this.m_publisherName, this.m_certificate, this.m_options))
				{
					this.m_ret = trustManagerPromptUI.ShowDialog();
				}
			}
			catch
			{
			}
		}

		// Token: 0x04003A41 RID: 14913
		private string m_appName;

		// Token: 0x04003A42 RID: 14914
		private string m_defaultBrowserExePath;

		// Token: 0x04003A43 RID: 14915
		private string m_supportUrl;

		// Token: 0x04003A44 RID: 14916
		private string m_deploymentUrl;

		// Token: 0x04003A45 RID: 14917
		private string m_publisherName;

		// Token: 0x04003A46 RID: 14918
		private X509Certificate2 m_certificate;

		// Token: 0x04003A47 RID: 14919
		private TrustManagerPromptOptions m_options;

		// Token: 0x04003A48 RID: 14920
		private DialogResult m_ret = DialogResult.No;
	}
}
