using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000286 RID: 646
	[TypeDependency("System.Collections.Generic.GenericComparer`1")]
	[Serializable]
	public abstract class Comparer<T> : IComparer, IComparer<T>
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x0004234C File Offset: 0x0004134C
		public static Comparer<T> Default
		{
			get
			{
				Comparer<T> comparer = Comparer<T>.defaultComparer;
				if (comparer == null)
				{
					comparer = Comparer<T>.CreateComparer();
					Comparer<T>.defaultComparer = comparer;
				}
				return comparer;
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00042370 File Offset: 0x00041370
		private static Comparer<T> CreateComparer()
		{
			Type typeFromHandle = typeof(T);
			if (typeof(IComparable<T>).IsAssignableFrom(typeFromHandle))
			{
				return (Comparer<T>)typeof(GenericComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(typeFromHandle);
			}
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				Type type = typeFromHandle.GetGenericArguments()[0];
				if (typeof(IComparable<>).MakeGenericType(new Type[]
				{
					type
				}).IsAssignableFrom(type))
				{
					return (Comparer<T>)typeof(NullableComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(type);
				}
			}
			return new ObjectComparer<T>();
		}

		// Token: 0x06001975 RID: 6517
		public abstract int Compare(T x, T y);

		// Token: 0x06001976 RID: 6518 RVA: 0x00042422 File Offset: 0x00041422
		int IComparer.Compare(object x, object y)
		{
			if (x == null)
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (y == null)
				{
					return 1;
				}
				if (x is T && y is T)
				{
					return this.Compare((T)((object)x), (T)((object)y));
				}
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
				return 0;
			}
		}

		// Token: 0x04000A07 RID: 2567
		private static Comparer<T> defaultComparer;
	}
}
