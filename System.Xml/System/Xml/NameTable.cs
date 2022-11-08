using System;

namespace System.Xml
{
	// Token: 0x02000025 RID: 37
	public class NameTable : XmlNameTable
	{
		// Token: 0x060000BA RID: 186 RVA: 0x0000435E File Offset: 0x0000335E
		public NameTable()
		{
			this.mask = 31;
			this.entries = new NameTable.Entry[this.mask + 1];
			this.marvinHashSeed = MarvinHash.DefaultSeed;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000438C File Offset: 0x0000338C
		public override string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (key.Length == 0)
			{
				return string.Empty;
			}
			int num = this.ComputeHash32(key);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && entry.str.Equals(key))
				{
					return entry.str;
				}
			}
			return this.AddEntry(key, num);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004404 File Offset: 0x00003404
		public override string Add(char[] key, int start, int len)
		{
			if (len == 0)
			{
				return string.Empty;
			}
			if (start >= key.Length || start < 0 || (long)start + (long)len > (long)key.Length)
			{
				throw new IndexOutOfRangeException();
			}
			if (len < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			int num = this.ComputeHash32(key, start, len);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && NameTable.TextEquals(entry.str, key, start, len))
				{
					return entry.str;
				}
			}
			return this.AddEntry(new string(key, start, len), num);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004494 File Offset: 0x00003494
		public override string Get(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length == 0)
			{
				return string.Empty;
			}
			int num = this.ComputeHash32(value);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && entry.str.Equals(value))
				{
					return entry.str;
				}
			}
			return null;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004500 File Offset: 0x00003500
		public override string Get(char[] key, int start, int len)
		{
			if (len == 0)
			{
				return string.Empty;
			}
			if (start >= key.Length || start < 0 || (long)start + (long)len > (long)key.Length)
			{
				throw new IndexOutOfRangeException();
			}
			if (len < 0)
			{
				return null;
			}
			int num = this.ComputeHash32(key, start, len);
			for (NameTable.Entry entry = this.entries[num & this.mask]; entry != null; entry = entry.next)
			{
				if (entry.hashCode == num && NameTable.TextEquals(entry.str, key, start, len))
				{
					return entry.str;
				}
			}
			return null;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004580 File Offset: 0x00003580
		private string AddEntry(string str, int hashCode)
		{
			int num = hashCode & this.mask;
			NameTable.Entry entry = new NameTable.Entry(str, hashCode, this.entries[num]);
			this.entries[num] = entry;
			if (this.count++ == this.mask)
			{
				this.Grow();
			}
			return entry.str;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000045D4 File Offset: 0x000035D4
		private void Grow()
		{
			int num = this.mask * 2 + 1;
			NameTable.Entry[] array = this.entries;
			NameTable.Entry[] array2 = new NameTable.Entry[num + 1];
			foreach (NameTable.Entry entry in array)
			{
				while (entry != null)
				{
					int num2 = entry.hashCode & num;
					NameTable.Entry next = entry.next;
					entry.next = array2[num2];
					array2[num2] = entry;
					entry = next;
				}
			}
			this.entries = array2;
			this.mask = num;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000464C File Offset: 0x0000364C
		private static bool TextEquals(string str1, char[] str2, int str2Start, int str2Length)
		{
			if (str1.Length != str2Length)
			{
				return false;
			}
			for (int i = 0; i < str1.Length; i++)
			{
				if (str1[i] != str2[str2Start + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004686 File Offset: 0x00003686
		private int ComputeHash32(string key)
		{
			return MarvinHash.ComputeHash32(key, this.marvinHashSeed);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004694 File Offset: 0x00003694
		private int ComputeHash32(char[] key, int start, int len)
		{
			return MarvinHash.ComputeHash32(key, start, len, this.marvinHashSeed);
		}

		// Token: 0x04000486 RID: 1158
		private NameTable.Entry[] entries;

		// Token: 0x04000487 RID: 1159
		private int count;

		// Token: 0x04000488 RID: 1160
		private int mask;

		// Token: 0x04000489 RID: 1161
		private int hashCodeRandomizer;

		// Token: 0x0400048A RID: 1162
		private ulong marvinHashSeed;

		// Token: 0x02000026 RID: 38
		private class Entry
		{
			// Token: 0x060000C4 RID: 196 RVA: 0x000046A4 File Offset: 0x000036A4
			internal Entry(string str, int hashCode, NameTable.Entry next)
			{
				this.str = str;
				this.hashCode = hashCode;
				this.next = next;
			}

			// Token: 0x0400048B RID: 1163
			internal string str;

			// Token: 0x0400048C RID: 1164
			internal int hashCode;

			// Token: 0x0400048D RID: 1165
			internal NameTable.Entry next;
		}
	}
}
