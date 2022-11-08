using System;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200065D RID: 1629
	internal sealed class SettingsSectionInternal
	{
		// Token: 0x06003255 RID: 12885 RVA: 0x000D6378 File Offset: 0x000D5378
		internal SettingsSectionInternal(SettingsSection section)
		{
			if (section == null)
			{
				section = new SettingsSection();
			}
			this.alwaysUseCompletionPortsForConnect = section.Socket.AlwaysUseCompletionPortsForConnect;
			this.alwaysUseCompletionPortsForAccept = section.Socket.AlwaysUseCompletionPortsForAccept;
			this.checkCertificateName = section.ServicePointManager.CheckCertificateName;
			this.CheckCertificateRevocationList = section.ServicePointManager.CheckCertificateRevocationList;
			this.DnsRefreshTimeout = section.ServicePointManager.DnsRefreshTimeout;
			this.ipv6Enabled = section.Ipv6.Enabled;
			this.EnableDnsRoundRobin = section.ServicePointManager.EnableDnsRoundRobin;
			this.Expect100Continue = section.ServicePointManager.Expect100Continue;
			this.maximumUnauthorizedUploadLength = section.HttpWebRequest.MaximumUnauthorizedUploadLength;
			this.maximumResponseHeadersLength = section.HttpWebRequest.MaximumResponseHeadersLength;
			this.maximumErrorResponseLength = section.HttpWebRequest.MaximumErrorResponseLength;
			this.useUnsafeHeaderParsing = section.HttpWebRequest.UseUnsafeHeaderParsing;
			this.UseNagleAlgorithm = section.ServicePointManager.UseNagleAlgorithm;
			TimeSpan t = section.WebProxyScript.DownloadTimeout;
			this.downloadTimeout = ((t == TimeSpan.MaxValue || t == TimeSpan.Zero) ? -1 : ((int)t.TotalMilliseconds));
			this.performanceCountersEnabled = section.PerformanceCounters.Enabled;
			NetworkingPerfCounters.Initialize();
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06003256 RID: 12886 RVA: 0x000D64C0 File Offset: 0x000D54C0
		internal static SettingsSectionInternal Section
		{
			get
			{
				if (SettingsSectionInternal.s_settings == null)
				{
					lock (SettingsSectionInternal.InternalSyncObject)
					{
						if (SettingsSectionInternal.s_settings == null)
						{
							SettingsSectionInternal.s_settings = new SettingsSectionInternal((SettingsSection)PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SettingsSectionPath));
						}
					}
				}
				return SettingsSectionInternal.s_settings;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x000D6520 File Offset: 0x000D5520
		private static object InternalSyncObject
		{
			get
			{
				if (SettingsSectionInternal.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref SettingsSectionInternal.s_InternalSyncObject, value, null);
				}
				return SettingsSectionInternal.s_InternalSyncObject;
			}
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x000D654C File Offset: 0x000D554C
		internal static SettingsSectionInternal GetSection()
		{
			return new SettingsSectionInternal((SettingsSection)PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SettingsSectionPath));
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x000D6562 File Offset: 0x000D5562
		internal bool AlwaysUseCompletionPortsForAccept
		{
			get
			{
				return this.alwaysUseCompletionPortsForAccept;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x000D656A File Offset: 0x000D556A
		internal bool AlwaysUseCompletionPortsForConnect
		{
			get
			{
				return this.alwaysUseCompletionPortsForConnect;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x000D6572 File Offset: 0x000D5572
		internal bool CheckCertificateName
		{
			get
			{
				return this.checkCertificateName;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x000D657A File Offset: 0x000D557A
		// (set) Token: 0x0600325D RID: 12893 RVA: 0x000D6582 File Offset: 0x000D5582
		internal bool CheckCertificateRevocationList
		{
			get
			{
				return this.checkCertificateRevocationList;
			}
			set
			{
				this.checkCertificateRevocationList = value;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x0600325E RID: 12894 RVA: 0x000D658B File Offset: 0x000D558B
		// (set) Token: 0x0600325F RID: 12895 RVA: 0x000D6593 File Offset: 0x000D5593
		internal int DnsRefreshTimeout
		{
			get
			{
				return this.dnsRefreshTimeout;
			}
			set
			{
				this.dnsRefreshTimeout = value;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x000D659C File Offset: 0x000D559C
		internal int DownloadTimeout
		{
			get
			{
				return this.downloadTimeout;
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06003261 RID: 12897 RVA: 0x000D65A4 File Offset: 0x000D55A4
		// (set) Token: 0x06003262 RID: 12898 RVA: 0x000D65AC File Offset: 0x000D55AC
		internal bool EnableDnsRoundRobin
		{
			get
			{
				return this.enableDnsRoundRobin;
			}
			set
			{
				this.enableDnsRoundRobin = value;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x000D65B5 File Offset: 0x000D55B5
		// (set) Token: 0x06003264 RID: 12900 RVA: 0x000D65BD File Offset: 0x000D55BD
		internal bool Expect100Continue
		{
			get
			{
				return this.expect100Continue;
			}
			set
			{
				this.expect100Continue = value;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06003265 RID: 12901 RVA: 0x000D65C6 File Offset: 0x000D55C6
		internal bool Ipv6Enabled
		{
			get
			{
				return this.ipv6Enabled;
			}
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x000D65CE File Offset: 0x000D55CE
		// (set) Token: 0x06003267 RID: 12903 RVA: 0x000D65D6 File Offset: 0x000D55D6
		internal int MaximumResponseHeadersLength
		{
			get
			{
				return this.maximumResponseHeadersLength;
			}
			set
			{
				this.maximumResponseHeadersLength = value;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06003268 RID: 12904 RVA: 0x000D65DF File Offset: 0x000D55DF
		internal int MaximumUnauthorizedUploadLength
		{
			get
			{
				return this.maximumUnauthorizedUploadLength;
			}
		}

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x000D65E7 File Offset: 0x000D55E7
		// (set) Token: 0x0600326A RID: 12906 RVA: 0x000D65EF File Offset: 0x000D55EF
		internal int MaximumErrorResponseLength
		{
			get
			{
				return this.maximumErrorResponseLength;
			}
			set
			{
				this.maximumErrorResponseLength = value;
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x0600326B RID: 12907 RVA: 0x000D65F8 File Offset: 0x000D55F8
		internal bool UseUnsafeHeaderParsing
		{
			get
			{
				return this.useUnsafeHeaderParsing;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x000D6600 File Offset: 0x000D5600
		// (set) Token: 0x0600326D RID: 12909 RVA: 0x000D6608 File Offset: 0x000D5608
		internal bool UseNagleAlgorithm
		{
			get
			{
				return this.useNagleAlgorithm;
			}
			set
			{
				this.useNagleAlgorithm = value;
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x0600326E RID: 12910 RVA: 0x000D6611 File Offset: 0x000D5611
		internal bool PerformanceCountersEnabled
		{
			get
			{
				return this.performanceCountersEnabled;
			}
		}

		// Token: 0x04002F2C RID: 12076
		private static object s_InternalSyncObject;

		// Token: 0x04002F2D RID: 12077
		private static SettingsSectionInternal s_settings;

		// Token: 0x04002F2E RID: 12078
		private bool alwaysUseCompletionPortsForAccept;

		// Token: 0x04002F2F RID: 12079
		private bool alwaysUseCompletionPortsForConnect;

		// Token: 0x04002F30 RID: 12080
		private bool checkCertificateName;

		// Token: 0x04002F31 RID: 12081
		private bool checkCertificateRevocationList;

		// Token: 0x04002F32 RID: 12082
		private int downloadTimeout;

		// Token: 0x04002F33 RID: 12083
		private int dnsRefreshTimeout;

		// Token: 0x04002F34 RID: 12084
		private bool enableDnsRoundRobin;

		// Token: 0x04002F35 RID: 12085
		private bool expect100Continue;

		// Token: 0x04002F36 RID: 12086
		private bool ipv6Enabled;

		// Token: 0x04002F37 RID: 12087
		private int maximumResponseHeadersLength;

		// Token: 0x04002F38 RID: 12088
		private int maximumErrorResponseLength;

		// Token: 0x04002F39 RID: 12089
		private int maximumUnauthorizedUploadLength;

		// Token: 0x04002F3A RID: 12090
		private bool useUnsafeHeaderParsing;

		// Token: 0x04002F3B RID: 12091
		private bool useNagleAlgorithm;

		// Token: 0x04002F3C RID: 12092
		private bool performanceCountersEnabled;
	}
}
