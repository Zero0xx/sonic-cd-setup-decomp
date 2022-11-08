using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000548 RID: 1352
	internal class SecurityPackageInfoClass
	{
		// Token: 0x06002913 RID: 10515 RVA: 0x000AB7FC File Offset: 0x000AA7FC
		internal SecurityPackageInfoClass(SafeHandle safeHandle, int index)
		{
			if (safeHandle.IsInvalid)
			{
				return;
			}
			IntPtr ptr = IntPtrHelper.Add(safeHandle.DangerousGetHandle(), SecurityPackageInfo.Size * index);
			this.Capabilities = Marshal.ReadInt32(ptr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Capabilities"));
			this.Version = Marshal.ReadInt16(ptr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Version"));
			this.RPCID = Marshal.ReadInt16(ptr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "RPCID"));
			this.MaxToken = Marshal.ReadInt32(ptr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "MaxToken"));
			IntPtr intPtr = Marshal.ReadIntPtr(ptr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Name"));
			if (intPtr != IntPtr.Zero)
			{
				if (ComNetOS.IsWin9x)
				{
					this.Name = Marshal.PtrToStringAnsi(intPtr);
				}
				else
				{
					this.Name = Marshal.PtrToStringUni(intPtr);
				}
			}
			intPtr = Marshal.ReadIntPtr(ptr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Comment"));
			if (intPtr != IntPtr.Zero)
			{
				if (ComNetOS.IsWin9x)
				{
					this.Comment = Marshal.PtrToStringAnsi(intPtr);
					return;
				}
				this.Comment = Marshal.PtrToStringUni(intPtr);
			}
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x000AB95C File Offset: 0x000AA95C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Capabilities:",
				string.Format(CultureInfo.InvariantCulture, "0x{0:x}", new object[]
				{
					this.Capabilities
				}),
				" Version:",
				this.Version.ToString(NumberFormatInfo.InvariantInfo),
				" RPCID:",
				this.RPCID.ToString(NumberFormatInfo.InvariantInfo),
				" MaxToken:",
				this.MaxToken.ToString(NumberFormatInfo.InvariantInfo),
				" Name:",
				(this.Name == null) ? "(null)" : this.Name,
				" Comment:",
				(this.Comment == null) ? "(null)" : this.Comment
			});
		}

		// Token: 0x04002824 RID: 10276
		internal int Capabilities;

		// Token: 0x04002825 RID: 10277
		internal short Version;

		// Token: 0x04002826 RID: 10278
		internal short RPCID;

		// Token: 0x04002827 RID: 10279
		internal int MaxToken;

		// Token: 0x04002828 RID: 10280
		internal string Name;

		// Token: 0x04002829 RID: 10281
		internal string Comment;
	}
}
