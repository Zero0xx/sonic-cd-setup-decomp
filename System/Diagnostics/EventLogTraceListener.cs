using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics
{
	// Token: 0x02000758 RID: 1880
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public sealed class EventLogTraceListener : TraceListener
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x000F43E3 File Offset: 0x000F33E3
		public EventLogTraceListener()
		{
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x000F43EB File Offset: 0x000F33EB
		public EventLogTraceListener(EventLog eventLog) : base((eventLog != null) ? eventLog.Source : string.Empty)
		{
			this.eventLog = eventLog;
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x000F440A File Offset: 0x000F340A
		public EventLogTraceListener(string source)
		{
			this.eventLog = new EventLog();
			this.eventLog.Source = source;
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x000F4429 File Offset: 0x000F3429
		// (set) Token: 0x06003998 RID: 14744 RVA: 0x000F4431 File Offset: 0x000F3431
		public EventLog EventLog
		{
			get
			{
				return this.eventLog;
			}
			set
			{
				this.eventLog = value;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003999 RID: 14745 RVA: 0x000F443A File Offset: 0x000F343A
		// (set) Token: 0x0600399A RID: 14746 RVA: 0x000F446A File Offset: 0x000F346A
		public override string Name
		{
			get
			{
				if (!this.nameSet && this.eventLog != null)
				{
					this.nameSet = true;
					base.Name = this.eventLog.Source;
				}
				return base.Name;
			}
			set
			{
				this.nameSet = true;
				base.Name = value;
			}
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x000F447A File Offset: 0x000F347A
		public override void Close()
		{
			if (this.eventLog != null)
			{
				this.eventLog.Close();
			}
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x000F448F File Offset: 0x000F348F
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x000F449A File Offset: 0x000F349A
		public override void Write(string message)
		{
			if (this.eventLog != null)
			{
				this.eventLog.WriteEntry(message);
			}
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x000F44B0 File Offset: 0x000F34B0
		public override void WriteLine(string message)
		{
			this.Write(message);
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x000F44BC File Offset: 0x000F34BC
		[ComVisible(false)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, format, args))
			{
				return;
			}
			EventInstance instance = this.CreateEventInstance(severity, id);
			if (args == null)
			{
				this.eventLog.WriteEvent(instance, new object[]
				{
					format
				});
				return;
			}
			if (string.IsNullOrEmpty(format))
			{
				string[] array = new string[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					array[i] = args[i].ToString();
				}
				this.eventLog.WriteEvent(instance, array);
				return;
			}
			this.eventLog.WriteEvent(instance, new object[]
			{
				string.Format(CultureInfo.InvariantCulture, format, args)
			});
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x000F4574 File Offset: 0x000F3574
		[ComVisible(false)]
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, message))
			{
				return;
			}
			EventInstance instance = this.CreateEventInstance(severity, id);
			this.eventLog.WriteEvent(instance, new object[]
			{
				message
			});
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000F45C4 File Offset: 0x000F35C4
		[ComVisible(false)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, data))
			{
				return;
			}
			EventInstance instance = this.CreateEventInstance(severity, id);
			this.eventLog.WriteEvent(instance, new object[]
			{
				data
			});
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x000F4614 File Offset: 0x000F3614
		[ComVisible(false)]
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
		{
			if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, null, data))
			{
				return;
			}
			EventInstance instance = this.CreateEventInstance(severity, id);
			StringBuilder stringBuilder = new StringBuilder();
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(", ");
					}
					if (data[i] != null)
					{
						stringBuilder.Append(data[i].ToString());
					}
				}
			}
			this.eventLog.WriteEvent(instance, new object[]
			{
				stringBuilder.ToString()
			});
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x000F46A8 File Offset: 0x000F36A8
		private EventInstance CreateEventInstance(TraceEventType severity, int id)
		{
			if (id > 65535)
			{
				id = 65535;
			}
			if (id < 0)
			{
				id = 0;
			}
			EventInstance eventInstance = new EventInstance((long)id, 0);
			if (severity == TraceEventType.Error || severity == TraceEventType.Critical)
			{
				eventInstance.EntryType = EventLogEntryType.Error;
			}
			else if (severity == TraceEventType.Warning)
			{
				eventInstance.EntryType = EventLogEntryType.Warning;
			}
			else
			{
				eventInstance.EntryType = EventLogEntryType.Information;
			}
			return eventInstance;
		}

		// Token: 0x040032BE RID: 12990
		private EventLog eventLog;

		// Token: 0x040032BF RID: 12991
		private bool nameSet;
	}
}
