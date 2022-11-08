using System;

namespace System.ComponentModel
{
	// Token: 0x020000A4 RID: 164
	[AttributeUsage(AttributeTargets.All)]
	public sealed class BindableAttribute : Attribute
	{
		// Token: 0x060005DE RID: 1502 RVA: 0x00017B81 File Offset: 0x00016B81
		public BindableAttribute(bool bindable) : this(bindable, BindingDirection.OneWay)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00017B8B File Offset: 0x00016B8B
		public BindableAttribute(bool bindable, BindingDirection direction)
		{
			this.bindable = bindable;
			this.direction = direction;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00017BA1 File Offset: 0x00016BA1
		public BindableAttribute(BindableSupport flags) : this(flags, BindingDirection.OneWay)
		{
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00017BAB File Offset: 0x00016BAB
		public BindableAttribute(BindableSupport flags, BindingDirection direction)
		{
			this.bindable = (flags != BindableSupport.No);
			this.isDefault = (flags == BindableSupport.Default);
			this.direction = direction;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00017BD1 File Offset: 0x00016BD1
		public bool Bindable
		{
			get
			{
				return this.bindable;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00017BD9 File Offset: 0x00016BD9
		public BindingDirection Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00017BE1 File Offset: 0x00016BE1
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is BindableAttribute && ((BindableAttribute)obj).Bindable == this.bindable);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00017C09 File Offset: 0x00016C09
		public override int GetHashCode()
		{
			return this.bindable.GetHashCode();
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00017C16 File Offset: 0x00016C16
		public override bool IsDefaultAttribute()
		{
			return this.Equals(BindableAttribute.Default) || this.isDefault;
		}

		// Token: 0x040008ED RID: 2285
		public static readonly BindableAttribute Yes = new BindableAttribute(true);

		// Token: 0x040008EE RID: 2286
		public static readonly BindableAttribute No = new BindableAttribute(false);

		// Token: 0x040008EF RID: 2287
		public static readonly BindableAttribute Default = BindableAttribute.No;

		// Token: 0x040008F0 RID: 2288
		private bool bindable;

		// Token: 0x040008F1 RID: 2289
		private bool isDefault;

		// Token: 0x040008F2 RID: 2290
		private BindingDirection direction;
	}
}
