using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000781 RID: 1921
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapDay : ISoapXsd
	{
		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x06004473 RID: 17523 RVA: 0x000EA88E File Offset: 0x000E988E
		public static string XsdType
		{
			get
			{
				return "gDay";
			}
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x000EA895 File Offset: 0x000E9895
		public string GetXsdType()
		{
			return SoapDay.XsdType;
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x000EA89C File Offset: 0x000E989C
		public SoapDay()
		{
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x000EA8AF File Offset: 0x000E98AF
		public SoapDay(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004477 RID: 17527 RVA: 0x000EA8C9 File Offset: 0x000E98C9
		// (set) Token: 0x06004478 RID: 17528 RVA: 0x000EA8D1 File Offset: 0x000E98D1
		public DateTime Value
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

		// Token: 0x06004479 RID: 17529 RVA: 0x000EA8DA File Offset: 0x000E98DA
		public override string ToString()
		{
			return this._value.ToString("---dd", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x000EA8F1 File Offset: 0x000E98F1
		public static SoapDay Parse(string value)
		{
			return new SoapDay(DateTime.ParseExact(value, SoapDay.formats, CultureInfo.InvariantCulture, DateTimeStyles.None));
		}

		// Token: 0x0400224E RID: 8782
		private DateTime _value = DateTime.MinValue;

		// Token: 0x0400224F RID: 8783
		private static string[] formats = new string[]
		{
			"---dd",
			"---ddzzz"
		};
	}
}
