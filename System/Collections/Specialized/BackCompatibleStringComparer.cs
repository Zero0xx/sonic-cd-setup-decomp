using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Specialized
{
	// Token: 0x020007A1 RID: 1953
	internal class BackCompatibleStringComparer : IEqualityComparer
	{
		// Token: 0x06003C10 RID: 15376 RVA: 0x00100DDF File Offset: 0x000FFDDF
		internal BackCompatibleStringComparer()
		{
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x00100DE8 File Offset: 0x000FFDE8
		public unsafe static int GetHashCode(string obj)
		{
			IntPtr intPtr2;
			IntPtr intPtr = intPtr2 = obj;
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			int num = 5381;
			char* ptr2 = ptr;
			int num2;
			while ((num2 = (int)(*ptr2)) != 0)
			{
				num = ((num << 5) + num ^ num2);
				ptr2++;
			}
			return num;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x00100E29 File Offset: 0x000FFE29
		bool IEqualityComparer.Equals(object a, object b)
		{
			return object.Equals(a, b);
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x00100E34 File Offset: 0x000FFE34
		public virtual int GetHashCode(object o)
		{
			string text = o as string;
			if (text == null)
			{
				return o.GetHashCode();
			}
			return BackCompatibleStringComparer.GetHashCode(text);
		}

		// Token: 0x04003503 RID: 13571
		internal static IEqualityComparer Default = new BackCompatibleStringComparer();
	}
}
