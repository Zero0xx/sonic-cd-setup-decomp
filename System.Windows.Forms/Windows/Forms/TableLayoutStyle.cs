using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000651 RID: 1617
	[TypeConverter(typeof(TableLayoutSettings.StyleConverter))]
	public abstract class TableLayoutStyle
	{
		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x060054FF RID: 21759 RVA: 0x00135EE1 File Offset: 0x00134EE1
		// (set) Token: 0x06005500 RID: 21760 RVA: 0x00135EEC File Offset: 0x00134EEC
		[DefaultValue(SizeType.AutoSize)]
		public SizeType SizeType
		{
			get
			{
				return this._sizeType;
			}
			set
			{
				if (this._sizeType != value)
				{
					this._sizeType = value;
					if (this.Owner != null)
					{
						LayoutTransaction.DoLayout(this.Owner, this.Owner, PropertyNames.Style);
						Control control = this.Owner as Control;
						if (control != null)
						{
							control.Invalidate();
						}
					}
				}
			}
		}

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x06005501 RID: 21761 RVA: 0x00135F3C File Offset: 0x00134F3C
		// (set) Token: 0x06005502 RID: 21762 RVA: 0x00135F44 File Offset: 0x00134F44
		internal float Size
		{
			get
			{
				return this._size;
			}
			set
			{
				if (value < 0f)
				{
					throw new ArgumentOutOfRangeException("Size", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"Size",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this._size != value)
				{
					this._size = value;
					if (this.Owner != null)
					{
						LayoutTransaction.DoLayout(this.Owner, this.Owner, PropertyNames.Style);
						Control control = this.Owner as Control;
						if (control != null)
						{
							control.Invalidate();
						}
					}
				}
			}
		}

		// Token: 0x06005503 RID: 21763 RVA: 0x00135FE1 File Offset: 0x00134FE1
		private bool ShouldSerializeSize()
		{
			return this.SizeType != SizeType.AutoSize;
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x06005504 RID: 21764 RVA: 0x00135FEF File Offset: 0x00134FEF
		// (set) Token: 0x06005505 RID: 21765 RVA: 0x00135FF7 File Offset: 0x00134FF7
		internal IArrangedElement Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				this._owner = value;
			}
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x00136000 File Offset: 0x00135000
		internal void SetSize(float size)
		{
			this._size = size;
		}

		// Token: 0x040036FC RID: 14076
		private IArrangedElement _owner;

		// Token: 0x040036FD RID: 14077
		private SizeType _sizeType;

		// Token: 0x040036FE RID: 14078
		private float _size;
	}
}
