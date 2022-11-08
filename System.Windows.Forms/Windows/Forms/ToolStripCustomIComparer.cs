using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x020006B2 RID: 1714
	internal class ToolStripCustomIComparer : IComparer
	{
		// Token: 0x060059F7 RID: 23031 RVA: 0x001470C8 File Offset: 0x001460C8
		int IComparer.Compare(object x, object y)
		{
			if (x.GetType() == y.GetType())
			{
				return 0;
			}
			if (x.GetType().IsAssignableFrom(y.GetType()))
			{
				return 1;
			}
			if (y.GetType().IsAssignableFrom(x.GetType()))
			{
				return -1;
			}
			return 0;
		}
	}
}
