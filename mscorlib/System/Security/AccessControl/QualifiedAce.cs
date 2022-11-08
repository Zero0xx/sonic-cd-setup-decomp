using System;
using System.Globalization;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008D0 RID: 2256
	public abstract class QualifiedAce : KnownAce
	{
		// Token: 0x060051FB RID: 20987 RVA: 0x00126E88 File Offset: 0x00125E88
		private AceQualifier QualifierFromType(AceType type, out bool isCallback)
		{
			switch (type)
			{
			case AceType.AccessAllowed:
				isCallback = false;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDenied:
				isCallback = false;
				return AceQualifier.AccessDenied;
			case AceType.SystemAudit:
				isCallback = false;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarm:
				isCallback = false;
				return AceQualifier.SystemAlarm;
			case AceType.AccessAllowedObject:
				isCallback = false;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDeniedObject:
				isCallback = false;
				return AceQualifier.AccessDenied;
			case AceType.SystemAuditObject:
				isCallback = false;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarmObject:
				isCallback = false;
				return AceQualifier.SystemAlarm;
			case AceType.AccessAllowedCallback:
				isCallback = true;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDeniedCallback:
				isCallback = true;
				return AceQualifier.AccessDenied;
			case AceType.AccessAllowedCallbackObject:
				isCallback = true;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDeniedCallbackObject:
				isCallback = true;
				return AceQualifier.AccessDenied;
			case AceType.SystemAuditCallback:
				isCallback = true;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarmCallback:
				isCallback = true;
				return AceQualifier.SystemAlarm;
			case AceType.SystemAuditCallbackObject:
				isCallback = true;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarmCallbackObject:
				isCallback = true;
				return AceQualifier.SystemAlarm;
			}
			throw new SystemException();
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x00126F38 File Offset: 0x00125F38
		internal QualifiedAce(AceType type, AceFlags flags, int accessMask, SecurityIdentifier sid, byte[] opaque) : base(type, flags, accessMask, sid)
		{
			this._qualifier = this.QualifierFromType(type, out this._isCallback);
			this.SetOpaque(opaque);
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x00126F60 File Offset: 0x00125F60
		public AceQualifier AceQualifier
		{
			get
			{
				return this._qualifier;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x060051FE RID: 20990 RVA: 0x00126F68 File Offset: 0x00125F68
		public bool IsCallback
		{
			get
			{
				return this._isCallback;
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x060051FF RID: 20991
		internal abstract int MaxOpaqueLengthInternal { get; }

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06005200 RID: 20992 RVA: 0x00126F70 File Offset: 0x00125F70
		public int OpaqueLength
		{
			get
			{
				if (this._opaque != null)
				{
					return this._opaque.Length;
				}
				return 0;
			}
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x00126F84 File Offset: 0x00125F84
		public byte[] GetOpaque()
		{
			return this._opaque;
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x00126F8C File Offset: 0x00125F8C
		public void SetOpaque(byte[] opaque)
		{
			if (opaque != null)
			{
				if (opaque.Length > this.MaxOpaqueLengthInternal)
				{
					throw new ArgumentOutOfRangeException("opaque", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ArrayLength"), new object[]
					{
						0,
						this.MaxOpaqueLengthInternal
					}));
				}
				if (opaque.Length % 4 != 0)
				{
					throw new ArgumentOutOfRangeException("opaque", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ArrayLengthMultiple"), new object[]
					{
						4
					}));
				}
			}
			this._opaque = opaque;
		}

		// Token: 0x04002A6A RID: 10858
		private readonly bool _isCallback;

		// Token: 0x04002A6B RID: 10859
		private readonly AceQualifier _qualifier;

		// Token: 0x04002A6C RID: 10860
		private byte[] _opaque;
	}
}
