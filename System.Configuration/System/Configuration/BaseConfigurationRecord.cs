using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Internal;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Win32;

namespace System.Configuration
{
	// Token: 0x02000010 RID: 16
	[DebuggerDisplay("ConfigPath = {ConfigPath}")]
	internal abstract class BaseConfigurationRecord : IInternalConfigRecord
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00005EC8 File Offset: 0x00004EC8
		internal BaseConfigurationRecord()
		{
			this._flags = default(SafeBitVector32);
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007B RID: 123
		protected abstract SimpleBitVector32 ClassFlags { get; }

		// Token: 0x0600007C RID: 124
		protected abstract object CreateSectionFactory(FactoryRecord factoryRecord);

		// Token: 0x0600007D RID: 125
		protected abstract object CreateSection(bool inputIsTrusted, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader);

		// Token: 0x0600007E RID: 126
		protected abstract object UseParentResult(string configKey, object parentResult, SectionRecord sectionRecord);

		// Token: 0x0600007F RID: 127
		protected abstract object GetRuntimeObject(object result);

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00005EDC File Offset: 0x00004EDC
		public string ConfigPath
		{
			get
			{
				return this._configPath;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00005EE4 File Offset: 0x00004EE4
		public string StreamName
		{
			get
			{
				return this.ConfigStreamInfo.StreamName;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00005EF4 File Offset: 0x00004EF4
		public bool HasInitErrors
		{
			get
			{
				return this._initErrors.HasErrors(this.ClassFlags[64]);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005F1C File Offset: 0x00004F1C
		public void ThrowIfInitErrors()
		{
			this.ThrowIfParseErrors(this._initErrors);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005F2A File Offset: 0x00004F2A
		public object GetSection(string configKey)
		{
			return this.GetSection(configKey, false, true);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005F35 File Offset: 0x00004F35
		public object GetLkgSection(string configKey)
		{
			return this.GetSection(configKey, true, true);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005F40 File Offset: 0x00004F40
		public void RefreshSection(string configKey)
		{
			this._configRoot.ClearResult(this, configKey, true);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005F50 File Offset: 0x00004F50
		public void Remove()
		{
			this._configRoot.RemoveConfigRecord(this);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00005F5E File Offset: 0x00004F5E
		internal bool HasStream
		{
			get
			{
				return this.ConfigStreamInfo.HasStream;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005F6C File Offset: 0x00004F6C
		private bool ShouldPrefetchRawXml(FactoryRecord factoryRecord)
		{
			string configKey;
			return this._flags[8] || ((configKey = factoryRecord.ConfigKey) != null && (configKey == "configProtectedData" || configKey == "system.diagnostics" || configKey == "appSettings" || configKey == "connectionStrings")) || this.Host.PrefetchSection(factoryRecord.Group, factoryRecord.Name);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005FE0 File Offset: 0x00004FE0
		protected IDisposable Impersonate()
		{
			IDisposable disposable = null;
			if (this.ClassFlags[4])
			{
				disposable = this.Host.Impersonate();
			}
			if (disposable == null)
			{
				disposable = EmptyImpersonationContext.GetStaticInstance();
			}
			return disposable;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006018 File Offset: 0x00005018
		internal PermissionSet GetRestrictedPermissions()
		{
			if (!this._flags[2048])
			{
				PermissionSet restrictedPermissions;
				bool flag;
				this.Host.GetRestrictedPermissions(this, out restrictedPermissions, out flag);
				if (flag)
				{
					this._restrictedPermissions = restrictedPermissions;
					this._flags[2048] = true;
				}
			}
			return this._restrictedPermissions;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00006068 File Offset: 0x00005068
		internal void Init(IInternalConfigRoot configRoot, BaseConfigurationRecord parent, string configPath, string locationSubPath)
		{
			this._initErrors = new ConfigurationSchemaErrors();
			try
			{
				this._configRoot = (InternalConfigRoot)configRoot;
				this._parent = parent;
				this._configPath = configPath;
				this._locationSubPath = locationSubPath;
				this._configName = ConfigPathUtility.GetName(configPath);
				if (this.IsLocationConfig)
				{
					this._configStreamInfo = this._parent.ConfigStreamInfo;
				}
				else
				{
					this._configStreamInfo = new BaseConfigurationRecord.ConfigRecordStreamInfo();
				}
				if (!this.IsRootConfig)
				{
					this._flags[65536] = (this.ClassFlags[1] && this.Host.SupportsChangeNotifications);
					this._flags[131072] = (this.ClassFlags[2] && this.Host.SupportsRefresh);
					this._flags[524288] = (this.ClassFlags[16] || this._flags[131072]);
					this._flags[262144] = this.Host.SupportsPath;
					this._flags[1048576] = this.Host.SupportsLocation;
					if (this._flags[1048576])
					{
						this._flags[32] = this.Host.IsAboveApplication(this._configPath);
					}
					this._flags[8192] = this.Host.IsTrustedConfigPath(this._configPath);
					ArrayList arrayList = null;
					if (this._flags[1048576])
					{
						if (this.IsLocationConfig && this._parent._locationSections != null)
						{
							this._parent.ResolveLocationSections();
							int i = 0;
							while (i < this._parent._locationSections.Count)
							{
								LocationSectionRecord locationSectionRecord = (LocationSectionRecord)this._parent._locationSections[i];
								if (!StringUtil.EqualsIgnoreCase(locationSectionRecord.SectionXmlInfo.TargetConfigPath, this.ConfigPath))
								{
									i++;
								}
								else
								{
									this._parent._locationSections.RemoveAt(i);
									if (arrayList == null)
									{
										arrayList = new ArrayList();
									}
									arrayList.Add(locationSectionRecord);
								}
							}
						}
						if (this.IsLocationConfig && this.Host.IsLocationApplicable(this._configPath))
						{
							Dictionary<string, List<SectionInput>> dictionary = null;
							BaseConfigurationRecord parent2 = this._parent;
							while (!parent2.IsRootConfig)
							{
								if (parent2._locationSections != null)
								{
									parent2.ResolveLocationSections();
									foreach (object obj in parent2._locationSections)
									{
										LocationSectionRecord locationSectionRecord2 = (LocationSectionRecord)obj;
										if (this.IsLocationConfig && UrlPath.IsSubpath(locationSectionRecord2.SectionXmlInfo.TargetConfigPath, this.ConfigPath) && UrlPath.IsSubpath(parent.ConfigPath, locationSectionRecord2.SectionXmlInfo.TargetConfigPath) && !this.ShouldSkipDueToInheritInChildApplications(locationSectionRecord2.SectionXmlInfo.SkipInChildApps, locationSectionRecord2.SectionXmlInfo.TargetConfigPath))
										{
											if (dictionary == null)
											{
												dictionary = new Dictionary<string, List<SectionInput>>(1);
											}
											string configKey = locationSectionRecord2.SectionXmlInfo.ConfigKey;
											if (!((IDictionary)dictionary).Contains(configKey))
											{
												dictionary.Add(configKey, new List<SectionInput>(1));
											}
											dictionary[configKey].Add(new SectionInput(locationSectionRecord2.SectionXmlInfo, locationSectionRecord2.ErrorsList));
											if (locationSectionRecord2.HasErrors)
											{
												this._initErrors.AddSavedLocalErrors(locationSectionRecord2.Errors);
											}
										}
									}
								}
								parent2 = parent2._parent;
							}
							if (dictionary != null)
							{
								foreach (KeyValuePair<string, List<SectionInput>> keyValuePair in dictionary)
								{
									List<SectionInput> value = keyValuePair.Value;
									string key = keyValuePair.Key;
									value.Sort(BaseConfigurationRecord.s_indirectInputsComparer);
									SectionRecord sectionRecord = this.EnsureSectionRecord(key, true);
									foreach (SectionInput sectionInput in value)
									{
										sectionRecord.AddIndirectLocationInput(sectionInput);
									}
								}
							}
						}
						if (this.Host.IsLocationApplicable(this._configPath))
						{
							BaseConfigurationRecord parent3 = this._parent;
							while (!parent3.IsRootConfig)
							{
								if (parent3._locationSections != null)
								{
									parent3.ResolveLocationSections();
									foreach (object obj2 in parent3._locationSections)
									{
										LocationSectionRecord locationSectionRecord3 = (LocationSectionRecord)obj2;
										if (StringUtil.EqualsIgnoreCase(locationSectionRecord3.SectionXmlInfo.TargetConfigPath, this._configPath) && !this.ShouldSkipDueToInheritInChildApplications(locationSectionRecord3.SectionXmlInfo.SkipInChildApps))
										{
											SectionRecord sectionRecord2 = this.EnsureSectionRecord(locationSectionRecord3.ConfigKey, true);
											SectionInput sectionInput2 = new SectionInput(locationSectionRecord3.SectionXmlInfo, locationSectionRecord3.ErrorsList);
											sectionRecord2.AddLocationInput(sectionInput2);
											if (locationSectionRecord3.HasErrors)
											{
												this._initErrors.AddSavedLocalErrors(locationSectionRecord3.Errors);
											}
										}
									}
								}
								parent3 = parent3._parent;
							}
						}
					}
					if (!this.IsLocationConfig)
					{
						this.InitConfigFromFile();
					}
					else if (arrayList != null)
					{
						foreach (object obj3 in arrayList)
						{
							LocationSectionRecord locationSectionRecord4 = (LocationSectionRecord)obj3;
							SectionRecord sectionRecord3 = this.EnsureSectionRecord(locationSectionRecord4.ConfigKey, true);
							SectionInput sectionInput3 = new SectionInput(locationSectionRecord4.SectionXmlInfo, locationSectionRecord4.ErrorsList);
							sectionRecord3.AddFileInput(sectionInput3);
							if (locationSectionRecord4.HasErrors)
							{
								this._initErrors.AddSavedLocalErrors(locationSectionRecord4.Errors);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				string filename = (this.ConfigStreamInfo != null) ? this.ConfigStreamInfo.StreamName : null;
				this._initErrors.AddError(ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), e, filename, 0), ExceptionAction.Global);
			}
			catch
			{
				string filename2 = (this.ConfigStreamInfo != null) ? this.ConfigStreamInfo.StreamName : null;
				this._initErrors.AddError(ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), null, filename2, 0), ExceptionAction.Global);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00006764 File Offset: 0x00005764
		private void InitConfigFromFile()
		{
			bool flag = false;
			try
			{
				if (this.ClassFlags[32] && this.Host.IsInitDelayed(this))
				{
					if (this._parent._initDelayedRoot == null)
					{
						this._initDelayedRoot = this;
					}
					else
					{
						this._initDelayedRoot = this._parent._initDelayedRoot;
					}
				}
				else
				{
					using (this.Impersonate())
					{
						this.ConfigStreamInfo.StreamName = this.Host.GetStreamName(this._configPath);
						if (!string.IsNullOrEmpty(this.ConfigStreamInfo.StreamName))
						{
							this.ConfigStreamInfo.StreamVersion = this.MonitorStream(null, null, this.ConfigStreamInfo.StreamName);
							using (Stream stream = this.Host.OpenStreamForRead(this.ConfigStreamInfo.StreamName))
							{
								if (stream == null)
								{
									return;
								}
								this.ConfigStreamInfo.HasStream = true;
								this._flags[8] = this.Host.PrefetchAll(this._configPath, this.ConfigStreamInfo.StreamName);
								using (XmlUtil xmlUtil = new XmlUtil(stream, this.ConfigStreamInfo.StreamName, true, this._initErrors))
								{
									this.ConfigStreamInfo.StreamEncoding = xmlUtil.Reader.Encoding;
									Hashtable factoryRecords = this.ScanFactories(xmlUtil);
									this._factoryRecords = factoryRecords;
									this.AddImplicitSections(null);
									flag = true;
									if (xmlUtil.Reader.Depth == 1)
									{
										this.ScanSections(xmlUtil);
									}
								}
							}
						}
					}
				}
			}
			catch (XmlException e)
			{
				this._initErrors.SetSingleGlobalError(ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), e, this.ConfigStreamInfo.StreamName, 0));
			}
			catch (Exception e2)
			{
				this._initErrors.AddError(ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), e2, this.ConfigStreamInfo.StreamName, 0), ExceptionAction.Global);
			}
			catch
			{
				this._initErrors.AddError(ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), null, this.ConfigStreamInfo.StreamName, 0), ExceptionAction.Global);
			}
			if (this._initErrors.HasGlobalErrors)
			{
				this._initErrors.ResetLocalErrors();
				HybridDictionary hybridDictionary = null;
				lock (this)
				{
					if (this.ConfigStreamInfo.HasStreamInfos)
					{
						hybridDictionary = this.ConfigStreamInfo.StreamInfos;
						this.ConfigStreamInfo.ClearStreamInfos();
						if (!string.IsNullOrEmpty(this.ConfigStreamInfo.StreamName))
						{
							StreamInfo streamInfo = (StreamInfo)hybridDictionary[this.ConfigStreamInfo.StreamName];
							if (streamInfo != null)
							{
								hybridDictionary.Remove(this.ConfigStreamInfo.StreamName);
								this.ConfigStreamInfo.StreamInfos.Add(this.ConfigStreamInfo.StreamName, streamInfo);
							}
						}
					}
				}
				if (hybridDictionary != null)
				{
					foreach (object obj in hybridDictionary.Values)
					{
						StreamInfo streamInfo2 = (StreamInfo)obj;
						if (streamInfo2.IsMonitored)
						{
							this.Host.StopMonitoringStreamForChanges(streamInfo2.StreamName, this.ConfigStreamInfo.CallbackDelegate);
						}
					}
				}
				if (this._sectionRecords != null)
				{
					List<SectionRecord> list = null;
					foreach (object obj2 in this._sectionRecords.Values)
					{
						SectionRecord sectionRecord = (SectionRecord)obj2;
						if (sectionRecord.HasLocationInputs)
						{
							sectionRecord.RemoveFileInput();
						}
						else
						{
							if (list == null)
							{
								list = new List<SectionRecord>();
							}
							list.Add(sectionRecord);
						}
					}
					if (list != null)
					{
						foreach (SectionRecord sectionRecord2 in list)
						{
							this._sectionRecords.Remove(sectionRecord2.ConfigKey);
						}
					}
				}
				if (this._locationSections != null)
				{
					this._locationSections.Clear();
				}
				if (this._factoryRecords != null)
				{
					this._factoryRecords.Clear();
				}
			}
			if (!flag)
			{
				this.AddImplicitSections(null);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00006C74 File Offset: 0x00005C74
		private bool IsInitDelayed
		{
			get
			{
				return this._initDelayedRoot != null;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006C84 File Offset: 0x00005C84
		private void RefreshFactoryRecord(string configKey)
		{
			Hashtable hashtable = null;
			FactoryRecord factoryRecord = null;
			ConfigurationSchemaErrors configurationSchemaErrors = new ConfigurationSchemaErrors();
			int line = 0;
			try
			{
				using (this.Impersonate())
				{
					using (Stream stream = this.Host.OpenStreamForRead(this.ConfigStreamInfo.StreamName))
					{
						if (stream != null)
						{
							this.ConfigStreamInfo.HasStream = true;
							using (XmlUtil xmlUtil = new XmlUtil(stream, this.ConfigStreamInfo.StreamName, true, configurationSchemaErrors))
							{
								try
								{
									hashtable = this.ScanFactories(xmlUtil);
									this.ThrowIfParseErrors(xmlUtil.SchemaErrors);
								}
								catch
								{
									line = xmlUtil.LineNumber;
									throw;
								}
							}
						}
					}
				}
				if (hashtable == null)
				{
					hashtable = new Hashtable();
				}
				this.AddImplicitSections(hashtable);
				if (hashtable != null)
				{
					factoryRecord = (FactoryRecord)hashtable[configKey];
				}
			}
			catch (Exception e)
			{
				configurationSchemaErrors.AddError(ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), e, this.ConfigStreamInfo.StreamName, line), ExceptionAction.Global);
			}
			catch
			{
				configurationSchemaErrors.AddError(ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), null, this.ConfigStreamInfo.StreamName, line), ExceptionAction.Global);
			}
			if (factoryRecord != null || this.HasFactoryRecords)
			{
				this.EnsureFactories()[configKey] = factoryRecord;
			}
			this.ThrowIfParseErrors(configurationSchemaErrors);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00006E0C File Offset: 0x00005E0C
		internal IInternalConfigHost Host
		{
			get
			{
				return this._configRoot.Host;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00006E19 File Offset: 0x00005E19
		internal BaseConfigurationRecord Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00006E21 File Offset: 0x00005E21
		internal bool IsRootConfig
		{
			get
			{
				return this._parent == null;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00006E2C File Offset: 0x00005E2C
		internal bool IsMachineConfig
		{
			get
			{
				return this._parent == this._configRoot.RootConfigRecord;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00006E41 File Offset: 0x00005E41
		internal string LocationSubPath
		{
			get
			{
				return this._locationSubPath;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00006E49 File Offset: 0x00005E49
		internal bool IsLocationConfig
		{
			get
			{
				return this._locationSubPath != null;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00006E57 File Offset: 0x00005E57
		protected BaseConfigurationRecord.ConfigRecordStreamInfo ConfigStreamInfo
		{
			get
			{
				if (this.IsLocationConfig)
				{
					return this._parent._configStreamInfo;
				}
				return this._configStreamInfo;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006E74 File Offset: 0x00005E74
		private object GetSection(string configKey, bool getLkg, bool checkPermission)
		{
			object obj;
			object result;
			this.GetSectionRecursive(configKey, getLkg, checkPermission, true, true, out obj, out result);
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00006E94 File Offset: 0x00005E94
		private void GetSectionRecursive(string configKey, bool getLkg, bool checkPermission, bool getRuntimeObject, bool requestIsHere, out object result, out object resultRuntimeObject)
		{
			result = null;
			resultRuntimeObject = null;
			object obj = null;
			object obj2 = null;
			bool requirePermission = true;
			bool flag = true;
			if (!getLkg)
			{
				this.ThrowIfInitErrors();
			}
			bool flag2 = false;
			SectionRecord sectionRecord = this.GetSectionRecord(configKey, getLkg);
			if (sectionRecord != null && sectionRecord.HasResult)
			{
				if (getRuntimeObject && !sectionRecord.HasResultRuntimeObject)
				{
					try
					{
						sectionRecord.ResultRuntimeObject = this.GetRuntimeObject(sectionRecord.Result);
					}
					catch
					{
						if (!getLkg)
						{
							throw;
						}
					}
				}
				if (!getRuntimeObject || sectionRecord.HasResultRuntimeObject)
				{
					requirePermission = sectionRecord.RequirePermission;
					flag = sectionRecord.IsResultTrustedWithoutAptca;
					obj = sectionRecord.Result;
					if (getRuntimeObject)
					{
						obj2 = sectionRecord.ResultRuntimeObject;
					}
					flag2 = true;
				}
			}
			if (!flag2)
			{
				bool flag3 = sectionRecord != null && sectionRecord.HasInput;
				bool flag4 = requestIsHere || flag3;
				try
				{
					bool flag5;
					FactoryRecord factoryRecord;
					if (requestIsHere)
					{
						factoryRecord = this.FindAndEnsureFactoryRecord(configKey, out flag5);
						if (this.IsInitDelayed && (factoryRecord == null || this._initDelayedRoot.IsDefinitionAllowed(factoryRecord.AllowDefinition, factoryRecord.AllowExeDefinition)))
						{
							if (factoryRecord == null && BaseConfigurationRecord.NeverLoadUserConfigFilesDuringFactorySearch(configKey))
							{
								return;
							}
							string configPath = this._configPath;
							InternalConfigRoot configRoot = this._configRoot;
							this.Host.RequireCompleteInit(this._initDelayedRoot);
							this._initDelayedRoot.Remove();
							BaseConfigurationRecord baseConfigurationRecord = (BaseConfigurationRecord)configRoot.GetConfigRecord(configPath);
							baseConfigurationRecord.GetSectionRecursive(configKey, getLkg, checkPermission, getRuntimeObject, requestIsHere, out result, out resultRuntimeObject);
							return;
						}
						else
						{
							if (factoryRecord == null || factoryRecord.IsGroup)
							{
								return;
							}
							configKey = factoryRecord.ConfigKey;
						}
					}
					else if (flag3)
					{
						factoryRecord = this.FindAndEnsureFactoryRecord(configKey, out flag5);
					}
					else
					{
						factoryRecord = this.GetFactoryRecord(configKey, false);
						if (factoryRecord == null)
						{
							flag5 = false;
						}
						else
						{
							factoryRecord = this.FindAndEnsureFactoryRecord(configKey, out flag5);
						}
					}
					if (flag5)
					{
						flag4 = true;
					}
					if (sectionRecord == null && flag4)
					{
						sectionRecord = this.EnsureSectionRecord(configKey, true);
					}
					bool getRuntimeObject2 = getRuntimeObject && !flag3;
					object obj3 = null;
					object obj4 = null;
					if (flag5)
					{
						SectionRecord sectionRecord2 = flag3 ? null : sectionRecord;
						this.CreateSectionDefault(configKey, getRuntimeObject2, factoryRecord, sectionRecord2, out obj3, out obj4);
					}
					else
					{
						this._parent.GetSectionRecursive(configKey, false, false, getRuntimeObject2, false, out obj3, out obj4);
					}
					if (flag3)
					{
						if (!this.Evaluate(factoryRecord, sectionRecord, obj3, getLkg, getRuntimeObject, out obj, out obj2))
						{
							flag4 = false;
						}
					}
					else if (sectionRecord != null)
					{
						obj = this.UseParentResult(configKey, obj3, sectionRecord);
						if (getRuntimeObject)
						{
							if (object.ReferenceEquals(obj3, obj4))
							{
								obj2 = obj;
							}
							else
							{
								obj2 = this.UseParentResult(configKey, obj4, sectionRecord);
							}
						}
					}
					else
					{
						obj = obj3;
						obj2 = obj4;
					}
					if (flag4 || checkPermission)
					{
						requirePermission = factoryRecord.RequirePermission;
						flag = factoryRecord.IsFactoryTrustedWithoutAptca;
						if (flag4)
						{
							if (sectionRecord == null)
							{
								sectionRecord = this.EnsureSectionRecord(configKey, true);
							}
							sectionRecord.Result = obj;
							if (getRuntimeObject)
							{
								sectionRecord.ResultRuntimeObject = obj2;
							}
							sectionRecord.RequirePermission = requirePermission;
							sectionRecord.IsResultTrustedWithoutAptca = flag;
						}
					}
					flag2 = true;
				}
				catch
				{
					if (!getLkg)
					{
						throw;
					}
				}
				if (flag2)
				{
					goto IL_2D3;
				}
				this._parent.GetSectionRecursive(configKey, true, checkPermission, true, true, out result, out resultRuntimeObject);
				return;
			}
			IL_2D3:
			if (checkPermission)
			{
				this.CheckPermissionAllowed(configKey, requirePermission, flag);
			}
			result = obj;
			if (getRuntimeObject)
			{
				resultRuntimeObject = obj2;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000071C0 File Offset: 0x000061C0
		private static bool NeverLoadUserConfigFilesDuringFactorySearch(string configKey)
		{
			return !BaseConfigurationRecord.CanLoadUserConfigFilesWhenSearchingForDatasetSerializationAllowedTypes() && configKey == "system.data.dataset.serialization/allowedTypes";
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000071D9 File Offset: 0x000061D9
		[SecurityTreatAsSafe]
		[SecurityCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool CanLoadUserConfigFilesWhenSearchingForDatasetSerializationAllowedTypes()
		{
			if (!BaseConfigurationRecord.s_allowDataSetSectionToLoadUserConfigValueInitialized)
			{
				BaseConfigurationRecord.s_allowDataSetSectionToLoadUserConfig = BaseConfigurationRecord.ReadUserConfigFileLoadRegistrySetting("Switch.System.Configuration.AllowUserConfigFilesToLoadWhenSearchingForDatasetSerializationAllowedTypes");
				BaseConfigurationRecord.s_allowDataSetSectionToLoadUserConfigValueInitialized = true;
			}
			return BaseConfigurationRecord.s_allowDataSetSectionToLoadUserConfig;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00007200 File Offset: 0x00006200
		private static bool ReadUserConfigFileLoadRegistrySetting(string switchName)
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AppContext", false))
				{
					if (registryKey != null && registryKey.GetValueKind(switchName) == RegistryValueKind.String && "true".Equals((string)registryKey.GetValue(switchName), StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00007278 File Offset: 0x00006278
		protected void CreateSectionDefault(string configKey, bool getRuntimeObject, FactoryRecord factoryRecord, SectionRecord sectionRecord, out object result, out object resultRuntimeObject)
		{
			result = null;
			resultRuntimeObject = null;
			SectionRecord sectionRecord2;
			if (sectionRecord != null)
			{
				sectionRecord2 = sectionRecord;
			}
			else
			{
				sectionRecord2 = new SectionRecord(configKey);
			}
			object obj = this.CallCreateSection(true, factoryRecord, sectionRecord2, null, null, null, -1);
			object obj2;
			if (getRuntimeObject)
			{
				obj2 = this.GetRuntimeObject(obj);
			}
			else
			{
				obj2 = null;
			}
			result = obj;
			resultRuntimeObject = obj2;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000072C2 File Offset: 0x000062C2
		private bool ShouldSkipDueToInheritInChildApplications(bool skipInChildApps)
		{
			return skipInChildApps && this._flags[32];
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000072D6 File Offset: 0x000062D6
		private bool ShouldSkipDueToInheritInChildApplications(bool skipInChildApps, string configPath)
		{
			return skipInChildApps && this.Host.IsAboveApplication(configPath);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000072EC File Offset: 0x000062EC
		private bool Evaluate(FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentResult, bool getLkg, bool getRuntimeObject, out object result, out object resultRuntimeObject)
		{
			result = null;
			resultRuntimeObject = null;
			object obj = null;
			object obj2 = null;
			List<SectionInput> locationInputs = sectionRecord.LocationInputs;
			List<SectionInput> indirectLocationInputs = sectionRecord.IndirectLocationInputs;
			SectionInput fileInput = sectionRecord.FileInput;
			bool flag = false;
			if (sectionRecord.HasResult)
			{
				if (getRuntimeObject && !sectionRecord.HasResultRuntimeObject)
				{
					try
					{
						sectionRecord.ResultRuntimeObject = this.GetRuntimeObject(sectionRecord.Result);
					}
					catch
					{
						if (!getLkg)
						{
							throw;
						}
					}
				}
				if (!getRuntimeObject || sectionRecord.HasResultRuntimeObject)
				{
					obj = sectionRecord.Result;
					if (getRuntimeObject)
					{
						obj2 = sectionRecord.ResultRuntimeObject;
					}
					flag = true;
				}
			}
			if (!flag)
			{
				Exception ex = null;
				try
				{
					string configKey = factoryRecord.ConfigKey;
					string[] keys = configKey.Split(BaseConfigurationRecord.ConfigPathSeparatorParams);
					object obj3 = parentResult;
					if (indirectLocationInputs != null)
					{
						foreach (SectionInput sectionInput in indirectLocationInputs)
						{
							if (!sectionInput.HasResult)
							{
								sectionInput.ThrowOnErrors();
								bool isTrusted = this.Host.IsTrustedConfigPath(sectionInput.SectionXmlInfo.DefinitionConfigPath);
								sectionInput.Result = this.EvaluateOne(keys, sectionInput, isTrusted, factoryRecord, sectionRecord, obj3);
							}
							obj3 = sectionInput.Result;
						}
					}
					if (locationInputs != null)
					{
						foreach (SectionInput sectionInput2 in locationInputs)
						{
							if (!sectionInput2.HasResult)
							{
								sectionInput2.ThrowOnErrors();
								bool isTrusted2 = this.Host.IsTrustedConfigPath(sectionInput2.SectionXmlInfo.DefinitionConfigPath);
								sectionInput2.Result = this.EvaluateOne(keys, sectionInput2, isTrusted2, factoryRecord, sectionRecord, obj3);
							}
							obj3 = sectionInput2.Result;
						}
					}
					if (fileInput != null)
					{
						if (!fileInput.HasResult)
						{
							fileInput.ThrowOnErrors();
							bool isTrusted3 = this._flags[8192];
							fileInput.Result = this.EvaluateOne(keys, fileInput, isTrusted3, factoryRecord, sectionRecord, obj3);
						}
						obj3 = fileInput.Result;
					}
					else
					{
						obj3 = this.UseParentResult(configKey, obj3, sectionRecord);
					}
					if (getRuntimeObject)
					{
						obj2 = this.GetRuntimeObject(obj3);
					}
					obj = obj3;
					flag = true;
				}
				catch (Exception ex2)
				{
					if (!getLkg || locationInputs == null)
					{
						throw;
					}
					ex = ex2;
				}
				if (!flag)
				{
					int num = locationInputs.Count;
					while (--num >= 0)
					{
						SectionInput sectionInput3 = locationInputs[num];
						if (sectionInput3.HasResult)
						{
							if (getRuntimeObject && !sectionInput3.HasResultRuntimeObject)
							{
								try
								{
									sectionInput3.ResultRuntimeObject = this.GetRuntimeObject(sectionInput3.Result);
								}
								catch
								{
								}
							}
							if (!getRuntimeObject || sectionInput3.HasResultRuntimeObject)
							{
								obj = sectionInput3.Result;
								if (getRuntimeObject)
								{
									obj2 = sectionInput3.ResultRuntimeObject;
									break;
								}
								break;
							}
						}
					}
					if (num < 0)
					{
						throw ex;
					}
				}
			}
			if (flag && !this._flags[524288])
			{
				sectionRecord.ClearRawXml();
			}
			result = obj;
			if (getRuntimeObject)
			{
				resultRuntimeObject = obj2;
			}
			return flag;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00007624 File Offset: 0x00006624
		private object EvaluateOne(string[] keys, SectionInput input, bool isTrusted, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentResult)
		{
			object result;
			try
			{
				ConfigXmlReader sectionXmlReader = this.GetSectionXmlReader(keys, input);
				if (sectionXmlReader == null)
				{
					result = this.UseParentResult(factoryRecord.ConfigKey, parentResult, sectionRecord);
				}
				else
				{
					result = this.CallCreateSection(isTrusted, factoryRecord, sectionRecord, parentResult, sectionXmlReader, input.SectionXmlInfo.Filename, input.SectionXmlInfo.LineNumber);
				}
			}
			catch (Exception e)
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_exception_creating_section", new object[]
				{
					factoryRecord.ConfigKey
				}), e, input.SectionXmlInfo);
			}
			catch
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_exception_creating_section", new object[]
				{
					factoryRecord.ConfigKey
				}), null, input.SectionXmlInfo);
			}
			return result;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000076EC File Offset: 0x000066EC
		private static ConfigurationPermission UnrestrictedConfigPermission
		{
			get
			{
				if (BaseConfigurationRecord.s_unrestrictedConfigPermission == null)
				{
					BaseConfigurationRecord.s_unrestrictedConfigPermission = new ConfigurationPermission(PermissionState.Unrestricted);
				}
				return BaseConfigurationRecord.s_unrestrictedConfigPermission;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007708 File Offset: 0x00006708
		private void CheckPermissionAllowed(string configKey, bool requirePermission, bool isTrustedWithoutAptca)
		{
			if (requirePermission)
			{
				try
				{
					BaseConfigurationRecord.UnrestrictedConfigPermission.Demand();
				}
				catch (SecurityException inner)
				{
					throw new SecurityException(SR.GetString("ConfigurationPermission_Denied", new object[]
					{
						configKey
					}), inner);
				}
			}
			if (isTrustedWithoutAptca && !this.Host.IsFullTrustSectionWithoutAptcaAllowed(this))
			{
				throw new ConfigurationErrorsException(SR.GetString("Section_from_untrusted_assembly", new object[]
				{
					configKey
				}));
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007780 File Offset: 0x00006780
		private ConfigXmlReader FindSection(string[] keys, SectionXmlInfo sectionXmlInfo, out int lineNumber)
		{
			lineNumber = 0;
			ConfigXmlReader configXmlReader = null;
			try
			{
				using (this.Impersonate())
				{
					using (Stream stream = this.Host.OpenStreamForRead(sectionXmlInfo.Filename))
					{
						if (!this._flags[131072] && (stream == null || this.HasStreamChanged(sectionXmlInfo.Filename, sectionXmlInfo.StreamVersion)))
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_file_has_changed"), sectionXmlInfo.Filename, 0);
						}
						if (stream != null)
						{
							using (XmlUtil xmlUtil = new XmlUtil(stream, sectionXmlInfo.Filename, true))
							{
								if (sectionXmlInfo.SubPath == null)
								{
									configXmlReader = this.FindSectionRecursive(keys, 0, xmlUtil, ref lineNumber);
								}
								else
								{
									xmlUtil.ReadToNextElement();
									while (xmlUtil.Reader.Depth > 0)
									{
										if (xmlUtil.Reader.Name == "location")
										{
											bool flag = false;
											string text = xmlUtil.Reader.GetAttribute("path");
											try
											{
												text = BaseConfigurationRecord.NormalizeLocationSubPath(text, xmlUtil);
												flag = true;
											}
											catch (ConfigurationException ce)
											{
												xmlUtil.SchemaErrors.AddError(ce, ExceptionAction.NonSpecific);
											}
											if (flag && StringUtil.EqualsIgnoreCase(sectionXmlInfo.SubPath, text))
											{
												configXmlReader = this.FindSectionRecursive(keys, 0, xmlUtil, ref lineNumber);
												if (configXmlReader != null)
												{
													break;
												}
											}
										}
										xmlUtil.SkipToNextElement();
									}
								}
								this.ThrowIfParseErrors(xmlUtil.SchemaErrors);
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
			return configXmlReader;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007954 File Offset: 0x00006954
		private ConfigXmlReader FindSectionRecursive(string[] keys, int iKey, XmlUtil xmlUtil, ref int lineNumber)
		{
			string b = keys[iKey];
			ConfigXmlReader configXmlReader = null;
			int depth = xmlUtil.Reader.Depth;
			xmlUtil.ReadToNextElement();
			while (xmlUtil.Reader.Depth > depth)
			{
				if (xmlUtil.Reader.Name == b)
				{
					if (iKey >= keys.Length - 1)
					{
						string filename = ((IConfigErrorInfo)xmlUtil).Filename;
						int lineNumber2 = xmlUtil.Reader.LineNumber;
						string rawXml = xmlUtil.CopySection();
						configXmlReader = new ConfigXmlReader(rawXml, filename, lineNumber2);
						break;
					}
					configXmlReader = this.FindSectionRecursive(keys, iKey + 1, xmlUtil, ref lineNumber);
					if (configXmlReader != null)
					{
						break;
					}
				}
				else
				{
					if (iKey == 0 && xmlUtil.Reader.Name == "location")
					{
						string text = xmlUtil.Reader.GetAttribute("path");
						bool flag = false;
						try
						{
							text = BaseConfigurationRecord.NormalizeLocationSubPath(text, xmlUtil);
							flag = true;
						}
						catch (ConfigurationException ce)
						{
							xmlUtil.SchemaErrors.AddError(ce, ExceptionAction.NonSpecific);
						}
						if (flag && text == null)
						{
							configXmlReader = this.FindSectionRecursive(keys, iKey, xmlUtil, ref lineNumber);
							if (configXmlReader != null)
							{
								break;
							}
							continue;
						}
					}
					xmlUtil.SkipToNextElement();
				}
			}
			return configXmlReader;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007A70 File Offset: 0x00006A70
		private ConfigXmlReader LoadConfigSource(string name, SectionXmlInfo sectionXmlInfo)
		{
			string configSourceStreamName = sectionXmlInfo.ConfigSourceStreamName;
			ConfigXmlReader result;
			try
			{
				using (this.Impersonate())
				{
					using (Stream stream = this.Host.OpenStreamForRead(configSourceStreamName))
					{
						if (stream == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_cannot_open_config_source", new object[]
							{
								sectionXmlInfo.ConfigSource
							}), sectionXmlInfo);
						}
						using (XmlUtil xmlUtil = new XmlUtil(stream, configSourceStreamName, true))
						{
							if (xmlUtil.Reader.Name != name)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_source_file_format"), xmlUtil);
							}
							string attribute = xmlUtil.Reader.GetAttribute("configProtectionProvider");
							if (attribute != null)
							{
								if (xmlUtil.Reader.AttributeCount != 1)
								{
									throw new ConfigurationErrorsException(SR.GetString("Protection_provider_syntax_error"), xmlUtil);
								}
								sectionXmlInfo.ProtectionProviderName = BaseConfigurationRecord.ValidateProtectionProviderAttribute(attribute, xmlUtil);
							}
							int lineNumber = xmlUtil.Reader.LineNumber;
							string rawXml = xmlUtil.CopySection();
							while (!xmlUtil.Reader.EOF)
							{
								XmlNodeType nodeType = xmlUtil.Reader.NodeType;
								if (nodeType != XmlNodeType.Comment)
								{
									throw new ConfigurationErrorsException(SR.GetString("Config_source_file_format"), xmlUtil);
								}
								xmlUtil.Reader.Read();
							}
							ConfigXmlReader configXmlReader = new ConfigXmlReader(rawXml, configSourceStreamName, lineNumber);
							result = configXmlReader;
						}
					}
				}
			}
			catch
			{
				throw;
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007C1C File Offset: 0x00006C1C
		protected ConfigXmlReader GetSectionXmlReader(string[] keys, SectionInput input)
		{
			ConfigXmlReader configXmlReader = null;
			string filename = input.SectionXmlInfo.Filename;
			int line = input.SectionXmlInfo.LineNumber;
			try
			{
				string name = keys[keys.Length - 1];
				string rawXml = input.SectionXmlInfo.RawXml;
				if (rawXml != null)
				{
					configXmlReader = new ConfigXmlReader(rawXml, input.SectionXmlInfo.Filename, input.SectionXmlInfo.LineNumber);
				}
				else if (!string.IsNullOrEmpty(input.SectionXmlInfo.ConfigSource))
				{
					filename = input.SectionXmlInfo.ConfigSourceStreamName;
					line = 0;
					configXmlReader = this.LoadConfigSource(name, input.SectionXmlInfo);
				}
				else
				{
					line = 0;
					configXmlReader = this.FindSection(keys, input.SectionXmlInfo, out line);
				}
				if (configXmlReader != null)
				{
					if (!input.IsProtectionProviderDetermined)
					{
						input.ProtectionProvider = this.GetProtectionProviderFromName(input.SectionXmlInfo.ProtectionProviderName, false);
					}
					if (input.ProtectionProvider != null)
					{
						configXmlReader = this.DecryptConfigSection(configXmlReader, input.ProtectionProvider);
					}
				}
			}
			catch (Exception e)
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), e, filename, line);
			}
			catch
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), null, filename, line);
			}
			return configXmlReader;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00007D40 File Offset: 0x00006D40
		internal string DefaultProviderName
		{
			get
			{
				return this.ProtectedConfig.DefaultProvider;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007D50 File Offset: 0x00006D50
		internal ProtectedConfigurationProvider GetProtectionProviderFromName(string providerName, bool throwIfNotFound)
		{
			if (!string.IsNullOrEmpty(providerName))
			{
				return this.ProtectedConfig.GetProviderFromName(providerName);
			}
			if (throwIfNotFound)
			{
				throw new ConfigurationErrorsException(SR.GetString("ProtectedConfigurationProvider_not_found", new object[]
				{
					providerName
				}));
			}
			return null;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00007D96 File Offset: 0x00006D96
		private ProtectedConfigurationSection ProtectedConfig
		{
			get
			{
				if (!this._flags[1])
				{
					this.InitProtectedConfigurationSection();
				}
				return this._protectedConfig;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007DB2 File Offset: 0x00006DB2
		internal void InitProtectedConfigurationSection()
		{
			if (!this._flags[1])
			{
				this._protectedConfig = (this.GetSection("configProtectedData", false, false) as ProtectedConfigurationSection);
				this._flags[1] = true;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007DE8 File Offset: 0x00006DE8
		protected object CallCreateSection(bool inputIsTrusted, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader, string filename, int line)
		{
			object obj;
			try
			{
				using (this.Impersonate())
				{
					obj = this.CreateSection(inputIsTrusted, factoryRecord, sectionRecord, parentConfig, reader);
					if (obj == null && parentConfig != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_object_is_null"), filename, line);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_exception_creating_section_handler", new object[]
				{
					factoryRecord.ConfigKey
				}), e, filename, line);
			}
			catch
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_exception_creating_section_handler", new object[]
				{
					factoryRecord.ConfigKey
				}), null, filename, line);
			}
			return obj;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007EBC File Offset: 0x00006EBC
		internal bool IsRootDeclaration(string configKey, bool implicitIsRooted)
		{
			return (implicitIsRooted || !BaseConfigurationRecord.IsImplicitSection(configKey)) && (this._parent.IsRootConfig || this._parent.FindFactoryRecord(configKey, true) == null);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007EEC File Offset: 0x00006EEC
		internal FactoryRecord FindFactoryRecord(string configKey, bool permitErrors, out BaseConfigurationRecord configRecord)
		{
			configRecord = null;
			BaseConfigurationRecord baseConfigurationRecord = this;
			while (!baseConfigurationRecord.IsRootConfig)
			{
				FactoryRecord factoryRecord = baseConfigurationRecord.GetFactoryRecord(configKey, permitErrors);
				if (factoryRecord != null)
				{
					configRecord = baseConfigurationRecord;
					return factoryRecord;
				}
				baseConfigurationRecord = baseConfigurationRecord._parent;
			}
			return null;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00007F24 File Offset: 0x00006F24
		internal FactoryRecord FindFactoryRecord(string configKey, bool permitErrors)
		{
			BaseConfigurationRecord baseConfigurationRecord;
			return this.FindFactoryRecord(configKey, permitErrors, out baseConfigurationRecord);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00007F3C File Offset: 0x00006F3C
		private FactoryRecord FindAndEnsureFactoryRecord(string configKey, out bool isRootDeclaredHere)
		{
			isRootDeclaredHere = false;
			BaseConfigurationRecord baseConfigurationRecord;
			FactoryRecord factoryRecord = this.FindFactoryRecord(configKey, false, out baseConfigurationRecord);
			if (factoryRecord != null && !factoryRecord.IsGroup)
			{
				FactoryRecord factoryRecord2 = factoryRecord;
				BaseConfigurationRecord baseConfigurationRecord2 = baseConfigurationRecord;
				BaseConfigurationRecord parent = baseConfigurationRecord._parent;
				while (!parent.IsRootConfig)
				{
					BaseConfigurationRecord baseConfigurationRecord3;
					FactoryRecord factoryRecord3 = parent.FindFactoryRecord(configKey, false, out baseConfigurationRecord3);
					if (factoryRecord3 == null)
					{
						break;
					}
					factoryRecord2 = factoryRecord3;
					baseConfigurationRecord2 = baseConfigurationRecord3;
					parent = baseConfigurationRecord3.Parent;
				}
				if (factoryRecord2.Factory == null)
				{
					try
					{
						object obj = baseConfigurationRecord2.CreateSectionFactory(factoryRecord2);
						bool isFactoryTrustedWithoutAptca = TypeUtil.IsTypeFromTrustedAssemblyWithoutAptca(obj.GetType());
						factoryRecord2.Factory = obj;
						factoryRecord2.IsFactoryTrustedWithoutAptca = isFactoryTrustedWithoutAptca;
					}
					catch (Exception e)
					{
						throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_exception_creating_section_handler", new object[]
						{
							factoryRecord.ConfigKey
						}), e, factoryRecord);
					}
					catch
					{
						throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_exception_creating_section_handler", new object[]
						{
							factoryRecord.ConfigKey
						}), null, factoryRecord);
					}
				}
				if (factoryRecord.Factory == null)
				{
					factoryRecord.Factory = factoryRecord2.Factory;
					factoryRecord.IsFactoryTrustedWithoutAptca = factoryRecord2.IsFactoryTrustedWithoutAptca;
				}
				isRootDeclaredHere = object.ReferenceEquals(this, baseConfigurationRecord2);
			}
			return factoryRecord;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00008068 File Offset: 0x00007068
		private Hashtable ScanFactories(XmlUtil xmlUtil)
		{
			Hashtable hashtable = new Hashtable();
			if (xmlUtil.Reader.NodeType != XmlNodeType.Element || xmlUtil.Reader.Name != "configuration")
			{
				string text = ConfigurationErrorsException.AlwaysSafeFilename(((IConfigErrorInfo)xmlUtil).Filename);
				throw new ConfigurationErrorsException(SR.GetString("Config_file_doesnt_have_root_configuration", new object[]
				{
					text
				}), xmlUtil);
			}
			while (xmlUtil.Reader.MoveToNextAttribute())
			{
				string name;
				if ((name = xmlUtil.Reader.Name) != null && name == "xmlns")
				{
					if (xmlUtil.Reader.Value == "http://schemas.microsoft.com/.NetConfiguration/v2.0")
					{
						this._flags[512] = true;
						this._flags[67108864] = true;
					}
					else
					{
						ConfigurationErrorsException ce = new ConfigurationErrorsException(SR.GetString("Config_namespace_invalid", new object[]
						{
							xmlUtil.Reader.Value,
							"http://schemas.microsoft.com/.NetConfiguration/v2.0"
						}), xmlUtil);
						xmlUtil.SchemaErrors.AddError(ce, ExceptionAction.Global);
					}
				}
				else
				{
					xmlUtil.AddErrorUnrecognizedAttribute(ExceptionAction.NonSpecific);
				}
			}
			xmlUtil.StrictReadToNextElement(ExceptionAction.NonSpecific);
			if (xmlUtil.Reader.Depth == 1 && xmlUtil.Reader.Name == "configSections")
			{
				xmlUtil.VerifyNoUnrecognizedAttributes(ExceptionAction.NonSpecific);
				this.ScanFactoriesRecursive(xmlUtil, string.Empty, hashtable);
			}
			return hashtable;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000081C4 File Offset: 0x000071C4
		private void ScanFactoriesRecursive(XmlUtil xmlUtil, string parentConfigKey, Hashtable factoryList)
		{
			xmlUtil.SchemaErrors.ResetLocalErrors();
			int depth = xmlUtil.Reader.Depth;
			xmlUtil.StrictReadToNextElement(ExceptionAction.NonSpecific);
			while (xmlUtil.Reader.Depth == depth + 1)
			{
				bool flag = false;
				string name;
				if ((name = xmlUtil.Reader.Name) == null)
				{
					goto IL_647;
				}
				if (!(name == "sectionGroup"))
				{
					if (!(name == "section"))
					{
						if (!(name == "remove"))
						{
							if (!(name == "clear"))
							{
								goto IL_647;
							}
							xmlUtil.VerifyNoUnrecognizedAttributes(ExceptionAction.NonSpecific);
						}
						else
						{
							string text = null;
							while (xmlUtil.Reader.MoveToNextAttribute())
							{
								if (xmlUtil.Reader.Name != "name")
								{
									xmlUtil.AddErrorUnrecognizedAttribute(ExceptionAction.NonSpecific);
								}
								text = xmlUtil.Reader.Value;
								int lineNumber = xmlUtil.Reader.LineNumber;
							}
							xmlUtil.Reader.MoveToElement();
							if (xmlUtil.VerifyRequiredAttribute(text, "name", ExceptionAction.NonSpecific))
							{
								BaseConfigurationRecord.VerifySectionName(text, xmlUtil, ExceptionAction.NonSpecific, false);
							}
						}
					}
					else
					{
						string text2 = null;
						string text3 = null;
						ConfigurationAllowDefinition allowDefinition = ConfigurationAllowDefinition.Everywhere;
						ConfigurationAllowExeDefinition allowExeDefinition = ConfigurationAllowExeDefinition.MachineToApplication;
						OverrideModeSetting overrideModeDefault = OverrideModeSetting.SectionDefault;
						bool allowLocation = true;
						bool restartOnExternalChanges = true;
						bool requirePermission = true;
						bool flag2 = false;
						int lineNumber2 = xmlUtil.Reader.LineNumber;
						while (xmlUtil.Reader.MoveToNextAttribute())
						{
							string name2;
							if ((name2 = xmlUtil.Reader.Name) != null)
							{
								if (<PrivateImplementationDetails>{1D93F23B-FFCA-4959-ABDC-265807E4242D}.$$method0x60000b0-1 == null)
								{
									<PrivateImplementationDetails>{1D93F23B-FFCA-4959-ABDC-265807E4242D}.$$method0x60000b0-1 = new Dictionary<string, int>(8)
									{
										{
											"name",
											0
										},
										{
											"type",
											1
										},
										{
											"allowLocation",
											2
										},
										{
											"allowExeDefinition",
											3
										},
										{
											"allowDefinition",
											4
										},
										{
											"restartOnExternalChanges",
											5
										},
										{
											"requirePermission",
											6
										},
										{
											"overrideModeDefault",
											7
										}
									};
								}
								int num;
								if (<PrivateImplementationDetails>{1D93F23B-FFCA-4959-ABDC-265807E4242D}.$$method0x60000b0-1.TryGetValue(name2, out num))
								{
									switch (num)
									{
									case 0:
										text2 = xmlUtil.Reader.Value;
										BaseConfigurationRecord.VerifySectionName(text2, xmlUtil, ExceptionAction.Local, false);
										continue;
									case 1:
										xmlUtil.VerifyAndGetNonEmptyStringAttribute(ExceptionAction.Local, out text3);
										flag2 = true;
										continue;
									case 2:
										xmlUtil.VerifyAndGetBooleanAttribute(ExceptionAction.Local, true, out allowLocation);
										continue;
									case 3:
										try
										{
											allowExeDefinition = BaseConfigurationRecord.AllowExeDefinitionToEnum(xmlUtil.Reader.Value, xmlUtil);
											continue;
										}
										catch (ConfigurationException ce)
										{
											xmlUtil.SchemaErrors.AddError(ce, ExceptionAction.Local);
											continue;
										}
										break;
									case 4:
										break;
									case 5:
										goto IL_3C5;
									case 6:
										xmlUtil.VerifyAndGetBooleanAttribute(ExceptionAction.Local, true, out requirePermission);
										continue;
									case 7:
										try
										{
											overrideModeDefault = OverrideModeSetting.CreateFromXmlReadValue(OverrideModeSetting.ParseOverrideModeXmlValue(xmlUtil.Reader.Value, xmlUtil));
											if (overrideModeDefault.OverrideMode == OverrideMode.Inherit)
											{
												overrideModeDefault.ChangeModeInternal(OverrideMode.Allow);
											}
											continue;
										}
										catch (ConfigurationException ce2)
										{
											xmlUtil.SchemaErrors.AddError(ce2, ExceptionAction.Local);
											continue;
										}
										goto IL_41A;
									default:
										goto IL_41A;
									}
									try
									{
										allowDefinition = BaseConfigurationRecord.AllowDefinitionToEnum(xmlUtil.Reader.Value, xmlUtil);
										continue;
									}
									catch (ConfigurationException ce3)
									{
										xmlUtil.SchemaErrors.AddError(ce3, ExceptionAction.Local);
										continue;
									}
									IL_3C5:
									xmlUtil.VerifyAndGetBooleanAttribute(ExceptionAction.Local, true, out restartOnExternalChanges);
									continue;
								}
							}
							IL_41A:
							xmlUtil.AddErrorUnrecognizedAttribute(ExceptionAction.Local);
						}
						xmlUtil.Reader.MoveToElement();
						if (!xmlUtil.VerifyRequiredAttribute(text2, "name", ExceptionAction.NonSpecific))
						{
							xmlUtil.SchemaErrors.RetrieveAndResetLocalErrors(true);
						}
						else
						{
							if (!flag2)
							{
								xmlUtil.AddErrorRequiredAttribute("type", ExceptionAction.Local);
							}
							string text4 = BaseConfigurationRecord.CombineConfigKey(parentConfigKey, text2);
							FactoryRecord factoryRecord = (FactoryRecord)factoryList[text4];
							if (factoryRecord != null)
							{
								xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined_at_this_level", new object[]
								{
									text2
								}), xmlUtil), ExceptionAction.Local);
							}
							else
							{
								FactoryRecord factoryRecord2 = this._parent.FindFactoryRecord(text4, true);
								if (factoryRecord2 != null)
								{
									text4 = factoryRecord2.ConfigKey;
									if (factoryRecord2.IsGroup)
									{
										xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[]
										{
											text2
										}), xmlUtil), ExceptionAction.Local);
										factoryRecord2 = null;
									}
									else if (!factoryRecord2.IsEquivalentSectionFactory(this.Host, text3, allowLocation, allowDefinition, allowExeDefinition, restartOnExternalChanges, requirePermission))
									{
										xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[]
										{
											text2
										}), xmlUtil), ExceptionAction.Local);
										factoryRecord2 = null;
									}
								}
								if (factoryRecord2 != null)
								{
									factoryRecord = factoryRecord2.CloneSection(xmlUtil.Filename, lineNumber2);
								}
								else
								{
									factoryRecord = new FactoryRecord(text4, parentConfigKey, text2, text3, allowLocation, allowDefinition, allowExeDefinition, overrideModeDefault, restartOnExternalChanges, requirePermission, this._flags[8192], false, xmlUtil.Filename, lineNumber2);
								}
								factoryList[text4] = factoryRecord;
							}
							factoryRecord.AddErrors(xmlUtil.SchemaErrors.RetrieveAndResetLocalErrors(true));
						}
					}
				}
				else
				{
					string text5 = null;
					string text6 = null;
					int lineNumber3 = xmlUtil.Reader.LineNumber;
					while (xmlUtil.Reader.MoveToNextAttribute())
					{
						string name3;
						if ((name3 = xmlUtil.Reader.Name) != null)
						{
							if (name3 == "name")
							{
								text5 = xmlUtil.Reader.Value;
								BaseConfigurationRecord.VerifySectionName(text5, xmlUtil, ExceptionAction.Local, false);
								continue;
							}
							if (name3 == "type")
							{
								xmlUtil.VerifyAndGetNonEmptyStringAttribute(ExceptionAction.Local, out text6);
								continue;
							}
						}
						xmlUtil.AddErrorUnrecognizedAttribute(ExceptionAction.Local);
					}
					xmlUtil.Reader.MoveToElement();
					if (!xmlUtil.VerifyRequiredAttribute(text5, "name", ExceptionAction.NonSpecific))
					{
						xmlUtil.SchemaErrors.RetrieveAndResetLocalErrors(true);
						xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
						continue;
					}
					string text7 = BaseConfigurationRecord.CombineConfigKey(parentConfigKey, text5);
					FactoryRecord factoryRecord3 = (FactoryRecord)factoryList[text7];
					if (factoryRecord3 != null)
					{
						xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined_at_this_level", new object[]
						{
							text5
						}), xmlUtil), ExceptionAction.Local);
					}
					else
					{
						FactoryRecord factoryRecord4 = this._parent.FindFactoryRecord(text7, true);
						if (factoryRecord4 != null)
						{
							text7 = factoryRecord4.ConfigKey;
							if (factoryRecord4 != null && (!factoryRecord4.IsGroup || !factoryRecord4.IsEquivalentSectionGroupFactory(this.Host, text6)))
							{
								xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[]
								{
									text5
								}), xmlUtil), ExceptionAction.Local);
								factoryRecord4 = null;
							}
						}
						if (factoryRecord4 != null)
						{
							factoryRecord3 = factoryRecord4.CloneSectionGroup(text6, xmlUtil.Filename, lineNumber3);
						}
						else
						{
							factoryRecord3 = new FactoryRecord(text7, parentConfigKey, text5, text6, xmlUtil.Filename, lineNumber3);
						}
						factoryList[text7] = factoryRecord3;
					}
					factoryRecord3.AddErrors(xmlUtil.SchemaErrors.RetrieveAndResetLocalErrors(true));
					this.ScanFactoriesRecursive(xmlUtil, text7, factoryList);
					continue;
				}
				IL_657:
				if (flag)
				{
					continue;
				}
				xmlUtil.StrictReadToNextElement(ExceptionAction.NonSpecific);
				if (xmlUtil.Reader.Depth > depth + 1)
				{
					xmlUtil.AddErrorUnrecognizedElement(ExceptionAction.NonSpecific);
					while (xmlUtil.Reader.Depth > depth + 1)
					{
						xmlUtil.ReadToNextElement();
					}
					continue;
				}
				continue;
				IL_647:
				xmlUtil.AddErrorUnrecognizedElement(ExceptionAction.NonSpecific);
				xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
				flag = true;
				goto IL_657;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000889C File Offset: 0x0000789C
		internal static ConfigurationAllowExeDefinition AllowExeDefinitionToEnum(string allowExeDefinition, XmlUtil xmlUtil)
		{
			if (allowExeDefinition != null)
			{
				if (allowExeDefinition == "MachineOnly")
				{
					return ConfigurationAllowExeDefinition.MachineOnly;
				}
				if (allowExeDefinition == "MachineToApplication")
				{
					return ConfigurationAllowExeDefinition.MachineToApplication;
				}
				if (allowExeDefinition == "MachineToRoamingUser")
				{
					return ConfigurationAllowExeDefinition.MachineToRoamingUser;
				}
				if (allowExeDefinition == "MachineToLocalUser")
				{
					return ConfigurationAllowExeDefinition.MachineToLocalUser;
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_section_allow_exe_definition_attribute_invalid"), xmlUtil);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00008908 File Offset: 0x00007908
		internal static ConfigurationAllowDefinition AllowDefinitionToEnum(string allowDefinition, XmlUtil xmlUtil)
		{
			string value;
			if ((value = xmlUtil.Reader.Value) != null)
			{
				if (value == "Everywhere")
				{
					return ConfigurationAllowDefinition.Everywhere;
				}
				if (value == "MachineOnly")
				{
					return ConfigurationAllowDefinition.MachineOnly;
				}
				if (value == "MachineToApplication")
				{
					return ConfigurationAllowDefinition.MachineToApplication;
				}
				if (value == "MachineToWebRoot")
				{
					return ConfigurationAllowDefinition.MachineToWebRoot;
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_section_allow_definition_attribute_invalid"), xmlUtil);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000897B File Offset: 0x0000797B
		internal static string CombineConfigKey(string parentConfigKey, string tagName)
		{
			if (string.IsNullOrEmpty(parentConfigKey))
			{
				return tagName;
			}
			if (string.IsNullOrEmpty(tagName))
			{
				return parentConfigKey;
			}
			return parentConfigKey + "/" + tagName;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000089A0 File Offset: 0x000079A0
		internal static void SplitConfigKey(string configKey, out string group, out string name)
		{
			int num = configKey.LastIndexOf('/');
			if (num == -1)
			{
				group = string.Empty;
				name = configKey;
				return;
			}
			group = configKey.Substring(0, num);
			name = configKey.Substring(num + 1);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000089DC File Offset: 0x000079DC
		[Conditional("DBG")]
		private void DebugValidateIndirectInputs(SectionRecord sectionRecord)
		{
			if (this._parent.IsRootConfig)
			{
				return;
			}
			for (int i = sectionRecord.IndirectLocationInputs.Count - 1; i >= 0; i--)
			{
				SectionInput sectionInput = sectionRecord.IndirectLocationInputs[i];
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00008A1C File Offset: 0x00007A1C
		private OverrideMode ResolveOverrideModeFromParent(string configKey, out OverrideMode childLockMode)
		{
			OverrideMode overrideMode = OverrideMode.Inherit;
			BaseConfigurationRecord parent = this.Parent;
			BaseConfigurationRecord parent2 = this.Parent;
			childLockMode = OverrideMode.Inherit;
			while (!parent.IsRootConfig && overrideMode == OverrideMode.Inherit)
			{
				SectionRecord sectionRecord = parent.GetSectionRecord(configKey, true);
				if (sectionRecord != null)
				{
					if (this.IsLocationConfig && object.ReferenceEquals(parent2, parent))
					{
						overrideMode = (sectionRecord.Locked ? OverrideMode.Deny : OverrideMode.Allow);
						childLockMode = (sectionRecord.LockChildren ? OverrideMode.Deny : OverrideMode.Allow);
					}
					else
					{
						overrideMode = (sectionRecord.LockChildren ? OverrideMode.Deny : OverrideMode.Allow);
						childLockMode = overrideMode;
					}
				}
				parent = parent._parent;
			}
			if (overrideMode == OverrideMode.Inherit)
			{
				OverrideMode overrideMode2 = this.FindFactoryRecord(configKey, true).OverrideModeDefault.OverrideMode;
				bool flag;
				if (this.IsLocationConfig)
				{
					flag = (this.Parent.GetFactoryRecord(configKey, true) != null);
				}
				else
				{
					flag = (this.GetFactoryRecord(configKey, true) != null);
				}
				if (!flag)
				{
					overrideMode = (childLockMode = overrideMode2);
				}
				else
				{
					overrideMode = OverrideMode.Allow;
					childLockMode = overrideMode2;
				}
			}
			return overrideMode;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008AFC File Offset: 0x00007AFC
		protected OverrideMode GetSectionLockedMode(string configKey)
		{
			OverrideMode overrideMode = OverrideMode.Inherit;
			return this.GetSectionLockedMode(configKey, out overrideMode);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00008B14 File Offset: 0x00007B14
		protected OverrideMode GetSectionLockedMode(string configKey, out OverrideMode childLockMode)
		{
			SectionRecord sectionRecord = this.GetSectionRecord(configKey, true);
			OverrideMode result;
			if (sectionRecord != null)
			{
				result = (sectionRecord.Locked ? OverrideMode.Deny : OverrideMode.Allow);
				childLockMode = (sectionRecord.LockChildren ? OverrideMode.Deny : OverrideMode.Allow);
			}
			else
			{
				result = this.ResolveOverrideModeFromParent(configKey, out childLockMode);
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008B56 File Offset: 0x00007B56
		private void ScanSections(XmlUtil xmlUtil)
		{
			this.ScanSectionsRecursive(xmlUtil, string.Empty, false, null, OverrideModeSetting.LocationDefault, false);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00008B6C File Offset: 0x00007B6C
		private void ScanSectionsRecursive(XmlUtil xmlUtil, string parentConfigKey, bool inLocation, string locationSubPath, OverrideModeSetting overrideMode, bool skipInChildApps)
		{
			xmlUtil.SchemaErrors.ResetLocalErrors();
			int num;
			if (parentConfigKey.Length == 0 && !inLocation)
			{
				num = 0;
			}
			else
			{
				num = xmlUtil.Reader.Depth;
				xmlUtil.StrictReadToNextElement(ExceptionAction.NonSpecific);
			}
			while (xmlUtil.Reader.Depth == num + 1)
			{
				string name = xmlUtil.Reader.Name;
				if (name == "configSections")
				{
					xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_client_config_too_many_configsections_elements", new object[]
					{
						name
					}), xmlUtil), ExceptionAction.NonSpecific);
					xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
				}
				else if (name == "location")
				{
					if (parentConfigKey.Length > 0 || inLocation)
					{
						xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_location_location_not_allowed"), xmlUtil), ExceptionAction.Global);
						xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
					}
					else
					{
						this.ScanLocationSection(xmlUtil);
					}
				}
				else
				{
					string text = BaseConfigurationRecord.CombineConfigKey(parentConfigKey, name);
					FactoryRecord factoryRecord = this.FindFactoryRecord(text, true);
					if (factoryRecord == null)
					{
						if (!this.ClassFlags[64])
						{
							xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_unrecognized_configuration_section", new object[]
							{
								text
							}), xmlUtil), ExceptionAction.Local);
						}
						BaseConfigurationRecord.VerifySectionName(name, xmlUtil, ExceptionAction.Local, false);
						factoryRecord = new FactoryRecord(text, parentConfigKey, name, typeof(DefaultSection).AssemblyQualifiedName, true, ConfigurationAllowDefinition.Everywhere, ConfigurationAllowExeDefinition.MachineToRoamingUser, OverrideModeSetting.SectionDefault, true, true, this._flags[8192], true, null, -1);
						factoryRecord.AddErrors(xmlUtil.SchemaErrors.RetrieveAndResetLocalErrors(true));
						this.EnsureFactories()[text] = factoryRecord;
					}
					if (factoryRecord.IsGroup)
					{
						if (factoryRecord.HasErrors)
						{
							xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
						}
						else
						{
							if (xmlUtil.Reader.AttributeCount > 0)
							{
								while (xmlUtil.Reader.MoveToNextAttribute())
								{
									if (BaseConfigurationRecord.IsReservedAttributeName(xmlUtil.Reader.Name))
									{
										xmlUtil.AddErrorReservedAttribute(ExceptionAction.NonSpecific);
									}
								}
								xmlUtil.Reader.MoveToElement();
							}
							this.ScanSectionsRecursive(xmlUtil, text, inLocation, locationSubPath, overrideMode, skipInChildApps);
						}
					}
					else
					{
						text = factoryRecord.ConfigKey;
						string filename = xmlUtil.Filename;
						int lineNumber = xmlUtil.LineNumber;
						string rawXml = null;
						string text2 = null;
						string text3 = null;
						object configSourceStreamVersion = null;
						string protectionProviderName = null;
						OverrideMode overrideMode2 = OverrideMode.Inherit;
						OverrideMode forChildren = OverrideMode.Inherit;
						bool flag = false;
						bool flag2 = locationSubPath == null;
						if (!factoryRecord.HasErrors)
						{
							if (inLocation && !factoryRecord.AllowLocation)
							{
								xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_section_cannot_be_used_in_location"), xmlUtil), ExceptionAction.Local);
							}
							if (flag2)
							{
								SectionRecord sectionRecord = this.GetSectionRecord(text, true);
								if (sectionRecord != null && sectionRecord.HasFileInput && !factoryRecord.IsIgnorable())
								{
									xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_sections_must_be_unique"), xmlUtil), ExceptionAction.Local);
								}
								try
								{
									this.VerifyDefinitionAllowed(factoryRecord, this._configPath, xmlUtil);
								}
								catch (ConfigurationException ce)
								{
									xmlUtil.SchemaErrors.AddError(ce, ExceptionAction.Local);
								}
							}
							overrideMode2 = this.GetSectionLockedMode(text, out forChildren);
							if (overrideMode2 == OverrideMode.Deny)
							{
								xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_section_locked"), xmlUtil), ExceptionAction.Local);
							}
							if (xmlUtil.Reader.AttributeCount >= 1)
							{
								string attribute = xmlUtil.Reader.GetAttribute("configSource");
								if (attribute != null)
								{
									try
									{
										text2 = BaseConfigurationRecord.NormalizeConfigSource(attribute, xmlUtil);
									}
									catch (ConfigurationException ce2)
									{
										xmlUtil.SchemaErrors.AddError(ce2, ExceptionAction.Local);
									}
									if (xmlUtil.Reader.AttributeCount != 1)
									{
										xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_source_syntax_error"), xmlUtil), ExceptionAction.Local);
									}
								}
								string attribute2 = xmlUtil.Reader.GetAttribute("configProtectionProvider");
								if (attribute2 != null)
								{
									try
									{
										protectionProviderName = BaseConfigurationRecord.ValidateProtectionProviderAttribute(attribute2, xmlUtil);
									}
									catch (ConfigurationException ce3)
									{
										xmlUtil.SchemaErrors.AddError(ce3, ExceptionAction.Local);
									}
									if (xmlUtil.Reader.AttributeCount != 1)
									{
										xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Protection_provider_syntax_error"), xmlUtil), ExceptionAction.Local);
									}
								}
								if (attribute != null && !xmlUtil.Reader.IsEmptyElement)
								{
									while (xmlUtil.Reader.Read())
									{
										XmlNodeType nodeType = xmlUtil.Reader.NodeType;
										if (nodeType == XmlNodeType.EndElement)
										{
											break;
										}
										if (nodeType != XmlNodeType.Comment)
										{
											xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Config_source_syntax_error"), xmlUtil), ExceptionAction.Local);
											if (nodeType == XmlNodeType.Element)
											{
												xmlUtil.StrictSkipToOurParentsEndElement(ExceptionAction.NonSpecific);
											}
											else
											{
												xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
											}
											flag = true;
											break;
										}
									}
								}
							}
							if (text2 != null)
							{
								try
								{
									try
									{
										text3 = this.Host.GetStreamNameForConfigSource(this.ConfigStreamInfo.StreamName, text2);
									}
									catch (Exception e)
									{
										throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_source_invalid"), e, xmlUtil);
									}
									this.ValidateUniqueConfigSource(text, text3, text2, xmlUtil);
									configSourceStreamVersion = this.MonitorStream(text, text2, text3);
								}
								catch (ConfigurationException ce4)
								{
									xmlUtil.SchemaErrors.AddError(ce4, ExceptionAction.Local);
								}
							}
							if (!xmlUtil.SchemaErrors.HasLocalErrors && text2 == null && this.ShouldPrefetchRawXml(factoryRecord))
							{
								rawXml = xmlUtil.CopySection();
								if (xmlUtil.Reader.NodeType != XmlNodeType.Element)
								{
									xmlUtil.VerifyIgnorableNodeType(ExceptionAction.NonSpecific);
									xmlUtil.StrictReadToNextElement(ExceptionAction.NonSpecific);
								}
								flag = true;
							}
						}
						List<ConfigurationException> errors = xmlUtil.SchemaErrors.RetrieveAndResetLocalErrors(flag2);
						if (!flag)
						{
							xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
						}
						bool flag3 = true;
						if (flag2)
						{
							if (this.ShouldSkipDueToInheritInChildApplications(skipInChildApps))
							{
								flag3 = false;
							}
						}
						else if (!this._flags[1048576])
						{
							flag3 = false;
						}
						if (flag3)
						{
							string targetConfigPath = (locationSubPath == null) ? this._configPath : null;
							SectionXmlInfo sectionXmlInfo = new SectionXmlInfo(text, this._configPath, targetConfigPath, locationSubPath, filename, lineNumber, this.ConfigStreamInfo.StreamVersion, rawXml, text2, text3, configSourceStreamVersion, protectionProviderName, overrideMode, skipInChildApps);
							if (locationSubPath == null)
							{
								SectionRecord sectionRecord2 = this.EnsureSectionRecordUnsafe(text, true);
								sectionRecord2.ChangeLockSettings(overrideMode2, forChildren);
								SectionInput sectionInput = new SectionInput(sectionXmlInfo, errors);
								sectionRecord2.AddFileInput(sectionInput);
							}
							else
							{
								LocationSectionRecord value = new LocationSectionRecord(sectionXmlInfo, errors);
								this.EnsureLocationSections().Add(value);
							}
						}
					}
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00009170 File Offset: 0x00008170
		private void ScanLocationSection(XmlUtil xmlUtil)
		{
			string text = null;
			bool flag = true;
			int globalErrorCount = xmlUtil.SchemaErrors.GlobalErrorCount;
			OverrideModeSetting overrideMode = OverrideModeSetting.LocationDefault;
			bool flag2 = false;
			while (xmlUtil.Reader.MoveToNextAttribute())
			{
				string name;
				if ((name = xmlUtil.Reader.Name) != null)
				{
					if (name == "path")
					{
						text = xmlUtil.Reader.Value;
						continue;
					}
					if (!(name == "allowOverride"))
					{
						if (!(name == "overrideMode"))
						{
							if (name == "inheritInChildApplications")
							{
								xmlUtil.VerifyAndGetBooleanAttribute(ExceptionAction.Global, true, out flag);
								continue;
							}
						}
						else
						{
							if (flag2)
							{
								xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Invalid_override_mode_declaration"), xmlUtil), ExceptionAction.Global);
								continue;
							}
							overrideMode = OverrideModeSetting.CreateFromXmlReadValue(OverrideModeSetting.ParseOverrideModeXmlValue(xmlUtil.Reader.Value, xmlUtil));
							flag2 = true;
							continue;
						}
					}
					else
					{
						if (flag2)
						{
							xmlUtil.SchemaErrors.AddError(new ConfigurationErrorsException(SR.GetString("Invalid_override_mode_declaration"), xmlUtil), ExceptionAction.Global);
							continue;
						}
						bool allowOverride = true;
						xmlUtil.VerifyAndGetBooleanAttribute(ExceptionAction.Global, true, out allowOverride);
						overrideMode = OverrideModeSetting.CreateFromXmlReadValue(allowOverride);
						flag2 = true;
						continue;
					}
				}
				xmlUtil.AddErrorUnrecognizedAttribute(ExceptionAction.Global);
			}
			xmlUtil.Reader.MoveToElement();
			try
			{
				text = BaseConfigurationRecord.NormalizeLocationSubPath(text, xmlUtil);
				if (text == null && !flag && this.Host.IsDefinitionAllowed(this._configPath, ConfigurationAllowDefinition.MachineToWebRoot, ConfigurationAllowExeDefinition.MachineOnly))
				{
					throw new ConfigurationErrorsException(SR.GetString("Location_invalid_inheritInChildApplications_in_machine_or_root_web_config"), xmlUtil);
				}
			}
			catch (ConfigurationErrorsException ce)
			{
				xmlUtil.SchemaErrors.AddError(ce, ExceptionAction.Global);
			}
			if (xmlUtil.SchemaErrors.GlobalErrorCount > globalErrorCount)
			{
				xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
				return;
			}
			if (text == null)
			{
				this.ScanSectionsRecursive(xmlUtil, string.Empty, true, null, overrideMode, !flag);
				return;
			}
			if (!this._flags[1048576])
			{
				xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
				return;
			}
			IInternalConfigHost host = this.Host;
			if (this is RuntimeConfigurationRecord && host != null && text.Length != 0 && text[0] != '.')
			{
				if (BaseConfigurationRecord.s_appConfigPath == null)
				{
					object configContext = this.ConfigContext;
					if (configContext != null)
					{
						string value = configContext.ToString();
						Interlocked.CompareExchange<string>(ref BaseConfigurationRecord.s_appConfigPath, value, null);
					}
				}
				string configPathFromLocationSubPath = host.GetConfigPathFromLocationSubPath(this._configPath, text);
				if (!StringUtil.StartsWithIgnoreCase(BaseConfigurationRecord.s_appConfigPath, configPathFromLocationSubPath) && !StringUtil.StartsWithIgnoreCase(configPathFromLocationSubPath, BaseConfigurationRecord.s_appConfigPath))
				{
					xmlUtil.StrictSkipToNextElement(ExceptionAction.NonSpecific);
					return;
				}
			}
			this.AddLocation(text);
			this.ScanSectionsRecursive(xmlUtil, string.Empty, true, text, overrideMode, !flag);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000093E8 File Offset: 0x000083E8
		protected virtual void AddLocation(string LocationSubPath)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000093EC File Offset: 0x000083EC
		private void ResolveLocationSections()
		{
			if (!this._flags[256])
			{
				if (!this._parent.IsRootConfig)
				{
					this._parent.ResolveLocationSections();
				}
				lock (this)
				{
					if (!this._flags[256] && this._locationSections != null)
					{
						HybridDictionary hybridDictionary = new HybridDictionary(true);
						foreach (object obj in this._locationSections)
						{
							LocationSectionRecord locationSectionRecord = (LocationSectionRecord)obj;
							string configPathFromLocationSubPath = this.Host.GetConfigPathFromLocationSubPath(this._configPath, locationSectionRecord.SectionXmlInfo.SubPath);
							locationSectionRecord.SectionXmlInfo.TargetConfigPath = configPathFromLocationSubPath;
							HybridDictionary hybridDictionary2 = (HybridDictionary)hybridDictionary[configPathFromLocationSubPath];
							if (hybridDictionary2 == null)
							{
								hybridDictionary2 = new HybridDictionary(false);
								hybridDictionary.Add(configPathFromLocationSubPath, hybridDictionary2);
							}
							LocationSectionRecord locationSectionRecord2 = (LocationSectionRecord)hybridDictionary2[locationSectionRecord.ConfigKey];
							FactoryRecord factoryRecord = null;
							if (locationSectionRecord2 == null)
							{
								hybridDictionary2.Add(locationSectionRecord.ConfigKey, locationSectionRecord);
							}
							else
							{
								factoryRecord = this.FindFactoryRecord(locationSectionRecord.ConfigKey, true);
								if (factoryRecord == null || !factoryRecord.IsIgnorable())
								{
									if (!locationSectionRecord2.HasErrors)
									{
										locationSectionRecord2.AddError(new ConfigurationErrorsException(SR.GetString("Config_sections_must_be_unique"), locationSectionRecord2.SectionXmlInfo));
									}
									locationSectionRecord.AddError(new ConfigurationErrorsException(SR.GetString("Config_sections_must_be_unique"), locationSectionRecord.SectionXmlInfo));
								}
							}
							if (factoryRecord == null)
							{
								factoryRecord = this.FindFactoryRecord(locationSectionRecord.ConfigKey, true);
							}
							if (!factoryRecord.HasErrors)
							{
								try
								{
									this.VerifyDefinitionAllowed(factoryRecord, configPathFromLocationSubPath, locationSectionRecord.SectionXmlInfo);
								}
								catch (ConfigurationException e)
								{
									locationSectionRecord.AddError(e);
								}
							}
						}
						BaseConfigurationRecord parent = this._parent;
						while (!parent.IsRootConfig)
						{
							foreach (object obj2 in this._locationSections)
							{
								LocationSectionRecord locationSectionRecord3 = (LocationSectionRecord)obj2;
								bool flag = false;
								SectionRecord sectionRecord = parent.GetSectionRecord(locationSectionRecord3.ConfigKey, true);
								if (sectionRecord != null && (sectionRecord.LockChildren || sectionRecord.Locked))
								{
									flag = true;
								}
								else if (parent._locationSections != null)
								{
									string targetConfigPath = locationSectionRecord3.SectionXmlInfo.TargetConfigPath;
									foreach (object obj3 in parent._locationSections)
									{
										LocationSectionRecord locationSectionRecord4 = (LocationSectionRecord)obj3;
										string targetConfigPath2 = locationSectionRecord4.SectionXmlInfo.TargetConfigPath;
										if (locationSectionRecord4.SectionXmlInfo.OverrideModeSetting.IsLocked && locationSectionRecord3.ConfigKey == locationSectionRecord4.ConfigKey && UrlPath.IsEqualOrSubpath(targetConfigPath, targetConfigPath2))
										{
											flag = true;
											break;
										}
									}
								}
								if (flag)
								{
									locationSectionRecord3.AddError(new ConfigurationErrorsException(SR.GetString("Config_section_locked"), locationSectionRecord3.SectionXmlInfo));
								}
							}
							parent = parent._parent;
						}
					}
					this._flags[256] = true;
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00009790 File Offset: 0x00008790
		private void VerifyDefinitionAllowed(FactoryRecord factoryRecord, string configPath, IConfigErrorInfo errorInfo)
		{
			this.Host.VerifyDefinitionAllowed(configPath, factoryRecord.AllowDefinition, factoryRecord.AllowExeDefinition, errorInfo);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000097AB File Offset: 0x000087AB
		internal bool IsDefinitionAllowed(ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition)
		{
			return this.Host.IsDefinitionAllowed(this._configPath, allowDefinition, allowExeDefinition);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000097C0 File Offset: 0x000087C0
		protected static void VerifySectionName(string name, XmlUtil xmlUtil, ExceptionAction action, bool allowImplicit)
		{
			try
			{
				BaseConfigurationRecord.VerifySectionName(name, xmlUtil, allowImplicit);
			}
			catch (ConfigurationErrorsException ce)
			{
				xmlUtil.SchemaErrors.AddError(ce, action);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000097F8 File Offset: 0x000087F8
		protected static void VerifySectionName(string name, IConfigErrorInfo errorInfo, bool allowImplicit)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_invalid"), errorInfo);
			}
			try
			{
				XmlConvert.VerifyName(name);
			}
			catch (Exception e)
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_tag_name_invalid"), e, errorInfo);
			}
			catch
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_tag_name_invalid"), null, errorInfo);
			}
			if (BaseConfigurationRecord.IsImplicitSection(name))
			{
				if (allowImplicit)
				{
					return;
				}
				throw new ConfigurationErrorsException(SR.GetString("Cannot_declare_or_remove_implicit_section", new object[]
				{
					name
				}), errorInfo);
			}
			else
			{
				if (StringUtil.StartsWith(name, "config"))
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_cannot_begin_with_config"), errorInfo);
				}
				if (name == "location")
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_cannot_be_location"), errorInfo);
				}
				return;
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000098D0 File Offset: 0x000088D0
		internal static string NormalizeLocationSubPath(string subPath, IConfigErrorInfo errorInfo)
		{
			if (string.IsNullOrEmpty(subPath))
			{
				return null;
			}
			if (subPath == ".")
			{
				return null;
			}
			string text = subPath.TrimStart(new char[0]);
			if (text.Length != subPath.Length)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_location_path_invalid_first_character"), errorInfo);
			}
			if ("\\./".IndexOf(subPath[0]) != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_location_path_invalid_first_character"), errorInfo);
			}
			text = subPath.TrimEnd(new char[0]);
			if (text.Length != subPath.Length)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_location_path_invalid_last_character"), errorInfo);
			}
			if ("\\./".IndexOf(subPath[subPath.Length - 1]) != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_location_path_invalid_last_character"), errorInfo);
			}
			if (subPath.IndexOfAny(BaseConfigurationRecord.s_invalidSubPathCharactersArray) != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_location_path_invalid_character"), errorInfo);
			}
			return subPath;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000099C0 File Offset: 0x000089C0
		protected SectionRecord GetSectionRecord(string configKey, bool permitErrors)
		{
			SectionRecord sectionRecord;
			if (this._sectionRecords != null)
			{
				sectionRecord = (SectionRecord)this._sectionRecords[configKey];
			}
			else
			{
				sectionRecord = null;
			}
			if (sectionRecord != null && !permitErrors)
			{
				sectionRecord.ThrowOnErrors();
			}
			return sectionRecord;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000099F8 File Offset: 0x000089F8
		protected SectionRecord EnsureSectionRecord(string configKey, bool permitErrors)
		{
			return this.EnsureSectionRecordImpl(configKey, permitErrors, true);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00009A03 File Offset: 0x00008A03
		protected SectionRecord EnsureSectionRecordUnsafe(string configKey, bool permitErrors)
		{
			return this.EnsureSectionRecordImpl(configKey, permitErrors, false);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00009A10 File Offset: 0x00008A10
		private SectionRecord EnsureSectionRecordImpl(string configKey, bool permitErrors, bool setLockSettings)
		{
			SectionRecord sectionRecord = this.GetSectionRecord(configKey, permitErrors);
			if (sectionRecord == null)
			{
				lock (this)
				{
					if (this._sectionRecords == null)
					{
						this._sectionRecords = new Hashtable();
					}
					else
					{
						sectionRecord = this.GetSectionRecord(configKey, permitErrors);
					}
					if (sectionRecord == null)
					{
						sectionRecord = new SectionRecord(configKey);
						this._sectionRecords.Add(configKey, sectionRecord);
					}
				}
				if (setLockSettings)
				{
					OverrideMode forChildren = OverrideMode.Inherit;
					OverrideMode forSelf = this.ResolveOverrideModeFromParent(configKey, out forChildren);
					sectionRecord.ChangeLockSettings(forSelf, forChildren);
				}
			}
			return sectionRecord;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00009A9C File Offset: 0x00008A9C
		private bool HasFactoryRecords
		{
			get
			{
				return this._factoryRecords != null;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00009AAC File Offset: 0x00008AAC
		internal FactoryRecord GetFactoryRecord(string configKey, bool permitErrors)
		{
			if (this._factoryRecords == null)
			{
				return null;
			}
			FactoryRecord factoryRecord = (FactoryRecord)this._factoryRecords[configKey];
			if (factoryRecord != null && !permitErrors)
			{
				factoryRecord.ThrowOnErrors();
			}
			return factoryRecord;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009AE2 File Offset: 0x00008AE2
		protected Hashtable EnsureFactories()
		{
			if (this._factoryRecords == null)
			{
				this._factoryRecords = new Hashtable();
			}
			return this._factoryRecords;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009AFD File Offset: 0x00008AFD
		private ArrayList EnsureLocationSections()
		{
			if (this._locationSections == null)
			{
				this._locationSections = new ArrayList();
			}
			return this._locationSections;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00009B18 File Offset: 0x00008B18
		internal bool IsEmpty
		{
			get
			{
				return this._parent != null && !this._initErrors.HasErrors(false) && (this._sectionRecords == null || this._sectionRecords.Count == 0) && (this._factoryRecords == null || this._factoryRecords.Count == 0) && (this._locationSections == null || this._locationSections.Count == 0);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00009B80 File Offset: 0x00008B80
		internal static string NormalizeConfigSource(string configSource, IConfigErrorInfo errorInfo)
		{
			if (string.IsNullOrEmpty(configSource))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_source_invalid_format"), errorInfo);
			}
			string text = configSource.Trim();
			if (text.Length != configSource.Length)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_source_invalid_format"), errorInfo);
			}
			if (configSource.IndexOf('/') != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_source_invalid_chars"), errorInfo);
			}
			if (string.IsNullOrEmpty(configSource) || Path.IsPathRooted(configSource))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_source_invalid_format"), errorInfo);
			}
			return configSource;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00009C0C File Offset: 0x00008C0C
		protected object MonitorStream(string configKey, string configSource, string streamname)
		{
			lock (this)
			{
				if (this._flags[2])
				{
					return null;
				}
				StreamInfo streamInfo = (StreamInfo)this.ConfigStreamInfo.StreamInfos[streamname];
				if (streamInfo != null)
				{
					if (streamInfo.SectionName != configKey)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_source_cannot_be_shared", new object[]
						{
							streamname
						}));
					}
					if (streamInfo.IsMonitored)
					{
						return streamInfo.Version;
					}
				}
				else
				{
					streamInfo = new StreamInfo(configKey, configSource, streamname);
					this.ConfigStreamInfo.StreamInfos.Add(streamname, streamInfo);
				}
			}
			object streamVersion = this.Host.GetStreamVersion(streamname);
			StreamChangeCallback callback = null;
			lock (this)
			{
				if (this._flags[2])
				{
					return null;
				}
				StreamInfo streamInfo2 = (StreamInfo)this.ConfigStreamInfo.StreamInfos[streamname];
				if (streamInfo2.IsMonitored)
				{
					return streamInfo2.Version;
				}
				streamInfo2.IsMonitored = true;
				streamInfo2.Version = streamVersion;
				if (this._flags[65536])
				{
					if (this.ConfigStreamInfo.CallbackDelegate == null)
					{
						this.ConfigStreamInfo.CallbackDelegate = new StreamChangeCallback(this.OnStreamChanged);
					}
					callback = this.ConfigStreamInfo.CallbackDelegate;
				}
			}
			if (this._flags[65536])
			{
				this.Host.StartMonitoringStreamForChanges(streamname, callback);
			}
			return streamVersion;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009DAC File Offset: 0x00008DAC
		private void OnStreamChanged(string streamname)
		{
			string sectionName;
			lock (this)
			{
				if (this._flags[2])
				{
					return;
				}
				StreamInfo streamInfo = (StreamInfo)this.ConfigStreamInfo.StreamInfos[streamname];
				if (streamInfo == null || !streamInfo.IsMonitored)
				{
					return;
				}
				sectionName = streamInfo.SectionName;
			}
			bool flag;
			if (sectionName == null)
			{
				flag = true;
			}
			else
			{
				FactoryRecord factoryRecord = this.FindFactoryRecord(sectionName, false);
				flag = factoryRecord.RestartOnExternalChanges;
			}
			if (flag)
			{
				this._configRoot.FireConfigChanged(this._configPath);
				return;
			}
			this._configRoot.ClearResult(this, sectionName, false);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009E54 File Offset: 0x00008E54
		private void ValidateUniqueConfigSource(string configKey, string configSourceStreamName, string configSourceArg, IConfigErrorInfo errorInfo)
		{
			lock (this)
			{
				if (this.ConfigStreamInfo.HasStreamInfos)
				{
					StreamInfo streamInfo = (StreamInfo)this.ConfigStreamInfo.StreamInfos[configSourceStreamName];
					if (streamInfo != null && streamInfo.SectionName != configKey)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_source_cannot_be_shared", new object[]
						{
							configSourceArg
						}), errorInfo);
					}
				}
			}
			this.ValidateUniqueChildConfigSource(configKey, configSourceStreamName, configSourceArg, errorInfo);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00009EE0 File Offset: 0x00008EE0
		protected void ValidateUniqueChildConfigSource(string configKey, string configSourceStreamName, string configSourceArg, IConfigErrorInfo errorInfo)
		{
			BaseConfigurationRecord parent;
			if (this.IsLocationConfig)
			{
				parent = this._parent._parent;
			}
			else
			{
				parent = this._parent;
			}
			while (!parent.IsRootConfig)
			{
				lock (parent)
				{
					if (parent.ConfigStreamInfo.HasStreamInfos)
					{
						StreamInfo streamInfo = (StreamInfo)parent.ConfigStreamInfo.StreamInfos[configSourceStreamName];
						if (streamInfo != null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_source_parent_conflict", new object[]
							{
								configSourceArg
							}), errorInfo);
						}
					}
				}
				parent = parent.Parent;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009F84 File Offset: 0x00008F84
		internal void hlClearResultRecursive(string configKey, bool forceEvaluatation)
		{
			this.RefreshFactoryRecord(configKey);
			SectionRecord sectionRecord = this.GetSectionRecord(configKey, false);
			if (sectionRecord != null)
			{
				sectionRecord.ClearResult();
				sectionRecord.ClearRawXml();
			}
			if (forceEvaluatation && !this.IsInitDelayed && !string.IsNullOrEmpty(this.ConfigStreamInfo.StreamName))
			{
				if (this._flags[262144])
				{
					throw ExceptionUtil.UnexpectedError("BaseConfigurationRecord::hlClearResultRecursive");
				}
				FactoryRecord factoryRecord = this.FindFactoryRecord(configKey, false);
				if (factoryRecord != null && !factoryRecord.IsGroup)
				{
					configKey = factoryRecord.ConfigKey;
					sectionRecord = this.EnsureSectionRecord(configKey, false);
					if (!sectionRecord.HasFileInput)
					{
						SectionXmlInfo sectionXmlInfo = new SectionXmlInfo(configKey, this._configPath, this._configPath, null, this.ConfigStreamInfo.StreamName, 0, null, null, null, null, null, null, OverrideModeSetting.LocationDefault, false);
						SectionInput sectionInput = new SectionInput(sectionXmlInfo, null);
						sectionRecord.AddFileInput(sectionInput);
					}
				}
			}
			if (this._children != null)
			{
				IEnumerable values = this._children.Values;
				foreach (object obj in values)
				{
					BaseConfigurationRecord baseConfigurationRecord = (BaseConfigurationRecord)obj;
					baseConfigurationRecord.hlClearResultRecursive(configKey, forceEvaluatation);
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000A0C4 File Offset: 0x000090C4
		internal BaseConfigurationRecord hlGetChild(string configName)
		{
			if (this._children == null)
			{
				return null;
			}
			return (BaseConfigurationRecord)this._children[configName];
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000A0E1 File Offset: 0x000090E1
		internal void hlAddChild(string configName, BaseConfigurationRecord child)
		{
			if (this._children == null)
			{
				this._children = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			this._children.Add(configName, child);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000A108 File Offset: 0x00009108
		internal void hlRemoveChild(string configName)
		{
			if (this._children != null)
			{
				this._children.Remove(configName);
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000A120 File Offset: 0x00009120
		internal bool hlNeedsChildFor(string configName)
		{
			if (this.IsRootConfig)
			{
				return true;
			}
			if (this.HasInitErrors)
			{
				return false;
			}
			string text = ConfigPathUtility.Combine(this._configPath, configName);
			try
			{
				using (this.Impersonate())
				{
					if (this.Host.IsConfigRecordRequired(text))
					{
						return true;
					}
				}
			}
			catch
			{
				throw;
			}
			if (this._flags[1048576])
			{
				BaseConfigurationRecord baseConfigurationRecord = this;
				while (!baseConfigurationRecord.IsRootConfig)
				{
					if (baseConfigurationRecord._locationSections != null)
					{
						baseConfigurationRecord.ResolveLocationSections();
						foreach (object obj in baseConfigurationRecord._locationSections)
						{
							LocationSectionRecord locationSectionRecord = (LocationSectionRecord)obj;
							if (UrlPath.IsEqualOrSubpath(text, locationSectionRecord.SectionXmlInfo.TargetConfigPath))
							{
								return true;
							}
						}
					}
					baseConfigurationRecord = baseConfigurationRecord._parent;
				}
			}
			return false;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000A230 File Offset: 0x00009230
		internal void CloseRecursive()
		{
			if (!this._flags[2])
			{
				bool flag = false;
				HybridDictionary hybridDictionary = null;
				StreamChangeCallback callback = null;
				lock (this)
				{
					if (!this._flags[2])
					{
						this._flags[2] = true;
						flag = true;
						if (!this.IsLocationConfig && this.ConfigStreamInfo.HasStreamInfos)
						{
							callback = this.ConfigStreamInfo.CallbackDelegate;
							hybridDictionary = this.ConfigStreamInfo.StreamInfos;
							this.ConfigStreamInfo.CallbackDelegate = null;
							this.ConfigStreamInfo.ClearStreamInfos();
						}
					}
				}
				if (flag)
				{
					if (this._children != null)
					{
						foreach (object obj in this._children.Values)
						{
							BaseConfigurationRecord baseConfigurationRecord = (BaseConfigurationRecord)obj;
							baseConfigurationRecord.CloseRecursive();
						}
					}
					if (hybridDictionary != null)
					{
						foreach (object obj2 in hybridDictionary.Values)
						{
							StreamInfo streamInfo = (StreamInfo)obj2;
							if (streamInfo.IsMonitored)
							{
								this.Host.StopMonitoringStreamForChanges(streamInfo.StreamName, callback);
								streamInfo.IsMonitored = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000A3AC File Offset: 0x000093AC
		internal string FindChangedConfigurationStream()
		{
			BaseConfigurationRecord baseConfigurationRecord = this;
			while (!baseConfigurationRecord.IsRootConfig)
			{
				lock (baseConfigurationRecord)
				{
					if (baseConfigurationRecord.ConfigStreamInfo.HasStreamInfos)
					{
						foreach (object obj2 in baseConfigurationRecord.ConfigStreamInfo.StreamInfos.Values)
						{
							StreamInfo streamInfo = (StreamInfo)obj2;
							if (streamInfo.IsMonitored && this.HasStreamChanged(streamInfo.StreamName, streamInfo.Version))
							{
								return streamInfo.StreamName;
							}
						}
					}
				}
				baseConfigurationRecord = baseConfigurationRecord._parent;
			}
			return null;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000A47C File Offset: 0x0000947C
		private bool HasStreamChanged(string streamname, object lastVersion)
		{
			object streamVersion = this.Host.GetStreamVersion(streamname);
			if (lastVersion != null)
			{
				return streamVersion == null || !lastVersion.Equals(streamVersion);
			}
			return streamVersion != null;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000A4B0 File Offset: 0x000094B0
		protected virtual string CallHostDecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfig)
		{
			return this.Host.DecryptSection(encryptedXml, protectionProvider, protectedConfig);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000A4C0 File Offset: 0x000094C0
		internal static string ValidateProtectionProviderAttribute(string protectionProvider, IConfigErrorInfo errorInfo)
		{
			if (string.IsNullOrEmpty(protectionProvider))
			{
				throw new ConfigurationErrorsException(SR.GetString("Protection_provider_invalid_format"), errorInfo);
			}
			return protectionProvider;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000A4DC File Offset: 0x000094DC
		private ConfigXmlReader DecryptConfigSection(ConfigXmlReader reader, ProtectedConfigurationProvider protectionProvider)
		{
			ConfigXmlReader configXmlReader = reader.Clone();
			IConfigErrorInfo configErrorInfo = configXmlReader;
			string rawXml = null;
			configXmlReader.Read();
			string filename = configErrorInfo.Filename;
			int lineNumber = configErrorInfo.LineNumber;
			int lineOffset = lineNumber;
			if (configXmlReader.IsEmptyElement)
			{
				throw new ConfigurationErrorsException(SR.GetString("EncryptedNode_not_found"), filename, lineNumber);
			}
			for (;;)
			{
				configXmlReader.Read();
				XmlNodeType nodeType = configXmlReader.NodeType;
				if (nodeType == XmlNodeType.Element && configXmlReader.Name == "EncryptedData")
				{
					goto IL_A3;
				}
				if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
				if (nodeType != XmlNodeType.Comment && nodeType != XmlNodeType.Whitespace)
				{
					goto Block_5;
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("EncryptedNode_not_found"), filename, lineNumber);
			Block_5:
			throw new ConfigurationErrorsException(SR.GetString("EncryptedNode_is_in_invalid_format"), filename, lineNumber);
			IL_A3:
			lineNumber = configErrorInfo.LineNumber;
			string encryptedXml = configXmlReader.ReadOuterXml();
			try
			{
				rawXml = this.CallHostDecryptSection(encryptedXml, protectionProvider, this.ProtectedConfig);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Decryption_failed", new object[]
				{
					protectionProvider.Name,
					ex.Message
				}), ex, filename, lineNumber);
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Decryption_failed", new object[]
				{
					protectionProvider.Name,
					ExceptionUtil.NoExceptionInformation
				}), filename, lineNumber);
			}
			for (;;)
			{
				XmlNodeType nodeType = configXmlReader.NodeType;
				if (nodeType == XmlNodeType.EndElement)
				{
					goto IL_161;
				}
				if (nodeType != XmlNodeType.Comment && nodeType != XmlNodeType.Whitespace)
				{
					break;
				}
				if (!configXmlReader.Read())
				{
					goto IL_161;
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("EncryptedNode_is_in_invalid_format"), filename, lineNumber);
			IL_161:
			return new ConfigXmlReader(rawXml, filename, lineOffset, true);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000DD RID: 221 RVA: 0x0000A674 File Offset: 0x00009674
		internal object ConfigContext
		{
			get
			{
				if (!this._flags[128])
				{
					this._configContext = this.Host.CreateConfigurationContext(this.ConfigPath, this.LocationSubPath);
					this._flags[128] = true;
				}
				return this._configContext;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000A6C8 File Offset: 0x000096C8
		private void ThrowIfParseErrors(ConfigurationSchemaErrors schemaErrors)
		{
			schemaErrors.ThrowIfErrors(this.ClassFlags[64]);
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000A6EB File Offset: 0x000096EB
		internal bool RecordSupportsLocation
		{
			get
			{
				return this._flags[1048576] || this.IsMachineConfig;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000A707 File Offset: 0x00009707
		internal static bool IsImplicitSection(string configKey)
		{
			return configKey == "configProtectedData";
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000A71C File Offset: 0x0000971C
		private void AddImplicitSections(Hashtable factoryList)
		{
			if (this._parent.IsRootConfig)
			{
				if (factoryList == null)
				{
					factoryList = this.EnsureFactories();
				}
				FactoryRecord factoryRecord = (FactoryRecord)factoryList["configProtectedData"];
				if (factoryRecord != null)
				{
					return;
				}
				factoryList["configProtectedData"] = new FactoryRecord("configProtectedData", string.Empty, "configProtectedData", "System.Configuration.ProtectedConfigurationSection, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true, ConfigurationAllowDefinition.Everywhere, ConfigurationAllowExeDefinition.MachineToApplication, OverrideModeSetting.SectionDefault, true, true, true, true, null, -1);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000A78D File Offset: 0x0000978D
		internal static bool IsReservedAttributeName(string name)
		{
			return StringUtil.StartsWith(name, "config") || StringUtil.StartsWith(name, "lock");
		}

		// Token: 0x04000162 RID: 354
		protected const string NL = "\r\n";

		// Token: 0x04000163 RID: 355
		internal const string KEYWORD_TRUE = "true";

		// Token: 0x04000164 RID: 356
		internal const string KEYWORD_FALSE = "false";

		// Token: 0x04000165 RID: 357
		protected const string KEYWORD_CONFIGURATION = "configuration";

		// Token: 0x04000166 RID: 358
		protected const string KEYWORD_CONFIGURATION_NAMESPACE = "http://schemas.microsoft.com/.NetConfiguration/v2.0";

		// Token: 0x04000167 RID: 359
		protected const string KEYWORD_CONFIGSECTIONS = "configSections";

		// Token: 0x04000168 RID: 360
		protected const string KEYWORD_SECTION = "section";

		// Token: 0x04000169 RID: 361
		protected const string KEYWORD_SECTION_NAME = "name";

		// Token: 0x0400016A RID: 362
		protected const string KEYWORD_SECTION_TYPE = "type";

		// Token: 0x0400016B RID: 363
		protected const string KEYWORD_SECTION_ALLOWLOCATION = "allowLocation";

		// Token: 0x0400016C RID: 364
		protected const string KEYWORD_SECTION_ALLOWDEFINITION = "allowDefinition";

		// Token: 0x0400016D RID: 365
		protected const string KEYWORD_SECTION_ALLOWDEFINITION_EVERYWHERE = "Everywhere";

		// Token: 0x0400016E RID: 366
		protected const string KEYWORD_SECTION_ALLOWDEFINITION_MACHINEONLY = "MachineOnly";

		// Token: 0x0400016F RID: 367
		protected const string KEYWORD_SECTION_ALLOWDEFINITION_MACHINETOAPPLICATION = "MachineToApplication";

		// Token: 0x04000170 RID: 368
		protected const string KEYWORD_SECTION_ALLOWDEFINITION_MACHINETOWEBROOT = "MachineToWebRoot";

		// Token: 0x04000171 RID: 369
		protected const string KEYWORD_SECTION_ALLOWEXEDEFINITION = "allowExeDefinition";

		// Token: 0x04000172 RID: 370
		protected const string KEYWORD_SECTION_ALLOWEXEDEFINITION_MACHTOROAMING = "MachineToRoamingUser";

		// Token: 0x04000173 RID: 371
		protected const string KEYWORD_SECTION_ALLOWEXEDEFINITION_MACHTOLOCAL = "MachineToLocalUser";

		// Token: 0x04000174 RID: 372
		protected const string KEYWORD_SECTION_RESTARTONEXTERNALCHANGES = "restartOnExternalChanges";

		// Token: 0x04000175 RID: 373
		protected const string KEYWORD_SECTION_REQUIREPERMISSION = "requirePermission";

		// Token: 0x04000176 RID: 374
		protected const string KEYWORD_SECTIONGROUP = "sectionGroup";

		// Token: 0x04000177 RID: 375
		protected const string KEYWORD_SECTIONGROUP_NAME = "name";

		// Token: 0x04000178 RID: 376
		protected const string KEYWORD_SECTIONGROUP_TYPE = "type";

		// Token: 0x04000179 RID: 377
		protected const string KEYWORD_REMOVE = "remove";

		// Token: 0x0400017A RID: 378
		protected const string KEYWORD_CLEAR = "clear";

		// Token: 0x0400017B RID: 379
		protected const string KEYWORD_LOCATION = "location";

		// Token: 0x0400017C RID: 380
		protected const string KEYWORD_LOCATION_PATH = "path";

		// Token: 0x0400017D RID: 381
		internal const string KEYWORD_LOCATION_ALLOWOVERRIDE = "allowOverride";

		// Token: 0x0400017E RID: 382
		protected const string KEYWORD_LOCATION_INHERITINCHILDAPPLICATIONS = "inheritInChildApplications";

		// Token: 0x0400017F RID: 383
		protected const string KEYWORD_CONFIGSOURCE = "configSource";

		// Token: 0x04000180 RID: 384
		protected const string KEYWORD_XMLNS = "xmlns";

		// Token: 0x04000181 RID: 385
		internal const string KEYWORD_PROTECTION_PROVIDER = "configProtectionProvider";

		// Token: 0x04000182 RID: 386
		protected const string FORMAT_NEWCONFIGFILE = "<?xml version=\"1.0\" encoding=\"{0}\"?>\r\n";

		// Token: 0x04000183 RID: 387
		protected const string FORMAT_CONFIGURATION = "<configuration>\r\n";

		// Token: 0x04000184 RID: 388
		protected const string FORMAT_CONFIGURATION_NAMESPACE = "<configuration xmlns=\"{0}\">\r\n";

		// Token: 0x04000185 RID: 389
		protected const string FORMAT_CONFIGURATION_ENDELEMENT = "</configuration>";

		// Token: 0x04000186 RID: 390
		internal const string KEYWORD_SECTION_OVERRIDEMODEDEFAULT = "overrideModeDefault";

		// Token: 0x04000187 RID: 391
		internal const string KEYWORD_LOCATION_OVERRIDEMODE = "overrideMode";

		// Token: 0x04000188 RID: 392
		internal const string KEYWORD_OVERRIDEMODE_INHERIT = "Inherit";

		// Token: 0x04000189 RID: 393
		internal const string KEYWORD_OVERRIDEMODE_ALLOW = "Allow";

		// Token: 0x0400018A RID: 394
		internal const string KEYWORD_OVERRIDEMODE_DENY = "Deny";

		// Token: 0x0400018B RID: 395
		protected const string FORMAT_LOCATION_NOPATH = "<location {0} inheritInChildApplications=\"{1}\">\r\n";

		// Token: 0x0400018C RID: 396
		protected const string FORMAT_LOCATION_PATH = "<location path=\"{2}\" {0} inheritInChildApplications=\"{1}\">\r\n";

		// Token: 0x0400018D RID: 397
		protected const string FORMAT_LOCATION_ENDELEMENT = "</location>";

		// Token: 0x0400018E RID: 398
		internal const string KEYWORD_LOCATION_OVERRIDEMODE_STRING = "{0}=\"{1}\"";

		// Token: 0x0400018F RID: 399
		protected const string FORMAT_SECTION_CONFIGSOURCE = "<{0} configSource=\"{1}\" />";

		// Token: 0x04000190 RID: 400
		protected const string FORMAT_CONFIGSOURCE_FILE = "<?xml version=\"1.0\" encoding=\"{0}\"?>\r\n";

		// Token: 0x04000191 RID: 401
		protected const string FORMAT_SECTIONGROUP_ENDELEMENT = "</sectionGroup>";

		// Token: 0x04000192 RID: 402
		protected const int ClassSupportsChangeNotifications = 1;

		// Token: 0x04000193 RID: 403
		protected const int ClassSupportsRefresh = 2;

		// Token: 0x04000194 RID: 404
		protected const int ClassSupportsImpersonation = 4;

		// Token: 0x04000195 RID: 405
		protected const int ClassSupportsRestrictedPermissions = 8;

		// Token: 0x04000196 RID: 406
		protected const int ClassSupportsKeepInputs = 16;

		// Token: 0x04000197 RID: 407
		protected const int ClassSupportsDelayedInit = 32;

		// Token: 0x04000198 RID: 408
		protected const int ClassIgnoreLocalErrors = 64;

		// Token: 0x04000199 RID: 409
		protected const int ProtectedDataInitialized = 1;

		// Token: 0x0400019A RID: 410
		protected const int Closed = 2;

		// Token: 0x0400019B RID: 411
		protected const int PrefetchAll = 8;

		// Token: 0x0400019C RID: 412
		protected const int IsAboveApplication = 32;

		// Token: 0x0400019D RID: 413
		private const int ContextEvaluated = 128;

		// Token: 0x0400019E RID: 414
		private const int IsLocationListResolved = 256;

		// Token: 0x0400019F RID: 415
		protected const int NamespacePresentInFile = 512;

		// Token: 0x040001A0 RID: 416
		private const int RestrictedPermissionsResolved = 2048;

		// Token: 0x040001A1 RID: 417
		protected const int IsTrusted = 8192;

		// Token: 0x040001A2 RID: 418
		protected const int SupportsChangeNotifications = 65536;

		// Token: 0x040001A3 RID: 419
		protected const int SupportsRefresh = 131072;

		// Token: 0x040001A4 RID: 420
		protected const int SupportsPath = 262144;

		// Token: 0x040001A5 RID: 421
		protected const int SupportsKeepInputs = 524288;

		// Token: 0x040001A6 RID: 422
		protected const int SupportsLocation = 1048576;

		// Token: 0x040001A7 RID: 423
		protected const int ForceLocationWritten = 16777216;

		// Token: 0x040001A8 RID: 424
		protected const int SuggestLocationRemoval = 33554432;

		// Token: 0x040001A9 RID: 425
		protected const int NamespacePresentCurrent = 67108864;

		// Token: 0x040001AA RID: 426
		internal const char ConfigPathSeparatorChar = '/';

		// Token: 0x040001AB RID: 427
		internal const string ConfigPathSeparatorString = "/";

		// Token: 0x040001AC RID: 428
		private const string invalidFirstSubPathCharacters = "\\./";

		// Token: 0x040001AD RID: 429
		private const string invalidLastSubPathCharacters = "\\./";

		// Token: 0x040001AE RID: 430
		private const string invalidSubPathCharactersString = "\\?:*\"<>|";

		// Token: 0x040001AF RID: 431
		private const string ProtectedConfigurationSectionTypeName = "System.Configuration.ProtectedConfigurationSection, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x040001B0 RID: 432
		internal const string RESERVED_SECTION_PROTECTED_CONFIGURATION = "configProtectedData";

		// Token: 0x040001B1 RID: 433
		internal static readonly char[] ConfigPathSeparatorParams = new char[]
		{
			'/'
		};

		// Token: 0x040001B2 RID: 434
		private static ConfigurationPermission s_unrestrictedConfigPermission;

		// Token: 0x040001B3 RID: 435
		protected SafeBitVector32 _flags;

		// Token: 0x040001B4 RID: 436
		protected BaseConfigurationRecord _parent;

		// Token: 0x040001B5 RID: 437
		protected Hashtable _children;

		// Token: 0x040001B6 RID: 438
		protected InternalConfigRoot _configRoot;

		// Token: 0x040001B7 RID: 439
		protected string _configName;

		// Token: 0x040001B8 RID: 440
		protected string _configPath;

		// Token: 0x040001B9 RID: 441
		protected string _locationSubPath;

		// Token: 0x040001BA RID: 442
		private BaseConfigurationRecord.ConfigRecordStreamInfo _configStreamInfo;

		// Token: 0x040001BB RID: 443
		private object _configContext;

		// Token: 0x040001BC RID: 444
		private ProtectedConfigurationSection _protectedConfig;

		// Token: 0x040001BD RID: 445
		private PermissionSet _restrictedPermissions;

		// Token: 0x040001BE RID: 446
		private ConfigurationSchemaErrors _initErrors;

		// Token: 0x040001BF RID: 447
		private BaseConfigurationRecord _initDelayedRoot;

		// Token: 0x040001C0 RID: 448
		protected Hashtable _factoryRecords;

		// Token: 0x040001C1 RID: 449
		protected Hashtable _sectionRecords;

		// Token: 0x040001C2 RID: 450
		protected ArrayList _locationSections;

		// Token: 0x040001C3 RID: 451
		private static string s_appConfigPath;

		// Token: 0x040001C4 RID: 452
		private static IComparer<SectionInput> s_indirectInputsComparer = new BaseConfigurationRecord.IndirectLocationInputComparer();

		// Token: 0x040001C5 RID: 453
		private static bool s_allowDataSetSectionToLoadUserConfig;

		// Token: 0x040001C6 RID: 454
		private static volatile bool s_allowDataSetSectionToLoadUserConfigValueInitialized;

		// Token: 0x040001C7 RID: 455
		private static char[] s_invalidSubPathCharactersArray = "\\?:*\"<>|".ToCharArray();

		// Token: 0x02000011 RID: 17
		protected class ConfigRecordStreamInfo
		{
			// Token: 0x060000E4 RID: 228 RVA: 0x0000A7E4 File Offset: 0x000097E4
			internal ConfigRecordStreamInfo()
			{
				this._encoding = Encoding.UTF8;
			}

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000A7F7 File Offset: 0x000097F7
			// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000A7FF File Offset: 0x000097FF
			internal bool HasStream
			{
				get
				{
					return this._hasStream;
				}
				set
				{
					this._hasStream = value;
				}
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x060000E7 RID: 231 RVA: 0x0000A808 File Offset: 0x00009808
			// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000A810 File Offset: 0x00009810
			internal string StreamName
			{
				get
				{
					return this._streamname;
				}
				set
				{
					this._streamname = value;
				}
			}

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x060000E9 RID: 233 RVA: 0x0000A819 File Offset: 0x00009819
			// (set) Token: 0x060000EA RID: 234 RVA: 0x0000A821 File Offset: 0x00009821
			internal object StreamVersion
			{
				get
				{
					return this._streamVersion;
				}
				set
				{
					this._streamVersion = value;
				}
			}

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x060000EB RID: 235 RVA: 0x0000A82A File Offset: 0x0000982A
			// (set) Token: 0x060000EC RID: 236 RVA: 0x0000A832 File Offset: 0x00009832
			internal Encoding StreamEncoding
			{
				get
				{
					return this._encoding;
				}
				set
				{
					this._encoding = value;
				}
			}

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x060000ED RID: 237 RVA: 0x0000A83B File Offset: 0x0000983B
			// (set) Token: 0x060000EE RID: 238 RVA: 0x0000A843 File Offset: 0x00009843
			internal StreamChangeCallback CallbackDelegate
			{
				get
				{
					return this._callbackDelegate;
				}
				set
				{
					this._callbackDelegate = value;
				}
			}

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x060000EF RID: 239 RVA: 0x0000A84C File Offset: 0x0000984C
			internal HybridDictionary StreamInfos
			{
				get
				{
					if (this._streamInfos == null)
					{
						this._streamInfos = new HybridDictionary(true);
					}
					return this._streamInfos;
				}
			}

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000A868 File Offset: 0x00009868
			internal bool HasStreamInfos
			{
				get
				{
					return this._streamInfos != null;
				}
			}

			// Token: 0x060000F1 RID: 241 RVA: 0x0000A876 File Offset: 0x00009876
			internal void ClearStreamInfos()
			{
				this._streamInfos = null;
			}

			// Token: 0x040001C8 RID: 456
			private bool _hasStream;

			// Token: 0x040001C9 RID: 457
			private string _streamname;

			// Token: 0x040001CA RID: 458
			private object _streamVersion;

			// Token: 0x040001CB RID: 459
			private Encoding _encoding;

			// Token: 0x040001CC RID: 460
			private StreamChangeCallback _callbackDelegate;

			// Token: 0x040001CD RID: 461
			private HybridDictionary _streamInfos;
		}

		// Token: 0x02000012 RID: 18
		private class IndirectLocationInputComparer : IComparer<SectionInput>
		{
			// Token: 0x060000F2 RID: 242 RVA: 0x0000A880 File Offset: 0x00009880
			public int Compare(SectionInput x, SectionInput y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return 0;
				}
				string targetConfigPath = x.SectionXmlInfo.TargetConfigPath;
				string targetConfigPath2 = y.SectionXmlInfo.TargetConfigPath;
				if (UrlPath.IsSubpath(targetConfigPath, targetConfigPath2))
				{
					return 1;
				}
				if (UrlPath.IsSubpath(targetConfigPath2, targetConfigPath))
				{
					return -1;
				}
				string definitionConfigPath = x.SectionXmlInfo.DefinitionConfigPath;
				string definitionConfigPath2 = y.SectionXmlInfo.DefinitionConfigPath;
				if (UrlPath.IsSubpath(definitionConfigPath, definitionConfigPath2))
				{
					return 1;
				}
				if (UrlPath.IsSubpath(definitionConfigPath2, definitionConfigPath))
				{
					return -1;
				}
				return 0;
			}
		}
	}
}
