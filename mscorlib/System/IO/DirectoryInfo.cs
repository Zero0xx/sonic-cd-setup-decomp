using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x020005AC RID: 1452
	[ComVisible(true)]
	[Serializable]
	public sealed class DirectoryInfo : FileSystemInfo
	{
		// Token: 0x06003574 RID: 13684 RVA: 0x000B2140 File Offset: 0x000B1140
		public DirectoryInfo(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 2 && path[1] == ':')
			{
				this.OriginalPath = ".";
			}
			else
			{
				this.OriginalPath = path;
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			this.demandDir = new string[]
			{
				Directory.GetDemandDir(fullPathInternal, true)
			};
			new FileIOPermission(FileIOPermissionAccess.Read, this.demandDir, false, false).Demand();
			this.FullPath = fullPathInternal;
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000B21C4 File Offset: 0x000B11C4
		internal DirectoryInfo(string fullPath, bool junk)
		{
			this.OriginalPath = Path.GetFileName(fullPath);
			this.FullPath = fullPath;
			this.demandDir = new string[]
			{
				Directory.GetDemandDir(fullPath, true)
			};
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000B2204 File Offset: 0x000B1204
		private DirectoryInfo(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.demandDir = new string[]
			{
				Directory.GetDemandDir(this.FullPath, true)
			};
			new FileIOPermission(FileIOPermissionAccess.Read, this.demandDir, false, false).Demand();
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x000B224C File Offset: 0x000B124C
		public override string Name
		{
			get
			{
				string text = this.FullPath;
				if (text.Length > 3)
				{
					if (text.EndsWith(Path.DirectorySeparatorChar))
					{
						text = this.FullPath.Substring(0, this.FullPath.Length - 1);
					}
					return Path.GetFileName(text);
				}
				return this.FullPath;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x000B22A0 File Offset: 0x000B12A0
		public DirectoryInfo Parent
		{
			get
			{
				string text = this.FullPath;
				if (text.Length > 3 && text.EndsWith(Path.DirectorySeparatorChar))
				{
					text = this.FullPath.Substring(0, this.FullPath.Length - 1);
				}
				string directoryName = Path.GetDirectoryName(text);
				if (directoryName == null)
				{
					return null;
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(directoryName, false);
				new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, directoryInfo.demandDir, false, false).Demand();
				return directoryInfo;
			}
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000B230D File Offset: 0x000B130D
		public DirectoryInfo CreateSubdirectory(string path)
		{
			return this.CreateSubdirectory(path, null);
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000B2318 File Offset: 0x000B1318
		public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string path2 = Path.InternalCombine(this.FullPath, path);
			string fullPathInternal = Path.GetFullPathInternal(path2);
			if (string.Compare(this.FullPath, 0, fullPathInternal, 0, this.FullPath.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				string displayablePath = __Error.GetDisplayablePath(this.OriginalPath, false);
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidSubPath"), new object[]
				{
					path,
					displayablePath
				}));
			}
			string text = Directory.GetDemandDir(fullPathInternal, true);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[]
			{
				text
			}, false, false).Demand();
			Directory.InternalCreateDirectory(fullPathInternal, path, directorySecurity);
			return new DirectoryInfo(fullPathInternal);
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000B23D0 File Offset: 0x000B13D0
		public void Create()
		{
			Directory.InternalCreateDirectory(this.FullPath, this.OriginalPath, null);
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000B23E4 File Offset: 0x000B13E4
		public void Create(DirectorySecurity directorySecurity)
		{
			Directory.InternalCreateDirectory(this.FullPath, this.OriginalPath, directorySecurity);
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x0600357D RID: 13693 RVA: 0x000B23F8 File Offset: 0x000B13F8
		public override bool Exists
		{
			get
			{
				bool result;
				try
				{
					if (this._dataInitialised == -1)
					{
						base.Refresh();
					}
					if (this._dataInitialised != 0)
					{
						result = false;
					}
					else
					{
						result = (this._data.fileAttributes != -1 && (this._data.fileAttributes & 16) != 0);
					}
				}
				catch
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000B2460 File Offset: 0x000B1460
		public DirectorySecurity GetAccessControl()
		{
			return Directory.GetAccessControl(this.FullPath, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000B246F File Offset: 0x000B146F
		public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
		{
			return Directory.GetAccessControl(this.FullPath, includeSections);
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000B247D File Offset: 0x000B147D
		public void SetAccessControl(DirectorySecurity directorySecurity)
		{
			Directory.SetAccessControl(this.FullPath, directorySecurity);
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000B248B File Offset: 0x000B148B
		public FileInfo[] GetFiles(string searchPattern)
		{
			return this.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000B2498 File Offset: 0x000B1498
		private string FixupFileDirFullPath(string fileDirUserPath)
		{
			string result;
			if (this.OriginalPath.Length == 0)
			{
				result = Path.InternalCombine(this.FullPath, fileDirUserPath);
			}
			else if (this.OriginalPath.EndsWith(Path.DirectorySeparatorChar) || this.OriginalPath.EndsWith(Path.AltDirectorySeparatorChar))
			{
				result = Path.InternalCombine(this.FullPath, fileDirUserPath.Substring(this.OriginalPath.Length));
			}
			else
			{
				result = Path.InternalCombine(this.FullPath, fileDirUserPath.Substring(this.OriginalPath.Length + 1));
			}
			return result;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000B2524 File Offset: 0x000B1524
		public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] array = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, true, false, searchOption);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FixupFileDirFullPath(array[i]);
			}
			if (array.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array, false, false).Demand();
			}
			FileInfo[] array2 = new FileInfo[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = new FileInfo(array[j], false);
			}
			return array2;
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000B25A6 File Offset: 0x000B15A6
		public FileInfo[] GetFiles()
		{
			return this.GetFiles("*");
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000B25B3 File Offset: 0x000B15B3
		public DirectoryInfo[] GetDirectories()
		{
			return this.GetDirectories("*");
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x000B25C0 File Offset: 0x000B15C0
		public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
		{
			return this.GetFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000B25CC File Offset: 0x000B15CC
		private FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] array = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, false, true, searchOption);
			string[] array2 = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, true, false, searchOption);
			FileSystemInfo[] array3 = new FileSystemInfo[array.Length + array2.Length];
			string[] array4 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FixupFileDirFullPath(array[i]);
				array4[i] = array[i] + "\\.";
			}
			if (array.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array4, false, false).Demand();
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = this.FixupFileDirFullPath(array2[j]);
			}
			if (array2.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array2, false, false).Demand();
			}
			int num = 0;
			for (int k = 0; k < array.Length; k++)
			{
				array3[num++] = new DirectoryInfo(array[k], false);
			}
			for (int l = 0; l < array2.Length; l++)
			{
				array3[num++] = new FileInfo(array2[l], false);
			}
			return array3;
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000B26EF File Offset: 0x000B16EF
		public FileSystemInfo[] GetFileSystemInfos()
		{
			return this.GetFileSystemInfos("*");
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000B26FC File Offset: 0x000B16FC
		public DirectoryInfo[] GetDirectories(string searchPattern)
		{
			return this.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000B2708 File Offset: 0x000B1708
		public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] array = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, false, true, searchOption);
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FixupFileDirFullPath(array[i]);
				array2[i] = array[i] + "\\.";
			}
			if (array.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array2, false, false).Demand();
			}
			DirectoryInfo[] array3 = new DirectoryInfo[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array3[j] = new DirectoryInfo(array[j], false);
			}
			return array3;
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x0600358B RID: 13707 RVA: 0x000B27AC File Offset: 0x000B17AC
		public DirectoryInfo Root
		{
			get
			{
				int rootLength = Path.GetRootLength(this.FullPath);
				string text = this.FullPath.Substring(0, rootLength);
				string text2 = Directory.GetDemandDir(text, true);
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[]
				{
					text2
				}, false, false).Demand();
				return new DirectoryInfo(text);
			}
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000B27FC File Offset: 0x000B17FC
		public void MoveTo(string destDirName)
		{
			if (destDirName == null)
			{
				throw new ArgumentNullException("destDirName");
			}
			if (destDirName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destDirName");
			}
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, this.demandDir, false, false).Demand();
			string text = Path.GetFullPathInternal(destDirName);
			if (!text.EndsWith(Path.DirectorySeparatorChar))
			{
				text += Path.DirectorySeparatorChar;
			}
			string path = text + '.';
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, path).Demand();
			string text2;
			if (this.FullPath.EndsWith(Path.DirectorySeparatorChar))
			{
				text2 = this.FullPath;
			}
			else
			{
				text2 = this.FullPath + Path.DirectorySeparatorChar;
			}
			if (CultureInfo.InvariantCulture.CompareInfo.Compare(text2, text, CompareOptions.IgnoreCase) == 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SourceDestMustBeDifferent"));
			}
			string pathRoot = Path.GetPathRoot(text2);
			string pathRoot2 = Path.GetPathRoot(text);
			if (CultureInfo.InvariantCulture.CompareInfo.Compare(pathRoot, pathRoot2, CompareOptions.IgnoreCase) != 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SourceDestMustHaveSameRoot"));
			}
			if (Environment.IsWin9X() && !Directory.InternalExists(this.FullPath))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.PathNotFound_Path"), new object[]
				{
					destDirName
				}));
			}
			if (!Win32Native.MoveFile(this.FullPath, destDirName))
			{
				int num = Marshal.GetLastWin32Error();
				if (num == 2)
				{
					num = 3;
					__Error.WinIOError(num, this.OriginalPath);
				}
				if (num == 5)
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[]
					{
						this.OriginalPath
					}));
				}
				__Error.WinIOError(num, string.Empty);
			}
			this.FullPath = text;
			this.OriginalPath = destDirName;
			this.demandDir = new string[]
			{
				Directory.GetDemandDir(this.FullPath, true)
			};
			this._dataInitialised = -1;
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000B29EA File Offset: 0x000B19EA
		public override void Delete()
		{
			Directory.Delete(this.FullPath, this.OriginalPath, false);
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000B29FE File Offset: 0x000B19FE
		public void Delete(bool recursive)
		{
			Directory.Delete(this.FullPath, this.OriginalPath, recursive);
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000B2A12 File Offset: 0x000B1A12
		public override string ToString()
		{
			return this.OriginalPath;
		}

		// Token: 0x04001C20 RID: 7200
		private string[] demandDir;
	}
}
