using System;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003F0 RID: 1008
	internal static class ExceptionHelper
	{
		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x000809CE File Offset: 0x0007F9CE
		internal static NotImplementedException MethodNotImplementedException
		{
			get
			{
				return new NotImplementedException(SR.GetString("net_MethodNotImplementedException"));
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x000809DF File Offset: 0x0007F9DF
		internal static NotImplementedException PropertyNotImplementedException
		{
			get
			{
				return new NotImplementedException(SR.GetString("net_PropertyNotImplementedException"));
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x000809F0 File Offset: 0x0007F9F0
		internal static NotSupportedException MethodNotSupportedException
		{
			get
			{
				return new NotSupportedException(SR.GetString("net_MethodNotSupportedException"));
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x00080A01 File Offset: 0x0007FA01
		internal static NotSupportedException PropertyNotSupportedException
		{
			get
			{
				return new NotSupportedException(SR.GetString("net_PropertyNotSupportedException"));
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002095 RID: 8341 RVA: 0x00080A12 File Offset: 0x0007FA12
		internal static WebException IsolatedException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.KeepAliveFailure), WebExceptionStatus.KeepAliveFailure, WebExceptionInternalStatus.Isolated, null);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x00080A29 File Offset: 0x0007FA29
		internal static WebException RequestAbortedException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x00080A3C File Offset: 0x0007FA3C
		internal static UriFormatException BadSchemeException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadScheme"));
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x00080A4D File Offset: 0x0007FA4D
		internal static UriFormatException BadAuthorityException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadAuthority"));
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002099 RID: 8345 RVA: 0x00080A5E File Offset: 0x0007FA5E
		internal static UriFormatException EmptyUriException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_EmptyUri"));
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x00080A6F File Offset: 0x0007FA6F
		internal static UriFormatException SchemeLimitException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_SchemeLimit"));
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x0600209B RID: 8347 RVA: 0x00080A80 File Offset: 0x0007FA80
		internal static UriFormatException SizeLimitException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_SizeLimit"));
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600209C RID: 8348 RVA: 0x00080A91 File Offset: 0x0007FA91
		internal static UriFormatException MustRootedPathException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_MustRootedPath"));
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x0600209D RID: 8349 RVA: 0x00080AA2 File Offset: 0x0007FAA2
		internal static UriFormatException BadHostNameException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadHostName"));
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x0600209E RID: 8350 RVA: 0x00080AB3 File Offset: 0x0007FAB3
		internal static UriFormatException BadPortException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadPort"));
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x0600209F RID: 8351 RVA: 0x00080AC4 File Offset: 0x0007FAC4
		internal static UriFormatException BadAuthorityTerminatorException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadAuthorityTerminator"));
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x00080AD5 File Offset: 0x0007FAD5
		internal static UriFormatException BadFormatException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_BadFormat"));
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060020A1 RID: 8353 RVA: 0x00080AE6 File Offset: 0x0007FAE6
		internal static UriFormatException CannotCreateRelativeException
		{
			get
			{
				return new UriFormatException(SR.GetString("net_uri_CannotCreateRelative"));
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x00080AF7 File Offset: 0x0007FAF7
		internal static WebException CacheEntryNotFoundException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.CacheEntryNotFound), WebExceptionStatus.CacheEntryNotFound);
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x00080B0C File Offset: 0x0007FB0C
		internal static WebException RequestProhibitedByCachePolicyException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestProhibitedByCachePolicy), WebExceptionStatus.RequestProhibitedByCachePolicy);
			}
		}

		// Token: 0x04001FD3 RID: 8147
		internal static readonly KeyContainerPermission KeyContainerPermissionOpen = new KeyContainerPermission(KeyContainerPermissionFlags.Open);

		// Token: 0x04001FD4 RID: 8148
		internal static readonly WebPermission WebPermissionUnrestricted = new WebPermission(NetworkAccess.Connect);

		// Token: 0x04001FD5 RID: 8149
		internal static readonly SecurityPermission UnmanagedPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

		// Token: 0x04001FD6 RID: 8150
		internal static readonly SocketPermission UnrestrictedSocketPermission = new SocketPermission(PermissionState.Unrestricted);

		// Token: 0x04001FD7 RID: 8151
		internal static readonly SecurityPermission InfrastructurePermission = new SecurityPermission(SecurityPermissionFlag.Infrastructure);

		// Token: 0x04001FD8 RID: 8152
		internal static readonly SecurityPermission ControlPolicyPermission = new SecurityPermission(SecurityPermissionFlag.ControlPolicy);

		// Token: 0x04001FD9 RID: 8153
		internal static readonly SecurityPermission ControlPrincipalPermission = new SecurityPermission(SecurityPermissionFlag.ControlPrincipal);
	}
}
