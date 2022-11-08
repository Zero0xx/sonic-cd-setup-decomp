using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200001C RID: 28
	internal static class ThrowHelper
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00004C4C File Offset: 0x00003C4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void ThrowArgumentOutOfRangeException()
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004C58 File Offset: 0x00003C58
		internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_WrongType", new object[]
			{
				key,
				targetType
			}), "key");
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004C8C File Offset: 0x00003C8C
		internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType)
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_WrongType", new object[]
			{
				value,
				targetType
			}), "value");
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004CBD File Offset: 0x00003CBD
		internal static void ThrowKeyNotFoundException()
		{
			throw new KeyNotFoundException();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004CC4 File Offset: 0x00003CC4
		internal static void ThrowArgumentException(ExceptionResource resource)
		{
			throw new ArgumentException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004CD6 File Offset: 0x00003CD6
		internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument)
		{
			throw new ArgumentException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)), ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004CEE File Offset: 0x00003CEE
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw new ArgumentNullException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004CFB File Offset: 0x00003CFB
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004D08 File Offset: 0x00003D08
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
		{
			throw new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004D20 File Offset: 0x00003D20
		internal static void ThrowInvalidOperationException(ExceptionResource resource)
		{
			throw new InvalidOperationException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004D32 File Offset: 0x00003D32
		internal static void ThrowSerializationException(ExceptionResource resource)
		{
			throw new SerializationException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004D44 File Offset: 0x00003D44
		internal static void ThrowSecurityException(ExceptionResource resource)
		{
			throw new SecurityException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004D56 File Offset: 0x00003D56
		internal static void ThrowNotSupportedException(ExceptionResource resource)
		{
			throw new NotSupportedException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004D68 File Offset: 0x00003D68
		internal static void ThrowUnauthorizedAccessException(ExceptionResource resource)
		{
			throw new UnauthorizedAccessException(Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004D7A File Offset: 0x00003D7A
		internal static void ThrowObjectDisposedException(string objectName, ExceptionResource resource)
		{
			throw new ObjectDisposedException(objectName, Environment.GetResourceString(ThrowHelper.GetResourceName(resource)));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004D90 File Offset: 0x00003D90
		internal static string GetArgumentName(ExceptionArgument argument)
		{
			string result;
			switch (argument)
			{
			case ExceptionArgument.obj:
				result = "obj";
				break;
			case ExceptionArgument.dictionary:
				result = "dictionary";
				break;
			case ExceptionArgument.dictionaryCreationThreshold:
				result = "dictionaryCreationThreshold";
				break;
			case ExceptionArgument.array:
				result = "array";
				break;
			case ExceptionArgument.info:
				result = "info";
				break;
			case ExceptionArgument.key:
				result = "key";
				break;
			case ExceptionArgument.collection:
				result = "collection";
				break;
			case ExceptionArgument.list:
				result = "list";
				break;
			case ExceptionArgument.match:
				result = "match";
				break;
			case ExceptionArgument.converter:
				result = "converter";
				break;
			case ExceptionArgument.queue:
				result = "queue";
				break;
			case ExceptionArgument.stack:
				result = "stack";
				break;
			case ExceptionArgument.capacity:
				result = "capacity";
				break;
			case ExceptionArgument.index:
				result = "index";
				break;
			case ExceptionArgument.startIndex:
				result = "startIndex";
				break;
			case ExceptionArgument.value:
				result = "value";
				break;
			case ExceptionArgument.count:
				result = "count";
				break;
			case ExceptionArgument.arrayIndex:
				result = "arrayIndex";
				break;
			case ExceptionArgument.name:
				result = "name";
				break;
			case ExceptionArgument.mode:
				result = "mode";
				break;
			default:
				return string.Empty;
			}
			return result;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004EB0 File Offset: 0x00003EB0
		internal static string GetResourceName(ExceptionResource resource)
		{
			string result;
			switch (resource)
			{
			case ExceptionResource.Argument_ImplementIComparable:
				result = "Argument_ImplementIComparable";
				break;
			case ExceptionResource.Argument_InvalidType:
				result = "Argument_InvalidType";
				break;
			case ExceptionResource.Argument_InvalidArgumentForComparison:
				result = "Argument_InvalidArgumentForComparison";
				break;
			case ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck:
				result = "Argument_InvalidRegistryKeyPermissionCheck";
				break;
			case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
				result = "ArgumentOutOfRange_NeedNonNegNum";
				break;
			case ExceptionResource.Arg_ArrayPlusOffTooSmall:
				result = "Arg_ArrayPlusOffTooSmall";
				break;
			case ExceptionResource.Arg_NonZeroLowerBound:
				result = "Arg_NonZeroLowerBound";
				break;
			case ExceptionResource.Arg_RankMultiDimNotSupported:
				result = "Arg_RankMultiDimNotSupported";
				break;
			case ExceptionResource.Arg_RegKeyDelHive:
				result = "Arg_RegKeyDelHive";
				break;
			case ExceptionResource.Arg_RegKeyStrLenBug:
				result = "Arg_RegKeyStrLenBug";
				break;
			case ExceptionResource.Arg_RegSetStrArrNull:
				result = "Arg_RegSetStrArrNull";
				break;
			case ExceptionResource.Arg_RegSetMismatchedKind:
				result = "Arg_RegSetMismatchedKind";
				break;
			case ExceptionResource.Arg_RegSubKeyAbsent:
				result = "Arg_RegSubKeyAbsent";
				break;
			case ExceptionResource.Arg_RegSubKeyValueAbsent:
				result = "Arg_RegSubKeyValueAbsent";
				break;
			case ExceptionResource.Argument_AddingDuplicate:
				result = "Argument_AddingDuplicate";
				break;
			case ExceptionResource.Serialization_InvalidOnDeser:
				result = "Serialization_InvalidOnDeser";
				break;
			case ExceptionResource.Serialization_MissingKeyValuePairs:
				result = "Serialization_MissingKeyValuePairs";
				break;
			case ExceptionResource.Serialization_NullKey:
				result = "Serialization_NullKey";
				break;
			case ExceptionResource.Argument_InvalidArrayType:
				result = "Argument_InvalidArrayType";
				break;
			case ExceptionResource.NotSupported_KeyCollectionSet:
				result = "NotSupported_KeyCollectionSet";
				break;
			case ExceptionResource.NotSupported_ValueCollectionSet:
				result = "NotSupported_ValueCollectionSet";
				break;
			case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
				result = "ArgumentOutOfRange_SmallCapacity";
				break;
			case ExceptionResource.ArgumentOutOfRange_Index:
				result = "ArgumentOutOfRange_Index";
				break;
			case ExceptionResource.Argument_InvalidOffLen:
				result = "Argument_InvalidOffLen";
				break;
			case ExceptionResource.Argument_ItemNotExist:
				result = "Argument_ItemNotExist";
				break;
			case ExceptionResource.ArgumentOutOfRange_Count:
				result = "ArgumentOutOfRange_Count";
				break;
			case ExceptionResource.ArgumentOutOfRange_InvalidThreshold:
				result = "ArgumentOutOfRange_InvalidThreshold";
				break;
			case ExceptionResource.ArgumentOutOfRange_ListInsert:
				result = "ArgumentOutOfRange_ListInsert";
				break;
			case ExceptionResource.NotSupported_ReadOnlyCollection:
				result = "NotSupported_ReadOnlyCollection";
				break;
			case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
				result = "InvalidOperation_CannotRemoveFromStackOrQueue";
				break;
			case ExceptionResource.InvalidOperation_EmptyQueue:
				result = "InvalidOperation_EmptyQueue";
				break;
			case ExceptionResource.InvalidOperation_EnumOpCantHappen:
				result = "InvalidOperation_EnumOpCantHappen";
				break;
			case ExceptionResource.InvalidOperation_EnumFailedVersion:
				result = "InvalidOperation_EnumFailedVersion";
				break;
			case ExceptionResource.InvalidOperation_EmptyStack:
				result = "InvalidOperation_EmptyStack";
				break;
			case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
				result = "ArgumentOutOfRange_BiggerThanCollection";
				break;
			case ExceptionResource.InvalidOperation_EnumNotStarted:
				result = "InvalidOperation_EnumNotStarted";
				break;
			case ExceptionResource.InvalidOperation_EnumEnded:
				result = "InvalidOperation_EnumEnded";
				break;
			case ExceptionResource.NotSupported_SortedListNestedWrite:
				result = "NotSupported_SortedListNestedWrite";
				break;
			case ExceptionResource.InvalidOperation_NoValue:
				result = "InvalidOperation_NoValue";
				break;
			case ExceptionResource.InvalidOperation_RegRemoveSubKey:
				result = "InvalidOperation_RegRemoveSubKey";
				break;
			case ExceptionResource.Security_RegistryPermission:
				result = "Security_RegistryPermission";
				break;
			case ExceptionResource.UnauthorizedAccess_RegistryNoWrite:
				result = "UnauthorizedAccess_RegistryNoWrite";
				break;
			case ExceptionResource.ObjectDisposed_RegKeyClosed:
				result = "ObjectDisposed_RegKeyClosed";
				break;
			case ExceptionResource.NotSupported_InComparableType:
				result = "NotSupported_InComparableType";
				break;
			default:
				return string.Empty;
			}
			return result;
		}
	}
}
