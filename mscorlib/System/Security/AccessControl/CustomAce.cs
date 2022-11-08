using System;
using System.Globalization;

namespace System.Security.AccessControl
{
	// Token: 0x020008CC RID: 2252
	public sealed class CustomAce : GenericAce
	{
		// Token: 0x060051EE RID: 20974 RVA: 0x00126C14 File Offset: 0x00125C14
		public CustomAce(AceType type, AceFlags flags, byte[] opaque) : base(type, flags)
		{
			if (type <= AceType.SystemAlarmCallbackObject)
			{
				throw new ArgumentOutOfRangeException("type", Environment.GetResourceString("ArgumentOutOfRange_InvalidUserDefinedAceType"));
			}
			this.SetOpaque(opaque);
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x060051EF RID: 20975 RVA: 0x00126C3F File Offset: 0x00125C3F
		public int OpaqueLength
		{
			get
			{
				if (this._opaque == null)
				{
					return 0;
				}
				return this._opaque.Length;
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x060051F0 RID: 20976 RVA: 0x00126C53 File Offset: 0x00125C53
		public override int BinaryLength
		{
			get
			{
				return 4 + this.OpaqueLength;
			}
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x00126C5D File Offset: 0x00125C5D
		public byte[] GetOpaque()
		{
			return this._opaque;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x00126C68 File Offset: 0x00125C68
		public void SetOpaque(byte[] opaque)
		{
			if (opaque != null)
			{
				if (opaque.Length > CustomAce.MaxOpaqueLength)
				{
					throw new ArgumentOutOfRangeException("opaque", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ArrayLength"), new object[]
					{
						0,
						CustomAce.MaxOpaqueLength
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

		// Token: 0x060051F3 RID: 20979 RVA: 0x00126CFD File Offset: 0x00125CFD
		public override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			base.MarshalHeader(binaryForm, offset);
			offset += 4;
			if (this.OpaqueLength != 0)
			{
				if (this.OpaqueLength > CustomAce.MaxOpaqueLength)
				{
					throw new SystemException();
				}
				this.GetOpaque().CopyTo(binaryForm, offset);
			}
		}

		// Token: 0x04002A5F RID: 10847
		private byte[] _opaque;

		// Token: 0x04002A60 RID: 10848
		public static readonly int MaxOpaqueLength = 65531;
	}
}
