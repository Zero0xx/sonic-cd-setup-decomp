using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security
{
	// Token: 0x02000621 RID: 1569
	[ComVisible(true)]
	[Serializable]
	public sealed class XmlSyntaxException : SystemException
	{
		// Token: 0x06003893 RID: 14483 RVA: 0x000BEE02 File Offset: 0x000BDE02
		public XmlSyntaxException() : base(Environment.GetResourceString("XMLSyntax_InvalidSyntax"))
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x000BEE1F File Offset: 0x000BDE1F
		public XmlSyntaxException(string message) : base(message)
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x000BEE33 File Offset: 0x000BDE33
		public XmlSyntaxException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x000BEE48 File Offset: 0x000BDE48
		public XmlSyntaxException(int lineNumber) : base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("XMLSyntax_SyntaxError"), new object[]
		{
			lineNumber
		}))
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x000BEE8C File Offset: 0x000BDE8C
		public XmlSyntaxException(int lineNumber, string message) : base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("XMLSyntax_SyntaxErrorEx"), new object[]
		{
			lineNumber,
			message
		}))
		{
			base.SetErrorCode(-2146233320);
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x000BEED3 File Offset: 0x000BDED3
		internal XmlSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
