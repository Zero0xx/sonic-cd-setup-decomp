using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000766 RID: 1894
	internal class GetNameItemEvent : EventArgs
	{
		// Token: 0x06006425 RID: 25637 RVA: 0x0016DEA1 File Offset: 0x0016CEA1
		public GetNameItemEvent(object defName)
		{
			this.nameItem = defName;
		}

		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x06006426 RID: 25638 RVA: 0x0016DEB0 File Offset: 0x0016CEB0
		// (set) Token: 0x06006427 RID: 25639 RVA: 0x0016DEB8 File Offset: 0x0016CEB8
		public object Name
		{
			get
			{
				return this.nameItem;
			}
			set
			{
				this.nameItem = value;
			}
		}

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x06006428 RID: 25640 RVA: 0x0016DEC1 File Offset: 0x0016CEC1
		public string NameString
		{
			get
			{
				if (this.nameItem != null)
				{
					return this.nameItem.ToString();
				}
				return "";
			}
		}

		// Token: 0x04003BA3 RID: 15267
		private object nameItem;
	}
}
