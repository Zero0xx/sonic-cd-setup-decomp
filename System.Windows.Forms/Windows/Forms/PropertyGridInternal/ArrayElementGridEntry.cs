using System;
using System.Globalization;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007AB RID: 1963
	internal class ArrayElementGridEntry : GridEntry
	{
		// Token: 0x0600681A RID: 26650 RVA: 0x0017D3C1 File Offset: 0x0017C3C1
		public ArrayElementGridEntry(PropertyGrid ownerGrid, GridEntry peParent, int index) : base(ownerGrid, peParent)
		{
			this.index = index;
			this.SetFlag(256, (peParent.Flags & 256) != 0 || peParent.ForceReadOnly);
		}

		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x0600681B RID: 26651 RVA: 0x0017D3F4 File Offset: 0x0017C3F4
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.ArrayValue;
			}
		}

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x0600681C RID: 26652 RVA: 0x0017D3F7 File Offset: 0x0017C3F7
		public override bool IsValueEditable
		{
			get
			{
				return this.ParentGridEntry.IsValueEditable;
			}
		}

		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x0600681D RID: 26653 RVA: 0x0017D404 File Offset: 0x0017C404
		public override string PropertyLabel
		{
			get
			{
				return "[" + this.index.ToString(CultureInfo.CurrentCulture) + "]";
			}
		}

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x0600681E RID: 26654 RVA: 0x0017D425 File Offset: 0x0017C425
		public override Type PropertyType
		{
			get
			{
				return this.parentPE.PropertyType.GetElementType();
			}
		}

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x0600681F RID: 26655 RVA: 0x0017D438 File Offset: 0x0017C438
		// (set) Token: 0x06006820 RID: 26656 RVA: 0x0017D460 File Offset: 0x0017C460
		public override object PropertyValue
		{
			get
			{
				object valueOwner = this.GetValueOwner();
				return ((Array)valueOwner).GetValue(this.index);
			}
			set
			{
				object valueOwner = this.GetValueOwner();
				((Array)valueOwner).SetValue(value, this.index);
			}
		}

		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x06006821 RID: 26657 RVA: 0x0017D486 File Offset: 0x0017C486
		public override bool ShouldRenderReadOnly
		{
			get
			{
				return this.ParentGridEntry.ShouldRenderReadOnly;
			}
		}

		// Token: 0x04003D5C RID: 15708
		protected int index;
	}
}
