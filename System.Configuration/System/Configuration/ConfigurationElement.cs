using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200000C RID: 12
	public abstract class ConfigurationElement
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000023B2 File Offset: 0x000013B2
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000023BA File Offset: 0x000013BA
		internal bool DataToWriteInternal
		{
			get
			{
				return this._bDataToWrite;
			}
			set
			{
				this._bDataToWrite = value;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023C4 File Offset: 0x000013C4
		internal ConfigurationElement CreateElement(Type type)
		{
			ConfigurationElement configurationElement = (ConfigurationElement)TypeUtil.CreateInstanceRestricted(base.GetType(), type);
			configurationElement.CallInit();
			return configurationElement;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023EA File Offset: 0x000013EA
		protected ConfigurationElement()
		{
			this._values = new ConfigurationValues();
			ConfigurationElement.ApplyValidator(this);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000240E File Offset: 0x0000140E
		protected internal virtual void Init()
		{
			this._bInited = true;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002417 File Offset: 0x00001417
		internal void CallInit()
		{
			if (!this._bInited)
			{
				this.Init();
				this._bInited = true;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000242E File Offset: 0x0000142E
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002436 File Offset: 0x00001436
		internal bool ElementPresent
		{
			get
			{
				return this._bElementPresent;
			}
			set
			{
				this._bElementPresent = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000243F File Offset: 0x0000143F
		internal string ElementTagName
		{
			get
			{
				return this._elementTagName;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002447 File Offset: 0x00001447
		internal ConfigurationLockCollection LockedAttributesList
		{
			get
			{
				return this._lockedAttributesList;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000244F File Offset: 0x0000144F
		internal ConfigurationLockCollection LockedAllExceptAttributesList
		{
			get
			{
				return this._lockedAllExceptAttributesList;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002457 File Offset: 0x00001457
		internal ConfigurationValueFlags ItemLocked
		{
			get
			{
				return this._fItemLocked;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000245F File Offset: 0x0000145F
		public ConfigurationLockCollection LockAttributes
		{
			get
			{
				if (this._lockedAttributesList == null)
				{
					this._lockedAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedAttributes);
				}
				return this._lockedAttributesList;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000247C File Offset: 0x0000147C
		internal void MergeLocks(ConfigurationElement source)
		{
			if (source != null)
			{
				this._fItemLocked = (((source._fItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default) ? (ConfigurationValueFlags.Inherited | source._fItemLocked) : this._fItemLocked);
				if (source._lockedAttributesList != null)
				{
					if (this._lockedAttributesList == null)
					{
						this._lockedAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedAttributes);
					}
					foreach (object obj in source._lockedAttributesList)
					{
						string name = (string)obj;
						this._lockedAttributesList.Add(name, ConfigurationValueFlags.Inherited);
					}
				}
				if (source._lockedAllExceptAttributesList != null)
				{
					if (this._lockedAllExceptAttributesList == null)
					{
						this._lockedAllExceptAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedExceptionList, string.Empty, source._lockedAllExceptAttributesList);
					}
					StringCollection stringCollection = this.IntersectLockCollections(this._lockedAllExceptAttributesList, source._lockedAllExceptAttributesList);
					this._lockedAllExceptAttributesList.ClearInternal(false);
					foreach (string name2 in stringCollection)
					{
						this._lockedAllExceptAttributesList.Add(name2, ConfigurationValueFlags.Default);
					}
				}
				if (source._lockedElementsList != null)
				{
					if (this._lockedElementsList == null)
					{
						this._lockedElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElements);
					}
					ConfigurationElementCollection configurationElementCollection = null;
					if (this.Properties.DefaultCollectionProperty != null)
					{
						configurationElementCollection = (this[this.Properties.DefaultCollectionProperty] as ConfigurationElementCollection);
						if (configurationElementCollection != null)
						{
							configurationElementCollection.internalElementTagName = source.ElementTagName;
							if (configurationElementCollection._lockedElementsList == null)
							{
								configurationElementCollection._lockedElementsList = this._lockedElementsList;
							}
						}
					}
					foreach (object obj2 in source._lockedElementsList)
					{
						string name3 = (string)obj2;
						this._lockedElementsList.Add(name3, ConfigurationValueFlags.Inherited);
						if (configurationElementCollection != null)
						{
							configurationElementCollection._lockedElementsList.Add(name3, ConfigurationValueFlags.Inherited);
						}
					}
				}
				if (source._lockedAllExceptElementsList != null)
				{
					if (this._lockedAllExceptElementsList == null || this._lockedAllExceptElementsList.Count == 0)
					{
						this._lockedAllExceptElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElementsExceptionList, source._elementTagName, source._lockedAllExceptElementsList);
					}
					StringCollection stringCollection2 = this.IntersectLockCollections(this._lockedAllExceptElementsList, source._lockedAllExceptElementsList);
					if (this.Properties.DefaultCollectionProperty != null)
					{
						ConfigurationElementCollection configurationElementCollection2 = this[this.Properties.DefaultCollectionProperty] as ConfigurationElementCollection;
						if (configurationElementCollection2 != null && configurationElementCollection2._lockedAllExceptElementsList == null)
						{
							configurationElementCollection2._lockedAllExceptElementsList = this._lockedAllExceptElementsList;
						}
					}
					this._lockedAllExceptElementsList.ClearInternal(false);
					foreach (string text in stringCollection2)
					{
						if (!this._lockedAllExceptElementsList.Contains(text) || text == this.ElementTagName)
						{
							this._lockedAllExceptElementsList.Add(text, ConfigurationValueFlags.Default);
						}
					}
					if (this._lockedAllExceptElementsList.HasParentElements)
					{
						foreach (object obj3 in this.Properties)
						{
							ConfigurationProperty configurationProperty = (ConfigurationProperty)obj3;
							if (!this._lockedAllExceptElementsList.Contains(configurationProperty.Name) && typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
							{
								((ConfigurationElement)this[configurationProperty]).SetLocked();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002824 File Offset: 0x00001824
		internal void HandleLockedAttributes(ConfigurationElement source)
		{
			if (source != null && (source._lockedAttributesList != null || source._lockedAllExceptAttributesList != null))
			{
				foreach (object obj in source.ElementInformation.Properties)
				{
					PropertyInformation propertyInformation = (PropertyInformation)obj;
					if (((source._lockedAttributesList != null && (source._lockedAttributesList.Contains(propertyInformation.Name) || source._lockedAttributesList.Contains("*"))) || (source._lockedAllExceptAttributesList != null && !source._lockedAllExceptAttributesList.Contains(propertyInformation.Name))) && propertyInformation.Name != "lockAttributes" && propertyInformation.Name != "lockAllAttributesExcept")
					{
						if (this.ElementInformation.Properties[propertyInformation.Name] == null)
						{
							ConfigurationPropertyCollection properties = this.Properties;
							ConfigurationProperty property = source.Properties[propertyInformation.Name];
							properties.Add(property);
							this._evaluationElement = null;
							ConfigurationValueFlags valueFlags = ConfigurationValueFlags.Inherited | ConfigurationValueFlags.Locked;
							this._values.SetValue(propertyInformation.Name, propertyInformation.Value, valueFlags, source.PropertyInfoInternal(propertyInformation.Name));
						}
						else
						{
							if (this.ElementInformation.Properties[propertyInformation.Name].ValueOrigin == PropertyValueOrigin.SetHere)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[]
								{
									propertyInformation.Name
								}));
							}
							this.ElementInformation.Properties[propertyInformation.Name].Value = propertyInformation.Value;
						}
					}
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000029F0 File Offset: 0x000019F0
		internal virtual void AssociateContext(BaseConfigurationRecord configRecord)
		{
			this._configRecord = configRecord;
			this.Values.AssociateContext(configRecord);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002A05 File Offset: 0x00001A05
		public ConfigurationLockCollection LockAllAttributesExcept
		{
			get
			{
				if (this._lockedAllExceptAttributesList == null)
				{
					this._lockedAllExceptAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedExceptionList, this._elementTagName);
				}
				return this._lockedAllExceptAttributesList;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002A28 File Offset: 0x00001A28
		public ConfigurationLockCollection LockElements
		{
			get
			{
				if (this._lockedElementsList == null)
				{
					this._lockedElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElements);
				}
				return this._lockedElementsList;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002A45 File Offset: 0x00001A45
		public ConfigurationLockCollection LockAllElementsExcept
		{
			get
			{
				if (this._lockedAllExceptElementsList == null)
				{
					this._lockedAllExceptElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElementsExceptionList, this._elementTagName);
				}
				return this._lockedAllExceptElementsList;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002A68 File Offset: 0x00001A68
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002A78 File Offset: 0x00001A78
		public bool LockItem
		{
			get
			{
				return (this._fItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default;
			}
			set
			{
				if ((this._fItemLocked & ConfigurationValueFlags.Inherited) == ConfigurationValueFlags.Default)
				{
					this._fItemLocked = (value ? ConfigurationValueFlags.Locked : ConfigurationValueFlags.Default);
					this._fItemLocked |= ConfigurationValueFlags.Modified;
					return;
				}
				throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[]
				{
					"lockItem"
				}));
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002ACC File Offset: 0x00001ACC
		protected internal virtual bool IsModified()
		{
			if (this._bModified)
			{
				return true;
			}
			if (this._lockedAttributesList != null && this._lockedAttributesList.IsModified)
			{
				return true;
			}
			if (this._lockedAllExceptAttributesList != null && this._lockedAllExceptAttributesList.IsModified)
			{
				return true;
			}
			if (this._lockedElementsList != null && this._lockedElementsList.IsModified)
			{
				return true;
			}
			if (this._lockedAllExceptElementsList != null && this._lockedAllExceptElementsList.IsModified)
			{
				return true;
			}
			if ((this._fItemLocked & ConfigurationValueFlags.Modified) != ConfigurationValueFlags.Default)
			{
				return true;
			}
			foreach (object obj in this._values.ConfigurationElements)
			{
				ConfigurationElement configurationElement = (ConfigurationElement)obj;
				if (configurationElement.IsModified())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002BA4 File Offset: 0x00001BA4
		protected internal virtual void ResetModified()
		{
			this._bModified = false;
			if (this._lockedAttributesList != null)
			{
				this._lockedAttributesList.ResetModified();
			}
			if (this._lockedAllExceptAttributesList != null)
			{
				this._lockedAllExceptAttributesList.ResetModified();
			}
			if (this._lockedElementsList != null)
			{
				this._lockedElementsList.ResetModified();
			}
			if (this._lockedAllExceptElementsList != null)
			{
				this._lockedAllExceptElementsList.ResetModified();
			}
			foreach (object obj in this._values.ConfigurationElements)
			{
				ConfigurationElement configurationElement = (ConfigurationElement)obj;
				configurationElement.ResetModified();
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002C54 File Offset: 0x00001C54
		public virtual bool IsReadOnly()
		{
			return this._bReadOnly;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002C5C File Offset: 0x00001C5C
		protected internal virtual void SetReadOnly()
		{
			this._bReadOnly = true;
			foreach (object obj in this._values.ConfigurationElements)
			{
				ConfigurationElement configurationElement = (ConfigurationElement)obj;
				configurationElement.SetReadOnly();
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002CC0 File Offset: 0x00001CC0
		internal void SetLocked()
		{
			this._fItemLocked = (ConfigurationValueFlags.Locked | ConfigurationValueFlags.XMLParentInherited);
			foreach (object obj in this.Properties)
			{
				ConfigurationProperty prop = (ConfigurationProperty)obj;
				ConfigurationElement configurationElement = this[prop] as ConfigurationElement;
				if (configurationElement != null)
				{
					if (configurationElement.GetType() != base.GetType())
					{
						configurationElement.SetLocked();
					}
					ConfigurationElementCollection configurationElementCollection = this[prop] as ConfigurationElementCollection;
					if (configurationElementCollection != null)
					{
						foreach (object obj2 in configurationElementCollection)
						{
							ConfigurationElement configurationElement2 = obj2 as ConfigurationElement;
							if (configurationElement2 != null)
							{
								configurationElement2.SetLocked();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002DAC File Offset: 0x00001DAC
		internal ArrayList GetErrorsList()
		{
			ArrayList arrayList = new ArrayList();
			this.ListErrors(arrayList);
			return arrayList;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002DC8 File Offset: 0x00001DC8
		internal ConfigurationErrorsException GetErrors()
		{
			ArrayList errorsList = this.GetErrorsList();
			if (errorsList.Count == 0)
			{
				return null;
			}
			return new ConfigurationErrorsException(errorsList);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002DF0 File Offset: 0x00001DF0
		protected virtual void ListErrors(IList errorList)
		{
			foreach (object obj in this._values.InvalidValues)
			{
				InvalidPropValue invalidPropValue = (InvalidPropValue)obj;
				errorList.Add(invalidPropValue.Error);
			}
			foreach (object obj2 in this._values.ConfigurationElements)
			{
				ConfigurationElement configurationElement = (ConfigurationElement)obj2;
				configurationElement.ListErrors(errorList);
				ConfigurationElementCollection configurationElementCollection = configurationElement as ConfigurationElementCollection;
				if (configurationElementCollection != null)
				{
					foreach (object obj3 in configurationElementCollection)
					{
						ConfigurationElement configurationElement2 = (ConfigurationElement)obj3;
						configurationElement2.ListErrors(errorList);
					}
				}
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002F04 File Offset: 0x00001F04
		protected internal virtual void InitializeDefault()
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002F08 File Offset: 0x00001F08
		internal void CheckLockedElement(string elementName, XmlReader reader)
		{
			if (elementName != null && ((this._lockedElementsList != null && (this._lockedElementsList.DefinedInParent("*") || this._lockedElementsList.DefinedInParent(elementName))) || (this._lockedAllExceptElementsList != null && this._lockedAllExceptElementsList.Count != 0 && this._lockedAllExceptElementsList.HasParentElements && !this._lockedAllExceptElementsList.DefinedInParent(elementName)) || (this._fItemLocked & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_element_locked", new object[]
				{
					elementName
				}), reader);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002F98 File Offset: 0x00001F98
		internal void RemoveAllInheritedLocks()
		{
			if (this._lockedAttributesList != null)
			{
				this._lockedAttributesList.RemoveInheritedLocks();
			}
			if (this._lockedElementsList != null)
			{
				this._lockedElementsList.RemoveInheritedLocks();
			}
			if (this._lockedAllExceptAttributesList != null)
			{
				this._lockedAllExceptAttributesList.RemoveInheritedLocks();
			}
			if (this._lockedAllExceptElementsList != null)
			{
				this._lockedAllExceptElementsList.RemoveInheritedLocks();
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002FF4 File Offset: 0x00001FF4
		internal void ResetLockLists(ConfigurationElement parentElement)
		{
			this._lockedAttributesList = null;
			this._lockedAllExceptAttributesList = null;
			this._lockedElementsList = null;
			this._lockedAllExceptElementsList = null;
			if (parentElement != null)
			{
				this._fItemLocked = (((parentElement._fItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default) ? (ConfigurationValueFlags.Inherited | parentElement._fItemLocked) : ConfigurationValueFlags.Default);
				if (parentElement._lockedAttributesList != null)
				{
					this._lockedAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedAttributes);
					foreach (object obj in parentElement._lockedAttributesList)
					{
						string name = (string)obj;
						this._lockedAttributesList.Add(name, ConfigurationValueFlags.Inherited);
					}
				}
				if (parentElement._lockedAllExceptAttributesList != null)
				{
					this._lockedAllExceptAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedExceptionList, string.Empty, parentElement._lockedAllExceptAttributesList);
				}
				if (parentElement._lockedElementsList != null)
				{
					this._lockedElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElements);
					if (this.Properties.DefaultCollectionProperty != null)
					{
						ConfigurationElementCollection configurationElementCollection = this[this.Properties.DefaultCollectionProperty] as ConfigurationElementCollection;
						if (configurationElementCollection != null)
						{
							configurationElementCollection.internalElementTagName = parentElement.ElementTagName;
							if (configurationElementCollection._lockedElementsList == null)
							{
								configurationElementCollection._lockedElementsList = this._lockedElementsList;
							}
						}
					}
					foreach (object obj2 in parentElement._lockedElementsList)
					{
						string name2 = (string)obj2;
						this._lockedElementsList.Add(name2, ConfigurationValueFlags.Inherited);
					}
				}
				if (parentElement._lockedAllExceptElementsList != null)
				{
					this._lockedAllExceptElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElementsExceptionList, parentElement._elementTagName, parentElement._lockedAllExceptElementsList);
					if (this.Properties.DefaultCollectionProperty != null)
					{
						ConfigurationElementCollection configurationElementCollection2 = this[this.Properties.DefaultCollectionProperty] as ConfigurationElementCollection;
						if (configurationElementCollection2 != null && configurationElementCollection2._lockedAllExceptElementsList == null)
						{
							configurationElementCollection2._lockedAllExceptElementsList = this._lockedAllExceptElementsList;
						}
					}
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000031E0 File Offset: 0x000021E0
		protected internal virtual void Reset(ConfigurationElement parentElement)
		{
			this.Values.Clear();
			this.ResetLockLists(parentElement);
			ConfigurationPropertyCollection properties = this.Properties;
			this._bElementPresent = false;
			if (parentElement == null)
			{
				this.InitializeDefault();
				return;
			}
			bool flag = false;
			ConfigurationPropertyCollection configurationPropertyCollection = null;
			for (int i = 0; i < parentElement.Values.Count; i++)
			{
				string key = parentElement.Values.GetKey(i);
				ConfigurationValue configValue = parentElement.Values.GetConfigValue(i);
				object obj = (configValue != null) ? configValue.Value : null;
				PropertySourceInfo sourceInfo = (configValue != null) ? configValue.SourceInfo : null;
				ConfigurationProperty configurationProperty = parentElement.Properties[key];
				if (configurationProperty != null && (configurationPropertyCollection == null || configurationPropertyCollection.Contains(configurationProperty.Name)))
				{
					if (typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
					{
						flag = true;
					}
					else
					{
						ConfigurationValueFlags valueFlags = ConfigurationValueFlags.Inherited | (((this._lockedAttributesList != null && (this._lockedAttributesList.Contains(key) || this._lockedAttributesList.Contains("*"))) || (this._lockedAllExceptAttributesList != null && !this._lockedAllExceptAttributesList.Contains(key))) ? ConfigurationValueFlags.Locked : ConfigurationValueFlags.Default);
						if (obj != ConfigurationElement.s_nullPropertyValue)
						{
							this._values.SetValue(key, obj, valueFlags, sourceInfo);
						}
						if (!properties.Contains(key))
						{
							properties.Add(configurationProperty);
							this._values.SetValue(key, obj, valueFlags, sourceInfo);
						}
					}
				}
			}
			if (flag)
			{
				for (int j = 0; j < parentElement.Values.Count; j++)
				{
					string key2 = parentElement.Values.GetKey(j);
					object obj2 = parentElement.Values[j];
					ConfigurationProperty configurationProperty2 = parentElement.Properties[key2];
					if (configurationProperty2 != null && typeof(ConfigurationElement).IsAssignableFrom(configurationProperty2.Type))
					{
						ConfigurationElement configurationElement = (ConfigurationElement)this[configurationProperty2];
						configurationElement.Reset((ConfigurationElement)obj2);
					}
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000033CC File Offset: 0x000023CC
		public override bool Equals(object compareTo)
		{
			ConfigurationElement configurationElement = compareTo as ConfigurationElement;
			if (configurationElement == null || compareTo.GetType() != base.GetType() || (configurationElement != null && configurationElement.Properties.Count != this.Properties.Count))
			{
				return false;
			}
			foreach (object obj in this.Properties)
			{
				ConfigurationProperty configurationProperty = (ConfigurationProperty)obj;
				if (!object.Equals(this.Values[configurationProperty.Name], configurationElement.Values[configurationProperty.Name]) && ((this.Values[configurationProperty.Name] != null && this.Values[configurationProperty.Name] != ConfigurationElement.s_nullPropertyValue) || !object.Equals(configurationElement.Values[configurationProperty.Name], configurationProperty.DefaultValue)) && ((configurationElement.Values[configurationProperty.Name] != null && configurationElement.Values[configurationProperty.Name] != ConfigurationElement.s_nullPropertyValue) || !object.Equals(this.Values[configurationProperty.Name], configurationProperty.DefaultValue)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003524 File Offset: 0x00002524
		public override int GetHashCode()
		{
			int num = 0;
			foreach (object obj in this.Properties)
			{
				ConfigurationProperty prop = (ConfigurationProperty)obj;
				object obj2 = this[prop];
				if (obj2 != null)
				{
					num ^= this[prop].GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x17000011 RID: 17
		protected internal object this[ConfigurationProperty prop]
		{
			get
			{
				object obj = this._values[prop.Name];
				if (obj == null)
				{
					lock (this._values.SyncRoot)
					{
						obj = this._values[prop.Name];
						if (obj == null)
						{
							if (typeof(ConfigurationElement).IsAssignableFrom(prop.Type))
							{
								ConfigurationElement configurationElement = this.CreateElement(prop.Type);
								if (this._bReadOnly)
								{
									configurationElement.SetReadOnly();
								}
								if (typeof(ConfigurationElementCollection).IsAssignableFrom(prop.Type))
								{
									ConfigurationElementCollection configurationElementCollection = configurationElement as ConfigurationElementCollection;
									if (prop.AddElementName != null)
									{
										configurationElementCollection.AddElementName = prop.AddElementName;
									}
									if (prop.RemoveElementName != null)
									{
										configurationElementCollection.RemoveElementName = prop.RemoveElementName;
									}
									if (prop.ClearElementName != null)
									{
										configurationElementCollection.ClearElementName = prop.ClearElementName;
									}
								}
								this._values.SetValue(prop.Name, configurationElement, ConfigurationValueFlags.Inherited, null);
								obj = configurationElement;
							}
							else
							{
								obj = prop.DefaultValue;
							}
						}
						goto IL_103;
					}
				}
				if (obj == ConfigurationElement.s_nullPropertyValue)
				{
					obj = null;
				}
				IL_103:
				if (obj is InvalidPropValue)
				{
					throw ((InvalidPropValue)obj).Error;
				}
				return obj;
			}
			set
			{
				this.SetPropertyValue(prop, value, false);
			}
		}

		// Token: 0x17000012 RID: 18
		protected internal object this[string propertyName]
		{
			get
			{
				ConfigurationProperty configurationProperty = this.Properties[propertyName];
				if (configurationProperty == null)
				{
					configurationProperty = this.Properties[""];
					if (configurationProperty.ProvidedName != propertyName)
					{
						return null;
					}
				}
				return this[configurationProperty];
			}
			set
			{
				this.SetPropertyValue(this.Properties[propertyName], value, false);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003738 File Offset: 0x00002738
		private static void ApplyInstanceAttributes(object instance)
		{
			Type type = instance.GetType();
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				ConfigurationPropertyAttribute configurationPropertyAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ConfigurationPropertyAttribute)) as ConfigurationPropertyAttribute;
				if (configurationPropertyAttribute != null)
				{
					Type propertyType = propertyInfo.PropertyType;
					if (typeof(ConfigurationElementCollection).IsAssignableFrom(propertyType))
					{
						ConfigurationCollectionAttribute configurationCollectionAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ConfigurationCollectionAttribute)) as ConfigurationCollectionAttribute;
						if (configurationCollectionAttribute == null)
						{
							configurationCollectionAttribute = (Attribute.GetCustomAttribute(propertyType, typeof(ConfigurationCollectionAttribute)) as ConfigurationCollectionAttribute);
						}
						ConfigurationElementCollection configurationElementCollection = propertyInfo.GetValue(instance, null) as ConfigurationElementCollection;
						if (configurationElementCollection == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_element_null_instance", new object[]
							{
								propertyInfo.Name,
								configurationPropertyAttribute.Name
							}));
						}
						if (configurationCollectionAttribute != null)
						{
							if (configurationCollectionAttribute.AddItemName.IndexOf(',') == -1)
							{
								configurationElementCollection.AddElementName = configurationCollectionAttribute.AddItemName;
							}
							configurationElementCollection.RemoveElementName = configurationCollectionAttribute.RemoveItemName;
							configurationElementCollection.ClearElementName = configurationCollectionAttribute.ClearItemsName;
						}
					}
					else if (typeof(ConfigurationElement).IsAssignableFrom(propertyType))
					{
						object value = propertyInfo.GetValue(instance, null);
						if (value == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_element_null_instance", new object[]
							{
								propertyInfo.Name,
								configurationPropertyAttribute.Name
							}));
						}
						ConfigurationElement.ApplyInstanceAttributes(value);
					}
				}
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000038BC File Offset: 0x000028BC
		private static bool PropertiesFromType(Type type, out ConfigurationPropertyCollection result)
		{
			ConfigurationPropertyCollection configurationPropertyCollection = (ConfigurationPropertyCollection)ConfigurationElement.s_propertyBags[type];
			result = null;
			bool result2 = false;
			if (configurationPropertyCollection == null)
			{
				lock (ConfigurationElement.s_propertyBags.SyncRoot)
				{
					configurationPropertyCollection = (ConfigurationPropertyCollection)ConfigurationElement.s_propertyBags[type];
					if (configurationPropertyCollection == null)
					{
						configurationPropertyCollection = ConfigurationElement.CreatePropertyBagFromType(type);
						ConfigurationElement.s_propertyBags[type] = configurationPropertyCollection;
						result2 = true;
					}
				}
			}
			result = configurationPropertyCollection;
			return result2;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000393C File Offset: 0x0000293C
		private static ConfigurationPropertyCollection CreatePropertyBagFromType(Type type)
		{
			if (typeof(ConfigurationElement).IsAssignableFrom(type))
			{
				ConfigurationValidatorAttribute configurationValidatorAttribute = Attribute.GetCustomAttribute(type, typeof(ConfigurationValidatorAttribute)) as ConfigurationValidatorAttribute;
				if (configurationValidatorAttribute != null)
				{
					configurationValidatorAttribute.SetDeclaringType(type);
					ConfigurationValidatorBase validatorInstance = configurationValidatorAttribute.ValidatorInstance;
					if (validatorInstance != null)
					{
						ConfigurationElement.CachePerTypeValidator(type, validatorInstance);
					}
				}
			}
			ConfigurationPropertyCollection configurationPropertyCollection = new ConfigurationPropertyCollection();
			foreach (PropertyInfo propertyInformation in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				ConfigurationProperty configurationProperty = ConfigurationElement.CreateConfigurationPropertyFromAttributes(propertyInformation);
				if (configurationProperty != null)
				{
					configurationPropertyCollection.Add(configurationProperty);
				}
			}
			return configurationPropertyCollection;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000039CC File Offset: 0x000029CC
		private static ConfigurationProperty CreateConfigurationPropertyFromAttributes(PropertyInfo propertyInformation)
		{
			ConfigurationProperty configurationProperty = null;
			ConfigurationPropertyAttribute configurationPropertyAttribute = Attribute.GetCustomAttribute(propertyInformation, typeof(ConfigurationPropertyAttribute)) as ConfigurationPropertyAttribute;
			if (configurationPropertyAttribute != null)
			{
				configurationProperty = new ConfigurationProperty(propertyInformation);
			}
			if (configurationProperty != null && typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
			{
				ConfigurationPropertyCollection configurationPropertyCollection = null;
				ConfigurationElement.PropertiesFromType(configurationProperty.Type, out configurationPropertyCollection);
			}
			return configurationProperty;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003A28 File Offset: 0x00002A28
		private static void CachePerTypeValidator(Type type, ConfigurationValidatorBase validator)
		{
			if (ConfigurationElement.s_perTypeValidators == null)
			{
				ConfigurationElement.s_perTypeValidators = new Dictionary<Type, ConfigurationValidatorBase>();
			}
			if (!validator.CanValidate(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Validator_does_not_support_elem_type", new object[]
				{
					type.Name
				}));
			}
			ConfigurationElement.s_perTypeValidators.Add(type, validator);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003A7C File Offset: 0x00002A7C
		private static void ApplyValidatorsRecursive(ConfigurationElement root)
		{
			ConfigurationElement.ApplyValidator(root);
			foreach (object obj in root._values.ConfigurationElements)
			{
				ConfigurationElement root2 = (ConfigurationElement)obj;
				ConfigurationElement.ApplyValidatorsRecursive(root2);
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003AE0 File Offset: 0x00002AE0
		private static void ApplyValidator(ConfigurationElement elem)
		{
			if (ConfigurationElement.s_perTypeValidators != null && ConfigurationElement.s_perTypeValidators.ContainsKey(elem.GetType()))
			{
				elem._elementProperty = new ConfigurationElementProperty(ConfigurationElement.s_perTypeValidators[elem.GetType()]);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003B18 File Offset: 0x00002B18
		protected void SetPropertyValue(ConfigurationProperty prop, object value, bool ignoreLocks)
		{
			if (this.IsReadOnly())
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
			if (!ignoreLocks && ((this._lockedAllExceptAttributesList != null && this._lockedAllExceptAttributesList.HasParentElements && !this._lockedAllExceptAttributesList.DefinedInParent(prop.Name)) || (this._lockedAttributesList != null && (this._lockedAttributesList.DefinedInParent(prop.Name) || this._lockedAttributesList.DefinedInParent("*"))) || ((this._fItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default && (this._fItemLocked & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default)))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[]
				{
					prop.Name
				}));
			}
			this._bModified = true;
			if (value != null)
			{
				prop.Validate(value);
			}
			this._values[prop.Name] = ((value != null) ? value : ConfigurationElement.s_nullPropertyValue);
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003BFC File Offset: 0x00002BFC
		protected internal virtual ConfigurationPropertyCollection Properties
		{
			get
			{
				ConfigurationPropertyCollection result = null;
				if (ConfigurationElement.PropertiesFromType(base.GetType(), out result))
				{
					ConfigurationElement.ApplyInstanceAttributes(this);
					ConfigurationElement.ApplyValidatorsRecursive(this);
				}
				return result;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003C27 File Offset: 0x00002C27
		internal ConfigurationValues Values
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003C2F File Offset: 0x00002C2F
		internal PropertySourceInfo PropertyInfoInternal(string propertyName)
		{
			return this._values.GetSourceInfo(propertyName);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003C40 File Offset: 0x00002C40
		internal string PropertyFileName(string propertyName)
		{
			PropertySourceInfo propertySourceInfo = this.PropertyInfoInternal(propertyName);
			if (propertySourceInfo == null)
			{
				propertySourceInfo = this.PropertyInfoInternal(string.Empty);
			}
			if (propertySourceInfo == null)
			{
				return string.Empty;
			}
			return propertySourceInfo.FileName;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003C74 File Offset: 0x00002C74
		internal int PropertyLineNumber(string propertyName)
		{
			PropertySourceInfo propertySourceInfo = this.PropertyInfoInternal(propertyName);
			if (propertySourceInfo == null)
			{
				propertySourceInfo = this.PropertyInfoInternal(string.Empty);
			}
			if (propertySourceInfo == null)
			{
				return 0;
			}
			return propertySourceInfo.LineNumber;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003CA4 File Offset: 0x00002CA4
		internal virtual void Dump(TextWriter tw)
		{
			tw.WriteLine("Type: " + base.GetType().FullName);
			foreach (PropertyInfo propertyInfo in base.GetType().GetProperties())
			{
				tw.WriteLine("{0}: {1}", propertyInfo.Name, propertyInfo.GetValue(this, null));
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003D04 File Offset: 0x00002D04
		protected internal virtual void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			if (sourceElement != null)
			{
				bool flag = false;
				this._lockedAllExceptAttributesList = sourceElement._lockedAllExceptAttributesList;
				this._lockedAllExceptElementsList = sourceElement._lockedAllExceptElementsList;
				this._fItemLocked = sourceElement._fItemLocked;
				this._lockedAttributesList = sourceElement._lockedAttributesList;
				this._lockedElementsList = sourceElement._lockedElementsList;
				this.AssociateContext(sourceElement._configRecord);
				if (parentElement != null)
				{
					if (parentElement._lockedAttributesList != null)
					{
						this._lockedAttributesList = this.UnMergeLockList(sourceElement._lockedAttributesList, parentElement._lockedAttributesList, saveMode);
					}
					if (parentElement._lockedElementsList != null)
					{
						this._lockedElementsList = this.UnMergeLockList(sourceElement._lockedElementsList, parentElement._lockedElementsList, saveMode);
					}
					if (parentElement._lockedAllExceptAttributesList != null)
					{
						this._lockedAllExceptAttributesList = this.UnMergeLockList(sourceElement._lockedAllExceptAttributesList, parentElement._lockedAllExceptAttributesList, saveMode);
					}
					if (parentElement._lockedAllExceptElementsList != null)
					{
						this._lockedAllExceptElementsList = this.UnMergeLockList(sourceElement._lockedAllExceptElementsList, parentElement._lockedAllExceptElementsList, saveMode);
					}
				}
				ConfigurationPropertyCollection properties = this.Properties;
				ConfigurationPropertyCollection configurationPropertyCollection = null;
				for (int i = 0; i < sourceElement.Values.Count; i++)
				{
					string key = sourceElement.Values.GetKey(i);
					object obj = sourceElement.Values[i];
					ConfigurationProperty configurationProperty = sourceElement.Properties[key];
					if (configurationProperty != null && (configurationPropertyCollection == null || configurationPropertyCollection.Contains(configurationProperty.Name)))
					{
						if (typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
						{
							flag = true;
						}
						else if (obj != ConfigurationElement.s_nullPropertyValue && !properties.Contains(key))
						{
							ConfigurationValueFlags valueFlags = sourceElement.Values.RetrieveFlags(key);
							this._values.SetValue(key, obj, valueFlags, null);
							properties.Add(configurationProperty);
						}
					}
				}
				foreach (object obj2 in this.Properties)
				{
					ConfigurationProperty configurationProperty2 = (ConfigurationProperty)obj2;
					if (configurationProperty2 != null && (configurationPropertyCollection == null || configurationPropertyCollection.Contains(configurationProperty2.Name)))
					{
						if (typeof(ConfigurationElement).IsAssignableFrom(configurationProperty2.Type))
						{
							flag = true;
						}
						else
						{
							object obj3 = sourceElement.Values[configurationProperty2.Name];
							if ((configurationProperty2.IsRequired || saveMode == ConfigurationSaveMode.Full) && (obj3 == null || obj3 == ConfigurationElement.s_nullPropertyValue) && configurationProperty2.DefaultValue != null)
							{
								obj3 = configurationProperty2.DefaultValue;
							}
							if (obj3 != null && obj3 != ConfigurationElement.s_nullPropertyValue)
							{
								object obj4 = null;
								if (parentElement != null)
								{
									obj4 = parentElement.Values[configurationProperty2.Name];
								}
								if (obj4 == null)
								{
									obj4 = configurationProperty2.DefaultValue;
								}
								switch (saveMode)
								{
								case ConfigurationSaveMode.Modified:
								{
									bool flag2 = sourceElement.Values.IsModified(configurationProperty2.Name);
									bool flag3 = sourceElement.Values.IsInherited(configurationProperty2.Name);
									if (configurationProperty2.IsRequired || flag2 || !flag3 || (parentElement == null && flag3 && !object.Equals(obj3, obj4)))
									{
										this._values[configurationProperty2.Name] = obj3;
									}
									break;
								}
								case ConfigurationSaveMode.Minimal:
									if (!object.Equals(obj3, obj4) || configurationProperty2.IsRequired)
									{
										this._values[configurationProperty2.Name] = obj3;
									}
									break;
								case ConfigurationSaveMode.Full:
									if (obj3 != null && obj3 != ConfigurationElement.s_nullPropertyValue)
									{
										this._values[configurationProperty2.Name] = obj3;
									}
									else
									{
										this._values[configurationProperty2.Name] = obj4;
									}
									break;
								}
							}
						}
					}
				}
				if (flag)
				{
					foreach (object obj5 in this.Properties)
					{
						ConfigurationProperty configurationProperty3 = (ConfigurationProperty)obj5;
						if (typeof(ConfigurationElement).IsAssignableFrom(configurationProperty3.Type))
						{
							ConfigurationElement parentElement2 = (ConfigurationElement)((parentElement != null) ? parentElement[configurationProperty3] : null);
							ConfigurationElement configurationElement = (ConfigurationElement)this[configurationProperty3];
							if ((ConfigurationElement)sourceElement[configurationProperty3] != null)
							{
								configurationElement.Unmerge((ConfigurationElement)sourceElement[configurationProperty3], parentElement2, saveMode);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004160 File Offset: 0x00003160
		protected internal virtual bool SerializeToXmlElement(XmlWriter writer, string elementName)
		{
			bool flag = this._bDataToWrite;
			if ((this._lockedElementsList != null && this._lockedElementsList.DefinedInParent(elementName)) || (this._lockedAllExceptElementsList != null && this._lockedAllExceptElementsList.HasParentElements && !this._lockedAllExceptElementsList.DefinedInParent(elementName)))
			{
				return flag;
			}
			if (this.SerializeElement(null, false))
			{
				if (writer != null)
				{
					writer.WriteStartElement(elementName);
				}
				flag |= this.SerializeElement(writer, false);
				if (writer != null)
				{
					writer.WriteEndElement();
				}
			}
			return flag;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000041D8 File Offset: 0x000031D8
		protected internal virtual bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			this.PreSerialize(writer);
			bool flag = this._bDataToWrite;
			bool flag2 = false;
			bool flag3 = false;
			ConfigurationPropertyCollection properties = this.Properties;
			ConfigurationPropertyCollection configurationPropertyCollection = null;
			for (int i = 0; i < this._values.Count; i++)
			{
				string key = this._values.GetKey(i);
				object obj = this._values[i];
				ConfigurationProperty configurationProperty = properties[key];
				if (configurationProperty != null && (configurationPropertyCollection == null || configurationPropertyCollection.Contains(configurationProperty.Name)))
				{
					if (typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
					{
						flag2 = true;
					}
					else
					{
						if ((this._lockedAllExceptAttributesList != null && this._lockedAllExceptAttributesList.HasParentElements && !this._lockedAllExceptAttributesList.DefinedInParent(configurationProperty.Name)) || (this._lockedAttributesList != null && this._lockedAttributesList.DefinedInParent(configurationProperty.Name)))
						{
							if (configurationProperty.IsRequired)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_locked", new object[]
								{
									configurationProperty.Name
								}));
							}
							obj = ConfigurationElement.s_nullPropertyValue;
						}
						if (obj != ConfigurationElement.s_nullPropertyValue && (!serializeCollectionKey || configurationProperty.IsKey))
						{
							string text;
							if (obj is InvalidPropValue)
							{
								text = ((InvalidPropValue)obj).Value;
							}
							else
							{
								configurationProperty.Validate(obj);
								text = configurationProperty.ConvertToString(obj);
							}
							if (text != null && writer != null)
							{
								writer.WriteAttributeString(configurationProperty.Name, text);
							}
							flag = (flag || text != null);
						}
					}
				}
			}
			if (!serializeCollectionKey)
			{
				flag |= this.SerializeLockList(this._lockedAttributesList, "lockAttributes", writer);
				flag |= this.SerializeLockList(this._lockedAllExceptAttributesList, "lockAllAttributesExcept", writer);
				flag |= this.SerializeLockList(this._lockedElementsList, "lockElements", writer);
				flag |= this.SerializeLockList(this._lockedAllExceptElementsList, "lockAllElementsExcept", writer);
				if ((this._fItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default && (this._fItemLocked & ConfigurationValueFlags.Inherited) == ConfigurationValueFlags.Default && (this._fItemLocked & ConfigurationValueFlags.XMLParentInherited) == ConfigurationValueFlags.Default)
				{
					flag = true;
					if (writer != null)
					{
						writer.WriteAttributeString("lockItem", true.ToString().ToLower(CultureInfo.InvariantCulture));
					}
				}
			}
			if (flag2)
			{
				for (int j = 0; j < this._values.Count; j++)
				{
					string key2 = this._values.GetKey(j);
					object obj2 = this._values[j];
					ConfigurationProperty configurationProperty2 = properties[key2];
					if ((!serializeCollectionKey || configurationProperty2.IsKey) && obj2 is ConfigurationElement && (this._lockedElementsList == null || !this._lockedElementsList.DefinedInParent(key2)) && (this._lockedAllExceptElementsList == null || !this._lockedAllExceptElementsList.HasParentElements || this._lockedAllExceptElementsList.DefinedInParent(key2)))
					{
						ConfigurationElement configurationElement = (ConfigurationElement)obj2;
						if (configurationProperty2.Name != ConfigurationProperty.DefaultCollectionPropertyName)
						{
							flag |= configurationElement.SerializeToXmlElement(writer, configurationProperty2.Name);
						}
						else
						{
							if (flag3)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_base_element_cannot_have_multiple_child_elements", new object[]
								{
									configurationProperty2.Name
								}));
							}
							configurationElement._lockedAttributesList = null;
							configurationElement._lockedAllExceptAttributesList = null;
							configurationElement._lockedElementsList = null;
							configurationElement._lockedAllExceptElementsList = null;
							flag |= configurationElement.SerializeElement(writer, false);
							flag3 = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000453C File Offset: 0x0000353C
		private bool SerializeLockList(ConfigurationLockCollection list, string elementKey, XmlWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (list != null)
			{
				foreach (object obj in list)
				{
					string text = (string)obj;
					if (!list.DefinedInParent(text))
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(',');
						}
						stringBuilder.Append(text);
					}
				}
			}
			if (writer != null && stringBuilder.Length != 0)
			{
				writer.WriteAttributeString(elementKey, stringBuilder.ToString());
			}
			return stringBuilder.Length != 0;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000045D8 File Offset: 0x000035D8
		internal void ReportInvalidLock(string attribToLockTrim, ConfigurationLockCollectionType lockedType, ConfigurationValue value, string collectionProperties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(collectionProperties) && (lockedType == ConfigurationLockCollectionType.LockedElements || lockedType == ConfigurationLockCollectionType.LockedElementsExceptionList))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(collectionProperties);
			}
			foreach (object obj in this.Properties)
			{
				ConfigurationProperty configurationProperty = (ConfigurationProperty)obj;
				if (configurationProperty.Name != "lockAttributes" && configurationProperty.Name != "lockAllAttributesExcept" && configurationProperty.Name != "lockElements" && configurationProperty.Name != "lockAllElementsExcept")
				{
					if (lockedType == ConfigurationLockCollectionType.LockedElements || lockedType == ConfigurationLockCollectionType.LockedElementsExceptionList)
					{
						if (typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append("'");
							stringBuilder.Append(configurationProperty.Name);
							stringBuilder.Append("'");
						}
					}
					else if (!typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append("'");
						stringBuilder.Append(configurationProperty.Name);
						stringBuilder.Append("'");
					}
				}
			}
			string @string;
			if (lockedType == ConfigurationLockCollectionType.LockedElements || lockedType == ConfigurationLockCollectionType.LockedElementsExceptionList)
			{
				if (value != null)
				{
					@string = SR.GetString("Config_base_invalid_element_to_lock");
				}
				else
				{
					@string = SR.GetString("Config_base_invalid_element_to_lock_by_add");
				}
			}
			else if (value != null)
			{
				@string = SR.GetString("Config_base_invalid_attribute_to_lock");
			}
			else
			{
				@string = SR.GetString("Config_base_invalid_attribute_to_lock_by_add");
			}
			if (value != null)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, @string, new object[]
				{
					attribToLockTrim,
					stringBuilder.ToString()
				}), value.SourceInfo.FileName, value.SourceInfo.LineNumber);
			}
			throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, @string, new object[]
			{
				attribToLockTrim,
				stringBuilder.ToString()
			}));
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004830 File Offset: 0x00003830
		private ConfigurationLockCollection ParseLockedAttributes(ConfigurationValue value, ConfigurationLockCollectionType lockType)
		{
			ConfigurationLockCollection configurationLockCollection = new ConfigurationLockCollection(this, lockType);
			string text = (string)value.Value;
			if (string.IsNullOrEmpty(text))
			{
				if (lockType == ConfigurationLockCollectionType.LockedAttributes)
				{
					throw new ConfigurationErrorsException(SR.GetString("Empty_attribute", new object[]
					{
						"lockAttributes"
					}), value.SourceInfo.FileName, value.SourceInfo.LineNumber);
				}
				if (lockType == ConfigurationLockCollectionType.LockedElements)
				{
					throw new ConfigurationErrorsException(SR.GetString("Empty_attribute", new object[]
					{
						"lockElements"
					}), value.SourceInfo.FileName, value.SourceInfo.LineNumber);
				}
				if (lockType == ConfigurationLockCollectionType.LockedExceptionList)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_empty_lock_attributes_except", new object[]
					{
						"lockAllAttributesExcept",
						"lockAttributes"
					}), value.SourceInfo.FileName, value.SourceInfo.LineNumber);
				}
				if (lockType == ConfigurationLockCollectionType.LockedElementsExceptionList)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_empty_lock_element_except", new object[]
					{
						"lockAllElementsExcept",
						"lockElements"
					}), value.SourceInfo.FileName, value.SourceInfo.LineNumber);
				}
			}
			string[] array = text.Split(new char[]
			{
				',',
				':',
				';'
			});
			foreach (string text2 in array)
			{
				string text3 = text2.Trim();
				if (!string.IsNullOrEmpty(text3))
				{
					if ((lockType != ConfigurationLockCollectionType.LockedElements && lockType != ConfigurationLockCollectionType.LockedAttributes) || !(text3 == "*"))
					{
						ConfigurationProperty configurationProperty = this.Properties[text3];
						if (configurationProperty == null || text3 == "lockAttributes" || text3 == "lockAllAttributesExcept" || text3 == "lockElements" || (lockType != ConfigurationLockCollectionType.LockedElements && lockType != ConfigurationLockCollectionType.LockedElementsExceptionList && typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type)) || ((lockType == ConfigurationLockCollectionType.LockedElements || lockType == ConfigurationLockCollectionType.LockedElementsExceptionList) && !typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type)))
						{
							ConfigurationElementCollection configurationElementCollection = this as ConfigurationElementCollection;
							if (configurationElementCollection == null && this.Properties.DefaultCollectionProperty != null)
							{
								configurationElementCollection = (this[this.Properties.DefaultCollectionProperty] as ConfigurationElementCollection);
							}
							if (configurationElementCollection == null || lockType == ConfigurationLockCollectionType.LockedAttributes || lockType == ConfigurationLockCollectionType.LockedExceptionList)
							{
								this.ReportInvalidLock(text3, lockType, value, null);
							}
							else if (!configurationElementCollection.IsLockableElement(text3))
							{
								this.ReportInvalidLock(text3, lockType, value, configurationElementCollection.LockableElements);
							}
						}
						if (configurationProperty != null && configurationProperty.IsRequired)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_lock_attempt", new object[]
							{
								configurationProperty.Name
							}));
						}
					}
					configurationLockCollection.Add(text3, ConfigurationValueFlags.Default);
				}
			}
			return configurationLockCollection;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004AE8 File Offset: 0x00003AE8
		private StringCollection IntersectLockCollections(ConfigurationLockCollection Collection1, ConfigurationLockCollection Collection2)
		{
			ConfigurationLockCollection configurationLockCollection = (Collection1.Count < Collection2.Count) ? Collection1 : Collection2;
			ConfigurationLockCollection configurationLockCollection2 = (Collection1.Count >= Collection2.Count) ? Collection1 : Collection2;
			StringCollection stringCollection = new StringCollection();
			foreach (object obj in configurationLockCollection)
			{
				string text = (string)obj;
				if (configurationLockCollection2.Contains(text) || text == this.ElementTagName)
				{
					stringCollection.Add(text);
				}
			}
			return stringCollection;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004B88 File Offset: 0x00003B88
		protected internal virtual void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			ConfigurationPropertyCollection properties = this.Properties;
			ConfigurationValue configurationValue = null;
			ConfigurationValue configurationValue2 = null;
			ConfigurationValue configurationValue3 = null;
			ConfigurationValue configurationValue4 = null;
			bool flag = false;
			this._bElementPresent = true;
			ConfigurationElement configurationElement = null;
			ConfigurationProperty configurationProperty = (properties != null) ? properties.DefaultCollectionProperty : null;
			if (configurationProperty != null)
			{
				configurationElement = (ConfigurationElement)this[configurationProperty];
			}
			this._elementTagName = reader.Name;
			PropertySourceInfo sourceInfo = new PropertySourceInfo(reader);
			this._values.SetValue(reader.Name, null, ConfigurationValueFlags.Modified, sourceInfo);
			this._values.SetValue("", configurationElement, ConfigurationValueFlags.Modified, sourceInfo);
			if ((this._lockedElementsList != null && (this._lockedElementsList.Contains(reader.Name) || (this._lockedElementsList.Contains("*") && reader.Name != this.ElementTagName))) || (this._lockedAllExceptElementsList != null && this._lockedAllExceptElementsList.Count != 0 && !this._lockedAllExceptElementsList.Contains(reader.Name)) || ((this._fItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default && (this._fItemLocked & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_element_locked", new object[]
				{
					reader.Name
				}), reader);
			}
			if (reader.AttributeCount > 0)
			{
				while (reader.MoveToNextAttribute())
				{
					string name = reader.Name;
					if (((this._lockedAttributesList != null && (this._lockedAttributesList.Contains(name) || this._lockedAttributesList.Contains("*"))) || (this._lockedAllExceptAttributesList != null && !this._lockedAllExceptAttributesList.Contains(name))) && name != "lockAttributes" && name != "lockAllAttributesExcept")
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[]
						{
							name
						}), reader);
					}
					ConfigurationProperty configurationProperty2 = (properties != null) ? properties[name] : null;
					if (configurationProperty2 != null)
					{
						if (serializeCollectionKey && !configurationProperty2.IsKey)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[]
							{
								name
							}), reader);
						}
						this._values.SetValue(name, this.DeserializePropertyValue(configurationProperty2, reader), ConfigurationValueFlags.Modified, new PropertySourceInfo(reader));
					}
					else
					{
						if (name == "lockItem")
						{
							try
							{
								flag = bool.Parse(reader.Value);
								continue;
							}
							catch
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_invalid_boolean_attribute", new object[]
								{
									name
								}), reader);
							}
						}
						if (name == "lockAttributes")
						{
							configurationValue = new ConfigurationValue(reader.Value, ConfigurationValueFlags.Default, new PropertySourceInfo(reader));
						}
						else if (name == "lockAllAttributesExcept")
						{
							configurationValue2 = new ConfigurationValue(reader.Value, ConfigurationValueFlags.Default, new PropertySourceInfo(reader));
						}
						else if (name == "lockElements")
						{
							configurationValue3 = new ConfigurationValue(reader.Value, ConfigurationValueFlags.Default, new PropertySourceInfo(reader));
						}
						else if (name == "lockAllElementsExcept")
						{
							configurationValue4 = new ConfigurationValue(reader.Value, ConfigurationValueFlags.Default, new PropertySourceInfo(reader));
						}
						else if (serializeCollectionKey || !this.OnDeserializeUnrecognizedAttribute(name, reader.Value))
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[]
							{
								name
							}), reader);
						}
					}
				}
			}
			reader.MoveToElement();
			try
			{
				HybridDictionary hybridDictionary = new HybridDictionary();
				if (!reader.IsEmptyElement)
				{
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element)
						{
							string name2 = reader.Name;
							this.CheckLockedElement(name2, null);
							ConfigurationProperty configurationProperty3 = (properties != null) ? properties[name2] : null;
							if (configurationProperty3 != null)
							{
								if (!typeof(ConfigurationElement).IsAssignableFrom(configurationProperty3.Type))
								{
									throw new ConfigurationErrorsException(SR.GetString("Config_base_property_is_not_a_configuration_element", new object[]
									{
										name2
									}), reader);
								}
								if (hybridDictionary.Contains(name2))
								{
									throw new ConfigurationErrorsException(SR.GetString("Config_base_element_cannot_have_multiple_child_elements", new object[]
									{
										name2
									}), reader);
								}
								hybridDictionary.Add(name2, name2);
								ConfigurationElement configurationElement2 = (ConfigurationElement)this[configurationProperty3];
								configurationElement2.DeserializeElement(reader, serializeCollectionKey);
								ConfigurationElement.ValidateElement(configurationElement2, configurationProperty3.Validator, false);
							}
							else if (!this.OnDeserializeUnrecognizedElement(name2, reader) && (configurationElement == null || !configurationElement.OnDeserializeUnrecognizedElement(name2, reader)))
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element_name", new object[]
								{
									name2
								}), reader);
							}
						}
						else
						{
							if (reader.NodeType == XmlNodeType.EndElement)
							{
								break;
							}
							if (reader.NodeType == XmlNodeType.CDATA || reader.NodeType == XmlNodeType.Text)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_base_section_invalid_content"), reader);
							}
						}
					}
				}
				this.EnsureRequiredProperties(serializeCollectionKey);
				ConfigurationElement.ValidateElement(this, null, false);
			}
			catch (ConfigurationException ex)
			{
				if (ex.Filename == null || ex.Filename.Length == 0)
				{
					throw new ConfigurationErrorsException(ex.Message, reader);
				}
				throw ex;
			}
			if (flag)
			{
				this.SetLocked();
				this._fItemLocked = ConfigurationValueFlags.Locked;
			}
			if (configurationValue != null)
			{
				if (this._lockedAttributesList == null)
				{
					this._lockedAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedAttributes);
				}
				foreach (object obj in this.ParseLockedAttributes(configurationValue, ConfigurationLockCollectionType.LockedAttributes))
				{
					string name3 = (string)obj;
					if (!this._lockedAttributesList.Contains(name3))
					{
						this._lockedAttributesList.Add(name3, ConfigurationValueFlags.Default);
					}
					else
					{
						this._lockedAttributesList.Add(name3, ConfigurationValueFlags.Inherited | ConfigurationValueFlags.Modified);
					}
				}
			}
			if (configurationValue2 != null)
			{
				ConfigurationLockCollection configurationLockCollection = this.ParseLockedAttributes(configurationValue2, ConfigurationLockCollectionType.LockedExceptionList);
				if (this._lockedAllExceptAttributesList == null)
				{
					this._lockedAllExceptAttributesList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedExceptionList, string.Empty, configurationLockCollection);
					this._lockedAllExceptAttributesList.ClearSeedList();
				}
				StringCollection stringCollection = this.IntersectLockCollections(this._lockedAllExceptAttributesList, configurationLockCollection);
				this._lockedAllExceptAttributesList.ClearInternal(false);
				foreach (string name4 in stringCollection)
				{
					this._lockedAllExceptAttributesList.Add(name4, ConfigurationValueFlags.Default);
				}
			}
			if (configurationValue3 != null)
			{
				if (this._lockedElementsList == null)
				{
					this._lockedElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElements);
				}
				ConfigurationLockCollection configurationLockCollection2 = this.ParseLockedAttributes(configurationValue3, ConfigurationLockCollectionType.LockedElements);
				if (properties.DefaultCollectionProperty != null)
				{
					ConfigurationElementCollection configurationElementCollection = this[properties.DefaultCollectionProperty] as ConfigurationElementCollection;
					if (configurationElementCollection != null && configurationElementCollection._lockedElementsList == null)
					{
						configurationElementCollection._lockedElementsList = this._lockedElementsList;
					}
				}
				foreach (object obj2 in configurationLockCollection2)
				{
					string text = (string)obj2;
					if (!this._lockedElementsList.Contains(text))
					{
						this._lockedElementsList.Add(text, ConfigurationValueFlags.Default);
						ConfigurationProperty configurationProperty4 = this.Properties[text];
						if (configurationProperty4 != null && typeof(ConfigurationElement).IsAssignableFrom(configurationProperty4.Type))
						{
							((ConfigurationElement)this[text]).SetLocked();
						}
						if (text == "*")
						{
							foreach (object obj3 in this.Properties)
							{
								ConfigurationProperty configurationProperty5 = (ConfigurationProperty)obj3;
								if (!string.IsNullOrEmpty(configurationProperty5.Name) && typeof(ConfigurationElement).IsAssignableFrom(configurationProperty5.Type))
								{
									((ConfigurationElement)this[configurationProperty5]).SetLocked();
								}
							}
						}
					}
				}
			}
			if (configurationValue4 != null)
			{
				ConfigurationLockCollection configurationLockCollection3 = this.ParseLockedAttributes(configurationValue4, ConfigurationLockCollectionType.LockedElementsExceptionList);
				if (this._lockedAllExceptElementsList == null)
				{
					this._lockedAllExceptElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElementsExceptionList, this._elementTagName, configurationLockCollection3);
					this._lockedAllExceptElementsList.ClearSeedList();
				}
				StringCollection stringCollection2 = this.IntersectLockCollections(this._lockedAllExceptElementsList, configurationLockCollection3);
				if (properties.DefaultCollectionProperty != null)
				{
					ConfigurationElementCollection configurationElementCollection2 = this[properties.DefaultCollectionProperty] as ConfigurationElementCollection;
					if (configurationElementCollection2 != null && configurationElementCollection2._lockedAllExceptElementsList == null)
					{
						configurationElementCollection2._lockedAllExceptElementsList = this._lockedAllExceptElementsList;
					}
				}
				this._lockedAllExceptElementsList.ClearInternal(false);
				foreach (string text2 in stringCollection2)
				{
					if (!this._lockedAllExceptElementsList.Contains(text2) || text2 == this.ElementTagName)
					{
						this._lockedAllExceptElementsList.Add(text2, ConfigurationValueFlags.Default);
					}
				}
				foreach (object obj4 in this.Properties)
				{
					ConfigurationProperty configurationProperty6 = (ConfigurationProperty)obj4;
					if (!string.IsNullOrEmpty(configurationProperty6.Name) && !this._lockedAllExceptElementsList.Contains(configurationProperty6.Name) && typeof(ConfigurationElement).IsAssignableFrom(configurationProperty6.Type))
					{
						((ConfigurationElement)this[configurationProperty6]).SetLocked();
					}
				}
			}
			if (configurationProperty != null)
			{
				configurationElement = (ConfigurationElement)this[configurationProperty];
				if (this._lockedElementsList == null)
				{
					this._lockedElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElements);
				}
				configurationElement._lockedElementsList = this._lockedElementsList;
				if (this._lockedAllExceptElementsList == null)
				{
					this._lockedAllExceptElementsList = new ConfigurationLockCollection(this, ConfigurationLockCollectionType.LockedElementsExceptionList, reader.Name);
					this._lockedAllExceptElementsList.ClearSeedList();
				}
				configurationElement._lockedAllExceptElementsList = this._lockedAllExceptElementsList;
			}
			this.PostDeserialize();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000055AC File Offset: 0x000045AC
		private object DeserializePropertyValue(ConfigurationProperty prop, XmlReader reader)
		{
			string value = reader.Value;
			object obj = null;
			try
			{
				obj = prop.ConvertFromString(value);
				prop.Validate(obj);
			}
			catch (ConfigurationException ex)
			{
				if (string.IsNullOrEmpty(ex.Filename))
				{
					ex = new ConfigurationErrorsException(ex.Message, reader);
				}
				obj = new InvalidPropValue(value, ex);
			}
			catch
			{
			}
			return obj;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005618 File Offset: 0x00004618
		internal static void ValidateElement(ConfigurationElement elem, ConfigurationValidatorBase propValidator, bool recursive)
		{
			ConfigurationValidatorBase configurationValidatorBase = propValidator;
			if (configurationValidatorBase == null && elem.ElementProperty != null)
			{
				configurationValidatorBase = elem.ElementProperty.Validator;
				if (configurationValidatorBase != null && !configurationValidatorBase.CanValidate(elem.GetType()))
				{
					throw new ConfigurationErrorsException(SR.GetString("Validator_does_not_support_elem_type", new object[]
					{
						elem.GetType().Name
					}));
				}
			}
			try
			{
				if (configurationValidatorBase != null)
				{
					configurationValidatorBase.Validate(elem);
				}
			}
			catch (ConfigurationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Validator_element_not_valid", new object[]
				{
					elem._elementTagName,
					ex.Message
				}));
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Validator_element_not_valid", new object[]
				{
					elem._elementTagName,
					ExceptionUtil.NoExceptionInformation
				}));
			}
			if (recursive)
			{
				if (elem is ConfigurationElementCollection && elem is ConfigurationElementCollection)
				{
					IEnumerator elementsEnumerator = ((ConfigurationElementCollection)elem).GetElementsEnumerator();
					while (elementsEnumerator.MoveNext())
					{
						object obj = elementsEnumerator.Current;
						ConfigurationElement.ValidateElement((ConfigurationElement)obj, null, true);
					}
				}
				for (int i = 0; i < elem.Values.Count; i++)
				{
					ConfigurationElement configurationElement = elem.Values[i] as ConfigurationElement;
					if (configurationElement != null)
					{
						ConfigurationElement.ValidateElement(configurationElement, null, true);
					}
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005780 File Offset: 0x00004780
		private void EnsureRequiredProperties(bool ensureKeysOnly)
		{
			ConfigurationPropertyCollection properties = this.Properties;
			if (properties != null)
			{
				foreach (object obj in properties)
				{
					ConfigurationProperty configurationProperty = (ConfigurationProperty)obj;
					if (configurationProperty.IsRequired && !this._values.Contains(configurationProperty.Name) && (!ensureKeysOnly || configurationProperty.IsKey))
					{
						this._values[configurationProperty.Name] = this.OnRequiredPropertyNotFound(configurationProperty.Name);
					}
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000581C File Offset: 0x0000481C
		protected virtual object OnRequiredPropertyNotFound(string name)
		{
			throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_missing", new object[]
			{
				name
			}), this.PropertyFileName(name), this.PropertyLineNumber(name));
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005852 File Offset: 0x00004852
		protected virtual void PostDeserialize()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005854 File Offset: 0x00004854
		protected virtual void PreSerialize(XmlWriter writer)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005856 File Offset: 0x00004856
		protected virtual bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			return false;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005859 File Offset: 0x00004859
		protected virtual bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			return false;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000585C File Offset: 0x0000485C
		public ElementInformation ElementInformation
		{
			get
			{
				if (this._evaluationElement == null)
				{
					this._evaluationElement = new ElementInformation(this);
				}
				return this._evaluationElement;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00005878 File Offset: 0x00004878
		protected ContextInformation EvaluationContext
		{
			get
			{
				if (this._evalContext == null)
				{
					if (this._configRecord == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_element_no_context"));
					}
					this._evalContext = new ContextInformation(this._configRecord);
				}
				return this._evalContext;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000058B1 File Offset: 0x000048B1
		protected internal virtual ConfigurationElementProperty ElementProperty
		{
			get
			{
				return this._elementProperty;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000058BC File Offset: 0x000048BC
		internal ConfigurationLockCollection UnMergeLockList(ConfigurationLockCollection sourceLockList, ConfigurationLockCollection parentLockList, ConfigurationSaveMode saveMode)
		{
			if (!sourceLockList.ExceptionList)
			{
				switch (saveMode)
				{
				case ConfigurationSaveMode.Modified:
				{
					ConfigurationLockCollection configurationLockCollection = new ConfigurationLockCollection(this, sourceLockList.LockType);
					foreach (object obj in sourceLockList)
					{
						string name = (string)obj;
						if (!parentLockList.Contains(name) || sourceLockList.IsValueModified(name))
						{
							configurationLockCollection.Add(name, ConfigurationValueFlags.Default);
						}
					}
					return configurationLockCollection;
				}
				case ConfigurationSaveMode.Minimal:
				{
					ConfigurationLockCollection configurationLockCollection2 = new ConfigurationLockCollection(this, sourceLockList.LockType);
					foreach (object obj2 in sourceLockList)
					{
						string name2 = (string)obj2;
						if (!parentLockList.Contains(name2))
						{
							configurationLockCollection2.Add(name2, ConfigurationValueFlags.Default);
						}
					}
					return configurationLockCollection2;
				}
				}
			}
			else if (saveMode == ConfigurationSaveMode.Modified || saveMode == ConfigurationSaveMode.Minimal)
			{
				bool flag = false;
				if (sourceLockList.Count == parentLockList.Count)
				{
					flag = true;
					foreach (object obj3 in sourceLockList)
					{
						string name3 = (string)obj3;
						if (!parentLockList.Contains(name3) || (sourceLockList.IsValueModified(name3) && saveMode == ConfigurationSaveMode.Modified))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					return null;
				}
			}
			return sourceLockList;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005A40 File Offset: 0x00004A40
		internal static bool IsLockAttributeName(string name)
		{
			if (!StringUtil.StartsWith(name, "lock"))
			{
				return false;
			}
			foreach (string b in ConfigurationElement.s_lockAttributeNames)
			{
				if (name == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000141 RID: 321
		private const string LockAttributesKey = "lockAttributes";

		// Token: 0x04000142 RID: 322
		private const string LockAllAttributesExceptKey = "lockAllAttributesExcept";

		// Token: 0x04000143 RID: 323
		private const string LockElementsKey = "lockElements";

		// Token: 0x04000144 RID: 324
		private const string LockAll = "*";

		// Token: 0x04000145 RID: 325
		private const string LockAllElementsExceptKey = "lockAllElementsExcept";

		// Token: 0x04000146 RID: 326
		private const string LockItemKey = "lockItem";

		// Token: 0x04000147 RID: 327
		internal const string DefaultCollectionPropertyName = "";

		// Token: 0x04000148 RID: 328
		private static string[] s_lockAttributeNames = new string[]
		{
			"lockAttributes",
			"lockAllAttributesExcept",
			"lockElements",
			"lockAllElementsExcept",
			"lockItem"
		};

		// Token: 0x04000149 RID: 329
		private static Hashtable s_propertyBags = new Hashtable();

		// Token: 0x0400014A RID: 330
		private static Dictionary<Type, ConfigurationValidatorBase> s_perTypeValidators;

		// Token: 0x0400014B RID: 331
		internal static readonly object s_nullPropertyValue = new object();

		// Token: 0x0400014C RID: 332
		private static ConfigurationElementProperty s_ElementProperty = new ConfigurationElementProperty(new DefaultValidator());

		// Token: 0x0400014D RID: 333
		private bool _bDataToWrite;

		// Token: 0x0400014E RID: 334
		private bool _bModified;

		// Token: 0x0400014F RID: 335
		private bool _bReadOnly;

		// Token: 0x04000150 RID: 336
		private bool _bElementPresent;

		// Token: 0x04000151 RID: 337
		private bool _bInited;

		// Token: 0x04000152 RID: 338
		internal ConfigurationLockCollection _lockedAttributesList;

		// Token: 0x04000153 RID: 339
		internal ConfigurationLockCollection _lockedAllExceptAttributesList;

		// Token: 0x04000154 RID: 340
		internal ConfigurationLockCollection _lockedElementsList;

		// Token: 0x04000155 RID: 341
		internal ConfigurationLockCollection _lockedAllExceptElementsList;

		// Token: 0x04000156 RID: 342
		private ConfigurationValues _values;

		// Token: 0x04000157 RID: 343
		private string _elementTagName;

		// Token: 0x04000158 RID: 344
		private ElementInformation _evaluationElement;

		// Token: 0x04000159 RID: 345
		private ConfigurationElementProperty _elementProperty = ConfigurationElement.s_ElementProperty;

		// Token: 0x0400015A RID: 346
		internal ConfigurationValueFlags _fItemLocked;

		// Token: 0x0400015B RID: 347
		internal ContextInformation _evalContext;

		// Token: 0x0400015C RID: 348
		internal BaseConfigurationRecord _configRecord;
	}
}
