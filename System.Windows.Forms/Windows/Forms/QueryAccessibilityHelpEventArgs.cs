using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020005D8 RID: 1496
	[ComVisible(true)]
	public class QueryAccessibilityHelpEventArgs : EventArgs
	{
		// Token: 0x06004E43 RID: 20035 RVA: 0x00121027 File Offset: 0x00120027
		public QueryAccessibilityHelpEventArgs()
		{
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x0012102F File Offset: 0x0012002F
		public QueryAccessibilityHelpEventArgs(string helpNamespace, string helpString, string helpKeyword)
		{
			this.helpNamespace = helpNamespace;
			this.helpString = helpString;
			this.helpKeyword = helpKeyword;
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06004E45 RID: 20037 RVA: 0x0012104C File Offset: 0x0012004C
		// (set) Token: 0x06004E46 RID: 20038 RVA: 0x00121054 File Offset: 0x00120054
		public string HelpNamespace
		{
			get
			{
				return this.helpNamespace;
			}
			set
			{
				this.helpNamespace = value;
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06004E47 RID: 20039 RVA: 0x0012105D File Offset: 0x0012005D
		// (set) Token: 0x06004E48 RID: 20040 RVA: 0x00121065 File Offset: 0x00120065
		public string HelpString
		{
			get
			{
				return this.helpString;
			}
			set
			{
				this.helpString = value;
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x0012106E File Offset: 0x0012006E
		// (set) Token: 0x06004E4A RID: 20042 RVA: 0x00121076 File Offset: 0x00120076
		public string HelpKeyword
		{
			get
			{
				return this.helpKeyword;
			}
			set
			{
				this.helpKeyword = value;
			}
		}

		// Token: 0x040032B4 RID: 12980
		private string helpNamespace;

		// Token: 0x040032B5 RID: 12981
		private string helpString;

		// Token: 0x040032B6 RID: 12982
		private string helpKeyword;
	}
}
