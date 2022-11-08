using System;

namespace System.Text
{
	// Token: 0x0200040B RID: 1035
	[Serializable]
	public sealed class EncoderReplacementFallback : EncoderFallback
	{
		// Token: 0x06002A76 RID: 10870 RVA: 0x000846E2 File Offset: 0x000836E2
		public EncoderReplacementFallback() : this("?")
		{
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000846F0 File Offset: 0x000836F0
		public EncoderReplacementFallback(string replacement)
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

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x00084775 File Offset: 0x00083775
		public string DefaultString
		{
			get
			{
				return this.strDefault;
			}
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x0008477D File Offset: 0x0008377D
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new EncoderReplacementFallbackBuffer(this);
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x00084785 File Offset: 0x00083785
		public override int MaxCharCount
		{
			get
			{
				return this.strDefault.Length;
			}
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x00084794 File Offset: 0x00083794
		public override bool Equals(object value)
		{
			EncoderReplacementFallback encoderReplacementFallback = value as EncoderReplacementFallback;
			return encoderReplacementFallback != null && this.strDefault == encoderReplacementFallback.strDefault;
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000847BE File Offset: 0x000837BE
		public override int GetHashCode()
		{
			return this.strDefault.GetHashCode();
		}

		// Token: 0x040014AB RID: 5291
		private string strDefault;
	}
}
