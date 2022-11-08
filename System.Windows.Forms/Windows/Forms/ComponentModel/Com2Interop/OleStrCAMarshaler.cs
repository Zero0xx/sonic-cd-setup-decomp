using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000779 RID: 1913
	internal class OleStrCAMarshaler : BaseCAMarshaler
	{
		// Token: 0x06006495 RID: 25749 RVA: 0x00170017 File Offset: 0x0016F017
		public OleStrCAMarshaler(NativeMethods.CA_STRUCT caAddr) : base(caAddr)
		{
		}

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x06006496 RID: 25750 RVA: 0x00170020 File Offset: 0x0016F020
		public override Type ItemType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x06006497 RID: 25751 RVA: 0x0017002C File Offset: 0x0016F02C
		protected override Array CreateArray()
		{
			return new string[base.Count];
		}

		// Token: 0x06006498 RID: 25752 RVA: 0x0017003C File Offset: 0x0016F03C
		protected override object GetItemFromAddress(IntPtr addr)
		{
			string result = Marshal.PtrToStringUni(addr);
			Marshal.FreeCoTaskMem(addr);
			return result;
		}
	}
}
