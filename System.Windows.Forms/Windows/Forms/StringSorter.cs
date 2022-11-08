using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000636 RID: 1590
	internal sealed class StringSorter
	{
		// Token: 0x06005332 RID: 21298 RVA: 0x00130710 File Offset: 0x0012F710
		private StringSorter(CultureInfo culture, string[] keys, object[] items, int options)
		{
			if (keys == null)
			{
				if (items is string[])
				{
					keys = (string[])items;
					items = null;
				}
				else
				{
					keys = new string[items.Length];
					for (int i = 0; i < items.Length; i++)
					{
						object obj = items[i];
						if (obj != null)
						{
							keys[i] = obj.ToString();
						}
					}
				}
			}
			this.keys = keys;
			this.items = items;
			this.lcid = ((culture == null) ? SafeNativeMethods.GetThreadLocale() : culture.LCID);
			this.options = (options & 200711);
			this.descending = ((options & int.MinValue) != 0);
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x001307A9 File Offset: 0x0012F7A9
		internal static int ArrayLength(object[] array)
		{
			if (array == null)
			{
				return 0;
			}
			return array.Length;
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001307B3 File Offset: 0x0012F7B3
		public static int Compare(string s1, string s2)
		{
			return StringSorter.Compare(SafeNativeMethods.GetThreadLocale(), s1, s2, 0);
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x001307C2 File Offset: 0x0012F7C2
		public static int Compare(string s1, string s2, int options)
		{
			return StringSorter.Compare(SafeNativeMethods.GetThreadLocale(), s1, s2, options);
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x001307D1 File Offset: 0x0012F7D1
		public static int Compare(CultureInfo culture, string s1, string s2, int options)
		{
			return StringSorter.Compare(culture.LCID, s1, s2, options);
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x001307E1 File Offset: 0x0012F7E1
		private static int Compare(int lcid, string s1, string s2, int options)
		{
			if (s1 == null)
			{
				if (s2 != null)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (s2 == null)
				{
					return 1;
				}
				return string.Compare(s1, s2, false, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x00130800 File Offset: 0x0012F800
		private int CompareKeys(string s1, string s2)
		{
			int num = StringSorter.Compare(this.lcid, s1, s2, this.options);
			if (!this.descending)
			{
				return num;
			}
			return -num;
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x00130830 File Offset: 0x0012F830
		private void QuickSort(int left, int right)
		{
			do
			{
				int num = left;
				int num2 = right;
				string text = this.keys[num + num2 >> 1];
				for (;;)
				{
					if (this.CompareKeys(this.keys[num], text) >= 0)
					{
						while (this.CompareKeys(text, this.keys[num2]) < 0)
						{
							num2--;
						}
						if (num > num2)
						{
							break;
						}
						if (num < num2)
						{
							string text2 = this.keys[num];
							this.keys[num] = this.keys[num2];
							this.keys[num2] = text2;
							if (this.items != null)
							{
								object obj = this.items[num];
								this.items[num] = this.items[num2];
								this.items[num2] = obj;
							}
						}
						num++;
						num2--;
						if (num > num2)
						{
							break;
						}
					}
					else
					{
						num++;
					}
				}
				if (num2 - left <= right - num)
				{
					if (left < num2)
					{
						this.QuickSort(left, num2);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						this.QuickSort(num, right);
					}
					right = num2;
				}
			}
			while (left < right);
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x00130912 File Offset: 0x0012F912
		public static void Sort(object[] items)
		{
			StringSorter.Sort(null, null, items, 0, StringSorter.ArrayLength(items), 0);
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x00130924 File Offset: 0x0012F924
		public static void Sort(object[] items, int index, int count)
		{
			StringSorter.Sort(null, null, items, index, count, 0);
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x00130931 File Offset: 0x0012F931
		public static void Sort(string[] keys, object[] items)
		{
			StringSorter.Sort(null, keys, items, 0, StringSorter.ArrayLength(items), 0);
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x00130943 File Offset: 0x0012F943
		public static void Sort(string[] keys, object[] items, int index, int count)
		{
			StringSorter.Sort(null, keys, items, index, count, 0);
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x00130950 File Offset: 0x0012F950
		public static void Sort(object[] items, int options)
		{
			StringSorter.Sort(null, null, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x00130962 File Offset: 0x0012F962
		public static void Sort(object[] items, int index, int count, int options)
		{
			StringSorter.Sort(null, null, items, index, count, options);
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0013096F File Offset: 0x0012F96F
		public static void Sort(string[] keys, object[] items, int options)
		{
			StringSorter.Sort(null, keys, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x00130981 File Offset: 0x0012F981
		public static void Sort(string[] keys, object[] items, int index, int count, int options)
		{
			StringSorter.Sort(null, keys, items, index, count, options);
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x0013098F File Offset: 0x0012F98F
		public static void Sort(CultureInfo culture, object[] items, int options)
		{
			StringSorter.Sort(culture, null, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x001309A1 File Offset: 0x0012F9A1
		public static void Sort(CultureInfo culture, object[] items, int index, int count, int options)
		{
			StringSorter.Sort(culture, null, items, index, count, options);
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x001309AF File Offset: 0x0012F9AF
		public static void Sort(CultureInfo culture, string[] keys, object[] items, int options)
		{
			StringSorter.Sort(culture, keys, items, 0, StringSorter.ArrayLength(items), options);
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x001309C4 File Offset: 0x0012F9C4
		public static void Sort(CultureInfo culture, string[] keys, object[] items, int index, int count, int options)
		{
			if (items == null || (keys != null && keys.Length != items.Length))
			{
				throw new ArgumentException(SR.GetString("ArraysNotSameSize", new object[]
				{
					"keys",
					"items"
				}));
			}
			if (count > 1)
			{
				StringSorter stringSorter = new StringSorter(culture, keys, items, options);
				stringSorter.QuickSort(index, index + count - 1);
			}
		}

		// Token: 0x0400366C RID: 13932
		public const int IgnoreCase = 1;

		// Token: 0x0400366D RID: 13933
		public const int IgnoreKanaType = 65536;

		// Token: 0x0400366E RID: 13934
		public const int IgnoreNonSpace = 2;

		// Token: 0x0400366F RID: 13935
		public const int IgnoreSymbols = 4;

		// Token: 0x04003670 RID: 13936
		public const int IgnoreWidth = 131072;

		// Token: 0x04003671 RID: 13937
		public const int StringSort = 4096;

		// Token: 0x04003672 RID: 13938
		public const int Descending = -2147483648;

		// Token: 0x04003673 RID: 13939
		private const int CompareOptions = 200711;

		// Token: 0x04003674 RID: 13940
		private string[] keys;

		// Token: 0x04003675 RID: 13941
		private object[] items;

		// Token: 0x04003676 RID: 13942
		private int lcid;

		// Token: 0x04003677 RID: 13943
		private int options;

		// Token: 0x04003678 RID: 13944
		private bool descending;
	}
}
