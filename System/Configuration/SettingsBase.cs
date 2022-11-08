using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x020006DE RID: 1758
	public abstract class SettingsBase
	{
		// Token: 0x06003634 RID: 13876 RVA: 0x000E7750 File Offset: 0x000E6750
		protected SettingsBase()
		{
			this._PropertyValues = new SettingsPropertyValueCollection();
		}

		// Token: 0x17000C8A RID: 3210
		public virtual object this[string propertyName]
		{
			get
			{
				if (this.IsSynchronized)
				{
					lock (this)
					{
						return this.GetPropertyValueByName(propertyName);
					}
				}
				return this.GetPropertyValueByName(propertyName);
			}
			set
			{
				if (this.IsSynchronized)
				{
					lock (this)
					{
						this.SetPropertyValueByName(propertyName, value);
						return;
					}
				}
				this.SetPropertyValueByName(propertyName, value);
			}
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000E77F4 File Offset: 0x000E67F4
		private object GetPropertyValueByName(string propertyName)
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[]
				{
					propertyName
				}));
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[]
				{
					propertyName
				}));
			}
			SettingsPropertyValue settingsPropertyValue = this._PropertyValues[propertyName];
			if (settingsPropertyValue == null)
			{
				this.GetPropertiesFromProvider(settingsProperty.Provider);
				settingsPropertyValue = this._PropertyValues[propertyName];
				if (settingsPropertyValue == null)
				{
					throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[]
					{
						propertyName
					}));
				}
			}
			return settingsPropertyValue.PropertyValue;
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000E78B8 File Offset: 0x000E68B8
		private void SetPropertyValueByName(string propertyName, object propertyValue)
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[]
				{
					propertyName
				}));
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[]
				{
					propertyName
				}));
			}
			if (settingsProperty.IsReadOnly)
			{
				throw new SettingsPropertyIsReadOnlyException(SR.GetString("SettingsPropertyReadOnly", new object[]
				{
					propertyName
				}));
			}
			if (propertyValue != null && !settingsProperty.PropertyType.IsInstanceOfType(propertyValue))
			{
				throw new SettingsPropertyWrongTypeException(SR.GetString("SettingsPropertyWrongType", new object[]
				{
					propertyName
				}));
			}
			SettingsPropertyValue settingsPropertyValue = this._PropertyValues[propertyName];
			if (settingsPropertyValue == null)
			{
				this.GetPropertiesFromProvider(settingsProperty.Provider);
				settingsPropertyValue = this._PropertyValues[propertyName];
				if (settingsPropertyValue == null)
				{
					throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[]
					{
						propertyName
					}));
				}
			}
			settingsPropertyValue.PropertyValue = propertyValue;
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000E79D3 File Offset: 0x000E69D3
		public void Initialize(SettingsContext context, SettingsPropertyCollection properties, SettingsProviderCollection providers)
		{
			this._Context = context;
			this._Properties = properties;
			this._Providers = providers;
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000E79EC File Offset: 0x000E69EC
		public virtual void Save()
		{
			if (this.IsSynchronized)
			{
				lock (this)
				{
					this.SaveCore();
					return;
				}
			}
			this.SaveCore();
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000E7A30 File Offset: 0x000E6A30
		private void SaveCore()
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				return;
			}
			foreach (object obj in this.Providers)
			{
				SettingsProvider settingsProvider = (SettingsProvider)obj;
				SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
				foreach (object obj2 in this.PropertyValues)
				{
					SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj2;
					if (settingsPropertyValue.Property.Provider == settingsProvider)
					{
						settingsPropertyValueCollection.Add(settingsPropertyValue);
					}
				}
				if (settingsPropertyValueCollection.Count > 0)
				{
					settingsProvider.SetPropertyValues(this.Context, settingsPropertyValueCollection);
				}
			}
			foreach (object obj3 in this.PropertyValues)
			{
				SettingsPropertyValue settingsPropertyValue2 = (SettingsPropertyValue)obj3;
				settingsPropertyValue2.IsDirty = false;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600363C RID: 13884 RVA: 0x000E7B74 File Offset: 0x000E6B74
		public virtual SettingsPropertyCollection Properties
		{
			get
			{
				return this._Properties;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600363D RID: 13885 RVA: 0x000E7B7C File Offset: 0x000E6B7C
		public virtual SettingsProviderCollection Providers
		{
			get
			{
				return this._Providers;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x000E7B84 File Offset: 0x000E6B84
		public virtual SettingsPropertyValueCollection PropertyValues
		{
			get
			{
				return this._PropertyValues;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600363F RID: 13887 RVA: 0x000E7B8C File Offset: 0x000E6B8C
		public virtual SettingsContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000E7B94 File Offset: 0x000E6B94
		private void GetPropertiesFromProvider(SettingsProvider provider)
		{
			SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				if (settingsProperty.Provider == provider)
				{
					settingsPropertyCollection.Add(settingsProperty);
				}
			}
			if (settingsPropertyCollection.Count > 0)
			{
				SettingsPropertyValueCollection propertyValues = provider.GetPropertyValues(this.Context, settingsPropertyCollection);
				foreach (object obj2 in propertyValues)
				{
					SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj2;
					if (this._PropertyValues[settingsPropertyValue.Name] == null)
					{
						this._PropertyValues.Add(settingsPropertyValue);
					}
				}
			}
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000E7C7C File Offset: 0x000E6C7C
		public static SettingsBase Synchronized(SettingsBase settingsBase)
		{
			settingsBase._IsSynchronized = true;
			return settingsBase;
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06003642 RID: 13890 RVA: 0x000E7C86 File Offset: 0x000E6C86
		[Browsable(false)]
		public bool IsSynchronized
		{
			get
			{
				return this._IsSynchronized;
			}
		}

		// Token: 0x0400316A RID: 12650
		private SettingsPropertyCollection _Properties;

		// Token: 0x0400316B RID: 12651
		private SettingsProviderCollection _Providers;

		// Token: 0x0400316C RID: 12652
		private SettingsPropertyValueCollection _PropertyValues;

		// Token: 0x0400316D RID: 12653
		private SettingsContext _Context;

		// Token: 0x0400316E RID: 12654
		private bool _IsSynchronized;
	}
}
