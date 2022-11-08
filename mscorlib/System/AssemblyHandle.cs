using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x0200010D RID: 269
	internal struct AssemblyHandle
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0002CDDD File Offset: 0x0002BDDD
		internal unsafe void* Value
		{
			get
			{
				return this.m_ptr.ToPointer();
			}
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0002CDEA File Offset: 0x0002BDEA
		internal unsafe AssemblyHandle(void* pAssembly)
		{
			this.m_ptr = new IntPtr(pAssembly);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0002CDF8 File Offset: 0x0002BDF8
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0002CE08 File Offset: 0x0002BE08
		public override bool Equals(object obj)
		{
			return obj is AssemblyHandle && ((AssemblyHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0002CE38 File Offset: 0x0002BE38
		public bool Equals(AssemblyHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x06000F90 RID: 3984
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Assembly GetAssembly();

		// Token: 0x06000F91 RID: 3985
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetManifestModule();

		// Token: 0x06000F92 RID: 3986 RVA: 0x0002CE4C File Offset: 0x0002BE4C
		internal ModuleHandle GetManifestModule()
		{
			return new ModuleHandle(this._GetManifestModule());
		}

		// Token: 0x06000F93 RID: 3987
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _AptcaCheck(IntPtr sourceAssembly);

		// Token: 0x06000F94 RID: 3988 RVA: 0x0002CE59 File Offset: 0x0002BE59
		internal bool AptcaCheck(AssemblyHandle sourceAssembly)
		{
			return this._AptcaCheck((IntPtr)sourceAssembly.Value);
		}

		// Token: 0x06000F95 RID: 3989
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetToken();

		// Token: 0x0400053C RID: 1340
		private IntPtr m_ptr;
	}
}
