using System;
using System.Net.Security;

namespace System.Net
{
	// Token: 0x020004AC RID: 1196
	internal class AuthenticationState
	{
		// Token: 0x060024D4 RID: 9428 RVA: 0x00091D2C File Offset: 0x00090D2C
		internal NTAuthentication GetSecurityContext(IAuthenticationModule module)
		{
			if (module != this.Module)
			{
				return null;
			}
			return this.SecurityContext;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00091D3F File Offset: 0x00090D3F
		internal void SetSecurityContext(NTAuthentication securityContext, IAuthenticationModule module)
		{
			this.SecurityContext = securityContext;
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x00091D48 File Offset: 0x00090D48
		// (set) Token: 0x060024D7 RID: 9431 RVA: 0x00091D50 File Offset: 0x00090D50
		internal TransportContext TransportContext
		{
			get
			{
				return this._TransportContext;
			}
			set
			{
				this._TransportContext = value;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060024D8 RID: 9432 RVA: 0x00091D59 File Offset: 0x00090D59
		internal HttpResponseHeader AuthenticateHeader
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return HttpResponseHeader.WwwAuthenticate;
				}
				return HttpResponseHeader.ProxyAuthenticate;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060024D9 RID: 9433 RVA: 0x00091D68 File Offset: 0x00090D68
		internal string AuthorizationHeader
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return "Authorization";
				}
				return "Proxy-Authorization";
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x00091D7D File Offset: 0x00090D7D
		internal HttpStatusCode StatusCodeMatch
		{
			get
			{
				if (!this.IsProxyAuth)
				{
					return HttpStatusCode.Unauthorized;
				}
				return HttpStatusCode.ProxyAuthenticationRequired;
			}
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x00091D92 File Offset: 0x00090D92
		internal AuthenticationState(bool isProxyAuth)
		{
			this.IsProxyAuth = isProxyAuth;
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x00091DA4 File Offset: 0x00090DA4
		private void PrepareState(HttpWebRequest httpWebRequest)
		{
			Uri uri = this.IsProxyAuth ? httpWebRequest.ServicePoint.InternalAddress : httpWebRequest.Address;
			if (this.ChallengedUri != uri)
			{
				if (this.ChallengedUri == null || this.ChallengedUri.Scheme != uri.Scheme || this.ChallengedUri.Host != uri.Host || this.ChallengedUri.Port != uri.Port)
				{
					this.ChallengedSpn = null;
				}
				this.ChallengedUri = uri;
			}
			httpWebRequest.CurrentAuthenticationState = this;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x00091E34 File Offset: 0x00090E34
		internal string GetComputeSpn(HttpWebRequest httpWebRequest)
		{
			if (this.ChallengedSpn != null)
			{
				return this.ChallengedSpn;
			}
			string canonicalKey = httpWebRequest.ChallengedUri.GetParts(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.SafeUnescaped);
			string text = AuthenticationManager.SpnDictionary.InternalGet(canonicalKey);
			if (text == null)
			{
				if (!this.IsProxyAuth && httpWebRequest.ServicePoint.InternalProxyServicePoint)
				{
					text = httpWebRequest.ChallengedUri.Host;
					if (httpWebRequest.ChallengedUri.HostNameType == UriHostNameType.IPv6 || httpWebRequest.ChallengedUri.HostNameType == UriHostNameType.IPv4 || text.IndexOf('.') != -1)
					{
						goto IL_9F;
					}
					try
					{
						text = Dns.InternalGetHostByName(text).HostName;
						goto IL_9F;
					}
					catch (Exception exception)
					{
						if (NclUtilities.IsFatal(exception))
						{
							throw;
						}
						goto IL_9F;
					}
				}
				text = httpWebRequest.ServicePoint.Hostname;
				IL_9F:
				text = "HTTP/" + text;
				canonicalKey = httpWebRequest.ChallengedUri.GetParts(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped) + "/";
				AuthenticationManager.SpnDictionary.InternalSet(canonicalKey, text);
			}
			return this.ChallengedSpn = text;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x00091F2C File Offset: 0x00090F2C
		internal void PreAuthIfNeeded(HttpWebRequest httpWebRequest, ICredentials authInfo)
		{
			if (!this.TriedPreAuth)
			{
				this.TriedPreAuth = true;
				if (authInfo != null)
				{
					this.PrepareState(httpWebRequest);
					try
					{
						Authorization authorization = AuthenticationManager.PreAuthenticate(httpWebRequest, authInfo);
						if (authorization != null && authorization.Message != null)
						{
							this.UniqueGroupId = authorization.ConnectionGroupId;
							httpWebRequest.Headers.Set(this.AuthorizationHeader, authorization.Message);
						}
					}
					catch (Exception)
					{
						this.ClearSession(httpWebRequest);
					}
					catch
					{
						this.ClearSession(httpWebRequest);
					}
				}
			}
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x00091FBC File Offset: 0x00090FBC
		internal bool AttemptAuthenticate(HttpWebRequest httpWebRequest, ICredentials authInfo)
		{
			if (this.Authorization != null && this.Authorization.Complete)
			{
				if (this.IsProxyAuth)
				{
					this.ClearAuthReq(httpWebRequest);
				}
				return false;
			}
			if (authInfo == null)
			{
				return false;
			}
			string text = httpWebRequest.AuthHeader(this.AuthenticateHeader);
			if (text == null)
			{
				if (!this.IsProxyAuth && this.Authorization != null && httpWebRequest.ProxyAuthenticationState.Authorization != null)
				{
					httpWebRequest.Headers.Set(this.AuthorizationHeader, this.Authorization.Message);
				}
				return false;
			}
			this.PrepareState(httpWebRequest);
			try
			{
				this.Authorization = AuthenticationManager.Authenticate(text, httpWebRequest, authInfo);
			}
			catch (Exception)
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			catch
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			if (this.Authorization == null)
			{
				return false;
			}
			if (this.Authorization.Message == null)
			{
				this.Authorization = null;
				return false;
			}
			this.UniqueGroupId = this.Authorization.ConnectionGroupId;
			try
			{
				httpWebRequest.Headers.Set(this.AuthorizationHeader, this.Authorization.Message);
			}
			catch
			{
				this.Authorization = null;
				this.ClearSession(httpWebRequest);
				throw;
			}
			return true;
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x00092104 File Offset: 0x00091104
		internal void ClearAuthReq(HttpWebRequest httpWebRequest)
		{
			this.TriedPreAuth = false;
			this.Authorization = null;
			this.UniqueGroupId = null;
			httpWebRequest.Headers.Remove(this.AuthorizationHeader);
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0009212C File Offset: 0x0009112C
		internal void Update(HttpWebRequest httpWebRequest)
		{
			if (this.Authorization != null)
			{
				this.PrepareState(httpWebRequest);
				ISessionAuthenticationModule sessionAuthenticationModule = this.Module as ISessionAuthenticationModule;
				if (sessionAuthenticationModule != null)
				{
					string challenge = httpWebRequest.AuthHeader(this.AuthenticateHeader);
					if (this.IsProxyAuth || httpWebRequest.ResponseStatusCode != HttpStatusCode.ProxyAuthenticationRequired)
					{
						bool complete = true;
						try
						{
							complete = sessionAuthenticationModule.Update(challenge, httpWebRequest);
						}
						catch (Exception)
						{
							this.ClearSession(httpWebRequest);
							if (httpWebRequest.AuthenticationLevel == AuthenticationLevel.MutualAuthRequired && (httpWebRequest.CurrentAuthenticationState == null || httpWebRequest.CurrentAuthenticationState.Authorization == null || !httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated))
							{
								throw;
							}
						}
						catch
						{
							this.ClearSession(httpWebRequest);
							if (httpWebRequest.AuthenticationLevel == AuthenticationLevel.MutualAuthRequired && (httpWebRequest.CurrentAuthenticationState == null || httpWebRequest.CurrentAuthenticationState.Authorization == null || !httpWebRequest.CurrentAuthenticationState.Authorization.MutuallyAuthenticated))
							{
								throw;
							}
						}
						this.Authorization.SetComplete(complete);
					}
				}
				if (this.Module != null && this.Authorization.Complete && this.Module.CanPreAuthenticate && httpWebRequest.ResponseStatusCode != this.StatusCodeMatch)
				{
					AuthenticationManager.BindModule(this.ChallengedUri, this.Authorization, this.Module);
				}
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x00092278 File Offset: 0x00091278
		internal void ClearSession()
		{
			if (this.SecurityContext != null)
			{
				this.SecurityContext.CloseContext();
				this.SecurityContext = null;
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x00092294 File Offset: 0x00091294
		internal void ClearSession(HttpWebRequest httpWebRequest)
		{
			this.PrepareState(httpWebRequest);
			ISessionAuthenticationModule sessionAuthenticationModule = this.Module as ISessionAuthenticationModule;
			this.Module = null;
			if (sessionAuthenticationModule != null)
			{
				try
				{
					sessionAuthenticationModule.ClearSession(httpWebRequest);
				}
				catch (Exception exception)
				{
					if (NclUtilities.IsFatal(exception))
					{
						throw;
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x040024D2 RID: 9426
		private bool TriedPreAuth;

		// Token: 0x040024D3 RID: 9427
		internal Authorization Authorization;

		// Token: 0x040024D4 RID: 9428
		internal IAuthenticationModule Module;

		// Token: 0x040024D5 RID: 9429
		internal string UniqueGroupId;

		// Token: 0x040024D6 RID: 9430
		private bool IsProxyAuth;

		// Token: 0x040024D7 RID: 9431
		internal Uri ChallengedUri;

		// Token: 0x040024D8 RID: 9432
		private string ChallengedSpn;

		// Token: 0x040024D9 RID: 9433
		private NTAuthentication SecurityContext;

		// Token: 0x040024DA RID: 9434
		private TransportContext _TransportContext;
	}
}
