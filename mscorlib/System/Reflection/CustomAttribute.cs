using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200030A RID: 778
	internal static class CustomAttribute
	{
		// Token: 0x06001E69 RID: 7785 RVA: 0x0004BD70 File Offset: 0x0004AD70
		internal static bool IsDefined(RuntimeType type, RuntimeType caType, bool inherit)
		{
			if (type.GetElementType() != null)
			{
				return false;
			}
			if (PseudoCustomAttribute.IsDefined(type, caType))
			{
				return true;
			}
			if (CustomAttribute.IsCustomAttributeDefined(type.Module, type.MetadataToken, caType))
			{
				return true;
			}
			if (!inherit)
			{
				return false;
			}
			for (type = (type.BaseType as RuntimeType); type != null; type = (type.BaseType as RuntimeType))
			{
				if (CustomAttribute.IsCustomAttributeDefined(type.Module, type.MetadataToken, caType, inherit))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0004BDE4 File Offset: 0x0004ADE4
		internal static bool IsDefined(RuntimeMethodInfo method, RuntimeType caType, bool inherit)
		{
			if (PseudoCustomAttribute.IsDefined(method, caType))
			{
				return true;
			}
			if (CustomAttribute.IsCustomAttributeDefined(method.Module, method.MetadataToken, caType))
			{
				return true;
			}
			if (!inherit)
			{
				return false;
			}
			for (method = (method.GetParentDefinition() as RuntimeMethodInfo); method != null; method = (method.GetParentDefinition() as RuntimeMethodInfo))
			{
				if (CustomAttribute.IsCustomAttributeDefined(method.Module, method.MetadataToken, caType, inherit))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0004BE4E File Offset: 0x0004AE4E
		internal static bool IsDefined(RuntimeConstructorInfo ctor, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(ctor, caType) || CustomAttribute.IsCustomAttributeDefined(ctor.Module, ctor.MetadataToken, caType);
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0004BE6D File Offset: 0x0004AE6D
		internal static bool IsDefined(RuntimePropertyInfo property, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(property, caType) || CustomAttribute.IsCustomAttributeDefined(property.Module, property.MetadataToken, caType);
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0004BE8C File Offset: 0x0004AE8C
		internal static bool IsDefined(RuntimeEventInfo e, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(e, caType) || CustomAttribute.IsCustomAttributeDefined(e.Module, e.MetadataToken, caType);
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0004BEAB File Offset: 0x0004AEAB
		internal static bool IsDefined(RuntimeFieldInfo field, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(field, caType) || CustomAttribute.IsCustomAttributeDefined(field.Module, field.MetadataToken, caType);
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0004BECA File Offset: 0x0004AECA
		internal static bool IsDefined(ParameterInfo parameter, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(parameter, caType) || CustomAttribute.IsCustomAttributeDefined(parameter.Member.Module, parameter.MetadataToken, caType);
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0004BEF0 File Offset: 0x0004AEF0
		internal static bool IsDefined(Assembly assembly, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(assembly, caType) || CustomAttribute.IsCustomAttributeDefined(assembly.ManifestModule, assembly.AssemblyHandle.GetToken(), caType);
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0004BF22 File Offset: 0x0004AF22
		internal static bool IsDefined(Module module, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(module, caType) || CustomAttribute.IsCustomAttributeDefined(module, module.MetadataToken, caType);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0004BF3C File Offset: 0x0004AF3C
		internal static object[] GetCustomAttributes(RuntimeType type, RuntimeType caType, bool inherit)
		{
			if (type.GetElementType() != null)
			{
				if (!caType.IsValueType)
				{
					return (object[])Array.CreateInstance(caType, 0);
				}
				return new object[0];
			}
			else
			{
				if (type.IsGenericType && !type.IsGenericTypeDefinition)
				{
					type = (type.GetGenericTypeDefinition() as RuntimeType);
				}
				int i = 0;
				Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(type, caType, true, out i);
				if (!inherit || (caType.IsSealed && !CustomAttribute.GetAttributeUsage(caType).Inherited))
				{
					object[] customAttributes2 = CustomAttribute.GetCustomAttributes(type.Module, type.MetadataToken, i, caType);
					if (i > 0)
					{
						Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - i, i);
					}
					return customAttributes2;
				}
				List<object> list = new List<object>();
				bool mustBeInheritable = false;
				Type elementType = (caType == null || caType.IsValueType || caType.ContainsGenericParameters) ? typeof(object) : caType;
				while (i > 0)
				{
					list.Add(customAttributes[--i]);
				}
				while (type != typeof(object) && type != null)
				{
					object[] customAttributes3 = CustomAttribute.GetCustomAttributes(type.Module, type.MetadataToken, 0, caType, mustBeInheritable, list);
					mustBeInheritable = true;
					for (int j = 0; j < customAttributes3.Length; j++)
					{
						list.Add(customAttributes3[j]);
					}
					type = (type.BaseType as RuntimeType);
				}
				object[] array = Array.CreateInstance(elementType, list.Count) as object[];
				Array.Copy(list.ToArray(), 0, array, 0, list.Count);
				return array;
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0004C0A0 File Offset: 0x0004B0A0
		internal static object[] GetCustomAttributes(RuntimeMethodInfo method, RuntimeType caType, bool inherit)
		{
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{
				method = (method.GetGenericMethodDefinition() as RuntimeMethodInfo);
			}
			int i = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(method, caType, true, out i);
			if (!inherit || (caType.IsSealed && !CustomAttribute.GetAttributeUsage(caType).Inherited))
			{
				object[] customAttributes2 = CustomAttribute.GetCustomAttributes(method.Module, method.MetadataToken, i, caType);
				if (i > 0)
				{
					Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - i, i);
				}
				return customAttributes2;
			}
			List<object> list = new List<object>();
			bool mustBeInheritable = false;
			Type elementType = (caType == null || caType.IsValueType || caType.ContainsGenericParameters) ? typeof(object) : caType;
			while (i > 0)
			{
				list.Add(customAttributes[--i]);
			}
			while (method != null)
			{
				object[] customAttributes3 = CustomAttribute.GetCustomAttributes(method.Module, method.MetadataToken, 0, caType, mustBeInheritable, list);
				mustBeInheritable = true;
				for (int j = 0; j < customAttributes3.Length; j++)
				{
					list.Add(customAttributes3[j]);
				}
				method = (method.GetParentDefinition() as RuntimeMethodInfo);
			}
			object[] array = Array.CreateInstance(elementType, list.Count) as object[];
			Array.Copy(list.ToArray(), 0, array, 0, list.Count);
			return array;
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0004C1D4 File Offset: 0x0004B1D4
		internal static object[] GetCustomAttributes(RuntimeConstructorInfo ctor, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(ctor, caType, true, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(ctor.Module, ctor.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0004C218 File Offset: 0x0004B218
		internal static object[] GetCustomAttributes(RuntimePropertyInfo property, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(property, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(property.Module, property.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0004C258 File Offset: 0x0004B258
		internal static object[] GetCustomAttributes(RuntimeEventInfo e, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(e, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(e.Module, e.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0004C298 File Offset: 0x0004B298
		internal static object[] GetCustomAttributes(RuntimeFieldInfo field, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(field, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(field.Module, field.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0004C2D8 File Offset: 0x0004B2D8
		internal static object[] GetCustomAttributes(ParameterInfo parameter, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(parameter, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(parameter.Member.Module, parameter.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0004C320 File Offset: 0x0004B320
		internal static object[] GetCustomAttributes(Assembly assembly, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(assembly, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(assembly.ManifestModule, assembly.AssemblyHandle.GetToken(), num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0004C368 File Offset: 0x0004B368
		internal static object[] GetCustomAttributes(Module module, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(module, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(module, module.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0004C3A3 File Offset: 0x0004B3A3
		internal static bool IsCustomAttributeDefined(Module decoratedModule, int decoratedMetadataToken, RuntimeType attributeFilterType)
		{
			return CustomAttribute.IsCustomAttributeDefined(decoratedModule, decoratedMetadataToken, attributeFilterType, false);
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0004C3B0 File Offset: 0x0004B3B0
		internal static bool IsCustomAttributeDefined(Module decoratedModule, int decoratedMetadataToken, RuntimeType attributeFilterType, bool mustBeInheritable)
		{
			if (decoratedModule.Assembly.ReflectionOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyCA"));
			}
			MetadataImport metadataImport = decoratedModule.MetadataImport;
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(decoratedModule, decoratedMetadataToken);
			Assembly assembly = null;
			foreach (CustomAttributeRecord caRecord in customAttributeRecords)
			{
				RuntimeType runtimeType;
				RuntimeMethodHandle runtimeMethodHandle;
				bool flag;
				bool flag2;
				if (CustomAttribute.FilterCustomAttributeRecord(caRecord, metadataImport, ref assembly, decoratedModule, decoratedMetadataToken, attributeFilterType, mustBeInheritable, null, null, out runtimeType, out runtimeMethodHandle, out flag, out flag2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0004C42F File Offset: 0x0004B42F
		internal static object[] GetCustomAttributes(Module decoratedModule, int decoratedMetadataToken, int pcaCount, RuntimeType attributeFilterType)
		{
			return CustomAttribute.GetCustomAttributes(decoratedModule, decoratedMetadataToken, pcaCount, attributeFilterType, false, null);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0004C43C File Offset: 0x0004B43C
		internal unsafe static object[] GetCustomAttributes(Module decoratedModule, int decoratedMetadataToken, int pcaCount, RuntimeType attributeFilterType, bool mustBeInheritable, IList derivedAttributes)
		{
			if (decoratedModule.Assembly.ReflectionOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyCA"));
			}
			MetadataImport metadataImport = decoratedModule.MetadataImport;
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(decoratedModule, decoratedMetadataToken);
			Type elementType = (attributeFilterType == null || attributeFilterType.IsValueType || attributeFilterType.ContainsGenericParameters) ? typeof(object) : attributeFilterType;
			if (attributeFilterType == null && customAttributeRecords.Length == 0)
			{
				return Array.CreateInstance(elementType, 0) as object[];
			}
			object[] array = Array.CreateInstance(elementType, customAttributeRecords.Length) as object[];
			int num = 0;
			SecurityContextFrame securityContextFrame = default(SecurityContextFrame);
			securityContextFrame.Push(decoratedModule.Assembly.InternalAssembly);
			Assembly assembly = null;
			for (int i = 0; i < customAttributeRecords.Length; i++)
			{
				object obj = null;
				CustomAttributeRecord caRecord = customAttributeRecords[i];
				RuntimeMethodHandle ctor = default(RuntimeMethodHandle);
				RuntimeType runtimeType = null;
				int num2 = 0;
				IntPtr intPtr = caRecord.blob.Signature;
				IntPtr intPtr2 = (IntPtr)((void*)((byte*)((void*)intPtr) + caRecord.blob.Length));
				bool flag;
				bool isVarArg;
				if (CustomAttribute.FilterCustomAttributeRecord(caRecord, metadataImport, ref assembly, decoratedModule, decoratedMetadataToken, attributeFilterType, mustBeInheritable, array, derivedAttributes, out runtimeType, out ctor, out flag, out isVarArg))
				{
					if (!ctor.IsNullHandle())
					{
						ctor.CheckLinktimeDemands(decoratedModule, decoratedMetadataToken);
					}
					RuntimeConstructorInfo.CheckCanCreateInstance(runtimeType, isVarArg);
					if (flag)
					{
						obj = CustomAttribute.CreateCaObject(decoratedModule, ctor, ref intPtr, intPtr2, out num2);
					}
					else
					{
						obj = runtimeType.TypeHandle.CreateCaInstance(ctor);
						if (Marshal.ReadInt16(intPtr) != 1)
						{
							throw new CustomAttributeFormatException();
						}
						intPtr = (IntPtr)((void*)((byte*)((void*)intPtr) + 2));
						num2 = (int)Marshal.ReadInt16(intPtr);
						intPtr = (IntPtr)((void*)((byte*)((void*)intPtr) + 2));
					}
					for (int j = 0; j < num2; j++)
					{
						IntPtr signature = caRecord.blob.Signature;
						string text;
						bool flag2;
						Type type;
						object obj2;
						CustomAttribute.GetPropertyOrFieldData(decoratedModule, ref intPtr, intPtr2, out text, out flag2, out type, out obj2);
						try
						{
							if (flag2)
							{
								if (type == null && obj2 != null)
								{
									type = ((obj2.GetType() == typeof(RuntimeType)) ? typeof(Type) : obj2.GetType());
								}
								RuntimePropertyInfo runtimePropertyInfo;
								if (type == null)
								{
									runtimePropertyInfo = (runtimeType.GetProperty(text) as RuntimePropertyInfo);
								}
								else
								{
									runtimePropertyInfo = (runtimeType.GetProperty(text, type, Type.EmptyTypes) as RuntimePropertyInfo);
								}
								RuntimeMethodInfo runtimeMethodInfo = runtimePropertyInfo.GetSetMethod(true) as RuntimeMethodInfo;
								if (runtimeMethodInfo.IsPublic)
								{
									runtimeMethodInfo.MethodHandle.CheckLinktimeDemands(decoratedModule, decoratedMetadataToken);
									runtimeMethodInfo.Invoke(obj, BindingFlags.Default, null, new object[]
									{
										obj2
									}, null, true);
								}
							}
							else
							{
								RtFieldInfo rtFieldInfo = runtimeType.GetField(text) as RtFieldInfo;
								rtFieldInfo.InternalSetValue(obj, obj2, BindingFlags.Default, Type.DefaultBinder, null, false);
							}
						}
						catch (Exception inner)
						{
							throw new CustomAttributeFormatException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString(flag2 ? "RFLCT.InvalidPropFail" : "RFLCT.InvalidFieldFail"), new object[]
							{
								text
							}), inner);
						}
					}
					if (!intPtr.Equals(intPtr2))
					{
						throw new CustomAttributeFormatException();
					}
					array[num++] = obj;
				}
			}
			securityContextFrame.Pop();
			if (num == customAttributeRecords.Length && pcaCount == 0)
			{
				return array;
			}
			if (num == 0)
			{
				Array.CreateInstance(elementType, 0);
			}
			object[] array2 = Array.CreateInstance(elementType, num + pcaCount) as object[];
			Array.Copy(array, 0, array2, 0, num);
			return array2;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0004C7A8 File Offset: 0x0004B7A8
		internal unsafe static bool FilterCustomAttributeRecord(CustomAttributeRecord caRecord, MetadataImport scope, ref Assembly lastAptcaOkAssembly, Module decoratedModule, MetadataToken decoratedToken, RuntimeType attributeFilterType, bool mustBeInheritable, object[] attributes, IList derivedAttributes, out RuntimeType attributeType, out RuntimeMethodHandle ctor, out bool ctorHasParameters, out bool isVarArg)
		{
			ctor = default(RuntimeMethodHandle);
			attributeType = null;
			ctorHasParameters = false;
			isVarArg = false;
			IntPtr signature = caRecord.blob.Signature;
			(IntPtr)((void*)((byte*)((void*)signature) + caRecord.blob.Length));
			attributeType = (decoratedModule.ResolveType(scope.GetParentToken(caRecord.tkCtor), null, null) as RuntimeType);
			if (!attributeFilterType.IsAssignableFrom(attributeType))
			{
				return false;
			}
			if (!CustomAttribute.AttributeUsageCheck(attributeType, mustBeInheritable, attributes, derivedAttributes))
			{
				return false;
			}
			if (attributeType.Assembly != lastAptcaOkAssembly && !attributeType.Assembly.AptcaCheck(decoratedModule.Assembly))
			{
				return false;
			}
			lastAptcaOkAssembly = decoratedModule.Assembly;
			ConstArray methodSignature = scope.GetMethodSignature(caRecord.tkCtor);
			isVarArg = ((methodSignature[0] & 5) != 0);
			ctorHasParameters = (methodSignature[1] != 0);
			if (ctorHasParameters)
			{
				ctor = decoratedModule.ModuleHandle.ResolveMethodHandle(caRecord.tkCtor);
			}
			else
			{
				ctor = attributeType.GetTypeHandleInternal().GetDefaultConstructor();
				if (ctor.IsNullHandle() && !attributeType.IsValueType)
				{
					throw new MissingMethodException(".ctor");
				}
			}
			if (ctor.IsNullHandle())
			{
				return attributeType.IsVisible || attributeType.TypeHandle.IsVisibleFromModule(decoratedModule.ModuleHandle);
			}
			if (ctor.IsVisibleFromModule(decoratedModule))
			{
				return true;
			}
			MetadataToken token = default(MetadataToken);
			if (decoratedToken.IsParamDef)
			{
				token = new MetadataToken(scope.GetParentToken(decoratedToken));
				token = new MetadataToken(scope.GetParentToken(token));
			}
			else if (decoratedToken.IsMethodDef || decoratedToken.IsProperty || decoratedToken.IsEvent || decoratedToken.IsFieldDef)
			{
				token = new MetadataToken(scope.GetParentToken(decoratedToken));
			}
			else if (decoratedToken.IsTypeDef)
			{
				token = decoratedToken;
			}
			return token.IsTypeDef && ctor.IsVisibleFromType(decoratedModule.ModuleHandle.ResolveTypeHandle(token));
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0004C9D4 File Offset: 0x0004B9D4
		private static bool AttributeUsageCheck(RuntimeType attributeType, bool mustBeInheritable, object[] attributes, IList derivedAttributes)
		{
			AttributeUsageAttribute attributeUsageAttribute = null;
			if (mustBeInheritable)
			{
				attributeUsageAttribute = CustomAttribute.GetAttributeUsage(attributeType);
				if (!attributeUsageAttribute.Inherited)
				{
					return false;
				}
			}
			if (derivedAttributes == null)
			{
				return true;
			}
			for (int i = 0; i < derivedAttributes.Count; i++)
			{
				if (derivedAttributes[i].GetType() == attributeType)
				{
					if (attributeUsageAttribute == null)
					{
						attributeUsageAttribute = CustomAttribute.GetAttributeUsage(attributeType);
					}
					return attributeUsageAttribute.AllowMultiple;
				}
			}
			return true;
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0004CA30 File Offset: 0x0004BA30
		internal static AttributeUsageAttribute GetAttributeUsage(RuntimeType decoratedAttribute)
		{
			Module module = decoratedAttribute.Module;
			MetadataImport metadataImport = module.MetadataImport;
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(module, decoratedAttribute.MetadataToken);
			AttributeUsageAttribute attributeUsageAttribute = null;
			foreach (CustomAttributeRecord customAttributeRecord in customAttributeRecords)
			{
				RuntimeType runtimeType = module.ResolveType(metadataImport.GetParentToken(customAttributeRecord.tkCtor), null, null) as RuntimeType;
				if (runtimeType == typeof(AttributeUsageAttribute))
				{
					if (attributeUsageAttribute != null)
					{
						throw new FormatException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Format_AttributeUsage"), new object[]
						{
							runtimeType
						}));
					}
					AttributeTargets validOn;
					bool inherited;
					bool allowMultiple;
					CustomAttribute.ParseAttributeUsageAttribute(customAttributeRecord.blob, out validOn, out inherited, out allowMultiple);
					attributeUsageAttribute = new AttributeUsageAttribute(validOn, allowMultiple, inherited);
				}
			}
			if (attributeUsageAttribute == null)
			{
				return AttributeUsageAttribute.Default;
			}
			return attributeUsageAttribute;
		}

		// Token: 0x06001E82 RID: 7810
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _ParseAttributeUsageAttribute(IntPtr pCa, int cCa, out int targets, out bool inherited, out bool allowMultiple);

		// Token: 0x06001E83 RID: 7811 RVA: 0x0004CB08 File Offset: 0x0004BB08
		private static void ParseAttributeUsageAttribute(ConstArray ca, out AttributeTargets targets, out bool inherited, out bool allowMultiple)
		{
			int num;
			CustomAttribute._ParseAttributeUsageAttribute(ca.Signature, ca.Length, out num, out inherited, out allowMultiple);
			targets = (AttributeTargets)num;
		}

		// Token: 0x06001E84 RID: 7812
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern object _CreateCaObject(void* pModule, void* pCtor, byte** ppBlob, byte* pEndBlob, int* pcNamedArgs);

		// Token: 0x06001E85 RID: 7813 RVA: 0x0004CB30 File Offset: 0x0004BB30
		private unsafe static object CreateCaObject(Module module, RuntimeMethodHandle ctor, ref IntPtr blob, IntPtr blobEnd, out int namedArgs)
		{
			byte* value = (byte*)((void*)blob);
			byte* pEndBlob = (byte*)((void*)blobEnd);
			int num;
			object result = CustomAttribute._CreateCaObject(module.ModuleHandle.Value, (void*)ctor.Value, &value, pEndBlob, &num);
			blob = (IntPtr)((void*)value);
			namedArgs = num;
			return result;
		}

		// Token: 0x06001E86 RID: 7814
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetPropertyOrFieldData(IntPtr pModule, byte** ppBlobStart, byte* pBlobEnd, out string name, out bool bIsProperty, out Type type, out object value);

		// Token: 0x06001E87 RID: 7815 RVA: 0x0004CB8C File Offset: 0x0004BB8C
		private unsafe static void GetPropertyOrFieldData(Module module, ref IntPtr blobStart, IntPtr blobEnd, out string name, out bool isProperty, out Type type, out object value)
		{
			byte* value2 = (byte*)((void*)blobStart);
			CustomAttribute._GetPropertyOrFieldData((IntPtr)module.ModuleHandle.Value, &value2, (byte*)((void*)blobEnd), out name, out isProperty, out type, out value);
			blobStart = (IntPtr)((void*)value2);
		}
	}
}
