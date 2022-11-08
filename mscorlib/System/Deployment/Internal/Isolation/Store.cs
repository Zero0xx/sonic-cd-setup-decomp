using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000231 RID: 561
	internal class Store
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x000377CC File Offset: 0x000367CC
		public IStore InternalStore
		{
			get
			{
				return this._pStore;
			}
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x000377D4 File Offset: 0x000367D4
		public Store(IStore pStore)
		{
			if (pStore == null)
			{
				throw new ArgumentNullException("pStore");
			}
			this._pStore = pStore;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x000377F4 File Offset: 0x000367F4
		public uint[] Transact(StoreTransactionOperation[] operations)
		{
			if (operations == null || operations.Length == 0)
			{
				throw new ArgumentException("operations");
			}
			uint[] array = new uint[operations.Length];
			int[] rgResults = new int[operations.Length];
			this._pStore.Transact(new IntPtr(operations.Length), operations, array, rgResults);
			return array;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00037840 File Offset: 0x00036840
		public IDefinitionIdentity BindReferenceToAssemblyIdentity(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)obj;
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0003786C File Offset: 0x0003686C
		public void CalculateDelimiterOfDeploymentsBasedOnQuota(uint dwFlags, uint cDeployments, IDefinitionAppId[] rgpIDefinitionAppId_Deployments, ref StoreApplicationReference InstallerReference, ulong ulonglongQuota, ref uint Delimiter, ref ulong SizeSharedWithExternalDeployment, ref ulong SizeConsumedByInputDeploymentArray)
		{
			IntPtr zero = IntPtr.Zero;
			this._pStore.CalculateDelimiterOfDeploymentsBasedOnQuota(dwFlags, new IntPtr((long)((ulong)cDeployments)), rgpIDefinitionAppId_Deployments, ref InstallerReference, ulonglongQuota, ref zero, ref SizeSharedWithExternalDeployment, ref SizeConsumedByInputDeploymentArray);
			Delimiter = (uint)zero.ToInt64();
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x000378A8 File Offset: 0x000368A8
		public ICMS BindReferenceToAssemblyManifest(uint Flags, IReferenceIdentity ReferenceIdentity, uint cDeploymentsToIgnore, IDefinitionIdentity[] DefinitionIdentity_DeploymentsToIgnore)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object obj = this._pStore.BindReferenceToAssembly(Flags, ReferenceIdentity, cDeploymentsToIgnore, DefinitionIdentity_DeploymentsToIgnore, ref iid_ICMS);
			return (ICMS)obj;
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x000378D4 File Offset: 0x000368D4
		public ICMS GetAssemblyManifest(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_ICMS = IsolationInterop.IID_ICMS;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_ICMS);
			return (ICMS)assemblyInformation;
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00037900 File Offset: 0x00036900
		public IDefinitionIdentity GetAssemblyIdentity(uint Flags, IDefinitionIdentity DefinitionIdentity)
		{
			Guid iid_IDefinitionIdentity = IsolationInterop.IID_IDefinitionIdentity;
			object assemblyInformation = this._pStore.GetAssemblyInformation(Flags, DefinitionIdentity, ref iid_IDefinitionIdentity);
			return (IDefinitionIdentity)assemblyInformation;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00037929 File Offset: 0x00036929
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags)
		{
			return this.EnumAssemblies(Flags, null);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00037934 File Offset: 0x00036934
		public StoreAssemblyEnumeration EnumAssemblies(Store.EnumAssembliesFlags Flags, IReferenceIdentity refToMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));
			object obj = this._pStore.EnumAssemblies((uint)Flags, refToMatch, ref guidOfType);
			return new StoreAssemblyEnumeration((IEnumSTORE_ASSEMBLY)obj);
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0003796C File Offset: 0x0003696C
		public StoreAssemblyFileEnumeration EnumFiles(Store.EnumAssemblyFilesFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumFiles((uint)Flags, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x000379A4 File Offset: 0x000369A4
		public StoreAssemblyFileEnumeration EnumPrivateFiles(Store.EnumApplicationPrivateFiles Flags, IDefinitionAppId Application, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));
			object obj = this._pStore.EnumPrivateFiles((uint)Flags, Application, Assembly, ref guidOfType);
			return new StoreAssemblyFileEnumeration((IEnumSTORE_ASSEMBLY_FILE)obj);
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x000379E0 File Offset: 0x000369E0
		public IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE EnumInstallationReferences(Store.EnumAssemblyInstallReferenceFlags Flags, IDefinitionIdentity Assembly)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE));
			object obj = this._pStore.EnumInstallationReferences((uint)Flags, Assembly, ref guidOfType);
			return (IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE)obj;
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00037A14 File Offset: 0x00036A14
		public Store.IPathLock LockAssemblyPath(IDefinitionIdentity asm)
		{
			IntPtr c;
			string path = this._pStore.LockAssemblyPath(0U, asm, out c);
			return new Store.AssemblyPathLock(this._pStore, c, path);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00037A40 File Offset: 0x00036A40
		public Store.IPathLock LockApplicationPath(IDefinitionAppId app)
		{
			IntPtr c;
			string path = this._pStore.LockApplicationPath(0U, app, out c);
			return new Store.ApplicationPathLock(this._pStore, c, path);
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00037A6C File Offset: 0x00036A6C
		public ulong QueryChangeID(IDefinitionIdentity asm)
		{
			return this._pStore.QueryChangeID(asm);
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00037A88 File Offset: 0x00036A88
		public StoreCategoryEnumeration EnumCategories(Store.EnumCategoriesFlags Flags, IReferenceIdentity CategoryMatch)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));
			object obj = this._pStore.EnumCategories((uint)Flags, CategoryMatch, ref guidOfType);
			return new StoreCategoryEnumeration((IEnumSTORE_CATEGORY)obj);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00037AC0 File Offset: 0x00036AC0
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity CategoryMatch)
		{
			return this.EnumSubcategories(Flags, CategoryMatch, null);
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00037ACC File Offset: 0x00036ACC
		public StoreSubcategoryEnumeration EnumSubcategories(Store.EnumSubcategoriesFlags Flags, IDefinitionIdentity Category, string SearchPattern)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_SUBCATEGORY));
			object obj = this._pStore.EnumSubcategories((uint)Flags, Category, SearchPattern, ref guidOfType);
			return new StoreSubcategoryEnumeration((IEnumSTORE_CATEGORY_SUBCATEGORY)obj);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00037B08 File Offset: 0x00036B08
		public StoreCategoryInstanceEnumeration EnumCategoryInstances(Store.EnumCategoryInstancesFlags Flags, IDefinitionIdentity Category, string SubCat)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));
			object obj = this._pStore.EnumCategoryInstances((uint)Flags, Category, SubCat, ref guidOfType);
			return new StoreCategoryInstanceEnumeration((IEnumSTORE_CATEGORY_INSTANCE)obj);
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00037B44 File Offset: 0x00036B44
		public byte[] GetDeploymentProperty(Store.GetPackagePropertyFlags Flags, IDefinitionAppId Deployment, StoreApplicationReference Reference, Guid PropertySet, string PropertyName)
		{
			BLOB blob = default(BLOB);
			byte[] array = null;
			try
			{
				this._pStore.GetDeploymentProperty((uint)Flags, Deployment, ref Reference, ref PropertySet, PropertyName, out blob);
				array = new byte[blob.Size];
				Marshal.Copy(blob.BlobData, array, 0, (int)blob.Size);
			}
			finally
			{
				blob.Dispose();
			}
			return array;
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x00037BB0 File Offset: 0x00036BB0
		public StoreDeploymentMetadataEnumeration EnumInstallerDeployments(Guid InstallerId, string InstallerName, string InstallerMetadata, IReferenceAppId DeploymentFilter)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadata(0U, ref storeApplicationReference, DeploymentFilter, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA);
			return new StoreDeploymentMetadataEnumeration((IEnumSTORE_DEPLOYMENT_METADATA)obj);
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x00037BF0 File Offset: 0x00036BF0
		public StoreDeploymentMetadataPropertyEnumeration EnumInstallerDeploymentProperties(Guid InstallerId, string InstallerName, string InstallerMetadata, IDefinitionAppId Deployment)
		{
			StoreApplicationReference storeApplicationReference = new StoreApplicationReference(InstallerId, InstallerName, InstallerMetadata);
			object obj = this._pStore.EnumInstallerDeploymentMetadataProperties(0U, ref storeApplicationReference, Deployment, ref IsolationInterop.IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY);
			return new StoreDeploymentMetadataPropertyEnumeration((IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY)obj);
		}

		// Token: 0x04000901 RID: 2305
		private IStore _pStore;

		// Token: 0x02000232 RID: 562
		[Flags]
		public enum EnumAssembliesFlags
		{
			// Token: 0x04000903 RID: 2307
			Nothing = 0,
			// Token: 0x04000904 RID: 2308
			VisibleOnly = 1,
			// Token: 0x04000905 RID: 2309
			MatchServicing = 2,
			// Token: 0x04000906 RID: 2310
			ForceLibrarySemantics = 4
		}

		// Token: 0x02000233 RID: 563
		[Flags]
		public enum EnumAssemblyFilesFlags
		{
			// Token: 0x04000908 RID: 2312
			Nothing = 0,
			// Token: 0x04000909 RID: 2313
			IncludeInstalled = 1,
			// Token: 0x0400090A RID: 2314
			IncludeMissing = 2
		}

		// Token: 0x02000234 RID: 564
		[Flags]
		public enum EnumApplicationPrivateFiles
		{
			// Token: 0x0400090C RID: 2316
			Nothing = 0,
			// Token: 0x0400090D RID: 2317
			IncludeInstalled = 1,
			// Token: 0x0400090E RID: 2318
			IncludeMissing = 2
		}

		// Token: 0x02000235 RID: 565
		[Flags]
		public enum EnumAssemblyInstallReferenceFlags
		{
			// Token: 0x04000910 RID: 2320
			Nothing = 0
		}

		// Token: 0x02000236 RID: 566
		public interface IPathLock : IDisposable
		{
			// Token: 0x170002EB RID: 747
			// (get) Token: 0x060015DD RID: 5597
			string Path { get; }
		}

		// Token: 0x02000237 RID: 567
		private class AssemblyPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x060015DE RID: 5598 RVA: 0x00037C2F File Offset: 0x00036C2F
			public AssemblyPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x060015DF RID: 5599 RVA: 0x00037C57 File Offset: 0x00036C57
			private void Dispose(bool fDisposing)
			{
				if (fDisposing)
				{
					GC.SuppressFinalize(this);
				}
				if (this._pLockCookie != IntPtr.Zero)
				{
					this._pSourceStore.ReleaseAssemblyPath(this._pLockCookie);
					this._pLockCookie = IntPtr.Zero;
				}
			}

			// Token: 0x060015E0 RID: 5600 RVA: 0x00037C90 File Offset: 0x00036C90
			~AssemblyPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x060015E1 RID: 5601 RVA: 0x00037CC0 File Offset: 0x00036CC0
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x170002EC RID: 748
			// (get) Token: 0x060015E2 RID: 5602 RVA: 0x00037CC9 File Offset: 0x00036CC9
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x04000911 RID: 2321
			private IStore _pSourceStore;

			// Token: 0x04000912 RID: 2322
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x04000913 RID: 2323
			private string _path;
		}

		// Token: 0x02000238 RID: 568
		private class ApplicationPathLock : Store.IPathLock, IDisposable
		{
			// Token: 0x060015E3 RID: 5603 RVA: 0x00037CD1 File Offset: 0x00036CD1
			public ApplicationPathLock(IStore s, IntPtr c, string path)
			{
				this._pSourceStore = s;
				this._pLockCookie = c;
				this._path = path;
			}

			// Token: 0x060015E4 RID: 5604 RVA: 0x00037CF9 File Offset: 0x00036CF9
			private void Dispose(bool fDisposing)
			{
				if (fDisposing)
				{
					GC.SuppressFinalize(this);
				}
				if (this._pLockCookie != IntPtr.Zero)
				{
					this._pSourceStore.ReleaseApplicationPath(this._pLockCookie);
					this._pLockCookie = IntPtr.Zero;
				}
			}

			// Token: 0x060015E5 RID: 5605 RVA: 0x00037D34 File Offset: 0x00036D34
			~ApplicationPathLock()
			{
				this.Dispose(false);
			}

			// Token: 0x060015E6 RID: 5606 RVA: 0x00037D64 File Offset: 0x00036D64
			void IDisposable.Dispose()
			{
				this.Dispose(true);
			}

			// Token: 0x170002ED RID: 749
			// (get) Token: 0x060015E7 RID: 5607 RVA: 0x00037D6D File Offset: 0x00036D6D
			public string Path
			{
				get
				{
					return this._path;
				}
			}

			// Token: 0x04000914 RID: 2324
			private IStore _pSourceStore;

			// Token: 0x04000915 RID: 2325
			private IntPtr _pLockCookie = IntPtr.Zero;

			// Token: 0x04000916 RID: 2326
			private string _path;
		}

		// Token: 0x02000239 RID: 569
		[Flags]
		public enum EnumCategoriesFlags
		{
			// Token: 0x04000918 RID: 2328
			Nothing = 0
		}

		// Token: 0x0200023A RID: 570
		[Flags]
		public enum EnumSubcategoriesFlags
		{
			// Token: 0x0400091A RID: 2330
			Nothing = 0
		}

		// Token: 0x0200023B RID: 571
		[Flags]
		public enum EnumCategoryInstancesFlags
		{
			// Token: 0x0400091C RID: 2332
			Nothing = 0
		}

		// Token: 0x0200023C RID: 572
		[Flags]
		public enum GetPackagePropertyFlags
		{
			// Token: 0x0400091E RID: 2334
			Nothing = 0
		}
	}
}
