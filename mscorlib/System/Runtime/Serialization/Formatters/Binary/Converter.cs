using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007F7 RID: 2039
	internal sealed class Converter
	{
		// Token: 0x06004812 RID: 18450 RVA: 0x000F921A File Offset: 0x000F821A
		private Converter()
		{
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x000F9224 File Offset: 0x000F8224
		internal static InternalPrimitiveTypeE ToCode(Type type)
		{
			InternalPrimitiveTypeE result;
			if (type != null && !type.IsPrimitive)
			{
				if (type == Converter.typeofDateTime)
				{
					result = InternalPrimitiveTypeE.DateTime;
				}
				else if (type == Converter.typeofTimeSpan)
				{
					result = InternalPrimitiveTypeE.TimeSpan;
				}
				else if (type == Converter.typeofDecimal)
				{
					result = InternalPrimitiveTypeE.Decimal;
				}
				else
				{
					result = InternalPrimitiveTypeE.Invalid;
				}
			}
			else
			{
				result = Converter.ToPrimitiveTypeEnum(Type.GetTypeCode(type));
			}
			return result;
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x000F9274 File Offset: 0x000F8274
		internal static bool IsWriteAsByteArray(InternalPrimitiveTypeE code)
		{
			bool result = false;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
			case InternalPrimitiveTypeE.Byte:
			case InternalPrimitiveTypeE.Char:
			case InternalPrimitiveTypeE.Double:
			case InternalPrimitiveTypeE.Int16:
			case InternalPrimitiveTypeE.Int32:
			case InternalPrimitiveTypeE.Int64:
			case InternalPrimitiveTypeE.SByte:
			case InternalPrimitiveTypeE.Single:
			case InternalPrimitiveTypeE.UInt16:
			case InternalPrimitiveTypeE.UInt32:
			case InternalPrimitiveTypeE.UInt64:
				result = true;
				break;
			}
			return result;
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x000F92D4 File Offset: 0x000F82D4
		internal static int TypeLength(InternalPrimitiveTypeE code)
		{
			int result = 0;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				result = 1;
				break;
			case InternalPrimitiveTypeE.Byte:
				result = 1;
				break;
			case InternalPrimitiveTypeE.Char:
				result = 2;
				break;
			case InternalPrimitiveTypeE.Double:
				result = 8;
				break;
			case InternalPrimitiveTypeE.Int16:
				result = 2;
				break;
			case InternalPrimitiveTypeE.Int32:
				result = 4;
				break;
			case InternalPrimitiveTypeE.Int64:
				result = 8;
				break;
			case InternalPrimitiveTypeE.SByte:
				result = 1;
				break;
			case InternalPrimitiveTypeE.Single:
				result = 4;
				break;
			case InternalPrimitiveTypeE.UInt16:
				result = 2;
				break;
			case InternalPrimitiveTypeE.UInt32:
				result = 4;
				break;
			case InternalPrimitiveTypeE.UInt64:
				result = 8;
				break;
			}
			return result;
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x000F9360 File Offset: 0x000F8360
		internal static InternalNameSpaceE GetNameSpaceEnum(InternalPrimitiveTypeE code, Type type, WriteObjectInfo objectInfo, out string typeName)
		{
			InternalNameSpaceE internalNameSpaceE = InternalNameSpaceE.None;
			typeName = null;
			if (code != InternalPrimitiveTypeE.Invalid)
			{
				switch (code)
				{
				case InternalPrimitiveTypeE.Boolean:
				case InternalPrimitiveTypeE.Byte:
				case InternalPrimitiveTypeE.Char:
				case InternalPrimitiveTypeE.Double:
				case InternalPrimitiveTypeE.Int16:
				case InternalPrimitiveTypeE.Int32:
				case InternalPrimitiveTypeE.Int64:
				case InternalPrimitiveTypeE.SByte:
				case InternalPrimitiveTypeE.Single:
				case InternalPrimitiveTypeE.TimeSpan:
				case InternalPrimitiveTypeE.DateTime:
				case InternalPrimitiveTypeE.UInt16:
				case InternalPrimitiveTypeE.UInt32:
				case InternalPrimitiveTypeE.UInt64:
					internalNameSpaceE = InternalNameSpaceE.XdrPrimitive;
					typeName = "System." + Converter.ToComType(code);
					break;
				case InternalPrimitiveTypeE.Decimal:
					internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					typeName = "System." + Converter.ToComType(code);
					break;
				}
			}
			if (internalNameSpaceE == InternalNameSpaceE.None && type != null)
			{
				if (type == Converter.typeofString)
				{
					internalNameSpaceE = InternalNameSpaceE.XdrString;
				}
				else if (objectInfo == null)
				{
					typeName = type.FullName;
					if (type.Assembly == Converter.urtAssembly)
					{
						internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					}
					else
					{
						internalNameSpaceE = InternalNameSpaceE.UrtUser;
					}
				}
				else
				{
					typeName = objectInfo.GetTypeFullName();
					if (objectInfo.GetAssemblyString().Equals(Converter.urtAssemblyString))
					{
						internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					}
					else
					{
						internalNameSpaceE = InternalNameSpaceE.UrtUser;
					}
				}
			}
			return internalNameSpaceE;
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x000F943E File Offset: 0x000F843E
		internal static Type ToArrayType(InternalPrimitiveTypeE code)
		{
			if (Converter.arrayTypeA == null)
			{
				Converter.InitArrayTypeA();
			}
			return Converter.arrayTypeA[(int)code];
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x000F9454 File Offset: 0x000F8454
		private static void InitTypeA()
		{
			Type[] array = new Type[Converter.primitiveTypeEnumLength];
			array[0] = null;
			array[1] = Converter.typeofBoolean;
			array[2] = Converter.typeofByte;
			array[3] = Converter.typeofChar;
			array[5] = Converter.typeofDecimal;
			array[6] = Converter.typeofDouble;
			array[7] = Converter.typeofInt16;
			array[8] = Converter.typeofInt32;
			array[9] = Converter.typeofInt64;
			array[10] = Converter.typeofSByte;
			array[11] = Converter.typeofSingle;
			array[12] = Converter.typeofTimeSpan;
			array[13] = Converter.typeofDateTime;
			array[14] = Converter.typeofUInt16;
			array[15] = Converter.typeofUInt32;
			array[16] = Converter.typeofUInt64;
			Converter.typeA = array;
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x000F94F8 File Offset: 0x000F84F8
		private static void InitArrayTypeA()
		{
			Type[] array = new Type[Converter.primitiveTypeEnumLength];
			array[0] = null;
			array[1] = Converter.typeofBooleanArray;
			array[2] = Converter.typeofByteArray;
			array[3] = Converter.typeofCharArray;
			array[5] = Converter.typeofDecimalArray;
			array[6] = Converter.typeofDoubleArray;
			array[7] = Converter.typeofInt16Array;
			array[8] = Converter.typeofInt32Array;
			array[9] = Converter.typeofInt64Array;
			array[10] = Converter.typeofSByteArray;
			array[11] = Converter.typeofSingleArray;
			array[12] = Converter.typeofTimeSpanArray;
			array[13] = Converter.typeofDateTimeArray;
			array[14] = Converter.typeofUInt16Array;
			array[15] = Converter.typeofUInt32Array;
			array[16] = Converter.typeofUInt64Array;
			Converter.arrayTypeA = array;
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x000F959A File Offset: 0x000F859A
		internal static Type ToType(InternalPrimitiveTypeE code)
		{
			if (Converter.typeA == null)
			{
				Converter.InitTypeA();
			}
			return Converter.typeA[(int)code];
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x000F95B0 File Offset: 0x000F85B0
		internal static Array CreatePrimitiveArray(InternalPrimitiveTypeE code, int length)
		{
			Array result = null;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				result = new bool[length];
				break;
			case InternalPrimitiveTypeE.Byte:
				result = new byte[length];
				break;
			case InternalPrimitiveTypeE.Char:
				result = new char[length];
				break;
			case InternalPrimitiveTypeE.Decimal:
				result = new decimal[length];
				break;
			case InternalPrimitiveTypeE.Double:
				result = new double[length];
				break;
			case InternalPrimitiveTypeE.Int16:
				result = new short[length];
				break;
			case InternalPrimitiveTypeE.Int32:
				result = new int[length];
				break;
			case InternalPrimitiveTypeE.Int64:
				result = new long[length];
				break;
			case InternalPrimitiveTypeE.SByte:
				result = new sbyte[length];
				break;
			case InternalPrimitiveTypeE.Single:
				result = new float[length];
				break;
			case InternalPrimitiveTypeE.TimeSpan:
				result = new TimeSpan[length];
				break;
			case InternalPrimitiveTypeE.DateTime:
				result = new DateTime[length];
				break;
			case InternalPrimitiveTypeE.UInt16:
				result = new ushort[length];
				break;
			case InternalPrimitiveTypeE.UInt32:
				result = new uint[length];
				break;
			case InternalPrimitiveTypeE.UInt64:
				result = new ulong[length];
				break;
			}
			return result;
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x000F9694 File Offset: 0x000F8694
		internal static bool IsPrimitiveArray(Type type, out object typeInformation)
		{
			typeInformation = null;
			bool result = true;
			if (type == Converter.typeofBooleanArray)
			{
				typeInformation = InternalPrimitiveTypeE.Boolean;
			}
			else if (type == Converter.typeofByteArray)
			{
				typeInformation = InternalPrimitiveTypeE.Byte;
			}
			else if (type == Converter.typeofCharArray)
			{
				typeInformation = InternalPrimitiveTypeE.Char;
			}
			else if (type == Converter.typeofDoubleArray)
			{
				typeInformation = InternalPrimitiveTypeE.Double;
			}
			else if (type == Converter.typeofInt16Array)
			{
				typeInformation = InternalPrimitiveTypeE.Int16;
			}
			else if (type == Converter.typeofInt32Array)
			{
				typeInformation = InternalPrimitiveTypeE.Int32;
			}
			else if (type == Converter.typeofInt64Array)
			{
				typeInformation = InternalPrimitiveTypeE.Int64;
			}
			else if (type == Converter.typeofSByteArray)
			{
				typeInformation = InternalPrimitiveTypeE.SByte;
			}
			else if (type == Converter.typeofSingleArray)
			{
				typeInformation = InternalPrimitiveTypeE.Single;
			}
			else if (type == Converter.typeofUInt16Array)
			{
				typeInformation = InternalPrimitiveTypeE.UInt16;
			}
			else if (type == Converter.typeofUInt32Array)
			{
				typeInformation = InternalPrimitiveTypeE.UInt32;
			}
			else if (type == Converter.typeofUInt64Array)
			{
				typeInformation = InternalPrimitiveTypeE.UInt64;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x000F9798 File Offset: 0x000F8798
		private static void InitValueA()
		{
			string[] array = new string[Converter.primitiveTypeEnumLength];
			array[0] = null;
			array[1] = "Boolean";
			array[2] = "Byte";
			array[3] = "Char";
			array[5] = "Decimal";
			array[6] = "Double";
			array[7] = "Int16";
			array[8] = "Int32";
			array[9] = "Int64";
			array[10] = "SByte";
			array[11] = "Single";
			array[12] = "TimeSpan";
			array[13] = "DateTime";
			array[14] = "UInt16";
			array[15] = "UInt32";
			array[16] = "UInt64";
			Converter.valueA = array;
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x000F983A File Offset: 0x000F883A
		internal static string ToComType(InternalPrimitiveTypeE code)
		{
			if (Converter.valueA == null)
			{
				Converter.InitValueA();
			}
			return Converter.valueA[(int)code];
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x000F9850 File Offset: 0x000F8850
		private static void InitTypeCodeA()
		{
			TypeCode[] array = new TypeCode[Converter.primitiveTypeEnumLength];
			array[0] = TypeCode.Object;
			array[1] = TypeCode.Boolean;
			array[2] = TypeCode.Byte;
			array[3] = TypeCode.Char;
			array[5] = TypeCode.Decimal;
			array[6] = TypeCode.Double;
			array[7] = TypeCode.Int16;
			array[8] = TypeCode.Int32;
			array[9] = TypeCode.Int64;
			array[10] = TypeCode.SByte;
			array[11] = TypeCode.Single;
			array[12] = TypeCode.Object;
			array[13] = TypeCode.DateTime;
			array[14] = TypeCode.UInt16;
			array[15] = TypeCode.UInt32;
			array[16] = TypeCode.UInt64;
			Converter.typeCodeA = array;
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x000F98BE File Offset: 0x000F88BE
		internal static TypeCode ToTypeCode(InternalPrimitiveTypeE code)
		{
			if (Converter.typeCodeA == null)
			{
				Converter.InitTypeCodeA();
			}
			return Converter.typeCodeA[(int)code];
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x000F98D4 File Offset: 0x000F88D4
		private static void InitCodeA()
		{
			Converter.codeA = new InternalPrimitiveTypeE[]
			{
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Boolean,
				InternalPrimitiveTypeE.Char,
				InternalPrimitiveTypeE.SByte,
				InternalPrimitiveTypeE.Byte,
				InternalPrimitiveTypeE.Int16,
				InternalPrimitiveTypeE.UInt16,
				InternalPrimitiveTypeE.Int32,
				InternalPrimitiveTypeE.UInt32,
				InternalPrimitiveTypeE.Int64,
				InternalPrimitiveTypeE.UInt64,
				InternalPrimitiveTypeE.Single,
				InternalPrimitiveTypeE.Double,
				InternalPrimitiveTypeE.Decimal,
				InternalPrimitiveTypeE.DateTime,
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Invalid
			};
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x000F994C File Offset: 0x000F894C
		internal static InternalPrimitiveTypeE ToPrimitiveTypeEnum(TypeCode typeCode)
		{
			if (Converter.codeA == null)
			{
				Converter.InitCodeA();
			}
			return Converter.codeA[(int)typeCode];
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x000F9964 File Offset: 0x000F8964
		internal static object FromString(string value, InternalPrimitiveTypeE code)
		{
			object result;
			if (code != InternalPrimitiveTypeE.Invalid)
			{
				result = Convert.ChangeType(value, Converter.ToTypeCode(code), CultureInfo.InvariantCulture);
			}
			else
			{
				result = value;
			}
			return result;
		}

		// Token: 0x040024E9 RID: 9449
		private static int primitiveTypeEnumLength = 17;

		// Token: 0x040024EA RID: 9450
		private static Type[] typeA;

		// Token: 0x040024EB RID: 9451
		private static Type[] arrayTypeA;

		// Token: 0x040024EC RID: 9452
		private static string[] valueA;

		// Token: 0x040024ED RID: 9453
		private static TypeCode[] typeCodeA;

		// Token: 0x040024EE RID: 9454
		private static InternalPrimitiveTypeE[] codeA;

		// Token: 0x040024EF RID: 9455
		internal static Type typeofISerializable = typeof(ISerializable);

		// Token: 0x040024F0 RID: 9456
		internal static Type typeofString = typeof(string);

		// Token: 0x040024F1 RID: 9457
		internal static Type typeofConverter = typeof(Converter);

		// Token: 0x040024F2 RID: 9458
		internal static Type typeofBoolean = typeof(bool);

		// Token: 0x040024F3 RID: 9459
		internal static Type typeofByte = typeof(byte);

		// Token: 0x040024F4 RID: 9460
		internal static Type typeofChar = typeof(char);

		// Token: 0x040024F5 RID: 9461
		internal static Type typeofDecimal = typeof(decimal);

		// Token: 0x040024F6 RID: 9462
		internal static Type typeofDouble = typeof(double);

		// Token: 0x040024F7 RID: 9463
		internal static Type typeofInt16 = typeof(short);

		// Token: 0x040024F8 RID: 9464
		internal static Type typeofInt32 = typeof(int);

		// Token: 0x040024F9 RID: 9465
		internal static Type typeofInt64 = typeof(long);

		// Token: 0x040024FA RID: 9466
		internal static Type typeofSByte = typeof(sbyte);

		// Token: 0x040024FB RID: 9467
		internal static Type typeofSingle = typeof(float);

		// Token: 0x040024FC RID: 9468
		internal static Type typeofTimeSpan = typeof(TimeSpan);

		// Token: 0x040024FD RID: 9469
		internal static Type typeofDateTime = typeof(DateTime);

		// Token: 0x040024FE RID: 9470
		internal static Type typeofUInt16 = typeof(ushort);

		// Token: 0x040024FF RID: 9471
		internal static Type typeofUInt32 = typeof(uint);

		// Token: 0x04002500 RID: 9472
		internal static Type typeofUInt64 = typeof(ulong);

		// Token: 0x04002501 RID: 9473
		internal static Type typeofObject = typeof(object);

		// Token: 0x04002502 RID: 9474
		internal static Type typeofSystemVoid = typeof(void);

		// Token: 0x04002503 RID: 9475
		internal static Assembly urtAssembly = Assembly.GetAssembly(Converter.typeofString);

		// Token: 0x04002504 RID: 9476
		internal static string urtAssemblyString = Converter.urtAssembly.FullName;

		// Token: 0x04002505 RID: 9477
		internal static Type typeofTypeArray = typeof(Type[]);

		// Token: 0x04002506 RID: 9478
		internal static Type typeofObjectArray = typeof(object[]);

		// Token: 0x04002507 RID: 9479
		internal static Type typeofStringArray = typeof(string[]);

		// Token: 0x04002508 RID: 9480
		internal static Type typeofBooleanArray = typeof(bool[]);

		// Token: 0x04002509 RID: 9481
		internal static Type typeofByteArray = typeof(byte[]);

		// Token: 0x0400250A RID: 9482
		internal static Type typeofCharArray = typeof(char[]);

		// Token: 0x0400250B RID: 9483
		internal static Type typeofDecimalArray = typeof(decimal[]);

		// Token: 0x0400250C RID: 9484
		internal static Type typeofDoubleArray = typeof(double[]);

		// Token: 0x0400250D RID: 9485
		internal static Type typeofInt16Array = typeof(short[]);

		// Token: 0x0400250E RID: 9486
		internal static Type typeofInt32Array = typeof(int[]);

		// Token: 0x0400250F RID: 9487
		internal static Type typeofInt64Array = typeof(long[]);

		// Token: 0x04002510 RID: 9488
		internal static Type typeofSByteArray = typeof(sbyte[]);

		// Token: 0x04002511 RID: 9489
		internal static Type typeofSingleArray = typeof(float[]);

		// Token: 0x04002512 RID: 9490
		internal static Type typeofTimeSpanArray = typeof(TimeSpan[]);

		// Token: 0x04002513 RID: 9491
		internal static Type typeofDateTimeArray = typeof(DateTime[]);

		// Token: 0x04002514 RID: 9492
		internal static Type typeofUInt16Array = typeof(ushort[]);

		// Token: 0x04002515 RID: 9493
		internal static Type typeofUInt32Array = typeof(uint[]);

		// Token: 0x04002516 RID: 9494
		internal static Type typeofUInt64Array = typeof(ulong[]);

		// Token: 0x04002517 RID: 9495
		internal static Type typeofMarshalByRefObject = typeof(MarshalByRefObject);
	}
}
