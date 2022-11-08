using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x020005BD RID: 1469
	[ComVisible(true)]
	public class FileStream : Stream
	{
		// Token: 0x0600362B RID: 13867 RVA: 0x000B49D4 File Offset: 0x000B39D4
		internal FileStream()
		{
			this._fileName = null;
			this._handle = null;
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000B49EC File Offset: 0x000B39EC
		public FileStream(string path, FileMode mode) : this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000B4A18 File Offset: 0x000B3A18
		public FileStream(string path, FileMode mode, FileAccess access) : this(path, mode, access, FileShare.Read, 4096, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000B4A3C File Offset: 0x000B3A3C
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share) : this(path, mode, access, share, 4096, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000B4A64 File Offset: 0x000B3A64
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) : this(path, mode, access, share, bufferSize, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000B4A88 File Offset: 0x000B3A88
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options) : this(path, mode, access, share, bufferSize, options, Path.GetFileName(path), false)
		{
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000B4AAC File Offset: 0x000B3AAC
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync) : this(path, mode, access, share, bufferSize, useAsync ? FileOptions.Asynchronous : FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000B4ADC File Offset: 0x000B3ADC
		public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			object obj;
			Win32Native.SECURITY_ATTRIBUTES secAttrs = FileStream.GetSecAttrs(share, fileSecurity, out obj);
			try
			{
				this.Init(path, mode, (FileAccess)0, (int)rights, true, share, bufferSize, options, secAttrs, Path.GetFileName(path), false);
			}
			finally
			{
				if (obj != null)
				{
					((GCHandle)obj).Free();
				}
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000B4B38 File Offset: 0x000B3B38
		public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
		{
			Win32Native.SECURITY_ATTRIBUTES secAttrs = FileStream.GetSecAttrs(share);
			this.Init(path, mode, (FileAccess)0, (int)rights, true, share, bufferSize, options, secAttrs, Path.GetFileName(path), false);
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000B4B6C File Offset: 0x000B3B6C
		internal FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, string msgPath, bool bFromProxy)
		{
			Win32Native.SECURITY_ATTRIBUTES secAttrs = FileStream.GetSecAttrs(share);
			this.Init(path, mode, access, 0, false, share, bufferSize, options, secAttrs, msgPath, bFromProxy);
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000B4BA0 File Offset: 0x000B3BA0
		internal void Init(string path, FileMode mode, FileAccess access, int rights, bool useRights, FileShare share, int bufferSize, FileOptions options, Win32Native.SECURITY_ATTRIBUTES secAttrs, string msgPath, bool bFromProxy)
		{
			this._fileName = msgPath;
			this._exposedHandle = false;
			if (path == null)
			{
				throw new ArgumentNullException("path", Environment.GetResourceString("ArgumentNull_Path"));
			}
			if (path.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
			}
			if (Environment.IsWin9X())
			{
				if ((share & FileShare.Delete) != FileShare.None)
				{
					throw new PlatformNotSupportedException(Environment.GetResourceString("NotSupported_FileShareDeleteOnWin9x"));
				}
				if (useRights)
				{
					throw new PlatformNotSupportedException(Environment.GetResourceString("NotSupported_FileSystemRightsOnWin9x"));
				}
			}
			FileShare fileShare = share & ~FileShare.Inheritable;
			string text = null;
			if (mode < FileMode.CreateNew || mode > FileMode.Append)
			{
				text = "mode";
			}
			else if (!useRights && (access < FileAccess.Read || access > FileAccess.ReadWrite))
			{
				text = "access";
			}
			else if (useRights && (rights < 1 || rights > 2032127))
			{
				text = "rights";
			}
			else if (fileShare < FileShare.None || fileShare > (FileShare.Read | FileShare.Write | FileShare.Delete))
			{
				text = "share";
			}
			if (text != null)
			{
				throw new ArgumentOutOfRangeException(text, Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (options != FileOptions.None && (options & (FileOptions)67092479) != FileOptions.None)
			{
				throw new ArgumentOutOfRangeException("options", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (((!useRights && (access & FileAccess.Write) == (FileAccess)0) || (useRights && (rights & 278) == 0)) && (mode == FileMode.Truncate || mode == FileMode.CreateNew || mode == FileMode.Create || mode == FileMode.Append))
			{
				if (!useRights)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidFileMode&AccessCombo"), new object[]
					{
						mode,
						access
					}));
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidFileMode&RightsCombo"), new object[]
				{
					mode,
					(FileSystemRights)rights
				}));
			}
			else
			{
				if (useRights && mode == FileMode.Truncate)
				{
					if (rights != 278)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidFileModeTruncate&RightsCombo"), new object[]
						{
							mode,
							(FileSystemRights)rights
						}));
					}
					useRights = false;
					access = FileAccess.Write;
				}
				int dwDesiredAccess;
				if (!useRights)
				{
					dwDesiredAccess = ((access == FileAccess.Read) ? int.MinValue : ((access == FileAccess.Write) ? 1073741824 : -1073741824));
				}
				else
				{
					dwDesiredAccess = rights;
				}
				string fullPathInternal = Path.GetFullPathInternal(path);
				this._fileName = fullPathInternal;
				if (fullPathInternal.StartsWith("\\\\.\\", StringComparison.Ordinal))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_DevicesNotSupported"));
				}
				FileIOPermissionAccess fileIOPermissionAccess = FileIOPermissionAccess.NoAccess;
				if ((!useRights && (access & FileAccess.Read) != (FileAccess)0) || (useRights && (rights & 131241) != 0))
				{
					if (mode == FileMode.Append)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAppendMode"));
					}
					fileIOPermissionAccess |= FileIOPermissionAccess.Read;
				}
				if ((!useRights && (access & FileAccess.Write) != (FileAccess)0) || (useRights && (rights & 852310) != 0))
				{
					if (mode == FileMode.Append)
					{
						fileIOPermissionAccess |= FileIOPermissionAccess.Append;
					}
					else
					{
						fileIOPermissionAccess |= FileIOPermissionAccess.Write;
					}
				}
				AccessControlActions control = (secAttrs != null && secAttrs.pSecurityDescriptor != null) ? AccessControlActions.Change : AccessControlActions.None;
				new FileIOPermission(fileIOPermissionAccess, control, new string[]
				{
					fullPathInternal
				}, false, false).Demand();
				share &= ~FileShare.Inheritable;
				bool flag = mode == FileMode.Append;
				if (mode == FileMode.Append)
				{
					mode = FileMode.OpenOrCreate;
				}
				if (FileStream._canUseAsync && (options & FileOptions.Asynchronous) != FileOptions.None)
				{
					this._isAsync = true;
				}
				else
				{
					options &= ~FileOptions.Asynchronous;
				}
				int num = (int)options;
				num |= 1048576;
				int errorMode = Win32Native.SetErrorMode(1);
				try
				{
					this._handle = Win32Native.SafeCreateFile(fullPathInternal, dwDesiredAccess, share, secAttrs, mode, num, Win32Native.NULL);
					if (this._handle.IsInvalid)
					{
						int num2 = Marshal.GetLastWin32Error();
						if (num2 == 3 && fullPathInternal.Equals(Directory.InternalGetDirectoryRoot(fullPathInternal)))
						{
							num2 = 5;
						}
						bool flag2 = false;
						if (!bFromProxy)
						{
							try
							{
								new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[]
								{
									this._fileName
								}, false, false).Demand();
								flag2 = true;
							}
							catch (SecurityException)
							{
							}
						}
						if (flag2)
						{
							__Error.WinIOError(num2, this._fileName);
						}
						else
						{
							__Error.WinIOError(num2, msgPath);
						}
					}
				}
				finally
				{
					Win32Native.SetErrorMode(errorMode);
				}
				int fileType = Win32Native.GetFileType(this._handle);
				if (fileType != 1)
				{
					this._handle.Close();
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_FileStreamOnNonFiles"));
				}
				if (this._isAsync)
				{
					bool flag3 = false;
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					try
					{
						flag3 = ThreadPool.BindHandle(this._handle);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
						if (!flag3)
						{
							this._handle.Close();
						}
					}
					if (!flag3)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_BindHandleFailed"));
					}
				}
				if (!useRights)
				{
					this._canRead = ((access & FileAccess.Read) != (FileAccess)0);
					this._canWrite = ((access & FileAccess.Write) != (FileAccess)0);
				}
				else
				{
					this._canRead = ((rights & 1) != 0);
					this._canWrite = ((rights & 2) != 0 || (rights & 4) != 0);
				}
				this._canSeek = true;
				this._isPipe = false;
				this._pos = 0L;
				this._bufferSize = bufferSize;
				this._readPos = 0;
				this._readLen = 0;
				this._writePos = 0;
				if (flag)
				{
					this._appendStart = this.SeekCore(0L, SeekOrigin.End);
					return;
				}
				this._appendStart = -1L;
				return;
			}
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000B50CC File Offset: 0x000B40CC
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public FileStream(IntPtr handle, FileAccess access) : this(handle, access, true, 4096, false)
		{
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000B50DD File Offset: 0x000B40DD
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle) : this(handle, access, ownsHandle, 4096, false)
		{
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000B50EE File Offset: 0x000B40EE
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) : this(handle, access, ownsHandle, bufferSize, false)
		{
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000B50FC File Offset: 0x000B40FC
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync) : this(new SafeFileHandle(handle, ownsHandle), access, bufferSize, isAsync)
		{
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000B5110 File Offset: 0x000B4110
		public FileStream(SafeFileHandle handle, FileAccess access) : this(handle, access, 4096, false)
		{
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000B5120 File Offset: 0x000B4120
		public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) : this(handle, access, bufferSize, false)
		{
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000B512C File Offset: 0x000B412C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
		{
			if (handle.IsInvalid)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidHandle"), "handle");
			}
			this._handle = handle;
			this._exposedHandle = true;
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			int fileType = Win32Native.GetFileType(this._handle);
			this._isAsync = (isAsync && FileStream._canUseAsync);
			this._canRead = ((FileAccess)0 != (access & FileAccess.Read));
			this._canWrite = ((FileAccess)0 != (access & FileAccess.Write));
			this._canSeek = (fileType == 1);
			this._bufferSize = bufferSize;
			this._readPos = 0;
			this._readLen = 0;
			this._writePos = 0;
			this._fileName = null;
			this._isPipe = (fileType == 3);
			if (this._isAsync)
			{
				bool flag = false;
				try
				{
					flag = ThreadPool.BindHandle(this._handle);
				}
				catch (ApplicationException)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_HandleNotAsync"));
				}
				if (!flag)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_BindHandleFailed"));
				}
			}
			else if (fileType != 3)
			{
				this.VerifyHandleIsSync();
			}
			if (this._canSeek)
			{
				this.SeekCore(0L, SeekOrigin.Current);
				return;
			}
			this._pos = 0L;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000B5284 File Offset: 0x000B4284
		private static Win32Native.SECURITY_ATTRIBUTES GetSecAttrs(FileShare share)
		{
			Win32Native.SECURITY_ATTRIBUTES security_ATTRIBUTES = null;
			if ((share & FileShare.Inheritable) != FileShare.None)
			{
				security_ATTRIBUTES = new Win32Native.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				security_ATTRIBUTES.bInheritHandle = 1;
			}
			return security_ATTRIBUTES;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000B52B4 File Offset: 0x000B42B4
		private unsafe static Win32Native.SECURITY_ATTRIBUTES GetSecAttrs(FileShare share, FileSecurity fileSecurity, out object pinningHandle)
		{
			pinningHandle = null;
			Win32Native.SECURITY_ATTRIBUTES security_ATTRIBUTES = null;
			if ((share & FileShare.Inheritable) != FileShare.None || fileSecurity != null)
			{
				security_ATTRIBUTES = new Win32Native.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				if ((share & FileShare.Inheritable) != FileShare.None)
				{
					security_ATTRIBUTES.bInheritHandle = 1;
				}
				if (fileSecurity != null)
				{
					byte[] securityDescriptorBinaryForm = fileSecurity.GetSecurityDescriptorBinaryForm();
					pinningHandle = GCHandle.Alloc(securityDescriptorBinaryForm, GCHandleType.Pinned);
					fixed (byte* ptr = securityDescriptorBinaryForm)
					{
						security_ATTRIBUTES.pSecurityDescriptor = ptr;
					}
				}
			}
			return security_ATTRIBUTES;
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000B532C File Offset: 0x000B432C
		private void VerifyHandleIsSync()
		{
			byte[] bytes = new byte[1];
			int num = 0;
			if (this.CanRead)
			{
				this.ReadFileNative(this._handle, bytes, 0, 0, null, out num);
			}
			else if (this.CanWrite)
			{
				this.WriteFileNative(this._handle, bytes, 0, 0, null, out num);
			}
			if (num == 87)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_HandleNotSync"));
			}
			if (num == 6)
			{
				__Error.WinIOError(num, "<OS handle>");
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x000B53A0 File Offset: 0x000B43A0
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003641 RID: 13889 RVA: 0x000B53A8 File Offset: 0x000B43A8
		public override bool CanWrite
		{
			get
			{
				return this._canWrite;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003642 RID: 13890 RVA: 0x000B53B0 File Offset: 0x000B43B0
		public override bool CanSeek
		{
			get
			{
				return this._canSeek;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003643 RID: 13891 RVA: 0x000B53B8 File Offset: 0x000B43B8
		public virtual bool IsAsync
		{
			get
			{
				return this._isAsync;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003644 RID: 13892 RVA: 0x000B53C0 File Offset: 0x000B43C0
		public override long Length
		{
			get
			{
				if (this._handle.IsClosed)
				{
					__Error.FileNotOpen();
				}
				if (!this.CanSeek)
				{
					__Error.SeekNotSupported();
				}
				int num = 0;
				int fileSize = Win32Native.GetFileSize(this._handle, out num);
				if (fileSize == -1)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error != 0)
					{
						__Error.WinIOError(lastWin32Error, string.Empty);
					}
				}
				long num2 = (long)num << 32 | (long)((ulong)fileSize);
				if (this._writePos > 0 && this._pos + (long)this._writePos > num2)
				{
					num2 = (long)this._writePos + this._pos;
				}
				return num2;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003645 RID: 13893 RVA: 0x000B544C File Offset: 0x000B444C
		public string Name
		{
			get
			{
				if (this._fileName == null)
				{
					return Environment.GetResourceString("IO_UnknownFileName");
				}
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[]
				{
					this._fileName
				}, false, false).Demand();
				return this._fileName;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003646 RID: 13894 RVA: 0x000B5490 File Offset: 0x000B4490
		internal string NameInternal
		{
			get
			{
				if (this._fileName == null)
				{
					return "<UnknownFileName>";
				}
				return this._fileName;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003647 RID: 13895 RVA: 0x000B54A8 File Offset: 0x000B44A8
		// (set) Token: 0x06003648 RID: 13896 RVA: 0x000B5500 File Offset: 0x000B4500
		public override long Position
		{
			get
			{
				if (this._handle.IsClosed)
				{
					__Error.FileNotOpen();
				}
				if (!this.CanSeek)
				{
					__Error.SeekNotSupported();
				}
				if (this._exposedHandle)
				{
					this.VerifyOSHandlePosition();
				}
				return this._pos + (long)(this._readPos - this._readLen + this._writePos);
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				this._readPos = 0;
				this._readLen = 0;
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000B554E File Offset: 0x000B454E
		public FileSecurity GetAccessControl()
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			return new FileSecurity(this._handle, this._fileName, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000B5575 File Offset: 0x000B4575
		public void SetAccessControl(FileSecurity fileSecurity)
		{
			if (fileSecurity == null)
			{
				throw new ArgumentNullException("fileSecurity");
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			fileSecurity.Persist(this._handle, this._fileName);
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000B55AC File Offset: 0x000B45AC
		private unsafe static void AsyncFSCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(pOverlapped);
			FileStreamAsyncResult fileStreamAsyncResult = (FileStreamAsyncResult)overlapped.AsyncResult;
			fileStreamAsyncResult._numBytes = (int)numBytes;
			if (errorCode == 109U || errorCode == 232U)
			{
				errorCode = 0U;
			}
			fileStreamAsyncResult._errorCode = (int)errorCode;
			fileStreamAsyncResult._completedSynchronously = false;
			fileStreamAsyncResult._isComplete = true;
			ManualResetEvent waitHandle = fileStreamAsyncResult._waitHandle;
			if (waitHandle != null && !waitHandle.Set())
			{
				__Error.WinIOError();
			}
			AsyncCallback userCallback = fileStreamAsyncResult._userCallback;
			if (userCallback != null)
			{
				userCallback(fileStreamAsyncResult);
			}
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000B5628 File Offset: 0x000B4628
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._handle != null && !this._handle.IsClosed && this._writePos > 0)
				{
					this.FlushWrite(!disposing);
				}
			}
			finally
			{
				if (this._handle != null && !this._handle.IsClosed)
				{
					this._handle.Dispose();
				}
				this._canRead = false;
				this._canWrite = false;
				this._canSeek = false;
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000B56AC File Offset: 0x000B46AC
		~FileStream()
		{
			if (this._handle != null)
			{
				this.Dispose(false);
			}
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000B56E4 File Offset: 0x000B46E4
		public override void Flush()
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			else if (this._readPos < this._readLen && this.CanSeek)
			{
				this.FlushRead();
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000B573F File Offset: 0x000B473F
		private void FlushRead()
		{
			if (this._readPos - this._readLen != 0)
			{
				this.SeekCore((long)(this._readPos - this._readLen), SeekOrigin.Current);
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000B5774 File Offset: 0x000B4774
		private void FlushWrite(bool calledFromFinalizer)
		{
			if (this._isAsync)
			{
				IAsyncResult asyncResult = this.BeginWriteCore(this._buffer, 0, this._writePos, null, null);
				if (!calledFromFinalizer)
				{
					this.EndWrite(asyncResult);
				}
			}
			else
			{
				this.WriteCore(this._buffer, 0, this._writePos);
			}
			this._writePos = 0;
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003651 RID: 13905 RVA: 0x000B57C5 File Offset: 0x000B47C5
		[Obsolete("This property has been deprecated.  Please use FileStream's SafeFileHandle property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual IntPtr Handle
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.Flush();
				this._readPos = 0;
				this._readLen = 0;
				this._writePos = 0;
				this._exposedHandle = true;
				return this._handle.DangerousGetHandle();
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003652 RID: 13906 RVA: 0x000B57F4 File Offset: 0x000B47F4
		public virtual SafeFileHandle SafeFileHandle
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.Flush();
				this._readPos = 0;
				this._readLen = 0;
				this._writePos = 0;
				this._exposedHandle = true;
				return this._handle;
			}
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000B5820 File Offset: 0x000B4820
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this.CanSeek)
			{
				__Error.SeekNotSupported();
			}
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			else if (this._readPos < this._readLen)
			{
				this.FlushRead();
			}
			this._readPos = 0;
			this._readLen = 0;
			if (this._appendStart != -1L && value < this._appendStart)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SetLengthAppendTruncate"));
			}
			this.SetLengthCore(value);
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000B58D4 File Offset: 0x000B48D4
		private void SetLengthCore(long value)
		{
			long pos = this._pos;
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			if (this._pos != value)
			{
				this.SeekCore(value, SeekOrigin.Begin);
			}
			if (!Win32Native.SetEndOfFile(this._handle))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 87)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_FileLengthTooBig"));
				}
				__Error.WinIOError(lastWin32Error, string.Empty);
			}
			if (pos != value)
			{
				if (pos < value)
				{
					this.SeekCore(pos, SeekOrigin.Begin);
					return;
				}
				this.SeekCore(0L, SeekOrigin.End);
			}
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000B595C File Offset: 0x000B495C
		public override int Read([In] [Out] byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			bool flag = false;
			int num = this._readLen - this._readPos;
			if (num == 0)
			{
				if (!this.CanRead)
				{
					__Error.ReadNotSupported();
				}
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				if (!this.CanSeek || count >= this._bufferSize)
				{
					num = this.ReadCore(array, offset, count);
					this._readPos = 0;
					this._readLen = 0;
					return num;
				}
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
				num = this.ReadCore(this._buffer, 0, this._bufferSize);
				if (num == 0)
				{
					return 0;
				}
				flag = (num < this._bufferSize);
				this._readPos = 0;
				this._readLen = num;
			}
			if (num > count)
			{
				num = count;
			}
			Buffer.InternalBlockCopy(this._buffer, this._readPos, array, offset, num);
			this._readPos += num;
			if (!this._isPipe && num < count && !flag)
			{
				int num2 = this.ReadCore(array, offset + num, count - num);
				num += num2;
				this._readPos = 0;
				this._readLen = 0;
			}
			return num;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000B5AE0 File Offset: 0x000B4AE0
		private int ReadCore(byte[] buffer, int offset, int count)
		{
			if (this._isAsync)
			{
				IAsyncResult asyncResult = this.BeginReadCore(buffer, offset, count, null, null, 0);
				return this.EndRead(asyncResult);
			}
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			int num = 0;
			int num2 = this.ReadFileNative(this._handle, buffer, offset, count, null, out num);
			if (num2 == -1)
			{
				if (num == 109)
				{
					num2 = 0;
				}
				else
				{
					if (num == 87)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_HandleNotSync"));
					}
					__Error.WinIOError(num, string.Empty);
				}
			}
			this._pos += (long)num2;
			return num2;
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000B5B6C File Offset: 0x000B4B6C
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin < SeekOrigin.Begin || origin > SeekOrigin.End)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSeekOrigin"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this.CanSeek)
			{
				__Error.SeekNotSupported();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			else if (origin == SeekOrigin.Current)
			{
				offset -= (long)(this._readLen - this._readPos);
			}
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			long num = this._pos + (long)(this._readPos - this._readLen);
			long num2 = this.SeekCore(offset, origin);
			if (this._appendStart != -1L && num2 < this._appendStart)
			{
				this.SeekCore(num, SeekOrigin.Begin);
				throw new IOException(Environment.GetResourceString("IO.IO_SeekAppendOverwrite"));
			}
			if (this._readLen > 0)
			{
				if (num == num2)
				{
					if (this._readPos > 0)
					{
						Buffer.InternalBlockCopy(this._buffer, this._readPos, this._buffer, 0, this._readLen - this._readPos);
						this._readLen -= this._readPos;
						this._readPos = 0;
					}
					if (this._readLen > 0)
					{
						this.SeekCore((long)this._readLen, SeekOrigin.Current);
					}
				}
				else if (num - (long)this._readPos < num2 && num2 < num + (long)this._readLen - (long)this._readPos)
				{
					int num3 = (int)(num2 - num);
					Buffer.InternalBlockCopy(this._buffer, this._readPos + num3, this._buffer, 0, this._readLen - (this._readPos + num3));
					this._readLen -= this._readPos + num3;
					this._readPos = 0;
					if (this._readLen > 0)
					{
						this.SeekCore((long)this._readLen, SeekOrigin.Current);
					}
				}
				else
				{
					this._readPos = 0;
					this._readLen = 0;
				}
			}
			return num2;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000B5D3C File Offset: 0x000B4D3C
		private long SeekCore(long offset, SeekOrigin origin)
		{
			int num = 0;
			long num2 = Win32Native.SetFilePointer(this._handle, offset, origin, out num);
			if (num2 == -1L)
			{
				if (num == 6 && !this._handle.IsInvalid)
				{
					this._handle.Dispose();
				}
				__Error.WinIOError(num, string.Empty);
			}
			this._pos = num2;
			return num2;
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000B5D94 File Offset: 0x000B4D94
		private void VerifyOSHandlePosition()
		{
			if (!this.CanSeek)
			{
				return;
			}
			long pos = this._pos;
			long num = this.SeekCore(0L, SeekOrigin.Current);
			if (num != pos)
			{
				this._readPos = 0;
				this._readLen = 0;
				if (this._writePos > 0)
				{
					this._writePos = 0;
					throw new IOException(Environment.GetResourceString("IO.IO_FileStreamHandlePosition"));
				}
			}
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000B5DF0 File Offset: 0x000B4DF0
		public override void Write(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._writePos == 0)
			{
				if (!this.CanWrite)
				{
					__Error.WriteNotSupported();
				}
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				this._readPos = 0;
				this._readLen = 0;
			}
			if (this._writePos > 0)
			{
				int num = this._bufferSize - this._writePos;
				if (num > 0)
				{
					if (num > count)
					{
						num = count;
					}
					Buffer.InternalBlockCopy(array, offset, this._buffer, this._writePos, num);
					this._writePos += num;
					if (count == num)
					{
						return;
					}
					offset += num;
					count -= num;
				}
				if (this._isAsync)
				{
					IAsyncResult asyncResult = this.BeginWriteCore(this._buffer, 0, this._writePos, null, null);
					this.EndWrite(asyncResult);
				}
				else
				{
					this.WriteCore(this._buffer, 0, this._writePos);
				}
				this._writePos = 0;
			}
			if (count >= this._bufferSize)
			{
				this.WriteCore(array, offset, count);
				return;
			}
			if (count == 0)
			{
				return;
			}
			if (this._buffer == null)
			{
				this._buffer = new byte[this._bufferSize];
			}
			Buffer.InternalBlockCopy(array, offset, this._buffer, this._writePos, count);
			this._writePos = count;
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000B5F8C File Offset: 0x000B4F8C
		private void WriteCore(byte[] buffer, int offset, int count)
		{
			if (this._isAsync)
			{
				IAsyncResult asyncResult = this.BeginWriteCore(buffer, offset, count, null, null);
				this.EndWrite(asyncResult);
				return;
			}
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			int num = 0;
			int num2 = this.WriteFileNative(this._handle, buffer, offset, count, null, out num);
			if (num2 == -1)
			{
				if (num == 232)
				{
					num2 = 0;
				}
				else
				{
					if (num == 87)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_FileTooLongOrHandleNotSync"));
					}
					__Error.WinIOError(num, string.Empty);
				}
			}
			this._pos += (long)num2;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000B6018 File Offset: 0x000B5018
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < numBytes)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this._isAsync)
			{
				return base.BeginRead(array, offset, numBytes, userCallback, stateObject);
			}
			if (!this.CanRead)
			{
				__Error.ReadNotSupported();
			}
			if (this._isPipe)
			{
				if (this._readPos < this._readLen)
				{
					int num = this._readLen - this._readPos;
					if (num > numBytes)
					{
						num = numBytes;
					}
					Buffer.InternalBlockCopy(this._buffer, this._readPos, array, offset, num);
					this._readPos += num;
					FileStreamAsyncResult fileStreamAsyncResult = FileStreamAsyncResult.CreateBufferedReadResult(num, userCallback, stateObject);
					fileStreamAsyncResult.CallUserCallback();
					return fileStreamAsyncResult;
				}
				return this.BeginReadCore(array, offset, numBytes, userCallback, stateObject, 0);
			}
			else
			{
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				if (this._readPos == this._readLen)
				{
					if (numBytes < this._bufferSize)
					{
						if (this._buffer == null)
						{
							this._buffer = new byte[this._bufferSize];
						}
						IAsyncResult asyncResult = this.BeginReadCore(this._buffer, 0, this._bufferSize, null, null, 0);
						this._readLen = this.EndRead(asyncResult);
						int num2 = this._readLen;
						if (num2 > numBytes)
						{
							num2 = numBytes;
						}
						Buffer.InternalBlockCopy(this._buffer, 0, array, offset, num2);
						this._readPos = num2;
						FileStreamAsyncResult fileStreamAsyncResult = FileStreamAsyncResult.CreateBufferedReadResult(num2, userCallback, stateObject);
						fileStreamAsyncResult.CallUserCallback();
						return fileStreamAsyncResult;
					}
					this._readPos = 0;
					this._readLen = 0;
					return this.BeginReadCore(array, offset, numBytes, userCallback, stateObject, 0);
				}
				else
				{
					int num3 = this._readLen - this._readPos;
					if (num3 > numBytes)
					{
						num3 = numBytes;
					}
					Buffer.InternalBlockCopy(this._buffer, this._readPos, array, offset, num3);
					this._readPos += num3;
					if (num3 >= numBytes || this._isPipe)
					{
						FileStreamAsyncResult fileStreamAsyncResult = FileStreamAsyncResult.CreateBufferedReadResult(num3, userCallback, stateObject);
						fileStreamAsyncResult.CallUserCallback();
						return fileStreamAsyncResult;
					}
					this._readPos = 0;
					this._readLen = 0;
					return this.BeginReadCore(array, offset + num3, numBytes - num3, userCallback, stateObject, num3);
				}
			}
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000B6260 File Offset: 0x000B5260
		private unsafe FileStreamAsyncResult BeginReadCore(byte[] bytes, int offset, int numBytes, AsyncCallback userCallback, object stateObject, int numBufferedBytesRead)
		{
			FileStreamAsyncResult fileStreamAsyncResult = new FileStreamAsyncResult();
			fileStreamAsyncResult._handle = this._handle;
			fileStreamAsyncResult._userCallback = userCallback;
			fileStreamAsyncResult._userStateObject = stateObject;
			fileStreamAsyncResult._isWrite = false;
			fileStreamAsyncResult._numBufferedBytes = numBufferedBytesRead;
			ManualResetEvent waitHandle = new ManualResetEvent(false);
			fileStreamAsyncResult._waitHandle = waitHandle;
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, fileStreamAsyncResult);
			NativeOverlapped* ptr;
			if (userCallback != null)
			{
				ptr = overlapped.Pack(FileStream.IOCallback, bytes);
			}
			else
			{
				ptr = overlapped.UnsafePack(null, bytes);
			}
			fileStreamAsyncResult._overlapped = ptr;
			if (this.CanSeek)
			{
				long length = this.Length;
				if (this._exposedHandle)
				{
					this.VerifyOSHandlePosition();
				}
				if (this._pos + (long)numBytes > length)
				{
					if (this._pos <= length)
					{
						numBytes = (int)(length - this._pos);
					}
					else
					{
						numBytes = 0;
					}
				}
				ptr->OffsetLow = (int)this._pos;
				ptr->OffsetHigh = (int)(this._pos >> 32);
				this.SeekCore((long)numBytes, SeekOrigin.Current);
			}
			int num = 0;
			int num2 = this.ReadFileNative(this._handle, bytes, offset, numBytes, ptr, out num);
			if (num2 == -1 && numBytes != -1)
			{
				if (num == 109)
				{
					ptr->InternalLow = IntPtr.Zero;
					fileStreamAsyncResult.CallUserCallback();
				}
				else if (num != 997)
				{
					if (!this._handle.IsClosed && this.CanSeek)
					{
						this.SeekCore(0L, SeekOrigin.Current);
					}
					if (num == 38)
					{
						__Error.EndOfFile();
					}
					else
					{
						__Error.WinIOError(num, string.Empty);
					}
				}
			}
			return fileStreamAsyncResult;
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000B63C8 File Offset: 0x000B53C8
		public unsafe override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!this._isAsync)
			{
				return base.EndRead(asyncResult);
			}
			FileStreamAsyncResult fileStreamAsyncResult = asyncResult as FileStreamAsyncResult;
			if (fileStreamAsyncResult == null || fileStreamAsyncResult._isWrite)
			{
				__Error.WrongAsyncResult();
			}
			if (1 == Interlocked.CompareExchange(ref fileStreamAsyncResult._EndXxxCalled, 1, 0))
			{
				__Error.EndReadCalledTwice();
			}
			WaitHandle waitHandle = fileStreamAsyncResult._waitHandle;
			if (waitHandle != null)
			{
				try
				{
					waitHandle.WaitOne();
				}
				finally
				{
					waitHandle.Close();
				}
			}
			NativeOverlapped* overlapped = fileStreamAsyncResult._overlapped;
			if (overlapped != null)
			{
				Overlapped.Free(overlapped);
			}
			if (fileStreamAsyncResult._errorCode != 0)
			{
				__Error.WinIOError(fileStreamAsyncResult._errorCode, Path.GetFileName(this._fileName));
			}
			return fileStreamAsyncResult._numBytes + fileStreamAsyncResult._numBufferedBytes;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000B6488 File Offset: 0x000B5488
		public override int ReadByte()
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._readLen == 0 && !this.CanRead)
			{
				__Error.ReadNotSupported();
			}
			if (this._readPos == this._readLen)
			{
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
				this._readLen = this.ReadCore(this._buffer, 0, this._bufferSize);
				this._readPos = 0;
			}
			if (this._readPos == this._readLen)
			{
				return -1;
			}
			int result = (int)this._buffer[this._readPos];
			this._readPos++;
			return result;
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000B6540 File Offset: 0x000B5540
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < numBytes)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this._isAsync)
			{
				return base.BeginWrite(array, offset, numBytes, userCallback, stateObject);
			}
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
			if (this._isPipe)
			{
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				return this.BeginWriteCore(array, offset, numBytes, userCallback, stateObject);
			}
			if (this._writePos == 0)
			{
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				this._readPos = 0;
				this._readLen = 0;
			}
			int num = this._bufferSize - this._writePos;
			if (numBytes <= num)
			{
				if (this._writePos == 0)
				{
					this._buffer = new byte[this._bufferSize];
				}
				Buffer.InternalBlockCopy(array, offset, this._buffer, this._writePos, numBytes);
				this._writePos += numBytes;
				FileStreamAsyncResult fileStreamAsyncResult = new FileStreamAsyncResult();
				fileStreamAsyncResult._userCallback = userCallback;
				fileStreamAsyncResult._userStateObject = stateObject;
				fileStreamAsyncResult._waitHandle = null;
				fileStreamAsyncResult._isWrite = true;
				fileStreamAsyncResult._numBufferedBytes = numBytes;
				fileStreamAsyncResult.CallUserCallback();
				return fileStreamAsyncResult;
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			return this.BeginWriteCore(array, offset, numBytes, userCallback, stateObject);
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000B66C8 File Offset: 0x000B56C8
		private unsafe FileStreamAsyncResult BeginWriteCore(byte[] bytes, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			FileStreamAsyncResult fileStreamAsyncResult = new FileStreamAsyncResult();
			fileStreamAsyncResult._handle = this._handle;
			fileStreamAsyncResult._userCallback = userCallback;
			fileStreamAsyncResult._userStateObject = stateObject;
			fileStreamAsyncResult._isWrite = true;
			ManualResetEvent waitHandle = new ManualResetEvent(false);
			fileStreamAsyncResult._waitHandle = waitHandle;
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, fileStreamAsyncResult);
			NativeOverlapped* ptr;
			if (userCallback != null)
			{
				ptr = overlapped.Pack(FileStream.IOCallback, bytes);
			}
			else
			{
				ptr = overlapped.UnsafePack(null, bytes);
			}
			fileStreamAsyncResult._overlapped = ptr;
			if (this.CanSeek)
			{
				long length = this.Length;
				if (this._exposedHandle)
				{
					this.VerifyOSHandlePosition();
				}
				if (this._pos + (long)numBytes > length)
				{
					this.SetLengthCore(this._pos + (long)numBytes);
				}
				ptr->OffsetLow = (int)this._pos;
				ptr->OffsetHigh = (int)(this._pos >> 32);
				this.SeekCore((long)numBytes, SeekOrigin.Current);
			}
			int num = 0;
			int num2 = this.WriteFileNative(this._handle, bytes, offset, numBytes, ptr, out num);
			if (num2 == -1 && numBytes != -1)
			{
				if (num == 232)
				{
					fileStreamAsyncResult.CallUserCallback();
				}
				else if (num != 997)
				{
					if (!this._handle.IsClosed && this.CanSeek)
					{
						this.SeekCore(0L, SeekOrigin.Current);
					}
					if (num == 38)
					{
						__Error.EndOfFile();
					}
					else
					{
						__Error.WinIOError(num, string.Empty);
					}
				}
			}
			return fileStreamAsyncResult;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000B6814 File Offset: 0x000B5814
		public unsafe override void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!this._isAsync)
			{
				base.EndWrite(asyncResult);
				return;
			}
			FileStreamAsyncResult fileStreamAsyncResult = asyncResult as FileStreamAsyncResult;
			if (fileStreamAsyncResult == null || !fileStreamAsyncResult._isWrite)
			{
				__Error.WrongAsyncResult();
			}
			if (1 == Interlocked.CompareExchange(ref fileStreamAsyncResult._EndXxxCalled, 1, 0))
			{
				__Error.EndWriteCalledTwice();
			}
			WaitHandle waitHandle = fileStreamAsyncResult._waitHandle;
			if (waitHandle != null)
			{
				try
				{
					waitHandle.WaitOne();
				}
				finally
				{
					waitHandle.Close();
				}
			}
			NativeOverlapped* overlapped = fileStreamAsyncResult._overlapped;
			if (overlapped != null)
			{
				Overlapped.Free(overlapped);
			}
			if (fileStreamAsyncResult._errorCode != 0)
			{
				__Error.WinIOError(fileStreamAsyncResult._errorCode, Path.GetFileName(this._fileName));
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000B68C8 File Offset: 0x000B58C8
		public override void WriteByte(byte value)
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._writePos == 0)
			{
				if (!this.CanWrite)
				{
					__Error.WriteNotSupported();
				}
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				this._readPos = 0;
				this._readLen = 0;
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
			}
			if (this._writePos == this._bufferSize)
			{
				this.FlushWrite(false);
			}
			this._buffer[this._writePos] = value;
			this._writePos++;
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x000B6968 File Offset: 0x000B5968
		public virtual void Lock(long position, long length)
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (position < 0L || length < 0L)
			{
				throw new ArgumentOutOfRangeException((position < 0L) ? "position" : "length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int offsetLow = (int)position;
			int offsetHigh = (int)(position >> 32);
			int countLow = (int)length;
			int countHigh = (int)(length >> 32);
			if (!Win32Native.LockFile(this._handle, offsetLow, offsetHigh, countLow, countHigh))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000B69DC File Offset: 0x000B59DC
		public virtual void Unlock(long position, long length)
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (position < 0L || length < 0L)
			{
				throw new ArgumentOutOfRangeException((position < 0L) ? "position" : "length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int offsetLow = (int)position;
			int offsetHigh = (int)(position >> 32);
			int countLow = (int)length;
			int countHigh = (int)(length >> 32);
			if (!Win32Native.UnlockFile(this._handle, offsetLow, offsetHigh, countLow, countHigh))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000B6A50 File Offset: 0x000B5A50
		private unsafe int ReadFileNative(SafeFileHandle handle, byte[] bytes, int offset, int count, NativeOverlapped* overlapped, out int hr)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				hr = 0;
				return 0;
			}
			int result = 0;
			int num;
			fixed (byte* ptr = bytes)
			{
				if (this._isAsync)
				{
					num = Win32Native.ReadFile(handle, ptr + offset, count, IntPtr.Zero, overlapped);
				}
				else
				{
					num = Win32Native.ReadFile(handle, ptr + offset, count, out result, IntPtr.Zero);
				}
			}
			if (num != 0)
			{
				hr = 0;
				return result;
			}
			hr = Marshal.GetLastWin32Error();
			if (hr == 109 || hr == 233)
			{
				return -1;
			}
			if (hr == 6 && !this._handle.IsInvalid)
			{
				this._handle.Dispose();
			}
			return -1;
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000B6B14 File Offset: 0x000B5B14
		private unsafe int WriteFileNative(SafeFileHandle handle, byte[] bytes, int offset, int count, NativeOverlapped* overlapped, out int hr)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				hr = 0;
				return 0;
			}
			int result = 0;
			int num;
			fixed (byte* ptr = bytes)
			{
				if (this._isAsync)
				{
					num = Win32Native.WriteFile(handle, ptr + offset, count, IntPtr.Zero, overlapped);
				}
				else
				{
					num = Win32Native.WriteFile(handle, ptr + offset, count, out result, IntPtr.Zero);
				}
			}
			if (num != 0)
			{
				hr = 0;
				return result;
			}
			hr = Marshal.GetLastWin32Error();
			if (hr == 232)
			{
				return -1;
			}
			if (hr == 6 && !this._handle.IsInvalid)
			{
				this._handle.Dispose();
			}
			return -1;
		}

		// Token: 0x04001C5D RID: 7261
		internal const int DefaultBufferSize = 4096;

		// Token: 0x04001C5E RID: 7262
		private const int FILE_ATTRIBUTE_NORMAL = 128;

		// Token: 0x04001C5F RID: 7263
		private const int FILE_ATTRIBUTE_ENCRYPTED = 16384;

		// Token: 0x04001C60 RID: 7264
		private const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x04001C61 RID: 7265
		internal const int GENERIC_READ = -2147483648;

		// Token: 0x04001C62 RID: 7266
		private const int GENERIC_WRITE = 1073741824;

		// Token: 0x04001C63 RID: 7267
		private const int FILE_BEGIN = 0;

		// Token: 0x04001C64 RID: 7268
		private const int FILE_CURRENT = 1;

		// Token: 0x04001C65 RID: 7269
		private const int FILE_END = 2;

		// Token: 0x04001C66 RID: 7270
		private const int ERROR_BROKEN_PIPE = 109;

		// Token: 0x04001C67 RID: 7271
		private const int ERROR_NO_DATA = 232;

		// Token: 0x04001C68 RID: 7272
		private const int ERROR_HANDLE_EOF = 38;

		// Token: 0x04001C69 RID: 7273
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001C6A RID: 7274
		private const int ERROR_IO_PENDING = 997;

		// Token: 0x04001C6B RID: 7275
		private static readonly bool _canUseAsync = Environment.RunningOnWinNT;

		// Token: 0x04001C6C RID: 7276
		private static readonly IOCompletionCallback IOCallback = new IOCompletionCallback(FileStream.AsyncFSCallback);

		// Token: 0x04001C6D RID: 7277
		private byte[] _buffer;

		// Token: 0x04001C6E RID: 7278
		private string _fileName;

		// Token: 0x04001C6F RID: 7279
		private bool _isAsync;

		// Token: 0x04001C70 RID: 7280
		private bool _canRead;

		// Token: 0x04001C71 RID: 7281
		private bool _canWrite;

		// Token: 0x04001C72 RID: 7282
		private bool _canSeek;

		// Token: 0x04001C73 RID: 7283
		private bool _exposedHandle;

		// Token: 0x04001C74 RID: 7284
		private bool _isPipe;

		// Token: 0x04001C75 RID: 7285
		private int _readPos;

		// Token: 0x04001C76 RID: 7286
		private int _readLen;

		// Token: 0x04001C77 RID: 7287
		private int _writePos;

		// Token: 0x04001C78 RID: 7288
		private int _bufferSize;

		// Token: 0x04001C79 RID: 7289
		private SafeFileHandle _handle;

		// Token: 0x04001C7A RID: 7290
		private long _pos;

		// Token: 0x04001C7B RID: 7291
		private long _appendStart;
	}
}
