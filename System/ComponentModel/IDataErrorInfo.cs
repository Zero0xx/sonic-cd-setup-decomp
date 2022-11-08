using System;

namespace System.ComponentModel
{
	// Token: 0x020000ED RID: 237
	public interface IDataErrorInfo
	{
		// Token: 0x1700019E RID: 414
		string this[string columnName]
		{
			get;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007EC RID: 2028
		string Error { get; }
	}
}
