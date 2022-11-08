using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000793 RID: 1939
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapNmtoken : ISoapXsd
	{
		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x0600450F RID: 17679 RVA: 0x000EB5FC File Offset: 0x000EA5FC
		public static string XsdType
		{
			get
			{
				return "NMTOKEN";
			}
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x000EB603 File Offset: 0x000EA603
		public string GetXsdType()
		{
			return SoapNmtoken.XsdType;
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x000EB60A File Offset: 0x000EA60A
		public SoapNmtoken()
		{
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x000EB612 File Offset: 0x000EA612
		public SoapNmtoken(string value)
		{
			this._value = value;
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06004513 RID: 17683 RVA: 0x000EB621 File Offset: 0x000EA621
		// (set) Token: 0x06004514 RID: 17684 RVA: 0x000EB629 File Offset: 0x000EA629
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

		// Token: 0x06004515 RID: 17685 RVA: 0x000EB632 File Offset: 0x000EA632
		public override string ToString()
		{
			return SoapType.Escape(this._value);
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x000EB63F File Offset: 0x000EA63F
		public static SoapNmtoken Parse(string value)
		{
			return new SoapNmtoken(value);
		}

		// Token: 0x04002265 RID: 8805
		private string _value;
	}
}
