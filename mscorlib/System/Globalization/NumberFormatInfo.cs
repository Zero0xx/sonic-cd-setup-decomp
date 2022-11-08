using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x020003CE RID: 974
	[ComVisible(true)]
	[Serializable]
	public sealed class NumberFormatInfo : ICloneable, IFormatProvider
	{
		// Token: 0x060027C8 RID: 10184 RVA: 0x00077139 File Offset: 0x00076139
		public NumberFormatInfo() : this(null)
		{
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00077144 File Offset: 0x00076144
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if (this.numberDecimalSeparator != this.numberGroupSeparator)
			{
				this.validForParseAsNumber = true;
			}
			else
			{
				this.validForParseAsNumber = false;
			}
			if (this.numberDecimalSeparator != this.numberGroupSeparator && this.numberDecimalSeparator != this.currencyGroupSeparator && this.currencyDecimalSeparator != this.numberGroupSeparator && this.currencyDecimalSeparator != this.currencyGroupSeparator)
			{
				this.validForParseAsCurrency = true;
				return;
			}
			this.validForParseAsCurrency = false;
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000771CF File Offset: 0x000761CF
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.nativeDigits = null;
			this.digitSubstitution = -1;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000771E0 File Offset: 0x000761E0
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.nativeDigits == null)
			{
				this.nativeDigits = new string[]
				{
					"0",
					"1",
					"2",
					"3",
					"4",
					"5",
					"6",
					"7",
					"8",
					"9"
				};
			}
			if (this.digitSubstitution < 0)
			{
				this.digitSubstitution = 1;
			}
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00077265 File Offset: 0x00076265
		private void VerifyDecimalSeparator(string decSep, string propertyName)
		{
			if (decSep == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_String"));
			}
			if (decSep.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyDecString"));
			}
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x00077293 File Offset: 0x00076293
		private void VerifyGroupSeparator(string groupSep, string propertyName)
		{
			if (groupSep == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_String"));
			}
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000772AC File Offset: 0x000762AC
		private void VerifyNativeDigits(string[] nativeDig, string propertyName)
		{
			if (nativeDig == null)
			{
				throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (nativeDig.Length != 10)
			{
				throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitCount"));
			}
			for (int i = 0; i < nativeDig.Length; i++)
			{
				if (nativeDig[i] == null)
				{
					throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_ArrayValue"));
				}
				if (nativeDig[i].Length != 1)
				{
					if (nativeDig[i].Length != 2)
					{
						throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitValue"));
					}
					if (!char.IsSurrogatePair(nativeDig[i][0], nativeDig[i][1]))
					{
						throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitValue"));
					}
				}
				if (CharUnicodeInfo.GetDecimalDigitValue(nativeDig[i], 0) != i && CharUnicodeInfo.GetUnicodeCategory(nativeDig[i], 0) != UnicodeCategory.PrivateUse)
				{
					throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidNativeDigitValue"));
				}
			}
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x0007738C File Offset: 0x0007638C
		private void VerifyDigitSubstitution(DigitShapes digitSub, string propertyName)
		{
			switch (digitSub)
			{
			case DigitShapes.Context:
			case DigitShapes.None:
			case DigitShapes.NativeNational:
				return;
			default:
				throw new ArgumentException(propertyName, Environment.GetResourceString("Argument_InvalidDigitSubstitution"));
			}
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000773C0 File Offset: 0x000763C0
		internal NumberFormatInfo(CultureTableRecord cultureTableRecord)
		{
			if (cultureTableRecord != null)
			{
				cultureTableRecord.GetNFIOverrideValues(this);
				if (932 == cultureTableRecord.IDEFAULTANSICODEPAGE || 949 == cultureTableRecord.IDEFAULTANSICODEPAGE)
				{
					this.ansiCurrencySymbol = "\\";
				}
				this.negativeInfinitySymbol = cultureTableRecord.SNEGINFINITY;
				this.positiveInfinitySymbol = cultureTableRecord.SPOSINFINITY;
				this.nanSymbol = cultureTableRecord.SNAN;
			}
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x00077587 File Offset: 0x00076587
		private void VerifyWritable()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060027D2 RID: 10194 RVA: 0x000775A1 File Offset: 0x000765A1
		public static NumberFormatInfo InvariantInfo
		{
			get
			{
				if (NumberFormatInfo.invariantInfo == null)
				{
					NumberFormatInfo.invariantInfo = NumberFormatInfo.ReadOnly(new NumberFormatInfo());
				}
				return NumberFormatInfo.invariantInfo;
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000775C0 File Offset: 0x000765C0
		public static NumberFormatInfo GetInstance(IFormatProvider formatProvider)
		{
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo != null && !cultureInfo.m_isInherited)
			{
				NumberFormatInfo numberFormatInfo = cultureInfo.numInfo;
				if (numberFormatInfo != null)
				{
					return numberFormatInfo;
				}
				return cultureInfo.NumberFormat;
			}
			else
			{
				NumberFormatInfo numberFormatInfo = formatProvider as NumberFormatInfo;
				if (numberFormatInfo != null)
				{
					return numberFormatInfo;
				}
				if (formatProvider != null)
				{
					numberFormatInfo = (formatProvider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo);
					if (numberFormatInfo != null)
					{
						return numberFormatInfo;
					}
				}
				return NumberFormatInfo.CurrentInfo;
			}
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x00077624 File Offset: 0x00076624
		public object Clone()
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)base.MemberwiseClone();
			numberFormatInfo.isReadOnly = false;
			return numberFormatInfo;
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060027D5 RID: 10197 RVA: 0x00077645 File Offset: 0x00076645
		// (set) Token: 0x060027D6 RID: 10198 RVA: 0x00077650 File Offset: 0x00076650
		public int CurrencyDecimalDigits
		{
			get
			{
				return this.currencyDecimalDigits;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("CurrencyDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						99
					}));
				}
				this.currencyDecimalDigits = value;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000776AD File Offset: 0x000766AD
		// (set) Token: 0x060027D8 RID: 10200 RVA: 0x000776B5 File Offset: 0x000766B5
		public string CurrencyDecimalSeparator
		{
			get
			{
				return this.currencyDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDecimalSeparator(value, "CurrencyDecimalSeparator");
				this.currencyDecimalSeparator = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x000776D0 File Offset: 0x000766D0
		public bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000776D8 File Offset: 0x000766D8
		internal void CheckGroupSize(string propName, int[] groupSize)
		{
			int i = 0;
			while (i < groupSize.Length)
			{
				if (groupSize[i] < 1)
				{
					if (i == groupSize.Length - 1 && groupSize[i] == 0)
					{
						return;
					}
					throw new ArgumentException(propName, Environment.GetResourceString("Argument_InvalidGroupSize"));
				}
				else
				{
					if (groupSize[i] > 9)
					{
						throw new ArgumentException(propName, Environment.GetResourceString("Argument_InvalidGroupSize"));
					}
					i++;
				}
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x00077730 File Offset: 0x00076730
		// (set) Token: 0x060027DC RID: 10204 RVA: 0x00077744 File Offset: 0x00076744
		public int[] CurrencyGroupSizes
		{
			get
			{
				return (int[])this.currencyGroupSizes.Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("CurrencyGroupSizes", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				int[] groupSize = (int[])value.Clone();
				this.CheckGroupSize("CurrencyGroupSizes", groupSize);
				this.currencyGroupSizes = groupSize;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060027DD RID: 10205 RVA: 0x0007778E File Offset: 0x0007678E
		// (set) Token: 0x060027DE RID: 10206 RVA: 0x000777A0 File Offset: 0x000767A0
		public int[] NumberGroupSizes
		{
			get
			{
				return (int[])this.numberGroupSizes.Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NumberGroupSizes", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				int[] groupSize = (int[])value.Clone();
				this.CheckGroupSize("NumberGroupSizes", groupSize);
				this.numberGroupSizes = groupSize;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000777EA File Offset: 0x000767EA
		// (set) Token: 0x060027E0 RID: 10208 RVA: 0x000777FC File Offset: 0x000767FC
		public int[] PercentGroupSizes
		{
			get
			{
				return (int[])this.percentGroupSizes.Clone();
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PercentGroupSizes", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				int[] groupSize = (int[])value.Clone();
				this.CheckGroupSize("PercentGroupSizes", groupSize);
				this.percentGroupSizes = groupSize;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x00077846 File Offset: 0x00076846
		// (set) Token: 0x060027E2 RID: 10210 RVA: 0x0007784E File Offset: 0x0007684E
		public string CurrencyGroupSeparator
		{
			get
			{
				return this.currencyGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyGroupSeparator(value, "CurrencyGroupSeparator");
				this.currencyGroupSeparator = value;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060027E3 RID: 10211 RVA: 0x00077869 File Offset: 0x00076869
		// (set) Token: 0x060027E4 RID: 10212 RVA: 0x00077871 File Offset: 0x00076871
		public string CurrencySymbol
		{
			get
			{
				return this.currencySymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("CurrencySymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.currencySymbol = value;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060027E5 RID: 10213 RVA: 0x00077898 File Offset: 0x00076898
		public static NumberFormatInfo CurrentInfo
		{
			get
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				if (!currentCulture.m_isInherited)
				{
					NumberFormatInfo numInfo = currentCulture.numInfo;
					if (numInfo != null)
					{
						return numInfo;
					}
				}
				return (NumberFormatInfo)currentCulture.GetFormat(typeof(NumberFormatInfo));
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000778D9 File Offset: 0x000768D9
		// (set) Token: 0x060027E7 RID: 10215 RVA: 0x000778E1 File Offset: 0x000768E1
		public string NaNSymbol
		{
			get
			{
				return this.nanSymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NaNSymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.nanSymbol = value;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x00077908 File Offset: 0x00076908
		// (set) Token: 0x060027E9 RID: 10217 RVA: 0x00077910 File Offset: 0x00076910
		public int CurrencyNegativePattern
		{
			get
			{
				return this.currencyNegativePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 15)
				{
					throw new ArgumentOutOfRangeException("CurrencyNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						15
					}));
				}
				this.currencyNegativePattern = value;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x0007796D File Offset: 0x0007696D
		// (set) Token: 0x060027EB RID: 10219 RVA: 0x00077978 File Offset: 0x00076978
		public int NumberNegativePattern
		{
			get
			{
				return this.numberNegativePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 4)
				{
					throw new ArgumentOutOfRangeException("NumberNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						4
					}));
				}
				this.numberNegativePattern = value;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x000779D3 File Offset: 0x000769D3
		// (set) Token: 0x060027ED RID: 10221 RVA: 0x000779DC File Offset: 0x000769DC
		public int PercentPositivePattern
		{
			get
			{
				return this.percentPositivePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 3)
				{
					throw new ArgumentOutOfRangeException("PercentPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						3
					}));
				}
				this.percentPositivePattern = value;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060027EE RID: 10222 RVA: 0x00077A37 File Offset: 0x00076A37
		// (set) Token: 0x060027EF RID: 10223 RVA: 0x00077A40 File Offset: 0x00076A40
		public int PercentNegativePattern
		{
			get
			{
				return this.percentNegativePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 11)
				{
					throw new ArgumentOutOfRangeException("PercentNegativePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						11
					}));
				}
				this.percentNegativePattern = value;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x00077A9D File Offset: 0x00076A9D
		// (set) Token: 0x060027F1 RID: 10225 RVA: 0x00077AA5 File Offset: 0x00076AA5
		public string NegativeInfinitySymbol
		{
			get
			{
				return this.negativeInfinitySymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NegativeInfinitySymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.negativeInfinitySymbol = value;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x00077ACC File Offset: 0x00076ACC
		// (set) Token: 0x060027F3 RID: 10227 RVA: 0x00077AD4 File Offset: 0x00076AD4
		public string NegativeSign
		{
			get
			{
				return this.negativeSign;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("NegativeSign", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.negativeSign = value;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x00077AFB File Offset: 0x00076AFB
		// (set) Token: 0x060027F5 RID: 10229 RVA: 0x00077B04 File Offset: 0x00076B04
		public int NumberDecimalDigits
		{
			get
			{
				return this.numberDecimalDigits;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("NumberDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						99
					}));
				}
				this.numberDecimalDigits = value;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x00077B61 File Offset: 0x00076B61
		// (set) Token: 0x060027F7 RID: 10231 RVA: 0x00077B69 File Offset: 0x00076B69
		public string NumberDecimalSeparator
		{
			get
			{
				return this.numberDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDecimalSeparator(value, "NumberDecimalSeparator");
				this.numberDecimalSeparator = value;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x00077B84 File Offset: 0x00076B84
		// (set) Token: 0x060027F9 RID: 10233 RVA: 0x00077B8C File Offset: 0x00076B8C
		public string NumberGroupSeparator
		{
			get
			{
				return this.numberGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyGroupSeparator(value, "NumberGroupSeparator");
				this.numberGroupSeparator = value;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x00077BA7 File Offset: 0x00076BA7
		// (set) Token: 0x060027FB RID: 10235 RVA: 0x00077BB0 File Offset: 0x00076BB0
		public int CurrencyPositivePattern
		{
			get
			{
				return this.currencyPositivePattern;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 3)
				{
					throw new ArgumentOutOfRangeException("CurrencyPositivePattern", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						3
					}));
				}
				this.currencyPositivePattern = value;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x00077C0B File Offset: 0x00076C0B
		// (set) Token: 0x060027FD RID: 10237 RVA: 0x00077C13 File Offset: 0x00076C13
		public string PositiveInfinitySymbol
		{
			get
			{
				return this.positiveInfinitySymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PositiveInfinitySymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.positiveInfinitySymbol = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x00077C3A File Offset: 0x00076C3A
		// (set) Token: 0x060027FF RID: 10239 RVA: 0x00077C42 File Offset: 0x00076C42
		public string PositiveSign
		{
			get
			{
				return this.positiveSign;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PositiveSign", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.positiveSign = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002800 RID: 10240 RVA: 0x00077C69 File Offset: 0x00076C69
		// (set) Token: 0x06002801 RID: 10241 RVA: 0x00077C74 File Offset: 0x00076C74
		public int PercentDecimalDigits
		{
			get
			{
				return this.percentDecimalDigits;
			}
			set
			{
				this.VerifyWritable();
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("PercentDecimalDigits", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						0,
						99
					}));
				}
				this.percentDecimalDigits = value;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002802 RID: 10242 RVA: 0x00077CD1 File Offset: 0x00076CD1
		// (set) Token: 0x06002803 RID: 10243 RVA: 0x00077CD9 File Offset: 0x00076CD9
		public string PercentDecimalSeparator
		{
			get
			{
				return this.percentDecimalSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDecimalSeparator(value, "PercentDecimalSeparator");
				this.percentDecimalSeparator = value;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x00077CF4 File Offset: 0x00076CF4
		// (set) Token: 0x06002805 RID: 10245 RVA: 0x00077CFC File Offset: 0x00076CFC
		public string PercentGroupSeparator
		{
			get
			{
				return this.percentGroupSeparator;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyGroupSeparator(value, "PercentGroupSeparator");
				this.percentGroupSeparator = value;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x00077D17 File Offset: 0x00076D17
		// (set) Token: 0x06002807 RID: 10247 RVA: 0x00077D1F File Offset: 0x00076D1F
		public string PercentSymbol
		{
			get
			{
				return this.percentSymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PercentSymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.percentSymbol = value;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x00077D46 File Offset: 0x00076D46
		// (set) Token: 0x06002809 RID: 10249 RVA: 0x00077D4E File Offset: 0x00076D4E
		public string PerMilleSymbol
		{
			get
			{
				return this.perMilleSymbol;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("PerMilleSymbol", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.perMilleSymbol = value;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x00077D75 File Offset: 0x00076D75
		// (set) Token: 0x0600280B RID: 10251 RVA: 0x00077D7D File Offset: 0x00076D7D
		[ComVisible(false)]
		public string[] NativeDigits
		{
			get
			{
				return this.nativeDigits;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyNativeDigits(value, "NativeDigits");
				this.nativeDigits = value;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x00077D98 File Offset: 0x00076D98
		// (set) Token: 0x0600280D RID: 10253 RVA: 0x00077DA0 File Offset: 0x00076DA0
		[ComVisible(false)]
		public DigitShapes DigitSubstitution
		{
			get
			{
				return (DigitShapes)this.digitSubstitution;
			}
			set
			{
				this.VerifyWritable();
				this.VerifyDigitSubstitution(value, "DigitSubstitution");
				this.digitSubstitution = (int)value;
			}
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x00077DBB File Offset: 0x00076DBB
		public object GetFormat(Type formatType)
		{
			if (formatType != typeof(NumberFormatInfo))
			{
				return null;
			}
			return this;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x00077DD0 File Offset: 0x00076DD0
		public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi)
		{
			if (nfi == null)
			{
				throw new ArgumentNullException("nfi");
			}
			if (nfi.IsReadOnly)
			{
				return nfi;
			}
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)nfi.MemberwiseClone();
			numberFormatInfo.isReadOnly = true;
			return numberFormatInfo;
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x00077E0C File Offset: 0x00076E0C
		internal static void ValidateParseStyleInteger(NumberStyles style)
		{
			if ((style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNumberStyles"), "style");
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None && (style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidHexStyle"));
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x00077E59 File Offset: 0x00076E59
		internal static void ValidateParseStyleFloatingPoint(NumberStyles style)
		{
			if ((style & ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNumberStyles"), "style");
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_HexStyleNotSupported"));
			}
		}

		// Token: 0x0400121F RID: 4639
		private const NumberStyles InvalidNumberStyles = ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier);

		// Token: 0x04001220 RID: 4640
		private static NumberFormatInfo invariantInfo;

		// Token: 0x04001221 RID: 4641
		internal int[] numberGroupSizes = new int[]
		{
			3
		};

		// Token: 0x04001222 RID: 4642
		internal int[] currencyGroupSizes = new int[]
		{
			3
		};

		// Token: 0x04001223 RID: 4643
		internal int[] percentGroupSizes = new int[]
		{
			3
		};

		// Token: 0x04001224 RID: 4644
		internal string positiveSign = "+";

		// Token: 0x04001225 RID: 4645
		internal string negativeSign = "-";

		// Token: 0x04001226 RID: 4646
		internal string numberDecimalSeparator = ".";

		// Token: 0x04001227 RID: 4647
		internal string numberGroupSeparator = ",";

		// Token: 0x04001228 RID: 4648
		internal string currencyGroupSeparator = ",";

		// Token: 0x04001229 RID: 4649
		internal string currencyDecimalSeparator = ".";

		// Token: 0x0400122A RID: 4650
		internal string currencySymbol = "¤";

		// Token: 0x0400122B RID: 4651
		internal string ansiCurrencySymbol;

		// Token: 0x0400122C RID: 4652
		internal string nanSymbol = "NaN";

		// Token: 0x0400122D RID: 4653
		internal string positiveInfinitySymbol = "Infinity";

		// Token: 0x0400122E RID: 4654
		internal string negativeInfinitySymbol = "-Infinity";

		// Token: 0x0400122F RID: 4655
		internal string percentDecimalSeparator = ".";

		// Token: 0x04001230 RID: 4656
		internal string percentGroupSeparator = ",";

		// Token: 0x04001231 RID: 4657
		internal string percentSymbol = "%";

		// Token: 0x04001232 RID: 4658
		internal string perMilleSymbol = "‰";

		// Token: 0x04001233 RID: 4659
		[OptionalField(VersionAdded = 2)]
		internal string[] nativeDigits = new string[]
		{
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9"
		};

		// Token: 0x04001234 RID: 4660
		internal int m_dataItem;

		// Token: 0x04001235 RID: 4661
		internal int numberDecimalDigits = 2;

		// Token: 0x04001236 RID: 4662
		internal int currencyDecimalDigits = 2;

		// Token: 0x04001237 RID: 4663
		internal int currencyPositivePattern;

		// Token: 0x04001238 RID: 4664
		internal int currencyNegativePattern;

		// Token: 0x04001239 RID: 4665
		internal int numberNegativePattern = 1;

		// Token: 0x0400123A RID: 4666
		internal int percentPositivePattern;

		// Token: 0x0400123B RID: 4667
		internal int percentNegativePattern;

		// Token: 0x0400123C RID: 4668
		internal int percentDecimalDigits = 2;

		// Token: 0x0400123D RID: 4669
		[OptionalField(VersionAdded = 2)]
		internal int digitSubstitution = 1;

		// Token: 0x0400123E RID: 4670
		internal bool isReadOnly;

		// Token: 0x0400123F RID: 4671
		internal bool m_useUserOverride;

		// Token: 0x04001240 RID: 4672
		internal bool validForParseAsNumber = true;

		// Token: 0x04001241 RID: 4673
		internal bool validForParseAsCurrency = true;
	}
}
