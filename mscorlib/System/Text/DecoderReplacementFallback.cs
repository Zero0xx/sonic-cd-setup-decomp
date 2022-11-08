using System;

namespace System.Text
{
	// Token: 0x02000401 RID: 1025
	[Serializable]
	public sealed class DecoderReplacementFallback : DecoderFallback
	{
		// Token: 0x06002A23 RID: 10787 RVA: 0x00083726 File Offset: 0x00082726
		public DecoderReplacementFallback() : this("?")
		{
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x00083734 File Offset: 0x00082734
		public DecoderReplacementFallback(string replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			bool flag = false;
			for (int i = 0; i < replacement.Length; i++)
			{
				if (char.IsSurrogate(replacement, i))
				{
					if (char.IsHighSurrogate(replacement, i))
					{
						if (flag)
						{
							break;
						}
						flag = true;
					}
					else
					{
						if (!flag)
						{
							flag = true;
							break;
						}
						flag = false;
					}
				}
				else if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex", new object[]
				{
					"replacement"
				}));
			}
			this.strDefault = replacement;
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x000837B9 File Offset: 0x000827B9
		public string DefaultString
		{
			get
			{
				return this.strDefault;
			}
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000837C1 File Offset: 0x000827C1
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new DecoderReplacementFallbackBuffer(this);
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x000837C9 File Offset: 0x000827C9
		public override int MaxCharCount
		{
			get
			{
				return this.strDefault.Length;
			}
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x000837D8 File Offset: 0x000827D8
		public override bool Equals(object value)
		{
			DecoderReplacementFallback decoderReplacementFallback = value as DecoderReplacementFallback;
			return decoderReplacementFallback != null && this.strDefault == decoderReplacementFallback.strDefault;
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x00083802 File Offset: 0x00082802
		public override int GetHashCode()
		{
			return this.strDefault.GetHashCode();
		}

		// Token: 0x0400148B RID: 5259
		private string strDefault;
	}
}
