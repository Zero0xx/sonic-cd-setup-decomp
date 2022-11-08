using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Permissions;
using System.Text;

namespace System.ComponentModel
{
	// Token: 0x0200011B RID: 283
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class MaskedTextProvider : ICloneable
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x0001D1FF File Offset: 0x0001C1FF
		public MaskedTextProvider(string mask) : this(mask, null, true, '_', '\0', false)
		{
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001D20E File Offset: 0x0001C20E
		public MaskedTextProvider(string mask, bool restrictToAscii) : this(mask, null, true, '_', '\0', restrictToAscii)
		{
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001D21D File Offset: 0x0001C21D
		public MaskedTextProvider(string mask, CultureInfo culture) : this(mask, culture, true, '_', '\0', false)
		{
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001D22C File Offset: 0x0001C22C
		public MaskedTextProvider(string mask, CultureInfo culture, bool restrictToAscii) : this(mask, culture, true, '_', '\0', restrictToAscii)
		{
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001D23B File Offset: 0x0001C23B
		public MaskedTextProvider(string mask, char passwordChar, bool allowPromptAsInput) : this(mask, null, allowPromptAsInput, '_', passwordChar, false)
		{
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001D24A File Offset: 0x0001C24A
		public MaskedTextProvider(string mask, CultureInfo culture, char passwordChar, bool allowPromptAsInput) : this(mask, culture, allowPromptAsInput, '_', passwordChar, false)
		{
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001D25C File Offset: 0x0001C25C
		public MaskedTextProvider(string mask, CultureInfo culture, bool allowPromptAsInput, char promptChar, char passwordChar, bool restrictToAscii)
		{
			if (string.IsNullOrEmpty(mask))
			{
				throw new ArgumentException(SR.GetString("MaskedTextProviderMaskNullOrEmpty"), "mask");
			}
			foreach (char c in mask)
			{
				if (!MaskedTextProvider.IsPrintableChar(c))
				{
					throw new ArgumentException(SR.GetString("MaskedTextProviderMaskInvalidChar"));
				}
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			this.flagState = default(BitVector32);
			this.mask = mask;
			this.promptChar = promptChar;
			this.passwordChar = passwordChar;
			if (culture.IsNeutralCulture)
			{
				foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
				{
					if (culture.Equals(cultureInfo.Parent))
					{
						this.culture = cultureInfo;
						break;
					}
				}
				if (this.culture == null)
				{
					this.culture = CultureInfo.InvariantCulture;
				}
			}
			else
			{
				this.culture = culture;
			}
			if (!this.culture.IsReadOnly)
			{
				this.culture = CultureInfo.ReadOnly(this.culture);
			}
			this.flagState[MaskedTextProvider.ALLOW_PROMPT_AS_INPUT] = allowPromptAsInput;
			this.flagState[MaskedTextProvider.ASCII_ONLY] = restrictToAscii;
			this.flagState[MaskedTextProvider.INCLUDE_PROMPT] = false;
			this.flagState[MaskedTextProvider.INCLUDE_LITERALS] = true;
			this.flagState[MaskedTextProvider.RESET_ON_PROMPT] = true;
			this.flagState[MaskedTextProvider.SKIP_SPACE] = true;
			this.flagState[MaskedTextProvider.RESET_ON_LITERALS] = true;
			this.Initialize();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001D3E4 File Offset: 0x0001C3E4
		private void Initialize()
		{
			this.testString = new StringBuilder();
			this.stringDescriptor = new List<MaskedTextProvider.CharDescriptor>();
			MaskedTextProvider.CaseConversion caseConversion = MaskedTextProvider.CaseConversion.None;
			bool flag = false;
			int num = 0;
			MaskedTextProvider.CharType charType = MaskedTextProvider.CharType.Literal;
			string text = string.Empty;
			int i = 0;
			while (i < this.mask.Length)
			{
				char c = this.mask[i];
				if (!flag)
				{
					char c2 = c;
					if (c2 <= 'C')
					{
						switch (c2)
						{
						case '#':
							goto IL_1A2;
						case '$':
							text = this.culture.NumberFormat.CurrencySymbol;
							charType = MaskedTextProvider.CharType.Separator;
							goto IL_1C2;
						case '%':
							goto IL_1BC;
						case '&':
							break;
						default:
							switch (c2)
							{
							case ',':
								text = this.culture.NumberFormat.NumberGroupSeparator;
								charType = MaskedTextProvider.CharType.Separator;
								goto IL_1C2;
							case '-':
								goto IL_1BC;
							case '.':
								text = this.culture.NumberFormat.NumberDecimalSeparator;
								charType = MaskedTextProvider.CharType.Separator;
								goto IL_1C2;
							case '/':
								text = this.culture.DateTimeFormat.DateSeparator;
								charType = MaskedTextProvider.CharType.Separator;
								goto IL_1C2;
							case '0':
								break;
							default:
								switch (c2)
								{
								case '9':
								case '?':
								case 'C':
									goto IL_1A2;
								case ':':
									text = this.culture.DateTimeFormat.TimeSeparator;
									charType = MaskedTextProvider.CharType.Separator;
									goto IL_1C2;
								case ';':
								case '=':
								case '@':
								case 'B':
									goto IL_1BC;
								case '<':
									caseConversion = MaskedTextProvider.CaseConversion.ToLower;
									goto IL_22E;
								case '>':
									caseConversion = MaskedTextProvider.CaseConversion.ToUpper;
									goto IL_22E;
								case 'A':
									break;
								default:
									goto IL_1BC;
								}
								break;
							}
							break;
						}
					}
					else if (c2 <= '\\')
					{
						if (c2 != 'L')
						{
							if (c2 != '\\')
							{
								goto IL_1BC;
							}
							flag = true;
							charType = MaskedTextProvider.CharType.Literal;
							goto IL_22E;
						}
					}
					else
					{
						if (c2 == 'a')
						{
							goto IL_1A2;
						}
						if (c2 != '|')
						{
							goto IL_1BC;
						}
						caseConversion = MaskedTextProvider.CaseConversion.None;
						goto IL_22E;
					}
					this.requiredEditChars++;
					c = this.promptChar;
					charType = MaskedTextProvider.CharType.EditRequired;
					goto IL_1C2;
					IL_1A2:
					this.optionalEditChars++;
					c = this.promptChar;
					charType = MaskedTextProvider.CharType.EditOptional;
					goto IL_1C2;
					IL_1BC:
					charType = MaskedTextProvider.CharType.Literal;
					goto IL_1C2;
				}
				flag = false;
				goto IL_1C2;
				IL_22E:
				i++;
				continue;
				IL_1C2:
				MaskedTextProvider.CharDescriptor charDescriptor = new MaskedTextProvider.CharDescriptor(i, charType);
				if (MaskedTextProvider.IsEditPosition(charDescriptor))
				{
					charDescriptor.CaseConversion = caseConversion;
				}
				if (charType != MaskedTextProvider.CharType.Separator)
				{
					text = c.ToString();
				}
				foreach (char value in text)
				{
					this.testString.Append(value);
					this.stringDescriptor.Add(charDescriptor);
					num++;
				}
				goto IL_22E;
			}
			this.testString.Capacity = this.testString.Length;
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001D64D File Offset: 0x0001C64D
		public bool AllowPromptAsInput
		{
			get
			{
				return this.flagState[MaskedTextProvider.ALLOW_PROMPT_AS_INPUT];
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0001D65F File Offset: 0x0001C65F
		public int AssignedEditPositionCount
		{
			get
			{
				return this.assignedCharCount;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0001D667 File Offset: 0x0001C667
		public int AvailableEditPositionCount
		{
			get
			{
				return this.EditPositionCount - this.assignedCharCount;
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001D678 File Offset: 0x0001C678
		public object Clone()
		{
			Type type = base.GetType();
			MaskedTextProvider maskedTextProvider;
			if (type == MaskedTextProvider.maskTextProviderType)
			{
				maskedTextProvider = new MaskedTextProvider(this.Mask, this.Culture, this.AllowPromptAsInput, this.PromptChar, this.PasswordChar, this.AsciiOnly);
			}
			else
			{
				object[] args = new object[]
				{
					this.Mask,
					this.Culture,
					this.AllowPromptAsInput,
					this.PromptChar,
					this.PasswordChar,
					this.AsciiOnly
				};
				maskedTextProvider = (SecurityUtils.SecureCreateInstance(type, args) as MaskedTextProvider);
			}
			maskedTextProvider.ResetOnPrompt = false;
			maskedTextProvider.ResetOnSpace = false;
			maskedTextProvider.SkipLiterals = false;
			for (int i = 0; i < this.testString.Length; i++)
			{
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
				if (MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned)
				{
					maskedTextProvider.Replace(this.testString[i], i);
				}
			}
			maskedTextProvider.ResetOnPrompt = this.ResetOnPrompt;
			maskedTextProvider.ResetOnSpace = this.ResetOnSpace;
			maskedTextProvider.SkipLiterals = this.SkipLiterals;
			maskedTextProvider.IncludeLiterals = this.IncludeLiterals;
			maskedTextProvider.IncludePrompt = this.IncludePrompt;
			return maskedTextProvider;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0001D7C4 File Offset: 0x0001C7C4
		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0001D7CC File Offset: 0x0001C7CC
		public static char DefaultPasswordChar
		{
			get
			{
				return '*';
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001D7D0 File Offset: 0x0001C7D0
		public int EditPositionCount
		{
			get
			{
				return this.optionalEditChars + this.requiredEditChars;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0001D7E0 File Offset: 0x0001C7E0
		public IEnumerator EditPositions
		{
			get
			{
				List<int> list = new List<int>();
				int num = 0;
				foreach (MaskedTextProvider.CharDescriptor charDescriptor in this.stringDescriptor)
				{
					if (MaskedTextProvider.IsEditPosition(charDescriptor))
					{
						list.Add(num);
					}
					num++;
				}
				return ((IEnumerable)list).GetEnumerator();
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0001D850 File Offset: 0x0001C850
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0001D862 File Offset: 0x0001C862
		public bool IncludeLiterals
		{
			get
			{
				return this.flagState[MaskedTextProvider.INCLUDE_LITERALS];
			}
			set
			{
				this.flagState[MaskedTextProvider.INCLUDE_LITERALS] = value;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0001D875 File Offset: 0x0001C875
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0001D887 File Offset: 0x0001C887
		public bool IncludePrompt
		{
			get
			{
				return this.flagState[MaskedTextProvider.INCLUDE_PROMPT];
			}
			set
			{
				this.flagState[MaskedTextProvider.INCLUDE_PROMPT] = value;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0001D89A File Offset: 0x0001C89A
		public bool AsciiOnly
		{
			get
			{
				return this.flagState[MaskedTextProvider.ASCII_ONLY];
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x0001D8AC File Offset: 0x0001C8AC
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x0001D8BA File Offset: 0x0001C8BA
		public bool IsPassword
		{
			get
			{
				return this.passwordChar != '\0';
			}
			set
			{
				if (this.IsPassword != value)
				{
					this.passwordChar = (value ? MaskedTextProvider.DefaultPasswordChar : '\0');
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0001D8D6 File Offset: 0x0001C8D6
		public static int InvalidIndex
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0001D8D9 File Offset: 0x0001C8D9
		public int LastAssignedPosition
		{
			get
			{
				return this.FindAssignedEditPositionFrom(this.testString.Length - 1, false);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0001D8EF File Offset: 0x0001C8EF
		public int Length
		{
			get
			{
				return this.testString.Length;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0001D8FC File Offset: 0x0001C8FC
		public string Mask
		{
			get
			{
				return this.mask;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0001D904 File Offset: 0x0001C904
		public bool MaskCompleted
		{
			get
			{
				return this.requiredCharCount == this.requiredEditChars;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0001D914 File Offset: 0x0001C914
		public bool MaskFull
		{
			get
			{
				return this.assignedCharCount == this.EditPositionCount;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x0001D924 File Offset: 0x0001C924
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x0001D92C File Offset: 0x0001C92C
		public char PasswordChar
		{
			get
			{
				return this.passwordChar;
			}
			set
			{
				if (value == this.promptChar)
				{
					throw new InvalidOperationException(SR.GetString("MaskedTextProviderPasswordAndPromptCharError"));
				}
				if (!MaskedTextProvider.IsValidPasswordChar(value) && value != '\0')
				{
					throw new ArgumentException(SR.GetString("MaskedTextProviderInvalidCharError"));
				}
				if (value != this.passwordChar)
				{
					this.passwordChar = value;
				}
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0001D97D File Offset: 0x0001C97D
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x0001D988 File Offset: 0x0001C988
		public char PromptChar
		{
			get
			{
				return this.promptChar;
			}
			set
			{
				if (value == this.passwordChar)
				{
					throw new InvalidOperationException(SR.GetString("MaskedTextProviderPasswordAndPromptCharError"));
				}
				if (!MaskedTextProvider.IsPrintableChar(value))
				{
					throw new ArgumentException(SR.GetString("MaskedTextProviderInvalidCharError"));
				}
				if (value != this.promptChar)
				{
					this.promptChar = value;
					for (int i = 0; i < this.testString.Length; i++)
					{
						MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
						if (this.IsEditPosition(i) && !charDescriptor.IsAssigned)
						{
							this.testString[i] = this.promptChar;
						}
					}
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0001DA1C File Offset: 0x0001CA1C
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x0001DA2E File Offset: 0x0001CA2E
		public bool ResetOnPrompt
		{
			get
			{
				return this.flagState[MaskedTextProvider.RESET_ON_PROMPT];
			}
			set
			{
				this.flagState[MaskedTextProvider.RESET_ON_PROMPT] = value;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0001DA41 File Offset: 0x0001CA41
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x0001DA53 File Offset: 0x0001CA53
		public bool ResetOnSpace
		{
			get
			{
				return this.flagState[MaskedTextProvider.SKIP_SPACE];
			}
			set
			{
				this.flagState[MaskedTextProvider.SKIP_SPACE] = value;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0001DA66 File Offset: 0x0001CA66
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0001DA78 File Offset: 0x0001CA78
		public bool SkipLiterals
		{
			get
			{
				return this.flagState[MaskedTextProvider.RESET_ON_LITERALS];
			}
			set
			{
				this.flagState[MaskedTextProvider.RESET_ON_LITERALS] = value;
			}
		}

		// Token: 0x170001E6 RID: 486
		public char this[int index]
		{
			get
			{
				if (index < 0 || index >= this.testString.Length)
				{
					throw new IndexOutOfRangeException(index.ToString(CultureInfo.CurrentCulture));
				}
				return this.testString[index];
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001DAC0 File Offset: 0x0001CAC0
		public bool Add(char input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Add(input, out num, out maskedTextResultHint);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001DAD8 File Offset: 0x0001CAD8
		public bool Add(char input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			int lastAssignedPosition = this.LastAssignedPosition;
			if (lastAssignedPosition == this.testString.Length - 1)
			{
				testPosition = this.testString.Length;
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				return false;
			}
			testPosition = lastAssignedPosition + 1;
			testPosition = this.FindEditPositionFrom(testPosition, true);
			if (testPosition == -1)
			{
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = this.testString.Length;
				return false;
			}
			return this.TestSetChar(input, testPosition, out resultHint);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001DB48 File Offset: 0x0001CB48
		public bool Add(string input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Add(input, out num, out maskedTextResultHint);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001DB60 File Offset: 0x0001CB60
		public bool Add(string input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			testPosition = this.LastAssignedPosition + 1;
			if (input.Length == 0)
			{
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			return this.TestSetString(input, testPosition, out testPosition, out resultHint);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001DB94 File Offset: 0x0001CB94
		public void Clear()
		{
			MaskedTextResultHint maskedTextResultHint;
			this.Clear(out maskedTextResultHint);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001DBAC File Offset: 0x0001CBAC
		public void Clear(out MaskedTextResultHint resultHint)
		{
			if (this.assignedCharCount == 0)
			{
				resultHint = MaskedTextResultHint.NoEffect;
				return;
			}
			resultHint = MaskedTextResultHint.Success;
			for (int i = 0; i < this.testString.Length; i++)
			{
				this.ResetChar(i);
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001DBE8 File Offset: 0x0001CBE8
		public int FindAssignedEditPositionFrom(int position, bool direction)
		{
			if (this.assignedCharCount == 0)
			{
				return -1;
			}
			int startPosition;
			int endPosition;
			if (direction)
			{
				startPosition = position;
				endPosition = this.testString.Length - 1;
			}
			else
			{
				startPosition = 0;
				endPosition = position;
			}
			return this.FindAssignedEditPositionInRange(startPosition, endPosition, direction);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001DC21 File Offset: 0x0001CC21
		public int FindAssignedEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			if (this.assignedCharCount == 0)
			{
				return -1;
			}
			return this.FindEditPositionInRange(startPosition, endPosition, direction, 2);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001DC38 File Offset: 0x0001CC38
		public int FindEditPositionFrom(int position, bool direction)
		{
			int startPosition;
			int endPosition;
			if (direction)
			{
				startPosition = position;
				endPosition = this.testString.Length - 1;
			}
			else
			{
				startPosition = 0;
				endPosition = position;
			}
			return this.FindEditPositionInRange(startPosition, endPosition, direction);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001DC68 File Offset: 0x0001CC68
		public int FindEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			MaskedTextProvider.CharType charTypeFlags = MaskedTextProvider.CharType.EditOptional | MaskedTextProvider.CharType.EditRequired;
			return this.FindPositionInRange(startPosition, endPosition, direction, charTypeFlags);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001DC84 File Offset: 0x0001CC84
		private int FindEditPositionInRange(int startPosition, int endPosition, bool direction, byte assignedStatus)
		{
			int num;
			for (;;)
			{
				num = this.FindEditPositionInRange(startPosition, endPosition, direction);
				if (num == -1)
				{
					return -1;
				}
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num];
				switch (assignedStatus)
				{
				case 1:
					if (!charDescriptor.IsAssigned)
					{
						return num;
					}
					goto IL_46;
				case 2:
					if (charDescriptor.IsAssigned)
					{
						return num;
					}
					goto IL_46;
				}
				break;
				IL_46:
				if (direction)
				{
					startPosition++;
				}
				else
				{
					endPosition--;
				}
				if (startPosition > endPosition)
				{
					return -1;
				}
			}
			return num;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001DCEC File Offset: 0x0001CCEC
		public int FindNonEditPositionFrom(int position, bool direction)
		{
			int startPosition;
			int endPosition;
			if (direction)
			{
				startPosition = position;
				endPosition = this.testString.Length - 1;
			}
			else
			{
				startPosition = 0;
				endPosition = position;
			}
			return this.FindNonEditPositionInRange(startPosition, endPosition, direction);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001DD1C File Offset: 0x0001CD1C
		public int FindNonEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			MaskedTextProvider.CharType charTypeFlags = MaskedTextProvider.CharType.Separator | MaskedTextProvider.CharType.Literal;
			return this.FindPositionInRange(startPosition, endPosition, direction, charTypeFlags);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001DD38 File Offset: 0x0001CD38
		private int FindPositionInRange(int startPosition, int endPosition, bool direction, MaskedTextProvider.CharType charTypeFlags)
		{
			if (startPosition < 0)
			{
				startPosition = 0;
			}
			if (endPosition >= this.testString.Length)
			{
				endPosition = this.testString.Length - 1;
			}
			if (startPosition > endPosition)
			{
				return -1;
			}
			while (startPosition <= endPosition)
			{
				int num;
				if (!direction)
				{
					endPosition = (num = endPosition) - 1;
				}
				else
				{
					startPosition = (num = startPosition) + 1;
				}
				int num2 = num;
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num2];
				if ((charDescriptor.CharType & charTypeFlags) == charDescriptor.CharType)
				{
					return num2;
				}
			}
			return -1;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001DDA8 File Offset: 0x0001CDA8
		public int FindUnassignedEditPositionFrom(int position, bool direction)
		{
			int startPosition;
			int endPosition;
			if (direction)
			{
				startPosition = position;
				endPosition = this.testString.Length - 1;
			}
			else
			{
				startPosition = 0;
				endPosition = position;
			}
			return this.FindEditPositionInRange(startPosition, endPosition, direction, 1);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001DDD8 File Offset: 0x0001CDD8
		public int FindUnassignedEditPositionInRange(int startPosition, int endPosition, bool direction)
		{
			for (;;)
			{
				int num = this.FindEditPositionInRange(startPosition, endPosition, direction, 0);
				if (num == -1)
				{
					break;
				}
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num];
				if (!charDescriptor.IsAssigned)
				{
					return num;
				}
				if (direction)
				{
					startPosition++;
				}
				else
				{
					endPosition--;
				}
			}
			return -1;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001DE1D File Offset: 0x0001CE1D
		public static bool GetOperationResultFromHint(MaskedTextResultHint hint)
		{
			return hint > MaskedTextResultHint.Unknown;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001DE23 File Offset: 0x0001CE23
		public bool InsertAt(char input, int position)
		{
			return position >= 0 && position < this.testString.Length && this.InsertAt(input.ToString(), position);
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001DE47 File Offset: 0x0001CE47
		public bool InsertAt(char input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			return this.InsertAt(input.ToString(), position, out testPosition, out resultHint);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001DE5C File Offset: 0x0001CE5C
		public bool InsertAt(string input, int position)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.InsertAt(input, position, out num, out maskedTextResultHint);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001DE75 File Offset: 0x0001CE75
		public bool InsertAt(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (position < 0 || position >= this.testString.Length)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			return this.InsertAtInt(input, position, out testPosition, out resultHint, false);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001DEB0 File Offset: 0x0001CEB0
		private bool InsertAtInt(string input, int position, out int testPosition, out MaskedTextResultHint resultHint, bool testOnly)
		{
			if (input.Length == 0)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			if (!this.TestString(input, position, out testPosition, out resultHint))
			{
				return false;
			}
			int i = this.FindEditPositionFrom(position, true);
			bool flag = this.FindAssignedEditPositionInRange(i, testPosition, true) != -1;
			int lastAssignedPosition = this.LastAssignedPosition;
			if (flag && testPosition == this.testString.Length - 1)
			{
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = this.testString.Length;
				return false;
			}
			int num = this.FindEditPositionFrom(testPosition + 1, true);
			if (flag)
			{
				MaskedTextResultHint maskedTextResultHint = MaskedTextResultHint.Unknown;
				while (num != -1)
				{
					MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
					if (charDescriptor.IsAssigned && !this.TestChar(this.testString[i], num, out maskedTextResultHint))
					{
						resultHint = maskedTextResultHint;
						testPosition = num;
						return false;
					}
					if (i != lastAssignedPosition)
					{
						i = this.FindEditPositionFrom(i + 1, true);
						num = this.FindEditPositionFrom(num + 1, true);
					}
					else
					{
						if (maskedTextResultHint > resultHint)
						{
							resultHint = maskedTextResultHint;
							goto IL_F3;
						}
						goto IL_F3;
					}
				}
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = this.testString.Length;
				return false;
			}
			IL_F3:
			if (testOnly)
			{
				return true;
			}
			if (flag)
			{
				while (i >= position)
				{
					MaskedTextProvider.CharDescriptor charDescriptor2 = this.stringDescriptor[i];
					if (charDescriptor2.IsAssigned)
					{
						this.SetChar(this.testString[i], num);
					}
					else
					{
						this.ResetChar(num);
					}
					num = this.FindEditPositionFrom(num - 1, false);
					i = this.FindEditPositionFrom(i - 1, false);
				}
			}
			this.SetString(input, position);
			return true;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001E011 File Offset: 0x0001D011
		private static bool IsAscii(char c)
		{
			return c >= '!' && c <= '~';
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001E022 File Offset: 0x0001D022
		private static bool IsAciiAlphanumeric(char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001E049 File Offset: 0x0001D049
		private static bool IsAlphanumeric(char c)
		{
			return char.IsLetter(c) || char.IsDigit(c);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001E05B File Offset: 0x0001D05B
		private static bool IsAsciiLetter(char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001E078 File Offset: 0x0001D078
		public bool IsAvailablePosition(int position)
		{
			if (position < 0 || position >= this.testString.Length)
			{
				return false;
			}
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			return MaskedTextProvider.IsEditPosition(charDescriptor) && !charDescriptor.IsAssigned;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001E0BC File Offset: 0x0001D0BC
		public bool IsEditPosition(int position)
		{
			if (position < 0 || position >= this.testString.Length)
			{
				return false;
			}
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			return MaskedTextProvider.IsEditPosition(charDescriptor);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001E0F0 File Offset: 0x0001D0F0
		private static bool IsEditPosition(MaskedTextProvider.CharDescriptor charDescriptor)
		{
			return charDescriptor.CharType == MaskedTextProvider.CharType.EditRequired || charDescriptor.CharType == MaskedTextProvider.CharType.EditOptional;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001E106 File Offset: 0x0001D106
		private static bool IsLiteralPosition(MaskedTextProvider.CharDescriptor charDescriptor)
		{
			return charDescriptor.CharType == MaskedTextProvider.CharType.Literal || charDescriptor.CharType == MaskedTextProvider.CharType.Separator;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001E11C File Offset: 0x0001D11C
		private static bool IsPrintableChar(char c)
		{
			return char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c) || c == ' ';
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001E13D File Offset: 0x0001D13D
		public static bool IsValidInputChar(char c)
		{
			return MaskedTextProvider.IsPrintableChar(c);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001E145 File Offset: 0x0001D145
		public static bool IsValidMaskChar(char c)
		{
			return MaskedTextProvider.IsPrintableChar(c);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001E14D File Offset: 0x0001D14D
		public static bool IsValidPasswordChar(char c)
		{
			return MaskedTextProvider.IsPrintableChar(c) || c == '\0';
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001E160 File Offset: 0x0001D160
		public bool Remove()
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Remove(out num, out maskedTextResultHint);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001E178 File Offset: 0x0001D178
		public bool Remove(out int testPosition, out MaskedTextResultHint resultHint)
		{
			int lastAssignedPosition = this.LastAssignedPosition;
			if (lastAssignedPosition == -1)
			{
				testPosition = 0;
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			this.ResetChar(lastAssignedPosition);
			testPosition = lastAssignedPosition;
			resultHint = MaskedTextResultHint.Success;
			return true;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001E1A6 File Offset: 0x0001D1A6
		public bool RemoveAt(int position)
		{
			return this.RemoveAt(position, position);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001E1B0 File Offset: 0x0001D1B0
		public bool RemoveAt(int startPosition, int endPosition)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.RemoveAt(startPosition, endPosition, out num, out maskedTextResultHint);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001E1C9 File Offset: 0x0001D1C9
		public bool RemoveAt(int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (endPosition >= this.testString.Length)
			{
				testPosition = endPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition < 0 || startPosition > endPosition)
			{
				testPosition = startPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			return this.RemoveAtInt(startPosition, endPosition, out testPosition, out resultHint, false);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001E204 File Offset: 0x0001D204
		private bool RemoveAtInt(int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint, bool testOnly)
		{
			int lastAssignedPosition = this.LastAssignedPosition;
			int num = this.FindEditPositionInRange(startPosition, endPosition, true);
			resultHint = MaskedTextResultHint.NoEffect;
			if (num == -1 || num > lastAssignedPosition)
			{
				testPosition = startPosition;
				return true;
			}
			testPosition = startPosition;
			bool flag = endPosition < lastAssignedPosition;
			if (this.FindAssignedEditPositionInRange(startPosition, endPosition, true) != -1)
			{
				resultHint = MaskedTextResultHint.Success;
			}
			if (flag)
			{
				int num2 = this.FindEditPositionFrom(endPosition + 1, true);
				int num3 = num2;
				startPosition = num;
				MaskedTextResultHint maskedTextResultHint;
				for (;;)
				{
					char c = this.testString[num2];
					MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[num2];
					if ((c != this.PromptChar || charDescriptor.IsAssigned) && !this.TestChar(c, num, out maskedTextResultHint))
					{
						break;
					}
					if (num2 == lastAssignedPosition)
					{
						goto IL_B3;
					}
					num2 = this.FindEditPositionFrom(num2 + 1, true);
					num = this.FindEditPositionFrom(num + 1, true);
				}
				resultHint = maskedTextResultHint;
				testPosition = num;
				return false;
				IL_B3:
				if (MaskedTextResultHint.SideEffect > resultHint)
				{
					resultHint = MaskedTextResultHint.SideEffect;
				}
				if (testOnly)
				{
					return true;
				}
				num2 = num3;
				num = startPosition;
				for (;;)
				{
					char c2 = this.testString[num2];
					MaskedTextProvider.CharDescriptor charDescriptor2 = this.stringDescriptor[num2];
					if (c2 == this.PromptChar && !charDescriptor2.IsAssigned)
					{
						this.ResetChar(num);
					}
					else
					{
						this.SetChar(c2, num);
						this.ResetChar(num2);
					}
					if (num2 == lastAssignedPosition)
					{
						break;
					}
					num2 = this.FindEditPositionFrom(num2 + 1, true);
					num = this.FindEditPositionFrom(num + 1, true);
				}
				startPosition = num + 1;
			}
			if (startPosition <= endPosition)
			{
				this.ResetString(startPosition, endPosition);
			}
			return true;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001E350 File Offset: 0x0001D350
		public bool Replace(char input, int position)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Replace(input, position, out num, out maskedTextResultHint);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001E36C File Offset: 0x0001D36C
		public bool Replace(char input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (position < 0 || position >= this.testString.Length)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			testPosition = position;
			if (!this.TestEscapeChar(input, testPosition))
			{
				testPosition = this.FindEditPositionFrom(testPosition, true);
			}
			if (testPosition == -1)
			{
				resultHint = MaskedTextResultHint.UnavailableEditPosition;
				testPosition = position;
				return false;
			}
			return this.TestSetChar(input, testPosition, out resultHint);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001E3D0 File Offset: 0x0001D3D0
		public bool Replace(char input, int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (endPosition >= this.testString.Length)
			{
				testPosition = endPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition < 0 || startPosition > endPosition)
			{
				testPosition = startPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition == endPosition)
			{
				testPosition = startPosition;
				return this.TestSetChar(input, startPosition, out resultHint);
			}
			return this.Replace(input.ToString(), startPosition, endPosition, out testPosition, out resultHint);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001E430 File Offset: 0x0001D430
		public bool Replace(string input, int position)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Replace(input, position, out num, out maskedTextResultHint);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001E44C File Offset: 0x0001D44C
		public bool Replace(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (position < 0 || position >= this.testString.Length)
			{
				testPosition = position;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (input.Length == 0)
			{
				return this.RemoveAt(position, position, out testPosition, out resultHint);
			}
			return this.TestSetString(input, position, out testPosition, out resultHint);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001E4A8 File Offset: 0x0001D4A8
		public bool Replace(string input, int startPosition, int endPosition, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (endPosition >= this.testString.Length)
			{
				testPosition = endPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (startPosition < 0 || startPosition > endPosition)
			{
				testPosition = startPosition;
				resultHint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			if (input.Length == 0)
			{
				return this.RemoveAt(startPosition, endPosition, out testPosition, out resultHint);
			}
			if (!this.TestString(input, startPosition, out testPosition, out resultHint))
			{
				return false;
			}
			if (this.assignedCharCount > 0)
			{
				if (testPosition < endPosition)
				{
					int num;
					MaskedTextResultHint maskedTextResultHint;
					if (!this.RemoveAtInt(testPosition + 1, endPosition, out num, out maskedTextResultHint, false))
					{
						testPosition = num;
						resultHint = maskedTextResultHint;
						return false;
					}
					if (maskedTextResultHint == MaskedTextResultHint.Success && resultHint != maskedTextResultHint)
					{
						resultHint = MaskedTextResultHint.SideEffect;
					}
				}
				else if (testPosition > endPosition)
				{
					int lastAssignedPosition = this.LastAssignedPosition;
					int i = testPosition + 1;
					int num2 = endPosition + 1;
					MaskedTextResultHint maskedTextResultHint;
					for (;;)
					{
						num2 = this.FindEditPositionFrom(num2, true);
						i = this.FindEditPositionFrom(i, true);
						if (i == -1)
						{
							goto Block_12;
						}
						if (!this.TestChar(this.testString[num2], i, out maskedTextResultHint))
						{
							goto Block_13;
						}
						if (maskedTextResultHint == MaskedTextResultHint.Success && resultHint != maskedTextResultHint)
						{
							resultHint = MaskedTextResultHint.Success;
						}
						if (num2 == lastAssignedPosition)
						{
							break;
						}
						num2++;
						i++;
					}
					while (i > testPosition)
					{
						this.SetChar(this.testString[num2], i);
						num2 = this.FindEditPositionFrom(num2 - 1, false);
						i = this.FindEditPositionFrom(i - 1, false);
					}
					goto IL_162;
					Block_12:
					testPosition = this.testString.Length;
					resultHint = MaskedTextResultHint.UnavailableEditPosition;
					return false;
					Block_13:
					testPosition = i;
					resultHint = maskedTextResultHint;
					return false;
				}
			}
			IL_162:
			this.SetString(input, startPosition);
			return true;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001E620 File Offset: 0x0001D620
		private void ResetChar(int testPosition)
		{
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[testPosition];
			if (this.IsEditPosition(testPosition) && charDescriptor.IsAssigned)
			{
				charDescriptor.IsAssigned = false;
				this.testString[testPosition] = this.promptChar;
				this.assignedCharCount--;
				if (charDescriptor.CharType == MaskedTextProvider.CharType.EditRequired)
				{
					this.requiredCharCount--;
				}
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001E689 File Offset: 0x0001D689
		private void ResetString(int startPosition, int endPosition)
		{
			startPosition = this.FindAssignedEditPositionFrom(startPosition, true);
			if (startPosition != -1)
			{
				endPosition = this.FindAssignedEditPositionFrom(endPosition, false);
				while (startPosition <= endPosition)
				{
					startPosition = this.FindAssignedEditPositionFrom(startPosition, true);
					this.ResetChar(startPosition);
					startPosition++;
				}
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001E6C0 File Offset: 0x0001D6C0
		public bool Set(string input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.Set(input, out num, out maskedTextResultHint);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001E6D8 File Offset: 0x0001D6D8
		public bool Set(string input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			resultHint = MaskedTextResultHint.Unknown;
			testPosition = 0;
			if (input.Length == 0)
			{
				this.Clear(out resultHint);
				return true;
			}
			if (!this.TestSetString(input, testPosition, out testPosition, out resultHint))
			{
				return false;
			}
			int num = this.FindAssignedEditPositionFrom(testPosition + 1, true);
			if (num != -1)
			{
				this.ResetString(num, this.testString.Length - 1);
			}
			return true;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001E740 File Offset: 0x0001D740
		private void SetChar(char input, int position)
		{
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			this.SetChar(input, position, charDescriptor);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001E764 File Offset: 0x0001D764
		private void SetChar(char input, int position, MaskedTextProvider.CharDescriptor charDescriptor)
		{
			MaskedTextProvider.CharDescriptor charDescriptor2 = this.stringDescriptor[position];
			if (this.TestEscapeChar(input, position, charDescriptor))
			{
				this.ResetChar(position);
				return;
			}
			if (char.IsLetter(input))
			{
				if (char.IsUpper(input))
				{
					if (charDescriptor.CaseConversion == MaskedTextProvider.CaseConversion.ToLower)
					{
						input = this.culture.TextInfo.ToLower(input);
					}
				}
				else if (charDescriptor.CaseConversion == MaskedTextProvider.CaseConversion.ToUpper)
				{
					input = this.culture.TextInfo.ToUpper(input);
				}
			}
			this.testString[position] = input;
			if (!charDescriptor.IsAssigned)
			{
				charDescriptor.IsAssigned = true;
				this.assignedCharCount++;
				if (charDescriptor.CharType == MaskedTextProvider.CharType.EditRequired)
				{
					this.requiredCharCount++;
				}
			}
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001E81C File Offset: 0x0001D81C
		private void SetString(string input, int testPosition)
		{
			foreach (char input2 in input)
			{
				if (!this.TestEscapeChar(input2, testPosition))
				{
					testPosition = this.FindEditPositionFrom(testPosition, true);
				}
				this.SetChar(input2, testPosition);
				testPosition++;
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001E868 File Offset: 0x0001D868
		private bool TestChar(char input, int position, out MaskedTextResultHint resultHint)
		{
			if (!MaskedTextProvider.IsPrintableChar(input))
			{
				resultHint = MaskedTextResultHint.InvalidInput;
				return false;
			}
			MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[position];
			if (MaskedTextProvider.IsLiteralPosition(charDescriptor))
			{
				if (this.SkipLiterals && input == this.testString[position])
				{
					resultHint = MaskedTextResultHint.CharacterEscaped;
					return true;
				}
				resultHint = MaskedTextResultHint.NonEditPosition;
				return false;
			}
			else
			{
				if (input == this.promptChar)
				{
					if (this.ResetOnPrompt)
					{
						if (MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned)
						{
							resultHint = MaskedTextResultHint.SideEffect;
						}
						else
						{
							resultHint = MaskedTextResultHint.CharacterEscaped;
						}
						return true;
					}
					if (!this.AllowPromptAsInput)
					{
						resultHint = MaskedTextResultHint.PromptCharNotAllowed;
						return false;
					}
				}
				if (input == ' ' && this.ResetOnSpace)
				{
					if (MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned)
					{
						resultHint = MaskedTextResultHint.SideEffect;
					}
					else
					{
						resultHint = MaskedTextResultHint.CharacterEscaped;
					}
					return true;
				}
				char c = this.mask[charDescriptor.MaskPosition];
				if (c <= '0')
				{
					if (c != '#')
					{
						if (c != '&')
						{
							if (c == '0')
							{
								if (!char.IsDigit(input))
								{
									resultHint = MaskedTextResultHint.DigitExpected;
									return false;
								}
							}
						}
						else if (!MaskedTextProvider.IsAscii(input) && this.AsciiOnly)
						{
							resultHint = MaskedTextResultHint.AsciiCharacterExpected;
							return false;
						}
					}
					else if (!char.IsDigit(input) && input != '-' && input != '+' && input != ' ')
					{
						resultHint = MaskedTextResultHint.DigitExpected;
						return false;
					}
				}
				else if (c <= 'C')
				{
					if (c != '9')
					{
						switch (c)
						{
						case '?':
							if (!char.IsLetter(input) && input != ' ')
							{
								resultHint = MaskedTextResultHint.LetterExpected;
								return false;
							}
							if (!MaskedTextProvider.IsAsciiLetter(input) && this.AsciiOnly)
							{
								resultHint = MaskedTextResultHint.AsciiCharacterExpected;
								return false;
							}
							break;
						case 'A':
							if (!MaskedTextProvider.IsAlphanumeric(input))
							{
								resultHint = MaskedTextResultHint.AlphanumericCharacterExpected;
								return false;
							}
							if (!MaskedTextProvider.IsAciiAlphanumeric(input) && this.AsciiOnly)
							{
								resultHint = MaskedTextResultHint.AsciiCharacterExpected;
								return false;
							}
							break;
						case 'C':
							if (!MaskedTextProvider.IsAscii(input) && this.AsciiOnly && input != ' ')
							{
								resultHint = MaskedTextResultHint.AsciiCharacterExpected;
								return false;
							}
							break;
						}
					}
					else if (!char.IsDigit(input) && input != ' ')
					{
						resultHint = MaskedTextResultHint.DigitExpected;
						return false;
					}
				}
				else if (c != 'L')
				{
					if (c == 'a')
					{
						if (!MaskedTextProvider.IsAlphanumeric(input) && input != ' ')
						{
							resultHint = MaskedTextResultHint.AlphanumericCharacterExpected;
							return false;
						}
						if (!MaskedTextProvider.IsAciiAlphanumeric(input) && this.AsciiOnly)
						{
							resultHint = MaskedTextResultHint.AsciiCharacterExpected;
							return false;
						}
					}
				}
				else
				{
					if (!char.IsLetter(input))
					{
						resultHint = MaskedTextResultHint.LetterExpected;
						return false;
					}
					if (!MaskedTextProvider.IsAsciiLetter(input) && this.AsciiOnly)
					{
						resultHint = MaskedTextResultHint.AsciiCharacterExpected;
						return false;
					}
				}
				if (input == this.testString[position] && charDescriptor.IsAssigned)
				{
					resultHint = MaskedTextResultHint.NoEffect;
				}
				else
				{
					resultHint = MaskedTextResultHint.Success;
				}
				return true;
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001EAC8 File Offset: 0x0001DAC8
		private bool TestEscapeChar(char input, int position)
		{
			MaskedTextProvider.CharDescriptor charDex = this.stringDescriptor[position];
			return this.TestEscapeChar(input, position, charDex);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001EAEC File Offset: 0x0001DAEC
		private bool TestEscapeChar(char input, int position, MaskedTextProvider.CharDescriptor charDex)
		{
			if (MaskedTextProvider.IsLiteralPosition(charDex))
			{
				return this.SkipLiterals && input == this.testString[position];
			}
			return (this.ResetOnPrompt && input == this.promptChar) || (this.ResetOnSpace && input == ' ');
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001EB3C File Offset: 0x0001DB3C
		private bool TestSetChar(char input, int position, out MaskedTextResultHint resultHint)
		{
			if (this.TestChar(input, position, out resultHint))
			{
				if (resultHint == MaskedTextResultHint.Success || resultHint == MaskedTextResultHint.SideEffect)
				{
					this.SetChar(input, position);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001EB5E File Offset: 0x0001DB5E
		private bool TestSetString(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			if (this.TestString(input, position, out testPosition, out resultHint))
			{
				this.SetString(input, position);
				return true;
			}
			return false;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001EB78 File Offset: 0x0001DB78
		private bool TestString(string input, int position, out int testPosition, out MaskedTextResultHint resultHint)
		{
			resultHint = MaskedTextResultHint.Unknown;
			testPosition = position;
			if (input.Length == 0)
			{
				return true;
			}
			MaskedTextResultHint maskedTextResultHint = resultHint;
			int i = 0;
			while (i < input.Length)
			{
				char input2 = input[i];
				bool result;
				if (testPosition >= this.testString.Length)
				{
					resultHint = MaskedTextResultHint.UnavailableEditPosition;
					result = false;
				}
				else
				{
					if (!this.TestEscapeChar(input2, testPosition))
					{
						testPosition = this.FindEditPositionFrom(testPosition, true);
						if (testPosition == -1)
						{
							testPosition = this.testString.Length;
							resultHint = MaskedTextResultHint.UnavailableEditPosition;
							return false;
						}
					}
					if (this.TestChar(input2, testPosition, out maskedTextResultHint))
					{
						if (maskedTextResultHint > resultHint)
						{
							resultHint = maskedTextResultHint;
						}
						testPosition++;
						i++;
						continue;
					}
					resultHint = maskedTextResultHint;
					result = false;
				}
				return result;
			}
			testPosition--;
			return true;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001EC34 File Offset: 0x0001DC34
		public string ToDisplayString()
		{
			if (!this.IsPassword || this.assignedCharCount == 0)
			{
				return this.testString.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder(this.testString.Length);
			for (int i = 0; i < this.testString.Length; i++)
			{
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
				stringBuilder.Append((MaskedTextProvider.IsEditPosition(charDescriptor) && charDescriptor.IsAssigned) ? this.passwordChar : this.testString[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001ECC2 File Offset: 0x0001DCC2
		public override string ToString()
		{
			return this.ToString(true, this.IncludePrompt, this.IncludeLiterals, 0, this.testString.Length);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001ECE3 File Offset: 0x0001DCE3
		public string ToString(bool ignorePasswordChar)
		{
			return this.ToString(ignorePasswordChar, this.IncludePrompt, this.IncludeLiterals, 0, this.testString.Length);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001ED04 File Offset: 0x0001DD04
		public string ToString(int startPosition, int length)
		{
			return this.ToString(true, this.IncludePrompt, this.IncludeLiterals, startPosition, length);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001ED1B File Offset: 0x0001DD1B
		public string ToString(bool ignorePasswordChar, int startPosition, int length)
		{
			return this.ToString(ignorePasswordChar, this.IncludePrompt, this.IncludeLiterals, startPosition, length);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001ED32 File Offset: 0x0001DD32
		public string ToString(bool includePrompt, bool includeLiterals)
		{
			return this.ToString(true, includePrompt, includeLiterals, 0, this.testString.Length);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001ED49 File Offset: 0x0001DD49
		public string ToString(bool includePrompt, bool includeLiterals, int startPosition, int length)
		{
			return this.ToString(true, includePrompt, includeLiterals, startPosition, length);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001ED58 File Offset: 0x0001DD58
		public string ToString(bool ignorePasswordChar, bool includePrompt, bool includeLiterals, int startPosition, int length)
		{
			if (length <= 0)
			{
				return string.Empty;
			}
			if (startPosition < 0)
			{
				startPosition = 0;
			}
			if (startPosition >= this.testString.Length)
			{
				return string.Empty;
			}
			int num = this.testString.Length - startPosition;
			if (length > num)
			{
				length = num;
			}
			if ((!this.IsPassword || ignorePasswordChar) && includePrompt && includeLiterals)
			{
				return this.testString.ToString(startPosition, length);
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num2 = startPosition + length - 1;
			if (!includePrompt)
			{
				int num3 = includeLiterals ? this.FindNonEditPositionInRange(startPosition, num2, false) : MaskedTextProvider.InvalidIndex;
				int num4 = this.FindAssignedEditPositionInRange((num3 == MaskedTextProvider.InvalidIndex) ? startPosition : num3, num2, false);
				num2 = ((num4 != MaskedTextProvider.InvalidIndex) ? num4 : num3);
				if (num2 == MaskedTextProvider.InvalidIndex)
				{
					return string.Empty;
				}
			}
			int i = startPosition;
			while (i <= num2)
			{
				char value = this.testString[i];
				MaskedTextProvider.CharDescriptor charDescriptor = this.stringDescriptor[i];
				MaskedTextProvider.CharType charType = charDescriptor.CharType;
				switch (charType)
				{
				case MaskedTextProvider.CharType.EditOptional:
				case MaskedTextProvider.CharType.EditRequired:
					if (charDescriptor.IsAssigned)
					{
						if (!this.IsPassword || ignorePasswordChar)
						{
							goto IL_13E;
						}
						stringBuilder.Append(this.passwordChar);
					}
					else
					{
						if (includePrompt)
						{
							goto IL_13E;
						}
						stringBuilder.Append(' ');
					}
					break;
				case MaskedTextProvider.CharType.EditOptional | MaskedTextProvider.CharType.EditRequired:
					goto IL_13E;
				case MaskedTextProvider.CharType.Separator:
					goto IL_13B;
				default:
					if (charType != MaskedTextProvider.CharType.Literal)
					{
						goto IL_13E;
					}
					goto IL_13B;
				}
				IL_147:
				i++;
				continue;
				IL_13B:
				if (!includeLiterals)
				{
					goto IL_147;
				}
				IL_13E:
				stringBuilder.Append(value);
				goto IL_147;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001EEC0 File Offset: 0x0001DEC0
		public bool VerifyChar(char input, int position, out MaskedTextResultHint hint)
		{
			hint = MaskedTextResultHint.NoEffect;
			if (position < 0 || position >= this.testString.Length)
			{
				hint = MaskedTextResultHint.PositionOutOfRange;
				return false;
			}
			return this.TestChar(input, position, out hint);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001EEE6 File Offset: 0x0001DEE6
		public bool VerifyEscapeChar(char input, int position)
		{
			return position >= 0 && position < this.testString.Length && this.TestEscapeChar(input, position);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001EF04 File Offset: 0x0001DF04
		public bool VerifyString(string input)
		{
			int num;
			MaskedTextResultHint maskedTextResultHint;
			return this.VerifyString(input, out num, out maskedTextResultHint);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001EF1C File Offset: 0x0001DF1C
		public bool VerifyString(string input, out int testPosition, out MaskedTextResultHint resultHint)
		{
			testPosition = 0;
			if (input == null || input.Length == 0)
			{
				resultHint = MaskedTextResultHint.NoEffect;
				return true;
			}
			return this.TestString(input, 0, out testPosition, out resultHint);
		}

		// Token: 0x040009C2 RID: 2498
		private const char spaceChar = ' ';

		// Token: 0x040009C3 RID: 2499
		private const char defaultPromptChar = '_';

		// Token: 0x040009C4 RID: 2500
		private const char nullPasswordChar = '\0';

		// Token: 0x040009C5 RID: 2501
		private const bool defaultAllowPrompt = true;

		// Token: 0x040009C6 RID: 2502
		private const int invalidIndex = -1;

		// Token: 0x040009C7 RID: 2503
		private const byte editAny = 0;

		// Token: 0x040009C8 RID: 2504
		private const byte editUnassigned = 1;

		// Token: 0x040009C9 RID: 2505
		private const byte editAssigned = 2;

		// Token: 0x040009CA RID: 2506
		private const bool forward = true;

		// Token: 0x040009CB RID: 2507
		private const bool backward = false;

		// Token: 0x040009CC RID: 2508
		private static int ASCII_ONLY = BitVector32.CreateMask();

		// Token: 0x040009CD RID: 2509
		private static int ALLOW_PROMPT_AS_INPUT = BitVector32.CreateMask(MaskedTextProvider.ASCII_ONLY);

		// Token: 0x040009CE RID: 2510
		private static int INCLUDE_PROMPT = BitVector32.CreateMask(MaskedTextProvider.ALLOW_PROMPT_AS_INPUT);

		// Token: 0x040009CF RID: 2511
		private static int INCLUDE_LITERALS = BitVector32.CreateMask(MaskedTextProvider.INCLUDE_PROMPT);

		// Token: 0x040009D0 RID: 2512
		private static int RESET_ON_PROMPT = BitVector32.CreateMask(MaskedTextProvider.INCLUDE_LITERALS);

		// Token: 0x040009D1 RID: 2513
		private static int RESET_ON_LITERALS = BitVector32.CreateMask(MaskedTextProvider.RESET_ON_PROMPT);

		// Token: 0x040009D2 RID: 2514
		private static int SKIP_SPACE = BitVector32.CreateMask(MaskedTextProvider.RESET_ON_LITERALS);

		// Token: 0x040009D3 RID: 2515
		private static Type maskTextProviderType = typeof(MaskedTextProvider);

		// Token: 0x040009D4 RID: 2516
		private BitVector32 flagState;

		// Token: 0x040009D5 RID: 2517
		private CultureInfo culture;

		// Token: 0x040009D6 RID: 2518
		private StringBuilder testString;

		// Token: 0x040009D7 RID: 2519
		private int assignedCharCount;

		// Token: 0x040009D8 RID: 2520
		private int requiredCharCount;

		// Token: 0x040009D9 RID: 2521
		private int requiredEditChars;

		// Token: 0x040009DA RID: 2522
		private int optionalEditChars;

		// Token: 0x040009DB RID: 2523
		private string mask;

		// Token: 0x040009DC RID: 2524
		private char passwordChar;

		// Token: 0x040009DD RID: 2525
		private char promptChar;

		// Token: 0x040009DE RID: 2526
		private List<MaskedTextProvider.CharDescriptor> stringDescriptor;

		// Token: 0x0200011C RID: 284
		private enum CaseConversion
		{
			// Token: 0x040009E0 RID: 2528
			None,
			// Token: 0x040009E1 RID: 2529
			ToLower,
			// Token: 0x040009E2 RID: 2530
			ToUpper
		}

		// Token: 0x0200011D RID: 285
		[Flags]
		private enum CharType
		{
			// Token: 0x040009E4 RID: 2532
			EditOptional = 1,
			// Token: 0x040009E5 RID: 2533
			EditRequired = 2,
			// Token: 0x040009E6 RID: 2534
			Separator = 4,
			// Token: 0x040009E7 RID: 2535
			Literal = 8,
			// Token: 0x040009E8 RID: 2536
			Modifier = 16
		}

		// Token: 0x0200011E RID: 286
		private class CharDescriptor
		{
			// Token: 0x06000931 RID: 2353 RVA: 0x0001EFBC File Offset: 0x0001DFBC
			public CharDescriptor(int maskPos, MaskedTextProvider.CharType charType)
			{
				this.MaskPosition = maskPos;
				this.CharType = charType;
			}

			// Token: 0x06000932 RID: 2354 RVA: 0x0001EFD4 File Offset: 0x0001DFD4
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "MaskPosition[{0}] <CaseConversion.{1}><CharType.{2}><IsAssigned: {3}", new object[]
				{
					this.MaskPosition,
					this.CaseConversion,
					this.CharType,
					this.IsAssigned
				});
			}

			// Token: 0x040009E9 RID: 2537
			public int MaskPosition;

			// Token: 0x040009EA RID: 2538
			public MaskedTextProvider.CaseConversion CaseConversion;

			// Token: 0x040009EB RID: 2539
			public MaskedTextProvider.CharType CharType;

			// Token: 0x040009EC RID: 2540
			public bool IsAssigned;
		}
	}
}
