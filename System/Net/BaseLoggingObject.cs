using System;

namespace System.Net
{
	// Token: 0x020004E8 RID: 1256
	internal class BaseLoggingObject
	{
		// Token: 0x0600271C RID: 10012 RVA: 0x000A2140 File Offset: 0x000A1140
		internal BaseLoggingObject()
		{
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000A2148 File Offset: 0x000A1148
		internal virtual void EnterFunc(string funcname)
		{
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000A214A File Offset: 0x000A114A
		internal virtual void LeaveFunc(string funcname)
		{
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000A214C File Offset: 0x000A114C
		internal virtual void DumpArrayToConsole()
		{
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000A214E File Offset: 0x000A114E
		internal virtual void PrintLine(string msg)
		{
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000A2150 File Offset: 0x000A1150
		internal virtual void DumpArray(bool shouldClose)
		{
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000A2152 File Offset: 0x000A1152
		internal virtual void DumpArrayToFile(bool shouldClose)
		{
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000A2154 File Offset: 0x000A1154
		internal virtual void Flush()
		{
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000A2156 File Offset: 0x000A1156
		internal virtual void Flush(bool close)
		{
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000A2158 File Offset: 0x000A1158
		internal virtual void LoggingMonitorTick()
		{
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000A215A File Offset: 0x000A115A
		internal virtual void Dump(byte[] buffer)
		{
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000A215C File Offset: 0x000A115C
		internal virtual void Dump(byte[] buffer, int length)
		{
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000A215E File Offset: 0x000A115E
		internal virtual void Dump(byte[] buffer, int offset, int length)
		{
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000A2160 File Offset: 0x000A1160
		internal virtual void Dump(IntPtr pBuffer, int offset, int length)
		{
		}
	}
}
