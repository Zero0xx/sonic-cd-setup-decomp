using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO.IsolatedStorage
{
	// Token: 0x020007AC RID: 1964
	[ComVisible(true)]
	public sealed class IsolatedStorageFile : IsolatedStorage, IDisposable
	{
		// Token: 0x060045EC RID: 17900 RVA: 0x000EE1A1 File Offset: 0x000ED1A1
		internal IsolatedStorageFile()
		{
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x000EE1A9 File Offset: 0x000ED1A9
		public static IsolatedStorageFile GetUserStoreForDomain()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x000EE1B3 File Offset: 0x000ED1B3
		public static IsolatedStorageFile GetUserStoreForAssembly()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x000EE1BD File Offset: 0x000ED1BD
		public static IsolatedStorageFile GetUserStoreForApplication()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Application, null);
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x000EE1C7 File Offset: 0x000ED1C7
		public static IsolatedStorageFile GetMachineStoreForDomain()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine, null, null);
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x000EE1D2 File Offset: 0x000ED1D2
		public static IsolatedStorageFile GetMachineStoreForAssembly()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine, null, null);
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x000EE1DD File Offset: 0x000ED1DD
		public static IsolatedStorageFile GetMachineStoreForApplication()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.Machine | IsolatedStorageScope.Application, null);
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x000EE1E8 File Offset: 0x000ED1E8
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			if (domainEvidenceType != null)
			{
				IsolatedStorageFile.DemandAdminPermission();
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, domainEvidenceType, assemblyEvidenceType);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x000EE214 File Offset: 0x000ED214
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object domainIdentity, object assemblyIdentity)
		{
			if (IsolatedStorage.IsDomain(scope) && domainIdentity == null)
			{
				throw new ArgumentNullException("domainIdentity");
			}
			if (assemblyIdentity == null)
			{
				throw new ArgumentNullException("assemblyIdentity");
			}
			IsolatedStorageFile.DemandAdminPermission();
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, domainIdentity, assemblyIdentity, null);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x000EE264 File Offset: 0x000ED264
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Evidence domainEvidence, Type domainEvidenceType, Evidence assemblyEvidence, Type assemblyEvidenceType)
		{
			if (IsolatedStorage.IsDomain(scope) && domainEvidence == null)
			{
				throw new ArgumentNullException("domainEvidence");
			}
			if (assemblyEvidence == null)
			{
				throw new ArgumentNullException("assemblyEvidence");
			}
			IsolatedStorageFile.DemandAdminPermission();
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, domainEvidence, domainEvidenceType, assemblyEvidence, assemblyEvidenceType, null, null);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x000EE2B8 File Offset: 0x000ED2B8
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type applicationEvidenceType)
		{
			if (applicationEvidenceType != null)
			{
				IsolatedStorageFile.DemandAdminPermission();
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, applicationEvidenceType);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x000EE2E4 File Offset: 0x000ED2E4
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object applicationIdentity)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			IsolatedStorageFile.DemandAdminPermission();
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, null, null, applicationIdentity);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x060045F8 RID: 17912 RVA: 0x000EE31C File Offset: 0x000ED31C
		[CLSCompliant(false)]
		public override ulong CurrentSize
		{
			get
			{
				if (base.IsRoaming())
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_CurrentSizeUndefined"));
				}
				ulong result;
				lock (this)
				{
					if (this.m_bDisposed)
					{
						throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
					}
					if (this.m_closed)
					{
						throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
					}
					if (this.m_handle == Win32Native.NULL)
					{
						this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
					}
					result = IsolatedStorageFile.nGetUsage(this.m_handle);
				}
				return result;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x000EE3CC File Offset: 0x000ED3CC
		[CLSCompliant(false)]
		public override ulong MaximumSize
		{
			get
			{
				if (base.IsRoaming())
				{
					return 9223372036854775807UL;
				}
				return base.MaximumSize;
			}
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x000EE3E8 File Offset: 0x000ED3E8
		internal unsafe void Reserve(ulong lReserve)
		{
			if (base.IsRoaming())
			{
				return;
			}
			ulong maximumSize = this.MaximumSize;
			ulong num = lReserve;
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nReserve(this.m_handle, &maximumSize, &num, false);
			}
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x000EE494 File Offset: 0x000ED494
		internal unsafe void Unreserve(ulong lFree)
		{
			if (base.IsRoaming())
			{
				return;
			}
			ulong maximumSize = this.MaximumSize;
			ulong num = lFree;
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nReserve(this.m_handle, &maximumSize, &num, true);
			}
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x000EE540 File Offset: 0x000ED540
		public void DeleteFile(string file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			FileInfo fileInfo = new FileInfo(this.GetFullPath(file));
			long num = 0L;
			this.Lock();
			try
			{
				try
				{
					num = fileInfo.Length;
					fileInfo.Delete();
				}
				catch
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteFile"));
				}
				this.Unreserve(IsolatedStorageFile.RoundToBlockSize((ulong)num));
			}
			finally
			{
				this.Unlock();
			}
			CodeAccessPermission.RevertAll();
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x000EE5E0 File Offset: 0x000ED5E0
		public void CreateDirectory(string dir)
		{
			if (dir == null)
			{
				throw new ArgumentNullException("dir");
			}
			string fullPath = this.GetFullPath(dir);
			string fullPathInternal = Path.GetFullPathInternal(fullPath);
			string[] array = this.DirectoriesToCreate(fullPathInternal);
			if (array != null && array.Length != 0)
			{
				this.Reserve((ulong)(1024L * (long)array.Length));
				this.m_fiop.Assert();
				this.m_fiop.PermitOnly();
				try
				{
					Directory.CreateDirectory(array[array.Length - 1]);
				}
				catch
				{
					this.Unreserve((ulong)(1024L * (long)array.Length));
					Directory.Delete(array[0], true);
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_CreateDirectory"));
				}
				CodeAccessPermission.RevertAll();
				return;
			}
			if (Directory.Exists(fullPath))
			{
				return;
			}
			throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_CreateDirectory"));
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x000EE6AC File Offset: 0x000ED6AC
		private string[] DirectoriesToCreate(string fullPath)
		{
			ArrayList arrayList = new ArrayList();
			int num = fullPath.Length;
			if (num >= 2 && fullPath[num - 1] == this.SeparatorExternal)
			{
				num--;
			}
			int i = Path.GetRootLength(fullPath);
			while (i < num)
			{
				i++;
				while (i < num && fullPath[i] != this.SeparatorExternal)
				{
					i++;
				}
				string text = fullPath.Substring(0, i);
				if (!Directory.InternalExists(text))
				{
					arrayList.Add(text);
				}
			}
			if (arrayList.Count != 0)
			{
				return (string[])arrayList.ToArray(typeof(string));
			}
			return null;
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x000EE744 File Offset: 0x000ED744
		public void DeleteDirectory(string dir)
		{
			if (dir == null)
			{
				throw new ArgumentNullException("dir");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			this.Lock();
			try
			{
				try
				{
					new DirectoryInfo(this.GetFullPath(dir)).Delete(false);
				}
				catch
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectory"));
				}
				this.Unreserve(1024UL);
			}
			finally
			{
				this.Unlock();
			}
			CodeAccessPermission.RevertAll();
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x000EE7D8 File Offset: 0x000ED7D8
		public string[] GetFileNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(this.GetFullPath(searchPattern), searchPattern, true);
			CodeAccessPermission.RevertAll();
			return fileDirectoryNames;
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x000EE820 File Offset: 0x000ED820
		public string[] GetDirectoryNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(this.GetFullPath(searchPattern), searchPattern, false);
			CodeAccessPermission.RevertAll();
			return fileDirectoryNames;
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x000EE868 File Offset: 0x000ED868
		public override void Remove()
		{
			string text = null;
			this.RemoveLogicalDir();
			this.Close();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(IsolatedStorageFile.GetRootDir(base.Scope));
			if (base.IsApp())
			{
				stringBuilder.Append(base.AppName);
				stringBuilder.Append(this.SeparatorExternal);
			}
			else
			{
				if (base.IsDomain())
				{
					stringBuilder.Append(base.DomainName);
					stringBuilder.Append(this.SeparatorExternal);
					text = stringBuilder.ToString();
				}
				stringBuilder.Append(base.AssemName);
				stringBuilder.Append(this.SeparatorExternal);
			}
			string text2 = stringBuilder.ToString();
			new FileIOPermission(FileIOPermissionAccess.AllAccess, text2).Assert();
			if (this.ContainsUnknownFiles(text2))
			{
				return;
			}
			try
			{
				Directory.Delete(text2, true);
			}
			catch
			{
				return;
			}
			if (base.IsDomain())
			{
				CodeAccessPermission.RevertAssert();
				new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Assert();
				if (!this.ContainsUnknownFiles(text))
				{
					try
					{
						Directory.Delete(text, true);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x000EE978 File Offset: 0x000ED978
		private void RemoveLogicalDir()
		{
			this.m_fiop.Assert();
			this.Lock();
			try
			{
				ulong lFree = base.IsRoaming() ? 0UL : this.CurrentSize;
				try
				{
					Directory.Delete(this.RootDirectory, true);
				}
				catch
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectories"));
				}
				this.Unreserve(lFree);
			}
			finally
			{
				this.Unlock();
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x000EE9F4 File Offset: 0x000ED9F4
		private bool ContainsUnknownFiles(string rootDir)
		{
			string[] fileDirectoryNames;
			string[] fileDirectoryNames2;
			try
			{
				fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", true);
				fileDirectoryNames2 = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", false);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectories"));
			}
			if (fileDirectoryNames2 != null && fileDirectoryNames2.Length > 0)
			{
				if (fileDirectoryNames2.Length > 1)
				{
					return true;
				}
				if (base.IsApp())
				{
					if (IsolatedStorageFile.NotAppFilesDir(fileDirectoryNames2[0]))
					{
						return true;
					}
				}
				else if (base.IsDomain())
				{
					if (IsolatedStorageFile.NotFilesDir(fileDirectoryNames2[0]))
					{
						return true;
					}
				}
				else if (IsolatedStorageFile.NotAssemFilesDir(fileDirectoryNames2[0]))
				{
					return true;
				}
			}
			if (fileDirectoryNames == null || fileDirectoryNames.Length == 0)
			{
				return false;
			}
			if (base.IsRoaming())
			{
				return fileDirectoryNames.Length > 1 || IsolatedStorageFile.NotIDFile(fileDirectoryNames[0]);
			}
			return fileDirectoryNames.Length > 2 || (IsolatedStorageFile.NotIDFile(fileDirectoryNames[0]) && IsolatedStorageFile.NotInfoFile(fileDirectoryNames[0])) || (fileDirectoryNames.Length == 2 && IsolatedStorageFile.NotIDFile(fileDirectoryNames[1]) && IsolatedStorageFile.NotInfoFile(fileDirectoryNames[1]));
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x000EEAF4 File Offset: 0x000EDAF4
		public void Close()
		{
			if (base.IsRoaming())
			{
				return;
			}
			lock (this)
			{
				if (!this.m_closed)
				{
					this.m_closed = true;
					IntPtr handle = this.m_handle;
					this.m_handle = Win32Native.NULL;
					IsolatedStorageFile.nClose(handle);
					GC.nativeSuppressFinalize(this);
				}
			}
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x000EEB58 File Offset: 0x000EDB58
		public void Dispose()
		{
			this.Close();
			this.m_bDisposed = true;
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x000EEB68 File Offset: 0x000EDB68
		~IsolatedStorageFile()
		{
			this.Dispose();
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x000EEB94 File Offset: 0x000EDB94
		private static bool NotIDFile(string file)
		{
			return string.Compare(file, "identity.dat", StringComparison.Ordinal) != 0;
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x000EEBA8 File Offset: 0x000EDBA8
		private static bool NotInfoFile(string file)
		{
			return string.Compare(file, "info.dat", StringComparison.Ordinal) != 0 && string.Compare(file, "appinfo.dat", StringComparison.Ordinal) != 0;
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x000EEBCC File Offset: 0x000EDBCC
		private static bool NotFilesDir(string dir)
		{
			return string.Compare(dir, "Files", StringComparison.Ordinal) != 0;
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x000EEBE0 File Offset: 0x000EDBE0
		internal static bool NotAssemFilesDir(string dir)
		{
			return string.Compare(dir, "AssemFiles", StringComparison.Ordinal) != 0;
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x000EEBF4 File Offset: 0x000EDBF4
		internal static bool NotAppFilesDir(string dir)
		{
			return string.Compare(dir, "AppFiles", StringComparison.Ordinal) != 0;
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x000EEC08 File Offset: 0x000EDC08
		public static void Remove(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.VerifyGlobalScope(scope);
			IsolatedStorageFile.DemandAdminPermission();
			string rootDir = IsolatedStorageFile.GetRootDir(scope);
			new FileIOPermission(FileIOPermissionAccess.Write, rootDir).Assert();
			try
			{
				Directory.Delete(rootDir, true);
				Directory.CreateDirectory(rootDir);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectories"));
			}
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x000EEC64 File Offset: 0x000EDC64
		public static IEnumerator GetEnumerator(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.VerifyGlobalScope(scope);
			IsolatedStorageFile.DemandAdminPermission();
			return new IsolatedStorageFileEnumerator(scope);
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x0600460F RID: 17935 RVA: 0x000EEC77 File Offset: 0x000EDC77
		internal string RootDirectory
		{
			get
			{
				return this.m_RootDir;
			}
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x000EEC80 File Offset: 0x000EDC80
		internal string GetFullPath(string path)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.RootDirectory);
			if (path[0] == this.SeparatorExternal)
			{
				stringBuilder.Append(path.Substring(1));
			}
			else
			{
				stringBuilder.Append(path);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x000EECD0 File Offset: 0x000EDCD0
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static string GetDataDirectoryFromActivationContext()
		{
			if (IsolatedStorageFile.s_appDataDir == null)
			{
				ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
				if (activationContext == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationMissingIdentity"));
				}
				string text = activationContext.DataDirectory;
				if (text != null && text[text.Length - 1] != '\\')
				{
					text += "\\";
				}
				IsolatedStorageFile.s_appDataDir = text;
			}
			return IsolatedStorageFile.s_appDataDir;
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x000EED38 File Offset: 0x000EDD38
		internal void Init(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.GetGlobalFileIOPerm(scope).Assert();
			StringBuilder stringBuilder = new StringBuilder();
			if (IsolatedStorage.IsApp(scope))
			{
				stringBuilder.Append(IsolatedStorageFile.GetRootDir(scope));
				if (IsolatedStorageFile.s_appDataDir == null)
				{
					stringBuilder.Append(base.AppName);
					stringBuilder.Append(this.SeparatorExternal);
				}
				try
				{
					Directory.CreateDirectory(stringBuilder.ToString());
				}
				catch
				{
				}
				this.CreateIDFile(stringBuilder.ToString(), scope);
				this.m_InfoFile = stringBuilder.ToString() + "appinfo.dat";
				stringBuilder.Append("AppFiles");
			}
			else
			{
				stringBuilder.Append(IsolatedStorageFile.GetRootDir(scope));
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append(base.DomainName);
					stringBuilder.Append(this.SeparatorExternal);
					try
					{
						Directory.CreateDirectory(stringBuilder.ToString());
						this.CreateIDFile(stringBuilder.ToString(), scope);
					}
					catch
					{
					}
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
				}
				stringBuilder.Append(base.AssemName);
				stringBuilder.Append(this.SeparatorExternal);
				try
				{
					Directory.CreateDirectory(stringBuilder.ToString());
					this.CreateIDFile(stringBuilder.ToString(), scope);
				}
				catch
				{
				}
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append("Files");
				}
				else
				{
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
					stringBuilder.Append("AssemFiles");
				}
			}
			stringBuilder.Append(this.SeparatorExternal);
			string text = stringBuilder.ToString();
			try
			{
				Directory.CreateDirectory(text);
			}
			catch
			{
			}
			this.m_RootDir = text;
			this.m_fiop = new FileIOPermission(FileIOPermissionAccess.AllAccess, text);
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x000EEF10 File Offset: 0x000EDF10
		internal bool InitExistingStore(IsolatedStorageScope scope)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(IsolatedStorageFile.GetRootDir(scope));
			if (IsolatedStorage.IsApp(scope))
			{
				stringBuilder.Append(base.AppName);
				stringBuilder.Append(this.SeparatorExternal);
				this.m_InfoFile = stringBuilder.ToString() + "appinfo.dat";
				stringBuilder.Append("AppFiles");
			}
			else
			{
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append(base.DomainName);
					stringBuilder.Append(this.SeparatorExternal);
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
				}
				stringBuilder.Append(base.AssemName);
				stringBuilder.Append(this.SeparatorExternal);
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append("Files");
				}
				else
				{
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
					stringBuilder.Append("AssemFiles");
				}
			}
			stringBuilder.Append(this.SeparatorExternal);
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, stringBuilder.ToString());
			fileIOPermission.Assert();
			if (!Directory.Exists(stringBuilder.ToString()))
			{
				return false;
			}
			this.m_RootDir = stringBuilder.ToString();
			this.m_fiop = fileIOPermission;
			return true;
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x000EF047 File Offset: 0x000EE047
		protected override IsolatedStoragePermission GetPermission(PermissionSet ps)
		{
			if (ps == null)
			{
				return null;
			}
			if (ps.IsUnrestricted())
			{
				return new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			}
			return (IsolatedStoragePermission)ps.GetPermission(typeof(IsolatedStorageFilePermission));
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x000EF072 File Offset: 0x000EE072
		internal void UndoReserveOperation(ulong oldLen, ulong newLen)
		{
			oldLen = IsolatedStorageFile.RoundToBlockSize(oldLen);
			if (newLen > oldLen)
			{
				this.Unreserve(IsolatedStorageFile.RoundToBlockSize(newLen - oldLen));
			}
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x000EF08E File Offset: 0x000EE08E
		internal void Reserve(ulong oldLen, ulong newLen)
		{
			oldLen = IsolatedStorageFile.RoundToBlockSize(oldLen);
			if (newLen > oldLen)
			{
				this.Reserve(IsolatedStorageFile.RoundToBlockSize(newLen - oldLen));
			}
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x000EF0AA File Offset: 0x000EE0AA
		internal void ReserveOneBlock()
		{
			this.Reserve(1024UL);
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x000EF0B8 File Offset: 0x000EE0B8
		internal void UnreserveOneBlock()
		{
			this.Unreserve(1024UL);
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x000EF0C8 File Offset: 0x000EE0C8
		internal static ulong RoundToBlockSize(ulong num)
		{
			if (num < 1024UL)
			{
				return 1024UL;
			}
			ulong num2 = num % 1024UL;
			if (num2 != 0UL)
			{
				num += 1024UL - num2;
			}
			return num;
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x000EF100 File Offset: 0x000EE100
		internal static string GetRootDir(IsolatedStorageScope scope)
		{
			if (IsolatedStorage.IsRoaming(scope))
			{
				if (IsolatedStorageFile.s_RootDirRoaming == null)
				{
					IsolatedStorageFile.s_RootDirRoaming = IsolatedStorageFile.nGetRootDir(scope);
				}
				return IsolatedStorageFile.s_RootDirRoaming;
			}
			if (IsolatedStorage.IsMachine(scope))
			{
				if (IsolatedStorageFile.s_RootDirMachine == null)
				{
					IsolatedStorageFile.InitGlobalsMachine(scope);
				}
				return IsolatedStorageFile.s_RootDirMachine;
			}
			if (IsolatedStorageFile.s_RootDirUser == null)
			{
				IsolatedStorageFile.InitGlobalsNonRoamingUser(scope);
			}
			return IsolatedStorageFile.s_RootDirUser;
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x000EF15C File Offset: 0x000EE15C
		private static void InitGlobalsMachine(IsolatedStorageScope scope)
		{
			string text = IsolatedStorageFile.nGetRootDir(scope);
			new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Assert();
			string text2 = IsolatedStorageFile.GetMachineRandomDirectory(text);
			if (text2 == null)
			{
				Mutex mutex = IsolatedStorageFile.CreateMutexNotOwned(text);
				if (!mutex.WaitOne())
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
				}
				try
				{
					text2 = IsolatedStorageFile.GetMachineRandomDirectory(text);
					if (text2 == null)
					{
						string randomFileName = Path.GetRandomFileName();
						string randomFileName2 = Path.GetRandomFileName();
						try
						{
							IsolatedStorageFile.nCreateDirectoryWithDacl(text + randomFileName);
							IsolatedStorageFile.nCreateDirectoryWithDacl(text + randomFileName + "\\" + randomFileName2);
						}
						catch
						{
							throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
						}
						text2 = randomFileName + "\\" + randomFileName2;
					}
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}
			IsolatedStorageFile.s_RootDirMachine = text + text2 + "\\";
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x000EF234 File Offset: 0x000EE234
		private static void InitGlobalsNonRoamingUser(IsolatedStorageScope scope)
		{
			string text = null;
			if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Application))
			{
				text = IsolatedStorageFile.GetDataDirectoryFromActivationContext();
				if (text != null)
				{
					IsolatedStorageFile.s_RootDirUser = text;
					return;
				}
			}
			text = IsolatedStorageFile.nGetRootDir(scope);
			new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Assert();
			bool flag = false;
			string oldRandomDirectory = null;
			string text2 = IsolatedStorageFile.GetRandomDirectory(text, out flag, out oldRandomDirectory);
			if (text2 == null)
			{
				Mutex mutex = IsolatedStorageFile.CreateMutexNotOwned(text);
				if (!mutex.WaitOne())
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
				}
				try
				{
					text2 = IsolatedStorageFile.GetRandomDirectory(text, out flag, out oldRandomDirectory);
					if (text2 == null)
					{
						if (flag)
						{
							text2 = IsolatedStorageFile.MigrateOldIsoStoreDirectory(text, oldRandomDirectory);
						}
						else
						{
							text2 = IsolatedStorageFile.CreateRandomDirectory(text);
						}
					}
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}
			IsolatedStorageFile.s_RootDirUser = text + text2 + "\\";
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x000EF2EC File Offset: 0x000EE2EC
		internal static string MigrateOldIsoStoreDirectory(string rootDir, string oldRandomDirectory)
		{
			string randomFileName = Path.GetRandomFileName();
			string randomFileName2 = Path.GetRandomFileName();
			string text = rootDir + randomFileName;
			string destDirName = text + "\\" + randomFileName2;
			try
			{
				Directory.CreateDirectory(text);
				Directory.Move(rootDir + oldRandomDirectory, destDirName);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
			}
			return randomFileName + "\\" + randomFileName2;
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x000EF360 File Offset: 0x000EE360
		internal static string CreateRandomDirectory(string rootDir)
		{
			string text = Path.GetRandomFileName() + "\\" + Path.GetRandomFileName();
			try
			{
				Directory.CreateDirectory(rootDir + text);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
			}
			return text;
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x000EF3B4 File Offset: 0x000EE3B4
		internal static string GetRandomDirectory(string rootDir, out bool bMigrateNeeded, out string sOldStoreLocation)
		{
			bMigrateNeeded = false;
			sOldStoreLocation = null;
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", false);
			for (int i = 0; i < fileDirectoryNames.Length; i++)
			{
				if (fileDirectoryNames[i].Length == 12)
				{
					string[] fileDirectoryNames2 = IsolatedStorageFile.GetFileDirectoryNames(rootDir + fileDirectoryNames[i] + "\\*", "*", false);
					for (int j = 0; j < fileDirectoryNames2.Length; j++)
					{
						if (fileDirectoryNames2[j].Length == 12)
						{
							return fileDirectoryNames[i] + "\\" + fileDirectoryNames2[j];
						}
					}
				}
			}
			for (int k = 0; k < fileDirectoryNames.Length; k++)
			{
				if (fileDirectoryNames[k].Length == 24)
				{
					bMigrateNeeded = true;
					sOldStoreLocation = fileDirectoryNames[k];
					return null;
				}
			}
			return null;
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x000EF468 File Offset: 0x000EE468
		internal static string GetMachineRandomDirectory(string rootDir)
		{
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", false);
			for (int i = 0; i < fileDirectoryNames.Length; i++)
			{
				if (fileDirectoryNames[i].Length == 12)
				{
					string[] fileDirectoryNames2 = IsolatedStorageFile.GetFileDirectoryNames(rootDir + fileDirectoryNames[i] + "\\*", "*", false);
					for (int j = 0; j < fileDirectoryNames2.Length; j++)
					{
						if (fileDirectoryNames2[j].Length == 12)
						{
							return fileDirectoryNames[i] + "\\" + fileDirectoryNames2[j];
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x000EF4EC File Offset: 0x000EE4EC
		internal static Mutex CreateMutexNotOwned(string pathName)
		{
			return new Mutex(false, "Global\\" + IsolatedStorageFile.GetStrongHashSuitableForObjectName(pathName));
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x000EF504 File Offset: 0x000EE504
		internal static string GetStrongHashSuitableForObjectName(string name)
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryWriter(memoryStream).Write(name.ToUpper(CultureInfo.InvariantCulture));
			memoryStream.Position = 0L;
			return IsolatedStorage.ToBase32StringSuitableForDirName(new SHA1CryptoServiceProvider().ComputeHash(memoryStream));
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x000EF545 File Offset: 0x000EE545
		private string GetSyncObjectName()
		{
			if (this.m_SyncObjectName == null)
			{
				this.m_SyncObjectName = IsolatedStorageFile.GetStrongHashSuitableForObjectName(this.m_InfoFile);
			}
			return this.m_SyncObjectName;
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x000EF568 File Offset: 0x000EE568
		internal void Lock()
		{
			if (base.IsRoaming())
			{
				return;
			}
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nLock(this.m_handle, true);
			}
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x000EF608 File Offset: 0x000EE608
		internal void Unlock()
		{
			if (base.IsRoaming())
			{
				return;
			}
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nLock(this.m_handle, false);
			}
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x000EF6A8 File Offset: 0x000EE6A8
		internal static FileIOPermission GetGlobalFileIOPerm(IsolatedStorageScope scope)
		{
			if (IsolatedStorage.IsRoaming(scope))
			{
				if (IsolatedStorageFile.s_PermRoaming == null)
				{
					IsolatedStorageFile.s_PermRoaming = new FileIOPermission(FileIOPermissionAccess.AllAccess, IsolatedStorageFile.GetRootDir(scope));
				}
				return IsolatedStorageFile.s_PermRoaming;
			}
			if (IsolatedStorage.IsMachine(scope))
			{
				if (IsolatedStorageFile.s_PermMachine == null)
				{
					IsolatedStorageFile.s_PermMachine = new FileIOPermission(FileIOPermissionAccess.AllAccess, IsolatedStorageFile.GetRootDir(scope));
				}
				return IsolatedStorageFile.s_PermMachine;
			}
			if (IsolatedStorageFile.s_PermUser == null)
			{
				IsolatedStorageFile.s_PermUser = new FileIOPermission(FileIOPermissionAccess.AllAccess, IsolatedStorageFile.GetRootDir(scope));
			}
			return IsolatedStorageFile.s_PermUser;
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x000EF721 File Offset: 0x000EE721
		private static void DemandAdminPermission()
		{
			if (IsolatedStorageFile.s_PermAdminUser == null)
			{
				IsolatedStorageFile.s_PermAdminUser = new IsolatedStorageFilePermission(IsolatedStorageContainment.AdministerIsolatedStorageByUser, 0L, false);
			}
			IsolatedStorageFile.s_PermAdminUser.Demand();
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x000EF743 File Offset: 0x000EE743
		internal static void VerifyGlobalScope(IsolatedStorageScope scope)
		{
			if (scope != IsolatedStorageScope.User && scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming) && scope != IsolatedStorageScope.Machine)
			{
				throw new ArgumentException(Environment.GetResourceString("IsolatedStorage_Scope_U_R_M"));
			}
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x000EF764 File Offset: 0x000EE764
		internal void CreateIDFile(string path, IsolatedStorageScope scope)
		{
			try
			{
				using (FileStream fileStream = new FileStream(path + "identity.dat", FileMode.OpenOrCreate))
				{
					MemoryStream identityStream = base.GetIdentityStream(scope);
					byte[] buffer = identityStream.GetBuffer();
					fileStream.Write(buffer, 0, (int)identityStream.Length);
					identityStream.Close();
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x000EF7D4 File Offset: 0x000EE7D4
		private static string[] GetFileDirectoryNames(string path, string msg, bool file)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", Environment.GetResourceString("ArgumentNull_Path"));
			}
			bool flag = false;
			char c = path[path.Length - 1];
			if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == '.')
			{
				flag = true;
			}
			string text = Path.GetFullPathInternal(path);
			if (flag && text[text.Length - 1] != c)
			{
				text += "\\*";
			}
			string text2 = Path.GetDirectoryName(text);
			if (text2 != null)
			{
				text2 += "\\";
			}
			new FileIOPermission(FileIOPermissionAccess.Read, (text2 == null) ? text : text2).Demand();
			string[] array = new string[10];
			int num = 0;
			Win32Native.WIN32_FIND_DATA win32_FIND_DATA = new Win32Native.WIN32_FIND_DATA();
			SafeFindHandle safeFindHandle = Win32Native.FindFirstFile(text, win32_FIND_DATA);
			int lastWin32Error;
			if (safeFindHandle.IsInvalid)
			{
				lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 2)
				{
					return new string[0];
				}
				__Error.WinIOError(lastWin32Error, msg);
			}
			int num2 = 0;
			do
			{
				bool flag2;
				if (file)
				{
					flag2 = (0 == (win32_FIND_DATA.dwFileAttributes & 16));
				}
				else
				{
					flag2 = (0 != (win32_FIND_DATA.dwFileAttributes & 16));
					if (flag2 && (win32_FIND_DATA.cFileName.Equals(".") || win32_FIND_DATA.cFileName.Equals("..")))
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					num2++;
					if (num == array.Length)
					{
						string[] array2 = new string[array.Length * 2];
						Array.Copy(array, 0, array2, 0, num);
						array = array2;
					}
					array[num++] = win32_FIND_DATA.cFileName;
				}
			}
			while (Win32Native.FindNextFile(safeFindHandle, win32_FIND_DATA));
			lastWin32Error = Marshal.GetLastWin32Error();
			safeFindHandle.Close();
			if (lastWin32Error != 0 && lastWin32Error != 18)
			{
				__Error.WinIOError(lastWin32Error, msg);
			}
			if (!file && num2 == 1 && (win32_FIND_DATA.dwFileAttributes & 16) != 0)
			{
				return new string[]
				{
					win32_FIND_DATA.cFileName
				};
			}
			if (num == array.Length)
			{
				return array;
			}
			string[] array3 = new string[num];
			Array.Copy(array, 0, array3, 0, num);
			return array3;
		}

		// Token: 0x0600462B RID: 17963
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong nGetUsage(IntPtr handle);

		// Token: 0x0600462C RID: 17964
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr nOpen(string infoFile, string syncName);

		// Token: 0x0600462D RID: 17965
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nClose(IntPtr handle);

		// Token: 0x0600462E RID: 17966
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void nReserve(IntPtr handle, ulong* plQuota, ulong* plReserve, bool fFree);

		// Token: 0x0600462F RID: 17967
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nGetRootDir(IsolatedStorageScope scope);

		// Token: 0x06004630 RID: 17968
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nLock(IntPtr handle, bool fLock);

		// Token: 0x06004631 RID: 17969
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nCreateDirectoryWithDacl(string path);

		// Token: 0x040022D3 RID: 8915
		private const int s_BlockSize = 1024;

		// Token: 0x040022D4 RID: 8916
		private const int s_DirSize = 1024;

		// Token: 0x040022D5 RID: 8917
		private const string s_name = "file.store";

		// Token: 0x040022D6 RID: 8918
		internal const string s_Files = "Files";

		// Token: 0x040022D7 RID: 8919
		internal const string s_AssemFiles = "AssemFiles";

		// Token: 0x040022D8 RID: 8920
		internal const string s_AppFiles = "AppFiles";

		// Token: 0x040022D9 RID: 8921
		internal const string s_IDFile = "identity.dat";

		// Token: 0x040022DA RID: 8922
		internal const string s_InfoFile = "info.dat";

		// Token: 0x040022DB RID: 8923
		internal const string s_AppInfoFile = "appinfo.dat";

		// Token: 0x040022DC RID: 8924
		private static string s_RootDirUser;

		// Token: 0x040022DD RID: 8925
		private static string s_RootDirMachine;

		// Token: 0x040022DE RID: 8926
		private static string s_RootDirRoaming;

		// Token: 0x040022DF RID: 8927
		private static string s_appDataDir;

		// Token: 0x040022E0 RID: 8928
		private static FileIOPermission s_PermUser;

		// Token: 0x040022E1 RID: 8929
		private static FileIOPermission s_PermMachine;

		// Token: 0x040022E2 RID: 8930
		private static FileIOPermission s_PermRoaming;

		// Token: 0x040022E3 RID: 8931
		private static IsolatedStorageFilePermission s_PermAdminUser;

		// Token: 0x040022E4 RID: 8932
		private FileIOPermission m_fiop;

		// Token: 0x040022E5 RID: 8933
		private string m_RootDir;

		// Token: 0x040022E6 RID: 8934
		private string m_InfoFile;

		// Token: 0x040022E7 RID: 8935
		private string m_SyncObjectName;

		// Token: 0x040022E8 RID: 8936
		private IntPtr m_handle;

		// Token: 0x040022E9 RID: 8937
		private bool m_closed;

		// Token: 0x040022EA RID: 8938
		private bool m_bDisposed;
	}
}
