using System;

namespace System.ComponentModel
{
	// Token: 0x020000CC RID: 204
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefaultEventAttribute : Attribute
	{
		// Token: 0x060006F3 RID: 1779 RVA: 0x0001A2BE File Offset: 0x000192BE
		public DefaultEventAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001A2CD File Offset: 0x000192CD
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001A2D8 File Offset: 0x000192D8
		public override bool Equals(object obj)
		{
			DefaultEventAttribute defaultEventAttribute = obj as DefaultEventAttribute;
			return defaultEventAttribute != null && defaultEventAttribute.Name == this.name;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001A302 File Offset: 0x00019302
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000939 RID: 2361
		private readonly string name;

		// Token: 0x0400093A RID: 2362
		public static readonly DefaultEventAttribute Default = new DefaultEventAttribute(null);
	}
}
