using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000DE RID: 222
	[ComVisible(true)]
	[Serializable]
	public class NotFiniteNumberException : ArithmeticException
	{
		// Token: 0x06000C1E RID: 3102 RVA: 0x00024131 File Offset: 0x00023131
		public NotFiniteNumberException() : base(Environment.GetResourceString("Arg_NotFiniteNumberException"))
		{
			this._offendingNumber = 0.0;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0002415D File Offset: 0x0002315D
		public NotFiniteNumberException(double offendingNumber)
		{
			this._offendingNumber = offendingNumber;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00024177 File Offset: 0x00023177
		public NotFiniteNumberException(string message) : base(message)
		{
			this._offendingNumber = 0.0;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0002419A File Offset: 0x0002319A
		public NotFiniteNumberException(string message, double offendingNumber) : base(message)
		{
			this._offendingNumber = offendingNumber;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000241B5 File Offset: 0x000231B5
		public NotFiniteNumberException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000241CA File Offset: 0x000231CA
		public NotFiniteNumberException(string message, double offendingNumber, Exception innerException) : base(message, innerException)
		{
			this._offendingNumber = offendingNumber;
			base.SetErrorCode(-2146233048);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x000241E6 File Offset: 0x000231E6
		protected NotFiniteNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._offendingNumber = (double)info.GetInt32("OffendingNumber");
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00024202 File Offset: 0x00023202
		public double OffendingNumber
		{
			get
			{
				return this._offendingNumber;
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0002420A File Offset: 0x0002320A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("OffendingNumber", this._offendingNumber, typeof(int));
		}

		// Token: 0x04000460 RID: 1120
		private double _offendingNumber;
	}
}
