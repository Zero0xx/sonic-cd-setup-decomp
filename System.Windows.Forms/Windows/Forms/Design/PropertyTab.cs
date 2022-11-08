using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200077F RID: 1919
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class PropertyTab : IExtenderProvider
	{
		// Token: 0x060064E6 RID: 25830 RVA: 0x001714BC File Offset: 0x001704BC
		~PropertyTab()
		{
			this.Dispose(false);
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x060064E7 RID: 25831 RVA: 0x001714EC File Offset: 0x001704EC
		public virtual Bitmap Bitmap
		{
			get
			{
				if (!this.checkedBmp && this.bitmap == null)
				{
					string resource = base.GetType().Name + ".bmp";
					try
					{
						this.bitmap = new Bitmap(base.GetType(), resource);
					}
					catch (Exception)
					{
					}
					this.checkedBmp = true;
				}
				return this.bitmap;
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x060064E8 RID: 25832 RVA: 0x00171554 File Offset: 0x00170554
		// (set) Token: 0x060064E9 RID: 25833 RVA: 0x0017155C File Offset: 0x0017055C
		public virtual object[] Components
		{
			get
			{
				return this.components;
			}
			set
			{
				this.components = value;
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x060064EA RID: 25834
		public abstract string TabName { get; }

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x060064EB RID: 25835 RVA: 0x00171565 File Offset: 0x00170565
		public virtual string HelpKeyword
		{
			get
			{
				return this.TabName;
			}
		}

		// Token: 0x060064EC RID: 25836 RVA: 0x0017156D File Offset: 0x0017056D
		public virtual bool CanExtend(object extendee)
		{
			return true;
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x00171570 File Offset: 0x00170570
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x0017157F File Offset: 0x0017057F
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.bitmap != null)
			{
				this.bitmap.Dispose();
				this.bitmap = null;
			}
		}

		// Token: 0x060064EF RID: 25839 RVA: 0x0017159E File Offset: 0x0017059E
		public virtual PropertyDescriptor GetDefaultProperty(object component)
		{
			return TypeDescriptor.GetDefaultProperty(component);
		}

		// Token: 0x060064F0 RID: 25840 RVA: 0x001715A6 File Offset: 0x001705A6
		public virtual PropertyDescriptorCollection GetProperties(object component)
		{
			return this.GetProperties(component, null);
		}

		// Token: 0x060064F1 RID: 25841
		public abstract PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes);

		// Token: 0x060064F2 RID: 25842 RVA: 0x001715B0 File Offset: 0x001705B0
		public virtual PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
		{
			return this.GetProperties(component, attributes);
		}

		// Token: 0x04003C03 RID: 15363
		private object[] components;

		// Token: 0x04003C04 RID: 15364
		private Bitmap bitmap;

		// Token: 0x04003C05 RID: 15365
		private bool checkedBmp;
	}
}
