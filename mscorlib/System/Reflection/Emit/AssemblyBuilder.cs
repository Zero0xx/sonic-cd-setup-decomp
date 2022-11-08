using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection.Emit
{
	// Token: 0x02000800 RID: 2048
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_AssemblyBuilder))]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class AssemblyBuilder : Assembly, _AssemblyBuilder
	{
		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x000FADE8 File Offset: 0x000F9DE8
		private PermissionSet GrantedPermissionSet
		{
			get
			{
				AssemblyBuilder assemblyBuilder = (AssemblyBuilder)this.InternalAssembly;
				if (assemblyBuilder.m_grantedPermissionSet == null)
				{
					PermissionSet permissionSet = null;
					this.InternalAssembly.nGetGrantSet(out assemblyBuilder.m_grantedPermissionSet, out permissionSet);
					if (assemblyBuilder.m_grantedPermissionSet == null)
					{
						assemblyBuilder.m_grantedPermissionSet = new PermissionSet(PermissionState.Unrestricted);
					}
				}
				return assemblyBuilder.m_grantedPermissionSet;
			}
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x000FAE38 File Offset: 0x000F9E38
		internal void DemandGrantedPermission()
		{
			this.GrantedPermissionSet.Demand();
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004867 RID: 18535 RVA: 0x000FAE45 File Offset: 0x000F9E45
		private bool IsInternal
		{
			get
			{
				return this.m_internalAssemblyBuilder == null;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004868 RID: 18536 RVA: 0x000FAE50 File Offset: 0x000F9E50
		internal override Assembly InternalAssembly
		{
			get
			{
				if (this.IsInternal)
				{
					return this;
				}
				return this.m_internalAssemblyBuilder;
			}
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x000FAE64 File Offset: 0x000F9E64
		internal override Module[] nGetModules(bool loadIfNotFound, bool getResourceModules)
		{
			Module[] array = this.InternalAssembly._nGetModules(loadIfNotFound, getResourceModules);
			if (!this.IsInternal)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ModuleBuilder.GetModuleBuilder(array[i]);
				}
			}
			return array;
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x000FAEA4 File Offset: 0x000F9EA4
		internal override Module GetModuleInternal(string name)
		{
			Module module = this.InternalAssembly._GetModule(name);
			if (module == null)
			{
				return null;
			}
			if (!this.IsInternal)
			{
				return ModuleBuilder.GetModuleBuilder(module);
			}
			return module;
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x000FAED3 File Offset: 0x000F9ED3
		internal AssemblyBuilder(AssemblyBuilder internalAssemblyBuilder)
		{
			this.m_internalAssemblyBuilder = internalAssemblyBuilder;
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x000FAEE4 File Offset: 0x000F9EE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, false, ref stackCrawlMark);
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x000FAF0C File Offset: 0x000F9F0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name, bool emitSymbolInfo)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, emitSymbolInfo, ref stackCrawlMark);
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x000FAF34 File Offset: 0x000F9F34
		private ModuleBuilder DefineDynamicModuleInternal(string name, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.DefineDynamicModuleInternalNoLock(name, emitSymbolInfo, ref stackMark);
				}
			}
			return this.DefineDynamicModuleInternalNoLock(name, emitSymbolInfo, ref stackMark);
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x000FAF8C File Offset: 0x000F9F8C
		private ModuleBuilder DefineDynamicModuleInternalNoLock(string name, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
			}
			base.m_assemblyData.CheckNameConflict(name);
			ModuleBuilder moduleBuilder = (ModuleBuilder)Assembly.nDefineDynamicModule(this, emitSymbolInfo, name, ref stackMark);
			moduleBuilder = new ModuleBuilder(this, moduleBuilder);
			ISymbolWriter writer = null;
			if (emitSymbolInfo)
			{
				Assembly assembly = this.LoadISymWrapper();
				Type type = assembly.GetType("System.Diagnostics.SymbolStore.SymWriter", true, false);
				if (type != null && !type.IsVisible)
				{
					type = null;
				}
				if (type == null)
				{
					throw new ExecutionEngineException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingType"), new object[]
					{
						"SymWriter"
					}));
				}
				new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit).Demand();
				try
				{
					new PermissionSet(PermissionState.Unrestricted).Assert();
					writer = (ISymbolWriter)Activator.CreateInstance(type);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			moduleBuilder.Init(name, null, writer);
			base.m_assemblyData.AddModule(moduleBuilder);
			return moduleBuilder;
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x000FB0AC File Offset: 0x000FA0AC
		private Assembly LoadISymWrapper()
		{
			if (base.m_assemblyData.m_ISymWrapperAssembly != null)
			{
				return base.m_assemblyData.m_ISymWrapperAssembly;
			}
			Assembly assembly = Assembly.Load("ISymWrapper, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
			base.m_assemblyData.m_ISymWrapperAssembly = assembly;
			return assembly;
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x000FB0EC File Offset: 0x000FA0EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name, string fileName)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, fileName, false, ref stackCrawlMark);
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x000FB114 File Offset: 0x000FA114
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.DefineDynamicModuleInternal(name, fileName, emitSymbolInfo, ref stackCrawlMark);
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x000FB13C File Offset: 0x000FA13C
		private ModuleBuilder DefineDynamicModuleInternal(string name, string fileName, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.DefineDynamicModuleInternalNoLock(name, fileName, emitSymbolInfo, ref stackMark);
				}
			}
			return this.DefineDynamicModuleInternalNoLock(name, fileName, emitSymbolInfo, ref stackMark);
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x000FB198 File Offset: 0x000FA198
		internal void CheckContext(params Type[][] typess)
		{
			if (typess == null)
			{
				return;
			}
			foreach (Type[] array in typess)
			{
				if (array != null)
				{
					this.CheckContext(array);
				}
			}
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x000FB1C8 File Offset: 0x000FA1C8
		internal void CheckContext(params Type[] types)
		{
			if (types == null)
			{
				return;
			}
			foreach (Type type in types)
			{
				if (type == null || type.Module.Assembly == typeof(object).Module.Assembly)
				{
					break;
				}
				if (type.Module.Assembly.ReflectionOnly && !this.ReflectionOnly)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arugment_EmitMixedContext1"), new object[]
					{
						type.AssemblyQualifiedName
					}));
				}
				if (!type.Module.Assembly.ReflectionOnly && this.ReflectionOnly)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arugment_EmitMixedContext2"), new object[]
					{
						type.AssemblyQualifiedName
					}));
				}
			}
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x000FB2AC File Offset: 0x000FA2AC
		private ModuleBuilder DefineDynamicModuleInternalNoLock(string name, string fileName, bool emitSymbolInfo, ref StackCrawlMark stackMark)
		{
			if (base.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_BadPersistableModuleInTransientAssembly"));
			}
			if (base.m_assemblyData.m_isSaved)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotAlterAssembly"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "fileName");
			}
			if (!string.Equals(fileName, Path.GetFileName(fileName)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
			}
			base.m_assemblyData.CheckNameConflict(name);
			base.m_assemblyData.CheckFileNameConflict(fileName);
			ModuleBuilder moduleBuilder = (ModuleBuilder)Assembly.nDefineDynamicModule(this, emitSymbolInfo, fileName, ref stackMark);
			moduleBuilder = new ModuleBuilder(this, moduleBuilder);
			ISymbolWriter writer = null;
			if (emitSymbolInfo)
			{
				Assembly assembly = this.LoadISymWrapper();
				Type type = assembly.GetType("System.Diagnostics.SymbolStore.SymWriter", true, false);
				if (type != null && !type.IsVisible)
				{
					type = null;
				}
				if (type == null)
				{
					throw new ExecutionEngineException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingType"), new object[]
					{
						"SymWriter"
					}));
				}
				new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit).Demand();
				try
				{
					new PermissionSet(PermissionState.Unrestricted).Assert();
					writer = (ISymbolWriter)Activator.CreateInstance(type);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			moduleBuilder.Init(name, fileName, writer);
			base.m_assemblyData.AddModule(moduleBuilder);
			return moduleBuilder;
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x000FB464 File Offset: 0x000FA464
		public IResourceWriter DefineResource(string name, string description, string fileName)
		{
			return this.DefineResource(name, description, fileName, ResourceAttributes.Public);
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x000FB470 File Offset: 0x000FA470
		public IResourceWriter DefineResource(string name, string description, string fileName, ResourceAttributes attribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.DefineResourceNoLock(name, description, fileName, attribute);
				}
			}
			return this.DefineResourceNoLock(name, description, fileName, attribute);
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x000FB4D8 File Offset: 0x000FA4D8
		private IResourceWriter DefineResourceNoLock(string name, string description, string fileName, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), name);
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "fileName");
			}
			if (!string.Equals(fileName, Path.GetFileName(fileName)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
			}
			base.m_assemblyData.CheckResNameConflict(name);
			base.m_assemblyData.CheckFileNameConflict(fileName);
			string text;
			ResourceWriter resourceWriter;
			if (base.m_assemblyData.m_strDir == null)
			{
				text = Path.Combine(Environment.CurrentDirectory, fileName);
				resourceWriter = new ResourceWriter(text);
			}
			else
			{
				text = Path.Combine(base.m_assemblyData.m_strDir, fileName);
				resourceWriter = new ResourceWriter(text);
			}
			text = Path.GetFullPath(text);
			fileName = Path.GetFileName(text);
			base.m_assemblyData.AddResWriter(new ResWriterData(resourceWriter, null, name, fileName, text, attribute));
			return resourceWriter;
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x000FB5D4 File Offset: 0x000FA5D4
		public void AddResourceFile(string name, string fileName)
		{
			this.AddResourceFile(name, fileName, ResourceAttributes.Public);
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x000FB5E0 File Offset: 0x000FA5E0
		public void AddResourceFile(string name, string fileName, ResourceAttributes attribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.AddResourceFileNoLock(name, fileName, attribute);
					return;
				}
			}
			this.AddResourceFileNoLock(name, fileName, attribute);
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x000FB640 File Offset: 0x000FA640
		private void AddResourceFileNoLock(string name, string fileName, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), name);
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), fileName);
			}
			if (!string.Equals(fileName, Path.GetFileName(fileName)))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
			}
			base.m_assemblyData.CheckResNameConflict(name);
			base.m_assemblyData.CheckFileNameConflict(fileName);
			string text;
			if (base.m_assemblyData.m_strDir == null)
			{
				text = Path.Combine(Environment.CurrentDirectory, fileName);
			}
			else
			{
				text = Path.Combine(base.m_assemblyData.m_strDir, fileName);
			}
			text = Path.GetFullPath(text);
			fileName = Path.GetFileName(text);
			if (!File.Exists(text))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[]
				{
					fileName
				}), fileName);
			}
			base.m_assemblyData.AddResWriter(new ResWriterData(null, null, name, fileName, text, attribute));
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x000FB757 File Offset: 0x000FA757
		public override string[] GetManifestResourceNames()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x000FB768 File Offset: 0x000FA768
		public override FileStream GetFile(string name)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x000FB779 File Offset: 0x000FA779
		public override FileStream[] GetFiles(bool getResourceModules)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x000FB78A File Offset: 0x000FA78A
		public override Stream GetManifestResourceStream(Type type, string name)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x000FB79B File Offset: 0x000FA79B
		public override Stream GetManifestResourceStream(string name)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x000FB7AC File Offset: 0x000FA7AC
		public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x000FB7BD File Offset: 0x000FA7BD
		public override string Location
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004884 RID: 18564 RVA: 0x000FB7CE File Offset: 0x000FA7CE
		public override string ImageRuntimeVersion
		{
			get
			{
				return RuntimeEnvironment.GetSystemVersion();
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06004885 RID: 18565 RVA: 0x000FB7D5 File Offset: 0x000FA7D5
		public override string CodeBase
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06004886 RID: 18566 RVA: 0x000FB7E6 File Offset: 0x000FA7E6
		public override MethodInfo EntryPoint
		{
			get
			{
				if (this.IsInternal)
				{
					this.DemandGrantedPermission();
				}
				return base.m_assemblyData.m_entryPointMethod;
			}
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x000FB801 File Offset: 0x000FA801
		public override Type[] GetExportedTypes()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x000FB814 File Offset: 0x000FA814
		public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineVersionInfoResourceNoLock(product, productVersion, company, copyright, trademark);
					return;
				}
			}
			this.DefineVersionInfoResourceNoLock(product, productVersion, company, copyright, trademark);
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x000FB87C File Offset: 0x000FA87C
		private void DefineVersionInfoResourceNoLock(string product, string productVersion, string company, string copyright, string trademark)
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			base.m_assemblyData.m_nativeVersion = new NativeVersionInfo();
			base.m_assemblyData.m_nativeVersion.m_strCopyright = copyright;
			base.m_assemblyData.m_nativeVersion.m_strTrademark = trademark;
			base.m_assemblyData.m_nativeVersion.m_strCompany = company;
			base.m_assemblyData.m_nativeVersion.m_strProduct = product;
			base.m_assemblyData.m_nativeVersion.m_strProductVersion = productVersion;
			base.m_assemblyData.m_hasUnmanagedVersionInfo = true;
			base.m_assemblyData.m_OverrideUnmanagedVersionInfo = true;
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x000FB940 File Offset: 0x000FA940
		public void DefineVersionInfoResource()
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineVersionInfoResourceNoLock();
					return;
				}
			}
			this.DefineVersionInfoResourceNoLock();
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x000FB99C File Offset: 0x000FA99C
		private void DefineVersionInfoResourceNoLock()
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			base.m_assemblyData.m_hasUnmanagedVersionInfo = true;
			base.m_assemblyData.m_nativeVersion = new NativeVersionInfo();
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x000FB9FC File Offset: 0x000FA9FC
		public void DefineUnmanagedResource(byte[] resource)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineUnmanagedResourceNoLock(resource);
					return;
				}
			}
			this.DefineUnmanagedResourceNoLock(resource);
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x000FBA68 File Offset: 0x000FAA68
		private void DefineUnmanagedResourceNoLock(byte[] resource)
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			base.m_assemblyData.m_resourceBytes = new byte[resource.Length];
			Array.Copy(resource, base.m_assemblyData.m_resourceBytes, resource.Length);
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x000FBAD4 File Offset: 0x000FAAD4
		public void DefineUnmanagedResource(string resourceFileName)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (resourceFileName == null)
			{
				throw new ArgumentNullException("resourceFileName");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.DefineUnmanagedResourceNoLock(resourceFileName);
					return;
				}
			}
			this.DefineUnmanagedResourceNoLock(resourceFileName);
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x000FBB40 File Offset: 0x000FAB40
		private void DefineUnmanagedResourceNoLock(string resourceFileName)
		{
			if (base.m_assemblyData.m_strResourceFileName != null || base.m_assemblyData.m_resourceBytes != null || base.m_assemblyData.m_nativeVersion != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			string text;
			if (base.m_assemblyData.m_strDir == null)
			{
				text = Path.Combine(Environment.CurrentDirectory, resourceFileName);
			}
			else
			{
				text = Path.Combine(base.m_assemblyData.m_strDir, resourceFileName);
			}
			text = Path.GetFullPath(resourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read, text).Demand();
			if (!File.Exists(text))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[]
				{
					resourceFileName
				}), resourceFileName);
			}
			base.m_assemblyData.m_strResourceFileName = text;
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x000FBC00 File Offset: 0x000FAC00
		public ModuleBuilder GetDynamicModule(string name)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					return this.GetDynamicModuleNoLock(name);
				}
			}
			return this.GetDynamicModuleNoLock(name);
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x000FBC60 File Offset: 0x000FAC60
		private ModuleBuilder GetDynamicModuleNoLock(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			int count = base.m_assemblyData.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)base.m_assemblyData.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strModuleName.Equals(name))
				{
					return moduleBuilder;
				}
			}
			return null;
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x000FBCE2 File Offset: 0x000FACE2
		public void SetEntryPoint(MethodInfo entryMethod)
		{
			this.SetEntryPoint(entryMethod, PEFileKinds.ConsoleApplication);
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x000FBCEC File Offset: 0x000FACEC
		public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SetEntryPointNoLock(entryMethod, fileKind);
					return;
				}
			}
			this.SetEntryPointNoLock(entryMethod, fileKind);
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x000FBD4C File Offset: 0x000FAD4C
		private void SetEntryPointNoLock(MethodInfo entryMethod, PEFileKinds fileKind)
		{
			if (entryMethod == null)
			{
				throw new ArgumentNullException("entryMethod");
			}
			Module internalModule = entryMethod.Module.InternalModule;
			if (!(internalModule is ModuleBuilder) || !this.InternalAssembly.Equals(internalModule.Assembly.InternalAssembly))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EntryMethodNotDefinedInAssembly"));
			}
			base.m_assemblyData.m_entryPointModule = (ModuleBuilder)ModuleBuilder.GetModuleBuilder(internalModule);
			base.m_assemblyData.m_entryPointMethod = entryMethod;
			base.m_assemblyData.m_peFileKind = fileKind;
			base.m_assemblyData.m_entryPointModule.SetEntryPoint(entryMethod);
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x000FBDE4 File Offset: 0x000FADE4
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SetCustomAttributeNoLock(con, binaryAttribute);
					return;
				}
			}
			this.SetCustomAttributeNoLock(con, binaryAttribute);
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x000FBE60 File Offset: 0x000FAE60
		private void SetCustomAttributeNoLock(ConstructorInfo con, byte[] binaryAttribute)
		{
			ModuleBuilder inMemoryAssemblyModule = base.m_assemblyData.GetInMemoryAssemblyModule();
			TypeBuilder.InternalCreateCustomAttribute(536870913, inMemoryAssemblyModule.GetConstructorToken(con).Token, binaryAttribute, inMemoryAssemblyModule, false, typeof(DebuggableAttribute) == con.DeclaringType);
			if (base.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
			{
				return;
			}
			base.m_assemblyData.AddCustomAttribute(con, binaryAttribute);
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x000FBEC4 File Offset: 0x000FAEC4
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SetCustomAttributeNoLock(customBuilder);
					return;
				}
			}
			this.SetCustomAttributeNoLock(customBuilder);
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x000FBF30 File Offset: 0x000FAF30
		private void SetCustomAttributeNoLock(CustomAttributeBuilder customBuilder)
		{
			ModuleBuilder inMemoryAssemblyModule = base.m_assemblyData.GetInMemoryAssemblyModule();
			customBuilder.CreateCustomAttribute(inMemoryAssemblyModule, 536870913);
			if (base.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
			{
				return;
			}
			base.m_assemblyData.AddCustomAttribute(customBuilder);
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x000FBF70 File Offset: 0x000FAF70
		public void Save(string assemblyFileName)
		{
			this.Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x000FBF80 File Offset: 0x000FAF80
		public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedPermission();
			}
			if (base.m_assemblyData.m_isSynchronized)
			{
				lock (base.m_assemblyData)
				{
					this.SaveNoLock(assemblyFileName, portableExecutableKind, imageFileMachine);
					return;
				}
			}
			this.SaveNoLock(assemblyFileName, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x000FBFE0 File Offset: 0x000FAFE0
		private void SaveNoLock(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			int[] array = null;
			int[] array2 = null;
			string text = null;
			try
			{
				if (base.m_assemblyData.m_iCABuilder != 0)
				{
					array = new int[base.m_assemblyData.m_iCABuilder];
				}
				if (base.m_assemblyData.m_iCAs != 0)
				{
					array2 = new int[base.m_assemblyData.m_iCAs];
				}
				if (base.m_assemblyData.m_isSaved)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("InvalidOperation_AssemblyHasBeenSaved"), new object[]
					{
						base.nGetSimpleName()
					}));
				}
				if ((base.m_assemblyData.m_access & AssemblyBuilderAccess.Save) != AssemblyBuilderAccess.Save)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CantSaveTransientAssembly"));
				}
				if (assemblyFileName == null)
				{
					throw new ArgumentNullException("assemblyFileName");
				}
				if (assemblyFileName.Length == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "assemblyFileName");
				}
				if (!string.Equals(assemblyFileName, Path.GetFileName(assemblyFileName)))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "assemblyFileName");
				}
				ModuleBuilder moduleBuilder = base.m_assemblyData.FindModuleWithFileName(assemblyFileName);
				if (moduleBuilder != null)
				{
					base.m_assemblyData.SetOnDiskAssemblyModule(moduleBuilder);
				}
				if (moduleBuilder == null)
				{
					base.m_assemblyData.CheckFileNameConflict(assemblyFileName);
				}
				if (base.m_assemblyData.m_strDir == null)
				{
					base.m_assemblyData.m_strDir = Environment.CurrentDirectory;
				}
				else if (!Directory.Exists(base.m_assemblyData.m_strDir))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Argument_InvalidDirectory"), new object[]
					{
						base.m_assemblyData.m_strDir
					}));
				}
				assemblyFileName = Path.Combine(base.m_assemblyData.m_strDir, assemblyFileName);
				assemblyFileName = Path.GetFullPath(assemblyFileName);
				new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, assemblyFileName).Demand();
				if (moduleBuilder != null)
				{
					for (int i = 0; i < base.m_assemblyData.m_iCABuilder; i++)
					{
						array[i] = base.m_assemblyData.m_CABuilders[i].PrepareCreateCustomAttributeToDisk(moduleBuilder);
					}
					for (int i = 0; i < base.m_assemblyData.m_iCAs; i++)
					{
						array2[i] = moduleBuilder.InternalGetConstructorToken(base.m_assemblyData.m_CACons[i], true).Token;
					}
					moduleBuilder.PreSave(assemblyFileName, portableExecutableKind, imageFileMachine);
				}
				base.nPrepareForSavingManifestToDisk(moduleBuilder);
				ModuleBuilder onDiskAssemblyModule = base.m_assemblyData.GetOnDiskAssemblyModule();
				if (base.m_assemblyData.m_strResourceFileName != null)
				{
					onDiskAssemblyModule.DefineUnmanagedResourceFileInternalNoLock(base.m_assemblyData.m_strResourceFileName);
				}
				else if (base.m_assemblyData.m_resourceBytes != null)
				{
					onDiskAssemblyModule.DefineUnmanagedResourceInternalNoLock(base.m_assemblyData.m_resourceBytes);
				}
				else if (base.m_assemblyData.m_hasUnmanagedVersionInfo)
				{
					base.m_assemblyData.FillUnmanagedVersionInfo();
					string text2 = base.m_assemblyData.m_nativeVersion.m_strFileVersion;
					if (text2 == null)
					{
						text2 = base.GetVersion().ToString();
					}
					text = Assembly.nDefineVersionInfoResource(assemblyFileName, base.m_assemblyData.m_nativeVersion.m_strTitle, null, base.m_assemblyData.m_nativeVersion.m_strDescription, base.m_assemblyData.m_nativeVersion.m_strCopyright, base.m_assemblyData.m_nativeVersion.m_strTrademark, base.m_assemblyData.m_nativeVersion.m_strCompany, base.m_assemblyData.m_nativeVersion.m_strProduct, base.m_assemblyData.m_nativeVersion.m_strProductVersion, text2, base.m_assemblyData.m_nativeVersion.m_lcid, base.m_assemblyData.m_peFileKind == PEFileKinds.Dll);
					onDiskAssemblyModule.DefineUnmanagedResourceFileInternalNoLock(text);
				}
				if (moduleBuilder == null)
				{
					for (int i = 0; i < base.m_assemblyData.m_iCABuilder; i++)
					{
						array[i] = base.m_assemblyData.m_CABuilders[i].PrepareCreateCustomAttributeToDisk(onDiskAssemblyModule);
					}
					for (int i = 0; i < base.m_assemblyData.m_iCAs; i++)
					{
						array2[i] = onDiskAssemblyModule.InternalGetConstructorToken(base.m_assemblyData.m_CACons[i], true).Token;
					}
				}
				int count = base.m_assemblyData.m_moduleBuilderList.Count;
				for (int i = 0; i < count; i++)
				{
					ModuleBuilder moduleBuilder2 = (ModuleBuilder)base.m_assemblyData.m_moduleBuilderList[i];
					if (!moduleBuilder2.IsTransient() && moduleBuilder2 != moduleBuilder)
					{
						string text3 = moduleBuilder2.m_moduleData.m_strFileName;
						if (base.m_assemblyData.m_strDir != null)
						{
							text3 = Path.Combine(base.m_assemblyData.m_strDir, text3);
							text3 = Path.GetFullPath(text3);
						}
						new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, text3).Demand();
						moduleBuilder2.m_moduleData.m_tkFile = base.nSaveToFileList(moduleBuilder2.m_moduleData.m_strFileName);
						moduleBuilder2.PreSave(text3, portableExecutableKind, imageFileMachine);
						moduleBuilder2.Save(text3, false, portableExecutableKind, imageFileMachine);
						base.nSetHashValue(moduleBuilder2.m_moduleData.m_tkFile, text3);
					}
				}
				for (int i = 0; i < base.m_assemblyData.m_iPublicComTypeCount; i++)
				{
					Type type = base.m_assemblyData.m_publicComTypeList[i];
					if (type is RuntimeType)
					{
						ModuleBuilder moduleBuilder3 = base.m_assemblyData.FindModuleWithName(type.Module.m_moduleData.m_strModuleName);
						if (moduleBuilder3 != moduleBuilder)
						{
							this.DefineNestedComType(type, moduleBuilder3.m_moduleData.m_tkFile, type.MetadataTokenInternal);
						}
					}
					else
					{
						TypeBuilder typeBuilder = (TypeBuilder)type;
						ModuleBuilder moduleBuilder3 = (ModuleBuilder)type.Module;
						if (moduleBuilder3 != moduleBuilder)
						{
							this.DefineNestedComType(type, moduleBuilder3.m_moduleData.m_tkFile, typeBuilder.MetadataTokenInternal);
						}
					}
				}
				for (int i = 0; i < base.m_assemblyData.m_iCABuilder; i++)
				{
					base.m_assemblyData.m_CABuilders[i].CreateCustomAttribute(onDiskAssemblyModule, 536870913, array[i], true);
				}
				for (int i = 0; i < base.m_assemblyData.m_iCAs; i++)
				{
					TypeBuilder.InternalCreateCustomAttribute(536870913, array2[i], base.m_assemblyData.m_CABytes[i], onDiskAssemblyModule, true);
				}
				if (base.m_assemblyData.m_RequiredPset != null || base.m_assemblyData.m_OptionalPset != null || base.m_assemblyData.m_RefusedPset != null)
				{
					byte[] required = null;
					byte[] optional = null;
					byte[] refused = null;
					if (base.m_assemblyData.m_RequiredPset != null)
					{
						required = base.m_assemblyData.m_RequiredPset.EncodeXml();
					}
					if (base.m_assemblyData.m_OptionalPset != null)
					{
						optional = base.m_assemblyData.m_OptionalPset.EncodeXml();
					}
					if (base.m_assemblyData.m_RefusedPset != null)
					{
						refused = base.m_assemblyData.m_RefusedPset.EncodeXml();
					}
					base.nSavePermissionRequests(required, optional, refused);
				}
				count = base.m_assemblyData.m_resWriterList.Count;
				for (int i = 0; i < count; i++)
				{
					ResWriterData resWriterData = null;
					try
					{
						resWriterData = (ResWriterData)base.m_assemblyData.m_resWriterList[i];
						if (resWriterData.m_resWriter != null)
						{
							new FileIOPermission(FileIOPermissionAccess.Write | FileIOPermissionAccess.Append, resWriterData.m_strFullFileName).Demand();
						}
					}
					finally
					{
						if (resWriterData != null && resWriterData.m_resWriter != null)
						{
							resWriterData.m_resWriter.Close();
						}
					}
					base.nAddStandAloneResource(resWriterData.m_strName, resWriterData.m_strFileName, resWriterData.m_strFullFileName, (int)resWriterData.m_attribute);
				}
				if (moduleBuilder == null)
				{
					if (onDiskAssemblyModule.m_moduleData.m_strResourceFileName != null)
					{
						onDiskAssemblyModule.InternalDefineNativeResourceFile(onDiskAssemblyModule.m_moduleData.m_strResourceFileName, (int)portableExecutableKind, (int)imageFileMachine);
					}
					else if (onDiskAssemblyModule.m_moduleData.m_resourceBytes != null)
					{
						onDiskAssemblyModule.InternalDefineNativeResourceBytes(onDiskAssemblyModule.m_moduleData.m_resourceBytes, (int)portableExecutableKind, (int)imageFileMachine);
					}
					if (base.m_assemblyData.m_entryPointModule != null)
					{
						base.nSaveManifestToDisk(assemblyFileName, base.m_assemblyData.m_entryPointModule.m_moduleData.m_tkFile, (int)base.m_assemblyData.m_peFileKind, (int)portableExecutableKind, (int)imageFileMachine);
					}
					else
					{
						base.nSaveManifestToDisk(assemblyFileName, 0, (int)base.m_assemblyData.m_peFileKind, (int)portableExecutableKind, (int)imageFileMachine);
					}
				}
				else
				{
					if (base.m_assemblyData.m_entryPointModule != null && base.m_assemblyData.m_entryPointModule != moduleBuilder)
					{
						moduleBuilder.m_EntryPoint = new MethodToken(base.m_assemblyData.m_entryPointModule.m_moduleData.m_tkFile);
					}
					moduleBuilder.Save(assemblyFileName, true, portableExecutableKind, imageFileMachine);
				}
				base.m_assemblyData.m_isSaved = true;
			}
			finally
			{
				if (text != null)
				{
					File.Delete(text);
				}
			}
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x000FC7FC File Offset: 0x000FB7FC
		internal bool IsPersistable()
		{
			return (base.m_assemblyData.m_access & AssemblyBuilderAccess.Save) == AssemblyBuilderAccess.Save;
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x000FC814 File Offset: 0x000FB814
		private int DefineNestedComType(Type type, int tkResolutionScope, int tkTypeDef)
		{
			Type declaringType = type.DeclaringType;
			if (declaringType == null)
			{
				return base.nSaveExportedType(type.FullName, tkResolutionScope, tkTypeDef, type.Attributes);
			}
			tkResolutionScope = this.DefineNestedComType(declaringType, tkResolutionScope, tkTypeDef);
			return base.nSaveExportedType(type.FullName, tkResolutionScope, tkTypeDef, type.Attributes);
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x000FC85F File Offset: 0x000FB85F
		private AssemblyBuilder()
		{
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x000FC867 File Offset: 0x000FB867
		void _AssemblyBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x000FC86E File Offset: 0x000FB86E
		void _AssemblyBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x000FC875 File Offset: 0x000FB875
		void _AssemblyBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x000FC87C File Offset: 0x000FB87C
		void _AssemblyBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002554 RID: 9556
		private AssemblyBuilder m_internalAssemblyBuilder;

		// Token: 0x04002555 RID: 9557
		private PermissionSet m_grantedPermissionSet;
	}
}
