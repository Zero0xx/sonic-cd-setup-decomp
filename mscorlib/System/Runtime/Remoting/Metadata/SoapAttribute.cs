using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000747 RID: 1863
	[ComVisible(true)]
	public class SoapAttribute : Attribute
	{
		// Token: 0x06004286 RID: 17030 RVA: 0x000E26A3 File Offset: 0x000E16A3
		internal void SetReflectInfo(object info)
		{
			this.ReflectInfo = info;
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06004287 RID: 17031 RVA: 0x000E26AC File Offset: 0x000E16AC
		// (set) Token: 0x06004288 RID: 17032 RVA: 0x000E26B4 File Offset: 0x000E16B4
		public virtual string XmlNamespace
		{
			get
			{
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06004289 RID: 17033 RVA: 0x000E26BD File Offset: 0x000E16BD
		// (set) Token: 0x0600428A RID: 17034 RVA: 0x000E26C5 File Offset: 0x000E16C5
		public virtual bool UseAttribute
		{
			get
			{
				return this._bUseAttribute;
			}
			set
			{
				this._bUseAttribute = value;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600428B RID: 17035 RVA: 0x000E26CE File Offset: 0x000E16CE
		// (set) Token: 0x0600428C RID: 17036 RVA: 0x000E26D6 File Offset: 0x000E16D6
		public virtual bool Embedded
		{
			get
			{
				return this._bEmbedded;
			}
			set
			{
				this._bEmbedded = value;
			}
		}

		// Token: 0x0400216C RID: 8556
		protected string ProtXmlNamespace;

		// Token: 0x0400216D RID: 8557
		private bool _bUseAttribute;

		// Token: 0x0400216E RID: 8558
		private bool _bEmbedded;

		// Token: 0x0400216F RID: 8559
		protected object ReflectInfo;
	}
}
