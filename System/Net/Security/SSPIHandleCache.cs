using System;
using System.Threading;

namespace System.Net.Security
{
	// Token: 0x0200053D RID: 1341
	internal static class SSPIHandleCache
	{
		// Token: 0x060028F4 RID: 10484 RVA: 0x000AA56C File Offset: 0x000A956C
		internal static void CacheCredential(SafeFreeCredentials newHandle)
		{
			try
			{
				SafeCredentialReference safeCredentialReference = SafeCredentialReference.CreateReference(newHandle);
				if (safeCredentialReference != null)
				{
					int num = Interlocked.Increment(ref SSPIHandleCache._Current) & 31;
					safeCredentialReference = Interlocked.Exchange<SafeCredentialReference>(ref SSPIHandleCache._CacheSlots[num], safeCredentialReference);
					if (safeCredentialReference != null)
					{
						safeCredentialReference.Close();
					}
				}
			}
			catch (Exception exception)
			{
				NclUtilities.IsFatal(exception);
			}
		}

		// Token: 0x040027CA RID: 10186
		private const int c_MaxCacheSize = 31;

		// Token: 0x040027CB RID: 10187
		private static SafeCredentialReference[] _CacheSlots = new SafeCredentialReference[32];

		// Token: 0x040027CC RID: 10188
		private static int _Current = -1;
	}
}
