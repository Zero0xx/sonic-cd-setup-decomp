using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x020005B6 RID: 1462
	[ComVisible(true)]
	[Serializable]
	public sealed class FileInfo : FileSystemInfo
	{
		// Token: 0x060035E7 RID: 13799 RVA: 0x000B3EB8 File Offset: 0x000B2EB8
		public FileInfo(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.OriginalPath = fileName;
			string fullPathInternal = Path.GetFullPathInternal(fileName);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			this._name = Path.GetFileName(fileName);
			this.FullPath = fullPathInternal;
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x000B3F14 File Offset: 0x000B2F14
		private FileInfo(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			new FileIOPermission(FileIOPermissionAccess.Read, new string[]
			{
				this.FullPath
			}, false, false).Demand();
			this._name = Path.GetFileName(this.OriginalPath);
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x000B3F58 File Offset: 0x000B2F58
		internal FileInfo(string fullPath, bool ignoreThis)
		{
			this._name = Path.GetFileName(fullPath);
			this.OriginalPath = this._name;
			this.FullPath = fullPath;
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060035EA RID: 13802 RVA: 0x000B3F7F File Offset: 0x000B2F7F
		public override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060035EB RID: 13803 RVA: 0x000B3F88 File Offset: 0x000B2F88
		public long Length
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					base.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				if ((this._data.fileAttributes & 16) != 0)
				{
					__Error.WinIOError(2, this.OriginalPath);
				}
				return (long)this._data.fileSizeHigh << 32 | ((long)this._data.fileSizeLow & (long)((ulong)-1));
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060035EC RID: 13804 RVA: 0x000B3FF8 File Offset: 0x000B2FF8
		public string DirectoryName
		{
			get
			{
				string directoryName = Path.GetDirectoryName(this.FullPath);
				if (directoryName != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[]
					{
						directoryName
					}, false, false).Demand();
				}
				return directoryName;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060035ED RID: 13805 RVA: 0x000B4030 File Offset: 0x000B3030
		public DirectoryInfo Directory
		{
			get
			{
				string directoryName = this.DirectoryName;
				if (directoryName == null)
				{
					return null;
				}
				return new DirectoryInfo(directoryName);
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060035EE RID: 13806 RVA: 0x000B404F File Offset: 0x000B304F
		// (set) Token: 0x060035EF RID: 13807 RVA: 0x000B405F File Offset: 0x000B305F
		public bool IsReadOnly
		{
			get
			{
				return (base.Attributes & FileAttributes.ReadOnly) != (FileAttributes)0;
			}
			set
			{
				if (value)
				{
					base.Attributes |= FileAttributes.ReadOnly;
					return;
				}
				base.Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000B4082 File Offset: 0x000B3082
		public FileSecurity GetAccessControl()
		{
			return File.GetAccessControl(this.FullPath, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000B4091 File Offset: 0x000B3091
		public FileSecurity GetAccessControl(AccessControlSections includeSections)
		{
			return File.GetAccessControl(this.FullPath, includeSections);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000B409F File Offset: 0x000B309F
		public void SetAccessControl(FileSecurity fileSecurity)
		{
			File.SetAccessControl(this.FullPath, fileSecurity);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000B40AD File Offset: 0x000B30AD
		public StreamReader OpenText()
		{
			return new StreamReader(this.FullPath, Encoding.UTF8, true, 1024);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000B40C5 File Offset: 0x000B30C5
		public StreamWriter CreateText()
		{
			return new StreamWriter(this.FullPath, false);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x000B40D3 File Offset: 0x000B30D3
		public StreamWriter AppendText()
		{
			return new StreamWriter(this.FullPath, true);
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x000B40E1 File Offset: 0x000B30E1
		public FileInfo CopyTo(string destFileName)
		{
			return this.CopyTo(destFileName, false);
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000B40EB File Offset: 0x000B30EB
		public FileInfo CopyTo(string destFileName, bool overwrite)
		{
			destFileName = File.InternalCopy(this.FullPath, destFileName, overwrite);
			return new FileInfo(destFileName, false);
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x000B4103 File Offset: 0x000B3103
		public FileStream Create()
		{
			return File.Create(this.FullPath);
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x000B4110 File Offset: 0x000B3110
		public override void Delete()
		{
			new FileIOPermission(FileIOPermissionAccess.Write, new string[]
			{
				this.FullPath
			}, false, false).Demand();
			if (Environment.IsWin9X() && System.IO.Directory.InternalExists(this.FullPath))
			{
				throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[]
				{
					this.OriginalPath
				}));
			}
			if (!Win32Native.DeleteFile(this.FullPath))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 2)
				{
					return;
				}
				__Error.WinIOError(lastWin32Error, this.OriginalPath);
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x000B41A0 File Offset: 0x000B31A0
		[ComVisible(false)]
		public void Decrypt()
		{
			File.Decrypt(this.FullPath);
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000B41AD File Offset: 0x000B31AD
		[ComVisible(false)]
		public void Encrypt()
		{
			File.Encrypt(this.FullPath);
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060035FC RID: 13820 RVA: 0x000B41BC File Offset: 0x000B31BC
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
						result = ((this._data.fileAttributes & 16) == 0);
					}
				}
				catch
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000B4210 File Offset: 0x000B3210
		public FileStream Open(FileMode mode)
		{
			return this.Open(mode, FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000B421B File Offset: 0x000B321B
		public FileStream Open(FileMode mode, FileAccess access)
		{
			return this.Open(mode, access, FileShare.None);
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000B4226 File Offset: 0x000B3226
		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(this.FullPath, mode, access, share);
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000B4236 File Offset: 0x000B3236
		public FileStream OpenRead()
		{
			return new FileStream(this.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000B4246 File Offset: 0x000B3246
		public FileStream OpenWrite()
		{
			return new FileStream(this.FullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x000B4258 File Offset: 0x000B3258
		public void MoveTo(string destFileName)
		{
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destFileName");
			}
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[]
			{
				this.FullPath
			}, false, false).Demand();
			string fullPathInternal = Path.GetFullPathInternal(destFileName);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			if (!Win32Native.MoveFile(this.FullPath, fullPathInternal))
			{
				__Error.WinIOError();
			}
			this.FullPath = fullPathInternal;
			this.OriginalPath = destFileName;
			this._name = Path.GetFileName(fullPathInternal);
			this._dataInitialised = -1;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000B4302 File Offset: 0x000B3302
		[ComVisible(false)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
		{
			return this.Replace(destinationFileName, destinationBackupFileName, false);
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x000B430D File Offset: 0x000B330D
		[ComVisible(false)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			File.Replace(this.FullPath, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
			return new FileInfo(destinationFileName);
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000B4323 File Offset: 0x000B3323
		public override string ToString()
		{
			return this.OriginalPath;
		}

		// Token: 0x04001C36 RID: 7222
		private string _name;
	}
}
