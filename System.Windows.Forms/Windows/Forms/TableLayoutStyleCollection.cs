using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000655 RID: 1621
	[Editor("System.Windows.Forms.Design.StyleCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public abstract class TableLayoutStyleCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x0600551B RID: 21787 RVA: 0x00136700 File Offset: 0x00135700
		internal TableLayoutStyleCollection(IArrangedElement owner)
		{
			this._owner = owner;
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x0600551C RID: 21788 RVA: 0x0013671A File Offset: 0x0013571A
		internal IArrangedElement Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x0600551D RID: 21789 RVA: 0x00136722 File Offset: 0x00135722
		internal virtual string PropertyName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x00136728 File Offset: 0x00135728
		int IList.Add(object style)
		{
			this.EnsureNotOwned((TableLayoutStyle)style);
			((TableLayoutStyle)style).Owner = this.Owner;
			int result = this._innerList.Add(style);
			this.PerformLayoutIfOwned();
			return result;
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x00136766 File Offset: 0x00135766
		public int Add(TableLayoutStyle style)
		{
			return ((IList)this).Add(style);
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x0013676F File Offset: 0x0013576F
		void IList.Insert(int index, object style)
		{
			this.EnsureNotOwned((TableLayoutStyle)style);
			((TableLayoutStyle)style).Owner = this.Owner;
			this._innerList.Insert(index, style);
			this.PerformLayoutIfOwned();
		}

		// Token: 0x170011A0 RID: 4512
		object IList.this[int index]
		{
			get
			{
				return this._innerList[index];
			}
			set
			{
				TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)value;
				this.EnsureNotOwned(tableLayoutStyle);
				tableLayoutStyle.Owner = this.Owner;
				this._innerList[index] = tableLayoutStyle;
				this.PerformLayoutIfOwned();
			}
		}

		// Token: 0x170011A1 RID: 4513
		public TableLayoutStyle this[int index]
		{
			get
			{
				return (TableLayoutStyle)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		// Token: 0x06005525 RID: 21797 RVA: 0x00136802 File Offset: 0x00135802
		void IList.Remove(object style)
		{
			((TableLayoutStyle)style).Owner = null;
			this._innerList.Remove(style);
			this.PerformLayoutIfOwned();
		}

		// Token: 0x06005526 RID: 21798 RVA: 0x00136824 File Offset: 0x00135824
		public void Clear()
		{
			foreach (object obj in this._innerList)
			{
				TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)obj;
				tableLayoutStyle.Owner = null;
			}
			this._innerList.Clear();
			this.PerformLayoutIfOwned();
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x00136890 File Offset: 0x00135890
		public void RemoveAt(int index)
		{
			TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)this._innerList[index];
			tableLayoutStyle.Owner = null;
			this._innerList.RemoveAt(index);
			this.PerformLayoutIfOwned();
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x001368C8 File Offset: 0x001358C8
		bool IList.Contains(object style)
		{
			return this._innerList.Contains(style);
		}

		// Token: 0x06005529 RID: 21801 RVA: 0x001368D6 File Offset: 0x001358D6
		int IList.IndexOf(object style)
		{
			return this._innerList.IndexOf(style);
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x0600552A RID: 21802 RVA: 0x001368E4 File Offset: 0x001358E4
		bool IList.IsFixedSize
		{
			get
			{
				return this._innerList.IsFixedSize;
			}
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x0600552B RID: 21803 RVA: 0x001368F1 File Offset: 0x001358F1
		bool IList.IsReadOnly
		{
			get
			{
				return this._innerList.IsReadOnly;
			}
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x001368FE File Offset: 0x001358FE
		void ICollection.CopyTo(Array array, int startIndex)
		{
			this._innerList.CopyTo(array, startIndex);
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x0600552D RID: 21805 RVA: 0x0013690D File Offset: 0x0013590D
		public int Count
		{
			get
			{
				return this._innerList.Count;
			}
		}

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x0600552E RID: 21806 RVA: 0x0013691A File Offset: 0x0013591A
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._innerList.IsSynchronized;
			}
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x0600552F RID: 21807 RVA: 0x00136927 File Offset: 0x00135927
		object ICollection.SyncRoot
		{
			get
			{
				return this._innerList.SyncRoot;
			}
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x00136934 File Offset: 0x00135934
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._innerList.GetEnumerator();
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x00136944 File Offset: 0x00135944
		private void EnsureNotOwned(TableLayoutStyle style)
		{
			if (style.Owner != null)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					style.GetType().Name
				}), "style");
			}
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x00136984 File Offset: 0x00135984
		internal void EnsureOwnership(IArrangedElement owner)
		{
			this._owner = owner;
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Owner = owner;
			}
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x001369B6 File Offset: 0x001359B6
		private void PerformLayoutIfOwned()
		{
			if (this.Owner != null)
			{
				LayoutTransaction.DoLayout(this.Owner, this.Owner, this.PropertyName);
			}
		}

		// Token: 0x040036FF RID: 14079
		private IArrangedElement _owner;

		// Token: 0x04003700 RID: 14080
		private ArrayList _innerList = new ArrayList();
	}
}
