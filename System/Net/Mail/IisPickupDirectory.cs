using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Authentication;
using System.Security.Permissions;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000693 RID: 1683
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal static class IisPickupDirectory
	{
		// Token: 0x060033F3 RID: 13299 RVA: 0x000DB1F0 File Offset: 0x000DA1F0
		internal unsafe static string GetPickupDirectory()
		{
			uint num = 0U;
			string text = string.Empty;
			IMSAdminBase imsadminBase = null;
			IntPtr zero = IntPtr.Zero;
			StringBuilder stringBuilder = new StringBuilder(256);
			uint num2 = 1040U;
			byte[] array = new byte[num2];
			try
			{
				imsadminBase = (new MSAdminBase() as IMSAdminBase);
				int num3 = imsadminBase.OpenKey(IntPtr.Zero, "LM/SmtpSvc", MBKeyAccess.Read, -1, ref zero);
				if (num3 >= 0)
				{
					MetadataRecord metadataRecord = default(MetadataRecord);
					try
					{
						fixed (byte* ptr = array)
						{
							int num4 = 0;
							for (;;)
							{
								num3 = imsadminBase.EnumKeys(zero, "", stringBuilder, num4);
								if (num3 == -2147024637)
								{
									goto IL_1AE;
								}
								if (num3 < 0)
								{
									break;
								}
								metadataRecord.Identifier = 1016U;
								metadataRecord.Attributes = 0U;
								metadataRecord.UserType = 1U;
								metadataRecord.DataType = 1U;
								metadataRecord.DataTag = 0U;
								metadataRecord.DataBuf = (IntPtr)((void*)ptr);
								metadataRecord.DataLen = num2;
								num3 = imsadminBase.GetData(zero, stringBuilder.ToString(), ref metadataRecord, ref num);
								if (num3 < 0)
								{
									if (num3 != -2146646015 && num3 != -2147024891)
									{
										break;
									}
								}
								else
								{
									int num5 = Marshal.ReadInt32((IntPtr)((void*)ptr));
									if (num5 == 2)
									{
										goto Block_14;
									}
								}
								num4++;
							}
							goto IL_280;
							Block_14:
							metadataRecord.Identifier = 36880U;
							metadataRecord.Attributes = 0U;
							metadataRecord.UserType = 1U;
							metadataRecord.DataType = 2U;
							metadataRecord.DataTag = 0U;
							metadataRecord.DataBuf = (IntPtr)((void*)ptr);
							metadataRecord.DataLen = num2;
							num3 = imsadminBase.GetData(zero, stringBuilder.ToString(), ref metadataRecord, ref num);
							if (num3 < 0)
							{
								goto IL_280;
							}
							text = Marshal.PtrToStringUni((IntPtr)((void*)ptr));
							IL_1AE:
							if (num3 == -2147024637)
							{
								int num6 = 0;
								for (;;)
								{
									num3 = imsadminBase.EnumKeys(zero, "", stringBuilder, num6);
									if (num3 == -2147024637)
									{
										break;
									}
									if (num3 < 0)
									{
										break;
									}
									metadataRecord.Identifier = 36880U;
									metadataRecord.Attributes = 0U;
									metadataRecord.UserType = 1U;
									metadataRecord.DataType = 2U;
									metadataRecord.DataTag = 0U;
									metadataRecord.DataBuf = (IntPtr)((void*)ptr);
									metadataRecord.DataLen = num2;
									num3 = imsadminBase.GetData(zero, stringBuilder.ToString(), ref metadataRecord, ref num);
									if (num3 < 0)
									{
										if (num3 != -2146646015 && num3 != -2147024891)
										{
											break;
										}
									}
									else
									{
										text = Marshal.PtrToStringUni((IntPtr)((void*)ptr));
										if (Directory.Exists(text))
										{
											break;
										}
										text = string.Empty;
									}
									num6++;
								}
							}
						}
					}
					finally
					{
						byte* ptr = null;
					}
				}
				IL_280:;
			}
			catch (Exception ex)
			{
				if (ex is SecurityException || ex is AuthenticationException || ex is SmtpException)
				{
					throw;
				}
				throw new SmtpException(SR.GetString("SmtpGetIisPickupDirectoryFailed"));
			}
			catch
			{
				throw new SmtpException(SR.GetString("SmtpGetIisPickupDirectoryFailed"));
			}
			finally
			{
				if (imsadminBase != null && zero != IntPtr.Zero)
				{
					imsadminBase.CloseKey(zero);
				}
			}
			if (text == string.Empty)
			{
				throw new SmtpException(SR.GetString("SmtpGetIisPickupDirectoryFailed"));
			}
			return text;
		}

		// Token: 0x04002FEC RID: 12268
		private const int MaxPathSize = 260;

		// Token: 0x04002FED RID: 12269
		private const int InfiniteTimeout = -1;

		// Token: 0x04002FEE RID: 12270
		private const int MetadataMaxNameLen = 256;
	}
}
