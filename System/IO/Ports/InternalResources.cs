using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace System.IO.Ports
{
	// Token: 0x020007A7 RID: 1959
	internal static class InternalResources
	{
		// Token: 0x06003C21 RID: 15393 RVA: 0x001010C0 File Offset: 0x001000C0
		internal static void EndOfFile()
		{
			throw new EndOfStreamException(SR.GetString("IO_EOF_ReadBeyondEOF"));
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x001010D4 File Offset: 0x001000D4
		internal static string GetMessage(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = SafeNativeMethods.FormatMessage(12800, new HandleRef(null, IntPtr.Zero), errorCode, 0, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			return SR.GetString("IO_UnknownError", new object[]
			{
				errorCode
			});
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x00101137 File Offset: 0x00100137
		internal static void FileNotOpen()
		{
			throw new ObjectDisposedException(null, SR.GetString("Port_not_open"));
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x00101149 File Offset: 0x00100149
		internal static void WrongAsyncResult()
		{
			throw new ArgumentException(SR.GetString("Arg_WrongAsyncResult"));
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x0010115A File Offset: 0x0010015A
		internal static void EndReadCalledTwice()
		{
			throw new ArgumentException(SR.GetString("InvalidOperation_EndReadCalledMultiple"));
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x0010116B File Offset: 0x0010016B
		internal static void EndWriteCalledTwice()
		{
			throw new ArgumentException(SR.GetString("InvalidOperation_EndWriteCalledMultiple"));
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x0010117C File Offset: 0x0010017C
		internal static void WinIOError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			InternalResources.WinIOError(lastWin32Error, string.Empty);
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0010119C File Offset: 0x0010019C
		internal static void WinIOError(string str)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			InternalResources.WinIOError(lastWin32Error, str);
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x001011B8 File Offset: 0x001001B8
		internal static void WinIOError(int errorCode, string str)
		{
			switch (errorCode)
			{
			case 2:
			case 3:
				if (str.Length == 0)
				{
					throw new IOException(SR.GetString("IO_PortNotFound"));
				}
				throw new IOException(SR.GetString("IO_PortNotFoundFileName", new object[]
				{
					str
				}));
			case 4:
				break;
			case 5:
				if (str.Length == 0)
				{
					throw new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_NoPathName"));
				}
				throw new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_Path", new object[]
				{
					str
				}));
			default:
				if (errorCode != 32)
				{
					if (errorCode == 206)
					{
						throw new PathTooLongException(SR.GetString("IO_PathTooLong"));
					}
				}
				else
				{
					if (str.Length == 0)
					{
						throw new IOException(SR.GetString("IO_SharingViolation_NoFileName"));
					}
					throw new IOException(SR.GetString("IO_SharingViolation_File", new object[]
					{
						str
					}));
				}
				break;
			}
			throw new IOException(InternalResources.GetMessage(errorCode), InternalResources.MakeHRFromErrorCode(errorCode));
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x001012B1 File Offset: 0x001002B1
		internal static int MakeHRFromErrorCode(int errorCode)
		{
			return -2147024896 | errorCode;
		}
	}
}
