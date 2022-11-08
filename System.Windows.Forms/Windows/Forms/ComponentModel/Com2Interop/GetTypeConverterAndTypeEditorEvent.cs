using System;
using System.ComponentModel;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200076C RID: 1900
	internal class GetTypeConverterAndTypeEditorEvent : EventArgs
	{
		// Token: 0x06006439 RID: 25657 RVA: 0x0016DF0C File Offset: 0x0016CF0C
		public GetTypeConverterAndTypeEditorEvent(TypeConverter typeConverter, object typeEditor)
		{
			this.typeEditor = typeEditor;
			this.typeConverter = typeConverter;
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x0600643A RID: 25658 RVA: 0x0016DF22 File Offset: 0x0016CF22
		// (set) Token: 0x0600643B RID: 25659 RVA: 0x0016DF2A File Offset: 0x0016CF2A
		public TypeConverter TypeConverter
		{
			get
			{
				return this.typeConverter;
			}
			set
			{
				this.typeConverter = value;
			}
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x0600643C RID: 25660 RVA: 0x0016DF33 File Offset: 0x0016CF33
		// (set) Token: 0x0600643D RID: 25661 RVA: 0x0016DF3B File Offset: 0x0016CF3B
		public object TypeEditor
		{
			get
			{
				return this.typeEditor;
			}
			set
			{
				this.typeEditor = value;
			}
		}

		// Token: 0x04003BA6 RID: 15270
		private TypeConverter typeConverter;

		// Token: 0x04003BA7 RID: 15271
		private object typeEditor;
	}
}
