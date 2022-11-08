using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000A2 RID: 162
	[ComVisible(true)]
	[Serializable]
	public sealed class DBNull : ISerializable, IConvertible
	{
		// Token: 0x06000968 RID: 2408 RVA: 0x0001CBE4 File Offset: 0x0001BBE4
		private DBNull()
		{
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001CBEC File Offset: 0x0001BBEC
		private DBNull(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DBNullSerial"));
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001CC03 File Offset: 0x0001BC03
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			UnitySerializationHolder.GetUnitySerializationInfo(info, 2, null, null);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001CC0E File Offset: 0x0001BC0E
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001CC15 File Offset: 0x0001BC15
		public string ToString(IFormatProvider provider)
		{
			return string.Empty;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001CC1C File Offset: 0x0001BC1C
		public TypeCode GetTypeCode()
		{
			return TypeCode.DBNull;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001CC1F File Offset: 0x0001BC1F
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0001CC30 File Offset: 0x0001BC30
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0001CC41 File Offset: 0x0001BC41
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001CC52 File Offset: 0x0001BC52
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001CC63 File Offset: 0x0001BC63
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001CC74 File Offset: 0x0001BC74
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0001CC85 File Offset: 0x0001BC85
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001CC96 File Offset: 0x0001BC96
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001CCA7 File Offset: 0x0001BCA7
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001CCB8 File Offset: 0x0001BCB8
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001CCC9 File Offset: 0x0001BCC9
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001CCDA File Offset: 0x0001BCDA
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001CCEB File Offset: 0x0001BCEB
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0001CCFC File Offset: 0x0001BCFC
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0001CD0D File Offset: 0x0001BD0D
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400039E RID: 926
		public static readonly DBNull Value = new DBNull();
	}
}
