using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000792 RID: 1938
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapEntities : ISoapXsd
	{
		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06004507 RID: 17671 RVA: 0x000EB5B1 File Offset: 0x000EA5B1
		public static string XsdType
		{
			get
			{
				return "ENTITIES";
			}
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x000EB5B8 File Offset: 0x000EA5B8
		public string GetXsdType()
		{
			return SoapEntities.XsdType;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x000EB5BF File Offset: 0x000EA5BF
		public SoapEntities()
		{
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x000EB5C7 File Offset: 0x000EA5C7
		public SoapEntities(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x0600450B RID: 17675 RVA: 0x000EB5D6 File Offset: 0x000EA5D6
		// (set) Token: 0x0600450C RID: 17676 RVA: 0x000EB5DE File Offset: 0x000EA5DE
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x000EB5E7 File Offset: 0x000EA5E7
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x000EB5F4 File Offset: 0x000EA5F4
		public static SoapEntities Parse(string value)
		{
			return new SoapEntities(value);
		}

		// Token: 0x04002264 RID: 8804
		private string _value;
	}
}
