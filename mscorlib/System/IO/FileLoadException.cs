using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x020005B7 RID: 1463
	[ComVisible(true)]
	[Serializable]
	public class FileLoadException : IOException
	{
		// Token: 0x06003606 RID: 13830 RVA: 0x000B432B File Offset: 0x000B332B
		public FileLoadException() : base(Environment.GetResourceString("IO.FileLoad"))
		{
			base.SetErrorCode(-2146232799);
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x000B4348 File Offset: 0x000B3348
		public FileLoadException(string message) : base(message)
		{
			base.SetErrorCode(-2146232799);
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x000B435C File Offset: 0x000B335C
		public FileLoadException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146232799);
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x000B4371 File Offset: 0x000B3371
		public FileLoadException(string message, string fileName) : base(message)
		{
			base.SetErrorCode(-2146232799);
			this._fileName = fileName;
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x000B438C File Offset: 0x000B338C
		public FileLoadException(string message, string fileName, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146232799);
			this._fileName = fileName;
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x0600360B RID: 13835 RVA: 0x000B43A8 File Offset: 0x000B33A8
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000B43B6 File Offset: 0x000B33B6
		private void SetMessageField()
		{
			if (this._message == null)
			{
				this._message = FileLoadException.FormatFileLoadExceptionMessage(this._fileName, base.HResult);
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600360D RID: 13837 RVA: 0x000B43D7 File Offset: 0x000B33D7
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000B43E0 File Offset: 0x000B33E0
		public override string ToString()
		{
			string text = base.GetType().FullName + ": " + this.Message;
			if (this._fileName != null && this._fileName.Length != 0)
			{
				text = text + Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileName_Name"), new object[]
				{
					this._fileName
				});
			}
			if (base.InnerException != null)
			{
				text = text + " ---> " + base.InnerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			try
			{
				if (this.FusionLog != null)
				{
					if (text == null)
					{
						text = " ";
					}
					text += Environment.NewLine;
					text += Environment.NewLine;
					text += this.FusionLog;
				}
			}
			catch (SecurityException)
			{
			}
			return text;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000B44D4 File Offset: 0x000B34D4
		protected FileLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._fileName = info.GetString("FileLoad_FileName");
			try
			{
				this._fusionLog = info.GetString("FileLoad_FusionLog");
			}
			catch
			{
				this._fusionLog = null;
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000B4528 File Offset: 0x000B3528
		private FileLoadException(string fileName, string fusionLog, int hResult) : base(null)
		{
			base.SetErrorCode(hResult);
			this._fileName = fileName;
			this._fusionLog = fusionLog;
			this.SetMessageField();
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003611 RID: 13841 RVA: 0x000B454C File Offset: 0x000B354C
		public string FusionLog
		{
			[SecurityPermission(SecurityAction.Demand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
			get
			{
				return this._fusionLog;
			}
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000B4554 File Offset: 0x000B3554
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("FileLoad_FileName", this._fileName, typeof(string));
			try
			{
				info.AddValue("FileLoad_FusionLog", this.FusionLog, typeof(string));
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000B45B4 File Offset: 0x000B35B4
		internal static string FormatFileLoadExceptionMessage(string fileName, int hResult)
		{
			return string.Format(CultureInfo.CurrentCulture, FileLoadException.GetFileLoadExceptionMessage(hResult), new object[]
			{
				fileName,
				FileLoadException.GetMessageForHR(hResult)
			});
		}

		// Token: 0x06003614 RID: 13844
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetFileLoadExceptionMessage(int hResult);

		// Token: 0x06003615 RID: 13845
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetMessageForHR(int hresult);

		// Token: 0x04001C37 RID: 7223
		private string _fileName;

		// Token: 0x04001C38 RID: 7224
		private string _fusionLog;
	}
}
