using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000784 RID: 1924
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ToolStripItemDesignerAvailabilityAttribute : Attribute
	{
		// Token: 0x0600650D RID: 25869 RVA: 0x00171846 File Offset: 0x00170846
		public ToolStripItemDesignerAvailabilityAttribute()
		{
			this.visibility = ToolStripItemDesignerAvailability.None;
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x00171855 File Offset: 0x00170855
		public ToolStripItemDesignerAvailabilityAttribute(ToolStripItemDesignerAvailability visibility)
		{
			this.visibility = visibility;
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x0600650F RID: 25871 RVA: 0x00171864 File Offset: 0x00170864
		public ToolStripItemDesignerAvailability ItemAdditionVisibility
		{
			get
			{
				return this.visibility;
			}
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x0017186C File Offset: 0x0017086C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ToolStripItemDesignerAvailabilityAttribute toolStripItemDesignerAvailabilityAttribute = obj as ToolStripItemDesignerAvailabilityAttribute;
			return toolStripItemDesignerAvailabilityAttribute != null && toolStripItemDesignerAvailabilityAttribute.ItemAdditionVisibility == this.visibility;
		}

		// Token: 0x06006511 RID: 25873 RVA: 0x00171899 File Offset: 0x00170899
		public override int GetHashCode()
		{
			return this.visibility.GetHashCode();
		}

		// Token: 0x06006512 RID: 25874 RVA: 0x001718AB File Offset: 0x001708AB
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ToolStripItemDesignerAvailabilityAttribute.Default);
		}

		// Token: 0x04003C10 RID: 15376
		private ToolStripItemDesignerAvailability visibility;

		// Token: 0x04003C11 RID: 15377
		public static readonly ToolStripItemDesignerAvailabilityAttribute Default = new ToolStripItemDesignerAvailabilityAttribute();
	}
}
