using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Microsoft.Win32;

namespace System.Security.Policy
{
	// Token: 0x02000716 RID: 1814
	internal partial class TrustManagerPromptUI : Form
	{
		// Token: 0x06006077 RID: 24695 RVA: 0x00160278 File Offset: 0x0015F278
		internal TrustManagerPromptUI(string appName, string defaultBrowserExePath, string supportUrl, string deploymentUrl, string publisherName, X509Certificate2 certificate, TrustManagerPromptOptions options)
		{
			this.m_appName = appName;
			this.m_defaultBrowserExePath = defaultBrowserExePath;
			this.m_supportUrl = supportUrl;
			this.m_deploymentUrl = deploymentUrl;
			this.m_publisherName = publisherName;
			this.m_certificate = certificate;
			this.m_options = options;
			this.InitializeComponent();
			this.LoadResources();
		}

		// Token: 0x0600607A RID: 24698 RVA: 0x00160DFC File Offset: 0x0015FDFC
		private void LoadGlobeBitmap()
		{
			Bitmap bitmap;
			lock (typeof(Form))
			{
				bitmap = new Bitmap(typeof(Form), "TrustManagerGlobe.bmp");
			}
			if (bitmap != null)
			{
				bitmap.MakeTransparent();
				this.pictureBoxQuestion.Image = bitmap;
			}
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x00160E60 File Offset: 0x0015FE60
		private void LoadResources()
		{
			base.SuspendAllLayout(this);
			this.LoadGlobeBitmap();
			this.UpdateFonts();
			if ((this.m_options & TrustManagerPromptOptions.StopApp) != TrustManagerPromptOptions.None)
			{
				this.btnInstall.Visible = false;
				this.btnCancel.Text = SR.GetString("TrustManagerPromptUI_Close");
				this.btnCancel.DialogResult = DialogResult.OK;
			}
			else
			{
				if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
				{
					this.btnCancel.Text = SR.GetString("TrustManagerPromptUI_DoNotInstall");
				}
				else
				{
					this.btnCancel.Text = SR.GetString("TrustManagerPromptUI_DoNotRun");
				}
				this.btnInstall.DialogResult = DialogResult.OK;
				this.btnCancel.DialogResult = DialogResult.Cancel;
			}
			this.linkLblName.Links.Clear();
			this.linkLblPublisher.Links.Clear();
			this.linkLblFromUrl.Links.Clear();
			this.linkLblMoreInformation.Links.Clear();
			this.linkLblName.Text = this.m_appName;
			if (this.m_defaultBrowserExePath != null && this.m_certificate != null && this.m_supportUrl != null && this.m_supportUrl.Length > 0)
			{
				this.linkLblName.Links.Add(0, this.m_appName.Length, this.m_supportUrl);
			}
			if (this.linkLblName.Links.Count == 0)
			{
				this.lblName.Text = TrustManagerPromptUI.StripOutAccelerator(this.lblName.Text);
			}
			this.linkLblFromUrl.Text = this.m_deploymentUrl;
			if (this.m_publisherName == null)
			{
				this.linkLblPublisher.Text = SR.GetString("TrustManagerPromptUI_UnknownPublisher");
				if (this.m_certificate != null)
				{
					this.linkLblPublisher.Links.Add(0, this.linkLblPublisher.Text.Length);
				}
			}
			else
			{
				this.linkLblPublisher.Text = this.m_publisherName;
				if (this.m_publisherName.Length > 0)
				{
					this.linkLblPublisher.Links.Add(0, this.m_publisherName.Length);
				}
			}
			if (this.linkLblPublisher.Links.Count == 0)
			{
				this.lblPublisher.Text = TrustManagerPromptUI.StripOutAccelerator(this.lblPublisher.Text);
			}
			if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
			{
				this.Text = SR.GetString("TrustManagerPromptUI_InstallTitle");
			}
			else
			{
				this.Text = SR.GetString("TrustManagerPromptUI_RunTitle");
				this.btnInstall.Text = SR.GetString("TrustManagerPromptUI_Run");
			}
			if ((this.m_options & TrustManagerPromptOptions.StopApp) != TrustManagerPromptOptions.None)
			{
				this.lblQuestion.Text = SR.GetString("TrustManagerPromptUI_BlockedApp");
			}
			else if (this.m_publisherName == null)
			{
				if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
				{
					this.lblQuestion.Text = SR.GetString("TrustManagerPromptUI_NoPublisherInstallQuestion");
				}
				else
				{
					this.lblQuestion.Text = SR.GetString("TrustManagerPromptUI_NoPublisherRunQuestion");
				}
			}
			else if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
			{
				this.lblQuestion.Text = SR.GetString("TrustManagerPromptUI_InstallQuestion");
			}
			else
			{
				this.lblQuestion.Text = SR.GetString("TrustManagerPromptUI_RunQuestion");
			}
			if ((this.m_options & TrustManagerPromptOptions.StopApp) != TrustManagerPromptOptions.None)
			{
				if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
				{
					this.linkLblMoreInformation.Text = SR.GetString("TrustManagerPromptUI_InstalledAppBlockedWarning");
				}
				else
				{
					this.linkLblMoreInformation.Text = SR.GetString("TrustManagerPromptUI_RunAppBlockedWarning");
				}
				this.linkLblMoreInformation.TabStop = false;
				this.linkLblMoreInformation.AccessibleDescription = SR.GetString("TrustManagerPromptUI_WarningAccessibleDescription");
				this.linkLblMoreInformation.AccessibleName = SR.GetString("TrustManagerPromptUI_WarningAccessibleName");
			}
			else
			{
				string @string = SR.GetString("TrustManagerPromptUI_MoreInformation");
				if ((this.m_options & TrustManagerPromptOptions.LocalComputerSource) != TrustManagerPromptOptions.None)
				{
					if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
					{
						this.linkLblMoreInformation.Text = SR.GetString("TrustManagerPromptUI_InstallFromLocalMachineWarning", new object[]
						{
							@string
						});
					}
					else
					{
						this.linkLblMoreInformation.Text = SR.GetString("TrustManagerPromptUI_RunFromLocalMachineWarning", new object[]
						{
							@string
						});
					}
				}
				else if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
				{
					this.linkLblMoreInformation.Text = SR.GetString("TrustManagerPromptUI_InstallWarning", new object[]
					{
						@string
					});
				}
				else
				{
					this.linkLblMoreInformation.Text = SR.GetString("TrustManagerPromptUI_RunWarning", new object[]
					{
						@string
					});
				}
				this.linkLblMoreInformation.TabStop = true;
				this.linkLblMoreInformation.AccessibleDescription = SR.GetString("TrustManagerPromptUI_MoreInformationAccessibleDescription");
				this.linkLblMoreInformation.AccessibleName = SR.GetString("TrustManagerPromptUI_MoreInformationAccessibleName");
				this.linkLblMoreInformation.Links.Add(new LinkLabel.Link(this.linkLblMoreInformation.Text.Length - @string.Length, @string.Length));
			}
			if ((this.m_options & TrustManagerPromptOptions.StopApp) != TrustManagerPromptOptions.None || this.m_publisherName == null)
			{
				if ((this.m_options & TrustManagerPromptOptions.RequiresPermissions) == TrustManagerPromptOptions.None && (this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
				{
					this.LoadWarningBitmap(TrustManagerWarningLevel.Yellow);
				}
				else
				{
					this.LoadWarningBitmap(TrustManagerWarningLevel.Red);
				}
			}
			else if ((this.m_options & TrustManagerPromptOptions.RequiresPermissions) == TrustManagerPromptOptions.None)
			{
				this.LoadWarningBitmap(TrustManagerWarningLevel.Green);
			}
			else
			{
				this.LoadWarningBitmap(TrustManagerWarningLevel.Yellow);
			}
			if ((this.m_options & TrustManagerPromptOptions.StopApp) != TrustManagerPromptOptions.None)
			{
				if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
				{
					base.AccessibleDescription = SR.GetString("TrustManagerPromptUI_AccessibleDescription_InstallBlocked");
				}
				else
				{
					base.AccessibleDescription = SR.GetString("TrustManagerPromptUI_AccessibleDescription_RunBlocked");
				}
			}
			else if ((this.m_options & TrustManagerPromptOptions.RequiresPermissions) != TrustManagerPromptOptions.None)
			{
				if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
				{
					base.AccessibleDescription = SR.GetString("TrustManagerPromptUI_AccessibleDescription_InstallWithElevatedPermissions");
				}
				else
				{
					base.AccessibleDescription = SR.GetString("TrustManagerPromptUI_AccessibleDescription_RunWithElevatedPermissions");
				}
			}
			else if ((this.m_options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
			{
				base.AccessibleDescription = SR.GetString("TrustManagerPromptUI_AccessibleDescription_InstallConfirmation");
			}
			else
			{
				base.AccessibleDescription = SR.GetString("TrustManagerPromptUI_AccessibleDescription_RunConfirmation");
			}
			base.ResumeAllLayout(this, true);
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x001613FC File Offset: 0x001603FC
		private void LoadWarningBitmap(TrustManagerWarningLevel warningLevel)
		{
			Bitmap bitmap;
			switch (warningLevel)
			{
			case TrustManagerWarningLevel.Green:
				bitmap = new Bitmap(typeof(Form), "TrustManagerOK.bmp");
				this.pictureBoxWarning.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_LowRisk"), new object[]
				{
					this.pictureBoxWarning.AccessibleDescription
				});
				break;
			case TrustManagerWarningLevel.Yellow:
				bitmap = new Bitmap(typeof(Form), "TrustManagerWarning.bmp");
				this.pictureBoxWarning.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_MediumRisk"), new object[]
				{
					this.pictureBoxWarning.AccessibleDescription
				});
				break;
			default:
				bitmap = new Bitmap(typeof(Form), "TrustManagerHighRisk.bmp");
				this.pictureBoxWarning.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_HighRisk"), new object[]
				{
					this.pictureBoxWarning.AccessibleDescription
				});
				break;
			}
			if (bitmap != null)
			{
				bitmap.MakeTransparent();
				this.pictureBoxWarning.Image = bitmap;
			}
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x00161520 File Offset: 0x00160520
		private static string StripOutAccelerator(string text)
		{
			int num = text.IndexOf('&');
			if (num == -1)
			{
				return text;
			}
			if (num > 0 && text[num - 1] == '(' && text.Length > num + 2 && text[num + 2] == ')')
			{
				return text.Remove(num - 1, 4);
			}
			return text.Replace("&", "");
		}

		// Token: 0x0600607E RID: 24702 RVA: 0x00161580 File Offset: 0x00160580
		private void TrustManagerPromptUI_Load(object sender, EventArgs e)
		{
			base.ActiveControl = this.btnCancel;
		}

		// Token: 0x0600607F RID: 24703 RVA: 0x00161590 File Offset: 0x00160590
		private void TrustManagerPromptUI_ShowMoreInformation(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				using (TrustManagerMoreInformation trustManagerMoreInformation = new TrustManagerMoreInformation(this.m_options, this.m_publisherName))
				{
					trustManagerMoreInformation.ShowDialog(this);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06006080 RID: 24704 RVA: 0x001615E4 File Offset: 0x001605E4
		private void TrustManagerPromptUI_ShowPublisherCertificate(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				X509Certificate2UI.DisplayCertificate(this.m_certificate, base.Handle);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06006081 RID: 24705 RVA: 0x00161618 File Offset: 0x00160618
		private void TrustManagerPromptUI_ShowSupportPage(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Process.Start(this.m_defaultBrowserExePath, e.Link.LinkData.ToString());
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06006082 RID: 24706 RVA: 0x00161658 File Offset: 0x00160658
		private void TrustManagerPromptUI_VisibleChanged(object sender, EventArgs e)
		{
			if (base.Visible && Form.ActiveForm != this)
			{
				base.Activate();
				base.ActiveControl = this.btnCancel;
			}
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x0016167C File Offset: 0x0016067C
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
		}

		// Token: 0x06006084 RID: 24708 RVA: 0x00161696 File Offset: 0x00160696
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06006085 RID: 24709 RVA: 0x001616B0 File Offset: 0x001606B0
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Window)
			{
				this.UpdateFonts();
			}
			base.Invalidate();
		}

		// Token: 0x06006086 RID: 24710 RVA: 0x001616C8 File Offset: 0x001606C8
		private void UpdateFonts()
		{
			this.Font = SystemFonts.MessageBoxFont;
			this.lblQuestion.Font = (this.linkLblPublisher.Font = (this.linkLblFromUrl.Font = (this.linkLblName.Font = new Font(this.Font, FontStyle.Bold))));
			this.linkLblPublisher.MaximumSize = (this.linkLblFromUrl.MaximumSize = (this.linkLblName.MaximumSize = new Size(0, this.Font.Height + 2)));
		}

		// Token: 0x04003A74 RID: 14964
		private string m_appName;

		// Token: 0x04003A75 RID: 14965
		private string m_defaultBrowserExePath;

		// Token: 0x04003A76 RID: 14966
		private string m_supportUrl;

		// Token: 0x04003A77 RID: 14967
		private string m_deploymentUrl;

		// Token: 0x04003A78 RID: 14968
		private string m_publisherName;

		// Token: 0x04003A79 RID: 14969
		private X509Certificate2 m_certificate;

		// Token: 0x04003A7A RID: 14970
		private TrustManagerPromptOptions m_options;
	}
}
