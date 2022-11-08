using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000285 RID: 645
	[ComVisible(true)]
	public static class Nullable
	{
		// Token: 0x06001970 RID: 6512 RVA: 0x0004228A File Offset: 0x0004128A
		[ComVisible(true)]
		public static int Compare<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				if (n2 != null)
				{
					return Comparer<T>.Default.Compare(n1.value, n2.value);
				}
				return 1;
			}
			else
			{
				if (n2 != null)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x000422C5 File Offset: 0x000412C5
		[ComVisible(true)]
		public static bool Equals<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				return n2 != null && EqualityComparer<T>.Default.Equals(n1.value, n2.value);
			}
			return n2 == null;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00042300 File Offset: 0x00041300
		public static Type GetUnderlyingType(Type nullableType)
		{
			if (nullableType == null)
			{
				throw new ArgumentNullException("nullableType");
			}
			Type result = null;
			if (nullableType.IsGenericType && !nullableType.IsGenericTypeDefinition)
			{
				Type genericTypeDefinition = nullableType.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(Nullable<>))
				{
					result = nullableType.GetGenericArguments()[0];
				}
			}
			return result;
		}
	}
}
