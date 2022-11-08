using System;
using System.Text;

namespace System.Xml
{
	// Token: 0x02000035 RID: 53
	internal class Ucs4Encoding : Encoding
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00007932 File Offset: 0x00006932
		public override Decoder GetDecoder()
		{
			return this.ucs4Decoder;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000793A File Offset: 0x0000693A
		public override int GetByteCount(char[] chars, int index, int count)
		{
			return checked(count * 4);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000793F File Offset: 0x0000693F
		public override int GetByteCount(char[] chars)
		{
			return chars.Length * 4;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007946 File Offset: 0x00006946
		public override byte[] GetBytes(string s)
		{
			return null;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007949 File Offset: 0x00006949
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			return 0;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000794C File Offset: 0x0000694C
		public override int GetMaxByteCount(int charCount)
		{
			return 0;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000794F File Offset: 0x0000694F
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return this.ucs4Decoder.GetCharCount(bytes, index, count);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000795F File Offset: 0x0000695F
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			return this.ucs4Decoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007973 File Offset: 0x00006973
		public override int GetMaxCharCount(int byteCount)
		{
			return (byteCount + 3) / 4;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000797A File Offset: 0x0000697A
		public override int CodePage
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000797D File Offset: 0x0000697D
		public override int GetCharCount(byte[] bytes)
		{
			return bytes.Length / 4;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007984 File Offset: 0x00006984
		public override Encoder GetEncoder()
		{
			return null;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007987 File Offset: 0x00006987
		internal static Encoding UCS4_Littleendian
		{
			get
			{
				return new Ucs4Encoding4321();
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000798E File Offset: 0x0000698E
		internal static Encoding UCS4_Bigendian
		{
			get
			{
				return new Ucs4Encoding1234();
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00007995 File Offset: 0x00006995
		internal static Encoding UCS4_2143
		{
			get
			{
				return new Ucs4Encoding2143();
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000799C File Offset: 0x0000699C
		internal static Encoding UCS4_3412
		{
			get
			{
				return new Ucs4Encoding3412();
			}
		}

		// Token: 0x040004BE RID: 1214
		internal Ucs4Decoder ucs4Decoder;
	}
}
