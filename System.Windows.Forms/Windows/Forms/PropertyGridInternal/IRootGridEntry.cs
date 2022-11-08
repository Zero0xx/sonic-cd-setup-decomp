using System;
using System.ComponentModel;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007BB RID: 1979
	public interface IRootGridEntry
	{
		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x060068B7 RID: 26807
		// (set) Token: 0x060068B8 RID: 26808
		AttributeCollection BrowsableAttributes { get; set; }

		// Token: 0x060068B9 RID: 26809
		void ResetBrowsableAttributes();

		// Token: 0x060068BA RID: 26810
		void ShowCategories(bool showCategories);
	}
}
