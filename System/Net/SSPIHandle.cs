using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200051B RID: 1307
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SSPIHandle
	{
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x000A5B7D File Offset: 0x000A4B7D
		public bool IsZero
		{
			get
			{
				return this.HandleHi == IntPtr.Zero && this.HandleLo == IntPtr.Zero;
			}
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000A5BA3 File Offset: 0x000A4BA3
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void SetToInvalid()
		{
			this.HandleHi = IntPtr.Zero;
			this.HandleLo = IntPtr.Zero;
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000A5BBB File Offset: 0x000A4BBB
		public override string ToString()
		{
			return this.HandleHi.ToString("x") + ":" + this.HandleLo.ToString("x");
		}

		// Token: 0x04002770 RID: 10096
		private IntPtr HandleHi;

		// Token: 0x04002771 RID: 10097
		private IntPtr HandleLo;
	}
}
