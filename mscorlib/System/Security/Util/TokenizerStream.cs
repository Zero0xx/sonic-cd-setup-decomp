using System;

namespace System.Security.Util
{
	// Token: 0x02000620 RID: 1568
	internal sealed class TokenizerStream
	{
		// Token: 0x06003888 RID: 14472 RVA: 0x000BEB2E File Offset: 0x000BDB2E
		internal TokenizerStream()
		{
			this.m_countTokens = 0;
			this.m_headTokens = new TokenizerShortBlock();
			this.m_headStrings = new TokenizerStringBlock();
			this.Reset();
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x000BEB5C File Offset: 0x000BDB5C
		internal void AddToken(short token)
		{
			if (this.m_currentTokens.m_block.Length <= this.m_indexTokens)
			{
				this.m_currentTokens.m_next = new TokenizerShortBlock();
				this.m_currentTokens = this.m_currentTokens.m_next;
				this.m_indexTokens = 0;
			}
			this.m_countTokens++;
			this.m_currentTokens.m_block[this.m_indexTokens++] = token;
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x000BEBD4 File Offset: 0x000BDBD4
		internal void AddString(string str)
		{
			if (this.m_currentStrings.m_block.Length <= this.m_indexStrings)
			{
				this.m_currentStrings.m_next = new TokenizerStringBlock();
				this.m_currentStrings = this.m_currentStrings.m_next;
				this.m_indexStrings = 0;
			}
			this.m_currentStrings.m_block[this.m_indexStrings++] = str;
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x000BEC3C File Offset: 0x000BDC3C
		internal void Reset()
		{
			this.m_lastTokens = null;
			this.m_currentTokens = this.m_headTokens;
			this.m_currentStrings = this.m_headStrings;
			this.m_indexTokens = 0;
			this.m_indexStrings = 0;
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x000BEC6C File Offset: 0x000BDC6C
		internal short GetNextFullToken()
		{
			if (this.m_currentTokens.m_block.Length <= this.m_indexTokens)
			{
				this.m_lastTokens = this.m_currentTokens;
				this.m_currentTokens = this.m_currentTokens.m_next;
				this.m_indexTokens = 0;
			}
			return this.m_currentTokens.m_block[this.m_indexTokens++];
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x000BECD0 File Offset: 0x000BDCD0
		internal short GetNextToken()
		{
			return this.GetNextFullToken() & 255;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x000BECEC File Offset: 0x000BDCEC
		internal string GetNextString()
		{
			if (this.m_currentStrings.m_block.Length <= this.m_indexStrings)
			{
				this.m_currentStrings = this.m_currentStrings.m_next;
				this.m_indexStrings = 0;
			}
			return this.m_currentStrings.m_block[this.m_indexStrings++];
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x000BED43 File Offset: 0x000BDD43
		internal void ThrowAwayNextString()
		{
			this.GetNextString();
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x000BED4C File Offset: 0x000BDD4C
		internal void TagLastToken(short tag)
		{
			if (this.m_indexTokens == 0)
			{
				this.m_lastTokens.m_block[this.m_lastTokens.m_block.Length - 1] = (short)((ushort)this.m_lastTokens.m_block[this.m_lastTokens.m_block.Length - 1] | (ushort)tag);
				return;
			}
			this.m_currentTokens.m_block[this.m_indexTokens - 1] = (short)((ushort)this.m_currentTokens.m_block[this.m_indexTokens - 1] | (ushort)tag);
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x000BEDCA File Offset: 0x000BDDCA
		internal int GetTokenCount()
		{
			return this.m_countTokens;
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x000BEDD4 File Offset: 0x000BDDD4
		internal void GoToPosition(int position)
		{
			this.Reset();
			for (int i = 0; i < position; i++)
			{
				if (this.GetNextToken() == 3)
				{
					this.ThrowAwayNextString();
				}
			}
		}

		// Token: 0x04001D75 RID: 7541
		private int m_countTokens;

		// Token: 0x04001D76 RID: 7542
		private TokenizerShortBlock m_headTokens;

		// Token: 0x04001D77 RID: 7543
		private TokenizerShortBlock m_lastTokens;

		// Token: 0x04001D78 RID: 7544
		private TokenizerShortBlock m_currentTokens;

		// Token: 0x04001D79 RID: 7545
		private int m_indexTokens;

		// Token: 0x04001D7A RID: 7546
		private TokenizerStringBlock m_headStrings;

		// Token: 0x04001D7B RID: 7547
		private TokenizerStringBlock m_currentStrings;

		// Token: 0x04001D7C RID: 7548
		private int m_indexStrings;
	}
}
