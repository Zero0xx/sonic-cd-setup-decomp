using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x02000676 RID: 1654
	[ComVisible(true)]
	[Serializable]
	public sealed class NamedPermissionSet : PermissionSet
	{
		// Token: 0x06003BCB RID: 15307 RVA: 0x000CC1BE File Offset: 0x000CB1BE
		internal NamedPermissionSet()
		{
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x000CC1C6 File Offset: 0x000CB1C6
		public NamedPermissionSet(string name)
		{
			NamedPermissionSet.CheckName(name);
			this.m_name = name;
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x000CC1DB File Offset: 0x000CB1DB
		public NamedPermissionSet(string name, PermissionState state) : base(state)
		{
			NamedPermissionSet.CheckName(name);
			this.m_name = name;
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x000CC1F1 File Offset: 0x000CB1F1
		public NamedPermissionSet(string name, PermissionSet permSet) : base(permSet)
		{
			NamedPermissionSet.CheckName(name);
			this.m_name = name;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x000CC207 File Offset: 0x000CB207
		public NamedPermissionSet(NamedPermissionSet permSet) : base(permSet)
		{
			this.m_name = permSet.m_name;
			this.m_description = permSet.Description;
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06003BD0 RID: 15312 RVA: 0x000CC228 File Offset: 0x000CB228
		// (set) Token: 0x06003BD1 RID: 15313 RVA: 0x000CC230 File Offset: 0x000CB230
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				NamedPermissionSet.CheckName(value);
				this.m_name = value;
			}
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x000CC23F File Offset: 0x000CB23F
		private static void CheckName(string name)
		{
			if (name == null || name.Equals(""))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NPMSInvalidName"));
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x000CC261 File Offset: 0x000CB261
		// (set) Token: 0x06003BD4 RID: 15316 RVA: 0x000CC289 File Offset: 0x000CB289
		public string Description
		{
			get
			{
				if (this.m_descrResource != null)
				{
					this.m_description = Environment.GetResourceString(this.m_descrResource);
					this.m_descrResource = null;
				}
				return this.m_description;
			}
			set
			{
				this.m_description = value;
				this.m_descrResource = null;
			}
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x000CC299 File Offset: 0x000CB299
		public override PermissionSet Copy()
		{
			return new NamedPermissionSet(this);
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x000CC2A4 File Offset: 0x000CB2A4
		public NamedPermissionSet Copy(string name)
		{
			return new NamedPermissionSet(this)
			{
				Name = name
			};
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x000CC2C0 File Offset: 0x000CB2C0
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.ToXml("System.Security.NamedPermissionSet");
			if (this.m_name != null && !this.m_name.Equals(""))
			{
				securityElement.AddAttribute("Name", SecurityElement.Escape(this.m_name));
			}
			if (this.Description != null && !this.Description.Equals(""))
			{
				securityElement.AddAttribute("Description", SecurityElement.Escape(this.Description));
			}
			return securityElement;
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x000CC33A File Offset: 0x000CB33A
		public override void FromXml(SecurityElement et)
		{
			this.FromXml(et, false, false);
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x000CC348 File Offset: 0x000CB348
		internal override void FromXml(SecurityElement et, bool allowInternalOnly, bool ignoreTypeLoadFailures)
		{
			if (et == null)
			{
				throw new ArgumentNullException("et");
			}
			string text = et.Attribute("Name");
			this.m_name = ((text == null) ? null : text);
			text = et.Attribute("Description");
			this.m_description = ((text == null) ? "" : text);
			this.m_descrResource = null;
			base.FromXml(et, allowInternalOnly, ignoreTypeLoadFailures);
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x000CC3AC File Offset: 0x000CB3AC
		internal void FromXmlNameOnly(SecurityElement et)
		{
			string text = et.Attribute("Name");
			this.m_name = ((text == null) ? null : text);
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x000CC3D2 File Offset: 0x000CB3D2
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x000CC3DB File Offset: 0x000CB3DB
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04001EDA RID: 7898
		private string m_name;

		// Token: 0x04001EDB RID: 7899
		private string m_description;

		// Token: 0x04001EDC RID: 7900
		[OptionalField(VersionAdded = 2)]
		internal string m_descrResource;
	}
}
