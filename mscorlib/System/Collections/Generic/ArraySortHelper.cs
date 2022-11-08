using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x020002A7 RID: 679
	[TypeDependency("System.Collections.Generic.GenericArraySortHelper`1")]
	internal class ArraySortHelper<T> : IArraySortHelper<T>
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x000455C8 File Offset: 0x000445C8
		public static IArraySortHelper<T> Default
		{
			get
			{
				IArraySortHelper<T> arraySortHelper = ArraySortHelper<T>.defaultArraySortHelper;
				if (arraySortHelper == null)
				{
					arraySortHelper = ArraySortHelper<T>.CreateArraySortHelper();
				}
				return arraySortHelper;
			}
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000455E8 File Offset: 0x000445E8
		private static IArraySortHelper<T> CreateArraySortHelper()
		{
			if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
			{
				ArraySortHelper<T>.defaultArraySortHelper = (IArraySortHelper<T>)typeof(GenericArraySortHelper<string>).TypeHandle.Instantiate(new RuntimeTypeHandle[]
				{
					typeof(T).TypeHandle
				}).Allocate();
			}
			else
			{
				ArraySortHelper<T>.defaultArraySortHelper = new ArraySortHelper<T>();
			}
			return ArraySortHelper<T>.defaultArraySortHelper;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00045670 File Offset: 0x00044670
		public void Sort(T[] keys, int index, int length, IComparer<T> comparer)
		{
			try
			{
				if (comparer == null)
				{
					comparer = Comparer<T>.Default;
				}
				ArraySortHelper<T>.QuickSort(keys, index, index + (length - 1), comparer);
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_BogusIComparer", new object[]
				{
					null,
					typeof(T).Name,
					comparer
				}));
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), innerException);
			}
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x000456F8 File Offset: 0x000446F8
		public int BinarySearch(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			int result;
			try
			{
				if (comparer == null)
				{
					comparer = Comparer<T>.Default;
				}
				result = ArraySortHelper<T>.InternalBinarySearch(array, index, length, value, comparer);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_IComparerFailed"), innerException);
			}
			return result;
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00045744 File Offset: 0x00044744
		internal static int InternalBinarySearch(T[] array, int index, int length, T value, IComparer<T> comparer)
		{
			int i = index;
			int num = index + length - 1;
			while (i <= num)
			{
				int num2 = i + (num - i >> 1);
				int num3 = comparer.Compare(array[num2], value);
				if (num3 == 0)
				{
					return num2;
				}
				if (num3 < 0)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return ~i;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x0004578C File Offset: 0x0004478C
		private static void SwapIfGreaterWithItems(T[] keys, IComparer<T> comparer, int a, int b)
		{
			if (a != b && comparer.Compare(keys[a], keys[b]) > 0)
			{
				T t = keys[a];
				keys[a] = keys[b];
				keys[b] = t;
			}
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000457D4 File Offset: 0x000447D4
		internal static void QuickSort(T[] keys, int left, int right, IComparer<T> comparer)
		{
			do
			{
				int num = left;
				int num2 = right;
				int num3 = num + (num2 - num >> 1);
				ArraySortHelper<T>.SwapIfGreaterWithItems(keys, comparer, num, num3);
				ArraySortHelper<T>.SwapIfGreaterWithItems(keys, comparer, num, num2);
				ArraySortHelper<T>.SwapIfGreaterWithItems(keys, comparer, num3, num2);
				T t = keys[num3];
				for (;;)
				{
					if (comparer.Compare(keys[num], t) >= 0)
					{
						while (comparer.Compare(t, keys[num2]) < 0)
						{
							num2--;
						}
						if (num > num2)
						{
							break;
						}
						if (num < num2)
						{
							T t2 = keys[num];
							keys[num] = keys[num2];
							keys[num2] = t2;
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
						ArraySortHelper<T>.QuickSort(keys, left, num2, comparer);
					}
					left = num;
				}
				else
				{
					if (num < right)
					{
						ArraySortHelper<T>.QuickSort(keys, num, right, comparer);
					}
					right = num2;
				}
			}
			while (left < right);
		}

		// Token: 0x04000A3E RID: 2622
		private static IArraySortHelper<T> defaultArraySortHelper;
	}
}
