using System;
using System.Collections;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x0200041A RID: 1050
	internal class PolicyWrapper
	{
		// Token: 0x060020D5 RID: 8405 RVA: 0x000811C0 File Offset: 0x000801C0
		internal PolicyWrapper(ICertificatePolicy policy, ServicePoint sp, WebRequest wr)
		{
			this.fwdPolicy = policy;
			this.srvPoint = sp;
			this.request = wr;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000811DD File Offset: 0x000801DD
		public bool Accept(X509Certificate Certificate, int CertificateProblem)
		{
			return this.fwdPolicy.CheckValidationResult(this.srvPoint, Certificate, this.request, CertificateProblem);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000811F8 File Offset: 0x000801F8
		internal static uint VerifyChainPolicy(SafeFreeCertChain chainContext, ref ChainPolicyParameter cpp)
		{
			ChainPolicyStatus chainPolicyStatus = default(ChainPolicyStatus);
			chainPolicyStatus.cbSize = ChainPolicyStatus.StructSize;
			UnsafeNclNativeMethods.NativePKI.CertVerifyCertificateChainPolicy((IntPtr)4L, chainContext, ref cpp, ref chainPolicyStatus);
			return chainPolicyStatus.dwError;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00081234 File Offset: 0x00080234
		private static IgnoreCertProblem MapErrorCode(uint errorCode)
		{
			switch (errorCode)
			{
			case 2148081682U:
			case 2148081683U:
				break;
			default:
				if (errorCode != 2148098073U)
				{
					switch (errorCode)
					{
					case 2148204801U:
						return (IgnoreCertProblem)3;
					case 2148204802U:
						return IgnoreCertProblem.not_time_nested;
					case 2148204803U:
						return IgnoreCertProblem.invalid_basic_constraints;
					case 2148204806U:
					case 2148204819U:
						return IgnoreCertProblem.invalid_policy;
					case 2148204809U:
					case 2148204810U:
					case 2148204818U:
						return IgnoreCertProblem.allow_unknown_ca;
					case 2148204812U:
					case 2148204814U:
						return IgnoreCertProblem.all_rev_unknown;
					case 2148204815U:
					case 2148204820U:
						return IgnoreCertProblem.invalid_name;
					case 2148204816U:
						return IgnoreCertProblem.wrong_usage;
					}
					return (IgnoreCertProblem)0;
				}
				return IgnoreCertProblem.invalid_basic_constraints;
			}
			return IgnoreCertProblem.all_rev_unknown;
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000812DC File Offset: 0x000802DC
		private unsafe uint[] GetChainErrors(string hostName, X509Chain chain, ref bool fatalError)
		{
			fatalError = false;
			SafeFreeCertChain chainContext = new SafeFreeCertChain(chain.ChainContext);
			ArrayList arrayList = new ArrayList();
			ChainPolicyParameter chainPolicyParameter = default(ChainPolicyParameter);
			chainPolicyParameter.cbSize = ChainPolicyParameter.StructSize;
			chainPolicyParameter.dwFlags = 0U;
			SSL_EXTRA_CERT_CHAIN_POLICY_PARA ssl_EXTRA_CERT_CHAIN_POLICY_PARA = new SSL_EXTRA_CERT_CHAIN_POLICY_PARA(false);
			chainPolicyParameter.pvExtraPolicyPara = &ssl_EXTRA_CERT_CHAIN_POLICY_PARA;
			fixed (char* pwszServerName = hostName)
			{
				if (ServicePointManager.CheckCertificateName)
				{
					ssl_EXTRA_CERT_CHAIN_POLICY_PARA.pwszServerName = pwszServerName;
				}
				for (;;)
				{
					uint num = PolicyWrapper.VerifyChainPolicy(chainContext, ref chainPolicyParameter);
					uint num2 = (uint)PolicyWrapper.MapErrorCode(num);
					arrayList.Add(num);
					if (num == 0U)
					{
						goto IL_C2;
					}
					if (num2 == 0U)
					{
						break;
					}
					chainPolicyParameter.dwFlags |= num2;
					if (num == 2148204815U && ServicePointManager.CheckCertificateName)
					{
						ssl_EXTRA_CERT_CHAIN_POLICY_PARA.fdwChecks = 4096U;
					}
				}
				fatalError = true;
				IL_C2:;
			}
			return (uint[])arrayList.ToArray(typeof(uint));
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000813C4 File Offset: 0x000803C4
		internal bool CheckErrors(string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return this.Accept(certificate, 0);
			}
			if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != SslPolicyErrors.None)
			{
				return this.Accept(certificate, -2146762491);
			}
			if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != SslPolicyErrors.None || (sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != SslPolicyErrors.None)
			{
				bool flag = false;
				uint[] chainErrors = this.GetChainErrors(hostName, chain, ref flag);
				if (flag)
				{
					this.Accept(certificate, -2146893052);
					return false;
				}
				if (chainErrors.Length == 0)
				{
					return this.Accept(certificate, 0);
				}
				foreach (uint certificateProblem in chainErrors)
				{
					if (!this.Accept(certificate, (int)certificateProblem))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04002123 RID: 8483
		private const uint IgnoreUnmatchedCN = 4096U;

		// Token: 0x04002124 RID: 8484
		private ICertificatePolicy fwdPolicy;

		// Token: 0x04002125 RID: 8485
		private ServicePoint srvPoint;

		// Token: 0x04002126 RID: 8486
		private WebRequest request;
	}
}
