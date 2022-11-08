using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000D8 RID: 216
	[ComVisible(true)]
	[Serializable]
	public class MissingFieldException : MissingMemberException, ISerializable
	{
		// Token: 0x06000C09 RID: 3081 RVA: 0x00023EB0 File Offset: 0x00022EB0
		public MissingFieldException() : base(Environment.GetResourceString("Arg_MissingFieldException"))
		{
			base.SetErrorCode(-2146233071);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00023ECD File Offset: 0x00022ECD
		public MissingFieldException(string message) : base(message)
		{
			base.SetErrorCode(-2146233071);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00023EE1 File Offset: 0x00022EE1
		public MissingFieldException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233071);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00023EF6 File Offset: 0x00022EF6
		protected MissingFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00023F00 File Offset: 0x00022F00
		public override string Message
		{
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingField_Name", new object[]
				{
					((this.Signature != null) ? (MissingMemberException.FormatSignature(this.Signature) + " ") : "") + this.ClassName + "." + this.MemberName
				}), new object[0]);
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00023F7B File Offset: 0x00022F7B
		private MissingFieldException(string className, string fieldName, byte[] signature)
		{
			this.ClassName = className;
			this.MemberName = fieldName;
			this.Signature = signature;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00023F98 File Offset: 0x00022F98
		public MissingFieldException(string className, string fieldName)
		{
			this.ClassName = className;
			this.MemberName = fieldName;
		}
	}
}
