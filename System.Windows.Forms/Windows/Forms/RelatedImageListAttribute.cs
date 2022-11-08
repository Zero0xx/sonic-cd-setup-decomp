using System;

namespace System.Windows.Forms
{
	// Token: 0x020005E2 RID: 1506
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class RelatedImageListAttribute : Attribute
	{
		// Token: 0x06004EAA RID: 20138 RVA: 0x00121E8C File Offset: 0x00120E8C
		public RelatedImageListAttribute(string relatedImageList)
		{
			this.relatedImageList = relatedImageList;
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06004EAB RID: 20139 RVA: 0x00121E9B File Offset: 0x00120E9B
		public string RelatedImageList
		{
			get
			{
				return this.relatedImageList;
			}
		}

		// Token: 0x040032CA RID: 13002
		private string relatedImageList;
	}
}
