using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Win32
{
	// Token: 0x02000471 RID: 1137
	internal sealed class OAVariantLib
	{
		// Token: 0x06002D48 RID: 11592 RVA: 0x00097026 File Offset: 0x00096026
		private OAVariantLib()
		{
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x00097030 File Offset: 0x00096030
		internal static Variant ChangeType(Variant source, Type targetClass, short options, CultureInfo culture)
		{
			if (targetClass == null)
			{
				throw new ArgumentNullException("targetClass");
			}
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Variant result = default(Variant);
			OAVariantLib.ChangeTypeEx(ref result, source, culture.LCID, OAVariantLib.GetCVTypeFromClass(targetClass), options);
			return result;
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x00097078 File Offset: 0x00096078
		private static int GetCVTypeFromClass(Type ctype)
		{
			int num = -1;
			for (int i = 0; i < OAVariantLib.ClassTypes.Length; i++)
			{
				if (ctype.Equals(OAVariantLib.ClassTypes[i]))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 18;
			}
			return num;
		}

		// Token: 0x06002D4B RID: 11595
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ChangeTypeEx(ref Variant result, Variant source, int lcid, int cvType, short flags);

		// Token: 0x0400175E RID: 5982
		public const int NoValueProp = 1;

		// Token: 0x0400175F RID: 5983
		public const int AlphaBool = 2;

		// Token: 0x04001760 RID: 5984
		public const int NoUserOverride = 4;

		// Token: 0x04001761 RID: 5985
		public const int CalendarHijri = 8;

		// Token: 0x04001762 RID: 5986
		public const int LocalBool = 16;

		// Token: 0x04001763 RID: 5987
		private const int CV_OBJECT = 18;

		// Token: 0x04001764 RID: 5988
		internal static readonly Type[] ClassTypes = new Type[]
		{
			typeof(Empty),
			typeof(void),
			typeof(bool),
			typeof(char),
			typeof(sbyte),
			typeof(byte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(string),
			typeof(void),
			typeof(DateTime),
			typeof(TimeSpan),
			typeof(object),
			typeof(decimal),
			null,
			typeof(Missing),
			typeof(DBNull)
		};
	}
}
