using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x0200068D RID: 1677
	internal class ToolStripDropTargetManager : IDropTarget
	{
		// Token: 0x06005844 RID: 22596 RVA: 0x001408CD File Offset: 0x0013F8CD
		public ToolStripDropTargetManager(ToolStrip owner)
		{
			this.owner = owner;
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x001408EA File Offset: 0x0013F8EA
		public void EnsureRegistered(IDropTarget dropTarget)
		{
			this.SetAcceptDrops(true);
		}

		// Token: 0x06005846 RID: 22598 RVA: 0x001408F4 File Offset: 0x0013F8F4
		public void EnsureUnRegistered(IDropTarget dropTarget)
		{
			for (int i = 0; i < this.owner.Items.Count; i++)
			{
				if (this.owner.Items[i].AllowDrop)
				{
					return;
				}
			}
			if (this.owner.AllowDrop || this.owner.AllowItemReorder)
			{
				return;
			}
			this.SetAcceptDrops(false);
			this.owner.DropTargetManager = null;
		}

		// Token: 0x06005847 RID: 22599 RVA: 0x00140963 File Offset: 0x0013F963
		private ToolStripItem FindItemAtPoint(int x, int y)
		{
			return this.owner.GetItemAt(this.owner.PointToClient(new Point(x, y)));
		}

		// Token: 0x06005848 RID: 22600 RVA: 0x00140984 File Offset: 0x0013F984
		public void OnDragEnter(DragEventArgs e)
		{
			if (this.owner.AllowItemReorder && e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				this.lastDropTarget = this.owner.ItemReorderDropTarget;
			}
			else
			{
				ToolStripItem toolStripItem = this.FindItemAtPoint(e.X, e.Y);
				if (toolStripItem != null && toolStripItem.AllowDrop)
				{
					this.lastDropTarget = toolStripItem;
				}
				else if (this.owner.AllowDrop)
				{
					this.lastDropTarget = this.owner;
				}
				else
				{
					this.lastDropTarget = null;
				}
			}
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragEnter(e);
			}
		}

		// Token: 0x06005849 RID: 22601 RVA: 0x00140A28 File Offset: 0x0013FA28
		public void OnDragOver(DragEventArgs e)
		{
			IDropTarget dropTarget;
			if (this.owner.AllowItemReorder && e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				dropTarget = this.owner.ItemReorderDropTarget;
			}
			else
			{
				ToolStripItem toolStripItem = this.FindItemAtPoint(e.X, e.Y);
				if (toolStripItem != null && toolStripItem.AllowDrop)
				{
					dropTarget = toolStripItem;
				}
				else if (this.owner.AllowDrop)
				{
					dropTarget = this.owner;
				}
				else
				{
					dropTarget = null;
				}
			}
			if (dropTarget != this.lastDropTarget)
			{
				this.UpdateDropTarget(dropTarget, e);
			}
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragOver(e);
			}
		}

		// Token: 0x0600584A RID: 22602 RVA: 0x00140AC8 File Offset: 0x0013FAC8
		public void OnDragLeave(EventArgs e)
		{
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragLeave(e);
			}
			this.lastDropTarget = null;
		}

		// Token: 0x0600584B RID: 22603 RVA: 0x00140AE5 File Offset: 0x0013FAE5
		public void OnDragDrop(DragEventArgs e)
		{
			if (this.lastDropTarget != null)
			{
				this.lastDropTarget.OnDragDrop(e);
			}
			this.lastDropTarget = null;
		}

		// Token: 0x0600584C RID: 22604 RVA: 0x00140B04 File Offset: 0x0013FB04
		private void SetAcceptDrops(bool accept)
		{
			if (this.owner.AllowDrop && accept)
			{
				IntSecurity.ClipboardRead.Demand();
			}
			if (accept && this.owner.IsHandleCreated)
			{
				try
				{
					if (Application.OleRequired() != ApartmentState.STA)
					{
						throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
					}
					if (accept)
					{
						int num = UnsafeNativeMethods.RegisterDragDrop(new HandleRef(this.owner, this.owner.Handle), new DropTarget(this));
						if (num != 0 && num != -2147221247)
						{
							throw new Win32Exception(num);
						}
					}
					else
					{
						IntSecurity.ClipboardRead.Assert();
						try
						{
							int num2 = UnsafeNativeMethods.RevokeDragDrop(new HandleRef(this.owner, this.owner.Handle));
							if (num2 != 0 && num2 != -2147221248)
							{
								throw new Win32Exception(num2);
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
				catch (Exception innerException)
				{
					throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), innerException);
				}
			}
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x00140C00 File Offset: 0x0013FC00
		private void UpdateDropTarget(IDropTarget newTarget, DragEventArgs e)
		{
			if (newTarget != this.lastDropTarget)
			{
				if (this.lastDropTarget != null)
				{
					this.OnDragLeave(new EventArgs());
				}
				this.lastDropTarget = newTarget;
				if (newTarget != null)
				{
					this.OnDragEnter(new DragEventArgs(e.Data, e.KeyState, e.X, e.Y, e.AllowedEffect, e.Effect)
					{
						Effect = DragDropEffects.None
					});
				}
			}
		}

		// Token: 0x04003802 RID: 14338
		private IDropTarget lastDropTarget;

		// Token: 0x04003803 RID: 14339
		private ToolStrip owner;

		// Token: 0x04003804 RID: 14340
		internal static readonly TraceSwitch DragDropDebug;
	}
}
