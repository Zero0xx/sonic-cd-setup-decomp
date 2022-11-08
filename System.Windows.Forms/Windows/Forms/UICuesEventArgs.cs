using System;

namespace System.Windows.Forms
{
	// Token: 0x0200071B RID: 1819
	public class UICuesEventArgs : EventArgs
	{
		// Token: 0x0600609A RID: 24730 RVA: 0x0016232F File Offset: 0x0016132F
		public UICuesEventArgs(UICues uicues)
		{
			this.uicues = uicues;
		}

		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x0600609B RID: 24731 RVA: 0x0016233E File Offset: 0x0016133E
		public bool ShowFocus
		{
			get
			{
				return (this.uicues & UICues.ShowFocus) != UICues.None;
			}
		}

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x0600609C RID: 24732 RVA: 0x0016234E File Offset: 0x0016134E
		public bool ShowKeyboard
		{
			get
			{
				return (this.uicues & UICues.ShowKeyboard) != UICues.None;
			}
		}

		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x0600609D RID: 24733 RVA: 0x0016235E File Offset: 0x0016135E
		public bool ChangeFocus
		{
			get
			{
				return (this.uicues & UICues.ChangeFocus) != UICues.None;
			}
		}

		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x0600609E RID: 24734 RVA: 0x0016236E File Offset: 0x0016136E
		public bool ChangeKeyboard
		{
			get
			{
				return (this.uicues & UICues.ChangeKeyboard) != UICues.None;
			}
		}

		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x0600609F RID: 24735 RVA: 0x0016237E File Offset: 0x0016137E
		public UICues Changed
		{
			get
			{
				return this.uicues & UICues.Changed;
			}
		}

		// Token: 0x04003A99 RID: 15001
		private readonly UICues uicues;
	}
}
