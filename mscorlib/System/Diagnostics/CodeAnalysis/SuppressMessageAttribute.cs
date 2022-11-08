using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x020002CA RID: 714
	[Conditional("CODE_ANALYSIS")]
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class SuppressMessageAttribute : Attribute
	{
		// Token: 0x06001BA7 RID: 7079 RVA: 0x0004837E File Offset: 0x0004737E
		public SuppressMessageAttribute(string category, string checkId)
		{
			this.category = category;
			this.checkId = checkId;
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x00048394 File Offset: 0x00047394
		public string Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x0004839C File Offset: 0x0004739C
		public string CheckId
		{
			get
			{
				return this.checkId;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x000483A4 File Offset: 0x000473A4
		// (set) Token: 0x06001BAB RID: 7083 RVA: 0x000483AC File Offset: 0x000473AC
		public string Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001BAC RID: 7084 RVA: 0x000483B5 File Offset: 0x000473B5
		// (set) Token: 0x06001BAD RID: 7085 RVA: 0x000483BD File Offset: 0x000473BD
		public string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001BAE RID: 7086 RVA: 0x000483C6 File Offset: 0x000473C6
		// (set) Token: 0x06001BAF RID: 7087 RVA: 0x000483CE File Offset: 0x000473CE
		public string MessageId
		{
			get
			{
				return this.messageId;
			}
			set
			{
				this.messageId = value;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x000483D7 File Offset: 0x000473D7
		// (set) Token: 0x06001BB1 RID: 7089 RVA: 0x000483DF File Offset: 0x000473DF
		public string Justification
		{
			get
			{
				return this.justification;
			}
			set
			{
				this.justification = value;
			}
		}

		// Token: 0x04000AA7 RID: 2727
		private string category;

		// Token: 0x04000AA8 RID: 2728
		private string justification;

		// Token: 0x04000AA9 RID: 2729
		private string checkId;

		// Token: 0x04000AAA RID: 2730
		private string scope;

		// Token: 0x04000AAB RID: 2731
		private string target;

		// Token: 0x04000AAC RID: 2732
		private string messageId;
	}
}
