using System;

namespace System.Net
{
	// Token: 0x02000538 RID: 1336
	internal struct ShellExpression
	{
		// Token: 0x060028D6 RID: 10454 RVA: 0x000A9B6F File Offset: 0x000A8B6F
		internal ShellExpression(string pattern)
		{
			this.pattern = null;
			this.match = null;
			this.Parse(pattern);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000A9B88 File Offset: 0x000A8B88
		internal bool IsMatch(string target)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			bool result = false;
			for (;;)
			{
				if (!flag)
				{
					if (num2 > target.Length)
					{
						return result;
					}
					switch (this.pattern[num])
					{
					case ShellExpression.ShExpTokens.End:
						if (num2 == target.Length)
						{
							goto Block_10;
						}
						flag = true;
						break;
					case ShellExpression.ShExpTokens.Start:
						if (num2 != 0)
						{
							return result;
						}
						this.match[num++] = 0;
						break;
					case ShellExpression.ShExpTokens.AugmentedQuestion:
						if (num2 == target.Length || target[num2] == '.')
						{
							this.match[num++] = num2;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.AugmentedAsterisk:
						if (num2 == target.Length || target[num2] == '.')
						{
							flag = true;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.AugmentedDot:
						if (num2 == target.Length)
						{
							this.match[num++] = num2;
						}
						else if (target[num2] == '.')
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						else
						{
							flag = true;
						}
						break;
					case ShellExpression.ShExpTokens.Question:
						if (num2 == target.Length)
						{
							flag = true;
						}
						else
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						break;
					case ShellExpression.ShExpTokens.Asterisk:
						num2 = (this.match[num++] = target.Length);
						break;
					default:
						if (num2 < target.Length && this.pattern[num] == (ShellExpression.ShExpTokens)char.ToLowerInvariant(target[num2]))
						{
							num2 = (this.match[num++] = num2 + 1);
						}
						else
						{
							flag = true;
						}
						break;
					}
				}
				else
				{
					switch (this.pattern[--num])
					{
					case ShellExpression.ShExpTokens.End:
					case ShellExpression.ShExpTokens.Start:
						return result;
					case ShellExpression.ShExpTokens.AugmentedQuestion:
					case ShellExpression.ShExpTokens.Asterisk:
						if (this.match[num] != this.match[num - 1])
						{
							num2 = --this.match[num++];
							flag = false;
						}
						break;
					}
				}
			}
			Block_10:
			result = true;
			return result;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000A9DB0 File Offset: 0x000A8DB0
		private void Parse(string patString)
		{
			this.pattern = new ShellExpression.ShExpTokens[patString.Length + 2];
			this.match = null;
			int num = 0;
			this.pattern[num++] = ShellExpression.ShExpTokens.Start;
			for (int i = 0; i < patString.Length; i++)
			{
				char c = patString[i];
				if (c != '*')
				{
					if (c != '?')
					{
						if (c != '^')
						{
							this.pattern[num++] = (ShellExpression.ShExpTokens)char.ToLowerInvariant(patString[i]);
						}
						else
						{
							if (i >= patString.Length - 1)
							{
								this.pattern = null;
								if (Logging.On)
								{
									Logging.PrintWarning(Logging.Web, SR.GetString("net_log_shell_expression_pattern_format_warning", new object[]
									{
										patString
									}));
								}
								throw new FormatException(SR.GetString("net_format_shexp", new object[]
								{
									patString
								}));
							}
							i++;
							char c2 = patString[i];
							if (c2 != '*')
							{
								if (c2 != '.')
								{
									if (c2 != '?')
									{
										this.pattern = null;
										if (Logging.On)
										{
											Logging.PrintWarning(Logging.Web, SR.GetString("net_log_shell_expression_pattern_format_warning", new object[]
											{
												patString
											}));
										}
										throw new FormatException(SR.GetString("net_format_shexp", new object[]
										{
											patString
										}));
									}
									this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedQuestion;
								}
								else
								{
									this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedDot;
								}
							}
							else
							{
								this.pattern[num++] = ShellExpression.ShExpTokens.AugmentedAsterisk;
							}
						}
					}
					else
					{
						this.pattern[num++] = ShellExpression.ShExpTokens.Question;
					}
				}
				else
				{
					this.pattern[num++] = ShellExpression.ShExpTokens.Asterisk;
				}
			}
			this.pattern[num++] = ShellExpression.ShExpTokens.End;
			this.match = new int[num];
		}

		// Token: 0x040027B9 RID: 10169
		private ShellExpression.ShExpTokens[] pattern;

		// Token: 0x040027BA RID: 10170
		private int[] match;

		// Token: 0x02000539 RID: 1337
		private enum ShExpTokens
		{
			// Token: 0x040027BC RID: 10172
			Asterisk = -1,
			// Token: 0x040027BD RID: 10173
			Question = -2,
			// Token: 0x040027BE RID: 10174
			AugmentedDot = -3,
			// Token: 0x040027BF RID: 10175
			AugmentedAsterisk = -4,
			// Token: 0x040027C0 RID: 10176
			AugmentedQuestion = -5,
			// Token: 0x040027C1 RID: 10177
			Start = -6,
			// Token: 0x040027C2 RID: 10178
			End = -7
		}
	}
}
