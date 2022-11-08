using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020005DA RID: 1498
	[ComVisible(true)]
	public class QueryContinueDragEventArgs : EventArgs
	{
		// Token: 0x06004E4F RID: 20047 RVA: 0x0012107F File Offset: 0x0012007F
		public QueryContinueDragEventArgs(int keyState, bool escapePressed, DragAction action)
		{
			this.keyState = keyState;
			this.escapePressed = escapePressed;
			this.action = action;
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06004E50 RID: 20048 RVA: 0x0012109C File Offset: 0x0012009C
		public int KeyState
		{
			get
			{
				return this.keyState;
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06004E51 RID: 20049 RVA: 0x001210A4 File Offset: 0x001200A4
		public bool EscapePressed
		{
			get
			{
				return this.escapePressed;
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06004E52 RID: 20050 RVA: 0x001210AC File Offset: 0x001200AC
		// (set) Token: 0x06004E53 RID: 20051 RVA: 0x001210B4 File Offset: 0x001200B4
		public DragAction Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}

		// Token: 0x040032B7 RID: 12983
		private readonly int keyState;

		// Token: 0x040032B8 RID: 12984
		private readonly bool escapePressed;

		// Token: 0x040032B9 RID: 12985
		private DragAction action;
	}
}
