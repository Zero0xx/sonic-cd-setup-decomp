using System;
using System.Collections;

namespace System.ComponentModel
{
	// Token: 0x020000F1 RID: 241
	[TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Editor("System.Windows.Forms.Design.DataSourceListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[MergableProperty(false)]
	public interface IListSource
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007F3 RID: 2035
		bool ContainsListCollection { get; }

		// Token: 0x060007F4 RID: 2036
		IList GetList();
	}
}
