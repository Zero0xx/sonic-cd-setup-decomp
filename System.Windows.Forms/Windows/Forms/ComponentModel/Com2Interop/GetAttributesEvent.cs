using System;
using System.Collections;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000763 RID: 1891
	internal class GetAttributesEvent : EventArgs
	{
		// Token: 0x0600641B RID: 25627 RVA: 0x0016DE83 File Offset: 0x0016CE83
		public GetAttributesEvent(ArrayList attrList)
		{
			this.attrList = attrList;
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x0016DE92 File Offset: 0x0016CE92
		public void Add(Attribute attribute)
		{
			this.attrList.Add(attribute);
		}

		// Token: 0x04003BA2 RID: 15266
		private ArrayList attrList;
	}
}
