using System;
using System.Collections;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200064A RID: 1610
	internal sealed class ConnectionManagementSectionInternal
	{
		// Token: 0x060031D9 RID: 12761 RVA: 0x000D4BC8 File Offset: 0x000D3BC8
		internal ConnectionManagementSectionInternal(ConnectionManagementSection section)
		{
			if (section.ConnectionManagement.Count > 0)
			{
				this.connectionManagement = new Hashtable(section.ConnectionManagement.Count);
				foreach (object obj in section.ConnectionManagement)
				{
					ConnectionManagementElement connectionManagementElement = (ConnectionManagementElement)obj;
					this.connectionManagement[connectionManagementElement.Address] = connectionManagementElement.MaxConnection;
				}
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x000D4C60 File Offset: 0x000D3C60
		internal Hashtable ConnectionManagement
		{
			get
			{
				Hashtable hashtable = this.connectionManagement;
				if (hashtable == null)
				{
					hashtable = new Hashtable();
				}
				return hashtable;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x060031DB RID: 12763 RVA: 0x000D4C80 File Offset: 0x000D3C80
		internal static object ClassSyncObject
		{
			get
			{
				if (ConnectionManagementSectionInternal.classSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref ConnectionManagementSectionInternal.classSyncObject, value, null);
				}
				return ConnectionManagementSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000D4CAC File Offset: 0x000D3CAC
		internal static ConnectionManagementSectionInternal GetSection()
		{
			ConnectionManagementSectionInternal result;
			lock (ConnectionManagementSectionInternal.ClassSyncObject)
			{
				ConnectionManagementSection connectionManagementSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.ConnectionManagementSectionPath) as ConnectionManagementSection;
				if (connectionManagementSection == null)
				{
					result = null;
				}
				else
				{
					result = new ConnectionManagementSectionInternal(connectionManagementSection);
				}
			}
			return result;
		}

		// Token: 0x04002EE3 RID: 12003
		private Hashtable connectionManagement;

		// Token: 0x04002EE4 RID: 12004
		private static object classSyncObject;
	}
}
