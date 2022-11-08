using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200006F RID: 111
	[ComVisible(true)]
	[Serializable]
	public class ArgumentOutOfRangeException : ArgumentException, ISerializable
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00015AD5 File Offset: 0x00014AD5
		private static string RangeMessage
		{
			get
			{
				if (ArgumentOutOfRangeException._rangeMessage == null)
				{
					ArgumentOutOfRangeException._rangeMessage = Environment.GetResourceString("Arg_ArgumentOutOfRangeException");
				}
				return ArgumentOutOfRangeException._rangeMessage;
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00015AF2 File Offset: 0x00014AF2
		public ArgumentOutOfRangeException() : base(ArgumentOutOfRangeException.RangeMessage)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00015B0A File Offset: 0x00014B0A
		public ArgumentOutOfRangeException(string paramName) : base(ArgumentOutOfRangeException.RangeMessage, paramName)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00015B23 File Offset: 0x00014B23
		public ArgumentOutOfRangeException(string paramName, string message) : base(message, paramName)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00015B38 File Offset: 0x00014B38
		public ArgumentOutOfRangeException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00015B4D File Offset: 0x00014B4D
		public ArgumentOutOfRangeException(string paramName, object actualValue, string message) : base(message, paramName)
		{
			this.m_actualValue = actualValue;
			base.SetErrorCode(-2146233086);
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00015B6C File Offset: 0x00014B6C
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (this.m_actualValue == null)
				{
					return message;
				}
				string text = string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ActualValue"), new object[]
				{
					this.m_actualValue.ToString()
				});
				if (message == null)
				{
					return text;
				}
				return message + Environment.NewLine + text;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00015BC6 File Offset: 0x00014BC6
		public virtual object ActualValue
		{
			get
			{
				return this.m_actualValue;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00015BCE File Offset: 0x00014BCE
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ActualValue", this.m_actualValue, typeof(object));
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00015C01 File Offset: 0x00014C01
		protected ArgumentOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.m_actualValue = info.GetValue("ActualValue", typeof(object));
		}

		// Token: 0x040001F7 RID: 503
		private static string _rangeMessage;

		// Token: 0x040001F8 RID: 504
		private object m_actualValue;
	}
}
