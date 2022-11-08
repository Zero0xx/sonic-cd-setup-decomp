using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AC RID: 428
	[StructLayout(LayoutKind.Sequential)]
	internal class FileEntry : IDisposable
	{
		// Token: 0x06001473 RID: 5235 RVA: 0x00036854 File Offset: 0x00035854
		~FileEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00036884 File Offset: 0x00035884
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00036890 File Offset: 0x00035890
		public void Dispose(bool fDisposing)
		{
			if (this.HashValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.HashValue);
				this.HashValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				if (this.MuiMapping != null)
				{
					this.MuiMapping.Dispose(true);
					this.MuiMapping = null;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000769 RID: 1897
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x0400076A RID: 1898
		public uint HashAlgorithm;

		// Token: 0x0400076B RID: 1899
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LoadFrom;

		// Token: 0x0400076C RID: 1900
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourcePath;

		// Token: 0x0400076D RID: 1901
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ImportPath;

		// Token: 0x0400076E RID: 1902
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SourceName;

		// Token: 0x0400076F RID: 1903
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Location;

		// Token: 0x04000770 RID: 1904
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x04000771 RID: 1905
		public uint HashValueSize;

		// Token: 0x04000772 RID: 1906
		public ulong Size;

		// Token: 0x04000773 RID: 1907
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x04000774 RID: 1908
		public uint Flags;

		// Token: 0x04000775 RID: 1909
		public MuiResourceMapEntry MuiMapping;

		// Token: 0x04000776 RID: 1910
		public uint WritableType;

		// Token: 0x04000777 RID: 1911
		public ISection HashElements;
	}
}
