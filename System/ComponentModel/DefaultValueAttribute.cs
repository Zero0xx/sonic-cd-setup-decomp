using System;

namespace System.ComponentModel
{
	// Token: 0x020000CE RID: 206
	[AttributeUsage(AttributeTargets.All)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0001A370 File Offset: 0x00019370
		public DefaultValueAttribute(Type type, string value)
		{
			try
			{
				this.value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
			}
			catch
			{
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001A3AC File Offset: 0x000193AC
		public DefaultValueAttribute(char value)
		{
			this.value = value;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001A3C0 File Offset: 0x000193C0
		public DefaultValueAttribute(byte value)
		{
			this.value = value;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001A3D4 File Offset: 0x000193D4
		public DefaultValueAttribute(short value)
		{
			this.value = value;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001A3E8 File Offset: 0x000193E8
		public DefaultValueAttribute(int value)
		{
			this.value = value;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001A3FC File Offset: 0x000193FC
		public DefaultValueAttribute(long value)
		{
			this.value = value;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001A410 File Offset: 0x00019410
		public DefaultValueAttribute(float value)
		{
			this.value = value;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001A424 File Offset: 0x00019424
		public DefaultValueAttribute(double value)
		{
			this.value = value;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001A438 File Offset: 0x00019438
		public DefaultValueAttribute(bool value)
		{
			this.value = value;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001A44C File Offset: 0x0001944C
		public DefaultValueAttribute(string value)
		{
			this.value = value;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001A45B File Offset: 0x0001945B
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001A46A File Offset: 0x0001946A
		public virtual object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001A474 File Offset: 0x00019474
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DefaultValueAttribute defaultValueAttribute = obj as DefaultValueAttribute;
			if (defaultValueAttribute == null)
			{
				return false;
			}
			if (this.Value != null)
			{
				return this.Value.Equals(defaultValueAttribute.Value);
			}
			return defaultValueAttribute.Value == null;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001A4B6 File Offset: 0x000194B6
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001A4BE File Offset: 0x000194BE
		protected void SetValue(object value)
		{
			this.value = value;
		}

		// Token: 0x0400093D RID: 2365
		private object value;
	}
}
