using System;
using System.Security.Policy;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200047B RID: 1147
	internal sealed class SafePEFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002DA7 RID: 11687 RVA: 0x00098F0F File Offset: 0x00097F0F
		private SafePEFileHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002DA8 RID: 11688 RVA: 0x00098F1F File Offset: 0x00097F1F
		internal static SafePEFileHandle InvalidHandle
		{
			get
			{
				return new SafePEFileHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x00098F2B File Offset: 0x00097F2B
		protected override bool ReleaseHandle()
		{
			Hash._ReleasePEFile(this.handle);
			return true;
		}
	}
}
