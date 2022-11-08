using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000676 RID: 1654
	public sealed class AlternateViewCollection : Collection<AlternateView>, IDisposable
	{
		// Token: 0x0600331F RID: 13087 RVA: 0x000D8230 File Offset: 0x000D7230
		internal AlternateViewCollection()
		{
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000D8238 File Offset: 0x000D7238
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (AlternateView alternateView in this)
			{
				alternateView.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000D8298 File Offset: 0x000D7298
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000D82BA File Offset: 0x000D72BA
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000D82DB File Offset: 0x000D72DB
		protected override void SetItem(int index, AlternateView item)
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

		// Token: 0x06003324 RID: 13092 RVA: 0x000D830C File Offset: 0x000D730C
		protected override void InsertItem(int index, AlternateView item)
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

		// Token: 0x04002F7A RID: 12154
		private bool disposed;
	}
}
