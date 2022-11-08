using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x020005A3 RID: 1443
	internal sealed class __ConsoleStream : Stream
	{
		// Token: 0x060034C0 RID: 13504 RVA: 0x000AE55C File Offset: 0x000AD55C
		internal __ConsoleStream(SafeFileHandle handle, FileAccess access)
		{
			this._handle = handle;
			this._canRead = (access == FileAccess.Read);
			this._canWrite = (access == FileAccess.Write);
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x060034C1 RID: 13505 RVA: 0x000AE57F File Offset: 0x000AD57F
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060034C2 RID: 13506 RVA: 0x000AE587 File Offset: 0x000AD587
		public override bool CanWrite
		{
			get
			{
				return this._canWrite;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060034C3 RID: 13507 RVA: 0x000AE58F File Offset: 0x000AD58F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x060034C4 RID: 13508 RVA: 0x000AE592 File Offset: 0x000AD592
		public override long Length
		{
			get
			{
				__Error.SeekNotSupported();
				return 0L;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x060034C5 RID: 13509 RVA: 0x000AE59B File Offset: 0x000AD59B
		// (set) Token: 0x060034C6 RID: 13510 RVA: 0x000AE5A4 File Offset: 0x000AD5A4
		public override long Position
		{
			get
			{
				__Error.SeekNotSupported();
				return 0L;
			}
			set
			{
				__Error.SeekNotSupported();
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000AE5AB File Offset: 0x000AD5AB
		protected override void Dispose(bool disposing)
		{
			if (this._handle != null)
			{
				this._handle = null;
			}
			this._canRead = false;
			this._canWrite = false;
			base.Dispose(disposing);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000AE5D1 File Offset: 0x000AD5D1
		public override void Flush()
		{
			if (this._handle == null)
			{
				__Error.FileNotOpen();
			}
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000AE5ED File Offset: 0x000AD5ED
		public override void SetLength(long value)
		{
			__Error.SeekNotSupported();
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000AE5F4 File Offset: 0x000AD5F4
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((offset < 0) ? "offset" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (!this._canRead)
			{
				__Error.ReadNotSupported();
			}
			int errorCode = 0;
			int num = __ConsoleStream.ReadFileNative(this._handle, buffer, offset, count, 0, out errorCode);
			if (num == -1)
			{
				__Error.WinIOError(errorCode, string.Empty);
			}
			return num;
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000AE680 File Offset: 0x000AD680
		public override long Seek(long offset, SeekOrigin origin)
		{
			__Error.SeekNotSupported();
			return 0L;
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000AE68C File Offset: 0x000AD68C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((offset < 0) ? "offset" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (!this._canWrite)
			{
				__Error.WriteNotSupported();
			}
			int errorCode = 0;
			int num = __ConsoleStream.WriteFileNative(this._handle, buffer, offset, count, 0, out errorCode);
			if (num == -1)
			{
				__Error.WinIOError(errorCode, string.Empty);
			}
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000AE718 File Offset: 0x000AD718
		private unsafe static int ReadFileNative(SafeFileHandle hFile, byte[] bytes, int offset, int count, int mustBeZero, out int errorCode)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				errorCode = 0;
				return 0;
			}
			int result;
			int num;
			fixed (byte* ptr = bytes)
			{
				num = __ConsoleStream.ReadFile(hFile, ptr + offset, count, out result, Win32Native.NULL);
			}
			if (num != 0)
			{
				errorCode = 0;
				return result;
			}
			errorCode = Marshal.GetLastWin32Error();
			if (errorCode == 109)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000AE790 File Offset: 0x000AD790
		private unsafe static int WriteFileNative(SafeFileHandle hFile, byte[] bytes, int offset, int count, int mustBeZero, out int errorCode)
		{
			if (bytes.Length == 0)
			{
				errorCode = 0;
				return 0;
			}
			int result = 0;
			int num;
			fixed (byte* ptr = bytes)
			{
				num = __ConsoleStream.WriteFile(hFile, ptr + offset, count, out result, Win32Native.NULL);
			}
			if (num != 0)
			{
				errorCode = 0;
				return result;
			}
			errorCode = Marshal.GetLastWin32Error();
			if (errorCode == 232 || errorCode == 109)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x060034CF RID: 13519
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		private unsafe static extern int ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr mustBeZero);

		// Token: 0x060034D0 RID: 13520
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr mustBeZero);

		// Token: 0x04001BE6 RID: 7142
		internal const int DefaultBufferSize = 128;

		// Token: 0x04001BE7 RID: 7143
		private const int ERROR_BROKEN_PIPE = 109;

		// Token: 0x04001BE8 RID: 7144
		private const int ERROR_NO_DATA = 232;

		// Token: 0x04001BE9 RID: 7145
		private SafeFileHandle _handle;

		// Token: 0x04001BEA RID: 7146
		private bool _canRead;

		// Token: 0x04001BEB RID: 7147
		private bool _canWrite;
	}
}
