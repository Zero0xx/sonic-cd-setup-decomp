using System;
using System.Collections;
using System.Configuration.Internal;
using System.IO;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020006E5 RID: 1765
	internal sealed class ClientSettingsStore
	{
		// Token: 0x0600367D RID: 13949 RVA: 0x000E8A0C File Offset: 0x000E7A0C
		private Configuration GetUserConfig(bool isRoaming)
		{
			ConfigurationUserLevel userLevel = isRoaming ? ConfigurationUserLevel.PerUserRoaming : ConfigurationUserLevel.PerUserRoamingAndLocal;
			return ClientSettingsStore.ClientSettingsConfigurationHost.OpenExeConfiguration(userLevel);
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x000E8A2C File Offset: 0x000E7A2C
		private ClientSettingsSection GetConfigSection(Configuration config, string sectionName, bool declare)
		{
			string sectionName2 = "userSettings/" + sectionName;
			ClientSettingsSection clientSettingsSection = null;
			if (config != null)
			{
				clientSettingsSection = (config.GetSection(sectionName2) as ClientSettingsSection);
				if (clientSettingsSection == null && declare)
				{
					this.DeclareSection(config, sectionName);
					clientSettingsSection = (config.GetSection(sectionName2) as ClientSettingsSection);
				}
			}
			return clientSettingsSection;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x000E8A74 File Offset: 0x000E7A74
		private void DeclareSection(Configuration config, string sectionName)
		{
			if (config.GetSectionGroup("userSettings") == null)
			{
				ConfigurationSectionGroup sectionGroup = new UserSettingsGroup();
				config.SectionGroups.Add("userSettings", sectionGroup);
			}
			ConfigurationSectionGroup sectionGroup2 = config.GetSectionGroup("userSettings");
			if (sectionGroup2 != null && sectionGroup2.Sections[sectionName] == null)
			{
				ConfigurationSection configurationSection = new ClientSettingsSection();
				configurationSection.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
				configurationSection.SectionInformation.RequirePermission = false;
				sectionGroup2.Sections.Add(sectionName, configurationSection);
			}
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x000E8AF8 File Offset: 0x000E7AF8
		internal IDictionary ReadSettings(string sectionName, bool isUserScoped)
		{
			IDictionary dictionary = new Hashtable();
			if (isUserScoped && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				return dictionary;
			}
			string str = isUserScoped ? "userSettings/" : "applicationSettings/";
			ConfigurationManager.RefreshSection(str + sectionName);
			ClientSettingsSection clientSettingsSection = ConfigurationManager.GetSection(str + sectionName) as ClientSettingsSection;
			if (clientSettingsSection != null)
			{
				foreach (object obj in clientSettingsSection.Settings)
				{
					SettingElement settingElement = (SettingElement)obj;
					dictionary[settingElement.Name] = new StoredSetting(settingElement.SerializeAs, settingElement.Value.ValueXml);
				}
			}
			return dictionary;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000E8BC4 File Offset: 0x000E7BC4
		internal static IDictionary ReadSettingsFromFile(string configFileName, string sectionName, bool isUserScoped)
		{
			IDictionary dictionary = new Hashtable();
			if (isUserScoped && !ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				return dictionary;
			}
			string str = isUserScoped ? "userSettings/" : "applicationSettings/";
			ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap();
			ConfigurationUserLevel userLevel = isUserScoped ? ConfigurationUserLevel.PerUserRoaming : ConfigurationUserLevel.None;
			if (isUserScoped)
			{
				exeConfigurationFileMap.ExeConfigFilename = ConfigurationManagerInternalFactory.Instance.ApplicationConfigUri;
				exeConfigurationFileMap.RoamingUserConfigFilename = configFileName;
			}
			else
			{
				exeConfigurationFileMap.ExeConfigFilename = configFileName;
			}
			Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, userLevel);
			ClientSettingsSection clientSettingsSection = configuration.GetSection(str + sectionName) as ClientSettingsSection;
			if (clientSettingsSection != null)
			{
				foreach (object obj in clientSettingsSection.Settings)
				{
					SettingElement settingElement = (SettingElement)obj;
					dictionary[settingElement.Name] = new StoredSetting(settingElement.SerializeAs, settingElement.Value.ValueXml);
				}
			}
			return dictionary;
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000E8CC8 File Offset: 0x000E7CC8
		internal ConnectionStringSettingsCollection ReadConnectionStrings()
		{
			return PrivilegedConfigurationManager.ConnectionStrings;
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000E8CD0 File Offset: 0x000E7CD0
		internal void RevertToParent(string sectionName, bool isRoaming)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			Configuration userConfig = this.GetUserConfig(isRoaming);
			ClientSettingsSection configSection = this.GetConfigSection(userConfig, sectionName, false);
			if (configSection != null)
			{
				configSection.SectionInformation.RevertToParent();
				userConfig.Save();
			}
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x000E8D20 File Offset: 0x000E7D20
		internal void WriteSettings(string sectionName, bool isRoaming, IDictionary newSettings)
		{
			if (!ConfigurationManagerInternalFactory.Instance.SupportsUserConfig)
			{
				throw new ConfigurationErrorsException(SR.GetString("UserSettingsNotSupported"));
			}
			Configuration userConfig = this.GetUserConfig(isRoaming);
			ClientSettingsSection configSection = this.GetConfigSection(userConfig, sectionName, true);
			if (configSection != null)
			{
				SettingElementCollection settings = configSection.Settings;
				foreach (object obj in newSettings)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					SettingElement settingElement = settings.Get((string)dictionaryEntry.Key);
					if (settingElement == null)
					{
						settingElement = new SettingElement();
						settingElement.Name = (string)dictionaryEntry.Key;
						settings.Add(settingElement);
					}
					StoredSetting storedSetting = (StoredSetting)dictionaryEntry.Value;
					settingElement.SerializeAs = storedSetting.SerializeAs;
					settingElement.Value.ValueXml = storedSetting.Value;
				}
				try
				{
					userConfig.Save();
					return;
				}
				catch (ConfigurationErrorsException ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("SettingsSaveFailed", new object[]
					{
						ex.Message
					}), ex);
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("SettingsSaveFailedNoSection"));
		}

		// Token: 0x04003182 RID: 12674
		private const string ApplicationSettingsGroupName = "applicationSettings";

		// Token: 0x04003183 RID: 12675
		private const string UserSettingsGroupName = "userSettings";

		// Token: 0x04003184 RID: 12676
		private const string ApplicationSettingsGroupPrefix = "applicationSettings/";

		// Token: 0x04003185 RID: 12677
		private const string UserSettingsGroupPrefix = "userSettings/";

		// Token: 0x020006E6 RID: 1766
		private sealed class ClientSettingsConfigurationHost : DelegatingConfigHost
		{
			// Token: 0x17000C9C RID: 3228
			// (get) Token: 0x06003686 RID: 13958 RVA: 0x000E8E70 File Offset: 0x000E7E70
			private IInternalConfigClientHost ClientHost
			{
				get
				{
					return (IInternalConfigClientHost)base.Host;
				}
			}

			// Token: 0x17000C9D RID: 3229
			// (get) Token: 0x06003687 RID: 13959 RVA: 0x000E8E7D File Offset: 0x000E7E7D
			internal static IInternalConfigConfigurationFactory ConfigFactory
			{
				get
				{
					if (ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory == null)
					{
						ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory = (IInternalConfigConfigurationFactory)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.Internal.InternalConfigConfigurationFactory,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
					}
					return ClientSettingsStore.ClientSettingsConfigurationHost.s_configFactory;
				}
			}

			// Token: 0x06003688 RID: 13960 RVA: 0x000E8E9F File Offset: 0x000E7E9F
			private ClientSettingsConfigurationHost()
			{
			}

			// Token: 0x06003689 RID: 13961 RVA: 0x000E8EA7 File Offset: 0x000E7EA7
			public override void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
			{
			}

			// Token: 0x0600368A RID: 13962 RVA: 0x000E8EAC File Offset: 0x000E7EAC
			public override void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
			{
				ConfigurationUserLevel configurationUserLevel = (ConfigurationUserLevel)hostInitConfigurationParams[0];
				base.Host = (IInternalConfigHost)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.ClientConfigurationHost,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				ConfigurationUserLevel configurationUserLevel2 = configurationUserLevel;
				string text;
				if (configurationUserLevel2 != ConfigurationUserLevel.None)
				{
					if (configurationUserLevel2 != ConfigurationUserLevel.PerUserRoaming)
					{
						if (configurationUserLevel2 != ConfigurationUserLevel.PerUserRoamingAndLocal)
						{
							throw new ArgumentException(SR.GetString("UnknownUserLevel"));
						}
						text = this.ClientHost.GetLocalUserConfigPath();
					}
					else
					{
						text = this.ClientHost.GetRoamingUserConfigPath();
					}
				}
				else
				{
					text = this.ClientHost.GetExeConfigPath();
				}
				base.Host.InitForConfiguration(ref locationSubPath, out configPath, out locationConfigPath, configRoot, new object[]
				{
					null,
					null,
					text
				});
			}

			// Token: 0x0600368B RID: 13963 RVA: 0x000E8F44 File Offset: 0x000E7F44
			private bool IsKnownConfigFile(string filename)
			{
				return string.Equals(filename, ConfigurationManagerInternalFactory.Instance.MachineConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ApplicationConfigUri, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(filename, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x0600368C RID: 13964 RVA: 0x000E8FA0 File Offset: 0x000E7FA0
			internal static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
			{
				return ClientSettingsStore.ClientSettingsConfigurationHost.ConfigFactory.Create(typeof(ClientSettingsStore.ClientSettingsConfigurationHost), new object[]
				{
					userLevel
				});
			}

			// Token: 0x0600368D RID: 13965 RVA: 0x000E8FD2 File Offset: 0x000E7FD2
			public override Stream OpenStreamForRead(string streamName)
			{
				if (this.IsKnownConfigFile(streamName))
				{
					return base.Host.OpenStreamForRead(streamName, true);
				}
				return base.Host.OpenStreamForRead(streamName);
			}

			// Token: 0x0600368E RID: 13966 RVA: 0x000E8FF8 File Offset: 0x000E7FF8
			public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
			{
				Stream result;
				if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					result = new ClientSettingsStore.QuotaEnforcedStream(base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, true), false);
				}
				else if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					result = new ClientSettingsStore.QuotaEnforcedStream(base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, true), true);
				}
				else
				{
					result = base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext);
				}
				return result;
			}

			// Token: 0x0600368F RID: 13967 RVA: 0x000E9070 File Offset: 0x000E8070
			public override void WriteCompleted(string streamName, bool success, object writeContext)
			{
				if (string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeLocalConfigPath, StringComparison.OrdinalIgnoreCase) || string.Equals(streamName, ConfigurationManagerInternalFactory.Instance.ExeRoamingConfigPath, StringComparison.OrdinalIgnoreCase))
				{
					base.Host.WriteCompleted(streamName, success, writeContext, true);
					return;
				}
				base.Host.WriteCompleted(streamName, success, writeContext);
			}

			// Token: 0x04003186 RID: 12678
			private const string ClientConfigurationHostTypeName = "System.Configuration.ClientConfigurationHost,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			// Token: 0x04003187 RID: 12679
			private const string InternalConfigConfigurationFactoryTypeName = "System.Configuration.Internal.InternalConfigConfigurationFactory,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			// Token: 0x04003188 RID: 12680
			private static IInternalConfigConfigurationFactory s_configFactory;
		}

		// Token: 0x020006E7 RID: 1767
		private sealed class QuotaEnforcedStream : Stream
		{
			// Token: 0x06003690 RID: 13968 RVA: 0x000E90C1 File Offset: 0x000E80C1
			internal QuotaEnforcedStream(Stream originalStream, bool isRoaming)
			{
				this._originalStream = originalStream;
				this._isRoaming = isRoaming;
			}

			// Token: 0x17000C9E RID: 3230
			// (get) Token: 0x06003691 RID: 13969 RVA: 0x000E90D7 File Offset: 0x000E80D7
			public override bool CanRead
			{
				get
				{
					return this._originalStream.CanRead;
				}
			}

			// Token: 0x17000C9F RID: 3231
			// (get) Token: 0x06003692 RID: 13970 RVA: 0x000E90E4 File Offset: 0x000E80E4
			public override bool CanWrite
			{
				get
				{
					return this._originalStream.CanWrite;
				}
			}

			// Token: 0x17000CA0 RID: 3232
			// (get) Token: 0x06003693 RID: 13971 RVA: 0x000E90F1 File Offset: 0x000E80F1
			public override bool CanSeek
			{
				get
				{
					return this._originalStream.CanSeek;
				}
			}

			// Token: 0x17000CA1 RID: 3233
			// (get) Token: 0x06003694 RID: 13972 RVA: 0x000E90FE File Offset: 0x000E80FE
			public override long Length
			{
				get
				{
					return this._originalStream.Length;
				}
			}

			// Token: 0x17000CA2 RID: 3234
			// (get) Token: 0x06003695 RID: 13973 RVA: 0x000E910B File Offset: 0x000E810B
			// (set) Token: 0x06003696 RID: 13974 RVA: 0x000E9118 File Offset: 0x000E8118
			public override long Position
			{
				get
				{
					return this._originalStream.Position;
				}
				set
				{
					if (value < 0L)
					{
						throw new ArgumentOutOfRangeException("value", SR.GetString("PositionOutOfRange"));
					}
					this.Seek(value, SeekOrigin.Begin);
				}
			}

			// Token: 0x06003697 RID: 13975 RVA: 0x000E913D File Offset: 0x000E813D
			public override void Close()
			{
				this._originalStream.Close();
			}

			// Token: 0x06003698 RID: 13976 RVA: 0x000E914A File Offset: 0x000E814A
			protected override void Dispose(bool disposing)
			{
				if (disposing && this._originalStream != null)
				{
					((IDisposable)this._originalStream).Dispose();
					this._originalStream = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x06003699 RID: 13977 RVA: 0x000E9170 File Offset: 0x000E8170
			public override void Flush()
			{
				this._originalStream.Flush();
			}

			// Token: 0x0600369A RID: 13978 RVA: 0x000E9180 File Offset: 0x000E8180
			public override void SetLength(long value)
			{
				long length = this._originalStream.Length;
				this.EnsureQuota(Math.Max(length, value));
				this._originalStream.SetLength(value);
			}

			// Token: 0x0600369B RID: 13979 RVA: 0x000E91B4 File Offset: 0x000E81B4
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this._originalStream.Read(buffer, offset, count);
			}

			// Token: 0x0600369C RID: 13980 RVA: 0x000E91C4 File Offset: 0x000E81C4
			public override int ReadByte()
			{
				return this._originalStream.ReadByte();
			}

			// Token: 0x0600369D RID: 13981 RVA: 0x000E91D4 File Offset: 0x000E81D4
			public override long Seek(long offset, SeekOrigin origin)
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long val;
				switch (origin)
				{
				case SeekOrigin.Begin:
					val = offset;
					break;
				case SeekOrigin.Current:
					val = this._originalStream.Position + offset;
					break;
				case SeekOrigin.End:
					val = length + offset;
					break;
				default:
					throw new ArgumentException(SR.GetString("UnknownSeekOrigin"), "origin");
				}
				this.EnsureQuota(Math.Max(length, val));
				return this._originalStream.Seek(offset, origin);
			}

			// Token: 0x0600369E RID: 13982 RVA: 0x000E925C File Offset: 0x000E825C
			public override void Write(byte[] buffer, int offset, int count)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long val = this._originalStream.CanSeek ? (this._originalStream.Position + (long)count) : (this._originalStream.Length + (long)count);
				this.EnsureQuota(Math.Max(length, val));
				this._originalStream.Write(buffer, offset, count);
			}

			// Token: 0x0600369F RID: 13983 RVA: 0x000E92CC File Offset: 0x000E82CC
			public override void WriteByte(byte value)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long val = this._originalStream.CanSeek ? (this._originalStream.Position + 1L) : (this._originalStream.Length + 1L);
				this.EnsureQuota(Math.Max(length, val));
				this._originalStream.WriteByte(value);
			}

			// Token: 0x060036A0 RID: 13984 RVA: 0x000E9338 File Offset: 0x000E8338
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
			{
				return this._originalStream.BeginRead(buffer, offset, numBytes, userCallback, stateObject);
			}

			// Token: 0x060036A1 RID: 13985 RVA: 0x000E934C File Offset: 0x000E834C
			public override int EndRead(IAsyncResult asyncResult)
			{
				return this._originalStream.EndRead(asyncResult);
			}

			// Token: 0x060036A2 RID: 13986 RVA: 0x000E935C File Offset: 0x000E835C
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
			{
				if (!this.CanWrite)
				{
					throw new NotSupportedException();
				}
				long length = this._originalStream.Length;
				long val = this._originalStream.CanSeek ? (this._originalStream.Position + (long)numBytes) : (this._originalStream.Length + (long)numBytes);
				this.EnsureQuota(Math.Max(length, val));
				return this._originalStream.BeginWrite(buffer, offset, numBytes, userCallback, stateObject);
			}

			// Token: 0x060036A3 RID: 13987 RVA: 0x000E93CE File Offset: 0x000E83CE
			public override void EndWrite(IAsyncResult asyncResult)
			{
				this._originalStream.EndWrite(asyncResult);
			}

			// Token: 0x060036A4 RID: 13988 RVA: 0x000E93DC File Offset: 0x000E83DC
			private void EnsureQuota(long size)
			{
				new IsolatedStorageFilePermission(PermissionState.None)
				{
					UserQuota = size,
					UsageAllowed = (this._isRoaming ? IsolatedStorageContainment.DomainIsolationByRoamingUser : IsolatedStorageContainment.DomainIsolationByUser)
				}.Demand();
			}

			// Token: 0x04003189 RID: 12681
			private Stream _originalStream;

			// Token: 0x0400318A RID: 12682
			private bool _isRoaming;
		}
	}
}
