using System;

namespace System.Collections.Generic
{
	// Token: 0x020002AB RID: 683
	internal class GenericArraySortHelper<TKey, TValue> : IArraySortHelper<TKey, TValue> where TKey : IComparable<TKey>
	{
		// Token: 0x06001AAB RID: 6827 RVA: 0x00045E44 File Offset: 0x00044E44
		public void Sort(TKey[] keys, TValue[] values, int index, int length, IComparer<TKey> comparer)
		{
			try
			{
				if (comparer == null || comparer == Comparer<TKey>.Default)
				{
					GenericArraySortHelper<TKey, TValue>.QuickSort(keys, values, index, index + length - 1);
				}
				else
				{
					ArraySortHelper<TKey, TValue>.QuickSort(keys, values, index, index + length - 1, comparer);
				}
			}
			catch (IndexOutOfRangeException)
			{
				string key = "Arg_BogusIComparer";
				object[] array = new object[3];
				array[1] = typeof(TKey).Name;
				throw new ArgumentException(Environment.GetResourceString(key, array));
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), innerException);
			}
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00045EDC File Offset: 0x00044EDC
		private static void SwapIfGreaterWithItems(TKey[] keys, TValue[] values, int a, int b)
		{
			if (a != b && keys[a] != null && keys[a].CompareTo(keys[b]) > 0)
			{
				TKey tkey = keys[a];
				keys[a] = keys[b];
				keys[b] = tkey;
				if (values != null)
				{
					TValue tvalue = values[a];
					values[a] = values[b];
					values[b] = tvalue;
				}
			}
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00045F58 File Offset: 0x00044F58
		private static void QuickSort(TKey[] keys, TValue[] values, int left, int right)
		{
			do
			{
				int num = left;
				int num2 = right;
				int num3 = num + (num2 - num >> 1);
				GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, num, num3);
				GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, num, num2);
				GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, num3, num2);
				TKey tkey = keys[num3];
				do
				{
					if (tkey == null)
					{
						while (keys[num2] != null)
						{
							num2--;
						}
					}
					else
					{
						while (tkey.CompareTo(keys[num]) > 0)
						{
							num++;
						}
						while (tkey.CompareTo(keys[num2]) < 0)
						{
							num2--;
						}
					}
					if (num > num2)
					{
						break;
					}
					if (num < num2)
					{
						TKey tkey2 = keys[num];
						keys[num] = keys[num2];
						keys[num2] = tkey2;
						if (values != null)
						{
							TValue tvalue = values[num];
							values[num] = values[num2];
							values[num2] = tvalue;
						}
					}
					num++;
					num2--;
				}
				while (num <= num2);
				if (num2 - left <= right - num)
				{
					if (left < num2)
					{
						GenericArraySortHelper<TKey, TValue>.QuickSort(keys, values, left, num2);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						GenericArraySortHelper<TKey, TValue>.QuickSort(keys, values, num, right);
					}
					right = num2;
				}
			}
			while (left < right);
		}
	}
}
