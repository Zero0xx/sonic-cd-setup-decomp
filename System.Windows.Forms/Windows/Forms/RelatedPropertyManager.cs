using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020005E3 RID: 1507
	internal class RelatedPropertyManager : PropertyManager
	{
		// Token: 0x06004EAC RID: 20140 RVA: 0x00121EA3 File Offset: 0x00120EA3
		internal RelatedPropertyManager(BindingManagerBase parentManager, string dataField) : base(RelatedPropertyManager.GetCurrentOrNull(parentManager), dataField)
		{
			this.Bind(parentManager, dataField);
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x00121EBC File Offset: 0x00120EBC
		private void Bind(BindingManagerBase parentManager, string dataField)
		{
			this.parentManager = parentManager;
			this.dataField = dataField;
			this.fieldInfo = parentManager.GetItemProperties().Find(dataField, true);
			if (this.fieldInfo == null)
			{
				throw new ArgumentException(SR.GetString("RelatedListManagerChild", new object[]
				{
					dataField
				}));
			}
			parentManager.CurrentItemChanged += this.ParentManager_CurrentItemChanged;
			this.Refresh();
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x00121F28 File Offset: 0x00120F28
		internal override string GetListName()
		{
			string listName = this.GetListName(new ArrayList());
			if (listName.Length > 0)
			{
				return listName;
			}
			return base.GetListName();
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x00121F52 File Offset: 0x00120F52
		protected internal override string GetListName(ArrayList listAccessors)
		{
			listAccessors.Insert(0, this.fieldInfo);
			return this.parentManager.GetListName(listAccessors);
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x00121F70 File Offset: 0x00120F70
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

		// Token: 0x06004EB1 RID: 20145 RVA: 0x00121FB7 File Offset: 0x00120FB7
		private void ParentManager_CurrentItemChanged(object sender, EventArgs e)
		{
			this.Refresh();
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x00121FBF File Offset: 0x00120FBF
		private void Refresh()
		{
			this.EndCurrentEdit();
			this.SetDataSource(RelatedPropertyManager.GetCurrentOrNull(this.parentManager));
			this.OnCurrentChanged(EventArgs.Empty);
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06004EB3 RID: 20147 RVA: 0x00121FE3 File Offset: 0x00120FE3
		internal override Type BindType
		{
			get
			{
				return this.fieldInfo.PropertyType;
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06004EB4 RID: 20148 RVA: 0x00121FF0 File Offset: 0x00120FF0
		public override object Current
		{
			get
			{
				if (this.DataSource == null)
				{
					return null;
				}
				return this.fieldInfo.GetValue(this.DataSource);
			}
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x00122010 File Offset: 0x00121010
		private static object GetCurrentOrNull(BindingManagerBase parentManager)
		{
			if (parentManager.Position < 0 || parentManager.Position >= parentManager.Count)
			{
				return null;
			}
			return parentManager.Current;
		}

		// Token: 0x040032CB RID: 13003
		private BindingManagerBase parentManager;

		// Token: 0x040032CC RID: 13004
		private string dataField;

		// Token: 0x040032CD RID: 13005
		private PropertyDescriptor fieldInfo;
	}
}
