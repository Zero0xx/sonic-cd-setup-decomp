using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E2 RID: 226
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class EventHandlerList : IDisposable
	{
		// Token: 0x060007A9 RID: 1961 RVA: 0x0001B9B8 File Offset: 0x0001A9B8
		public EventHandlerList()
		{
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001B9C0 File Offset: 0x0001A9C0
		internal EventHandlerList(Component parent)
		{
			this.parent = parent;
		}

		// Token: 0x17000190 RID: 400
		public Delegate this[object key]
		{
			get
			{
				EventHandlerList.ListEntry listEntry = null;
				if (this.parent == null || this.parent.CanRaiseEventsInternal)
				{
					listEntry = this.Find(key);
				}
				if (listEntry != null)
				{
					return listEntry.handler;
				}
				return null;
			}
			set
			{
				EventHandlerList.ListEntry listEntry = this.Find(key);
				if (listEntry != null)
				{
					listEntry.handler = value;
					return;
				}
				this.head = new EventHandlerList.ListEntry(key, value, this.head);
			}
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001BA3C File Offset: 0x0001AA3C
		public void AddHandler(object key, Delegate value)
		{
			EventHandlerList.ListEntry listEntry = this.Find(key);
			if (listEntry != null)
			{
				listEntry.handler = Delegate.Combine(listEntry.handler, value);
				return;
			}
			this.head = new EventHandlerList.ListEntry(key, value, this.head);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001BA7C File Offset: 0x0001AA7C
		public void AddHandlers(EventHandlerList listToAddFrom)
		{
			for (EventHandlerList.ListEntry next = listToAddFrom.head; next != null; next = next.next)
			{
				this.AddHandler(next.key, next.handler);
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001BAAE File Offset: 0x0001AAAE
		public void Dispose()
		{
			this.head = null;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001BAB8 File Offset: 0x0001AAB8
		private EventHandlerList.ListEntry Find(object key)
		{
			EventHandlerList.ListEntry next = this.head;
			while (next != null && next.key != key)
			{
				next = next.next;
			}
			return next;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001BAE4 File Offset: 0x0001AAE4
		public void RemoveHandler(object key, Delegate value)
		{
			EventHandlerList.ListEntry listEntry = this.Find(key);
			if (listEntry != null)
			{
				listEntry.handler = Delegate.Remove(listEntry.handler, value);
			}
		}

		// Token: 0x04000971 RID: 2417
		private EventHandlerList.ListEntry head;

		// Token: 0x04000972 RID: 2418
		private Component parent;

		// Token: 0x020000E3 RID: 227
		private sealed class ListEntry
		{
			// Token: 0x060007B2 RID: 1970 RVA: 0x0001BB0E File Offset: 0x0001AB0E
			public ListEntry(object key, Delegate handler, EventHandlerList.ListEntry next)
			{
				this.next = next;
				this.key = key;
				this.handler = handler;
			}

			// Token: 0x04000973 RID: 2419
			internal EventHandlerList.ListEntry next;

			// Token: 0x04000974 RID: 2420
			internal object key;

			// Token: 0x04000975 RID: 2421
			internal Delegate handler;
		}
	}
}
