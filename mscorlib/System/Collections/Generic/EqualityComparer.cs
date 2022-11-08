using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000293 RID: 659
	[TypeDependency("System.Collections.Generic.GenericEqualityComparer`1")]
	[Serializable]
	public abstract class EqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x00043C90 File Offset: 0x00042C90
		public static EqualityComparer<T> Default
		{
			get
			{
				EqualityComparer<T> equalityComparer = EqualityComparer<T>.defaultComparer;
				if (equalityComparer == null)
				{
					equalityComparer = EqualityComparer<T>.CreateComparer();
					EqualityComparer<T>.defaultComparer = equalityComparer;
				}
				return equalityComparer;
			}
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00043CB4 File Offset: 0x00042CB4
		private static EqualityComparer<T> CreateComparer()
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(byte))
			{
				return (EqualityComparer<T>)new ByteEqualityComparer();
			}
			if (typeof(IEquatable<T>).IsAssignableFrom(typeFromHandle))
			{
				return (EqualityComparer<T>)typeof(GenericEqualityComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(typeFromHandle);
			}
			if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				Type type = typeFromHandle.GetGenericArguments()[0];
				if (typeof(IEquatable<>).MakeGenericType(new Type[]
				{
					type
				}).IsAssignableFrom(type))
				{
					return (EqualityComparer<T>)typeof(NullableEqualityComparer<int>).TypeHandle.CreateInstanceForAnotherGenericParameter(type);
				}
			}
			return new ObjectEqualityComparer<T>();
		}

		// Token: 0x060019FA RID: 6650
		public abstract bool Equals(T x, T y);

		// Token: 0x060019FB RID: 6651
		public abstract int GetHashCode(T obj);

		// Token: 0x060019FC RID: 6652 RVA: 0x00043D80 File Offset: 0x00042D80
		internal virtual int IndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (this.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00043DB4 File Offset: 0x00042DB4
		internal virtual int LastIndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			for (int i = startIndex; i >= num; i--)
			{
				if (this.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x00043DE7 File Offset: 0x00042DE7
		int IEqualityComparer.GetHashCode(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			if (obj is T)
			{
				return this.GetHashCode((T)((object)obj));
			}
			ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
			return 0;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x00043E0A File Offset: 0x00042E0A
		bool IEqualityComparer.Equals(object x, object y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			if (x is T && y is T)
			{
				return this.Equals((T)((object)x), (T)((object)y));
			}
			ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
			return false;
		}

		// Token: 0x04000A2C RID: 2604
		private static EqualityComparer<T> defaultComparer;
	}
}
