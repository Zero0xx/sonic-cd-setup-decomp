using System;
using System.Globalization;
using System.Threading;

namespace System
{
	// Token: 0x0200039C RID: 924
	internal struct __DTString
	{
		// Token: 0x060024FB RID: 9467 RVA: 0x00064A55 File Offset: 0x00063A55
		internal __DTString(string str, DateTimeFormatInfo dtfi, bool checkDigitToken)
		{
			this = new __DTString(str, dtfi);
			this.m_checkDigitToken = checkDigitToken;
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x00064A68 File Offset: 0x00063A68
		internal __DTString(string str, DateTimeFormatInfo dtfi)
		{
			this.Index = -1;
			this.Value = str;
			this.len = this.Value.Length;
			this.m_current = '\0';
			if (dtfi != null)
			{
				this.m_info = dtfi.CompareInfo;
				this.m_checkDigitToken = ((dtfi.FormatFlags & DateTimeFormatFlags.UseDigitPrefixInTokens) != DateTimeFormatFlags.None);
				return;
			}
			this.m_info = Thread.CurrentThread.CurrentCulture.CompareInfo;
			this.m_checkDigitToken = false;
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x00064ADC File Offset: 0x00063ADC
		internal CompareInfo CompareInfo
		{
			get
			{
				return this.m_info;
			}
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00064AE4 File Offset: 0x00063AE4
		internal bool GetNext()
		{
			this.Index++;
			if (this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				return true;
			}
			return false;
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x00064B1C File Offset: 0x00063B1C
		internal bool Advance(int count)
		{
			this.Index += count;
			if (this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				return true;
			}
			return false;
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x00064B54 File Offset: 0x00063B54
		internal void GetRegularToken(out TokenType tokenType, out int tokenValue, DateTimeFormatInfo dtfi)
		{
			tokenValue = 0;
			if (this.Index >= this.len)
			{
				tokenType = TokenType.EndOfString;
				return;
			}
			tokenType = TokenType.UnknownToken;
			IL_19:
			while (!DateTimeParse.IsDigit(this.m_current))
			{
				if (char.IsWhiteSpace(this.m_current))
				{
					while (++this.Index < this.len)
					{
						this.m_current = this.Value[this.Index];
						if (!char.IsWhiteSpace(this.m_current))
						{
							goto IL_19;
						}
					}
					tokenType = TokenType.EndOfString;
					return;
				}
				dtfi.Tokenize(TokenType.RegularTokenMask, out tokenType, out tokenValue, ref this);
				return;
			}
			tokenValue = (int)(this.m_current - '0');
			int index = this.Index;
			while (++this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				int num = (int)(this.m_current - '0');
				if (num < 0 || num > 9)
				{
					break;
				}
				tokenValue = tokenValue * 10 + num;
			}
			if (this.Index - index > 8)
			{
				tokenType = TokenType.NumberToken;
				tokenValue = -1;
			}
			else if (this.Index - index < 3)
			{
				tokenType = TokenType.NumberToken;
			}
			else
			{
				tokenType = TokenType.YearNumberToken;
			}
			if (!this.m_checkDigitToken)
			{
				return;
			}
			int index2 = this.Index;
			char current = this.m_current;
			this.Index = index;
			this.m_current = this.Value[this.Index];
			TokenType tokenType2;
			int num2;
			if (dtfi.Tokenize(TokenType.RegularTokenMask, out tokenType2, out num2, ref this))
			{
				tokenType = tokenType2;
				tokenValue = num2;
				return;
			}
			this.Index = index2;
			this.m_current = current;
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x00064CD8 File Offset: 0x00063CD8
		internal TokenType GetSeparatorToken(DateTimeFormatInfo dtfi, out int indexBeforeSeparator, out char charBeforeSeparator)
		{
			indexBeforeSeparator = this.Index;
			charBeforeSeparator = this.m_current;
			if (!this.SkipWhiteSpaceCurrent())
			{
				return TokenType.SEP_End;
			}
			TokenType result;
			if (!DateTimeParse.IsDigit(this.m_current))
			{
				int num;
				if (!dtfi.Tokenize(TokenType.SeparatorTokenMask, out result, out num, ref this))
				{
					result = TokenType.SEP_Space;
				}
			}
			else
			{
				result = TokenType.SEP_Space;
			}
			return result;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x00064D33 File Offset: 0x00063D33
		internal bool MatchSpecifiedWord(string target)
		{
			return this.MatchSpecifiedWord(target, target.Length + this.Index);
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00064D4C File Offset: 0x00063D4C
		internal bool MatchSpecifiedWord(string target, int endIndex)
		{
			int num = endIndex - this.Index;
			return num == target.Length && this.Index + num <= this.len && this.m_info.Compare(this.Value, this.Index, num, target, 0, num, CompareOptions.IgnoreCase) == 0;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x00064DA0 File Offset: 0x00063DA0
		internal bool MatchSpecifiedWords(string target, bool checkWordBoundary, ref int matchLength)
		{
			int num = this.Value.Length - this.Index;
			matchLength = target.Length;
			if (matchLength > num || this.m_info.Compare(this.Value, this.Index, matchLength, target, 0, matchLength, CompareOptions.IgnoreCase) != 0)
			{
				int num2 = 0;
				int num3 = this.Index;
				int num4 = target.IndexOfAny(__DTString.WhiteSpaceChecks, num2);
				if (num4 == -1)
				{
					return false;
				}
				for (;;)
				{
					int num5 = num4 - num2;
					if (num3 >= this.Value.Length - num5)
					{
						break;
					}
					if (num5 == 0)
					{
						matchLength--;
					}
					else
					{
						if (!char.IsWhiteSpace(this.Value[num3 + num5]))
						{
							return false;
						}
						if (this.m_info.Compare(this.Value, num3, num5, target, num2, num5, CompareOptions.IgnoreCase) != 0)
						{
							return false;
						}
						num3 = num3 + num5 + 1;
					}
					num2 = num4 + 1;
					while (num3 < this.Value.Length && char.IsWhiteSpace(this.Value[num3]))
					{
						num3++;
						matchLength++;
					}
					if ((num4 = target.IndexOfAny(__DTString.WhiteSpaceChecks, num2)) < 0)
					{
						goto Block_8;
					}
				}
				return false;
				Block_8:
				if (num2 < target.Length)
				{
					int num6 = target.Length - num2;
					if (num3 > this.Value.Length - num6)
					{
						return false;
					}
					if (this.m_info.Compare(this.Value, num3, num6, target, num2, num6, CompareOptions.IgnoreCase) != 0)
					{
						return false;
					}
				}
			}
			if (checkWordBoundary)
			{
				int num7 = this.Index + matchLength;
				if (num7 < this.Value.Length && char.IsLetter(this.Value[num7]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x00064F28 File Offset: 0x00063F28
		internal bool Match(string str)
		{
			if (++this.Index >= this.len)
			{
				return false;
			}
			if (str.Length > this.Value.Length - this.Index)
			{
				return false;
			}
			if (this.m_info.Compare(this.Value, this.Index, str.Length, str, 0, str.Length, CompareOptions.Ordinal) == 0)
			{
				this.Index += str.Length - 1;
				return true;
			}
			return false;
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x00064FB0 File Offset: 0x00063FB0
		internal bool Match(char ch)
		{
			if (++this.Index >= this.len)
			{
				return false;
			}
			if (this.Value[this.Index] == ch)
			{
				this.m_current = ch;
				return true;
			}
			this.Index--;
			return false;
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x00065004 File Offset: 0x00064004
		internal int MatchLongestWords(string[] words, ref int maxMatchStrLen)
		{
			int result = -1;
			for (int i = 0; i < words.Length; i++)
			{
				string text = words[i];
				int length = text.Length;
				if (this.MatchSpecifiedWords(text, false, ref length) && length > maxMatchStrLen)
				{
					maxMatchStrLen = length;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x00065044 File Offset: 0x00064044
		internal int GetRepeatCount()
		{
			char c = this.Value[this.Index];
			int num = this.Index + 1;
			while (num < this.len && this.Value[num] == c)
			{
				num++;
			}
			int result = num - this.Index;
			this.Index = num - 1;
			return result;
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000650A0 File Offset: 0x000640A0
		internal bool GetNextDigit()
		{
			return ++this.Index < this.len && DateTimeParse.IsDigit(this.Value[this.Index]);
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000650DE File Offset: 0x000640DE
		internal char GetChar()
		{
			return this.Value[this.Index];
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000650F1 File Offset: 0x000640F1
		internal int GetDigit()
		{
			return (int)(this.Value[this.Index] - '0');
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x00065108 File Offset: 0x00064108
		internal void SkipWhiteSpaces()
		{
			while (this.Index + 1 < this.len)
			{
				char c = this.Value[this.Index + 1];
				if (!char.IsWhiteSpace(c))
				{
					return;
				}
				this.Index++;
			}
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x00065154 File Offset: 0x00064154
		internal bool SkipWhiteSpaceCurrent()
		{
			if (this.Index >= this.len)
			{
				return false;
			}
			if (!char.IsWhiteSpace(this.m_current))
			{
				return true;
			}
			while (++this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				if (!char.IsWhiteSpace(this.m_current))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000651C0 File Offset: 0x000641C0
		internal void TrimTail()
		{
			int num = this.len - 1;
			while (num >= 0 && char.IsWhiteSpace(this.Value[num]))
			{
				num--;
			}
			this.Value = this.Value.Substring(0, num + 1);
			this.len = this.Value.Length;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x0006521C File Offset: 0x0006421C
		internal void RemoveTrailingInQuoteSpaces()
		{
			int num = this.len - 1;
			if (num <= 1)
			{
				return;
			}
			char c = this.Value[num];
			if ((c == '\'' || c == '"') && char.IsWhiteSpace(this.Value[num - 1]))
			{
				num--;
				while (num >= 1 && char.IsWhiteSpace(this.Value[num - 1]))
				{
					num--;
				}
				this.Value = this.Value.Remove(num, this.Value.Length - 1 - num);
				this.len = this.Value.Length;
			}
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x000652B8 File Offset: 0x000642B8
		internal void RemoveLeadingInQuoteSpaces()
		{
			if (this.len <= 2)
			{
				return;
			}
			int num = 0;
			char c = this.Value[num];
			if (c != '\'')
			{
				if (c != '"')
				{
					return;
				}
			}
			while (num + 1 < this.len && char.IsWhiteSpace(this.Value[num + 1]))
			{
				num++;
			}
			if (num != 0)
			{
				this.Value = this.Value.Remove(1, num);
				this.len = this.Value.Length;
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x00065338 File Offset: 0x00064338
		internal DTSubString GetSubString()
		{
			DTSubString result = default(DTSubString);
			result.index = this.Index;
			result.s = this.Value;
			while (this.Index + result.length < this.len)
			{
				char c = this.Value[this.Index + result.length];
				DTSubStringType dtsubStringType;
				if (c >= '0' && c <= '9')
				{
					dtsubStringType = DTSubStringType.Number;
				}
				else
				{
					dtsubStringType = DTSubStringType.Other;
				}
				if (result.length == 0)
				{
					result.type = dtsubStringType;
				}
				else if (result.type != dtsubStringType)
				{
					break;
				}
				result.length++;
				if (dtsubStringType != DTSubStringType.Number)
				{
					break;
				}
				if (result.length > 8)
				{
					result.type = DTSubStringType.Invalid;
					return result;
				}
				int num = (int)(c - '0');
				result.value = result.value * 10 + num;
			}
			if (result.length == 0)
			{
				result.type = DTSubStringType.End;
				return result;
			}
			return result;
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0006541F File Offset: 0x0006441F
		internal void ConsumeSubString(DTSubString sub)
		{
			this.Index = sub.index + sub.length;
			if (this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
			}
		}

		// Token: 0x04000FF6 RID: 4086
		internal string Value;

		// Token: 0x04000FF7 RID: 4087
		internal int Index;

		// Token: 0x04000FF8 RID: 4088
		internal int len;

		// Token: 0x04000FF9 RID: 4089
		internal char m_current;

		// Token: 0x04000FFA RID: 4090
		private CompareInfo m_info;

		// Token: 0x04000FFB RID: 4091
		private bool m_checkDigitToken;

		// Token: 0x04000FFC RID: 4092
		private static char[] WhiteSpaceChecks = new char[]
		{
			' ',
			'\u00a0'
		};
	}
}
