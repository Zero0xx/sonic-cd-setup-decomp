using System;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000024 RID: 36
	internal sealed class RegexInterpreter : RegexRunner
	{
		// Token: 0x0600017C RID: 380 RVA: 0x0000BB1C File Offset: 0x0000AB1C
		internal RegexInterpreter(RegexCode code, CultureInfo culture)
		{
			this.runcode = code;
			this.runcodes = code._codes;
			this.runstrings = code._strings;
			this.runfcPrefix = code._fcPrefix;
			this.runbmPrefix = code._bmPrefix;
			this.runanchors = code._anchors;
			this.runculture = culture;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000BB79 File Offset: 0x0000AB79
		protected override void InitTrackCount()
		{
			this.runtrackcount = this.runcode._trackcount;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000BB8C File Offset: 0x0000AB8C
		private void Advance()
		{
			this.Advance(0);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000BB95 File Offset: 0x0000AB95
		private void Advance(int i)
		{
			this.runcodepos += i + 1;
			this.SetOperator(this.runcodes[this.runcodepos]);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000BBBA File Offset: 0x0000ABBA
		private void Goto(int newpos)
		{
			if (newpos < this.runcodepos)
			{
				base.EnsureStorage();
			}
			this.SetOperator(this.runcodes[newpos]);
			this.runcodepos = newpos;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000BBE0 File Offset: 0x0000ABE0
		private void Textto(int newpos)
		{
			this.runtextpos = newpos;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000BBE9 File Offset: 0x0000ABE9
		private void Trackto(int newpos)
		{
			this.runtrackpos = this.runtrack.Length - newpos;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000BBFB File Offset: 0x0000ABFB
		private int Textstart()
		{
			return this.runtextstart;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000BC03 File Offset: 0x0000AC03
		private int Textpos()
		{
			return this.runtextpos;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000BC0B File Offset: 0x0000AC0B
		private int Trackpos()
		{
			return this.runtrack.Length - this.runtrackpos;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000BC1C File Offset: 0x0000AC1C
		private void TrackPush()
		{
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000BC48 File Offset: 0x0000AC48
		private void TrackPush(int I1)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000BC8C File Offset: 0x0000AC8C
		private void TrackPush(int I1, int I2)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = I2;
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000BCEC File Offset: 0x0000ACEC
		private void TrackPush(int I1, int I2, int I3)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = I2;
			this.runtrack[--this.runtrackpos] = I3;
			this.runtrack[--this.runtrackpos] = this.runcodepos;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000BD64 File Offset: 0x0000AD64
		private void TrackPush2(int I1)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = -this.runcodepos;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000BDAC File Offset: 0x0000ADAC
		private void TrackPush2(int I1, int I2)
		{
			this.runtrack[--this.runtrackpos] = I1;
			this.runtrack[--this.runtrackpos] = I2;
			this.runtrack[--this.runtrackpos] = -this.runcodepos;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000BE0C File Offset: 0x0000AE0C
		private void Backtrack()
		{
			int num = this.runtrack[this.runtrackpos++];
			if (num < 0)
			{
				num = -num;
				this.SetOperator(this.runcodes[num] | 256);
			}
			else
			{
				this.SetOperator(this.runcodes[num] | 128);
			}
			if (num < this.runcodepos)
			{
				base.EnsureStorage();
			}
			this.runcodepos = num;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000BE79 File Offset: 0x0000AE79
		private void SetOperator(int op)
		{
			this.runci = (0 != (op & 512));
			this.runrtl = (0 != (op & 64));
			this.runoperator = (op & -577);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000BEAB File Offset: 0x0000AEAB
		private void TrackPop()
		{
			this.runtrackpos++;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000BEBB File Offset: 0x0000AEBB
		private void TrackPop(int framesize)
		{
			this.runtrackpos += framesize;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000BECB File Offset: 0x0000AECB
		private int TrackPeek()
		{
			return this.runtrack[this.runtrackpos - 1];
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000BEDC File Offset: 0x0000AEDC
		private int TrackPeek(int i)
		{
			return this.runtrack[this.runtrackpos - i - 1];
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000BEF0 File Offset: 0x0000AEF0
		private void StackPush(int I1)
		{
			this.runstack[--this.runstackpos] = I1;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000BF18 File Offset: 0x0000AF18
		private void StackPush(int I1, int I2)
		{
			this.runstack[--this.runstackpos] = I1;
			this.runstack[--this.runstackpos] = I2;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000BF57 File Offset: 0x0000AF57
		private void StackPop()
		{
			this.runstackpos++;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000BF67 File Offset: 0x0000AF67
		private void StackPop(int framesize)
		{
			this.runstackpos += framesize;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000BF77 File Offset: 0x0000AF77
		private int StackPeek()
		{
			return this.runstack[this.runstackpos - 1];
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000BF88 File Offset: 0x0000AF88
		private int StackPeek(int i)
		{
			return this.runstack[this.runstackpos - i - 1];
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000BF9B File Offset: 0x0000AF9B
		private int Operator()
		{
			return this.runoperator;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000BFA3 File Offset: 0x0000AFA3
		private int Operand(int i)
		{
			return this.runcodes[this.runcodepos + i + 1];
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000BFB6 File Offset: 0x0000AFB6
		private int Leftchars()
		{
			return this.runtextpos - this.runtextbeg;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000BFC5 File Offset: 0x0000AFC5
		private int Rightchars()
		{
			return this.runtextend - this.runtextpos;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000BFD4 File Offset: 0x0000AFD4
		private int Bump()
		{
			if (!this.runrtl)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000BFE1 File Offset: 0x0000AFE1
		private int Forwardchars()
		{
			if (!this.runrtl)
			{
				return this.runtextend - this.runtextpos;
			}
			return this.runtextpos - this.runtextbeg;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000C008 File Offset: 0x0000B008
		private char Forwardcharnext()
		{
			char c = this.runrtl ? this.runtext[--this.runtextpos] : this.runtext[this.runtextpos++];
			if (!this.runci)
			{
				return c;
			}
			return char.ToLower(c, this.runculture);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000C070 File Offset: 0x0000B070
		private bool Stringmatch(string str)
		{
			int num;
			int num2;
			if (!this.runrtl)
			{
				if (this.runtextend - this.runtextpos < (num = str.Length))
				{
					return false;
				}
				num2 = this.runtextpos + num;
			}
			else
			{
				if (this.runtextpos - this.runtextbeg < (num = str.Length))
				{
					return false;
				}
				num2 = this.runtextpos;
			}
			if (!this.runci)
			{
				while (num != 0)
				{
					if (str[--num] != this.runtext[--num2])
					{
						return false;
					}
				}
			}
			else
			{
				while (num != 0)
				{
					if (str[--num] != char.ToLower(this.runtext[--num2], this.runculture))
					{
						return false;
					}
				}
			}
			if (!this.runrtl)
			{
				num2 += str.Length;
			}
			this.runtextpos = num2;
			return true;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000C140 File Offset: 0x0000B140
		private bool Refmatch(int index, int len)
		{
			int num;
			if (!this.runrtl)
			{
				if (this.runtextend - this.runtextpos < len)
				{
					return false;
				}
				num = this.runtextpos + len;
			}
			else
			{
				if (this.runtextpos - this.runtextbeg < len)
				{
					return false;
				}
				num = this.runtextpos;
			}
			int num2 = index + len;
			int num3 = len;
			if (!this.runci)
			{
				while (num3-- != 0)
				{
					if (this.runtext[--num2] != this.runtext[--num])
					{
						return false;
					}
				}
			}
			else
			{
				while (num3-- != 0)
				{
					if (char.ToLower(this.runtext[--num2], this.runculture) != char.ToLower(this.runtext[--num], this.runculture))
					{
						return false;
					}
				}
			}
			if (!this.runrtl)
			{
				num += len;
			}
			this.runtextpos = num;
			return true;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000C21D File Offset: 0x0000B21D
		private void Backwardnext()
		{
			this.runtextpos += (this.runrtl ? 1 : -1);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000C238 File Offset: 0x0000B238
		private char CharAt(int j)
		{
			return this.runtext[j];
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000C248 File Offset: 0x0000B248
		protected override bool FindFirstChar()
		{
			if ((this.runanchors & 53) != 0)
			{
				if (!this.runcode._rightToLeft)
				{
					if (((this.runanchors & 1) != 0 && this.runtextpos > this.runtextbeg) || ((this.runanchors & 4) != 0 && this.runtextpos > this.runtextstart))
					{
						this.runtextpos = this.runtextend;
						return false;
					}
					if ((this.runanchors & 16) != 0 && this.runtextpos < this.runtextend - 1)
					{
						this.runtextpos = this.runtextend - 1;
					}
					else if ((this.runanchors & 32) != 0 && this.runtextpos < this.runtextend)
					{
						this.runtextpos = this.runtextend;
					}
				}
				else
				{
					if (((this.runanchors & 32) != 0 && this.runtextpos < this.runtextend) || ((this.runanchors & 16) != 0 && (this.runtextpos < this.runtextend - 1 || (this.runtextpos == this.runtextend - 1 && this.CharAt(this.runtextpos) != '\n'))) || ((this.runanchors & 4) != 0 && this.runtextpos < this.runtextstart))
					{
						this.runtextpos = this.runtextbeg;
						return false;
					}
					if ((this.runanchors & 1) != 0 && this.runtextpos > this.runtextbeg)
					{
						this.runtextpos = this.runtextbeg;
					}
				}
				if (this.runbmPrefix != null)
				{
					return this.runbmPrefix.IsMatch(this.runtext, this.runtextpos, this.runtextbeg, this.runtextend);
				}
			}
			else if (this.runbmPrefix != null)
			{
				this.runtextpos = this.runbmPrefix.Scan(this.runtext, this.runtextpos, this.runtextbeg, this.runtextend);
				if (this.runtextpos == -1)
				{
					this.runtextpos = (this.runcode._rightToLeft ? this.runtextbeg : this.runtextend);
					return false;
				}
				return true;
			}
			if (this.runfcPrefix == null)
			{
				return true;
			}
			this.runrtl = this.runcode._rightToLeft;
			this.runci = this.runfcPrefix.CaseInsensitive;
			string prefix = this.runfcPrefix.Prefix;
			if (RegexCharClass.IsSingleton(prefix))
			{
				char c = RegexCharClass.SingletonChar(prefix);
				for (int i = this.Forwardchars(); i > 0; i--)
				{
					if (c == this.Forwardcharnext())
					{
						this.Backwardnext();
						return true;
					}
				}
			}
			else
			{
				for (int i = this.Forwardchars(); i > 0; i--)
				{
					if (RegexCharClass.CharInClass(this.Forwardcharnext(), prefix))
					{
						this.Backwardnext();
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000C4D0 File Offset: 0x0000B4D0
		protected override void Go()
		{
			this.Goto(0);
			for (;;)
			{
				int num = this.Operator();
				switch (num)
				{
				case 0:
				{
					int num2 = this.Operand(1);
					if (this.Forwardchars() >= num2)
					{
						char c = (char)this.Operand(0);
						while (num2-- > 0)
						{
							if (this.Forwardcharnext() != c)
							{
								goto IL_E4E;
							}
						}
						this.Advance(2);
						continue;
					}
					break;
				}
				case 1:
				{
					int num3 = this.Operand(1);
					if (this.Forwardchars() >= num3)
					{
						char c2 = (char)this.Operand(0);
						while (num3-- > 0)
						{
							if (this.Forwardcharnext() == c2)
							{
								goto IL_E4E;
							}
						}
						this.Advance(2);
						continue;
					}
					break;
				}
				case 2:
				{
					int num4 = this.Operand(1);
					if (this.Forwardchars() >= num4)
					{
						string set = this.runstrings[this.Operand(0)];
						while (num4-- > 0)
						{
							if (!RegexCharClass.CharInClass(this.Forwardcharnext(), set))
							{
								goto IL_E4E;
							}
						}
						this.Advance(2);
						continue;
					}
					break;
				}
				case 3:
				{
					int num5 = this.Operand(1);
					if (num5 > this.Forwardchars())
					{
						num5 = this.Forwardchars();
					}
					char c3 = (char)this.Operand(0);
					int i;
					for (i = num5; i > 0; i--)
					{
						if (this.Forwardcharnext() != c3)
						{
							this.Backwardnext();
							break;
						}
					}
					if (num5 > i)
					{
						this.TrackPush(num5 - i - 1, this.Textpos() - this.Bump());
					}
					this.Advance(2);
					continue;
				}
				case 4:
				{
					int num6 = this.Operand(1);
					if (num6 > this.Forwardchars())
					{
						num6 = this.Forwardchars();
					}
					char c4 = (char)this.Operand(0);
					int j;
					for (j = num6; j > 0; j--)
					{
						if (this.Forwardcharnext() == c4)
						{
							this.Backwardnext();
							break;
						}
					}
					if (num6 > j)
					{
						this.TrackPush(num6 - j - 1, this.Textpos() - this.Bump());
					}
					this.Advance(2);
					continue;
				}
				case 5:
				{
					int num7 = this.Operand(1);
					if (num7 > this.Forwardchars())
					{
						num7 = this.Forwardchars();
					}
					string set2 = this.runstrings[this.Operand(0)];
					int k;
					for (k = num7; k > 0; k--)
					{
						if (!RegexCharClass.CharInClass(this.Forwardcharnext(), set2))
						{
							this.Backwardnext();
							break;
						}
					}
					if (num7 > k)
					{
						this.TrackPush(num7 - k - 1, this.Textpos() - this.Bump());
					}
					this.Advance(2);
					continue;
				}
				case 6:
				case 7:
				{
					int num8 = this.Operand(1);
					if (num8 > this.Forwardchars())
					{
						num8 = this.Forwardchars();
					}
					if (num8 > 0)
					{
						this.TrackPush(num8 - 1, this.Textpos());
					}
					this.Advance(2);
					continue;
				}
				case 8:
				{
					int num9 = this.Operand(1);
					if (num9 > this.Forwardchars())
					{
						num9 = this.Forwardchars();
					}
					if (num9 > 0)
					{
						this.TrackPush(num9 - 1, this.Textpos());
					}
					this.Advance(2);
					continue;
				}
				case 9:
					if (this.Forwardchars() >= 1 && this.Forwardcharnext() == (char)this.Operand(0))
					{
						this.Advance(1);
						continue;
					}
					break;
				case 10:
					if (this.Forwardchars() >= 1 && this.Forwardcharnext() != (char)this.Operand(0))
					{
						this.Advance(1);
						continue;
					}
					break;
				case 11:
					if (this.Forwardchars() >= 1 && RegexCharClass.CharInClass(this.Forwardcharnext(), this.runstrings[this.Operand(0)]))
					{
						this.Advance(1);
						continue;
					}
					break;
				case 12:
					if (this.Stringmatch(this.runstrings[this.Operand(0)]))
					{
						this.Advance(1);
						continue;
					}
					break;
				case 13:
				{
					int cap = this.Operand(0);
					if (base.IsMatched(cap))
					{
						if (!this.Refmatch(base.MatchIndex(cap), base.MatchLength(cap)))
						{
							break;
						}
					}
					else if ((this.runregex.roptions & RegexOptions.ECMAScript) == RegexOptions.None)
					{
						break;
					}
					this.Advance(1);
					continue;
				}
				case 14:
					if (this.Leftchars() <= 0 || this.CharAt(this.Textpos() - 1) == '\n')
					{
						this.Advance();
						continue;
					}
					break;
				case 15:
					if (this.Rightchars() <= 0 || this.CharAt(this.Textpos()) == '\n')
					{
						this.Advance();
						continue;
					}
					break;
				case 16:
					if (base.IsBoundary(this.Textpos(), this.runtextbeg, this.runtextend))
					{
						this.Advance();
						continue;
					}
					break;
				case 17:
					if (!base.IsBoundary(this.Textpos(), this.runtextbeg, this.runtextend))
					{
						this.Advance();
						continue;
					}
					break;
				case 18:
					if (this.Leftchars() <= 0)
					{
						this.Advance();
						continue;
					}
					break;
				case 19:
					if (this.Textpos() == this.Textstart())
					{
						this.Advance();
						continue;
					}
					break;
				case 20:
					if (this.Rightchars() <= 1 && (this.Rightchars() != 1 || this.CharAt(this.Textpos()) == '\n'))
					{
						this.Advance();
						continue;
					}
					break;
				case 21:
					if (this.Rightchars() <= 0)
					{
						this.Advance();
						continue;
					}
					break;
				case 22:
					break;
				case 23:
					this.TrackPush(this.Textpos());
					this.Advance(1);
					continue;
				case 24:
				{
					this.StackPop();
					int num10 = this.Textpos() - this.StackPeek();
					if (num10 != 0)
					{
						this.TrackPush(this.StackPeek(), this.Textpos());
						this.StackPush(this.Textpos());
						this.Goto(this.Operand(0));
						continue;
					}
					this.TrackPush2(this.StackPeek());
					this.Advance(1);
					continue;
				}
				case 25:
				{
					this.StackPop();
					int num11 = this.StackPeek();
					if (this.Textpos() != num11)
					{
						if (num11 != -1)
						{
							this.TrackPush(num11, this.Textpos());
						}
						else
						{
							this.TrackPush(this.Textpos(), this.Textpos());
						}
					}
					else
					{
						this.StackPush(num11);
						this.TrackPush2(this.StackPeek());
					}
					this.Advance(1);
					continue;
				}
				case 26:
					this.StackPush(-1, this.Operand(0));
					this.TrackPush();
					this.Advance(1);
					continue;
				case 27:
					this.StackPush(this.Textpos(), this.Operand(0));
					this.TrackPush();
					this.Advance(1);
					continue;
				case 28:
				{
					this.StackPop(2);
					int num12 = this.StackPeek();
					int num13 = this.StackPeek(1);
					int num14 = this.Textpos() - num12;
					if (num13 >= this.Operand(1) || (num14 == 0 && num13 >= 0))
					{
						this.TrackPush2(num12, num13);
						this.Advance(2);
						continue;
					}
					this.TrackPush(num12);
					this.StackPush(this.Textpos(), num13 + 1);
					this.Goto(this.Operand(0));
					continue;
				}
				case 29:
				{
					this.StackPop(2);
					int i2 = this.StackPeek();
					int num15 = this.StackPeek(1);
					if (num15 < 0)
					{
						this.TrackPush2(i2);
						this.StackPush(this.Textpos(), num15 + 1);
						this.Goto(this.Operand(0));
						continue;
					}
					this.TrackPush(i2, num15, this.Textpos());
					this.Advance(2);
					continue;
				}
				case 30:
					this.StackPush(-1);
					this.TrackPush();
					this.Advance();
					continue;
				case 31:
					this.StackPush(this.Textpos());
					this.TrackPush();
					this.Advance();
					continue;
				case 32:
					if (this.Operand(1) == -1 || base.IsMatched(this.Operand(1)))
					{
						this.StackPop();
						if (this.Operand(1) != -1)
						{
							base.TransferCapture(this.Operand(0), this.Operand(1), this.StackPeek(), this.Textpos());
						}
						else
						{
							base.Capture(this.Operand(0), this.StackPeek(), this.Textpos());
						}
						this.TrackPush(this.StackPeek());
						this.Advance(2);
						continue;
					}
					break;
				case 33:
					this.StackPop();
					this.TrackPush(this.StackPeek());
					this.Textto(this.StackPeek());
					this.Advance();
					continue;
				case 34:
					this.StackPush(this.Trackpos(), base.Crawlpos());
					this.TrackPush();
					this.Advance();
					continue;
				case 35:
					this.StackPop(2);
					this.Trackto(this.StackPeek());
					while (base.Crawlpos() != this.StackPeek(1))
					{
						base.Uncapture();
					}
					break;
				case 36:
					this.StackPop(2);
					this.Trackto(this.StackPeek());
					this.TrackPush(this.StackPeek(1));
					this.Advance();
					continue;
				case 37:
					if (base.IsMatched(this.Operand(0)))
					{
						this.Advance(1);
						continue;
					}
					break;
				case 38:
					this.Goto(this.Operand(0));
					continue;
				case 39:
					goto IL_E3E;
				case 40:
					return;
				case 41:
					if (base.IsECMABoundary(this.Textpos(), this.runtextbeg, this.runtextend))
					{
						this.Advance();
						continue;
					}
					break;
				case 42:
					if (!base.IsECMABoundary(this.Textpos(), this.runtextbeg, this.runtextend))
					{
						this.Advance();
						continue;
					}
					break;
				default:
					switch (num)
					{
					case 131:
					case 132:
					{
						this.TrackPop(2);
						int num16 = this.TrackPeek();
						int num17 = this.TrackPeek(1);
						this.Textto(num17);
						if (num16 > 0)
						{
							this.TrackPush(num16 - 1, num17 - this.Bump());
						}
						this.Advance(2);
						continue;
					}
					case 133:
					{
						this.TrackPop(2);
						int num18 = this.TrackPeek();
						int num19 = this.TrackPeek(1);
						this.Textto(num19);
						if (num18 > 0)
						{
							this.TrackPush(num18 - 1, num19 - this.Bump());
						}
						this.Advance(2);
						continue;
					}
					case 134:
					{
						this.TrackPop(2);
						int num20 = this.TrackPeek(1);
						this.Textto(num20);
						if (this.Forwardcharnext() == (char)this.Operand(0))
						{
							int num21 = this.TrackPeek();
							if (num21 > 0)
							{
								this.TrackPush(num21 - 1, num20 + this.Bump());
							}
							this.Advance(2);
							continue;
						}
						break;
					}
					case 135:
					{
						this.TrackPop(2);
						int num22 = this.TrackPeek(1);
						this.Textto(num22);
						if (this.Forwardcharnext() != (char)this.Operand(0))
						{
							int num23 = this.TrackPeek();
							if (num23 > 0)
							{
								this.TrackPush(num23 - 1, num22 + this.Bump());
							}
							this.Advance(2);
							continue;
						}
						break;
					}
					case 136:
					{
						this.TrackPop(2);
						int num24 = this.TrackPeek(1);
						this.Textto(num24);
						if (RegexCharClass.CharInClass(this.Forwardcharnext(), this.runstrings[this.Operand(0)]))
						{
							int num25 = this.TrackPeek();
							if (num25 > 0)
							{
								this.TrackPush(num25 - 1, num24 + this.Bump());
							}
							this.Advance(2);
							continue;
						}
						break;
					}
					case 137:
					case 138:
					case 139:
					case 140:
					case 141:
					case 142:
					case 143:
					case 144:
					case 145:
					case 146:
					case 147:
					case 148:
					case 149:
					case 150:
					case 163:
						goto IL_E3E;
					case 151:
						this.TrackPop();
						this.Textto(this.TrackPeek());
						this.Goto(this.Operand(0));
						continue;
					case 152:
						this.TrackPop(2);
						this.StackPop();
						this.Textto(this.TrackPeek(1));
						this.TrackPush2(this.TrackPeek());
						this.Advance(1);
						continue;
					case 153:
					{
						this.TrackPop(2);
						int num26 = this.TrackPeek(1);
						this.TrackPush2(this.TrackPeek());
						this.StackPush(num26);
						this.Textto(num26);
						this.Goto(this.Operand(0));
						continue;
					}
					case 154:
						this.StackPop(2);
						break;
					case 155:
						this.StackPop(2);
						break;
					case 156:
						this.TrackPop();
						this.StackPop(2);
						if (this.StackPeek(1) > 0)
						{
							this.Textto(this.StackPeek());
							this.TrackPush2(this.TrackPeek(), this.StackPeek(1) - 1);
							this.Advance(2);
							continue;
						}
						this.StackPush(this.TrackPeek(), this.StackPeek(1) - 1);
						break;
					case 157:
					{
						this.TrackPop(3);
						int num27 = this.TrackPeek();
						int num28 = this.TrackPeek(2);
						if (this.TrackPeek(1) <= this.Operand(1) && num28 != num27)
						{
							this.Textto(num28);
							this.StackPush(num28, this.TrackPeek(1) + 1);
							this.TrackPush2(num27);
							this.Goto(this.Operand(0));
							continue;
						}
						this.StackPush(this.TrackPeek(), this.TrackPeek(1));
						break;
					}
					case 158:
					case 159:
						this.StackPop();
						break;
					case 160:
						this.TrackPop();
						this.StackPush(this.TrackPeek());
						base.Uncapture();
						if (this.Operand(0) != -1 && this.Operand(1) != -1)
						{
							base.Uncapture();
						}
						break;
					case 161:
						this.TrackPop();
						this.StackPush(this.TrackPeek());
						break;
					case 162:
						this.StackPop(2);
						break;
					case 164:
						this.TrackPop();
						while (base.Crawlpos() != this.TrackPeek())
						{
							base.Uncapture();
						}
						break;
					default:
						switch (num)
						{
						case 280:
							this.TrackPop();
							this.StackPush(this.TrackPeek());
							goto IL_E4E;
						case 281:
							this.StackPop();
							this.TrackPop();
							this.StackPush(this.TrackPeek());
							goto IL_E4E;
						case 284:
							this.TrackPop(2);
							this.StackPush(this.TrackPeek(), this.TrackPeek(1));
							goto IL_E4E;
						case 285:
							this.TrackPop();
							this.StackPop(2);
							this.StackPush(this.TrackPeek(), this.StackPeek(1) - 1);
							goto IL_E4E;
						}
						goto Block_3;
					}
					break;
				}
				IL_E4E:
				this.Backtrack();
			}
			Block_3:
			IL_E3E:
			throw new NotImplementedException(SR.GetString("UnimplementedState"));
		}

		// Token: 0x04000740 RID: 1856
		internal int runoperator;

		// Token: 0x04000741 RID: 1857
		internal int[] runcodes;

		// Token: 0x04000742 RID: 1858
		internal int runcodepos;

		// Token: 0x04000743 RID: 1859
		internal string[] runstrings;

		// Token: 0x04000744 RID: 1860
		internal RegexCode runcode;

		// Token: 0x04000745 RID: 1861
		internal RegexPrefix runfcPrefix;

		// Token: 0x04000746 RID: 1862
		internal RegexBoyerMoore runbmPrefix;

		// Token: 0x04000747 RID: 1863
		internal int runanchors;

		// Token: 0x04000748 RID: 1864
		internal bool runrtl;

		// Token: 0x04000749 RID: 1865
		internal bool runci;

		// Token: 0x0400074A RID: 1866
		internal CultureInfo runculture;
	}
}
