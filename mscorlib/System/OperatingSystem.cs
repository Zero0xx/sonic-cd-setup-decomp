using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000E7 RID: 231
	[ComVisible(true)]
	[Serializable]
	public sealed class OperatingSystem : ICloneable, ISerializable
	{
		// Token: 0x06000C6B RID: 3179 RVA: 0x000254A4 File Offset: 0x000244A4
		private OperatingSystem()
		{
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x000254AC File Offset: 0x000244AC
		public OperatingSystem(PlatformID platform, Version version) : this(platform, version, null)
		{
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x000254B8 File Offset: 0x000244B8
		internal OperatingSystem(PlatformID platform, Version version, string servicePack)
		{
			if (platform < PlatformID.Win32S || platform > PlatformID.Unix)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[]
				{
					(int)platform
				}), "platform");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._platform = platform;
			this._version = (Version)version.Clone();
			this._servicePack = servicePack;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00025530 File Offset: 0x00024530
		private OperatingSystem(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null)
				{
					if (!(name == "_version"))
					{
						if (!(name == "_platform"))
						{
							if (name == "_servicePack")
							{
								this._servicePack = info.GetString("_servicePack");
							}
						}
						else
						{
							this._platform = (PlatformID)info.GetValue("_platform", typeof(PlatformID));
						}
					}
					else
					{
						this._version = (Version)info.GetValue("_version", typeof(Version));
					}
				}
			}
			if (this._version == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_MissField", new object[]
				{
					"_version"
				}));
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00025610 File Offset: 0x00024610
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("_version", this._version);
			info.AddValue("_platform", this._platform);
			info.AddValue("_servicePack", this._servicePack);
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00025663 File Offset: 0x00024663
		public PlatformID Platform
		{
			get
			{
				return this._platform;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x0002566B File Offset: 0x0002466B
		public string ServicePack
		{
			get
			{
				if (this._servicePack == null)
				{
					return string.Empty;
				}
				return this._servicePack;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00025681 File Offset: 0x00024681
		public Version Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00025689 File Offset: 0x00024689
		public object Clone()
		{
			return new OperatingSystem(this._platform, this._version, this._servicePack);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000256A2 File Offset: 0x000246A2
		public override string ToString()
		{
			return this.VersionString;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x000256AC File Offset: 0x000246AC
		public string VersionString
		{
			get
			{
				if (this._versionString != null)
				{
					return this._versionString;
				}
				string str;
				if (this._platform == PlatformID.Win32NT)
				{
					str = "Microsoft Windows NT ";
				}
				else if (this._platform == PlatformID.Win32Windows)
				{
					if (this._version.Major > 4 || (this._version.Major == 4 && this._version.Minor > 0))
					{
						str = "Microsoft Windows 98 ";
					}
					else
					{
						str = "Microsoft Windows 95 ";
					}
				}
				else if (this._platform == PlatformID.Win32S)
				{
					str = "Microsoft Win32S ";
				}
				else if (this._platform == PlatformID.WinCE)
				{
					str = "Microsoft Windows CE ";
				}
				else
				{
					str = "<unknown> ";
				}
				if (string.IsNullOrEmpty(this._servicePack))
				{
					this._versionString = str + this._version.ToString();
				}
				else
				{
					this._versionString = str + this._version.ToString(3) + " " + this._servicePack;
				}
				return this._versionString;
			}
		}

		// Token: 0x0400046F RID: 1135
		private Version _version;

		// Token: 0x04000470 RID: 1136
		private PlatformID _platform;

		// Token: 0x04000471 RID: 1137
		private string _servicePack;

		// Token: 0x04000472 RID: 1138
		private string _versionString;
	}
}
