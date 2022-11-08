using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000250 RID: 592
	[DefaultProperty("DataSource")]
	[DefaultEvent("CurrentChanged")]
	[ComplexBindingProperties("DataSource", "DataMember")]
	[Designer("System.Windows.Forms.Design.BindingSourceDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionBindingSource")]
	public class BindingSource : Component, IBindingListView, IBindingList, IList, ICollection, IEnumerable, ITypedList, ICancelAddNew, ISupportInitializeNotification, ISupportInitialize, ICurrencyManagerProvider
	{
		// Token: 0x06001E92 RID: 7826 RVA: 0x0003F7C6 File Offset: 0x0003E7C6
		public BindingSource() : this(null, string.Empty)
		{
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0003F7D4 File Offset: 0x0003E7D4
		public BindingSource(object dataSource, string dataMember)
		{
			this.dataSource = dataSource;
			this.dataMember = dataMember;
			this._innerList = new ArrayList();
			this.currencyManager = new CurrencyManager(this);
			this.WireCurrencyManager(this.currencyManager);
			this.ResetList();
			this.listItemPropertyChangedHandler = new EventHandler(this.ListItem_PropertyChanged);
			this.WireDataSource();
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0003F856 File Offset: 0x0003E856
		public BindingSource(IContainer container) : this()
		{
			container.Add(this);
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0003F868 File Offset: 0x0003E868
		private bool AllowNewInternal(bool checkconstructor)
		{
			if (this.disposedOrFinalized)
			{
				return false;
			}
			if (this.allowNewIsSet)
			{
				return this.allowNewSetValue;
			}
			if (this.listExtractedFromEnumerable)
			{
				return false;
			}
			if (this.isBindingList)
			{
				return ((IBindingList)this.List).AllowNew;
			}
			return this.IsListWriteable(checkconstructor);
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0003F8B8 File Offset: 0x0003E8B8
		private bool IsListWriteable(bool checkconstructor)
		{
			return !this.List.IsReadOnly && !this.List.IsFixedSize && (!checkconstructor || this.itemConstructor != null);
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001E97 RID: 7831 RVA: 0x0003F8E7 File Offset: 0x0003E8E7
		[Browsable(false)]
		public virtual CurrencyManager CurrencyManager
		{
			get
			{
				return ((ICurrencyManagerProvider)this).GetRelatedCurrencyManager(null);
			}
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0003F8F0 File Offset: 0x0003E8F0
		public virtual CurrencyManager GetRelatedCurrencyManager(string dataMember)
		{
			this.EnsureInnerList();
			if (string.IsNullOrEmpty(dataMember))
			{
				return this.currencyManager;
			}
			if (dataMember.IndexOf(".") != -1)
			{
				return null;
			}
			BindingSource relatedBindingSource = this.GetRelatedBindingSource(dataMember);
			return ((ICurrencyManagerProvider)relatedBindingSource).CurrencyManager;
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0003F930 File Offset: 0x0003E930
		private BindingSource GetRelatedBindingSource(string dataMember)
		{
			if (this.relatedBindingSources == null)
			{
				this.relatedBindingSources = new Dictionary<string, BindingSource>();
			}
			foreach (string text in this.relatedBindingSources.Keys)
			{
				if (string.Equals(text, dataMember, StringComparison.OrdinalIgnoreCase))
				{
					return this.relatedBindingSources[text];
				}
			}
			BindingSource bindingSource = new BindingSource(this, dataMember);
			this.relatedBindingSources[dataMember] = bindingSource;
			return bindingSource;
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001E9A RID: 7834 RVA: 0x0003F9C8 File Offset: 0x0003E9C8
		[Browsable(false)]
		public object Current
		{
			get
			{
				if (this.currencyManager.Count <= 0)
				{
					return null;
				}
				return this.currencyManager.Current;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x0003F9E5 File Offset: 0x0003E9E5
		// (set) Token: 0x06001E9C RID: 7836 RVA: 0x0003F9ED File Offset: 0x0003E9ED
		[RefreshProperties(RefreshProperties.Repaint)]
		[Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("BindingSourceDataMemberDescr")]
		[SRCategory("CatData")]
		[DefaultValue("")]
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (!this.dataMember.Equals(value))
				{
					this.dataMember = value;
					this.ResetList();
					this.OnDataMemberChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x0003FA1F File Offset: 0x0003EA1F
		// (set) Token: 0x06001E9E RID: 7838 RVA: 0x0003FA27 File Offset: 0x0003EA27
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[SRDescription("BindingSourceDataSourceDescr")]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (this.dataSource != value)
				{
					this.ThrowIfBindingSourceRecursionDetected(value);
					this.UnwireDataSource();
					this.dataSource = value;
					this.ClearInvalidDataMember();
					this.ResetList();
					this.WireDataSource();
					this.OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x0003FA64 File Offset: 0x0003EA64
		// (set) Token: 0x06001EA0 RID: 7840 RVA: 0x0003FA94 File Offset: 0x0003EA94
		private string InnerListFilter
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				if (bindingListView != null && bindingListView.SupportsFiltering)
				{
					return bindingListView.Filter;
				}
				return string.Empty;
			}
			set
			{
				if (this.initializing || base.DesignMode)
				{
					return;
				}
				if (string.Equals(value, this.InnerListFilter, StringComparison.Ordinal))
				{
					return;
				}
				IBindingListView bindingListView = this.List as IBindingListView;
				if (bindingListView != null && bindingListView.SupportsFiltering)
				{
					bindingListView.Filter = value;
				}
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x0003FAE0 File Offset: 0x0003EAE0
		// (set) Token: 0x06001EA2 RID: 7842 RVA: 0x0003FB58 File Offset: 0x0003EB58
		private string InnerListSort
		{
			get
			{
				ListSortDescriptionCollection sortsColln = null;
				IBindingListView bindingListView = this.List as IBindingListView;
				IBindingList bindingList = this.List as IBindingList;
				if (bindingListView != null && bindingListView.SupportsAdvancedSorting)
				{
					sortsColln = bindingListView.SortDescriptions;
				}
				else if (bindingList != null && bindingList.SupportsSorting && bindingList.IsSorted)
				{
					sortsColln = new ListSortDescriptionCollection(new ListSortDescription[]
					{
						new ListSortDescription(bindingList.SortProperty, bindingList.SortDirection)
					});
				}
				return BindingSource.BuildSortString(sortsColln);
			}
			set
			{
				if (this.initializing || base.DesignMode)
				{
					return;
				}
				if (string.Compare(value, this.InnerListSort, false, CultureInfo.InvariantCulture) == 0)
				{
					return;
				}
				ListSortDescriptionCollection listSortDescriptionCollection = this.ParseSortString(value);
				IBindingListView bindingListView = this.List as IBindingListView;
				IBindingList bindingList = this.List as IBindingList;
				if (bindingListView == null || !bindingListView.SupportsAdvancedSorting)
				{
					if (bindingList != null && bindingList.SupportsSorting)
					{
						if (listSortDescriptionCollection.Count == 0)
						{
							bindingList.RemoveSort();
							return;
						}
						if (listSortDescriptionCollection.Count == 1)
						{
							bindingList.ApplySort(listSortDescriptionCollection[0].PropertyDescriptor, listSortDescriptionCollection[0].SortDirection);
						}
					}
					return;
				}
				if (listSortDescriptionCollection.Count == 0)
				{
					bindingListView.RemoveSort();
					return;
				}
				bindingListView.ApplySort(listSortDescriptionCollection);
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x0003FC0E File Offset: 0x0003EC0E
		[Browsable(false)]
		public bool IsBindingSuspended
		{
			get
			{
				return this.currencyManager.IsBindingSuspended;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x0003FC1B File Offset: 0x0003EC1B
		[Browsable(false)]
		public IList List
		{
			get
			{
				this.EnsureInnerList();
				return this._innerList;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x0003FC29 File Offset: 0x0003EC29
		// (set) Token: 0x06001EA6 RID: 7846 RVA: 0x0003FC36 File Offset: 0x0003EC36
		[DefaultValue(-1)]
		[Browsable(false)]
		public int Position
		{
			get
			{
				return this.currencyManager.Position;
			}
			set
			{
				if (this.currencyManager.Position != value)
				{
					this.currencyManager.Position = value;
				}
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001EA7 RID: 7847 RVA: 0x0003FC52 File Offset: 0x0003EC52
		// (set) Token: 0x06001EA8 RID: 7848 RVA: 0x0003FC5A File Offset: 0x0003EC5A
		[DefaultValue(true)]
		[Browsable(false)]
		public bool RaiseListChangedEvents
		{
			get
			{
				return this.raiseListChangedEvents;
			}
			set
			{
				if (this.raiseListChangedEvents != value)
				{
					this.raiseListChangedEvents = value;
				}
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x0003FC6C File Offset: 0x0003EC6C
		// (set) Token: 0x06001EAA RID: 7850 RVA: 0x0003FC74 File Offset: 0x0003EC74
		[SRDescription("BindingSourceSortDescr")]
		[DefaultValue(null)]
		[SRCategory("CatData")]
		public string Sort
		{
			get
			{
				return this.sort;
			}
			set
			{
				this.sort = value;
				this.InnerListSort = value;
			}
		}

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06001EAB RID: 7851 RVA: 0x0003FC84 File Offset: 0x0003EC84
		// (remove) Token: 0x06001EAC RID: 7852 RVA: 0x0003FC97 File Offset: 0x0003EC97
		[SRDescription("BindingSourceAddingNewEventHandlerDescr")]
		[SRCategory("CatData")]
		public event AddingNewEventHandler AddingNew
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_ADDINGNEW, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_ADDINGNEW, value);
			}
		}

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06001EAD RID: 7853 RVA: 0x0003FCAA File Offset: 0x0003ECAA
		// (remove) Token: 0x06001EAE RID: 7854 RVA: 0x0003FCBD File Offset: 0x0003ECBD
		[SRDescription("BindingSourceBindingCompleteEventHandlerDescr")]
		[SRCategory("CatData")]
		public event BindingCompleteEventHandler BindingComplete
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_BINDINGCOMPLETE, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_BINDINGCOMPLETE, value);
			}
		}

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06001EAF RID: 7855 RVA: 0x0003FCD0 File Offset: 0x0003ECD0
		// (remove) Token: 0x06001EB0 RID: 7856 RVA: 0x0003FCE3 File Offset: 0x0003ECE3
		[SRDescription("BindingSourceDataErrorEventHandlerDescr")]
		[SRCategory("CatData")]
		public event BindingManagerDataErrorEventHandler DataError
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_DATAERROR, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_DATAERROR, value);
			}
		}

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06001EB1 RID: 7857 RVA: 0x0003FCF6 File Offset: 0x0003ECF6
		// (remove) Token: 0x06001EB2 RID: 7858 RVA: 0x0003FD09 File Offset: 0x0003ED09
		[SRCategory("CatData")]
		[SRDescription("BindingSourceDataSourceChangedEventHandlerDescr")]
		public event EventHandler DataSourceChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_DATASOURCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_DATASOURCECHANGED, value);
			}
		}

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06001EB3 RID: 7859 RVA: 0x0003FD1C File Offset: 0x0003ED1C
		// (remove) Token: 0x06001EB4 RID: 7860 RVA: 0x0003FD2F File Offset: 0x0003ED2F
		[SRCategory("CatData")]
		[SRDescription("BindingSourceDataMemberChangedEventHandlerDescr")]
		public event EventHandler DataMemberChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_DATAMEMBERCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_DATAMEMBERCHANGED, value);
			}
		}

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06001EB5 RID: 7861 RVA: 0x0003FD42 File Offset: 0x0003ED42
		// (remove) Token: 0x06001EB6 RID: 7862 RVA: 0x0003FD55 File Offset: 0x0003ED55
		[SRDescription("BindingSourceCurrentChangedEventHandlerDescr")]
		[SRCategory("CatData")]
		public event EventHandler CurrentChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_CURRENTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_CURRENTCHANGED, value);
			}
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06001EB7 RID: 7863 RVA: 0x0003FD68 File Offset: 0x0003ED68
		// (remove) Token: 0x06001EB8 RID: 7864 RVA: 0x0003FD7B File Offset: 0x0003ED7B
		[SRCategory("CatData")]
		[SRDescription("BindingSourceCurrentItemChangedEventHandlerDescr")]
		public event EventHandler CurrentItemChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_CURRENTITEMCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_CURRENTITEMCHANGED, value);
			}
		}

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06001EB9 RID: 7865 RVA: 0x0003FD8E File Offset: 0x0003ED8E
		// (remove) Token: 0x06001EBA RID: 7866 RVA: 0x0003FDA1 File Offset: 0x0003EDA1
		[SRDescription("BindingSourceListChangedEventHandlerDescr")]
		[SRCategory("CatData")]
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_LISTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_LISTCHANGED, value);
			}
		}

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06001EBB RID: 7867 RVA: 0x0003FDB4 File Offset: 0x0003EDB4
		// (remove) Token: 0x06001EBC RID: 7868 RVA: 0x0003FDC7 File Offset: 0x0003EDC7
		[SRCategory("CatData")]
		[SRDescription("BindingSourcePositionChangedEventHandlerDescr")]
		public event EventHandler PositionChanged
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_POSITIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_POSITIONCHANGED, value);
			}
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0003FDDC File Offset: 0x0003EDDC
		private static string BuildSortString(ListSortDescriptionCollection sortsColln)
		{
			if (sortsColln == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(sortsColln.Count);
			for (int i = 0; i < sortsColln.Count; i++)
			{
				stringBuilder.Append(sortsColln[i].PropertyDescriptor.Name + ((sortsColln[i].SortDirection == ListSortDirection.Ascending) ? " ASC" : " DESC") + ((i < sortsColln.Count - 1) ? "," : string.Empty));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0003FE63 File Offset: 0x0003EE63
		public void CancelEdit()
		{
			this.currencyManager.CancelCurrentEdit();
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0003FE70 File Offset: 0x0003EE70
		private void ThrowIfBindingSourceRecursionDetected(object newDataSource)
		{
			for (BindingSource bindingSource = newDataSource as BindingSource; bindingSource != null; bindingSource = (bindingSource.DataSource as BindingSource))
			{
				if (bindingSource == this)
				{
					throw new InvalidOperationException(SR.GetString("BindingSourceRecursionDetected"));
				}
			}
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0003FEA9 File Offset: 0x0003EEA9
		private void ClearInvalidDataMember()
		{
			if (!this.IsDataMemberValid())
			{
				this.dataMember = "";
				this.OnDataMemberChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0003FECC File Offset: 0x0003EECC
		private static IList CreateBindingList(Type type)
		{
			Type typeFromHandle = typeof(BindingList<>);
			Type type2 = typeFromHandle.MakeGenericType(new Type[]
			{
				type
			});
			return (IList)SecurityUtils.SecureCreateInstance(type2);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0003FF04 File Offset: 0x0003EF04
		private static object CreateInstanceOfType(Type type)
		{
			object result = null;
			Exception ex = null;
			try
			{
				result = SecurityUtils.SecureCreateInstance(type);
			}
			catch (TargetInvocationException ex2)
			{
				ex = ex2;
			}
			catch (MethodAccessException ex3)
			{
				ex = ex3;
			}
			catch (MissingMethodException ex4)
			{
				ex = ex4;
			}
			if (ex != null)
			{
				throw new NotSupportedException(SR.GetString("BindingSourceInstanceError"), ex);
			}
			return result;
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0003FF6C File Offset: 0x0003EF6C
		private void CurrencyManager_PositionChanged(object sender, EventArgs e)
		{
			this.OnPositionChanged(e);
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0003FF75 File Offset: 0x0003EF75
		private void CurrencyManager_CurrentChanged(object sender, EventArgs e)
		{
			this.OnCurrentChanged(EventArgs.Empty);
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0003FF82 File Offset: 0x0003EF82
		private void CurrencyManager_CurrentItemChanged(object sender, EventArgs e)
		{
			this.OnCurrentItemChanged(EventArgs.Empty);
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0003FF8F File Offset: 0x0003EF8F
		private void CurrencyManager_BindingComplete(object sender, BindingCompleteEventArgs e)
		{
			this.OnBindingComplete(e);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0003FF98 File Offset: 0x0003EF98
		private void CurrencyManager_DataError(object sender, BindingManagerDataErrorEventArgs e)
		{
			this.OnDataError(e);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0003FFA4 File Offset: 0x0003EFA4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnwireDataSource();
				this.UnwireInnerList();
				this.UnhookItemChangedEventsForOldCurrent();
				this.UnwireCurrencyManager(this.currencyManager);
				this.dataSource = null;
				this.sort = null;
				this.dataMember = null;
				this._innerList = null;
				this.isBindingList = false;
				this.needToSetList = true;
				this.raiseListChangedEvents = false;
			}
			this.disposedOrFinalized = true;
			base.Dispose(disposing);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00040014 File Offset: 0x0003F014
		public void EndEdit()
		{
			if (this.endingEdit)
			{
				return;
			}
			try
			{
				this.endingEdit = true;
				this.currencyManager.EndCurrentEdit();
			}
			finally
			{
				this.endingEdit = false;
			}
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00040058 File Offset: 0x0003F058
		private void EnsureInnerList()
		{
			if (!this.initializing && this.needToSetList)
			{
				this.needToSetList = false;
				this.ResetList();
			}
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00040078 File Offset: 0x0003F078
		public int Find(string propertyName, object key)
		{
			PropertyDescriptor propertyDescriptor = (this.itemShape == null) ? null : this.itemShape.Find(propertyName, true);
			if (propertyDescriptor == null)
			{
				throw new ArgumentException(SR.GetString("DataSourceDataMemberPropNotFound", new object[]
				{
					propertyName
				}));
			}
			return ((IBindingList)this).Find(propertyDescriptor, key);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000400C8 File Offset: 0x0003F0C8
		private static IList GetListFromType(Type type)
		{
			IList result;
			if (typeof(ITypedList).IsAssignableFrom(type) && typeof(IList).IsAssignableFrom(type))
			{
				result = (BindingSource.CreateInstanceOfType(type) as IList);
			}
			else if (typeof(IListSource).IsAssignableFrom(type))
			{
				result = (BindingSource.CreateInstanceOfType(type) as IListSource).GetList();
			}
			else
			{
				result = BindingSource.CreateBindingList(ListBindingHelper.GetListItemType(type));
			}
			return result;
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0004013C File Offset: 0x0003F13C
		private static IList GetListFromEnumerable(IEnumerable enumerable)
		{
			IList list = null;
			foreach (object obj in enumerable)
			{
				if (list == null)
				{
					list = BindingSource.CreateBindingList(obj.GetType());
				}
				list.Add(obj);
			}
			return list;
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000401A0 File Offset: 0x0003F1A0
		private bool IsDataMemberValid()
		{
			if (this.initializing)
			{
				return true;
			}
			if (string.IsNullOrEmpty(this.dataMember))
			{
				return true;
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(this.dataSource);
			return listItemProperties[this.dataMember] != null;
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000401E8 File Offset: 0x0003F1E8
		private void InnerList_ListChanged(object sender, ListChangedEventArgs e)
		{
			if (!this.innerListChanging)
			{
				try
				{
					this.innerListChanging = true;
					this.OnListChanged(e);
				}
				finally
				{
					this.innerListChanging = false;
				}
			}
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x00040228 File Offset: 0x0003F228
		private void ListItem_PropertyChanged(object sender, EventArgs e)
		{
			int newIndex;
			if (sender == this.currentItemHookedForItemChange)
			{
				newIndex = this.Position;
			}
			else
			{
				newIndex = ((IList)this).IndexOf(sender);
			}
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, newIndex));
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0004025C File Offset: 0x0003F25C
		public void MoveFirst()
		{
			this.Position = 0;
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x00040265 File Offset: 0x0003F265
		public void MoveLast()
		{
			this.Position = this.Count - 1;
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x00040275 File Offset: 0x0003F275
		public void MoveNext()
		{
			this.Position++;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x00040285 File Offset: 0x0003F285
		public void MovePrevious()
		{
			this.Position--;
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x00040295 File Offset: 0x0003F295
		private void OnSimpleListChanged(ListChangedType listChangedType, int newIndex)
		{
			if (!this.isBindingList)
			{
				this.OnListChanged(new ListChangedEventArgs(listChangedType, newIndex));
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000402AC File Offset: 0x0003F2AC
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			AddingNewEventHandler addingNewEventHandler = (AddingNewEventHandler)base.Events[BindingSource.EVENT_ADDINGNEW];
			if (addingNewEventHandler != null)
			{
				addingNewEventHandler(this, e);
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000402DC File Offset: 0x0003F2DC
		protected virtual void OnBindingComplete(BindingCompleteEventArgs e)
		{
			BindingCompleteEventHandler bindingCompleteEventHandler = (BindingCompleteEventHandler)base.Events[BindingSource.EVENT_BINDINGCOMPLETE];
			if (bindingCompleteEventHandler != null)
			{
				bindingCompleteEventHandler(this, e);
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0004030C File Offset: 0x0003F30C
		protected virtual void OnCurrentChanged(EventArgs e)
		{
			this.UnhookItemChangedEventsForOldCurrent();
			this.HookItemChangedEventsForNewCurrent();
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_CURRENTCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x00040348 File Offset: 0x0003F348
		protected virtual void OnCurrentItemChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_CURRENTITEMCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00040378 File Offset: 0x0003F378
		protected virtual void OnDataError(BindingManagerDataErrorEventArgs e)
		{
			BindingManagerDataErrorEventHandler bindingManagerDataErrorEventHandler = base.Events[BindingSource.EVENT_DATAERROR] as BindingManagerDataErrorEventHandler;
			if (bindingManagerDataErrorEventHandler != null)
			{
				bindingManagerDataErrorEventHandler(this, e);
			}
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000403A8 File Offset: 0x0003F3A8
		protected virtual void OnDataMemberChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[BindingSource.EVENT_DATAMEMBERCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000403D8 File Offset: 0x0003F3D8
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[BindingSource.EVENT_DATASOURCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x00040408 File Offset: 0x0003F408
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			if (!this.raiseListChangedEvents || this.initializing)
			{
				return;
			}
			ListChangedEventHandler listChangedEventHandler = (ListChangedEventHandler)base.Events[BindingSource.EVENT_LISTCHANGED];
			if (listChangedEventHandler != null)
			{
				listChangedEventHandler(this, e);
			}
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x00040448 File Offset: 0x0003F448
		protected virtual void OnPositionChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_POSITIONCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x00040478 File Offset: 0x0003F478
		private void ParentCurrencyManager_CurrentItemChanged(object sender, EventArgs e)
		{
			if (this.initializing)
			{
				return;
			}
			if (this.parentsCurrentItemChanging)
			{
				return;
			}
			try
			{
				this.parentsCurrentItemChanging = true;
				bool flag;
				this.currencyManager.PullData(out flag);
			}
			finally
			{
				this.parentsCurrentItemChanging = false;
			}
			CurrencyManager currencyManager = (CurrencyManager)sender;
			if (!string.IsNullOrEmpty(this.dataMember))
			{
				object obj = null;
				IList list = null;
				if (currencyManager.Count > 0)
				{
					PropertyDescriptorCollection itemProperties = currencyManager.GetItemProperties();
					PropertyDescriptor propertyDescriptor = itemProperties[this.dataMember];
					if (propertyDescriptor != null)
					{
						obj = ListBindingHelper.GetList(propertyDescriptor.GetValue(currencyManager.Current));
						list = (obj as IList);
					}
				}
				if (list != null)
				{
					this.SetList(list, false, true);
				}
				else if (obj != null)
				{
					this.SetList(BindingSource.WrapObjectInBindingList(obj), false, false);
				}
				else
				{
					this.SetList(BindingSource.CreateBindingList(this.itemType), false, false);
				}
				bool flag2 = this.lastCurrentItem == null || currencyManager.Count == 0 || this.lastCurrentItem != currencyManager.Current || this.Position >= this.Count;
				this.lastCurrentItem = ((currencyManager.Count > 0) ? currencyManager.Current : null);
				if (flag2)
				{
					this.Position = ((this.Count > 0) ? 0 : -1);
				}
			}
			this.OnCurrentItemChanged(EventArgs.Empty);
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000405C4 File Offset: 0x0003F5C4
		private void ParentCurrencyManager_MetaDataChanged(object sender, EventArgs e)
		{
			this.ClearInvalidDataMember();
			this.ResetList();
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000405D4 File Offset: 0x0003F5D4
		private ListSortDescriptionCollection ParseSortString(string sortString)
		{
			if (string.IsNullOrEmpty(sortString))
			{
				return new ListSortDescriptionCollection();
			}
			ArrayList arrayList = new ArrayList();
			PropertyDescriptorCollection itemProperties = this.currencyManager.GetItemProperties();
			string[] array = sortString.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				int length = text.Length;
				bool flag = true;
				if (length >= 5 && string.Compare(text, length - 4, " ASC", 0, 4, true, CultureInfo.InvariantCulture) == 0)
				{
					text = text.Substring(0, length - 4).Trim();
				}
				else if (length >= 6 && string.Compare(text, length - 5, " DESC", 0, 5, true, CultureInfo.InvariantCulture) == 0)
				{
					flag = false;
					text = text.Substring(0, length - 5).Trim();
				}
				if (text.StartsWith("["))
				{
					if (!text.EndsWith("]"))
					{
						throw new ArgumentException(SR.GetString("BindingSourceBadSortString"));
					}
					text = text.Substring(1, text.Length - 2);
				}
				PropertyDescriptor propertyDescriptor = itemProperties.Find(text, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("BindingSourceSortStringPropertyNotInIBindingList"));
				}
				arrayList.Add(new ListSortDescription(propertyDescriptor, flag ? ListSortDirection.Ascending : ListSortDirection.Descending));
			}
			ListSortDescription[] array2 = new ListSortDescription[arrayList.Count];
			arrayList.CopyTo(array2);
			return new ListSortDescriptionCollection(array2);
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x00040740 File Offset: 0x0003F740
		public void RemoveCurrent()
		{
			if (!((IBindingList)this).AllowRemove)
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceRemoveCurrentNotAllowed"));
			}
			if (this.Position < 0 || this.Position >= this.Count)
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceRemoveCurrentNoCurrentItem"));
			}
			this.RemoveAt(this.Position);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x00040798 File Offset: 0x0003F798
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void ResetAllowNew()
		{
			this.allowNewIsSet = false;
			this.allowNewSetValue = true;
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000407A8 File Offset: 0x0003F7A8
		public void ResetBindings(bool metadataChanged)
		{
			if (metadataChanged)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, null));
			}
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000407C7 File Offset: 0x0003F7C7
		public void ResetCurrentItem()
		{
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.Position));
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000407DB File Offset: 0x0003F7DB
		public void ResetItem(int itemIndex)
		{
			this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, itemIndex));
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000407EA File Offset: 0x0003F7EA
		public void ResumeBinding()
		{
			this.currencyManager.ResumeBinding();
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000407F7 File Offset: 0x0003F7F7
		public void SuspendBinding()
		{
			this.currencyManager.SuspendBinding();
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x00040804 File Offset: 0x0003F804
		private void ResetList()
		{
			if (this.initializing)
			{
				this.needToSetList = true;
				return;
			}
			this.needToSetList = false;
			object obj = (this.dataSource is Type) ? BindingSource.GetListFromType(this.dataSource as Type) : this.dataSource;
			object obj2 = ListBindingHelper.GetList(obj, this.dataMember);
			this.listExtractedFromEnumerable = false;
			if (!(obj2 is IList))
			{
				if (obj2 is IEnumerable)
				{
					obj2 = BindingSource.GetListFromEnumerable(obj2 as IEnumerable);
					this.listExtractedFromEnumerable = true;
				}
				else if (obj2 != null)
				{
					obj2 = BindingSource.WrapObjectInBindingList(obj2);
				}
				else
				{
					obj2 = BindingSource.CreateBindingList(ListBindingHelper.GetListItemType(this.dataSource, this.dataMember));
				}
			}
			this.SetList(obj2 as IList, true, true);
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000408B8 File Offset: 0x0003F8B8
		private void SetList(IList list, bool metaDataChanged, bool applySortAndFilter)
		{
			if (list == null)
			{
				list = BindingSource.CreateBindingList(this.itemType);
			}
			this.UnwireInnerList();
			this.UnhookItemChangedEventsForOldCurrent();
			this._innerList = list;
			this.isBindingList = (list is IBindingList);
			if (list is IRaiseItemChangedEvents)
			{
				this.listRaisesItemChangedEvents = (list as IRaiseItemChangedEvents).RaisesItemChangedEvents;
			}
			else
			{
				this.listRaisesItemChangedEvents = this.isBindingList;
			}
			if (metaDataChanged)
			{
				this.itemType = ListBindingHelper.GetListItemType(this.List);
				this.itemShape = ListBindingHelper.GetListItemProperties(this.List);
				this.itemConstructor = this.itemType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new Type[0], null);
			}
			this.WireInnerList();
			this.HookItemChangedEventsForNewCurrent();
			this.ResetBindings(metaDataChanged);
			if (applySortAndFilter)
			{
				if (this.Sort != null)
				{
					this.InnerListSort = this.Sort;
				}
				if (this.Filter != null)
				{
					this.InnerListFilter = this.Filter;
				}
			}
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000409A0 File Offset: 0x0003F9A0
		private static IList WrapObjectInBindingList(object obj)
		{
			IList list = BindingSource.CreateBindingList(obj.GetType());
			list.Add(obj);
			return list;
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000409C2 File Offset: 0x0003F9C2
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal virtual bool ShouldSerializeAllowNew()
		{
			return this.allowNewIsSet;
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000409CC File Offset: 0x0003F9CC
		private void HookItemChangedEventsForNewCurrent()
		{
			if (!this.listRaisesItemChangedEvents)
			{
				if (this.Position >= 0 && this.Position <= this.Count - 1)
				{
					this.currentItemHookedForItemChange = this.Current;
					this.WirePropertyChangedEvents(this.currentItemHookedForItemChange);
					return;
				}
				this.currentItemHookedForItemChange = null;
			}
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x00040A1A File Offset: 0x0003FA1A
		private void UnhookItemChangedEventsForOldCurrent()
		{
			if (!this.listRaisesItemChangedEvents)
			{
				this.UnwirePropertyChangedEvents(this.currentItemHookedForItemChange);
				this.currentItemHookedForItemChange = null;
			}
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x00040A38 File Offset: 0x0003FA38
		private void WireCurrencyManager(CurrencyManager cm)
		{
			if (cm != null)
			{
				cm.PositionChanged += this.CurrencyManager_PositionChanged;
				cm.CurrentChanged += this.CurrencyManager_CurrentChanged;
				cm.CurrentItemChanged += this.CurrencyManager_CurrentItemChanged;
				cm.BindingComplete += this.CurrencyManager_BindingComplete;
				cm.DataError += this.CurrencyManager_DataError;
			}
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x00040AA4 File Offset: 0x0003FAA4
		private void UnwireCurrencyManager(CurrencyManager cm)
		{
			if (cm != null)
			{
				cm.PositionChanged -= this.CurrencyManager_PositionChanged;
				cm.CurrentChanged -= this.CurrencyManager_CurrentChanged;
				cm.CurrentItemChanged -= this.CurrencyManager_CurrentItemChanged;
				cm.BindingComplete -= this.CurrencyManager_BindingComplete;
				cm.DataError -= this.CurrencyManager_DataError;
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x00040B10 File Offset: 0x0003FB10
		private void WireDataSource()
		{
			if (this.dataSource is ICurrencyManagerProvider)
			{
				CurrencyManager currencyManager = (this.dataSource as ICurrencyManagerProvider).CurrencyManager;
				currencyManager.CurrentItemChanged += this.ParentCurrencyManager_CurrentItemChanged;
				currencyManager.MetaDataChanged += this.ParentCurrencyManager_MetaDataChanged;
			}
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x00040B60 File Offset: 0x0003FB60
		private void UnwireDataSource()
		{
			if (this.dataSource is ICurrencyManagerProvider)
			{
				CurrencyManager currencyManager = (this.dataSource as ICurrencyManagerProvider).CurrencyManager;
				currencyManager.CurrentItemChanged -= this.ParentCurrencyManager_CurrentItemChanged;
				currencyManager.MetaDataChanged -= this.ParentCurrencyManager_MetaDataChanged;
			}
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x00040BB0 File Offset: 0x0003FBB0
		private void WireInnerList()
		{
			if (this._innerList is IBindingList)
			{
				IBindingList bindingList = this._innerList as IBindingList;
				bindingList.ListChanged += this.InnerList_ListChanged;
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x00040BE8 File Offset: 0x0003FBE8
		private void UnwireInnerList()
		{
			if (this._innerList is IBindingList)
			{
				IBindingList bindingList = this._innerList as IBindingList;
				bindingList.ListChanged -= this.InnerList_ListChanged;
			}
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x00040C20 File Offset: 0x0003FC20
		private void WirePropertyChangedEvents(object item)
		{
			if (item != null && this.itemShape != null)
			{
				for (int i = 0; i < this.itemShape.Count; i++)
				{
					this.itemShape[i].AddValueChanged(item, this.listItemPropertyChangedHandler);
				}
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x00040C68 File Offset: 0x0003FC68
		private void UnwirePropertyChangedEvents(object item)
		{
			if (item != null && this.itemShape != null)
			{
				for (int i = 0; i < this.itemShape.Count; i++)
				{
					this.itemShape[i].RemoveValueChanged(item, this.listItemPropertyChangedHandler);
				}
			}
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x00040CAE File Offset: 0x0003FCAE
		void ISupportInitialize.BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x00040CB7 File Offset: 0x0003FCB7
		private void EndInitCore()
		{
			this.initializing = false;
			this.EnsureInnerList();
			this.OnInitialized();
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x00040CCC File Offset: 0x0003FCCC
		void ISupportInitialize.EndInit()
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
			{
				supportInitializeNotification.Initialized += this.DataSource_Initialized;
				return;
			}
			this.EndInitCore();
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x00040D0C File Offset: 0x0003FD0C
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.EndInitCore();
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001EFB RID: 7931 RVA: 0x00040D40 File Offset: 0x0003FD40
		bool ISupportInitializeNotification.IsInitialized
		{
			get
			{
				return !this.initializing;
			}
		}

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06001EFC RID: 7932 RVA: 0x00040D4B File Offset: 0x0003FD4B
		// (remove) Token: 0x06001EFD RID: 7933 RVA: 0x00040D5E File Offset: 0x0003FD5E
		event EventHandler ISupportInitializeNotification.Initialized
		{
			add
			{
				base.Events.AddHandler(BindingSource.EVENT_INITIALIZED, value);
			}
			remove
			{
				base.Events.RemoveHandler(BindingSource.EVENT_INITIALIZED, value);
			}
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x00040D74 File Offset: 0x0003FD74
		private void OnInitialized()
		{
			EventHandler eventHandler = (EventHandler)base.Events[BindingSource.EVENT_INITIALIZED];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x00040DA6 File Offset: 0x0003FDA6
		public virtual IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x00040DB3 File Offset: 0x0003FDB3
		public virtual void CopyTo(Array arr, int index)
		{
			this.List.CopyTo(arr, index);
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x00040DC4 File Offset: 0x0003FDC4
		[Browsable(false)]
		public virtual int Count
		{
			get
			{
				int result;
				try
				{
					if (this.disposedOrFinalized)
					{
						result = 0;
					}
					else
					{
						if (this.recursionDetectionFlag)
						{
							throw new InvalidOperationException(SR.GetString("BindingSourceRecursionDetected"));
						}
						this.recursionDetectionFlag = true;
						result = this.List.Count;
					}
				}
				finally
				{
					this.recursionDetectionFlag = false;
				}
				return result;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x00040E24 File Offset: 0x0003FE24
		[Browsable(false)]
		public virtual bool IsSynchronized
		{
			get
			{
				return this.List.IsSynchronized;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001F03 RID: 7939 RVA: 0x00040E31 File Offset: 0x0003FE31
		[Browsable(false)]
		public virtual object SyncRoot
		{
			get
			{
				return this.List.SyncRoot;
			}
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x00040E40 File Offset: 0x0003FE40
		public virtual int Add(object value)
		{
			if (this.dataSource == null && this.List.Count == 0)
			{
				this.SetList(BindingSource.CreateBindingList((value == null) ? typeof(object) : value.GetType()), true, true);
			}
			if (value != null && !this.itemType.IsAssignableFrom(value.GetType()))
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceItemTypeMismatchOnAdd"));
			}
			if (value == null && this.itemType.IsValueType)
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceItemTypeIsValueType"));
			}
			int num = this.List.Add(value);
			this.OnSimpleListChanged(ListChangedType.ItemAdded, num);
			return num;
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x00040EE2 File Offset: 0x0003FEE2
		public virtual void Clear()
		{
			this.UnhookItemChangedEventsForOldCurrent();
			this.List.Clear();
			this.OnSimpleListChanged(ListChangedType.Reset, -1);
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x00040EFD File Offset: 0x0003FEFD
		public virtual bool Contains(object value)
		{
			return this.List.Contains(value);
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x00040F0B File Offset: 0x0003FF0B
		public virtual int IndexOf(object value)
		{
			return this.List.IndexOf(value);
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x00040F19 File Offset: 0x0003FF19
		public virtual void Insert(int index, object value)
		{
			this.List.Insert(index, value);
			this.OnSimpleListChanged(ListChangedType.ItemAdded, index);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x00040F30 File Offset: 0x0003FF30
		public virtual void Remove(object value)
		{
			int num = ((IList)this).IndexOf(value);
			this.List.Remove(value);
			if (num != -1)
			{
				this.OnSimpleListChanged(ListChangedType.ItemDeleted, num);
			}
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x00040F5D File Offset: 0x0003FF5D
		public virtual void RemoveAt(int index)
		{
			object obj = ((IList)this)[index];
			this.List.RemoveAt(index);
			this.OnSimpleListChanged(ListChangedType.ItemDeleted, index);
		}

		// Token: 0x1700044B RID: 1099
		[Browsable(false)]
		public virtual object this[int index]
		{
			get
			{
				return this.List[index];
			}
			set
			{
				this.List[index] = value;
				if (!this.isBindingList)
				{
					this.OnSimpleListChanged(ListChangedType.ItemChanged, index);
				}
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x00040FA8 File Offset: 0x0003FFA8
		[Browsable(false)]
		public virtual bool IsFixedSize
		{
			get
			{
				return this.List.IsFixedSize;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x00040FB5 File Offset: 0x0003FFB5
		[Browsable(false)]
		public virtual bool IsReadOnly
		{
			get
			{
				return this.List.IsReadOnly;
			}
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x00040FC2 File Offset: 0x0003FFC2
		public virtual string GetListName(PropertyDescriptor[] listAccessors)
		{
			return ListBindingHelper.GetListName(this.List, listAccessors);
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x00040FD0 File Offset: 0x0003FFD0
		public virtual PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			object list = ListBindingHelper.GetList(this.dataSource);
			if (list is ITypedList && !string.IsNullOrEmpty(this.dataMember))
			{
				return ListBindingHelper.GetListItemProperties(list, this.dataMember, listAccessors);
			}
			return ListBindingHelper.GetListItemProperties(this.List, listAccessors);
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x00041018 File Offset: 0x00040018
		public virtual object AddNew()
		{
			if (!this.AllowNewInternal(false))
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceBindingListWrapperAddToReadOnlyList"));
			}
			if (!this.AllowNewInternal(true))
			{
				throw new InvalidOperationException(SR.GetString("BindingSourceBindingListWrapperNeedToSetAllowNew", new object[]
				{
					(this.itemType == null) ? "(null)" : this.itemType.FullName
				}));
			}
			int num = this.addNewPos;
			this.EndEdit();
			if (num != -1)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, num));
			}
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs();
			int count = this.List.Count;
			this.OnAddingNew(addingNewEventArgs);
			object obj = addingNewEventArgs.NewObject;
			if (obj == null)
			{
				if (this.isBindingList)
				{
					obj = (this.List as IBindingList).AddNew();
					this.Position = this.Count - 1;
					return obj;
				}
				if (this.itemConstructor == null)
				{
					throw new InvalidOperationException(SR.GetString("BindingSourceBindingListWrapperNeedAParameterlessConstructor", new object[]
					{
						(this.itemType == null) ? "(null)" : this.itemType.FullName
					}));
				}
				obj = this.itemConstructor.Invoke(null);
			}
			if (this.List.Count > count)
			{
				this.addNewPos = this.Position;
			}
			else
			{
				this.addNewPos = this.Add(obj);
				this.Position = this.addNewPos;
			}
			return obj;
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x0004116C File Offset: 0x0004016C
		[Browsable(false)]
		public virtual bool AllowEdit
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).AllowEdit;
				}
				return !this.List.IsReadOnly;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x00041195 File Offset: 0x00040195
		// (set) Token: 0x06001F14 RID: 7956 RVA: 0x000411A0 File Offset: 0x000401A0
		[SRDescription("BindingSourceAllowNewDescr")]
		[SRCategory("CatBehavior")]
		public virtual bool AllowNew
		{
			get
			{
				return this.AllowNewInternal(true);
			}
			set
			{
				if (this.allowNewIsSet && value == this.allowNewSetValue)
				{
					return;
				}
				if (value && !this.isBindingList && !this.IsListWriteable(false))
				{
					throw new InvalidOperationException(SR.GetString("NoAllowNewOnReadOnlyList"));
				}
				this.allowNewIsSet = true;
				this.allowNewSetValue = value;
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x000411FE File Offset: 0x000401FE
		[Browsable(false)]
		public virtual bool AllowRemove
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).AllowRemove;
				}
				return !this.List.IsReadOnly && !this.List.IsFixedSize;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x00041236 File Offset: 0x00040236
		[Browsable(false)]
		public virtual bool SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x00041239 File Offset: 0x00040239
		[Browsable(false)]
		public virtual bool SupportsSearching
		{
			get
			{
				return this.isBindingList && ((IBindingList)this.List).SupportsSearching;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001F18 RID: 7960 RVA: 0x00041255 File Offset: 0x00040255
		[Browsable(false)]
		public virtual bool SupportsSorting
		{
			get
			{
				return this.isBindingList && ((IBindingList)this.List).SupportsSorting;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001F19 RID: 7961 RVA: 0x00041271 File Offset: 0x00040271
		[Browsable(false)]
		public virtual bool IsSorted
		{
			get
			{
				return this.isBindingList && ((IBindingList)this.List).IsSorted;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x0004128D File Offset: 0x0004028D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual PropertyDescriptor SortProperty
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).SortProperty;
				}
				return null;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000412A9 File Offset: 0x000402A9
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public virtual ListSortDirection SortDirection
		{
			get
			{
				if (this.isBindingList)
				{
					return ((IBindingList)this.List).SortDirection;
				}
				return ListSortDirection.Ascending;
			}
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000412C5 File Offset: 0x000402C5
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			if (this.isBindingList)
			{
				((IBindingList)this.List).AddIndex(property);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000412F0 File Offset: 0x000402F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ApplySort(PropertyDescriptor property, ListSortDirection sort)
		{
			if (this.isBindingList)
			{
				((IBindingList)this.List).ApplySort(property, sort);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0004131C File Offset: 0x0004031C
		public virtual int Find(PropertyDescriptor prop, object key)
		{
			if (this.isBindingList)
			{
				return ((IBindingList)this.List).Find(prop, key);
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x00041348 File Offset: 0x00040348
		void IBindingList.RemoveIndex(PropertyDescriptor prop)
		{
			if (this.isBindingList)
			{
				((IBindingList)this.List).RemoveIndex(prop);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingList"));
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x00041373 File Offset: 0x00040373
		public virtual void RemoveSort()
		{
			this.sort = null;
			if (this.isBindingList)
			{
				((IBindingList)this.List).RemoveSort();
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x00041394 File Offset: 0x00040394
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ApplySort(ListSortDescriptionCollection sorts)
		{
			IBindingListView bindingListView = this.List as IBindingListView;
			if (bindingListView != null)
			{
				bindingListView.ApplySort(sorts);
				return;
			}
			throw new NotSupportedException(SR.GetString("OperationRequiresIBindingListView"));
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001F22 RID: 7970 RVA: 0x000413C8 File Offset: 0x000403C8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual ListSortDescriptionCollection SortDescriptions
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				if (bindingListView != null)
				{
					return bindingListView.SortDescriptions;
				}
				return null;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x000413EC File Offset: 0x000403EC
		// (set) Token: 0x06001F24 RID: 7972 RVA: 0x000413F4 File Offset: 0x000403F4
		[SRDescription("BindingSourceFilterDescr")]
		[SRCategory("CatData")]
		[DefaultValue(null)]
		public virtual string Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				this.filter = value;
				this.InnerListFilter = value;
			}
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00041404 File Offset: 0x00040404
		public virtual void RemoveFilter()
		{
			this.filter = null;
			IBindingListView bindingListView = this.List as IBindingListView;
			if (bindingListView != null)
			{
				bindingListView.RemoveFilter();
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x00041430 File Offset: 0x00040430
		[Browsable(false)]
		public virtual bool SupportsAdvancedSorting
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				return bindingListView != null && bindingListView.SupportsAdvancedSorting;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x00041454 File Offset: 0x00040454
		[Browsable(false)]
		public virtual bool SupportsFiltering
		{
			get
			{
				IBindingListView bindingListView = this.List as IBindingListView;
				return bindingListView != null && bindingListView.SupportsFiltering;
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x00041478 File Offset: 0x00040478
		void ICancelAddNew.CancelNew(int position)
		{
			if (this.addNewPos >= 0 && this.addNewPos == position)
			{
				this.RemoveAt(this.addNewPos);
				this.addNewPos = -1;
				return;
			}
			ICancelAddNew cancelAddNew = this.List as ICancelAddNew;
			if (cancelAddNew != null)
			{
				cancelAddNew.CancelNew(position);
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000414C4 File Offset: 0x000404C4
		void ICancelAddNew.EndNew(int position)
		{
			if (this.addNewPos >= 0 && this.addNewPos == position)
			{
				this.addNewPos = -1;
				return;
			}
			ICancelAddNew cancelAddNew = this.List as ICancelAddNew;
			if (cancelAddNew != null)
			{
				cancelAddNew.EndNew(position);
			}
		}

		// Token: 0x040013EC RID: 5100
		private static readonly object EVENT_ADDINGNEW = new object();

		// Token: 0x040013ED RID: 5101
		private static readonly object EVENT_BINDINGCOMPLETE = new object();

		// Token: 0x040013EE RID: 5102
		private static readonly object EVENT_CURRENTCHANGED = new object();

		// Token: 0x040013EF RID: 5103
		private static readonly object EVENT_CURRENTITEMCHANGED = new object();

		// Token: 0x040013F0 RID: 5104
		private static readonly object EVENT_DATAERROR = new object();

		// Token: 0x040013F1 RID: 5105
		private static readonly object EVENT_DATAMEMBERCHANGED = new object();

		// Token: 0x040013F2 RID: 5106
		private static readonly object EVENT_DATASOURCECHANGED = new object();

		// Token: 0x040013F3 RID: 5107
		private static readonly object EVENT_LISTCHANGED = new object();

		// Token: 0x040013F4 RID: 5108
		private static readonly object EVENT_POSITIONCHANGED = new object();

		// Token: 0x040013F5 RID: 5109
		private static readonly object EVENT_INITIALIZED = new object();

		// Token: 0x040013F6 RID: 5110
		private object dataSource;

		// Token: 0x040013F7 RID: 5111
		private string dataMember = string.Empty;

		// Token: 0x040013F8 RID: 5112
		private string sort;

		// Token: 0x040013F9 RID: 5113
		private string filter;

		// Token: 0x040013FA RID: 5114
		private CurrencyManager currencyManager;

		// Token: 0x040013FB RID: 5115
		private bool raiseListChangedEvents = true;

		// Token: 0x040013FC RID: 5116
		private bool parentsCurrentItemChanging;

		// Token: 0x040013FD RID: 5117
		private bool disposedOrFinalized;

		// Token: 0x040013FE RID: 5118
		private IList _innerList;

		// Token: 0x040013FF RID: 5119
		private bool isBindingList;

		// Token: 0x04001400 RID: 5120
		private bool listRaisesItemChangedEvents;

		// Token: 0x04001401 RID: 5121
		private bool listExtractedFromEnumerable;

		// Token: 0x04001402 RID: 5122
		private Type itemType;

		// Token: 0x04001403 RID: 5123
		private ConstructorInfo itemConstructor;

		// Token: 0x04001404 RID: 5124
		private PropertyDescriptorCollection itemShape;

		// Token: 0x04001405 RID: 5125
		private Dictionary<string, BindingSource> relatedBindingSources;

		// Token: 0x04001406 RID: 5126
		private bool allowNewIsSet;

		// Token: 0x04001407 RID: 5127
		private bool allowNewSetValue = true;

		// Token: 0x04001408 RID: 5128
		private object currentItemHookedForItemChange;

		// Token: 0x04001409 RID: 5129
		private object lastCurrentItem;

		// Token: 0x0400140A RID: 5130
		private EventHandler listItemPropertyChangedHandler;

		// Token: 0x0400140B RID: 5131
		private int addNewPos = -1;

		// Token: 0x0400140C RID: 5132
		private bool initializing;

		// Token: 0x0400140D RID: 5133
		private bool needToSetList;

		// Token: 0x0400140E RID: 5134
		private bool recursionDetectionFlag;

		// Token: 0x0400140F RID: 5135
		private bool innerListChanging;

		// Token: 0x04001410 RID: 5136
		private bool endingEdit;
	}
}
