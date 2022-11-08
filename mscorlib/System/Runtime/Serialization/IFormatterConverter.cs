using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200035D RID: 861
	[ComVisible(true)]
	[CLSCompliant(false)]
	public interface IFormatterConverter
	{
		// Token: 0x060021CE RID: 8654
		object Convert(object value, Type type);

		// Token: 0x060021CF RID: 8655
		object Convert(object value, TypeCode typeCode);

		// Token: 0x060021D0 RID: 8656
		bool ToBoolean(object value);

		// Token: 0x060021D1 RID: 8657
		char ToChar(object value);

		// Token: 0x060021D2 RID: 8658
		sbyte ToSByte(object value);

		// Token: 0x060021D3 RID: 8659
		byte ToByte(object value);

		// Token: 0x060021D4 RID: 8660
		short ToInt16(object value);

		// Token: 0x060021D5 RID: 8661
		ushort ToUInt16(object value);

		// Token: 0x060021D6 RID: 8662
		int ToInt32(object value);

		// Token: 0x060021D7 RID: 8663
		uint ToUInt32(object value);

		// Token: 0x060021D8 RID: 8664
		long ToInt64(object value);

		// Token: 0x060021D9 RID: 8665
		ulong ToUInt64(object value);

		// Token: 0x060021DA RID: 8666
		float ToSingle(object value);

		// Token: 0x060021DB RID: 8667
		double ToDouble(object value);

		// Token: 0x060021DC RID: 8668
		decimal ToDecimal(object value);

		// Token: 0x060021DD RID: 8669
		DateTime ToDateTime(object value);

		// Token: 0x060021DE RID: 8670
		string ToString(object value);
	}
}
