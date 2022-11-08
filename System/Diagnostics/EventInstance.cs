using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x0200074A RID: 1866
	public class EventInstance
	{
		// Token: 0x060038D7 RID: 14551 RVA: 0x000F0085 File Offset: 0x000EF085
		public EventInstance(long instanceId, int categoryId)
		{
			this.CategoryId = categoryId;
			this.InstanceId = instanceId;
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x000F00A2 File Offset: 0x000EF0A2
		public EventInstance(long instanceId, int categoryId, EventLogEntryType entryType) : this(instanceId, categoryId)
		{
			this.EntryType = entryType;
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x060038D9 RID: 14553 RVA: 0x000F00B3 File Offset: 0x000EF0B3
		// (set) Token: 0x060038DA RID: 14554 RVA: 0x000F00BB File Offset: 0x000EF0BB
		public int CategoryId
		{
			get
			{
				return this._categoryNumber;
			}
			set
			{
				if (value > 65535 || value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._categoryNumber = value;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x060038DB RID: 14555 RVA: 0x000F00DB File Offset: 0x000EF0DB
		// (set) Token: 0x060038DC RID: 14556 RVA: 0x000F00E3 File Offset: 0x000EF0E3
		public EventLogEntryType EntryType
		{
			get
			{
				return this._entryType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(EventLogEntryType), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(EventLogEntryType));
				}
				this._entryType = value;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x060038DD RID: 14557 RVA: 0x000F0119 File Offset: 0x000EF119
		// (set) Token: 0x060038DE RID: 14558 RVA: 0x000F0121 File Offset: 0x000EF121
		public long InstanceId
		{
			get
			{
				return this._instanceId;
			}
			set
			{
				if (value > (long)((ulong)-1) || value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._instanceId = value;
			}
		}

		// Token: 0x04003263 RID: 12899
		private int _categoryNumber;

		// Token: 0x04003264 RID: 12900
		private EventLogEntryType _entryType = EventLogEntryType.Information;

		// Token: 0x04003265 RID: 12901
		private long _instanceId;
	}
}
