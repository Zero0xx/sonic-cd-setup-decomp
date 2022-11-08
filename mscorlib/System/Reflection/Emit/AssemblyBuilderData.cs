using System;
using System.Collections;
using System.Globalization;
using System.Security;

namespace System.Reflection.Emit
{
	// Token: 0x02000801 RID: 2049
	internal class AssemblyBuilderData
	{
		// Token: 0x060048A3 RID: 18595 RVA: 0x000FC884 File Offset: 0x000FB884
		internal AssemblyBuilderData(Assembly assembly, string strAssemblyName, AssemblyBuilderAccess access, string dir)
		{
			this.m_assembly = assembly;
			this.m_strAssemblyName = strAssemblyName;
			this.m_access = access;
			this.m_moduleBuilderList = new ArrayList();
			this.m_resWriterList = new ArrayList();
			this.m_publicComTypeList = null;
			this.m_CABuilders = null;
			this.m_CABytes = null;
			this.m_CACons = null;
			this.m_iPublicComTypeCount = 0;
			this.m_iCABuilder = 0;
			this.m_iCAs = 0;
			this.m_entryPointModule = null;
			this.m_isSaved = false;
			if (dir == null && access != AssemblyBuilderAccess.Run)
			{
				this.m_strDir = Environment.CurrentDirectory;
			}
			else
			{
				this.m_strDir = dir;
			}
			this.m_RequiredPset = null;
			this.m_OptionalPset = null;
			this.m_RefusedPset = null;
			this.m_isSynchronized = true;
			this.m_hasUnmanagedVersionInfo = false;
			this.m_OverrideUnmanagedVersionInfo = false;
			this.m_InMemoryAssemblyModule = null;
			this.m_OnDiskAssemblyModule = null;
			this.m_peFileKind = PEFileKinds.Dll;
			this.m_strResourceFileName = null;
			this.m_resourceBytes = null;
			this.m_nativeVersion = null;
			this.m_entryPointMethod = null;
			this.m_ISymWrapperAssembly = null;
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x000FC980 File Offset: 0x000FB980
		internal void AddModule(ModuleBuilder dynModule)
		{
			this.m_moduleBuilderList.Add(dynModule);
			if (this.m_assembly != null)
			{
				this.m_assembly.nAddFileToInMemoryFileList(dynModule.m_moduleData.m_strFileName, dynModule);
			}
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x000FC9AF File Offset: 0x000FB9AF
		internal void AddResWriter(ResWriterData resData)
		{
			this.m_resWriterList.Add(resData);
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x000FC9C0 File Offset: 0x000FB9C0
		internal void AddCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.m_CABuilders == null)
			{
				this.m_CABuilders = new CustomAttributeBuilder[16];
			}
			if (this.m_iCABuilder == this.m_CABuilders.Length)
			{
				CustomAttributeBuilder[] array = new CustomAttributeBuilder[this.m_iCABuilder * 2];
				Array.Copy(this.m_CABuilders, array, this.m_iCABuilder);
				this.m_CABuilders = array;
			}
			this.m_CABuilders[this.m_iCABuilder] = customBuilder;
			this.m_iCABuilder++;
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x000FCA38 File Offset: 0x000FBA38
		internal void AddCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.m_CABytes == null)
			{
				this.m_CABytes = new byte[16][];
				this.m_CACons = new ConstructorInfo[16];
			}
			if (this.m_iCAs == this.m_CABytes.Length)
			{
				byte[][] array = new byte[this.m_iCAs * 2][];
				ConstructorInfo[] array2 = new ConstructorInfo[this.m_iCAs * 2];
				for (int i = 0; i < this.m_iCAs; i++)
				{
					array[i] = this.m_CABytes[i];
					array2[i] = this.m_CACons[i];
				}
				this.m_CABytes = array;
				this.m_CACons = array2;
			}
			byte[] array3 = new byte[binaryAttribute.Length];
			Array.Copy(binaryAttribute, array3, binaryAttribute.Length);
			this.m_CABytes[this.m_iCAs] = array3;
			this.m_CACons[this.m_iCAs] = con;
			this.m_iCAs++;
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x000FCB08 File Offset: 0x000FBB08
		internal void FillUnmanagedVersionInfo()
		{
			CultureInfo locale = this.m_assembly.GetLocale();
			if (locale != null)
			{
				this.m_nativeVersion.m_lcid = locale.LCID;
			}
			for (int i = 0; i < this.m_iCABuilder; i++)
			{
				Type declaringType = this.m_CABuilders[i].m_con.DeclaringType;
				if (this.m_CABuilders[i].m_constructorArgs.Length != 0 && this.m_CABuilders[i].m_constructorArgs[0] != null)
				{
					if (declaringType.Equals(typeof(AssemblyCopyrightAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strCopyright = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyTrademarkAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strTrademark = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyProductAttribute)))
					{
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strProduct = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyCompanyAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strCompany = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyDescriptionAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						this.m_nativeVersion.m_strDescription = this.m_CABuilders[i].m_constructorArgs[0].ToString();
					}
					else if (declaringType.Equals(typeof(AssemblyTitleAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						this.m_nativeVersion.m_strTitle = this.m_CABuilders[i].m_constructorArgs[0].ToString();
					}
					else if (declaringType.Equals(typeof(AssemblyInformationalVersionAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strProductVersion = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
					else if (declaringType.Equals(typeof(AssemblyCultureAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						CultureInfo cultureInfo = new CultureInfo(this.m_CABuilders[i].m_constructorArgs[0].ToString());
						this.m_nativeVersion.m_lcid = cultureInfo.LCID;
					}
					else if (declaringType.Equals(typeof(AssemblyFileVersionAttribute)))
					{
						if (this.m_CABuilders[i].m_constructorArgs.Length != 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_BadCAForUnmngRSC"), new object[]
							{
								this.m_CABuilders[i].m_con.ReflectedType.Name
							}));
						}
						if (!this.m_OverrideUnmanagedVersionInfo)
						{
							this.m_nativeVersion.m_strFileVersion = this.m_CABuilders[i].m_constructorArgs[0].ToString();
						}
					}
				}
			}
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x000FD04C File Offset: 0x000FC04C
		internal void CheckResNameConflict(string strNewResName)
		{
			int count = this.m_resWriterList.Count;
			for (int i = 0; i < count; i++)
			{
				ResWriterData resWriterData = (ResWriterData)this.m_resWriterList[i];
				if (resWriterData.m_strName.Equals(strNewResName))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateResourceName"));
				}
			}
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x000FD0A4 File Offset: 0x000FC0A4
		internal void CheckNameConflict(string strNewModuleName)
		{
			int count = this.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strModuleName.Equals(strNewModuleName))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateModuleName"));
				}
			}
			if (!(this.m_assembly is AssemblyBuilder) && this.m_assembly.GetModule(strNewModuleName) != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateModuleName"));
			}
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x000FD12C File Offset: 0x000FC12C
		internal void CheckTypeNameConflict(string strTypeName, TypeBuilder enclosingType)
		{
			for (int i = 0; i < this.m_moduleBuilderList.Count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				for (int j = 0; j < moduleBuilder.m_TypeBuilderList.Count; j++)
				{
					Type type = (Type)moduleBuilder.m_TypeBuilderList[j];
					if (type.FullName.Equals(strTypeName) && type.DeclaringType == enclosingType)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateTypeName"));
					}
				}
			}
			if (enclosingType == null && !(this.m_assembly is AssemblyBuilder) && this.m_assembly.GetType(strTypeName, false, false) != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DuplicateTypeName"));
			}
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x000FD1E0 File Offset: 0x000FC1E0
		internal void CheckFileNameConflict(string strFileName)
		{
			int count = this.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strFileName != null && string.Compare(moduleBuilder.m_moduleData.m_strFileName, strFileName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicatedFileName"));
				}
			}
			count = this.m_resWriterList.Count;
			for (int i = 0; i < count; i++)
			{
				ResWriterData resWriterData = (ResWriterData)this.m_resWriterList[i];
				if (resWriterData.m_strFileName != null && string.Compare(resWriterData.m_strFileName, strFileName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_DuplicatedFileName"));
				}
			}
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x000FD29C File Offset: 0x000FC29C
		internal ModuleBuilder FindModuleWithFileName(string strFileName)
		{
			int count = this.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strFileName != null && string.Compare(moduleBuilder.m_moduleData.m_strFileName, strFileName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return moduleBuilder;
				}
			}
			return null;
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x000FD2F8 File Offset: 0x000FC2F8
		internal ModuleBuilder FindModuleWithName(string strName)
		{
			int count = this.m_moduleBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_moduleBuilderList[i];
				if (moduleBuilder.m_moduleData.m_strModuleName != null && string.Compare(moduleBuilder.m_moduleData.m_strModuleName, strName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return moduleBuilder;
				}
			}
			return null;
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x000FD353 File Offset: 0x000FC353
		internal void AddPublicComType(Type type)
		{
			if (this.m_isSaved)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotAlterAssembly"));
			}
			this.EnsurePublicComTypeCapacity();
			this.m_publicComTypeList[this.m_iPublicComTypeCount] = type;
			this.m_iPublicComTypeCount++;
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x000FD38F File Offset: 0x000FC38F
		internal void AddPermissionRequests(PermissionSet required, PermissionSet optional, PermissionSet refused)
		{
			if (this.m_isSaved)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotAlterAssembly"));
			}
			this.m_RequiredPset = required;
			this.m_OptionalPset = optional;
			this.m_RefusedPset = refused;
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x000FD3C0 File Offset: 0x000FC3C0
		internal void EnsurePublicComTypeCapacity()
		{
			if (this.m_publicComTypeList == null)
			{
				this.m_publicComTypeList = new Type[16];
			}
			if (this.m_iPublicComTypeCount == this.m_publicComTypeList.Length)
			{
				Type[] array = new Type[this.m_iPublicComTypeCount * 2];
				Array.Copy(this.m_publicComTypeList, array, this.m_iPublicComTypeCount);
				this.m_publicComTypeList = array;
			}
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x000FD41C File Offset: 0x000FC41C
		internal ModuleBuilder GetInMemoryAssemblyModule()
		{
			if (this.m_InMemoryAssemblyModule == null)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_assembly.nGetInMemoryAssemblyModule();
				moduleBuilder.Init("RefEmit_InMemoryManifestModule", null, null);
				this.m_InMemoryAssemblyModule = moduleBuilder;
			}
			return this.m_InMemoryAssemblyModule;
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x000FD45C File Offset: 0x000FC45C
		internal ModuleBuilder GetOnDiskAssemblyModule()
		{
			if (this.m_OnDiskAssemblyModule == null)
			{
				ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_assembly.nGetOnDiskAssemblyModule();
				moduleBuilder.Init("RefEmit_OnDiskManifestModule", null, null);
				this.m_OnDiskAssemblyModule = moduleBuilder;
			}
			return this.m_OnDiskAssemblyModule;
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x000FD49C File Offset: 0x000FC49C
		internal void SetOnDiskAssemblyModule(ModuleBuilder modBuilder)
		{
			this.m_OnDiskAssemblyModule = modBuilder;
		}

		// Token: 0x04002556 RID: 9558
		internal const int m_iInitialSize = 16;

		// Token: 0x04002557 RID: 9559
		internal const int m_tkAssembly = 536870913;

		// Token: 0x04002558 RID: 9560
		internal ArrayList m_moduleBuilderList;

		// Token: 0x04002559 RID: 9561
		internal ArrayList m_resWriterList;

		// Token: 0x0400255A RID: 9562
		internal string m_strAssemblyName;

		// Token: 0x0400255B RID: 9563
		internal AssemblyBuilderAccess m_access;

		// Token: 0x0400255C RID: 9564
		internal Assembly m_assembly;

		// Token: 0x0400255D RID: 9565
		internal Type[] m_publicComTypeList;

		// Token: 0x0400255E RID: 9566
		internal int m_iPublicComTypeCount;

		// Token: 0x0400255F RID: 9567
		internal ModuleBuilder m_entryPointModule;

		// Token: 0x04002560 RID: 9568
		internal bool m_isSaved;

		// Token: 0x04002561 RID: 9569
		internal string m_strDir;

		// Token: 0x04002562 RID: 9570
		internal PermissionSet m_RequiredPset;

		// Token: 0x04002563 RID: 9571
		internal PermissionSet m_OptionalPset;

		// Token: 0x04002564 RID: 9572
		internal PermissionSet m_RefusedPset;

		// Token: 0x04002565 RID: 9573
		internal bool m_isSynchronized;

		// Token: 0x04002566 RID: 9574
		internal CustomAttributeBuilder[] m_CABuilders;

		// Token: 0x04002567 RID: 9575
		internal int m_iCABuilder;

		// Token: 0x04002568 RID: 9576
		internal byte[][] m_CABytes;

		// Token: 0x04002569 RID: 9577
		internal ConstructorInfo[] m_CACons;

		// Token: 0x0400256A RID: 9578
		internal int m_iCAs;

		// Token: 0x0400256B RID: 9579
		internal PEFileKinds m_peFileKind;

		// Token: 0x0400256C RID: 9580
		private ModuleBuilder m_InMemoryAssemblyModule;

		// Token: 0x0400256D RID: 9581
		private ModuleBuilder m_OnDiskAssemblyModule;

		// Token: 0x0400256E RID: 9582
		internal MethodInfo m_entryPointMethod;

		// Token: 0x0400256F RID: 9583
		internal Assembly m_ISymWrapperAssembly;

		// Token: 0x04002570 RID: 9584
		internal string m_strResourceFileName;

		// Token: 0x04002571 RID: 9585
		internal byte[] m_resourceBytes;

		// Token: 0x04002572 RID: 9586
		internal NativeVersionInfo m_nativeVersion;

		// Token: 0x04002573 RID: 9587
		internal bool m_hasUnmanagedVersionInfo;

		// Token: 0x04002574 RID: 9588
		internal bool m_OverrideUnmanagedVersionInfo;
	}
}
