using System;

namespace System.ComponentModel
{
	// Token: 0x020000D8 RID: 216
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
	public class DisplayNameAttribute : Attribute
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x0001AAA5 File Offset: 0x00019AA5
		public DisplayNameAttribute() : this(string.Empty)
		{
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001AAB2 File Offset: 0x00019AB2
		public DisplayNameAttribute(string displayName)
		{
			this._displayName = displayName;
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001AAC1 File Offset: 0x00019AC1
		public virtual string DisplayName
		{
			get
			{
				return this.DisplayNameValue;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001AAC9 File Offset: 0x00019AC9
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x0001AAD1 File Offset: 0x00019AD1
		protected string DisplayNameValue
		{
			get
			{
				return this._displayName;
			}
			set
			{
				this._displayName = value;
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001AADC File Offset: 0x00019ADC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DisplayNameAttribute displayNameAttribute = obj as DisplayNameAttribute;
			return displayNameAttribute != null && displayNameAttribute.DisplayName == this.DisplayName;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001AB0C File Offset: 0x00019B0C
		public override int GetHashCode()
		{
			return this.DisplayName.GetHashCode();
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001AB19 File Offset: 0x00019B19
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DisplayNameAttribute.Default);
		}

		// Token: 0x0400095B RID: 2395
		public static readonly DisplayNameAttribute Default = new DisplayNameAttribute();

		// Token: 0x0400095C RID: 2396
		private string _displayName;
	}
}
