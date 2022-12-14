using System;

namespace System.Xml
{
	// Token: 0x0200003E RID: 62
	internal class Ucs4Decoder3412 : Ucs4Decoder
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00007EF0 File Offset: 0x00006EF0
		internal override int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			byteCount += byteIndex;
			int num = byteIndex;
			int num2 = charIndex;
			while (num + 3 < byteCount)
			{
				uint num3 = (uint)((int)bytes[num + 2] << 24 | (int)bytes[num + 3] << 16 | (int)bytes[num] << 8 | (int)bytes[num + 1]);
				if (num3 > 1114111U)
				{
					throw new ArgumentException(Res.GetString("Enc_InvalidByteInEncoding", new object[]
					{
						num
					}), null);
				}
				if (num3 > 65535U)
				{
					chars[num2] = base.UnicodeToUTF16(num3);
					num2++;
				}
				else
				{
					if (num3 >= 55296U && num3 <= 57343U)
					{
						throw new XmlException("Xml_InvalidCharInThisEncoding", string.Empty);
					}
					chars[num2] = (char)num3;
				}
				num2++;
				num += 4;
			}
			return num2 - charIndex;
		}
	}
}
