using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020005E1 RID: 1505
	internal class RelatedCurrencyManager : CurrencyManager
	{
		// Token: 0x06004E9F RID: 20127 RVA: 0x00121B8F File Offset: 0x00120B8F
		internal RelatedCurrencyManager(BindingManagerBase parentManager, string dataField) : base(null)
		{
			this.Bind(parentManager, dataField);
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x00121BA0 File Offset: 0x00120BA0
		internal void Bind(BindingManagerBase parentManager, string dataField)
		{
			this.UnwireParentManager(this.parentManager);
			this.parentManager = parentManager;
			this.dataField = dataField;
			this.fieldInfo = parentManager.GetItemProperties().Find(dataField, true);
			if (this.fieldInfo == null || !typeof(IList).IsAssignableFrom(this.fieldInfo.PropertyType))
			{
				throw new ArgumentException(SR.GetString("RelatedListManagerChild", new object[]
				{
					dataField
				}));
			}
			this.finalType = this.fieldInfo.PropertyType;
			this.WireParentManager(this.parentManager);
			this.ParentManager_CurrentItemChanged(parentManager, EventArgs.Empty);
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x00121C43 File Offset: 0x00120C43
		private void UnwireParentManager(BindingManagerBase bmb)
		{
			if (bmb != null)
			{
				bmb.CurrentItemChanged -= this.ParentManager_CurrentItemChanged;
				if (bmb is CurrencyManager)
				{
					(bmb as CurrencyManager).MetaDataChanged -= this.ParentManager_MetaDataChanged;
				}
			}
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x00121C79 File Offset: 0x00120C79
		private void WireParentManager(BindingManagerBase bmb)
		{
			if (bmb != null)
			{
				bmb.CurrentItemChanged += this.ParentManager_CurrentItemChanged;
				if (bmb is CurrencyManager)
				{
					(bmb as CurrencyManager).MetaDataChanged += this.ParentManager_MetaDataChanged;
				}
			}
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x00121CB0 File Offset: 0x00120CB0
		internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptor[] array;
			if (listAccessors != null && listAccessors.Length > 0)
			{
				array = new PropertyDescriptor[listAccessors.Length + 1];
				listAccessors.CopyTo(array, 1);
			}
			else
			{
				array = new PropertyDescriptor[1];
			}
			array[0] = this.fieldInfo;
			return this.parentManager.GetItemProperties(array);
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x00121CF7 File Offset: 0x00120CF7
		public override PropertyDescriptorCollection GetItemProperties()
		{
			return this.GetItemProperties(null);
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x00121D00 File Offset: 0x00120D00
		internal override string GetListName()
		{
			string listName = this.GetListName(new ArrayList());
			if (listName.Length > 0)
			{
				return listName;
			}
			return base.GetListName();
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x00121D2A File Offset: 0x00120D2A
		protected internal override string GetListName(ArrayList listAccessors)
		{
			listAccessors.Insert(0, this.fieldInfo);
			return this.parentManager.GetListName(listAccessors);
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x00121D45 File Offset: 0x00120D45
		private void ParentManager_MetaDataChanged(object sender, EventArgs e)
		{
			base.OnMetaDataChanged(e);
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x00121D50 File Offset: 0x00120D50
		private void ParentManager_CurrentItemChanged(object sender, EventArgs e)
		{
			if (RelatedCurrencyManager.IgnoreItemChangedTable.Contains(this.parentManager))
			{
				return;
			}
			int listposition = this.listposition;
			try
			{
				base.PullData();
			}
			catch (Exception e2)
			{
				base.OnDataError(e2);
			}
			if (this.parentManager is CurrencyManager)
			{
				CurrencyManager currencyManager = (CurrencyManager)this.parentManager;
				if (currencyManager.Count > 0)
				{
					this.SetDataSource(this.fieldInfo.GetValue(currencyManager.Current));
					this.listposition = ((this.Count > 0) ? 0 : -1);
					goto IL_DC;
				}
				currencyManager.AddNew();
				try
				{
					RelatedCurrencyManager.IgnoreItemChangedTable.Add(currencyManager);
					currencyManager.CancelCurrentEdit();
					goto IL_DC;
				}
				finally
				{
					if (RelatedCurrencyManager.IgnoreItemChangedTable.Contains(currencyManager))
					{
						RelatedCurrencyManager.IgnoreItemChangedTable.Remove(currencyManager);
					}
				}
			}
			this.SetDataSource(this.fieldInfo.GetValue(this.parentManager.Current));
			this.listposition = ((this.Count > 0) ? 0 : -1);
			IL_DC:
			if (listposition != this.listposition)
			{
				this.OnPositionChanged(EventArgs.Empty);
			}
			this.OnCurrentChanged(EventArgs.Empty);
			this.OnCurrentItemChanged(EventArgs.Empty);
		}

		// Token: 0x040032C6 RID: 12998
		private BindingManagerBase parentManager;

		// Token: 0x040032C7 RID: 12999
		private string dataField;

		// Token: 0x040032C8 RID: 13000
		private PropertyDescriptor fieldInfo;

		// Token: 0x040032C9 RID: 13001
		private static List<BindingManagerBase> IgnoreItemChangedTable = new List<BindingManagerBase>();
	}
}
