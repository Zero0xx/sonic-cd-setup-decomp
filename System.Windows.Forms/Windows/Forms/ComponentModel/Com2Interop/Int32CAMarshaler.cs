using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000778 RID: 1912
	internal class Int32CAMarshaler : BaseCAMarshaler
	{
		// Token: 0x06006491 RID: 25745 RVA: 0x0016FFE7 File Offset: 0x0016EFE7
		public Int32CAMarshaler(NativeMethods.CA_STRUCT caStruct) : base(caStruct)
		{
		}

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06006492 RID: 25746 RVA: 0x0016FFF0 File Offset: 0x0016EFF0
		public override Type ItemType
		{
			get
			{
				return typeof(int);
			}
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x0016FFFC File Offset: 0x0016EFFC
		protected override Array CreateArray()
		{
			return new int[base.Count];
		}

		// Token: 0x06006494 RID: 25748 RVA: 0x00170009 File Offset: 0x0016F009
		protected override object GetItemFromAddress(IntPtr addr)
		{
			return addr.ToInt32();
		}
	}
}
