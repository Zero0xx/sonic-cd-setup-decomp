using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;

namespace System
{
	// Token: 0x0200013A RID: 314
	[Serializable]
	internal struct Variant
	{
		// Token: 0x06001150 RID: 4432
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitVariant();

		// Token: 0x06001151 RID: 4433 RVA: 0x0002FF14 File Offset: 0x0002EF14
		static Variant()
		{
			Variant.InitVariant();
		}

		// Token: 0x06001152 RID: 4434
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern double GetR8FromVar();

		// Token: 0x06001153 RID: 4435
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetR4FromVar();

		// Token: 0x06001154 RID: 4436
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetFieldsR4(float val);

		// Token: 0x06001155 RID: 4437
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetFieldsR8(double val);

		// Token: 0x06001156 RID: 4438
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetFieldsObject(object val);

		// Token: 0x06001157 RID: 4439 RVA: 0x000300A4 File Offset: 0x0002F0A4
		internal long GetI8FromVar()
		{
			return (long)this.m_data2 << 32 | ((long)this.m_data1 & (long)((ulong)-1));
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x000300BB File Offset: 0x0002F0BB
		internal Variant(int flags, object or, int data1, int data2)
		{
			this.m_flags = flags;
			this.m_objref = or;
			this.m_data1 = data1;
			this.m_data2 = data2;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000300DA File Offset: 0x0002F0DA
		public Variant(bool val)
		{
			this.m_objref = null;
			this.m_flags = 2;
			this.m_data1 = (val ? 1 : 0);
			this.m_data2 = 0;
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x000300FE File Offset: 0x0002F0FE
		public Variant(sbyte val)
		{
			this.m_objref = null;
			this.m_flags = 4;
			this.m_data1 = (int)val;
			this.m_data2 = (int)((long)val >> 32);
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00030121 File Offset: 0x0002F121
		public Variant(byte val)
		{
			this.m_objref = null;
			this.m_flags = 5;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0003013F File Offset: 0x0002F13F
		public Variant(short val)
		{
			this.m_objref = null;
			this.m_flags = 6;
			this.m_data1 = (int)val;
			this.m_data2 = (int)((long)val >> 32);
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00030162 File Offset: 0x0002F162
		public Variant(ushort val)
		{
			this.m_objref = null;
			this.m_flags = 7;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00030180 File Offset: 0x0002F180
		public Variant(char val)
		{
			this.m_objref = null;
			this.m_flags = 3;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0003019E File Offset: 0x0002F19E
		public Variant(int val)
		{
			this.m_objref = null;
			this.m_flags = 8;
			this.m_data1 = val;
			this.m_data2 = val >> 31;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x000301BF File Offset: 0x0002F1BF
		public Variant(uint val)
		{
			this.m_objref = null;
			this.m_flags = 9;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x000301DE File Offset: 0x0002F1DE
		public Variant(long val)
		{
			this.m_objref = null;
			this.m_flags = 10;
			this.m_data1 = (int)val;
			this.m_data2 = (int)(val >> 32);
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00030202 File Offset: 0x0002F202
		public Variant(ulong val)
		{
			this.m_objref = null;
			this.m_flags = 11;
			this.m_data1 = (int)val;
			this.m_data2 = (int)(val >> 32);
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00030226 File Offset: 0x0002F226
		public Variant(float val)
		{
			this.m_objref = null;
			this.m_flags = 12;
			this.m_data1 = 0;
			this.m_data2 = 0;
			this.SetFieldsR4(val);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0003024C File Offset: 0x0002F24C
		public Variant(double val)
		{
			this.m_objref = null;
			this.m_flags = 13;
			this.m_data1 = 0;
			this.m_data2 = 0;
			this.SetFieldsR8(val);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00030274 File Offset: 0x0002F274
		public Variant(DateTime val)
		{
			this.m_objref = null;
			this.m_flags = 16;
			ulong ticks = (ulong)val.Ticks;
			this.m_data1 = (int)ticks;
			this.m_data2 = (int)(ticks >> 32);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000302AB File Offset: 0x0002F2AB
		public Variant(decimal val)
		{
			this.m_objref = val;
			this.m_flags = 19;
			this.m_data1 = 0;
			this.m_data2 = 0;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000302D0 File Offset: 0x0002F2D0
		public Variant(object obj)
		{
			this.m_data1 = 0;
			this.m_data2 = 0;
			VarEnum varEnum = VarEnum.VT_EMPTY;
			if (obj is DateTime)
			{
				this.m_objref = null;
				this.m_flags = 16;
				ulong ticks = (ulong)((DateTime)obj).Ticks;
				this.m_data1 = (int)ticks;
				this.m_data2 = (int)(ticks >> 32);
				return;
			}
			if (obj is string)
			{
				this.m_flags = 14;
				this.m_objref = obj;
				return;
			}
			if (obj == null)
			{
				this = Variant.Empty;
				return;
			}
			if (obj == System.DBNull.Value)
			{
				this = Variant.DBNull;
				return;
			}
			if (obj == Type.Missing)
			{
				this = Variant.Missing;
				return;
			}
			if (obj is Array)
			{
				this.m_flags = 65554;
				this.m_objref = obj;
				return;
			}
			this.m_flags = 0;
			this.m_objref = null;
			if (obj is UnknownWrapper)
			{
				varEnum = VarEnum.VT_UNKNOWN;
				obj = ((UnknownWrapper)obj).WrappedObject;
			}
			else if (obj is DispatchWrapper)
			{
				varEnum = VarEnum.VT_DISPATCH;
				obj = ((DispatchWrapper)obj).WrappedObject;
			}
			else if (obj is ErrorWrapper)
			{
				varEnum = VarEnum.VT_ERROR;
				obj = ((ErrorWrapper)obj).ErrorCode;
			}
			else if (obj is CurrencyWrapper)
			{
				varEnum = VarEnum.VT_CY;
				obj = ((CurrencyWrapper)obj).WrappedObject;
			}
			else if (obj is BStrWrapper)
			{
				varEnum = VarEnum.VT_BSTR;
				obj = ((BStrWrapper)obj).WrappedObject;
			}
			if (obj != null)
			{
				this.SetFieldsObject(obj);
			}
			if (varEnum != VarEnum.VT_EMPTY)
			{
				this.m_flags |= (int)((int)varEnum << 24);
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00030448 File Offset: 0x0002F448
		public unsafe Variant(void* voidPointer, Type pointerType)
		{
			if (pointerType == null)
			{
				throw new ArgumentNullException("pointerType");
			}
			if (!pointerType.IsPointer)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBePointer"), "pointerType");
			}
			this.m_objref = pointerType;
			this.m_flags = 15;
			this.m_data1 = voidPointer;
			this.m_data2 = 0;
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06001169 RID: 4457 RVA: 0x0003049E File Offset: 0x0002F49E
		internal int CVType
		{
			get
			{
				return this.m_flags & 65535;
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000304AC File Offset: 0x0002F4AC
		public object ToObject()
		{
			switch (this.CVType)
			{
			case 0:
				return null;
			case 2:
				return this.m_data1 != 0;
			case 3:
				return (char)this.m_data1;
			case 4:
				return (sbyte)this.m_data1;
			case 5:
				return (byte)this.m_data1;
			case 6:
				return (short)this.m_data1;
			case 7:
				return (ushort)this.m_data1;
			case 8:
				return this.m_data1;
			case 9:
				return (uint)this.m_data1;
			case 10:
				return this.GetI8FromVar();
			case 11:
				return (ulong)this.GetI8FromVar();
			case 12:
				return this.GetR4FromVar();
			case 13:
				return this.GetR8FromVar();
			case 16:
				return new DateTime(this.GetI8FromVar());
			case 17:
				return new TimeSpan(this.GetI8FromVar());
			case 21:
				return this.BoxEnum();
			case 22:
				return Type.Missing;
			case 23:
				return System.DBNull.Value;
			}
			return this.m_objref;
		}

		// Token: 0x0600116B RID: 4459
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object BoxEnum();

		// Token: 0x0600116C RID: 4460 RVA: 0x00030604 File Offset: 0x0002F604
		internal static void MarshalHelperConvertObjectToVariant(object o, ref Variant v)
		{
			IConvertible convertible = RemotingServices.IsTransparentProxy(o) ? null : (o as IConvertible);
			if (o == null)
			{
				v = Variant.Empty;
				return;
			}
			if (convertible == null)
			{
				v = new Variant(o);
				return;
			}
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			switch (convertible.GetTypeCode())
			{
			case TypeCode.Empty:
				v = Variant.Empty;
				return;
			case TypeCode.Object:
				v = new Variant(o);
				return;
			case TypeCode.DBNull:
				v = Variant.DBNull;
				return;
			case TypeCode.Boolean:
				v = new Variant(convertible.ToBoolean(invariantCulture));
				return;
			case TypeCode.Char:
				v = new Variant(convertible.ToChar(invariantCulture));
				return;
			case TypeCode.SByte:
				v = new Variant(convertible.ToSByte(invariantCulture));
				return;
			case TypeCode.Byte:
				v = new Variant(convertible.ToByte(invariantCulture));
				return;
			case TypeCode.Int16:
				v = new Variant(convertible.ToInt16(invariantCulture));
				return;
			case TypeCode.UInt16:
				v = new Variant(convertible.ToUInt16(invariantCulture));
				return;
			case TypeCode.Int32:
				v = new Variant(convertible.ToInt32(invariantCulture));
				return;
			case TypeCode.UInt32:
				v = new Variant(convertible.ToUInt32(invariantCulture));
				return;
			case TypeCode.Int64:
				v = new Variant(convertible.ToInt64(invariantCulture));
				return;
			case TypeCode.UInt64:
				v = new Variant(convertible.ToUInt64(invariantCulture));
				return;
			case TypeCode.Single:
				v = new Variant(convertible.ToSingle(invariantCulture));
				return;
			case TypeCode.Double:
				v = new Variant(convertible.ToDouble(invariantCulture));
				return;
			case TypeCode.Decimal:
				v = new Variant(convertible.ToDecimal(invariantCulture));
				return;
			case TypeCode.DateTime:
				v = new Variant(convertible.ToDateTime(invariantCulture));
				return;
			case TypeCode.String:
				v = new Variant(convertible.ToString(invariantCulture));
				return;
			}
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("NotSupported_UnknownTypeCode"), new object[]
			{
				convertible.GetTypeCode()
			}));
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00030817 File Offset: 0x0002F817
		internal static object MarshalHelperConvertVariantToObject(ref Variant v)
		{
			return v.ToObject();
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00030820 File Offset: 0x0002F820
		internal static void MarshalHelperCastVariant(object pValue, int vt, ref Variant v)
		{
			IConvertible convertible = pValue as IConvertible;
			if (convertible == null)
			{
				switch (vt)
				{
				case 8:
					if (pValue == null)
					{
						v = new Variant(null);
						v.m_flags = 14;
						return;
					}
					throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_CannotCoerceByRefVariant"), new object[0]));
				case 9:
					v = new Variant(new DispatchWrapper(pValue));
					return;
				case 10:
				case 11:
					break;
				case 12:
					v = new Variant(pValue);
					return;
				case 13:
					v = new Variant(new UnknownWrapper(pValue));
					return;
				default:
					if (vt == 36)
					{
						v = new Variant(pValue);
						return;
					}
					break;
				}
				throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_CannotCoerceByRefVariant"), new object[0]));
			}
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			switch (vt)
			{
			case 0:
				v = Variant.Empty;
				return;
			case 1:
				v = Variant.DBNull;
				return;
			case 2:
				v = new Variant(convertible.ToInt16(invariantCulture));
				return;
			case 3:
				v = new Variant(convertible.ToInt32(invariantCulture));
				return;
			case 4:
				v = new Variant(convertible.ToSingle(invariantCulture));
				return;
			case 5:
				v = new Variant(convertible.ToDouble(invariantCulture));
				return;
			case 6:
				v = new Variant(new CurrencyWrapper(convertible.ToDecimal(invariantCulture)));
				return;
			case 7:
				v = new Variant(convertible.ToDateTime(invariantCulture));
				return;
			case 8:
				v = new Variant(convertible.ToString(invariantCulture));
				return;
			case 9:
				v = new Variant(new DispatchWrapper(convertible));
				return;
			case 10:
				v = new Variant(new ErrorWrapper(convertible.ToInt32(invariantCulture)));
				return;
			case 11:
				v = new Variant(convertible.ToBoolean(invariantCulture));
				return;
			case 12:
				v = new Variant(convertible);
				return;
			case 13:
				v = new Variant(new UnknownWrapper(convertible));
				return;
			case 14:
				v = new Variant(convertible.ToDecimal(invariantCulture));
				return;
			case 16:
				v = new Variant(convertible.ToSByte(invariantCulture));
				return;
			case 17:
				v = new Variant(convertible.ToByte(invariantCulture));
				return;
			case 18:
				v = new Variant(convertible.ToUInt16(invariantCulture));
				return;
			case 19:
				v = new Variant(convertible.ToUInt32(invariantCulture));
				return;
			case 20:
				v = new Variant(convertible.ToInt64(invariantCulture));
				return;
			case 21:
				v = new Variant(convertible.ToUInt64(invariantCulture));
				return;
			case 22:
				v = new Variant(convertible.ToInt32(invariantCulture));
				return;
			case 23:
				v = new Variant(convertible.ToUInt32(invariantCulture));
				return;
			}
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_CannotCoerceByRefVariant"), new object[0]));
		}

		// Token: 0x040005DE RID: 1502
		internal const int CV_EMPTY = 0;

		// Token: 0x040005DF RID: 1503
		internal const int CV_VOID = 1;

		// Token: 0x040005E0 RID: 1504
		internal const int CV_BOOLEAN = 2;

		// Token: 0x040005E1 RID: 1505
		internal const int CV_CHAR = 3;

		// Token: 0x040005E2 RID: 1506
		internal const int CV_I1 = 4;

		// Token: 0x040005E3 RID: 1507
		internal const int CV_U1 = 5;

		// Token: 0x040005E4 RID: 1508
		internal const int CV_I2 = 6;

		// Token: 0x040005E5 RID: 1509
		internal const int CV_U2 = 7;

		// Token: 0x040005E6 RID: 1510
		internal const int CV_I4 = 8;

		// Token: 0x040005E7 RID: 1511
		internal const int CV_U4 = 9;

		// Token: 0x040005E8 RID: 1512
		internal const int CV_I8 = 10;

		// Token: 0x040005E9 RID: 1513
		internal const int CV_U8 = 11;

		// Token: 0x040005EA RID: 1514
		internal const int CV_R4 = 12;

		// Token: 0x040005EB RID: 1515
		internal const int CV_R8 = 13;

		// Token: 0x040005EC RID: 1516
		internal const int CV_STRING = 14;

		// Token: 0x040005ED RID: 1517
		internal const int CV_PTR = 15;

		// Token: 0x040005EE RID: 1518
		internal const int CV_DATETIME = 16;

		// Token: 0x040005EF RID: 1519
		internal const int CV_TIMESPAN = 17;

		// Token: 0x040005F0 RID: 1520
		internal const int CV_OBJECT = 18;

		// Token: 0x040005F1 RID: 1521
		internal const int CV_DECIMAL = 19;

		// Token: 0x040005F2 RID: 1522
		internal const int CV_ENUM = 21;

		// Token: 0x040005F3 RID: 1523
		internal const int CV_MISSING = 22;

		// Token: 0x040005F4 RID: 1524
		internal const int CV_NULL = 23;

		// Token: 0x040005F5 RID: 1525
		internal const int CV_LAST = 24;

		// Token: 0x040005F6 RID: 1526
		internal const int TypeCodeBitMask = 65535;

		// Token: 0x040005F7 RID: 1527
		internal const int VTBitMask = -16777216;

		// Token: 0x040005F8 RID: 1528
		internal const int VTBitShift = 24;

		// Token: 0x040005F9 RID: 1529
		internal const int ArrayBitMask = 65536;

		// Token: 0x040005FA RID: 1530
		internal const int EnumI1 = 1048576;

		// Token: 0x040005FB RID: 1531
		internal const int EnumU1 = 2097152;

		// Token: 0x040005FC RID: 1532
		internal const int EnumI2 = 3145728;

		// Token: 0x040005FD RID: 1533
		internal const int EnumU2 = 4194304;

		// Token: 0x040005FE RID: 1534
		internal const int EnumI4 = 5242880;

		// Token: 0x040005FF RID: 1535
		internal const int EnumU4 = 6291456;

		// Token: 0x04000600 RID: 1536
		internal const int EnumI8 = 7340032;

		// Token: 0x04000601 RID: 1537
		internal const int EnumU8 = 8388608;

		// Token: 0x04000602 RID: 1538
		internal const int EnumMask = 15728640;

		// Token: 0x04000603 RID: 1539
		private object m_objref;

		// Token: 0x04000604 RID: 1540
		private int m_data1;

		// Token: 0x04000605 RID: 1541
		private int m_data2;

		// Token: 0x04000606 RID: 1542
		private int m_flags;

		// Token: 0x04000607 RID: 1543
		private static Type _voidPtr = null;

		// Token: 0x04000608 RID: 1544
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
			typeof(object),
			typeof(Missing),
			typeof(DBNull)
		};

		// Token: 0x04000609 RID: 1545
		internal static readonly Variant Empty = default(Variant);

		// Token: 0x0400060A RID: 1546
		internal static readonly Variant Missing = new Variant(22, Type.Missing, 0, 0);

		// Token: 0x0400060B RID: 1547
		internal static readonly Variant DBNull = new Variant(23, System.DBNull.Value, 0, 0);
	}
}
