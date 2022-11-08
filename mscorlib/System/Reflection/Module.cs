using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000332 RID: 818
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_Module))]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public class Module : _Module, ISerializable, ICustomAttributeProvider
	{
		// Token: 0x06001F14 RID: 7956
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type _GetTypeInternal(string className, bool ignoreCase, bool throwOnError);

		// Token: 0x06001F15 RID: 7957 RVA: 0x0004E376 File Offset: 0x0004D376
		internal Type GetTypeInternal(string className, bool ignoreCase, bool throwOnError)
		{
			return this.InternalModule._GetTypeInternal(className, ignoreCase, throwOnError);
		}

		// Token: 0x06001F16 RID: 7958
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr _GetHINSTANCE();

		// Token: 0x06001F17 RID: 7959 RVA: 0x0004E386 File Offset: 0x0004D386
		internal IntPtr GetHINSTANCE()
		{
			return this.InternalModule._GetHINSTANCE();
		}

		// Token: 0x06001F18 RID: 7960
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _InternalGetName();

		// Token: 0x06001F19 RID: 7961 RVA: 0x0004E393 File Offset: 0x0004D393
		private string InternalGetName()
		{
			return this.InternalModule._InternalGetName();
		}

		// Token: 0x06001F1A RID: 7962
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _InternalGetFullyQualifiedName();

		// Token: 0x06001F1B RID: 7963 RVA: 0x0004E3A0 File Offset: 0x0004D3A0
		internal string InternalGetFullyQualifiedName()
		{
			return this.InternalModule._InternalGetFullyQualifiedName();
		}

		// Token: 0x06001F1C RID: 7964
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type[] _GetTypesInternal(ref StackCrawlMark stackMark);

		// Token: 0x06001F1D RID: 7965 RVA: 0x0004E3AD File Offset: 0x0004D3AD
		internal Type[] GetTypesInternal(ref StackCrawlMark stackMark)
		{
			return this.InternalModule._GetTypesInternal(ref stackMark);
		}

		// Token: 0x06001F1E RID: 7966
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Assembly _GetAssemblyInternal();

		// Token: 0x06001F1F RID: 7967 RVA: 0x0004E3BB File Offset: 0x0004D3BB
		internal virtual Assembly GetAssemblyInternal()
		{
			return this.InternalModule._GetAssemblyInternal();
		}

		// Token: 0x06001F20 RID: 7968
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetTypeToken(string strFullName, Module refedModule, string strRefedModuleFileName, int tkResolution);

		// Token: 0x06001F21 RID: 7969 RVA: 0x0004E3C8 File Offset: 0x0004D3C8
		internal int InternalGetTypeToken(string strFullName, Module refedModule, string strRefedModuleFileName, int tkResolution)
		{
			return this.InternalModule._InternalGetTypeToken(strFullName, refedModule.InternalModule, strRefedModuleFileName, tkResolution);
		}

		// Token: 0x06001F22 RID: 7970
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type _InternalLoadInMemoryTypeByName(string className);

		// Token: 0x06001F23 RID: 7971 RVA: 0x0004E3DF File Offset: 0x0004D3DF
		internal Type InternalLoadInMemoryTypeByName(string className)
		{
			return this.InternalModule._InternalLoadInMemoryTypeByName(className);
		}

		// Token: 0x06001F24 RID: 7972
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRef(Module refedModule, int tr, int defToken);

		// Token: 0x06001F25 RID: 7973 RVA: 0x0004E3ED File Offset: 0x0004D3ED
		internal int InternalGetMemberRef(Module refedModule, int tr, int defToken)
		{
			return this.InternalModule._InternalGetMemberRef(refedModule.InternalModule, tr, defToken);
		}

		// Token: 0x06001F26 RID: 7974
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRefFromSignature(int tr, string methodName, byte[] signature, int length);

		// Token: 0x06001F27 RID: 7975 RVA: 0x0004E402 File Offset: 0x0004D402
		internal int InternalGetMemberRefFromSignature(int tr, string methodName, byte[] signature, int length)
		{
			return this.InternalModule._InternalGetMemberRefFromSignature(tr, methodName, signature, length);
		}

		// Token: 0x06001F28 RID: 7976
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRefOfMethodInfo(int tr, IntPtr method);

		// Token: 0x06001F29 RID: 7977 RVA: 0x0004E414 File Offset: 0x0004D414
		internal int InternalGetMemberRefOfMethodInfo(int tr, RuntimeMethodHandle method)
		{
			return this.InternalModule._InternalGetMemberRefOfMethodInfo(tr, method.Value);
		}

		// Token: 0x06001F2A RID: 7978
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRefOfFieldInfo(int tkType, IntPtr interfaceHandle, int tkField);

		// Token: 0x06001F2B RID: 7979 RVA: 0x0004E429 File Offset: 0x0004D429
		internal int InternalGetMemberRefOfFieldInfo(int tkType, RuntimeTypeHandle declaringType, int tkField)
		{
			return this.InternalModule._InternalGetMemberRefOfFieldInfo(tkType, declaringType.Value, tkField);
		}

		// Token: 0x06001F2C RID: 7980
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetTypeSpecTokenWithBytes(byte[] signature, int length);

		// Token: 0x06001F2D RID: 7981 RVA: 0x0004E43F File Offset: 0x0004D43F
		internal int InternalGetTypeSpecTokenWithBytes(byte[] signature, int length)
		{
			return this.InternalModule._InternalGetTypeSpecTokenWithBytes(signature, length);
		}

		// Token: 0x06001F2E RID: 7982
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nativeGetArrayMethodToken(int tkTypeSpec, string methodName, byte[] signature, int sigLength, int baseToken);

		// Token: 0x06001F2F RID: 7983 RVA: 0x0004E44E File Offset: 0x0004D44E
		internal int nativeGetArrayMethodToken(int tkTypeSpec, string methodName, byte[] signature, int sigLength, int baseToken)
		{
			return this.InternalModule._nativeGetArrayMethodToken(tkTypeSpec, methodName, signature, sigLength, baseToken);
		}

		// Token: 0x06001F30 RID: 7984
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSetFieldRVAContent(int fdToken, byte[] data, int length);

		// Token: 0x06001F31 RID: 7985 RVA: 0x0004E462 File Offset: 0x0004D462
		internal void InternalSetFieldRVAContent(int fdToken, byte[] data, int length)
		{
			this.InternalModule._InternalSetFieldRVAContent(fdToken, data, length);
		}

		// Token: 0x06001F32 RID: 7986
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetStringConstant(string str);

		// Token: 0x06001F33 RID: 7987 RVA: 0x0004E472 File Offset: 0x0004D472
		internal int InternalGetStringConstant(string str)
		{
			return this.InternalModule._InternalGetStringConstant(str);
		}

		// Token: 0x06001F34 RID: 7988
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalPreSavePEFile(int portableExecutableKind, int imageFileMachine);

		// Token: 0x06001F35 RID: 7989 RVA: 0x0004E480 File Offset: 0x0004D480
		internal void InternalPreSavePEFile(int portableExecutableKind, int imageFileMachine)
		{
			this.InternalModule._InternalPreSavePEFile(portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06001F36 RID: 7990
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSavePEFile(string fileName, int entryPoint, int isExe, bool isManifestFile);

		// Token: 0x06001F37 RID: 7991 RVA: 0x0004E48F File Offset: 0x0004D48F
		internal void InternalSavePEFile(string fileName, MethodToken entryPoint, int isExe, bool isManifestFile)
		{
			this.InternalModule._InternalSavePEFile(fileName, entryPoint.Token, isExe, isManifestFile);
		}

		// Token: 0x06001F38 RID: 7992
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSetResourceCounts(int resCount);

		// Token: 0x06001F39 RID: 7993 RVA: 0x0004E4A7 File Offset: 0x0004D4A7
		internal void InternalSetResourceCounts(int resCount)
		{
			this.InternalModule._InternalSetResourceCounts(resCount);
		}

		// Token: 0x06001F3A RID: 7994
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalAddResource(string strName, byte[] resBytes, int resByteCount, int tkFile, int attribute, int portableExecutableKind, int imageFileMachine);

		// Token: 0x06001F3B RID: 7995 RVA: 0x0004E4B5 File Offset: 0x0004D4B5
		internal void InternalAddResource(string strName, byte[] resBytes, int resByteCount, int tkFile, int attribute, int portableExecutableKind, int imageFileMachine)
		{
			this.InternalModule._InternalAddResource(strName, resBytes, resByteCount, tkFile, attribute, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06001F3C RID: 7996
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSetModuleProps(string strModuleName);

		// Token: 0x06001F3D RID: 7997 RVA: 0x0004E4CD File Offset: 0x0004D4CD
		internal void InternalSetModuleProps(string strModuleName)
		{
			this.InternalModule._InternalSetModuleProps(strModuleName);
		}

		// Token: 0x06001F3E RID: 7998
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _IsResourceInternal();

		// Token: 0x06001F3F RID: 7999 RVA: 0x0004E4DB File Offset: 0x0004D4DB
		internal bool IsResourceInternal()
		{
			return this.InternalModule._IsResourceInternal();
		}

		// Token: 0x06001F40 RID: 8000
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern X509Certificate _GetSignerCertificateInternal();

		// Token: 0x06001F41 RID: 8001 RVA: 0x0004E4E8 File Offset: 0x0004D4E8
		internal X509Certificate GetSignerCertificateInternal()
		{
			return this.InternalModule._GetSignerCertificateInternal();
		}

		// Token: 0x06001F42 RID: 8002
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalDefineNativeResourceFile(string strFilename, int portableExecutableKind, int ImageFileMachine);

		// Token: 0x06001F43 RID: 8003 RVA: 0x0004E4F5 File Offset: 0x0004D4F5
		internal void InternalDefineNativeResourceFile(string strFilename, int portableExecutableKind, int ImageFileMachine)
		{
			this.InternalModule._InternalDefineNativeResourceFile(strFilename, portableExecutableKind, ImageFileMachine);
		}

		// Token: 0x06001F44 RID: 8004
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalDefineNativeResourceBytes(byte[] resource, int portableExecutableKind, int imageFileMachine);

		// Token: 0x06001F45 RID: 8005 RVA: 0x0004E505 File Offset: 0x0004D505
		internal void InternalDefineNativeResourceBytes(byte[] resource, int portableExecutableKind, int imageFileMachine)
		{
			this.InternalModule._InternalDefineNativeResourceBytes(resource, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0004E518 File Offset: 0x0004D518
		static Module()
		{
			__Filters @object = new __Filters();
			Module.FilterTypeName = new TypeFilter(@object.FilterTypeName);
			Module.FilterTypeNameIgnoreCase = new TypeFilter(@object.FilterTypeNameIgnoreCase);
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0004E54F File Offset: 0x0004D54F
		public MethodBase ResolveMethod(int metadataToken)
		{
			return this.ResolveMethod(metadataToken, null, null);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0004E55C File Offset: 0x0004D55C
		private static RuntimeTypeHandle[] ConvertToTypeHandleArray(Type[] genericArguments)
		{
			if (genericArguments == null)
			{
				return null;
			}
			int num = genericArguments.Length;
			RuntimeTypeHandle[] array = new RuntimeTypeHandle[num];
			for (int i = 0; i < num; i++)
			{
				Type type = genericArguments[i];
				if (type == null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGenericInstArray"));
				}
				type = type.UnderlyingSystemType;
				if (type == null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGenericInstArray"));
				}
				if (!(type is RuntimeType))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGenericInstArray"));
				}
				array[i] = type.GetTypeHandleInternal();
			}
			return array;
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0004E5E4 File Offset: 0x0004D5E4
		public byte[] ResolveSignature(int metadataToken)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			if (!metadataToken2.IsMemberRef && !metadataToken2.IsMethodDef && !metadataToken2.IsTypeSpec && !metadataToken2.IsSignature && !metadataToken2.IsFieldDef)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]), "metadataToken");
			}
			ConstArray constArray;
			if (metadataToken2.IsMemberRef)
			{
				constArray = this.MetadataImport.GetMemberRefProps(metadataToken);
			}
			else
			{
				constArray = this.MetadataImport.GetSignatureFromToken(metadataToken);
			}
			byte[] array = new byte[constArray.Length];
			for (int i = 0; i < constArray.Length; i++)
			{
				array[i] = constArray[i];
			}
			return array;
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0004E714 File Offset: 0x0004D714
		public unsafe MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			RuntimeTypeHandle[] typeInstantiationContext = Module.ConvertToTypeHandleArray(genericTypeArguments);
			RuntimeTypeHandle[] methodInstantiationContext = Module.ConvertToTypeHandleArray(genericMethodArguments);
			MethodBase methodBase;
			try
			{
				if (!metadataToken2.IsMethodDef && !metadataToken2.IsMethodSpec)
				{
					if (!metadataToken2.IsMemberRef)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethod", new object[]
						{
							metadataToken2,
							this
						}), new object[0]));
					}
					if (*(byte*)this.MetadataImport.GetMemberRefProps(metadataToken2).Signature.ToPointer() == 6)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethod"), new object[]
						{
							metadataToken2,
							this
						}));
					}
				}
				RuntimeMethodHandle methodHandle = this.GetModuleHandle().ResolveMethodHandle(metadataToken2, typeInstantiationContext, methodInstantiationContext);
				Type type = methodHandle.GetDeclaringType().GetRuntimeType();
				if (type.IsGenericType || type.IsArray)
				{
					MetadataToken token = new MetadataToken(this.MetadataImport.GetParentToken(metadataToken2));
					if (metadataToken2.IsMethodSpec)
					{
						token = new MetadataToken(this.MetadataImport.GetParentToken(token));
					}
					type = this.ResolveType(token, genericTypeArguments, genericMethodArguments);
				}
				methodBase = RuntimeType.GetMethodBase(type.GetTypeHandleInternal(), methodHandle);
			}
			catch (BadImageFormatException innerException)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadImageFormatExceptionResolve"), innerException);
			}
			return methodBase;
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0004E924 File Offset: 0x0004D924
		internal FieldInfo ResolveLiteralField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			string name = this.MetadataImport.GetName(metadataToken2).ToString();
			int parentToken = this.MetadataImport.GetParentToken(metadataToken2);
			Type type = this.ResolveType(parentToken, genericTypeArguments, genericMethodArguments);
			type.GetFields();
			FieldInfo field;
			try
			{
				field = type.GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			catch
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveField"), new object[]
				{
					metadataToken2,
					this
				}), "metadataToken");
			}
			return field;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0004EA34 File Offset: 0x0004DA34
		public FieldInfo ResolveField(int metadataToken)
		{
			return this.ResolveField(metadataToken, null, null);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0004EA40 File Offset: 0x0004DA40
		public unsafe FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			RuntimeTypeHandle[] typeInstantiationContext = Module.ConvertToTypeHandleArray(genericTypeArguments);
			RuntimeTypeHandle[] methodInstantiationContext = Module.ConvertToTypeHandleArray(genericMethodArguments);
			FieldInfo result;
			try
			{
				RuntimeFieldHandle fieldHandle = default(RuntimeFieldHandle);
				if (!metadataToken2.IsFieldDef)
				{
					if (!metadataToken2.IsMemberRef)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveField"), new object[]
						{
							metadataToken2,
							this
						}));
					}
					if (*(byte*)this.MetadataImport.GetMemberRefProps(metadataToken2).Signature.ToPointer() != 6)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveField"), new object[]
						{
							metadataToken2,
							this
						}));
					}
					fieldHandle = this.GetModuleHandle().ResolveFieldHandle(metadataToken2, typeInstantiationContext, methodInstantiationContext);
				}
				fieldHandle = this.GetModuleHandle().ResolveFieldHandle(metadataToken, typeInstantiationContext, methodInstantiationContext);
				Type type = fieldHandle.GetApproxDeclaringType().GetRuntimeType();
				if (type.IsGenericType || type.IsArray)
				{
					int parentToken = this.GetModuleHandle().GetMetadataImport().GetParentToken(metadataToken);
					type = this.ResolveType(parentToken, genericTypeArguments, genericMethodArguments);
				}
				result = RuntimeType.GetFieldInfo(type.GetTypeHandleInternal(), fieldHandle);
			}
			catch (MissingFieldException)
			{
				result = this.ResolveLiteralField(metadataToken2, genericTypeArguments, genericMethodArguments);
			}
			catch (BadImageFormatException innerException)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadImageFormatExceptionResolve"), innerException);
			}
			return result;
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0004EC54 File Offset: 0x0004DC54
		public Type ResolveType(int metadataToken)
		{
			return this.ResolveType(metadataToken, null, null);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0004EC60 File Offset: 0x0004DC60
		public Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (metadataToken2.IsGlobalTypeDefToken)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveModuleType"), new object[]
				{
					metadataToken2
				}), "metadataToken");
			}
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			if (!metadataToken2.IsTypeDef && !metadataToken2.IsTypeSpec && !metadataToken2.IsTypeRef)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveType"), new object[]
				{
					metadataToken2,
					this
				}), "metadataToken");
			}
			RuntimeTypeHandle[] typeInstantiationContext = Module.ConvertToTypeHandleArray(genericTypeArguments);
			RuntimeTypeHandle[] methodInstantiationContext = Module.ConvertToTypeHandleArray(genericMethodArguments);
			Type result;
			try
			{
				Type runtimeType = this.GetModuleHandle().ResolveTypeHandle(metadataToken, typeInstantiationContext, methodInstantiationContext).GetRuntimeType();
				if (runtimeType == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveType"), new object[]
					{
						metadataToken2,
						this
					}), "metadataToken");
				}
				result = runtimeType;
			}
			catch (BadImageFormatException innerException)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadImageFormatExceptionResolve"), innerException);
			}
			return result;
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0004EDEC File Offset: 0x0004DDEC
		public MemberInfo ResolveMember(int metadataToken)
		{
			return this.ResolveMember(metadataToken, null, null);
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0004EDF8 File Offset: 0x0004DDF8
		public unsafe MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (metadataToken2.IsProperty)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_PropertyInfoNotAvailable"));
			}
			if (metadataToken2.IsEvent)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_EventInfoNotAvailable"));
			}
			if (metadataToken2.IsMethodSpec || metadataToken2.IsMethodDef)
			{
				return this.ResolveMethod(metadataToken, genericTypeArguments, genericMethodArguments);
			}
			if (metadataToken2.IsFieldDef)
			{
				return this.ResolveField(metadataToken, genericTypeArguments, genericMethodArguments);
			}
			if (metadataToken2.IsTypeRef || metadataToken2.IsTypeDef || metadataToken2.IsTypeSpec)
			{
				return this.ResolveType(metadataToken, genericTypeArguments, genericMethodArguments);
			}
			if (!metadataToken2.IsMemberRef)
			{
				throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMember", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			if (*(byte*)this.MetadataImport.GetMemberRefProps(metadataToken2).Signature.ToPointer() == 6)
			{
				return this.ResolveField(metadataToken2, genericTypeArguments, genericMethodArguments);
			}
			return this.ResolveMethod(metadataToken2, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0004EF74 File Offset: 0x0004DF74
		public string ResolveString(int metadataToken)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!metadataToken2.IsString)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_ResolveString"), new object[]
				{
					metadataToken,
					this.ToString()
				}));
			}
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_InvalidToken", new object[]
				{
					metadataToken2,
					this
				}), new object[0]));
			}
			string userString = this.MetadataImport.GetUserString(metadataToken);
			if (userString == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_ResolveString"), new object[]
				{
					metadataToken,
					this.ToString()
				}));
			}
			return userString;
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0004F068 File Offset: 0x0004E068
		public void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
		{
			this.GetModuleHandle().GetPEKind(out peKind, out machine);
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x0004F088 File Offset: 0x0004E088
		public int MDStreamVersion
		{
			get
			{
				return this.GetModuleHandle().MDStreamVersion;
			}
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0004F0A4 File Offset: 0x0004E0A4
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (!(o is Module))
			{
				return false;
			}
			Module module = o as Module;
			module = module.InternalModule;
			return this.InternalModule == module;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0004F0D7 File Offset: 0x0004E0D7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x0004F0DF File Offset: 0x0004E0DF
		internal virtual Module InternalModule
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x0004F0E2 File Offset: 0x0004E0E2
		// (set) Token: 0x06001F59 RID: 8025 RVA: 0x0004F0EF File Offset: 0x0004E0EF
		internal ArrayList m_TypeBuilderList
		{
			get
			{
				return this.InternalModule.m__TypeBuilderList;
			}
			set
			{
				this.InternalModule.m__TypeBuilderList = value;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x0004F0FD File Offset: 0x0004E0FD
		// (set) Token: 0x06001F5B RID: 8027 RVA: 0x0004F10A File Offset: 0x0004E10A
		internal ISymbolWriter m_iSymWriter
		{
			get
			{
				return this.InternalModule.m__iSymWriter;
			}
			set
			{
				this.InternalModule.m__iSymWriter = value;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x0004F118 File Offset: 0x0004E118
		// (set) Token: 0x06001F5D RID: 8029 RVA: 0x0004F125 File Offset: 0x0004E125
		internal ModuleBuilderData m_moduleData
		{
			get
			{
				return this.InternalModule.m__moduleData;
			}
			set
			{
				this.InternalModule.m__moduleData = value;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001F5E RID: 8030 RVA: 0x0004F133 File Offset: 0x0004E133
		// (set) Token: 0x06001F5F RID: 8031 RVA: 0x0004F140 File Offset: 0x0004E140
		private RuntimeType m_runtimeType
		{
			get
			{
				return this.InternalModule.m__runtimeType;
			}
			set
			{
				this.InternalModule.m__runtimeType = value;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x0004F14E File Offset: 0x0004E14E
		private IntPtr m_pRefClass
		{
			get
			{
				return this.InternalModule.m__pRefClass;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001F61 RID: 8033 RVA: 0x0004F15B File Offset: 0x0004E15B
		internal IntPtr m_pData
		{
			get
			{
				return this.InternalModule.m__pData;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x0004F168 File Offset: 0x0004E168
		internal IntPtr m_pInternalSymWriter
		{
			get
			{
				return this.InternalModule.m__pInternalSymWriter;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001F63 RID: 8035 RVA: 0x0004F175 File Offset: 0x0004E175
		private IntPtr m_pGlobals
		{
			get
			{
				return this.InternalModule.m__pGlobals;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x0004F182 File Offset: 0x0004E182
		private IntPtr m_pFields
		{
			get
			{
				return this.InternalModule.m__pFields;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001F65 RID: 8037 RVA: 0x0004F18F File Offset: 0x0004E18F
		// (set) Token: 0x06001F66 RID: 8038 RVA: 0x0004F19C File Offset: 0x0004E19C
		internal MethodToken m_EntryPoint
		{
			get
			{
				return this.InternalModule.m__EntryPoint;
			}
			set
			{
				this.InternalModule.m__EntryPoint = value;
			}
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0004F1AA File Offset: 0x0004E1AA
		internal Module()
		{
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0004F1B2 File Offset: 0x0004E1B2
		private FieldInfo InternalGetField(string name, BindingFlags bindingAttr)
		{
			if (this.RuntimeType == null)
			{
				return null;
			}
			return this.RuntimeType.GetField(name, bindingAttr);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0004F1CB File Offset: 0x0004E1CB
		internal virtual bool IsDynamic()
		{
			return false;
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x0004F1D0 File Offset: 0x0004E1D0
		internal RuntimeType RuntimeType
		{
			get
			{
				if (this.m_runtimeType == null)
				{
					this.m_runtimeType = this.GetModuleHandle().GetModuleTypeHandle().GetRuntimeType();
				}
				return this.m_runtimeType;
			}
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0004F207 File Offset: 0x0004E207
		protected virtual MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.RuntimeType == null)
			{
				return null;
			}
			if (types == null)
			{
				return this.RuntimeType.GetMethod(name, bindingAttr);
			}
			return this.RuntimeType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x0004F23C File Offset: 0x0004E23C
		internal MetadataImport MetadataImport
		{
			get
			{
				return this.ModuleHandle.GetMetadataImport();
			}
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0004F257 File Offset: 0x0004E257
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0004F270 File Offset: 0x0004E270
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0004F2B8 File Offset: 0x0004E2B8
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "caType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0004F2FE File Offset: 0x0004E2FE
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, 5, this.ScopeName, this.GetAssemblyInternal());
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0004F321 File Offset: 0x0004E321
		[ComVisible(true)]
		public virtual Type GetType(string className, bool ignoreCase)
		{
			return this.GetType(className, false, ignoreCase);
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0004F32C File Offset: 0x0004E32C
		[ComVisible(true)]
		public virtual Type GetType(string className)
		{
			return this.GetType(className, false, false);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0004F337 File Offset: 0x0004E337
		[ComVisible(true)]
		public virtual Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			return this.GetTypeInternal(className, throwOnError, ignoreCase);
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x0004F344 File Offset: 0x0004E344
		public virtual string FullyQualifiedName
		{
			get
			{
				string text = this.InternalGetFullyQualifiedName();
				if (text != null)
				{
					bool flag = true;
					try
					{
						Path.GetFullPathInternal(text);
					}
					catch (ArgumentException)
					{
						flag = false;
					}
					if (flag)
					{
						new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
					}
				}
				return text;
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0004F38C File Offset: 0x0004E38C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual Type[] FindTypes(TypeFilter filter, object filterCriteria)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Type[] typesInternal = this.GetTypesInternal(ref stackCrawlMark);
			int num = 0;
			for (int i = 0; i < typesInternal.Length; i++)
			{
				if (filter != null && !filter(typesInternal[i], filterCriteria))
				{
					typesInternal[i] = null;
				}
				else
				{
					num++;
				}
			}
			if (num == typesInternal.Length)
			{
				return typesInternal;
			}
			Type[] array = new Type[num];
			num = 0;
			for (int j = 0; j < typesInternal.Length; j++)
			{
				if (typesInternal[j] != null)
				{
					array[num++] = typesInternal[j];
				}
			}
			return array;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0004F408 File Offset: 0x0004E408
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual Type[] GetTypes()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.GetTypesInternal(ref stackCrawlMark);
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001F77 RID: 8055 RVA: 0x0004F420 File Offset: 0x0004E420
		public Guid ModuleVersionId
		{
			get
			{
				Guid result;
				this.MetadataImport.GetScopeProps(out result);
				return result;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001F78 RID: 8056 RVA: 0x0004F440 File Offset: 0x0004E440
		public int MetadataToken
		{
			get
			{
				return this.GetModuleHandle().GetToken();
			}
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0004F45B File Offset: 0x0004E45B
		public bool IsResource()
		{
			return this.IsResourceInternal();
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0004F463 File Offset: 0x0004E463
		public FieldInfo[] GetFields()
		{
			if (this.RuntimeType == null)
			{
				return new FieldInfo[0];
			}
			return this.RuntimeType.GetFields();
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0004F47F File Offset: 0x0004E47F
		public FieldInfo[] GetFields(BindingFlags bindingFlags)
		{
			if (this.RuntimeType == null)
			{
				return new FieldInfo[0];
			}
			return this.RuntimeType.GetFields(bindingFlags);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0004F49C File Offset: 0x0004E49C
		public FieldInfo GetField(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0004F4B5 File Offset: 0x0004E4B5
		public FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.InternalGetField(name, bindingAttr);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0004F4CD File Offset: 0x0004E4CD
		public MethodInfo[] GetMethods()
		{
			if (this.RuntimeType == null)
			{
				return new MethodInfo[0];
			}
			return this.RuntimeType.GetMethods();
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0004F4E9 File Offset: 0x0004E4E9
		public MethodInfo[] GetMethods(BindingFlags bindingFlags)
		{
			if (this.RuntimeType == null)
			{
				return new MethodInfo[0];
			}
			return this.RuntimeType.GetMethods(bindingFlags);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0004F508 File Offset: 0x0004E508
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0004F564 File Offset: 0x0004E564
		public MethodInfo GetMethod(string name, Type[] types)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, types, null);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0004F5B8 File Offset: 0x0004E5B8
		public MethodInfo GetMethod(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, null, null);
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x0004F5D5 File Offset: 0x0004E5D5
		public string ScopeName
		{
			get
			{
				return this.InternalGetName();
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001F84 RID: 8068 RVA: 0x0004F5E0 File Offset: 0x0004E5E0
		public string Name
		{
			get
			{
				string text = this.InternalGetFullyQualifiedName();
				int num = text.LastIndexOf('\\');
				if (num == -1)
				{
					return text;
				}
				return new string(text.ToCharArray(), num + 1, text.Length - num - 1);
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0004F61B File Offset: 0x0004E61B
		public override string ToString()
		{
			return this.ScopeName;
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x0004F623 File Offset: 0x0004E623
		public Assembly Assembly
		{
			get
			{
				return this.GetAssemblyInternal();
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x0004F62B File Offset: 0x0004E62B
		public unsafe ModuleHandle ModuleHandle
		{
			get
			{
				return new ModuleHandle((void*)this.m_pData);
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x0004F63D File Offset: 0x0004E63D
		internal unsafe ModuleHandle GetModuleHandle()
		{
			return new ModuleHandle((void*)this.m_pData);
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x0004F64F File Offset: 0x0004E64F
		public X509Certificate GetSignerCertificate()
		{
			return this.GetSignerCertificateInternal();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0004F657 File Offset: 0x0004E657
		void _Module.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0004F65E File Offset: 0x0004E65E
		void _Module.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0004F665 File Offset: 0x0004E665
		void _Module.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0004F66C File Offset: 0x0004E66C
		void _Module.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000D77 RID: 3447
		private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04000D78 RID: 3448
		public static readonly TypeFilter FilterTypeName;

		// Token: 0x04000D79 RID: 3449
		public static readonly TypeFilter FilterTypeNameIgnoreCase;

		// Token: 0x04000D7A RID: 3450
		internal ArrayList m__TypeBuilderList;

		// Token: 0x04000D7B RID: 3451
		internal ISymbolWriter m__iSymWriter;

		// Token: 0x04000D7C RID: 3452
		internal ModuleBuilderData m__moduleData;

		// Token: 0x04000D7D RID: 3453
		private RuntimeType m__runtimeType;

		// Token: 0x04000D7E RID: 3454
		private IntPtr m__pRefClass;

		// Token: 0x04000D7F RID: 3455
		internal IntPtr m__pData;

		// Token: 0x04000D80 RID: 3456
		internal IntPtr m__pInternalSymWriter;

		// Token: 0x04000D81 RID: 3457
		private IntPtr m__pGlobals;

		// Token: 0x04000D82 RID: 3458
		private IntPtr m__pFields;

		// Token: 0x04000D83 RID: 3459
		internal MethodToken m__EntryPoint;
	}
}
