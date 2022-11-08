using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x020005AB RID: 1451
	[ComVisible(true)]
	[FileIOPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[Serializable]
	public abstract class FileSystemInfo : MarshalByRefObject, ISerializable
	{
		// Token: 0x0600355D RID: 13661 RVA: 0x000B1CD4 File Offset: 0x000B0CD4
		protected FileSystemInfo()
		{
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000B1CE4 File Offset: 0x000B0CE4
		protected FileSystemInfo(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.FullPath = Path.GetFullPathInternal(info.GetString("FullPath"));
			this.OriginalPath = info.GetString("OriginalPath");
			this._dataInitialised = -1;
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x0600355F RID: 13663 RVA: 0x000B1D3C File Offset: 0x000B0D3C
		public virtual string FullName
		{
			get
			{
				string path;
				if (this is DirectoryInfo)
				{
					path = Directory.GetDemandDir(this.FullPath, true);
				}
				else
				{
					path = this.FullPath;
				}
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path).Demand();
				return this.FullPath;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003560 RID: 13664 RVA: 0x000B1D7C File Offset: 0x000B0D7C
		public string Extension
		{
			get
			{
				int length = this.FullPath.Length;
				int num = length;
				while (--num >= 0)
				{
					char c = this.FullPath[num];
					if (c == '.')
					{
						return this.FullPath.Substring(num, length - num);
					}
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						break;
					}
				}
				return string.Empty;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003561 RID: 13665
		public abstract string Name { get; }

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003562 RID: 13666
		public abstract bool Exists { get; }

		// Token: 0x06003563 RID: 13667
		public abstract void Delete();

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003564 RID: 13668 RVA: 0x000B1DE0 File Offset: 0x000B0DE0
		// (set) Token: 0x06003565 RID: 13669 RVA: 0x000B1DFB File Offset: 0x000B0DFB
		public DateTime CreationTime
		{
			get
			{
				return this.CreationTimeUtc.ToLocalTime();
			}
			set
			{
				this.CreationTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003566 RID: 13670 RVA: 0x000B1E0C File Offset: 0x000B0E0C
		// (set) Token: 0x06003567 RID: 13671 RVA: 0x000B1E70 File Offset: 0x000B0E70
		[ComVisible(false)]
		public DateTime CreationTimeUtc
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				long fileTime = (long)((ulong)this._data.ftCreationTimeHigh << 32 | (ulong)this._data.ftCreationTimeLow);
				return DateTime.FromFileTimeUtc(fileTime);
			}
			set
			{
				if (this is DirectoryInfo)
				{
					Directory.SetCreationTimeUtc(this.FullPath, value);
				}
				else
				{
					File.SetCreationTimeUtc(this.FullPath, value);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003568 RID: 13672 RVA: 0x000B1E9C File Offset: 0x000B0E9C
		// (set) Token: 0x06003569 RID: 13673 RVA: 0x000B1EB7 File Offset: 0x000B0EB7
		public DateTime LastAccessTime
		{
			get
			{
				return this.LastAccessTimeUtc.ToLocalTime();
			}
			set
			{
				this.LastAccessTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x0600356A RID: 13674 RVA: 0x000B1EC8 File Offset: 0x000B0EC8
		// (set) Token: 0x0600356B RID: 13675 RVA: 0x000B1F2C File Offset: 0x000B0F2C
		[ComVisible(false)]
		public DateTime LastAccessTimeUtc
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				long fileTime = (long)((ulong)this._data.ftLastAccessTimeHigh << 32 | (ulong)this._data.ftLastAccessTimeLow);
				return DateTime.FromFileTimeUtc(fileTime);
			}
			set
			{
				if (this is DirectoryInfo)
				{
					Directory.SetLastAccessTimeUtc(this.FullPath, value);
				}
				else
				{
					File.SetLastAccessTimeUtc(this.FullPath, value);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x0600356C RID: 13676 RVA: 0x000B1F58 File Offset: 0x000B0F58
		// (set) Token: 0x0600356D RID: 13677 RVA: 0x000B1F73 File Offset: 0x000B0F73
		public DateTime LastWriteTime
		{
			get
			{
				return this.LastWriteTimeUtc.ToLocalTime();
			}
			set
			{
				this.LastWriteTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x0600356E RID: 13678 RVA: 0x000B1F84 File Offset: 0x000B0F84
		// (set) Token: 0x0600356F RID: 13679 RVA: 0x000B1FE8 File Offset: 0x000B0FE8
		[ComVisible(false)]
		public DateTime LastWriteTimeUtc
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				long fileTime = (long)((ulong)this._data.ftLastWriteTimeHigh << 32 | (ulong)this._data.ftLastWriteTimeLow);
				return DateTime.FromFileTimeUtc(fileTime);
			}
			set
			{
				if (this is DirectoryInfo)
				{
					Directory.SetLastWriteTimeUtc(this.FullPath, value);
				}
				else
				{
					File.SetLastWriteTimeUtc(this.FullPath, value);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000B2013 File Offset: 0x000B1013
		public void Refresh()
		{
			this._dataInitialised = File.FillAttributeInfo(this.FullPath, ref this._data, false, false);
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003571 RID: 13681 RVA: 0x000B2030 File Offset: 0x000B1030
		// (set) Token: 0x06003572 RID: 13682 RVA: 0x000B207C File Offset: 0x000B107C
		public FileAttributes Attributes
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				return (FileAttributes)this._data.fileAttributes;
			}
			set
			{
				new FileIOPermission(FileIOPermissionAccess.Write, this.FullPath).Demand();
				if (!Win32Native.SetFileAttributes(this.FullPath, (int)value))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 87)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_InvalidFileAttrs"));
					}
					if (lastWin32Error == 5)
					{
						throw new ArgumentException(Environment.GetResourceString("UnauthorizedAccess_IODenied_NoPathName"));
					}
					__Error.WinIOError(lastWin32Error, this.OriginalPath);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000B20EC File Offset: 0x000B10EC
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.FullPath).Demand();
			info.AddValue("OriginalPath", this.OriginalPath, typeof(string));
			info.AddValue("FullPath", this.FullPath, typeof(string));
		}

		// Token: 0x04001C1A RID: 7194
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001C1B RID: 7195
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04001C1C RID: 7196
		internal Win32Native.WIN32_FILE_ATTRIBUTE_DATA _data;

		// Token: 0x04001C1D RID: 7197
		internal int _dataInitialised = -1;

		// Token: 0x04001C1E RID: 7198
		protected string FullPath;

		// Token: 0x04001C1F RID: 7199
		protected string OriginalPath;
	}
}
