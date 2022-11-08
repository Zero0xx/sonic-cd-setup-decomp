using System;

namespace System.ComponentModel
{
	// Token: 0x02000120 RID: 288
	[AttributeUsage(AttributeTargets.All)]
	public sealed class MergablePropertyAttribute : Attribute
	{
		// Token: 0x06000933 RID: 2355 RVA: 0x0001F030 File Offset: 0x0001E030
		public MergablePropertyAttribute(bool allowMerge)
		{
			this.allowMerge = allowMerge;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0001F03F File Offset: 0x0001E03F
		public bool AllowMerge
		{
			get
			{
				return this.allowMerge;
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001F048 File Offset: 0x0001E048
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			MergablePropertyAttribute mergablePropertyAttribute = obj as MergablePropertyAttribute;
			return mergablePropertyAttribute != null && mergablePropertyAttribute.AllowMerge == this.allowMerge;
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001F075 File Offset: 0x0001E075
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001F07D File Offset: 0x0001E07D
		public override bool IsDefaultAttribute()
		{
			return this.Equals(MergablePropertyAttribute.Default);
		}

		// Token: 0x040009FD RID: 2557
		public static readonly MergablePropertyAttribute Yes = new MergablePropertyAttribute(true);

		// Token: 0x040009FE RID: 2558
		public static readonly MergablePropertyAttribute No = new MergablePropertyAttribute(false);

		// Token: 0x040009FF RID: 2559
		public static readonly MergablePropertyAttribute Default = MergablePropertyAttribute.Yes;

		// Token: 0x04000A00 RID: 2560
		private bool allowMerge;
	}
}
