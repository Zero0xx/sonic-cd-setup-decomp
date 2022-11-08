using System;
using System.Collections;
using System.Globalization;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000387 RID: 903
	internal class ConnectionPoolManager
	{
		// Token: 0x06001C20 RID: 7200 RVA: 0x00069FD4 File Offset: 0x00068FD4
		private ConnectionPoolManager()
		{
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001C21 RID: 7201 RVA: 0x00069FDC File Offset: 0x00068FDC
		private static object InternalSyncObject
		{
			get
			{
				if (ConnectionPoolManager.s_InternalSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref ConnectionPoolManager.s_InternalSyncObject, value, null);
				}
				return ConnectionPoolManager.s_InternalSyncObject;
			}
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0006A008 File Offset: 0x00069008
		private static string GenerateKey(string hostName, int port, string groupName)
		{
			return string.Concat(new string[]
			{
				hostName,
				"\r",
				port.ToString(NumberFormatInfo.InvariantInfo),
				"\r",
				groupName
			});
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0006A04C File Offset: 0x0006904C
		internal static ConnectionPool GetConnectionPool(ServicePoint servicePoint, string groupName, CreateConnectionDelegate createConnectionCallback)
		{
			string key = ConnectionPoolManager.GenerateKey(servicePoint.Host, servicePoint.Port, groupName);
			ConnectionPool result;
			lock (ConnectionPoolManager.InternalSyncObject)
			{
				ConnectionPool connectionPool = (ConnectionPool)ConnectionPoolManager.m_ConnectionPools[key];
				if (connectionPool == null)
				{
					connectionPool = new ConnectionPool(servicePoint, servicePoint.ConnectionLimit, 0, servicePoint.MaxIdleTime, createConnectionCallback);
					ConnectionPoolManager.m_ConnectionPools[key] = connectionPool;
				}
				result = connectionPool;
			}
			return result;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0006A0CC File Offset: 0x000690CC
		internal static bool RemoveConnectionPool(ServicePoint servicePoint, string groupName)
		{
			string key = ConnectionPoolManager.GenerateKey(servicePoint.Host, servicePoint.Port, groupName);
			lock (ConnectionPoolManager.InternalSyncObject)
			{
				ConnectionPool connectionPool = (ConnectionPool)ConnectionPoolManager.m_ConnectionPools[key];
				if (connectionPool != null)
				{
					ConnectionPoolManager.m_ConnectionPools[key] = null;
					ConnectionPoolManager.m_ConnectionPools.Remove(key);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001CC3 RID: 7363
		private static Hashtable m_ConnectionPools = new Hashtable();

		// Token: 0x04001CC4 RID: 7364
		private static object s_InternalSyncObject;
	}
}
