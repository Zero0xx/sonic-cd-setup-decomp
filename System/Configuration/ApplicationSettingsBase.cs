using System;
using System.Collections;
using System.ComponentModel;
using System.Deployment.Internal;
using System.Reflection;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020006DF RID: 1759
	public abstract class ApplicationSettingsBase : SettingsBase, INotifyPropertyChanged
	{
		// Token: 0x06003643 RID: 13891 RVA: 0x000E7C8E File Offset: 0x000E6C8E
		protected ApplicationSettingsBase()
		{
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000E7CA8 File Offset: 0x000E6CA8
		protected ApplicationSettingsBase(IComponent owner) : this(owner, string.Empty)
		{
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000E7CB6 File Offset: 0x000E6CB6
		protected ApplicationSettingsBase(string settingsKey)
		{
			this._settingsKey = settingsKey;
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000E7CD8 File Offset: 0x000E6CD8
		protected ApplicationSettingsBase(IComponent owner, string settingsKey) : this(settingsKey)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			if (owner.Site != null)
			{
				ISettingsProviderService settingsProviderService = owner.Site.GetService(typeof(ISettingsProviderService)) as ISettingsProviderService;
				if (settingsProviderService != null)
				{
					foreach (object obj in this.Properties)
					{
						SettingsProperty settingsProperty = (SettingsProperty)obj;
						SettingsProvider settingsProvider = settingsProviderService.GetSettingsProvider(settingsProperty);
						if (settingsProvider != null)
						{
							settingsProperty.Provider = settingsProvider;
						}
					}
					this.ResetProviders();
				}
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06003647 RID: 13895 RVA: 0x000E7D88 File Offset: 0x000E6D88
		[Browsable(false)]
		public override SettingsContext Context
		{
			get
			{
				if (this._context == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._context == null)
							{
								this._context = new SettingsContext();
								this.EnsureInitialized();
							}
							goto IL_4B;
						}
					}
					this._context = new SettingsContext();
					this.EnsureInitialized();
				}
				IL_4B:
				return this._context;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06003648 RID: 13896 RVA: 0x000E7DF8 File Offset: 0x000E6DF8
		[Browsable(false)]
		public override SettingsPropertyCollection Properties
		{
			get
			{
				if (this._settings == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._settings == null)
							{
								this._settings = new SettingsPropertyCollection();
								this.EnsureInitialized();
							}
							goto IL_4B;
						}
					}
					this._settings = new SettingsPropertyCollection();
					this.EnsureInitialized();
				}
				IL_4B:
				return this._settings;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x000E7E68 File Offset: 0x000E6E68
		[Browsable(false)]
		public override SettingsPropertyValueCollection PropertyValues
		{
			get
			{
				return base.PropertyValues;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x0600364A RID: 13898 RVA: 0x000E7E70 File Offset: 0x000E6E70
		[Browsable(false)]
		public override SettingsProviderCollection Providers
		{
			get
			{
				if (this._providers == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._providers == null)
							{
								this._providers = new SettingsProviderCollection();
								this.EnsureInitialized();
							}
							goto IL_4B;
						}
					}
					this._providers = new SettingsProviderCollection();
					this.EnsureInitialized();
				}
				IL_4B:
				return this._providers;
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x000E7EE0 File Offset: 0x000E6EE0
		// (set) Token: 0x0600364C RID: 13900 RVA: 0x000E7EE8 File Offset: 0x000E6EE8
		[Browsable(false)]
		public string SettingsKey
		{
			get
			{
				return this._settingsKey;
			}
			set
			{
				this._settingsKey = value;
				this.Context["SettingsKey"] = this._settingsKey;
			}
		}

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x0600364D RID: 13901 RVA: 0x000E7F07 File Offset: 0x000E6F07
		// (remove) Token: 0x0600364E RID: 13902 RVA: 0x000E7F20 File Offset: 0x000E6F20
		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				this._onPropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(this._onPropertyChanged, value);
			}
			remove
			{
				this._onPropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(this._onPropertyChanged, value);
			}
		}

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x0600364F RID: 13903 RVA: 0x000E7F39 File Offset: 0x000E6F39
		// (remove) Token: 0x06003650 RID: 13904 RVA: 0x000E7F52 File Offset: 0x000E6F52
		public event SettingChangingEventHandler SettingChanging
		{
			add
			{
				this._onSettingChanging = (SettingChangingEventHandler)Delegate.Combine(this._onSettingChanging, value);
			}
			remove
			{
				this._onSettingChanging = (SettingChangingEventHandler)Delegate.Remove(this._onSettingChanging, value);
			}
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06003651 RID: 13905 RVA: 0x000E7F6B File Offset: 0x000E6F6B
		// (remove) Token: 0x06003652 RID: 13906 RVA: 0x000E7F84 File Offset: 0x000E6F84
		public event SettingsLoadedEventHandler SettingsLoaded
		{
			add
			{
				this._onSettingsLoaded = (SettingsLoadedEventHandler)Delegate.Combine(this._onSettingsLoaded, value);
			}
			remove
			{
				this._onSettingsLoaded = (SettingsLoadedEventHandler)Delegate.Remove(this._onSettingsLoaded, value);
			}
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06003653 RID: 13907 RVA: 0x000E7F9D File Offset: 0x000E6F9D
		// (remove) Token: 0x06003654 RID: 13908 RVA: 0x000E7FB6 File Offset: 0x000E6FB6
		public event SettingsSavingEventHandler SettingsSaving
		{
			add
			{
				this._onSettingsSaving = (SettingsSavingEventHandler)Delegate.Combine(this._onSettingsSaving, value);
			}
			remove
			{
				this._onSettingsSaving = (SettingsSavingEventHandler)Delegate.Remove(this._onSettingsSaving, value);
			}
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000E7FD0 File Offset: 0x000E6FD0
		public object GetPreviousVersion(string propertyName)
		{
			if (this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException();
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			SettingsPropertyValue settingsPropertyValue = null;
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException();
			}
			IApplicationSettingsProvider applicationSettingsProvider = settingsProperty.Provider as IApplicationSettingsProvider;
			if (applicationSettingsProvider != null)
			{
				settingsPropertyValue = applicationSettingsProvider.GetPreviousVersion(this.Context, settingsProperty);
			}
			if (settingsPropertyValue != null)
			{
				return settingsPropertyValue.PropertyValue;
			}
			return null;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000E8030 File Offset: 0x000E7030
		protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this._onPropertyChanged != null)
			{
				this._onPropertyChanged(this, e);
			}
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000E8047 File Offset: 0x000E7047
		protected virtual void OnSettingChanging(object sender, SettingChangingEventArgs e)
		{
			if (this._onSettingChanging != null)
			{
				this._onSettingChanging(this, e);
			}
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000E805E File Offset: 0x000E705E
		protected virtual void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
		{
			if (this._onSettingsLoaded != null)
			{
				this._onSettingsLoaded(this, e);
			}
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000E8075 File Offset: 0x000E7075
		protected virtual void OnSettingsSaving(object sender, CancelEventArgs e)
		{
			if (this._onSettingsSaving != null)
			{
				this._onSettingsSaving(this, e);
			}
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000E808C File Offset: 0x000E708C
		public void Reload()
		{
			if (this.PropertyValues != null)
			{
				this.PropertyValues.Clear();
			}
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				PropertyChangedEventArgs e = new PropertyChangedEventArgs(settingsProperty.Name);
				this.OnPropertyChanged(this, e);
			}
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000E8108 File Offset: 0x000E7108
		public void Reset()
		{
			if (this.Properties != null)
			{
				foreach (object obj in this.Providers)
				{
					SettingsProvider settingsProvider = (SettingsProvider)obj;
					IApplicationSettingsProvider applicationSettingsProvider = settingsProvider as IApplicationSettingsProvider;
					if (applicationSettingsProvider != null)
					{
						applicationSettingsProvider.Reset(this.Context);
					}
				}
			}
			this.Reload();
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000E8180 File Offset: 0x000E7180
		public override void Save()
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
			this.OnSettingsSaving(this, cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				base.Save();
			}
		}

		// Token: 0x17000C95 RID: 3221
		public override object this[string propertyName]
		{
			get
			{
				if (base.IsSynchronized)
				{
					lock (this)
					{
						return this.GetPropertyValue(propertyName);
					}
				}
				return this.GetPropertyValue(propertyName);
			}
			set
			{
				SettingChangingEventArgs settingChangingEventArgs = new SettingChangingEventArgs(propertyName, base.GetType().FullName, this.SettingsKey, value, false);
				this.OnSettingChanging(this, settingChangingEventArgs);
				if (!settingChangingEventArgs.Cancel)
				{
					base[propertyName] = value;
					PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
					this.OnPropertyChanged(this, e);
				}
			}
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000E8244 File Offset: 0x000E7244
		public virtual void Upgrade()
		{
			if (this.Properties != null)
			{
				foreach (object obj in this.Providers)
				{
					SettingsProvider settingsProvider = (SettingsProvider)obj;
					IApplicationSettingsProvider applicationSettingsProvider = settingsProvider as IApplicationSettingsProvider;
					if (applicationSettingsProvider != null)
					{
						applicationSettingsProvider.Upgrade(this.Context, this.GetPropertiesForProvider(settingsProvider));
					}
				}
			}
			this.Reload();
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000E82C4 File Offset: 0x000E72C4
		private SettingsProperty CreateSetting(PropertyInfo propInfo)
		{
			object[] customAttributes = propInfo.GetCustomAttributes(false);
			SettingsProperty settingsProperty = new SettingsProperty(this.Initializer);
			bool flag = this._explicitSerializeOnClass;
			settingsProperty.Name = propInfo.Name;
			settingsProperty.PropertyType = propInfo.PropertyType;
			for (int i = 0; i < customAttributes.Length; i++)
			{
				Attribute attribute = customAttributes[i] as Attribute;
				if (attribute != null)
				{
					if (attribute is DefaultSettingValueAttribute)
					{
						settingsProperty.DefaultValue = ((DefaultSettingValueAttribute)attribute).Value;
					}
					else if (attribute is ReadOnlyAttribute)
					{
						settingsProperty.IsReadOnly = true;
					}
					else if (attribute is SettingsProviderAttribute)
					{
						string providerTypeName = ((SettingsProviderAttribute)attribute).ProviderTypeName;
						Type type = Type.GetType(providerTypeName);
						if (type == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("ProviderTypeLoadFailed", new object[]
							{
								providerTypeName
							}));
						}
						SettingsProvider settingsProvider = SecurityUtils.SecureCreateInstance(type) as SettingsProvider;
						if (settingsProvider == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("ProviderInstantiationFailed", new object[]
							{
								providerTypeName
							}));
						}
						settingsProvider.Initialize(null, null);
						settingsProvider.ApplicationName = ConfigurationManagerInternalFactory.Instance.ExeProductName;
						SettingsProvider settingsProvider2 = this._providers[settingsProvider.Name];
						if (settingsProvider2 != null)
						{
							settingsProvider = settingsProvider2;
						}
						settingsProperty.Provider = settingsProvider;
					}
					else if (attribute is SettingsSerializeAsAttribute)
					{
						settingsProperty.SerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
						flag = true;
					}
					else
					{
						settingsProperty.Attributes.Add(attribute.GetType(), attribute);
					}
				}
			}
			if (!flag)
			{
				settingsProperty.SerializeAs = this.GetSerializeAs(propInfo.PropertyType);
			}
			return settingsProperty;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000E8460 File Offset: 0x000E7460
		private void EnsureInitialized()
		{
			if (!this._initialized)
			{
				this._initialized = true;
				Type type = base.GetType();
				if (this._context == null)
				{
					this._context = new SettingsContext();
				}
				this._context["GroupName"] = type.FullName;
				this._context["SettingsKey"] = this.SettingsKey;
				this._context["SettingsClassType"] = type;
				PropertyInfo[] array = this.SettingsFilter(type.GetProperties(BindingFlags.Instance | BindingFlags.Public));
				this._classAttributes = type.GetCustomAttributes(false);
				if (this._settings == null)
				{
					this._settings = new SettingsPropertyCollection();
				}
				if (this._providers == null)
				{
					this._providers = new SettingsProviderCollection();
				}
				for (int i = 0; i < array.Length; i++)
				{
					SettingsProperty settingsProperty = this.CreateSetting(array[i]);
					if (settingsProperty != null)
					{
						this._settings.Add(settingsProperty);
						if (settingsProperty.Provider != null && this._providers[settingsProperty.Provider.Name] == null)
						{
							this._providers.Add(settingsProperty.Provider);
						}
					}
				}
			}
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x000E8570 File Offset: 0x000E7570
		private SettingsProperty Initializer
		{
			get
			{
				if (this._init == null)
				{
					this._init = new SettingsProperty("");
					this._init.DefaultValue = null;
					this._init.IsReadOnly = false;
					this._init.PropertyType = null;
					SettingsProvider settingsProvider = new LocalFileSettingsProvider();
					if (this._classAttributes != null)
					{
						for (int i = 0; i < this._classAttributes.Length; i++)
						{
							Attribute attribute = this._classAttributes[i] as Attribute;
							if (attribute != null)
							{
								if (attribute is ReadOnlyAttribute)
								{
									this._init.IsReadOnly = true;
								}
								else if (attribute is SettingsGroupNameAttribute)
								{
									if (this._context == null)
									{
										this._context = new SettingsContext();
									}
									this._context["GroupName"] = ((SettingsGroupNameAttribute)attribute).GroupName;
								}
								else if (attribute is SettingsProviderAttribute)
								{
									string providerTypeName = ((SettingsProviderAttribute)attribute).ProviderTypeName;
									Type type = Type.GetType(providerTypeName);
									if (type == null)
									{
										throw new ConfigurationErrorsException(SR.GetString("ProviderTypeLoadFailed", new object[]
										{
											providerTypeName
										}));
									}
									SettingsProvider settingsProvider2 = SecurityUtils.SecureCreateInstance(type) as SettingsProvider;
									if (settingsProvider2 == null)
									{
										throw new ConfigurationErrorsException(SR.GetString("ProviderInstantiationFailed", new object[]
										{
											providerTypeName
										}));
									}
									settingsProvider = settingsProvider2;
								}
								else if (attribute is SettingsSerializeAsAttribute)
								{
									this._init.SerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
									this._explicitSerializeOnClass = true;
								}
								else
								{
									this._init.Attributes.Add(attribute.GetType(), attribute);
								}
							}
						}
					}
					settingsProvider.Initialize(null, null);
					settingsProvider.ApplicationName = ConfigurationManagerInternalFactory.Instance.ExeProductName;
					this._init.Provider = settingsProvider;
				}
				return this._init;
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000E872C File Offset: 0x000E772C
		private SettingsPropertyCollection GetPropertiesForProvider(SettingsProvider provider)
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
			return settingsPropertyCollection;
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x000E8798 File Offset: 0x000E7798
		private object GetPropertyValue(string propertyName)
		{
			if (this.PropertyValues[propertyName] == null)
			{
				object obj = base[propertyName];
				SettingsProperty settingsProperty = this.Properties[propertyName];
				SettingsProvider provider = (settingsProperty != null) ? settingsProperty.Provider : null;
				if (this._firstLoad)
				{
					this._firstLoad = false;
					if (this.IsFirstRunOfClickOnceApp())
					{
						this.Upgrade();
					}
				}
				SettingsLoadedEventArgs e = new SettingsLoadedEventArgs(provider);
				this.OnSettingsLoaded(this, e);
				return base[propertyName];
			}
			return base[propertyName];
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000E8810 File Offset: 0x000E7810
		private SettingsSerializeAs GetSerializeAs(Type type)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			bool flag = converter.CanConvertTo(typeof(string));
			bool flag2 = converter.CanConvertFrom(typeof(string));
			if (flag && flag2)
			{
				return SettingsSerializeAs.String;
			}
			return SettingsSerializeAs.Xml;
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000E8850 File Offset: 0x000E7850
		private bool IsFirstRunOfClickOnceApp()
		{
			ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
			return ApplicationSettingsBase.IsClickOnceDeployed(AppDomain.CurrentDomain) && InternalActivationContextHelper.IsFirstRun(activationContext);
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000E887C File Offset: 0x000E787C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal static bool IsClickOnceDeployed(AppDomain appDomain)
		{
			ActivationContext activationContext = appDomain.ActivationContext;
			if (activationContext != null && activationContext.Form == ActivationContext.ContextForm.StoreBounded)
			{
				string fullName = activationContext.Identity.FullName;
				if (!string.IsNullOrEmpty(fullName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000E88B4 File Offset: 0x000E78B4
		private PropertyInfo[] SettingsFilter(PropertyInfo[] allProps)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < allProps.Length; i++)
			{
				object[] customAttributes = allProps[i].GetCustomAttributes(false);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					Attribute attribute = customAttributes[j] as Attribute;
					if (attribute is SettingAttribute)
					{
						arrayList.Add(allProps[i]);
						break;
					}
				}
			}
			return (PropertyInfo[])arrayList.ToArray(typeof(PropertyInfo));
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000E8924 File Offset: 0x000E7924
		private void ResetProviders()
		{
			this.Providers.Clear();
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				if (this.Providers[settingsProperty.Provider.Name] == null)
				{
					this.Providers.Add(settingsProperty.Provider);
				}
			}
		}

		// Token: 0x0400316F RID: 12655
		private bool _explicitSerializeOnClass;

		// Token: 0x04003170 RID: 12656
		private object[] _classAttributes;

		// Token: 0x04003171 RID: 12657
		private IComponent _owner;

		// Token: 0x04003172 RID: 12658
		private PropertyChangedEventHandler _onPropertyChanged;

		// Token: 0x04003173 RID: 12659
		private SettingsContext _context;

		// Token: 0x04003174 RID: 12660
		private SettingsProperty _init;

		// Token: 0x04003175 RID: 12661
		private SettingsPropertyCollection _settings;

		// Token: 0x04003176 RID: 12662
		private SettingsProviderCollection _providers;

		// Token: 0x04003177 RID: 12663
		private SettingChangingEventHandler _onSettingChanging;

		// Token: 0x04003178 RID: 12664
		private SettingsLoadedEventHandler _onSettingsLoaded;

		// Token: 0x04003179 RID: 12665
		private SettingsSavingEventHandler _onSettingsSaving;

		// Token: 0x0400317A RID: 12666
		private string _settingsKey = string.Empty;

		// Token: 0x0400317B RID: 12667
		private bool _firstLoad = true;

		// Token: 0x0400317C RID: 12668
		private bool _initialized;
	}
}
