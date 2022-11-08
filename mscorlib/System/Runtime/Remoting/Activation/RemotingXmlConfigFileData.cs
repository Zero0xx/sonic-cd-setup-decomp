using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x0200074E RID: 1870
	internal class RemotingXmlConfigFileData
	{
		// Token: 0x060042B6 RID: 17078 RVA: 0x000E2A5C File Offset: 0x000E1A5C
		internal void AddInteropXmlElementEntry(string xmlElementName, string xmlElementNamespace, string urtTypeName, string urtAssemblyName)
		{
			this.TryToLoadTypeIfApplicable(urtTypeName, urtAssemblyName);
			RemotingXmlConfigFileData.InteropXmlElementEntry value = new RemotingXmlConfigFileData.InteropXmlElementEntry(xmlElementName, xmlElementNamespace, urtTypeName, urtAssemblyName);
			this.InteropXmlElementEntries.Add(value);
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x000E2A8C File Offset: 0x000E1A8C
		internal void AddInteropXmlTypeEntry(string xmlTypeName, string xmlTypeNamespace, string urtTypeName, string urtAssemblyName)
		{
			this.TryToLoadTypeIfApplicable(urtTypeName, urtAssemblyName);
			RemotingXmlConfigFileData.InteropXmlTypeEntry value = new RemotingXmlConfigFileData.InteropXmlTypeEntry(xmlTypeName, xmlTypeNamespace, urtTypeName, urtAssemblyName);
			this.InteropXmlTypeEntries.Add(value);
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x000E2ABC File Offset: 0x000E1ABC
		internal void AddPreLoadEntry(string typeName, string assemblyName)
		{
			this.TryToLoadTypeIfApplicable(typeName, assemblyName);
			RemotingXmlConfigFileData.PreLoadEntry value = new RemotingXmlConfigFileData.PreLoadEntry(typeName, assemblyName);
			this.PreLoadEntries.Add(value);
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x000E2AE8 File Offset: 0x000E1AE8
		internal RemotingXmlConfigFileData.RemoteAppEntry AddRemoteAppEntry(string appUri)
		{
			RemotingXmlConfigFileData.RemoteAppEntry remoteAppEntry = new RemotingXmlConfigFileData.RemoteAppEntry(appUri);
			this.RemoteAppEntries.Add(remoteAppEntry);
			return remoteAppEntry;
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x000E2B0C File Offset: 0x000E1B0C
		internal void AddServerActivatedEntry(string typeName, string assemName, ArrayList contextAttributes)
		{
			this.TryToLoadTypeIfApplicable(typeName, assemName);
			RemotingXmlConfigFileData.TypeEntry value = new RemotingXmlConfigFileData.TypeEntry(typeName, assemName, contextAttributes);
			this.ServerActivatedEntries.Add(value);
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x000E2B38 File Offset: 0x000E1B38
		internal RemotingXmlConfigFileData.ServerWellKnownEntry AddServerWellKnownEntry(string typeName, string assemName, ArrayList contextAttributes, string objURI, WellKnownObjectMode objMode)
		{
			this.TryToLoadTypeIfApplicable(typeName, assemName);
			RemotingXmlConfigFileData.ServerWellKnownEntry serverWellKnownEntry = new RemotingXmlConfigFileData.ServerWellKnownEntry(typeName, assemName, contextAttributes, objURI, objMode);
			this.ServerWellKnownEntries.Add(serverWellKnownEntry);
			return serverWellKnownEntry;
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x000E2B68 File Offset: 0x000E1B68
		private void TryToLoadTypeIfApplicable(string typeName, string assemblyName)
		{
			if (!RemotingXmlConfigFileData.LoadTypes)
			{
				return;
			}
			Assembly assembly = Assembly.Load(assemblyName);
			if (assembly == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_AssemblyLoadFailed"), new object[]
				{
					assemblyName
				}));
			}
			if (assembly.GetType(typeName, false, false) == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[]
				{
					typeName
				}));
			}
		}

		// Token: 0x04002187 RID: 8583
		internal static bool LoadTypes;

		// Token: 0x04002188 RID: 8584
		internal string ApplicationName;

		// Token: 0x04002189 RID: 8585
		internal RemotingXmlConfigFileData.LifetimeEntry Lifetime;

		// Token: 0x0400218A RID: 8586
		internal bool UrlObjRefMode = RemotingConfigHandler.UrlObjRefMode;

		// Token: 0x0400218B RID: 8587
		internal RemotingXmlConfigFileData.CustomErrorsEntry CustomErrors;

		// Token: 0x0400218C RID: 8588
		internal ArrayList ChannelEntries = new ArrayList();

		// Token: 0x0400218D RID: 8589
		internal ArrayList InteropXmlElementEntries = new ArrayList();

		// Token: 0x0400218E RID: 8590
		internal ArrayList InteropXmlTypeEntries = new ArrayList();

		// Token: 0x0400218F RID: 8591
		internal ArrayList PreLoadEntries = new ArrayList();

		// Token: 0x04002190 RID: 8592
		internal ArrayList RemoteAppEntries = new ArrayList();

		// Token: 0x04002191 RID: 8593
		internal ArrayList ServerActivatedEntries = new ArrayList();

		// Token: 0x04002192 RID: 8594
		internal ArrayList ServerWellKnownEntries = new ArrayList();

		// Token: 0x0200074F RID: 1871
		internal class ChannelEntry
		{
			// Token: 0x060042BE RID: 17086 RVA: 0x000E2C4B File Offset: 0x000E1C4B
			internal ChannelEntry(string typeName, string assemblyName, Hashtable properties)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemblyName;
				this.Properties = properties;
			}

			// Token: 0x04002193 RID: 8595
			internal string TypeName;

			// Token: 0x04002194 RID: 8596
			internal string AssemblyName;

			// Token: 0x04002195 RID: 8597
			internal Hashtable Properties;

			// Token: 0x04002196 RID: 8598
			internal bool DelayLoad;

			// Token: 0x04002197 RID: 8599
			internal ArrayList ClientSinkProviders = new ArrayList();

			// Token: 0x04002198 RID: 8600
			internal ArrayList ServerSinkProviders = new ArrayList();
		}

		// Token: 0x02000750 RID: 1872
		internal class ClientWellKnownEntry
		{
			// Token: 0x060042BF RID: 17087 RVA: 0x000E2C7E File Offset: 0x000E1C7E
			internal ClientWellKnownEntry(string typeName, string assemName, string url)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.Url = url;
			}

			// Token: 0x04002199 RID: 8601
			internal string TypeName;

			// Token: 0x0400219A RID: 8602
			internal string AssemblyName;

			// Token: 0x0400219B RID: 8603
			internal string Url;
		}

		// Token: 0x02000751 RID: 1873
		internal class ContextAttributeEntry
		{
			// Token: 0x060042C0 RID: 17088 RVA: 0x000E2C9B File Offset: 0x000E1C9B
			internal ContextAttributeEntry(string typeName, string assemName, Hashtable properties)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.Properties = properties;
			}

			// Token: 0x0400219C RID: 8604
			internal string TypeName;

			// Token: 0x0400219D RID: 8605
			internal string AssemblyName;

			// Token: 0x0400219E RID: 8606
			internal Hashtable Properties;
		}

		// Token: 0x02000752 RID: 1874
		internal class InteropXmlElementEntry
		{
			// Token: 0x060042C1 RID: 17089 RVA: 0x000E2CB8 File Offset: 0x000E1CB8
			internal InteropXmlElementEntry(string xmlElementName, string xmlElementNamespace, string urtTypeName, string urtAssemblyName)
			{
				this.XmlElementName = xmlElementName;
				this.XmlElementNamespace = xmlElementNamespace;
				this.UrtTypeName = urtTypeName;
				this.UrtAssemblyName = urtAssemblyName;
			}

			// Token: 0x0400219F RID: 8607
			internal string XmlElementName;

			// Token: 0x040021A0 RID: 8608
			internal string XmlElementNamespace;

			// Token: 0x040021A1 RID: 8609
			internal string UrtTypeName;

			// Token: 0x040021A2 RID: 8610
			internal string UrtAssemblyName;
		}

		// Token: 0x02000753 RID: 1875
		internal class CustomErrorsEntry
		{
			// Token: 0x060042C2 RID: 17090 RVA: 0x000E2CDD File Offset: 0x000E1CDD
			internal CustomErrorsEntry(CustomErrorsModes mode)
			{
				this.Mode = mode;
			}

			// Token: 0x040021A3 RID: 8611
			internal CustomErrorsModes Mode;
		}

		// Token: 0x02000754 RID: 1876
		internal class InteropXmlTypeEntry
		{
			// Token: 0x060042C3 RID: 17091 RVA: 0x000E2CEC File Offset: 0x000E1CEC
			internal InteropXmlTypeEntry(string xmlTypeName, string xmlTypeNamespace, string urtTypeName, string urtAssemblyName)
			{
				this.XmlTypeName = xmlTypeName;
				this.XmlTypeNamespace = xmlTypeNamespace;
				this.UrtTypeName = urtTypeName;
				this.UrtAssemblyName = urtAssemblyName;
			}

			// Token: 0x040021A4 RID: 8612
			internal string XmlTypeName;

			// Token: 0x040021A5 RID: 8613
			internal string XmlTypeNamespace;

			// Token: 0x040021A6 RID: 8614
			internal string UrtTypeName;

			// Token: 0x040021A7 RID: 8615
			internal string UrtAssemblyName;
		}

		// Token: 0x02000755 RID: 1877
		internal class LifetimeEntry
		{
			// Token: 0x17000BCB RID: 3019
			// (get) Token: 0x060042C4 RID: 17092 RVA: 0x000E2D11 File Offset: 0x000E1D11
			// (set) Token: 0x060042C5 RID: 17093 RVA: 0x000E2D19 File Offset: 0x000E1D19
			internal TimeSpan LeaseTime
			{
				get
				{
					return this._leaseTime;
				}
				set
				{
					this._leaseTime = value;
					this.IsLeaseTimeSet = true;
				}
			}

			// Token: 0x17000BCC RID: 3020
			// (get) Token: 0x060042C6 RID: 17094 RVA: 0x000E2D29 File Offset: 0x000E1D29
			// (set) Token: 0x060042C7 RID: 17095 RVA: 0x000E2D31 File Offset: 0x000E1D31
			internal TimeSpan RenewOnCallTime
			{
				get
				{
					return this._renewOnCallTime;
				}
				set
				{
					this._renewOnCallTime = value;
					this.IsRenewOnCallTimeSet = true;
				}
			}

			// Token: 0x17000BCD RID: 3021
			// (get) Token: 0x060042C8 RID: 17096 RVA: 0x000E2D41 File Offset: 0x000E1D41
			// (set) Token: 0x060042C9 RID: 17097 RVA: 0x000E2D49 File Offset: 0x000E1D49
			internal TimeSpan SponsorshipTimeout
			{
				get
				{
					return this._sponsorshipTimeout;
				}
				set
				{
					this._sponsorshipTimeout = value;
					this.IsSponsorshipTimeoutSet = true;
				}
			}

			// Token: 0x17000BCE RID: 3022
			// (get) Token: 0x060042CA RID: 17098 RVA: 0x000E2D59 File Offset: 0x000E1D59
			// (set) Token: 0x060042CB RID: 17099 RVA: 0x000E2D61 File Offset: 0x000E1D61
			internal TimeSpan LeaseManagerPollTime
			{
				get
				{
					return this._leaseManagerPollTime;
				}
				set
				{
					this._leaseManagerPollTime = value;
					this.IsLeaseManagerPollTimeSet = true;
				}
			}

			// Token: 0x040021A8 RID: 8616
			internal bool IsLeaseTimeSet;

			// Token: 0x040021A9 RID: 8617
			internal bool IsRenewOnCallTimeSet;

			// Token: 0x040021AA RID: 8618
			internal bool IsSponsorshipTimeoutSet;

			// Token: 0x040021AB RID: 8619
			internal bool IsLeaseManagerPollTimeSet;

			// Token: 0x040021AC RID: 8620
			private TimeSpan _leaseTime;

			// Token: 0x040021AD RID: 8621
			private TimeSpan _renewOnCallTime;

			// Token: 0x040021AE RID: 8622
			private TimeSpan _sponsorshipTimeout;

			// Token: 0x040021AF RID: 8623
			private TimeSpan _leaseManagerPollTime;
		}

		// Token: 0x02000756 RID: 1878
		internal class PreLoadEntry
		{
			// Token: 0x060042CD RID: 17101 RVA: 0x000E2D79 File Offset: 0x000E1D79
			public PreLoadEntry(string typeName, string assemblyName)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemblyName;
			}

			// Token: 0x040021B0 RID: 8624
			internal string TypeName;

			// Token: 0x040021B1 RID: 8625
			internal string AssemblyName;
		}

		// Token: 0x02000757 RID: 1879
		internal class RemoteAppEntry
		{
			// Token: 0x060042CE RID: 17102 RVA: 0x000E2D8F File Offset: 0x000E1D8F
			internal RemoteAppEntry(string appUri)
			{
				this.AppUri = appUri;
			}

			// Token: 0x060042CF RID: 17103 RVA: 0x000E2DB4 File Offset: 0x000E1DB4
			internal void AddWellKnownEntry(string typeName, string assemName, string url)
			{
				RemotingXmlConfigFileData.ClientWellKnownEntry value = new RemotingXmlConfigFileData.ClientWellKnownEntry(typeName, assemName, url);
				this.WellKnownObjects.Add(value);
			}

			// Token: 0x060042D0 RID: 17104 RVA: 0x000E2DD8 File Offset: 0x000E1DD8
			internal void AddActivatedEntry(string typeName, string assemName, ArrayList contextAttributes)
			{
				RemotingXmlConfigFileData.TypeEntry value = new RemotingXmlConfigFileData.TypeEntry(typeName, assemName, contextAttributes);
				this.ActivatedObjects.Add(value);
			}

			// Token: 0x040021B2 RID: 8626
			internal string AppUri;

			// Token: 0x040021B3 RID: 8627
			internal ArrayList WellKnownObjects = new ArrayList();

			// Token: 0x040021B4 RID: 8628
			internal ArrayList ActivatedObjects = new ArrayList();
		}

		// Token: 0x02000758 RID: 1880
		internal class TypeEntry
		{
			// Token: 0x060042D1 RID: 17105 RVA: 0x000E2DFB File Offset: 0x000E1DFB
			internal TypeEntry(string typeName, string assemName, ArrayList contextAttributes)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.ContextAttributes = contextAttributes;
			}

			// Token: 0x040021B5 RID: 8629
			internal string TypeName;

			// Token: 0x040021B6 RID: 8630
			internal string AssemblyName;

			// Token: 0x040021B7 RID: 8631
			internal ArrayList ContextAttributes;
		}

		// Token: 0x02000759 RID: 1881
		internal class ServerWellKnownEntry : RemotingXmlConfigFileData.TypeEntry
		{
			// Token: 0x060042D2 RID: 17106 RVA: 0x000E2E18 File Offset: 0x000E1E18
			internal ServerWellKnownEntry(string typeName, string assemName, ArrayList contextAttributes, string objURI, WellKnownObjectMode objMode) : base(typeName, assemName, contextAttributes)
			{
				this.ObjectURI = objURI;
				this.ObjectMode = objMode;
			}

			// Token: 0x040021B8 RID: 8632
			internal string ObjectURI;

			// Token: 0x040021B9 RID: 8633
			internal WellKnownObjectMode ObjectMode;
		}

		// Token: 0x0200075A RID: 1882
		internal class SinkProviderEntry
		{
			// Token: 0x060042D3 RID: 17107 RVA: 0x000E2E33 File Offset: 0x000E1E33
			internal SinkProviderEntry(string typeName, string assemName, Hashtable properties, bool isFormatter)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.Properties = properties;
				this.IsFormatter = isFormatter;
			}

			// Token: 0x040021BA RID: 8634
			internal string TypeName;

			// Token: 0x040021BB RID: 8635
			internal string AssemblyName;

			// Token: 0x040021BC RID: 8636
			internal Hashtable Properties;

			// Token: 0x040021BD RID: 8637
			internal ArrayList ProviderData = new ArrayList();

			// Token: 0x040021BE RID: 8638
			internal bool IsFormatter;
		}
	}
}
