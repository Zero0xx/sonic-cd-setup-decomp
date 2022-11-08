using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200013B RID: 315
	[ComVisible(true)]
	[Serializable]
	public sealed class Version : ICloneable, IComparable, IComparable<Version>, IEquatable<Version>
	{
		// Token: 0x0600116F RID: 4463 RVA: 0x00030B34 File Offset: 0x0002FB34
		public Version(int major, int minor, int build, int revision)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (revision < 0)
			{
				throw new ArgumentOutOfRangeException("revision", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
			this._Revision = revision;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00030BD8 File Offset: 0x0002FBD8
		public Version(int major, int minor, int build)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00030C5C File Offset: 0x0002FC5C
		public Version(int major, int minor)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00030CC0 File Offset: 0x0002FCC0
		public Version(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			string[] array = version.Split(new char[]
			{
				'.'
			});
			int num = array.Length;
			if (num < 2 || num > 4)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_VersionString"));
			}
			this._Major = int.Parse(array[0], CultureInfo.InvariantCulture);
			if (this._Major < 0)
			{
				throw new ArgumentOutOfRangeException("version", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Minor = int.Parse(array[1], CultureInfo.InvariantCulture);
			if (this._Minor < 0)
			{
				throw new ArgumentOutOfRangeException("version", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			num -= 2;
			if (num > 0)
			{
				this._Build = int.Parse(array[2], CultureInfo.InvariantCulture);
				if (this._Build < 0)
				{
					throw new ArgumentOutOfRangeException("build", Environment.GetResourceString("ArgumentOutOfRange_Version"));
				}
				num--;
				if (num > 0)
				{
					this._Revision = int.Parse(array[3], CultureInfo.InvariantCulture);
					if (this._Revision < 0)
					{
						throw new ArgumentOutOfRangeException("revision", Environment.GetResourceString("ArgumentOutOfRange_Version"));
					}
				}
			}
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00030DF3 File Offset: 0x0002FDF3
		public Version()
		{
			this._Major = 0;
			this._Minor = 0;
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00030E17 File Offset: 0x0002FE17
		public int Major
		{
			get
			{
				return this._Major;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x00030E1F File Offset: 0x0002FE1F
		public int Minor
		{
			get
			{
				return this._Minor;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x00030E27 File Offset: 0x0002FE27
		public int Build
		{
			get
			{
				return this._Build;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x00030E2F File Offset: 0x0002FE2F
		public int Revision
		{
			get
			{
				return this._Revision;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x00030E37 File Offset: 0x0002FE37
		public short MajorRevision
		{
			get
			{
				return (short)(this._Revision >> 16);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x00030E43 File Offset: 0x0002FE43
		public short MinorRevision
		{
			get
			{
				return (short)(this._Revision & 65535);
			}
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00030E54 File Offset: 0x0002FE54
		public object Clone()
		{
			return new Version
			{
				_Major = this._Major,
				_Minor = this._Minor,
				_Build = this._Build,
				_Revision = this._Revision
			};
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00030E98 File Offset: 0x0002FE98
		public int CompareTo(object version)
		{
			if (version == null)
			{
				return 1;
			}
			Version version2 = version as Version;
			if (version2 == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeVersion"));
			}
			if (this._Major != version2._Major)
			{
				if (this._Major > version2._Major)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Minor != version2._Minor)
			{
				if (this._Minor > version2._Minor)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Build != version2._Build)
			{
				if (this._Build > version2._Build)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (this._Revision == version2._Revision)
				{
					return 0;
				}
				if (this._Revision > version2._Revision)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00030F4C File Offset: 0x0002FF4C
		public int CompareTo(Version value)
		{
			if (value == null)
			{
				return 1;
			}
			if (this._Major != value._Major)
			{
				if (this._Major > value._Major)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Minor != value._Minor)
			{
				if (this._Minor > value._Minor)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Build != value._Build)
			{
				if (this._Build > value._Build)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (this._Revision == value._Revision)
				{
					return 0;
				}
				if (this._Revision > value._Revision)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00030FE8 File Offset: 0x0002FFE8
		public override bool Equals(object obj)
		{
			Version version = obj as Version;
			return !(version == null) && this._Major == version._Major && this._Minor == version._Minor && this._Build == version._Build && this._Revision == version._Revision;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00031044 File Offset: 0x00030044
		public bool Equals(Version obj)
		{
			return !(obj == null) && this._Major == obj._Major && this._Minor == obj._Minor && this._Build == obj._Build && this._Revision == obj._Revision;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00031098 File Offset: 0x00030098
		public override int GetHashCode()
		{
			int num = 0;
			num |= (this._Major & 15) << 28;
			num |= (this._Minor & 255) << 20;
			num |= (this._Build & 255) << 12;
			return num | (this._Revision & 4095);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000310EA File Offset: 0x000300EA
		public override string ToString()
		{
			if (this._Build == -1)
			{
				return this.ToString(2);
			}
			if (this._Revision == -1)
			{
				return this.ToString(3);
			}
			return this.ToString(4);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00031118 File Offset: 0x00030118
		public string ToString(int fieldCount)
		{
			switch (fieldCount)
			{
			case 0:
				return string.Empty;
			case 1:
				return string.Concat(this._Major);
			case 2:
				return this._Major + "." + this._Minor;
			default:
				if (this._Build == -1)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[]
					{
						"0",
						"2"
					}), "fieldCount");
				}
				if (fieldCount == 3)
				{
					return string.Concat(new object[]
					{
						this._Major,
						".",
						this._Minor,
						".",
						this._Build
					});
				}
				if (this._Revision == -1)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[]
					{
						"0",
						"3"
					}), "fieldCount");
				}
				if (fieldCount == 4)
				{
					return string.Concat(new object[]
					{
						this.Major,
						".",
						this._Minor,
						".",
						this._Build,
						".",
						this._Revision
					});
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[]
				{
					"0",
					"4"
				}), "fieldCount");
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000312E4 File Offset: 0x000302E4
		public static bool operator ==(Version v1, Version v2)
		{
			if (object.ReferenceEquals(v1, null))
			{
				return object.ReferenceEquals(v2, null);
			}
			return v1.Equals(v2);
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000312FE File Offset: 0x000302FE
		public static bool operator !=(Version v1, Version v2)
		{
			return !(v1 == v2);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0003130A File Offset: 0x0003030A
		public static bool operator <(Version v1, Version v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return v1.CompareTo(v2) < 0;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00031324 File Offset: 0x00030324
		public static bool operator <=(Version v1, Version v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return v1.CompareTo(v2) <= 0;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00031341 File Offset: 0x00030341
		public static bool operator >(Version v1, Version v2)
		{
			return v2 < v1;
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0003134A File Offset: 0x0003034A
		public static bool operator >=(Version v1, Version v2)
		{
			return v2 <= v1;
		}

		// Token: 0x0400060C RID: 1548
		private int _Major;

		// Token: 0x0400060D RID: 1549
		private int _Minor;

		// Token: 0x0400060E RID: 1550
		private int _Build = -1;

		// Token: 0x0400060F RID: 1551
		private int _Revision = -1;
	}
}
