using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x020005A4 RID: 1444
	internal static class __Error
	{
		// Token: 0x060034D1 RID: 13521 RVA: 0x000AE7FC File Offset: 0x000AD7FC
		internal static void EndOfFile()
		{
			throw new EndOfStreamException(Environment.GetResourceString("IO.EOF_ReadBeyondEOF"));
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000AE80D File Offset: 0x000AD80D
		internal static void FileNotOpen()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_FileClosed"));
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000AE81F File Offset: 0x000AD81F
		internal static void StreamIsClosed()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_StreamClosed"));
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000AE831 File Offset: 0x000AD831
		internal static void MemoryStreamNotExpandable()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_MemStreamNotExpandable"));
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x000AE842 File Offset: 0x000AD842
		internal static void ReaderClosed()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ReaderClosed"));
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000AE854 File Offset: 0x000AD854
		internal static void ReadNotSupported()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnreadableStream"));
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000AE865 File Offset: 0x000AD865
		internal static void SeekNotSupported()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x000AE876 File Offset: 0x000AD876
		internal static void WrongAsyncResult()
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_WrongAsyncResult"));
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x000AE887 File Offset: 0x000AD887
		internal static void EndReadCalledTwice()
		{
			throw new ArgumentException(Environment.GetResourceString("InvalidOperation_EndReadCalledMultiple"));
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x000AE898 File Offset: 0x000AD898
		internal static void EndWriteCalledTwice()
		{
			throw new ArgumentException(Environment.GetResourceString("InvalidOperation_EndWriteCalledMultiple"));
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000AE8AC File Offset: 0x000AD8AC
		internal static string GetDisplayablePath(string path, bool isInvalidPath)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			bool flag = false;
			if (path.Length < 2)
			{
				return path;
			}
			if (Path.IsDirectorySeparator(path[0]) && Path.IsDirectorySeparator(path[1]))
			{
				flag = true;
			}
			else if (path[1] == Path.VolumeSeparatorChar)
			{
				flag = true;
			}
			if (!flag && !isInvalidPath)
			{
				return path;
			}
			bool flag2 = false;
			try
			{
				if (!isInvalidPath)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[]
					{
						path
					}, false, false).Demand();
					flag2 = true;
				}
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
			if (!flag2)
			{
				if (Path.IsDirectorySeparator(path[path.Length - 1]))
				{
					path = Environment.GetResourceString("IO.IO_NoPermissionToDirectoryName");
				}
				else
				{
					path = Path.GetFileName(path);
				}
			}
			return path;
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000AE98C File Offset: 0x000AD98C
		internal static void WinIOError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			__Error.WinIOError(lastWin32Error, string.Empty);
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x000AE9AC File Offset: 0x000AD9AC
		internal static void WinIOError(int errorCode, string maybeFullPath)
		{
			bool isInvalidPath = errorCode == 123 || errorCode == 161;
			string displayablePath = __Error.GetDisplayablePath(maybeFullPath, isInvalidPath);
			if (errorCode <= 80)
			{
				if (errorCode <= 15)
				{
					switch (errorCode)
					{
					case 2:
						if (displayablePath.Length == 0)
						{
							throw new FileNotFoundException(Environment.GetResourceString("IO.FileNotFound"));
						}
						throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[]
						{
							displayablePath
						}), displayablePath);
					case 3:
						if (displayablePath.Length == 0)
						{
							throw new DirectoryNotFoundException(Environment.GetResourceString("IO.PathNotFound_NoPathName"));
						}
						throw new DirectoryNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.PathNotFound_Path"), new object[]
						{
							displayablePath
						}));
					case 4:
						break;
					case 5:
						if (displayablePath.Length == 0)
						{
							throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_IODenied_NoPathName"));
						}
						throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[]
						{
							displayablePath
						}));
					default:
						if (errorCode == 15)
						{
							throw new DriveNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.DriveNotFound_Drive"), new object[]
							{
								displayablePath
							}));
						}
						break;
					}
				}
				else if (errorCode != 32)
				{
					if (errorCode == 80)
					{
						if (displayablePath.Length != 0)
						{
							throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.IO_FileExists_Name"), new object[]
							{
								displayablePath
							}), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
						}
					}
				}
				else
				{
					if (displayablePath.Length == 0)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_SharingViolation_NoFileName"), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
					}
					throw new IOException(Environment.GetResourceString("IO.IO_SharingViolation_File", new object[]
					{
						displayablePath
					}), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
				}
			}
			else if (errorCode <= 183)
			{
				if (errorCode == 87)
				{
					throw new IOException(Win32Native.GetMessage(errorCode), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
				}
				if (errorCode == 183)
				{
					if (displayablePath.Length != 0)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_AlreadyExists_Name", new object[]
						{
							displayablePath
						}), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
					}
				}
			}
			else
			{
				if (errorCode == 206)
				{
					throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
				}
				if (errorCode == 995)
				{
					throw new OperationCanceledException();
				}
			}
			throw new IOException(Win32Native.GetMessage(errorCode), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x000AEC28 File Offset: 0x000ADC28
		internal static void WinIODriveError(string driveName)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			__Error.WinIODriveError(driveName, lastWin32Error);
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x000AEC44 File Offset: 0x000ADC44
		internal static void WinIODriveError(string driveName, int errorCode)
		{
			if (errorCode == 3 || errorCode == 15)
			{
				throw new DriveNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.DriveNotFound_Drive"), new object[]
				{
					driveName
				}));
			}
			__Error.WinIOError(errorCode, driveName);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x000AEC89 File Offset: 0x000ADC89
		internal static void WriteNotSupported()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnwritableStream"));
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x000AEC9A File Offset: 0x000ADC9A
		internal static void WriterClosed()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_WriterClosed"));
		}

		// Token: 0x04001BEC RID: 7148
		internal const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x04001BED RID: 7149
		internal const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x04001BEE RID: 7150
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04001BEF RID: 7151
		internal const int ERROR_INVALID_PARAMETER = 87;
	}
}
