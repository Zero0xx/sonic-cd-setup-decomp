using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000BF RID: 191
	[ComVisible(true)]
	[Serializable]
	public struct Guid : IFormattable, IComparable, IComparable<Guid>, IEquatable<Guid>
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x000211C8 File Offset: 0x000201C8
		public Guid(byte[] b)
		{
			if (b == null)
			{
				throw new ArgumentNullException("b");
			}
			if (b.Length != 16)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_GuidArrayCtor"), new object[]
				{
					"16"
				}));
			}
			this._a = ((int)b[3] << 24 | (int)b[2] << 16 | (int)b[1] << 8 | (int)b[0]);
			this._b = (short)((int)b[5] << 8 | (int)b[4]);
			this._c = (short)((int)b[7] << 8 | (int)b[6]);
			this._d = b[8];
			this._e = b[9];
			this._f = b[10];
			this._g = b[11];
			this._h = b[12];
			this._i = b[13];
			this._j = b[14];
			this._k = b[15];
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000212A0 File Offset: 0x000202A0
		[CLSCompliant(false)]
		public Guid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
		{
			this._a = (int)a;
			this._b = (short)b;
			this._c = (short)c;
			this._d = d;
			this._e = e;
			this._f = f;
			this._g = g;
			this._h = h;
			this._i = i;
			this._j = j;
			this._k = k;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00021304 File Offset: 0x00020304
		private Guid(bool blank)
		{
			this._a = 0;
			this._b = 0;
			this._c = 0;
			this._d = 0;
			this._e = 0;
			this._f = 0;
			this._g = 0;
			this._h = 0;
			this._i = 0;
			this._j = 0;
			this._k = 0;
			if (!blank)
			{
				this.CompleteGuid();
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00021368 File Offset: 0x00020368
		public Guid(string g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			int num = 0;
			int num2 = 0;
			try
			{
				if (g.IndexOf('-', 0) >= 0)
				{
					string text = g.Trim();
					if (text[0] == '{')
					{
						if (text.Length != 38 || text[37] != '}')
						{
							throw new FormatException(Environment.GetResourceString("Format_GuidInvLen"));
						}
						num = 1;
					}
					else if (text[0] == '(')
					{
						if (text.Length != 38 || text[37] != ')')
						{
							throw new FormatException(Environment.GetResourceString("Format_GuidInvLen"));
						}
						num = 1;
					}
					else if (text.Length != 36)
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidInvLen"));
					}
					if (text[8 + num] != '-' || text[13 + num] != '-' || text[18 + num] != '-' || text[23 + num] != '-')
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidDashes"));
					}
					num2 = num;
					this._a = Guid.TryParse(text, ref num2, 8);
					num2++;
					this._b = (short)Guid.TryParse(text, ref num2, 4);
					num2++;
					this._c = (short)Guid.TryParse(text, ref num2, 4);
					num2++;
					int num3 = Guid.TryParse(text, ref num2, 4);
					num2++;
					num = num2;
					long num4 = ParseNumbers.StringToLong(text, 16, 8192, ref num2);
					if (num2 - num != 12)
					{
						throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_GuidInvLen"), new object[0]));
					}
					this._d = (byte)(num3 >> 8);
					this._e = (byte)num3;
					num3 = (int)(num4 >> 32);
					this._f = (byte)(num3 >> 8);
					this._g = (byte)num3;
					num3 = (int)num4;
					this._h = (byte)(num3 >> 24);
					this._i = (byte)(num3 >> 16);
					this._j = (byte)(num3 >> 8);
					this._k = (byte)num3;
				}
				else if (g.IndexOf('{', 0) >= 0)
				{
					g = Guid.EatAllWhitespace(g);
					if (g[0] != '{')
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidBrace"));
					}
					if (!Guid.IsHexPrefix(g, 1))
					{
						throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_GuidHexPrefix"), new object[]
						{
							"{0xdddddddd, etc}"
						}));
					}
					int num5 = 3;
					int num6 = g.IndexOf(',', num5) - num5;
					if (num6 <= 0)
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidComma"));
					}
					this._a = ParseNumbers.StringToInt(g.Substring(num5, num6), 16, 4096);
					if (!Guid.IsHexPrefix(g, num5 + num6 + 1))
					{
						throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_GuidHexPrefix"), new object[]
						{
							"{0xdddddddd, 0xdddd, etc}"
						}));
					}
					num5 = num5 + num6 + 3;
					num6 = g.IndexOf(',', num5) - num5;
					if (num6 <= 0)
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidComma"));
					}
					this._b = (short)ParseNumbers.StringToInt(g.Substring(num5, num6), 16, 4096);
					if (!Guid.IsHexPrefix(g, num5 + num6 + 1))
					{
						throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_GuidHexPrefix"), new object[]
						{
							"{0xdddddddd, 0xdddd, 0xdddd, etc}"
						}));
					}
					num5 = num5 + num6 + 3;
					num6 = g.IndexOf(',', num5) - num5;
					if (num6 <= 0)
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidComma"));
					}
					this._c = (short)ParseNumbers.StringToInt(g.Substring(num5, num6), 16, 4096);
					if (g.Length <= num5 + num6 + 1 || g[num5 + num6 + 1] != '{')
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidBrace"));
					}
					num6++;
					byte[] array = new byte[8];
					for (int i = 0; i < 8; i++)
					{
						if (!Guid.IsHexPrefix(g, num5 + num6 + 1))
						{
							throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_GuidHexPrefix"), new object[]
							{
								"{... { ... 0xdd, ...}}"
							}));
						}
						num5 = num5 + num6 + 3;
						if (i < 7)
						{
							num6 = g.IndexOf(',', num5) - num5;
							if (num6 <= 0)
							{
								throw new FormatException(Environment.GetResourceString("Format_GuidComma"));
							}
						}
						else
						{
							num6 = g.IndexOf('}', num5) - num5;
							if (num6 <= 0)
							{
								throw new FormatException(Environment.GetResourceString("Format_GuidBraceAfterLastNumber"));
							}
						}
						uint num7 = (uint)Convert.ToInt32(g.Substring(num5, num6), 16);
						if (num7 > 255U)
						{
							throw new FormatException(Environment.GetResourceString("Overflow_Byte"));
						}
						array[i] = (byte)num7;
					}
					this._d = array[0];
					this._e = array[1];
					this._f = array[2];
					this._g = array[3];
					this._h = array[4];
					this._i = array[5];
					this._j = array[6];
					this._k = array[7];
					if (num5 + num6 + 1 >= g.Length || g[num5 + num6 + 1] != '}')
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidEndBrace"));
					}
					if (num5 + num6 + 1 != g.Length - 1)
					{
						throw new FormatException(Environment.GetResourceString("Format_ExtraJunkAtEnd"));
					}
				}
				else
				{
					string text2 = g.Trim();
					if (text2.Length != 32)
					{
						throw new FormatException(Environment.GetResourceString("Format_GuidInvLen"));
					}
					foreach (char c in text2)
					{
						if (c < '0' || c > '9')
						{
							char c2 = char.ToUpper(c, CultureInfo.InvariantCulture);
							if (c2 < 'A' || c2 > 'F')
							{
								throw new FormatException(Environment.GetResourceString("Format_GuidInvalidChar"));
							}
						}
					}
					this._a = ParseNumbers.StringToInt(text2.Substring(num, 8), 16, 4096);
					num += 8;
					this._b = (short)ParseNumbers.StringToInt(text2.Substring(num, 4), 16, 4096);
					num += 4;
					this._c = (short)ParseNumbers.StringToInt(text2.Substring(num, 4), 16, 4096);
					num += 4;
					int num3 = (int)((short)ParseNumbers.StringToInt(text2.Substring(num, 4), 16, 4096));
					num += 4;
					num2 = num;
					long num4 = ParseNumbers.StringToLong(text2, 16, num, ref num2);
					if (num2 - num != 12)
					{
						throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_GuidInvLen"), new object[0]));
					}
					this._d = (byte)(num3 >> 8);
					this._e = (byte)num3;
					num3 = (int)(num4 >> 32);
					this._f = (byte)(num3 >> 8);
					this._g = (byte)num3;
					num3 = (int)num4;
					this._h = (byte)(num3 >> 24);
					this._i = (byte)(num3 >> 16);
					this._j = (byte)(num3 >> 8);
					this._k = (byte)num3;
				}
			}
			catch (IndexOutOfRangeException)
			{
				throw new FormatException(Environment.GetResourceString("Format_GuidUnrecognized"));
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00021A84 File Offset: 0x00020A84
		public Guid(int a, short b, short c, byte[] d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (d.Length != 8)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_GuidArrayCtor"), new object[]
				{
					"8"
				}));
			}
			this._a = a;
			this._b = b;
			this._c = c;
			this._d = d[0];
			this._e = d[1];
			this._f = d[2];
			this._g = d[3];
			this._h = d[4];
			this._i = d[5];
			this._j = d[6];
			this._k = d[7];
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00021B38 File Offset: 0x00020B38
		public Guid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
		{
			this._a = a;
			this._b = b;
			this._c = c;
			this._d = d;
			this._e = e;
			this._f = f;
			this._g = g;
			this._h = h;
			this._i = i;
			this._j = j;
			this._k = k;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00021B9C File Offset: 0x00020B9C
		private static int TryParse(string str, ref int parsePos, int requiredLength)
		{
			int num = parsePos;
			int result = ParseNumbers.StringToInt(str, 16, 8192, ref parsePos);
			if (parsePos - num != requiredLength)
			{
				throw new FormatException(Environment.GetResourceString("Format_GuidInvalidChar"));
			}
			return result;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00021BD4 File Offset: 0x00020BD4
		private static string EatAllWhitespace(string str)
		{
			int length = 0;
			char[] array = new char[str.Length];
			foreach (char c in str)
			{
				if (!char.IsWhiteSpace(c))
				{
					array[length++] = c;
				}
			}
			return new string(array, 0, length);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00021C20 File Offset: 0x00020C20
		private static bool IsHexPrefix(string str, int i)
		{
			return str[i] == '0' && char.ToLower(str[i + 1], CultureInfo.InvariantCulture) == 'x';
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00021C48 File Offset: 0x00020C48
		public byte[] ToByteArray()
		{
			return new byte[]
			{
				(byte)this._a,
				(byte)(this._a >> 8),
				(byte)(this._a >> 16),
				(byte)(this._a >> 24),
				(byte)this._b,
				(byte)(this._b >> 8),
				(byte)this._c,
				(byte)(this._c >> 8),
				this._d,
				this._e,
				this._f,
				this._g,
				this._h,
				this._i,
				this._j,
				this._k
			};
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00021D09 File Offset: 0x00020D09
		public override string ToString()
		{
			return this.ToString("D", null);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00021D17 File Offset: 0x00020D17
		public override int GetHashCode()
		{
			return this._a ^ ((int)this._b << 16 | (int)((ushort)this._c)) ^ ((int)this._f << 24 | (int)this._k);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00021D44 File Offset: 0x00020D44
		public override bool Equals(object o)
		{
			if (o == null || !(o is Guid))
			{
				return false;
			}
			Guid guid = (Guid)o;
			return guid._a == this._a && guid._b == this._b && guid._c == this._c && guid._d == this._d && guid._e == this._e && guid._f == this._f && guid._g == this._g && guid._h == this._h && guid._i == this._i && guid._j == this._j && guid._k == this._k;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00021E24 File Offset: 0x00020E24
		public bool Equals(Guid g)
		{
			return g._a == this._a && g._b == this._b && g._c == this._c && g._d == this._d && g._e == this._e && g._f == this._f && g._g == this._g && g._h == this._h && g._i == this._i && g._j == this._j && g._k == this._k;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00021EED File Offset: 0x00020EED
		private int GetResult(uint me, uint them)
		{
			if (me < them)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00021EF8 File Offset: 0x00020EF8
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is Guid))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeGuid"));
			}
			Guid guid = (Guid)value;
			if (guid._a != this._a)
			{
				return this.GetResult((uint)this._a, (uint)guid._a);
			}
			if (guid._b != this._b)
			{
				return this.GetResult((uint)this._b, (uint)guid._b);
			}
			if (guid._c != this._c)
			{
				return this.GetResult((uint)this._c, (uint)guid._c);
			}
			if (guid._d != this._d)
			{
				return this.GetResult((uint)this._d, (uint)guid._d);
			}
			if (guid._e != this._e)
			{
				return this.GetResult((uint)this._e, (uint)guid._e);
			}
			if (guid._f != this._f)
			{
				return this.GetResult((uint)this._f, (uint)guid._f);
			}
			if (guid._g != this._g)
			{
				return this.GetResult((uint)this._g, (uint)guid._g);
			}
			if (guid._h != this._h)
			{
				return this.GetResult((uint)this._h, (uint)guid._h);
			}
			if (guid._i != this._i)
			{
				return this.GetResult((uint)this._i, (uint)guid._i);
			}
			if (guid._j != this._j)
			{
				return this.GetResult((uint)this._j, (uint)guid._j);
			}
			if (guid._k != this._k)
			{
				return this.GetResult((uint)this._k, (uint)guid._k);
			}
			return 0;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000220AC File Offset: 0x000210AC
		public int CompareTo(Guid value)
		{
			if (value._a != this._a)
			{
				return this.GetResult((uint)this._a, (uint)value._a);
			}
			if (value._b != this._b)
			{
				return this.GetResult((uint)this._b, (uint)value._b);
			}
			if (value._c != this._c)
			{
				return this.GetResult((uint)this._c, (uint)value._c);
			}
			if (value._d != this._d)
			{
				return this.GetResult((uint)this._d, (uint)value._d);
			}
			if (value._e != this._e)
			{
				return this.GetResult((uint)this._e, (uint)value._e);
			}
			if (value._f != this._f)
			{
				return this.GetResult((uint)this._f, (uint)value._f);
			}
			if (value._g != this._g)
			{
				return this.GetResult((uint)this._g, (uint)value._g);
			}
			if (value._h != this._h)
			{
				return this.GetResult((uint)this._h, (uint)value._h);
			}
			if (value._i != this._i)
			{
				return this.GetResult((uint)this._i, (uint)value._i);
			}
			if (value._j != this._j)
			{
				return this.GetResult((uint)this._j, (uint)value._j);
			}
			if (value._k != this._k)
			{
				return this.GetResult((uint)this._k, (uint)value._k);
			}
			return 0;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002223C File Offset: 0x0002123C
		public static bool operator ==(Guid a, Guid b)
		{
			return a._a == b._a && a._b == b._b && a._c == b._c && a._d == b._d && a._e == b._e && a._f == b._f && a._g == b._g && a._h == b._h && a._i == b._i && a._j == b._j && a._k == b._k;
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00022310 File Offset: 0x00021310
		public static bool operator !=(Guid a, Guid b)
		{
			return !(a == b);
		}

		// Token: 0x06000AF4 RID: 2804
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CompleteGuid();

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002231C File Offset: 0x0002131C
		public static Guid NewGuid()
		{
			return new Guid(false);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00022324 File Offset: 0x00021324
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002232E File Offset: 0x0002132E
		private static char HexToChar(int a)
		{
			a &= 15;
			return (char)((a > 9) ? (a - 10 + 97) : (a + 48));
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00022349 File Offset: 0x00021349
		private static int HexsToChars(char[] guidChars, int offset, int a, int b)
		{
			guidChars[offset++] = Guid.HexToChar(a >> 4);
			guidChars[offset++] = Guid.HexToChar(a);
			guidChars[offset++] = Guid.HexToChar(b >> 4);
			guidChars[offset++] = Guid.HexToChar(b);
			return offset;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00022388 File Offset: 0x00021388
		public string ToString(string format, IFormatProvider provider)
		{
			if (format == null || format.Length == 0)
			{
				format = "D";
			}
			int offset = 0;
			int length = 38;
			bool flag = true;
			if (format.Length != 1)
			{
				throw new FormatException(Environment.GetResourceString("Format_InvalidGuidFormatSpecification"));
			}
			char c = format[0];
			char[] array;
			if (c == 'D' || c == 'd')
			{
				array = new char[36];
				length = 36;
			}
			else if (c == 'N' || c == 'n')
			{
				array = new char[32];
				length = 32;
				flag = false;
			}
			else if (c == 'B' || c == 'b')
			{
				array = new char[38];
				array[offset++] = '{';
				array[37] = '}';
			}
			else
			{
				if (c != 'P' && c != 'p')
				{
					throw new FormatException(Environment.GetResourceString("Format_InvalidGuidFormatSpecification"));
				}
				array = new char[38];
				array[offset++] = '(';
				array[37] = ')';
			}
			offset = Guid.HexsToChars(array, offset, this._a >> 24, this._a >> 16);
			offset = Guid.HexsToChars(array, offset, this._a >> 8, this._a);
			if (flag)
			{
				array[offset++] = '-';
			}
			offset = Guid.HexsToChars(array, offset, this._b >> 8, (int)this._b);
			if (flag)
			{
				array[offset++] = '-';
			}
			offset = Guid.HexsToChars(array, offset, this._c >> 8, (int)this._c);
			if (flag)
			{
				array[offset++] = '-';
			}
			offset = Guid.HexsToChars(array, offset, (int)this._d, (int)this._e);
			if (flag)
			{
				array[offset++] = '-';
			}
			offset = Guid.HexsToChars(array, offset, (int)this._f, (int)this._g);
			offset = Guid.HexsToChars(array, offset, (int)this._h, (int)this._i);
			offset = Guid.HexsToChars(array, offset, (int)this._j, (int)this._k);
			return new string(array, 0, length);
		}

		// Token: 0x0400040A RID: 1034
		public static readonly Guid Empty = default(Guid);

		// Token: 0x0400040B RID: 1035
		private int _a;

		// Token: 0x0400040C RID: 1036
		private short _b;

		// Token: 0x0400040D RID: 1037
		private short _c;

		// Token: 0x0400040E RID: 1038
		private byte _d;

		// Token: 0x0400040F RID: 1039
		private byte _e;

		// Token: 0x04000410 RID: 1040
		private byte _f;

		// Token: 0x04000411 RID: 1041
		private byte _g;

		// Token: 0x04000412 RID: 1042
		private byte _h;

		// Token: 0x04000413 RID: 1043
		private byte _i;

		// Token: 0x04000414 RID: 1044
		private byte _j;

		// Token: 0x04000415 RID: 1045
		private byte _k;
	}
}
