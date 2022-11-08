using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003B4 RID: 948
	[Serializable]
	public abstract class WebResponse : MarshalByRefObject, ISerializable, IDisposable
	{
		// Token: 0x06001DC7 RID: 7623 RVA: 0x00071249 File Offset: 0x00070249
		protected WebResponse()
		{
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00071251 File Offset: 0x00070251
		protected WebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00071259 File Offset: 0x00070259
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00071263 File Offset: 0x00070263
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x00071265 File Offset: 0x00070265
		public virtual void Close()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0007126C File Offset: 0x0007026C
		void IDisposable.Dispose()
		{
			try
			{
				this.Close();
				this.OnDispose();
			}
			catch
			{
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0007129C File Offset: 0x0007029C
		internal virtual void OnDispose()
		{
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001DCE RID: 7630 RVA: 0x0007129E File Offset: 0x0007029E
		public virtual bool IsFromCache
		{
			get
			{
				return this.m_IsFromCache;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (set) Token: 0x06001DCF RID: 7631 RVA: 0x000712A6 File Offset: 0x000702A6
		internal bool InternalSetFromCache
		{
			set
			{
				this.m_IsFromCache = value;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001DD0 RID: 7632 RVA: 0x000712AF File Offset: 0x000702AF
		internal virtual bool IsCacheFresh
		{
			get
			{
				return this.m_IsCacheFresh;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (set) Token: 0x06001DD1 RID: 7633 RVA: 0x000712B7 File Offset: 0x000702B7
		internal bool InternalSetIsCacheFresh
		{
			set
			{
				this.m_IsCacheFresh = value;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x000712C0 File Offset: 0x000702C0
		public virtual bool IsMutuallyAuthenticated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x000712C3 File Offset: 0x000702C3
		// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x000712CA File Offset: 0x000702CA
		public virtual long ContentLength
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x000712D1 File Offset: 0x000702D1
		// (set) Token: 0x06001DD6 RID: 7638 RVA: 0x000712D8 File Offset: 0x000702D8
		public virtual string ContentType
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000712DF File Offset: 0x000702DF
		public virtual Stream GetResponseStream()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x000712E6 File Offset: 0x000702E6
		public virtual Uri ResponseUri
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x000712ED File Offset: 0x000702ED
		public virtual WebHeaderCollection Headers
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x04001DA1 RID: 7585
		private bool m_IsCacheFresh;

		// Token: 0x04001DA2 RID: 7586
		private bool m_IsFromCache;
	}
}
