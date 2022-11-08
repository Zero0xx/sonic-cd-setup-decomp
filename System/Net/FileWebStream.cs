using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020003B3 RID: 947
	internal sealed class FileWebStream : FileStream, ICloseEx
	{
		// Token: 0x06001DBC RID: 7612 RVA: 0x00071045 File Offset: 0x00070045
		public FileWebStream(FileWebRequest request, string path, FileMode mode, FileAccess access, FileShare sharing) : base(path, mode, access, sharing)
		{
			this.m_request = request;
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0007105A File Offset: 0x0007005A
		public FileWebStream(FileWebRequest request, string path, FileMode mode, FileAccess access, FileShare sharing, int length, bool async) : base(path, mode, access, sharing, length, async)
		{
			this.m_request = request;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x00071074 File Offset: 0x00070074
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.m_request != null)
				{
					this.m_request.UnblockReader();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x000710B4 File Offset: 0x000700B4
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			if ((closeState & CloseExState.Abort) != CloseExState.Normal)
			{
				this.SafeFileHandle.Close();
				return;
			}
			this.Close();
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000710D0 File Offset: 0x000700D0
		public override int Read(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			int result;
			try
			{
				result = base.Read(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return result;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0007110C File Offset: 0x0007010C
		public override void Write(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			try
			{
				base.Write(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00071144 File Offset: 0x00070144
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			IAsyncResult result;
			try
			{
				result = base.BeginRead(buffer, offset, size, callback, state);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return result;
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00071184 File Offset: 0x00070184
		public override int EndRead(IAsyncResult ar)
		{
			int result;
			try
			{
				result = base.EndRead(ar);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return result;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000711B8 File Offset: 0x000701B8
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			IAsyncResult result;
			try
			{
				result = base.BeginWrite(buffer, offset, size, callback, state);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return result;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000711F8 File Offset: 0x000701F8
		public override void EndWrite(IAsyncResult ar)
		{
			try
			{
				base.EndWrite(ar);
			}
			catch
			{
				this.CheckError();
				throw;
			}
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00071228 File Offset: 0x00070228
		private void CheckError()
		{
			if (this.m_request.Aborted)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
		}

		// Token: 0x04001DA0 RID: 7584
		private FileWebRequest m_request;
	}
}
