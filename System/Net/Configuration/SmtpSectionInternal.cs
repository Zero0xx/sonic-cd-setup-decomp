using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000661 RID: 1633
	internal sealed class SmtpSectionInternal
	{
		// Token: 0x06003289 RID: 12937 RVA: 0x000D6B0C File Offset: 0x000D5B0C
		internal SmtpSectionInternal(SmtpSection section)
		{
			this.deliveryMethod = section.DeliveryMethod;
			this.from = section.From;
			this.network = new SmtpNetworkElementInternal(section.Network);
			this.specifiedPickupDirectory = new SmtpSpecifiedPickupDirectoryElementInternal(section.SpecifiedPickupDirectory);
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600328A RID: 12938 RVA: 0x000D6B59 File Offset: 0x000D5B59
		internal SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return this.deliveryMethod;
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x000D6B61 File Offset: 0x000D5B61
		internal SmtpNetworkElementInternal Network
		{
			get
			{
				return this.network;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600328C RID: 12940 RVA: 0x000D6B69 File Offset: 0x000D5B69
		internal string From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x000D6B71 File Offset: 0x000D5B71
		internal SmtpSpecifiedPickupDirectoryElementInternal SpecifiedPickupDirectory
		{
			get
			{
				return this.specifiedPickupDirectory;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x000D6B79 File Offset: 0x000D5B79
		internal static object ClassSyncObject
		{
			get
			{
				if (SmtpSectionInternal.classSyncObject == null)
				{
					Interlocked.CompareExchange(ref SmtpSectionInternal.classSyncObject, new object(), null);
				}
				return SmtpSectionInternal.classSyncObject;
			}
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x000D6B98 File Offset: 0x000D5B98
		internal static SmtpSectionInternal GetSection()
		{
			SmtpSectionInternal result;
			lock (SmtpSectionInternal.ClassSyncObject)
			{
				SmtpSection smtpSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SmtpSectionPath) as SmtpSection;
				if (smtpSection == null)
				{
					result = null;
				}
				else
				{
					result = new SmtpSectionInternal(smtpSection);
				}
			}
			return result;
		}

		// Token: 0x04002F49 RID: 12105
		private SmtpDeliveryMethod deliveryMethod;

		// Token: 0x04002F4A RID: 12106
		private string from;

		// Token: 0x04002F4B RID: 12107
		private SmtpNetworkElementInternal network;

		// Token: 0x04002F4C RID: 12108
		private SmtpSpecifiedPickupDirectoryElementInternal specifiedPickupDirectory;

		// Token: 0x04002F4D RID: 12109
		private static object classSyncObject;
	}
}
