using System;

namespace System.Windows.Forms
{
	// Token: 0x02000718 RID: 1816
	public class TypeValidationEventArgs : EventArgs
	{
		// Token: 0x0600608F RID: 24719 RVA: 0x001622D9 File Offset: 0x001612D9
		public TypeValidationEventArgs(Type validatingType, bool isValidInput, object returnValue, string message)
		{
			this.validatingType = validatingType;
			this.isValidInput = isValidInput;
			this.returnValue = returnValue;
			this.message = message;
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x06006090 RID: 24720 RVA: 0x001622FE File Offset: 0x001612FE
		// (set) Token: 0x06006091 RID: 24721 RVA: 0x00162306 File Offset: 0x00161306
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x06006092 RID: 24722 RVA: 0x0016230F File Offset: 0x0016130F
		public bool IsValidInput
		{
			get
			{
				return this.isValidInput;
			}
		}

		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x06006093 RID: 24723 RVA: 0x00162317 File Offset: 0x00161317
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x06006094 RID: 24724 RVA: 0x0016231F File Offset: 0x0016131F
		public object ReturnValue
		{
			get
			{
				return this.returnValue;
			}
		}

		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x06006095 RID: 24725 RVA: 0x00162327 File Offset: 0x00161327
		public Type ValidatingType
		{
			get
			{
				return this.validatingType;
			}
		}

		// Token: 0x04003A8C RID: 14988
		private Type validatingType;

		// Token: 0x04003A8D RID: 14989
		private string message;

		// Token: 0x04003A8E RID: 14990
		private bool isValidInput;

		// Token: 0x04003A8F RID: 14991
		private object returnValue;

		// Token: 0x04003A90 RID: 14992
		private bool cancel;
	}
}
