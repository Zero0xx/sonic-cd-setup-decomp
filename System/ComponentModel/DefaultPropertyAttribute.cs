using System;

namespace System.ComponentModel
{
	// Token: 0x020000CD RID: 205
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefaultPropertyAttribute : Attribute
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0001A317 File Offset: 0x00019317
		public DefaultPropertyAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0001A326 File Offset: 0x00019326
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001A330 File Offset: 0x00019330
		public override bool Equals(object obj)
		{
			DefaultPropertyAttribute defaultPropertyAttribute = obj as DefaultPropertyAttribute;
			return defaultPropertyAttribute != null && defaultPropertyAttribute.Name == this.name;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001A35A File Offset: 0x0001935A
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400093B RID: 2363
		private readonly string name;

		// Token: 0x0400093C RID: 2364
		public static readonly DefaultPropertyAttribute Default = new DefaultPropertyAttribute(null);
	}
}
