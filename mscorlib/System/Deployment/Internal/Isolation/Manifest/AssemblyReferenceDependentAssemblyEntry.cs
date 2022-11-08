using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C4 RID: 452
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceDependentAssemblyEntry : IDisposable
	{
		// Token: 0x060014AD RID: 5293 RVA: 0x0003692C File Offset: 0x0003592C
		~AssemblyReferenceDependentAssemblyEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0003695C File Offset: 0x0003595C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00036965 File Offset: 0x00035965
		public void Dispose(bool fDisposing)
		{
			if (this.HashValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.HashValue);
				this.HashValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x040007B9 RID: 1977
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Group;

		// Token: 0x040007BA RID: 1978
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Codebase;

		// Token: 0x040007BB RID: 1979
		public ulong Size;

		// Token: 0x040007BC RID: 1980
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr HashValue;

		// Token: 0x040007BD RID: 1981
		public uint HashValueSize;

		// Token: 0x040007BE RID: 1982
		public uint HashAlgorithm;

		// Token: 0x040007BF RID: 1983
		public uint Flags;

		// Token: 0x040007C0 RID: 1984
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ResourceFallbackCulture;

		// Token: 0x040007C1 RID: 1985
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x040007C2 RID: 1986
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportUrl;

		// Token: 0x040007C3 RID: 1987
		public ISection HashElements;
	}
}
