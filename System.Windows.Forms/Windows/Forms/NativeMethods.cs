using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Internal;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020004CB RID: 1227
	internal static class NativeMethods
	{
		// Token: 0x06004975 RID: 18805 RVA: 0x0010BF17 File Offset: 0x0010AF17
		public static int MAKELANGID(int primary, int sub)
		{
			return (int)((ushort)sub) << 10 | (int)((ushort)primary);
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x0010BF21 File Offset: 0x0010AF21
		public static int MAKELCID(int lgid)
		{
			return NativeMethods.MAKELCID(lgid, 0);
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x0010BF2A File Offset: 0x0010AF2A
		public static int MAKELCID(int lgid, int sort)
		{
			return (65535 & lgid) | (15 & sort) << 16;
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x0010BF3B File Offset: 0x0010AF3B
		public static bool Succeeded(int hr)
		{
			return hr >= 0;
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x0010BF44 File Offset: 0x0010AF44
		public static bool Failed(int hr)
		{
			return hr < 0;
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x0010BF4A File Offset: 0x0010AF4A
		public static int WM_MOUSEENTER
		{
			get
			{
				if (NativeMethods.wmMouseEnterMessage == -1)
				{
					NativeMethods.wmMouseEnterMessage = SafeNativeMethods.RegisterWindowMessage("WinFormsMouseEnter");
				}
				return NativeMethods.wmMouseEnterMessage;
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x0600497B RID: 18811 RVA: 0x0010BF68 File Offset: 0x0010AF68
		public static int WM_UIUNSUBCLASS
		{
			get
			{
				if (NativeMethods.wmUnSubclass == -1)
				{
					NativeMethods.wmUnSubclass = SafeNativeMethods.RegisterWindowMessage("WinFormsUnSubclass");
				}
				return NativeMethods.wmUnSubclass;
			}
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x0010BF88 File Offset: 0x0010AF88
		static NativeMethods()
		{
			if (Marshal.SystemDefaultCharSize == 1)
			{
				NativeMethods.BFFM_SETSELECTION = 1126;
				NativeMethods.CBEM_GETITEM = 1028;
				NativeMethods.CBEM_SETITEM = 1029;
				NativeMethods.CBEN_ENDEDIT = -805;
				NativeMethods.CBEM_INSERTITEM = 1025;
				NativeMethods.LVM_GETITEMTEXT = 4141;
				NativeMethods.LVM_SETITEMTEXT = 4142;
				NativeMethods.ACM_OPEN = 1124;
				NativeMethods.DTM_SETFORMAT = 4101;
				NativeMethods.DTN_USERSTRING = -758;
				NativeMethods.DTN_WMKEYDOWN = -757;
				NativeMethods.DTN_FORMAT = -756;
				NativeMethods.DTN_FORMATQUERY = -755;
				NativeMethods.EMR_POLYTEXTOUT = 96;
				NativeMethods.HDM_INSERTITEM = 4609;
				NativeMethods.HDM_GETITEM = 4611;
				NativeMethods.HDM_SETITEM = 4612;
				NativeMethods.HDN_ITEMCHANGING = -300;
				NativeMethods.HDN_ITEMCHANGED = -301;
				NativeMethods.HDN_ITEMCLICK = -302;
				NativeMethods.HDN_ITEMDBLCLICK = -303;
				NativeMethods.HDN_DIVIDERDBLCLICK = -305;
				NativeMethods.HDN_BEGINTRACK = -306;
				NativeMethods.HDN_ENDTRACK = -307;
				NativeMethods.HDN_TRACK = -308;
				NativeMethods.HDN_GETDISPINFO = -309;
				NativeMethods.LVM_SETBKIMAGE = 4164;
				NativeMethods.LVM_GETITEM = 4101;
				NativeMethods.LVM_SETITEM = 4102;
				NativeMethods.LVM_INSERTITEM = 4103;
				NativeMethods.LVM_FINDITEM = 4109;
				NativeMethods.LVM_GETSTRINGWIDTH = 4113;
				NativeMethods.LVM_EDITLABEL = 4119;
				NativeMethods.LVM_GETCOLUMN = 4121;
				NativeMethods.LVM_SETCOLUMN = 4122;
				NativeMethods.LVM_GETISEARCHSTRING = 4148;
				NativeMethods.LVM_INSERTCOLUMN = 4123;
				NativeMethods.LVN_BEGINLABELEDIT = -105;
				NativeMethods.LVN_ENDLABELEDIT = -106;
				NativeMethods.LVN_ODFINDITEM = -152;
				NativeMethods.LVN_GETDISPINFO = -150;
				NativeMethods.LVN_GETINFOTIP = -157;
				NativeMethods.LVN_SETDISPINFO = -151;
				NativeMethods.PSM_SETTITLE = 1135;
				NativeMethods.PSM_SETFINISHTEXT = 1139;
				NativeMethods.RB_INSERTBAND = 1025;
				NativeMethods.SB_SETTEXT = 1025;
				NativeMethods.SB_GETTEXT = 1026;
				NativeMethods.SB_GETTEXTLENGTH = 1027;
				NativeMethods.SB_SETTIPTEXT = 1040;
				NativeMethods.SB_GETTIPTEXT = 1042;
				NativeMethods.TB_SAVERESTORE = 1050;
				NativeMethods.TB_ADDSTRING = 1052;
				NativeMethods.TB_GETBUTTONTEXT = 1069;
				NativeMethods.TB_MAPACCELERATOR = 1102;
				NativeMethods.TB_GETBUTTONINFO = 1089;
				NativeMethods.TB_SETBUTTONINFO = 1090;
				NativeMethods.TB_INSERTBUTTON = 1045;
				NativeMethods.TB_ADDBUTTONS = 1044;
				NativeMethods.TBN_GETBUTTONINFO = -700;
				NativeMethods.TBN_GETINFOTIP = -718;
				NativeMethods.TBN_GETDISPINFO = -716;
				NativeMethods.TTM_ADDTOOL = 1028;
				NativeMethods.TTM_SETTITLE = 1056;
				NativeMethods.TTM_DELTOOL = 1029;
				NativeMethods.TTM_NEWTOOLRECT = 1030;
				NativeMethods.TTM_GETTOOLINFO = 1032;
				NativeMethods.TTM_SETTOOLINFO = 1033;
				NativeMethods.TTM_HITTEST = 1034;
				NativeMethods.TTM_GETTEXT = 1035;
				NativeMethods.TTM_UPDATETIPTEXT = 1036;
				NativeMethods.TTM_ENUMTOOLS = 1038;
				NativeMethods.TTM_GETCURRENTTOOL = 1039;
				NativeMethods.TTN_GETDISPINFO = -520;
				NativeMethods.TTN_NEEDTEXT = -520;
				NativeMethods.TVM_INSERTITEM = 4352;
				NativeMethods.TVM_GETITEM = 4364;
				NativeMethods.TVM_SETITEM = 4365;
				NativeMethods.TVM_EDITLABEL = 4366;
				NativeMethods.TVM_GETISEARCHSTRING = 4375;
				NativeMethods.TVN_SELCHANGING = -401;
				NativeMethods.TVN_SELCHANGED = -402;
				NativeMethods.TVN_GETDISPINFO = -403;
				NativeMethods.TVN_SETDISPINFO = -404;
				NativeMethods.TVN_ITEMEXPANDING = -405;
				NativeMethods.TVN_ITEMEXPANDED = -406;
				NativeMethods.TVN_BEGINDRAG = -407;
				NativeMethods.TVN_BEGINRDRAG = -408;
				NativeMethods.TVN_BEGINLABELEDIT = -410;
				NativeMethods.TVN_ENDLABELEDIT = -411;
				NativeMethods.TCM_GETITEM = 4869;
				NativeMethods.TCM_SETITEM = 4870;
				NativeMethods.TCM_INSERTITEM = 4871;
				return;
			}
			NativeMethods.BFFM_SETSELECTION = 1127;
			NativeMethods.CBEM_GETITEM = 1037;
			NativeMethods.CBEM_SETITEM = 1036;
			NativeMethods.CBEN_ENDEDIT = -806;
			NativeMethods.CBEM_INSERTITEM = 1035;
			NativeMethods.LVM_GETITEMTEXT = 4211;
			NativeMethods.LVM_SETITEMTEXT = 4212;
			NativeMethods.ACM_OPEN = 1127;
			NativeMethods.DTM_SETFORMAT = 4146;
			NativeMethods.DTN_USERSTRING = -745;
			NativeMethods.DTN_WMKEYDOWN = -744;
			NativeMethods.DTN_FORMAT = -743;
			NativeMethods.DTN_FORMATQUERY = -742;
			NativeMethods.EMR_POLYTEXTOUT = 97;
			NativeMethods.HDM_INSERTITEM = 4618;
			NativeMethods.HDM_GETITEM = 4619;
			NativeMethods.HDM_SETITEM = 4620;
			NativeMethods.HDN_ITEMCHANGING = -320;
			NativeMethods.HDN_ITEMCHANGED = -321;
			NativeMethods.HDN_ITEMCLICK = -322;
			NativeMethods.HDN_ITEMDBLCLICK = -323;
			NativeMethods.HDN_DIVIDERDBLCLICK = -325;
			NativeMethods.HDN_BEGINTRACK = -326;
			NativeMethods.HDN_ENDTRACK = -327;
			NativeMethods.HDN_TRACK = -328;
			NativeMethods.HDN_GETDISPINFO = -329;
			NativeMethods.LVM_SETBKIMAGE = 4234;
			NativeMethods.LVM_GETITEM = 4171;
			NativeMethods.LVM_SETITEM = 4172;
			NativeMethods.LVM_INSERTITEM = 4173;
			NativeMethods.LVM_FINDITEM = 4179;
			NativeMethods.LVM_GETSTRINGWIDTH = 4183;
			NativeMethods.LVM_EDITLABEL = 4214;
			NativeMethods.LVM_GETCOLUMN = 4191;
			NativeMethods.LVM_SETCOLUMN = 4192;
			NativeMethods.LVM_GETISEARCHSTRING = 4213;
			NativeMethods.LVM_INSERTCOLUMN = 4193;
			NativeMethods.LVN_BEGINLABELEDIT = -175;
			NativeMethods.LVN_ENDLABELEDIT = -176;
			NativeMethods.LVN_ODFINDITEM = -179;
			NativeMethods.LVN_GETDISPINFO = -177;
			NativeMethods.LVN_GETINFOTIP = -158;
			NativeMethods.LVN_SETDISPINFO = -178;
			NativeMethods.PSM_SETTITLE = 1144;
			NativeMethods.PSM_SETFINISHTEXT = 1145;
			NativeMethods.RB_INSERTBAND = 1034;
			NativeMethods.SB_SETTEXT = 1035;
			NativeMethods.SB_GETTEXT = 1037;
			NativeMethods.SB_GETTEXTLENGTH = 1036;
			NativeMethods.SB_SETTIPTEXT = 1041;
			NativeMethods.SB_GETTIPTEXT = 1043;
			NativeMethods.TB_SAVERESTORE = 1100;
			NativeMethods.TB_ADDSTRING = 1101;
			NativeMethods.TB_GETBUTTONTEXT = 1099;
			NativeMethods.TB_MAPACCELERATOR = 1114;
			NativeMethods.TB_GETBUTTONINFO = 1087;
			NativeMethods.TB_SETBUTTONINFO = 1088;
			NativeMethods.TB_INSERTBUTTON = 1091;
			NativeMethods.TB_ADDBUTTONS = 1092;
			NativeMethods.TBN_GETBUTTONINFO = -720;
			NativeMethods.TBN_GETINFOTIP = -719;
			NativeMethods.TBN_GETDISPINFO = -717;
			NativeMethods.TTM_ADDTOOL = 1074;
			NativeMethods.TTM_SETTITLE = 1057;
			NativeMethods.TTM_DELTOOL = 1075;
			NativeMethods.TTM_NEWTOOLRECT = 1076;
			NativeMethods.TTM_GETTOOLINFO = 1077;
			NativeMethods.TTM_SETTOOLINFO = 1078;
			NativeMethods.TTM_HITTEST = 1079;
			NativeMethods.TTM_GETTEXT = 1080;
			NativeMethods.TTM_UPDATETIPTEXT = 1081;
			NativeMethods.TTM_ENUMTOOLS = 1082;
			NativeMethods.TTM_GETCURRENTTOOL = 1083;
			NativeMethods.TTN_GETDISPINFO = -530;
			NativeMethods.TTN_NEEDTEXT = -530;
			NativeMethods.TVM_INSERTITEM = 4402;
			NativeMethods.TVM_GETITEM = 4414;
			NativeMethods.TVM_SETITEM = 4415;
			NativeMethods.TVM_EDITLABEL = 4417;
			NativeMethods.TVM_GETISEARCHSTRING = 4416;
			NativeMethods.TVN_SELCHANGING = -450;
			NativeMethods.TVN_SELCHANGED = -451;
			NativeMethods.TVN_GETDISPINFO = -452;
			NativeMethods.TVN_SETDISPINFO = -453;
			NativeMethods.TVN_ITEMEXPANDING = -454;
			NativeMethods.TVN_ITEMEXPANDED = -455;
			NativeMethods.TVN_BEGINDRAG = -456;
			NativeMethods.TVN_BEGINRDRAG = -457;
			NativeMethods.TVN_BEGINLABELEDIT = -459;
			NativeMethods.TVN_ENDLABELEDIT = -460;
			NativeMethods.TCM_GETITEM = 4924;
			NativeMethods.TCM_SETITEM = 4925;
			NativeMethods.TCM_INSERTITEM = 4926;
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x0010C784 File Offset: 0x0010B784
		internal static string GetLocalPath(string fileName)
		{
			Uri uri = new Uri(fileName);
			return uri.LocalPath + uri.Fragment;
		}

		// Token: 0x040022BA RID: 8890
		public const int BITMAPINFO_MAX_COLORSIZE = 256;

		// Token: 0x040022BB RID: 8891
		public const int BI_BITFIELDS = 3;

		// Token: 0x040022BC RID: 8892
		public const int STATUS_PENDING = 259;

		// Token: 0x040022BD RID: 8893
		public const int DESKTOP_SWITCHDESKTOP = 256;

		// Token: 0x040022BE RID: 8894
		public const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x040022BF RID: 8895
		public const int FW_DONTCARE = 0;

		// Token: 0x040022C0 RID: 8896
		public const int FW_NORMAL = 400;

		// Token: 0x040022C1 RID: 8897
		public const int FW_BOLD = 700;

		// Token: 0x040022C2 RID: 8898
		public const int ANSI_CHARSET = 0;

		// Token: 0x040022C3 RID: 8899
		public const int DEFAULT_CHARSET = 1;

		// Token: 0x040022C4 RID: 8900
		public const int OUT_DEFAULT_PRECIS = 0;

		// Token: 0x040022C5 RID: 8901
		public const int OUT_TT_PRECIS = 4;

		// Token: 0x040022C6 RID: 8902
		public const int OUT_TT_ONLY_PRECIS = 7;

		// Token: 0x040022C7 RID: 8903
		public const int ALTERNATE = 1;

		// Token: 0x040022C8 RID: 8904
		public const int WINDING = 2;

		// Token: 0x040022C9 RID: 8905
		public const int TA_DEFAULT = 0;

		// Token: 0x040022CA RID: 8906
		public const int BS_SOLID = 0;

		// Token: 0x040022CB RID: 8907
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x040022CC RID: 8908
		public const int R2_BLACK = 1;

		// Token: 0x040022CD RID: 8909
		public const int R2_NOTMERGEPEN = 2;

		// Token: 0x040022CE RID: 8910
		public const int R2_MASKNOTPEN = 3;

		// Token: 0x040022CF RID: 8911
		public const int R2_NOTCOPYPEN = 4;

		// Token: 0x040022D0 RID: 8912
		public const int R2_MASKPENNOT = 5;

		// Token: 0x040022D1 RID: 8913
		public const int R2_NOT = 6;

		// Token: 0x040022D2 RID: 8914
		public const int R2_XORPEN = 7;

		// Token: 0x040022D3 RID: 8915
		public const int R2_NOTMASKPEN = 8;

		// Token: 0x040022D4 RID: 8916
		public const int R2_MASKPEN = 9;

		// Token: 0x040022D5 RID: 8917
		public const int R2_NOTXORPEN = 10;

		// Token: 0x040022D6 RID: 8918
		public const int R2_NOP = 11;

		// Token: 0x040022D7 RID: 8919
		public const int R2_MERGENOTPEN = 12;

		// Token: 0x040022D8 RID: 8920
		public const int R2_COPYPEN = 13;

		// Token: 0x040022D9 RID: 8921
		public const int R2_MERGEPENNOT = 14;

		// Token: 0x040022DA RID: 8922
		public const int R2_MERGEPEN = 15;

		// Token: 0x040022DB RID: 8923
		public const int R2_WHITE = 16;

		// Token: 0x040022DC RID: 8924
		public const int GM_COMPATIBLE = 1;

		// Token: 0x040022DD RID: 8925
		public const int GM_ADVANCED = 2;

		// Token: 0x040022DE RID: 8926
		public const int MWT_IDENTITY = 1;

		// Token: 0x040022DF RID: 8927
		public const int PAGE_READONLY = 2;

		// Token: 0x040022E0 RID: 8928
		public const int PAGE_READWRITE = 4;

		// Token: 0x040022E1 RID: 8929
		public const int PAGE_WRITECOPY = 8;

		// Token: 0x040022E2 RID: 8930
		public const int FILE_MAP_COPY = 1;

		// Token: 0x040022E3 RID: 8931
		public const int FILE_MAP_WRITE = 2;

		// Token: 0x040022E4 RID: 8932
		public const int FILE_MAP_READ = 4;

		// Token: 0x040022E5 RID: 8933
		public const int SHGFI_ICON = 256;

		// Token: 0x040022E6 RID: 8934
		public const int SHGFI_DISPLAYNAME = 512;

		// Token: 0x040022E7 RID: 8935
		public const int SHGFI_TYPENAME = 1024;

		// Token: 0x040022E8 RID: 8936
		public const int SHGFI_ATTRIBUTES = 2048;

		// Token: 0x040022E9 RID: 8937
		public const int SHGFI_ICONLOCATION = 4096;

		// Token: 0x040022EA RID: 8938
		public const int SHGFI_EXETYPE = 8192;

		// Token: 0x040022EB RID: 8939
		public const int SHGFI_SYSICONINDEX = 16384;

		// Token: 0x040022EC RID: 8940
		public const int SHGFI_LINKOVERLAY = 32768;

		// Token: 0x040022ED RID: 8941
		public const int SHGFI_SELECTED = 65536;

		// Token: 0x040022EE RID: 8942
		public const int SHGFI_ATTR_SPECIFIED = 131072;

		// Token: 0x040022EF RID: 8943
		public const int SHGFI_LARGEICON = 0;

		// Token: 0x040022F0 RID: 8944
		public const int SHGFI_SMALLICON = 1;

		// Token: 0x040022F1 RID: 8945
		public const int SHGFI_OPENICON = 2;

		// Token: 0x040022F2 RID: 8946
		public const int SHGFI_SHELLICONSIZE = 4;

		// Token: 0x040022F3 RID: 8947
		public const int SHGFI_PIDL = 8;

		// Token: 0x040022F4 RID: 8948
		public const int SHGFI_USEFILEATTRIBUTES = 16;

		// Token: 0x040022F5 RID: 8949
		public const int SHGFI_ADDOVERLAYS = 32;

		// Token: 0x040022F6 RID: 8950
		public const int SHGFI_OVERLAYINDEX = 64;

		// Token: 0x040022F7 RID: 8951
		public const int DM_DISPLAYORIENTATION = 128;

		// Token: 0x040022F8 RID: 8952
		public const int AUTOSUGGEST = 268435456;

		// Token: 0x040022F9 RID: 8953
		public const int AUTOSUGGEST_OFF = 536870912;

		// Token: 0x040022FA RID: 8954
		public const int AUTOAPPEND = 1073741824;

		// Token: 0x040022FB RID: 8955
		public const int AUTOAPPEND_OFF = -2147483648;

		// Token: 0x040022FC RID: 8956
		public const int ARW_BOTTOMLEFT = 0;

		// Token: 0x040022FD RID: 8957
		public const int ARW_BOTTOMRIGHT = 1;

		// Token: 0x040022FE RID: 8958
		public const int ARW_TOPLEFT = 2;

		// Token: 0x040022FF RID: 8959
		public const int ARW_TOPRIGHT = 3;

		// Token: 0x04002300 RID: 8960
		public const int ARW_LEFT = 0;

		// Token: 0x04002301 RID: 8961
		public const int ARW_RIGHT = 0;

		// Token: 0x04002302 RID: 8962
		public const int ARW_UP = 4;

		// Token: 0x04002303 RID: 8963
		public const int ARW_DOWN = 4;

		// Token: 0x04002304 RID: 8964
		public const int ARW_HIDE = 8;

		// Token: 0x04002305 RID: 8965
		public const int ACM_OPENA = 1124;

		// Token: 0x04002306 RID: 8966
		public const int ACM_OPENW = 1127;

		// Token: 0x04002307 RID: 8967
		public const int ADVF_NODATA = 1;

		// Token: 0x04002308 RID: 8968
		public const int ADVF_ONLYONCE = 4;

		// Token: 0x04002309 RID: 8969
		public const int ADVF_PRIMEFIRST = 2;

		// Token: 0x0400230A RID: 8970
		public const int BCM_GETIDEALSIZE = 5633;

		// Token: 0x0400230B RID: 8971
		public const int BI_RGB = 0;

		// Token: 0x0400230C RID: 8972
		public const int BS_PATTERN = 3;

		// Token: 0x0400230D RID: 8973
		public const int BITSPIXEL = 12;

		// Token: 0x0400230E RID: 8974
		public const int BDR_RAISEDOUTER = 1;

		// Token: 0x0400230F RID: 8975
		public const int BDR_SUNKENOUTER = 2;

		// Token: 0x04002310 RID: 8976
		public const int BDR_RAISEDINNER = 4;

		// Token: 0x04002311 RID: 8977
		public const int BDR_SUNKENINNER = 8;

		// Token: 0x04002312 RID: 8978
		public const int BDR_RAISED = 5;

		// Token: 0x04002313 RID: 8979
		public const int BDR_SUNKEN = 10;

		// Token: 0x04002314 RID: 8980
		public const int BF_LEFT = 1;

		// Token: 0x04002315 RID: 8981
		public const int BF_TOP = 2;

		// Token: 0x04002316 RID: 8982
		public const int BF_RIGHT = 4;

		// Token: 0x04002317 RID: 8983
		public const int BF_BOTTOM = 8;

		// Token: 0x04002318 RID: 8984
		public const int BF_ADJUST = 8192;

		// Token: 0x04002319 RID: 8985
		public const int BF_FLAT = 16384;

		// Token: 0x0400231A RID: 8986
		public const int BF_MIDDLE = 2048;

		// Token: 0x0400231B RID: 8987
		public const int BFFM_INITIALIZED = 1;

		// Token: 0x0400231C RID: 8988
		public const int BFFM_SELCHANGED = 2;

		// Token: 0x0400231D RID: 8989
		public const int BFFM_SETSELECTIONA = 1126;

		// Token: 0x0400231E RID: 8990
		public const int BFFM_SETSELECTIONW = 1127;

		// Token: 0x0400231F RID: 8991
		public const int BFFM_ENABLEOK = 1125;

		// Token: 0x04002320 RID: 8992
		public const int BS_PUSHBUTTON = 0;

		// Token: 0x04002321 RID: 8993
		public const int BS_DEFPUSHBUTTON = 1;

		// Token: 0x04002322 RID: 8994
		public const int BS_MULTILINE = 8192;

		// Token: 0x04002323 RID: 8995
		public const int BS_PUSHLIKE = 4096;

		// Token: 0x04002324 RID: 8996
		public const int BS_OWNERDRAW = 11;

		// Token: 0x04002325 RID: 8997
		public const int BS_RADIOBUTTON = 4;

		// Token: 0x04002326 RID: 8998
		public const int BS_3STATE = 5;

		// Token: 0x04002327 RID: 8999
		public const int BS_GROUPBOX = 7;

		// Token: 0x04002328 RID: 9000
		public const int BS_LEFT = 256;

		// Token: 0x04002329 RID: 9001
		public const int BS_RIGHT = 512;

		// Token: 0x0400232A RID: 9002
		public const int BS_CENTER = 768;

		// Token: 0x0400232B RID: 9003
		public const int BS_TOP = 1024;

		// Token: 0x0400232C RID: 9004
		public const int BS_BOTTOM = 2048;

		// Token: 0x0400232D RID: 9005
		public const int BS_VCENTER = 3072;

		// Token: 0x0400232E RID: 9006
		public const int BS_RIGHTBUTTON = 32;

		// Token: 0x0400232F RID: 9007
		public const int BN_CLICKED = 0;

		// Token: 0x04002330 RID: 9008
		public const int BM_SETCHECK = 241;

		// Token: 0x04002331 RID: 9009
		public const int BM_SETSTATE = 243;

		// Token: 0x04002332 RID: 9010
		public const int BM_CLICK = 245;

		// Token: 0x04002333 RID: 9011
		public const int CDERR_DIALOGFAILURE = 65535;

		// Token: 0x04002334 RID: 9012
		public const int CDERR_STRUCTSIZE = 1;

		// Token: 0x04002335 RID: 9013
		public const int CDERR_INITIALIZATION = 2;

		// Token: 0x04002336 RID: 9014
		public const int CDERR_NOTEMPLATE = 3;

		// Token: 0x04002337 RID: 9015
		public const int CDERR_NOHINSTANCE = 4;

		// Token: 0x04002338 RID: 9016
		public const int CDERR_LOADSTRFAILURE = 5;

		// Token: 0x04002339 RID: 9017
		public const int CDERR_FINDRESFAILURE = 6;

		// Token: 0x0400233A RID: 9018
		public const int CDERR_LOADRESFAILURE = 7;

		// Token: 0x0400233B RID: 9019
		public const int CDERR_LOCKRESFAILURE = 8;

		// Token: 0x0400233C RID: 9020
		public const int CDERR_MEMALLOCFAILURE = 9;

		// Token: 0x0400233D RID: 9021
		public const int CDERR_MEMLOCKFAILURE = 10;

		// Token: 0x0400233E RID: 9022
		public const int CDERR_NOHOOK = 11;

		// Token: 0x0400233F RID: 9023
		public const int CDERR_REGISTERMSGFAIL = 12;

		// Token: 0x04002340 RID: 9024
		public const int CFERR_NOFONTS = 8193;

		// Token: 0x04002341 RID: 9025
		public const int CFERR_MAXLESSTHANMIN = 8194;

		// Token: 0x04002342 RID: 9026
		public const int CC_RGBINIT = 1;

		// Token: 0x04002343 RID: 9027
		public const int CC_FULLOPEN = 2;

		// Token: 0x04002344 RID: 9028
		public const int CC_PREVENTFULLOPEN = 4;

		// Token: 0x04002345 RID: 9029
		public const int CC_SHOWHELP = 8;

		// Token: 0x04002346 RID: 9030
		public const int CC_ENABLEHOOK = 16;

		// Token: 0x04002347 RID: 9031
		public const int CC_SOLIDCOLOR = 128;

		// Token: 0x04002348 RID: 9032
		public const int CC_ANYCOLOR = 256;

		// Token: 0x04002349 RID: 9033
		public const int CF_SCREENFONTS = 1;

		// Token: 0x0400234A RID: 9034
		public const int CF_SHOWHELP = 4;

		// Token: 0x0400234B RID: 9035
		public const int CF_ENABLEHOOK = 8;

		// Token: 0x0400234C RID: 9036
		public const int CF_INITTOLOGFONTSTRUCT = 64;

		// Token: 0x0400234D RID: 9037
		public const int CF_EFFECTS = 256;

		// Token: 0x0400234E RID: 9038
		public const int CF_APPLY = 512;

		// Token: 0x0400234F RID: 9039
		public const int CF_SCRIPTSONLY = 1024;

		// Token: 0x04002350 RID: 9040
		public const int CF_NOVECTORFONTS = 2048;

		// Token: 0x04002351 RID: 9041
		public const int CF_NOSIMULATIONS = 4096;

		// Token: 0x04002352 RID: 9042
		public const int CF_LIMITSIZE = 8192;

		// Token: 0x04002353 RID: 9043
		public const int CF_FIXEDPITCHONLY = 16384;

		// Token: 0x04002354 RID: 9044
		public const int CF_FORCEFONTEXIST = 65536;

		// Token: 0x04002355 RID: 9045
		public const int CF_TTONLY = 262144;

		// Token: 0x04002356 RID: 9046
		public const int CF_SELECTSCRIPT = 4194304;

		// Token: 0x04002357 RID: 9047
		public const int CF_NOVERTFONTS = 16777216;

		// Token: 0x04002358 RID: 9048
		public const int CP_WINANSI = 1004;

		// Token: 0x04002359 RID: 9049
		public const int cmb4 = 1139;

		// Token: 0x0400235A RID: 9050
		public const int CS_DBLCLKS = 8;

		// Token: 0x0400235B RID: 9051
		public const int CS_DROPSHADOW = 131072;

		// Token: 0x0400235C RID: 9052
		public const int CS_SAVEBITS = 2048;

		// Token: 0x0400235D RID: 9053
		public const int CF_TEXT = 1;

		// Token: 0x0400235E RID: 9054
		public const int CF_BITMAP = 2;

		// Token: 0x0400235F RID: 9055
		public const int CF_METAFILEPICT = 3;

		// Token: 0x04002360 RID: 9056
		public const int CF_SYLK = 4;

		// Token: 0x04002361 RID: 9057
		public const int CF_DIF = 5;

		// Token: 0x04002362 RID: 9058
		public const int CF_TIFF = 6;

		// Token: 0x04002363 RID: 9059
		public const int CF_OEMTEXT = 7;

		// Token: 0x04002364 RID: 9060
		public const int CF_DIB = 8;

		// Token: 0x04002365 RID: 9061
		public const int CF_PALETTE = 9;

		// Token: 0x04002366 RID: 9062
		public const int CF_PENDATA = 10;

		// Token: 0x04002367 RID: 9063
		public const int CF_RIFF = 11;

		// Token: 0x04002368 RID: 9064
		public const int CF_WAVE = 12;

		// Token: 0x04002369 RID: 9065
		public const int CF_UNICODETEXT = 13;

		// Token: 0x0400236A RID: 9066
		public const int CF_ENHMETAFILE = 14;

		// Token: 0x0400236B RID: 9067
		public const int CF_HDROP = 15;

		// Token: 0x0400236C RID: 9068
		public const int CF_LOCALE = 16;

		// Token: 0x0400236D RID: 9069
		public const int CLSCTX_INPROC_SERVER = 1;

		// Token: 0x0400236E RID: 9070
		public const int CLSCTX_LOCAL_SERVER = 4;

		// Token: 0x0400236F RID: 9071
		public const int CW_USEDEFAULT = -2147483648;

		// Token: 0x04002370 RID: 9072
		public const int CWP_SKIPINVISIBLE = 1;

		// Token: 0x04002371 RID: 9073
		public const int COLOR_WINDOW = 5;

		// Token: 0x04002372 RID: 9074
		public const int CB_ERR = -1;

		// Token: 0x04002373 RID: 9075
		public const int CBN_SELCHANGE = 1;

		// Token: 0x04002374 RID: 9076
		public const int CBN_DBLCLK = 2;

		// Token: 0x04002375 RID: 9077
		public const int CBN_EDITCHANGE = 5;

		// Token: 0x04002376 RID: 9078
		public const int CBN_EDITUPDATE = 6;

		// Token: 0x04002377 RID: 9079
		public const int CBN_DROPDOWN = 7;

		// Token: 0x04002378 RID: 9080
		public const int CBN_CLOSEUP = 8;

		// Token: 0x04002379 RID: 9081
		public const int CBN_SELENDOK = 9;

		// Token: 0x0400237A RID: 9082
		public const int CBS_SIMPLE = 1;

		// Token: 0x0400237B RID: 9083
		public const int CBS_DROPDOWN = 2;

		// Token: 0x0400237C RID: 9084
		public const int CBS_DROPDOWNLIST = 3;

		// Token: 0x0400237D RID: 9085
		public const int CBS_OWNERDRAWFIXED = 16;

		// Token: 0x0400237E RID: 9086
		public const int CBS_OWNERDRAWVARIABLE = 32;

		// Token: 0x0400237F RID: 9087
		public const int CBS_AUTOHSCROLL = 64;

		// Token: 0x04002380 RID: 9088
		public const int CBS_HASSTRINGS = 512;

		// Token: 0x04002381 RID: 9089
		public const int CBS_NOINTEGRALHEIGHT = 1024;

		// Token: 0x04002382 RID: 9090
		public const int CB_GETEDITSEL = 320;

		// Token: 0x04002383 RID: 9091
		public const int CB_LIMITTEXT = 321;

		// Token: 0x04002384 RID: 9092
		public const int CB_SETEDITSEL = 322;

		// Token: 0x04002385 RID: 9093
		public const int CB_ADDSTRING = 323;

		// Token: 0x04002386 RID: 9094
		public const int CB_DELETESTRING = 324;

		// Token: 0x04002387 RID: 9095
		public const int CB_GETCURSEL = 327;

		// Token: 0x04002388 RID: 9096
		public const int CB_GETLBTEXT = 328;

		// Token: 0x04002389 RID: 9097
		public const int CB_GETLBTEXTLEN = 329;

		// Token: 0x0400238A RID: 9098
		public const int CB_INSERTSTRING = 330;

		// Token: 0x0400238B RID: 9099
		public const int CB_RESETCONTENT = 331;

		// Token: 0x0400238C RID: 9100
		public const int CB_FINDSTRING = 332;

		// Token: 0x0400238D RID: 9101
		public const int CB_SETCURSEL = 334;

		// Token: 0x0400238E RID: 9102
		public const int CB_SHOWDROPDOWN = 335;

		// Token: 0x0400238F RID: 9103
		public const int CB_GETITEMDATA = 336;

		// Token: 0x04002390 RID: 9104
		public const int CB_SETITEMHEIGHT = 339;

		// Token: 0x04002391 RID: 9105
		public const int CB_GETITEMHEIGHT = 340;

		// Token: 0x04002392 RID: 9106
		public const int CB_GETDROPPEDSTATE = 343;

		// Token: 0x04002393 RID: 9107
		public const int CB_FINDSTRINGEXACT = 344;

		// Token: 0x04002394 RID: 9108
		public const int CB_GETDROPPEDWIDTH = 351;

		// Token: 0x04002395 RID: 9109
		public const int CB_SETDROPPEDWIDTH = 352;

		// Token: 0x04002396 RID: 9110
		public const int CDRF_DODEFAULT = 0;

		// Token: 0x04002397 RID: 9111
		public const int CDRF_NEWFONT = 2;

		// Token: 0x04002398 RID: 9112
		public const int CDRF_SKIPDEFAULT = 4;

		// Token: 0x04002399 RID: 9113
		public const int CDRF_NOTIFYPOSTPAINT = 16;

		// Token: 0x0400239A RID: 9114
		public const int CDRF_NOTIFYITEMDRAW = 32;

		// Token: 0x0400239B RID: 9115
		public const int CDRF_NOTIFYSUBITEMDRAW = 32;

		// Token: 0x0400239C RID: 9116
		public const int CDDS_PREPAINT = 1;

		// Token: 0x0400239D RID: 9117
		public const int CDDS_POSTPAINT = 2;

		// Token: 0x0400239E RID: 9118
		public const int CDDS_ITEM = 65536;

		// Token: 0x0400239F RID: 9119
		public const int CDDS_SUBITEM = 131072;

		// Token: 0x040023A0 RID: 9120
		public const int CDDS_ITEMPREPAINT = 65537;

		// Token: 0x040023A1 RID: 9121
		public const int CDDS_ITEMPOSTPAINT = 65538;

		// Token: 0x040023A2 RID: 9122
		public const int CDIS_SELECTED = 1;

		// Token: 0x040023A3 RID: 9123
		public const int CDIS_GRAYED = 2;

		// Token: 0x040023A4 RID: 9124
		public const int CDIS_DISABLED = 4;

		// Token: 0x040023A5 RID: 9125
		public const int CDIS_CHECKED = 8;

		// Token: 0x040023A6 RID: 9126
		public const int CDIS_FOCUS = 16;

		// Token: 0x040023A7 RID: 9127
		public const int CDIS_DEFAULT = 32;

		// Token: 0x040023A8 RID: 9128
		public const int CDIS_HOT = 64;

		// Token: 0x040023A9 RID: 9129
		public const int CDIS_MARKED = 128;

		// Token: 0x040023AA RID: 9130
		public const int CDIS_INDETERMINATE = 256;

		// Token: 0x040023AB RID: 9131
		public const int CDIS_SHOWKEYBOARDCUES = 512;

		// Token: 0x040023AC RID: 9132
		public const int CLR_NONE = -1;

		// Token: 0x040023AD RID: 9133
		public const int CLR_DEFAULT = -16777216;

		// Token: 0x040023AE RID: 9134
		public const int CCM_SETVERSION = 8199;

		// Token: 0x040023AF RID: 9135
		public const int CCM_GETVERSION = 8200;

		// Token: 0x040023B0 RID: 9136
		public const int CCS_NORESIZE = 4;

		// Token: 0x040023B1 RID: 9137
		public const int CCS_NOPARENTALIGN = 8;

		// Token: 0x040023B2 RID: 9138
		public const int CCS_NODIVIDER = 64;

		// Token: 0x040023B3 RID: 9139
		public const int CBEM_INSERTITEMA = 1025;

		// Token: 0x040023B4 RID: 9140
		public const int CBEM_GETITEMA = 1028;

		// Token: 0x040023B5 RID: 9141
		public const int CBEM_SETITEMA = 1029;

		// Token: 0x040023B6 RID: 9142
		public const int CBEM_INSERTITEMW = 1035;

		// Token: 0x040023B7 RID: 9143
		public const int CBEM_SETITEMW = 1036;

		// Token: 0x040023B8 RID: 9144
		public const int CBEM_GETITEMW = 1037;

		// Token: 0x040023B9 RID: 9145
		public const int CBEN_ENDEDITA = -805;

		// Token: 0x040023BA RID: 9146
		public const int CBEN_ENDEDITW = -806;

		// Token: 0x040023BB RID: 9147
		public const int CONNECT_E_NOCONNECTION = -2147220992;

		// Token: 0x040023BC RID: 9148
		public const int CONNECT_E_CANNOTCONNECT = -2147220990;

		// Token: 0x040023BD RID: 9149
		public const int CTRLINFO_EATS_RETURN = 1;

		// Token: 0x040023BE RID: 9150
		public const int CTRLINFO_EATS_ESCAPE = 2;

		// Token: 0x040023BF RID: 9151
		public const int CSIDL_DESKTOP = 0;

		// Token: 0x040023C0 RID: 9152
		public const int CSIDL_INTERNET = 1;

		// Token: 0x040023C1 RID: 9153
		public const int CSIDL_PROGRAMS = 2;

		// Token: 0x040023C2 RID: 9154
		public const int CSIDL_PERSONAL = 5;

		// Token: 0x040023C3 RID: 9155
		public const int CSIDL_FAVORITES = 6;

		// Token: 0x040023C4 RID: 9156
		public const int CSIDL_STARTUP = 7;

		// Token: 0x040023C5 RID: 9157
		public const int CSIDL_RECENT = 8;

		// Token: 0x040023C6 RID: 9158
		public const int CSIDL_SENDTO = 9;

		// Token: 0x040023C7 RID: 9159
		public const int CSIDL_STARTMENU = 11;

		// Token: 0x040023C8 RID: 9160
		public const int CSIDL_DESKTOPDIRECTORY = 16;

		// Token: 0x040023C9 RID: 9161
		public const int CSIDL_TEMPLATES = 21;

		// Token: 0x040023CA RID: 9162
		public const int CSIDL_APPDATA = 26;

		// Token: 0x040023CB RID: 9163
		public const int CSIDL_LOCAL_APPDATA = 28;

		// Token: 0x040023CC RID: 9164
		public const int CSIDL_INTERNET_CACHE = 32;

		// Token: 0x040023CD RID: 9165
		public const int CSIDL_COOKIES = 33;

		// Token: 0x040023CE RID: 9166
		public const int CSIDL_HISTORY = 34;

		// Token: 0x040023CF RID: 9167
		public const int CSIDL_COMMON_APPDATA = 35;

		// Token: 0x040023D0 RID: 9168
		public const int CSIDL_SYSTEM = 37;

		// Token: 0x040023D1 RID: 9169
		public const int CSIDL_PROGRAM_FILES = 38;

		// Token: 0x040023D2 RID: 9170
		public const int CSIDL_PROGRAM_FILES_COMMON = 43;

		// Token: 0x040023D3 RID: 9171
		public const int DUPLICATE = 6;

		// Token: 0x040023D4 RID: 9172
		public const int DISPID_UNKNOWN = -1;

		// Token: 0x040023D5 RID: 9173
		public const int DISPID_PROPERTYPUT = -3;

		// Token: 0x040023D6 RID: 9174
		public const int DISPATCH_METHOD = 1;

		// Token: 0x040023D7 RID: 9175
		public const int DISPATCH_PROPERTYGET = 2;

		// Token: 0x040023D8 RID: 9176
		public const int DISPATCH_PROPERTYPUT = 4;

		// Token: 0x040023D9 RID: 9177
		public const int DV_E_DVASPECT = -2147221397;

		// Token: 0x040023DA RID: 9178
		public const int DISP_E_MEMBERNOTFOUND = -2147352573;

		// Token: 0x040023DB RID: 9179
		public const int DISP_E_PARAMNOTFOUND = -2147352572;

		// Token: 0x040023DC RID: 9180
		public const int DISP_E_EXCEPTION = -2147352567;

		// Token: 0x040023DD RID: 9181
		public const int DEFAULT_GUI_FONT = 17;

		// Token: 0x040023DE RID: 9182
		public const int DIB_RGB_COLORS = 0;

		// Token: 0x040023DF RID: 9183
		public const int DRAGDROP_E_NOTREGISTERED = -2147221248;

		// Token: 0x040023E0 RID: 9184
		public const int DRAGDROP_E_ALREADYREGISTERED = -2147221247;

		// Token: 0x040023E1 RID: 9185
		public const int DUPLICATE_SAME_ACCESS = 2;

		// Token: 0x040023E2 RID: 9186
		public const int DFC_CAPTION = 1;

		// Token: 0x040023E3 RID: 9187
		public const int DFC_MENU = 2;

		// Token: 0x040023E4 RID: 9188
		public const int DFC_SCROLL = 3;

		// Token: 0x040023E5 RID: 9189
		public const int DFC_BUTTON = 4;

		// Token: 0x040023E6 RID: 9190
		public const int DFCS_CAPTIONCLOSE = 0;

		// Token: 0x040023E7 RID: 9191
		public const int DFCS_CAPTIONMIN = 1;

		// Token: 0x040023E8 RID: 9192
		public const int DFCS_CAPTIONMAX = 2;

		// Token: 0x040023E9 RID: 9193
		public const int DFCS_CAPTIONRESTORE = 3;

		// Token: 0x040023EA RID: 9194
		public const int DFCS_CAPTIONHELP = 4;

		// Token: 0x040023EB RID: 9195
		public const int DFCS_MENUARROW = 0;

		// Token: 0x040023EC RID: 9196
		public const int DFCS_MENUCHECK = 1;

		// Token: 0x040023ED RID: 9197
		public const int DFCS_MENUBULLET = 2;

		// Token: 0x040023EE RID: 9198
		public const int DFCS_SCROLLUP = 0;

		// Token: 0x040023EF RID: 9199
		public const int DFCS_SCROLLDOWN = 1;

		// Token: 0x040023F0 RID: 9200
		public const int DFCS_SCROLLLEFT = 2;

		// Token: 0x040023F1 RID: 9201
		public const int DFCS_SCROLLRIGHT = 3;

		// Token: 0x040023F2 RID: 9202
		public const int DFCS_SCROLLCOMBOBOX = 5;

		// Token: 0x040023F3 RID: 9203
		public const int DFCS_BUTTONCHECK = 0;

		// Token: 0x040023F4 RID: 9204
		public const int DFCS_BUTTONRADIO = 4;

		// Token: 0x040023F5 RID: 9205
		public const int DFCS_BUTTON3STATE = 8;

		// Token: 0x040023F6 RID: 9206
		public const int DFCS_BUTTONPUSH = 16;

		// Token: 0x040023F7 RID: 9207
		public const int DFCS_INACTIVE = 256;

		// Token: 0x040023F8 RID: 9208
		public const int DFCS_PUSHED = 512;

		// Token: 0x040023F9 RID: 9209
		public const int DFCS_CHECKED = 1024;

		// Token: 0x040023FA RID: 9210
		public const int DFCS_FLAT = 16384;

		// Token: 0x040023FB RID: 9211
		public const int DCX_WINDOW = 1;

		// Token: 0x040023FC RID: 9212
		public const int DCX_CACHE = 2;

		// Token: 0x040023FD RID: 9213
		public const int DCX_LOCKWINDOWUPDATE = 1024;

		// Token: 0x040023FE RID: 9214
		public const int DCX_INTERSECTRGN = 128;

		// Token: 0x040023FF RID: 9215
		public const int DI_NORMAL = 3;

		// Token: 0x04002400 RID: 9216
		public const int DLGC_WANTARROWS = 1;

		// Token: 0x04002401 RID: 9217
		public const int DLGC_WANTTAB = 2;

		// Token: 0x04002402 RID: 9218
		public const int DLGC_WANTALLKEYS = 4;

		// Token: 0x04002403 RID: 9219
		public const int DLGC_WANTCHARS = 128;

		// Token: 0x04002404 RID: 9220
		public const int DLGC_WANTMESSAGE = 4;

		// Token: 0x04002405 RID: 9221
		public const int DLGC_HASSETSEL = 8;

		// Token: 0x04002406 RID: 9222
		public const int DTM_GETSYSTEMTIME = 4097;

		// Token: 0x04002407 RID: 9223
		public const int DTM_SETSYSTEMTIME = 4098;

		// Token: 0x04002408 RID: 9224
		public const int DTM_SETRANGE = 4100;

		// Token: 0x04002409 RID: 9225
		public const int DTM_SETFORMATA = 4101;

		// Token: 0x0400240A RID: 9226
		public const int DTM_SETFORMATW = 4146;

		// Token: 0x0400240B RID: 9227
		public const int DTM_SETMCCOLOR = 4102;

		// Token: 0x0400240C RID: 9228
		public const int DTM_GETMONTHCAL = 4104;

		// Token: 0x0400240D RID: 9229
		public const int DTM_SETMCFONT = 4105;

		// Token: 0x0400240E RID: 9230
		public const int DTS_UPDOWN = 1;

		// Token: 0x0400240F RID: 9231
		public const int DTS_SHOWNONE = 2;

		// Token: 0x04002410 RID: 9232
		public const int DTS_LONGDATEFORMAT = 4;

		// Token: 0x04002411 RID: 9233
		public const int DTS_TIMEFORMAT = 9;

		// Token: 0x04002412 RID: 9234
		public const int DTS_RIGHTALIGN = 32;

		// Token: 0x04002413 RID: 9235
		public const int DTN_DATETIMECHANGE = -759;

		// Token: 0x04002414 RID: 9236
		public const int DTN_USERSTRINGA = -758;

		// Token: 0x04002415 RID: 9237
		public const int DTN_USERSTRINGW = -745;

		// Token: 0x04002416 RID: 9238
		public const int DTN_WMKEYDOWNA = -757;

		// Token: 0x04002417 RID: 9239
		public const int DTN_WMKEYDOWNW = -744;

		// Token: 0x04002418 RID: 9240
		public const int DTN_FORMATA = -756;

		// Token: 0x04002419 RID: 9241
		public const int DTN_FORMATW = -743;

		// Token: 0x0400241A RID: 9242
		public const int DTN_FORMATQUERYA = -755;

		// Token: 0x0400241B RID: 9243
		public const int DTN_FORMATQUERYW = -742;

		// Token: 0x0400241C RID: 9244
		public const int DTN_DROPDOWN = -754;

		// Token: 0x0400241D RID: 9245
		public const int DTN_CLOSEUP = -753;

		// Token: 0x0400241E RID: 9246
		public const int DVASPECT_CONTENT = 1;

		// Token: 0x0400241F RID: 9247
		public const int DVASPECT_TRANSPARENT = 32;

		// Token: 0x04002420 RID: 9248
		public const int DVASPECT_OPAQUE = 16;

		// Token: 0x04002421 RID: 9249
		public const int E_NOTIMPL = -2147467263;

		// Token: 0x04002422 RID: 9250
		public const int E_OUTOFMEMORY = -2147024882;

		// Token: 0x04002423 RID: 9251
		public const int E_INVALIDARG = -2147024809;

		// Token: 0x04002424 RID: 9252
		public const int E_NOINTERFACE = -2147467262;

		// Token: 0x04002425 RID: 9253
		public const int E_FAIL = -2147467259;

		// Token: 0x04002426 RID: 9254
		public const int E_ABORT = -2147467260;

		// Token: 0x04002427 RID: 9255
		public const int E_UNEXPECTED = -2147418113;

		// Token: 0x04002428 RID: 9256
		public const int INET_E_DEFAULT_ACTION = -2146697199;

		// Token: 0x04002429 RID: 9257
		public const int ETO_OPAQUE = 2;

		// Token: 0x0400242A RID: 9258
		public const int ETO_CLIPPED = 4;

		// Token: 0x0400242B RID: 9259
		public const int EMR_POLYTEXTOUTA = 96;

		// Token: 0x0400242C RID: 9260
		public const int EMR_POLYTEXTOUTW = 97;

		// Token: 0x0400242D RID: 9261
		public const int EDGE_RAISED = 5;

		// Token: 0x0400242E RID: 9262
		public const int EDGE_SUNKEN = 10;

		// Token: 0x0400242F RID: 9263
		public const int EDGE_ETCHED = 6;

		// Token: 0x04002430 RID: 9264
		public const int EDGE_BUMP = 9;

		// Token: 0x04002431 RID: 9265
		public const int ES_LEFT = 0;

		// Token: 0x04002432 RID: 9266
		public const int ES_CENTER = 1;

		// Token: 0x04002433 RID: 9267
		public const int ES_RIGHT = 2;

		// Token: 0x04002434 RID: 9268
		public const int ES_MULTILINE = 4;

		// Token: 0x04002435 RID: 9269
		public const int ES_UPPERCASE = 8;

		// Token: 0x04002436 RID: 9270
		public const int ES_LOWERCASE = 16;

		// Token: 0x04002437 RID: 9271
		public const int ES_AUTOVSCROLL = 64;

		// Token: 0x04002438 RID: 9272
		public const int ES_AUTOHSCROLL = 128;

		// Token: 0x04002439 RID: 9273
		public const int ES_NOHIDESEL = 256;

		// Token: 0x0400243A RID: 9274
		public const int ES_READONLY = 2048;

		// Token: 0x0400243B RID: 9275
		public const int ES_PASSWORD = 32;

		// Token: 0x0400243C RID: 9276
		public const int EN_CHANGE = 768;

		// Token: 0x0400243D RID: 9277
		public const int EN_UPDATE = 1024;

		// Token: 0x0400243E RID: 9278
		public const int EN_HSCROLL = 1537;

		// Token: 0x0400243F RID: 9279
		public const int EN_VSCROLL = 1538;

		// Token: 0x04002440 RID: 9280
		public const int EN_ALIGN_LTR_EC = 1792;

		// Token: 0x04002441 RID: 9281
		public const int EN_ALIGN_RTL_EC = 1793;

		// Token: 0x04002442 RID: 9282
		public const int EC_LEFTMARGIN = 1;

		// Token: 0x04002443 RID: 9283
		public const int EC_RIGHTMARGIN = 2;

		// Token: 0x04002444 RID: 9284
		public const int EM_GETSEL = 176;

		// Token: 0x04002445 RID: 9285
		public const int EM_SETSEL = 177;

		// Token: 0x04002446 RID: 9286
		public const int EM_SCROLL = 181;

		// Token: 0x04002447 RID: 9287
		public const int EM_SCROLLCARET = 183;

		// Token: 0x04002448 RID: 9288
		public const int EM_GETMODIFY = 184;

		// Token: 0x04002449 RID: 9289
		public const int EM_SETMODIFY = 185;

		// Token: 0x0400244A RID: 9290
		public const int EM_GETLINECOUNT = 186;

		// Token: 0x0400244B RID: 9291
		public const int EM_REPLACESEL = 194;

		// Token: 0x0400244C RID: 9292
		public const int EM_GETLINE = 196;

		// Token: 0x0400244D RID: 9293
		public const int EM_LIMITTEXT = 197;

		// Token: 0x0400244E RID: 9294
		public const int EM_CANUNDO = 198;

		// Token: 0x0400244F RID: 9295
		public const int EM_UNDO = 199;

		// Token: 0x04002450 RID: 9296
		public const int EM_SETPASSWORDCHAR = 204;

		// Token: 0x04002451 RID: 9297
		public const int EM_GETPASSWORDCHAR = 210;

		// Token: 0x04002452 RID: 9298
		public const int EM_EMPTYUNDOBUFFER = 205;

		// Token: 0x04002453 RID: 9299
		public const int EM_SETREADONLY = 207;

		// Token: 0x04002454 RID: 9300
		public const int EM_SETMARGINS = 211;

		// Token: 0x04002455 RID: 9301
		public const int EM_POSFROMCHAR = 214;

		// Token: 0x04002456 RID: 9302
		public const int EM_CHARFROMPOS = 215;

		// Token: 0x04002457 RID: 9303
		public const int EM_LINEFROMCHAR = 201;

		// Token: 0x04002458 RID: 9304
		public const int EM_GETFIRSTVISIBLELINE = 206;

		// Token: 0x04002459 RID: 9305
		public const int EM_LINEINDEX = 187;

		// Token: 0x0400245A RID: 9306
		public const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x0400245B RID: 9307
		public const int ERROR_CLASS_ALREADY_EXISTS = 1410;

		// Token: 0x0400245C RID: 9308
		public const int FNERR_SUBCLASSFAILURE = 12289;

		// Token: 0x0400245D RID: 9309
		public const int FNERR_INVALIDFILENAME = 12290;

		// Token: 0x0400245E RID: 9310
		public const int FNERR_BUFFERTOOSMALL = 12291;

		// Token: 0x0400245F RID: 9311
		public const int FRERR_BUFFERLENGTHZERO = 16385;

		// Token: 0x04002460 RID: 9312
		public const int FADF_BSTR = 256;

		// Token: 0x04002461 RID: 9313
		public const int FADF_UNKNOWN = 512;

		// Token: 0x04002462 RID: 9314
		public const int FADF_DISPATCH = 1024;

		// Token: 0x04002463 RID: 9315
		public const int FADF_VARIANT = 2048;

		// Token: 0x04002464 RID: 9316
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04002465 RID: 9317
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04002466 RID: 9318
		public const int FVIRTKEY = 1;

		// Token: 0x04002467 RID: 9319
		public const int FSHIFT = 4;

		// Token: 0x04002468 RID: 9320
		public const int FALT = 16;

		// Token: 0x04002469 RID: 9321
		public const int GMEM_FIXED = 0;

		// Token: 0x0400246A RID: 9322
		public const int GMEM_MOVEABLE = 2;

		// Token: 0x0400246B RID: 9323
		public const int GMEM_NOCOMPACT = 16;

		// Token: 0x0400246C RID: 9324
		public const int GMEM_NODISCARD = 32;

		// Token: 0x0400246D RID: 9325
		public const int GMEM_ZEROINIT = 64;

		// Token: 0x0400246E RID: 9326
		public const int GMEM_MODIFY = 128;

		// Token: 0x0400246F RID: 9327
		public const int GMEM_DISCARDABLE = 256;

		// Token: 0x04002470 RID: 9328
		public const int GMEM_NOT_BANKED = 4096;

		// Token: 0x04002471 RID: 9329
		public const int GMEM_SHARE = 8192;

		// Token: 0x04002472 RID: 9330
		public const int GMEM_DDESHARE = 8192;

		// Token: 0x04002473 RID: 9331
		public const int GMEM_NOTIFY = 16384;

		// Token: 0x04002474 RID: 9332
		public const int GMEM_LOWER = 4096;

		// Token: 0x04002475 RID: 9333
		public const int GMEM_VALID_FLAGS = 32626;

		// Token: 0x04002476 RID: 9334
		public const int GMEM_INVALID_HANDLE = 32768;

		// Token: 0x04002477 RID: 9335
		public const int GHND = 66;

		// Token: 0x04002478 RID: 9336
		public const int GPTR = 64;

		// Token: 0x04002479 RID: 9337
		public const int GCL_WNDPROC = -24;

		// Token: 0x0400247A RID: 9338
		public const int GWL_WNDPROC = -4;

		// Token: 0x0400247B RID: 9339
		public const int GWL_HWNDPARENT = -8;

		// Token: 0x0400247C RID: 9340
		public const int GWL_STYLE = -16;

		// Token: 0x0400247D RID: 9341
		public const int GWL_EXSTYLE = -20;

		// Token: 0x0400247E RID: 9342
		public const int GWL_ID = -12;

		// Token: 0x0400247F RID: 9343
		public const int GW_HWNDFIRST = 0;

		// Token: 0x04002480 RID: 9344
		public const int GW_HWNDLAST = 1;

		// Token: 0x04002481 RID: 9345
		public const int GW_HWNDNEXT = 2;

		// Token: 0x04002482 RID: 9346
		public const int GW_HWNDPREV = 3;

		// Token: 0x04002483 RID: 9347
		public const int GW_CHILD = 5;

		// Token: 0x04002484 RID: 9348
		public const int GMR_VISIBLE = 0;

		// Token: 0x04002485 RID: 9349
		public const int GMR_DAYSTATE = 1;

		// Token: 0x04002486 RID: 9350
		public const int GDI_ERROR = -1;

		// Token: 0x04002487 RID: 9351
		public const int GDTR_MIN = 1;

		// Token: 0x04002488 RID: 9352
		public const int GDTR_MAX = 2;

		// Token: 0x04002489 RID: 9353
		public const int GDT_VALID = 0;

		// Token: 0x0400248A RID: 9354
		public const int GDT_NONE = 1;

		// Token: 0x0400248B RID: 9355
		public const int GA_PARENT = 1;

		// Token: 0x0400248C RID: 9356
		public const int GA_ROOT = 2;

		// Token: 0x0400248D RID: 9357
		public const int GCS_COMPSTR = 8;

		// Token: 0x0400248E RID: 9358
		public const int GCS_COMPATTR = 16;

		// Token: 0x0400248F RID: 9359
		public const int GCS_RESULTSTR = 2048;

		// Token: 0x04002490 RID: 9360
		public const int ATTR_INPUT = 0;

		// Token: 0x04002491 RID: 9361
		public const int ATTR_TARGET_CONVERTED = 1;

		// Token: 0x04002492 RID: 9362
		public const int ATTR_CONVERTED = 2;

		// Token: 0x04002493 RID: 9363
		public const int ATTR_TARGET_NOTCONVERTED = 3;

		// Token: 0x04002494 RID: 9364
		public const int ATTR_INPUT_ERROR = 4;

		// Token: 0x04002495 RID: 9365
		public const int ATTR_FIXEDCONVERTED = 5;

		// Token: 0x04002496 RID: 9366
		public const int NI_COMPOSITIONSTR = 21;

		// Token: 0x04002497 RID: 9367
		public const int CPS_COMPLETE = 1;

		// Token: 0x04002498 RID: 9368
		public const int CPS_CANCEL = 4;

		// Token: 0x04002499 RID: 9369
		public const int HC_ACTION = 0;

		// Token: 0x0400249A RID: 9370
		public const int HC_GETNEXT = 1;

		// Token: 0x0400249B RID: 9371
		public const int HC_SKIP = 2;

		// Token: 0x0400249C RID: 9372
		public const int HTTRANSPARENT = -1;

		// Token: 0x0400249D RID: 9373
		public const int HTNOWHERE = 0;

		// Token: 0x0400249E RID: 9374
		public const int HTCLIENT = 1;

		// Token: 0x0400249F RID: 9375
		public const int HTLEFT = 10;

		// Token: 0x040024A0 RID: 9376
		public const int HTBOTTOM = 15;

		// Token: 0x040024A1 RID: 9377
		public const int HTBOTTOMLEFT = 16;

		// Token: 0x040024A2 RID: 9378
		public const int HTBOTTOMRIGHT = 17;

		// Token: 0x040024A3 RID: 9379
		public const int HTBORDER = 18;

		// Token: 0x040024A4 RID: 9380
		public const int HELPINFO_WINDOW = 1;

		// Token: 0x040024A5 RID: 9381
		public const int HCF_HIGHCONTRASTON = 1;

		// Token: 0x040024A6 RID: 9382
		public const int HDI_ORDER = 128;

		// Token: 0x040024A7 RID: 9383
		public const int HDI_WIDTH = 1;

		// Token: 0x040024A8 RID: 9384
		public const int HDM_GETITEMCOUNT = 4608;

		// Token: 0x040024A9 RID: 9385
		public const int HDM_INSERTITEMA = 4609;

		// Token: 0x040024AA RID: 9386
		public const int HDM_INSERTITEMW = 4618;

		// Token: 0x040024AB RID: 9387
		public const int HDM_GETITEMA = 4611;

		// Token: 0x040024AC RID: 9388
		public const int HDM_GETITEMW = 4619;

		// Token: 0x040024AD RID: 9389
		public const int HDM_LAYOUT = 4613;

		// Token: 0x040024AE RID: 9390
		public const int HDM_SETITEMA = 4612;

		// Token: 0x040024AF RID: 9391
		public const int HDM_SETITEMW = 4620;

		// Token: 0x040024B0 RID: 9392
		public const int HDN_ITEMCHANGINGA = -300;

		// Token: 0x040024B1 RID: 9393
		public const int HDN_ITEMCHANGINGW = -320;

		// Token: 0x040024B2 RID: 9394
		public const int HDN_ITEMCHANGEDA = -301;

		// Token: 0x040024B3 RID: 9395
		public const int HDN_ITEMCHANGEDW = -321;

		// Token: 0x040024B4 RID: 9396
		public const int HDN_ITEMCLICKA = -302;

		// Token: 0x040024B5 RID: 9397
		public const int HDN_ITEMCLICKW = -322;

		// Token: 0x040024B6 RID: 9398
		public const int HDN_ITEMDBLCLICKA = -303;

		// Token: 0x040024B7 RID: 9399
		public const int HDN_ITEMDBLCLICKW = -323;

		// Token: 0x040024B8 RID: 9400
		public const int HDN_DIVIDERDBLCLICKA = -305;

		// Token: 0x040024B9 RID: 9401
		public const int HDN_DIVIDERDBLCLICKW = -325;

		// Token: 0x040024BA RID: 9402
		public const int HDN_BEGINTDRAG = -310;

		// Token: 0x040024BB RID: 9403
		public const int HDN_BEGINTRACKA = -306;

		// Token: 0x040024BC RID: 9404
		public const int HDN_BEGINTRACKW = -326;

		// Token: 0x040024BD RID: 9405
		public const int HDN_ENDDRAG = -311;

		// Token: 0x040024BE RID: 9406
		public const int HDN_ENDTRACKA = -307;

		// Token: 0x040024BF RID: 9407
		public const int HDN_ENDTRACKW = -327;

		// Token: 0x040024C0 RID: 9408
		public const int HDN_TRACKA = -308;

		// Token: 0x040024C1 RID: 9409
		public const int HDN_TRACKW = -328;

		// Token: 0x040024C2 RID: 9410
		public const int HDN_GETDISPINFOA = -309;

		// Token: 0x040024C3 RID: 9411
		public const int HDN_GETDISPINFOW = -329;

		// Token: 0x040024C4 RID: 9412
		public const int HDS_FULLDRAG = 128;

		// Token: 0x040024C5 RID: 9413
		public const int HBMMENU_CALLBACK = -1;

		// Token: 0x040024C6 RID: 9414
		public const int HBMMENU_SYSTEM = 1;

		// Token: 0x040024C7 RID: 9415
		public const int HBMMENU_MBAR_RESTORE = 2;

		// Token: 0x040024C8 RID: 9416
		public const int HBMMENU_MBAR_MINIMIZE = 3;

		// Token: 0x040024C9 RID: 9417
		public const int HBMMENU_MBAR_CLOSE = 5;

		// Token: 0x040024CA RID: 9418
		public const int HBMMENU_MBAR_CLOSE_D = 6;

		// Token: 0x040024CB RID: 9419
		public const int HBMMENU_MBAR_MINIMIZE_D = 7;

		// Token: 0x040024CC RID: 9420
		public const int HBMMENU_POPUP_CLOSE = 8;

		// Token: 0x040024CD RID: 9421
		public const int HBMMENU_POPUP_RESTORE = 9;

		// Token: 0x040024CE RID: 9422
		public const int HBMMENU_POPUP_MAXIMIZE = 10;

		// Token: 0x040024CF RID: 9423
		public const int HBMMENU_POPUP_MINIMIZE = 11;

		// Token: 0x040024D0 RID: 9424
		public const int IME_CMODE_NATIVE = 1;

		// Token: 0x040024D1 RID: 9425
		public const int IME_CMODE_KATAKANA = 2;

		// Token: 0x040024D2 RID: 9426
		public const int IME_CMODE_FULLSHAPE = 8;

		// Token: 0x040024D3 RID: 9427
		public const int INPLACE_E_NOTOOLSPACE = -2147221087;

		// Token: 0x040024D4 RID: 9428
		public const int ICON_SMALL = 0;

		// Token: 0x040024D5 RID: 9429
		public const int ICON_BIG = 1;

		// Token: 0x040024D6 RID: 9430
		public const int IDC_ARROW = 32512;

		// Token: 0x040024D7 RID: 9431
		public const int IDC_IBEAM = 32513;

		// Token: 0x040024D8 RID: 9432
		public const int IDC_WAIT = 32514;

		// Token: 0x040024D9 RID: 9433
		public const int IDC_CROSS = 32515;

		// Token: 0x040024DA RID: 9434
		public const int IDC_SIZEALL = 32646;

		// Token: 0x040024DB RID: 9435
		public const int IDC_SIZENWSE = 32642;

		// Token: 0x040024DC RID: 9436
		public const int IDC_SIZENESW = 32643;

		// Token: 0x040024DD RID: 9437
		public const int IDC_SIZEWE = 32644;

		// Token: 0x040024DE RID: 9438
		public const int IDC_SIZENS = 32645;

		// Token: 0x040024DF RID: 9439
		public const int IDC_UPARROW = 32516;

		// Token: 0x040024E0 RID: 9440
		public const int IDC_NO = 32648;

		// Token: 0x040024E1 RID: 9441
		public const int IDC_APPSTARTING = 32650;

		// Token: 0x040024E2 RID: 9442
		public const int IDC_HELP = 32651;

		// Token: 0x040024E3 RID: 9443
		public const int IMAGE_ICON = 1;

		// Token: 0x040024E4 RID: 9444
		public const int IMAGE_CURSOR = 2;

		// Token: 0x040024E5 RID: 9445
		public const int ICC_LISTVIEW_CLASSES = 1;

		// Token: 0x040024E6 RID: 9446
		public const int ICC_TREEVIEW_CLASSES = 2;

		// Token: 0x040024E7 RID: 9447
		public const int ICC_BAR_CLASSES = 4;

		// Token: 0x040024E8 RID: 9448
		public const int ICC_TAB_CLASSES = 8;

		// Token: 0x040024E9 RID: 9449
		public const int ICC_PROGRESS_CLASS = 32;

		// Token: 0x040024EA RID: 9450
		public const int ICC_DATE_CLASSES = 256;

		// Token: 0x040024EB RID: 9451
		public const int ILC_MASK = 1;

		// Token: 0x040024EC RID: 9452
		public const int ILC_COLOR = 0;

		// Token: 0x040024ED RID: 9453
		public const int ILC_COLOR4 = 4;

		// Token: 0x040024EE RID: 9454
		public const int ILC_COLOR8 = 8;

		// Token: 0x040024EF RID: 9455
		public const int ILC_COLOR16 = 16;

		// Token: 0x040024F0 RID: 9456
		public const int ILC_COLOR24 = 24;

		// Token: 0x040024F1 RID: 9457
		public const int ILC_COLOR32 = 32;

		// Token: 0x040024F2 RID: 9458
		public const int ILC_MIRROR = 8192;

		// Token: 0x040024F3 RID: 9459
		public const int ILD_NORMAL = 0;

		// Token: 0x040024F4 RID: 9460
		public const int ILD_TRANSPARENT = 1;

		// Token: 0x040024F5 RID: 9461
		public const int ILD_MASK = 16;

		// Token: 0x040024F6 RID: 9462
		public const int ILD_ROP = 64;

		// Token: 0x040024F7 RID: 9463
		public const int ILP_NORMAL = 0;

		// Token: 0x040024F8 RID: 9464
		public const int ILP_DOWNLEVEL = 1;

		// Token: 0x040024F9 RID: 9465
		public const int ILS_NORMAL = 0;

		// Token: 0x040024FA RID: 9466
		public const int ILS_GLOW = 1;

		// Token: 0x040024FB RID: 9467
		public const int ILS_SHADOW = 2;

		// Token: 0x040024FC RID: 9468
		public const int ILS_SATURATE = 4;

		// Token: 0x040024FD RID: 9469
		public const int ILS_ALPHA = 8;

		// Token: 0x040024FE RID: 9470
		public const int IDM_PRINT = 27;

		// Token: 0x040024FF RID: 9471
		public const int IDM_PAGESETUP = 2004;

		// Token: 0x04002500 RID: 9472
		public const int IDM_PRINTPREVIEW = 2003;

		// Token: 0x04002501 RID: 9473
		public const int IDM_PROPERTIES = 28;

		// Token: 0x04002502 RID: 9474
		public const int IDM_SAVEAS = 71;

		// Token: 0x04002503 RID: 9475
		public const int CSC_NAVIGATEFORWARD = 1;

		// Token: 0x04002504 RID: 9476
		public const int CSC_NAVIGATEBACK = 2;

		// Token: 0x04002505 RID: 9477
		public const int STG_E_INVALIDFUNCTION = -2147287039;

		// Token: 0x04002506 RID: 9478
		public const int STG_E_FILENOTFOUND = -2147287038;

		// Token: 0x04002507 RID: 9479
		public const int STG_E_PATHNOTFOUND = -2147287037;

		// Token: 0x04002508 RID: 9480
		public const int STG_E_TOOMANYOPENFILES = -2147287036;

		// Token: 0x04002509 RID: 9481
		public const int STG_E_ACCESSDENIED = -2147287035;

		// Token: 0x0400250A RID: 9482
		public const int STG_E_INVALIDHANDLE = -2147287034;

		// Token: 0x0400250B RID: 9483
		public const int STG_E_INSUFFICIENTMEMORY = -2147287032;

		// Token: 0x0400250C RID: 9484
		public const int STG_E_INVALIDPOINTER = -2147287031;

		// Token: 0x0400250D RID: 9485
		public const int STG_E_NOMOREFILES = -2147287022;

		// Token: 0x0400250E RID: 9486
		public const int STG_E_DISKISWRITEPROTECTED = -2147287021;

		// Token: 0x0400250F RID: 9487
		public const int STG_E_SEEKERROR = -2147287015;

		// Token: 0x04002510 RID: 9488
		public const int STG_E_WRITEFAULT = -2147287011;

		// Token: 0x04002511 RID: 9489
		public const int STG_E_READFAULT = -2147287010;

		// Token: 0x04002512 RID: 9490
		public const int STG_E_SHAREVIOLATION = -2147287008;

		// Token: 0x04002513 RID: 9491
		public const int STG_E_LOCKVIOLATION = -2147287007;

		// Token: 0x04002514 RID: 9492
		public const int INPUT_KEYBOARD = 1;

		// Token: 0x04002515 RID: 9493
		public const int KEYEVENTF_EXTENDEDKEY = 1;

		// Token: 0x04002516 RID: 9494
		public const int KEYEVENTF_KEYUP = 2;

		// Token: 0x04002517 RID: 9495
		public const int KEYEVENTF_UNICODE = 4;

		// Token: 0x04002518 RID: 9496
		public const int LOGPIXELSX = 88;

		// Token: 0x04002519 RID: 9497
		public const int LOGPIXELSY = 90;

		// Token: 0x0400251A RID: 9498
		public const int LB_ERR = -1;

		// Token: 0x0400251B RID: 9499
		public const int LB_ERRSPACE = -2;

		// Token: 0x0400251C RID: 9500
		public const int LBN_SELCHANGE = 1;

		// Token: 0x0400251D RID: 9501
		public const int LBN_DBLCLK = 2;

		// Token: 0x0400251E RID: 9502
		public const int LB_ADDSTRING = 384;

		// Token: 0x0400251F RID: 9503
		public const int LB_INSERTSTRING = 385;

		// Token: 0x04002520 RID: 9504
		public const int LB_DELETESTRING = 386;

		// Token: 0x04002521 RID: 9505
		public const int LB_RESETCONTENT = 388;

		// Token: 0x04002522 RID: 9506
		public const int LB_SETSEL = 389;

		// Token: 0x04002523 RID: 9507
		public const int LB_SETCURSEL = 390;

		// Token: 0x04002524 RID: 9508
		public const int LB_GETSEL = 391;

		// Token: 0x04002525 RID: 9509
		public const int LB_GETCARETINDEX = 415;

		// Token: 0x04002526 RID: 9510
		public const int LB_GETCURSEL = 392;

		// Token: 0x04002527 RID: 9511
		public const int LB_GETTEXT = 393;

		// Token: 0x04002528 RID: 9512
		public const int LB_GETTEXTLEN = 394;

		// Token: 0x04002529 RID: 9513
		public const int LB_GETTOPINDEX = 398;

		// Token: 0x0400252A RID: 9514
		public const int LB_FINDSTRING = 399;

		// Token: 0x0400252B RID: 9515
		public const int LB_GETSELCOUNT = 400;

		// Token: 0x0400252C RID: 9516
		public const int LB_GETSELITEMS = 401;

		// Token: 0x0400252D RID: 9517
		public const int LB_SETTABSTOPS = 402;

		// Token: 0x0400252E RID: 9518
		public const int LB_SETHORIZONTALEXTENT = 404;

		// Token: 0x0400252F RID: 9519
		public const int LB_SETCOLUMNWIDTH = 405;

		// Token: 0x04002530 RID: 9520
		public const int LB_SETTOPINDEX = 407;

		// Token: 0x04002531 RID: 9521
		public const int LB_GETITEMRECT = 408;

		// Token: 0x04002532 RID: 9522
		public const int LB_SETITEMHEIGHT = 416;

		// Token: 0x04002533 RID: 9523
		public const int LB_GETITEMHEIGHT = 417;

		// Token: 0x04002534 RID: 9524
		public const int LB_FINDSTRINGEXACT = 418;

		// Token: 0x04002535 RID: 9525
		public const int LB_ITEMFROMPOINT = 425;

		// Token: 0x04002536 RID: 9526
		public const int LB_SETLOCALE = 421;

		// Token: 0x04002537 RID: 9527
		public const int LBS_NOTIFY = 1;

		// Token: 0x04002538 RID: 9528
		public const int LBS_MULTIPLESEL = 8;

		// Token: 0x04002539 RID: 9529
		public const int LBS_OWNERDRAWFIXED = 16;

		// Token: 0x0400253A RID: 9530
		public const int LBS_OWNERDRAWVARIABLE = 32;

		// Token: 0x0400253B RID: 9531
		public const int LBS_HASSTRINGS = 64;

		// Token: 0x0400253C RID: 9532
		public const int LBS_USETABSTOPS = 128;

		// Token: 0x0400253D RID: 9533
		public const int LBS_NOINTEGRALHEIGHT = 256;

		// Token: 0x0400253E RID: 9534
		public const int LBS_MULTICOLUMN = 512;

		// Token: 0x0400253F RID: 9535
		public const int LBS_WANTKEYBOARDINPUT = 1024;

		// Token: 0x04002540 RID: 9536
		public const int LBS_EXTENDEDSEL = 2048;

		// Token: 0x04002541 RID: 9537
		public const int LBS_DISABLENOSCROLL = 4096;

		// Token: 0x04002542 RID: 9538
		public const int LBS_NOSEL = 16384;

		// Token: 0x04002543 RID: 9539
		public const int LOCK_WRITE = 1;

		// Token: 0x04002544 RID: 9540
		public const int LOCK_EXCLUSIVE = 2;

		// Token: 0x04002545 RID: 9541
		public const int LOCK_ONLYONCE = 4;

		// Token: 0x04002546 RID: 9542
		public const int LV_VIEW_TILE = 4;

		// Token: 0x04002547 RID: 9543
		public const int LVBKIF_SOURCE_NONE = 0;

		// Token: 0x04002548 RID: 9544
		public const int LVBKIF_SOURCE_URL = 2;

		// Token: 0x04002549 RID: 9545
		public const int LVBKIF_STYLE_NORMAL = 0;

		// Token: 0x0400254A RID: 9546
		public const int LVBKIF_STYLE_TILE = 16;

		// Token: 0x0400254B RID: 9547
		public const int LVS_ICON = 0;

		// Token: 0x0400254C RID: 9548
		public const int LVS_REPORT = 1;

		// Token: 0x0400254D RID: 9549
		public const int LVS_SMALLICON = 2;

		// Token: 0x0400254E RID: 9550
		public const int LVS_LIST = 3;

		// Token: 0x0400254F RID: 9551
		public const int LVS_SINGLESEL = 4;

		// Token: 0x04002550 RID: 9552
		public const int LVS_SHOWSELALWAYS = 8;

		// Token: 0x04002551 RID: 9553
		public const int LVS_SORTASCENDING = 16;

		// Token: 0x04002552 RID: 9554
		public const int LVS_SORTDESCENDING = 32;

		// Token: 0x04002553 RID: 9555
		public const int LVS_SHAREIMAGELISTS = 64;

		// Token: 0x04002554 RID: 9556
		public const int LVS_NOLABELWRAP = 128;

		// Token: 0x04002555 RID: 9557
		public const int LVS_AUTOARRANGE = 256;

		// Token: 0x04002556 RID: 9558
		public const int LVS_EDITLABELS = 512;

		// Token: 0x04002557 RID: 9559
		public const int LVS_NOSCROLL = 8192;

		// Token: 0x04002558 RID: 9560
		public const int LVS_ALIGNTOP = 0;

		// Token: 0x04002559 RID: 9561
		public const int LVS_ALIGNLEFT = 2048;

		// Token: 0x0400255A RID: 9562
		public const int LVS_NOCOLUMNHEADER = 16384;

		// Token: 0x0400255B RID: 9563
		public const int LVS_NOSORTHEADER = 32768;

		// Token: 0x0400255C RID: 9564
		public const int LVS_OWNERDATA = 4096;

		// Token: 0x0400255D RID: 9565
		public const int LVSCW_AUTOSIZE = -1;

		// Token: 0x0400255E RID: 9566
		public const int LVSCW_AUTOSIZE_USEHEADER = -2;

		// Token: 0x0400255F RID: 9567
		public const int LVM_REDRAWITEMS = 4117;

		// Token: 0x04002560 RID: 9568
		public const int LVM_SCROLL = 4116;

		// Token: 0x04002561 RID: 9569
		public const int LVM_SETBKCOLOR = 4097;

		// Token: 0x04002562 RID: 9570
		public const int LVM_SETBKIMAGEA = 4164;

		// Token: 0x04002563 RID: 9571
		public const int LVM_SETBKIMAGEW = 4234;

		// Token: 0x04002564 RID: 9572
		public const int LVM_SETCALLBACKMASK = 4107;

		// Token: 0x04002565 RID: 9573
		public const int LVM_GETCALLBACKMASK = 4106;

		// Token: 0x04002566 RID: 9574
		public const int LVM_GETCOLUMNORDERARRAY = 4155;

		// Token: 0x04002567 RID: 9575
		public const int LVM_GETITEMCOUNT = 4100;

		// Token: 0x04002568 RID: 9576
		public const int LVM_SETCOLUMNORDERARRAY = 4154;

		// Token: 0x04002569 RID: 9577
		public const int LVM_SETINFOTIP = 4269;

		// Token: 0x0400256A RID: 9578
		public const int LVSIL_NORMAL = 0;

		// Token: 0x0400256B RID: 9579
		public const int LVSIL_SMALL = 1;

		// Token: 0x0400256C RID: 9580
		public const int LVSIL_STATE = 2;

		// Token: 0x0400256D RID: 9581
		public const int LVM_SETIMAGELIST = 4099;

		// Token: 0x0400256E RID: 9582
		public const int LVM_SETSELECTIONMARK = 4163;

		// Token: 0x0400256F RID: 9583
		public const int LVM_SETTOOLTIPS = 4170;

		// Token: 0x04002570 RID: 9584
		public const int LVIF_TEXT = 1;

		// Token: 0x04002571 RID: 9585
		public const int LVIF_IMAGE = 2;

		// Token: 0x04002572 RID: 9586
		public const int LVIF_INDENT = 16;

		// Token: 0x04002573 RID: 9587
		public const int LVIF_PARAM = 4;

		// Token: 0x04002574 RID: 9588
		public const int LVIF_STATE = 8;

		// Token: 0x04002575 RID: 9589
		public const int LVIF_GROUPID = 256;

		// Token: 0x04002576 RID: 9590
		public const int LVIF_COLUMNS = 512;

		// Token: 0x04002577 RID: 9591
		public const int LVIS_FOCUSED = 1;

		// Token: 0x04002578 RID: 9592
		public const int LVIS_SELECTED = 2;

		// Token: 0x04002579 RID: 9593
		public const int LVIS_CUT = 4;

		// Token: 0x0400257A RID: 9594
		public const int LVIS_DROPHILITED = 8;

		// Token: 0x0400257B RID: 9595
		public const int LVIS_OVERLAYMASK = 3840;

		// Token: 0x0400257C RID: 9596
		public const int LVIS_STATEIMAGEMASK = 61440;

		// Token: 0x0400257D RID: 9597
		public const int LVM_GETITEMA = 4101;

		// Token: 0x0400257E RID: 9598
		public const int LVM_GETITEMW = 4171;

		// Token: 0x0400257F RID: 9599
		public const int LVM_SETITEMA = 4102;

		// Token: 0x04002580 RID: 9600
		public const int LVM_SETITEMW = 4172;

		// Token: 0x04002581 RID: 9601
		public const int LVM_SETITEMPOSITION32 = 4145;

		// Token: 0x04002582 RID: 9602
		public const int LVM_INSERTITEMA = 4103;

		// Token: 0x04002583 RID: 9603
		public const int LVM_INSERTITEMW = 4173;

		// Token: 0x04002584 RID: 9604
		public const int LVM_DELETEITEM = 4104;

		// Token: 0x04002585 RID: 9605
		public const int LVM_DELETECOLUMN = 4124;

		// Token: 0x04002586 RID: 9606
		public const int LVM_DELETEALLITEMS = 4105;

		// Token: 0x04002587 RID: 9607
		public const int LVM_UPDATE = 4138;

		// Token: 0x04002588 RID: 9608
		public const int LVNI_FOCUSED = 1;

		// Token: 0x04002589 RID: 9609
		public const int LVNI_SELECTED = 2;

		// Token: 0x0400258A RID: 9610
		public const int LVM_GETNEXTITEM = 4108;

		// Token: 0x0400258B RID: 9611
		public const int LVFI_PARAM = 1;

		// Token: 0x0400258C RID: 9612
		public const int LVFI_NEARESTXY = 64;

		// Token: 0x0400258D RID: 9613
		public const int LVFI_PARTIAL = 8;

		// Token: 0x0400258E RID: 9614
		public const int LVFI_STRING = 2;

		// Token: 0x0400258F RID: 9615
		public const int LVM_FINDITEMA = 4109;

		// Token: 0x04002590 RID: 9616
		public const int LVM_FINDITEMW = 4179;

		// Token: 0x04002591 RID: 9617
		public const int LVIR_BOUNDS = 0;

		// Token: 0x04002592 RID: 9618
		public const int LVIR_ICON = 1;

		// Token: 0x04002593 RID: 9619
		public const int LVIR_LABEL = 2;

		// Token: 0x04002594 RID: 9620
		public const int LVIR_SELECTBOUNDS = 3;

		// Token: 0x04002595 RID: 9621
		public const int LVM_GETITEMPOSITION = 4112;

		// Token: 0x04002596 RID: 9622
		public const int LVM_GETITEMRECT = 4110;

		// Token: 0x04002597 RID: 9623
		public const int LVM_GETSUBITEMRECT = 4152;

		// Token: 0x04002598 RID: 9624
		public const int LVM_GETSTRINGWIDTHA = 4113;

		// Token: 0x04002599 RID: 9625
		public const int LVM_GETSTRINGWIDTHW = 4183;

		// Token: 0x0400259A RID: 9626
		public const int LVHT_NOWHERE = 1;

		// Token: 0x0400259B RID: 9627
		public const int LVHT_ONITEMICON = 2;

		// Token: 0x0400259C RID: 9628
		public const int LVHT_ONITEMLABEL = 4;

		// Token: 0x0400259D RID: 9629
		public const int LVHT_ABOVE = 8;

		// Token: 0x0400259E RID: 9630
		public const int LVHT_BELOW = 16;

		// Token: 0x0400259F RID: 9631
		public const int LVHT_RIGHT = 32;

		// Token: 0x040025A0 RID: 9632
		public const int LVHT_LEFT = 64;

		// Token: 0x040025A1 RID: 9633
		public const int LVHT_ONITEM = 14;

		// Token: 0x040025A2 RID: 9634
		public const int LVHT_ONITEMSTATEICON = 8;

		// Token: 0x040025A3 RID: 9635
		public const int LVM_SUBITEMHITTEST = 4153;

		// Token: 0x040025A4 RID: 9636
		public const int LVM_HITTEST = 4114;

		// Token: 0x040025A5 RID: 9637
		public const int LVM_ENSUREVISIBLE = 4115;

		// Token: 0x040025A6 RID: 9638
		public const int LVA_DEFAULT = 0;

		// Token: 0x040025A7 RID: 9639
		public const int LVA_ALIGNLEFT = 1;

		// Token: 0x040025A8 RID: 9640
		public const int LVA_ALIGNTOP = 2;

		// Token: 0x040025A9 RID: 9641
		public const int LVA_SNAPTOGRID = 5;

		// Token: 0x040025AA RID: 9642
		public const int LVM_ARRANGE = 4118;

		// Token: 0x040025AB RID: 9643
		public const int LVM_EDITLABELA = 4119;

		// Token: 0x040025AC RID: 9644
		public const int LVM_EDITLABELW = 4214;

		// Token: 0x040025AD RID: 9645
		public const int LVCDI_ITEM = 0;

		// Token: 0x040025AE RID: 9646
		public const int LVCDI_GROUP = 1;

		// Token: 0x040025AF RID: 9647
		public const int LVCF_FMT = 1;

		// Token: 0x040025B0 RID: 9648
		public const int LVCF_WIDTH = 2;

		// Token: 0x040025B1 RID: 9649
		public const int LVCF_TEXT = 4;

		// Token: 0x040025B2 RID: 9650
		public const int LVCF_SUBITEM = 8;

		// Token: 0x040025B3 RID: 9651
		public const int LVCF_IMAGE = 16;

		// Token: 0x040025B4 RID: 9652
		public const int LVCF_ORDER = 32;

		// Token: 0x040025B5 RID: 9653
		public const int LVCFMT_IMAGE = 2048;

		// Token: 0x040025B6 RID: 9654
		public const int LVGA_HEADER_LEFT = 1;

		// Token: 0x040025B7 RID: 9655
		public const int LVGA_HEADER_CENTER = 2;

		// Token: 0x040025B8 RID: 9656
		public const int LVGA_HEADER_RIGHT = 4;

		// Token: 0x040025B9 RID: 9657
		public const int LVGA_FOOTER_LEFT = 8;

		// Token: 0x040025BA RID: 9658
		public const int LVGA_FOOTER_CENTER = 16;

		// Token: 0x040025BB RID: 9659
		public const int LVGA_FOOTER_RIGHT = 32;

		// Token: 0x040025BC RID: 9660
		public const int LVGF_NONE = 0;

		// Token: 0x040025BD RID: 9661
		public const int LVGF_HEADER = 1;

		// Token: 0x040025BE RID: 9662
		public const int LVGF_FOOTER = 2;

		// Token: 0x040025BF RID: 9663
		public const int LVGF_STATE = 4;

		// Token: 0x040025C0 RID: 9664
		public const int LVGF_ALIGN = 8;

		// Token: 0x040025C1 RID: 9665
		public const int LVGF_GROUPID = 16;

		// Token: 0x040025C2 RID: 9666
		public const int LVGS_NORMAL = 0;

		// Token: 0x040025C3 RID: 9667
		public const int LVGS_COLLAPSED = 1;

		// Token: 0x040025C4 RID: 9668
		public const int LVGS_HIDDEN = 2;

		// Token: 0x040025C5 RID: 9669
		public const int LVIM_AFTER = 1;

		// Token: 0x040025C6 RID: 9670
		public const int LVTVIF_FIXEDSIZE = 3;

		// Token: 0x040025C7 RID: 9671
		public const int LVTVIM_TILESIZE = 1;

		// Token: 0x040025C8 RID: 9672
		public const int LVTVIM_COLUMNS = 2;

		// Token: 0x040025C9 RID: 9673
		public const int LVM_ENABLEGROUPVIEW = 4253;

		// Token: 0x040025CA RID: 9674
		public const int LVM_MOVEITEMTOGROUP = 4250;

		// Token: 0x040025CB RID: 9675
		public const int LVM_GETCOLUMNA = 4121;

		// Token: 0x040025CC RID: 9676
		public const int LVM_GETCOLUMNW = 4191;

		// Token: 0x040025CD RID: 9677
		public const int LVM_SETCOLUMNA = 4122;

		// Token: 0x040025CE RID: 9678
		public const int LVM_SETCOLUMNW = 4192;

		// Token: 0x040025CF RID: 9679
		public const int LVM_INSERTCOLUMNA = 4123;

		// Token: 0x040025D0 RID: 9680
		public const int LVM_INSERTCOLUMNW = 4193;

		// Token: 0x040025D1 RID: 9681
		public const int LVM_INSERTGROUP = 4241;

		// Token: 0x040025D2 RID: 9682
		public const int LVM_REMOVEGROUP = 4246;

		// Token: 0x040025D3 RID: 9683
		public const int LVM_INSERTMARKHITTEST = 4264;

		// Token: 0x040025D4 RID: 9684
		public const int LVM_REMOVEALLGROUPS = 4256;

		// Token: 0x040025D5 RID: 9685
		public const int LVM_GETCOLUMNWIDTH = 4125;

		// Token: 0x040025D6 RID: 9686
		public const int LVM_SETCOLUMNWIDTH = 4126;

		// Token: 0x040025D7 RID: 9687
		public const int LVM_SETINSERTMARK = 4262;

		// Token: 0x040025D8 RID: 9688
		public const int LVM_GETHEADER = 4127;

		// Token: 0x040025D9 RID: 9689
		public const int LVM_SETTEXTCOLOR = 4132;

		// Token: 0x040025DA RID: 9690
		public const int LVM_SETTEXTBKCOLOR = 4134;

		// Token: 0x040025DB RID: 9691
		public const int LVM_GETTOPINDEX = 4135;

		// Token: 0x040025DC RID: 9692
		public const int LVM_SETITEMPOSITION = 4111;

		// Token: 0x040025DD RID: 9693
		public const int LVM_SETITEMSTATE = 4139;

		// Token: 0x040025DE RID: 9694
		public const int LVM_GETITEMSTATE = 4140;

		// Token: 0x040025DF RID: 9695
		public const int LVM_GETITEMTEXTA = 4141;

		// Token: 0x040025E0 RID: 9696
		public const int LVM_GETITEMTEXTW = 4211;

		// Token: 0x040025E1 RID: 9697
		public const int LVM_GETHOTITEM = 4157;

		// Token: 0x040025E2 RID: 9698
		public const int LVM_SETITEMTEXTA = 4142;

		// Token: 0x040025E3 RID: 9699
		public const int LVM_SETITEMTEXTW = 4212;

		// Token: 0x040025E4 RID: 9700
		public const int LVM_SETITEMCOUNT = 4143;

		// Token: 0x040025E5 RID: 9701
		public const int LVM_SORTITEMS = 4144;

		// Token: 0x040025E6 RID: 9702
		public const int LVM_GETSELECTEDCOUNT = 4146;

		// Token: 0x040025E7 RID: 9703
		public const int LVM_GETISEARCHSTRINGA = 4148;

		// Token: 0x040025E8 RID: 9704
		public const int LVM_GETISEARCHSTRINGW = 4213;

		// Token: 0x040025E9 RID: 9705
		public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;

		// Token: 0x040025EA RID: 9706
		public const int LVM_SETVIEW = 4238;

		// Token: 0x040025EB RID: 9707
		public const int LVM_GETGROUPINFO = 4245;

		// Token: 0x040025EC RID: 9708
		public const int LVM_SETGROUPINFO = 4243;

		// Token: 0x040025ED RID: 9709
		public const int LVM_HASGROUP = 4257;

		// Token: 0x040025EE RID: 9710
		public const int LVM_SETTILEVIEWINFO = 4258;

		// Token: 0x040025EF RID: 9711
		public const int LVM_GETTILEVIEWINFO = 4259;

		// Token: 0x040025F0 RID: 9712
		public const int LVM_GETINSERTMARK = 4263;

		// Token: 0x040025F1 RID: 9713
		public const int LVM_GETINSERTMARKRECT = 4265;

		// Token: 0x040025F2 RID: 9714
		public const int LVM_SETINSERTMARKCOLOR = 4266;

		// Token: 0x040025F3 RID: 9715
		public const int LVM_GETINSERTMARKCOLOR = 4267;

		// Token: 0x040025F4 RID: 9716
		public const int LVM_ISGROUPVIEWENABLED = 4271;

		// Token: 0x040025F5 RID: 9717
		public const int LVS_EX_GRIDLINES = 1;

		// Token: 0x040025F6 RID: 9718
		public const int LVS_EX_CHECKBOXES = 4;

		// Token: 0x040025F7 RID: 9719
		public const int LVS_EX_TRACKSELECT = 8;

		// Token: 0x040025F8 RID: 9720
		public const int LVS_EX_HEADERDRAGDROP = 16;

		// Token: 0x040025F9 RID: 9721
		public const int LVS_EX_FULLROWSELECT = 32;

		// Token: 0x040025FA RID: 9722
		public const int LVS_EX_ONECLICKACTIVATE = 64;

		// Token: 0x040025FB RID: 9723
		public const int LVS_EX_TWOCLICKACTIVATE = 128;

		// Token: 0x040025FC RID: 9724
		public const int LVS_EX_INFOTIP = 1024;

		// Token: 0x040025FD RID: 9725
		public const int LVS_EX_UNDERLINEHOT = 2048;

		// Token: 0x040025FE RID: 9726
		public const int LVS_EX_DOUBLEBUFFER = 65536;

		// Token: 0x040025FF RID: 9727
		public const int LVN_ITEMCHANGING = -100;

		// Token: 0x04002600 RID: 9728
		public const int LVN_ITEMCHANGED = -101;

		// Token: 0x04002601 RID: 9729
		public const int LVN_BEGINLABELEDITA = -105;

		// Token: 0x04002602 RID: 9730
		public const int LVN_BEGINLABELEDITW = -175;

		// Token: 0x04002603 RID: 9731
		public const int LVN_ENDLABELEDITA = -106;

		// Token: 0x04002604 RID: 9732
		public const int LVN_ENDLABELEDITW = -176;

		// Token: 0x04002605 RID: 9733
		public const int LVN_COLUMNCLICK = -108;

		// Token: 0x04002606 RID: 9734
		public const int LVN_BEGINDRAG = -109;

		// Token: 0x04002607 RID: 9735
		public const int LVN_BEGINRDRAG = -111;

		// Token: 0x04002608 RID: 9736
		public const int LVN_ODFINDITEMA = -152;

		// Token: 0x04002609 RID: 9737
		public const int LVN_ODFINDITEMW = -179;

		// Token: 0x0400260A RID: 9738
		public const int LVN_ITEMACTIVATE = -114;

		// Token: 0x0400260B RID: 9739
		public const int LVN_GETDISPINFOA = -150;

		// Token: 0x0400260C RID: 9740
		public const int LVN_GETDISPINFOW = -177;

		// Token: 0x0400260D RID: 9741
		public const int LVN_ODCACHEHINT = -113;

		// Token: 0x0400260E RID: 9742
		public const int LVN_ODSTATECHANGED = -115;

		// Token: 0x0400260F RID: 9743
		public const int LVN_SETDISPINFOA = -151;

		// Token: 0x04002610 RID: 9744
		public const int LVN_SETDISPINFOW = -178;

		// Token: 0x04002611 RID: 9745
		public const int LVN_GETINFOTIPA = -157;

		// Token: 0x04002612 RID: 9746
		public const int LVN_GETINFOTIPW = -158;

		// Token: 0x04002613 RID: 9747
		public const int LVN_KEYDOWN = -155;

		// Token: 0x04002614 RID: 9748
		public const int LWA_COLORKEY = 1;

		// Token: 0x04002615 RID: 9749
		public const int LWA_ALPHA = 2;

		// Token: 0x04002616 RID: 9750
		public const int LANG_NEUTRAL = 0;

		// Token: 0x04002617 RID: 9751
		public const int LOCALE_IFIRSTDAYOFWEEK = 4108;

		// Token: 0x04002618 RID: 9752
		public const int LOCALE_IMEASURE = 13;

		// Token: 0x04002619 RID: 9753
		public const int MEMBERID_NIL = -1;

		// Token: 0x0400261A RID: 9754
		public const int MAX_PATH = 260;

		// Token: 0x0400261B RID: 9755
		public const int MA_ACTIVATE = 1;

		// Token: 0x0400261C RID: 9756
		public const int MA_ACTIVATEANDEAT = 2;

		// Token: 0x0400261D RID: 9757
		public const int MA_NOACTIVATE = 3;

		// Token: 0x0400261E RID: 9758
		public const int MA_NOACTIVATEANDEAT = 4;

		// Token: 0x0400261F RID: 9759
		public const int MM_TEXT = 1;

		// Token: 0x04002620 RID: 9760
		public const int MM_ANISOTROPIC = 8;

		// Token: 0x04002621 RID: 9761
		public const int MK_LBUTTON = 1;

		// Token: 0x04002622 RID: 9762
		public const int MK_RBUTTON = 2;

		// Token: 0x04002623 RID: 9763
		public const int MK_SHIFT = 4;

		// Token: 0x04002624 RID: 9764
		public const int MK_CONTROL = 8;

		// Token: 0x04002625 RID: 9765
		public const int MK_MBUTTON = 16;

		// Token: 0x04002626 RID: 9766
		public const int MNC_EXECUTE = 2;

		// Token: 0x04002627 RID: 9767
		public const int MNC_SELECT = 3;

		// Token: 0x04002628 RID: 9768
		public const int MIIM_STATE = 1;

		// Token: 0x04002629 RID: 9769
		public const int MIIM_ID = 2;

		// Token: 0x0400262A RID: 9770
		public const int MIIM_SUBMENU = 4;

		// Token: 0x0400262B RID: 9771
		public const int MIIM_TYPE = 16;

		// Token: 0x0400262C RID: 9772
		public const int MIIM_DATA = 32;

		// Token: 0x0400262D RID: 9773
		public const int MIIM_STRING = 64;

		// Token: 0x0400262E RID: 9774
		public const int MIIM_BITMAP = 128;

		// Token: 0x0400262F RID: 9775
		public const int MIIM_FTYPE = 256;

		// Token: 0x04002630 RID: 9776
		public const int MB_OK = 0;

		// Token: 0x04002631 RID: 9777
		public const int MF_BYCOMMAND = 0;

		// Token: 0x04002632 RID: 9778
		public const int MF_BYPOSITION = 1024;

		// Token: 0x04002633 RID: 9779
		public const int MF_ENABLED = 0;

		// Token: 0x04002634 RID: 9780
		public const int MF_GRAYED = 1;

		// Token: 0x04002635 RID: 9781
		public const int MF_POPUP = 16;

		// Token: 0x04002636 RID: 9782
		public const int MF_SYSMENU = 8192;

		// Token: 0x04002637 RID: 9783
		public const int MFS_DISABLED = 3;

		// Token: 0x04002638 RID: 9784
		public const int MFT_MENUBREAK = 64;

		// Token: 0x04002639 RID: 9785
		public const int MFT_SEPARATOR = 2048;

		// Token: 0x0400263A RID: 9786
		public const int MFT_RIGHTORDER = 8192;

		// Token: 0x0400263B RID: 9787
		public const int MFT_RIGHTJUSTIFY = 16384;

		// Token: 0x0400263C RID: 9788
		public const int MDIS_ALLCHILDSTYLES = 1;

		// Token: 0x0400263D RID: 9789
		public const int MDITILE_VERTICAL = 0;

		// Token: 0x0400263E RID: 9790
		public const int MDITILE_HORIZONTAL = 1;

		// Token: 0x0400263F RID: 9791
		public const int MDITILE_SKIPDISABLED = 2;

		// Token: 0x04002640 RID: 9792
		public const int MCM_SETMAXSELCOUNT = 4100;

		// Token: 0x04002641 RID: 9793
		public const int MCM_SETSELRANGE = 4102;

		// Token: 0x04002642 RID: 9794
		public const int MCM_GETMONTHRANGE = 4103;

		// Token: 0x04002643 RID: 9795
		public const int MCM_GETMINREQRECT = 4105;

		// Token: 0x04002644 RID: 9796
		public const int MCM_SETCOLOR = 4106;

		// Token: 0x04002645 RID: 9797
		public const int MCM_SETTODAY = 4108;

		// Token: 0x04002646 RID: 9798
		public const int MCM_GETTODAY = 4109;

		// Token: 0x04002647 RID: 9799
		public const int MCM_HITTEST = 4110;

		// Token: 0x04002648 RID: 9800
		public const int MCM_SETFIRSTDAYOFWEEK = 4111;

		// Token: 0x04002649 RID: 9801
		public const int MCM_SETRANGE = 4114;

		// Token: 0x0400264A RID: 9802
		public const int MCM_SETMONTHDELTA = 4116;

		// Token: 0x0400264B RID: 9803
		public const int MCM_GETMAXTODAYWIDTH = 4117;

		// Token: 0x0400264C RID: 9804
		public const int MCHT_TITLE = 65536;

		// Token: 0x0400264D RID: 9805
		public const int MCHT_CALENDAR = 131072;

		// Token: 0x0400264E RID: 9806
		public const int MCHT_TODAYLINK = 196608;

		// Token: 0x0400264F RID: 9807
		public const int MCHT_TITLEBK = 65536;

		// Token: 0x04002650 RID: 9808
		public const int MCHT_TITLEMONTH = 65537;

		// Token: 0x04002651 RID: 9809
		public const int MCHT_TITLEYEAR = 65538;

		// Token: 0x04002652 RID: 9810
		public const int MCHT_TITLEBTNNEXT = 16842755;

		// Token: 0x04002653 RID: 9811
		public const int MCHT_TITLEBTNPREV = 33619971;

		// Token: 0x04002654 RID: 9812
		public const int MCHT_CALENDARBK = 131072;

		// Token: 0x04002655 RID: 9813
		public const int MCHT_CALENDARDATE = 131073;

		// Token: 0x04002656 RID: 9814
		public const int MCHT_CALENDARDATENEXT = 16908289;

		// Token: 0x04002657 RID: 9815
		public const int MCHT_CALENDARDATEPREV = 33685505;

		// Token: 0x04002658 RID: 9816
		public const int MCHT_CALENDARDAY = 131074;

		// Token: 0x04002659 RID: 9817
		public const int MCHT_CALENDARWEEKNUM = 131075;

		// Token: 0x0400265A RID: 9818
		public const int MCSC_TEXT = 1;

		// Token: 0x0400265B RID: 9819
		public const int MCSC_TITLEBK = 2;

		// Token: 0x0400265C RID: 9820
		public const int MCSC_TITLETEXT = 3;

		// Token: 0x0400265D RID: 9821
		public const int MCSC_MONTHBK = 4;

		// Token: 0x0400265E RID: 9822
		public const int MCSC_TRAILINGTEXT = 5;

		// Token: 0x0400265F RID: 9823
		public const int MCN_SELCHANGE = -749;

		// Token: 0x04002660 RID: 9824
		public const int MCN_GETDAYSTATE = -747;

		// Token: 0x04002661 RID: 9825
		public const int MCN_SELECT = -746;

		// Token: 0x04002662 RID: 9826
		public const int MCS_DAYSTATE = 1;

		// Token: 0x04002663 RID: 9827
		public const int MCS_MULTISELECT = 2;

		// Token: 0x04002664 RID: 9828
		public const int MCS_WEEKNUMBERS = 4;

		// Token: 0x04002665 RID: 9829
		public const int MCS_NOTODAYCIRCLE = 8;

		// Token: 0x04002666 RID: 9830
		public const int MCS_NOTODAY = 16;

		// Token: 0x04002667 RID: 9831
		public const int MSAA_MENU_SIG = -1441927155;

		// Token: 0x04002668 RID: 9832
		public const int NIM_ADD = 0;

		// Token: 0x04002669 RID: 9833
		public const int NIM_MODIFY = 1;

		// Token: 0x0400266A RID: 9834
		public const int NIM_DELETE = 2;

		// Token: 0x0400266B RID: 9835
		public const int NIF_MESSAGE = 1;

		// Token: 0x0400266C RID: 9836
		public const int NIM_SETVERSION = 4;

		// Token: 0x0400266D RID: 9837
		public const int NIF_ICON = 2;

		// Token: 0x0400266E RID: 9838
		public const int NIF_INFO = 16;

		// Token: 0x0400266F RID: 9839
		public const int NIF_TIP = 4;

		// Token: 0x04002670 RID: 9840
		public const int NIIF_NONE = 0;

		// Token: 0x04002671 RID: 9841
		public const int NIIF_INFO = 1;

		// Token: 0x04002672 RID: 9842
		public const int NIIF_WARNING = 2;

		// Token: 0x04002673 RID: 9843
		public const int NIIF_ERROR = 3;

		// Token: 0x04002674 RID: 9844
		public const int NIN_BALLOONSHOW = 1026;

		// Token: 0x04002675 RID: 9845
		public const int NIN_BALLOONHIDE = 1027;

		// Token: 0x04002676 RID: 9846
		public const int NIN_BALLOONTIMEOUT = 1028;

		// Token: 0x04002677 RID: 9847
		public const int NIN_BALLOONUSERCLICK = 1029;

		// Token: 0x04002678 RID: 9848
		public const int NFR_ANSI = 1;

		// Token: 0x04002679 RID: 9849
		public const int NFR_UNICODE = 2;

		// Token: 0x0400267A RID: 9850
		public const int NM_CLICK = -2;

		// Token: 0x0400267B RID: 9851
		public const int NM_DBLCLK = -3;

		// Token: 0x0400267C RID: 9852
		public const int NM_RCLICK = -5;

		// Token: 0x0400267D RID: 9853
		public const int NM_RDBLCLK = -6;

		// Token: 0x0400267E RID: 9854
		public const int NM_CUSTOMDRAW = -12;

		// Token: 0x0400267F RID: 9855
		public const int NM_RELEASEDCAPTURE = -16;

		// Token: 0x04002680 RID: 9856
		public const int NONANTIALIASED_QUALITY = 3;

		// Token: 0x04002681 RID: 9857
		public const int OFN_READONLY = 1;

		// Token: 0x04002682 RID: 9858
		public const int OFN_OVERWRITEPROMPT = 2;

		// Token: 0x04002683 RID: 9859
		public const int OFN_HIDEREADONLY = 4;

		// Token: 0x04002684 RID: 9860
		public const int OFN_NOCHANGEDIR = 8;

		// Token: 0x04002685 RID: 9861
		public const int OFN_SHOWHELP = 16;

		// Token: 0x04002686 RID: 9862
		public const int OFN_ENABLEHOOK = 32;

		// Token: 0x04002687 RID: 9863
		public const int OFN_NOVALIDATE = 256;

		// Token: 0x04002688 RID: 9864
		public const int OFN_ALLOWMULTISELECT = 512;

		// Token: 0x04002689 RID: 9865
		public const int OFN_PATHMUSTEXIST = 2048;

		// Token: 0x0400268A RID: 9866
		public const int OFN_FILEMUSTEXIST = 4096;

		// Token: 0x0400268B RID: 9867
		public const int OFN_CREATEPROMPT = 8192;

		// Token: 0x0400268C RID: 9868
		public const int OFN_EXPLORER = 524288;

		// Token: 0x0400268D RID: 9869
		public const int OFN_NODEREFERENCELINKS = 1048576;

		// Token: 0x0400268E RID: 9870
		public const int OFN_ENABLESIZING = 8388608;

		// Token: 0x0400268F RID: 9871
		public const int OFN_USESHELLITEM = 16777216;

		// Token: 0x04002690 RID: 9872
		public const int OLEIVERB_PRIMARY = 0;

		// Token: 0x04002691 RID: 9873
		public const int OLEIVERB_SHOW = -1;

		// Token: 0x04002692 RID: 9874
		public const int OLEIVERB_HIDE = -3;

		// Token: 0x04002693 RID: 9875
		public const int OLEIVERB_UIACTIVATE = -4;

		// Token: 0x04002694 RID: 9876
		public const int OLEIVERB_INPLACEACTIVATE = -5;

		// Token: 0x04002695 RID: 9877
		public const int OLEIVERB_DISCARDUNDOSTATE = -6;

		// Token: 0x04002696 RID: 9878
		public const int OLEIVERB_PROPERTIES = -7;

		// Token: 0x04002697 RID: 9879
		public const int OLE_E_INVALIDRECT = -2147221491;

		// Token: 0x04002698 RID: 9880
		public const int OLE_E_NOCONNECTION = -2147221500;

		// Token: 0x04002699 RID: 9881
		public const int OLE_E_PROMPTSAVECANCELLED = -2147221492;

		// Token: 0x0400269A RID: 9882
		public const int OLEMISC_RECOMPOSEONRESIZE = 1;

		// Token: 0x0400269B RID: 9883
		public const int OLEMISC_INSIDEOUT = 128;

		// Token: 0x0400269C RID: 9884
		public const int OLEMISC_ACTIVATEWHENVISIBLE = 256;

		// Token: 0x0400269D RID: 9885
		public const int OLEMISC_ACTSLIKEBUTTON = 4096;

		// Token: 0x0400269E RID: 9886
		public const int OLEMISC_SETCLIENTSITEFIRST = 131072;

		// Token: 0x0400269F RID: 9887
		public const int OBJ_PEN = 1;

		// Token: 0x040026A0 RID: 9888
		public const int OBJ_BRUSH = 2;

		// Token: 0x040026A1 RID: 9889
		public const int OBJ_DC = 3;

		// Token: 0x040026A2 RID: 9890
		public const int OBJ_METADC = 4;

		// Token: 0x040026A3 RID: 9891
		public const int OBJ_PAL = 5;

		// Token: 0x040026A4 RID: 9892
		public const int OBJ_FONT = 6;

		// Token: 0x040026A5 RID: 9893
		public const int OBJ_BITMAP = 7;

		// Token: 0x040026A6 RID: 9894
		public const int OBJ_REGION = 8;

		// Token: 0x040026A7 RID: 9895
		public const int OBJ_METAFILE = 9;

		// Token: 0x040026A8 RID: 9896
		public const int OBJ_MEMDC = 10;

		// Token: 0x040026A9 RID: 9897
		public const int OBJ_EXTPEN = 11;

		// Token: 0x040026AA RID: 9898
		public const int OBJ_ENHMETADC = 12;

		// Token: 0x040026AB RID: 9899
		public const int ODS_CHECKED = 8;

		// Token: 0x040026AC RID: 9900
		public const int ODS_COMBOBOXEDIT = 4096;

		// Token: 0x040026AD RID: 9901
		public const int ODS_DEFAULT = 32;

		// Token: 0x040026AE RID: 9902
		public const int ODS_DISABLED = 4;

		// Token: 0x040026AF RID: 9903
		public const int ODS_FOCUS = 16;

		// Token: 0x040026B0 RID: 9904
		public const int ODS_GRAYED = 2;

		// Token: 0x040026B1 RID: 9905
		public const int ODS_HOTLIGHT = 64;

		// Token: 0x040026B2 RID: 9906
		public const int ODS_INACTIVE = 128;

		// Token: 0x040026B3 RID: 9907
		public const int ODS_NOACCEL = 256;

		// Token: 0x040026B4 RID: 9908
		public const int ODS_NOFOCUSRECT = 512;

		// Token: 0x040026B5 RID: 9909
		public const int ODS_SELECTED = 1;

		// Token: 0x040026B6 RID: 9910
		public const int OLECLOSE_SAVEIFDIRTY = 0;

		// Token: 0x040026B7 RID: 9911
		public const int OLECLOSE_PROMPTSAVE = 2;

		// Token: 0x040026B8 RID: 9912
		public const int PDERR_SETUPFAILURE = 4097;

		// Token: 0x040026B9 RID: 9913
		public const int PDERR_PARSEFAILURE = 4098;

		// Token: 0x040026BA RID: 9914
		public const int PDERR_RETDEFFAILURE = 4099;

		// Token: 0x040026BB RID: 9915
		public const int PDERR_LOADDRVFAILURE = 4100;

		// Token: 0x040026BC RID: 9916
		public const int PDERR_GETDEVMODEFAIL = 4101;

		// Token: 0x040026BD RID: 9917
		public const int PDERR_INITFAILURE = 4102;

		// Token: 0x040026BE RID: 9918
		public const int PDERR_NODEVICES = 4103;

		// Token: 0x040026BF RID: 9919
		public const int PDERR_NODEFAULTPRN = 4104;

		// Token: 0x040026C0 RID: 9920
		public const int PDERR_DNDMMISMATCH = 4105;

		// Token: 0x040026C1 RID: 9921
		public const int PDERR_CREATEICFAILURE = 4106;

		// Token: 0x040026C2 RID: 9922
		public const int PDERR_PRINTERNOTFOUND = 4107;

		// Token: 0x040026C3 RID: 9923
		public const int PDERR_DEFAULTDIFFERENT = 4108;

		// Token: 0x040026C4 RID: 9924
		public const int PD_ALLPAGES = 0;

		// Token: 0x040026C5 RID: 9925
		public const int PD_SELECTION = 1;

		// Token: 0x040026C6 RID: 9926
		public const int PD_PAGENUMS = 2;

		// Token: 0x040026C7 RID: 9927
		public const int PD_NOSELECTION = 4;

		// Token: 0x040026C8 RID: 9928
		public const int PD_NOPAGENUMS = 8;

		// Token: 0x040026C9 RID: 9929
		public const int PD_COLLATE = 16;

		// Token: 0x040026CA RID: 9930
		public const int PD_PRINTTOFILE = 32;

		// Token: 0x040026CB RID: 9931
		public const int PD_PRINTSETUP = 64;

		// Token: 0x040026CC RID: 9932
		public const int PD_NOWARNING = 128;

		// Token: 0x040026CD RID: 9933
		public const int PD_RETURNDC = 256;

		// Token: 0x040026CE RID: 9934
		public const int PD_RETURNIC = 512;

		// Token: 0x040026CF RID: 9935
		public const int PD_RETURNDEFAULT = 1024;

		// Token: 0x040026D0 RID: 9936
		public const int PD_SHOWHELP = 2048;

		// Token: 0x040026D1 RID: 9937
		public const int PD_ENABLEPRINTHOOK = 4096;

		// Token: 0x040026D2 RID: 9938
		public const int PD_ENABLESETUPHOOK = 8192;

		// Token: 0x040026D3 RID: 9939
		public const int PD_ENABLEPRINTTEMPLATE = 16384;

		// Token: 0x040026D4 RID: 9940
		public const int PD_ENABLESETUPTEMPLATE = 32768;

		// Token: 0x040026D5 RID: 9941
		public const int PD_ENABLEPRINTTEMPLATEHANDLE = 65536;

		// Token: 0x040026D6 RID: 9942
		public const int PD_ENABLESETUPTEMPLATEHANDLE = 131072;

		// Token: 0x040026D7 RID: 9943
		public const int PD_USEDEVMODECOPIES = 262144;

		// Token: 0x040026D8 RID: 9944
		public const int PD_USEDEVMODECOPIESANDCOLLATE = 262144;

		// Token: 0x040026D9 RID: 9945
		public const int PD_DISABLEPRINTTOFILE = 524288;

		// Token: 0x040026DA RID: 9946
		public const int PD_HIDEPRINTTOFILE = 1048576;

		// Token: 0x040026DB RID: 9947
		public const int PD_NONETWORKBUTTON = 2097152;

		// Token: 0x040026DC RID: 9948
		public const int PD_CURRENTPAGE = 4194304;

		// Token: 0x040026DD RID: 9949
		public const int PD_NOCURRENTPAGE = 8388608;

		// Token: 0x040026DE RID: 9950
		public const int PD_EXCLUSIONFLAGS = 16777216;

		// Token: 0x040026DF RID: 9951
		public const int PD_USELARGETEMPLATE = 268435456;

		// Token: 0x040026E0 RID: 9952
		public const int PSD_MINMARGINS = 1;

		// Token: 0x040026E1 RID: 9953
		public const int PSD_MARGINS = 2;

		// Token: 0x040026E2 RID: 9954
		public const int PSD_INHUNDREDTHSOFMILLIMETERS = 8;

		// Token: 0x040026E3 RID: 9955
		public const int PSD_DISABLEMARGINS = 16;

		// Token: 0x040026E4 RID: 9956
		public const int PSD_DISABLEPRINTER = 32;

		// Token: 0x040026E5 RID: 9957
		public const int PSD_DISABLEORIENTATION = 256;

		// Token: 0x040026E6 RID: 9958
		public const int PSD_DISABLEPAPER = 512;

		// Token: 0x040026E7 RID: 9959
		public const int PSD_SHOWHELP = 2048;

		// Token: 0x040026E8 RID: 9960
		public const int PSD_ENABLEPAGESETUPHOOK = 8192;

		// Token: 0x040026E9 RID: 9961
		public const int PSD_NONETWORKBUTTON = 2097152;

		// Token: 0x040026EA RID: 9962
		public const int PS_SOLID = 0;

		// Token: 0x040026EB RID: 9963
		public const int PS_DOT = 2;

		// Token: 0x040026EC RID: 9964
		public const int PLANES = 14;

		// Token: 0x040026ED RID: 9965
		public const int PRF_CHECKVISIBLE = 1;

		// Token: 0x040026EE RID: 9966
		public const int PRF_NONCLIENT = 2;

		// Token: 0x040026EF RID: 9967
		public const int PRF_CLIENT = 4;

		// Token: 0x040026F0 RID: 9968
		public const int PRF_ERASEBKGND = 8;

		// Token: 0x040026F1 RID: 9969
		public const int PRF_CHILDREN = 16;

		// Token: 0x040026F2 RID: 9970
		public const int PM_NOREMOVE = 0;

		// Token: 0x040026F3 RID: 9971
		public const int PM_REMOVE = 1;

		// Token: 0x040026F4 RID: 9972
		public const int PM_NOYIELD = 2;

		// Token: 0x040026F5 RID: 9973
		public const int PBM_SETRANGE = 1025;

		// Token: 0x040026F6 RID: 9974
		public const int PBM_SETPOS = 1026;

		// Token: 0x040026F7 RID: 9975
		public const int PBM_SETSTEP = 1028;

		// Token: 0x040026F8 RID: 9976
		public const int PBM_SETRANGE32 = 1030;

		// Token: 0x040026F9 RID: 9977
		public const int PBM_SETBARCOLOR = 1033;

		// Token: 0x040026FA RID: 9978
		public const int PBM_SETMARQUEE = 1034;

		// Token: 0x040026FB RID: 9979
		public const int PBM_SETBKCOLOR = 8193;

		// Token: 0x040026FC RID: 9980
		public const int PSM_SETTITLEA = 1135;

		// Token: 0x040026FD RID: 9981
		public const int PSM_SETTITLEW = 1144;

		// Token: 0x040026FE RID: 9982
		public const int PSM_SETFINISHTEXTA = 1139;

		// Token: 0x040026FF RID: 9983
		public const int PSM_SETFINISHTEXTW = 1145;

		// Token: 0x04002700 RID: 9984
		public const int PATCOPY = 15728673;

		// Token: 0x04002701 RID: 9985
		public const int PATINVERT = 5898313;

		// Token: 0x04002702 RID: 9986
		public const int PBS_SMOOTH = 1;

		// Token: 0x04002703 RID: 9987
		public const int PBS_MARQUEE = 8;

		// Token: 0x04002704 RID: 9988
		public const int QS_KEY = 1;

		// Token: 0x04002705 RID: 9989
		public const int QS_MOUSEMOVE = 2;

		// Token: 0x04002706 RID: 9990
		public const int QS_MOUSEBUTTON = 4;

		// Token: 0x04002707 RID: 9991
		public const int QS_POSTMESSAGE = 8;

		// Token: 0x04002708 RID: 9992
		public const int QS_TIMER = 16;

		// Token: 0x04002709 RID: 9993
		public const int QS_PAINT = 32;

		// Token: 0x0400270A RID: 9994
		public const int QS_SENDMESSAGE = 64;

		// Token: 0x0400270B RID: 9995
		public const int QS_HOTKEY = 128;

		// Token: 0x0400270C RID: 9996
		public const int QS_ALLPOSTMESSAGE = 256;

		// Token: 0x0400270D RID: 9997
		public const int QS_MOUSE = 6;

		// Token: 0x0400270E RID: 9998
		public const int QS_INPUT = 7;

		// Token: 0x0400270F RID: 9999
		public const int QS_ALLEVENTS = 191;

		// Token: 0x04002710 RID: 10000
		public const int QS_ALLINPUT = 255;

		// Token: 0x04002711 RID: 10001
		public const int MWMO_INPUTAVAILABLE = 4;

		// Token: 0x04002712 RID: 10002
		public const int RECO_DROP = 1;

		// Token: 0x04002713 RID: 10003
		public const int RPC_E_CHANGED_MODE = -2147417850;

		// Token: 0x04002714 RID: 10004
		public const int RPC_E_CANTCALLOUT_ININPUTSYNCCALL = -2147417843;

		// Token: 0x04002715 RID: 10005
		public const int RGN_AND = 1;

		// Token: 0x04002716 RID: 10006
		public const int RGN_XOR = 3;

		// Token: 0x04002717 RID: 10007
		public const int RGN_DIFF = 4;

		// Token: 0x04002718 RID: 10008
		public const int RDW_INVALIDATE = 1;

		// Token: 0x04002719 RID: 10009
		public const int RDW_ERASE = 4;

		// Token: 0x0400271A RID: 10010
		public const int RDW_ALLCHILDREN = 128;

		// Token: 0x0400271B RID: 10011
		public const int RDW_ERASENOW = 512;

		// Token: 0x0400271C RID: 10012
		public const int RDW_UPDATENOW = 256;

		// Token: 0x0400271D RID: 10013
		public const int RDW_FRAME = 1024;

		// Token: 0x0400271E RID: 10014
		public const int RB_INSERTBANDA = 1025;

		// Token: 0x0400271F RID: 10015
		public const int RB_INSERTBANDW = 1034;

		// Token: 0x04002720 RID: 10016
		public const int stc4 = 1091;

		// Token: 0x04002721 RID: 10017
		public const int SHGFP_TYPE_CURRENT = 0;

		// Token: 0x04002722 RID: 10018
		public const int STGM_READ = 0;

		// Token: 0x04002723 RID: 10019
		public const int STGM_WRITE = 1;

		// Token: 0x04002724 RID: 10020
		public const int STGM_READWRITE = 2;

		// Token: 0x04002725 RID: 10021
		public const int STGM_SHARE_EXCLUSIVE = 16;

		// Token: 0x04002726 RID: 10022
		public const int STGM_CREATE = 4096;

		// Token: 0x04002727 RID: 10023
		public const int STGM_TRANSACTED = 65536;

		// Token: 0x04002728 RID: 10024
		public const int STGM_CONVERT = 131072;

		// Token: 0x04002729 RID: 10025
		public const int STGM_DELETEONRELEASE = 67108864;

		// Token: 0x0400272A RID: 10026
		public const int STARTF_USESHOWWINDOW = 1;

		// Token: 0x0400272B RID: 10027
		public const int SB_HORZ = 0;

		// Token: 0x0400272C RID: 10028
		public const int SB_VERT = 1;

		// Token: 0x0400272D RID: 10029
		public const int SB_CTL = 2;

		// Token: 0x0400272E RID: 10030
		public const int SB_LINEUP = 0;

		// Token: 0x0400272F RID: 10031
		public const int SB_LINELEFT = 0;

		// Token: 0x04002730 RID: 10032
		public const int SB_LINEDOWN = 1;

		// Token: 0x04002731 RID: 10033
		public const int SB_LINERIGHT = 1;

		// Token: 0x04002732 RID: 10034
		public const int SB_PAGEUP = 2;

		// Token: 0x04002733 RID: 10035
		public const int SB_PAGELEFT = 2;

		// Token: 0x04002734 RID: 10036
		public const int SB_PAGEDOWN = 3;

		// Token: 0x04002735 RID: 10037
		public const int SB_PAGERIGHT = 3;

		// Token: 0x04002736 RID: 10038
		public const int SB_THUMBPOSITION = 4;

		// Token: 0x04002737 RID: 10039
		public const int SB_THUMBTRACK = 5;

		// Token: 0x04002738 RID: 10040
		public const int SB_LEFT = 6;

		// Token: 0x04002739 RID: 10041
		public const int SB_RIGHT = 7;

		// Token: 0x0400273A RID: 10042
		public const int SB_ENDSCROLL = 8;

		// Token: 0x0400273B RID: 10043
		public const int SB_TOP = 6;

		// Token: 0x0400273C RID: 10044
		public const int SB_BOTTOM = 7;

		// Token: 0x0400273D RID: 10045
		public const int SIZE_RESTORED = 0;

		// Token: 0x0400273E RID: 10046
		public const int SIZE_MAXIMIZED = 2;

		// Token: 0x0400273F RID: 10047
		public const int ESB_ENABLE_BOTH = 0;

		// Token: 0x04002740 RID: 10048
		public const int ESB_DISABLE_BOTH = 3;

		// Token: 0x04002741 RID: 10049
		public const int SORT_DEFAULT = 0;

		// Token: 0x04002742 RID: 10050
		public const int SUBLANG_DEFAULT = 1;

		// Token: 0x04002743 RID: 10051
		public const int SW_HIDE = 0;

		// Token: 0x04002744 RID: 10052
		public const int SW_NORMAL = 1;

		// Token: 0x04002745 RID: 10053
		public const int SW_SHOWMINIMIZED = 2;

		// Token: 0x04002746 RID: 10054
		public const int SW_SHOWMAXIMIZED = 3;

		// Token: 0x04002747 RID: 10055
		public const int SW_MAXIMIZE = 3;

		// Token: 0x04002748 RID: 10056
		public const int SW_SHOWNOACTIVATE = 4;

		// Token: 0x04002749 RID: 10057
		public const int SW_SHOW = 5;

		// Token: 0x0400274A RID: 10058
		public const int SW_MINIMIZE = 6;

		// Token: 0x0400274B RID: 10059
		public const int SW_SHOWMINNOACTIVE = 7;

		// Token: 0x0400274C RID: 10060
		public const int SW_SHOWNA = 8;

		// Token: 0x0400274D RID: 10061
		public const int SW_RESTORE = 9;

		// Token: 0x0400274E RID: 10062
		public const int SW_MAX = 10;

		// Token: 0x0400274F RID: 10063
		public const int SWP_NOSIZE = 1;

		// Token: 0x04002750 RID: 10064
		public const int SWP_NOMOVE = 2;

		// Token: 0x04002751 RID: 10065
		public const int SWP_NOZORDER = 4;

		// Token: 0x04002752 RID: 10066
		public const int SWP_NOACTIVATE = 16;

		// Token: 0x04002753 RID: 10067
		public const int SWP_SHOWWINDOW = 64;

		// Token: 0x04002754 RID: 10068
		public const int SWP_HIDEWINDOW = 128;

		// Token: 0x04002755 RID: 10069
		public const int SWP_DRAWFRAME = 32;

		// Token: 0x04002756 RID: 10070
		public const int SWP_NOOWNERZORDER = 512;

		// Token: 0x04002757 RID: 10071
		public const int SM_CXSCREEN = 0;

		// Token: 0x04002758 RID: 10072
		public const int SM_CYSCREEN = 1;

		// Token: 0x04002759 RID: 10073
		public const int SM_CXVSCROLL = 2;

		// Token: 0x0400275A RID: 10074
		public const int SM_CYHSCROLL = 3;

		// Token: 0x0400275B RID: 10075
		public const int SM_CYCAPTION = 4;

		// Token: 0x0400275C RID: 10076
		public const int SM_CXBORDER = 5;

		// Token: 0x0400275D RID: 10077
		public const int SM_CYBORDER = 6;

		// Token: 0x0400275E RID: 10078
		public const int SM_CYVTHUMB = 9;

		// Token: 0x0400275F RID: 10079
		public const int SM_CXHTHUMB = 10;

		// Token: 0x04002760 RID: 10080
		public const int SM_CXICON = 11;

		// Token: 0x04002761 RID: 10081
		public const int SM_CYICON = 12;

		// Token: 0x04002762 RID: 10082
		public const int SM_CXCURSOR = 13;

		// Token: 0x04002763 RID: 10083
		public const int SM_CYCURSOR = 14;

		// Token: 0x04002764 RID: 10084
		public const int SM_CYMENU = 15;

		// Token: 0x04002765 RID: 10085
		public const int SM_CYKANJIWINDOW = 18;

		// Token: 0x04002766 RID: 10086
		public const int SM_MOUSEPRESENT = 19;

		// Token: 0x04002767 RID: 10087
		public const int SM_CYVSCROLL = 20;

		// Token: 0x04002768 RID: 10088
		public const int SM_CXHSCROLL = 21;

		// Token: 0x04002769 RID: 10089
		public const int SM_DEBUG = 22;

		// Token: 0x0400276A RID: 10090
		public const int SM_SWAPBUTTON = 23;

		// Token: 0x0400276B RID: 10091
		public const int SM_CXMIN = 28;

		// Token: 0x0400276C RID: 10092
		public const int SM_CYMIN = 29;

		// Token: 0x0400276D RID: 10093
		public const int SM_CXSIZE = 30;

		// Token: 0x0400276E RID: 10094
		public const int SM_CYSIZE = 31;

		// Token: 0x0400276F RID: 10095
		public const int SM_CXFRAME = 32;

		// Token: 0x04002770 RID: 10096
		public const int SM_CYFRAME = 33;

		// Token: 0x04002771 RID: 10097
		public const int SM_CXMINTRACK = 34;

		// Token: 0x04002772 RID: 10098
		public const int SM_CYMINTRACK = 35;

		// Token: 0x04002773 RID: 10099
		public const int SM_CXDOUBLECLK = 36;

		// Token: 0x04002774 RID: 10100
		public const int SM_CYDOUBLECLK = 37;

		// Token: 0x04002775 RID: 10101
		public const int SM_CXICONSPACING = 38;

		// Token: 0x04002776 RID: 10102
		public const int SM_CYICONSPACING = 39;

		// Token: 0x04002777 RID: 10103
		public const int SM_MENUDROPALIGNMENT = 40;

		// Token: 0x04002778 RID: 10104
		public const int SM_PENWINDOWS = 41;

		// Token: 0x04002779 RID: 10105
		public const int SM_DBCSENABLED = 42;

		// Token: 0x0400277A RID: 10106
		public const int SM_CMOUSEBUTTONS = 43;

		// Token: 0x0400277B RID: 10107
		public const int SM_CXFIXEDFRAME = 7;

		// Token: 0x0400277C RID: 10108
		public const int SM_CYFIXEDFRAME = 8;

		// Token: 0x0400277D RID: 10109
		public const int SM_SECURE = 44;

		// Token: 0x0400277E RID: 10110
		public const int SM_CXEDGE = 45;

		// Token: 0x0400277F RID: 10111
		public const int SM_CYEDGE = 46;

		// Token: 0x04002780 RID: 10112
		public const int SM_CXMINSPACING = 47;

		// Token: 0x04002781 RID: 10113
		public const int SM_CYMINSPACING = 48;

		// Token: 0x04002782 RID: 10114
		public const int SM_CXSMICON = 49;

		// Token: 0x04002783 RID: 10115
		public const int SM_CYSMICON = 50;

		// Token: 0x04002784 RID: 10116
		public const int SM_CYSMCAPTION = 51;

		// Token: 0x04002785 RID: 10117
		public const int SM_CXSMSIZE = 52;

		// Token: 0x04002786 RID: 10118
		public const int SM_CYSMSIZE = 53;

		// Token: 0x04002787 RID: 10119
		public const int SM_CXMENUSIZE = 54;

		// Token: 0x04002788 RID: 10120
		public const int SM_CYMENUSIZE = 55;

		// Token: 0x04002789 RID: 10121
		public const int SM_ARRANGE = 56;

		// Token: 0x0400278A RID: 10122
		public const int SM_CXMINIMIZED = 57;

		// Token: 0x0400278B RID: 10123
		public const int SM_CYMINIMIZED = 58;

		// Token: 0x0400278C RID: 10124
		public const int SM_CXMAXTRACK = 59;

		// Token: 0x0400278D RID: 10125
		public const int SM_CYMAXTRACK = 60;

		// Token: 0x0400278E RID: 10126
		public const int SM_CXMAXIMIZED = 61;

		// Token: 0x0400278F RID: 10127
		public const int SM_CYMAXIMIZED = 62;

		// Token: 0x04002790 RID: 10128
		public const int SM_NETWORK = 63;

		// Token: 0x04002791 RID: 10129
		public const int SM_CLEANBOOT = 67;

		// Token: 0x04002792 RID: 10130
		public const int SM_CXDRAG = 68;

		// Token: 0x04002793 RID: 10131
		public const int SM_CYDRAG = 69;

		// Token: 0x04002794 RID: 10132
		public const int SM_SHOWSOUNDS = 70;

		// Token: 0x04002795 RID: 10133
		public const int SM_CXMENUCHECK = 71;

		// Token: 0x04002796 RID: 10134
		public const int SM_CYMENUCHECK = 72;

		// Token: 0x04002797 RID: 10135
		public const int SM_MIDEASTENABLED = 74;

		// Token: 0x04002798 RID: 10136
		public const int SM_MOUSEWHEELPRESENT = 75;

		// Token: 0x04002799 RID: 10137
		public const int SM_XVIRTUALSCREEN = 76;

		// Token: 0x0400279A RID: 10138
		public const int SM_YVIRTUALSCREEN = 77;

		// Token: 0x0400279B RID: 10139
		public const int SM_CXVIRTUALSCREEN = 78;

		// Token: 0x0400279C RID: 10140
		public const int SM_CYVIRTUALSCREEN = 79;

		// Token: 0x0400279D RID: 10141
		public const int SM_CMONITORS = 80;

		// Token: 0x0400279E RID: 10142
		public const int SM_SAMEDISPLAYFORMAT = 81;

		// Token: 0x0400279F RID: 10143
		public const int SM_REMOTESESSION = 4096;

		// Token: 0x040027A0 RID: 10144
		public const int HLP_FILE = 1;

		// Token: 0x040027A1 RID: 10145
		public const int HLP_KEYWORD = 2;

		// Token: 0x040027A2 RID: 10146
		public const int HLP_NAVIGATOR = 3;

		// Token: 0x040027A3 RID: 10147
		public const int HLP_OBJECT = 4;

		// Token: 0x040027A4 RID: 10148
		public const int SW_SCROLLCHILDREN = 1;

		// Token: 0x040027A5 RID: 10149
		public const int SW_INVALIDATE = 2;

		// Token: 0x040027A6 RID: 10150
		public const int SW_ERASE = 4;

		// Token: 0x040027A7 RID: 10151
		public const int SW_SMOOTHSCROLL = 16;

		// Token: 0x040027A8 RID: 10152
		public const int SC_SIZE = 61440;

		// Token: 0x040027A9 RID: 10153
		public const int SC_MINIMIZE = 61472;

		// Token: 0x040027AA RID: 10154
		public const int SC_MAXIMIZE = 61488;

		// Token: 0x040027AB RID: 10155
		public const int SC_CLOSE = 61536;

		// Token: 0x040027AC RID: 10156
		public const int SC_KEYMENU = 61696;

		// Token: 0x040027AD RID: 10157
		public const int SC_RESTORE = 61728;

		// Token: 0x040027AE RID: 10158
		public const int SC_MOVE = 61456;

		// Token: 0x040027AF RID: 10159
		public const int SC_CONTEXTHELP = 61824;

		// Token: 0x040027B0 RID: 10160
		public const int SS_LEFT = 0;

		// Token: 0x040027B1 RID: 10161
		public const int SS_CENTER = 1;

		// Token: 0x040027B2 RID: 10162
		public const int SS_RIGHT = 2;

		// Token: 0x040027B3 RID: 10163
		public const int SS_OWNERDRAW = 13;

		// Token: 0x040027B4 RID: 10164
		public const int SS_NOPREFIX = 128;

		// Token: 0x040027B5 RID: 10165
		public const int SS_SUNKEN = 4096;

		// Token: 0x040027B6 RID: 10166
		public const int SBS_HORZ = 0;

		// Token: 0x040027B7 RID: 10167
		public const int SBS_VERT = 1;

		// Token: 0x040027B8 RID: 10168
		public const int SIF_RANGE = 1;

		// Token: 0x040027B9 RID: 10169
		public const int SIF_PAGE = 2;

		// Token: 0x040027BA RID: 10170
		public const int SIF_POS = 4;

		// Token: 0x040027BB RID: 10171
		public const int SIF_TRACKPOS = 16;

		// Token: 0x040027BC RID: 10172
		public const int SIF_ALL = 23;

		// Token: 0x040027BD RID: 10173
		public const int SPI_GETFONTSMOOTHING = 74;

		// Token: 0x040027BE RID: 10174
		public const int SPI_GETDROPSHADOW = 4132;

		// Token: 0x040027BF RID: 10175
		public const int SPI_GETFLATMENU = 4130;

		// Token: 0x040027C0 RID: 10176
		public const int SPI_GETFONTSMOOTHINGTYPE = 8202;

		// Token: 0x040027C1 RID: 10177
		public const int SPI_GETFONTSMOOTHINGCONTRAST = 8204;

		// Token: 0x040027C2 RID: 10178
		public const int SPI_ICONHORIZONTALSPACING = 13;

		// Token: 0x040027C3 RID: 10179
		public const int SPI_ICONVERTICALSPACING = 24;

		// Token: 0x040027C4 RID: 10180
		public const int SPI_GETICONTITLEWRAP = 25;

		// Token: 0x040027C5 RID: 10181
		public const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x040027C6 RID: 10182
		public const int SPI_GETKEYBOARDCUES = 4106;

		// Token: 0x040027C7 RID: 10183
		public const int SPI_GETKEYBOARDDELAY = 22;

		// Token: 0x040027C8 RID: 10184
		public const int SPI_GETKEYBOARDPREF = 68;

		// Token: 0x040027C9 RID: 10185
		public const int SPI_GETKEYBOARDSPEED = 10;

		// Token: 0x040027CA RID: 10186
		public const int SPI_GETMOUSEHOVERWIDTH = 98;

		// Token: 0x040027CB RID: 10187
		public const int SPI_GETMOUSEHOVERHEIGHT = 100;

		// Token: 0x040027CC RID: 10188
		public const int SPI_GETMOUSEHOVERTIME = 102;

		// Token: 0x040027CD RID: 10189
		public const int SPI_GETMOUSESPEED = 112;

		// Token: 0x040027CE RID: 10190
		public const int SPI_GETMENUDROPALIGNMENT = 27;

		// Token: 0x040027CF RID: 10191
		public const int SPI_GETMENUFADE = 4114;

		// Token: 0x040027D0 RID: 10192
		public const int SPI_GETMENUSHOWDELAY = 106;

		// Token: 0x040027D1 RID: 10193
		public const int SPI_GETCOMBOBOXANIMATION = 4100;

		// Token: 0x040027D2 RID: 10194
		public const int SPI_GETGRADIENTCAPTIONS = 4104;

		// Token: 0x040027D3 RID: 10195
		public const int SPI_GETHOTTRACKING = 4110;

		// Token: 0x040027D4 RID: 10196
		public const int SPI_GETLISTBOXSMOOTHSCROLLING = 4102;

		// Token: 0x040027D5 RID: 10197
		public const int SPI_GETMENUANIMATION = 4098;

		// Token: 0x040027D6 RID: 10198
		public const int SPI_GETSELECTIONFADE = 4116;

		// Token: 0x040027D7 RID: 10199
		public const int SPI_GETTOOLTIPANIMATION = 4118;

		// Token: 0x040027D8 RID: 10200
		public const int SPI_GETUIEFFECTS = 4158;

		// Token: 0x040027D9 RID: 10201
		public const int SPI_GETACTIVEWINDOWTRACKING = 4096;

		// Token: 0x040027DA RID: 10202
		public const int SPI_GETACTIVEWNDTRKTIMEOUT = 8194;

		// Token: 0x040027DB RID: 10203
		public const int SPI_GETANIMATION = 72;

		// Token: 0x040027DC RID: 10204
		public const int SPI_GETBORDER = 5;

		// Token: 0x040027DD RID: 10205
		public const int SPI_GETCARETWIDTH = 8198;

		// Token: 0x040027DE RID: 10206
		public const int SM_CYFOCUSBORDER = 84;

		// Token: 0x040027DF RID: 10207
		public const int SM_CXFOCUSBORDER = 83;

		// Token: 0x040027E0 RID: 10208
		public const int SM_CYSIZEFRAME = 33;

		// Token: 0x040027E1 RID: 10209
		public const int SM_CXSIZEFRAME = 32;

		// Token: 0x040027E2 RID: 10210
		public const int SPI_GETDRAGFULLWINDOWS = 38;

		// Token: 0x040027E3 RID: 10211
		public const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x040027E4 RID: 10212
		public const int SPI_GETWORKAREA = 48;

		// Token: 0x040027E5 RID: 10213
		public const int SPI_GETHIGHCONTRAST = 66;

		// Token: 0x040027E6 RID: 10214
		public const int SPI_GETDEFAULTINPUTLANG = 89;

		// Token: 0x040027E7 RID: 10215
		public const int SPI_GETSNAPTODEFBUTTON = 95;

		// Token: 0x040027E8 RID: 10216
		public const int SPI_GETWHEELSCROLLLINES = 104;

		// Token: 0x040027E9 RID: 10217
		public const int SBARS_SIZEGRIP = 256;

		// Token: 0x040027EA RID: 10218
		public const int SB_SETTEXTA = 1025;

		// Token: 0x040027EB RID: 10219
		public const int SB_SETTEXTW = 1035;

		// Token: 0x040027EC RID: 10220
		public const int SB_GETTEXTA = 1026;

		// Token: 0x040027ED RID: 10221
		public const int SB_GETTEXTW = 1037;

		// Token: 0x040027EE RID: 10222
		public const int SB_GETTEXTLENGTHA = 1027;

		// Token: 0x040027EF RID: 10223
		public const int SB_GETTEXTLENGTHW = 1036;

		// Token: 0x040027F0 RID: 10224
		public const int SB_SETPARTS = 1028;

		// Token: 0x040027F1 RID: 10225
		public const int SB_SIMPLE = 1033;

		// Token: 0x040027F2 RID: 10226
		public const int SB_GETRECT = 1034;

		// Token: 0x040027F3 RID: 10227
		public const int SB_SETICON = 1039;

		// Token: 0x040027F4 RID: 10228
		public const int SB_SETTIPTEXTA = 1040;

		// Token: 0x040027F5 RID: 10229
		public const int SB_SETTIPTEXTW = 1041;

		// Token: 0x040027F6 RID: 10230
		public const int SB_GETTIPTEXTA = 1042;

		// Token: 0x040027F7 RID: 10231
		public const int SB_GETTIPTEXTW = 1043;

		// Token: 0x040027F8 RID: 10232
		public const int SBT_OWNERDRAW = 4096;

		// Token: 0x040027F9 RID: 10233
		public const int SBT_NOBORDERS = 256;

		// Token: 0x040027FA RID: 10234
		public const int SBT_POPOUT = 512;

		// Token: 0x040027FB RID: 10235
		public const int SBT_RTLREADING = 1024;

		// Token: 0x040027FC RID: 10236
		public const int SRCCOPY = 13369376;

		// Token: 0x040027FD RID: 10237
		public const int SRCAND = 8913094;

		// Token: 0x040027FE RID: 10238
		public const int SRCPAINT = 15597702;

		// Token: 0x040027FF RID: 10239
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x04002800 RID: 10240
		public const int STATFLAG_DEFAULT = 0;

		// Token: 0x04002801 RID: 10241
		public const int STATFLAG_NONAME = 1;

		// Token: 0x04002802 RID: 10242
		public const int STATFLAG_NOOPEN = 2;

		// Token: 0x04002803 RID: 10243
		public const int STGC_DEFAULT = 0;

		// Token: 0x04002804 RID: 10244
		public const int STGC_OVERWRITE = 1;

		// Token: 0x04002805 RID: 10245
		public const int STGC_ONLYIFCURRENT = 2;

		// Token: 0x04002806 RID: 10246
		public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4;

		// Token: 0x04002807 RID: 10247
		public const int STREAM_SEEK_SET = 0;

		// Token: 0x04002808 RID: 10248
		public const int STREAM_SEEK_CUR = 1;

		// Token: 0x04002809 RID: 10249
		public const int STREAM_SEEK_END = 2;

		// Token: 0x0400280A RID: 10250
		public const int S_OK = 0;

		// Token: 0x0400280B RID: 10251
		public const int S_FALSE = 1;

		// Token: 0x0400280C RID: 10252
		public const int TRANSPARENT = 1;

		// Token: 0x0400280D RID: 10253
		public const int OPAQUE = 2;

		// Token: 0x0400280E RID: 10254
		public const int TME_HOVER = 1;

		// Token: 0x0400280F RID: 10255
		public const int TME_LEAVE = 2;

		// Token: 0x04002810 RID: 10256
		public const int TPM_LEFTBUTTON = 0;

		// Token: 0x04002811 RID: 10257
		public const int TPM_RIGHTBUTTON = 2;

		// Token: 0x04002812 RID: 10258
		public const int TPM_LEFTALIGN = 0;

		// Token: 0x04002813 RID: 10259
		public const int TPM_RIGHTALIGN = 8;

		// Token: 0x04002814 RID: 10260
		public const int TPM_VERTICAL = 64;

		// Token: 0x04002815 RID: 10261
		public const int TV_FIRST = 4352;

		// Token: 0x04002816 RID: 10262
		public const int TBSTATE_CHECKED = 1;

		// Token: 0x04002817 RID: 10263
		public const int TBSTATE_ENABLED = 4;

		// Token: 0x04002818 RID: 10264
		public const int TBSTATE_HIDDEN = 8;

		// Token: 0x04002819 RID: 10265
		public const int TBSTATE_INDETERMINATE = 16;

		// Token: 0x0400281A RID: 10266
		public const int TBSTYLE_BUTTON = 0;

		// Token: 0x0400281B RID: 10267
		public const int TBSTYLE_SEP = 1;

		// Token: 0x0400281C RID: 10268
		public const int TBSTYLE_CHECK = 2;

		// Token: 0x0400281D RID: 10269
		public const int TBSTYLE_DROPDOWN = 8;

		// Token: 0x0400281E RID: 10270
		public const int TBSTYLE_TOOLTIPS = 256;

		// Token: 0x0400281F RID: 10271
		public const int TBSTYLE_FLAT = 2048;

		// Token: 0x04002820 RID: 10272
		public const int TBSTYLE_LIST = 4096;

		// Token: 0x04002821 RID: 10273
		public const int TBSTYLE_EX_DRAWDDARROWS = 1;

		// Token: 0x04002822 RID: 10274
		public const int TB_ENABLEBUTTON = 1025;

		// Token: 0x04002823 RID: 10275
		public const int TB_ISBUTTONCHECKED = 1034;

		// Token: 0x04002824 RID: 10276
		public const int TB_ISBUTTONINDETERMINATE = 1037;

		// Token: 0x04002825 RID: 10277
		public const int TB_ADDBUTTONSA = 1044;

		// Token: 0x04002826 RID: 10278
		public const int TB_ADDBUTTONSW = 1092;

		// Token: 0x04002827 RID: 10279
		public const int TB_INSERTBUTTONA = 1045;

		// Token: 0x04002828 RID: 10280
		public const int TB_INSERTBUTTONW = 1091;

		// Token: 0x04002829 RID: 10281
		public const int TB_DELETEBUTTON = 1046;

		// Token: 0x0400282A RID: 10282
		public const int TB_GETBUTTON = 1047;

		// Token: 0x0400282B RID: 10283
		public const int TB_SAVERESTOREA = 1050;

		// Token: 0x0400282C RID: 10284
		public const int TB_SAVERESTOREW = 1100;

		// Token: 0x0400282D RID: 10285
		public const int TB_ADDSTRINGA = 1052;

		// Token: 0x0400282E RID: 10286
		public const int TB_ADDSTRINGW = 1101;

		// Token: 0x0400282F RID: 10287
		public const int TB_BUTTONSTRUCTSIZE = 1054;

		// Token: 0x04002830 RID: 10288
		public const int TB_SETBUTTONSIZE = 1055;

		// Token: 0x04002831 RID: 10289
		public const int TB_AUTOSIZE = 1057;

		// Token: 0x04002832 RID: 10290
		public const int TB_GETROWS = 1064;

		// Token: 0x04002833 RID: 10291
		public const int TB_GETBUTTONTEXTA = 1069;

		// Token: 0x04002834 RID: 10292
		public const int TB_GETBUTTONTEXTW = 1099;

		// Token: 0x04002835 RID: 10293
		public const int TB_SETIMAGELIST = 1072;

		// Token: 0x04002836 RID: 10294
		public const int TB_GETRECT = 1075;

		// Token: 0x04002837 RID: 10295
		public const int TB_GETBUTTONSIZE = 1082;

		// Token: 0x04002838 RID: 10296
		public const int TB_GETBUTTONINFOW = 1087;

		// Token: 0x04002839 RID: 10297
		public const int TB_SETBUTTONINFOW = 1088;

		// Token: 0x0400283A RID: 10298
		public const int TB_GETBUTTONINFOA = 1089;

		// Token: 0x0400283B RID: 10299
		public const int TB_SETBUTTONINFOA = 1090;

		// Token: 0x0400283C RID: 10300
		public const int TB_MAPACCELERATORA = 1102;

		// Token: 0x0400283D RID: 10301
		public const int TB_SETEXTENDEDSTYLE = 1108;

		// Token: 0x0400283E RID: 10302
		public const int TB_MAPACCELERATORW = 1114;

		// Token: 0x0400283F RID: 10303
		public const int TB_GETTOOLTIPS = 1059;

		// Token: 0x04002840 RID: 10304
		public const int TB_SETTOOLTIPS = 1060;

		// Token: 0x04002841 RID: 10305
		public const int TBIF_IMAGE = 1;

		// Token: 0x04002842 RID: 10306
		public const int TBIF_TEXT = 2;

		// Token: 0x04002843 RID: 10307
		public const int TBIF_STATE = 4;

		// Token: 0x04002844 RID: 10308
		public const int TBIF_STYLE = 8;

		// Token: 0x04002845 RID: 10309
		public const int TBIF_COMMAND = 32;

		// Token: 0x04002846 RID: 10310
		public const int TBIF_SIZE = 64;

		// Token: 0x04002847 RID: 10311
		public const int TBN_GETBUTTONINFOA = -700;

		// Token: 0x04002848 RID: 10312
		public const int TBN_GETBUTTONINFOW = -720;

		// Token: 0x04002849 RID: 10313
		public const int TBN_QUERYINSERT = -706;

		// Token: 0x0400284A RID: 10314
		public const int TBN_DROPDOWN = -710;

		// Token: 0x0400284B RID: 10315
		public const int TBN_HOTITEMCHANGE = -713;

		// Token: 0x0400284C RID: 10316
		public const int TBN_GETDISPINFOA = -716;

		// Token: 0x0400284D RID: 10317
		public const int TBN_GETDISPINFOW = -717;

		// Token: 0x0400284E RID: 10318
		public const int TBN_GETINFOTIPA = -718;

		// Token: 0x0400284F RID: 10319
		public const int TBN_GETINFOTIPW = -719;

		// Token: 0x04002850 RID: 10320
		public const int TTS_ALWAYSTIP = 1;

		// Token: 0x04002851 RID: 10321
		public const int TTS_NOPREFIX = 2;

		// Token: 0x04002852 RID: 10322
		public const int TTS_NOANIMATE = 16;

		// Token: 0x04002853 RID: 10323
		public const int TTS_NOFADE = 32;

		// Token: 0x04002854 RID: 10324
		public const int TTS_BALLOON = 64;

		// Token: 0x04002855 RID: 10325
		public const int TTI_WARNING = 2;

		// Token: 0x04002856 RID: 10326
		public const int TTF_IDISHWND = 1;

		// Token: 0x04002857 RID: 10327
		public const int TTF_RTLREADING = 4;

		// Token: 0x04002858 RID: 10328
		public const int TTF_TRACK = 32;

		// Token: 0x04002859 RID: 10329
		public const int TTF_CENTERTIP = 2;

		// Token: 0x0400285A RID: 10330
		public const int TTF_SUBCLASS = 16;

		// Token: 0x0400285B RID: 10331
		public const int TTF_TRANSPARENT = 256;

		// Token: 0x0400285C RID: 10332
		public const int TTF_ABSOLUTE = 128;

		// Token: 0x0400285D RID: 10333
		public const int TTDT_AUTOMATIC = 0;

		// Token: 0x0400285E RID: 10334
		public const int TTDT_RESHOW = 1;

		// Token: 0x0400285F RID: 10335
		public const int TTDT_AUTOPOP = 2;

		// Token: 0x04002860 RID: 10336
		public const int TTDT_INITIAL = 3;

		// Token: 0x04002861 RID: 10337
		public const int TTM_TRACKACTIVATE = 1041;

		// Token: 0x04002862 RID: 10338
		public const int TTM_TRACKPOSITION = 1042;

		// Token: 0x04002863 RID: 10339
		public const int TTM_ACTIVATE = 1025;

		// Token: 0x04002864 RID: 10340
		public const int TTM_POP = 1052;

		// Token: 0x04002865 RID: 10341
		public const int TTM_ADJUSTRECT = 1055;

		// Token: 0x04002866 RID: 10342
		public const int TTM_SETDELAYTIME = 1027;

		// Token: 0x04002867 RID: 10343
		public const int TTM_SETTITLEA = 1056;

		// Token: 0x04002868 RID: 10344
		public const int TTM_SETTITLEW = 1057;

		// Token: 0x04002869 RID: 10345
		public const int TTM_ADDTOOLA = 1028;

		// Token: 0x0400286A RID: 10346
		public const int TTM_ADDTOOLW = 1074;

		// Token: 0x0400286B RID: 10347
		public const int TTM_DELTOOLA = 1029;

		// Token: 0x0400286C RID: 10348
		public const int TTM_DELTOOLW = 1075;

		// Token: 0x0400286D RID: 10349
		public const int TTM_NEWTOOLRECTA = 1030;

		// Token: 0x0400286E RID: 10350
		public const int TTM_NEWTOOLRECTW = 1076;

		// Token: 0x0400286F RID: 10351
		public const int TTM_RELAYEVENT = 1031;

		// Token: 0x04002870 RID: 10352
		public const int TTM_GETTIPBKCOLOR = 1046;

		// Token: 0x04002871 RID: 10353
		public const int TTM_SETTIPBKCOLOR = 1043;

		// Token: 0x04002872 RID: 10354
		public const int TTM_SETTIPTEXTCOLOR = 1044;

		// Token: 0x04002873 RID: 10355
		public const int TTM_GETTIPTEXTCOLOR = 1047;

		// Token: 0x04002874 RID: 10356
		public const int TTM_GETTOOLINFOA = 1032;

		// Token: 0x04002875 RID: 10357
		public const int TTM_GETTOOLINFOW = 1077;

		// Token: 0x04002876 RID: 10358
		public const int TTM_SETTOOLINFOA = 1033;

		// Token: 0x04002877 RID: 10359
		public const int TTM_SETTOOLINFOW = 1078;

		// Token: 0x04002878 RID: 10360
		public const int TTM_HITTESTA = 1034;

		// Token: 0x04002879 RID: 10361
		public const int TTM_HITTESTW = 1079;

		// Token: 0x0400287A RID: 10362
		public const int TTM_GETTEXTA = 1035;

		// Token: 0x0400287B RID: 10363
		public const int TTM_GETTEXTW = 1080;

		// Token: 0x0400287C RID: 10364
		public const int TTM_UPDATE = 1053;

		// Token: 0x0400287D RID: 10365
		public const int TTM_UPDATETIPTEXTA = 1036;

		// Token: 0x0400287E RID: 10366
		public const int TTM_UPDATETIPTEXTW = 1081;

		// Token: 0x0400287F RID: 10367
		public const int TTM_ENUMTOOLSA = 1038;

		// Token: 0x04002880 RID: 10368
		public const int TTM_ENUMTOOLSW = 1082;

		// Token: 0x04002881 RID: 10369
		public const int TTM_GETCURRENTTOOLA = 1039;

		// Token: 0x04002882 RID: 10370
		public const int TTM_GETCURRENTTOOLW = 1083;

		// Token: 0x04002883 RID: 10371
		public const int TTM_WINDOWFROMPOINT = 1040;

		// Token: 0x04002884 RID: 10372
		public const int TTM_GETDELAYTIME = 1045;

		// Token: 0x04002885 RID: 10373
		public const int TTM_SETMAXTIPWIDTH = 1048;

		// Token: 0x04002886 RID: 10374
		public const int TTN_GETDISPINFOA = -520;

		// Token: 0x04002887 RID: 10375
		public const int TTN_GETDISPINFOW = -530;

		// Token: 0x04002888 RID: 10376
		public const int TTN_SHOW = -521;

		// Token: 0x04002889 RID: 10377
		public const int TTN_POP = -522;

		// Token: 0x0400288A RID: 10378
		public const int TTN_NEEDTEXTA = -520;

		// Token: 0x0400288B RID: 10379
		public const int TTN_NEEDTEXTW = -530;

		// Token: 0x0400288C RID: 10380
		public const int TBS_AUTOTICKS = 1;

		// Token: 0x0400288D RID: 10381
		public const int TBS_VERT = 2;

		// Token: 0x0400288E RID: 10382
		public const int TBS_TOP = 4;

		// Token: 0x0400288F RID: 10383
		public const int TBS_BOTTOM = 0;

		// Token: 0x04002890 RID: 10384
		public const int TBS_BOTH = 8;

		// Token: 0x04002891 RID: 10385
		public const int TBS_NOTICKS = 16;

		// Token: 0x04002892 RID: 10386
		public const int TBM_GETPOS = 1024;

		// Token: 0x04002893 RID: 10387
		public const int TBM_SETTIC = 1028;

		// Token: 0x04002894 RID: 10388
		public const int TBM_SETPOS = 1029;

		// Token: 0x04002895 RID: 10389
		public const int TBM_SETRANGE = 1030;

		// Token: 0x04002896 RID: 10390
		public const int TBM_SETRANGEMIN = 1031;

		// Token: 0x04002897 RID: 10391
		public const int TBM_SETRANGEMAX = 1032;

		// Token: 0x04002898 RID: 10392
		public const int TBM_SETTICFREQ = 1044;

		// Token: 0x04002899 RID: 10393
		public const int TBM_SETPAGESIZE = 1045;

		// Token: 0x0400289A RID: 10394
		public const int TBM_SETLINESIZE = 1047;

		// Token: 0x0400289B RID: 10395
		public const int TB_LINEUP = 0;

		// Token: 0x0400289C RID: 10396
		public const int TB_LINEDOWN = 1;

		// Token: 0x0400289D RID: 10397
		public const int TB_PAGEUP = 2;

		// Token: 0x0400289E RID: 10398
		public const int TB_PAGEDOWN = 3;

		// Token: 0x0400289F RID: 10399
		public const int TB_THUMBPOSITION = 4;

		// Token: 0x040028A0 RID: 10400
		public const int TB_THUMBTRACK = 5;

		// Token: 0x040028A1 RID: 10401
		public const int TB_TOP = 6;

		// Token: 0x040028A2 RID: 10402
		public const int TB_BOTTOM = 7;

		// Token: 0x040028A3 RID: 10403
		public const int TB_ENDTRACK = 8;

		// Token: 0x040028A4 RID: 10404
		public const int TVS_HASBUTTONS = 1;

		// Token: 0x040028A5 RID: 10405
		public const int TVS_HASLINES = 2;

		// Token: 0x040028A6 RID: 10406
		public const int TVS_LINESATROOT = 4;

		// Token: 0x040028A7 RID: 10407
		public const int TVS_EDITLABELS = 8;

		// Token: 0x040028A8 RID: 10408
		public const int TVS_SHOWSELALWAYS = 32;

		// Token: 0x040028A9 RID: 10409
		public const int TVS_RTLREADING = 64;

		// Token: 0x040028AA RID: 10410
		public const int TVS_CHECKBOXES = 256;

		// Token: 0x040028AB RID: 10411
		public const int TVS_TRACKSELECT = 512;

		// Token: 0x040028AC RID: 10412
		public const int TVS_FULLROWSELECT = 4096;

		// Token: 0x040028AD RID: 10413
		public const int TVS_NONEVENHEIGHT = 16384;

		// Token: 0x040028AE RID: 10414
		public const int TVS_INFOTIP = 2048;

		// Token: 0x040028AF RID: 10415
		public const int TVS_NOTOOLTIPS = 128;

		// Token: 0x040028B0 RID: 10416
		public const int TVIF_TEXT = 1;

		// Token: 0x040028B1 RID: 10417
		public const int TVIF_IMAGE = 2;

		// Token: 0x040028B2 RID: 10418
		public const int TVIF_PARAM = 4;

		// Token: 0x040028B3 RID: 10419
		public const int TVIF_STATE = 8;

		// Token: 0x040028B4 RID: 10420
		public const int TVIF_HANDLE = 16;

		// Token: 0x040028B5 RID: 10421
		public const int TVIF_SELECTEDIMAGE = 32;

		// Token: 0x040028B6 RID: 10422
		public const int TVIS_SELECTED = 2;

		// Token: 0x040028B7 RID: 10423
		public const int TVIS_EXPANDED = 32;

		// Token: 0x040028B8 RID: 10424
		public const int TVIS_EXPANDEDONCE = 64;

		// Token: 0x040028B9 RID: 10425
		public const int TVIS_STATEIMAGEMASK = 61440;

		// Token: 0x040028BA RID: 10426
		public const int TVI_ROOT = -65536;

		// Token: 0x040028BB RID: 10427
		public const int TVI_FIRST = -65535;

		// Token: 0x040028BC RID: 10428
		public const int TVM_INSERTITEMA = 4352;

		// Token: 0x040028BD RID: 10429
		public const int TVM_INSERTITEMW = 4402;

		// Token: 0x040028BE RID: 10430
		public const int TVM_DELETEITEM = 4353;

		// Token: 0x040028BF RID: 10431
		public const int TVM_EXPAND = 4354;

		// Token: 0x040028C0 RID: 10432
		public const int TVE_COLLAPSE = 1;

		// Token: 0x040028C1 RID: 10433
		public const int TVE_EXPAND = 2;

		// Token: 0x040028C2 RID: 10434
		public const int TVM_GETITEMRECT = 4356;

		// Token: 0x040028C3 RID: 10435
		public const int TVM_GETINDENT = 4358;

		// Token: 0x040028C4 RID: 10436
		public const int TVM_SETINDENT = 4359;

		// Token: 0x040028C5 RID: 10437
		public const int TVM_SETIMAGELIST = 4361;

		// Token: 0x040028C6 RID: 10438
		public const int TVM_GETNEXTITEM = 4362;

		// Token: 0x040028C7 RID: 10439
		public const int TVGN_NEXT = 1;

		// Token: 0x040028C8 RID: 10440
		public const int TVGN_PREVIOUS = 2;

		// Token: 0x040028C9 RID: 10441
		public const int TVGN_FIRSTVISIBLE = 5;

		// Token: 0x040028CA RID: 10442
		public const int TVGN_NEXTVISIBLE = 6;

		// Token: 0x040028CB RID: 10443
		public const int TVGN_PREVIOUSVISIBLE = 7;

		// Token: 0x040028CC RID: 10444
		public const int TVGN_DROPHILITE = 8;

		// Token: 0x040028CD RID: 10445
		public const int TVGN_CARET = 9;

		// Token: 0x040028CE RID: 10446
		public const int TVM_SELECTITEM = 4363;

		// Token: 0x040028CF RID: 10447
		public const int TVM_GETITEMA = 4364;

		// Token: 0x040028D0 RID: 10448
		public const int TVM_GETITEMW = 4414;

		// Token: 0x040028D1 RID: 10449
		public const int TVM_SETITEMA = 4365;

		// Token: 0x040028D2 RID: 10450
		public const int TVM_SETITEMW = 4415;

		// Token: 0x040028D3 RID: 10451
		public const int TVM_EDITLABELA = 4366;

		// Token: 0x040028D4 RID: 10452
		public const int TVM_EDITLABELW = 4417;

		// Token: 0x040028D5 RID: 10453
		public const int TVM_GETEDITCONTROL = 4367;

		// Token: 0x040028D6 RID: 10454
		public const int TVM_GETVISIBLECOUNT = 4368;

		// Token: 0x040028D7 RID: 10455
		public const int TVM_HITTEST = 4369;

		// Token: 0x040028D8 RID: 10456
		public const int TVM_ENSUREVISIBLE = 4372;

		// Token: 0x040028D9 RID: 10457
		public const int TVM_ENDEDITLABELNOW = 4374;

		// Token: 0x040028DA RID: 10458
		public const int TVM_GETISEARCHSTRINGA = 4375;

		// Token: 0x040028DB RID: 10459
		public const int TVM_GETISEARCHSTRINGW = 4416;

		// Token: 0x040028DC RID: 10460
		public const int TVM_SETITEMHEIGHT = 4379;

		// Token: 0x040028DD RID: 10461
		public const int TVM_GETITEMHEIGHT = 4380;

		// Token: 0x040028DE RID: 10462
		public const int TVN_SELCHANGINGA = -401;

		// Token: 0x040028DF RID: 10463
		public const int TVN_SELCHANGINGW = -450;

		// Token: 0x040028E0 RID: 10464
		public const int TVN_GETINFOTIPA = -413;

		// Token: 0x040028E1 RID: 10465
		public const int TVN_GETINFOTIPW = -414;

		// Token: 0x040028E2 RID: 10466
		public const int TVN_SELCHANGEDA = -402;

		// Token: 0x040028E3 RID: 10467
		public const int TVN_SELCHANGEDW = -451;

		// Token: 0x040028E4 RID: 10468
		public const int TVC_UNKNOWN = 0;

		// Token: 0x040028E5 RID: 10469
		public const int TVC_BYMOUSE = 1;

		// Token: 0x040028E6 RID: 10470
		public const int TVC_BYKEYBOARD = 2;

		// Token: 0x040028E7 RID: 10471
		public const int TVN_GETDISPINFOA = -403;

		// Token: 0x040028E8 RID: 10472
		public const int TVN_GETDISPINFOW = -452;

		// Token: 0x040028E9 RID: 10473
		public const int TVN_SETDISPINFOA = -404;

		// Token: 0x040028EA RID: 10474
		public const int TVN_SETDISPINFOW = -453;

		// Token: 0x040028EB RID: 10475
		public const int TVN_ITEMEXPANDINGA = -405;

		// Token: 0x040028EC RID: 10476
		public const int TVN_ITEMEXPANDINGW = -454;

		// Token: 0x040028ED RID: 10477
		public const int TVN_ITEMEXPANDEDA = -406;

		// Token: 0x040028EE RID: 10478
		public const int TVN_ITEMEXPANDEDW = -455;

		// Token: 0x040028EF RID: 10479
		public const int TVN_BEGINDRAGA = -407;

		// Token: 0x040028F0 RID: 10480
		public const int TVN_BEGINDRAGW = -456;

		// Token: 0x040028F1 RID: 10481
		public const int TVN_BEGINRDRAGA = -408;

		// Token: 0x040028F2 RID: 10482
		public const int TVN_BEGINRDRAGW = -457;

		// Token: 0x040028F3 RID: 10483
		public const int TVN_BEGINLABELEDITA = -410;

		// Token: 0x040028F4 RID: 10484
		public const int TVN_BEGINLABELEDITW = -459;

		// Token: 0x040028F5 RID: 10485
		public const int TVN_ENDLABELEDITA = -411;

		// Token: 0x040028F6 RID: 10486
		public const int TVN_ENDLABELEDITW = -460;

		// Token: 0x040028F7 RID: 10487
		public const int TCS_BOTTOM = 2;

		// Token: 0x040028F8 RID: 10488
		public const int TCS_RIGHT = 2;

		// Token: 0x040028F9 RID: 10489
		public const int TCS_FLATBUTTONS = 8;

		// Token: 0x040028FA RID: 10490
		public const int TCS_HOTTRACK = 64;

		// Token: 0x040028FB RID: 10491
		public const int TCS_VERTICAL = 128;

		// Token: 0x040028FC RID: 10492
		public const int TCS_TABS = 0;

		// Token: 0x040028FD RID: 10493
		public const int TCS_BUTTONS = 256;

		// Token: 0x040028FE RID: 10494
		public const int TCS_MULTILINE = 512;

		// Token: 0x040028FF RID: 10495
		public const int TCS_RIGHTJUSTIFY = 0;

		// Token: 0x04002900 RID: 10496
		public const int TCS_FIXEDWIDTH = 1024;

		// Token: 0x04002901 RID: 10497
		public const int TCS_RAGGEDRIGHT = 2048;

		// Token: 0x04002902 RID: 10498
		public const int TCS_OWNERDRAWFIXED = 8192;

		// Token: 0x04002903 RID: 10499
		public const int TCS_TOOLTIPS = 16384;

		// Token: 0x04002904 RID: 10500
		public const int TCM_SETIMAGELIST = 4867;

		// Token: 0x04002905 RID: 10501
		public const int TCIF_TEXT = 1;

		// Token: 0x04002906 RID: 10502
		public const int TCIF_IMAGE = 2;

		// Token: 0x04002907 RID: 10503
		public const int TCM_GETITEMA = 4869;

		// Token: 0x04002908 RID: 10504
		public const int TCM_GETITEMW = 4924;

		// Token: 0x04002909 RID: 10505
		public const int TCM_SETITEMA = 4870;

		// Token: 0x0400290A RID: 10506
		public const int TCM_SETITEMW = 4925;

		// Token: 0x0400290B RID: 10507
		public const int TCM_INSERTITEMA = 4871;

		// Token: 0x0400290C RID: 10508
		public const int TCM_INSERTITEMW = 4926;

		// Token: 0x0400290D RID: 10509
		public const int TCM_DELETEITEM = 4872;

		// Token: 0x0400290E RID: 10510
		public const int TCM_DELETEALLITEMS = 4873;

		// Token: 0x0400290F RID: 10511
		public const int TCM_GETITEMRECT = 4874;

		// Token: 0x04002910 RID: 10512
		public const int TCM_GETCURSEL = 4875;

		// Token: 0x04002911 RID: 10513
		public const int TCM_SETCURSEL = 4876;

		// Token: 0x04002912 RID: 10514
		public const int TCM_ADJUSTRECT = 4904;

		// Token: 0x04002913 RID: 10515
		public const int TCM_SETITEMSIZE = 4905;

		// Token: 0x04002914 RID: 10516
		public const int TCM_SETPADDING = 4907;

		// Token: 0x04002915 RID: 10517
		public const int TCM_GETROWCOUNT = 4908;

		// Token: 0x04002916 RID: 10518
		public const int TCM_GETTOOLTIPS = 4909;

		// Token: 0x04002917 RID: 10519
		public const int TCM_SETTOOLTIPS = 4910;

		// Token: 0x04002918 RID: 10520
		public const int TCN_SELCHANGE = -551;

		// Token: 0x04002919 RID: 10521
		public const int TCN_SELCHANGING = -552;

		// Token: 0x0400291A RID: 10522
		public const int TBSTYLE_WRAPPABLE = 512;

		// Token: 0x0400291B RID: 10523
		public const int TVM_SETBKCOLOR = 4381;

		// Token: 0x0400291C RID: 10524
		public const int TVM_SETTEXTCOLOR = 4382;

		// Token: 0x0400291D RID: 10525
		public const int TYMED_NULL = 0;

		// Token: 0x0400291E RID: 10526
		public const int TVM_GETLINECOLOR = 4393;

		// Token: 0x0400291F RID: 10527
		public const int TVM_SETLINECOLOR = 4392;

		// Token: 0x04002920 RID: 10528
		public const int TVM_SETTOOLTIPS = 4376;

		// Token: 0x04002921 RID: 10529
		public const int TVSIL_STATE = 2;

		// Token: 0x04002922 RID: 10530
		public const int TVM_SORTCHILDRENCB = 4373;

		// Token: 0x04002923 RID: 10531
		public const int TMPF_FIXED_PITCH = 1;

		// Token: 0x04002924 RID: 10532
		public const int TVHT_NOWHERE = 1;

		// Token: 0x04002925 RID: 10533
		public const int TVHT_ONITEMICON = 2;

		// Token: 0x04002926 RID: 10534
		public const int TVHT_ONITEMLABEL = 4;

		// Token: 0x04002927 RID: 10535
		public const int TVHT_ONITEM = 70;

		// Token: 0x04002928 RID: 10536
		public const int TVHT_ONITEMINDENT = 8;

		// Token: 0x04002929 RID: 10537
		public const int TVHT_ONITEMBUTTON = 16;

		// Token: 0x0400292A RID: 10538
		public const int TVHT_ONITEMRIGHT = 32;

		// Token: 0x0400292B RID: 10539
		public const int TVHT_ONITEMSTATEICON = 64;

		// Token: 0x0400292C RID: 10540
		public const int TVHT_ABOVE = 256;

		// Token: 0x0400292D RID: 10541
		public const int TVHT_BELOW = 512;

		// Token: 0x0400292E RID: 10542
		public const int TVHT_TORIGHT = 1024;

		// Token: 0x0400292F RID: 10543
		public const int TVHT_TOLEFT = 2048;

		// Token: 0x04002930 RID: 10544
		public const int UIS_SET = 1;

		// Token: 0x04002931 RID: 10545
		public const int UIS_CLEAR = 2;

		// Token: 0x04002932 RID: 10546
		public const int UIS_INITIALIZE = 3;

		// Token: 0x04002933 RID: 10547
		public const int UISF_HIDEFOCUS = 1;

		// Token: 0x04002934 RID: 10548
		public const int UISF_HIDEACCEL = 2;

		// Token: 0x04002935 RID: 10549
		public const int USERCLASSTYPE_FULL = 1;

		// Token: 0x04002936 RID: 10550
		public const int USERCLASSTYPE_SHORT = 2;

		// Token: 0x04002937 RID: 10551
		public const int USERCLASSTYPE_APPNAME = 3;

		// Token: 0x04002938 RID: 10552
		public const int UOI_FLAGS = 1;

		// Token: 0x04002939 RID: 10553
		public const int VIEW_E_DRAW = -2147221184;

		// Token: 0x0400293A RID: 10554
		public const int VK_PRIOR = 33;

		// Token: 0x0400293B RID: 10555
		public const int VK_NEXT = 34;

		// Token: 0x0400293C RID: 10556
		public const int VK_LEFT = 37;

		// Token: 0x0400293D RID: 10557
		public const int VK_UP = 38;

		// Token: 0x0400293E RID: 10558
		public const int VK_RIGHT = 39;

		// Token: 0x0400293F RID: 10559
		public const int VK_DOWN = 40;

		// Token: 0x04002940 RID: 10560
		public const int VK_TAB = 9;

		// Token: 0x04002941 RID: 10561
		public const int VK_SHIFT = 16;

		// Token: 0x04002942 RID: 10562
		public const int VK_CONTROL = 17;

		// Token: 0x04002943 RID: 10563
		public const int VK_MENU = 18;

		// Token: 0x04002944 RID: 10564
		public const int VK_CAPITAL = 20;

		// Token: 0x04002945 RID: 10565
		public const int VK_KANA = 21;

		// Token: 0x04002946 RID: 10566
		public const int VK_ESCAPE = 27;

		// Token: 0x04002947 RID: 10567
		public const int VK_END = 35;

		// Token: 0x04002948 RID: 10568
		public const int VK_HOME = 36;

		// Token: 0x04002949 RID: 10569
		public const int VK_NUMLOCK = 144;

		// Token: 0x0400294A RID: 10570
		public const int VK_SCROLL = 145;

		// Token: 0x0400294B RID: 10571
		public const int VK_INSERT = 45;

		// Token: 0x0400294C RID: 10572
		public const int VK_DELETE = 46;

		// Token: 0x0400294D RID: 10573
		public const int WH_JOURNALPLAYBACK = 1;

		// Token: 0x0400294E RID: 10574
		public const int WH_GETMESSAGE = 3;

		// Token: 0x0400294F RID: 10575
		public const int WH_MOUSE = 7;

		// Token: 0x04002950 RID: 10576
		public const int WSF_VISIBLE = 1;

		// Token: 0x04002951 RID: 10577
		public const int WM_NULL = 0;

		// Token: 0x04002952 RID: 10578
		public const int WM_CREATE = 1;

		// Token: 0x04002953 RID: 10579
		public const int WM_DELETEITEM = 45;

		// Token: 0x04002954 RID: 10580
		public const int WM_DESTROY = 2;

		// Token: 0x04002955 RID: 10581
		public const int WM_MOVE = 3;

		// Token: 0x04002956 RID: 10582
		public const int WM_SIZE = 5;

		// Token: 0x04002957 RID: 10583
		public const int WM_ACTIVATE = 6;

		// Token: 0x04002958 RID: 10584
		public const int WA_INACTIVE = 0;

		// Token: 0x04002959 RID: 10585
		public const int WA_ACTIVE = 1;

		// Token: 0x0400295A RID: 10586
		public const int WA_CLICKACTIVE = 2;

		// Token: 0x0400295B RID: 10587
		public const int WM_SETFOCUS = 7;

		// Token: 0x0400295C RID: 10588
		public const int WM_KILLFOCUS = 8;

		// Token: 0x0400295D RID: 10589
		public const int WM_ENABLE = 10;

		// Token: 0x0400295E RID: 10590
		public const int WM_SETREDRAW = 11;

		// Token: 0x0400295F RID: 10591
		public const int WM_SETTEXT = 12;

		// Token: 0x04002960 RID: 10592
		public const int WM_GETTEXT = 13;

		// Token: 0x04002961 RID: 10593
		public const int WM_GETTEXTLENGTH = 14;

		// Token: 0x04002962 RID: 10594
		public const int WM_PAINT = 15;

		// Token: 0x04002963 RID: 10595
		public const int WM_CLOSE = 16;

		// Token: 0x04002964 RID: 10596
		public const int WM_QUERYENDSESSION = 17;

		// Token: 0x04002965 RID: 10597
		public const int WM_QUIT = 18;

		// Token: 0x04002966 RID: 10598
		public const int WM_QUERYOPEN = 19;

		// Token: 0x04002967 RID: 10599
		public const int WM_ERASEBKGND = 20;

		// Token: 0x04002968 RID: 10600
		public const int WM_SYSCOLORCHANGE = 21;

		// Token: 0x04002969 RID: 10601
		public const int WM_ENDSESSION = 22;

		// Token: 0x0400296A RID: 10602
		public const int WM_SHOWWINDOW = 24;

		// Token: 0x0400296B RID: 10603
		public const int WM_WININICHANGE = 26;

		// Token: 0x0400296C RID: 10604
		public const int WM_SETTINGCHANGE = 26;

		// Token: 0x0400296D RID: 10605
		public const int WM_DEVMODECHANGE = 27;

		// Token: 0x0400296E RID: 10606
		public const int WM_ACTIVATEAPP = 28;

		// Token: 0x0400296F RID: 10607
		public const int WM_FONTCHANGE = 29;

		// Token: 0x04002970 RID: 10608
		public const int WM_TIMECHANGE = 30;

		// Token: 0x04002971 RID: 10609
		public const int WM_CANCELMODE = 31;

		// Token: 0x04002972 RID: 10610
		public const int WM_SETCURSOR = 32;

		// Token: 0x04002973 RID: 10611
		public const int WM_MOUSEACTIVATE = 33;

		// Token: 0x04002974 RID: 10612
		public const int WM_CHILDACTIVATE = 34;

		// Token: 0x04002975 RID: 10613
		public const int WM_QUEUESYNC = 35;

		// Token: 0x04002976 RID: 10614
		public const int WM_GETMINMAXINFO = 36;

		// Token: 0x04002977 RID: 10615
		public const int WM_PAINTICON = 38;

		// Token: 0x04002978 RID: 10616
		public const int WM_ICONERASEBKGND = 39;

		// Token: 0x04002979 RID: 10617
		public const int WM_NEXTDLGCTL = 40;

		// Token: 0x0400297A RID: 10618
		public const int WM_SPOOLERSTATUS = 42;

		// Token: 0x0400297B RID: 10619
		public const int WM_DRAWITEM = 43;

		// Token: 0x0400297C RID: 10620
		public const int WM_MEASUREITEM = 44;

		// Token: 0x0400297D RID: 10621
		public const int WM_VKEYTOITEM = 46;

		// Token: 0x0400297E RID: 10622
		public const int WM_CHARTOITEM = 47;

		// Token: 0x0400297F RID: 10623
		public const int WM_SETFONT = 48;

		// Token: 0x04002980 RID: 10624
		public const int WM_GETFONT = 49;

		// Token: 0x04002981 RID: 10625
		public const int WM_SETHOTKEY = 50;

		// Token: 0x04002982 RID: 10626
		public const int WM_GETHOTKEY = 51;

		// Token: 0x04002983 RID: 10627
		public const int WM_QUERYDRAGICON = 55;

		// Token: 0x04002984 RID: 10628
		public const int WM_COMPAREITEM = 57;

		// Token: 0x04002985 RID: 10629
		public const int WM_GETOBJECT = 61;

		// Token: 0x04002986 RID: 10630
		public const int WM_COMPACTING = 65;

		// Token: 0x04002987 RID: 10631
		public const int WM_COMMNOTIFY = 68;

		// Token: 0x04002988 RID: 10632
		public const int WM_WINDOWPOSCHANGING = 70;

		// Token: 0x04002989 RID: 10633
		public const int WM_WINDOWPOSCHANGED = 71;

		// Token: 0x0400298A RID: 10634
		public const int WM_POWER = 72;

		// Token: 0x0400298B RID: 10635
		public const int WM_COPYDATA = 74;

		// Token: 0x0400298C RID: 10636
		public const int WM_CANCELJOURNAL = 75;

		// Token: 0x0400298D RID: 10637
		public const int WM_NOTIFY = 78;

		// Token: 0x0400298E RID: 10638
		public const int WM_INPUTLANGCHANGEREQUEST = 80;

		// Token: 0x0400298F RID: 10639
		public const int WM_INPUTLANGCHANGE = 81;

		// Token: 0x04002990 RID: 10640
		public const int WM_TCARD = 82;

		// Token: 0x04002991 RID: 10641
		public const int WM_HELP = 83;

		// Token: 0x04002992 RID: 10642
		public const int WM_USERCHANGED = 84;

		// Token: 0x04002993 RID: 10643
		public const int WM_NOTIFYFORMAT = 85;

		// Token: 0x04002994 RID: 10644
		public const int WM_CONTEXTMENU = 123;

		// Token: 0x04002995 RID: 10645
		public const int WM_STYLECHANGING = 124;

		// Token: 0x04002996 RID: 10646
		public const int WM_STYLECHANGED = 125;

		// Token: 0x04002997 RID: 10647
		public const int WM_DISPLAYCHANGE = 126;

		// Token: 0x04002998 RID: 10648
		public const int WM_GETICON = 127;

		// Token: 0x04002999 RID: 10649
		public const int WM_SETICON = 128;

		// Token: 0x0400299A RID: 10650
		public const int WM_NCCREATE = 129;

		// Token: 0x0400299B RID: 10651
		public const int WM_NCDESTROY = 130;

		// Token: 0x0400299C RID: 10652
		public const int WM_NCCALCSIZE = 131;

		// Token: 0x0400299D RID: 10653
		public const int WM_NCHITTEST = 132;

		// Token: 0x0400299E RID: 10654
		public const int WM_NCPAINT = 133;

		// Token: 0x0400299F RID: 10655
		public const int WM_NCACTIVATE = 134;

		// Token: 0x040029A0 RID: 10656
		public const int WM_GETDLGCODE = 135;

		// Token: 0x040029A1 RID: 10657
		public const int WM_NCMOUSEMOVE = 160;

		// Token: 0x040029A2 RID: 10658
		public const int WM_NCMOUSELEAVE = 674;

		// Token: 0x040029A3 RID: 10659
		public const int WM_NCLBUTTONDOWN = 161;

		// Token: 0x040029A4 RID: 10660
		public const int WM_NCLBUTTONUP = 162;

		// Token: 0x040029A5 RID: 10661
		public const int WM_NCLBUTTONDBLCLK = 163;

		// Token: 0x040029A6 RID: 10662
		public const int WM_NCRBUTTONDOWN = 164;

		// Token: 0x040029A7 RID: 10663
		public const int WM_NCRBUTTONUP = 165;

		// Token: 0x040029A8 RID: 10664
		public const int WM_NCRBUTTONDBLCLK = 166;

		// Token: 0x040029A9 RID: 10665
		public const int WM_NCMBUTTONDOWN = 167;

		// Token: 0x040029AA RID: 10666
		public const int WM_NCMBUTTONUP = 168;

		// Token: 0x040029AB RID: 10667
		public const int WM_NCMBUTTONDBLCLK = 169;

		// Token: 0x040029AC RID: 10668
		public const int WM_NCXBUTTONDOWN = 171;

		// Token: 0x040029AD RID: 10669
		public const int WM_NCXBUTTONUP = 172;

		// Token: 0x040029AE RID: 10670
		public const int WM_NCXBUTTONDBLCLK = 173;

		// Token: 0x040029AF RID: 10671
		public const int WM_KEYFIRST = 256;

		// Token: 0x040029B0 RID: 10672
		public const int WM_KEYDOWN = 256;

		// Token: 0x040029B1 RID: 10673
		public const int WM_KEYUP = 257;

		// Token: 0x040029B2 RID: 10674
		public const int WM_CHAR = 258;

		// Token: 0x040029B3 RID: 10675
		public const int WM_DEADCHAR = 259;

		// Token: 0x040029B4 RID: 10676
		public const int WM_CTLCOLOR = 25;

		// Token: 0x040029B5 RID: 10677
		public const int WM_SYSKEYDOWN = 260;

		// Token: 0x040029B6 RID: 10678
		public const int WM_SYSKEYUP = 261;

		// Token: 0x040029B7 RID: 10679
		public const int WM_SYSCHAR = 262;

		// Token: 0x040029B8 RID: 10680
		public const int WM_SYSDEADCHAR = 263;

		// Token: 0x040029B9 RID: 10681
		public const int WM_KEYLAST = 264;

		// Token: 0x040029BA RID: 10682
		public const int WM_IME_STARTCOMPOSITION = 269;

		// Token: 0x040029BB RID: 10683
		public const int WM_IME_ENDCOMPOSITION = 270;

		// Token: 0x040029BC RID: 10684
		public const int WM_IME_COMPOSITION = 271;

		// Token: 0x040029BD RID: 10685
		public const int WM_IME_KEYLAST = 271;

		// Token: 0x040029BE RID: 10686
		public const int WM_INITDIALOG = 272;

		// Token: 0x040029BF RID: 10687
		public const int WM_COMMAND = 273;

		// Token: 0x040029C0 RID: 10688
		public const int WM_SYSCOMMAND = 274;

		// Token: 0x040029C1 RID: 10689
		public const int WM_TIMER = 275;

		// Token: 0x040029C2 RID: 10690
		public const int WM_HSCROLL = 276;

		// Token: 0x040029C3 RID: 10691
		public const int WM_VSCROLL = 277;

		// Token: 0x040029C4 RID: 10692
		public const int WM_INITMENU = 278;

		// Token: 0x040029C5 RID: 10693
		public const int WM_INITMENUPOPUP = 279;

		// Token: 0x040029C6 RID: 10694
		public const int WM_MENUSELECT = 287;

		// Token: 0x040029C7 RID: 10695
		public const int WM_MENUCHAR = 288;

		// Token: 0x040029C8 RID: 10696
		public const int WM_ENTERIDLE = 289;

		// Token: 0x040029C9 RID: 10697
		public const int WM_UNINITMENUPOPUP = 293;

		// Token: 0x040029CA RID: 10698
		public const int WM_CHANGEUISTATE = 295;

		// Token: 0x040029CB RID: 10699
		public const int WM_UPDATEUISTATE = 296;

		// Token: 0x040029CC RID: 10700
		public const int WM_QUERYUISTATE = 297;

		// Token: 0x040029CD RID: 10701
		public const int WM_CTLCOLORMSGBOX = 306;

		// Token: 0x040029CE RID: 10702
		public const int WM_CTLCOLOREDIT = 307;

		// Token: 0x040029CF RID: 10703
		public const int WM_CTLCOLORLISTBOX = 308;

		// Token: 0x040029D0 RID: 10704
		public const int WM_CTLCOLORBTN = 309;

		// Token: 0x040029D1 RID: 10705
		public const int WM_CTLCOLORDLG = 310;

		// Token: 0x040029D2 RID: 10706
		public const int WM_CTLCOLORSCROLLBAR = 311;

		// Token: 0x040029D3 RID: 10707
		public const int WM_CTLCOLORSTATIC = 312;

		// Token: 0x040029D4 RID: 10708
		public const int WM_MOUSEFIRST = 512;

		// Token: 0x040029D5 RID: 10709
		public const int WM_MOUSEMOVE = 512;

		// Token: 0x040029D6 RID: 10710
		public const int WM_LBUTTONDOWN = 513;

		// Token: 0x040029D7 RID: 10711
		public const int WM_LBUTTONUP = 514;

		// Token: 0x040029D8 RID: 10712
		public const int WM_LBUTTONDBLCLK = 515;

		// Token: 0x040029D9 RID: 10713
		public const int WM_RBUTTONDOWN = 516;

		// Token: 0x040029DA RID: 10714
		public const int WM_RBUTTONUP = 517;

		// Token: 0x040029DB RID: 10715
		public const int WM_RBUTTONDBLCLK = 518;

		// Token: 0x040029DC RID: 10716
		public const int WM_MBUTTONDOWN = 519;

		// Token: 0x040029DD RID: 10717
		public const int WM_MBUTTONUP = 520;

		// Token: 0x040029DE RID: 10718
		public const int WM_MBUTTONDBLCLK = 521;

		// Token: 0x040029DF RID: 10719
		public const int WM_XBUTTONDOWN = 523;

		// Token: 0x040029E0 RID: 10720
		public const int WM_XBUTTONUP = 524;

		// Token: 0x040029E1 RID: 10721
		public const int WM_XBUTTONDBLCLK = 525;

		// Token: 0x040029E2 RID: 10722
		public const int WM_MOUSEWHEEL = 522;

		// Token: 0x040029E3 RID: 10723
		public const int WM_MOUSELAST = 522;

		// Token: 0x040029E4 RID: 10724
		public const int WHEEL_DELTA = 120;

		// Token: 0x040029E5 RID: 10725
		public const int WM_PARENTNOTIFY = 528;

		// Token: 0x040029E6 RID: 10726
		public const int WM_ENTERMENULOOP = 529;

		// Token: 0x040029E7 RID: 10727
		public const int WM_EXITMENULOOP = 530;

		// Token: 0x040029E8 RID: 10728
		public const int WM_NEXTMENU = 531;

		// Token: 0x040029E9 RID: 10729
		public const int WM_SIZING = 532;

		// Token: 0x040029EA RID: 10730
		public const int WM_CAPTURECHANGED = 533;

		// Token: 0x040029EB RID: 10731
		public const int WM_MOVING = 534;

		// Token: 0x040029EC RID: 10732
		public const int WM_POWERBROADCAST = 536;

		// Token: 0x040029ED RID: 10733
		public const int WM_DEVICECHANGE = 537;

		// Token: 0x040029EE RID: 10734
		public const int WM_IME_SETCONTEXT = 641;

		// Token: 0x040029EF RID: 10735
		public const int WM_IME_NOTIFY = 642;

		// Token: 0x040029F0 RID: 10736
		public const int WM_IME_CONTROL = 643;

		// Token: 0x040029F1 RID: 10737
		public const int WM_IME_COMPOSITIONFULL = 644;

		// Token: 0x040029F2 RID: 10738
		public const int WM_IME_SELECT = 645;

		// Token: 0x040029F3 RID: 10739
		public const int WM_IME_CHAR = 646;

		// Token: 0x040029F4 RID: 10740
		public const int WM_IME_KEYDOWN = 656;

		// Token: 0x040029F5 RID: 10741
		public const int WM_IME_KEYUP = 657;

		// Token: 0x040029F6 RID: 10742
		public const int WM_MDICREATE = 544;

		// Token: 0x040029F7 RID: 10743
		public const int WM_MDIDESTROY = 545;

		// Token: 0x040029F8 RID: 10744
		public const int WM_MDIACTIVATE = 546;

		// Token: 0x040029F9 RID: 10745
		public const int WM_MDIRESTORE = 547;

		// Token: 0x040029FA RID: 10746
		public const int WM_MDINEXT = 548;

		// Token: 0x040029FB RID: 10747
		public const int WM_MDIMAXIMIZE = 549;

		// Token: 0x040029FC RID: 10748
		public const int WM_MDITILE = 550;

		// Token: 0x040029FD RID: 10749
		public const int WM_MDICASCADE = 551;

		// Token: 0x040029FE RID: 10750
		public const int WM_MDIICONARRANGE = 552;

		// Token: 0x040029FF RID: 10751
		public const int WM_MDIGETACTIVE = 553;

		// Token: 0x04002A00 RID: 10752
		public const int WM_MDISETMENU = 560;

		// Token: 0x04002A01 RID: 10753
		public const int WM_ENTERSIZEMOVE = 561;

		// Token: 0x04002A02 RID: 10754
		public const int WM_EXITSIZEMOVE = 562;

		// Token: 0x04002A03 RID: 10755
		public const int WM_DROPFILES = 563;

		// Token: 0x04002A04 RID: 10756
		public const int WM_MDIREFRESHMENU = 564;

		// Token: 0x04002A05 RID: 10757
		public const int WM_MOUSEHOVER = 673;

		// Token: 0x04002A06 RID: 10758
		public const int WM_MOUSELEAVE = 675;

		// Token: 0x04002A07 RID: 10759
		public const int WM_CUT = 768;

		// Token: 0x04002A08 RID: 10760
		public const int WM_COPY = 769;

		// Token: 0x04002A09 RID: 10761
		public const int WM_PASTE = 770;

		// Token: 0x04002A0A RID: 10762
		public const int WM_CLEAR = 771;

		// Token: 0x04002A0B RID: 10763
		public const int WM_UNDO = 772;

		// Token: 0x04002A0C RID: 10764
		public const int WM_RENDERFORMAT = 773;

		// Token: 0x04002A0D RID: 10765
		public const int WM_RENDERALLFORMATS = 774;

		// Token: 0x04002A0E RID: 10766
		public const int WM_DESTROYCLIPBOARD = 775;

		// Token: 0x04002A0F RID: 10767
		public const int WM_DRAWCLIPBOARD = 776;

		// Token: 0x04002A10 RID: 10768
		public const int WM_PAINTCLIPBOARD = 777;

		// Token: 0x04002A11 RID: 10769
		public const int WM_VSCROLLCLIPBOARD = 778;

		// Token: 0x04002A12 RID: 10770
		public const int WM_SIZECLIPBOARD = 779;

		// Token: 0x04002A13 RID: 10771
		public const int WM_ASKCBFORMATNAME = 780;

		// Token: 0x04002A14 RID: 10772
		public const int WM_CHANGECBCHAIN = 781;

		// Token: 0x04002A15 RID: 10773
		public const int WM_HSCROLLCLIPBOARD = 782;

		// Token: 0x04002A16 RID: 10774
		public const int WM_QUERYNEWPALETTE = 783;

		// Token: 0x04002A17 RID: 10775
		public const int WM_PALETTEISCHANGING = 784;

		// Token: 0x04002A18 RID: 10776
		public const int WM_PALETTECHANGED = 785;

		// Token: 0x04002A19 RID: 10777
		public const int WM_HOTKEY = 786;

		// Token: 0x04002A1A RID: 10778
		public const int WM_PRINT = 791;

		// Token: 0x04002A1B RID: 10779
		public const int WM_PRINTCLIENT = 792;

		// Token: 0x04002A1C RID: 10780
		public const int WM_THEMECHANGED = 794;

		// Token: 0x04002A1D RID: 10781
		public const int WM_HANDHELDFIRST = 856;

		// Token: 0x04002A1E RID: 10782
		public const int WM_HANDHELDLAST = 863;

		// Token: 0x04002A1F RID: 10783
		public const int WM_AFXFIRST = 864;

		// Token: 0x04002A20 RID: 10784
		public const int WM_AFXLAST = 895;

		// Token: 0x04002A21 RID: 10785
		public const int WM_PENWINFIRST = 896;

		// Token: 0x04002A22 RID: 10786
		public const int WM_PENWINLAST = 911;

		// Token: 0x04002A23 RID: 10787
		public const int WM_APP = 32768;

		// Token: 0x04002A24 RID: 10788
		public const int WM_USER = 1024;

		// Token: 0x04002A25 RID: 10789
		public const int WM_REFLECT = 8192;

		// Token: 0x04002A26 RID: 10790
		public const int WS_OVERLAPPED = 0;

		// Token: 0x04002A27 RID: 10791
		public const int WS_POPUP = -2147483648;

		// Token: 0x04002A28 RID: 10792
		public const int WS_CHILD = 1073741824;

		// Token: 0x04002A29 RID: 10793
		public const int WS_MINIMIZE = 536870912;

		// Token: 0x04002A2A RID: 10794
		public const int WS_VISIBLE = 268435456;

		// Token: 0x04002A2B RID: 10795
		public const int WS_DISABLED = 134217728;

		// Token: 0x04002A2C RID: 10796
		public const int WS_CLIPSIBLINGS = 67108864;

		// Token: 0x04002A2D RID: 10797
		public const int WS_CLIPCHILDREN = 33554432;

		// Token: 0x04002A2E RID: 10798
		public const int WS_MAXIMIZE = 16777216;

		// Token: 0x04002A2F RID: 10799
		public const int WS_CAPTION = 12582912;

		// Token: 0x04002A30 RID: 10800
		public const int WS_BORDER = 8388608;

		// Token: 0x04002A31 RID: 10801
		public const int WS_DLGFRAME = 4194304;

		// Token: 0x04002A32 RID: 10802
		public const int WS_VSCROLL = 2097152;

		// Token: 0x04002A33 RID: 10803
		public const int WS_HSCROLL = 1048576;

		// Token: 0x04002A34 RID: 10804
		public const int WS_SYSMENU = 524288;

		// Token: 0x04002A35 RID: 10805
		public const int WS_THICKFRAME = 262144;

		// Token: 0x04002A36 RID: 10806
		public const int WS_TABSTOP = 65536;

		// Token: 0x04002A37 RID: 10807
		public const int WS_MINIMIZEBOX = 131072;

		// Token: 0x04002A38 RID: 10808
		public const int WS_MAXIMIZEBOX = 65536;

		// Token: 0x04002A39 RID: 10809
		public const int WS_EX_DLGMODALFRAME = 1;

		// Token: 0x04002A3A RID: 10810
		public const int WS_EX_MDICHILD = 64;

		// Token: 0x04002A3B RID: 10811
		public const int WS_EX_TOOLWINDOW = 128;

		// Token: 0x04002A3C RID: 10812
		public const int WS_EX_CLIENTEDGE = 512;

		// Token: 0x04002A3D RID: 10813
		public const int WS_EX_CONTEXTHELP = 1024;

		// Token: 0x04002A3E RID: 10814
		public const int WS_EX_RIGHT = 4096;

		// Token: 0x04002A3F RID: 10815
		public const int WS_EX_LEFT = 0;

		// Token: 0x04002A40 RID: 10816
		public const int WS_EX_RTLREADING = 8192;

		// Token: 0x04002A41 RID: 10817
		public const int WS_EX_LEFTSCROLLBAR = 16384;

		// Token: 0x04002A42 RID: 10818
		public const int WS_EX_CONTROLPARENT = 65536;

		// Token: 0x04002A43 RID: 10819
		public const int WS_EX_STATICEDGE = 131072;

		// Token: 0x04002A44 RID: 10820
		public const int WS_EX_APPWINDOW = 262144;

		// Token: 0x04002A45 RID: 10821
		public const int WS_EX_LAYERED = 524288;

		// Token: 0x04002A46 RID: 10822
		public const int WS_EX_TOPMOST = 8;

		// Token: 0x04002A47 RID: 10823
		public const int WS_EX_LAYOUTRTL = 4194304;

		// Token: 0x04002A48 RID: 10824
		public const int WS_EX_NOINHERITLAYOUT = 1048576;

		// Token: 0x04002A49 RID: 10825
		public const int WPF_SETMINPOSITION = 1;

		// Token: 0x04002A4A RID: 10826
		public const int WM_CHOOSEFONT_GETLOGFONT = 1025;

		// Token: 0x04002A4B RID: 10827
		public const int IMN_OPENSTATUSWINDOW = 2;

		// Token: 0x04002A4C RID: 10828
		public const int IMN_SETCONVERSIONMODE = 6;

		// Token: 0x04002A4D RID: 10829
		public const int IMN_SETOPENSTATUS = 8;

		// Token: 0x04002A4E RID: 10830
		public const int PD_RESULT_CANCEL = 0;

		// Token: 0x04002A4F RID: 10831
		public const int PD_RESULT_PRINT = 1;

		// Token: 0x04002A50 RID: 10832
		public const int PD_RESULT_APPLY = 2;

		// Token: 0x04002A51 RID: 10833
		public const int XBUTTON1 = 1;

		// Token: 0x04002A52 RID: 10834
		public const int XBUTTON2 = 2;

		// Token: 0x04002A53 RID: 10835
		public const string TOOLTIPS_CLASS = "tooltips_class32";

		// Token: 0x04002A54 RID: 10836
		public const string WC_DATETIMEPICK = "SysDateTimePick32";

		// Token: 0x04002A55 RID: 10837
		public const string WC_LISTVIEW = "SysListView32";

		// Token: 0x04002A56 RID: 10838
		public const string WC_MONTHCAL = "SysMonthCal32";

		// Token: 0x04002A57 RID: 10839
		public const string WC_PROGRESS = "msctls_progress32";

		// Token: 0x04002A58 RID: 10840
		public const string WC_STATUSBAR = "msctls_statusbar32";

		// Token: 0x04002A59 RID: 10841
		public const string WC_TOOLBAR = "ToolbarWindow32";

		// Token: 0x04002A5A RID: 10842
		public const string WC_TRACKBAR = "msctls_trackbar32";

		// Token: 0x04002A5B RID: 10843
		public const string WC_TREEVIEW = "SysTreeView32";

		// Token: 0x04002A5C RID: 10844
		public const string WC_TABCONTROL = "SysTabControl32";

		// Token: 0x04002A5D RID: 10845
		public const string MSH_MOUSEWHEEL = "MSWHEEL_ROLLMSG";

		// Token: 0x04002A5E RID: 10846
		public const string MSH_SCROLL_LINES = "MSH_SCROLL_LINES_MSG";

		// Token: 0x04002A5F RID: 10847
		public const string MOUSEZ_CLASSNAME = "MouseZ";

		// Token: 0x04002A60 RID: 10848
		public const string MOUSEZ_TITLE = "Magellan MSWHEEL";

		// Token: 0x04002A61 RID: 10849
		public const int CHILDID_SELF = 0;

		// Token: 0x04002A62 RID: 10850
		public const int OBJID_QUERYCLASSNAMEIDX = -12;

		// Token: 0x04002A63 RID: 10851
		public const int OBJID_CLIENT = -4;

		// Token: 0x04002A64 RID: 10852
		public const int OBJID_WINDOW = 0;

		// Token: 0x04002A65 RID: 10853
		public const string uuid_IAccessible = "{618736E0-3C3D-11CF-810C-00AA00389B71}";

		// Token: 0x04002A66 RID: 10854
		public const string uuid_IEnumVariant = "{00020404-0000-0000-C000-000000000046}";

		// Token: 0x04002A67 RID: 10855
		public const int HH_FTS_DEFAULT_PROXIMITY = -1;

		// Token: 0x04002A68 RID: 10856
		public const int HICF_OTHER = 0;

		// Token: 0x04002A69 RID: 10857
		public const int HICF_MOUSE = 1;

		// Token: 0x04002A6A RID: 10858
		public const int HICF_ARROWKEYS = 2;

		// Token: 0x04002A6B RID: 10859
		public const int HICF_ACCELERATOR = 4;

		// Token: 0x04002A6C RID: 10860
		public const int HICF_DUPACCEL = 8;

		// Token: 0x04002A6D RID: 10861
		public const int HICF_ENTERING = 16;

		// Token: 0x04002A6E RID: 10862
		public const int HICF_LEAVING = 32;

		// Token: 0x04002A6F RID: 10863
		public const int HICF_RESELECT = 64;

		// Token: 0x04002A70 RID: 10864
		public const int HICF_LMOUSE = 128;

		// Token: 0x04002A71 RID: 10865
		public const int HICF_TOGGLEDROPDOWN = 256;

		// Token: 0x04002A72 RID: 10866
		public const int STAP_ALLOW_NONCLIENT = 1;

		// Token: 0x04002A73 RID: 10867
		public const int STAP_ALLOW_CONTROLS = 2;

		// Token: 0x04002A74 RID: 10868
		public const int STAP_ALLOW_WEBCONTENT = 4;

		// Token: 0x04002A75 RID: 10869
		public const int PS_NULL = 5;

		// Token: 0x04002A76 RID: 10870
		public const int PS_INSIDEFRAME = 6;

		// Token: 0x04002A77 RID: 10871
		public const int PS_GEOMETRIC = 65536;

		// Token: 0x04002A78 RID: 10872
		public const int PS_ENDCAP_SQUARE = 256;

		// Token: 0x04002A79 RID: 10873
		public const int NULL_BRUSH = 5;

		// Token: 0x04002A7A RID: 10874
		public const int MM_HIMETRIC = 3;

		// Token: 0x04002A7B RID: 10875
		public const uint STILL_ACTIVE = 259U;

		// Token: 0x04002A7C RID: 10876
		public static IntPtr InvalidIntPtr = (IntPtr)(-1);

		// Token: 0x04002A7D RID: 10877
		public static IntPtr LPSTR_TEXTCALLBACK = (IntPtr)(-1);

		// Token: 0x04002A7E RID: 10878
		public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		// Token: 0x04002A7F RID: 10879
		public static HandleRef HWND_TOP = new HandleRef(null, (IntPtr)0);

		// Token: 0x04002A80 RID: 10880
		public static HandleRef HWND_BOTTOM = new HandleRef(null, (IntPtr)1);

		// Token: 0x04002A81 RID: 10881
		public static HandleRef HWND_TOPMOST = new HandleRef(null, new IntPtr(-1));

		// Token: 0x04002A82 RID: 10882
		public static HandleRef HWND_NOTOPMOST = new HandleRef(null, new IntPtr(-2));

		// Token: 0x04002A83 RID: 10883
		public static HandleRef HWND_MESSAGE = new HandleRef(null, new IntPtr(-3));

		// Token: 0x04002A84 RID: 10884
		public static readonly int LOCALE_USER_DEFAULT = NativeMethods.MAKELCID(NativeMethods.LANG_USER_DEFAULT);

		// Token: 0x04002A85 RID: 10885
		public static readonly int LANG_USER_DEFAULT = NativeMethods.MAKELANGID(0, 1);

		// Token: 0x04002A86 RID: 10886
		public static int START_PAGE_GENERAL = -1;

		// Token: 0x04002A87 RID: 10887
		private static int wmMouseEnterMessage = -1;

		// Token: 0x04002A88 RID: 10888
		private static int wmUnSubclass = -1;

		// Token: 0x04002A89 RID: 10889
		public static readonly int BFFM_SETSELECTION;

		// Token: 0x04002A8A RID: 10890
		public static readonly int CBEM_GETITEM;

		// Token: 0x04002A8B RID: 10891
		public static readonly int CBEM_SETITEM;

		// Token: 0x04002A8C RID: 10892
		public static readonly int CBEN_ENDEDIT;

		// Token: 0x04002A8D RID: 10893
		public static readonly int CBEM_INSERTITEM;

		// Token: 0x04002A8E RID: 10894
		public static readonly int LVM_GETITEMTEXT;

		// Token: 0x04002A8F RID: 10895
		public static readonly int LVM_SETITEMTEXT;

		// Token: 0x04002A90 RID: 10896
		public static readonly int ACM_OPEN;

		// Token: 0x04002A91 RID: 10897
		public static readonly int DTM_SETFORMAT;

		// Token: 0x04002A92 RID: 10898
		public static readonly int DTN_USERSTRING;

		// Token: 0x04002A93 RID: 10899
		public static readonly int DTN_WMKEYDOWN;

		// Token: 0x04002A94 RID: 10900
		public static readonly int DTN_FORMAT;

		// Token: 0x04002A95 RID: 10901
		public static readonly int DTN_FORMATQUERY;

		// Token: 0x04002A96 RID: 10902
		public static readonly int EMR_POLYTEXTOUT;

		// Token: 0x04002A97 RID: 10903
		public static readonly int HDM_INSERTITEM;

		// Token: 0x04002A98 RID: 10904
		public static readonly int HDM_GETITEM;

		// Token: 0x04002A99 RID: 10905
		public static readonly int HDM_SETITEM;

		// Token: 0x04002A9A RID: 10906
		public static readonly int HDN_ITEMCHANGING;

		// Token: 0x04002A9B RID: 10907
		public static readonly int HDN_ITEMCHANGED;

		// Token: 0x04002A9C RID: 10908
		public static readonly int HDN_ITEMCLICK;

		// Token: 0x04002A9D RID: 10909
		public static readonly int HDN_ITEMDBLCLICK;

		// Token: 0x04002A9E RID: 10910
		public static readonly int HDN_DIVIDERDBLCLICK;

		// Token: 0x04002A9F RID: 10911
		public static readonly int HDN_BEGINTRACK;

		// Token: 0x04002AA0 RID: 10912
		public static readonly int HDN_ENDTRACK;

		// Token: 0x04002AA1 RID: 10913
		public static readonly int HDN_TRACK;

		// Token: 0x04002AA2 RID: 10914
		public static readonly int HDN_GETDISPINFO;

		// Token: 0x04002AA3 RID: 10915
		public static readonly int LVM_GETITEM;

		// Token: 0x04002AA4 RID: 10916
		public static readonly int LVM_SETBKIMAGE;

		// Token: 0x04002AA5 RID: 10917
		public static readonly int LVM_SETITEM;

		// Token: 0x04002AA6 RID: 10918
		public static readonly int LVM_INSERTITEM;

		// Token: 0x04002AA7 RID: 10919
		public static readonly int LVM_FINDITEM;

		// Token: 0x04002AA8 RID: 10920
		public static readonly int LVM_GETSTRINGWIDTH;

		// Token: 0x04002AA9 RID: 10921
		public static readonly int LVM_EDITLABEL;

		// Token: 0x04002AAA RID: 10922
		public static readonly int LVM_GETCOLUMN;

		// Token: 0x04002AAB RID: 10923
		public static readonly int LVM_SETCOLUMN;

		// Token: 0x04002AAC RID: 10924
		public static readonly int LVM_GETISEARCHSTRING;

		// Token: 0x04002AAD RID: 10925
		public static readonly int LVM_INSERTCOLUMN;

		// Token: 0x04002AAE RID: 10926
		public static readonly int LVN_BEGINLABELEDIT;

		// Token: 0x04002AAF RID: 10927
		public static readonly int LVN_ENDLABELEDIT;

		// Token: 0x04002AB0 RID: 10928
		public static readonly int LVN_ODFINDITEM;

		// Token: 0x04002AB1 RID: 10929
		public static readonly int LVN_GETDISPINFO;

		// Token: 0x04002AB2 RID: 10930
		public static readonly int LVN_GETINFOTIP;

		// Token: 0x04002AB3 RID: 10931
		public static readonly int LVN_SETDISPINFO;

		// Token: 0x04002AB4 RID: 10932
		public static readonly int PSM_SETTITLE;

		// Token: 0x04002AB5 RID: 10933
		public static readonly int PSM_SETFINISHTEXT;

		// Token: 0x04002AB6 RID: 10934
		public static readonly int RB_INSERTBAND;

		// Token: 0x04002AB7 RID: 10935
		public static readonly int SB_SETTEXT;

		// Token: 0x04002AB8 RID: 10936
		public static readonly int SB_GETTEXT;

		// Token: 0x04002AB9 RID: 10937
		public static readonly int SB_GETTEXTLENGTH;

		// Token: 0x04002ABA RID: 10938
		public static readonly int SB_SETTIPTEXT;

		// Token: 0x04002ABB RID: 10939
		public static readonly int SB_GETTIPTEXT;

		// Token: 0x04002ABC RID: 10940
		public static readonly int TB_SAVERESTORE;

		// Token: 0x04002ABD RID: 10941
		public static readonly int TB_ADDSTRING;

		// Token: 0x04002ABE RID: 10942
		public static readonly int TB_GETBUTTONTEXT;

		// Token: 0x04002ABF RID: 10943
		public static readonly int TB_MAPACCELERATOR;

		// Token: 0x04002AC0 RID: 10944
		public static readonly int TB_GETBUTTONINFO;

		// Token: 0x04002AC1 RID: 10945
		public static readonly int TB_SETBUTTONINFO;

		// Token: 0x04002AC2 RID: 10946
		public static readonly int TB_INSERTBUTTON;

		// Token: 0x04002AC3 RID: 10947
		public static readonly int TB_ADDBUTTONS;

		// Token: 0x04002AC4 RID: 10948
		public static readonly int TBN_GETBUTTONINFO;

		// Token: 0x04002AC5 RID: 10949
		public static readonly int TBN_GETINFOTIP;

		// Token: 0x04002AC6 RID: 10950
		public static readonly int TBN_GETDISPINFO;

		// Token: 0x04002AC7 RID: 10951
		public static readonly int TTM_ADDTOOL;

		// Token: 0x04002AC8 RID: 10952
		public static readonly int TTM_SETTITLE;

		// Token: 0x04002AC9 RID: 10953
		public static readonly int TTM_DELTOOL;

		// Token: 0x04002ACA RID: 10954
		public static readonly int TTM_NEWTOOLRECT;

		// Token: 0x04002ACB RID: 10955
		public static readonly int TTM_GETTOOLINFO;

		// Token: 0x04002ACC RID: 10956
		public static readonly int TTM_SETTOOLINFO;

		// Token: 0x04002ACD RID: 10957
		public static readonly int TTM_HITTEST;

		// Token: 0x04002ACE RID: 10958
		public static readonly int TTM_GETTEXT;

		// Token: 0x04002ACF RID: 10959
		public static readonly int TTM_UPDATETIPTEXT;

		// Token: 0x04002AD0 RID: 10960
		public static readonly int TTM_ENUMTOOLS;

		// Token: 0x04002AD1 RID: 10961
		public static readonly int TTM_GETCURRENTTOOL;

		// Token: 0x04002AD2 RID: 10962
		public static readonly int TTN_GETDISPINFO;

		// Token: 0x04002AD3 RID: 10963
		public static readonly int TTN_NEEDTEXT;

		// Token: 0x04002AD4 RID: 10964
		public static readonly int TVM_INSERTITEM;

		// Token: 0x04002AD5 RID: 10965
		public static readonly int TVM_GETITEM;

		// Token: 0x04002AD6 RID: 10966
		public static readonly int TVM_SETITEM;

		// Token: 0x04002AD7 RID: 10967
		public static readonly int TVM_EDITLABEL;

		// Token: 0x04002AD8 RID: 10968
		public static readonly int TVM_GETISEARCHSTRING;

		// Token: 0x04002AD9 RID: 10969
		public static readonly int TVN_SELCHANGING;

		// Token: 0x04002ADA RID: 10970
		public static readonly int TVN_SELCHANGED;

		// Token: 0x04002ADB RID: 10971
		public static readonly int TVN_GETDISPINFO;

		// Token: 0x04002ADC RID: 10972
		public static readonly int TVN_SETDISPINFO;

		// Token: 0x04002ADD RID: 10973
		public static readonly int TVN_ITEMEXPANDING;

		// Token: 0x04002ADE RID: 10974
		public static readonly int TVN_ITEMEXPANDED;

		// Token: 0x04002ADF RID: 10975
		public static readonly int TVN_BEGINDRAG;

		// Token: 0x04002AE0 RID: 10976
		public static readonly int TVN_BEGINRDRAG;

		// Token: 0x04002AE1 RID: 10977
		public static readonly int TVN_BEGINLABELEDIT;

		// Token: 0x04002AE2 RID: 10978
		public static readonly int TVN_ENDLABELEDIT;

		// Token: 0x04002AE3 RID: 10979
		public static readonly int TCM_GETITEM;

		// Token: 0x04002AE4 RID: 10980
		public static readonly int TCM_SETITEM;

		// Token: 0x04002AE5 RID: 10981
		public static readonly int TCM_INSERTITEM;

		// Token: 0x020004CC RID: 1228
		public enum RegionFlags
		{
			// Token: 0x04002AE7 RID: 10983
			ERROR,
			// Token: 0x04002AE8 RID: 10984
			NULLREGION,
			// Token: 0x04002AE9 RID: 10985
			SIMPLEREGION,
			// Token: 0x04002AEA RID: 10986
			COMPLEXREGION
		}

		// Token: 0x020004CD RID: 1229
		[CLSCompliant(false)]
		[StructLayout(LayoutKind.Sequential)]
		public class OLECMD
		{
			// Token: 0x04002AEB RID: 10987
			[MarshalAs(UnmanagedType.U4)]
			public int cmdID;

			// Token: 0x04002AEC RID: 10988
			[MarshalAs(UnmanagedType.U4)]
			public int cmdf;
		}

		// Token: 0x020004CE RID: 1230
		[CLSCompliant(false)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
		[ComVisible(true)]
		[ComImport]
		public interface IOleCommandTarget
		{
			// Token: 0x0600497F RID: 18815
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In] [Out] NativeMethods.OLECMD prgCmds, [In] [Out] IntPtr pCmdText);

			// Token: 0x06004980 RID: 18816
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt, [MarshalAs(UnmanagedType.LPArray)] [In] object[] pvaIn, int pvaOut);
		}

		// Token: 0x020004CF RID: 1231
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class FONTDESC
		{
			// Token: 0x04002AED RID: 10989
			public int cbSizeOfStruct = Marshal.SizeOf(typeof(NativeMethods.FONTDESC));

			// Token: 0x04002AEE RID: 10990
			public string lpstrName;

			// Token: 0x04002AEF RID: 10991
			public long cySize;

			// Token: 0x04002AF0 RID: 10992
			public short sWeight;

			// Token: 0x04002AF1 RID: 10993
			public short sCharset;

			// Token: 0x04002AF2 RID: 10994
			public bool fItalic;

			// Token: 0x04002AF3 RID: 10995
			public bool fUnderline;

			// Token: 0x04002AF4 RID: 10996
			public bool fStrikethrough;
		}

		// Token: 0x020004D0 RID: 1232
		[StructLayout(LayoutKind.Sequential)]
		public class PICTDESCbmp
		{
			// Token: 0x06004982 RID: 18818 RVA: 0x0010C7D0 File Offset: 0x0010B7D0
			public PICTDESCbmp(Bitmap bitmap)
			{
				this.hbitmap = bitmap.GetHbitmap();
			}

			// Token: 0x04002AF5 RID: 10997
			internal int cbSizeOfStruct = Marshal.SizeOf(typeof(NativeMethods.PICTDESCbmp));

			// Token: 0x04002AF6 RID: 10998
			internal int picType = 1;

			// Token: 0x04002AF7 RID: 10999
			internal IntPtr hbitmap = IntPtr.Zero;

			// Token: 0x04002AF8 RID: 11000
			internal IntPtr hpalette = IntPtr.Zero;

			// Token: 0x04002AF9 RID: 11001
			internal int unused;
		}

		// Token: 0x020004D1 RID: 1233
		[StructLayout(LayoutKind.Sequential)]
		public class PICTDESCicon
		{
			// Token: 0x06004983 RID: 18819 RVA: 0x0010C824 File Offset: 0x0010B824
			public PICTDESCicon(Icon icon)
			{
				this.hicon = SafeNativeMethods.CopyImage(new HandleRef(icon, icon.Handle), 1, icon.Size.Width, icon.Size.Height, 0);
			}

			// Token: 0x04002AFA RID: 11002
			internal int cbSizeOfStruct = Marshal.SizeOf(typeof(NativeMethods.PICTDESCicon));

			// Token: 0x04002AFB RID: 11003
			internal int picType = 3;

			// Token: 0x04002AFC RID: 11004
			internal IntPtr hicon = IntPtr.Zero;

			// Token: 0x04002AFD RID: 11005
			internal int unused1;

			// Token: 0x04002AFE RID: 11006
			internal int unused2;
		}

		// Token: 0x020004D2 RID: 1234
		[StructLayout(LayoutKind.Sequential)]
		public class PICTDESCemf
		{
			// Token: 0x06004984 RID: 18820 RVA: 0x0010C893 File Offset: 0x0010B893
			public PICTDESCemf(Metafile metafile)
			{
			}

			// Token: 0x04002AFF RID: 11007
			internal int cbSizeOfStruct = Marshal.SizeOf(typeof(NativeMethods.PICTDESCemf));

			// Token: 0x04002B00 RID: 11008
			internal int picType = 4;

			// Token: 0x04002B01 RID: 11009
			internal IntPtr hemf = IntPtr.Zero;

			// Token: 0x04002B02 RID: 11010
			internal int unused1;

			// Token: 0x04002B03 RID: 11011
			internal int unused2;
		}

		// Token: 0x020004D3 RID: 1235
		[StructLayout(LayoutKind.Sequential)]
		public class USEROBJECTFLAGS
		{
			// Token: 0x04002B04 RID: 11012
			public int fInherit;

			// Token: 0x04002B05 RID: 11013
			public int fReserved;

			// Token: 0x04002B06 RID: 11014
			public int dwFlags;
		}

		// Token: 0x020004D4 RID: 1236
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class SYSTEMTIMEARRAY
		{
			// Token: 0x04002B07 RID: 11015
			public short wYear1;

			// Token: 0x04002B08 RID: 11016
			public short wMonth1;

			// Token: 0x04002B09 RID: 11017
			public short wDayOfWeek1;

			// Token: 0x04002B0A RID: 11018
			public short wDay1;

			// Token: 0x04002B0B RID: 11019
			public short wHour1;

			// Token: 0x04002B0C RID: 11020
			public short wMinute1;

			// Token: 0x04002B0D RID: 11021
			public short wSecond1;

			// Token: 0x04002B0E RID: 11022
			public short wMilliseconds1;

			// Token: 0x04002B0F RID: 11023
			public short wYear2;

			// Token: 0x04002B10 RID: 11024
			public short wMonth2;

			// Token: 0x04002B11 RID: 11025
			public short wDayOfWeek2;

			// Token: 0x04002B12 RID: 11026
			public short wDay2;

			// Token: 0x04002B13 RID: 11027
			public short wHour2;

			// Token: 0x04002B14 RID: 11028
			public short wMinute2;

			// Token: 0x04002B15 RID: 11029
			public short wSecond2;

			// Token: 0x04002B16 RID: 11030
			public short wMilliseconds2;
		}

		// Token: 0x020004D5 RID: 1237
		// (Invoke) Token: 0x06004988 RID: 18824
		public delegate bool EnumChildrenCallback(IntPtr hwnd, IntPtr lParam);

		// Token: 0x020004D6 RID: 1238
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class HH_AKLINK
		{
			// Token: 0x04002B17 RID: 11031
			internal int cbStruct = Marshal.SizeOf(typeof(NativeMethods.HH_AKLINK));

			// Token: 0x04002B18 RID: 11032
			internal bool fReserved;

			// Token: 0x04002B19 RID: 11033
			internal string pszKeywords;

			// Token: 0x04002B1A RID: 11034
			internal string pszUrl;

			// Token: 0x04002B1B RID: 11035
			internal string pszMsgText;

			// Token: 0x04002B1C RID: 11036
			internal string pszMsgTitle;

			// Token: 0x04002B1D RID: 11037
			internal string pszWindow;

			// Token: 0x04002B1E RID: 11038
			internal bool fIndexOnFail;
		}

		// Token: 0x020004D7 RID: 1239
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class HH_POPUP
		{
			// Token: 0x04002B1F RID: 11039
			internal int cbStruct = Marshal.SizeOf(typeof(NativeMethods.HH_POPUP));

			// Token: 0x04002B20 RID: 11040
			internal IntPtr hinst = IntPtr.Zero;

			// Token: 0x04002B21 RID: 11041
			internal int idString;

			// Token: 0x04002B22 RID: 11042
			internal IntPtr pszText;

			// Token: 0x04002B23 RID: 11043
			internal NativeMethods.POINT pt;

			// Token: 0x04002B24 RID: 11044
			internal int clrForeground = -1;

			// Token: 0x04002B25 RID: 11045
			internal int clrBackground = -1;

			// Token: 0x04002B26 RID: 11046
			internal NativeMethods.RECT rcMargins = NativeMethods.RECT.FromXYWH(-1, -1, -1, -1);

			// Token: 0x04002B27 RID: 11047
			internal string pszFont;
		}

		// Token: 0x020004D8 RID: 1240
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class HH_FTS_QUERY
		{
			// Token: 0x04002B28 RID: 11048
			internal int cbStruct = Marshal.SizeOf(typeof(NativeMethods.HH_FTS_QUERY));

			// Token: 0x04002B29 RID: 11049
			internal bool fUniCodeStrings;

			// Token: 0x04002B2A RID: 11050
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszSearchQuery;

			// Token: 0x04002B2B RID: 11051
			internal int iProximity = -1;

			// Token: 0x04002B2C RID: 11052
			internal bool fStemmedSearch;

			// Token: 0x04002B2D RID: 11053
			internal bool fTitleOnly;

			// Token: 0x04002B2E RID: 11054
			internal bool fExecute = true;

			// Token: 0x04002B2F RID: 11055
			[MarshalAs(UnmanagedType.LPStr)]
			internal string pszWindow;
		}

		// Token: 0x020004D9 RID: 1241
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		public class MONITORINFOEX
		{
			// Token: 0x04002B30 RID: 11056
			internal int cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFOEX));

			// Token: 0x04002B31 RID: 11057
			internal NativeMethods.RECT rcMonitor = default(NativeMethods.RECT);

			// Token: 0x04002B32 RID: 11058
			internal NativeMethods.RECT rcWork = default(NativeMethods.RECT);

			// Token: 0x04002B33 RID: 11059
			internal int dwFlags;

			// Token: 0x04002B34 RID: 11060
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			internal char[] szDevice = new char[32];
		}

		// Token: 0x020004DA RID: 1242
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		public class MONITORINFO
		{
			// Token: 0x04002B35 RID: 11061
			internal int cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFO));

			// Token: 0x04002B36 RID: 11062
			internal NativeMethods.RECT rcMonitor = default(NativeMethods.RECT);

			// Token: 0x04002B37 RID: 11063
			internal NativeMethods.RECT rcWork = default(NativeMethods.RECT);

			// Token: 0x04002B38 RID: 11064
			internal int dwFlags;
		}

		// Token: 0x020004DB RID: 1243
		// (Invoke) Token: 0x06004991 RID: 18833
		public delegate int EditStreamCallback(IntPtr dwCookie, IntPtr buf, int cb, out int transferred);

		// Token: 0x020004DC RID: 1244
		[StructLayout(LayoutKind.Sequential)]
		public class EDITSTREAM
		{
			// Token: 0x04002B39 RID: 11065
			public IntPtr dwCookie = IntPtr.Zero;

			// Token: 0x04002B3A RID: 11066
			public int dwError;

			// Token: 0x04002B3B RID: 11067
			public NativeMethods.EditStreamCallback pfnCallback;
		}

		// Token: 0x020004DD RID: 1245
		[StructLayout(LayoutKind.Sequential)]
		public class EDITSTREAM64
		{
			// Token: 0x04002B3C RID: 11068
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
			public byte[] contents = new byte[20];
		}

		// Token: 0x020004DE RID: 1246
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DEVMODE
		{
			// Token: 0x04002B3D RID: 11069
			private const int CCHDEVICENAME = 32;

			// Token: 0x04002B3E RID: 11070
			private const int CCHFORMNAME = 32;

			// Token: 0x04002B3F RID: 11071
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;

			// Token: 0x04002B40 RID: 11072
			public short dmSpecVersion;

			// Token: 0x04002B41 RID: 11073
			public short dmDriverVersion;

			// Token: 0x04002B42 RID: 11074
			public short dmSize;

			// Token: 0x04002B43 RID: 11075
			public short dmDriverExtra;

			// Token: 0x04002B44 RID: 11076
			public int dmFields;

			// Token: 0x04002B45 RID: 11077
			public int dmPositionX;

			// Token: 0x04002B46 RID: 11078
			public int dmPositionY;

			// Token: 0x04002B47 RID: 11079
			public ScreenOrientation dmDisplayOrientation;

			// Token: 0x04002B48 RID: 11080
			public int dmDisplayFixedOutput;

			// Token: 0x04002B49 RID: 11081
			public short dmColor;

			// Token: 0x04002B4A RID: 11082
			public short dmDuplex;

			// Token: 0x04002B4B RID: 11083
			public short dmYResolution;

			// Token: 0x04002B4C RID: 11084
			public short dmTTOption;

			// Token: 0x04002B4D RID: 11085
			public short dmCollate;

			// Token: 0x04002B4E RID: 11086
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;

			// Token: 0x04002B4F RID: 11087
			public short dmLogPixels;

			// Token: 0x04002B50 RID: 11088
			public int dmBitsPerPel;

			// Token: 0x04002B51 RID: 11089
			public int dmPelsWidth;

			// Token: 0x04002B52 RID: 11090
			public int dmPelsHeight;

			// Token: 0x04002B53 RID: 11091
			public int dmDisplayFlags;

			// Token: 0x04002B54 RID: 11092
			public int dmDisplayFrequency;

			// Token: 0x04002B55 RID: 11093
			public int dmICMMethod;

			// Token: 0x04002B56 RID: 11094
			public int dmICMIntent;

			// Token: 0x04002B57 RID: 11095
			public int dmMediaType;

			// Token: 0x04002B58 RID: 11096
			public int dmDitherType;

			// Token: 0x04002B59 RID: 11097
			public int dmReserved1;

			// Token: 0x04002B5A RID: 11098
			public int dmReserved2;

			// Token: 0x04002B5B RID: 11099
			public int dmPanningWidth;

			// Token: 0x04002B5C RID: 11100
			public int dmPanningHeight;
		}

		// Token: 0x020004DF RID: 1247
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0FF510A3-5FA5-49F1-8CCC-190D71083F3E")]
		[ComImport]
		public interface IVsPerPropertyBrowsing
		{
			// Token: 0x06004996 RID: 18838
			[PreserveSig]
			int HideProperty(int dispid, ref bool pfHide);

			// Token: 0x06004997 RID: 18839
			[PreserveSig]
			int DisplayChildProperties(int dispid, ref bool pfDisplay);

			// Token: 0x06004998 RID: 18840
			[PreserveSig]
			int GetLocalizedPropertyInfo(int dispid, int localeID, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pbstrLocalizedName, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pbstrLocalizeDescription);

			// Token: 0x06004999 RID: 18841
			[PreserveSig]
			int HasDefaultValue(int dispid, ref bool fDefault);

			// Token: 0x0600499A RID: 18842
			[PreserveSig]
			int IsPropertyReadOnly(int dispid, ref bool fReadOnly);

			// Token: 0x0600499B RID: 18843
			[PreserveSig]
			int GetClassName([In] [Out] ref string pbstrClassName);

			// Token: 0x0600499C RID: 18844
			[PreserveSig]
			int CanResetPropertyValue(int dispid, [In] [Out] ref bool pfCanReset);

			// Token: 0x0600499D RID: 18845
			[PreserveSig]
			int ResetPropertyValue(int dispid);
		}

		// Token: 0x020004E0 RID: 1248
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("7494683C-37A0-11d2-A273-00C04F8EF4FF")]
		[ComImport]
		public interface IManagedPerPropertyBrowsing
		{
			// Token: 0x0600499E RID: 18846
			[PreserveSig]
			int GetPropertyAttributes(int dispid, ref int pcAttributes, ref IntPtr pbstrAttrNames, ref IntPtr pvariantInitValues);
		}

		// Token: 0x020004E1 RID: 1249
		[Guid("33C0C1D8-33CF-11d3-BFF2-00C04F990235")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IProvidePropertyBuilder
		{
			// Token: 0x0600499F RID: 18847
			[PreserveSig]
			int MapPropertyToBuilder(int dispid, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] pdwCtlBldType, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] string[] pbstrGuidBldr, [MarshalAs(UnmanagedType.Bool)] [In] [Out] ref bool builderAvailable);

			// Token: 0x060049A0 RID: 18848
			[PreserveSig]
			int ExecuteBuilder(int dispid, [MarshalAs(UnmanagedType.BStr)] [In] string bstrGuidBldr, [MarshalAs(UnmanagedType.Interface)] [In] object pdispApp, HandleRef hwndBldrOwner, [MarshalAs(UnmanagedType.Struct)] [In] [Out] ref object pvarValue, [MarshalAs(UnmanagedType.Bool)] [In] [Out] ref bool actionCommitted);
		}

		// Token: 0x020004E2 RID: 1250
		[StructLayout(LayoutKind.Sequential)]
		public class INITCOMMONCONTROLSEX
		{
			// Token: 0x04002B5D RID: 11101
			public int dwSize = 8;

			// Token: 0x04002B5E RID: 11102
			public int dwICC;
		}

		// Token: 0x020004E3 RID: 1251
		[StructLayout(LayoutKind.Sequential)]
		public class IMAGELISTDRAWPARAMS
		{
			// Token: 0x04002B5F RID: 11103
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.IMAGELISTDRAWPARAMS));

			// Token: 0x04002B60 RID: 11104
			public IntPtr himl = IntPtr.Zero;

			// Token: 0x04002B61 RID: 11105
			public int i;

			// Token: 0x04002B62 RID: 11106
			public IntPtr hdcDst = IntPtr.Zero;

			// Token: 0x04002B63 RID: 11107
			public int x;

			// Token: 0x04002B64 RID: 11108
			public int y;

			// Token: 0x04002B65 RID: 11109
			public int cx;

			// Token: 0x04002B66 RID: 11110
			public int cy;

			// Token: 0x04002B67 RID: 11111
			public int xBitmap;

			// Token: 0x04002B68 RID: 11112
			public int yBitmap;

			// Token: 0x04002B69 RID: 11113
			public int rgbBk;

			// Token: 0x04002B6A RID: 11114
			public int rgbFg;

			// Token: 0x04002B6B RID: 11115
			public int fStyle;

			// Token: 0x04002B6C RID: 11116
			public int dwRop;

			// Token: 0x04002B6D RID: 11117
			public int fState;

			// Token: 0x04002B6E RID: 11118
			public int Frame;

			// Token: 0x04002B6F RID: 11119
			public int crEffect;
		}

		// Token: 0x020004E4 RID: 1252
		[StructLayout(LayoutKind.Sequential)]
		public class IMAGEINFO
		{
			// Token: 0x04002B70 RID: 11120
			public IntPtr hbmImage = IntPtr.Zero;

			// Token: 0x04002B71 RID: 11121
			public IntPtr hbmMask = IntPtr.Zero;

			// Token: 0x04002B72 RID: 11122
			public int Unused1;

			// Token: 0x04002B73 RID: 11123
			public int Unused2;

			// Token: 0x04002B74 RID: 11124
			public int rcImage_left;

			// Token: 0x04002B75 RID: 11125
			public int rcImage_top;

			// Token: 0x04002B76 RID: 11126
			public int rcImage_right;

			// Token: 0x04002B77 RID: 11127
			public int rcImage_bottom;
		}

		// Token: 0x020004E5 RID: 1253
		[StructLayout(LayoutKind.Sequential)]
		public class TRACKMOUSEEVENT
		{
			// Token: 0x04002B78 RID: 11128
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.TRACKMOUSEEVENT));

			// Token: 0x04002B79 RID: 11129
			public int dwFlags;

			// Token: 0x04002B7A RID: 11130
			public IntPtr hwndTrack;

			// Token: 0x04002B7B RID: 11131
			public int dwHoverTime = 100;
		}

		// Token: 0x020004E6 RID: 1254
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			// Token: 0x060049A5 RID: 18853 RVA: 0x0010CA9B File Offset: 0x0010BA9B
			public POINT()
			{
			}

			// Token: 0x060049A6 RID: 18854 RVA: 0x0010CAA3 File Offset: 0x0010BAA3
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x04002B7C RID: 11132
			public int x;

			// Token: 0x04002B7D RID: 11133
			public int y;
		}

		// Token: 0x020004E7 RID: 1255
		public struct POINTSTRUCT
		{
			// Token: 0x060049A7 RID: 18855 RVA: 0x0010CAB9 File Offset: 0x0010BAB9
			public POINTSTRUCT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x04002B7E RID: 11134
			public int x;

			// Token: 0x04002B7F RID: 11135
			public int y;
		}

		// Token: 0x020004E8 RID: 1256
		// (Invoke) Token: 0x060049A9 RID: 18857
		public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x020004E9 RID: 1257
		public struct RECT
		{
			// Token: 0x060049AC RID: 18860 RVA: 0x0010CAC9 File Offset: 0x0010BAC9
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x060049AD RID: 18861 RVA: 0x0010CAE8 File Offset: 0x0010BAE8
			public RECT(Rectangle r)
			{
				this.left = r.Left;
				this.top = r.Top;
				this.right = r.Right;
				this.bottom = r.Bottom;
			}

			// Token: 0x060049AE RID: 18862 RVA: 0x0010CB1E File Offset: 0x0010BB1E
			public static NativeMethods.RECT FromXYWH(int x, int y, int width, int height)
			{
				return new NativeMethods.RECT(x, y, x + width, y + height);
			}

			// Token: 0x17000EAE RID: 3758
			// (get) Token: 0x060049AF RID: 18863 RVA: 0x0010CB2D File Offset: 0x0010BB2D
			public Size Size
			{
				get
				{
					return new Size(this.right - this.left, this.bottom - this.top);
				}
			}

			// Token: 0x04002B80 RID: 11136
			public int left;

			// Token: 0x04002B81 RID: 11137
			public int top;

			// Token: 0x04002B82 RID: 11138
			public int right;

			// Token: 0x04002B83 RID: 11139
			public int bottom;
		}

		// Token: 0x020004EA RID: 1258
		public struct MARGINS
		{
			// Token: 0x04002B84 RID: 11140
			public int cxLeftWidth;

			// Token: 0x04002B85 RID: 11141
			public int cxRightWidth;

			// Token: 0x04002B86 RID: 11142
			public int cyTopHeight;

			// Token: 0x04002B87 RID: 11143
			public int cyBottomHeight;
		}

		// Token: 0x020004EB RID: 1259
		// (Invoke) Token: 0x060049B1 RID: 18865
		public delegate int ListViewCompareCallback(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

		// Token: 0x020004EC RID: 1260
		// (Invoke) Token: 0x060049B5 RID: 18869
		public delegate int TreeViewCompareCallback(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

		// Token: 0x020004ED RID: 1261
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class WNDCLASS_I
		{
			// Token: 0x04002B88 RID: 11144
			public int style;

			// Token: 0x04002B89 RID: 11145
			public IntPtr lpfnWndProc = IntPtr.Zero;

			// Token: 0x04002B8A RID: 11146
			public int cbClsExtra;

			// Token: 0x04002B8B RID: 11147
			public int cbWndExtra;

			// Token: 0x04002B8C RID: 11148
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x04002B8D RID: 11149
			public IntPtr hIcon = IntPtr.Zero;

			// Token: 0x04002B8E RID: 11150
			public IntPtr hCursor = IntPtr.Zero;

			// Token: 0x04002B8F RID: 11151
			public IntPtr hbrBackground = IntPtr.Zero;

			// Token: 0x04002B90 RID: 11152
			public IntPtr lpszMenuName = IntPtr.Zero;

			// Token: 0x04002B91 RID: 11153
			public IntPtr lpszClassName = IntPtr.Zero;
		}

		// Token: 0x020004EE RID: 1262
		[StructLayout(LayoutKind.Sequential)]
		public class NONCLIENTMETRICS
		{
			// Token: 0x04002B92 RID: 11154
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.NONCLIENTMETRICS));

			// Token: 0x04002B93 RID: 11155
			public int iBorderWidth;

			// Token: 0x04002B94 RID: 11156
			public int iScrollWidth;

			// Token: 0x04002B95 RID: 11157
			public int iScrollHeight;

			// Token: 0x04002B96 RID: 11158
			public int iCaptionWidth;

			// Token: 0x04002B97 RID: 11159
			public int iCaptionHeight;

			// Token: 0x04002B98 RID: 11160
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfCaptionFont;

			// Token: 0x04002B99 RID: 11161
			public int iSmCaptionWidth;

			// Token: 0x04002B9A RID: 11162
			public int iSmCaptionHeight;

			// Token: 0x04002B9B RID: 11163
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfSmCaptionFont;

			// Token: 0x04002B9C RID: 11164
			public int iMenuWidth;

			// Token: 0x04002B9D RID: 11165
			public int iMenuHeight;

			// Token: 0x04002B9E RID: 11166
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfMenuFont;

			// Token: 0x04002B9F RID: 11167
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfStatusFont;

			// Token: 0x04002BA0 RID: 11168
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfMessageFont;
		}

		// Token: 0x020004EF RID: 1263
		[Serializable]
		public struct MSG
		{
			// Token: 0x04002BA1 RID: 11169
			public IntPtr hwnd;

			// Token: 0x04002BA2 RID: 11170
			public int message;

			// Token: 0x04002BA3 RID: 11171
			public IntPtr wParam;

			// Token: 0x04002BA4 RID: 11172
			public IntPtr lParam;

			// Token: 0x04002BA5 RID: 11173
			public int time;

			// Token: 0x04002BA6 RID: 11174
			public int pt_x;

			// Token: 0x04002BA7 RID: 11175
			public int pt_y;
		}

		// Token: 0x020004F0 RID: 1264
		public struct PAINTSTRUCT
		{
			// Token: 0x04002BA8 RID: 11176
			public IntPtr hdc;

			// Token: 0x04002BA9 RID: 11177
			public bool fErase;

			// Token: 0x04002BAA RID: 11178
			public int rcPaint_left;

			// Token: 0x04002BAB RID: 11179
			public int rcPaint_top;

			// Token: 0x04002BAC RID: 11180
			public int rcPaint_right;

			// Token: 0x04002BAD RID: 11181
			public int rcPaint_bottom;

			// Token: 0x04002BAE RID: 11182
			public bool fRestore;

			// Token: 0x04002BAF RID: 11183
			public bool fIncUpdate;

			// Token: 0x04002BB0 RID: 11184
			public int reserved1;

			// Token: 0x04002BB1 RID: 11185
			public int reserved2;

			// Token: 0x04002BB2 RID: 11186
			public int reserved3;

			// Token: 0x04002BB3 RID: 11187
			public int reserved4;

			// Token: 0x04002BB4 RID: 11188
			public int reserved5;

			// Token: 0x04002BB5 RID: 11189
			public int reserved6;

			// Token: 0x04002BB6 RID: 11190
			public int reserved7;

			// Token: 0x04002BB7 RID: 11191
			public int reserved8;
		}

		// Token: 0x020004F1 RID: 1265
		[StructLayout(LayoutKind.Sequential)]
		public class SCROLLINFO
		{
			// Token: 0x060049BA RID: 18874 RVA: 0x0010CBCD File Offset: 0x0010BBCD
			public SCROLLINFO()
			{
			}

			// Token: 0x060049BB RID: 18875 RVA: 0x0010CBEC File Offset: 0x0010BBEC
			public SCROLLINFO(int mask, int min, int max, int page, int pos)
			{
				this.fMask = mask;
				this.nMin = min;
				this.nMax = max;
				this.nPage = page;
				this.nPos = pos;
			}

			// Token: 0x04002BB8 RID: 11192
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));

			// Token: 0x04002BB9 RID: 11193
			public int fMask;

			// Token: 0x04002BBA RID: 11194
			public int nMin;

			// Token: 0x04002BBB RID: 11195
			public int nMax;

			// Token: 0x04002BBC RID: 11196
			public int nPage;

			// Token: 0x04002BBD RID: 11197
			public int nPos;

			// Token: 0x04002BBE RID: 11198
			public int nTrackPos;
		}

		// Token: 0x020004F2 RID: 1266
		[StructLayout(LayoutKind.Sequential)]
		public class TPMPARAMS
		{
			// Token: 0x04002BBF RID: 11199
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.TPMPARAMS));

			// Token: 0x04002BC0 RID: 11200
			public int rcExclude_left;

			// Token: 0x04002BC1 RID: 11201
			public int rcExclude_top;

			// Token: 0x04002BC2 RID: 11202
			public int rcExclude_right;

			// Token: 0x04002BC3 RID: 11203
			public int rcExclude_bottom;
		}

		// Token: 0x020004F3 RID: 1267
		[StructLayout(LayoutKind.Sequential)]
		public class SIZE
		{
			// Token: 0x060049BD RID: 18877 RVA: 0x0010CC56 File Offset: 0x0010BC56
			public SIZE()
			{
			}

			// Token: 0x060049BE RID: 18878 RVA: 0x0010CC5E File Offset: 0x0010BC5E
			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}

			// Token: 0x04002BC4 RID: 11204
			public int cx;

			// Token: 0x04002BC5 RID: 11205
			public int cy;
		}

		// Token: 0x020004F4 RID: 1268
		public struct WINDOWPLACEMENT
		{
			// Token: 0x04002BC6 RID: 11206
			public int length;

			// Token: 0x04002BC7 RID: 11207
			public int flags;

			// Token: 0x04002BC8 RID: 11208
			public int showCmd;

			// Token: 0x04002BC9 RID: 11209
			public int ptMinPosition_x;

			// Token: 0x04002BCA RID: 11210
			public int ptMinPosition_y;

			// Token: 0x04002BCB RID: 11211
			public int ptMaxPosition_x;

			// Token: 0x04002BCC RID: 11212
			public int ptMaxPosition_y;

			// Token: 0x04002BCD RID: 11213
			public int rcNormalPosition_left;

			// Token: 0x04002BCE RID: 11214
			public int rcNormalPosition_top;

			// Token: 0x04002BCF RID: 11215
			public int rcNormalPosition_right;

			// Token: 0x04002BD0 RID: 11216
			public int rcNormalPosition_bottom;
		}

		// Token: 0x020004F5 RID: 1269
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class STARTUPINFO_I
		{
			// Token: 0x04002BD1 RID: 11217
			public int cb;

			// Token: 0x04002BD2 RID: 11218
			public IntPtr lpReserved = IntPtr.Zero;

			// Token: 0x04002BD3 RID: 11219
			public IntPtr lpDesktop = IntPtr.Zero;

			// Token: 0x04002BD4 RID: 11220
			public IntPtr lpTitle = IntPtr.Zero;

			// Token: 0x04002BD5 RID: 11221
			public int dwX;

			// Token: 0x04002BD6 RID: 11222
			public int dwY;

			// Token: 0x04002BD7 RID: 11223
			public int dwXSize;

			// Token: 0x04002BD8 RID: 11224
			public int dwYSize;

			// Token: 0x04002BD9 RID: 11225
			public int dwXCountChars;

			// Token: 0x04002BDA RID: 11226
			public int dwYCountChars;

			// Token: 0x04002BDB RID: 11227
			public int dwFillAttribute;

			// Token: 0x04002BDC RID: 11228
			public int dwFlags;

			// Token: 0x04002BDD RID: 11229
			public short wShowWindow;

			// Token: 0x04002BDE RID: 11230
			public short cbReserved2;

			// Token: 0x04002BDF RID: 11231
			public IntPtr lpReserved2 = IntPtr.Zero;

			// Token: 0x04002BE0 RID: 11232
			public IntPtr hStdInput = IntPtr.Zero;

			// Token: 0x04002BE1 RID: 11233
			public IntPtr hStdOutput = IntPtr.Zero;

			// Token: 0x04002BE2 RID: 11234
			public IntPtr hStdError = IntPtr.Zero;
		}

		// Token: 0x020004F6 RID: 1270
		[StructLayout(LayoutKind.Sequential)]
		public class PAGESETUPDLG
		{
			// Token: 0x04002BE3 RID: 11235
			public int lStructSize;

			// Token: 0x04002BE4 RID: 11236
			public IntPtr hwndOwner;

			// Token: 0x04002BE5 RID: 11237
			public IntPtr hDevMode;

			// Token: 0x04002BE6 RID: 11238
			public IntPtr hDevNames;

			// Token: 0x04002BE7 RID: 11239
			public int Flags;

			// Token: 0x04002BE8 RID: 11240
			public int paperSizeX;

			// Token: 0x04002BE9 RID: 11241
			public int paperSizeY;

			// Token: 0x04002BEA RID: 11242
			public int minMarginLeft;

			// Token: 0x04002BEB RID: 11243
			public int minMarginTop;

			// Token: 0x04002BEC RID: 11244
			public int minMarginRight;

			// Token: 0x04002BED RID: 11245
			public int minMarginBottom;

			// Token: 0x04002BEE RID: 11246
			public int marginLeft;

			// Token: 0x04002BEF RID: 11247
			public int marginTop;

			// Token: 0x04002BF0 RID: 11248
			public int marginRight;

			// Token: 0x04002BF1 RID: 11249
			public int marginBottom;

			// Token: 0x04002BF2 RID: 11250
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x04002BF3 RID: 11251
			public IntPtr lCustData = IntPtr.Zero;

			// Token: 0x04002BF4 RID: 11252
			public NativeMethods.WndProc lpfnPageSetupHook;

			// Token: 0x04002BF5 RID: 11253
			public NativeMethods.WndProc lpfnPagePaintHook;

			// Token: 0x04002BF6 RID: 11254
			public string lpPageSetupTemplateName;

			// Token: 0x04002BF7 RID: 11255
			public IntPtr hPageSetupTemplate = IntPtr.Zero;
		}

		// Token: 0x020004F7 RID: 1271
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class PRINTDLG
		{
			// Token: 0x04002BF8 RID: 11256
			public int lStructSize;

			// Token: 0x04002BF9 RID: 11257
			public IntPtr hwndOwner;

			// Token: 0x04002BFA RID: 11258
			public IntPtr hDevMode;

			// Token: 0x04002BFB RID: 11259
			public IntPtr hDevNames;

			// Token: 0x04002BFC RID: 11260
			public IntPtr hDC;

			// Token: 0x04002BFD RID: 11261
			public int Flags;

			// Token: 0x04002BFE RID: 11262
			public short nFromPage;

			// Token: 0x04002BFF RID: 11263
			public short nToPage;

			// Token: 0x04002C00 RID: 11264
			public short nMinPage;

			// Token: 0x04002C01 RID: 11265
			public short nMaxPage;

			// Token: 0x04002C02 RID: 11266
			public short nCopies;

			// Token: 0x04002C03 RID: 11267
			public IntPtr hInstance;

			// Token: 0x04002C04 RID: 11268
			public IntPtr lCustData;

			// Token: 0x04002C05 RID: 11269
			public NativeMethods.WndProc lpfnPrintHook;

			// Token: 0x04002C06 RID: 11270
			public NativeMethods.WndProc lpfnSetupHook;

			// Token: 0x04002C07 RID: 11271
			public string lpPrintTemplateName;

			// Token: 0x04002C08 RID: 11272
			public string lpSetupTemplateName;

			// Token: 0x04002C09 RID: 11273
			public IntPtr hPrintTemplate;

			// Token: 0x04002C0A RID: 11274
			public IntPtr hSetupTemplate;
		}

		// Token: 0x020004F8 RID: 1272
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class PRINTDLGEX
		{
			// Token: 0x04002C0B RID: 11275
			public int lStructSize;

			// Token: 0x04002C0C RID: 11276
			public IntPtr hwndOwner;

			// Token: 0x04002C0D RID: 11277
			public IntPtr hDevMode;

			// Token: 0x04002C0E RID: 11278
			public IntPtr hDevNames;

			// Token: 0x04002C0F RID: 11279
			public IntPtr hDC;

			// Token: 0x04002C10 RID: 11280
			public int Flags;

			// Token: 0x04002C11 RID: 11281
			public int Flags2;

			// Token: 0x04002C12 RID: 11282
			public int ExclusionFlags;

			// Token: 0x04002C13 RID: 11283
			public int nPageRanges;

			// Token: 0x04002C14 RID: 11284
			public int nMaxPageRanges;

			// Token: 0x04002C15 RID: 11285
			public IntPtr pageRanges;

			// Token: 0x04002C16 RID: 11286
			public int nMinPage;

			// Token: 0x04002C17 RID: 11287
			public int nMaxPage;

			// Token: 0x04002C18 RID: 11288
			public int nCopies;

			// Token: 0x04002C19 RID: 11289
			public IntPtr hInstance;

			// Token: 0x04002C1A RID: 11290
			[MarshalAs(UnmanagedType.LPStr)]
			public string lpPrintTemplateName;

			// Token: 0x04002C1B RID: 11291
			public NativeMethods.WndProc lpCallback;

			// Token: 0x04002C1C RID: 11292
			public int nPropertyPages;

			// Token: 0x04002C1D RID: 11293
			public IntPtr lphPropertyPages;

			// Token: 0x04002C1E RID: 11294
			public int nStartPage;

			// Token: 0x04002C1F RID: 11295
			public int dwResultAction;
		}

		// Token: 0x020004F9 RID: 1273
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class PRINTPAGERANGE
		{
			// Token: 0x04002C20 RID: 11296
			public int nFromPage;

			// Token: 0x04002C21 RID: 11297
			public int nToPage;
		}

		// Token: 0x020004FA RID: 1274
		[StructLayout(LayoutKind.Sequential)]
		public class PICTDESC
		{
			// Token: 0x060049C4 RID: 18884 RVA: 0x0010CD18 File Offset: 0x0010BD18
			public static NativeMethods.PICTDESC CreateBitmapPICTDESC(IntPtr hbitmap, IntPtr hpal)
			{
				return new NativeMethods.PICTDESC
				{
					cbSizeOfStruct = 16,
					picType = 1,
					union1 = hbitmap,
					union2 = (int)((long)hpal & (long)((ulong)-1)),
					union3 = (int)((long)hpal >> 32)
				};
			}

			// Token: 0x060049C5 RID: 18885 RVA: 0x0010CD64 File Offset: 0x0010BD64
			public static NativeMethods.PICTDESC CreateIconPICTDESC(IntPtr hicon)
			{
				return new NativeMethods.PICTDESC
				{
					cbSizeOfStruct = 12,
					picType = 3,
					union1 = hicon
				};
			}

			// Token: 0x060049C6 RID: 18886 RVA: 0x0010CD8E File Offset: 0x0010BD8E
			public virtual IntPtr GetHandle()
			{
				return this.union1;
			}

			// Token: 0x060049C7 RID: 18887 RVA: 0x0010CD96 File Offset: 0x0010BD96
			public virtual IntPtr GetHPal()
			{
				if (this.picType == 1)
				{
					return (IntPtr)((long)((ulong)this.union2 | (ulong)((ulong)((long)this.union3) << 32)));
				}
				return IntPtr.Zero;
			}

			// Token: 0x04002C22 RID: 11298
			internal int cbSizeOfStruct;

			// Token: 0x04002C23 RID: 11299
			public int picType;

			// Token: 0x04002C24 RID: 11300
			internal IntPtr union1;

			// Token: 0x04002C25 RID: 11301
			internal int union2;

			// Token: 0x04002C26 RID: 11302
			internal int union3;
		}

		// Token: 0x020004FB RID: 1275
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagFONTDESC
		{
			// Token: 0x04002C27 RID: 11303
			public int cbSizeofstruct = Marshal.SizeOf(typeof(NativeMethods.tagFONTDESC));

			// Token: 0x04002C28 RID: 11304
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpstrName;

			// Token: 0x04002C29 RID: 11305
			[MarshalAs(UnmanagedType.U8)]
			public long cySize;

			// Token: 0x04002C2A RID: 11306
			[MarshalAs(UnmanagedType.U2)]
			public short sWeight;

			// Token: 0x04002C2B RID: 11307
			[MarshalAs(UnmanagedType.U2)]
			public short sCharset;

			// Token: 0x04002C2C RID: 11308
			[MarshalAs(UnmanagedType.Bool)]
			public bool fItalic;

			// Token: 0x04002C2D RID: 11309
			[MarshalAs(UnmanagedType.Bool)]
			public bool fUnderline;

			// Token: 0x04002C2E RID: 11310
			[MarshalAs(UnmanagedType.Bool)]
			public bool fStrikethrough;
		}

		// Token: 0x020004FC RID: 1276
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class CHOOSECOLOR
		{
			// Token: 0x04002C2F RID: 11311
			public int lStructSize = Marshal.SizeOf(typeof(NativeMethods.CHOOSECOLOR));

			// Token: 0x04002C30 RID: 11312
			public IntPtr hwndOwner;

			// Token: 0x04002C31 RID: 11313
			public IntPtr hInstance;

			// Token: 0x04002C32 RID: 11314
			public int rgbResult;

			// Token: 0x04002C33 RID: 11315
			public IntPtr lpCustColors;

			// Token: 0x04002C34 RID: 11316
			public int Flags;

			// Token: 0x04002C35 RID: 11317
			public IntPtr lCustData = IntPtr.Zero;

			// Token: 0x04002C36 RID: 11318
			public NativeMethods.WndProc lpfnHook;

			// Token: 0x04002C37 RID: 11319
			public string lpTemplateName;
		}

		// Token: 0x020004FD RID: 1277
		// (Invoke) Token: 0x060049CC RID: 18892
		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		// Token: 0x020004FE RID: 1278
		[StructLayout(LayoutKind.Sequential)]
		public class BITMAP
		{
			// Token: 0x04002C38 RID: 11320
			public int bmType;

			// Token: 0x04002C39 RID: 11321
			public int bmWidth;

			// Token: 0x04002C3A RID: 11322
			public int bmHeight;

			// Token: 0x04002C3B RID: 11323
			public int bmWidthBytes;

			// Token: 0x04002C3C RID: 11324
			public short bmPlanes;

			// Token: 0x04002C3D RID: 11325
			public short bmBitsPixel;

			// Token: 0x04002C3E RID: 11326
			public IntPtr bmBits = IntPtr.Zero;
		}

		// Token: 0x020004FF RID: 1279
		[StructLayout(LayoutKind.Sequential)]
		public class ICONINFO
		{
			// Token: 0x04002C3F RID: 11327
			public int fIcon;

			// Token: 0x04002C40 RID: 11328
			public int xHotspot;

			// Token: 0x04002C41 RID: 11329
			public int yHotspot;

			// Token: 0x04002C42 RID: 11330
			public IntPtr hbmMask = IntPtr.Zero;

			// Token: 0x04002C43 RID: 11331
			public IntPtr hbmColor = IntPtr.Zero;
		}

		// Token: 0x02000500 RID: 1280
		[StructLayout(LayoutKind.Sequential)]
		public class LOGPEN
		{
			// Token: 0x04002C44 RID: 11332
			public int lopnStyle;

			// Token: 0x04002C45 RID: 11333
			public int lopnWidth_x;

			// Token: 0x04002C46 RID: 11334
			public int lopnWidth_y;

			// Token: 0x04002C47 RID: 11335
			public int lopnColor;
		}

		// Token: 0x02000501 RID: 1281
		[StructLayout(LayoutKind.Sequential)]
		public class LOGBRUSH
		{
			// Token: 0x04002C48 RID: 11336
			public int lbStyle;

			// Token: 0x04002C49 RID: 11337
			public int lbColor;

			// Token: 0x04002C4A RID: 11338
			public IntPtr lbHatch;
		}

		// Token: 0x02000502 RID: 1282
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x060049D3 RID: 18899 RVA: 0x0010CE4C File Offset: 0x0010BE4C
			public LOGFONT()
			{
			}

			// Token: 0x060049D4 RID: 18900 RVA: 0x0010CE54 File Offset: 0x0010BE54
			public LOGFONT(NativeMethods.LOGFONT lf)
			{
				this.lfHeight = lf.lfHeight;
				this.lfWidth = lf.lfWidth;
				this.lfEscapement = lf.lfEscapement;
				this.lfOrientation = lf.lfOrientation;
				this.lfWeight = lf.lfWeight;
				this.lfItalic = lf.lfItalic;
				this.lfUnderline = lf.lfUnderline;
				this.lfStrikeOut = lf.lfStrikeOut;
				this.lfCharSet = lf.lfCharSet;
				this.lfOutPrecision = lf.lfOutPrecision;
				this.lfClipPrecision = lf.lfClipPrecision;
				this.lfQuality = lf.lfQuality;
				this.lfPitchAndFamily = lf.lfPitchAndFamily;
				this.lfFaceName = lf.lfFaceName;
			}

			// Token: 0x04002C4B RID: 11339
			public int lfHeight;

			// Token: 0x04002C4C RID: 11340
			public int lfWidth;

			// Token: 0x04002C4D RID: 11341
			public int lfEscapement;

			// Token: 0x04002C4E RID: 11342
			public int lfOrientation;

			// Token: 0x04002C4F RID: 11343
			public int lfWeight;

			// Token: 0x04002C50 RID: 11344
			public byte lfItalic;

			// Token: 0x04002C51 RID: 11345
			public byte lfUnderline;

			// Token: 0x04002C52 RID: 11346
			public byte lfStrikeOut;

			// Token: 0x04002C53 RID: 11347
			public byte lfCharSet;

			// Token: 0x04002C54 RID: 11348
			public byte lfOutPrecision;

			// Token: 0x04002C55 RID: 11349
			public byte lfClipPrecision;

			// Token: 0x04002C56 RID: 11350
			public byte lfQuality;

			// Token: 0x04002C57 RID: 11351
			public byte lfPitchAndFamily;

			// Token: 0x04002C58 RID: 11352
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x02000503 RID: 1283
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TEXTMETRIC
		{
			// Token: 0x04002C59 RID: 11353
			public int tmHeight;

			// Token: 0x04002C5A RID: 11354
			public int tmAscent;

			// Token: 0x04002C5B RID: 11355
			public int tmDescent;

			// Token: 0x04002C5C RID: 11356
			public int tmInternalLeading;

			// Token: 0x04002C5D RID: 11357
			public int tmExternalLeading;

			// Token: 0x04002C5E RID: 11358
			public int tmAveCharWidth;

			// Token: 0x04002C5F RID: 11359
			public int tmMaxCharWidth;

			// Token: 0x04002C60 RID: 11360
			public int tmWeight;

			// Token: 0x04002C61 RID: 11361
			public int tmOverhang;

			// Token: 0x04002C62 RID: 11362
			public int tmDigitizedAspectX;

			// Token: 0x04002C63 RID: 11363
			public int tmDigitizedAspectY;

			// Token: 0x04002C64 RID: 11364
			public char tmFirstChar;

			// Token: 0x04002C65 RID: 11365
			public char tmLastChar;

			// Token: 0x04002C66 RID: 11366
			public char tmDefaultChar;

			// Token: 0x04002C67 RID: 11367
			public char tmBreakChar;

			// Token: 0x04002C68 RID: 11368
			public byte tmItalic;

			// Token: 0x04002C69 RID: 11369
			public byte tmUnderlined;

			// Token: 0x04002C6A RID: 11370
			public byte tmStruckOut;

			// Token: 0x04002C6B RID: 11371
			public byte tmPitchAndFamily;

			// Token: 0x04002C6C RID: 11372
			public byte tmCharSet;
		}

		// Token: 0x02000504 RID: 1284
		public struct TEXTMETRICA
		{
			// Token: 0x04002C6D RID: 11373
			public int tmHeight;

			// Token: 0x04002C6E RID: 11374
			public int tmAscent;

			// Token: 0x04002C6F RID: 11375
			public int tmDescent;

			// Token: 0x04002C70 RID: 11376
			public int tmInternalLeading;

			// Token: 0x04002C71 RID: 11377
			public int tmExternalLeading;

			// Token: 0x04002C72 RID: 11378
			public int tmAveCharWidth;

			// Token: 0x04002C73 RID: 11379
			public int tmMaxCharWidth;

			// Token: 0x04002C74 RID: 11380
			public int tmWeight;

			// Token: 0x04002C75 RID: 11381
			public int tmOverhang;

			// Token: 0x04002C76 RID: 11382
			public int tmDigitizedAspectX;

			// Token: 0x04002C77 RID: 11383
			public int tmDigitizedAspectY;

			// Token: 0x04002C78 RID: 11384
			public byte tmFirstChar;

			// Token: 0x04002C79 RID: 11385
			public byte tmLastChar;

			// Token: 0x04002C7A RID: 11386
			public byte tmDefaultChar;

			// Token: 0x04002C7B RID: 11387
			public byte tmBreakChar;

			// Token: 0x04002C7C RID: 11388
			public byte tmItalic;

			// Token: 0x04002C7D RID: 11389
			public byte tmUnderlined;

			// Token: 0x04002C7E RID: 11390
			public byte tmStruckOut;

			// Token: 0x04002C7F RID: 11391
			public byte tmPitchAndFamily;

			// Token: 0x04002C80 RID: 11392
			public byte tmCharSet;
		}

		// Token: 0x02000505 RID: 1285
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NOTIFYICONDATA
		{
			// Token: 0x04002C81 RID: 11393
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.NOTIFYICONDATA));

			// Token: 0x04002C82 RID: 11394
			public IntPtr hWnd;

			// Token: 0x04002C83 RID: 11395
			public int uID;

			// Token: 0x04002C84 RID: 11396
			public int uFlags;

			// Token: 0x04002C85 RID: 11397
			public int uCallbackMessage;

			// Token: 0x04002C86 RID: 11398
			public IntPtr hIcon;

			// Token: 0x04002C87 RID: 11399
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szTip;

			// Token: 0x04002C88 RID: 11400
			public int dwState;

			// Token: 0x04002C89 RID: 11401
			public int dwStateMask;

			// Token: 0x04002C8A RID: 11402
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string szInfo;

			// Token: 0x04002C8B RID: 11403
			public int uTimeoutOrVersion;

			// Token: 0x04002C8C RID: 11404
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string szInfoTitle;

			// Token: 0x04002C8D RID: 11405
			public int dwInfoFlags;
		}

		// Token: 0x02000506 RID: 1286
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MENUITEMINFO_T
		{
			// Token: 0x04002C8E RID: 11406
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T));

			// Token: 0x04002C8F RID: 11407
			public int fMask;

			// Token: 0x04002C90 RID: 11408
			public int fType;

			// Token: 0x04002C91 RID: 11409
			public int fState;

			// Token: 0x04002C92 RID: 11410
			public int wID;

			// Token: 0x04002C93 RID: 11411
			public IntPtr hSubMenu;

			// Token: 0x04002C94 RID: 11412
			public IntPtr hbmpChecked;

			// Token: 0x04002C95 RID: 11413
			public IntPtr hbmpUnchecked;

			// Token: 0x04002C96 RID: 11414
			public IntPtr dwItemData;

			// Token: 0x04002C97 RID: 11415
			public string dwTypeData;

			// Token: 0x04002C98 RID: 11416
			public int cch;
		}

		// Token: 0x02000507 RID: 1287
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MENUITEMINFO_T_RW
		{
			// Token: 0x04002C99 RID: 11417
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T_RW));

			// Token: 0x04002C9A RID: 11418
			public int fMask;

			// Token: 0x04002C9B RID: 11419
			public int fType;

			// Token: 0x04002C9C RID: 11420
			public int fState;

			// Token: 0x04002C9D RID: 11421
			public int wID;

			// Token: 0x04002C9E RID: 11422
			public IntPtr hSubMenu = IntPtr.Zero;

			// Token: 0x04002C9F RID: 11423
			public IntPtr hbmpChecked = IntPtr.Zero;

			// Token: 0x04002CA0 RID: 11424
			public IntPtr hbmpUnchecked = IntPtr.Zero;

			// Token: 0x04002CA1 RID: 11425
			public IntPtr dwItemData = IntPtr.Zero;

			// Token: 0x04002CA2 RID: 11426
			public IntPtr dwTypeData = IntPtr.Zero;

			// Token: 0x04002CA3 RID: 11427
			public int cch;

			// Token: 0x04002CA4 RID: 11428
			public IntPtr hbmpItem = IntPtr.Zero;
		}

		// Token: 0x02000508 RID: 1288
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct MSAAMENUINFO
		{
			// Token: 0x060049D8 RID: 18904 RVA: 0x0010CFB6 File Offset: 0x0010BFB6
			public MSAAMENUINFO(string text)
			{
				this.dwMSAASignature = -1441927155;
				this.cchWText = text.Length;
				this.pszWText = text;
			}

			// Token: 0x04002CA5 RID: 11429
			public int dwMSAASignature;

			// Token: 0x04002CA6 RID: 11430
			public int cchWText;

			// Token: 0x04002CA7 RID: 11431
			public string pszWText;
		}

		// Token: 0x02000509 RID: 1289
		// (Invoke) Token: 0x060049DA RID: 18906
		public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

		// Token: 0x0200050A RID: 1290
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class OPENFILENAME_I
		{
			// Token: 0x04002CA8 RID: 11432
			public int lStructSize = Marshal.SizeOf(typeof(NativeMethods.OPENFILENAME_I));

			// Token: 0x04002CA9 RID: 11433
			public IntPtr hwndOwner;

			// Token: 0x04002CAA RID: 11434
			public IntPtr hInstance;

			// Token: 0x04002CAB RID: 11435
			public string lpstrFilter;

			// Token: 0x04002CAC RID: 11436
			public IntPtr lpstrCustomFilter = IntPtr.Zero;

			// Token: 0x04002CAD RID: 11437
			public int nMaxCustFilter;

			// Token: 0x04002CAE RID: 11438
			public int nFilterIndex;

			// Token: 0x04002CAF RID: 11439
			public IntPtr lpstrFile;

			// Token: 0x04002CB0 RID: 11440
			public int nMaxFile = 260;

			// Token: 0x04002CB1 RID: 11441
			public IntPtr lpstrFileTitle = IntPtr.Zero;

			// Token: 0x04002CB2 RID: 11442
			public int nMaxFileTitle = 260;

			// Token: 0x04002CB3 RID: 11443
			public string lpstrInitialDir;

			// Token: 0x04002CB4 RID: 11444
			public string lpstrTitle;

			// Token: 0x04002CB5 RID: 11445
			public int Flags;

			// Token: 0x04002CB6 RID: 11446
			public short nFileOffset;

			// Token: 0x04002CB7 RID: 11447
			public short nFileExtension;

			// Token: 0x04002CB8 RID: 11448
			public string lpstrDefExt;

			// Token: 0x04002CB9 RID: 11449
			public IntPtr lCustData = IntPtr.Zero;

			// Token: 0x04002CBA RID: 11450
			public NativeMethods.WndProc lpfnHook;

			// Token: 0x04002CBB RID: 11451
			public string lpTemplateName;

			// Token: 0x04002CBC RID: 11452
			public IntPtr pvReserved = IntPtr.Zero;

			// Token: 0x04002CBD RID: 11453
			public int dwReserved;

			// Token: 0x04002CBE RID: 11454
			public int FlagsEx;
		}

		// Token: 0x0200050B RID: 1291
		[CLSCompliant(false)]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class CHOOSEFONT
		{
			// Token: 0x04002CBF RID: 11455
			public int lStructSize = Marshal.SizeOf(typeof(NativeMethods.CHOOSEFONT));

			// Token: 0x04002CC0 RID: 11456
			public IntPtr hwndOwner;

			// Token: 0x04002CC1 RID: 11457
			public IntPtr hDC;

			// Token: 0x04002CC2 RID: 11458
			public IntPtr lpLogFont;

			// Token: 0x04002CC3 RID: 11459
			public int iPointSize;

			// Token: 0x04002CC4 RID: 11460
			public int Flags;

			// Token: 0x04002CC5 RID: 11461
			public int rgbColors;

			// Token: 0x04002CC6 RID: 11462
			public IntPtr lCustData = IntPtr.Zero;

			// Token: 0x04002CC7 RID: 11463
			public NativeMethods.WndProc lpfnHook;

			// Token: 0x04002CC8 RID: 11464
			public string lpTemplateName;

			// Token: 0x04002CC9 RID: 11465
			public IntPtr hInstance;

			// Token: 0x04002CCA RID: 11466
			public string lpszStyle;

			// Token: 0x04002CCB RID: 11467
			public short nFontType;

			// Token: 0x04002CCC RID: 11468
			public short ___MISSING_ALIGNMENT__;

			// Token: 0x04002CCD RID: 11469
			public int nSizeMin;

			// Token: 0x04002CCE RID: 11470
			public int nSizeMax;
		}

		// Token: 0x0200050C RID: 1292
		[StructLayout(LayoutKind.Sequential)]
		public class BITMAPINFO
		{
			// Token: 0x060049DF RID: 18911 RVA: 0x0010D06A File Offset: 0x0010C06A
			private BITMAPINFO()
			{
			}

			// Token: 0x04002CCF RID: 11471
			public int bmiHeader_biSize = 40;

			// Token: 0x04002CD0 RID: 11472
			public int bmiHeader_biWidth;

			// Token: 0x04002CD1 RID: 11473
			public int bmiHeader_biHeight;

			// Token: 0x04002CD2 RID: 11474
			public short bmiHeader_biPlanes;

			// Token: 0x04002CD3 RID: 11475
			public short bmiHeader_biBitCount;

			// Token: 0x04002CD4 RID: 11476
			public int bmiHeader_biCompression;

			// Token: 0x04002CD5 RID: 11477
			public int bmiHeader_biSizeImage;

			// Token: 0x04002CD6 RID: 11478
			public int bmiHeader_biXPelsPerMeter;

			// Token: 0x04002CD7 RID: 11479
			public int bmiHeader_biYPelsPerMeter;

			// Token: 0x04002CD8 RID: 11480
			public int bmiHeader_biClrUsed;

			// Token: 0x04002CD9 RID: 11481
			public int bmiHeader_biClrImportant;

			// Token: 0x04002CDA RID: 11482
			public byte bmiColors_rgbBlue;

			// Token: 0x04002CDB RID: 11483
			public byte bmiColors_rgbGreen;

			// Token: 0x04002CDC RID: 11484
			public byte bmiColors_rgbRed;

			// Token: 0x04002CDD RID: 11485
			public byte bmiColors_rgbReserved;
		}

		// Token: 0x0200050D RID: 1293
		[StructLayout(LayoutKind.Sequential)]
		public class BITMAPINFOHEADER
		{
			// Token: 0x04002CDE RID: 11486
			public int biSize = 40;

			// Token: 0x04002CDF RID: 11487
			public int biWidth;

			// Token: 0x04002CE0 RID: 11488
			public int biHeight;

			// Token: 0x04002CE1 RID: 11489
			public short biPlanes;

			// Token: 0x04002CE2 RID: 11490
			public short biBitCount;

			// Token: 0x04002CE3 RID: 11491
			public int biCompression;

			// Token: 0x04002CE4 RID: 11492
			public int biSizeImage;

			// Token: 0x04002CE5 RID: 11493
			public int biXPelsPerMeter;

			// Token: 0x04002CE6 RID: 11494
			public int biYPelsPerMeter;

			// Token: 0x04002CE7 RID: 11495
			public int biClrUsed;

			// Token: 0x04002CE8 RID: 11496
			public int biClrImportant;
		}

		// Token: 0x0200050E RID: 1294
		public class Ole
		{
			// Token: 0x04002CE9 RID: 11497
			public const int PICTYPE_UNINITIALIZED = -1;

			// Token: 0x04002CEA RID: 11498
			public const int PICTYPE_NONE = 0;

			// Token: 0x04002CEB RID: 11499
			public const int PICTYPE_BITMAP = 1;

			// Token: 0x04002CEC RID: 11500
			public const int PICTYPE_METAFILE = 2;

			// Token: 0x04002CED RID: 11501
			public const int PICTYPE_ICON = 3;

			// Token: 0x04002CEE RID: 11502
			public const int PICTYPE_ENHMETAFILE = 4;

			// Token: 0x04002CEF RID: 11503
			public const int STATFLAG_DEFAULT = 0;

			// Token: 0x04002CF0 RID: 11504
			public const int STATFLAG_NONAME = 1;
		}

		// Token: 0x0200050F RID: 1295
		[StructLayout(LayoutKind.Sequential)]
		public class STATSTG
		{
			// Token: 0x04002CF1 RID: 11505
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwcsName;

			// Token: 0x04002CF2 RID: 11506
			public int type;

			// Token: 0x04002CF3 RID: 11507
			[MarshalAs(UnmanagedType.I8)]
			public long cbSize;

			// Token: 0x04002CF4 RID: 11508
			[MarshalAs(UnmanagedType.I8)]
			public long mtime;

			// Token: 0x04002CF5 RID: 11509
			[MarshalAs(UnmanagedType.I8)]
			public long ctime;

			// Token: 0x04002CF6 RID: 11510
			[MarshalAs(UnmanagedType.I8)]
			public long atime;

			// Token: 0x04002CF7 RID: 11511
			[MarshalAs(UnmanagedType.I4)]
			public int grfMode;

			// Token: 0x04002CF8 RID: 11512
			[MarshalAs(UnmanagedType.I4)]
			public int grfLocksSupported;

			// Token: 0x04002CF9 RID: 11513
			public int clsid_data1;

			// Token: 0x04002CFA RID: 11514
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data2;

			// Token: 0x04002CFB RID: 11515
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data3;

			// Token: 0x04002CFC RID: 11516
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b0;

			// Token: 0x04002CFD RID: 11517
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b1;

			// Token: 0x04002CFE RID: 11518
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b2;

			// Token: 0x04002CFF RID: 11519
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b3;

			// Token: 0x04002D00 RID: 11520
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b4;

			// Token: 0x04002D01 RID: 11521
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b5;

			// Token: 0x04002D02 RID: 11522
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b6;

			// Token: 0x04002D03 RID: 11523
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b7;

			// Token: 0x04002D04 RID: 11524
			[MarshalAs(UnmanagedType.I4)]
			public int grfStateBits;

			// Token: 0x04002D05 RID: 11525
			[MarshalAs(UnmanagedType.I4)]
			public int reserved;
		}

		// Token: 0x02000510 RID: 1296
		[StructLayout(LayoutKind.Sequential)]
		public class FILETIME
		{
			// Token: 0x04002D06 RID: 11526
			public int dwLowDateTime;

			// Token: 0x04002D07 RID: 11527
			public int dwHighDateTime;
		}

		// Token: 0x02000511 RID: 1297
		[StructLayout(LayoutKind.Sequential)]
		public class SYSTEMTIME
		{
			// Token: 0x060049E4 RID: 18916 RVA: 0x0010D0A4 File Offset: 0x0010C0A4
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"[SYSTEMTIME: ",
					this.wDay.ToString(CultureInfo.InvariantCulture),
					"/",
					this.wMonth.ToString(CultureInfo.InvariantCulture),
					"/",
					this.wYear.ToString(CultureInfo.InvariantCulture),
					" ",
					this.wHour.ToString(CultureInfo.InvariantCulture),
					":",
					this.wMinute.ToString(CultureInfo.InvariantCulture),
					":",
					this.wSecond.ToString(CultureInfo.InvariantCulture),
					"]"
				});
			}

			// Token: 0x04002D08 RID: 11528
			public short wYear;

			// Token: 0x04002D09 RID: 11529
			public short wMonth;

			// Token: 0x04002D0A RID: 11530
			public short wDayOfWeek;

			// Token: 0x04002D0B RID: 11531
			public short wDay;

			// Token: 0x04002D0C RID: 11532
			public short wHour;

			// Token: 0x04002D0D RID: 11533
			public short wMinute;

			// Token: 0x04002D0E RID: 11534
			public short wSecond;

			// Token: 0x04002D0F RID: 11535
			public short wMilliseconds;
		}

		// Token: 0x02000512 RID: 1298
		[CLSCompliant(false)]
		[StructLayout(LayoutKind.Sequential)]
		public sealed class _POINTL
		{
			// Token: 0x04002D10 RID: 11536
			public int x;

			// Token: 0x04002D11 RID: 11537
			public int y;
		}

		// Token: 0x02000513 RID: 1299
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZE
		{
			// Token: 0x04002D12 RID: 11538
			public int cx;

			// Token: 0x04002D13 RID: 11539
			public int cy;
		}

		// Token: 0x02000514 RID: 1300
		[StructLayout(LayoutKind.Sequential)]
		public class COMRECT
		{
			// Token: 0x060049E8 RID: 18920 RVA: 0x0010D185 File Offset: 0x0010C185
			public COMRECT()
			{
			}

			// Token: 0x060049E9 RID: 18921 RVA: 0x0010D18D File Offset: 0x0010C18D
			public COMRECT(Rectangle r)
			{
				this.left = r.X;
				this.top = r.Y;
				this.right = r.Right;
				this.bottom = r.Bottom;
			}

			// Token: 0x060049EA RID: 18922 RVA: 0x0010D1C9 File Offset: 0x0010C1C9
			public COMRECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x060049EB RID: 18923 RVA: 0x0010D1EE File Offset: 0x0010C1EE
			public static NativeMethods.COMRECT FromXYWH(int x, int y, int width, int height)
			{
				return new NativeMethods.COMRECT(x, y, x + width, y + height);
			}

			// Token: 0x060049EC RID: 18924 RVA: 0x0010D200 File Offset: 0x0010C200
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"Left = ",
					this.left,
					" Top ",
					this.top,
					" Right = ",
					this.right,
					" Bottom = ",
					this.bottom
				});
			}

			// Token: 0x04002D14 RID: 11540
			public int left;

			// Token: 0x04002D15 RID: 11541
			public int top;

			// Token: 0x04002D16 RID: 11542
			public int right;

			// Token: 0x04002D17 RID: 11543
			public int bottom;
		}

		// Token: 0x02000515 RID: 1301
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagOleMenuGroupWidths
		{
			// Token: 0x04002D18 RID: 11544
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public int[] widths = new int[6];
		}

		// Token: 0x02000516 RID: 1302
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public class MSOCRINFOSTRUCT
		{
			// Token: 0x04002D19 RID: 11545
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.MSOCRINFOSTRUCT));

			// Token: 0x04002D1A RID: 11546
			public int uIdleTimeInterval;

			// Token: 0x04002D1B RID: 11547
			public int grfcrf;

			// Token: 0x04002D1C RID: 11548
			public int grfcadvf;
		}

		// Token: 0x02000517 RID: 1303
		public struct NMLISTVIEW
		{
			// Token: 0x04002D1D RID: 11549
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002D1E RID: 11550
			public int iItem;

			// Token: 0x04002D1F RID: 11551
			public int iSubItem;

			// Token: 0x04002D20 RID: 11552
			public int uNewState;

			// Token: 0x04002D21 RID: 11553
			public int uOldState;

			// Token: 0x04002D22 RID: 11554
			public int uChanged;

			// Token: 0x04002D23 RID: 11555
			public IntPtr lParam;
		}

		// Token: 0x02000518 RID: 1304
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagPOINTF
		{
			// Token: 0x04002D24 RID: 11556
			[MarshalAs(UnmanagedType.R4)]
			public float x;

			// Token: 0x04002D25 RID: 11557
			[MarshalAs(UnmanagedType.R4)]
			public float y;
		}

		// Token: 0x02000519 RID: 1305
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagOIFI
		{
			// Token: 0x04002D26 RID: 11558
			[MarshalAs(UnmanagedType.U4)]
			public int cb;

			// Token: 0x04002D27 RID: 11559
			public bool fMDIApp;

			// Token: 0x04002D28 RID: 11560
			public IntPtr hwndFrame;

			// Token: 0x04002D29 RID: 11561
			public IntPtr hAccel;

			// Token: 0x04002D2A RID: 11562
			[MarshalAs(UnmanagedType.U4)]
			public int cAccelEntries;
		}

		// Token: 0x0200051A RID: 1306
		public struct NMLVFINDITEM
		{
			// Token: 0x04002D2B RID: 11563
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002D2C RID: 11564
			public int iStart;

			// Token: 0x04002D2D RID: 11565
			public NativeMethods.LVFINDINFO lvfi;
		}

		// Token: 0x0200051B RID: 1307
		public struct NMHDR
		{
			// Token: 0x04002D2E RID: 11566
			public IntPtr hwndFrom;

			// Token: 0x04002D2F RID: 11567
			public IntPtr idFrom;

			// Token: 0x04002D30 RID: 11568
			public int code;
		}

		// Token: 0x0200051C RID: 1308
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
		internal interface IHTMLDocument
		{
			// Token: 0x060049F1 RID: 18929
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScript();
		}

		// Token: 0x0200051D RID: 1309
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("376BD3AA-3845-101B-84ED-08002B2EC713")]
		[ComImport]
		public interface IPerPropertyBrowsing
		{
			// Token: 0x060049F2 RID: 18930
			[PreserveSig]
			int GetDisplayString(int dispID, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstr);

			// Token: 0x060049F3 RID: 18931
			[PreserveSig]
			int MapPropertyToPage(int dispID, out Guid pGuid);

			// Token: 0x060049F4 RID: 18932
			[PreserveSig]
			int GetPredefinedStrings(int dispID, [Out] NativeMethods.CA_STRUCT pCaStringsOut, [Out] NativeMethods.CA_STRUCT pCaCookiesOut);

			// Token: 0x060049F5 RID: 18933
			[PreserveSig]
			int GetPredefinedValue(int dispID, [MarshalAs(UnmanagedType.U4)] [In] int dwCookie, [Out] NativeMethods.VARIANT pVarOut);
		}

		// Token: 0x0200051E RID: 1310
		[Guid("4D07FC10-F931-11CE-B001-00AA006884E5")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ICategorizeProperties
		{
			// Token: 0x060049F6 RID: 18934
			[PreserveSig]
			int MapPropertyToCategory(int dispID, ref int categoryID);

			// Token: 0x060049F7 RID: 18935
			[PreserveSig]
			int GetCategoryName(int propcat, [MarshalAs(UnmanagedType.U4)] [In] int lcid, out string categoryName);
		}

		// Token: 0x0200051F RID: 1311
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZEL
		{
			// Token: 0x04002D31 RID: 11569
			public int cx;

			// Token: 0x04002D32 RID: 11570
			public int cy;
		}

		// Token: 0x02000520 RID: 1312
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagOLEVERB
		{
			// Token: 0x04002D33 RID: 11571
			public int lVerb;

			// Token: 0x04002D34 RID: 11572
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszVerbName;

			// Token: 0x04002D35 RID: 11573
			[MarshalAs(UnmanagedType.U4)]
			public int fuFlags;

			// Token: 0x04002D36 RID: 11574
			[MarshalAs(UnmanagedType.U4)]
			public int grfAttribs;
		}

		// Token: 0x02000521 RID: 1313
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagLOGPALETTE
		{
			// Token: 0x04002D37 RID: 11575
			[MarshalAs(UnmanagedType.U2)]
			public short palVersion;

			// Token: 0x04002D38 RID: 11576
			[MarshalAs(UnmanagedType.U2)]
			public short palNumEntries;
		}

		// Token: 0x02000522 RID: 1314
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagCONTROLINFO
		{
			// Token: 0x04002D39 RID: 11577
			[MarshalAs(UnmanagedType.U4)]
			public int cb = Marshal.SizeOf(typeof(NativeMethods.tagCONTROLINFO));

			// Token: 0x04002D3A RID: 11578
			public IntPtr hAccel;

			// Token: 0x04002D3B RID: 11579
			[MarshalAs(UnmanagedType.U2)]
			public short cAccel;

			// Token: 0x04002D3C RID: 11580
			[MarshalAs(UnmanagedType.U4)]
			public int dwFlags;
		}

		// Token: 0x02000523 RID: 1315
		[StructLayout(LayoutKind.Sequential)]
		public sealed class CA_STRUCT
		{
			// Token: 0x04002D3D RID: 11581
			public int cElems;

			// Token: 0x04002D3E RID: 11582
			public IntPtr pElems = IntPtr.Zero;
		}

		// Token: 0x02000524 RID: 1316
		[StructLayout(LayoutKind.Sequential)]
		public sealed class VARIANT
		{
			// Token: 0x17000EAF RID: 3759
			// (get) Token: 0x060049FD RID: 18941 RVA: 0x0010D2FB File Offset: 0x0010C2FB
			public bool Byref
			{
				get
				{
					return 0 != (this.vt & 16384);
				}
			}

			// Token: 0x060049FE RID: 18942 RVA: 0x0010D310 File Offset: 0x0010C310
			public void Clear()
			{
				if ((this.vt == 13 || this.vt == 9) && this.data1 != IntPtr.Zero)
				{
					Marshal.Release(this.data1);
				}
				if (this.vt == 8 && this.data1 != IntPtr.Zero)
				{
					NativeMethods.VARIANT.SysFreeString(this.data1);
				}
				this.data1 = (this.data2 = IntPtr.Zero);
				this.vt = 0;
			}

			// Token: 0x060049FF RID: 18943 RVA: 0x0010D390 File Offset: 0x0010C390
			~VARIANT()
			{
				this.Clear();
			}

			// Token: 0x06004A00 RID: 18944 RVA: 0x0010D3BC File Offset: 0x0010C3BC
			public static NativeMethods.VARIANT FromObject(object var)
			{
				NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
				if (var == null)
				{
					variant.vt = 0;
				}
				else if (!Convert.IsDBNull(var))
				{
					Type type = var.GetType();
					if (type == typeof(bool))
					{
						variant.vt = 11;
					}
					else if (type == typeof(byte))
					{
						variant.vt = 17;
						variant.data1 = (IntPtr)((int)Convert.ToByte(var, CultureInfo.InvariantCulture));
					}
					else if (type == typeof(char))
					{
						variant.vt = 18;
						variant.data1 = (IntPtr)((int)Convert.ToChar(var, CultureInfo.InvariantCulture));
					}
					else if (type == typeof(string))
					{
						variant.vt = 8;
						variant.data1 = NativeMethods.VARIANT.SysAllocString(Convert.ToString(var, CultureInfo.InvariantCulture));
					}
					else if (type == typeof(short))
					{
						variant.vt = 2;
						variant.data1 = (IntPtr)((int)Convert.ToInt16(var, CultureInfo.InvariantCulture));
					}
					else if (type == typeof(int))
					{
						variant.vt = 3;
						variant.data1 = (IntPtr)Convert.ToInt32(var, CultureInfo.InvariantCulture);
					}
					else if (type == typeof(long))
					{
						variant.vt = 20;
						variant.SetLong(Convert.ToInt64(var, CultureInfo.InvariantCulture));
					}
					else if (type == typeof(decimal))
					{
						variant.vt = 6;
						decimal d = (decimal)var;
						variant.SetLong(decimal.ToInt64(d));
					}
					else if (type == typeof(decimal))
					{
						variant.vt = 14;
						decimal d2 = Convert.ToDecimal(var, CultureInfo.InvariantCulture);
						variant.SetLong(decimal.ToInt64(d2));
					}
					else if (type == typeof(double))
					{
						variant.vt = 5;
					}
					else if (type == typeof(float) || type == typeof(float))
					{
						variant.vt = 4;
					}
					else if (type == typeof(DateTime))
					{
						variant.vt = 7;
						variant.SetLong(Convert.ToDateTime(var, CultureInfo.InvariantCulture).ToFileTime());
					}
					else if (type == typeof(sbyte))
					{
						variant.vt = 16;
						variant.data1 = (IntPtr)((int)Convert.ToSByte(var, CultureInfo.InvariantCulture));
					}
					else if (type == typeof(ushort))
					{
						variant.vt = 18;
						variant.data1 = (IntPtr)((int)Convert.ToUInt16(var, CultureInfo.InvariantCulture));
					}
					else if (type == typeof(uint))
					{
						variant.vt = 19;
						variant.data1 = (IntPtr)((long)((ulong)Convert.ToUInt32(var, CultureInfo.InvariantCulture)));
					}
					else if (type == typeof(ulong))
					{
						variant.vt = 21;
						variant.SetLong((long)Convert.ToUInt64(var, CultureInfo.InvariantCulture));
					}
					else
					{
						if (type != typeof(object) && type != typeof(UnsafeNativeMethods.IDispatch) && !type.IsCOMObject)
						{
							throw new ArgumentException(SR.GetString("ConnPointUnhandledType", new object[]
							{
								type.Name
							}));
						}
						variant.vt = ((type == typeof(UnsafeNativeMethods.IDispatch)) ? 9 : 13);
						variant.data1 = Marshal.GetIUnknownForObject(var);
					}
				}
				return variant;
			}

			// Token: 0x06004A01 RID: 18945
			[DllImport("oleaut32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SysAllocString([MarshalAs(UnmanagedType.LPWStr)] [In] string s);

			// Token: 0x06004A02 RID: 18946
			[DllImport("oleaut32.dll", CharSet = CharSet.Auto)]
			private static extern void SysFreeString(IntPtr pbstr);

			// Token: 0x06004A03 RID: 18947 RVA: 0x0010D719 File Offset: 0x0010C719
			public void SetLong(long lVal)
			{
				this.data1 = (IntPtr)(lVal & (long)((ulong)-1));
				this.data2 = (IntPtr)(lVal >> 32 & (long)((ulong)-1));
			}

			// Token: 0x06004A04 RID: 18948 RVA: 0x0010D73C File Offset: 0x0010C73C
			public IntPtr ToCoTaskMemPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(16);
				Marshal.WriteInt16(intPtr, this.vt);
				Marshal.WriteInt16(intPtr, 2, this.reserved1);
				Marshal.WriteInt16(intPtr, 4, this.reserved2);
				Marshal.WriteInt16(intPtr, 6, this.reserved3);
				Marshal.WriteInt32(intPtr, 8, (int)this.data1);
				Marshal.WriteInt32(intPtr, 12, (int)this.data2);
				return intPtr;
			}

			// Token: 0x06004A05 RID: 18949 RVA: 0x0010D7AC File Offset: 0x0010C7AC
			public object ToObject()
			{
				IntPtr intPtr = this.data1;
				int num = (int)(this.vt & 4095);
				int num2 = num;
				switch (num2)
				{
				case 0:
					return null;
				case 1:
					return Convert.DBNull;
				case 2:
					if (this.Byref)
					{
						intPtr = (IntPtr)((int)Marshal.ReadInt16(intPtr));
					}
					return (short)(65535 & (int)((short)((int)intPtr)));
				case 3:
					break;
				default:
					switch (num2)
					{
					case 16:
						if (this.Byref)
						{
							intPtr = (IntPtr)((int)Marshal.ReadByte(intPtr));
						}
						return (sbyte)(255 & (int)((sbyte)((int)intPtr)));
					case 17:
						if (this.Byref)
						{
							intPtr = (IntPtr)((int)Marshal.ReadByte(intPtr));
						}
						return byte.MaxValue & (byte)((int)intPtr);
					case 18:
						if (this.Byref)
						{
							intPtr = (IntPtr)((int)Marshal.ReadInt16(intPtr));
						}
						return ushort.MaxValue & (ushort)((int)intPtr);
					case 19:
					case 23:
						if (this.Byref)
						{
							intPtr = (IntPtr)Marshal.ReadInt32(intPtr);
						}
						return (uint)((int)intPtr);
					case 20:
					case 21:
					{
						long num3;
						if (this.Byref)
						{
							num3 = Marshal.ReadInt64(intPtr);
						}
						else
						{
							num3 = (long)((ulong)(((int)this.data1 & -1) | (int)this.data2));
						}
						if (this.vt == 20)
						{
							return num3;
						}
						return (ulong)num3;
					}
					case 22:
						break;
					default:
					{
						if (this.Byref)
						{
							intPtr = NativeMethods.VARIANT.GetRefInt(intPtr);
						}
						int num4 = num;
						if (num4 <= 72)
						{
							switch (num4)
							{
							case 4:
							case 5:
								throw new FormatException(SR.GetString("CannotConvertIntToFloat"));
							case 6:
							{
								long num3 = (long)((ulong)(((int)this.data1 & -1) | (int)this.data2));
								return new decimal(num3);
							}
							case 7:
								throw new FormatException(SR.GetString("CannotConvertDoubleToDate"));
							case 8:
							case 31:
								return Marshal.PtrToStringUni(intPtr);
							case 9:
							case 13:
								return Marshal.GetObjectForIUnknown(intPtr);
							case 10:
							case 15:
							case 16:
							case 17:
							case 18:
							case 19:
							case 20:
							case 21:
							case 22:
							case 23:
							case 24:
							case 26:
							case 27:
							case 28:
							case 32:
							case 33:
							case 34:
							case 35:
							case 36:
								break;
							case 11:
								return intPtr != IntPtr.Zero;
							case 12:
							{
								NativeMethods.VARIANT variant = (NativeMethods.VARIANT)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(NativeMethods.VARIANT));
								return variant.ToObject();
							}
							case 14:
							{
								long num3 = (long)((ulong)(((int)this.data1 & -1) | (int)this.data2));
								return new decimal(num3);
							}
							case 25:
								return intPtr;
							case 29:
								throw new ArgumentException(SR.GetString("COM2UnhandledVT", new object[]
								{
									"VT_USERDEFINED"
								}));
							case 30:
								return Marshal.PtrToStringAnsi(intPtr);
							default:
								switch (num4)
								{
								case 64:
								{
									long num3 = (long)((ulong)(((int)this.data1 & -1) | (int)this.data2));
									return new DateTime(num3);
								}
								case 72:
								{
									Guid guid = (Guid)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(Guid));
									return guid;
								}
								}
								break;
							}
						}
						else
						{
							switch (num4)
							{
							case 4095:
							case 4096:
								break;
							default:
								if (num4 != 8192 && num4 != 16384)
								{
								}
								break;
							}
						}
						int num5 = (int)this.vt;
						throw new ArgumentException(SR.GetString("COM2UnhandledVT", new object[]
						{
							num5.ToString(CultureInfo.InvariantCulture)
						}));
					}
					}
					break;
				}
				if (this.Byref)
				{
					intPtr = (IntPtr)Marshal.ReadInt32(intPtr);
				}
				return (int)intPtr;
			}

			// Token: 0x06004A06 RID: 18950 RVA: 0x0010DBA1 File Offset: 0x0010CBA1
			private static IntPtr GetRefInt(IntPtr value)
			{
				return Marshal.ReadIntPtr(value);
			}

			// Token: 0x04002D3F RID: 11583
			[MarshalAs(UnmanagedType.I2)]
			public short vt;

			// Token: 0x04002D40 RID: 11584
			[MarshalAs(UnmanagedType.I2)]
			public short reserved1;

			// Token: 0x04002D41 RID: 11585
			[MarshalAs(UnmanagedType.I2)]
			public short reserved2;

			// Token: 0x04002D42 RID: 11586
			[MarshalAs(UnmanagedType.I2)]
			public short reserved3;

			// Token: 0x04002D43 RID: 11587
			public IntPtr data1;

			// Token: 0x04002D44 RID: 11588
			public IntPtr data2;
		}

		// Token: 0x02000525 RID: 1317
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagLICINFO
		{
			// Token: 0x04002D45 RID: 11589
			[MarshalAs(UnmanagedType.U4)]
			public int cbLicInfo = Marshal.SizeOf(typeof(NativeMethods.tagLICINFO));

			// Token: 0x04002D46 RID: 11590
			public int fRuntimeAvailable;

			// Token: 0x04002D47 RID: 11591
			public int fLicVerified;
		}

		// Token: 0x02000526 RID: 1318
		public enum tagVT
		{
			// Token: 0x04002D49 RID: 11593
			VT_EMPTY,
			// Token: 0x04002D4A RID: 11594
			VT_NULL,
			// Token: 0x04002D4B RID: 11595
			VT_I2,
			// Token: 0x04002D4C RID: 11596
			VT_I4,
			// Token: 0x04002D4D RID: 11597
			VT_R4,
			// Token: 0x04002D4E RID: 11598
			VT_R8,
			// Token: 0x04002D4F RID: 11599
			VT_CY,
			// Token: 0x04002D50 RID: 11600
			VT_DATE,
			// Token: 0x04002D51 RID: 11601
			VT_BSTR,
			// Token: 0x04002D52 RID: 11602
			VT_DISPATCH,
			// Token: 0x04002D53 RID: 11603
			VT_ERROR,
			// Token: 0x04002D54 RID: 11604
			VT_BOOL,
			// Token: 0x04002D55 RID: 11605
			VT_VARIANT,
			// Token: 0x04002D56 RID: 11606
			VT_UNKNOWN,
			// Token: 0x04002D57 RID: 11607
			VT_DECIMAL,
			// Token: 0x04002D58 RID: 11608
			VT_I1 = 16,
			// Token: 0x04002D59 RID: 11609
			VT_UI1,
			// Token: 0x04002D5A RID: 11610
			VT_UI2,
			// Token: 0x04002D5B RID: 11611
			VT_UI4,
			// Token: 0x04002D5C RID: 11612
			VT_I8,
			// Token: 0x04002D5D RID: 11613
			VT_UI8,
			// Token: 0x04002D5E RID: 11614
			VT_INT,
			// Token: 0x04002D5F RID: 11615
			VT_UINT,
			// Token: 0x04002D60 RID: 11616
			VT_VOID,
			// Token: 0x04002D61 RID: 11617
			VT_HRESULT,
			// Token: 0x04002D62 RID: 11618
			VT_PTR,
			// Token: 0x04002D63 RID: 11619
			VT_SAFEARRAY,
			// Token: 0x04002D64 RID: 11620
			VT_CARRAY,
			// Token: 0x04002D65 RID: 11621
			VT_USERDEFINED,
			// Token: 0x04002D66 RID: 11622
			VT_LPSTR,
			// Token: 0x04002D67 RID: 11623
			VT_LPWSTR,
			// Token: 0x04002D68 RID: 11624
			VT_RECORD = 36,
			// Token: 0x04002D69 RID: 11625
			VT_FILETIME = 64,
			// Token: 0x04002D6A RID: 11626
			VT_BLOB,
			// Token: 0x04002D6B RID: 11627
			VT_STREAM,
			// Token: 0x04002D6C RID: 11628
			VT_STORAGE,
			// Token: 0x04002D6D RID: 11629
			VT_STREAMED_OBJECT,
			// Token: 0x04002D6E RID: 11630
			VT_STORED_OBJECT,
			// Token: 0x04002D6F RID: 11631
			VT_BLOB_OBJECT,
			// Token: 0x04002D70 RID: 11632
			VT_CF,
			// Token: 0x04002D71 RID: 11633
			VT_CLSID,
			// Token: 0x04002D72 RID: 11634
			VT_BSTR_BLOB = 4095,
			// Token: 0x04002D73 RID: 11635
			VT_VECTOR,
			// Token: 0x04002D74 RID: 11636
			VT_ARRAY = 8192,
			// Token: 0x04002D75 RID: 11637
			VT_BYREF = 16384,
			// Token: 0x04002D76 RID: 11638
			VT_RESERVED = 32768,
			// Token: 0x04002D77 RID: 11639
			VT_ILLEGAL = 65535,
			// Token: 0x04002D78 RID: 11640
			VT_ILLEGALMASKED = 4095,
			// Token: 0x04002D79 RID: 11641
			VT_TYPEMASK = 4095
		}

		// Token: 0x02000527 RID: 1319
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class WNDCLASS_D
		{
			// Token: 0x04002D7A RID: 11642
			public int style;

			// Token: 0x04002D7B RID: 11643
			public NativeMethods.WndProc lpfnWndProc;

			// Token: 0x04002D7C RID: 11644
			public int cbClsExtra;

			// Token: 0x04002D7D RID: 11645
			public int cbWndExtra;

			// Token: 0x04002D7E RID: 11646
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x04002D7F RID: 11647
			public IntPtr hIcon = IntPtr.Zero;

			// Token: 0x04002D80 RID: 11648
			public IntPtr hCursor = IntPtr.Zero;

			// Token: 0x04002D81 RID: 11649
			public IntPtr hbrBackground = IntPtr.Zero;

			// Token: 0x04002D82 RID: 11650
			public string lpszMenuName;

			// Token: 0x04002D83 RID: 11651
			public string lpszClassName;
		}

		// Token: 0x02000528 RID: 1320
		public class MSOCM
		{
			// Token: 0x04002D84 RID: 11652
			public const int msocrfNeedIdleTime = 1;

			// Token: 0x04002D85 RID: 11653
			public const int msocrfNeedPeriodicIdleTime = 2;

			// Token: 0x04002D86 RID: 11654
			public const int msocrfPreTranslateKeys = 4;

			// Token: 0x04002D87 RID: 11655
			public const int msocrfPreTranslateAll = 8;

			// Token: 0x04002D88 RID: 11656
			public const int msocrfNeedSpecActiveNotifs = 16;

			// Token: 0x04002D89 RID: 11657
			public const int msocrfNeedAllActiveNotifs = 32;

			// Token: 0x04002D8A RID: 11658
			public const int msocrfExclusiveBorderSpace = 64;

			// Token: 0x04002D8B RID: 11659
			public const int msocrfExclusiveActivation = 128;

			// Token: 0x04002D8C RID: 11660
			public const int msocrfNeedAllMacEvents = 256;

			// Token: 0x04002D8D RID: 11661
			public const int msocrfMaster = 512;

			// Token: 0x04002D8E RID: 11662
			public const int msocadvfModal = 1;

			// Token: 0x04002D8F RID: 11663
			public const int msocadvfRedrawOff = 2;

			// Token: 0x04002D90 RID: 11664
			public const int msocadvfWarningsOff = 4;

			// Token: 0x04002D91 RID: 11665
			public const int msocadvfRecording = 8;

			// Token: 0x04002D92 RID: 11666
			public const int msochostfExclusiveBorderSpace = 1;

			// Token: 0x04002D93 RID: 11667
			public const int msoidlefPeriodic = 1;

			// Token: 0x04002D94 RID: 11668
			public const int msoidlefNonPeriodic = 2;

			// Token: 0x04002D95 RID: 11669
			public const int msoidlefPriority = 4;

			// Token: 0x04002D96 RID: 11670
			public const int msoidlefAll = -1;

			// Token: 0x04002D97 RID: 11671
			public const int msoloopDoEventsModal = -2;

			// Token: 0x04002D98 RID: 11672
			public const int msoloopMain = -1;

			// Token: 0x04002D99 RID: 11673
			public const int msoloopFocusWait = 1;

			// Token: 0x04002D9A RID: 11674
			public const int msoloopDoEvents = 2;

			// Token: 0x04002D9B RID: 11675
			public const int msoloopDebug = 3;

			// Token: 0x04002D9C RID: 11676
			public const int msoloopModalForm = 4;

			// Token: 0x04002D9D RID: 11677
			public const int msoloopModalAlert = 5;

			// Token: 0x04002D9E RID: 11678
			public const int msocstateModal = 1;

			// Token: 0x04002D9F RID: 11679
			public const int msocstateRedrawOff = 2;

			// Token: 0x04002DA0 RID: 11680
			public const int msocstateWarningsOff = 3;

			// Token: 0x04002DA1 RID: 11681
			public const int msocstateRecording = 4;

			// Token: 0x04002DA2 RID: 11682
			public const int msoccontextAll = 0;

			// Token: 0x04002DA3 RID: 11683
			public const int msoccontextMine = 1;

			// Token: 0x04002DA4 RID: 11684
			public const int msoccontextOthers = 2;

			// Token: 0x04002DA5 RID: 11685
			public const int msogacActive = 0;

			// Token: 0x04002DA6 RID: 11686
			public const int msogacTracking = 1;

			// Token: 0x04002DA7 RID: 11687
			public const int msogacTrackingOrActive = 2;

			// Token: 0x04002DA8 RID: 11688
			public const int msocWindowFrameToplevel = 0;

			// Token: 0x04002DA9 RID: 11689
			public const int msocWindowFrameOwner = 1;

			// Token: 0x04002DAA RID: 11690
			public const int msocWindowComponent = 2;

			// Token: 0x04002DAB RID: 11691
			public const int msocWindowDlgOwner = 3;
		}

		// Token: 0x02000529 RID: 1321
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TOOLINFO_T
		{
			// Token: 0x04002DAC RID: 11692
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));

			// Token: 0x04002DAD RID: 11693
			public int uFlags;

			// Token: 0x04002DAE RID: 11694
			public IntPtr hwnd;

			// Token: 0x04002DAF RID: 11695
			public IntPtr uId;

			// Token: 0x04002DB0 RID: 11696
			public NativeMethods.RECT rect;

			// Token: 0x04002DB1 RID: 11697
			public IntPtr hinst = IntPtr.Zero;

			// Token: 0x04002DB2 RID: 11698
			public string lpszText;

			// Token: 0x04002DB3 RID: 11699
			public IntPtr lParam = IntPtr.Zero;
		}

		// Token: 0x0200052A RID: 1322
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TOOLINFO_TOOLTIP
		{
			// Token: 0x04002DB4 RID: 11700
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));

			// Token: 0x04002DB5 RID: 11701
			public int uFlags;

			// Token: 0x04002DB6 RID: 11702
			public IntPtr hwnd;

			// Token: 0x04002DB7 RID: 11703
			public IntPtr uId;

			// Token: 0x04002DB8 RID: 11704
			public NativeMethods.RECT rect;

			// Token: 0x04002DB9 RID: 11705
			public IntPtr hinst = IntPtr.Zero;

			// Token: 0x04002DBA RID: 11706
			public IntPtr lpszText;

			// Token: 0x04002DBB RID: 11707
			public IntPtr lParam = IntPtr.Zero;
		}

		// Token: 0x0200052B RID: 1323
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagDVTARGETDEVICE
		{
			// Token: 0x04002DBC RID: 11708
			[MarshalAs(UnmanagedType.U4)]
			public int tdSize;

			// Token: 0x04002DBD RID: 11709
			[MarshalAs(UnmanagedType.U2)]
			public short tdDriverNameOffset;

			// Token: 0x04002DBE RID: 11710
			[MarshalAs(UnmanagedType.U2)]
			public short tdDeviceNameOffset;

			// Token: 0x04002DBF RID: 11711
			[MarshalAs(UnmanagedType.U2)]
			public short tdPortNameOffset;

			// Token: 0x04002DC0 RID: 11712
			[MarshalAs(UnmanagedType.U2)]
			public short tdExtDevmodeOffset;
		}

		// Token: 0x0200052C RID: 1324
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct TV_ITEM
		{
			// Token: 0x04002DC1 RID: 11713
			public int mask;

			// Token: 0x04002DC2 RID: 11714
			public IntPtr hItem;

			// Token: 0x04002DC3 RID: 11715
			public int state;

			// Token: 0x04002DC4 RID: 11716
			public int stateMask;

			// Token: 0x04002DC5 RID: 11717
			public IntPtr pszText;

			// Token: 0x04002DC6 RID: 11718
			public int cchTextMax;

			// Token: 0x04002DC7 RID: 11719
			public int iImage;

			// Token: 0x04002DC8 RID: 11720
			public int iSelectedImage;

			// Token: 0x04002DC9 RID: 11721
			public int cChildren;

			// Token: 0x04002DCA RID: 11722
			public IntPtr lParam;
		}

		// Token: 0x0200052D RID: 1325
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct TVSORTCB
		{
			// Token: 0x04002DCB RID: 11723
			public IntPtr hParent;

			// Token: 0x04002DCC RID: 11724
			public NativeMethods.TreeViewCompareCallback lpfnCompare;

			// Token: 0x04002DCD RID: 11725
			public IntPtr lParam;
		}

		// Token: 0x0200052E RID: 1326
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct TV_INSERTSTRUCT
		{
			// Token: 0x04002DCE RID: 11726
			public IntPtr hParent;

			// Token: 0x04002DCF RID: 11727
			public IntPtr hInsertAfter;

			// Token: 0x04002DD0 RID: 11728
			public int item_mask;

			// Token: 0x04002DD1 RID: 11729
			public IntPtr item_hItem;

			// Token: 0x04002DD2 RID: 11730
			public int item_state;

			// Token: 0x04002DD3 RID: 11731
			public int item_stateMask;

			// Token: 0x04002DD4 RID: 11732
			public IntPtr item_pszText;

			// Token: 0x04002DD5 RID: 11733
			public int item_cchTextMax;

			// Token: 0x04002DD6 RID: 11734
			public int item_iImage;

			// Token: 0x04002DD7 RID: 11735
			public int item_iSelectedImage;

			// Token: 0x04002DD8 RID: 11736
			public int item_cChildren;

			// Token: 0x04002DD9 RID: 11737
			public IntPtr item_lParam;

			// Token: 0x04002DDA RID: 11738
			public int item_iIntegral;
		}

		// Token: 0x0200052F RID: 1327
		public struct NMTREEVIEW
		{
			// Token: 0x04002DDB RID: 11739
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002DDC RID: 11740
			public int action;

			// Token: 0x04002DDD RID: 11741
			public NativeMethods.TV_ITEM itemOld;

			// Token: 0x04002DDE RID: 11742
			public NativeMethods.TV_ITEM itemNew;

			// Token: 0x04002DDF RID: 11743
			public int ptDrag_X;

			// Token: 0x04002DE0 RID: 11744
			public int ptDrag_Y;
		}

		// Token: 0x02000530 RID: 1328
		public struct NMTVGETINFOTIP
		{
			// Token: 0x04002DE1 RID: 11745
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002DE2 RID: 11746
			public string pszText;

			// Token: 0x04002DE3 RID: 11747
			public int cchTextMax;

			// Token: 0x04002DE4 RID: 11748
			public IntPtr item;

			// Token: 0x04002DE5 RID: 11749
			public IntPtr lParam;
		}

		// Token: 0x02000531 RID: 1329
		[StructLayout(LayoutKind.Sequential)]
		public class NMTVDISPINFO
		{
			// Token: 0x04002DE6 RID: 11750
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002DE7 RID: 11751
			public NativeMethods.TV_ITEM item;
		}

		// Token: 0x02000532 RID: 1330
		[StructLayout(LayoutKind.Sequential)]
		public sealed class POINTL
		{
			// Token: 0x04002DE8 RID: 11752
			public int x;

			// Token: 0x04002DE9 RID: 11753
			public int y;
		}

		// Token: 0x02000533 RID: 1331
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct HIGHCONTRAST
		{
			// Token: 0x04002DEA RID: 11754
			public int cbSize;

			// Token: 0x04002DEB RID: 11755
			public int dwFlags;

			// Token: 0x04002DEC RID: 11756
			public string lpszDefaultScheme;
		}

		// Token: 0x02000534 RID: 1332
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct HIGHCONTRAST_I
		{
			// Token: 0x04002DED RID: 11757
			public int cbSize;

			// Token: 0x04002DEE RID: 11758
			public int dwFlags;

			// Token: 0x04002DEF RID: 11759
			public IntPtr lpszDefaultScheme;
		}

		// Token: 0x02000535 RID: 1333
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TCITEM_T
		{
			// Token: 0x04002DF0 RID: 11760
			public int mask;

			// Token: 0x04002DF1 RID: 11761
			public int dwState;

			// Token: 0x04002DF2 RID: 11762
			public int dwStateMask;

			// Token: 0x04002DF3 RID: 11763
			public string pszText;

			// Token: 0x04002DF4 RID: 11764
			public int cchTextMax;

			// Token: 0x04002DF5 RID: 11765
			public int iImage;

			// Token: 0x04002DF6 RID: 11766
			public IntPtr lParam;
		}

		// Token: 0x02000536 RID: 1334
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagDISPPARAMS
		{
			// Token: 0x04002DF7 RID: 11767
			public IntPtr rgvarg;

			// Token: 0x04002DF8 RID: 11768
			public IntPtr rgdispidNamedArgs;

			// Token: 0x04002DF9 RID: 11769
			[MarshalAs(UnmanagedType.U4)]
			public int cArgs;

			// Token: 0x04002DFA RID: 11770
			[MarshalAs(UnmanagedType.U4)]
			public int cNamedArgs;
		}

		// Token: 0x02000537 RID: 1335
		public enum tagINVOKEKIND
		{
			// Token: 0x04002DFC RID: 11772
			INVOKE_FUNC = 1,
			// Token: 0x04002DFD RID: 11773
			INVOKE_PROPERTYGET,
			// Token: 0x04002DFE RID: 11774
			INVOKE_PROPERTYPUT = 4,
			// Token: 0x04002DFF RID: 11775
			INVOKE_PROPERTYPUTREF = 8
		}

		// Token: 0x02000538 RID: 1336
		[StructLayout(LayoutKind.Sequential)]
		public class tagEXCEPINFO
		{
			// Token: 0x04002E00 RID: 11776
			[MarshalAs(UnmanagedType.U2)]
			public short wCode;

			// Token: 0x04002E01 RID: 11777
			[MarshalAs(UnmanagedType.U2)]
			public short wReserved;

			// Token: 0x04002E02 RID: 11778
			[MarshalAs(UnmanagedType.BStr)]
			public string bstrSource;

			// Token: 0x04002E03 RID: 11779
			[MarshalAs(UnmanagedType.BStr)]
			public string bstrDescription;

			// Token: 0x04002E04 RID: 11780
			[MarshalAs(UnmanagedType.BStr)]
			public string bstrHelpFile;

			// Token: 0x04002E05 RID: 11781
			[MarshalAs(UnmanagedType.U4)]
			public int dwHelpContext;

			// Token: 0x04002E06 RID: 11782
			public IntPtr pvReserved = IntPtr.Zero;

			// Token: 0x04002E07 RID: 11783
			public IntPtr pfnDeferredFillIn = IntPtr.Zero;

			// Token: 0x04002E08 RID: 11784
			[MarshalAs(UnmanagedType.U4)]
			public int scode;
		}

		// Token: 0x02000539 RID: 1337
		public enum tagDESCKIND
		{
			// Token: 0x04002E0A RID: 11786
			DESCKIND_NONE,
			// Token: 0x04002E0B RID: 11787
			DESCKIND_FUNCDESC,
			// Token: 0x04002E0C RID: 11788
			DESCKIND_VARDESC,
			// Token: 0x04002E0D RID: 11789
			DESCKIND_TYPECOMP,
			// Token: 0x04002E0E RID: 11790
			DESCKIND_IMPLICITAPPOBJ,
			// Token: 0x04002E0F RID: 11791
			DESCKIND_MAX
		}

		// Token: 0x0200053A RID: 1338
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagFUNCDESC
		{
			// Token: 0x04002E10 RID: 11792
			public int memid;

			// Token: 0x04002E11 RID: 11793
			public IntPtr lprgscode = IntPtr.Zero;

			// Token: 0x04002E12 RID: 11794
			public IntPtr lprgelemdescParam = IntPtr.Zero;

			// Token: 0x04002E13 RID: 11795
			public int funckind;

			// Token: 0x04002E14 RID: 11796
			public int invkind;

			// Token: 0x04002E15 RID: 11797
			public int callconv;

			// Token: 0x04002E16 RID: 11798
			[MarshalAs(UnmanagedType.I2)]
			public short cParams;

			// Token: 0x04002E17 RID: 11799
			[MarshalAs(UnmanagedType.I2)]
			public short cParamsOpt;

			// Token: 0x04002E18 RID: 11800
			[MarshalAs(UnmanagedType.I2)]
			public short oVft;

			// Token: 0x04002E19 RID: 11801
			[MarshalAs(UnmanagedType.I2)]
			public short cScodesi;

			// Token: 0x04002E1A RID: 11802
			public NativeMethods.value_tagELEMDESC elemdescFunc;

			// Token: 0x04002E1B RID: 11803
			[MarshalAs(UnmanagedType.U2)]
			public short wFuncFlags;
		}

		// Token: 0x0200053B RID: 1339
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagVARDESC
		{
			// Token: 0x04002E1C RID: 11804
			public int memid;

			// Token: 0x04002E1D RID: 11805
			public IntPtr lpstrSchema = IntPtr.Zero;

			// Token: 0x04002E1E RID: 11806
			public IntPtr unionMember = IntPtr.Zero;

			// Token: 0x04002E1F RID: 11807
			public NativeMethods.value_tagELEMDESC elemdescVar;

			// Token: 0x04002E20 RID: 11808
			[MarshalAs(UnmanagedType.U2)]
			public short wVarFlags;

			// Token: 0x04002E21 RID: 11809
			public int varkind;
		}

		// Token: 0x0200053C RID: 1340
		public struct value_tagELEMDESC
		{
			// Token: 0x04002E22 RID: 11810
			public NativeMethods.tagTYPEDESC tdesc;

			// Token: 0x04002E23 RID: 11811
			public NativeMethods.tagPARAMDESC paramdesc;
		}

		// Token: 0x0200053D RID: 1341
		public struct WINDOWPOS
		{
			// Token: 0x04002E24 RID: 11812
			public IntPtr hwnd;

			// Token: 0x04002E25 RID: 11813
			public IntPtr hwndInsertAfter;

			// Token: 0x04002E26 RID: 11814
			public int x;

			// Token: 0x04002E27 RID: 11815
			public int y;

			// Token: 0x04002E28 RID: 11816
			public int cx;

			// Token: 0x04002E29 RID: 11817
			public int cy;

			// Token: 0x04002E2A RID: 11818
			public int flags;
		}

		// Token: 0x0200053E RID: 1342
		public struct HDLAYOUT
		{
			// Token: 0x04002E2B RID: 11819
			public IntPtr prc;

			// Token: 0x04002E2C RID: 11820
			public IntPtr pwpos;
		}

		// Token: 0x0200053F RID: 1343
		[StructLayout(LayoutKind.Sequential)]
		public class DRAWITEMSTRUCT
		{
			// Token: 0x04002E2D RID: 11821
			public int CtlType;

			// Token: 0x04002E2E RID: 11822
			public int CtlID;

			// Token: 0x04002E2F RID: 11823
			public int itemID;

			// Token: 0x04002E30 RID: 11824
			public int itemAction;

			// Token: 0x04002E31 RID: 11825
			public int itemState;

			// Token: 0x04002E32 RID: 11826
			public IntPtr hwndItem = IntPtr.Zero;

			// Token: 0x04002E33 RID: 11827
			public IntPtr hDC = IntPtr.Zero;

			// Token: 0x04002E34 RID: 11828
			public NativeMethods.RECT rcItem;

			// Token: 0x04002E35 RID: 11829
			public IntPtr itemData = IntPtr.Zero;
		}

		// Token: 0x02000540 RID: 1344
		[StructLayout(LayoutKind.Sequential)]
		public class MEASUREITEMSTRUCT
		{
			// Token: 0x04002E36 RID: 11830
			public int CtlType;

			// Token: 0x04002E37 RID: 11831
			public int CtlID;

			// Token: 0x04002E38 RID: 11832
			public int itemID;

			// Token: 0x04002E39 RID: 11833
			public int itemWidth;

			// Token: 0x04002E3A RID: 11834
			public int itemHeight;

			// Token: 0x04002E3B RID: 11835
			public IntPtr itemData = IntPtr.Zero;
		}

		// Token: 0x02000541 RID: 1345
		[StructLayout(LayoutKind.Sequential)]
		public class HELPINFO
		{
			// Token: 0x04002E3C RID: 11836
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.HELPINFO));

			// Token: 0x04002E3D RID: 11837
			public int iContextType;

			// Token: 0x04002E3E RID: 11838
			public int iCtrlId;

			// Token: 0x04002E3F RID: 11839
			public IntPtr hItemHandle = IntPtr.Zero;

			// Token: 0x04002E40 RID: 11840
			public int dwContextId;

			// Token: 0x04002E41 RID: 11841
			public NativeMethods.POINT MousePos;
		}

		// Token: 0x02000542 RID: 1346
		[StructLayout(LayoutKind.Sequential)]
		public class ACCEL
		{
			// Token: 0x04002E42 RID: 11842
			public byte fVirt;

			// Token: 0x04002E43 RID: 11843
			public short key;

			// Token: 0x04002E44 RID: 11844
			public short cmd;
		}

		// Token: 0x02000543 RID: 1347
		[StructLayout(LayoutKind.Sequential)]
		public class MINMAXINFO
		{
			// Token: 0x04002E45 RID: 11845
			public NativeMethods.POINT ptReserved;

			// Token: 0x04002E46 RID: 11846
			public NativeMethods.POINT ptMaxSize;

			// Token: 0x04002E47 RID: 11847
			public NativeMethods.POINT ptMaxPosition;

			// Token: 0x04002E48 RID: 11848
			public NativeMethods.POINT ptMinTrackSize;

			// Token: 0x04002E49 RID: 11849
			public NativeMethods.POINT ptMaxTrackSize;
		}

		// Token: 0x02000544 RID: 1348
		[Guid("B196B28B-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ISpecifyPropertyPages
		{
			// Token: 0x06004A1A RID: 18970
			void GetPages([Out] NativeMethods.tagCAUUID pPages);
		}

		// Token: 0x02000545 RID: 1349
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagCAUUID
		{
			// Token: 0x04002E4A RID: 11850
			[MarshalAs(UnmanagedType.U4)]
			public int cElems;

			// Token: 0x04002E4B RID: 11851
			public IntPtr pElems = IntPtr.Zero;
		}

		// Token: 0x02000546 RID: 1350
		public struct NMTOOLBAR
		{
			// Token: 0x04002E4C RID: 11852
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002E4D RID: 11853
			public int iItem;

			// Token: 0x04002E4E RID: 11854
			public NativeMethods.TBBUTTON tbButton;

			// Token: 0x04002E4F RID: 11855
			public int cchText;

			// Token: 0x04002E50 RID: 11856
			public IntPtr pszText;
		}

		// Token: 0x02000547 RID: 1351
		public struct TBBUTTON
		{
			// Token: 0x04002E51 RID: 11857
			public int iBitmap;

			// Token: 0x04002E52 RID: 11858
			public int idCommand;

			// Token: 0x04002E53 RID: 11859
			public byte fsState;

			// Token: 0x04002E54 RID: 11860
			public byte fsStyle;

			// Token: 0x04002E55 RID: 11861
			public byte bReserved0;

			// Token: 0x04002E56 RID: 11862
			public byte bReserved1;

			// Token: 0x04002E57 RID: 11863
			public IntPtr dwData;

			// Token: 0x04002E58 RID: 11864
			public IntPtr iString;
		}

		// Token: 0x02000548 RID: 1352
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TOOLTIPTEXT
		{
			// Token: 0x04002E59 RID: 11865
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002E5A RID: 11866
			public string lpszText;

			// Token: 0x04002E5B RID: 11867
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szText;

			// Token: 0x04002E5C RID: 11868
			public IntPtr hinst;

			// Token: 0x04002E5D RID: 11869
			public int uFlags;
		}

		// Token: 0x02000549 RID: 1353
		[StructLayout(LayoutKind.Sequential)]
		public class TOOLTIPTEXTA
		{
			// Token: 0x04002E5E RID: 11870
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002E5F RID: 11871
			public string lpszText;

			// Token: 0x04002E60 RID: 11872
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szText;

			// Token: 0x04002E61 RID: 11873
			public IntPtr hinst;

			// Token: 0x04002E62 RID: 11874
			public int uFlags;
		}

		// Token: 0x0200054A RID: 1354
		public struct NMTBHOTITEM
		{
			// Token: 0x04002E63 RID: 11875
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002E64 RID: 11876
			public int idOld;

			// Token: 0x04002E65 RID: 11877
			public int idNew;

			// Token: 0x04002E66 RID: 11878
			public int dwFlags;
		}

		// Token: 0x0200054B RID: 1355
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class HDITEM2
		{
			// Token: 0x04002E67 RID: 11879
			public int mask;

			// Token: 0x04002E68 RID: 11880
			public int cxy;

			// Token: 0x04002E69 RID: 11881
			public IntPtr pszText_notUsed = IntPtr.Zero;

			// Token: 0x04002E6A RID: 11882
			public IntPtr hbm = IntPtr.Zero;

			// Token: 0x04002E6B RID: 11883
			public int cchTextMax;

			// Token: 0x04002E6C RID: 11884
			public int fmt;

			// Token: 0x04002E6D RID: 11885
			public IntPtr lParam = IntPtr.Zero;

			// Token: 0x04002E6E RID: 11886
			public int iImage;

			// Token: 0x04002E6F RID: 11887
			public int iOrder;

			// Token: 0x04002E70 RID: 11888
			public int type;

			// Token: 0x04002E71 RID: 11889
			public IntPtr pvFilter = IntPtr.Zero;
		}

		// Token: 0x0200054C RID: 1356
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct TBBUTTONINFO
		{
			// Token: 0x04002E72 RID: 11890
			public int cbSize;

			// Token: 0x04002E73 RID: 11891
			public int dwMask;

			// Token: 0x04002E74 RID: 11892
			public int idCommand;

			// Token: 0x04002E75 RID: 11893
			public int iImage;

			// Token: 0x04002E76 RID: 11894
			public byte fsState;

			// Token: 0x04002E77 RID: 11895
			public byte fsStyle;

			// Token: 0x04002E78 RID: 11896
			public short cx;

			// Token: 0x04002E79 RID: 11897
			public IntPtr lParam;

			// Token: 0x04002E7A RID: 11898
			public IntPtr pszText;

			// Token: 0x04002E7B RID: 11899
			public int cchTest;
		}

		// Token: 0x0200054D RID: 1357
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TV_HITTESTINFO
		{
			// Token: 0x04002E7C RID: 11900
			public int pt_x;

			// Token: 0x04002E7D RID: 11901
			public int pt_y;

			// Token: 0x04002E7E RID: 11902
			public int flags;

			// Token: 0x04002E7F RID: 11903
			public IntPtr hItem = IntPtr.Zero;
		}

		// Token: 0x0200054E RID: 1358
		[StructLayout(LayoutKind.Sequential)]
		public class NMTVCUSTOMDRAW
		{
			// Token: 0x04002E80 RID: 11904
			public NativeMethods.NMCUSTOMDRAW nmcd;

			// Token: 0x04002E81 RID: 11905
			public int clrText;

			// Token: 0x04002E82 RID: 11906
			public int clrTextBk;

			// Token: 0x04002E83 RID: 11907
			public int iLevel;
		}

		// Token: 0x0200054F RID: 1359
		public struct NMCUSTOMDRAW
		{
			// Token: 0x04002E84 RID: 11908
			public NativeMethods.NMHDR nmcd;

			// Token: 0x04002E85 RID: 11909
			public int dwDrawStage;

			// Token: 0x04002E86 RID: 11910
			public IntPtr hdc;

			// Token: 0x04002E87 RID: 11911
			public NativeMethods.RECT rc;

			// Token: 0x04002E88 RID: 11912
			public IntPtr dwItemSpec;

			// Token: 0x04002E89 RID: 11913
			public int uItemState;

			// Token: 0x04002E8A RID: 11914
			public IntPtr lItemlParam;
		}

		// Token: 0x02000550 RID: 1360
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class MCHITTESTINFO
		{
			// Token: 0x04002E8B RID: 11915
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.MCHITTESTINFO));

			// Token: 0x04002E8C RID: 11916
			public int pt_x;

			// Token: 0x04002E8D RID: 11917
			public int pt_y;

			// Token: 0x04002E8E RID: 11918
			public int uHit;

			// Token: 0x04002E8F RID: 11919
			public short st_wYear;

			// Token: 0x04002E90 RID: 11920
			public short st_wMonth;

			// Token: 0x04002E91 RID: 11921
			public short st_wDayOfWeek;

			// Token: 0x04002E92 RID: 11922
			public short st_wDay;

			// Token: 0x04002E93 RID: 11923
			public short st_wHour;

			// Token: 0x04002E94 RID: 11924
			public short st_wMinute;

			// Token: 0x04002E95 RID: 11925
			public short st_wSecond;

			// Token: 0x04002E96 RID: 11926
			public short st_wMilliseconds;
		}

		// Token: 0x02000551 RID: 1361
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NMSELCHANGE
		{
			// Token: 0x04002E97 RID: 11927
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002E98 RID: 11928
			public NativeMethods.SYSTEMTIME stSelStart;

			// Token: 0x04002E99 RID: 11929
			public NativeMethods.SYSTEMTIME stSelEnd;
		}

		// Token: 0x02000552 RID: 1362
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NMDAYSTATE
		{
			// Token: 0x04002E9A RID: 11930
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002E9B RID: 11931
			public NativeMethods.SYSTEMTIME stStart;

			// Token: 0x04002E9C RID: 11932
			public int cDayState;

			// Token: 0x04002E9D RID: 11933
			public IntPtr prgDayState;
		}

		// Token: 0x02000553 RID: 1363
		public struct NMLVCUSTOMDRAW
		{
			// Token: 0x04002E9E RID: 11934
			public NativeMethods.NMCUSTOMDRAW nmcd;

			// Token: 0x04002E9F RID: 11935
			public int clrText;

			// Token: 0x04002EA0 RID: 11936
			public int clrTextBk;

			// Token: 0x04002EA1 RID: 11937
			public int iSubItem;

			// Token: 0x04002EA2 RID: 11938
			public int dwItemType;

			// Token: 0x04002EA3 RID: 11939
			public int clrFace;

			// Token: 0x04002EA4 RID: 11940
			public int iIconEffect;

			// Token: 0x04002EA5 RID: 11941
			public int iIconPhase;

			// Token: 0x04002EA6 RID: 11942
			public int iPartId;

			// Token: 0x04002EA7 RID: 11943
			public int iStateId;

			// Token: 0x04002EA8 RID: 11944
			public NativeMethods.RECT rcText;

			// Token: 0x04002EA9 RID: 11945
			public uint uAlign;
		}

		// Token: 0x02000554 RID: 1364
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NMLVGETINFOTIP
		{
			// Token: 0x04002EAA RID: 11946
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002EAB RID: 11947
			public int flags;

			// Token: 0x04002EAC RID: 11948
			public IntPtr lpszText = IntPtr.Zero;

			// Token: 0x04002EAD RID: 11949
			public int cchTextMax;

			// Token: 0x04002EAE RID: 11950
			public int item;

			// Token: 0x04002EAF RID: 11951
			public int subItem;

			// Token: 0x04002EB0 RID: 11952
			public IntPtr lParam = IntPtr.Zero;
		}

		// Token: 0x02000555 RID: 1365
		[StructLayout(LayoutKind.Sequential)]
		public class NMLVKEYDOWN
		{
			// Token: 0x04002EB1 RID: 11953
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002EB2 RID: 11954
			public short wVKey;

			// Token: 0x04002EB3 RID: 11955
			public uint flags;
		}

		// Token: 0x02000556 RID: 1366
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LVHITTESTINFO
		{
			// Token: 0x04002EB4 RID: 11956
			public int pt_x;

			// Token: 0x04002EB5 RID: 11957
			public int pt_y;

			// Token: 0x04002EB6 RID: 11958
			public int flags;

			// Token: 0x04002EB7 RID: 11959
			public int iItem;

			// Token: 0x04002EB8 RID: 11960
			public int iSubItem;
		}

		// Token: 0x02000557 RID: 1367
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LVBKIMAGE
		{
			// Token: 0x04002EB9 RID: 11961
			public int ulFlags;

			// Token: 0x04002EBA RID: 11962
			public IntPtr hBmp = IntPtr.Zero;

			// Token: 0x04002EBB RID: 11963
			public string pszImage;

			// Token: 0x04002EBC RID: 11964
			public int cchImageMax;

			// Token: 0x04002EBD RID: 11965
			public int xOffset;

			// Token: 0x04002EBE RID: 11966
			public int yOffset;
		}

		// Token: 0x02000558 RID: 1368
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LVCOLUMN_T
		{
			// Token: 0x04002EBF RID: 11967
			public int mask;

			// Token: 0x04002EC0 RID: 11968
			public int fmt;

			// Token: 0x04002EC1 RID: 11969
			public int cx;

			// Token: 0x04002EC2 RID: 11970
			public string pszText;

			// Token: 0x04002EC3 RID: 11971
			public int cchTextMax;

			// Token: 0x04002EC4 RID: 11972
			public int iSubItem;

			// Token: 0x04002EC5 RID: 11973
			public int iImage;

			// Token: 0x04002EC6 RID: 11974
			public int iOrder;
		}

		// Token: 0x02000559 RID: 1369
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct LVFINDINFO
		{
			// Token: 0x04002EC7 RID: 11975
			public int flags;

			// Token: 0x04002EC8 RID: 11976
			public string psz;

			// Token: 0x04002EC9 RID: 11977
			public IntPtr lParam;

			// Token: 0x04002ECA RID: 11978
			public int ptX;

			// Token: 0x04002ECB RID: 11979
			public int ptY;

			// Token: 0x04002ECC RID: 11980
			public int vkDirection;
		}

		// Token: 0x0200055A RID: 1370
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct LVITEM
		{
			// Token: 0x06004A29 RID: 18985 RVA: 0x0010DE50 File Offset: 0x0010CE50
			public void Reset()
			{
				this.pszText = null;
				this.mask = 0;
				this.iItem = 0;
				this.iSubItem = 0;
				this.stateMask = 0;
				this.state = 0;
				this.cchTextMax = 0;
				this.iImage = 0;
				this.lParam = IntPtr.Zero;
				this.iIndent = 0;
				this.iGroupId = 0;
				this.cColumns = 0;
				this.puColumns = IntPtr.Zero;
			}

			// Token: 0x06004A2A RID: 18986 RVA: 0x0010DEC0 File Offset: 0x0010CEC0
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"LVITEM: pszText = ",
					this.pszText,
					", iItem = ",
					this.iItem.ToString(CultureInfo.InvariantCulture),
					", iSubItem = ",
					this.iSubItem.ToString(CultureInfo.InvariantCulture),
					", state = ",
					this.state.ToString(CultureInfo.InvariantCulture),
					", iGroupId = ",
					this.iGroupId.ToString(CultureInfo.InvariantCulture),
					", cColumns = ",
					this.cColumns.ToString(CultureInfo.InvariantCulture)
				});
			}

			// Token: 0x04002ECD RID: 11981
			public int mask;

			// Token: 0x04002ECE RID: 11982
			public int iItem;

			// Token: 0x04002ECF RID: 11983
			public int iSubItem;

			// Token: 0x04002ED0 RID: 11984
			public int state;

			// Token: 0x04002ED1 RID: 11985
			public int stateMask;

			// Token: 0x04002ED2 RID: 11986
			public string pszText;

			// Token: 0x04002ED3 RID: 11987
			public int cchTextMax;

			// Token: 0x04002ED4 RID: 11988
			public int iImage;

			// Token: 0x04002ED5 RID: 11989
			public IntPtr lParam;

			// Token: 0x04002ED6 RID: 11990
			public int iIndent;

			// Token: 0x04002ED7 RID: 11991
			public int iGroupId;

			// Token: 0x04002ED8 RID: 11992
			public int cColumns;

			// Token: 0x04002ED9 RID: 11993
			public IntPtr puColumns;
		}

		// Token: 0x0200055B RID: 1371
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct LVITEM_NOTEXT
		{
			// Token: 0x04002EDA RID: 11994
			public int mask;

			// Token: 0x04002EDB RID: 11995
			public int iItem;

			// Token: 0x04002EDC RID: 11996
			public int iSubItem;

			// Token: 0x04002EDD RID: 11997
			public int state;

			// Token: 0x04002EDE RID: 11998
			public int stateMask;

			// Token: 0x04002EDF RID: 11999
			public IntPtr pszText;

			// Token: 0x04002EE0 RID: 12000
			public int cchTextMax;

			// Token: 0x04002EE1 RID: 12001
			public int iImage;

			// Token: 0x04002EE2 RID: 12002
			public IntPtr lParam;

			// Token: 0x04002EE3 RID: 12003
			public int iIndent;
		}

		// Token: 0x0200055C RID: 1372
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LVCOLUMN
		{
			// Token: 0x04002EE4 RID: 12004
			public int mask;

			// Token: 0x04002EE5 RID: 12005
			public int fmt;

			// Token: 0x04002EE6 RID: 12006
			public int cx;

			// Token: 0x04002EE7 RID: 12007
			public IntPtr pszText;

			// Token: 0x04002EE8 RID: 12008
			public int cchTextMax;

			// Token: 0x04002EE9 RID: 12009
			public int iSubItem;

			// Token: 0x04002EEA RID: 12010
			public int iImage;

			// Token: 0x04002EEB RID: 12011
			public int iOrder;
		}

		// Token: 0x0200055D RID: 1373
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class LVGROUP
		{
			// Token: 0x06004A2C RID: 18988 RVA: 0x0010DF7E File Offset: 0x0010CF7E
			public override string ToString()
			{
				return "LVGROUP: header = " + this.pszHeader.ToString() + ", iGroupId = " + this.iGroupId.ToString(CultureInfo.InvariantCulture);
			}

			// Token: 0x04002EEC RID: 12012
			public uint cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.LVGROUP));

			// Token: 0x04002EED RID: 12013
			public uint mask;

			// Token: 0x04002EEE RID: 12014
			public IntPtr pszHeader;

			// Token: 0x04002EEF RID: 12015
			public int cchHeader;

			// Token: 0x04002EF0 RID: 12016
			public IntPtr pszFooter = IntPtr.Zero;

			// Token: 0x04002EF1 RID: 12017
			public int cchFooter;

			// Token: 0x04002EF2 RID: 12018
			public int iGroupId;

			// Token: 0x04002EF3 RID: 12019
			public uint stateMask;

			// Token: 0x04002EF4 RID: 12020
			public uint state;

			// Token: 0x04002EF5 RID: 12021
			public uint uAlign;
		}

		// Token: 0x0200055E RID: 1374
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LVINSERTMARK
		{
			// Token: 0x04002EF6 RID: 12022
			public uint cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.LVINSERTMARK));

			// Token: 0x04002EF7 RID: 12023
			public int dwFlags;

			// Token: 0x04002EF8 RID: 12024
			public int iItem;

			// Token: 0x04002EF9 RID: 12025
			public int dwReserved;
		}

		// Token: 0x0200055F RID: 1375
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LVTILEVIEWINFO
		{
			// Token: 0x04002EFA RID: 12026
			public uint cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.LVTILEVIEWINFO));

			// Token: 0x04002EFB RID: 12027
			public int dwMask;

			// Token: 0x04002EFC RID: 12028
			public int dwFlags;

			// Token: 0x04002EFD RID: 12029
			public NativeMethods.SIZE sizeTile;

			// Token: 0x04002EFE RID: 12030
			public int cLines;

			// Token: 0x04002EFF RID: 12031
			public NativeMethods.RECT rcLabelMargin;
		}

		// Token: 0x02000560 RID: 1376
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NMLVCACHEHINT
		{
			// Token: 0x04002F00 RID: 12032
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002F01 RID: 12033
			public int iFrom;

			// Token: 0x04002F02 RID: 12034
			public int iTo;
		}

		// Token: 0x02000561 RID: 1377
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NMLVDISPINFO
		{
			// Token: 0x04002F03 RID: 12035
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002F04 RID: 12036
			public NativeMethods.LVITEM item;
		}

		// Token: 0x02000562 RID: 1378
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NMLVDISPINFO_NOTEXT
		{
			// Token: 0x04002F05 RID: 12037
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002F06 RID: 12038
			public NativeMethods.LVITEM_NOTEXT item;
		}

		// Token: 0x02000563 RID: 1379
		[StructLayout(LayoutKind.Sequential)]
		public class NMLVODSTATECHANGE
		{
			// Token: 0x04002F07 RID: 12039
			public NativeMethods.NMHDR hdr;

			// Token: 0x04002F08 RID: 12040
			public int iFrom;

			// Token: 0x04002F09 RID: 12041
			public int iTo;

			// Token: 0x04002F0A RID: 12042
			public int uNewState;

			// Token: 0x04002F0B RID: 12043
			public int uOldState;
		}

		// Token: 0x02000564 RID: 1380
		[StructLayout(LayoutKind.Sequential)]
		public class CLIENTCREATESTRUCT
		{
			// Token: 0x06004A34 RID: 18996 RVA: 0x0010E032 File Offset: 0x0010D032
			public CLIENTCREATESTRUCT(IntPtr hmenu, int idFirst)
			{
				this.hWindowMenu = hmenu;
				this.idFirstChild = idFirst;
			}

			// Token: 0x04002F0C RID: 12044
			public IntPtr hWindowMenu;

			// Token: 0x04002F0D RID: 12045
			public int idFirstChild;
		}

		// Token: 0x02000565 RID: 1381
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class NMDATETIMECHANGE
		{
			// Token: 0x04002F0E RID: 12046
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002F0F RID: 12047
			public int dwFlags;

			// Token: 0x04002F10 RID: 12048
			public NativeMethods.SYSTEMTIME st;
		}

		// Token: 0x02000566 RID: 1382
		[StructLayout(LayoutKind.Sequential)]
		public class COPYDATASTRUCT
		{
			// Token: 0x04002F11 RID: 12049
			public int dwData;

			// Token: 0x04002F12 RID: 12050
			public int cbData;

			// Token: 0x04002F13 RID: 12051
			public IntPtr lpData = IntPtr.Zero;
		}

		// Token: 0x02000567 RID: 1383
		[StructLayout(LayoutKind.Sequential)]
		public class NMHEADER
		{
			// Token: 0x04002F14 RID: 12052
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002F15 RID: 12053
			public int iItem;

			// Token: 0x04002F16 RID: 12054
			public int iButton;

			// Token: 0x04002F17 RID: 12055
			public IntPtr pItem = IntPtr.Zero;
		}

		// Token: 0x02000568 RID: 1384
		[StructLayout(LayoutKind.Sequential)]
		public class MOUSEHOOKSTRUCT
		{
			// Token: 0x04002F18 RID: 12056
			public int pt_x;

			// Token: 0x04002F19 RID: 12057
			public int pt_y;

			// Token: 0x04002F1A RID: 12058
			public IntPtr hWnd = IntPtr.Zero;

			// Token: 0x04002F1B RID: 12059
			public int wHitTestCode;

			// Token: 0x04002F1C RID: 12060
			public int dwExtraInfo;
		}

		// Token: 0x02000569 RID: 1385
		public struct MOUSEINPUT
		{
			// Token: 0x04002F1D RID: 12061
			public int dx;

			// Token: 0x04002F1E RID: 12062
			public int dy;

			// Token: 0x04002F1F RID: 12063
			public int mouseData;

			// Token: 0x04002F20 RID: 12064
			public int dwFlags;

			// Token: 0x04002F21 RID: 12065
			public int time;

			// Token: 0x04002F22 RID: 12066
			public IntPtr dwExtraInfo;
		}

		// Token: 0x0200056A RID: 1386
		public struct KEYBDINPUT
		{
			// Token: 0x04002F23 RID: 12067
			public short wVk;

			// Token: 0x04002F24 RID: 12068
			public short wScan;

			// Token: 0x04002F25 RID: 12069
			public int dwFlags;

			// Token: 0x04002F26 RID: 12070
			public int time;

			// Token: 0x04002F27 RID: 12071
			public IntPtr dwExtraInfo;
		}

		// Token: 0x0200056B RID: 1387
		public struct HARDWAREINPUT
		{
			// Token: 0x04002F28 RID: 12072
			public int uMsg;

			// Token: 0x04002F29 RID: 12073
			public short wParamL;

			// Token: 0x04002F2A RID: 12074
			public short wParamH;
		}

		// Token: 0x0200056C RID: 1388
		public struct INPUT
		{
			// Token: 0x04002F2B RID: 12075
			public int type;

			// Token: 0x04002F2C RID: 12076
			public NativeMethods.INPUTUNION inputUnion;
		}

		// Token: 0x0200056D RID: 1389
		[StructLayout(LayoutKind.Explicit)]
		public struct INPUTUNION
		{
			// Token: 0x04002F2D RID: 12077
			[FieldOffset(0)]
			public NativeMethods.MOUSEINPUT mi;

			// Token: 0x04002F2E RID: 12078
			[FieldOffset(0)]
			public NativeMethods.KEYBDINPUT ki;

			// Token: 0x04002F2F RID: 12079
			[FieldOffset(0)]
			public NativeMethods.HARDWAREINPUT hi;
		}

		// Token: 0x0200056E RID: 1390
		[StructLayout(LayoutKind.Sequential)]
		public class CHARRANGE
		{
			// Token: 0x04002F30 RID: 12080
			public int cpMin;

			// Token: 0x04002F31 RID: 12081
			public int cpMax;
		}

		// Token: 0x0200056F RID: 1391
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public class CHARFORMATW
		{
			// Token: 0x04002F32 RID: 12082
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.CHARFORMATW));

			// Token: 0x04002F33 RID: 12083
			public int dwMask;

			// Token: 0x04002F34 RID: 12084
			public int dwEffects;

			// Token: 0x04002F35 RID: 12085
			public int yHeight;

			// Token: 0x04002F36 RID: 12086
			public int yOffset;

			// Token: 0x04002F37 RID: 12087
			public int crTextColor;

			// Token: 0x04002F38 RID: 12088
			public byte bCharSet;

			// Token: 0x04002F39 RID: 12089
			public byte bPitchAndFamily;

			// Token: 0x04002F3A RID: 12090
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] szFaceName = new byte[64];
		}

		// Token: 0x02000570 RID: 1392
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public class CHARFORMATA
		{
			// Token: 0x04002F3B RID: 12091
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.CHARFORMATA));

			// Token: 0x04002F3C RID: 12092
			public int dwMask;

			// Token: 0x04002F3D RID: 12093
			public int dwEffects;

			// Token: 0x04002F3E RID: 12094
			public int yHeight;

			// Token: 0x04002F3F RID: 12095
			public int yOffset;

			// Token: 0x04002F40 RID: 12096
			public int crTextColor;

			// Token: 0x04002F41 RID: 12097
			public byte bCharSet;

			// Token: 0x04002F42 RID: 12098
			public byte bPitchAndFamily;

			// Token: 0x04002F43 RID: 12099
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] szFaceName = new byte[32];
		}

		// Token: 0x02000571 RID: 1393
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public class CHARFORMAT2A
		{
			// Token: 0x04002F44 RID: 12100
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.CHARFORMAT2A));

			// Token: 0x04002F45 RID: 12101
			public int dwMask;

			// Token: 0x04002F46 RID: 12102
			public int dwEffects;

			// Token: 0x04002F47 RID: 12103
			public int yHeight;

			// Token: 0x04002F48 RID: 12104
			public int yOffset;

			// Token: 0x04002F49 RID: 12105
			public int crTextColor;

			// Token: 0x04002F4A RID: 12106
			public byte bCharSet;

			// Token: 0x04002F4B RID: 12107
			public byte bPitchAndFamily;

			// Token: 0x04002F4C RID: 12108
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] szFaceName = new byte[32];

			// Token: 0x04002F4D RID: 12109
			public short wWeight;

			// Token: 0x04002F4E RID: 12110
			public short sSpacing;

			// Token: 0x04002F4F RID: 12111
			public int crBackColor;

			// Token: 0x04002F50 RID: 12112
			public int lcid;

			// Token: 0x04002F51 RID: 12113
			public int dwReserved;

			// Token: 0x04002F52 RID: 12114
			public short sStyle;

			// Token: 0x04002F53 RID: 12115
			public short wKerning;

			// Token: 0x04002F54 RID: 12116
			public byte bUnderlineType;

			// Token: 0x04002F55 RID: 12117
			public byte bAnimation;

			// Token: 0x04002F56 RID: 12118
			public byte bRevAuthor;
		}

		// Token: 0x02000572 RID: 1394
		[StructLayout(LayoutKind.Sequential)]
		public class TEXTRANGE
		{
			// Token: 0x04002F57 RID: 12119
			public NativeMethods.CHARRANGE chrg;

			// Token: 0x04002F58 RID: 12120
			public IntPtr lpstrText;
		}

		// Token: 0x02000573 RID: 1395
		[StructLayout(LayoutKind.Sequential)]
		public class GETTEXTLENGTHEX
		{
			// Token: 0x04002F59 RID: 12121
			public uint flags;

			// Token: 0x04002F5A RID: 12122
			public uint codepage;
		}

		// Token: 0x02000574 RID: 1396
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public class SELCHANGE
		{
			// Token: 0x04002F5B RID: 12123
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002F5C RID: 12124
			public NativeMethods.CHARRANGE chrg;

			// Token: 0x04002F5D RID: 12125
			public int seltyp;
		}

		// Token: 0x02000575 RID: 1397
		[StructLayout(LayoutKind.Sequential)]
		public class PARAFORMAT
		{
			// Token: 0x04002F5E RID: 12126
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.PARAFORMAT));

			// Token: 0x04002F5F RID: 12127
			public int dwMask;

			// Token: 0x04002F60 RID: 12128
			public short wNumbering;

			// Token: 0x04002F61 RID: 12129
			public short wReserved;

			// Token: 0x04002F62 RID: 12130
			public int dxStartIndent;

			// Token: 0x04002F63 RID: 12131
			public int dxRightIndent;

			// Token: 0x04002F64 RID: 12132
			public int dxOffset;

			// Token: 0x04002F65 RID: 12133
			public short wAlignment;

			// Token: 0x04002F66 RID: 12134
			public short cTabCount;

			// Token: 0x04002F67 RID: 12135
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public int[] rgxTabs;
		}

		// Token: 0x02000576 RID: 1398
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class FINDTEXT
		{
			// Token: 0x04002F68 RID: 12136
			public NativeMethods.CHARRANGE chrg;

			// Token: 0x04002F69 RID: 12137
			public string lpstrText;
		}

		// Token: 0x02000577 RID: 1399
		[StructLayout(LayoutKind.Sequential)]
		public class REPASTESPECIAL
		{
			// Token: 0x04002F6A RID: 12138
			public int dwAspect;

			// Token: 0x04002F6B RID: 12139
			public int dwParam;
		}

		// Token: 0x02000578 RID: 1400
		[StructLayout(LayoutKind.Sequential)]
		public class ENLINK
		{
			// Token: 0x04002F6C RID: 12140
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002F6D RID: 12141
			public int msg;

			// Token: 0x04002F6E RID: 12142
			public IntPtr wParam = IntPtr.Zero;

			// Token: 0x04002F6F RID: 12143
			public IntPtr lParam = IntPtr.Zero;

			// Token: 0x04002F70 RID: 12144
			public NativeMethods.CHARRANGE charrange;
		}

		// Token: 0x02000579 RID: 1401
		[StructLayout(LayoutKind.Sequential)]
		public class ENLINK64
		{
			// Token: 0x04002F71 RID: 12145
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
			public byte[] contents = new byte[56];
		}

		// Token: 0x0200057A RID: 1402
		public struct RGNDATAHEADER
		{
			// Token: 0x04002F72 RID: 12146
			public int cbSizeOfStruct;

			// Token: 0x04002F73 RID: 12147
			public int iType;

			// Token: 0x04002F74 RID: 12148
			public int nCount;

			// Token: 0x04002F75 RID: 12149
			public int nRgnSize;
		}

		// Token: 0x0200057B RID: 1403
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class OCPFIPARAMS
		{
			// Token: 0x04002F76 RID: 12150
			public int cbSizeOfStruct = Marshal.SizeOf(typeof(NativeMethods.OCPFIPARAMS));

			// Token: 0x04002F77 RID: 12151
			public IntPtr hwndOwner;

			// Token: 0x04002F78 RID: 12152
			public int x;

			// Token: 0x04002F79 RID: 12153
			public int y;

			// Token: 0x04002F7A RID: 12154
			public string lpszCaption;

			// Token: 0x04002F7B RID: 12155
			public int cObjects = 1;

			// Token: 0x04002F7C RID: 12156
			public IntPtr ppUnk;

			// Token: 0x04002F7D RID: 12157
			public int pageCount = 1;

			// Token: 0x04002F7E RID: 12158
			public IntPtr uuid;

			// Token: 0x04002F7F RID: 12159
			public int lcid = Application.CurrentCulture.LCID;

			// Token: 0x04002F80 RID: 12160
			public int dispidInitial;
		}

		// Token: 0x0200057C RID: 1404
		[ComVisible(true)]
		[StructLayout(LayoutKind.Sequential)]
		public class DOCHOSTUIINFO
		{
			// Token: 0x04002F81 RID: 12161
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.DOCHOSTUIINFO));

			// Token: 0x04002F82 RID: 12162
			[MarshalAs(UnmanagedType.I4)]
			public int dwFlags;

			// Token: 0x04002F83 RID: 12163
			[MarshalAs(UnmanagedType.I4)]
			public int dwDoubleClick;

			// Token: 0x04002F84 RID: 12164
			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved1;

			// Token: 0x04002F85 RID: 12165
			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved2;
		}

		// Token: 0x0200057D RID: 1405
		public enum DOCHOSTUIFLAG
		{
			// Token: 0x04002F87 RID: 12167
			DIALOG = 1,
			// Token: 0x04002F88 RID: 12168
			DISABLE_HELP_MENU,
			// Token: 0x04002F89 RID: 12169
			NO3DBORDER = 4,
			// Token: 0x04002F8A RID: 12170
			SCROLL_NO = 8,
			// Token: 0x04002F8B RID: 12171
			DISABLE_SCRIPT_INACTIVE = 16,
			// Token: 0x04002F8C RID: 12172
			OPENNEWWIN = 32,
			// Token: 0x04002F8D RID: 12173
			DISABLE_OFFSCREEN = 64,
			// Token: 0x04002F8E RID: 12174
			FLAT_SCROLLBAR = 128,
			// Token: 0x04002F8F RID: 12175
			DIV_BLOCKDEFAULT = 256,
			// Token: 0x04002F90 RID: 12176
			ACTIVATE_CLIENTHIT_ONLY = 512,
			// Token: 0x04002F91 RID: 12177
			NO3DOUTERBORDER = 2097152,
			// Token: 0x04002F92 RID: 12178
			THEME = 262144,
			// Token: 0x04002F93 RID: 12179
			NOTHEME = 524288,
			// Token: 0x04002F94 RID: 12180
			DISABLE_COOKIE = 1024
		}

		// Token: 0x0200057E RID: 1406
		public enum DOCHOSTUIDBLCLICK
		{
			// Token: 0x04002F96 RID: 12182
			DEFAULT,
			// Token: 0x04002F97 RID: 12183
			SHOWPROPERTIES,
			// Token: 0x04002F98 RID: 12184
			SHOWCODE
		}

		// Token: 0x0200057F RID: 1407
		public enum OLECMDID
		{
			// Token: 0x04002F9A RID: 12186
			OLECMDID_SAVEAS = 4,
			// Token: 0x04002F9B RID: 12187
			OLECMDID_PRINT = 6,
			// Token: 0x04002F9C RID: 12188
			OLECMDID_PRINTPREVIEW,
			// Token: 0x04002F9D RID: 12189
			OLECMDID_PAGESETUP,
			// Token: 0x04002F9E RID: 12190
			OLECMDID_PROPERTIES = 10
		}

		// Token: 0x02000580 RID: 1408
		public enum OLECMDEXECOPT
		{
			// Token: 0x04002FA0 RID: 12192
			OLECMDEXECOPT_DODEFAULT,
			// Token: 0x04002FA1 RID: 12193
			OLECMDEXECOPT_PROMPTUSER,
			// Token: 0x04002FA2 RID: 12194
			OLECMDEXECOPT_DONTPROMPTUSER,
			// Token: 0x04002FA3 RID: 12195
			OLECMDEXECOPT_SHOWHELP
		}

		// Token: 0x02000581 RID: 1409
		public enum OLECMDF
		{
			// Token: 0x04002FA5 RID: 12197
			OLECMDF_SUPPORTED = 1,
			// Token: 0x04002FA6 RID: 12198
			OLECMDF_ENABLED,
			// Token: 0x04002FA7 RID: 12199
			OLECMDF_LATCHED = 4,
			// Token: 0x04002FA8 RID: 12200
			OLECMDF_NINCHED = 8,
			// Token: 0x04002FA9 RID: 12201
			OLECMDF_INVISIBLE = 16,
			// Token: 0x04002FAA RID: 12202
			OLECMDF_DEFHIDEONCTXTMENU = 32
		}

		// Token: 0x02000582 RID: 1410
		[StructLayout(LayoutKind.Sequential)]
		public class ENDROPFILES
		{
			// Token: 0x04002FAB RID: 12203
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002FAC RID: 12204
			public IntPtr hDrop = IntPtr.Zero;

			// Token: 0x04002FAD RID: 12205
			public int cp;

			// Token: 0x04002FAE RID: 12206
			public bool fProtected;
		}

		// Token: 0x02000583 RID: 1411
		[StructLayout(LayoutKind.Sequential)]
		public class REQRESIZE
		{
			// Token: 0x04002FAF RID: 12207
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002FB0 RID: 12208
			public NativeMethods.RECT rc;
		}

		// Token: 0x02000584 RID: 1412
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class ENPROTECTED
		{
			// Token: 0x04002FB1 RID: 12209
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x04002FB2 RID: 12210
			public int msg;

			// Token: 0x04002FB3 RID: 12211
			public IntPtr wParam;

			// Token: 0x04002FB4 RID: 12212
			public IntPtr lParam;

			// Token: 0x04002FB5 RID: 12213
			public NativeMethods.CHARRANGE chrg;
		}

		// Token: 0x02000585 RID: 1413
		[StructLayout(LayoutKind.Sequential)]
		public class ENPROTECTED64
		{
			// Token: 0x04002FB6 RID: 12214
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
			public byte[] contents = new byte[56];
		}

		// Token: 0x02000586 RID: 1414
		public class ActiveX
		{
			// Token: 0x06004A4B RID: 19019 RVA: 0x0010E217 File Offset: 0x0010D217
			private ActiveX()
			{
			}

			// Token: 0x04002FB7 RID: 12215
			public const int OCM__BASE = 8192;

			// Token: 0x04002FB8 RID: 12216
			public const int DISPID_VALUE = 0;

			// Token: 0x04002FB9 RID: 12217
			public const int DISPID_UNKNOWN = -1;

			// Token: 0x04002FBA RID: 12218
			public const int DISPID_AUTOSIZE = -500;

			// Token: 0x04002FBB RID: 12219
			public const int DISPID_BACKCOLOR = -501;

			// Token: 0x04002FBC RID: 12220
			public const int DISPID_BACKSTYLE = -502;

			// Token: 0x04002FBD RID: 12221
			public const int DISPID_BORDERCOLOR = -503;

			// Token: 0x04002FBE RID: 12222
			public const int DISPID_BORDERSTYLE = -504;

			// Token: 0x04002FBF RID: 12223
			public const int DISPID_BORDERWIDTH = -505;

			// Token: 0x04002FC0 RID: 12224
			public const int DISPID_DRAWMODE = -507;

			// Token: 0x04002FC1 RID: 12225
			public const int DISPID_DRAWSTYLE = -508;

			// Token: 0x04002FC2 RID: 12226
			public const int DISPID_DRAWWIDTH = -509;

			// Token: 0x04002FC3 RID: 12227
			public const int DISPID_FILLCOLOR = -510;

			// Token: 0x04002FC4 RID: 12228
			public const int DISPID_FILLSTYLE = -511;

			// Token: 0x04002FC5 RID: 12229
			public const int DISPID_FONT = -512;

			// Token: 0x04002FC6 RID: 12230
			public const int DISPID_FORECOLOR = -513;

			// Token: 0x04002FC7 RID: 12231
			public const int DISPID_ENABLED = -514;

			// Token: 0x04002FC8 RID: 12232
			public const int DISPID_HWND = -515;

			// Token: 0x04002FC9 RID: 12233
			public const int DISPID_TABSTOP = -516;

			// Token: 0x04002FCA RID: 12234
			public const int DISPID_TEXT = -517;

			// Token: 0x04002FCB RID: 12235
			public const int DISPID_CAPTION = -518;

			// Token: 0x04002FCC RID: 12236
			public const int DISPID_BORDERVISIBLE = -519;

			// Token: 0x04002FCD RID: 12237
			public const int DISPID_APPEARANCE = -520;

			// Token: 0x04002FCE RID: 12238
			public const int DISPID_MOUSEPOINTER = -521;

			// Token: 0x04002FCF RID: 12239
			public const int DISPID_MOUSEICON = -522;

			// Token: 0x04002FD0 RID: 12240
			public const int DISPID_PICTURE = -523;

			// Token: 0x04002FD1 RID: 12241
			public const int DISPID_VALID = -524;

			// Token: 0x04002FD2 RID: 12242
			public const int DISPID_READYSTATE = -525;

			// Token: 0x04002FD3 RID: 12243
			public const int DISPID_REFRESH = -550;

			// Token: 0x04002FD4 RID: 12244
			public const int DISPID_DOCLICK = -551;

			// Token: 0x04002FD5 RID: 12245
			public const int DISPID_ABOUTBOX = -552;

			// Token: 0x04002FD6 RID: 12246
			public const int DISPID_CLICK = -600;

			// Token: 0x04002FD7 RID: 12247
			public const int DISPID_DBLCLICK = -601;

			// Token: 0x04002FD8 RID: 12248
			public const int DISPID_KEYDOWN = -602;

			// Token: 0x04002FD9 RID: 12249
			public const int DISPID_KEYPRESS = -603;

			// Token: 0x04002FDA RID: 12250
			public const int DISPID_KEYUP = -604;

			// Token: 0x04002FDB RID: 12251
			public const int DISPID_MOUSEDOWN = -605;

			// Token: 0x04002FDC RID: 12252
			public const int DISPID_MOUSEMOVE = -606;

			// Token: 0x04002FDD RID: 12253
			public const int DISPID_MOUSEUP = -607;

			// Token: 0x04002FDE RID: 12254
			public const int DISPID_ERROREVENT = -608;

			// Token: 0x04002FDF RID: 12255
			public const int DISPID_RIGHTTOLEFT = -611;

			// Token: 0x04002FE0 RID: 12256
			public const int DISPID_READYSTATECHANGE = -609;

			// Token: 0x04002FE1 RID: 12257
			public const int DISPID_AMBIENT_BACKCOLOR = -701;

			// Token: 0x04002FE2 RID: 12258
			public const int DISPID_AMBIENT_DISPLAYNAME = -702;

			// Token: 0x04002FE3 RID: 12259
			public const int DISPID_AMBIENT_FONT = -703;

			// Token: 0x04002FE4 RID: 12260
			public const int DISPID_AMBIENT_FORECOLOR = -704;

			// Token: 0x04002FE5 RID: 12261
			public const int DISPID_AMBIENT_LOCALEID = -705;

			// Token: 0x04002FE6 RID: 12262
			public const int DISPID_AMBIENT_MESSAGEREFLECT = -706;

			// Token: 0x04002FE7 RID: 12263
			public const int DISPID_AMBIENT_SCALEUNITS = -707;

			// Token: 0x04002FE8 RID: 12264
			public const int DISPID_AMBIENT_TEXTALIGN = -708;

			// Token: 0x04002FE9 RID: 12265
			public const int DISPID_AMBIENT_USERMODE = -709;

			// Token: 0x04002FEA RID: 12266
			public const int DISPID_AMBIENT_UIDEAD = -710;

			// Token: 0x04002FEB RID: 12267
			public const int DISPID_AMBIENT_SHOWGRABHANDLES = -711;

			// Token: 0x04002FEC RID: 12268
			public const int DISPID_AMBIENT_SHOWHATCHING = -712;

			// Token: 0x04002FED RID: 12269
			public const int DISPID_AMBIENT_DISPLAYASDEFAULT = -713;

			// Token: 0x04002FEE RID: 12270
			public const int DISPID_AMBIENT_SUPPORTSMNEMONICS = -714;

			// Token: 0x04002FEF RID: 12271
			public const int DISPID_AMBIENT_AUTOCLIP = -715;

			// Token: 0x04002FF0 RID: 12272
			public const int DISPID_AMBIENT_APPEARANCE = -716;

			// Token: 0x04002FF1 RID: 12273
			public const int DISPID_AMBIENT_PALETTE = -726;

			// Token: 0x04002FF2 RID: 12274
			public const int DISPID_AMBIENT_TRANSFERPRIORITY = -728;

			// Token: 0x04002FF3 RID: 12275
			public const int DISPID_AMBIENT_RIGHTTOLEFT = -732;

			// Token: 0x04002FF4 RID: 12276
			public const int DISPID_Name = -800;

			// Token: 0x04002FF5 RID: 12277
			public const int DISPID_Delete = -801;

			// Token: 0x04002FF6 RID: 12278
			public const int DISPID_Object = -802;

			// Token: 0x04002FF7 RID: 12279
			public const int DISPID_Parent = -803;

			// Token: 0x04002FF8 RID: 12280
			public const int DVASPECT_CONTENT = 1;

			// Token: 0x04002FF9 RID: 12281
			public const int DVASPECT_THUMBNAIL = 2;

			// Token: 0x04002FFA RID: 12282
			public const int DVASPECT_ICON = 4;

			// Token: 0x04002FFB RID: 12283
			public const int DVASPECT_DOCPRINT = 8;

			// Token: 0x04002FFC RID: 12284
			public const int OLEMISC_RECOMPOSEONRESIZE = 1;

			// Token: 0x04002FFD RID: 12285
			public const int OLEMISC_ONLYICONIC = 2;

			// Token: 0x04002FFE RID: 12286
			public const int OLEMISC_INSERTNOTREPLACE = 4;

			// Token: 0x04002FFF RID: 12287
			public const int OLEMISC_STATIC = 8;

			// Token: 0x04003000 RID: 12288
			public const int OLEMISC_CANTLINKINSIDE = 16;

			// Token: 0x04003001 RID: 12289
			public const int OLEMISC_CANLINKBYOLE1 = 32;

			// Token: 0x04003002 RID: 12290
			public const int OLEMISC_ISLINKOBJECT = 64;

			// Token: 0x04003003 RID: 12291
			public const int OLEMISC_INSIDEOUT = 128;

			// Token: 0x04003004 RID: 12292
			public const int OLEMISC_ACTIVATEWHENVISIBLE = 256;

			// Token: 0x04003005 RID: 12293
			public const int OLEMISC_RENDERINGISDEVICEINDEPENDENT = 512;

			// Token: 0x04003006 RID: 12294
			public const int OLEMISC_INVISIBLEATRUNTIME = 1024;

			// Token: 0x04003007 RID: 12295
			public const int OLEMISC_ALWAYSRUN = 2048;

			// Token: 0x04003008 RID: 12296
			public const int OLEMISC_ACTSLIKEBUTTON = 4096;

			// Token: 0x04003009 RID: 12297
			public const int OLEMISC_ACTSLIKELABEL = 8192;

			// Token: 0x0400300A RID: 12298
			public const int OLEMISC_NOUIACTIVATE = 16384;

			// Token: 0x0400300B RID: 12299
			public const int OLEMISC_ALIGNABLE = 32768;

			// Token: 0x0400300C RID: 12300
			public const int OLEMISC_SIMPLEFRAME = 65536;

			// Token: 0x0400300D RID: 12301
			public const int OLEMISC_SETCLIENTSITEFIRST = 131072;

			// Token: 0x0400300E RID: 12302
			public const int OLEMISC_IMEMODE = 262144;

			// Token: 0x0400300F RID: 12303
			public const int OLEMISC_IGNOREACTIVATEWHENVISIBLE = 524288;

			// Token: 0x04003010 RID: 12304
			public const int OLEMISC_WANTSTOMENUMERGE = 1048576;

			// Token: 0x04003011 RID: 12305
			public const int OLEMISC_SUPPORTSMULTILEVELUNDO = 2097152;

			// Token: 0x04003012 RID: 12306
			public const int QACONTAINER_SHOWHATCHING = 1;

			// Token: 0x04003013 RID: 12307
			public const int QACONTAINER_SHOWGRABHANDLES = 2;

			// Token: 0x04003014 RID: 12308
			public const int QACONTAINER_USERMODE = 4;

			// Token: 0x04003015 RID: 12309
			public const int QACONTAINER_DISPLAYASDEFAULT = 8;

			// Token: 0x04003016 RID: 12310
			public const int QACONTAINER_UIDEAD = 16;

			// Token: 0x04003017 RID: 12311
			public const int QACONTAINER_AUTOCLIP = 32;

			// Token: 0x04003018 RID: 12312
			public const int QACONTAINER_MESSAGEREFLECT = 64;

			// Token: 0x04003019 RID: 12313
			public const int QACONTAINER_SUPPORTSMNEMONICS = 128;

			// Token: 0x0400301A RID: 12314
			public const int XFORMCOORDS_POSITION = 1;

			// Token: 0x0400301B RID: 12315
			public const int XFORMCOORDS_SIZE = 2;

			// Token: 0x0400301C RID: 12316
			public const int XFORMCOORDS_HIMETRICTOCONTAINER = 4;

			// Token: 0x0400301D RID: 12317
			public const int XFORMCOORDS_CONTAINERTOHIMETRIC = 8;

			// Token: 0x0400301E RID: 12318
			public const int PROPCAT_Nil = -1;

			// Token: 0x0400301F RID: 12319
			public const int PROPCAT_Misc = -2;

			// Token: 0x04003020 RID: 12320
			public const int PROPCAT_Font = -3;

			// Token: 0x04003021 RID: 12321
			public const int PROPCAT_Position = -4;

			// Token: 0x04003022 RID: 12322
			public const int PROPCAT_Appearance = -5;

			// Token: 0x04003023 RID: 12323
			public const int PROPCAT_Behavior = -6;

			// Token: 0x04003024 RID: 12324
			public const int PROPCAT_Data = -7;

			// Token: 0x04003025 RID: 12325
			public const int PROPCAT_List = -8;

			// Token: 0x04003026 RID: 12326
			public const int PROPCAT_Text = -9;

			// Token: 0x04003027 RID: 12327
			public const int PROPCAT_Scale = -10;

			// Token: 0x04003028 RID: 12328
			public const int PROPCAT_DDE = -11;

			// Token: 0x04003029 RID: 12329
			public const int GC_WCH_SIBLING = 1;

			// Token: 0x0400302A RID: 12330
			public const int GC_WCH_CONTAINER = 2;

			// Token: 0x0400302B RID: 12331
			public const int GC_WCH_CONTAINED = 3;

			// Token: 0x0400302C RID: 12332
			public const int GC_WCH_ALL = 4;

			// Token: 0x0400302D RID: 12333
			public const int GC_WCH_FREVERSEDIR = 134217728;

			// Token: 0x0400302E RID: 12334
			public const int GC_WCH_FONLYNEXT = 268435456;

			// Token: 0x0400302F RID: 12335
			public const int GC_WCH_FONLYPREV = 536870912;

			// Token: 0x04003030 RID: 12336
			public const int GC_WCH_FSELECTED = 1073741824;

			// Token: 0x04003031 RID: 12337
			public const int OLECONTF_EMBEDDINGS = 1;

			// Token: 0x04003032 RID: 12338
			public const int OLECONTF_LINKS = 2;

			// Token: 0x04003033 RID: 12339
			public const int OLECONTF_OTHERS = 4;

			// Token: 0x04003034 RID: 12340
			public const int OLECONTF_ONLYUSER = 8;

			// Token: 0x04003035 RID: 12341
			public const int OLECONTF_ONLYIFRUNNING = 16;

			// Token: 0x04003036 RID: 12342
			public const int ALIGN_MIN = 0;

			// Token: 0x04003037 RID: 12343
			public const int ALIGN_NO_CHANGE = 0;

			// Token: 0x04003038 RID: 12344
			public const int ALIGN_TOP = 1;

			// Token: 0x04003039 RID: 12345
			public const int ALIGN_BOTTOM = 2;

			// Token: 0x0400303A RID: 12346
			public const int ALIGN_LEFT = 3;

			// Token: 0x0400303B RID: 12347
			public const int ALIGN_RIGHT = 4;

			// Token: 0x0400303C RID: 12348
			public const int ALIGN_MAX = 4;

			// Token: 0x0400303D RID: 12349
			public const int OLEVERBATTRIB_NEVERDIRTIES = 1;

			// Token: 0x0400303E RID: 12350
			public const int OLEVERBATTRIB_ONCONTAINERMENU = 2;

			// Token: 0x0400303F RID: 12351
			public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
		}

		// Token: 0x02000587 RID: 1415
		public static class Util
		{
			// Token: 0x06004A4D RID: 19021 RVA: 0x0010E230 File Offset: 0x0010D230
			public static int MAKELONG(int low, int high)
			{
				return high << 16 | (low & 65535);
			}

			// Token: 0x06004A4E RID: 19022 RVA: 0x0010E23E File Offset: 0x0010D23E
			public static IntPtr MAKELPARAM(int low, int high)
			{
				return (IntPtr)(high << 16 | (low & 65535));
			}

			// Token: 0x06004A4F RID: 19023 RVA: 0x0010E251 File Offset: 0x0010D251
			public static int HIWORD(int n)
			{
				return n >> 16 & 65535;
			}

			// Token: 0x06004A50 RID: 19024 RVA: 0x0010E25D File Offset: 0x0010D25D
			public static int HIWORD(IntPtr n)
			{
				return NativeMethods.Util.HIWORD((int)((long)n));
			}

			// Token: 0x06004A51 RID: 19025 RVA: 0x0010E26B File Offset: 0x0010D26B
			public static int LOWORD(int n)
			{
				return n & 65535;
			}

			// Token: 0x06004A52 RID: 19026 RVA: 0x0010E274 File Offset: 0x0010D274
			public static int LOWORD(IntPtr n)
			{
				return NativeMethods.Util.LOWORD((int)((long)n));
			}

			// Token: 0x06004A53 RID: 19027 RVA: 0x0010E282 File Offset: 0x0010D282
			public static int SignedHIWORD(IntPtr n)
			{
				return NativeMethods.Util.SignedHIWORD((int)((long)n));
			}

			// Token: 0x06004A54 RID: 19028 RVA: 0x0010E290 File Offset: 0x0010D290
			public static int SignedLOWORD(IntPtr n)
			{
				return NativeMethods.Util.SignedLOWORD((int)((long)n));
			}

			// Token: 0x06004A55 RID: 19029 RVA: 0x0010E2A0 File Offset: 0x0010D2A0
			public static int SignedHIWORD(int n)
			{
				return (int)((short)(n >> 16 & 65535));
			}

			// Token: 0x06004A56 RID: 19030 RVA: 0x0010E2BC File Offset: 0x0010D2BC
			public static int SignedLOWORD(int n)
			{
				return (int)((short)(n & 65535));
			}

			// Token: 0x06004A57 RID: 19031 RVA: 0x0010E2D3 File Offset: 0x0010D2D3
			public static int GetPInvokeStringLength(string s)
			{
				if (s == null)
				{
					return 0;
				}
				if (Marshal.SystemDefaultCharSize == 2)
				{
					return s.Length;
				}
				if (s.Length == 0)
				{
					return 0;
				}
				if (s.IndexOf('\0') > -1)
				{
					return NativeMethods.Util.GetEmbeddedNullStringLengthAnsi(s);
				}
				return NativeMethods.Util.lstrlen(s);
			}

			// Token: 0x06004A58 RID: 19032 RVA: 0x0010E30C File Offset: 0x0010D30C
			private static int GetEmbeddedNullStringLengthAnsi(string s)
			{
				int num = s.IndexOf('\0');
				if (num > -1)
				{
					string s2 = s.Substring(0, num);
					string s3 = s.Substring(num + 1);
					return NativeMethods.Util.GetPInvokeStringLength(s2) + NativeMethods.Util.GetEmbeddedNullStringLengthAnsi(s3) + 1;
				}
				return NativeMethods.Util.GetPInvokeStringLength(s);
			}

			// Token: 0x06004A59 RID: 19033
			[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
			private static extern int lstrlen(string s);
		}

		// Token: 0x02000588 RID: 1416
		public enum tagTYPEKIND
		{
			// Token: 0x04003041 RID: 12353
			TKIND_ENUM,
			// Token: 0x04003042 RID: 12354
			TKIND_RECORD,
			// Token: 0x04003043 RID: 12355
			TKIND_MODULE,
			// Token: 0x04003044 RID: 12356
			TKIND_INTERFACE,
			// Token: 0x04003045 RID: 12357
			TKIND_DISPATCH,
			// Token: 0x04003046 RID: 12358
			TKIND_COCLASS,
			// Token: 0x04003047 RID: 12359
			TKIND_ALIAS,
			// Token: 0x04003048 RID: 12360
			TKIND_UNION,
			// Token: 0x04003049 RID: 12361
			TKIND_MAX
		}

		// Token: 0x02000589 RID: 1417
		[StructLayout(LayoutKind.Sequential)]
		public class tagTYPEDESC
		{
			// Token: 0x0400304A RID: 12362
			public IntPtr unionMember;

			// Token: 0x0400304B RID: 12363
			public short vt;
		}

		// Token: 0x0200058A RID: 1418
		public struct tagPARAMDESC
		{
			// Token: 0x0400304C RID: 12364
			public IntPtr pparamdescex;

			// Token: 0x0400304D RID: 12365
			[MarshalAs(UnmanagedType.U2)]
			public short wParamFlags;
		}

		// Token: 0x0200058B RID: 1419
		public sealed class CommonHandles
		{
			// Token: 0x0400304E RID: 12366
			public static readonly int Accelerator = System.Internal.HandleCollector.RegisterType("Accelerator", 80, 50);

			// Token: 0x0400304F RID: 12367
			public static readonly int Cursor = System.Internal.HandleCollector.RegisterType("Cursor", 20, 500);

			// Token: 0x04003050 RID: 12368
			public static readonly int EMF = System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x04003051 RID: 12369
			public static readonly int Find = System.Internal.HandleCollector.RegisterType("Find", 0, 1000);

			// Token: 0x04003052 RID: 12370
			public static readonly int GDI = System.Internal.HandleCollector.RegisterType("GDI", 50, 500);

			// Token: 0x04003053 RID: 12371
			public static readonly int HDC = System.Internal.HandleCollector.RegisterType("HDC", 100, 2);

			// Token: 0x04003054 RID: 12372
			public static readonly int CompatibleHDC = System.Internal.HandleCollector.RegisterType("ComptibleHDC", 50, 50);

			// Token: 0x04003055 RID: 12373
			public static readonly int Icon = System.Internal.HandleCollector.RegisterType("Icon", 20, 500);

			// Token: 0x04003056 RID: 12374
			public static readonly int Kernel = System.Internal.HandleCollector.RegisterType("Kernel", 0, 1000);

			// Token: 0x04003057 RID: 12375
			public static readonly int Menu = System.Internal.HandleCollector.RegisterType("Menu", 30, 1000);

			// Token: 0x04003058 RID: 12376
			public static readonly int Window = System.Internal.HandleCollector.RegisterType("Window", 5, 1000);
		}

		// Token: 0x0200058C RID: 1420
		public enum tagSYSKIND
		{
			// Token: 0x0400305A RID: 12378
			SYS_WIN16,
			// Token: 0x0400305B RID: 12379
			SYS_MAC = 2
		}

		// Token: 0x0200058D RID: 1421
		// (Invoke) Token: 0x06004A5E RID: 19038
		public delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);

		// Token: 0x0200058E RID: 1422
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("A7ABA9C1-8983-11cf-8F20-00805F2CD064")]
		[ComImport]
		public interface IProvideMultipleClassInfo
		{
			// Token: 0x06004A61 RID: 19041
			[PreserveSig]
			UnsafeNativeMethods.ITypeInfo GetClassInfo();

			// Token: 0x06004A62 RID: 19042
			[PreserveSig]
			int GetGUID(int dwGuidKind, [In] [Out] ref Guid pGuid);

			// Token: 0x06004A63 RID: 19043
			[PreserveSig]
			int GetMultiTypeInfoCount([In] [Out] ref int pcti);

			// Token: 0x06004A64 RID: 19044
			[PreserveSig]
			int GetInfoOfIndex(int iti, int dwFlags, [In] [Out] ref UnsafeNativeMethods.ITypeInfo pTypeInfo, int pTIFlags, int pcdispidReserved, IntPtr piidPrimary, IntPtr piidSource);
		}

		// Token: 0x0200058F RID: 1423
		[StructLayout(LayoutKind.Sequential)]
		public class EVENTMSG
		{
			// Token: 0x0400305C RID: 12380
			public int message;

			// Token: 0x0400305D RID: 12381
			public int paramL;

			// Token: 0x0400305E RID: 12382
			public int paramH;

			// Token: 0x0400305F RID: 12383
			public int time;

			// Token: 0x04003060 RID: 12384
			public IntPtr hwnd;
		}

		// Token: 0x02000590 RID: 1424
		[Guid("B196B283-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IProvideClassInfo
		{
			// Token: 0x06004A66 RID: 19046
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITypeInfo GetClassInfo();
		}

		// Token: 0x02000591 RID: 1425
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagTYPEATTR
		{
			// Token: 0x06004A67 RID: 19047 RVA: 0x0010E45C File Offset: 0x0010D45C
			public NativeMethods.tagTYPEDESC Get_tdescAlias()
			{
				return new NativeMethods.tagTYPEDESC
				{
					unionMember = (IntPtr)this.tdescAlias_unionMember,
					vt = this.tdescAlias_vt
				};
			}

			// Token: 0x06004A68 RID: 19048 RVA: 0x0010E490 File Offset: 0x0010D490
			public NativeMethods.tagIDLDESC Get_idldescType()
			{
				return new NativeMethods.tagIDLDESC
				{
					dwReserved = this.idldescType_dwReserved,
					wIDLFlags = this.idldescType_wIDLFlags
				};
			}

			// Token: 0x04003061 RID: 12385
			public Guid guid;

			// Token: 0x04003062 RID: 12386
			[MarshalAs(UnmanagedType.U4)]
			public int lcid;

			// Token: 0x04003063 RID: 12387
			[MarshalAs(UnmanagedType.U4)]
			public int dwReserved;

			// Token: 0x04003064 RID: 12388
			public int memidConstructor;

			// Token: 0x04003065 RID: 12389
			public int memidDestructor;

			// Token: 0x04003066 RID: 12390
			public IntPtr lpstrSchema = IntPtr.Zero;

			// Token: 0x04003067 RID: 12391
			[MarshalAs(UnmanagedType.U4)]
			public int cbSizeInstance;

			// Token: 0x04003068 RID: 12392
			public int typekind;

			// Token: 0x04003069 RID: 12393
			[MarshalAs(UnmanagedType.U2)]
			public short cFuncs;

			// Token: 0x0400306A RID: 12394
			[MarshalAs(UnmanagedType.U2)]
			public short cVars;

			// Token: 0x0400306B RID: 12395
			[MarshalAs(UnmanagedType.U2)]
			public short cImplTypes;

			// Token: 0x0400306C RID: 12396
			[MarshalAs(UnmanagedType.U2)]
			public short cbSizeVft;

			// Token: 0x0400306D RID: 12397
			[MarshalAs(UnmanagedType.U2)]
			public short cbAlignment;

			// Token: 0x0400306E RID: 12398
			[MarshalAs(UnmanagedType.U2)]
			public short wTypeFlags;

			// Token: 0x0400306F RID: 12399
			[MarshalAs(UnmanagedType.U2)]
			public short wMajorVerNum;

			// Token: 0x04003070 RID: 12400
			[MarshalAs(UnmanagedType.U2)]
			public short wMinorVerNum;

			// Token: 0x04003071 RID: 12401
			[MarshalAs(UnmanagedType.U4)]
			public int tdescAlias_unionMember;

			// Token: 0x04003072 RID: 12402
			[MarshalAs(UnmanagedType.U2)]
			public short tdescAlias_vt;

			// Token: 0x04003073 RID: 12403
			[MarshalAs(UnmanagedType.U4)]
			public int idldescType_dwReserved;

			// Token: 0x04003074 RID: 12404
			[MarshalAs(UnmanagedType.U2)]
			public short idldescType_wIDLFlags;
		}

		// Token: 0x02000592 RID: 1426
		public enum tagVARFLAGS
		{
			// Token: 0x04003076 RID: 12406
			VARFLAG_FREADONLY = 1,
			// Token: 0x04003077 RID: 12407
			VARFLAG_FSOURCE,
			// Token: 0x04003078 RID: 12408
			VARFLAG_FBINDABLE = 4,
			// Token: 0x04003079 RID: 12409
			VARFLAG_FREQUESTEDIT = 8,
			// Token: 0x0400307A RID: 12410
			VARFLAG_FDISPLAYBIND = 16,
			// Token: 0x0400307B RID: 12411
			VARFLAG_FDEFAULTBIND = 32,
			// Token: 0x0400307C RID: 12412
			VARFLAG_FHIDDEN = 64,
			// Token: 0x0400307D RID: 12413
			VARFLAG_FDEFAULTCOLLELEM = 256,
			// Token: 0x0400307E RID: 12414
			VARFLAG_FUIDEFAULT = 512,
			// Token: 0x0400307F RID: 12415
			VARFLAG_FNONBROWSABLE = 1024,
			// Token: 0x04003080 RID: 12416
			VARFLAG_FREPLACEABLE = 2048,
			// Token: 0x04003081 RID: 12417
			VARFLAG_FIMMEDIATEBIND = 4096
		}

		// Token: 0x02000593 RID: 1427
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagELEMDESC
		{
			// Token: 0x04003082 RID: 12418
			public NativeMethods.tagTYPEDESC tdesc;

			// Token: 0x04003083 RID: 12419
			public NativeMethods.tagPARAMDESC paramdesc;
		}

		// Token: 0x02000594 RID: 1428
		public enum tagVARKIND
		{
			// Token: 0x04003085 RID: 12421
			VAR_PERINSTANCE,
			// Token: 0x04003086 RID: 12422
			VAR_STATIC,
			// Token: 0x04003087 RID: 12423
			VAR_CONST,
			// Token: 0x04003088 RID: 12424
			VAR_DISPATCH
		}

		// Token: 0x02000595 RID: 1429
		public struct tagIDLDESC
		{
			// Token: 0x04003089 RID: 12425
			[MarshalAs(UnmanagedType.U4)]
			public int dwReserved;

			// Token: 0x0400308A RID: 12426
			[MarshalAs(UnmanagedType.U2)]
			public short wIDLFlags;
		}

		// Token: 0x02000596 RID: 1430
		public struct RGBQUAD
		{
			// Token: 0x0400308B RID: 12427
			public byte rgbBlue;

			// Token: 0x0400308C RID: 12428
			public byte rgbGreen;

			// Token: 0x0400308D RID: 12429
			public byte rgbRed;

			// Token: 0x0400308E RID: 12430
			public byte rgbReserved;
		}

		// Token: 0x02000597 RID: 1431
		public struct PALETTEENTRY
		{
			// Token: 0x0400308F RID: 12431
			public byte peRed;

			// Token: 0x04003090 RID: 12432
			public byte peGreen;

			// Token: 0x04003091 RID: 12433
			public byte peBlue;

			// Token: 0x04003092 RID: 12434
			public byte peFlags;
		}

		// Token: 0x02000598 RID: 1432
		public struct BITMAPINFO_FLAT
		{
			// Token: 0x04003093 RID: 12435
			public int bmiHeader_biSize;

			// Token: 0x04003094 RID: 12436
			public int bmiHeader_biWidth;

			// Token: 0x04003095 RID: 12437
			public int bmiHeader_biHeight;

			// Token: 0x04003096 RID: 12438
			public short bmiHeader_biPlanes;

			// Token: 0x04003097 RID: 12439
			public short bmiHeader_biBitCount;

			// Token: 0x04003098 RID: 12440
			public int bmiHeader_biCompression;

			// Token: 0x04003099 RID: 12441
			public int bmiHeader_biSizeImage;

			// Token: 0x0400309A RID: 12442
			public int bmiHeader_biXPelsPerMeter;

			// Token: 0x0400309B RID: 12443
			public int bmiHeader_biYPelsPerMeter;

			// Token: 0x0400309C RID: 12444
			public int bmiHeader_biClrUsed;

			// Token: 0x0400309D RID: 12445
			public int bmiHeader_biClrImportant;

			// Token: 0x0400309E RID: 12446
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
			public byte[] bmiColors;
		}

		// Token: 0x02000599 RID: 1433
		public struct SYSTEM_POWER_STATUS
		{
			// Token: 0x0400309F RID: 12447
			public byte ACLineStatus;

			// Token: 0x040030A0 RID: 12448
			public byte BatteryFlag;

			// Token: 0x040030A1 RID: 12449
			public byte BatteryLifePercent;

			// Token: 0x040030A2 RID: 12450
			public byte Reserved1;

			// Token: 0x040030A3 RID: 12451
			public int BatteryLifeTime;

			// Token: 0x040030A4 RID: 12452
			public int BatteryFullLifeTime;
		}

		// Token: 0x0200059A RID: 1434
		[StructLayout(LayoutKind.Sequential)]
		internal class DLLVERSIONINFO
		{
			// Token: 0x040030A5 RID: 12453
			internal uint cbSize;

			// Token: 0x040030A6 RID: 12454
			internal uint dwMajorVersion;

			// Token: 0x040030A7 RID: 12455
			internal uint dwMinorVersion;

			// Token: 0x040030A8 RID: 12456
			internal uint dwBuildNumber;

			// Token: 0x040030A9 RID: 12457
			internal uint dwPlatformID;
		}

		// Token: 0x0200059B RID: 1435
		public enum OLERENDER
		{
			// Token: 0x040030AB RID: 12459
			OLERENDER_NONE,
			// Token: 0x040030AC RID: 12460
			OLERENDER_DRAW,
			// Token: 0x040030AD RID: 12461
			OLERENDER_FORMAT,
			// Token: 0x040030AE RID: 12462
			OLERENDER_ASIS
		}
	}
}
