using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200076A RID: 1898
	internal class GetRefreshStateEvent : GetBoolValueEvent
	{
		// Token: 0x06006434 RID: 25652 RVA: 0x0016DEFC File Offset: 0x0016CEFC
		public GetRefreshStateEvent(Com2ShouldRefreshTypes item, bool defValue) : base(defValue)
		{
			this.item = item;
		}

		// Token: 0x04003BA5 RID: 15269
		private Com2ShouldRefreshTypes item;
	}
}
