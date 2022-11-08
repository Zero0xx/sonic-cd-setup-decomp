using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200073B RID: 1851
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class WinCategoryAttribute : CategoryAttribute
	{
		// Token: 0x0600629B RID: 25243 RVA: 0x001668EC File Offset: 0x001658EC
		public WinCategoryAttribute(string category) : base(category)
		{
		}

		// Token: 0x0600629C RID: 25244 RVA: 0x001668F8 File Offset: 0x001658F8
		protected override string GetLocalizedString(string value)
		{
			string text = base.GetLocalizedString(value);
			if (text == null)
			{
				text = (string)SR.GetObject("WinFormsCategory" + value);
			}
			return text;
		}
	}
}
