using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200078B RID: 1931
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapQName : ISoapXsd
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060044C7 RID: 17607 RVA: 0x000EB156 File Offset: 0x000EA156
		public static string XsdType
		{
			get
			{
				return "QName";
			}
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x000EB15D File Offset: 0x000EA15D
		public string GetXsdType()
		{
			return SoapQName.XsdType;
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x000EB164 File Offset: 0x000EA164
		public SoapQName()
		{
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x000EB16C File Offset: 0x000EA16C
		public SoapQName(string value)
		{
			this._name = value;
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x000EB17B File Offset: 0x000EA17B
		public SoapQName(string key, string name)
		{
			this._name = name;
			this._key = key;
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x000EB191 File Offset: 0x000EA191
		public SoapQName(string key, string name, string namespaceValue)
		{
			this._name = name;
			this._namespace = namespaceValue;
			this._key = key;
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060044CD RID: 17613 RVA: 0x000EB1AE File Offset: 0x000EA1AE
		// (set) Token: 0x060044CE RID: 17614 RVA: 0x000EB1B6 File Offset: 0x000EA1B6
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x060044CF RID: 17615 RVA: 0x000EB1BF File Offset: 0x000EA1BF
		// (set) Token: 0x060044D0 RID: 17616 RVA: 0x000EB1C7 File Offset: 0x000EA1C7
		public string Namespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x060044D1 RID: 17617 RVA: 0x000EB1D0 File Offset: 0x000EA1D0
		// (set) Token: 0x060044D2 RID: 17618 RVA: 0x000EB1D8 File Offset: 0x000EA1D8
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x000EB1E1 File Offset: 0x000EA1E1
		public override string ToString()
		{
			if (this._key == null || this._key.Length == 0)
			{
				return this._name;
			}
			return this._key + ":" + this._name;
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x000EB218 File Offset: 0x000EA218
		public static SoapQName Parse(string value)
		{
			if (value == null)
			{
				return new SoapQName();
			}
			string key = "";
			string name = value;
			int num = value.IndexOf(':');
			if (num > 0)
			{
				key = value.Substring(0, num);
				name = value.Substring(num + 1);
			}
			return new SoapQName(key, name);
		}

		// Token: 0x0400225B RID: 8795
		private string _name;

		// Token: 0x0400225C RID: 8796
		private string _namespace;

		// Token: 0x0400225D RID: 8797
		private string _key;
	}
}
