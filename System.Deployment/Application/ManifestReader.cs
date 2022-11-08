﻿using System;
using System.Deployment.Application.Manifest;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Internal.Performance;

namespace System.Deployment.Application
{
	// Token: 0x02000081 RID: 129
	internal static class ManifestReader
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x000157B0 File Offset: 0x000147B0
		internal static AssemblyManifest FromDocument(string localPath, AssemblyManifest.ManifestType manifestType, Uri sourceUri)
		{
			CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfParseBegin);
			FileInfo fileInfo = new FileInfo(localPath);
			if (fileInfo.Length > 16777216L)
			{
				throw new DeploymentException(Resources.GetString("Ex_ManifestFileTooLarge"));
			}
			AssemblyManifest assemblyManifest;
			using (FileStream fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
			{
				try
				{
					XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
					xmlReaderSettings.ProhibitDtd = true;
					xmlReaderSettings.XmlResolver = null;
					XmlReader xmlReader = PolicyKeys.SkipSchemaValidation() ? XmlReader.Create(fileStream, xmlReaderSettings) : ManifestValidatingReader.Create(fileStream);
					while (xmlReader.Read())
					{
					}
					assemblyManifest = new AssemblyManifest(fileStream);
					if (!PolicyKeys.SkipSemanticValidation())
					{
						assemblyManifest.ValidateSemantics(manifestType);
					}
					if (!PolicyKeys.SkipSignatureValidation())
					{
						fileStream.Position = 0L;
						assemblyManifest.ValidateSignature(fileStream);
					}
				}
				catch (XmlException innerException)
				{
					string message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestFromDocument"), new object[]
					{
						(sourceUri != null) ? sourceUri.AbsoluteUri : Path.GetFileName(localPath)
					});
					throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, message, innerException);
				}
				catch (XmlSchemaValidationException innerException2)
				{
					string message2 = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestFromDocument"), new object[]
					{
						(sourceUri != null) ? sourceUri.AbsoluteUri : Path.GetFileName(localPath)
					});
					throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, message2, innerException2);
				}
				catch (InvalidDeploymentException innerException3)
				{
					string message3 = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestFromDocument"), new object[]
					{
						(sourceUri != null) ? sourceUri.AbsoluteUri : Path.GetFileName(localPath)
					});
					throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, message3, innerException3);
				}
			}
			CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfParseEnd);
			return assemblyManifest;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000159BC File Offset: 0x000149BC
		internal static AssemblyManifest FromDocumentNoValidation(string localPath)
		{
			CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfParseBegin);
			FileInfo fileInfo = new FileInfo(localPath);
			if (fileInfo.Length > 16777216L)
			{
				throw new DeploymentException(Resources.GetString("Ex_ManifestFileTooLarge"));
			}
			AssemblyManifest result;
			using (FileStream fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
			{
				result = new AssemblyManifest(fileStream);
			}
			CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfParseEnd);
			return result;
		}
	}
}
