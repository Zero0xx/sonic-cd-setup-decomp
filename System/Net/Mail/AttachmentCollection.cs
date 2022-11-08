using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000678 RID: 1656
	public sealed class AttachmentCollection : Collection<Attachment>, IDisposable
	{
		// Token: 0x06003336 RID: 13110 RVA: 0x000D85B8 File Offset: 0x000D75B8
		internal AttachmentCollection()
		{
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000D85C0 File Offset: 0x000D75C0
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (Attachment attachment in this)
			{
				attachment.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000D8620 File Offset: 0x000D7620
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000D8642 File Offset: 0x000D7642
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000D8663 File Offset: 0x000D7663
		protected override void SetItem(int index, Attachment item)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x000D8694 File Offset: 0x000D7694
		protected override void InsertItem(int index, Attachment item)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x04002F7D RID: 12157
		private bool disposed;
	}
}
