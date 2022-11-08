using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Security.Util;
using System.Text;

namespace System.Security.Permissions
{
	// Token: 0x0200064B RID: 1611
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class PermissionSetAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003A06 RID: 14854 RVA: 0x000C2B76 File Offset: 0x000C1B76
		public PermissionSetAttribute(SecurityAction action) : base(action)
		{
			this.m_unicode = false;
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x000C2B86 File Offset: 0x000C1B86
		// (set) Token: 0x06003A08 RID: 14856 RVA: 0x000C2B8E File Offset: 0x000C1B8E
		public string File
		{
			get
			{
				return this.m_file;
			}
			set
			{
				this.m_file = value;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06003A09 RID: 14857 RVA: 0x000C2B97 File Offset: 0x000C1B97
		// (set) Token: 0x06003A0A RID: 14858 RVA: 0x000C2B9F File Offset: 0x000C1B9F
		public bool UnicodeEncoded
		{
			get
			{
				return this.m_unicode;
			}
			set
			{
				this.m_unicode = value;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06003A0B RID: 14859 RVA: 0x000C2BA8 File Offset: 0x000C1BA8
		// (set) Token: 0x06003A0C RID: 14860 RVA: 0x000C2BB0 File Offset: 0x000C1BB0
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x000C2BB9 File Offset: 0x000C1BB9
		// (set) Token: 0x06003A0E RID: 14862 RVA: 0x000C2BC1 File Offset: 0x000C1BC1
		public string XML
		{
			get
			{
				return this.m_xml;
			}
			set
			{
				this.m_xml = value;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06003A0F RID: 14863 RVA: 0x000C2BCA File Offset: 0x000C1BCA
		// (set) Token: 0x06003A10 RID: 14864 RVA: 0x000C2BD2 File Offset: 0x000C1BD2
		public string Hex
		{
			get
			{
				return this.m_hex;
			}
			set
			{
				this.m_hex = value;
			}
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x000C2BDB File Offset: 0x000C1BDB
		public override IPermission CreatePermission()
		{
			return null;
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x000C2BE0 File Offset: 0x000C1BE0
		private PermissionSet BruteForceParseStream(Stream stream)
		{
			Encoding[] array = new Encoding[]
			{
				Encoding.UTF8,
				Encoding.ASCII,
				Encoding.Unicode
			};
			StreamReader streamReader = null;
			Exception ex = null;
			int num = 0;
			while (streamReader == null && num < array.Length)
			{
				try
				{
					stream.Position = 0L;
					streamReader = new StreamReader(stream, array[num]);
					return this.ParsePermissionSet(new Parser(streamReader));
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
				num++;
			}
			throw ex;
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x000C2C68 File Offset: 0x000C1C68
		private PermissionSet ParsePermissionSet(Parser parser)
		{
			SecurityElement topElement = parser.GetTopElement();
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.FromXml(topElement);
			return permissionSet;
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x000C2C8C File Offset: 0x000C1C8C
		public PermissionSet CreatePermissionSet()
		{
			if (this.m_unrestricted)
			{
				return new PermissionSet(PermissionState.Unrestricted);
			}
			if (this.m_name != null)
			{
				return PolicyLevel.GetBuiltInSet(this.m_name);
			}
			if (this.m_xml != null)
			{
				return this.ParsePermissionSet(new Parser(this.m_xml.ToCharArray()));
			}
			if (this.m_hex != null)
			{
				return this.BruteForceParseStream(new MemoryStream(System.Security.Util.Hex.DecodeHexString(this.m_hex)));
			}
			if (this.m_file != null)
			{
				return this.BruteForceParseStream(new FileStream(this.m_file, FileMode.Open, FileAccess.Read));
			}
			return new PermissionSet(PermissionState.None);
		}

		// Token: 0x04001E20 RID: 7712
		private string m_file;

		// Token: 0x04001E21 RID: 7713
		private string m_name;

		// Token: 0x04001E22 RID: 7714
		private bool m_unicode;

		// Token: 0x04001E23 RID: 7715
		private string m_xml;

		// Token: 0x04001E24 RID: 7716
		private string m_hex;
	}
}
