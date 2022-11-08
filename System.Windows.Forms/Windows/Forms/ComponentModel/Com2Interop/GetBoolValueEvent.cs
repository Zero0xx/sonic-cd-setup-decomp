using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000768 RID: 1896
	internal class GetBoolValueEvent : EventArgs
	{
		// Token: 0x0600642D RID: 25645 RVA: 0x0016DEDC File Offset: 0x0016CEDC
		public GetBoolValueEvent(bool defValue)
		{
			this.value = defValue;
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x0600642E RID: 25646 RVA: 0x0016DEEB File Offset: 0x0016CEEB
		// (set) Token: 0x0600642F RID: 25647 RVA: 0x0016DEF3 File Offset: 0x0016CEF3
		public bool Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04003BA4 RID: 15268
		private bool value;
	}
}
