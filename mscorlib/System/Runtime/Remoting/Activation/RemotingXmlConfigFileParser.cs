using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200075B RID: 1883
	internal static class RemotingXmlConfigFileParser
	{
		// Token: 0x060042D4 RID: 17108 RVA: 0x000E2E63 File Offset: 0x000E1E63
		private static Hashtable CreateSyncCaseInsensitiveHashtable()
		{
			return Hashtable.Synchronized(RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable());
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x000E2E6F File Offset: 0x000E1E6F
		private static Hashtable CreateCaseInsensitiveHashtable()
		{
			return new Hashtable(StringComparer.InvariantCultureIgnoreCase);
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x000E2E7C File Offset: 0x000E1E7C
		public static RemotingXmlConfigFileData ParseDefaultConfiguration()
		{
			ConfigNode configNode = new ConfigNode("system.runtime.remoting", null);
			ConfigNode configNode2 = new ConfigNode("application", configNode);
			configNode.Children.Add(configNode2);
			ConfigNode configNode3 = new ConfigNode("channels", configNode2);
			configNode2.Children.Add(configNode3);
			ConfigNode configNode4 = new ConfigNode("channel", configNode2);
			configNode4.Attributes.Add(new DictionaryEntry("ref", "http client"));
			configNode4.Attributes.Add(new DictionaryEntry("displayName", "http client (delay loaded)"));
			configNode4.Attributes.Add(new DictionaryEntry("delayLoadAsClientChannel", "true"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode2);
			configNode4.Attributes.Add(new DictionaryEntry("ref", "tcp client"));
			configNode4.Attributes.Add(new DictionaryEntry("displayName", "tcp client (delay loaded)"));
			configNode4.Attributes.Add(new DictionaryEntry("delayLoadAsClientChannel", "true"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode2);
			configNode4.Attributes.Add(new DictionaryEntry("ref", "ipc client"));
			configNode4.Attributes.Add(new DictionaryEntry("displayName", "ipc client (delay loaded)"));
			configNode4.Attributes.Add(new DictionaryEntry("delayLoadAsClientChannel", "true"));
			configNode3.Children.Add(configNode4);
			configNode3 = new ConfigNode("channels", configNode);
			configNode.Children.Add(configNode3);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "http"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Http.HttpChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "http client"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Http.HttpClientChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "http server"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Http.HttpServerChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "tcp"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Tcp.TcpChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "tcp client"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Tcp.TcpClientChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "tcp server"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Tcp.TcpServerChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "ipc"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Ipc.IpcChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "ipc client"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Ipc.IpcClientChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "ipc server"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Ipc.IpcServerChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			ConfigNode configNode5 = new ConfigNode("channelSinkProviders", configNode);
			configNode.Children.Add(configNode5);
			ConfigNode configNode6 = new ConfigNode("clientProviders", configNode5);
			configNode5.Children.Add(configNode6);
			configNode4 = new ConfigNode("formatter", configNode6);
			configNode4.Attributes.Add(new DictionaryEntry("id", "soap"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.SoapClientFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode6.Children.Add(configNode4);
			configNode4 = new ConfigNode("formatter", configNode6);
			configNode4.Attributes.Add(new DictionaryEntry("id", "binary"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode6.Children.Add(configNode4);
			ConfigNode configNode7 = new ConfigNode("serverProviders", configNode5);
			configNode5.Children.Add(configNode7);
			configNode4 = new ConfigNode("formatter", configNode7);
			configNode4.Attributes.Add(new DictionaryEntry("id", "soap"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode7.Children.Add(configNode4);
			configNode4 = new ConfigNode("formatter", configNode7);
			configNode4.Attributes.Add(new DictionaryEntry("id", "binary"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode7.Children.Add(configNode4);
			configNode4 = new ConfigNode("provider", configNode7);
			configNode4.Attributes.Add(new DictionaryEntry("id", "wsdl"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.MetadataServices.SdlChannelSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode7.Children.Add(configNode4);
			return RemotingXmlConfigFileParser.ParseConfigNode(configNode);
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x000E3590 File Offset: 0x000E2590
		public static RemotingXmlConfigFileData ParseConfigFile(string filename)
		{
			ConfigTreeParser configTreeParser = new ConfigTreeParser();
			ConfigNode rootNode = configTreeParser.Parse(filename, "/configuration/system.runtime.remoting");
			return RemotingXmlConfigFileParser.ParseConfigNode(rootNode);
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x000E35B8 File Offset: 0x000E25B8
		private static RemotingXmlConfigFileData ParseConfigNode(ConfigNode rootNode)
		{
			RemotingXmlConfigFileData remotingXmlConfigFileData = new RemotingXmlConfigFileData();
			if (rootNode == null)
			{
				return null;
			}
			foreach (object obj in rootNode.Attributes)
			{
				string text = ((DictionaryEntry)obj).Key.ToString();
				string a;
				if ((a = text) != null)
				{
					a == "version";
				}
			}
			ConfigNode configNode = null;
			ConfigNode configNode2 = null;
			ConfigNode configNode3 = null;
			ConfigNode configNode4 = null;
			ConfigNode configNode5 = null;
			foreach (object obj2 in rootNode.Children)
			{
				ConfigNode configNode6 = (ConfigNode)obj2;
				string name;
				if ((name = configNode6.Name) != null)
				{
					if (!(name == "application"))
					{
						if (!(name == "channels"))
						{
							if (!(name == "channelSinkProviders"))
							{
								if (!(name == "debug"))
								{
									if (name == "customErrors")
									{
										if (configNode5 != null)
										{
											RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode5, remotingXmlConfigFileData);
										}
										configNode5 = configNode6;
									}
								}
								else
								{
									if (configNode4 != null)
									{
										RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode4, remotingXmlConfigFileData);
									}
									configNode4 = configNode6;
								}
							}
							else
							{
								if (configNode3 != null)
								{
									RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode3, remotingXmlConfigFileData);
								}
								configNode3 = configNode6;
							}
						}
						else
						{
							if (configNode2 != null)
							{
								RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode2, remotingXmlConfigFileData);
							}
							configNode2 = configNode6;
						}
					}
					else
					{
						if (configNode != null)
						{
							RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode, remotingXmlConfigFileData);
						}
						configNode = configNode6;
					}
				}
			}
			if (configNode4 != null)
			{
				RemotingXmlConfigFileParser.ProcessDebugNode(configNode4, remotingXmlConfigFileData);
			}
			if (configNode3 != null)
			{
				RemotingXmlConfigFileParser.ProcessChannelSinkProviderTemplates(configNode3, remotingXmlConfigFileData);
			}
			if (configNode2 != null)
			{
				RemotingXmlConfigFileParser.ProcessChannelTemplates(configNode2, remotingXmlConfigFileData);
			}
			if (configNode != null)
			{
				RemotingXmlConfigFileParser.ProcessApplicationNode(configNode, remotingXmlConfigFileData);
			}
			if (configNode5 != null)
			{
				RemotingXmlConfigFileParser.ProcessCustomErrorsNode(configNode5, remotingXmlConfigFileData);
			}
			return remotingXmlConfigFileData;
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x000E3788 File Offset: 0x000E2788
		private static void ReportError(string errorStr, RemotingXmlConfigFileData configData)
		{
			throw new RemotingException(errorStr);
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x000E3790 File Offset: 0x000E2790
		private static void ReportUniqueSectionError(ConfigNode parent, ConfigNode child, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_NodeMustBeUnique"), new object[]
			{
				child.Name,
				parent.Name
			}), configData);
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x000E37D4 File Offset: 0x000E27D4
		private static void ReportUnknownValueError(ConfigNode node, string value, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_UnknownValue"), new object[]
			{
				node.Name,
				value
			}), configData);
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x000E3810 File Offset: 0x000E2810
		private static void ReportMissingAttributeError(ConfigNode node, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportMissingAttributeError(node.Name, attributeName, configData);
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x000E3820 File Offset: 0x000E2820
		private static void ReportMissingAttributeError(string nodeDescription, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_RequiredXmlAttribute"), new object[]
			{
				nodeDescription,
				attributeName
			}), configData);
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x000E3858 File Offset: 0x000E2858
		private static void ReportMissingTypeAttributeError(ConfigNode node, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_MissingTypeAttribute"), new object[]
			{
				node.Name,
				attributeName
			}), configData);
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x000E3894 File Offset: 0x000E2894
		private static void ReportMissingXmlTypeAttributeError(ConfigNode node, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_MissingXmlTypeAttribute"), new object[]
			{
				node.Name,
				attributeName
			}), configData);
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x000E38D0 File Offset: 0x000E28D0
		private static void ReportInvalidTimeFormatError(string time, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_InvalidTimeFormat"), new object[]
			{
				time
			}), configData);
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x000E3904 File Offset: 0x000E2904
		private static void ReportNonTemplateIdAttributeError(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_NonTemplateIdAttribute"), new object[]
			{
				node.Name
			}), configData);
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x000E393C File Offset: 0x000E293C
		private static void ReportTemplateCannotReferenceTemplateError(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_TemplateCannotReferenceTemplate"), new object[]
			{
				node.Name
			}), configData);
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x000E3974 File Offset: 0x000E2974
		private static void ReportUnableToResolveTemplateReferenceError(ConfigNode node, string referenceName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_UnableToResolveTemplate"), new object[]
			{
				node.Name,
				referenceName
			}), configData);
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x000E39B0 File Offset: 0x000E29B0
		private static void ReportAssemblyVersionInfoPresent(string assemName, string entryDescription, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_VersionPresent"), new object[]
			{
				assemName,
				entryDescription
			}), configData);
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x000E39E8 File Offset: 0x000E29E8
		private static void ProcessDebugNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text) != null && a == "loadTypes")
				{
					RemotingXmlConfigFileData.LoadTypes = Convert.ToBoolean((string)dictionaryEntry.Value, CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x000E3A78 File Offset: 0x000E2A78
		private static void ProcessApplicationNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				if (text.Equals("name"))
				{
					configData.ApplicationName = (string)dictionaryEntry.Value;
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "channels"))
					{
						if (!(name == "client"))
						{
							if (!(name == "lifetime"))
							{
								if (!(name == "service"))
								{
									if (name == "soapInterop")
									{
										RemotingXmlConfigFileParser.ProcessSoapInteropNode(configNode, configData);
									}
								}
								else
								{
									RemotingXmlConfigFileParser.ProcessServiceNode(configNode, configData);
								}
							}
							else
							{
								RemotingXmlConfigFileParser.ProcessLifetimeNode(node, configNode, configData);
							}
						}
						else
						{
							RemotingXmlConfigFileParser.ProcessClientNode(configNode, configData);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessChannelsNode(configNode, configData);
					}
				}
			}
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x000E3BC8 File Offset: 0x000E2BC8
		private static void ProcessCustomErrorsNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				if (text.Equals("mode"))
				{
					string text2 = (string)dictionaryEntry.Value;
					CustomErrorsModes mode = CustomErrorsModes.On;
					if (string.Compare(text2, "on", StringComparison.OrdinalIgnoreCase) == 0)
					{
						mode = CustomErrorsModes.On;
					}
					else if (string.Compare(text2, "off", StringComparison.OrdinalIgnoreCase) == 0)
					{
						mode = CustomErrorsModes.Off;
					}
					else if (string.Compare(text2, "remoteonly", StringComparison.OrdinalIgnoreCase) == 0)
					{
						mode = CustomErrorsModes.RemoteOnly;
					}
					else
					{
						RemotingXmlConfigFileParser.ReportUnknownValueError(node, text2, configData);
					}
					configData.CustomErrors = new RemotingXmlConfigFileData.CustomErrorsEntry(mode);
				}
			}
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x000E3C9C File Offset: 0x000E2C9C
		private static void ProcessLifetimeNode(ConfigNode parentNode, ConfigNode node, RemotingXmlConfigFileData configData)
		{
			if (configData.Lifetime != null)
			{
				RemotingXmlConfigFileParser.ReportUniqueSectionError(node, parentNode, configData);
			}
			configData.Lifetime = new RemotingXmlConfigFileData.LifetimeEntry();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text) != null)
				{
					if (!(a == "leaseTime"))
					{
						if (!(a == "sponsorshipTimeout"))
						{
							if (!(a == "renewOnCallTime"))
							{
								if (a == "leaseManagerPollTime")
								{
									configData.Lifetime.LeaseManagerPollTime = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
								}
							}
							else
							{
								configData.Lifetime.RenewOnCallTime = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
							}
						}
						else
						{
							configData.Lifetime.SponsorshipTimeout = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
						}
					}
					else
					{
						configData.Lifetime.LeaseTime = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
					}
				}
			}
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x000E3DD8 File Offset: 0x000E2DD8
		private static void ProcessServiceNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "wellknown"))
					{
						if (name == "activated")
						{
							RemotingXmlConfigFileParser.ProcessServiceActivatedNode(configNode, configData);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessServiceWellKnownNode(configNode, configData);
					}
				}
			}
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x000E3E60 File Offset: 0x000E2E60
		private static void ProcessClientNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text2 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text2) != null)
				{
					if (!(a == "url"))
					{
						if (!(a == "displayName"))
						{
						}
					}
					else
					{
						text = (string)dictionaryEntry.Value;
					}
				}
			}
			RemotingXmlConfigFileData.RemoteAppEntry remoteAppEntry = configData.AddRemoteAppEntry(text);
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "wellknown"))
					{
						if (name == "activated")
						{
							RemotingXmlConfigFileParser.ProcessClientActivatedNode(configNode, configData, remoteAppEntry);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessClientWellKnownNode(configNode, configData, remoteAppEntry);
					}
				}
			}
			if (remoteAppEntry.ActivatedObjects.Count > 0 && text == null)
			{
				RemotingXmlConfigFileParser.ReportMissingAttributeError(node, "url", configData);
			}
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x000E3FA8 File Offset: 0x000E2FA8
		private static void ProcessSoapInteropNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text) != null && a == "urlObjRef")
				{
					configData.UrlObjRefMode = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "preLoad"))
					{
						if (!(name == "interopXmlElement"))
						{
							if (name == "interopXmlType")
							{
								RemotingXmlConfigFileParser.ProcessInteropXmlTypeNode(configNode, configData);
							}
						}
						else
						{
							RemotingXmlConfigFileParser.ProcessInteropXmlElementNode(configNode, configData);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessPreLoadNode(configNode, configData);
					}
				}
			}
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x000E40D0 File Offset: 0x000E30D0
		private static void ProcessChannelsNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				if (configNode.Name.Equals("channel"))
				{
					RemotingXmlConfigFileData.ChannelEntry value = RemotingXmlConfigFileParser.ProcessChannelsChannelNode(configNode, configData, false);
					configData.ChannelEntries.Add(value);
				}
			}
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x000E414C File Offset: 0x000E314C
		private static void ProcessServiceWellKnownNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			ArrayList arrayList = new ArrayList();
			string text3 = null;
			WellKnownObjectMode objMode = WellKnownObjectMode.Singleton;
			bool flag = false;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text4 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text4) != null && !(a == "displayName"))
				{
					if (!(a == "mode"))
					{
						if (!(a == "objectUri"))
						{
							if (a == "type")
							{
								RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
							}
						}
						else
						{
							text3 = (string)dictionaryEntry.Value;
						}
					}
					else
					{
						string strA = (string)dictionaryEntry.Value;
						flag = true;
						if (string.CompareOrdinal(strA, "Singleton") == 0)
						{
							objMode = WellKnownObjectMode.Singleton;
						}
						else if (string.CompareOrdinal(strA, "SingleCall") == 0)
						{
							objMode = WellKnownObjectMode.SingleCall;
						}
						else
						{
							flag = false;
						}
					}
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "contextAttribute"))
					{
						if (!(name == "lifetime"))
						{
						}
					}
					else
					{
						arrayList.Add(RemotingXmlConfigFileParser.ProcessContextAttributeNode(configNode, configData));
					}
				}
			}
			if (!flag)
			{
				RemotingXmlConfigFileParser.ReportError(Environment.GetResourceString("Remoting_Config_MissingWellKnownModeAttribute"), configData);
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			if (text3 == null)
			{
				text3 = text + ".soap";
			}
			configData.AddServerWellKnownEntry(text, text2, arrayList, text3, objMode);
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x000E4338 File Offset: 0x000E3338
		private static void ProcessServiceActivatedNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text3) != null && a == "type")
				{
					RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "contextAttribute"))
					{
						if (!(name == "lifetime"))
						{
						}
					}
					else
					{
						arrayList.Add(RemotingXmlConfigFileParser.ProcessContextAttributeNode(configNode, configData));
					}
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			if (RemotingXmlConfigFileParser.CheckAssemblyNameForVersionInfo(text2))
			{
				RemotingXmlConfigFileParser.ReportAssemblyVersionInfoPresent(text2, "service activated", configData);
			}
			configData.AddServerActivatedEntry(text, text2, arrayList);
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x000E4484 File Offset: 0x000E3484
		private static void ProcessClientWellKnownNode(ConfigNode node, RemotingXmlConfigFileData configData, RemotingXmlConfigFileData.RemoteAppEntry remoteApp)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text4 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text4) != null && !(a == "displayName"))
				{
					if (!(a == "type"))
					{
						if (a == "url")
						{
							text3 = (string)dictionaryEntry.Value;
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
					}
				}
			}
			if (text3 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingAttributeError("WellKnown client", "url", configData);
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			if (RemotingXmlConfigFileParser.CheckAssemblyNameForVersionInfo(text2))
			{
				RemotingXmlConfigFileParser.ReportAssemblyVersionInfoPresent(text2, "client wellknown", configData);
			}
			remoteApp.AddWellKnownEntry(text, text2, text3);
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x000E458C File Offset: 0x000E358C
		private static void ProcessClientActivatedNode(ConfigNode node, RemotingXmlConfigFileData configData, RemotingXmlConfigFileData.RemoteAppEntry remoteApp)
		{
			string text = null;
			string text2 = null;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text3) != null && a == "type")
				{
					RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null && name == "contextAttribute")
				{
					arrayList.Add(RemotingXmlConfigFileParser.ProcessContextAttributeNode(configNode, configData));
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			remoteApp.AddActivatedEntry(text, text2, arrayList);
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x000E46B4 File Offset: 0x000E36B4
		private static void ProcessInteropXmlElementNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text5 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text5) != null)
				{
					if (!(a == "xml"))
					{
						if (a == "clr")
						{
							RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text3, out text4);
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
					}
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingXmlTypeAttributeError(node, "xml", configData);
			}
			if (text3 == null || text4 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "clr", configData);
			}
			configData.AddInteropXmlElementEntry(text, text2, text3, text4);
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x000E47A4 File Offset: 0x000E37A4
		private static void ProcessInteropXmlTypeNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text5 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text5) != null)
				{
					if (!(a == "xml"))
					{
						if (a == "clr")
						{
							RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text3, out text4);
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
					}
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingXmlTypeAttributeError(node, "xml", configData);
			}
			if (text3 == null || text4 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "clr", configData);
			}
			configData.AddInteropXmlTypeEntry(text, text2, text3, text4);
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x000E4894 File Offset: 0x000E3894
		private static void ProcessPreLoadNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string typeName = null;
			string text = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text2 = dictionaryEntry.Key.ToString();
				string a;
				if ((a = text2) != null)
				{
					if (!(a == "type"))
					{
						if (a == "assembly")
						{
							text = (string)dictionaryEntry.Value;
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out typeName, out text);
					}
				}
			}
			if (text == null)
			{
				RemotingXmlConfigFileParser.ReportError(Environment.GetResourceString("Remoting_Config_PreloadRequiresTypeOrAssembly"), configData);
			}
			configData.AddPreLoadEntry(typeName, text);
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x000E4964 File Offset: 0x000E3964
		private static RemotingXmlConfigFileData.ContextAttributeEntry ProcessContextAttributeNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			Hashtable hashtable = RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = ((string)dictionaryEntry.Key).ToLower(CultureInfo.InvariantCulture);
				string a;
				if ((a = text3) != null && a == "type")
				{
					RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
				}
				else
				{
					hashtable[text3] = dictionaryEntry.Value;
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			return new RemotingXmlConfigFileData.ContextAttributeEntry(text, text2, hashtable);
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x000E4A38 File Offset: 0x000E3A38
		private static RemotingXmlConfigFileData.ChannelEntry ProcessChannelsChannelNode(ConfigNode node, RemotingXmlConfigFileData configData, bool isTemplate)
		{
			string key = null;
			string text = null;
			string text2 = null;
			Hashtable hashtable = RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable();
			bool delayLoad = false;
			RemotingXmlConfigFileData.ChannelEntry channelEntry = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = (string)dictionaryEntry.Key;
				string a;
				if ((a = text3) != null)
				{
					if (a == "displayName")
					{
						continue;
					}
					if (!(a == "id"))
					{
						if (!(a == "ref"))
						{
							if (!(a == "type"))
							{
								if (!(a == "delayLoadAsClientChannel"))
								{
									goto IL_19F;
								}
								delayLoad = Convert.ToBoolean((string)dictionaryEntry.Value, CultureInfo.InvariantCulture);
								continue;
							}
						}
						else
						{
							if (isTemplate)
							{
								RemotingXmlConfigFileParser.ReportTemplateCannotReferenceTemplateError(node, configData);
								continue;
							}
							channelEntry = (RemotingXmlConfigFileData.ChannelEntry)RemotingXmlConfigFileParser._channelTemplates[dictionaryEntry.Value];
							if (channelEntry == null)
							{
								RemotingXmlConfigFileParser.ReportUnableToResolveTemplateReferenceError(node, dictionaryEntry.Value.ToString(), configData);
								continue;
							}
							text = channelEntry.TypeName;
							text2 = channelEntry.AssemblyName;
							using (IDictionaryEnumerator enumerator2 = channelEntry.Properties.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
									hashtable[dictionaryEntry2.Key] = dictionaryEntry2.Value;
								}
								continue;
							}
						}
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
						continue;
					}
					if (!isTemplate)
					{
						RemotingXmlConfigFileParser.ReportNonTemplateIdAttributeError(node, configData);
						continue;
					}
					key = ((string)dictionaryEntry.Value).ToLower(CultureInfo.InvariantCulture);
					continue;
				}
				IL_19F:
				hashtable[text3] = dictionaryEntry.Value;
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			RemotingXmlConfigFileData.ChannelEntry channelEntry2 = new RemotingXmlConfigFileData.ChannelEntry(text, text2, hashtable);
			channelEntry2.DelayLoad = delayLoad;
			foreach (object obj3 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj3;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "clientProviders"))
					{
						if (name == "serverProviders")
						{
							RemotingXmlConfigFileParser.ProcessSinkProviderNodes(configNode, channelEntry2, configData, true);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessSinkProviderNodes(configNode, channelEntry2, configData, false);
					}
				}
			}
			if (channelEntry != null)
			{
				if (channelEntry2.ClientSinkProviders.Count == 0)
				{
					channelEntry2.ClientSinkProviders = channelEntry.ClientSinkProviders;
				}
				if (channelEntry2.ServerSinkProviders.Count == 0)
				{
					channelEntry2.ServerSinkProviders = channelEntry.ServerSinkProviders;
				}
			}
			if (isTemplate)
			{
				RemotingXmlConfigFileParser._channelTemplates[key] = channelEntry2;
				return null;
			}
			return channelEntry2;
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x000E4D58 File Offset: 0x000E3D58
		private static void ProcessSinkProviderNodes(ConfigNode node, RemotingXmlConfigFileData.ChannelEntry channelEntry, RemotingXmlConfigFileData configData, bool isServer)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode node2 = (ConfigNode)obj;
				RemotingXmlConfigFileData.SinkProviderEntry value = RemotingXmlConfigFileParser.ProcessSinkProviderNode(node2, configData, false, isServer);
				if (isServer)
				{
					channelEntry.ServerSinkProviders.Add(value);
				}
				else
				{
					channelEntry.ClientSinkProviders.Add(value);
				}
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x000E4DD4 File Offset: 0x000E3DD4
		private static RemotingXmlConfigFileData.SinkProviderEntry ProcessSinkProviderNode(ConfigNode node, RemotingXmlConfigFileData configData, bool isTemplate, bool isServer)
		{
			bool isFormatter = false;
			string name = node.Name;
			if (name.Equals("formatter"))
			{
				isFormatter = true;
			}
			else if (name.Equals("provider"))
			{
				isFormatter = false;
			}
			else
			{
				RemotingXmlConfigFileParser.ReportError(Environment.GetResourceString("Remoting_Config_ProviderNeedsElementName"), configData);
			}
			string key = null;
			string text = null;
			string text2 = null;
			Hashtable hashtable = RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable();
			RemotingXmlConfigFileData.SinkProviderEntry sinkProviderEntry = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = (string)dictionaryEntry.Key;
				string a;
				if ((a = text3) != null)
				{
					if (!(a == "id"))
					{
						if (!(a == "ref"))
						{
							if (!(a == "type"))
							{
								goto IL_1B2;
							}
						}
						else
						{
							if (isTemplate)
							{
								RemotingXmlConfigFileParser.ReportTemplateCannotReferenceTemplateError(node, configData);
								continue;
							}
							if (isServer)
							{
								sinkProviderEntry = (RemotingXmlConfigFileData.SinkProviderEntry)RemotingXmlConfigFileParser._serverChannelSinkTemplates[dictionaryEntry.Value];
							}
							else
							{
								sinkProviderEntry = (RemotingXmlConfigFileData.SinkProviderEntry)RemotingXmlConfigFileParser._clientChannelSinkTemplates[dictionaryEntry.Value];
							}
							if (sinkProviderEntry == null)
							{
								RemotingXmlConfigFileParser.ReportUnableToResolveTemplateReferenceError(node, dictionaryEntry.Value.ToString(), configData);
								continue;
							}
							text = sinkProviderEntry.TypeName;
							text2 = sinkProviderEntry.AssemblyName;
							using (IDictionaryEnumerator enumerator2 = sinkProviderEntry.Properties.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
									hashtable[dictionaryEntry2.Key] = dictionaryEntry2.Value;
								}
								continue;
							}
						}
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
						continue;
					}
					if (!isTemplate)
					{
						RemotingXmlConfigFileParser.ReportNonTemplateIdAttributeError(node, configData);
						continue;
					}
					key = (string)dictionaryEntry.Value;
					continue;
				}
				IL_1B2:
				hashtable[text3] = dictionaryEntry.Value;
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			RemotingXmlConfigFileData.SinkProviderEntry sinkProviderEntry2 = new RemotingXmlConfigFileData.SinkProviderEntry(text, text2, hashtable, isFormatter);
			foreach (object obj3 in node.Children)
			{
				ConfigNode node2 = (ConfigNode)obj3;
				SinkProviderData value = RemotingXmlConfigFileParser.ProcessSinkProviderData(node2, configData);
				sinkProviderEntry2.ProviderData.Add(value);
			}
			if (sinkProviderEntry != null && sinkProviderEntry2.ProviderData.Count == 0)
			{
				sinkProviderEntry2.ProviderData = sinkProviderEntry.ProviderData;
			}
			if (isTemplate)
			{
				if (isServer)
				{
					RemotingXmlConfigFileParser._serverChannelSinkTemplates[key] = sinkProviderEntry2;
				}
				else
				{
					RemotingXmlConfigFileParser._clientChannelSinkTemplates[key] = sinkProviderEntry2;
				}
				return null;
			}
			return sinkProviderEntry2;
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x000E50D0 File Offset: 0x000E40D0
		private static SinkProviderData ProcessSinkProviderData(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			SinkProviderData sinkProviderData = new SinkProviderData(node.Name);
			foreach (object obj in node.Children)
			{
				ConfigNode node2 = (ConfigNode)obj;
				SinkProviderData value = RemotingXmlConfigFileParser.ProcessSinkProviderData(node2, configData);
				sinkProviderData.Children.Add(value);
			}
			foreach (object obj2 in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				sinkProviderData.Properties[dictionaryEntry.Key] = dictionaryEntry.Value;
			}
			return sinkProviderData;
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x000E51AC File Offset: 0x000E41AC
		private static void ProcessChannelTemplates(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				string name;
				if ((name = configNode.Name) != null && name == "channel")
				{
					RemotingXmlConfigFileParser.ProcessChannelsChannelNode(configNode, configData, true);
				}
			}
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x000E5220 File Offset: 0x000E4220
		private static void ProcessChannelSinkProviderTemplates(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "clientProviders"))
					{
						if (name == "serverProviders")
						{
							RemotingXmlConfigFileParser.ProcessChannelProviderTemplates(configNode, configData, true);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessChannelProviderTemplates(configNode, configData, false);
					}
				}
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x000E52AC File Offset: 0x000E42AC
		private static void ProcessChannelProviderTemplates(ConfigNode node, RemotingXmlConfigFileData configData, bool isServer)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode node2 = (ConfigNode)obj;
				RemotingXmlConfigFileParser.ProcessSinkProviderNode(node2, configData, true, isServer);
			}
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x000E5308 File Offset: 0x000E4308
		private static bool CheckAssemblyNameForVersionInfo(string assemName)
		{
			if (assemName == null)
			{
				return false;
			}
			int num = assemName.IndexOf(',');
			return num != -1;
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x000E532C File Offset: 0x000E432C
		private static TimeSpan ParseTime(string time, RemotingXmlConfigFileData configData)
		{
			string time2 = time;
			string text = "s";
			int num = 0;
			char c = ' ';
			if (time.Length > 0)
			{
				c = time[time.Length - 1];
			}
			TimeSpan result = TimeSpan.FromSeconds(0.0);
			try
			{
				if (!char.IsDigit(c))
				{
					if (time.Length == 0)
					{
						RemotingXmlConfigFileParser.ReportInvalidTimeFormatError(time2, configData);
					}
					time = time.ToLower(CultureInfo.InvariantCulture);
					num = 1;
					if (time.EndsWith("ms", StringComparison.Ordinal))
					{
						num = 2;
					}
					text = time.Substring(time.Length - num, num);
				}
				int num2 = int.Parse(time.Substring(0, time.Length - num), CultureInfo.InvariantCulture);
				string a;
				if ((a = text) != null)
				{
					if (a == "d")
					{
						result = TimeSpan.FromDays((double)num2);
						goto IL_12A;
					}
					if (a == "h")
					{
						result = TimeSpan.FromHours((double)num2);
						goto IL_12A;
					}
					if (a == "m")
					{
						result = TimeSpan.FromMinutes((double)num2);
						goto IL_12A;
					}
					if (a == "s")
					{
						result = TimeSpan.FromSeconds((double)num2);
						goto IL_12A;
					}
					if (a == "ms")
					{
						result = TimeSpan.FromMilliseconds((double)num2);
						goto IL_12A;
					}
				}
				RemotingXmlConfigFileParser.ReportInvalidTimeFormatError(time2, configData);
				IL_12A:;
			}
			catch (Exception)
			{
				RemotingXmlConfigFileParser.ReportInvalidTimeFormatError(time2, configData);
			}
			return result;
		}

		// Token: 0x040021BF RID: 8639
		private static Hashtable _channelTemplates = RemotingXmlConfigFileParser.CreateSyncCaseInsensitiveHashtable();

		// Token: 0x040021C0 RID: 8640
		private static Hashtable _clientChannelSinkTemplates = RemotingXmlConfigFileParser.CreateSyncCaseInsensitiveHashtable();

		// Token: 0x040021C1 RID: 8641
		private static Hashtable _serverChannelSinkTemplates = RemotingXmlConfigFileParser.CreateSyncCaseInsensitiveHashtable();
	}
}
