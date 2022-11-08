using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Win32;

namespace System.Security.Policy
{
	// Token: 0x02000717 RID: 1815
	internal partial class TrustManagerMoreInformation : Form
	{
		// Token: 0x06006087 RID: 24711 RVA: 0x00161760 File Offset: 0x00160760
		internal TrustManagerMoreInformation(TrustManagerPromptOptions options, string publisherName)
		{
			this.InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;
			this.lblMachineAccess.Font = (this.lblPublisher.Font = (this.lblInstallation.Font = (this.lblLocation.Font = new Font(this.lblMachineAccess.Font, FontStyle.Bold))));
			this.FillContent(options, publisherName);
		}

		// Token: 0x06006089 RID: 24713 RVA: 0x001617F4 File Offset: 0x001607F4
		private void FillContent(TrustManagerPromptOptions options, string publisherName)
		{
			TrustManagerMoreInformation.LoadWarningBitmap((publisherName == null) ? TrustManagerWarningLevel.Red : TrustManagerWarningLevel.Green, this.pictureBoxPublisher);
			TrustManagerMoreInformation.LoadWarningBitmap(((options & (TrustManagerPromptOptions.RequiresPermissions | TrustManagerPromptOptions.WillHaveFullTrust)) != TrustManagerPromptOptions.None) ? TrustManagerWarningLevel.Red : TrustManagerWarningLevel.Green, this.pictureBoxMachineAccess);
			TrustManagerMoreInformation.LoadWarningBitmap(((options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None) ? TrustManagerWarningLevel.Yellow : TrustManagerWarningLevel.Green, this.pictureBoxInstallation);
			TrustManagerWarningLevel warningLevel;
			if ((options & (TrustManagerPromptOptions.LocalNetworkSource | TrustManagerPromptOptions.LocalComputerSource | TrustManagerPromptOptions.TrustedSitesSource)) != TrustManagerPromptOptions.None)
			{
				warningLevel = TrustManagerWarningLevel.Green;
			}
			else if ((options & TrustManagerPromptOptions.UntrustedSitesSource) != TrustManagerPromptOptions.None)
			{
				warningLevel = TrustManagerWarningLevel.Red;
			}
			else
			{
				warningLevel = TrustManagerWarningLevel.Yellow;
			}
			TrustManagerMoreInformation.LoadWarningBitmap(warningLevel, this.pictureBoxLocation);
			if (publisherName == null)
			{
				this.lblPublisherContent.Text = SR.GetString("TrustManagerMoreInfo_UnknownPublisher");
			}
			else
			{
				this.lblPublisherContent.Text = SR.GetString("TrustManagerMoreInfo_KnownPublisher", new object[]
				{
					publisherName
				});
			}
			if ((options & (TrustManagerPromptOptions.RequiresPermissions | TrustManagerPromptOptions.WillHaveFullTrust)) != TrustManagerPromptOptions.None)
			{
				this.lblMachineAccessContent.Text = SR.GetString("TrustManagerMoreInfo_UnsafeAccess");
			}
			else
			{
				this.lblMachineAccessContent.Text = SR.GetString("TrustManagerMoreInfo_SafeAccess");
			}
			if ((options & TrustManagerPromptOptions.AddsShortcut) != TrustManagerPromptOptions.None)
			{
				this.Text = SR.GetString("TrustManagerMoreInfo_InstallTitle");
				this.lblInstallationContent.Text = SR.GetString("TrustManagerMoreInfo_WithShortcut");
			}
			else
			{
				this.Text = SR.GetString("TrustManagerMoreInfo_RunTitle");
				this.lblInstallationContent.Text = SR.GetString("TrustManagerMoreInfo_WithoutShortcut");
			}
			string @string;
			if ((options & TrustManagerPromptOptions.LocalNetworkSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_LocalNetworkSource");
			}
			else if ((options & TrustManagerPromptOptions.LocalComputerSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_LocalComputerSource");
			}
			else if ((options & TrustManagerPromptOptions.InternetSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_InternetSource");
			}
			else if ((options & TrustManagerPromptOptions.TrustedSitesSource) != TrustManagerPromptOptions.None)
			{
				@string = SR.GetString("TrustManagerMoreInfo_TrustedSitesSource");
			}
			else
			{
				@string = SR.GetString("TrustManagerMoreInfo_UntrustedSitesSource");
			}
			this.lblLocationContent.Text = SR.GetString("TrustManagerMoreInfo_Location", new object[]
			{
				@string
			});
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x0016213C File Offset: 0x0016113C
		private static void LoadWarningBitmap(TrustManagerWarningLevel warningLevel, PictureBox pictureBox)
		{
			Bitmap bitmap;
			switch (warningLevel)
			{
			case TrustManagerWarningLevel.Green:
				bitmap = new Bitmap(typeof(Form), "TrustManagerOKSm.bmp");
				pictureBox.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_LowRisk"), new object[]
				{
					pictureBox.AccessibleDescription
				});
				break;
			case TrustManagerWarningLevel.Yellow:
				bitmap = new Bitmap(typeof(Form), "TrustManagerWarningSm.bmp");
				pictureBox.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_MediumRisk"), new object[]
				{
					pictureBox.AccessibleDescription
				});
				break;
			default:
				bitmap = new Bitmap(typeof(Form), "TrustManagerHighRiskSm.bmp");
				pictureBox.AccessibleDescription = string.Format(CultureInfo.CurrentCulture, SR.GetString("TrustManager_WarningIconAccessibleDescription_HighRisk"), new object[]
				{
					pictureBox.AccessibleDescription
				});
				break;
			}
			if (bitmap != null)
			{
				bitmap.MakeTransparent();
				pictureBox.Image = bitmap;
			}
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x0016223A File Offset: 0x0016123A
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x00162254 File Offset: 0x00161254
		protected override void OnHandleDestroyed(EventArgs e)
		{
			SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x00162270 File Offset: 0x00161270
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Window)
			{
				this.Font = SystemFonts.MessageBoxFont;
				this.lblLocation.Font = (this.lblInstallation.Font = (this.lblMachineAccess.Font = (this.lblPublisher.Font = new Font(this.Font, FontStyle.Bold))));
			}
			base.Invalidate();
		}
	}
}
