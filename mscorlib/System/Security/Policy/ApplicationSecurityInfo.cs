using System;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;
using System.Threading;

namespace System.Security.Policy
{
	// Token: 0x02000497 RID: 1175
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ApplicationSecurityInfo
	{
		// Token: 0x06002E89 RID: 11913 RVA: 0x0009CF18 File Offset: 0x0009BF18
		internal ApplicationSecurityInfo()
		{
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x0009CF20 File Offset: 0x0009BF20
		public ApplicationSecurityInfo(ActivationContext activationContext)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			this.m_context = activationContext;
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002E8B RID: 11915 RVA: 0x0009CF40 File Offset: 0x0009BF40
		// (set) Token: 0x06002E8C RID: 11916 RVA: 0x0009CF89 File Offset: 0x0009BF89
		public ApplicationId ApplicationId
		{
			get
			{
				if (this.m_appId == null && this.m_context != null)
				{
					ICMS applicationComponentManifest = this.m_context.ApplicationComponentManifest;
					ApplicationId value = ApplicationSecurityInfo.ParseApplicationId(applicationComponentManifest);
					Interlocked.CompareExchange(ref this.m_appId, value, null);
				}
				return this.m_appId as ApplicationId;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_appId = value;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002E8D RID: 11917 RVA: 0x0009CFA0 File Offset: 0x0009BFA0
		// (set) Token: 0x06002E8E RID: 11918 RVA: 0x0009CFE9 File Offset: 0x0009BFE9
		public ApplicationId DeploymentId
		{
			get
			{
				if (this.m_deployId == null && this.m_context != null)
				{
					ICMS deploymentComponentManifest = this.m_context.DeploymentComponentManifest;
					ApplicationId value = ApplicationSecurityInfo.ParseApplicationId(deploymentComponentManifest);
					Interlocked.CompareExchange(ref this.m_deployId, value, null);
				}
				return this.m_deployId as ApplicationId;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_deployId = value;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002E8F RID: 11919 RVA: 0x0009D000 File Offset: 0x0009C000
		// (set) Token: 0x06002E90 RID: 11920 RVA: 0x0009D1B3 File Offset: 0x0009C1B3
		public PermissionSet DefaultRequestSet
		{
			get
			{
				if (this.m_defaultRequest == null)
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					if (this.m_context != null)
					{
						ICMS applicationComponentManifest = this.m_context.ApplicationComponentManifest;
						string defaultPermissionSetID = ((IMetadataSectionEntry)applicationComponentManifest.MetadataSectionEntry).defaultPermissionSetID;
						object obj = null;
						if (defaultPermissionSetID != null && defaultPermissionSetID.Length > 0)
						{
							((ISectionWithStringKey)applicationComponentManifest.PermissionSetSection).Lookup(defaultPermissionSetID, out obj);
							IPermissionSetEntry permissionSetEntry = obj as IPermissionSetEntry;
							if (permissionSetEntry != null)
							{
								SecurityElement securityElement = SecurityElement.FromString(permissionSetEntry.AllData.XmlSegment);
								string text = securityElement.Attribute("temp:Unrestricted");
								if (text != null)
								{
									securityElement.AddAttribute("Unrestricted", text);
								}
								permissionSet = new PermissionSet(PermissionState.None);
								permissionSet.FromXml(securityElement);
								string strA = securityElement.Attribute("SameSite");
								if (string.Compare(strA, "Site", StringComparison.OrdinalIgnoreCase) == 0)
								{
									NetCodeGroup netCodeGroup = new NetCodeGroup(new AllMembershipCondition());
									Url url = new Url(this.m_context.Identity.CodeBase);
									PolicyStatement policyStatement = netCodeGroup.CalculatePolicy(url.GetURLString().Host, url.GetURLString().Scheme, url.GetURLString().Port);
									if (policyStatement != null)
									{
										PermissionSet permissionSet2 = policyStatement.PermissionSet;
										if (permissionSet2 != null)
										{
											permissionSet.InplaceUnion(permissionSet2);
										}
									}
									if (string.Compare("file:", 0, this.m_context.Identity.CodeBase, 0, 5, StringComparison.OrdinalIgnoreCase) == 0)
									{
										FileCodeGroup fileCodeGroup = new FileCodeGroup(new AllMembershipCondition(), FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery);
										policyStatement = fileCodeGroup.CalculatePolicy(url);
										if (policyStatement != null)
										{
											PermissionSet permissionSet3 = policyStatement.PermissionSet;
											if (permissionSet3 != null)
											{
												permissionSet.InplaceUnion(permissionSet3);
											}
										}
									}
								}
							}
						}
					}
					Interlocked.CompareExchange(ref this.m_defaultRequest, permissionSet, null);
				}
				return this.m_defaultRequest as PermissionSet;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_defaultRequest = value;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002E91 RID: 11921 RVA: 0x0009D1CC File Offset: 0x0009C1CC
		// (set) Token: 0x06002E92 RID: 11922 RVA: 0x0009D2C9 File Offset: 0x0009C2C9
		public Evidence ApplicationEvidence
		{
			get
			{
				if (this.m_appEvidence == null)
				{
					Evidence evidence = new Evidence();
					if (this.m_context != null)
					{
						evidence = new Evidence();
						Url id = new Url(this.m_context.Identity.CodeBase);
						evidence.AddHost(id);
						evidence.AddHost(Zone.CreateFromUrl(this.m_context.Identity.CodeBase));
						if (string.Compare("file:", 0, this.m_context.Identity.CodeBase, 0, 5, StringComparison.OrdinalIgnoreCase) != 0)
						{
							evidence.AddHost(Site.CreateFromUrl(this.m_context.Identity.CodeBase));
						}
						evidence.AddHost(new StrongName(new StrongNamePublicKeyBlob(this.DeploymentId.m_publicKeyToken), this.DeploymentId.Name, this.DeploymentId.Version));
						evidence.AddHost(new ActivationArguments(this.m_context));
					}
					Interlocked.CompareExchange(ref this.m_appEvidence, evidence, null);
				}
				return this.m_appEvidence as Evidence;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_appEvidence = value;
			}
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x0009D2E0 File Offset: 0x0009C2E0
		private static ApplicationId ParseApplicationId(ICMS manifest)
		{
			if (manifest.Identity == null)
			{
				return null;
			}
			return new ApplicationId(Hex.DecodeHexString(manifest.Identity.GetAttribute("", "publicKeyToken")), manifest.Identity.GetAttribute("", "name"), new Version(manifest.Identity.GetAttribute("", "version")), manifest.Identity.GetAttribute("", "processorArchitecture"), manifest.Identity.GetAttribute("", "culture"));
		}

		// Token: 0x040017CF RID: 6095
		private ActivationContext m_context;

		// Token: 0x040017D0 RID: 6096
		private object m_appId;

		// Token: 0x040017D1 RID: 6097
		private object m_deployId;

		// Token: 0x040017D2 RID: 6098
		private object m_defaultRequest;

		// Token: 0x040017D3 RID: 6099
		private object m_appEvidence;
	}
}
