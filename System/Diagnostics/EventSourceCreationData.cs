using System;

namespace System.Diagnostics
{
	// Token: 0x02000759 RID: 1881
	public class EventSourceCreationData
	{
		// Token: 0x060039A4 RID: 14756 RVA: 0x000F46FA File Offset: 0x000F36FA
		private EventSourceCreationData()
		{
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x000F4718 File Offset: 0x000F3718
		public EventSourceCreationData(string source, string logName)
		{
			this._source = source;
			this._logName = logName;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x000F4744 File Offset: 0x000F3744
		internal EventSourceCreationData(string source, string logName, string machineName)
		{
			this._source = source;
			this._logName = logName;
			this._machineName = machineName;
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x000F4778 File Offset: 0x000F3778
		private EventSourceCreationData(string source, string logName, string machineName, string messageResourceFile, string parameterResourceFile, string categoryResourceFile, short categoryCount)
		{
			this._source = source;
			this._logName = logName;
			this._machineName = machineName;
			this._messageResourceFile = messageResourceFile;
			this._parameterResourceFile = parameterResourceFile;
			this._categoryResourceFile = categoryResourceFile;
			this.CategoryCount = (int)categoryCount;
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x060039A8 RID: 14760 RVA: 0x000F47D6 File Offset: 0x000F37D6
		// (set) Token: 0x060039A9 RID: 14761 RVA: 0x000F47DE File Offset: 0x000F37DE
		public string LogName
		{
			get
			{
				return this._logName;
			}
			set
			{
				this._logName = value;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x060039AA RID: 14762 RVA: 0x000F47E7 File Offset: 0x000F37E7
		// (set) Token: 0x060039AB RID: 14763 RVA: 0x000F47EF File Offset: 0x000F37EF
		public string MachineName
		{
			get
			{
				return this._machineName;
			}
			set
			{
				this._machineName = value;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x060039AC RID: 14764 RVA: 0x000F47F8 File Offset: 0x000F37F8
		// (set) Token: 0x060039AD RID: 14765 RVA: 0x000F4800 File Offset: 0x000F3800
		public string Source
		{
			get
			{
				return this._source;
			}
			set
			{
				this._source = value;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x000F4809 File Offset: 0x000F3809
		// (set) Token: 0x060039AF RID: 14767 RVA: 0x000F4811 File Offset: 0x000F3811
		public string MessageResourceFile
		{
			get
			{
				return this._messageResourceFile;
			}
			set
			{
				this._messageResourceFile = value;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x060039B0 RID: 14768 RVA: 0x000F481A File Offset: 0x000F381A
		// (set) Token: 0x060039B1 RID: 14769 RVA: 0x000F4822 File Offset: 0x000F3822
		public string ParameterResourceFile
		{
			get
			{
				return this._parameterResourceFile;
			}
			set
			{
				this._parameterResourceFile = value;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x000F482B File Offset: 0x000F382B
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x000F4833 File Offset: 0x000F3833
		public string CategoryResourceFile
		{
			get
			{
				return this._categoryResourceFile;
			}
			set
			{
				this._categoryResourceFile = value;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000F483C File Offset: 0x000F383C
		// (set) Token: 0x060039B5 RID: 14773 RVA: 0x000F4844 File Offset: 0x000F3844
		public int CategoryCount
		{
			get
			{
				return this._categoryCount;
			}
			set
			{
				if (value > 65535 || value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._categoryCount = value;
			}
		}

		// Token: 0x040032C0 RID: 12992
		private string _logName = "Application";

		// Token: 0x040032C1 RID: 12993
		private string _machineName = ".";

		// Token: 0x040032C2 RID: 12994
		private string _source;

		// Token: 0x040032C3 RID: 12995
		private string _messageResourceFile;

		// Token: 0x040032C4 RID: 12996
		private string _parameterResourceFile;

		// Token: 0x040032C5 RID: 12997
		private string _categoryResourceFile;

		// Token: 0x040032C6 RID: 12998
		private int _categoryCount;
	}
}
