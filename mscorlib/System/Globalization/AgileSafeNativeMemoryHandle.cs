using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Globalization
{
	// Token: 0x020003E9 RID: 1001
	internal sealed class AgileSafeNativeMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002901 RID: 10497 RVA: 0x0007EF74 File Offset: 0x0007DF74
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal AgileSafeNativeMemoryHandle() : base(true)
		{
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x0007EF7D File Offset: 0x0007DF7D
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal AgileSafeNativeMemoryHandle(IntPtr handle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x0007EF8D File Offset: 0x0007DF8D
		internal AgileSafeNativeMemoryHandle(string fileName) : this(fileName, null)
		{
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x0007EF98 File Offset: 0x0007DF98
		internal unsafe AgileSafeNativeMemoryHandle(string fileName, string fileMappingName) : base(true)
		{
			this.mode = true;
			SafeFileHandle safeFileHandle = Win32Native.UnsafeCreateFile(fileName, int.MinValue, FileShare.Read, null, FileMode.Open, 0, IntPtr.Zero);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (safeFileHandle.IsInvalid)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[]
				{
					lastWin32Error
				}));
			}
			int num2;
			int num = Win32Native.GetFileSize(safeFileHandle, out num2);
			if (num == -1)
			{
				safeFileHandle.Close();
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[]
				{
					lastWin32Error
				}));
			}
			this.fileSize = ((long)num2 << 32 | (long)((ulong)num));
			if (this.fileSize == 0L)
			{
				safeFileHandle.Close();
				return;
			}
			SafeFileMappingHandle safeFileMappingHandle = Win32Native.CreateFileMapping(safeFileHandle, IntPtr.Zero, 2U, 0U, 0U, fileMappingName);
			lastWin32Error = Marshal.GetLastWin32Error();
			safeFileHandle.Close();
			if (safeFileMappingHandle.IsInvalid)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[]
				{
					lastWin32Error
				}));
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.handle = Win32Native.MapViewOfFile(safeFileMappingHandle, 4U, 0U, 0U, UIntPtr.Zero);
			}
			lastWin32Error = Marshal.GetLastWin32Error();
			if (this.handle == IntPtr.Zero)
			{
				safeFileMappingHandle.Close();
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_UnexpectedWin32Error"), new object[]
				{
					lastWin32Error
				}));
			}
			this.bytes = (byte*)((void*)base.DangerousGetHandle());
			safeFileMappingHandle.Close();
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002905 RID: 10501 RVA: 0x0007F14C File Offset: 0x0007E14C
		internal long FileSize
		{
			get
			{
				return this.fileSize;
			}
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x0007F154 File Offset: 0x0007E154
		internal unsafe byte* GetBytePtr()
		{
			return this.bytes;
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x0007F15C File Offset: 0x0007E15C
		protected override bool ReleaseHandle()
		{
			if (!this.IsInvalid)
			{
				if (!this.mode)
				{
					Marshal.FreeHGlobal(this.handle);
					this.handle = IntPtr.Zero;
					return true;
				}
				if (Win32Native.UnmapViewOfFile(this.handle))
				{
					this.handle = IntPtr.Zero;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040013F3 RID: 5107
		private const int PAGE_READONLY = 2;

		// Token: 0x040013F4 RID: 5108
		private const int SECTION_MAP_READ = 4;

		// Token: 0x040013F5 RID: 5109
		private unsafe byte* bytes;

		// Token: 0x040013F6 RID: 5110
		private long fileSize;

		// Token: 0x040013F7 RID: 5111
		private bool mode;
	}
}
