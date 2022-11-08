using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace System.Xml
{
	// Token: 0x0200002A RID: 42
	internal struct XmlCharType
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004D60 File Offset: 0x00003D60
		private static object StaticLock
		{
			get
			{
				if (XmlCharType.s_Lock == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref XmlCharType.s_Lock, value, null);
				}
				return XmlCharType.s_Lock;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004D8C File Offset: 0x00003D8C
		private unsafe static void InitInstance()
		{
			lock (XmlCharType.StaticLock)
			{
				if (XmlCharType.s_CharProperties == null)
				{
					UnmanagedMemoryStream unmanagedMemoryStream = (UnmanagedMemoryStream)Assembly.GetExecutingAssembly().GetManifestResourceStream("XmlCharType.bin");
					byte* positionPointer = unmanagedMemoryStream.PositionPointer;
					Thread.MemoryBarrier();
					XmlCharType.s_CharProperties = positionPointer;
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004DF0 File Offset: 0x00003DF0
		private unsafe XmlCharType(byte* charProperties)
		{
			this.charProperties = charProperties;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004DF9 File Offset: 0x00003DF9
		internal static XmlCharType Instance
		{
			get
			{
				if (XmlCharType.s_CharProperties == null)
				{
					XmlCharType.InitInstance();
				}
				return new XmlCharType(XmlCharType.s_CharProperties);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004E13 File Offset: 0x00003E13
		public unsafe bool IsWhiteSpace(char ch)
		{
			return (this.charProperties[ch] & 1) != 0;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004E26 File Offset: 0x00003E26
		public unsafe bool IsLetter(char ch)
		{
			return (this.charProperties[ch] & 2) != 0;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004E39 File Offset: 0x00003E39
		public bool IsExtender(char ch)
		{
			return ch == '·';
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004E43 File Offset: 0x00003E43
		public unsafe bool IsNCNameChar(char ch)
		{
			return (this.charProperties[ch] & 8) != 0;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004E56 File Offset: 0x00003E56
		public unsafe bool IsStartNCNameChar(char ch)
		{
			return (this.charProperties[ch] & 4) != 0;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004E69 File Offset: 0x00003E69
		public unsafe bool IsCharData(char ch)
		{
			return (this.charProperties[ch] & 16) != 0;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004E7D File Offset: 0x00003E7D
		public unsafe bool IsPubidChar(char ch)
		{
			return (this.charProperties[ch] & 32) != 0;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004E91 File Offset: 0x00003E91
		internal unsafe bool IsTextChar(char ch)
		{
			return (this.charProperties[ch] & 64) != 0;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004EA5 File Offset: 0x00003EA5
		internal unsafe bool IsAttributeValueChar(char ch)
		{
			return (this.charProperties[ch] & 128) != 0;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004EBC File Offset: 0x00003EBC
		public bool IsNameChar(char ch)
		{
			return this.IsNCNameChar(ch) || ch == ':';
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004ECE File Offset: 0x00003ECE
		public bool IsStartNameChar(char ch)
		{
			return this.IsStartNCNameChar(ch) || ch == ':';
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004EE0 File Offset: 0x00003EE0
		public bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004EF1 File Offset: 0x00003EF1
		public bool IsHexDigit(char ch)
		{
			return (ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004F18 File Offset: 0x00003F18
		internal bool IsOnlyWhitespace(string str)
		{
			return this.IsOnlyWhitespaceWithPos(str) == -1;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004F24 File Offset: 0x00003F24
		internal unsafe int IsOnlyWhitespaceWithPos(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if ((this.charProperties[str[i]] & 1) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004F5C File Offset: 0x00003F5C
		internal bool IsName(string str)
		{
			if (str.Length == 0 || !this.IsStartNameChar(str[0]))
			{
				return false;
			}
			for (int i = 1; i < str.Length; i++)
			{
				if (!this.IsNameChar(str[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004FA8 File Offset: 0x00003FA8
		internal unsafe bool IsNmToken(string str)
		{
			if (str.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < str.Length; i++)
			{
				if ((this.charProperties[str[i]] & 8) == 0 && str[i] != ':')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004FF4 File Offset: 0x00003FF4
		internal unsafe int IsOnlyCharData(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if ((this.charProperties[str[i]] & 16) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000502C File Offset: 0x0000402C
		internal unsafe int IsPublicId(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if ((this.charProperties[str[i]] & 32) == 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x04000495 RID: 1173
		internal const int fWhitespace = 1;

		// Token: 0x04000496 RID: 1174
		internal const int fLetter = 2;

		// Token: 0x04000497 RID: 1175
		internal const int fNCStartName = 4;

		// Token: 0x04000498 RID: 1176
		internal const int fNCName = 8;

		// Token: 0x04000499 RID: 1177
		internal const int fCharData = 16;

		// Token: 0x0400049A RID: 1178
		internal const int fPublicId = 32;

		// Token: 0x0400049B RID: 1179
		internal const int fText = 64;

		// Token: 0x0400049C RID: 1180
		internal const int fAttrValue = 128;

		// Token: 0x0400049D RID: 1181
		private const uint CharPropertiesSize = 65536U;

		// Token: 0x0400049E RID: 1182
		private static object s_Lock;

		// Token: 0x0400049F RID: 1183
		private unsafe static byte* s_CharProperties;

		// Token: 0x040004A0 RID: 1184
		internal unsafe byte* charProperties;
	}
}
