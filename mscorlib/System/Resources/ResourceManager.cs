using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Resources
{
	// Token: 0x02000435 RID: 1077
	[ComVisible(true)]
	[Serializable]
	public class ResourceManager
	{
		// Token: 0x06002BDE RID: 11230 RVA: 0x00092B54 File Offset: 0x00091B54
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected ResourceManager()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x00092B78 File Offset: 0x00091B78
		private ResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (resourceDir == null)
			{
				throw new ArgumentNullException("resourceDir");
			}
			this.BaseNameField = baseName;
			this.moduleDir = resourceDir;
			this._userResourceSet = usingResourceSet;
			this.ResourceSets = new Hashtable();
			this.UseManifest = false;
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x00092BD0 File Offset: 0x00091BD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(string baseName, Assembly assembly)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.MainAssembly = assembly;
			this._locationInfo = null;
			this.BaseNameField = baseName;
			this.CommonSatelliteAssemblyInit();
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
			if (assembly == typeof(object).Assembly && this._callingAssembly != assembly)
			{
				this._callingAssembly = null;
			}
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x00092C4C File Offset: 0x00091C4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(string baseName, Assembly assembly, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.MainAssembly = assembly;
			this._locationInfo = null;
			this.BaseNameField = baseName;
			if (usingResourceSet != null && usingResourceSet != ResourceManager._minResourceSet && !usingResourceSet.IsSubclassOf(ResourceManager._minResourceSet))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ResMgrNotResSet"), "usingResourceSet");
			}
			this._userResourceSet = usingResourceSet;
			this.CommonSatelliteAssemblyInit();
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
			if (assembly == typeof(object).Assembly && this._callingAssembly != assembly)
			{
				this._callingAssembly = null;
			}
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x00092CFC File Offset: 0x00091CFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(Type resourceSource)
		{
			if (resourceSource == null)
			{
				throw new ArgumentNullException("resourceSource");
			}
			this._locationInfo = resourceSource;
			this.MainAssembly = this._locationInfo.Assembly;
			this.BaseNameField = resourceSource.Name;
			this.CommonSatelliteAssemblyInit();
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			this._callingAssembly = Assembly.nGetExecutingAssembly(ref stackCrawlMark);
			if (this.MainAssembly == typeof(object).Assembly && this._callingAssembly != this.MainAssembly)
			{
				this._callingAssembly = null;
			}
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00092D82 File Offset: 0x00091D82
		private void CommonSatelliteAssemblyInit()
		{
			this.UseManifest = true;
			this.UseSatelliteAssem = true;
			this.ResourceSets = new Hashtable();
			this._fallbackLoc = UltimateResourceFallbackLocation.MainAssembly;
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002BE4 RID: 11236 RVA: 0x00092DA4 File Offset: 0x00091DA4
		public virtual string BaseName
		{
			get
			{
				return this.BaseNameField;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002BE5 RID: 11237 RVA: 0x00092DAC File Offset: 0x00091DAC
		// (set) Token: 0x06002BE6 RID: 11238 RVA: 0x00092DB4 File Offset: 0x00091DB4
		public virtual bool IgnoreCase
		{
			get
			{
				return this._ignoreCase;
			}
			set
			{
				this._ignoreCase = value;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002BE7 RID: 11239 RVA: 0x00092DBD File Offset: 0x00091DBD
		public virtual Type ResourceSetType
		{
			get
			{
				if (this._userResourceSet != null)
				{
					return this._userResourceSet;
				}
				return typeof(RuntimeResourceSet);
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x00092DD8 File Offset: 0x00091DD8
		// (set) Token: 0x06002BE9 RID: 11241 RVA: 0x00092DE0 File Offset: 0x00091DE0
		protected UltimateResourceFallbackLocation FallbackLocation
		{
			get
			{
				return this._fallbackLoc;
			}
			set
			{
				this._fallbackLoc = value;
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00092DEC File Offset: 0x00091DEC
		public virtual void ReleaseAllResources()
		{
			IDictionaryEnumerator enumerator = this.ResourceSets.GetEnumerator();
			this.ResourceSets = new Hashtable();
			while (enumerator.MoveNext())
			{
				((ResourceSet)enumerator.Value).Close();
			}
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x00092E2A File Offset: 0x00091E2A
		public static ResourceManager CreateFileBasedResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			return new ResourceManager(baseName, resourceDir, usingResourceSet);
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x00092E34 File Offset: 0x00091E34
		private string FindResourceFile(CultureInfo culture)
		{
			string resourceFileName = this.GetResourceFileName(culture);
			if (this.moduleDir != null)
			{
				string text = Path.Combine(this.moduleDir, resourceFileName);
				if (File.Exists(text))
				{
					return text;
				}
			}
			if (File.Exists(resourceFileName))
			{
				return resourceFileName;
			}
			return null;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x00092E74 File Offset: 0x00091E74
		protected virtual string GetResourceFileName(CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			stringBuilder.Append(this.BaseNameField);
			if (!culture.Equals(CultureInfo.InvariantCulture))
			{
				CultureInfo.VerifyCultureName(culture, true);
				stringBuilder.Append('.');
				stringBuilder.Append(culture.Name);
			}
			stringBuilder.Append(".resources");
			return stringBuilder.ToString();
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x00092ED8 File Offset: 0x00091ED8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Hashtable resourceSets = this.ResourceSets;
			if (resourceSets != null)
			{
				ResourceSet resourceSet = (ResourceSet)resourceSets[culture];
				if (resourceSet != null)
				{
					return resourceSet;
				}
			}
			if (this.UseManifest && culture.Equals(CultureInfo.InvariantCulture))
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				string resourceFileName = this.GetResourceFileName(culture);
				Stream manifestResourceStream = this.MainAssembly.GetManifestResourceStream(this._locationInfo, resourceFileName, this._callingAssembly == this.MainAssembly, ref stackCrawlMark);
				if (createIfNotExists && manifestResourceStream != null)
				{
					ResourceSet resourceSet = this.CreateResourceSet(manifestResourceStream, this.MainAssembly);
					lock (resourceSets)
					{
						resourceSets.Add(culture, resourceSet);
					}
					return resourceSet;
				}
			}
			return this.InternalGetResourceSet(culture, createIfNotExists, tryParents);
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x00092FA0 File Offset: 0x00091FA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected virtual ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			Hashtable resourceSets = this.ResourceSets;
			ResourceSet resourceSet = (ResourceSet)resourceSets[culture];
			if (resourceSet != null)
			{
				return resourceSet;
			}
			Stream stream = null;
			Assembly assembly = null;
			if (this.UseManifest)
			{
				string text = this.GetResourceFileName(culture);
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				if (this.UseSatelliteAssem)
				{
					CultureInfo cultureInfo = culture;
					if (this._neutralResourcesCulture == null)
					{
						this._neutralResourcesCulture = ResourceManager.GetNeutralResourcesLanguage(this.MainAssembly, ref this._fallbackLoc);
					}
					if (culture.Equals(this._neutralResourcesCulture) && this.FallbackLocation == UltimateResourceFallbackLocation.MainAssembly)
					{
						cultureInfo = CultureInfo.InvariantCulture;
						text = this.GetResourceFileName(cultureInfo);
					}
					if (cultureInfo.Equals(CultureInfo.InvariantCulture))
					{
						if (this.FallbackLocation == UltimateResourceFallbackLocation.Satellite)
						{
							assembly = this.GetSatelliteAssembly(this._neutralResourcesCulture);
							if (assembly == null)
							{
								string text2 = this.MainAssembly.nGetSimpleName() + ".resources.dll";
								if (this._satelliteContractVersion != null)
								{
									text2 = text2 + ", Version=" + this._satelliteContractVersion.ToString();
								}
								AssemblyName assemblyName = new AssemblyName();
								assemblyName.SetPublicKey(this.MainAssembly.nGetPublicKey());
								byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
								int num = publicKeyToken.Length;
								StringBuilder stringBuilder = new StringBuilder(num * 2);
								for (int i = 0; i < num; i++)
								{
									stringBuilder.Append(publicKeyToken[i].ToString("x", CultureInfo.InvariantCulture));
								}
								text2 = text2 + ", PublicKeyToken=" + stringBuilder;
								string text3 = this._neutralResourcesCulture.Name;
								if (text3.Length == 0)
								{
									text3 = "<invariant>";
								}
								throw new MissingSatelliteAssemblyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingSatelliteAssembly_Culture_Name"), new object[]
								{
									this._neutralResourcesCulture,
									text2
								}), text3);
							}
							text = this.GetResourceFileName(this._neutralResourcesCulture);
						}
						else
						{
							assembly = this.MainAssembly;
						}
					}
					else if (!this.TryLookingForSatellite(cultureInfo))
					{
						assembly = null;
					}
					else
					{
						assembly = this.GetSatelliteAssembly(cultureInfo);
					}
					if (assembly != null)
					{
						resourceSet = (ResourceSet)resourceSets[cultureInfo];
						if (resourceSet != null)
						{
							return resourceSet;
						}
						bool skipSecurityCheck = this.MainAssembly == assembly && this._callingAssembly == this.MainAssembly;
						stream = assembly.GetManifestResourceStream(this._locationInfo, text, skipSecurityCheck, ref stackCrawlMark);
						if (stream == null)
						{
							stream = this.CaseInsensitiveManifestResourceStreamLookup(assembly, text);
						}
					}
				}
				else
				{
					assembly = this.MainAssembly;
					stream = this.MainAssembly.GetManifestResourceStream(this._locationInfo, text, this._callingAssembly == this.MainAssembly, ref stackCrawlMark);
				}
				if (stream == null && tryParents)
				{
					if (!culture.Equals(CultureInfo.InvariantCulture))
					{
						CultureInfo parent = culture.Parent;
						resourceSet = this.InternalGetResourceSet(parent, createIfNotExists, tryParents);
						if (resourceSet != null)
						{
							ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
						}
						return resourceSet;
					}
					if (this.MainAssembly == typeof(object).Assembly && this.BaseName.Equals("mscorlib"))
					{
						throw new ExecutionEngineException("mscorlib.resources couldn't be found!  Large parts of the BCL won't work!");
					}
					string text4 = string.Empty;
					if (this._locationInfo != null && this._locationInfo.Namespace != null)
					{
						text4 = this._locationInfo.Namespace + Type.Delimiter;
					}
					text4 += text;
					throw new MissingManifestResourceException(Environment.GetResourceString("MissingManifestResource_NoNeutralAsm", new object[]
					{
						text4,
						this.MainAssembly.nGetSimpleName()
					}));
				}
			}
			else
			{
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				string text = this.FindResourceFile(culture);
				if (text != null)
				{
					resourceSet = this.CreateResourceSet(text);
					if (resourceSet != null)
					{
						ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
					}
					return resourceSet;
				}
				if (tryParents)
				{
					if (culture.Equals(CultureInfo.InvariantCulture))
					{
						throw new MissingManifestResourceException(string.Concat(new string[]
						{
							Environment.GetResourceString("MissingManifestResource_NoNeutralDisk"),
							Environment.NewLine,
							"baseName: ",
							this.BaseNameField,
							"  locationInfo: ",
							(this._locationInfo == null) ? "<null>" : this._locationInfo.FullName,
							"  fileName: ",
							this.GetResourceFileName(culture)
						}));
					}
					CultureInfo parent2 = culture.Parent;
					resourceSet = this.InternalGetResourceSet(parent2, createIfNotExists, tryParents);
					if (resourceSet != null)
					{
						ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
					}
					return resourceSet;
				}
			}
			if (createIfNotExists && stream != null && resourceSet == null)
			{
				resourceSet = this.CreateResourceSet(stream, assembly);
				ResourceManager.AddResourceSet(resourceSets, culture, ref resourceSet);
			}
			return resourceSet;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000933FC File Offset: 0x000923FC
		private static void AddResourceSet(Hashtable localResourceSets, CultureInfo culture, ref ResourceSet rs)
		{
			lock (localResourceSets)
			{
				ResourceSet resourceSet = (ResourceSet)localResourceSets[culture];
				if (resourceSet != null)
				{
					if (!object.Equals(resourceSet, rs))
					{
						if (!localResourceSets.ContainsValue(rs))
						{
							rs.Dispose();
						}
						rs = resourceSet;
					}
				}
				else
				{
					localResourceSets.Add(culture, rs);
				}
			}
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00093464 File Offset: 0x00092464
		[MethodImpl(MethodImplOptions.NoInlining)]
		private Stream CaseInsensitiveManifestResourceStreamLookup(Assembly satellite, string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._locationInfo != null)
			{
				string @namespace = this._locationInfo.Namespace;
				if (@namespace != null)
				{
					stringBuilder.Append(@namespace);
					if (name != null)
					{
						stringBuilder.Append(Type.Delimiter);
					}
				}
			}
			stringBuilder.Append(name);
			string text = stringBuilder.ToString();
			CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
			string text2 = null;
			foreach (string text3 in satellite.GetManifestResourceNames())
			{
				if (compareInfo.Compare(text3, text, CompareOptions.IgnoreCase) == 0)
				{
					if (text2 != null)
					{
						throw new MissingManifestResourceException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingManifestResource_MultipleBlobs"), new object[]
						{
							text,
							satellite.ToString()
						}));
					}
					text2 = text3;
				}
			}
			if (text2 == null)
			{
				return null;
			}
			bool skipSecurityCheck = this.MainAssembly == satellite && this._callingAssembly == this.MainAssembly;
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return satellite.GetManifestResourceStream(text2, ref stackCrawlMark, skipSecurityCheck);
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x00093564 File Offset: 0x00092564
		protected static Version GetSatelliteContractVersion(Assembly a)
		{
			string text = null;
			foreach (CustomAttributeData customAttributeData in CustomAttributeData.GetCustomAttributes(a))
			{
				if (customAttributeData.Constructor.DeclaringType == typeof(SatelliteContractVersionAttribute))
				{
					text = (string)customAttributeData.ConstructorArguments[0].Value;
					break;
				}
			}
			if (text == null)
			{
				return null;
			}
			Version result;
			try
			{
				result = new Version(text);
			}
			catch (Exception innerException)
			{
				if (a == typeof(object).Assembly)
				{
					return null;
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_InvalidSatelliteContract_Asm_Ver"), new object[]
				{
					a.ToString(),
					text
				}), innerException);
			}
			return result;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x00093650 File Offset: 0x00092650
		protected static CultureInfo GetNeutralResourcesLanguage(Assembly a)
		{
			UltimateResourceFallbackLocation ultimateResourceFallbackLocation = UltimateResourceFallbackLocation.MainAssembly;
			return ResourceManager.GetNeutralResourcesLanguage(a, ref ultimateResourceFallbackLocation);
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x0009366C File Offset: 0x0009266C
		private static CultureInfo GetNeutralResourcesLanguage(Assembly a, ref UltimateResourceFallbackLocation fallbackLocation)
		{
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(a);
			CustomAttributeData customAttributeData = null;
			for (int i = 0; i < customAttributes.Count; i++)
			{
				if (customAttributes[i].Constructor.DeclaringType == typeof(NeutralResourcesLanguageAttribute))
				{
					customAttributeData = customAttributes[i];
					break;
				}
			}
			if (customAttributeData == null)
			{
				fallbackLocation = UltimateResourceFallbackLocation.MainAssembly;
				return CultureInfo.InvariantCulture;
			}
			string text = null;
			if (customAttributeData.Constructor.GetParameters().Length == 2)
			{
				fallbackLocation = (UltimateResourceFallbackLocation)customAttributeData.ConstructorArguments[1].Value;
				if (fallbackLocation < UltimateResourceFallbackLocation.MainAssembly || fallbackLocation > UltimateResourceFallbackLocation.Satellite)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidNeutralResourcesLanguage_FallbackLoc", new object[]
					{
						fallbackLocation
					}));
				}
			}
			else
			{
				fallbackLocation = UltimateResourceFallbackLocation.MainAssembly;
			}
			text = (customAttributeData.ConstructorArguments[0].Value as string);
			CultureInfo result;
			try
			{
				CultureInfo cultureInfo = CultureInfo.GetCultureInfo(text);
				result = cultureInfo;
			}
			catch (ArgumentException innerException)
			{
				if (a != typeof(object).Assembly)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_InvalidNeutralResourcesLanguage_Asm_Culture"), new object[]
					{
						a.ToString(),
						text
					}), innerException);
				}
				result = CultureInfo.InvariantCulture;
			}
			return result;
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000937B4 File Offset: 0x000927B4
		private Assembly GetSatelliteAssembly(CultureInfo lookForCulture)
		{
			if (!this._lookedForSatelliteContractVersion)
			{
				this._satelliteContractVersion = ResourceManager.GetSatelliteContractVersion(this.MainAssembly);
				this._lookedForSatelliteContractVersion = true;
			}
			Assembly result = null;
			try
			{
				result = this.MainAssembly.InternalGetSatelliteAssembly(lookForCulture, this._satelliteContractVersion, false);
			}
			catch (FileLoadException e)
			{
				int hrforException = Marshal.GetHRForException(e);
				Win32Native.MakeHRFromErrorCode(5);
			}
			catch (BadImageFormatException)
			{
			}
			return result;
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x0009382C File Offset: 0x0009282C
		private ResourceSet CreateResourceSet(string file)
		{
			if (this._userResourceSet == null)
			{
				return new RuntimeResourceSet(file);
			}
			object[] args = new object[]
			{
				file
			};
			ResourceSet result;
			try
			{
				result = (ResourceSet)Activator.CreateInstance(this._userResourceSet, args);
			}
			catch (MissingMethodException innerException)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_ResMgrBadResSet_Type"), new object[]
				{
					this._userResourceSet.AssemblyQualifiedName
				}), innerException);
			}
			return result;
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000938AC File Offset: 0x000928AC
		private ResourceSet CreateResourceSet(Stream store, Assembly assembly)
		{
			if (store.CanSeek && store.Length > 4L)
			{
				long position = store.Position;
				BinaryReader binaryReader = new BinaryReader(store);
				int num = binaryReader.ReadInt32();
				if (num == ResourceManager.MagicNumber)
				{
					int num2 = binaryReader.ReadInt32();
					string text;
					string text2;
					if (num2 == ResourceManager.HeaderVersionNumber)
					{
						binaryReader.ReadInt32();
						text = binaryReader.ReadString();
						text2 = binaryReader.ReadString();
					}
					else
					{
						if (num2 <= ResourceManager.HeaderVersionNumber)
						{
							throw new NotSupportedException(Environment.GetResourceString("NotSupported_ObsoleteResourcesFile", new object[]
							{
								this.MainAssembly.nGetSimpleName()
							}));
						}
						int num3 = binaryReader.ReadInt32();
						long offset = binaryReader.BaseStream.Position + (long)num3;
						text = binaryReader.ReadString();
						text2 = binaryReader.ReadString();
						binaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
					}
					store.Position = position;
					if (this.CanUseDefaultResourceClasses(text, text2))
					{
						return new RuntimeResourceSet(store);
					}
					Type type = Type.GetType(text, true);
					IResourceReader resourceReader = (IResourceReader)Activator.CreateInstance(type, new object[]
					{
						store
					});
					object[] args = new object[]
					{
						resourceReader
					};
					Type type2;
					if (this._userResourceSet == null)
					{
						type2 = Type.GetType(text2, true, false);
					}
					else
					{
						type2 = this._userResourceSet;
					}
					return (ResourceSet)Activator.CreateInstance(type2, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null, null);
				}
				else
				{
					store.Position = position;
				}
			}
			if (this._userResourceSet == null)
			{
				return new RuntimeResourceSet(store);
			}
			object[] args2 = new object[]
			{
				store,
				assembly
			};
			ResourceSet result;
			try
			{
				try
				{
					return (ResourceSet)Activator.CreateInstance(this._userResourceSet, args2);
				}
				catch (MissingMethodException)
				{
				}
				args2 = new object[]
				{
					store
				};
				ResourceSet resourceSet = (ResourceSet)Activator.CreateInstance(this._userResourceSet, args2);
				result = resourceSet;
			}
			catch (MissingMethodException innerException)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_ResMgrBadResSet_Type"), new object[]
				{
					this._userResourceSet.AssemblyQualifiedName
				}), innerException);
			}
			return result;
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x00093ADC File Offset: 0x00092ADC
		private bool CanUseDefaultResourceClasses(string readerTypeName, string resSetTypeName)
		{
			if (this._userResourceSet != null)
			{
				return false;
			}
			AssemblyName asmName = new AssemblyName(ResourceManager.MscorlibName);
			return (readerTypeName == null || ResourceManager.CompareNames(readerTypeName, ResourceManager.ResReaderTypeName, asmName)) && (resSetTypeName == null || ResourceManager.CompareNames(resSetTypeName, ResourceManager.ResSetTypeName, asmName));
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00093B28 File Offset: 0x00092B28
		internal static bool CompareNames(string asmTypeName1, string typeName2, AssemblyName asmName2)
		{
			int num = asmTypeName1.IndexOf(',');
			if (((num == -1) ? asmTypeName1.Length : num) != typeName2.Length)
			{
				return false;
			}
			if (string.Compare(asmTypeName1, 0, typeName2, 0, typeName2.Length, StringComparison.Ordinal) != 0)
			{
				return false;
			}
			if (num == -1)
			{
				return true;
			}
			while (char.IsWhiteSpace(asmTypeName1[++num]))
			{
			}
			AssemblyName assemblyName = new AssemblyName(asmTypeName1.Substring(num));
			if (string.Compare(assemblyName.Name, asmName2.Name, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return false;
			}
			if (assemblyName.CultureInfo != null && asmName2.CultureInfo != null && assemblyName.CultureInfo.LCID != asmName2.CultureInfo.LCID)
			{
				return false;
			}
			byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
			byte[] publicKeyToken2 = asmName2.GetPublicKeyToken();
			if (publicKeyToken != null && publicKeyToken2 != null)
			{
				if (publicKeyToken.Length != publicKeyToken2.Length)
				{
					return false;
				}
				for (int i = 0; i < publicKeyToken.Length; i++)
				{
					if (publicKeyToken[i] != publicKeyToken2[i])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x00093C0B File Offset: 0x00092C0B
		public virtual string GetString(string name)
		{
			return this.GetString(name, null);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00093C18 File Offset: 0x00092C18
		public virtual string GetString(string name, CultureInfo culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			ResourceSet resourceSet = this.InternalGetResourceSet(culture, true, true);
			if (resourceSet != null)
			{
				string @string = resourceSet.GetString(name, this._ignoreCase);
				if (@string != null)
				{
					return @string;
				}
			}
			ResourceSet resourceSet2 = null;
			while (!culture.Equals(CultureInfo.InvariantCulture) && !culture.Equals(this._neutralResourcesCulture))
			{
				culture = culture.Parent;
				resourceSet = this.InternalGetResourceSet(culture, true, true);
				if (resourceSet == null)
				{
					break;
				}
				if (resourceSet != resourceSet2)
				{
					string string2 = resourceSet.GetString(name, this._ignoreCase);
					if (string2 != null)
					{
						return string2;
					}
					resourceSet2 = resourceSet;
				}
			}
			return null;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x00093CAB File Offset: 0x00092CAB
		public virtual object GetObject(string name)
		{
			return this.GetObject(name, null, true);
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x00093CB6 File Offset: 0x00092CB6
		public virtual object GetObject(string name, CultureInfo culture)
		{
			return this.GetObject(name, culture, true);
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x00093CC4 File Offset: 0x00092CC4
		private object GetObject(string name, CultureInfo culture, bool wrapUnmanagedMemStream)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			ResourceSet resourceSet = this.InternalGetResourceSet(culture, true, true);
			if (resourceSet != null)
			{
				object @object = resourceSet.GetObject(name, this._ignoreCase);
				if (@object != null)
				{
					UnmanagedMemoryStream unmanagedMemoryStream = @object as UnmanagedMemoryStream;
					if (unmanagedMemoryStream != null && wrapUnmanagedMemStream)
					{
						return new UnmanagedMemoryStreamWrapper(unmanagedMemoryStream);
					}
					return @object;
				}
			}
			ResourceSet resourceSet2 = null;
			while (!culture.Equals(CultureInfo.InvariantCulture) && !culture.Equals(this._neutralResourcesCulture))
			{
				culture = culture.Parent;
				resourceSet = this.InternalGetResourceSet(culture, true, true);
				if (resourceSet == null)
				{
					break;
				}
				if (resourceSet != resourceSet2)
				{
					object object2 = resourceSet.GetObject(name, this._ignoreCase);
					if (object2 != null)
					{
						UnmanagedMemoryStream unmanagedMemoryStream2 = object2 as UnmanagedMemoryStream;
						if (unmanagedMemoryStream2 != null && wrapUnmanagedMemStream)
						{
							return new UnmanagedMemoryStreamWrapper(unmanagedMemoryStream2);
						}
						return object2;
					}
					else
					{
						resourceSet2 = resourceSet;
					}
				}
			}
			return null;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x00093D86 File Offset: 0x00092D86
		[CLSCompliant(false)]
		[ComVisible(false)]
		public UnmanagedMemoryStream GetStream(string name)
		{
			return this.GetStream(name, null);
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x00093D90 File Offset: 0x00092D90
		[ComVisible(false)]
		[CLSCompliant(false)]
		public UnmanagedMemoryStream GetStream(string name, CultureInfo culture)
		{
			object @object = this.GetObject(name, culture, false);
			UnmanagedMemoryStream unmanagedMemoryStream = @object as UnmanagedMemoryStream;
			if (unmanagedMemoryStream == null && @object != null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotStream_Name", new object[]
				{
					name
				}));
			}
			return unmanagedMemoryStream;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x00093DD4 File Offset: 0x00092DD4
		private bool TryLookingForSatellite(CultureInfo lookForCulture)
		{
			if (!ResourceManager._checkedConfigFile)
			{
				lock (this)
				{
					if (!ResourceManager._checkedConfigFile)
					{
						ResourceManager._checkedConfigFile = true;
						ResourceManager._installedSatelliteInfo = this.GetSatelliteAssembliesFromConfig();
					}
				}
			}
			if (ResourceManager._installedSatelliteInfo == null)
			{
				return true;
			}
			CultureInfo[] array = (CultureInfo[])ResourceManager._installedSatelliteInfo[this.MainAssembly.FullName];
			if (array == null)
			{
				return true;
			}
			int num = Array.IndexOf<CultureInfo>(array, lookForCulture);
			return num >= 0;
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x00093E5C File Offset: 0x00092E5C
		private Hashtable GetSatelliteAssembliesFromConfig()
		{
			string configurationFileInternal = AppDomain.CurrentDomain.FusionStore.ConfigurationFileInternal;
			if (configurationFileInternal == null)
			{
				return null;
			}
			if (configurationFileInternal.Length >= 2 && (configurationFileInternal[1] == Path.VolumeSeparatorChar || (configurationFileInternal[0] == Path.DirectorySeparatorChar && configurationFileInternal[1] == Path.DirectorySeparatorChar)) && !File.InternalExists(configurationFileInternal))
			{
				return null;
			}
			ConfigTreeParser configTreeParser = new ConfigTreeParser();
			string configPath = "/configuration/satelliteassemblies";
			ConfigNode configNode = null;
			try
			{
				configNode = configTreeParser.Parse(configurationFileInternal, configPath, true);
			}
			catch (Exception)
			{
			}
			if (configNode == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in configNode.Children)
			{
				ConfigNode configNode2 = (ConfigNode)obj;
				if (!string.Equals(configNode2.Name, "assembly"))
				{
					throw new ApplicationException(Environment.GetResourceString("XMLSyntax_InvalidSyntaxSatAssemTag", new object[]
					{
						Path.GetFileName(configurationFileInternal),
						configNode2.Name
					}));
				}
				if (configNode2.Attributes.Count != 1)
				{
					throw new ApplicationException(Environment.GetResourceString("XMLSyntax_InvalidSyntaxSatAssemTagBadAttr", new object[]
					{
						Path.GetFileName(configurationFileInternal)
					}));
				}
				DictionaryEntry dictionaryEntry = (DictionaryEntry)configNode2.Attributes[0];
				string text = (string)dictionaryEntry.Value;
				if (!object.Equals(dictionaryEntry.Key, "name") || text == null || text.Length == 0)
				{
					throw new ApplicationException(Environment.GetResourceString("XMLSyntax_InvalidSyntaxSatAssemTagBadAttr", new object[]
					{
						Path.GetFileName(configurationFileInternal),
						dictionaryEntry.Key,
						dictionaryEntry.Value
					}));
				}
				ArrayList arrayList = new ArrayList(5);
				foreach (object obj2 in configNode2.Children)
				{
					ConfigNode configNode3 = (ConfigNode)obj2;
					if (configNode3.Value != null)
					{
						arrayList.Add(configNode3.Value);
					}
				}
				CultureInfo[] array = new CultureInfo[arrayList.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CultureInfo.GetCultureInfo((string)arrayList[i]);
				}
				hashtable.Add(text, array);
			}
			return hashtable;
		}

		// Token: 0x0400155D RID: 5469
		internal const string ResFileExtension = ".resources";

		// Token: 0x0400155E RID: 5470
		internal const int ResFileExtensionLength = 10;

		// Token: 0x0400155F RID: 5471
		protected string BaseNameField;

		// Token: 0x04001560 RID: 5472
		protected Hashtable ResourceSets;

		// Token: 0x04001561 RID: 5473
		private string moduleDir;

		// Token: 0x04001562 RID: 5474
		protected Assembly MainAssembly;

		// Token: 0x04001563 RID: 5475
		private Type _locationInfo;

		// Token: 0x04001564 RID: 5476
		private Type _userResourceSet;

		// Token: 0x04001565 RID: 5477
		private CultureInfo _neutralResourcesCulture;

		// Token: 0x04001566 RID: 5478
		private bool _ignoreCase;

		// Token: 0x04001567 RID: 5479
		private bool UseManifest;

		// Token: 0x04001568 RID: 5480
		private bool UseSatelliteAssem;

		// Token: 0x04001569 RID: 5481
		private static Hashtable _installedSatelliteInfo;

		// Token: 0x0400156A RID: 5482
		private static bool _checkedConfigFile;

		// Token: 0x0400156B RID: 5483
		[OptionalField]
		private UltimateResourceFallbackLocation _fallbackLoc;

		// Token: 0x0400156C RID: 5484
		[OptionalField]
		private Version _satelliteContractVersion;

		// Token: 0x0400156D RID: 5485
		[OptionalField]
		private bool _lookedForSatelliteContractVersion;

		// Token: 0x0400156E RID: 5486
		private Assembly _callingAssembly;

		// Token: 0x0400156F RID: 5487
		public static readonly int MagicNumber = -1091581234;

		// Token: 0x04001570 RID: 5488
		public static readonly int HeaderVersionNumber = 1;

		// Token: 0x04001571 RID: 5489
		private static readonly Type _minResourceSet = typeof(ResourceSet);

		// Token: 0x04001572 RID: 5490
		internal static readonly string ResReaderTypeName = typeof(ResourceReader).FullName;

		// Token: 0x04001573 RID: 5491
		internal static readonly string ResSetTypeName = typeof(RuntimeResourceSet).FullName;

		// Token: 0x04001574 RID: 5492
		internal static readonly string MscorlibName = typeof(ResourceReader).Assembly.FullName;

		// Token: 0x04001575 RID: 5493
		internal static readonly int DEBUG = 0;
	}
}
