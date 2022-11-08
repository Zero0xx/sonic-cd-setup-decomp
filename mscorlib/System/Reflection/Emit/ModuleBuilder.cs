using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x0200083A RID: 2106
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ModuleBuilder))]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class ModuleBuilder : Module, _ModuleBuilder
	{
		// Token: 0x06004B68 RID: 19304 RVA: 0x001059D0 File Offset: 0x001049D0
		internal static string UnmangleTypeName(string typeName)
		{
			int num = typeName.Length - 1;
			for (;;)
			{
				num = typeName.LastIndexOf('+', num);
				if (num == -1)
				{
					break;
				}
				bool flag = true;
				int num2 = num;
				while (typeName[--num2] == '\\')
				{
					flag = !flag;
				}
				if (flag)
				{
					break;
				}
				num = num2;
			}
			return typeName.Substring(num + 1);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x00105A20 File Offset: 0x00104A20
		internal static Module GetModuleBuilder(Module module)
		{
			ModuleBuilder moduleBuilder = module.InternalModule as ModuleBuilder;
			if (moduleBuilder == null)
			{
				return module;
			}
			ModuleBuilder moduleBuilder2 = null;
			Module result;
			lock (ModuleBuilder.s_moduleBuilders)
			{
				if (ModuleBuilder.s_moduleBuilders.TryGetValue(moduleBuilder, out moduleBuilder2))
				{
					result = moduleBuilder2;
				}
				else
				{
					result = moduleBuilder;
				}
			}
			return result;
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x00105A7C File Offset: 0x00104A7C
		internal ModuleBuilder(AssemblyBuilder assemblyBuilder, ModuleBuilder internalModuleBuilder)
		{
			this.m_internalModuleBuilder = internalModuleBuilder;
			this.m_assemblyBuilder = assemblyBuilder;
			lock (ModuleBuilder.s_moduleBuilders)
			{
				ModuleBuilder.s_moduleBuilders[internalModuleBuilder] = this;
			}
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x00105AD0 File Offset: 0x00104AD0
		private Type GetType(string strFormat, Type baseType)
		{
			if (strFormat == null || strFormat.Equals(string.Empty))
			{
				return baseType;
			}
			char[] bFormat = strFormat.ToCharArray();
			return SymbolType.FormCompoundType(bFormat, baseType, 0);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x00105AFE File Offset: 0x00104AFE
		internal void CheckContext(params Type[][] typess)
		{
			((AssemblyBuilder)base.Assembly).CheckContext(typess);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x00105B11 File Offset: 0x00104B11
		internal void CheckContext(params Type[] types)
		{
			((AssemblyBuilder)base.Assembly).CheckContext(types);
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004B6E RID: 19310 RVA: 0x00105B24 File Offset: 0x00104B24
		private bool IsInternal
		{
			get
			{
				return this.m_internalModuleBuilder == null;
			}
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x00105B30 File Offset: 0x00104B30
		private void DemandGrantedAssemblyPermission()
		{
			AssemblyBuilder assemblyBuilder = (AssemblyBuilder)base.Assembly;
			assemblyBuilder.DemandGrantedPermission();
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x00105B50 File Offset: 0x00104B50
		internal virtual Type FindTypeBuilderWithName(string strTypeName, bool ignoreCase)
		{
			int count = base.m_TypeBuilderList.Count;
			Type type = null;
			int i;
			for (i = 0; i < count; i++)
			{
				type = (Type)base.m_TypeBuilderList[i];
				if (ignoreCase)
				{
					if (string.Compare(type.FullName, strTypeName, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
					{
						break;
					}
				}
				else if (type.FullName.Equals(strTypeName))
				{
					break;
				}
			}
			if (i == count)
			{
				type = null;
			}
			return type;
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x00105BB8 File Offset: 0x00104BB8
		internal Type GetRootElementType(Type type)
		{
			if (!type.IsByRef && !type.IsPointer && !type.IsArray)
			{
				return type;
			}
			return this.GetRootElementType(type.GetElementType());
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x00105BE0 File Offset: 0x00104BE0
		internal void SetEntryPoint(MethodInfo entryPoint)
		{
			base.m_EntryPoint = this.GetMethodTokenInternal(entryPoint);
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x00105BF0 File Offset: 0x00104BF0
		internal void PreSave(string fileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.PreSaveNoLock(fileName, portableExecutableKind, imageFileMachine);
					return;
				}
			}
			this.PreSaveNoLock(fileName, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x00105C4C File Offset: 0x00104C4C
		private void PreSaveNoLock(string fileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.m_moduleData.m_isSaved)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("InvalidOperation_ModuleHasBeenSaved"), new object[]
				{
					base.m_moduleData.m_strModuleName
				}));
			}
			if (!base.m_moduleData.m_fGlobalBeenCreated && base.m_moduleData.m_fHasGlobal)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GlobalFunctionNotBaked"));
			}
			int count = base.m_TypeBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				object obj = base.m_TypeBuilderList[i];
				TypeBuilder typeBuilder;
				if (obj is TypeBuilder)
				{
					typeBuilder = (TypeBuilder)obj;
				}
				else
				{
					EnumBuilder enumBuilder = (EnumBuilder)obj;
					typeBuilder = enumBuilder.m_typeBuilder;
				}
				if (!typeBuilder.m_hasBeenCreated && !typeBuilder.m_isHiddenType)
				{
					throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("NotSupported_NotAllTypesAreBaked"), new object[]
					{
						typeBuilder.FullName
					}));
				}
			}
			base.InternalPreSavePEFile((int)portableExecutableKind, (int)imageFileMachine);
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00105D50 File Offset: 0x00104D50
		internal void Save(string fileName, bool isAssemblyFile, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.SaveNoLock(fileName, isAssemblyFile, portableExecutableKind, imageFileMachine);
					return;
				}
			}
			this.SaveNoLock(fileName, isAssemblyFile, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x00105DB0 File Offset: 0x00104DB0
		private void SaveNoLock(string fileName, bool isAssemblyFile, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.m_moduleData.m_embeddedRes != null)
			{
				ResWriterData resWriterData = base.m_moduleData.m_embeddedRes;
				int num = 0;
				while (resWriterData != null)
				{
					resWriterData = resWriterData.m_nextResWriter;
					num++;
				}
				base.InternalSetResourceCounts(num);
				for (resWriterData = base.m_moduleData.m_embeddedRes; resWriterData != null; resWriterData = resWriterData.m_nextResWriter)
				{
					if (resWriterData.m_resWriter != null)
					{
						resWriterData.m_resWriter.Generate();
					}
					byte[] array = new byte[resWriterData.m_memoryStream.Length];
					resWriterData.m_memoryStream.Flush();
					resWriterData.m_memoryStream.Position = 0L;
					resWriterData.m_memoryStream.Read(array, 0, array.Length);
					base.InternalAddResource(resWriterData.m_strName, array, array.Length, base.m_moduleData.m_tkFile, (int)resWriterData.m_attribute, (int)portableExecutableKind, (int)imageFileMachine);
				}
			}
			if (base.m_moduleData.m_strResourceFileName != null)
			{
				base.InternalDefineNativeResourceFile(base.m_moduleData.m_strResourceFileName, (int)portableExecutableKind, (int)imageFileMachine);
			}
			else if (base.m_moduleData.m_resourceBytes != null)
			{
				base.InternalDefineNativeResourceBytes(base.m_moduleData.m_resourceBytes, (int)portableExecutableKind, (int)imageFileMachine);
			}
			if (isAssemblyFile)
			{
				base.InternalSavePEFile(fileName, base.m_EntryPoint, (int)base.Assembly.m_assemblyData.m_peFileKind, true);
			}
			else
			{
				base.InternalSavePEFile(fileName, base.m_EntryPoint, 1, false);
			}
			base.m_moduleData.m_isSaved = true;
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x00105F00 File Offset: 0x00104F00
		internal int GetTypeRefNested(Type type, Module refedModule, string strRefedModuleFileName)
		{
			Type declaringType = type.DeclaringType;
			int tkResolution = 0;
			string text = type.FullName;
			if (declaringType != null)
			{
				tkResolution = this.GetTypeRefNested(declaringType, refedModule, strRefedModuleFileName);
				text = ModuleBuilder.UnmangleTypeName(text);
			}
			return base.InternalGetTypeToken(text, refedModule, strRefedModuleFileName, tkResolution);
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x00105F3C File Offset: 0x00104F3C
		internal MethodToken InternalGetConstructorToken(ConstructorInfo con, bool usingRef)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			int str;
			if (con is ConstructorBuilder)
			{
				ConstructorBuilder constructorBuilder = con as ConstructorBuilder;
				if (!usingRef && constructorBuilder.ReflectedType.Module.InternalModule.Equals(this.InternalModule))
				{
					return constructorBuilder.GetToken();
				}
				int token = this.GetTypeTokenInternal(con.ReflectedType).Token;
				str = base.InternalGetMemberRef(con.ReflectedType.Module, token, constructorBuilder.GetToken().Token);
			}
			else if (con is ConstructorOnTypeBuilderInstantiation)
			{
				ConstructorOnTypeBuilderInstantiation constructorOnTypeBuilderInstantiation = con as ConstructorOnTypeBuilderInstantiation;
				if (usingRef)
				{
					throw new InvalidOperationException();
				}
				int token = this.GetTypeTokenInternal(con.DeclaringType).Token;
				str = base.InternalGetMemberRef(con.DeclaringType.Module, token, constructorOnTypeBuilderInstantiation.m_ctor.MetadataTokenInternal);
			}
			else if (con is RuntimeConstructorInfo && !con.ReflectedType.IsArray)
			{
				int token = this.GetTypeTokenInternal(con.ReflectedType).Token;
				str = base.InternalGetMemberRefOfMethodInfo(token, con.GetMethodHandle());
			}
			else
			{
				ParameterInfo[] parameters = con.GetParameters();
				Type[] array = new Type[parameters.Length];
				Type[][] array2 = new Type[array.Length][];
				Type[][] array3 = new Type[array.Length][];
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i] = parameters[i].ParameterType;
					array2[i] = parameters[i].GetRequiredCustomModifiers();
					array3[i] = parameters[i].GetOptionalCustomModifiers();
				}
				int token = this.GetTypeTokenInternal(con.ReflectedType).Token;
				SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this, con.CallingConvention, null, null, null, array, array2, array3);
				int length;
				byte[] signature = methodSigHelper.InternalGetSignature(out length);
				str = base.InternalGetMemberRefFromSignature(token, con.Name, signature, length);
			}
			return new MethodToken(str);
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x00106114 File Offset: 0x00105114
		internal void Init(string strModuleName, string strFileName, ISymbolWriter writer)
		{
			base.m_moduleData = new ModuleBuilderData(this, strModuleName, strFileName);
			base.m_TypeBuilderList = new ArrayList();
			base.m_iSymWriter = writer;
			if (writer != null)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				writer.SetUnderlyingWriter(base.m_pInternalSymWriter);
			}
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x00106150 File Offset: 0x00105150
		internal int GetMemberRefToken(MethodBase method, Type[] optionalParameterTypes)
		{
			int cGenericParameters = 0;
			if (method.IsGenericMethod)
			{
				if (!method.IsGenericMethodDefinition)
				{
					throw new InvalidOperationException();
				}
				cGenericParameters = method.GetGenericArguments().Length;
			}
			if (optionalParameterTypes != null && (method.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
			}
			MethodInfo methodInfo = method as MethodInfo;
			Type[] parameterTypes;
			Type returnType;
			if (method.DeclaringType.IsGenericType)
			{
				MethodOnTypeBuilderInstantiation methodOnTypeBuilderInstantiation;
				MethodBase methodBase;
				ConstructorOnTypeBuilderInstantiation constructorOnTypeBuilderInstantiation;
				if ((methodOnTypeBuilderInstantiation = (method as MethodOnTypeBuilderInstantiation)) != null)
				{
					methodBase = methodOnTypeBuilderInstantiation.m_method;
				}
				else if ((constructorOnTypeBuilderInstantiation = (method as ConstructorOnTypeBuilderInstantiation)) != null)
				{
					methodBase = constructorOnTypeBuilderInstantiation.m_ctor;
				}
				else if (method is MethodBuilder || method is ConstructorBuilder)
				{
					methodBase = method;
				}
				else if (method.IsGenericMethod)
				{
					methodBase = methodInfo.GetGenericMethodDefinition();
					methodBase = methodBase.Module.ResolveMethod(methodBase.MetadataTokenInternal, methodBase.GetGenericArguments(), (methodBase.DeclaringType != null) ? methodBase.DeclaringType.GetGenericArguments() : null);
				}
				else
				{
					methodBase = method.Module.ResolveMethod(method.MetadataTokenInternal, null, (method.DeclaringType != null) ? method.DeclaringType.GetGenericArguments() : null);
				}
				parameterTypes = methodBase.GetParameterTypes();
				returnType = methodBase.GetReturnType();
			}
			else
			{
				parameterTypes = method.GetParameterTypes();
				returnType = method.GetReturnType();
			}
			int tr;
			if (method.DeclaringType.IsGenericType)
			{
				int length;
				byte[] signature = SignatureHelper.GetTypeSigToken(this, method.DeclaringType).InternalGetSignature(out length);
				tr = base.InternalGetTypeSpecTokenWithBytes(signature, length);
			}
			else if (method.Module.InternalModule != this.InternalModule)
			{
				tr = this.GetTypeToken(method.DeclaringType).Token;
			}
			else if (methodInfo != null)
			{
				tr = this.GetMethodToken(method as MethodInfo).Token;
			}
			else
			{
				tr = this.GetConstructorToken(method as ConstructorInfo).Token;
			}
			int length2;
			byte[] signature2 = this.GetMemberRefSignature(method.CallingConvention, returnType, parameterTypes, optionalParameterTypes, cGenericParameters).InternalGetSignature(out length2);
			return base.InternalGetMemberRefFromSignature(tr, method.Name, signature2, length2);
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x0010634C File Offset: 0x0010534C
		internal SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, int cGenericParameters)
		{
			int num;
			if (parameterTypes == null)
			{
				num = 0;
			}
			else
			{
				num = parameterTypes.Length;
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this, call, returnType, cGenericParameters);
			for (int i = 0; i < num; i++)
			{
				methodSigHelper.AddArgument(parameterTypes[i]);
			}
			if (optionalParameterTypes != null && optionalParameterTypes.Length != 0)
			{
				methodSigHelper.AddSentinel();
				for (int i = 0; i < optionalParameterTypes.Length; i++)
				{
					methodSigHelper.AddArgument(optionalParameterTypes[i]);
				}
			}
			return methodSigHelper;
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x001063AE File Offset: 0x001053AE
		internal override bool IsDynamic()
		{
			return true;
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06004B7D RID: 19325 RVA: 0x001063B1 File Offset: 0x001053B1
		internal override Module InternalModule
		{
			get
			{
				if (this.IsInternal)
				{
					return this;
				}
				return this.m_internalModuleBuilder;
			}
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x001063C3 File Offset: 0x001053C3
		internal override Assembly GetAssemblyInternal()
		{
			if (!this.IsInternal)
			{
				return this.m_assemblyBuilder;
			}
			return base._GetAssemblyInternal();
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x001063DC File Offset: 0x001053DC
		public override Type[] GetTypes()
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetTypesNoLock();
				}
			}
			return this.GetTypesNoLock();
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x00106438 File Offset: 0x00105438
		internal Type[] GetTypesNoLock()
		{
			int count = base.m_TypeBuilderList.Count;
			List<Type> list = new List<Type>(count);
			bool flag = false;
			if (this.IsInternal)
			{
				try
				{
					this.DemandGrantedAssemblyPermission();
					flag = true;
					goto IL_2E;
				}
				catch (SecurityException)
				{
					flag = false;
					goto IL_2E;
				}
			}
			flag = true;
			IL_2E:
			for (int i = 0; i < count; i++)
			{
				EnumBuilder enumBuilder = base.m_TypeBuilderList[i] as EnumBuilder;
				TypeBuilder typeBuilder;
				if (enumBuilder != null)
				{
					typeBuilder = enumBuilder.m_typeBuilder;
				}
				else
				{
					typeBuilder = (base.m_TypeBuilderList[i] as TypeBuilder);
				}
				if (typeBuilder != null)
				{
					if (typeBuilder.m_hasBeenCreated)
					{
						list.Add(typeBuilder.UnderlyingSystemType);
					}
					else if (flag)
					{
						list.Add(typeBuilder);
					}
				}
				else
				{
					list.Add((Type)base.m_TypeBuilderList[i]);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x0010650C File Offset: 0x0010550C
		[ComVisible(true)]
		public override Type GetType(string className)
		{
			return this.GetType(className, false, false);
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x00106517 File Offset: 0x00105517
		[ComVisible(true)]
		public override Type GetType(string className, bool ignoreCase)
		{
			return this.GetType(className, false, ignoreCase);
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x00106524 File Offset: 0x00105524
		[ComVisible(true)]
		public override Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetTypeNoLock(className, throwOnError, ignoreCase);
				}
			}
			return this.GetTypeNoLock(className, throwOnError, ignoreCase);
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x00106590 File Offset: 0x00105590
		private Type GetTypeNoLock(string className, bool throwOnError, bool ignoreCase)
		{
			Type type = base.GetType(className, throwOnError, ignoreCase);
			if (type != null)
			{
				return type;
			}
			string text = null;
			string text2 = null;
			int num;
			for (int i = 0; i <= className.Length; i = num + 1)
			{
				num = className.IndexOfAny(new char[]
				{
					'[',
					'*',
					'&'
				}, i);
				if (num == -1)
				{
					text = className;
					text2 = null;
					break;
				}
				int num2 = 0;
				int num3 = num - 1;
				while (num3 >= 0 && className[num3] == '\\')
				{
					num2++;
					num3--;
				}
				if (num2 % 2 != 1)
				{
					text = className.Substring(0, num);
					text2 = className.Substring(num);
					break;
				}
			}
			if (text == null)
			{
				text = className;
				text2 = null;
			}
			text = text.Replace("\\\\", "\\").Replace("\\[", "[").Replace("\\*", "*").Replace("\\&", "&");
			if (text2 != null)
			{
				type = base.GetType(text, false, ignoreCase);
			}
			bool flag = false;
			if (this.IsInternal)
			{
				try
				{
					this.DemandGrantedAssemblyPermission();
					flag = true;
					goto IL_101;
				}
				catch (SecurityException)
				{
					flag = false;
					goto IL_101;
				}
			}
			flag = true;
			IL_101:
			if (type == null && flag)
			{
				type = this.FindTypeBuilderWithName(text, ignoreCase);
				if (type == null && base.Assembly is AssemblyBuilder)
				{
					ArrayList moduleBuilderList = base.Assembly.m_assemblyData.m_moduleBuilderList;
					int count = moduleBuilderList.Count;
					int num4 = 0;
					while (num4 < count && type == null)
					{
						ModuleBuilder moduleBuilder = (ModuleBuilder)moduleBuilderList[num4];
						type = moduleBuilder.FindTypeBuilderWithName(text, ignoreCase);
						num4++;
					}
				}
			}
			if (type == null)
			{
				return null;
			}
			if (text2 == null)
			{
				return type;
			}
			return this.GetType(text2, type);
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06004B85 RID: 19333 RVA: 0x0010672C File Offset: 0x0010572C
		public override string FullyQualifiedName
		{
			get
			{
				string text = base.m_moduleData.m_strFileName;
				if (text == null)
				{
					return null;
				}
				if (base.Assembly.m_assemblyData.m_strDir != null)
				{
					text = Path.Combine(base.Assembly.m_assemblyData.m_strDir, text);
					text = Path.GetFullPath(text);
				}
				if (base.Assembly.m_assemblyData.m_strDir != null && text != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				}
				return text;
			}
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x0010679C File Offset: 0x0010579C
		public TypeBuilder DefineType(string name)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name);
				}
			}
			return this.DefineTypeNoLock(name);
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x00106808 File Offset: 0x00105808
		private TypeBuilder DefineTypeNoLock(string name)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, TypeAttributes.NotPublic, null, null, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x00106830 File Offset: 0x00105830
		public TypeBuilder DefineType(string name, TypeAttributes attr)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr);
				}
			}
			return this.DefineTypeNoLock(name, attr);
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x0010689C File Offset: 0x0010589C
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, null, null, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x001068C4 File Offset: 0x001058C4
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent);
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x00106934 File Offset: 0x00105934
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent)
		{
			this.CheckContext(new Type[]
			{
				parent
			});
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x00106970 File Offset: 0x00105970
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, int typesize)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, typesize);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, typesize);
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x001069E4 File Offset: 0x001059E4
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, int typesize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, this, PackingSize.Unspecified, typesize, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x00106A10 File Offset: 0x00105A10
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, packingSize, typesize);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, packingSize, typesize);
		}

		// Token: 0x06004B8F RID: 19343 RVA: 0x00106A88 File Offset: 0x00105A88
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, this, packingSize, typesize, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B90 RID: 19344 RVA: 0x00106AB4 File Offset: 0x00105AB4
		[ComVisible(true)]
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, interfaces);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, interfaces);
		}

		// Token: 0x06004B91 RID: 19345 RVA: 0x00106B28 File Offset: 0x00105B28
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, interfaces, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B92 RID: 19346 RVA: 0x00106B54 File Offset: 0x00105B54
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packsize)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, packsize);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, packsize);
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x00106BC8 File Offset: 0x00105BC8
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packsize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this, packsize, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x00106BF4 File Offset: 0x00105BF4
		public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			this.CheckContext(new Type[]
			{
				underlyingType
			});
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineEnumNoLock(name, visibility, underlyingType);
				}
			}
			return this.DefineEnumNoLock(name, visibility, underlyingType);
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x00106C74 File Offset: 0x00105C74
		private EnumBuilder DefineEnumNoLock(string name, TypeAttributes visibility, Type underlyingType)
		{
			EnumBuilder enumBuilder = new EnumBuilder(name, underlyingType, visibility, this);
			base.m_TypeBuilderList.Add(enumBuilder);
			return enumBuilder;
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x00106C99 File Offset: 0x00105C99
		public IResourceWriter DefineResource(string name, string description)
		{
			return this.DefineResource(name, description, ResourceAttributes.Public);
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x00106CA4 File Offset: 0x00105CA4
		public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineResourceNoLock(name, description, attribute);
				}
			}
			return this.DefineResourceNoLock(name, description, attribute);
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x00106D14 File Offset: 0x00105D14
		private IResourceWriter DefineResourceNoLock(string name, string description, ResourceAttributes attribute)
		{
			if (this.IsTransient())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			Assembly assembly = base.Assembly;
			if (!(assembly is AssemblyBuilder))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			AssemblyBuilder assemblyBuilder = (AssemblyBuilder)assembly;
			if (assemblyBuilder.IsPersistable())
			{
				assemblyBuilder.m_assemblyData.CheckResNameConflict(name);
				MemoryStream memoryStream = new MemoryStream();
				ResourceWriter resourceWriter = new ResourceWriter(memoryStream);
				ResWriterData resWriterData = new ResWriterData(resourceWriter, memoryStream, name, string.Empty, string.Empty, attribute);
				resWriterData.m_nextResWriter = base.m_moduleData.m_embeddedRes;
				base.m_moduleData.m_embeddedRes = resWriterData;
				return resourceWriter;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x00106DF0 File Offset: 0x00105DF0
		public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.DefineManifestResourceNoLock(name, stream, attribute);
					return;
				}
			}
			this.DefineManifestResourceNoLock(name, stream, attribute);
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x00106E78 File Offset: 0x00105E78
		private void DefineManifestResourceNoLock(string name, Stream stream, ResourceAttributes attribute)
		{
			if (this.IsTransient())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			Assembly assembly = base.Assembly;
			if (!(assembly is AssemblyBuilder))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			AssemblyBuilder assemblyBuilder = (AssemblyBuilder)assembly;
			if (assemblyBuilder.IsPersistable())
			{
				assemblyBuilder.m_assemblyData.CheckResNameConflict(name);
				ResWriterData resWriterData = new ResWriterData(null, stream, name, string.Empty, string.Empty, attribute);
				resWriterData.m_nextResWriter = base.m_moduleData.m_embeddedRes;
				base.m_moduleData.m_embeddedRes = resWriterData;
				return;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x00106F44 File Offset: 0x00105F44
		public void DefineUnmanagedResource(byte[] resource)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.DefineUnmanagedResourceInternalNoLock(resource);
					return;
				}
			}
			this.DefineUnmanagedResourceInternalNoLock(resource);
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x00106FAC File Offset: 0x00105FAC
		internal void DefineUnmanagedResourceInternalNoLock(byte[] resource)
		{
			if (base.m_moduleData.m_strResourceFileName != null || base.m_moduleData.m_resourceBytes != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			base.m_moduleData.m_resourceBytes = new byte[resource.Length];
			Array.Copy(resource, base.m_moduleData.m_resourceBytes, resource.Length);
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x00107018 File Offset: 0x00106018
		public void DefineUnmanagedResource(string resourceFileName)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.DefineUnmanagedResourceFileInternalNoLock(resourceFileName);
					return;
				}
			}
			this.DefineUnmanagedResourceFileInternalNoLock(resourceFileName);
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x00107080 File Offset: 0x00106080
		internal void DefineUnmanagedResourceFileInternalNoLock(string resourceFileName)
		{
			if (base.m_moduleData.m_resourceBytes != null || base.m_moduleData.m_strResourceFileName != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			if (resourceFileName == null)
			{
				throw new ArgumentNullException("resourceFileName");
			}
			string fullPath = Path.GetFullPath(resourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
			new EnvironmentPermission(PermissionState.Unrestricted).Assert();
			try
			{
				if (!File.Exists(resourceFileName))
				{
					throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[]
					{
						resourceFileName
					}), resourceFileName);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.m_moduleData.m_strResourceFileName = fullPath;
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x00107138 File Offset: 0x00106138
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineGlobalMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x00107148 File Offset: 0x00106148
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineGlobalMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x00107168 File Offset: 0x00106168
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineGlobalMethodNoLock(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
				}
			}
			return this.DefineGlobalMethodNoLock(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x001071F0 File Offset: 0x001061F0
		private MethodBuilder DefineGlobalMethodNoLock(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			this.CheckContext(new Type[]
			{
				returnType
			});
			this.CheckContext(new Type[][]
			{
				requiredReturnTypeCustomModifiers,
				optionalReturnTypeCustomModifiers,
				parameterTypes
			});
			this.CheckContext(requiredParameterTypeCustomModifiers);
			this.CheckContext(optionalParameterTypeCustomModifiers);
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_GlobalFunctionHasToBeStatic"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefineMethod(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x001072C8 File Offset: 0x001062C8
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x001072EC File Offset: 0x001062EC
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefinePInvokeMethodNoLock(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
				}
			}
			return this.DefinePInvokeMethodNoLock(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x00107374 File Offset: 0x00106374
		private MethodBuilder DefinePInvokeMethodNoLock(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.CheckContext(new Type[]
			{
				returnType
			});
			this.CheckContext(parameterTypes);
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_GlobalFunctionHasToBeStatic"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x001073E0 File Offset: 0x001063E0
		public void CreateGlobalFunctions()
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.CreateGlobalFunctionsNoLock();
					return;
				}
			}
			this.CreateGlobalFunctionsNoLock();
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x00107444 File Offset: 0x00106444
		private void CreateGlobalFunctionsNoLock()
		{
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			base.m_moduleData.m_globalTypeBuilder.CreateType();
			base.m_moduleData.m_fGlobalBeenCreated = true;
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x00107480 File Offset: 0x00106480
		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineInitializedDataNoLock(name, data, attributes);
				}
			}
			return this.DefineInitializedDataNoLock(name, data, attributes);
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x001074F0 File Offset: 0x001064F0
		private FieldBuilder DefineInitializedDataNoLock(string name, byte[] data, FieldAttributes attributes)
		{
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefineInitializedData(name, data, attributes);
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x00107530 File Offset: 0x00106530
		public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineUninitializedDataNoLock(name, size, attributes);
				}
			}
			return this.DefineUninitializedDataNoLock(name, size, attributes);
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x001075A0 File Offset: 0x001065A0
		private FieldBuilder DefineUninitializedDataNoLock(string name, int size, FieldAttributes attributes)
		{
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefineUninitializedData(name, size, attributes);
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x001075DE File Offset: 0x001065DE
		internal TypeToken GetTypeTokenInternal(Type type)
		{
			return this.GetTypeTokenInternal(type, false);
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x001075E8 File Offset: 0x001065E8
		internal TypeToken GetTypeTokenInternal(Type type, bool getGenericDefinition)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetTypeTokenWorkerNoLock(type, getGenericDefinition);
				}
			}
			return this.GetTypeTokenWorkerNoLock(type, getGenericDefinition);
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x00107648 File Offset: 0x00106648
		public TypeToken GetTypeToken(Type type)
		{
			return this.GetTypeTokenInternal(type, true);
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x00107654 File Offset: 0x00106654
		private TypeToken GetTypeTokenWorkerNoLock(Type type, bool getGenericDefinition)
		{
			this.CheckContext(new Type[]
			{
				type
			});
			string strRefedModuleFileName = string.Empty;
			bool flag = false;
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Module moduleBuilder = ModuleBuilder.GetModuleBuilder(type.Module);
			bool flag2 = moduleBuilder.Equals(this);
			if (type.IsByRef)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CannotGetTypeTokenForByRef"));
			}
			if ((type.IsGenericType && (!type.IsGenericTypeDefinition || !getGenericDefinition)) || type.IsGenericParameter || type.IsArray || type.IsPointer)
			{
				int length;
				byte[] signature = SignatureHelper.GetTypeSigToken(this, type).InternalGetSignature(out length);
				return new TypeToken(base.InternalGetTypeSpecTokenWithBytes(signature, length));
			}
			if (flag2)
			{
				EnumBuilder enumBuilder = type as EnumBuilder;
				TypeBuilder typeBuilder;
				if (enumBuilder != null)
				{
					typeBuilder = enumBuilder.m_typeBuilder;
				}
				else
				{
					typeBuilder = (type as TypeBuilder);
				}
				if (typeBuilder != null)
				{
					return typeBuilder.TypeToken;
				}
				if (type is GenericTypeParameterBuilder)
				{
					return new TypeToken(type.MetadataTokenInternal);
				}
				return new TypeToken(this.GetTypeRefNested(type, this, string.Empty));
			}
			else
			{
				ModuleBuilder moduleBuilder2 = moduleBuilder as ModuleBuilder;
				if (moduleBuilder2 != null)
				{
					if (moduleBuilder2.IsTransient())
					{
						flag = true;
					}
					strRefedModuleFileName = moduleBuilder2.m_moduleData.m_strFileName;
				}
				else
				{
					strRefedModuleFileName = moduleBuilder.ScopeName;
				}
				if (!this.IsTransient() && flag)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadTransientModuleReference"));
				}
				TypeToken result = new TypeToken(this.GetTypeRefNested(type, moduleBuilder, strRefedModuleFileName));
				return result;
			}
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x001077B4 File Offset: 0x001067B4
		public TypeToken GetTypeToken(string name)
		{
			return this.GetTypeToken(base.GetType(name, false, true));
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x001077C8 File Offset: 0x001067C8
		public MethodToken GetMethodToken(MethodInfo method)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetMethodTokenNoLock(method, true);
				}
			}
			return this.GetMethodTokenNoLock(method, true);
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x00107828 File Offset: 0x00106828
		internal MethodToken GetMethodTokenInternal(MethodInfo method)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetMethodTokenNoLock(method, false);
				}
			}
			return this.GetMethodTokenNoLock(method, false);
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x00107888 File Offset: 0x00106888
		private MethodToken GetMethodTokenNoLock(MethodInfo method, bool getGenericTypeDefinition)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			int str;
			if (method is MethodBuilder)
			{
				if (method.Module.InternalModule == this.InternalModule)
				{
					return new MethodToken(method.MetadataTokenInternal);
				}
				if (method.DeclaringType == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
				}
				int tr = getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token;
				str = base.InternalGetMemberRef(method.DeclaringType.Module, tr, method.MetadataTokenInternal);
			}
			else
			{
				if (method is MethodOnTypeBuilderInstantiation)
				{
					return new MethodToken(this.GetMemberRefToken(method, null));
				}
				if (method is SymbolMethod)
				{
					SymbolMethod symbolMethod = method as SymbolMethod;
					if (symbolMethod.GetModule() == this)
					{
						return symbolMethod.GetToken();
					}
					return symbolMethod.GetToken(this);
				}
				else
				{
					Type declaringType = method.DeclaringType;
					if (declaringType == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
					}
					if (declaringType.IsArray)
					{
						ParameterInfo[] parameters = method.GetParameters();
						Type[] array = new Type[parameters.Length];
						for (int i = 0; i < parameters.Length; i++)
						{
							array[i] = parameters[i].ParameterType;
						}
						return this.GetArrayMethodToken(declaringType, method.Name, method.CallingConvention, method.ReturnType, array);
					}
					if (method is RuntimeMethodInfo)
					{
						int tr = getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token;
						str = base.InternalGetMemberRefOfMethodInfo(tr, method.GetMethodHandle());
					}
					else
					{
						ParameterInfo[] parameters2 = method.GetParameters();
						Type[] array2 = new Type[parameters2.Length];
						Type[][] array3 = new Type[array2.Length][];
						Type[][] array4 = new Type[array2.Length][];
						for (int j = 0; j < parameters2.Length; j++)
						{
							array2[j] = parameters2[j].ParameterType;
							array3[j] = parameters2[j].GetRequiredCustomModifiers();
							array4[j] = parameters2[j].GetOptionalCustomModifiers();
						}
						int tr = getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token;
						SignatureHelper methodSigHelper;
						try
						{
							methodSigHelper = SignatureHelper.GetMethodSigHelper(this, method.CallingConvention, method.ReturnType, method.ReturnParameter.GetRequiredCustomModifiers(), method.ReturnParameter.GetOptionalCustomModifiers(), array2, array3, array4);
						}
						catch (NotImplementedException)
						{
							methodSigHelper = SignatureHelper.GetMethodSigHelper(this, method.ReturnType, array2);
						}
						int length;
						byte[] signature = methodSigHelper.InternalGetSignature(out length);
						str = base.InternalGetMemberRefFromSignature(tr, method.Name, signature, length);
					}
				}
			}
			return new MethodToken(str);
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x00107B44 File Offset: 0x00106B44
		public MethodToken GetArrayMethodToken(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetArrayMethodTokenNoLock(arrayClass, methodName, callingConvention, returnType, parameterTypes);
				}
			}
			return this.GetArrayMethodTokenNoLock(arrayClass, methodName, callingConvention, returnType, parameterTypes);
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x00107BAC File Offset: 0x00106BAC
		private MethodToken GetArrayMethodTokenNoLock(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			this.CheckContext(new Type[]
			{
				returnType,
				arrayClass
			});
			this.CheckContext(parameterTypes);
			if (arrayClass == null)
			{
				throw new ArgumentNullException("arrayClass");
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			if (methodName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "methodName");
			}
			if (!arrayClass.IsArray)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_HasToBeArrayClass"));
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this, callingConvention, returnType, null, null, parameterTypes, null, null);
			int sigLength;
			byte[] signature = methodSigHelper.InternalGetSignature(out sigLength);
			Type type = arrayClass;
			while (type.IsArray)
			{
				type = type.GetElementType();
			}
			int token = this.GetTypeTokenInternal(type).Token;
			return new MethodToken(base.nativeGetArrayMethodToken(this.GetTypeTokenInternal(arrayClass).Token, methodName, signature, sigLength, token));
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x00107C8C File Offset: 0x00106C8C
		public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			this.CheckContext(new Type[]
			{
				returnType,
				arrayClass
			});
			this.CheckContext(parameterTypes);
			MethodToken arrayMethodToken = this.GetArrayMethodToken(arrayClass, methodName, callingConvention, returnType, parameterTypes);
			return new SymbolMethod(this, arrayMethodToken, arrayClass, methodName, callingConvention, returnType, parameterTypes);
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x00107CD4 File Offset: 0x00106CD4
		[ComVisible(true)]
		public MethodToken GetConstructorToken(ConstructorInfo con)
		{
			return this.InternalGetConstructorToken(con, false);
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x00107CE0 File Offset: 0x00106CE0
		public FieldToken GetFieldToken(FieldInfo field)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetFieldTokenNoLock(field);
				}
			}
			return this.GetFieldTokenNoLock(field);
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x00107D3C File Offset: 0x00106D3C
		private FieldToken GetFieldTokenNoLock(FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("con");
			}
			int field2;
			if (field is FieldBuilder)
			{
				FieldBuilder fieldBuilder = (FieldBuilder)field;
				if (field.DeclaringType != null && field.DeclaringType.IsGenericType)
				{
					int length;
					byte[] signature = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out length);
					int num = base.InternalGetTypeSpecTokenWithBytes(signature, length);
					field2 = base.InternalGetMemberRef(this, num, fieldBuilder.GetToken().Token);
				}
				else
				{
					if (fieldBuilder.GetTypeBuilder().Module.InternalModule.Equals(this.InternalModule))
					{
						return fieldBuilder.GetToken();
					}
					if (field.DeclaringType == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
					}
					int num = this.GetTypeTokenInternal(field.DeclaringType).Token;
					field2 = base.InternalGetMemberRef(field.ReflectedType.Module, num, fieldBuilder.GetToken().Token);
				}
			}
			else if (field is RuntimeFieldInfo)
			{
				if (field.DeclaringType == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
				}
				if (field.DeclaringType != null && field.DeclaringType.IsGenericType)
				{
					int length2;
					byte[] signature2 = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out length2);
					int num = base.InternalGetTypeSpecTokenWithBytes(signature2, length2);
					field2 = base.InternalGetMemberRefOfFieldInfo(num, field.DeclaringType.GetTypeHandleInternal(), field.MetadataTokenInternal);
				}
				else
				{
					int num = this.GetTypeTokenInternal(field.DeclaringType).Token;
					field2 = base.InternalGetMemberRefOfFieldInfo(num, field.DeclaringType.GetTypeHandleInternal(), field.MetadataTokenInternal);
				}
			}
			else if (field is FieldOnTypeBuilderInstantiation)
			{
				FieldInfo fieldInfo = ((FieldOnTypeBuilderInstantiation)field).FieldInfo;
				int length3;
				byte[] signature3 = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out length3);
				int num = base.InternalGetTypeSpecTokenWithBytes(signature3, length3);
				field2 = base.InternalGetMemberRef(fieldInfo.ReflectedType.Module, num, fieldInfo.MetadataTokenInternal);
			}
			else
			{
				int num = this.GetTypeTokenInternal(field.ReflectedType).Token;
				SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(this);
				fieldSigHelper.AddArgument(field.FieldType, field.GetRequiredCustomModifiers(), field.GetOptionalCustomModifiers());
				int length4;
				byte[] signature4 = fieldSigHelper.InternalGetSignature(out length4);
				field2 = base.InternalGetMemberRefFromSignature(num, field.Name, signature4, length4);
			}
			return new FieldToken(field2, field.GetType());
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x00107F91 File Offset: 0x00106F91
		public StringToken GetStringConstant(string str)
		{
			return new StringToken(base.InternalGetStringConstant(str));
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x00107FA0 File Offset: 0x00106FA0
		public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
		{
			if (sigHelper == null)
			{
				throw new ArgumentNullException("sigHelper");
			}
			int sigLength;
			byte[] signature = sigHelper.InternalGetSignature(out sigLength);
			return new SignatureToken(TypeBuilder.InternalGetTokenFromSig(this, signature, sigLength), this);
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x00107FD4 File Offset: 0x00106FD4
		public SignatureToken GetSignatureToken(byte[] sigBytes, int sigLength)
		{
			if (sigBytes == null)
			{
				throw new ArgumentNullException("sigBytes");
			}
			byte[] array = new byte[sigBytes.Length];
			Array.Copy(sigBytes, array, sigBytes.Length);
			return new SignatureToken(TypeBuilder.InternalGetTokenFromSig(this, array, sigLength), this);
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x00108014 File Offset: 0x00107014
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(1, this.GetConstructorToken(con).Token, binaryAttribute, this, false);
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x00108063 File Offset: 0x00107063
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			customBuilder.CreateCustomAttribute(this, 1);
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x00108089 File Offset: 0x00107089
		public ISymbolWriter GetSymWriter()
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			return base.m_iSymWriter;
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x001080A0 File Offset: 0x001070A0
		public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineDocumentNoLock(url, language, languageVendor, documentType);
				}
			}
			return this.DefineDocumentNoLock(url, language, languageVendor, documentType);
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x00108114 File Offset: 0x00107114
		private ISymbolDocumentWriter DefineDocumentNoLock(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			if (base.m_iSymWriter == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			return base.m_iSymWriter.DefineDocument(url, language, languageVendor, documentType);
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00108140 File Offset: 0x00107140
		public void SetUserEntryPoint(MethodInfo entryPoint)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.SetUserEntryPointNoLock(entryPoint);
					return;
				}
			}
			this.SetUserEntryPointNoLock(entryPoint);
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x001081A8 File Offset: 0x001071A8
		private void SetUserEntryPointNoLock(MethodInfo entryPoint)
		{
			if (entryPoint == null)
			{
				throw new ArgumentNullException("entryPoint");
			}
			if (base.m_iSymWriter == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			if (entryPoint.DeclaringType != null)
			{
				if (entryPoint.Module.InternalModule != this.InternalModule)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Argument_NotInTheSameModuleBuilder"));
				}
			}
			else
			{
				MethodBuilder methodBuilder = entryPoint as MethodBuilder;
				if (methodBuilder != null && methodBuilder.GetModule().InternalModule != this.InternalModule)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Argument_NotInTheSameModuleBuilder"));
				}
			}
			SymbolToken userEntryPoint = new SymbolToken(this.GetMethodTokenInternal(entryPoint).Token);
			base.m_iSymWriter.SetUserEntryPoint(userEntryPoint);
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x00108258 File Offset: 0x00107258
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.SetSymCustomAttributeNoLock(name, data);
					return;
				}
			}
			this.SetSymCustomAttributeNoLock(name, data);
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x001082C0 File Offset: 0x001072C0
		private void SetSymCustomAttributeNoLock(string name, byte[] data)
		{
			if (base.m_iSymWriter == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x001082DA File Offset: 0x001072DA
		public bool IsTransient()
		{
			return base.m_moduleData.IsTransient();
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x001082E7 File Offset: 0x001072E7
		void _ModuleBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x001082EE File Offset: 0x001072EE
		void _ModuleBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x001082F5 File Offset: 0x001072F5
		void _ModuleBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x001082FC File Offset: 0x001072FC
		void _ModuleBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002686 RID: 9862
		internal ModuleBuilder m_internalModuleBuilder;

		// Token: 0x04002687 RID: 9863
		private AssemblyBuilder m_assemblyBuilder;

		// Token: 0x04002688 RID: 9864
		private static readonly Dictionary<ModuleBuilder, ModuleBuilder> s_moduleBuilders = new Dictionary<ModuleBuilder, ModuleBuilder>();
	}
}
