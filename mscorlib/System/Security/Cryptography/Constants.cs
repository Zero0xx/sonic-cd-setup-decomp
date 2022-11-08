using System;

namespace System.Security.Cryptography
{
	// Token: 0x020008B9 RID: 2233
	internal static class Constants
	{
		// Token: 0x04002992 RID: 10642
		internal const int S_OK = 0;

		// Token: 0x04002993 RID: 10643
		internal const int NTE_NO_KEY = -2146893811;

		// Token: 0x04002994 RID: 10644
		internal const int NTE_BAD_KEYSET = -2146893802;

		// Token: 0x04002995 RID: 10645
		internal const int NTE_KEYSET_NOT_DEF = -2146893799;

		// Token: 0x04002996 RID: 10646
		internal const int KP_IV = 1;

		// Token: 0x04002997 RID: 10647
		internal const int KP_MODE = 4;

		// Token: 0x04002998 RID: 10648
		internal const int KP_MODE_BITS = 5;

		// Token: 0x04002999 RID: 10649
		internal const int KP_EFFECTIVE_KEYLEN = 19;

		// Token: 0x0400299A RID: 10650
		internal const int ALG_CLASS_SIGNATURE = 8192;

		// Token: 0x0400299B RID: 10651
		internal const int ALG_CLASS_DATA_ENCRYPT = 24576;

		// Token: 0x0400299C RID: 10652
		internal const int ALG_CLASS_HASH = 32768;

		// Token: 0x0400299D RID: 10653
		internal const int ALG_CLASS_KEY_EXCHANGE = 40960;

		// Token: 0x0400299E RID: 10654
		internal const int ALG_TYPE_DSS = 512;

		// Token: 0x0400299F RID: 10655
		internal const int ALG_TYPE_RSA = 1024;

		// Token: 0x040029A0 RID: 10656
		internal const int ALG_TYPE_BLOCK = 1536;

		// Token: 0x040029A1 RID: 10657
		internal const int ALG_TYPE_STREAM = 2048;

		// Token: 0x040029A2 RID: 10658
		internal const int ALG_TYPE_ANY = 0;

		// Token: 0x040029A3 RID: 10659
		internal const int CALG_MD5 = 32771;

		// Token: 0x040029A4 RID: 10660
		internal const int CALG_SHA1 = 32772;

		// Token: 0x040029A5 RID: 10661
		internal const int CALG_SHA_256 = 32780;

		// Token: 0x040029A6 RID: 10662
		internal const int CALG_SHA_384 = 32781;

		// Token: 0x040029A7 RID: 10663
		internal const int CALG_SHA_512 = 32782;

		// Token: 0x040029A8 RID: 10664
		internal const int CALG_RSA_KEYX = 41984;

		// Token: 0x040029A9 RID: 10665
		internal const int CALG_RSA_SIGN = 9216;

		// Token: 0x040029AA RID: 10666
		internal const int CALG_DSS_SIGN = 8704;

		// Token: 0x040029AB RID: 10667
		internal const int CALG_DES = 26113;

		// Token: 0x040029AC RID: 10668
		internal const int CALG_RC2 = 26114;

		// Token: 0x040029AD RID: 10669
		internal const int CALG_3DES = 26115;

		// Token: 0x040029AE RID: 10670
		internal const int CALG_3DES_112 = 26121;

		// Token: 0x040029AF RID: 10671
		internal const int CALG_AES_128 = 26126;

		// Token: 0x040029B0 RID: 10672
		internal const int CALG_AES_192 = 26127;

		// Token: 0x040029B1 RID: 10673
		internal const int CALG_AES_256 = 26128;

		// Token: 0x040029B2 RID: 10674
		internal const int CALG_RC4 = 26625;

		// Token: 0x040029B3 RID: 10675
		internal const int PROV_RSA_FULL = 1;

		// Token: 0x040029B4 RID: 10676
		internal const int PROV_DSS_DH = 13;

		// Token: 0x040029B5 RID: 10677
		internal const int PROV_RSA_AES = 24;

		// Token: 0x040029B6 RID: 10678
		internal const int AT_KEYEXCHANGE = 1;

		// Token: 0x040029B7 RID: 10679
		internal const int AT_SIGNATURE = 2;

		// Token: 0x040029B8 RID: 10680
		internal const int PUBLICKEYBLOB = 6;

		// Token: 0x040029B9 RID: 10681
		internal const int PRIVATEKEYBLOB = 7;

		// Token: 0x040029BA RID: 10682
		internal const int CRYPT_OAEP = 64;

		// Token: 0x040029BB RID: 10683
		internal const uint CRYPT_VERIFYCONTEXT = 4026531840U;

		// Token: 0x040029BC RID: 10684
		internal const uint CRYPT_NEWKEYSET = 8U;

		// Token: 0x040029BD RID: 10685
		internal const uint CRYPT_DELETEKEYSET = 16U;

		// Token: 0x040029BE RID: 10686
		internal const uint CRYPT_MACHINE_KEYSET = 32U;

		// Token: 0x040029BF RID: 10687
		internal const uint CRYPT_SILENT = 64U;

		// Token: 0x040029C0 RID: 10688
		internal const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x040029C1 RID: 10689
		internal const uint CLR_KEYLEN = 1U;

		// Token: 0x040029C2 RID: 10690
		internal const uint CLR_PUBLICKEYONLY = 2U;

		// Token: 0x040029C3 RID: 10691
		internal const uint CLR_EXPORTABLE = 3U;

		// Token: 0x040029C4 RID: 10692
		internal const uint CLR_REMOVABLE = 4U;

		// Token: 0x040029C5 RID: 10693
		internal const uint CLR_HARDWARE = 5U;

		// Token: 0x040029C6 RID: 10694
		internal const uint CLR_ACCESSIBLE = 6U;

		// Token: 0x040029C7 RID: 10695
		internal const uint CLR_PROTECTED = 7U;

		// Token: 0x040029C8 RID: 10696
		internal const uint CLR_UNIQUE_CONTAINER = 8U;

		// Token: 0x040029C9 RID: 10697
		internal const uint CLR_ALGID = 9U;

		// Token: 0x040029CA RID: 10698
		internal const uint CLR_PP_CLIENT_HWND = 10U;

		// Token: 0x040029CB RID: 10699
		internal const uint CLR_PP_PIN = 11U;

		// Token: 0x040029CC RID: 10700
		internal const string OID_RSA_SMIMEalgCMS3DESwrap = "1.2.840.113549.1.9.16.3.6";

		// Token: 0x040029CD RID: 10701
		internal const string OID_RSA_MD5 = "1.2.840.113549.2.5";

		// Token: 0x040029CE RID: 10702
		internal const string OID_RSA_RC2CBC = "1.2.840.113549.3.2";

		// Token: 0x040029CF RID: 10703
		internal const string OID_RSA_DES_EDE3_CBC = "1.2.840.113549.3.7";

		// Token: 0x040029D0 RID: 10704
		internal const string OID_OIWSEC_desCBC = "1.3.14.3.2.7";

		// Token: 0x040029D1 RID: 10705
		internal const string OID_OIWSEC_SHA1 = "1.3.14.3.2.26";

		// Token: 0x040029D2 RID: 10706
		internal const string OID_OIWSEC_SHA256 = "2.16.840.1.101.3.4.2.1";

		// Token: 0x040029D3 RID: 10707
		internal const string OID_OIWSEC_SHA384 = "2.16.840.1.101.3.4.2.2";

		// Token: 0x040029D4 RID: 10708
		internal const string OID_OIWSEC_SHA512 = "2.16.840.1.101.3.4.2.3";

		// Token: 0x040029D5 RID: 10709
		internal const string OID_OIWSEC_RIPEMD160 = "1.3.36.3.2.1";
	}
}
