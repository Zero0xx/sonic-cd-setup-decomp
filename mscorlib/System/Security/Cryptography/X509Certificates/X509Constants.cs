using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008BD RID: 2237
	internal static class X509Constants
	{
		// Token: 0x040029DE RID: 10718
		internal const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x040029DF RID: 10719
		internal const uint CRYPT_USER_PROTECTED = 2U;

		// Token: 0x040029E0 RID: 10720
		internal const uint CRYPT_MACHINE_KEYSET = 32U;

		// Token: 0x040029E1 RID: 10721
		internal const uint CRYPT_USER_KEYSET = 4096U;

		// Token: 0x040029E2 RID: 10722
		internal const uint CERT_QUERY_CONTENT_CERT = 1U;

		// Token: 0x040029E3 RID: 10723
		internal const uint CERT_QUERY_CONTENT_CTL = 2U;

		// Token: 0x040029E4 RID: 10724
		internal const uint CERT_QUERY_CONTENT_CRL = 3U;

		// Token: 0x040029E5 RID: 10725
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_STORE = 4U;

		// Token: 0x040029E6 RID: 10726
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CERT = 5U;

		// Token: 0x040029E7 RID: 10727
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CTL = 6U;

		// Token: 0x040029E8 RID: 10728
		internal const uint CERT_QUERY_CONTENT_SERIALIZED_CRL = 7U;

		// Token: 0x040029E9 RID: 10729
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED = 8U;

		// Token: 0x040029EA RID: 10730
		internal const uint CERT_QUERY_CONTENT_PKCS7_UNSIGNED = 9U;

		// Token: 0x040029EB RID: 10731
		internal const uint CERT_QUERY_CONTENT_PKCS7_SIGNED_EMBED = 10U;

		// Token: 0x040029EC RID: 10732
		internal const uint CERT_QUERY_CONTENT_PKCS10 = 11U;

		// Token: 0x040029ED RID: 10733
		internal const uint CERT_QUERY_CONTENT_PFX = 12U;

		// Token: 0x040029EE RID: 10734
		internal const uint CERT_QUERY_CONTENT_CERT_PAIR = 13U;

		// Token: 0x040029EF RID: 10735
		internal const uint CERT_STORE_PROV_MEMORY = 2U;

		// Token: 0x040029F0 RID: 10736
		internal const uint CERT_STORE_PROV_SYSTEM = 10U;

		// Token: 0x040029F1 RID: 10737
		internal const uint CERT_STORE_NO_CRYPT_RELEASE_FLAG = 1U;

		// Token: 0x040029F2 RID: 10738
		internal const uint CERT_STORE_SET_LOCALIZED_NAME_FLAG = 2U;

		// Token: 0x040029F3 RID: 10739
		internal const uint CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG = 4U;

		// Token: 0x040029F4 RID: 10740
		internal const uint CERT_STORE_DELETE_FLAG = 16U;

		// Token: 0x040029F5 RID: 10741
		internal const uint CERT_STORE_SHARE_STORE_FLAG = 64U;

		// Token: 0x040029F6 RID: 10742
		internal const uint CERT_STORE_SHARE_CONTEXT_FLAG = 128U;

		// Token: 0x040029F7 RID: 10743
		internal const uint CERT_STORE_MANIFOLD_FLAG = 256U;

		// Token: 0x040029F8 RID: 10744
		internal const uint CERT_STORE_ENUM_ARCHIVED_FLAG = 512U;

		// Token: 0x040029F9 RID: 10745
		internal const uint CERT_STORE_UPDATE_KEYID_FLAG = 1024U;

		// Token: 0x040029FA RID: 10746
		internal const uint CERT_STORE_BACKUP_RESTORE_FLAG = 2048U;

		// Token: 0x040029FB RID: 10747
		internal const uint CERT_STORE_READONLY_FLAG = 32768U;

		// Token: 0x040029FC RID: 10748
		internal const uint CERT_STORE_OPEN_EXISTING_FLAG = 16384U;

		// Token: 0x040029FD RID: 10749
		internal const uint CERT_STORE_CREATE_NEW_FLAG = 8192U;

		// Token: 0x040029FE RID: 10750
		internal const uint CERT_STORE_MAXIMUM_ALLOWED_FLAG = 4096U;

		// Token: 0x040029FF RID: 10751
		internal const uint CERT_NAME_EMAIL_TYPE = 1U;

		// Token: 0x04002A00 RID: 10752
		internal const uint CERT_NAME_RDN_TYPE = 2U;

		// Token: 0x04002A01 RID: 10753
		internal const uint CERT_NAME_SIMPLE_DISPLAY_TYPE = 4U;

		// Token: 0x04002A02 RID: 10754
		internal const uint CERT_NAME_FRIENDLY_DISPLAY_TYPE = 5U;

		// Token: 0x04002A03 RID: 10755
		internal const uint CERT_NAME_DNS_TYPE = 6U;

		// Token: 0x04002A04 RID: 10756
		internal const uint CERT_NAME_URL_TYPE = 7U;

		// Token: 0x04002A05 RID: 10757
		internal const uint CERT_NAME_UPN_TYPE = 8U;
	}
}
