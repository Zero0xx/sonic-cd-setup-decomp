using System;

namespace System.ComponentModel
{
	// Token: 0x02000111 RID: 273
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ListBindableAttribute : Attribute
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x0001CC4D File Offset: 0x0001BC4D
		public ListBindableAttribute(bool listBindable)
		{
			this.listBindable = listBindable;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001CC5C File Offset: 0x0001BC5C
		public ListBindableAttribute(BindableSupport flags)
		{
			this.listBindable = (flags != BindableSupport.No);
			this.isDefault = (flags == BindableSupport.Default);
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0001CC7B File Offset: 0x0001BC7B
		public bool ListBindable
		{
			get
			{
				return this.listBindable;
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001CC84 File Offset: 0x0001BC84
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ListBindableAttribute listBindableAttribute = obj as ListBindableAttribute;
			return listBindableAttribute != null && listBindableAttribute.ListBindable == this.listBindable;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001CCB1 File Offset: 0x0001BCB1
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001CCB9 File Offset: 0x0001BCB9
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ListBindableAttribute.Default) || this.isDefault;
		}

		// Token: 0x0400099E RID: 2462
		public static readonly ListBindableAttribute Yes = new ListBindableAttribute(true);

		// Token: 0x0400099F RID: 2463
		public static readonly ListBindableAttribute No = new ListBindableAttribute(false);

		// Token: 0x040009A0 RID: 2464
		public static readonly ListBindableAttribute Default = ListBindableAttribute.Yes;

		// Token: 0x040009A1 RID: 2465
		private bool listBindable;

		// Token: 0x040009A2 RID: 2466
		private bool isDefault;
	}
}
