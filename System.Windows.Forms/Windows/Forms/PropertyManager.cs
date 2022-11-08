using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020005CB RID: 1483
	public class PropertyManager : BindingManagerBase
	{
		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06004DFD RID: 19965 RVA: 0x0011FFD2 File Offset: 0x0011EFD2
		public override object Current
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0011FFDA File Offset: 0x0011EFDA
		private void PropertyChanged(object sender, EventArgs ea)
		{
			this.EndCurrentEdit();
			this.OnCurrentChanged(EventArgs.Empty);
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x0011FFF0 File Offset: 0x0011EFF0
		internal override void SetDataSource(object dataSource)
		{
			if (this.dataSource != null && !string.IsNullOrEmpty(this.propName))
			{
				this.propInfo.RemoveValueChanged(this.dataSource, new EventHandler(this.PropertyChanged));
				this.propInfo = null;
			}
			this.dataSource = dataSource;
			if (this.dataSource != null && !string.IsNullOrEmpty(this.propName))
			{
				this.propInfo = TypeDescriptor.GetProperties(dataSource).Find(this.propName, true);
				if (this.propInfo == null)
				{
					throw new ArgumentException(SR.GetString("PropertyManagerPropDoesNotExist", new object[]
					{
						this.propName,
						dataSource.ToString()
					}));
				}
				this.propInfo.AddValueChanged(dataSource, new EventHandler(this.PropertyChanged));
			}
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x001200B4 File Offset: 0x0011F0B4
		public PropertyManager()
		{
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x001200BC File Offset: 0x0011F0BC
		internal PropertyManager(object dataSource) : base(dataSource)
		{
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x001200C5 File Offset: 0x0011F0C5
		internal PropertyManager(object dataSource, string propName)
		{
			this.propName = propName;
			this.SetDataSource(dataSource);
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x001200DB File Offset: 0x0011F0DB
		internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return ListBindingHelper.GetListItemProperties(this.dataSource, listAccessors);
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06004E04 RID: 19972 RVA: 0x001200E9 File Offset: 0x0011F0E9
		internal override Type BindType
		{
			get
			{
				return this.dataSource.GetType();
			}
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x001200F6 File Offset: 0x0011F0F6
		internal override string GetListName()
		{
			return TypeDescriptor.GetClassName(this.dataSource) + "." + this.propName;
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x00120114 File Offset: 0x0011F114
		public override void SuspendBinding()
		{
			this.EndCurrentEdit();
			if (this.bound)
			{
				try
				{
					this.bound = false;
					this.UpdateIsBinding();
				}
				catch
				{
					this.bound = true;
					this.UpdateIsBinding();
					throw;
				}
			}
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x00120160 File Offset: 0x0011F160
		public override void ResumeBinding()
		{
			this.OnCurrentChanged(new EventArgs());
			if (!this.bound)
			{
				try
				{
					this.bound = true;
					this.UpdateIsBinding();
				}
				catch
				{
					this.bound = false;
					this.UpdateIsBinding();
					throw;
				}
			}
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x001201B0 File Offset: 0x0011F1B0
		protected internal override string GetListName(ArrayList listAccessors)
		{
			return "";
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x001201B8 File Offset: 0x0011F1B8
		public override void CancelCurrentEdit()
		{
			IEditableObject editableObject = this.Current as IEditableObject;
			if (editableObject != null)
			{
				editableObject.CancelEdit();
			}
			base.PushData();
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x001201E0 File Offset: 0x0011F1E0
		public override void EndCurrentEdit()
		{
			bool flag;
			base.PullData(out flag);
			if (flag)
			{
				IEditableObject editableObject = this.Current as IEditableObject;
				if (editableObject != null)
				{
					editableObject.EndEdit();
				}
			}
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x00120210 File Offset: 0x0011F210
		protected override void UpdateIsBinding()
		{
			for (int i = 0; i < base.Bindings.Count; i++)
			{
				base.Bindings[i].UpdateIsBinding();
			}
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x00120244 File Offset: 0x0011F244
		protected internal override void OnCurrentChanged(EventArgs ea)
		{
			base.PushData();
			if (this.onCurrentChangedHandler != null)
			{
				this.onCurrentChangedHandler(this, ea);
			}
			if (this.onCurrentItemChangedHandler != null)
			{
				this.onCurrentItemChangedHandler(this, ea);
			}
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x00120276 File Offset: 0x0011F276
		protected internal override void OnCurrentItemChanged(EventArgs ea)
		{
			base.PushData();
			if (this.onCurrentItemChangedHandler != null)
			{
				this.onCurrentItemChangedHandler(this, ea);
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06004E0E RID: 19982 RVA: 0x00120293 File Offset: 0x0011F293
		internal override object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06004E0F RID: 19983 RVA: 0x0012029B File Offset: 0x0011F29B
		internal override bool IsBinding
		{
			get
			{
				return this.dataSource != null;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06004E10 RID: 19984 RVA: 0x001202A9 File Offset: 0x0011F2A9
		// (set) Token: 0x06004E11 RID: 19985 RVA: 0x001202AC File Offset: 0x0011F2AC
		public override int Position
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06004E12 RID: 19986 RVA: 0x001202AE File Offset: 0x0011F2AE
		public override int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x001202B1 File Offset: 0x0011F2B1
		public override void AddNew()
		{
			throw new NotSupportedException(SR.GetString("DataBindingAddNewNotSupportedOnPropertyManager"));
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x001202C2 File Offset: 0x0011F2C2
		public override void RemoveAt(int index)
		{
			throw new NotSupportedException(SR.GetString("DataBindingRemoveAtNotSupportedOnPropertyManager"));
		}

		// Token: 0x04003294 RID: 12948
		private object dataSource;

		// Token: 0x04003295 RID: 12949
		private string propName;

		// Token: 0x04003296 RID: 12950
		private PropertyDescriptor propInfo;

		// Token: 0x04003297 RID: 12951
		private bool bound;
	}
}
