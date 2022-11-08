using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x02000618 RID: 1560
	internal sealed class Tokenizer
	{
		// Token: 0x06003873 RID: 14451 RVA: 0x000BDF28 File Offset: 0x000BCF28
		internal void BasicInitialization()
		{
			this.LineNo = 1;
			this._inProcessingTag = 0;
			this._inSavedCharacter = -1;
			this._inIndex = 0;
			this._inSize = 0;
			this._inNestedSize = 0;
			this._inNestedIndex = 0;
			this._inTokenSource = Tokenizer.TokenSource.Other;
			this._maker = SharedStatics.GetSharedStringMaker();
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x000BDF78 File Offset: 0x000BCF78
		public void Recycle()
		{
			SharedStatics.ReleaseSharedStringMaker(ref this._maker);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x000BDF85 File Offset: 0x000BCF85
		internal Tokenizer(string input)
		{
			this.BasicInitialization();
			this._inString = input;
			this._inSize = input.Length;
			this._inTokenSource = Tokenizer.TokenSource.String;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x000BDFAD File Offset: 0x000BCFAD
		internal Tokenizer(string input, string[] searchStrings, string[] replaceStrings)
		{
			this.BasicInitialization();
			this._inString = input;
			this._inSize = this._inString.Length;
			this._inTokenSource = Tokenizer.TokenSource.NestedStrings;
			this._searchStrings = searchStrings;
			this._replaceStrings = replaceStrings;
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x000BDFE8 File Offset: 0x000BCFE8
		internal Tokenizer(byte[] array, Tokenizer.ByteTokenEncoding encoding, int startIndex)
		{
			this.BasicInitialization();
			this._inBytes = array;
			this._inSize = array.Length;
			this._inIndex = startIndex;
			switch (encoding)
			{
			case Tokenizer.ByteTokenEncoding.UnicodeTokens:
				this._inTokenSource = Tokenizer.TokenSource.UnicodeByteArray;
				return;
			case Tokenizer.ByteTokenEncoding.UTF8Tokens:
				this._inTokenSource = Tokenizer.TokenSource.UTF8ByteArray;
				return;
			case Tokenizer.ByteTokenEncoding.ByteTokens:
				this._inTokenSource = Tokenizer.TokenSource.ASCIIByteArray;
				return;
			default:
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[]
				{
					(int)encoding
				}));
			}
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000BE070 File Offset: 0x000BD070
		internal Tokenizer(char[] array)
		{
			this.BasicInitialization();
			this._inChars = array;
			this._inSize = array.Length;
			this._inTokenSource = Tokenizer.TokenSource.CharArray;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x000BE095 File Offset: 0x000BD095
		internal Tokenizer(StreamReader input)
		{
			this.BasicInitialization();
			this._inTokenReader = new Tokenizer.StreamTokenReader(input);
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x000BE0B0 File Offset: 0x000BD0B0
		internal void ChangeFormat(Encoding encoding)
		{
			if (encoding == null)
			{
				return;
			}
			switch (this._inTokenSource)
			{
			case Tokenizer.TokenSource.UnicodeByteArray:
			case Tokenizer.TokenSource.UTF8ByteArray:
			case Tokenizer.TokenSource.ASCIIByteArray:
				if (encoding == Encoding.Unicode)
				{
					this._inTokenSource = Tokenizer.TokenSource.UnicodeByteArray;
					return;
				}
				if (encoding == Encoding.UTF8)
				{
					this._inTokenSource = Tokenizer.TokenSource.UTF8ByteArray;
					return;
				}
				if (encoding != Encoding.ASCII)
				{
					goto IL_5B;
				}
				this._inTokenSource = Tokenizer.TokenSource.ASCIIByteArray;
				break;
			case Tokenizer.TokenSource.CharArray:
			case Tokenizer.TokenSource.String:
			case Tokenizer.TokenSource.NestedStrings:
				break;
			default:
				goto IL_5B;
			}
			return;
			IL_5B:
			Stream stream;
			switch (this._inTokenSource)
			{
			case Tokenizer.TokenSource.UnicodeByteArray:
			case Tokenizer.TokenSource.UTF8ByteArray:
			case Tokenizer.TokenSource.ASCIIByteArray:
				stream = new MemoryStream(this._inBytes, this._inIndex, this._inSize - this._inIndex);
				break;
			case Tokenizer.TokenSource.CharArray:
			case Tokenizer.TokenSource.String:
			case Tokenizer.TokenSource.NestedStrings:
				return;
			default:
			{
				Tokenizer.StreamTokenReader streamTokenReader = this._inTokenReader as Tokenizer.StreamTokenReader;
				if (streamTokenReader == null)
				{
					return;
				}
				stream = streamTokenReader._in.BaseStream;
				string s = new string(' ', streamTokenReader.NumCharEncountered);
				stream.Position = (long)streamTokenReader._in.CurrentEncoding.GetByteCount(s);
				break;
			}
			}
			this._inTokenReader = new Tokenizer.StreamTokenReader(new StreamReader(stream, encoding));
			this._inTokenSource = Tokenizer.TokenSource.Other;
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x000BE1C0 File Offset: 0x000BD1C0
		internal void GetTokens(TokenizerStream stream, int maxNum, bool endAfterKet)
		{
			while (maxNum == -1 || stream.GetTokenCount() < maxNum)
			{
				int num = 0;
				bool flag = false;
				bool flag2 = false;
				Tokenizer.StringMaker maker = this._maker;
				maker._outStringBuilder = null;
				maker._outIndex = 0;
				int num2;
				for (;;)
				{
					if (this._inSavedCharacter != -1)
					{
						num2 = this._inSavedCharacter;
						this._inSavedCharacter = -1;
					}
					else
					{
						switch (this._inTokenSource)
						{
						case Tokenizer.TokenSource.UnicodeByteArray:
							if (this._inIndex + 1 >= this._inSize)
							{
								goto Block_3;
							}
							num2 = ((int)this._inBytes[this._inIndex + 1] << 8) + (int)this._inBytes[this._inIndex];
							this._inIndex += 2;
							break;
						case Tokenizer.TokenSource.UTF8ByteArray:
							if (this._inIndex >= this._inSize)
							{
								goto Block_4;
							}
							num2 = (int)this._inBytes[this._inIndex++];
							if ((num2 & 128) != 0)
							{
								switch ((num2 & 240) >> 4)
								{
								case 8:
								case 9:
								case 10:
								case 11:
									goto IL_12C;
								case 12:
								case 13:
									num2 &= 31;
									num = 2;
									break;
								case 14:
									num2 &= 15;
									num = 3;
									break;
								case 15:
									goto IL_14A;
								}
								if (this._inIndex >= this._inSize)
								{
									goto Block_7;
								}
								byte b = this._inBytes[this._inIndex++];
								if ((b & 192) != 128)
								{
									goto Block_8;
								}
								num2 = (num2 << 6 | (int)(b & 63));
								if (num != 2)
								{
									if (this._inIndex >= this._inSize)
									{
										goto Block_10;
									}
									b = this._inBytes[this._inIndex++];
									if ((b & 192) != 128)
									{
										goto Block_11;
									}
									num2 = (num2 << 6 | (int)(b & 63));
								}
							}
							break;
						case Tokenizer.TokenSource.ASCIIByteArray:
							if (this._inIndex >= this._inSize)
							{
								goto Block_12;
							}
							num2 = (int)this._inBytes[this._inIndex++];
							break;
						case Tokenizer.TokenSource.CharArray:
							if (this._inIndex >= this._inSize)
							{
								goto Block_13;
							}
							num2 = (int)this._inChars[this._inIndex++];
							break;
						case Tokenizer.TokenSource.String:
							if (this._inIndex >= this._inSize)
							{
								goto Block_14;
							}
							num2 = (int)this._inString[this._inIndex++];
							break;
						case Tokenizer.TokenSource.NestedStrings:
							if (this._inNestedSize != 0)
							{
								if (this._inNestedIndex < this._inNestedSize)
								{
									num2 = (int)this._inNestedString[this._inNestedIndex++];
									break;
								}
								this._inNestedSize = 0;
							}
							if (this._inIndex >= this._inSize)
							{
								goto Block_17;
							}
							num2 = (int)this._inString[this._inIndex++];
							if (num2 == 123)
							{
								for (int i = 0; i < this._searchStrings.Length; i++)
								{
									if (string.Compare(this._searchStrings[i], 0, this._inString, this._inIndex - 1, this._searchStrings[i].Length, StringComparison.Ordinal) == 0)
									{
										this._inNestedString = this._replaceStrings[i];
										this._inNestedSize = this._inNestedString.Length;
										this._inNestedIndex = 1;
										num2 = (int)this._inNestedString[0];
										this._inIndex += this._searchStrings[i].Length - 1;
										break;
									}
								}
							}
							break;
						default:
							num2 = this._inTokenReader.Read();
							if (num2 == -1)
							{
								goto Block_21;
							}
							break;
						}
					}
					if (!flag)
					{
						int num3 = num2;
						if (num3 <= 34)
						{
							switch (num3)
							{
							case 9:
							case 13:
								continue;
							case 10:
								this.LineNo++;
								continue;
							case 11:
							case 12:
								break;
							default:
								switch (num3)
								{
								case 32:
									continue;
								case 33:
									if (this._inProcessingTag != 0)
									{
										goto Block_31;
									}
									break;
								case 34:
									flag = true;
									flag2 = true;
									continue;
								}
								break;
							}
						}
						else
						{
							switch (num3)
							{
							case 45:
								if (this._inProcessingTag != 0)
								{
									goto Block_32;
								}
								break;
							case 46:
								break;
							case 47:
								if (this._inProcessingTag != 0)
								{
									goto Block_29;
								}
								break;
							default:
								switch (num3)
								{
								case 60:
									goto IL_492;
								case 61:
									goto IL_4C8;
								case 62:
									goto IL_4AC;
								case 63:
									if (this._inProcessingTag != 0)
									{
										goto Block_30;
									}
									break;
								}
								break;
							}
						}
					}
					else
					{
						int num4 = num2;
						if (num4 <= 34)
						{
							switch (num4)
							{
							case 9:
							case 13:
								break;
							case 10:
								this.LineNo++;
								if (!flag2)
								{
									goto Block_43;
								}
								goto IL_650;
							case 11:
							case 12:
								goto IL_650;
							default:
								switch (num4)
								{
								case 32:
									break;
								case 33:
									goto IL_650;
								case 34:
									if (flag2)
									{
										goto Block_41;
									}
									goto IL_650;
								default:
									goto IL_650;
								}
								break;
							}
							if (!flag2)
							{
								goto Block_42;
							}
						}
						else
						{
							if (num4 != 47)
							{
								switch (num4)
								{
								case 60:
									if (!flag2)
									{
										goto Block_38;
									}
									goto IL_650;
								case 61:
								case 62:
									break;
								default:
									goto IL_650;
								}
							}
							if (!flag2 && this._inProcessingTag != 0)
							{
								goto Block_40;
							}
						}
					}
					IL_650:
					flag = true;
					if (maker._outIndex < 512)
					{
						maker._outChars[maker._outIndex++] = (char)num2;
					}
					else
					{
						if (maker._outStringBuilder == null)
						{
							maker._outStringBuilder = new StringBuilder();
						}
						maker._outStringBuilder.Append(maker._outChars, 0, 512);
						maker._outChars[0] = (char)num2;
						maker._outIndex = 1;
					}
				}
				IL_492:
				this._inProcessingTag++;
				stream.AddToken(0);
				continue;
				Block_3:
				stream.AddToken(-1);
				return;
				IL_4AC:
				this._inProcessingTag--;
				stream.AddToken(1);
				if (endAfterKet)
				{
					return;
				}
				continue;
				IL_4C8:
				stream.AddToken(4);
				continue;
				Block_29:
				stream.AddToken(2);
				continue;
				Block_30:
				stream.AddToken(5);
				continue;
				Block_31:
				stream.AddToken(6);
				continue;
				Block_32:
				stream.AddToken(7);
				continue;
				Block_38:
				this._inSavedCharacter = num2;
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_40:
				this._inSavedCharacter = num2;
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_41:
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_42:
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_43:
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_4:
				stream.AddToken(-1);
				return;
				IL_12C:
				throw new XmlSyntaxException(this.LineNo);
				IL_14A:
				throw new XmlSyntaxException(this.LineNo);
				Block_7:
				throw new XmlSyntaxException(this.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
				Block_8:
				throw new XmlSyntaxException(this.LineNo);
				Block_10:
				throw new XmlSyntaxException(this.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
				Block_11:
				throw new XmlSyntaxException(this.LineNo);
				Block_12:
				stream.AddToken(-1);
				return;
				Block_13:
				stream.AddToken(-1);
				return;
				Block_14:
				stream.AddToken(-1);
				return;
				Block_17:
				stream.AddToken(-1);
				return;
				Block_21:
				stream.AddToken(-1);
				return;
			}
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x000BE8AA File Offset: 0x000BD8AA
		private string GetStringToken()
		{
			return this._maker.MakeString();
		}

		// Token: 0x04001D38 RID: 7480
		internal const byte bra = 0;

		// Token: 0x04001D39 RID: 7481
		internal const byte ket = 1;

		// Token: 0x04001D3A RID: 7482
		internal const byte slash = 2;

		// Token: 0x04001D3B RID: 7483
		internal const byte cstr = 3;

		// Token: 0x04001D3C RID: 7484
		internal const byte equals = 4;

		// Token: 0x04001D3D RID: 7485
		internal const byte quest = 5;

		// Token: 0x04001D3E RID: 7486
		internal const byte bang = 6;

		// Token: 0x04001D3F RID: 7487
		internal const byte dash = 7;

		// Token: 0x04001D40 RID: 7488
		internal const int intOpenBracket = 60;

		// Token: 0x04001D41 RID: 7489
		internal const int intCloseBracket = 62;

		// Token: 0x04001D42 RID: 7490
		internal const int intSlash = 47;

		// Token: 0x04001D43 RID: 7491
		internal const int intEquals = 61;

		// Token: 0x04001D44 RID: 7492
		internal const int intQuote = 34;

		// Token: 0x04001D45 RID: 7493
		internal const int intQuest = 63;

		// Token: 0x04001D46 RID: 7494
		internal const int intBang = 33;

		// Token: 0x04001D47 RID: 7495
		internal const int intDash = 45;

		// Token: 0x04001D48 RID: 7496
		internal const int intTab = 9;

		// Token: 0x04001D49 RID: 7497
		internal const int intCR = 13;

		// Token: 0x04001D4A RID: 7498
		internal const int intLF = 10;

		// Token: 0x04001D4B RID: 7499
		internal const int intSpace = 32;

		// Token: 0x04001D4C RID: 7500
		public int LineNo;

		// Token: 0x04001D4D RID: 7501
		private int _inProcessingTag;

		// Token: 0x04001D4E RID: 7502
		private byte[] _inBytes;

		// Token: 0x04001D4F RID: 7503
		private char[] _inChars;

		// Token: 0x04001D50 RID: 7504
		private string _inString;

		// Token: 0x04001D51 RID: 7505
		private int _inIndex;

		// Token: 0x04001D52 RID: 7506
		private int _inSize;

		// Token: 0x04001D53 RID: 7507
		private int _inSavedCharacter;

		// Token: 0x04001D54 RID: 7508
		private Tokenizer.TokenSource _inTokenSource;

		// Token: 0x04001D55 RID: 7509
		private Tokenizer.ITokenReader _inTokenReader;

		// Token: 0x04001D56 RID: 7510
		private Tokenizer.StringMaker _maker;

		// Token: 0x04001D57 RID: 7511
		private string[] _searchStrings;

		// Token: 0x04001D58 RID: 7512
		private string[] _replaceStrings;

		// Token: 0x04001D59 RID: 7513
		private int _inNestedIndex;

		// Token: 0x04001D5A RID: 7514
		private int _inNestedSize;

		// Token: 0x04001D5B RID: 7515
		private string _inNestedString;

		// Token: 0x02000619 RID: 1561
		private enum TokenSource
		{
			// Token: 0x04001D5D RID: 7517
			UnicodeByteArray,
			// Token: 0x04001D5E RID: 7518
			UTF8ByteArray,
			// Token: 0x04001D5F RID: 7519
			ASCIIByteArray,
			// Token: 0x04001D60 RID: 7520
			CharArray,
			// Token: 0x04001D61 RID: 7521
			String,
			// Token: 0x04001D62 RID: 7522
			NestedStrings,
			// Token: 0x04001D63 RID: 7523
			Other
		}

		// Token: 0x0200061A RID: 1562
		internal enum ByteTokenEncoding
		{
			// Token: 0x04001D65 RID: 7525
			UnicodeTokens,
			// Token: 0x04001D66 RID: 7526
			UTF8Tokens,
			// Token: 0x04001D67 RID: 7527
			ByteTokens
		}

		// Token: 0x0200061B RID: 1563
		[Serializable]
		internal sealed class StringMaker
		{
			// Token: 0x0600387D RID: 14461 RVA: 0x000BE8B8 File Offset: 0x000BD8B8
			private static uint HashString(string str)
			{
				uint num = 0U;
				int length = str.Length;
				for (int i = 0; i < length; i++)
				{
					num = (num << 3 ^ (uint)str[i] ^ num >> 29);
				}
				return num;
			}

			// Token: 0x0600387E RID: 14462 RVA: 0x000BE8EC File Offset: 0x000BD8EC
			private static uint HashCharArray(char[] a, int l)
			{
				uint num = 0U;
				for (int i = 0; i < l; i++)
				{
					num = (num << 3 ^ (uint)a[i] ^ num >> 29);
				}
				return num;
			}

			// Token: 0x0600387F RID: 14463 RVA: 0x000BE915 File Offset: 0x000BD915
			public StringMaker()
			{
				this.cStringsMax = 2048U;
				this.cStringsUsed = 0U;
				this.aStrings = new string[this.cStringsMax];
				this._outChars = new char[512];
			}

			// Token: 0x06003880 RID: 14464 RVA: 0x000BE954 File Offset: 0x000BD954
			private bool CompareStringAndChars(string str, char[] a, int l)
			{
				if (str.Length != l)
				{
					return false;
				}
				for (int i = 0; i < l; i++)
				{
					if (a[i] != str[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06003881 RID: 14465 RVA: 0x000BE988 File Offset: 0x000BD988
			public string MakeString()
			{
				char[] outChars = this._outChars;
				int outIndex = this._outIndex;
				if (this._outStringBuilder != null)
				{
					this._outStringBuilder.Append(this._outChars, 0, this._outIndex);
					return this._outStringBuilder.ToString();
				}
				uint num3;
				if (this.cStringsUsed > this.cStringsMax / 4U * 3U)
				{
					uint num = this.cStringsMax * 2U;
					string[] array = new string[num];
					int num2 = 0;
					while ((long)num2 < (long)((ulong)this.cStringsMax))
					{
						if (this.aStrings[num2] != null)
						{
							num3 = Tokenizer.StringMaker.HashString(this.aStrings[num2]) % num;
							while (array[(int)((UIntPtr)num3)] != null)
							{
								if ((num3 += 1U) >= num)
								{
									num3 = 0U;
								}
							}
							array[(int)((UIntPtr)num3)] = this.aStrings[num2];
						}
						num2++;
					}
					this.cStringsMax = num;
					this.aStrings = array;
				}
				num3 = Tokenizer.StringMaker.HashCharArray(outChars, outIndex) % this.cStringsMax;
				string text;
				while ((text = this.aStrings[(int)((UIntPtr)num3)]) != null)
				{
					if (this.CompareStringAndChars(text, outChars, outIndex))
					{
						return text;
					}
					if ((num3 += 1U) >= this.cStringsMax)
					{
						num3 = 0U;
					}
				}
				text = new string(outChars, 0, outIndex);
				this.aStrings[(int)((UIntPtr)num3)] = text;
				this.cStringsUsed += 1U;
				return text;
			}

			// Token: 0x04001D68 RID: 7528
			public const int outMaxSize = 512;

			// Token: 0x04001D69 RID: 7529
			private string[] aStrings;

			// Token: 0x04001D6A RID: 7530
			private uint cStringsMax;

			// Token: 0x04001D6B RID: 7531
			private uint cStringsUsed;

			// Token: 0x04001D6C RID: 7532
			public StringBuilder _outStringBuilder;

			// Token: 0x04001D6D RID: 7533
			public char[] _outChars;

			// Token: 0x04001D6E RID: 7534
			public int _outIndex;
		}

		// Token: 0x0200061C RID: 1564
		internal interface ITokenReader
		{
			// Token: 0x06003882 RID: 14466
			int Read();
		}

		// Token: 0x0200061D RID: 1565
		internal class StreamTokenReader : Tokenizer.ITokenReader
		{
			// Token: 0x06003883 RID: 14467 RVA: 0x000BEAB9 File Offset: 0x000BDAB9
			internal StreamTokenReader(StreamReader input)
			{
				this._in = input;
				this._numCharRead = 0;
			}

			// Token: 0x06003884 RID: 14468 RVA: 0x000BEAD0 File Offset: 0x000BDAD0
			public virtual int Read()
			{
				int num = this._in.Read();
				if (num != -1)
				{
					this._numCharRead++;
				}
				return num;
			}

			// Token: 0x1700097D RID: 2429
			// (get) Token: 0x06003885 RID: 14469 RVA: 0x000BEAFC File Offset: 0x000BDAFC
			internal int NumCharEncountered
			{
				get
				{
					return this._numCharRead;
				}
			}

			// Token: 0x04001D6F RID: 7535
			internal StreamReader _in;

			// Token: 0x04001D70 RID: 7536
			internal int _numCharRead;
		}
	}
}
