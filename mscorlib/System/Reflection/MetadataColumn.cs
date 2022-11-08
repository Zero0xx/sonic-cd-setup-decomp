using System;

namespace System.Reflection
{
	// Token: 0x02000320 RID: 800
	[Serializable]
	internal enum MetadataColumn
	{
		// Token: 0x04000C86 RID: 3206
		ModuleGeneration,
		// Token: 0x04000C87 RID: 3207
		ModuleName,
		// Token: 0x04000C88 RID: 3208
		ModuleMvid,
		// Token: 0x04000C89 RID: 3209
		ModuleEncId,
		// Token: 0x04000C8A RID: 3210
		ModuleEncBaseId,
		// Token: 0x04000C8B RID: 3211
		TypeRefResolutionScope = 0,
		// Token: 0x04000C8C RID: 3212
		TypeRefName,
		// Token: 0x04000C8D RID: 3213
		TypeRefNamespace,
		// Token: 0x04000C8E RID: 3214
		TypeDefFlags = 0,
		// Token: 0x04000C8F RID: 3215
		TypeDefName,
		// Token: 0x04000C90 RID: 3216
		TypeDefNamespace,
		// Token: 0x04000C91 RID: 3217
		TypeDefExtends,
		// Token: 0x04000C92 RID: 3218
		TypeDefFieldList,
		// Token: 0x04000C93 RID: 3219
		TypeDefMethodList,
		// Token: 0x04000C94 RID: 3220
		FieldPtrField = 0,
		// Token: 0x04000C95 RID: 3221
		FieldFlags = 0,
		// Token: 0x04000C96 RID: 3222
		FieldName,
		// Token: 0x04000C97 RID: 3223
		FieldSignature,
		// Token: 0x04000C98 RID: 3224
		MethodPtrMethod = 0,
		// Token: 0x04000C99 RID: 3225
		MethodRVA = 0,
		// Token: 0x04000C9A RID: 3226
		MethodImplFlags,
		// Token: 0x04000C9B RID: 3227
		MethodFlags,
		// Token: 0x04000C9C RID: 3228
		MethodName,
		// Token: 0x04000C9D RID: 3229
		MethodSignature,
		// Token: 0x04000C9E RID: 3230
		MethodParamList,
		// Token: 0x04000C9F RID: 3231
		ParamPtrParam = 0,
		// Token: 0x04000CA0 RID: 3232
		ParamFlags = 0,
		// Token: 0x04000CA1 RID: 3233
		ParamSequence,
		// Token: 0x04000CA2 RID: 3234
		ParamName,
		// Token: 0x04000CA3 RID: 3235
		InterfaceImplClass = 0,
		// Token: 0x04000CA4 RID: 3236
		InterfaceImplInterface,
		// Token: 0x04000CA5 RID: 3237
		MemberRefClass = 0,
		// Token: 0x04000CA6 RID: 3238
		MemberRefName,
		// Token: 0x04000CA7 RID: 3239
		MemberRefSignature,
		// Token: 0x04000CA8 RID: 3240
		ConstantType = 0,
		// Token: 0x04000CA9 RID: 3241
		ConstantParent,
		// Token: 0x04000CAA RID: 3242
		ConstantValue,
		// Token: 0x04000CAB RID: 3243
		CustomAttributeParent = 0,
		// Token: 0x04000CAC RID: 3244
		CustomAttributeType,
		// Token: 0x04000CAD RID: 3245
		CustomAttributeArgument,
		// Token: 0x04000CAE RID: 3246
		FieldMarshalParent = 0,
		// Token: 0x04000CAF RID: 3247
		FieldMarshalNativeType,
		// Token: 0x04000CB0 RID: 3248
		DeclSecurityAction = 0,
		// Token: 0x04000CB1 RID: 3249
		DeclSecurityParent,
		// Token: 0x04000CB2 RID: 3250
		DeclSecurityPermissionSet,
		// Token: 0x04000CB3 RID: 3251
		ClassLayoutPackingSize = 0,
		// Token: 0x04000CB4 RID: 3252
		ClassLayoutClassSize,
		// Token: 0x04000CB5 RID: 3253
		ClassLayoutParent,
		// Token: 0x04000CB6 RID: 3254
		FieldLayoutOffSet = 0,
		// Token: 0x04000CB7 RID: 3255
		FieldLayoutField,
		// Token: 0x04000CB8 RID: 3256
		StandAloneSigSignature = 0,
		// Token: 0x04000CB9 RID: 3257
		EventMapParent = 0,
		// Token: 0x04000CBA RID: 3258
		EventMapEventList,
		// Token: 0x04000CBB RID: 3259
		EventPtrEvent = 0,
		// Token: 0x04000CBC RID: 3260
		EventEventFlags = 0,
		// Token: 0x04000CBD RID: 3261
		EventName,
		// Token: 0x04000CBE RID: 3262
		EventEventType,
		// Token: 0x04000CBF RID: 3263
		PropertyMapParent = 0,
		// Token: 0x04000CC0 RID: 3264
		PropertyMapPropertyList,
		// Token: 0x04000CC1 RID: 3265
		PropertyPtrProperty = 0,
		// Token: 0x04000CC2 RID: 3266
		PropertyPropFlags = 0,
		// Token: 0x04000CC3 RID: 3267
		PropertyName,
		// Token: 0x04000CC4 RID: 3268
		PropertyType,
		// Token: 0x04000CC5 RID: 3269
		MethodSemanticsSemantic = 0,
		// Token: 0x04000CC6 RID: 3270
		MethodSemanticsMethod,
		// Token: 0x04000CC7 RID: 3271
		MethodSemanticsAssociation,
		// Token: 0x04000CC8 RID: 3272
		MethodImplClass = 0,
		// Token: 0x04000CC9 RID: 3273
		MethodImplMethodBody,
		// Token: 0x04000CCA RID: 3274
		MethodImplMethodDeclaration,
		// Token: 0x04000CCB RID: 3275
		ModuleRefName = 0,
		// Token: 0x04000CCC RID: 3276
		TypeSpecSignature = 0,
		// Token: 0x04000CCD RID: 3277
		ImplMapMappingFlags = 0,
		// Token: 0x04000CCE RID: 3278
		ImplMapMemberForwarded,
		// Token: 0x04000CCF RID: 3279
		ImplMapImportName,
		// Token: 0x04000CD0 RID: 3280
		ImplMapImportScope,
		// Token: 0x04000CD1 RID: 3281
		FieldRVARVA = 0,
		// Token: 0x04000CD2 RID: 3282
		FieldRVAField,
		// Token: 0x04000CD3 RID: 3283
		ENCLogToken = 0,
		// Token: 0x04000CD4 RID: 3284
		ENCLogFuncCode,
		// Token: 0x04000CD5 RID: 3285
		ENCMapToken = 0,
		// Token: 0x04000CD6 RID: 3286
		AssemblyHashAlgId = 0,
		// Token: 0x04000CD7 RID: 3287
		AssemblyMajorVersion,
		// Token: 0x04000CD8 RID: 3288
		AssemblyMinorVersion,
		// Token: 0x04000CD9 RID: 3289
		AssemblyBuildNumber,
		// Token: 0x04000CDA RID: 3290
		AssemblyRevisionNumber,
		// Token: 0x04000CDB RID: 3291
		AssemblyFlags,
		// Token: 0x04000CDC RID: 3292
		AssemblyPublicKey,
		// Token: 0x04000CDD RID: 3293
		AssemblyName,
		// Token: 0x04000CDE RID: 3294
		AssemblyLocale,
		// Token: 0x04000CDF RID: 3295
		AssemblyProcessorProcessor = 0,
		// Token: 0x04000CE0 RID: 3296
		AssemblyOSOSPlatformId = 0,
		// Token: 0x04000CE1 RID: 3297
		AssemblyOSOSMajorVersion,
		// Token: 0x04000CE2 RID: 3298
		AssemblyOSOSMinorVersion,
		// Token: 0x04000CE3 RID: 3299
		AssemblyRefMajorVersion = 0,
		// Token: 0x04000CE4 RID: 3300
		AssemblyRefMinorVersion,
		// Token: 0x04000CE5 RID: 3301
		AssemblyRefBuildNumber,
		// Token: 0x04000CE6 RID: 3302
		AssemblyRefRevisionNumber,
		// Token: 0x04000CE7 RID: 3303
		AssemblyRefFlags,
		// Token: 0x04000CE8 RID: 3304
		AssemblyRefPublicKeyOrToken,
		// Token: 0x04000CE9 RID: 3305
		AssemblyRefName,
		// Token: 0x04000CEA RID: 3306
		AssemblyRefLocale,
		// Token: 0x04000CEB RID: 3307
		AssemblyRefHashValue,
		// Token: 0x04000CEC RID: 3308
		AssemblyRefProcessorProcessor = 0,
		// Token: 0x04000CED RID: 3309
		AssemblyRefProcessorAssemblyRef,
		// Token: 0x04000CEE RID: 3310
		AssemblyRefOSOSPlatformId = 0,
		// Token: 0x04000CEF RID: 3311
		AssemblyRefOSOSMajorVersion,
		// Token: 0x04000CF0 RID: 3312
		AssemblyRefOSOSMinorVersion,
		// Token: 0x04000CF1 RID: 3313
		AssemblyRefOSAssemblyRef,
		// Token: 0x04000CF2 RID: 3314
		FileFlags = 0,
		// Token: 0x04000CF3 RID: 3315
		FileName,
		// Token: 0x04000CF4 RID: 3316
		FileHashValue,
		// Token: 0x04000CF5 RID: 3317
		ExportedTypeFlags = 0,
		// Token: 0x04000CF6 RID: 3318
		ExportedTypeTypeDefId,
		// Token: 0x04000CF7 RID: 3319
		ExportedTypeTypeName,
		// Token: 0x04000CF8 RID: 3320
		ExportedTypeTypeNamespace,
		// Token: 0x04000CF9 RID: 3321
		ExportedTypeImplementation,
		// Token: 0x04000CFA RID: 3322
		ManifestResourceOffset = 0,
		// Token: 0x04000CFB RID: 3323
		ManifestResourceFlags,
		// Token: 0x04000CFC RID: 3324
		ManifestResourceName,
		// Token: 0x04000CFD RID: 3325
		ManifestResourceImplementation,
		// Token: 0x04000CFE RID: 3326
		NestedClassNestedClass = 0,
		// Token: 0x04000CFF RID: 3327
		NestedClassEnclosingClass,
		// Token: 0x04000D00 RID: 3328
		GenericParamNumber = 0,
		// Token: 0x04000D01 RID: 3329
		GenericParamFlags,
		// Token: 0x04000D02 RID: 3330
		GenericParamOwner,
		// Token: 0x04000D03 RID: 3331
		GenericParamName,
		// Token: 0x04000D04 RID: 3332
		GenericParamKind,
		// Token: 0x04000D05 RID: 3333
		GenericParamDeprecatedConstraint,
		// Token: 0x04000D06 RID: 3334
		MethodSpecMethod = 0,
		// Token: 0x04000D07 RID: 3335
		MethodSpecInstantiation,
		// Token: 0x04000D08 RID: 3336
		GenericParamConstraintOwner = 0,
		// Token: 0x04000D09 RID: 3337
		GenericParamConstraintConstraint
	}
}
