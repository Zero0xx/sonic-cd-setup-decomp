using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	// Token: 0x02000696 RID: 1686
	public sealed class LinkedResourceCollection : Collection<LinkedResource>, IDisposable
	{
		// Token: 0x06003403 RID: 13315 RVA: 0x000DB614 File Offset: 0x000DA614
		internal LinkedResourceCollection()
		{
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000DB61C File Offset: 0x000DA61C
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			foreach (LinkedResource linkedResource in this)
			{
				linkedResource.Dispose();
			}
			base.Clear();
			this.disposed = true;
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x000DB67C File Offset: 0x000DA67C
		protected override void RemoveItem(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.RemoveItem(index);
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x000DB69E File Offset: 0x000DA69E
		protected override void ClearItems()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			base.ClearItems();
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x000DB6BF File Offset: 0x000DA6BF
		protected override void SetItem(int index, LinkedResource item)
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

		// Token: 0x06003408 RID: 13320 RVA: 0x000DB6F0 File Offset: 0x000DA6F0
		protected override void InsertItem(int index, LinkedResource item)
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

		// Token: 0x04002FEF RID: 12271
		private bool disposed;
	}
}
