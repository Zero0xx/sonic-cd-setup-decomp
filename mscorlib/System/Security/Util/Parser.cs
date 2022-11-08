using System;
using System.IO;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x02000617 RID: 1559
	internal sealed class Parser
	{
		// Token: 0x06003867 RID: 14439 RVA: 0x000BD7A1 File Offset: 0x000BC7A1
		internal SecurityElement GetTopElement()
		{
			if (!this.ParsedSuccessfully())
			{
				throw new XmlSyntaxException(this._t.LineNo);
			}
			return this._doc.GetRootElement();
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x000BD7C7 File Offset: 0x000BC7C7
		internal bool ParsedSuccessfully()
		{
			return true;
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000BD7CC File Offset: 0x000BC7CC
		private void GetRequiredSizes(TokenizerStream stream, ref int index)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 1;
			SecurityElementType securityElementType = SecurityElementType.Regular;
			string text = null;
			bool flag5 = false;
			bool flag6 = false;
			int num2 = 0;
			for (;;)
			{
				short nextToken = stream.GetNextToken();
				while (nextToken != -1)
				{
					switch (nextToken & 255)
					{
					case 0:
						flag4 = true;
						flag6 = false;
						nextToken = stream.GetNextToken();
						if (nextToken == 2)
						{
							stream.TagLastToken(17408);
							for (;;)
							{
								nextToken = stream.GetNextToken();
								if (nextToken != 3)
								{
									break;
								}
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
							}
							if (nextToken == -1)
							{
								goto Block_9;
							}
							if (nextToken != 1)
							{
								goto Block_10;
							}
							flag4 = false;
							index++;
							flag6 = false;
							num--;
							flag = true;
							goto IL_3BD;
						}
						else if (nextToken == 3)
						{
							flag3 = true;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							if (securityElementType != SecurityElementType.Regular)
							{
								goto Block_12;
							}
							flag = true;
							num++;
							goto IL_3BD;
						}
						else
						{
							if (nextToken == 6)
							{
								num2 = 1;
								do
								{
									nextToken = stream.GetNextToken();
									switch (nextToken)
									{
									case 0:
										num2++;
										break;
									case 1:
										num2--;
										break;
									case 3:
										stream.ThrowAwayNextString();
										stream.TagLastToken(20480);
										break;
									}
								}
								while (num2 > 0);
								flag4 = false;
								flag6 = false;
								flag = true;
								goto IL_3BD;
							}
							if (nextToken != 5)
							{
								goto IL_2B7;
							}
							nextToken = stream.GetNextToken();
							if (nextToken != 3)
							{
								goto Block_17;
							}
							flag3 = true;
							securityElementType = SecurityElementType.Format;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							num2 = 1;
							num++;
							flag = true;
							goto IL_3BD;
						}
						break;
					case 1:
						if (flag4)
						{
							flag4 = false;
							goto IL_3C8;
						}
						goto IL_2E4;
					case 2:
						nextToken = stream.GetNextToken();
						if (nextToken == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_3BD;
						}
						goto IL_32D;
					case 3:
						if (flag4)
						{
							if (securityElementType == SecurityElementType.Comment)
							{
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
								goto IL_3BD;
							}
							if (text == null)
							{
								text = stream.GetNextString();
								goto IL_3BD;
							}
							if (!flag5)
							{
								goto Block_5;
							}
							stream.TagLastToken(16896);
							index += SecurityDocument.EncodedStringSize(text) + SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							text = null;
							flag5 = false;
							goto IL_3BD;
						}
						else
						{
							if (flag6)
							{
								stream.TagLastToken(25344);
								index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + SecurityDocument.EncodedStringSize(" ");
								goto IL_3BD;
							}
							stream.TagLastToken(17152);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							flag6 = true;
							goto IL_3BD;
						}
						break;
					case 4:
						flag5 = true;
						goto IL_3BD;
					case 5:
						if (!flag4 || securityElementType != SecurityElementType.Format || num2 != 1)
						{
							goto IL_39B;
						}
						nextToken = stream.GetNextToken();
						if (nextToken == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_3BD;
						}
						goto IL_380;
					}
					goto Block_1;
					IL_3C8:
					nextToken = stream.GetNextToken();
					continue;
					IL_3BD:
					if (flag)
					{
						flag = false;
						flag2 = false;
						break;
					}
					flag2 = true;
					goto IL_3C8;
				}
				if (flag2)
				{
					index++;
					num--;
					flag6 = false;
				}
				else if (nextToken == -1 && (num != 1 || !flag3))
				{
					goto IL_3F9;
				}
				if (num <= 1)
				{
					return;
				}
			}
			Block_1:
			goto IL_3AC;
			Block_5:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_9:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
			Block_10:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedCloseBracket"));
			Block_12:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_17:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_2B7:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedSlashOrString"));
			IL_2E4:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedCloseBracket"));
			IL_32D:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedCloseBracket"));
			IL_380:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedCloseBracket"));
			IL_39B:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_3AC:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_3F9:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x000BDBF8 File Offset: 0x000BCBF8
		private int DetermineFormat(TokenizerStream stream)
		{
			if (stream.GetNextToken() == 0 && stream.GetNextToken() == 5)
			{
				this._t.GetTokens(stream, -1, true);
				stream.GoToPosition(2);
				bool flag = false;
				bool flag2 = false;
				short nextToken = stream.GetNextToken();
				while (nextToken != -1 && nextToken != 1)
				{
					switch (nextToken)
					{
					case 3:
						if (flag && flag2)
						{
							this._t.ChangeFormat(Encoding.GetEncoding(stream.GetNextString()));
							return 0;
						}
						if (!flag)
						{
							if (string.Compare(stream.GetNextString(), "encoding", StringComparison.Ordinal) == 0)
							{
								flag2 = true;
							}
						}
						else
						{
							flag = false;
							flag2 = false;
							stream.ThrowAwayNextString();
						}
						break;
					case 4:
						flag = true;
						break;
					default:
						throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
					}
					nextToken = stream.GetNextToken();
				}
				return 0;
			}
			return 2;
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x000BDCCC File Offset: 0x000BCCCC
		private void ParseContents()
		{
			TokenizerStream tokenizerStream = new TokenizerStream();
			this._t.GetTokens(tokenizerStream, 2, false);
			tokenizerStream.Reset();
			int position = this.DetermineFormat(tokenizerStream);
			tokenizerStream.GoToPosition(position);
			this._t.GetTokens(tokenizerStream, -1, false);
			tokenizerStream.Reset();
			int numData = 0;
			this.GetRequiredSizes(tokenizerStream, ref numData);
			this._doc = new SecurityDocument(numData);
			int num = 0;
			tokenizerStream.Reset();
			for (short nextFullToken = tokenizerStream.GetNextFullToken(); nextFullToken != -1; nextFullToken = tokenizerStream.GetNextFullToken())
			{
				if ((nextFullToken & 16384) == 16384)
				{
					short num2 = (short)((int)nextFullToken & 65280);
					if (num2 <= 17152)
					{
						if (num2 == 16640)
						{
							this._doc.AddToken(1, ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
						if (num2 == 16896)
						{
							this._doc.AddToken(2, ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
						if (num2 == 17152)
						{
							this._doc.AddToken(3, ref num);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
					}
					else
					{
						if (num2 == 17408)
						{
							this._doc.AddToken(4, ref num);
							goto IL_19D;
						}
						if (num2 == 20480)
						{
							tokenizerStream.ThrowAwayNextString();
							goto IL_19D;
						}
						if (num2 == 25344)
						{
							this._doc.AppendString(" ", ref num);
							this._doc.AppendString(tokenizerStream.GetNextString(), ref num);
							goto IL_19D;
						}
					}
					throw new XmlSyntaxException();
				}
				IL_19D:;
			}
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x000BDE84 File Offset: 0x000BCE84
		private Parser(Tokenizer t)
		{
			this._t = t;
			this._doc = null;
			try
			{
				this.ParseContents();
			}
			finally
			{
				this._t.Recycle();
			}
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x000BDECC File Offset: 0x000BCECC
		internal Parser(string input) : this(new Tokenizer(input))
		{
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x000BDEDA File Offset: 0x000BCEDA
		internal Parser(string input, string[] searchStrings, string[] replaceStrings) : this(new Tokenizer(input, searchStrings, replaceStrings))
		{
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x000BDEEA File Offset: 0x000BCEEA
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding) : this(new Tokenizer(array, encoding, 0))
		{
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x000BDEFA File Offset: 0x000BCEFA
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding, int startIndex) : this(new Tokenizer(array, encoding, startIndex))
		{
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000BDF0A File Offset: 0x000BCF0A
		internal Parser(StreamReader input) : this(new Tokenizer(input))
		{
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x000BDF18 File Offset: 0x000BCF18
		internal Parser(char[] array) : this(new Tokenizer(array))
		{
		}

		// Token: 0x04001D2F RID: 7471
		private const short c_flag = 16384;

		// Token: 0x04001D30 RID: 7472
		private const short c_elementtag = 16640;

		// Token: 0x04001D31 RID: 7473
		private const short c_attributetag = 16896;

		// Token: 0x04001D32 RID: 7474
		private const short c_texttag = 17152;

		// Token: 0x04001D33 RID: 7475
		private const short c_additionaltexttag = 25344;

		// Token: 0x04001D34 RID: 7476
		private const short c_childrentag = 17408;

		// Token: 0x04001D35 RID: 7477
		private const short c_wastedstringtag = 20480;

		// Token: 0x04001D36 RID: 7478
		private SecurityDocument _doc;

		// Token: 0x04001D37 RID: 7479
		private Tokenizer _t;
	}
}
