using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004CB RID: 1227
	internal class ConnectionGroup
	{
		// Token: 0x060025ED RID: 9709 RVA: 0x00099044 File Offset: 0x00098044
		internal ConnectionGroup(ServicePoint servicePoint, string connName)
		{
			this.m_ServicePoint = servicePoint;
			this.m_ConnectionLimit = servicePoint.ConnectionLimit;
			this.m_ConnectionList = new ArrayList(3);
			this.m_Name = ConnectionGroup.MakeQueryStr(connName);
			this.m_AbortDelegate = new HttpAbortDelegate(this.Abort);
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060025EE RID: 9710 RVA: 0x0009909B File Offset: 0x0009809B
		internal ServicePoint ServicePoint
		{
			get
			{
				return this.m_ServicePoint;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060025EF RID: 9711 RVA: 0x000990A3 File Offset: 0x000980A3
		internal int CurrentConnections
		{
			get
			{
				return this.m_ConnectionList.Count;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x060025F0 RID: 9712 RVA: 0x000990B0 File Offset: 0x000980B0
		// (set) Token: 0x060025F1 RID: 9713 RVA: 0x000990B8 File Offset: 0x000980B8
		internal int ConnectionLimit
		{
			get
			{
				return this.m_ConnectionLimit;
			}
			set
			{
				this.m_ConnectionLimit = value;
				this.PruneExcesiveConnections();
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x000990C8 File Offset: 0x000980C8
		private ManualResetEvent AsyncWaitHandle
		{
			get
			{
				if (this.m_Event == null)
				{
					Interlocked.CompareExchange(ref this.m_Event, new ManualResetEvent(false), null);
				}
				return (ManualResetEvent)this.m_Event;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x060025F3 RID: 9715 RVA: 0x00099100 File Offset: 0x00098100
		// (set) Token: 0x060025F4 RID: 9716 RVA: 0x00099154 File Offset: 0x00098154
		private Queue AuthenticationRequestQueue
		{
			get
			{
				if (this.m_AuthenticationRequestQueue == null)
				{
					lock (this.m_ConnectionList)
					{
						if (this.m_AuthenticationRequestQueue == null)
						{
							this.m_AuthenticationRequestQueue = new Queue();
						}
					}
				}
				return this.m_AuthenticationRequestQueue;
			}
			set
			{
				this.m_AuthenticationRequestQueue = value;
			}
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0009915D File Offset: 0x0009815D
		internal static string MakeQueryStr(string connName)
		{
			if (connName != null)
			{
				return connName;
			}
			return "";
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0009916C File Offset: 0x0009816C
		internal void Associate(Connection connection)
		{
			lock (this.m_ConnectionList)
			{
				this.m_ConnectionList.Add(connection);
			}
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000991AC File Offset: 0x000981AC
		internal void Disassociate(Connection connection)
		{
			lock (this.m_ConnectionList)
			{
				this.m_ConnectionList.Remove(connection);
			}
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000991EC File Offset: 0x000981EC
		internal void ConnectionGoneIdle()
		{
			if (this.m_AuthenticationGroup)
			{
				lock (this.m_ConnectionList)
				{
					this.AsyncWaitHandle.Set();
				}
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x00099234 File Offset: 0x00098234
		private bool Abort(HttpWebRequest request, WebException webException)
		{
			lock (this.m_ConnectionList)
			{
				this.AsyncWaitHandle.Set();
			}
			return true;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x00099274 File Offset: 0x00098274
		private void PruneAbortedRequests()
		{
			lock (this.m_ConnectionList)
			{
				Queue queue = new Queue();
				foreach (object obj in this.AuthenticationRequestQueue)
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)obj;
					if (!httpWebRequest.Aborted)
					{
						queue.Enqueue(httpWebRequest);
					}
				}
				this.AuthenticationRequestQueue = queue;
			}
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x00099308 File Offset: 0x00098308
		private void PruneExcesiveConnections()
		{
			ArrayList arrayList = new ArrayList();
			lock (this.m_ConnectionList)
			{
				int connectionLimit = this.ConnectionLimit;
				if (this.CurrentConnections > connectionLimit)
				{
					int num = this.CurrentConnections - connectionLimit;
					for (int i = 0; i < num; i++)
					{
						arrayList.Add(this.m_ConnectionList[i]);
					}
					this.m_ConnectionList.RemoveRange(0, num);
				}
			}
			foreach (object obj in arrayList)
			{
				Connection connection = (Connection)obj;
				connection.CloseOnIdle();
			}
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000993D4 File Offset: 0x000983D4
		internal void DisableKeepAliveOnConnections()
		{
			ArrayList arrayList = new ArrayList();
			lock (this.m_ConnectionList)
			{
				foreach (object obj in this.m_ConnectionList)
				{
					Connection value = (Connection)obj;
					arrayList.Add(value);
				}
				this.m_ConnectionList.Clear();
			}
			foreach (object obj2 in arrayList)
			{
				Connection connection = (Connection)obj2;
				connection.CloseOnIdle();
			}
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000994B4 File Offset: 0x000984B4
		private Connection FindMatchingConnection(HttpWebRequest request, string connName, out Connection leastbusyConnection)
		{
			bool flag = false;
			leastbusyConnection = null;
			lock (this.m_ConnectionList)
			{
				int num = int.MaxValue;
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection = (Connection)obj;
					if (connection.LockedRequest == request)
					{
						leastbusyConnection = connection;
						return connection;
					}
					if (connection.BusyCount < num && connection.LockedRequest == null)
					{
						leastbusyConnection = connection;
						num = connection.BusyCount;
						if (num == 0)
						{
							flag = true;
						}
					}
				}
				if (!flag && this.CurrentConnections < this.ConnectionLimit)
				{
					leastbusyConnection = new Connection(this);
				}
			}
			return null;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x00099590 File Offset: 0x00098590
		private Connection FindConnectionAuthenticationGroup(HttpWebRequest request, string connName)
		{
			Connection connection = null;
			lock (this.m_ConnectionList)
			{
				Connection connection2 = this.FindMatchingConnection(request, connName, out connection);
				if (connection2 != null)
				{
					connection2.MarkAsReserved();
					return connection2;
				}
				if (this.AuthenticationRequestQueue.Count == 0)
				{
					if (connection != null)
					{
						if (request.LockConnection)
						{
							this.m_NtlmNegGroup = true;
							this.m_IISVersion = connection.IISVersion;
						}
						if (request.LockConnection || (this.m_NtlmNegGroup && !request.Pipelined && request.UnsafeOrProxyAuthenticatedConnectionSharing && this.m_IISVersion >= 6))
						{
							connection.LockedRequest = request;
						}
						connection.MarkAsReserved();
						return connection;
					}
				}
				else if (connection != null)
				{
					this.AsyncWaitHandle.Set();
				}
				this.AuthenticationRequestQueue.Enqueue(request);
			}
			Connection result;
			for (;;)
			{
				request.AbortDelegate = this.m_AbortDelegate;
				if (!request.Aborted)
				{
					this.AsyncWaitHandle.WaitOne();
				}
				lock (this.m_ConnectionList)
				{
					if (!request.Aborted)
					{
						this.FindMatchingConnection(request, connName, out connection);
						if (this.AuthenticationRequestQueue.Peek() == request)
						{
							this.AuthenticationRequestQueue.Dequeue();
							if (connection != null)
							{
								if (request.LockConnection)
								{
									this.m_NtlmNegGroup = true;
									this.m_IISVersion = connection.IISVersion;
								}
								if (request.LockConnection || (this.m_NtlmNegGroup && !request.Pipelined && request.UnsafeOrProxyAuthenticatedConnectionSharing && this.m_IISVersion >= 6))
								{
									connection.LockedRequest = request;
								}
								connection.MarkAsReserved();
								result = connection;
								break;
							}
							this.AuthenticationRequestQueue.Enqueue(request);
						}
						if (connection == null)
						{
							this.AsyncWaitHandle.Reset();
						}
						continue;
					}
					this.PruneAbortedRequests();
					result = null;
				}
				break;
			}
			return result;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x00099758 File Offset: 0x00098758
		internal Connection FindConnection(HttpWebRequest request, string connName)
		{
			Connection connection = null;
			Connection connection2 = null;
			bool flag = false;
			if (this.m_AuthenticationGroup || request.LockConnection)
			{
				this.m_AuthenticationGroup = true;
				return this.FindConnectionAuthenticationGroup(request, connName);
			}
			lock (this.m_ConnectionList)
			{
				int num = int.MaxValue;
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection3 = (Connection)obj;
					if (connection3.BusyCount < num)
					{
						connection = connection3;
						num = connection3.BusyCount;
						if (num == 0)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && this.CurrentConnections < this.ConnectionLimit)
				{
					connection2 = new Connection(this);
				}
				else
				{
					connection2 = connection;
				}
				connection2.MarkAsReserved();
			}
			return connection2;
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x00099844 File Offset: 0x00098844
		[Conditional("DEBUG")]
		internal void Debug(int requestHash)
		{
			foreach (object obj in this.m_ConnectionList)
			{
				Connection connection = (Connection)obj;
			}
		}

		// Token: 0x040025AD RID: 9645
		private const int DefaultConnectionListSize = 3;

		// Token: 0x040025AE RID: 9646
		private ServicePoint m_ServicePoint;

		// Token: 0x040025AF RID: 9647
		private string m_Name;

		// Token: 0x040025B0 RID: 9648
		private int m_ConnectionLimit;

		// Token: 0x040025B1 RID: 9649
		private ArrayList m_ConnectionList;

		// Token: 0x040025B2 RID: 9650
		private object m_Event;

		// Token: 0x040025B3 RID: 9651
		private Queue m_AuthenticationRequestQueue;

		// Token: 0x040025B4 RID: 9652
		internal bool m_AuthenticationGroup;

		// Token: 0x040025B5 RID: 9653
		private HttpAbortDelegate m_AbortDelegate;

		// Token: 0x040025B6 RID: 9654
		private bool m_NtlmNegGroup;

		// Token: 0x040025B7 RID: 9655
		private int m_IISVersion = -1;
	}
}
