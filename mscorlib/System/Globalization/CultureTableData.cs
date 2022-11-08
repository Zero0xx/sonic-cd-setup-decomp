using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003D8 RID: 984
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CultureTableData
	{
		// Token: 0x0400129C RID: 4764
		internal const int sizeofDataFields = 304;

		// Token: 0x0400129D RID: 4765
		internal const int LOCALE_IDIGITS = 17;

		// Token: 0x0400129E RID: 4766
		internal const int LOCALE_INEGNUMBER = 4112;

		// Token: 0x0400129F RID: 4767
		internal const int LOCALE_ICURRDIGITS = 25;

		// Token: 0x040012A0 RID: 4768
		internal const int LOCALE_ICURRENCY = 27;

		// Token: 0x040012A1 RID: 4769
		internal const int LOCALE_INEGCURR = 28;

		// Token: 0x040012A2 RID: 4770
		internal const int LOCALE_ILZERO = 18;

		// Token: 0x040012A3 RID: 4771
		internal const int LOCALE_IFIRSTDAYOFWEEK = 4108;

		// Token: 0x040012A4 RID: 4772
		internal const int LOCALE_IFIRSTWEEKOFYEAR = 4109;

		// Token: 0x040012A5 RID: 4773
		internal const int LOCALE_ICOUNTRY = 5;

		// Token: 0x040012A6 RID: 4774
		internal const int LOCALE_IMEASURE = 13;

		// Token: 0x040012A7 RID: 4775
		internal const int LOCALE_IDIGITSUBSTITUTION = 4116;

		// Token: 0x040012A8 RID: 4776
		internal const int LOCALE_SGROUPING = 16;

		// Token: 0x040012A9 RID: 4777
		internal const int LOCALE_SMONGROUPING = 24;

		// Token: 0x040012AA RID: 4778
		internal const int LOCALE_SLIST = 12;

		// Token: 0x040012AB RID: 4779
		internal const int LOCALE_SDECIMAL = 14;

		// Token: 0x040012AC RID: 4780
		internal const int LOCALE_STHOUSAND = 15;

		// Token: 0x040012AD RID: 4781
		internal const int LOCALE_SCURRENCY = 20;

		// Token: 0x040012AE RID: 4782
		internal const int LOCALE_SMONDECIMALSEP = 22;

		// Token: 0x040012AF RID: 4783
		internal const int LOCALE_SMONTHOUSANDSEP = 23;

		// Token: 0x040012B0 RID: 4784
		internal const int LOCALE_SPOSITIVESIGN = 80;

		// Token: 0x040012B1 RID: 4785
		internal const int LOCALE_SNEGATIVESIGN = 81;

		// Token: 0x040012B2 RID: 4786
		internal const int LOCALE_S1159 = 40;

		// Token: 0x040012B3 RID: 4787
		internal const int LOCALE_S2359 = 41;

		// Token: 0x040012B4 RID: 4788
		internal const int LOCALE_SNATIVEDIGITS = 19;

		// Token: 0x040012B5 RID: 4789
		internal const int LOCALE_STIMEFORMAT = 4099;

		// Token: 0x040012B6 RID: 4790
		internal const int LOCALE_SSHORTDATE = 31;

		// Token: 0x040012B7 RID: 4791
		internal const int LOCALE_SLONGDATE = 32;

		// Token: 0x040012B8 RID: 4792
		internal const int LOCALE_SYEARMONTH = 4102;

		// Token: 0x040012B9 RID: 4793
		internal uint sName;

		// Token: 0x040012BA RID: 4794
		internal uint sUnused;

		// Token: 0x040012BB RID: 4795
		internal ushort iLanguage;

		// Token: 0x040012BC RID: 4796
		internal ushort iParent;

		// Token: 0x040012BD RID: 4797
		internal ushort iDigits;

		// Token: 0x040012BE RID: 4798
		internal ushort iNegativeNumber;

		// Token: 0x040012BF RID: 4799
		internal ushort iCurrencyDigits;

		// Token: 0x040012C0 RID: 4800
		internal ushort iCurrency;

		// Token: 0x040012C1 RID: 4801
		internal ushort iNegativeCurrency;

		// Token: 0x040012C2 RID: 4802
		internal ushort iLeadingZeros;

		// Token: 0x040012C3 RID: 4803
		internal ushort iFlags;

		// Token: 0x040012C4 RID: 4804
		internal ushort iFirstDayOfWeek;

		// Token: 0x040012C5 RID: 4805
		internal ushort iFirstWeekOfYear;

		// Token: 0x040012C6 RID: 4806
		internal ushort iCountry;

		// Token: 0x040012C7 RID: 4807
		internal ushort iMeasure;

		// Token: 0x040012C8 RID: 4808
		internal ushort iDigitSubstitution;

		// Token: 0x040012C9 RID: 4809
		internal uint waGrouping;

		// Token: 0x040012CA RID: 4810
		internal uint waMonetaryGrouping;

		// Token: 0x040012CB RID: 4811
		internal uint sListSeparator;

		// Token: 0x040012CC RID: 4812
		internal uint sDecimalSeparator;

		// Token: 0x040012CD RID: 4813
		internal uint sThousandSeparator;

		// Token: 0x040012CE RID: 4814
		internal uint sCurrency;

		// Token: 0x040012CF RID: 4815
		internal uint sMonetaryDecimal;

		// Token: 0x040012D0 RID: 4816
		internal uint sMonetaryThousand;

		// Token: 0x040012D1 RID: 4817
		internal uint sPositiveSign;

		// Token: 0x040012D2 RID: 4818
		internal uint sNegativeSign;

		// Token: 0x040012D3 RID: 4819
		internal uint sAM1159;

		// Token: 0x040012D4 RID: 4820
		internal uint sPM2359;

		// Token: 0x040012D5 RID: 4821
		internal uint saNativeDigits;

		// Token: 0x040012D6 RID: 4822
		internal uint saTimeFormat;

		// Token: 0x040012D7 RID: 4823
		internal uint saShortDate;

		// Token: 0x040012D8 RID: 4824
		internal uint saLongDate;

		// Token: 0x040012D9 RID: 4825
		internal uint saYearMonth;

		// Token: 0x040012DA RID: 4826
		internal uint saDuration;

		// Token: 0x040012DB RID: 4827
		internal ushort iDefaultLanguage;

		// Token: 0x040012DC RID: 4828
		internal ushort iDefaultAnsiCodePage;

		// Token: 0x040012DD RID: 4829
		internal ushort iDefaultOemCodePage;

		// Token: 0x040012DE RID: 4830
		internal ushort iDefaultMacCodePage;

		// Token: 0x040012DF RID: 4831
		internal ushort iDefaultEbcdicCodePage;

		// Token: 0x040012E0 RID: 4832
		internal ushort iGeoId;

		// Token: 0x040012E1 RID: 4833
		internal ushort iPaperSize;

		// Token: 0x040012E2 RID: 4834
		internal ushort iIntlCurrencyDigits;

		// Token: 0x040012E3 RID: 4835
		internal uint waCalendars;

		// Token: 0x040012E4 RID: 4836
		internal uint sAbbrevLang;

		// Token: 0x040012E5 RID: 4837
		internal uint sISO639Language;

		// Token: 0x040012E6 RID: 4838
		internal uint sEnglishLanguage;

		// Token: 0x040012E7 RID: 4839
		internal uint sNativeLanguage;

		// Token: 0x040012E8 RID: 4840
		internal uint sEnglishCountry;

		// Token: 0x040012E9 RID: 4841
		internal uint sNativeCountry;

		// Token: 0x040012EA RID: 4842
		internal uint sAbbrevCountry;

		// Token: 0x040012EB RID: 4843
		internal uint sISO3166CountryName;

		// Token: 0x040012EC RID: 4844
		internal uint sIntlMonetarySymbol;

		// Token: 0x040012ED RID: 4845
		internal uint sEnglishCurrency;

		// Token: 0x040012EE RID: 4846
		internal uint sNativeCurrency;

		// Token: 0x040012EF RID: 4847
		internal uint waFontSignature;

		// Token: 0x040012F0 RID: 4848
		internal uint sISO639Language2;

		// Token: 0x040012F1 RID: 4849
		internal uint sISO3166CountryName2;

		// Token: 0x040012F2 RID: 4850
		internal uint sParent;

		// Token: 0x040012F3 RID: 4851
		internal uint saDayNames;

		// Token: 0x040012F4 RID: 4852
		internal uint saAbbrevDayNames;

		// Token: 0x040012F5 RID: 4853
		internal uint saMonthNames;

		// Token: 0x040012F6 RID: 4854
		internal uint saAbbrevMonthNames;

		// Token: 0x040012F7 RID: 4855
		internal uint saMonthGenitiveNames;

		// Token: 0x040012F8 RID: 4856
		internal uint saAbbrevMonthGenitiveNames;

		// Token: 0x040012F9 RID: 4857
		internal uint saNativeCalendarNames;

		// Token: 0x040012FA RID: 4858
		internal uint saAltSortID;

		// Token: 0x040012FB RID: 4859
		internal ushort iNegativePercent;

		// Token: 0x040012FC RID: 4860
		internal ushort iPositivePercent;

		// Token: 0x040012FD RID: 4861
		internal ushort iFormatFlags;

		// Token: 0x040012FE RID: 4862
		internal ushort iLineOrientations;

		// Token: 0x040012FF RID: 4863
		internal ushort iTextInfo;

		// Token: 0x04001300 RID: 4864
		internal ushort iInputLanguageHandle;

		// Token: 0x04001301 RID: 4865
		internal uint iCompareInfo;

		// Token: 0x04001302 RID: 4866
		internal uint sEnglishDisplayName;

		// Token: 0x04001303 RID: 4867
		internal uint sNativeDisplayName;

		// Token: 0x04001304 RID: 4868
		internal uint sPercent;

		// Token: 0x04001305 RID: 4869
		internal uint sNaN;

		// Token: 0x04001306 RID: 4870
		internal uint sPositiveInfinity;

		// Token: 0x04001307 RID: 4871
		internal uint sNegativeInfinity;

		// Token: 0x04001308 RID: 4872
		internal uint sMonthDay;

		// Token: 0x04001309 RID: 4873
		internal uint sAdEra;

		// Token: 0x0400130A RID: 4874
		internal uint sAbbrevAdEra;

		// Token: 0x0400130B RID: 4875
		internal uint sRegionName;

		// Token: 0x0400130C RID: 4876
		internal uint sConsoleFallbackName;

		// Token: 0x0400130D RID: 4877
		internal uint saShortTime;

		// Token: 0x0400130E RID: 4878
		internal uint saSuperShortDayNames;

		// Token: 0x0400130F RID: 4879
		internal uint saDateWords;

		// Token: 0x04001310 RID: 4880
		internal uint sSpecificCulture;

		// Token: 0x04001311 RID: 4881
		internal uint sKeyboardsToInstall;

		// Token: 0x04001312 RID: 4882
		internal uint sScripts;
	}
}
