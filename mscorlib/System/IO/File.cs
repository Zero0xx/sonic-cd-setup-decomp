using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x020005B4 RID: 1460
	[ComVisible(true)]
	public static class File
	{
		// Token: 0x060035B1 RID: 13745 RVA: 0x000B2FF5 File Offset: 0x000B1FF5
		public static StreamReader OpenText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamReader(path);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000B300B File Offset: 0x000B200B
		public static StreamWriter CreateText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, false);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000B3022 File Offset: 0x000B2022
		public static StreamWriter AppendText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, true);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000B3039 File Offset: 0x000B2039
		public static void Copy(string sourceFileName, string destFileName)
		{
			File.Copy(sourceFileName, destFileName, false);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000B3043 File Offset: 0x000B2043
		public static void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			File.InternalCopy(sourceFileName, destFileName, overwrite);
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000B3050 File Offset: 0x000B2050
		internal static string InternalCopy(string sourceFileName, string destFileName, bool overwrite)
		{
			if (sourceFileName == null || destFileName == null)
			{
				throw new ArgumentNullException((sourceFileName == null) ? "sourceFileName" : "destFileName", Environment.GetResourceString("ArgumentNull_FileName"));
			}
			if (sourceFileName.Length == 0 || destFileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), (sourceFileName.Length == 0) ? "sourceFileName" : "destFileName");
			}
			string fullPathInternal = Path.GetFullPathInternal(sourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			string fullPathInternal2 = Path.GetFullPathInternal(destFileName);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal2
			}, false, false).Demand();
			if (!Win32Native.CopyFile(fullPathInternal, fullPathInternal2, !overwrite))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				string maybeFullPath = destFileName;
				if (lastWin32Error != 80)
				{
					using (SafeFileHandle safeFileHandle = Win32Native.UnsafeCreateFile(fullPathInternal, int.MinValue, FileShare.Read, null, FileMode.Open, 0, IntPtr.Zero))
					{
						if (safeFileHandle.IsInvalid)
						{
							maybeFullPath = sourceFileName;
						}
					}
					if (lastWin32Error == 5 && Directory.InternalExists(fullPathInternal2))
					{
						throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_FileIsDirectory_Name"), new object[]
						{
							destFileName
						}), 5, fullPathInternal2);
					}
				}
				__Error.WinIOError(lastWin32Error, maybeFullPath);
			}
			return fullPathInternal2;
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000B319C File Offset: 0x000B219C
		public static FileStream Create(string path)
		{
			return File.Create(path, 4096, FileOptions.None);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000B31AA File Offset: 0x000B21AA
		public static FileStream Create(string path, int bufferSize)
		{
			return File.Create(path, bufferSize, FileOptions.None);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000B31B4 File Offset: 0x000B21B4
		public static FileStream Create(string path, int bufferSize, FileOptions options)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000B31C1 File Offset: 0x000B21C1
		public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			return new FileStream(path, FileMode.Create, FileSystemRights.ReadData | FileSystemRights.WriteData | FileSystemRights.AppendData | FileSystemRights.ReadExtendedAttributes | FileSystemRights.WriteExtendedAttributes | FileSystemRights.ReadAttributes | FileSystemRights.WriteAttributes | FileSystemRights.ReadPermissions, FileShare.None, bufferSize, options, fileSecurity);
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000B31D4 File Offset: 0x000B21D4
		public static void Delete(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			if (Environment.IsWin9X() && Directory.InternalExists(fullPathInternal))
			{
				throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[]
				{
					path
				}));
			}
			if (!Win32Native.DeleteFile(fullPathInternal))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 2)
				{
					return;
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000B3264 File Offset: 0x000B2264
		public static void Decrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!Environment.RunningOnWinNT)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			if (!Win32Native.DecryptFile(fullPathInternal, 0))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 5)
				{
					DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(fullPathInternal));
					if (!string.Equals("NTFS", driveInfo.DriveFormat))
					{
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_EncryptionNeedsNTFS"));
					}
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000B3304 File Offset: 0x000B2304
		public static void Encrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!Environment.RunningOnWinNT)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			if (!Win32Native.EncryptFile(fullPathInternal))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 5)
				{
					DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(fullPathInternal));
					if (!string.Equals("NTFS", driveInfo.DriveFormat))
					{
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_EncryptionNeedsNTFS"));
					}
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000B33A4 File Offset: 0x000B23A4
		public static bool Exists(string path)
		{
			try
			{
				if (path == null)
				{
					return false;
				}
				if (path.Length == 0)
				{
					return false;
				}
				path = Path.GetFullPathInternal(path);
				new FileIOPermission(FileIOPermissionAccess.Read, new string[]
				{
					path
				}, false, false).Demand();
				return File.InternalExists(path);
			}
			catch (ArgumentException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			return false;
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000B3440 File Offset: 0x000B2440
		internal static bool InternalExists(string path)
		{
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			return File.FillAttributeInfo(path, ref win32_FILE_ATTRIBUTE_DATA, false, true) == 0 && win32_FILE_ATTRIBUTE_DATA.fileAttributes != -1 && (win32_FILE_ATTRIBUTE_DATA.fileAttributes & 16) == 0;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000B347C File Offset: 0x000B247C
		public static FileStream Open(string path, FileMode mode)
		{
			return File.Open(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000B348E File Offset: 0x000B248E
		public static FileStream Open(string path, FileMode mode, FileAccess access)
		{
			return File.Open(path, mode, access, FileShare.None);
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000B3499 File Offset: 0x000B2499
		public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(path, mode, access, share);
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x000B34A4 File Offset: 0x000B24A4
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			File.SetCreationTimeUtc(path, creationTime.ToUniversalTime());
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000B34B4 File Offset: 0x000B24B4
		public unsafe static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			SafeFileHandle hFile;
			using (File.OpenFile(path, FileAccess.Write, out hFile))
			{
				Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(creationTimeUtc.ToFileTimeUtc());
				if (!Win32Native.SetFileTime(hFile, &file_TIME, null, null))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					__Error.WinIOError(lastWin32Error, path);
				}
			}
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000B351C File Offset: 0x000B251C
		public static DateTime GetCreationTime(string path)
		{
			return File.GetCreationTimeUtc(path).ToLocalTime();
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000B3538 File Offset: 0x000B2538
		public static DateTime GetCreationTimeUtc(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, false);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			long fileTime = (long)((ulong)win32_FILE_ATTRIBUTE_DATA.ftCreationTimeHigh << 32 | (ulong)win32_FILE_ATTRIBUTE_DATA.ftCreationTimeLow);
			return DateTime.FromFileTimeUtc(fileTime);
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000B35A0 File Offset: 0x000B25A0
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			File.SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000B35B0 File Offset: 0x000B25B0
		public unsafe static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			SafeFileHandle hFile;
			using (File.OpenFile(path, FileAccess.Write, out hFile))
			{
				Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(lastAccessTimeUtc.ToFileTimeUtc());
				if (!Win32Native.SetFileTime(hFile, null, &file_TIME, null))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					__Error.WinIOError(lastWin32Error, path);
				}
			}
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000B3618 File Offset: 0x000B2618
		public static DateTime GetLastAccessTime(string path)
		{
			return File.GetLastAccessTimeUtc(path).ToLocalTime();
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000B3634 File Offset: 0x000B2634
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, false);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			long fileTime = (long)((ulong)win32_FILE_ATTRIBUTE_DATA.ftLastAccessTimeHigh << 32 | (ulong)win32_FILE_ATTRIBUTE_DATA.ftLastAccessTimeLow);
			return DateTime.FromFileTimeUtc(fileTime);
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000B369C File Offset: 0x000B269C
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			File.SetLastWriteTimeUtc(path, lastWriteTime.ToUniversalTime());
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000B36AC File Offset: 0x000B26AC
		public unsafe static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			SafeFileHandle hFile;
			using (File.OpenFile(path, FileAccess.Write, out hFile))
			{
				Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(lastWriteTimeUtc.ToFileTimeUtc());
				if (!Win32Native.SetFileTime(hFile, null, null, &file_TIME))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					__Error.WinIOError(lastWin32Error, path);
				}
			}
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000B3714 File Offset: 0x000B2714
		public static DateTime GetLastWriteTime(string path)
		{
			return File.GetLastWriteTimeUtc(path).ToLocalTime();
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000B3730 File Offset: 0x000B2730
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, false);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			long fileTime = (long)((ulong)win32_FILE_ATTRIBUTE_DATA.ftLastWriteTimeHigh << 32 | (ulong)win32_FILE_ATTRIBUTE_DATA.ftLastWriteTimeLow);
			return DateTime.FromFileTimeUtc(fileTime);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x000B3798 File Offset: 0x000B2798
		public static FileAttributes GetAttributes(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, true);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			return (FileAttributes)win32_FILE_ATTRIBUTE_DATA.fileAttributes;
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x000B37EC File Offset: 0x000B27EC
		public static void SetAttributes(string path, FileAttributes fileAttributes)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			if (!Win32Native.SetFileAttributes(fullPathInternal, (int)fileAttributes))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 87 || (lastWin32Error == 5 && Environment.IsWin9X()))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidFileAttrs"));
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x000B3851 File Offset: 0x000B2851
		public static FileSecurity GetAccessControl(string path)
		{
			return File.GetAccessControl(path, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x000B385B File Offset: 0x000B285B
		public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			return new FileSecurity(path, includeSections);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x000B3864 File Offset: 0x000B2864
		public static void SetAccessControl(string path, FileSecurity fileSecurity)
		{
			if (fileSecurity == null)
			{
				throw new ArgumentNullException("fileSecurity");
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			fileSecurity.Persist(fullPathInternal);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x000B388D File Offset: 0x000B288D
		public static FileStream OpenRead(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x000B3898 File Offset: 0x000B2898
		public static FileStream OpenWrite(string path)
		{
			return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000B38A3 File Offset: 0x000B28A3
		public static string ReadAllText(string path)
		{
			return File.ReadAllText(path, Encoding.UTF8);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000B38B0 File Offset: 0x000B28B0
		public static string ReadAllText(string path, Encoding encoding)
		{
			string result;
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				result = streamReader.ReadToEnd();
			}
			return result;
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000B38EC File Offset: 0x000B28EC
		public static void WriteAllText(string path, string contents)
		{
			File.WriteAllText(path, contents, StreamWriter.UTF8NoBOM);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000B38FC File Offset: 0x000B28FC
		public static void WriteAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000B3938 File Offset: 0x000B2938
		public static byte[] ReadAllBytes(string path)
		{
			byte[] array;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				int num = 0;
				long length = fileStream.Length;
				if (length > 2147483647L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_FileTooLong2GB"));
				}
				int i = (int)length;
				array = new byte[i];
				while (i > 0)
				{
					int num2 = fileStream.Read(array, num, i);
					if (num2 == 0)
					{
						__Error.EndOfFile();
					}
					num += num2;
					i -= num2;
				}
			}
			return array;
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x000B39C4 File Offset: 0x000B29C4
		public static void WriteAllBytes(string path, byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x000B3A10 File Offset: 0x000B2A10
		public static string[] ReadAllLines(string path)
		{
			return File.ReadAllLines(path, Encoding.UTF8);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000B3A20 File Offset: 0x000B2A20
		public static string[] ReadAllLines(string path, Encoding encoding)
		{
			ArrayList arrayList = new ArrayList();
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				string value;
				while ((value = streamReader.ReadLine()) != null)
				{
					arrayList.Add(value);
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000B3A80 File Offset: 0x000B2A80
		public static void WriteAllLines(string path, string[] contents)
		{
			File.WriteAllLines(path, contents, StreamWriter.UTF8NoBOM);
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000B3A90 File Offset: 0x000B2A90
		public static void WriteAllLines(string path, string[] contents, Encoding encoding)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
			{
				foreach (string value in contents)
				{
					streamWriter.WriteLine(value);
				}
			}
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000B3AEC File Offset: 0x000B2AEC
		public static void AppendAllText(string path, string contents)
		{
			File.AppendAllText(path, contents, StreamWriter.UTF8NoBOM);
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000B3AFC File Offset: 0x000B2AFC
		public static void AppendAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, true, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000B3B38 File Offset: 0x000B2B38
		public static void Move(string sourceFileName, string destFileName)
		{
			if (sourceFileName == null || destFileName == null)
			{
				throw new ArgumentNullException((sourceFileName == null) ? "sourceFileName" : "destFileName", Environment.GetResourceString("ArgumentNull_FileName"));
			}
			if (sourceFileName.Length == 0 || destFileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), (sourceFileName.Length == 0) ? "sourceFileName" : "destFileName");
			}
			string fullPathInternal = Path.GetFullPathInternal(sourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal
			}, false, false).Demand();
			string fullPathInternal2 = Path.GetFullPathInternal(destFileName);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal2
			}, false, false).Demand();
			if (!File.InternalExists(fullPathInternal))
			{
				__Error.WinIOError(2, fullPathInternal);
			}
			if (!Win32Native.MoveFile(fullPathInternal, fullPathInternal2))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000B3BFB File Offset: 0x000B2BFB
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000B3C08 File Offset: 0x000B2C08
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			string fullPathInternal = Path.GetFullPathInternal(sourceFileName);
			string fullPathInternal2 = Path.GetFullPathInternal(destinationFileName);
			string text = null;
			if (destinationBackupFileName != null)
			{
				text = Path.GetFullPathInternal(destinationBackupFileName);
			}
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[]
			{
				fullPathInternal,
				fullPathInternal2
			});
			if (destinationBackupFileName != null)
			{
				fileIOPermission.AddPathList(FileIOPermissionAccess.Write, text);
			}
			fileIOPermission.Demand();
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			int num = 1;
			if (ignoreMetadataErrors)
			{
				num |= 2;
			}
			if (!Win32Native.ReplaceFile(fullPathInternal2, fullPathInternal, text, num, IntPtr.Zero, IntPtr.Zero))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000B3CC0 File Offset: 0x000B2CC0
		internal static int FillAttributeInfo(string path, ref Win32Native.WIN32_FILE_ATTRIBUTE_DATA data, bool tryagain, bool returnErrorOnNotFound)
		{
			int num = 0;
			if (Environment.OSInfo == Environment.OSName.Win95 || tryagain)
			{
				Win32Native.WIN32_FIND_DATA win32_FIND_DATA = new Win32Native.WIN32_FIND_DATA();
				string fileName = path.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});
				int errorMode = Win32Native.SetErrorMode(1);
				try
				{
					bool flag = false;
					SafeFindHandle safeFindHandle = Win32Native.FindFirstFile(fileName, win32_FIND_DATA);
					try
					{
						if (safeFindHandle.IsInvalid)
						{
							flag = true;
							num = Marshal.GetLastWin32Error();
							if ((num == 2 || num == 3 || num == 21) && !returnErrorOnNotFound)
							{
								num = 0;
								data.fileAttributes = -1;
							}
							return num;
						}
					}
					finally
					{
						try
						{
							safeFindHandle.Close();
						}
						catch
						{
							if (!flag)
							{
								__Error.WinIOError();
							}
						}
					}
				}
				finally
				{
					Win32Native.SetErrorMode(errorMode);
				}
				data.fileAttributes = win32_FIND_DATA.dwFileAttributes;
				data.ftCreationTimeLow = (uint)win32_FIND_DATA.ftCreationTime_dwLowDateTime;
				data.ftCreationTimeHigh = (uint)win32_FIND_DATA.ftCreationTime_dwHighDateTime;
				data.ftLastAccessTimeLow = (uint)win32_FIND_DATA.ftLastAccessTime_dwLowDateTime;
				data.ftLastAccessTimeHigh = (uint)win32_FIND_DATA.ftLastAccessTime_dwHighDateTime;
				data.ftLastWriteTimeLow = (uint)win32_FIND_DATA.ftLastWriteTime_dwLowDateTime;
				data.ftLastWriteTimeHigh = (uint)win32_FIND_DATA.ftLastWriteTime_dwHighDateTime;
				data.fileSizeHigh = win32_FIND_DATA.nFileSizeHigh;
				data.fileSizeLow = win32_FIND_DATA.nFileSizeLow;
				return num;
			}
			bool flag2 = false;
			int errorMode2 = Win32Native.SetErrorMode(1);
			try
			{
				flag2 = Win32Native.GetFileAttributesEx(path, 0, ref data);
			}
			finally
			{
				Win32Native.SetErrorMode(errorMode2);
			}
			if (!flag2)
			{
				num = Marshal.GetLastWin32Error();
				if (num != 2 && num != 3 && num != 21)
				{
					return File.FillAttributeInfo(path, ref data, true, returnErrorOnNotFound);
				}
				if (!returnErrorOnNotFound)
				{
					num = 0;
					data.fileAttributes = -1;
				}
			}
			return num;
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000B3E64 File Offset: 0x000B2E64
		private static FileStream OpenFile(string path, FileAccess access, out SafeFileHandle handle)
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, access, FileShare.ReadWrite, 1);
			handle = fileStream.SafeFileHandle;
			if (handle.IsInvalid)
			{
				int num = Marshal.GetLastWin32Error();
				string fullPathInternal = Path.GetFullPathInternal(path);
				if (num == 3 && fullPathInternal.Equals(Directory.GetDirectoryRoot(fullPathInternal)))
				{
					num = 5;
				}
				__Error.WinIOError(num, path);
			}
			return fileStream;
		}

		// Token: 0x04001C2F RID: 7215
		private const int GetFileExInfoStandard = 0;

		// Token: 0x04001C30 RID: 7216
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001C31 RID: 7217
		private const int ERROR_ACCESS_DENIED = 5;
	}
}
