using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Text
{
	// Token: 0x02000417 RID: 1047
	internal class Normalization
	{
		// Token: 0x06002AB4 RID: 10932 RVA: 0x00087E78 File Offset: 0x00086E78
		internal unsafe Normalization(NormalizationForm form, string strDataFile)
		{
			this.normalizationForm = form;
			if (!Normalization.nativeLoadNormalizationDLL())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNormalizationForm"));
			}
			byte* globalizationResourceBytePtr = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(Normalization).Assembly, strDataFile);
			if (globalizationResourceBytePtr == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNormalizationForm"));
			}
			byte* ptr = Normalization.nativeNormalizationInitNormalization(form, globalizationResourceBytePtr);
			if (ptr == null)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x00087EF4 File Offset: 0x00086EF4
		internal static Normalization GetNormalization(NormalizationForm form)
		{
			if (form <= (NormalizationForm)13)
			{
				switch (form)
				{
				case NormalizationForm.FormC:
					return Normalization.GetFormC();
				case NormalizationForm.FormD:
					return Normalization.GetFormD();
				case (NormalizationForm)3:
				case (NormalizationForm)4:
					break;
				case NormalizationForm.FormKC:
					return Normalization.GetFormKC();
				case NormalizationForm.FormKD:
					return Normalization.GetFormKD();
				default:
					if (form == (NormalizationForm)13)
					{
						return Normalization.GetFormIDNA();
					}
					break;
				}
			}
			else
			{
				switch (form)
				{
				case (NormalizationForm)257:
					return Normalization.GetFormCDisallowUnassigned();
				case (NormalizationForm)258:
					return Normalization.GetFormDDisallowUnassigned();
				case (NormalizationForm)259:
				case (NormalizationForm)260:
					break;
				case (NormalizationForm)261:
					return Normalization.GetFormKCDisallowUnassigned();
				case (NormalizationForm)262:
					return Normalization.GetFormKDDisallowUnassigned();
				default:
					if (form == (NormalizationForm)269)
					{
						return Normalization.GetFormIDNADisallowUnassigned();
					}
					break;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNormalizationForm"));
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x00087FA8 File Offset: 0x00086FA8
		internal static Normalization GetFormC()
		{
			if (Normalization.NFC != null)
			{
				return Normalization.NFC;
			}
			Normalization.NFC = new Normalization(NormalizationForm.FormC, "normnfc.nlp");
			return Normalization.NFC;
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x00087FCC File Offset: 0x00086FCC
		internal static Normalization GetFormD()
		{
			if (Normalization.NFD != null)
			{
				return Normalization.NFD;
			}
			Normalization.NFD = new Normalization(NormalizationForm.FormD, "normnfd.nlp");
			return Normalization.NFD;
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x00087FF0 File Offset: 0x00086FF0
		internal static Normalization GetFormKC()
		{
			if (Normalization.NFKC != null)
			{
				return Normalization.NFKC;
			}
			Normalization.NFKC = new Normalization(NormalizationForm.FormKC, "normnfkc.nlp");
			return Normalization.NFKC;
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x00088014 File Offset: 0x00087014
		internal static Normalization GetFormKD()
		{
			if (Normalization.NFKD != null)
			{
				return Normalization.NFKD;
			}
			Normalization.NFKD = new Normalization(NormalizationForm.FormKD, "normnfkd.nlp");
			return Normalization.NFKD;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x00088038 File Offset: 0x00087038
		internal static Normalization GetFormIDNA()
		{
			if (Normalization.IDNA != null)
			{
				return Normalization.IDNA;
			}
			Normalization.IDNA = new Normalization((NormalizationForm)13, "normidna.nlp");
			return Normalization.IDNA;
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x0008805D File Offset: 0x0008705D
		internal static Normalization GetFormCDisallowUnassigned()
		{
			if (Normalization.NFCDisallowUnassigned != null)
			{
				return Normalization.NFCDisallowUnassigned;
			}
			Normalization.NFCDisallowUnassigned = new Normalization((NormalizationForm)257, "normnfc.nlp");
			return Normalization.NFCDisallowUnassigned;
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x00088085 File Offset: 0x00087085
		internal static Normalization GetFormDDisallowUnassigned()
		{
			if (Normalization.NFDDisallowUnassigned != null)
			{
				return Normalization.NFDDisallowUnassigned;
			}
			Normalization.NFDDisallowUnassigned = new Normalization((NormalizationForm)258, "normnfd.nlp");
			return Normalization.NFDDisallowUnassigned;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000880AD File Offset: 0x000870AD
		internal static Normalization GetFormKCDisallowUnassigned()
		{
			if (Normalization.NFKCDisallowUnassigned != null)
			{
				return Normalization.NFKCDisallowUnassigned;
			}
			Normalization.NFKCDisallowUnassigned = new Normalization((NormalizationForm)261, "normnfkc.nlp");
			return Normalization.NFKCDisallowUnassigned;
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000880D5 File Offset: 0x000870D5
		internal static Normalization GetFormKDDisallowUnassigned()
		{
			if (Normalization.NFKDDisallowUnassigned != null)
			{
				return Normalization.NFKDDisallowUnassigned;
			}
			Normalization.NFKDDisallowUnassigned = new Normalization((NormalizationForm)262, "normnfkd.nlp");
			return Normalization.NFKDDisallowUnassigned;
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000880FD File Offset: 0x000870FD
		internal static Normalization GetFormIDNADisallowUnassigned()
		{
			if (Normalization.IDNADisallowUnassigned != null)
			{
				return Normalization.IDNADisallowUnassigned;
			}
			Normalization.IDNADisallowUnassigned = new Normalization((NormalizationForm)269, "normidna.nlp");
			return Normalization.IDNADisallowUnassigned;
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x00088125 File Offset: 0x00087125
		internal static bool IsNormalized(string strInput, NormalizationForm normForm)
		{
			return Normalization.GetNormalization(normForm).IsNormalized(strInput);
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x00088134 File Offset: 0x00087134
		private bool IsNormalized(string strInput)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException(Environment.GetResourceString("ArgumentNull_String"), "strInput");
			}
			int num = 0;
			int num2 = Normalization.nativeNormalizationIsNormalizedString(this.normalizationForm, ref num, strInput, strInput.Length);
			int num3 = num;
			if (num3 == 0)
			{
				return (num2 & 1) == 1;
			}
			if (num3 == 8)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
			if (num3 == 1113)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"), "strInput");
			}
			throw new InvalidOperationException(Environment.GetResourceString("UnknownError_Num", new object[]
			{
				num
			}));
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000881CE File Offset: 0x000871CE
		internal static string Normalize(string strInput, NormalizationForm normForm)
		{
			return Normalization.GetNormalization(normForm).Normalize(strInput);
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000881DC File Offset: 0x000871DC
		internal string Normalize(string strInput)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException("strInput", Environment.GetResourceString("ArgumentNull_String"));
			}
			int num = this.GuessLength(strInput);
			if (num == 0)
			{
				return string.Empty;
			}
			char[] array = null;
			int num2 = 122;
			while (num2 == 122)
			{
				array = new char[num];
				num = Normalization.nativeNormalizationNormalizeString(this.normalizationForm, ref num2, strInput, strInput.Length, array, array.Length);
				if (num2 != 0)
				{
					int num3 = num2;
					if (num3 <= 87)
					{
						if (num3 == 8)
						{
							throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
						}
						if (num3 != 87)
						{
						}
					}
					else
					{
						if (num3 == 122)
						{
							continue;
						}
						if (num3 == 1113)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequence", new object[]
							{
								num
							}), "strInput");
						}
					}
					throw new InvalidOperationException(Environment.GetResourceString("UnknownError_Num", new object[]
					{
						num2
					}));
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000882D8 File Offset: 0x000872D8
		internal int GuessLength(string strInput)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException("strInput", Environment.GetResourceString("ArgumentNull_String"));
			}
			int num = 0;
			int result = Normalization.nativeNormalizationNormalizeString(this.normalizationForm, ref num, strInput, strInput.Length, null, 0);
			if (num == 0)
			{
				return result;
			}
			if (num == 8)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
			throw new InvalidOperationException(Environment.GetResourceString("UnknownError_Num", new object[]
			{
				num
			}));
		}

		// Token: 0x06002AC5 RID: 10949
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeLoadNormalizationDLL();

		// Token: 0x06002AC6 RID: 10950
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeNormalizationNormalizeString(NormalizationForm NormForm, ref int iError, string lpSrcString, int cwSrcLength, char[] lpDstString, int cwDstLength);

		// Token: 0x06002AC7 RID: 10951
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeNormalizationIsNormalizedString(NormalizationForm NormForm, ref int iError, string lpString, int cwLength);

		// Token: 0x06002AC8 RID: 10952
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern byte* nativeNormalizationInitNormalization(NormalizationForm NormForm, byte* pTableData);

		// Token: 0x040014EF RID: 5359
		private const int ERROR_SUCCESS = 0;

		// Token: 0x040014F0 RID: 5360
		private const int ERROR_NOT_ENOUGH_MEMORY = 8;

		// Token: 0x040014F1 RID: 5361
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x040014F2 RID: 5362
		private const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x040014F3 RID: 5363
		private const int ERROR_NO_UNICODE_TRANSLATION = 1113;

		// Token: 0x040014F4 RID: 5364
		private static Normalization NFC;

		// Token: 0x040014F5 RID: 5365
		private static Normalization NFD;

		// Token: 0x040014F6 RID: 5366
		private static Normalization NFKC;

		// Token: 0x040014F7 RID: 5367
		private static Normalization NFKD;

		// Token: 0x040014F8 RID: 5368
		private static Normalization IDNA;

		// Token: 0x040014F9 RID: 5369
		private static Normalization NFCDisallowUnassigned;

		// Token: 0x040014FA RID: 5370
		private static Normalization NFDDisallowUnassigned;

		// Token: 0x040014FB RID: 5371
		private static Normalization NFKCDisallowUnassigned;

		// Token: 0x040014FC RID: 5372
		private static Normalization NFKDDisallowUnassigned;

		// Token: 0x040014FD RID: 5373
		private static Normalization IDNADisallowUnassigned;

		// Token: 0x040014FE RID: 5374
		private NormalizationForm normalizationForm;
	}
}
