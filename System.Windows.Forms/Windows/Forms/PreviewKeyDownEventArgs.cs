using System;

namespace System.Windows.Forms
{
	// Token: 0x020005BA RID: 1466
	public class PreviewKeyDownEventArgs : EventArgs
	{
		// Token: 0x06004BEB RID: 19435 RVA: 0x0011257E File Offset: 0x0011157E
		public PreviewKeyDownEventArgs(Keys keyData)
		{
			this._keyData = keyData;
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x06004BEC RID: 19436 RVA: 0x0011258D File Offset: 0x0011158D
		public bool Alt
		{
			get
			{
				return (this._keyData & Keys.Alt) == Keys.Alt;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x001125A2 File Offset: 0x001115A2
		public bool Control
		{
			get
			{
				return (this._keyData & Keys.Control) == Keys.Control;
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x001125B8 File Offset: 0x001115B8
		public Keys KeyCode
		{
			get
			{
				Keys keys = this._keyData & Keys.KeyCode;
				if (!Enum.IsDefined(typeof(Keys), (int)keys))
				{
					return Keys.None;
				}
				return keys;
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06004BEF RID: 19439 RVA: 0x001125EC File Offset: 0x001115EC
		public int KeyValue
		{
			get
			{
				return (int)(this._keyData & Keys.KeyCode);
			}
		}

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x001125FA File Offset: 0x001115FA
		public Keys KeyData
		{
			get
			{
				return this._keyData;
			}
		}

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x00112602 File Offset: 0x00111602
		public Keys Modifiers
		{
			get
			{
				return this._keyData & Keys.Modifiers;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x00112610 File Offset: 0x00111610
		public bool Shift
		{
			get
			{
				return (this._keyData & Keys.Shift) == Keys.Shift;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x00112625 File Offset: 0x00111625
		// (set) Token: 0x06004BF4 RID: 19444 RVA: 0x0011262D File Offset: 0x0011162D
		public bool IsInputKey
		{
			get
			{
				return this._isInputKey;
			}
			set
			{
				this._isInputKey = value;
			}
		}

		// Token: 0x04003144 RID: 12612
		private readonly Keys _keyData;

		// Token: 0x04003145 RID: 12613
		private bool _isInputKey;
	}
}
