using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008C8 RID: 2248
	public enum AceType : byte
	{
		// Token: 0x04002A3B RID: 10811
		AccessAllowed,
		// Token: 0x04002A3C RID: 10812
		AccessDenied,
		// Token: 0x04002A3D RID: 10813
		SystemAudit,
		// Token: 0x04002A3E RID: 10814
		SystemAlarm,
		// Token: 0x04002A3F RID: 10815
		AccessAllowedCompound,
		// Token: 0x04002A40 RID: 10816
		AccessAllowedObject,
		// Token: 0x04002A41 RID: 10817
		AccessDeniedObject,
		// Token: 0x04002A42 RID: 10818
		SystemAuditObject,
		// Token: 0x04002A43 RID: 10819
		SystemAlarmObject,
		// Token: 0x04002A44 RID: 10820
		AccessAllowedCallback,
		// Token: 0x04002A45 RID: 10821
		AccessDeniedCallback,
		// Token: 0x04002A46 RID: 10822
		AccessAllowedCallbackObject,
		// Token: 0x04002A47 RID: 10823
		AccessDeniedCallbackObject,
		// Token: 0x04002A48 RID: 10824
		SystemAuditCallback,
		// Token: 0x04002A49 RID: 10825
		SystemAlarmCallback,
		// Token: 0x04002A4A RID: 10826
		SystemAuditCallbackObject,
		// Token: 0x04002A4B RID: 10827
		SystemAlarmCallbackObject,
		// Token: 0x04002A4C RID: 10828
		MaxDefinedAceType = 16
	}
}
