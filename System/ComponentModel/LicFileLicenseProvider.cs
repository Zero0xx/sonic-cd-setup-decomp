using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200010F RID: 271
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class LicFileLicenseProvider : LicenseProvider
	{
		// Token: 0x06000871 RID: 2161 RVA: 0x0001CAE7 File Offset: 0x0001BAE7
		protected virtual bool IsKeyValid(string key, Type type)
		{
			return key != null && key.StartsWith(this.GetKey(type));
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0001CAFC File Offset: 0x0001BAFC
		protected virtual string GetKey(Type type)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} is a licensed component.", new object[]
			{
				type.FullName
			});
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0001CB2C File Offset: 0x0001BB2C
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
			LicFileLicenseProvider.LicFileLicense licFileLicense = null;
			if (context != null)
			{
				if (context.UsageMode == LicenseUsageMode.Runtime)
				{
					string savedLicenseKey = context.GetSavedLicenseKey(type, null);
					if (savedLicenseKey != null && this.IsKeyValid(savedLicenseKey, type))
					{
						licFileLicense = new LicFileLicenseProvider.LicFileLicense(this, savedLicenseKey);
					}
				}
				if (licFileLicense == null)
				{
					string text = null;
					if (context != null)
					{
						ITypeResolutionService typeResolutionService = (ITypeResolutionService)context.GetService(typeof(ITypeResolutionService));
						if (typeResolutionService != null)
						{
							text = typeResolutionService.GetPathOfAssembly(type.Assembly.GetName());
						}
					}
					if (text == null)
					{
						text = type.Module.FullyQualifiedName;
					}
					string directoryName = Path.GetDirectoryName(text);
					string path = directoryName + "\\" + type.FullName + ".lic";
					if (File.Exists(path))
					{
						Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
						StreamReader streamReader = new StreamReader(stream);
						string key = streamReader.ReadLine();
						streamReader.Close();
						if (this.IsKeyValid(key, type))
						{
							licFileLicense = new LicFileLicenseProvider.LicFileLicense(this, this.GetKey(type));
						}
					}
					if (licFileLicense != null)
					{
						context.SetSavedLicenseKey(type, licFileLicense.LicenseKey);
					}
				}
			}
			return licFileLicense;
		}

		// Token: 0x02000110 RID: 272
		private class LicFileLicense : License
		{
			// Token: 0x06000875 RID: 2165 RVA: 0x0001CC2D File Offset: 0x0001BC2D
			public LicFileLicense(LicFileLicenseProvider owner, string key)
			{
				this.owner = owner;
				this.key = key;
			}

			// Token: 0x170001B8 RID: 440
			// (get) Token: 0x06000876 RID: 2166 RVA: 0x0001CC43 File Offset: 0x0001BC43
			public override string LicenseKey
			{
				get
				{
					return this.key;
				}
			}

			// Token: 0x06000877 RID: 2167 RVA: 0x0001CC4B File Offset: 0x0001BC4B
			public override void Dispose()
			{
			}

			// Token: 0x0400099C RID: 2460
			private LicFileLicenseProvider owner;

			// Token: 0x0400099D RID: 2461
			private string key;
		}
	}
}
