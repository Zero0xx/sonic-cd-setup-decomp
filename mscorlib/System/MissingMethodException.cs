using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000D9 RID: 217
	[ComVisible(true)]
	[Serializable]
	public class MissingMethodException : MissingMemberException, ISerializable
	{
		// Token: 0x06000C10 RID: 3088 RVA: 0x00023FAE File Offset: 0x00022FAE
		public MissingMethodException() : base(Environment.GetResourceString("Arg_MissingMethodException"))
		{
			base.SetErrorCode(-2146233069);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00023FCB File Offset: 0x00022FCB
		public MissingMethodException(string message) : base(message)
		{
			base.SetErrorCode(-2146233069);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00023FDF File Offset: 0x00022FDF
		public MissingMethodException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233069);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00023FF4 File Offset: 0x00022FF4
		protected MissingMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00024000 File Offset: 0x00023000
		public override string Message
		{
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingMethod_Name", new object[]
				{
					this.ClassName + "." + this.MemberName + ((this.Signature != null) ? (" " + MissingMemberException.FormatSignature(this.Signature)) : "")
				}), new object[0]);
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0002407B File Offset: 0x0002307B
		private MissingMethodException(string className, string methodName, byte[] signature)
		{
			this.ClassName = className;
			this.MemberName = methodName;
			this.Signature = signature;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00024098 File Offset: 0x00023098
		public MissingMethodException(string className, string methodName)
		{
			this.ClassName = className;
			this.MemberName = methodName;
		}
	}
}
