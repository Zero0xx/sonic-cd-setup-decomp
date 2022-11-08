﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Net
{
	// Token: 0x020003D8 RID: 984
	public sealed class HttpListenerResponse : IDisposable
	{
		// Token: 0x06001F2C RID: 7980 RVA: 0x00078ED4 File Offset: 0x00077ED4
		internal static string GetStatusDescription(int code)
		{
			if (code >= 100 && code < 600)
			{
				int num = code / 100;
				int num2 = code % 100;
				if (num2 < HttpListenerResponse.s_HTTPStatusDescriptions[num].Length)
				{
					return HttpListenerResponse.s_HTTPStatusDescriptions[num][num2];
				}
			}
			return null;
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x00078F10 File Offset: 0x00077F10
		internal HttpListenerResponse()
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", "");
			}
			this.m_NativeResponse = default(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE);
			this.m_WebHeaders = new WebHeaderCollection(WebHeaderCollectionType.HttpListenerResponse);
			this.m_BoundaryType = BoundaryType.None;
			this.m_NativeResponse.StatusCode = 200;
			this.m_NativeResponse.Version.MajorVersion = 1;
			this.m_NativeResponse.Version.MinorVersion = 1;
			this.m_KeepAlive = true;
			this.m_ResponseState = HttpListenerResponse.ResponseState.Created;
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x00078F9E File Offset: 0x00077F9E
		internal HttpListenerResponse(HttpListenerContext httpContext) : this()
		{
			if (Logging.On)
			{
				Logging.Associate(Logging.HttpListener, this, httpContext);
			}
			this.m_HttpContext = httpContext;
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x00078FC0 File Offset: 0x00077FC0
		private HttpListenerContext HttpListenerContext
		{
			get
			{
				return this.m_HttpContext;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x00078FC8 File Offset: 0x00077FC8
		private HttpListenerRequest HttpListenerRequest
		{
			get
			{
				return this.HttpListenerContext.Request;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x00078FD5 File Offset: 0x00077FD5
		// (set) Token: 0x06001F32 RID: 7986 RVA: 0x00078FDD File Offset: 0x00077FDD
		public Encoding ContentEncoding
		{
			get
			{
				return this.m_ContentEncoding;
			}
			set
			{
				this.m_ContentEncoding = value;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x00078FE6 File Offset: 0x00077FE6
		// (set) Token: 0x06001F34 RID: 7988 RVA: 0x00078FF5 File Offset: 0x00077FF5
		public string ContentType
		{
			get
			{
				return this.Headers[HttpResponseHeader.ContentType];
			}
			set
			{
				this.CheckDisposed();
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(HttpResponseHeader.ContentType);
					return;
				}
				this.Headers.Set(HttpResponseHeader.ContentType, value);
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x00079021 File Offset: 0x00078021
		public Stream OutputStream
		{
			get
			{
				this.CheckDisposed();
				this.EnsureResponseStream();
				return this.m_ResponseStream;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001F36 RID: 7990 RVA: 0x00079035 File Offset: 0x00078035
		// (set) Token: 0x06001F37 RID: 7991 RVA: 0x00079044 File Offset: 0x00078044
		public string RedirectLocation
		{
			get
			{
				return this.Headers[HttpResponseHeader.Location];
			}
			set
			{
				this.CheckDisposed();
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(HttpResponseHeader.Location);
					return;
				}
				this.Headers.Set(HttpResponseHeader.Location, value);
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x00079070 File Offset: 0x00078070
		// (set) Token: 0x06001F39 RID: 7993 RVA: 0x0007907D File Offset: 0x0007807D
		public int StatusCode
		{
			get
			{
				return (int)this.m_NativeResponse.StatusCode;
			}
			set
			{
				this.CheckDisposed();
				if (value < 100 || value > 999)
				{
					throw new ProtocolViolationException(SR.GetString("net_invalidstatus"));
				}
				this.m_NativeResponse.StatusCode = (ushort)value;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x000790AF File Offset: 0x000780AF
		// (set) Token: 0x06001F3B RID: 7995 RVA: 0x000790E4 File Offset: 0x000780E4
		public string StatusDescription
		{
			get
			{
				if (this.m_StatusDescription == null)
				{
					this.m_StatusDescription = HttpListenerResponse.GetStatusDescription(this.StatusCode);
				}
				if (this.m_StatusDescription == null)
				{
					this.m_StatusDescription = string.Empty;
				}
				return this.m_StatusDescription;
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				for (int i = 0; i < value.Length; i++)
				{
					char c = 'ÿ' & value[i];
					if ((c <= '\u001f' && c != '\t') || c == '\u007f')
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidControlChars"), "name");
					}
				}
				this.m_StatusDescription = value;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x00079150 File Offset: 0x00078150
		// (set) Token: 0x06001F3D RID: 7997 RVA: 0x0007916C File Offset: 0x0007816C
		public CookieCollection Cookies
		{
			get
			{
				if (this.m_Cookies == null)
				{
					this.m_Cookies = new CookieCollection(false);
				}
				return this.m_Cookies;
			}
			set
			{
				this.m_Cookies = value;
			}
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x00079178 File Offset: 0x00078178
		public void CopyFrom(HttpListenerResponse templateResponse)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "CopyFrom", "templateResponse#" + ValidationHelper.HashString(templateResponse));
			}
			this.m_NativeResponse = default(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE);
			this.m_ResponseState = HttpListenerResponse.ResponseState.Created;
			this.m_WebHeaders = templateResponse.m_WebHeaders;
			this.m_BoundaryType = templateResponse.m_BoundaryType;
			this.m_ContentLength = templateResponse.m_ContentLength;
			this.m_NativeResponse.StatusCode = templateResponse.m_NativeResponse.StatusCode;
			this.m_NativeResponse.Version.MajorVersion = templateResponse.m_NativeResponse.Version.MajorVersion;
			this.m_NativeResponse.Version.MinorVersion = templateResponse.m_NativeResponse.Version.MinorVersion;
			this.m_StatusDescription = templateResponse.m_StatusDescription;
			this.m_KeepAlive = templateResponse.m_KeepAlive;
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x00079251 File Offset: 0x00078251
		// (set) Token: 0x06001F40 RID: 8000 RVA: 0x0007925C File Offset: 0x0007825C
		public bool SendChunked
		{
			get
			{
				return this.EntitySendFormat == EntitySendFormat.Chunked;
			}
			set
			{
				if (value)
				{
					this.EntitySendFormat = EntitySendFormat.Chunked;
					return;
				}
				this.EntitySendFormat = EntitySendFormat.ContentLength;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x00079270 File Offset: 0x00078270
		// (set) Token: 0x06001F42 RID: 8002 RVA: 0x00079278 File Offset: 0x00078278
		internal EntitySendFormat EntitySendFormat
		{
			get
			{
				return (EntitySendFormat)this.m_BoundaryType;
			}
			set
			{
				this.CheckDisposed();
				if (this.m_ResponseState >= HttpListenerResponse.ResponseState.SentHeaders)
				{
					throw new InvalidOperationException(SR.GetString("net_rspsubmitted"));
				}
				if (value == EntitySendFormat.Chunked && this.HttpListenerRequest.ProtocolVersion.Minor == 0)
				{
					throw new ProtocolViolationException(SR.GetString("net_nochunkuploadonhttp10"));
				}
				this.m_BoundaryType = (BoundaryType)value;
				if (value != EntitySendFormat.ContentLength)
				{
					this.m_ContentLength = -1L;
				}
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x000792DC File Offset: 0x000782DC
		// (set) Token: 0x06001F44 RID: 8004 RVA: 0x000792E4 File Offset: 0x000782E4
		public bool KeepAlive
		{
			get
			{
				return this.m_KeepAlive;
			}
			set
			{
				this.CheckDisposed();
				this.m_KeepAlive = value;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x000792F3 File Offset: 0x000782F3
		// (set) Token: 0x06001F46 RID: 8006 RVA: 0x000792FC File Offset: 0x000782FC
		public WebHeaderCollection Headers
		{
			get
			{
				return this.m_WebHeaders;
			}
			set
			{
				this.m_WebHeaders.Clear();
				foreach (string name in value.AllKeys)
				{
					this.m_WebHeaders.Add(name, value[name]);
				}
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x00079340 File Offset: 0x00078340
		public void AddHeader(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "AddHeader", " name=" + name + " value=" + value);
			}
			this.Headers.SetInternal(name, value);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x00079377 File Offset: 0x00078377
		public void AppendHeader(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "AppendHeader", " name=" + name + " value=" + value);
			}
			this.Headers.Add(name, value);
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000793B0 File Offset: 0x000783B0
		public void Redirect(string url)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "Redirect", " url=" + url);
			}
			this.Headers.SetInternal(HttpResponseHeader.Location, url);
			this.StatusCode = 302;
			this.StatusDescription = HttpListenerResponse.GetStatusDescription(this.StatusCode);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0007940C File Offset: 0x0007840C
		public void AppendCookie(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "AppendCookie", " cookie#" + ValidationHelper.HashString(cookie));
			}
			this.Cookies.Add(cookie);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0007945C File Offset: 0x0007845C
		public void SetCookie(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			int num = this.Cookies.InternalAdd(cookie, true);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.HttpListener, this, "SetCookie", " cookie#" + ValidationHelper.HashString(cookie));
			}
			if (num != 1)
			{
				throw new ArgumentException(SR.GetString("net_cookie_exists"), "cookie");
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x000794C5 File Offset: 0x000784C5
		// (set) Token: 0x06001F4D RID: 8013 RVA: 0x000794D0 File Offset: 0x000784D0
		public long ContentLength64
		{
			get
			{
				return this.m_ContentLength;
			}
			set
			{
				this.CheckDisposed();
				if (this.m_ResponseState >= HttpListenerResponse.ResponseState.SentHeaders)
				{
					throw new InvalidOperationException(SR.GetString("net_rspsubmitted"));
				}
				if (value >= 0L)
				{
					this.m_ContentLength = value;
					this.m_BoundaryType = BoundaryType.ContentLength;
					return;
				}
				throw new ArgumentOutOfRangeException(SR.GetString("net_clsmall"));
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x0007951F File Offset: 0x0007851F
		// (set) Token: 0x06001F4F RID: 8015 RVA: 0x00079548 File Offset: 0x00078548
		public Version ProtocolVersion
		{
			get
			{
				return new Version((int)this.m_NativeResponse.Version.MajorVersion, (int)this.m_NativeResponse.Version.MinorVersion);
			}
			set
			{
				this.CheckDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Major != 1 || (value.Minor != 0 && value.Minor != 1))
				{
					throw new ArgumentException(SR.GetString("net_wrongversion"), "value");
				}
				this.m_NativeResponse.Version.MajorVersion = (ushort)value.Major;
				this.m_NativeResponse.Version.MinorVersion = (ushort)value.Minor;
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000795CC File Offset: 0x000785CC
		public void Abort()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "abort", "");
			}
			try
			{
				if (this.m_ResponseState < HttpListenerResponse.ResponseState.Closed)
				{
					this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
					this.HttpListenerContext.Abort();
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "abort", "");
				}
			}
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x00079644 File Offset: 0x00078644
		public void Close(byte[] responseEntity, bool willBlock)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", string.Concat(new object[]
				{
					" responseEntity=",
					ValidationHelper.HashString(responseEntity),
					" willBlock=",
					willBlock
				}));
			}
			try
			{
				this.CheckDisposed();
				if (responseEntity == null)
				{
					throw new ArgumentNullException("responseEntity");
				}
				if (this.m_ResponseState < HttpListenerResponse.ResponseState.SentHeaders && this.m_BoundaryType != BoundaryType.Chunked)
				{
					this.ContentLength64 = (long)responseEntity.Length;
				}
				this.EnsureResponseStream();
				if (willBlock)
				{
					try
					{
						try
						{
							this.m_ResponseStream.Write(responseEntity, 0, responseEntity.Length);
						}
						catch (Win32Exception)
						{
						}
						goto IL_D2;
					}
					finally
					{
						this.m_ResponseStream.Close();
						this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
						this.HttpListenerContext.Close();
					}
				}
				this.m_ResponseStream.BeginWrite(responseEntity, 0, responseEntity.Length, new AsyncCallback(this.NonBlockingCloseCallback), null);
				IL_D2:;
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Close", "");
				}
			}
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0007976C File Offset: 0x0007876C
		public void Close()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			try
			{
				((IDisposable)this).Dispose();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Close", "");
				}
			}
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000797CC File Offset: 0x000787CC
		private void Dispose(bool disposing)
		{
			if (this.m_ResponseState >= HttpListenerResponse.ResponseState.Closed)
			{
				return;
			}
			this.EnsureResponseStream();
			this.m_ResponseStream.Close();
			this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
			this.HttpListenerContext.Close();
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000797FB File Offset: 0x000787FB
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x0007980A File Offset: 0x0007880A
		internal BoundaryType BoundaryType
		{
			get
			{
				return this.m_BoundaryType;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001F56 RID: 8022 RVA: 0x00079812 File Offset: 0x00078812
		internal bool SentHeaders
		{
			get
			{
				return this.m_ResponseState >= HttpListenerResponse.ResponseState.SentHeaders;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x00079820 File Offset: 0x00078820
		internal bool ComputedHeaders
		{
			get
			{
				return this.m_ResponseState >= HttpListenerResponse.ResponseState.ComputedHeaders;
			}
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0007982E File Offset: 0x0007882E
		private void EnsureResponseStream()
		{
			if (this.m_ResponseStream == null)
			{
				this.m_ResponseStream = new HttpResponseStream(this.HttpListenerContext);
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0007984C File Offset: 0x0007884C
		private void NonBlockingCloseCallback(IAsyncResult asyncResult)
		{
			try
			{
				this.m_ResponseStream.EndWrite(asyncResult);
			}
			catch (Win32Exception)
			{
			}
			finally
			{
				this.m_ResponseStream.Close();
				this.HttpListenerContext.Close();
				this.m_ResponseState = HttpListenerResponse.ResponseState.Closed;
			}
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000798A8 File Offset: 0x000788A8
		internal unsafe uint SendHeaders(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK* pDataChunk, HttpResponseStreamAsyncResult asyncResult, UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS flags)
		{
			if (Logging.On)
			{
				StringBuilder stringBuilder = new StringBuilder("HttpListenerResponse Headers:\n");
				for (int i = 0; i < this.Headers.Count; i++)
				{
					stringBuilder.Append("\t");
					stringBuilder.Append(this.Headers.GetKey(i));
					stringBuilder.Append(" : ");
					stringBuilder.Append(this.Headers.Get(i));
					stringBuilder.Append("\n");
				}
				Logging.PrintInfo(Logging.HttpListener, this, ".ctor", stringBuilder.ToString());
			}
			this.m_ResponseState = HttpListenerResponse.ResponseState.SentHeaders;
			List<GCHandle> pinnedHeaders = this.SerializeHeaders(ref this.m_NativeResponse.Headers);
			uint result;
			try
			{
				if (pDataChunk != null)
				{
					this.m_NativeResponse.EntityChunkCount = 1;
					this.m_NativeResponse.pEntityChunks = pDataChunk;
				}
				else if (asyncResult != null && asyncResult.pDataChunks != null)
				{
					this.m_NativeResponse.EntityChunkCount = asyncResult.dataChunkCount;
					this.m_NativeResponse.pEntityChunks = asyncResult.pDataChunks;
				}
				if (this.StatusDescription.Length > 0)
				{
					byte[] array = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(this.StatusDescription)];
					try
					{
						fixed (byte* ptr = array)
						{
							this.m_NativeResponse.ReasonLength = (ushort)array.Length;
							WebHeaderCollection.HeaderEncoding.GetBytes(this.StatusDescription, 0, array.Length, array, 0);
							this.m_NativeResponse.pReason = (sbyte*)ptr;
							try
							{
								fixed (UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr2 = &this.m_NativeResponse)
								{
									if (asyncResult != null)
									{
										this.HttpListenerContext.EnsureBoundHandle();
									}
									result = UnsafeNclNativeMethods.HttpApi.HttpSendHttpResponse(this.HttpListenerContext.RequestQueueHandle, this.HttpListenerRequest.RequestId, (uint)flags, ptr2, null, null, SafeLocalFree.Zero, 0U, (asyncResult == null) ? null : asyncResult.m_pOverlapped, null);
								}
							}
							finally
							{
								UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr2 = null;
							}
							goto IL_216;
						}
					}
					finally
					{
						byte* ptr = null;
					}
				}
				try
				{
					fixed (UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr3 = &this.m_NativeResponse)
					{
						if (asyncResult != null)
						{
							this.HttpListenerContext.EnsureBoundHandle();
						}
						result = UnsafeNclNativeMethods.HttpApi.HttpSendHttpResponse(this.HttpListenerContext.RequestQueueHandle, this.HttpListenerRequest.RequestId, (uint)flags, ptr3, null, null, SafeLocalFree.Zero, 0U, (asyncResult == null) ? null : asyncResult.m_pOverlapped, null);
					}
				}
				finally
				{
					UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE* ptr3 = null;
				}
				IL_216:;
			}
			finally
			{
				this.FreePinnedHeaders(pinnedHeaders);
			}
			return result;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x00079B3C File Offset: 0x00078B3C
		internal void ComputeCookies()
		{
			if (this.m_Cookies != null)
			{
				string text = null;
				string text2 = null;
				for (int i = 0; i < this.m_Cookies.Count; i++)
				{
					Cookie cookie = this.m_Cookies[i];
					string text3 = cookie.ToServerString();
					if (text3 != null && text3.Length != 0)
					{
						if (cookie.Variant == CookieVariant.Rfc2965 || (this.HttpListenerContext.PromoteCookiesToRfc2965 && cookie.Variant == CookieVariant.Rfc2109))
						{
							text = ((text == null) ? text3 : (text + ", " + text3));
						}
						else
						{
							text2 = ((text2 == null) ? text3 : (text2 + ", " + text3));
						}
					}
				}
				if (!string.IsNullOrEmpty(text2))
				{
					this.Headers.Set(HttpResponseHeader.SetCookie, text2);
					if (string.IsNullOrEmpty(text))
					{
						this.Headers.Remove("Set-Cookie2");
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.Headers.Set("Set-Cookie2", text);
					if (string.IsNullOrEmpty(text2))
					{
						this.Headers.Remove("Set-Cookie");
					}
				}
			}
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x00079C3C File Offset: 0x00078C3C
		internal UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS ComputeHeaders()
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			this.m_ResponseState = HttpListenerResponse.ResponseState.ComputedHeaders;
			if (this.HttpListenerContext.MutualAuthentication != null && this.HttpListenerContext.MutualAuthentication.Length > 0)
			{
				this.Headers.SetInternal(HttpResponseHeader.WwwAuthenticate, this.HttpListenerContext.MutualAuthentication);
			}
			this.ComputeCookies();
			if (this.m_BoundaryType == BoundaryType.None)
			{
				this.m_ContentLength = -1L;
				if (this.HttpListenerRequest.ProtocolVersion.Minor == 0)
				{
					this.m_KeepAlive = false;
				}
				else
				{
					this.m_BoundaryType = BoundaryType.Chunked;
				}
			}
			if (this.m_BoundaryType == BoundaryType.ContentLength)
			{
				this.Headers.SetInternal(HttpResponseHeader.ContentLength, this.m_ContentLength.ToString("D", NumberFormatInfo.InvariantInfo));
				if (this.m_ContentLength == 0L)
				{
					http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
				}
			}
			else if (this.m_BoundaryType == BoundaryType.Chunked)
			{
				this.Headers.SetInternal(HttpResponseHeader.TransferEncoding, "chunked");
			}
			else if (this.m_BoundaryType == BoundaryType.None)
			{
				http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			}
			else
			{
				this.m_KeepAlive = false;
			}
			if (!this.m_KeepAlive)
			{
				this.Headers.Add(HttpResponseHeader.Connection, "close");
				if (http_FLAGS == UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE)
				{
					http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_RECEIVE_REQUEST_FLAG_COPY_BODY;
				}
			}
			else if (this.HttpListenerRequest.ProtocolVersion.Minor == 0)
			{
				this.Headers.SetInternal(HttpResponseHeader.KeepAlive, "true");
			}
			return http_FLAGS;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x00079D70 File Offset: 0x00078D70
		private unsafe List<GCHandle> SerializeHeaders(ref UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADERS headers)
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[] array = null;
			if (this.Headers.Count == 0)
			{
				return null;
			}
			List<GCHandle> list = new List<GCHandle>();
			int num = 0;
			for (int i = 0; i < this.Headers.Count; i++)
			{
				string key = this.Headers.GetKey(i);
				int num2 = UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.IndexOfKnownHeader(key);
				if (num2 == 27)
				{
					num2 = -1;
				}
				if (num2 == -1)
				{
					string[] values = this.Headers.GetValues(i);
					num += values.Length;
				}
			}
			try
			{
				try
				{
					fixed (UnsafeNclNativeMethods.HttpApi.HTTP_KNOWN_HEADER* ptr = &headers.KnownHeaders)
					{
						for (int j = 0; j < this.Headers.Count; j++)
						{
							string key = this.Headers.GetKey(j);
							string text = this.Headers.Get(j);
							int num2 = UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.IndexOfKnownHeader(key);
							if (num2 == 27)
							{
								num2 = -1;
							}
							if (num2 == -1)
							{
								if (array == null)
								{
									array = new UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER[num];
									GCHandle item = GCHandle.Alloc(array, GCHandleType.Pinned);
									list.Add(item);
									headers.pUnknownHeaders = (UnsafeNclNativeMethods.HttpApi.HTTP_UNKNOWN_HEADER*)((void*)item.AddrOfPinnedObject());
								}
								string[] values2 = this.Headers.GetValues(j);
								for (int k = 0; k < values2.Length; k++)
								{
									byte[] array2 = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(key)];
									array[(int)headers.UnknownHeaderCount].NameLength = (ushort)array2.Length;
									WebHeaderCollection.HeaderEncoding.GetBytes(key, 0, array2.Length, array2, 0);
									GCHandle item = GCHandle.Alloc(array2, GCHandleType.Pinned);
									list.Add(item);
									array[(int)headers.UnknownHeaderCount].pName = (sbyte*)((void*)item.AddrOfPinnedObject());
									text = values2[k];
									array2 = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(text)];
									array[(int)headers.UnknownHeaderCount].RawValueLength = (ushort)array2.Length;
									WebHeaderCollection.HeaderEncoding.GetBytes(text, 0, array2.Length, array2, 0);
									item = GCHandle.Alloc(array2, GCHandleType.Pinned);
									list.Add(item);
									array[(int)headers.UnknownHeaderCount].pRawValue = (sbyte*)((void*)item.AddrOfPinnedObject());
									headers.UnknownHeaderCount += 1;
								}
							}
							else if (text != null)
							{
								byte[] array2 = new byte[WebHeaderCollection.HeaderEncoding.GetByteCount(text)];
								ptr[num2].RawValueLength = (ushort)array2.Length;
								WebHeaderCollection.HeaderEncoding.GetBytes(text, 0, array2.Length, array2, 0);
								GCHandle item = GCHandle.Alloc(array2, GCHandleType.Pinned);
								list.Add(item);
								ptr[num2].pRawValue = (sbyte*)((void*)item.AddrOfPinnedObject());
							}
						}
					}
				}
				finally
				{
					UnsafeNclNativeMethods.HttpApi.HTTP_KNOWN_HEADER* ptr = null;
				}
			}
			catch
			{
				this.FreePinnedHeaders(list);
				throw;
			}
			return list;
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0007A028 File Offset: 0x00079028
		private void FreePinnedHeaders(List<GCHandle> pinnedHeaders)
		{
			if (pinnedHeaders != null)
			{
				foreach (GCHandle gchandle in pinnedHeaders)
				{
					if (gchandle.IsAllocated)
					{
						gchandle.Free();
					}
				}
			}
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0007A084 File Offset: 0x00079084
		private void CheckDisposed()
		{
			if (this.m_ResponseState >= HttpListenerResponse.ResponseState.Closed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04001EA9 RID: 7849
		private static readonly string[][] s_HTTPStatusDescriptions = new string[][]
		{
			null,
			new string[]
			{
				"Continue",
				"Switching Protocols",
				"Processing"
			},
			new string[]
			{
				"OK",
				"Created",
				"Accepted",
				"Non-Authoritative Information",
				"No Content",
				"Reset Content",
				"Partial Content",
				"Multi-Status"
			},
			new string[]
			{
				"Multiple Choices",
				"Moved Permanently",
				"Found",
				"See Other",
				"Not Modified",
				"Use Proxy",
				null,
				"Temporary Redirect"
			},
			new string[]
			{
				"Bad Request",
				"Unauthorized",
				"Payment Required",
				"Forbidden",
				"Not Found",
				"Method Not Allowed",
				"Not Acceptable",
				"Proxy Authentication Required",
				"Request Timeout",
				"Conflict",
				"Gone",
				"Length Required",
				"Precondition Failed",
				"Request Entity Too Large",
				"Request-Uri Too Long",
				"Unsupported Media Type",
				"Requested Range Not Satisfiable",
				"Expectation Failed",
				null,
				null,
				null,
				null,
				"Unprocessable Entity",
				"Locked",
				"Failed Dependency"
			},
			new string[]
			{
				"Internal Server Error",
				"Not Implemented",
				"Bad Gateway",
				"Service Unavailable",
				"Gateway Timeout",
				"Http Version Not Supported",
				null,
				"Insufficient Storage"
			}
		};

		// Token: 0x04001EAA RID: 7850
		private Encoding m_ContentEncoding;

		// Token: 0x04001EAB RID: 7851
		private CookieCollection m_Cookies;

		// Token: 0x04001EAC RID: 7852
		private string m_StatusDescription;

		// Token: 0x04001EAD RID: 7853
		private bool m_KeepAlive;

		// Token: 0x04001EAE RID: 7854
		private HttpListenerResponse.ResponseState m_ResponseState;

		// Token: 0x04001EAF RID: 7855
		private WebHeaderCollection m_WebHeaders;

		// Token: 0x04001EB0 RID: 7856
		private HttpResponseStream m_ResponseStream;

		// Token: 0x04001EB1 RID: 7857
		private long m_ContentLength;

		// Token: 0x04001EB2 RID: 7858
		private BoundaryType m_BoundaryType;

		// Token: 0x04001EB3 RID: 7859
		private UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE m_NativeResponse;

		// Token: 0x04001EB4 RID: 7860
		private HttpListenerContext m_HttpContext;

		// Token: 0x020003D9 RID: 985
		private enum ResponseState
		{
			// Token: 0x04001EB6 RID: 7862
			Created,
			// Token: 0x04001EB7 RID: 7863
			ComputedHeaders,
			// Token: 0x04001EB8 RID: 7864
			SentHeaders,
			// Token: 0x04001EB9 RID: 7865
			Closed
		}
	}
}
