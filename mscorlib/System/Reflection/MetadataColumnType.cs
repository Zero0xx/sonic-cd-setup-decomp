using System;

namespace System.Reflection
{
	// Token: 0x0200031F RID: 799
	[Serializable]
	internal enum MetadataColumnType
	{
		// Token: 0x04000C40 RID: 3136
		Module,
		// Token: 0x04000C41 RID: 3137
		TypeRef,
		// Token: 0x04000C42 RID: 3138
		TypeDef,
		// Token: 0x04000C43 RID: 3139
		FieldPtr,
		// Token: 0x04000C44 RID: 3140
		Field,
		// Token: 0x04000C45 RID: 3141
		MethodPtr,
		// Token: 0x04000C46 RID: 3142
		Method,
		// Token: 0x04000C47 RID: 3143
		ParamPtr,
		// Token: 0x04000C48 RID: 3144
		Param,
		// Token: 0x04000C49 RID: 3145
		InterfaceImpl,
		// Token: 0x04000C4A RID: 3146
		MemberRef,
		// Token: 0x04000C4B RID: 3147
		Constant,
		// Token: 0x04000C4C RID: 3148
		CustomAttribute,
		// Token: 0x04000C4D RID: 3149
		FieldMarshal,
		// Token: 0x04000C4E RID: 3150
		DeclSecurity,
		// Token: 0x04000C4F RID: 3151
		ClassLayout,
		// Token: 0x04000C50 RID: 3152
		FieldLayout,
		// Token: 0x04000C51 RID: 3153
		StandAloneSig,
		// Token: 0x04000C52 RID: 3154
		EventMap,
		// Token: 0x04000C53 RID: 3155
		EventPtr,
		// Token: 0x04000C54 RID: 3156
		Event,
		// Token: 0x04000C55 RID: 3157
		PropertyMap,
		// Token: 0x04000C56 RID: 3158
		PropertyPtr,
		// Token: 0x04000C57 RID: 3159
		Property,
		// Token: 0x04000C58 RID: 3160
		MethodSemantics,
		// Token: 0x04000C59 RID: 3161
		MethodImpl,
		// Token: 0x04000C5A RID: 3162
		ModuleRef,
		// Token: 0x04000C5B RID: 3163
		TypeSpec,
		// Token: 0x04000C5C RID: 3164
		ImplMap,
		// Token: 0x04000C5D RID: 3165
		FieldRVA,
		// Token: 0x04000C5E RID: 3166
		ENCLog,
		// Token: 0x04000C5F RID: 3167
		ENCMap,
		// Token: 0x04000C60 RID: 3168
		Assembly,
		// Token: 0x04000C61 RID: 3169
		AssemblyProcessor,
		// Token: 0x04000C62 RID: 3170
		AssemblyOS,
		// Token: 0x04000C63 RID: 3171
		AssemblyRef,
		// Token: 0x04000C64 RID: 3172
		AssemblyRefProcessor,
		// Token: 0x04000C65 RID: 3173
		AssemblyRefOS,
		// Token: 0x04000C66 RID: 3174
		File,
		// Token: 0x04000C67 RID: 3175
		ExportedType,
		// Token: 0x04000C68 RID: 3176
		ManifestResource,
		// Token: 0x04000C69 RID: 3177
		NestedClass,
		// Token: 0x04000C6A RID: 3178
		GenericParam,
		// Token: 0x04000C6B RID: 3179
		MethodSpec,
		// Token: 0x04000C6C RID: 3180
		GenericParamConstraint,
		// Token: 0x04000C6D RID: 3181
		TableIdMax = 63,
		// Token: 0x04000C6E RID: 3182
		CodedToken,
		// Token: 0x04000C6F RID: 3183
		TypeDefOrRef,
		// Token: 0x04000C70 RID: 3184
		HasConstant,
		// Token: 0x04000C71 RID: 3185
		HasCustomAttribute,
		// Token: 0x04000C72 RID: 3186
		HasFieldMarshal,
		// Token: 0x04000C73 RID: 3187
		HasDeclSecurity,
		// Token: 0x04000C74 RID: 3188
		MemberRefParent,
		// Token: 0x04000C75 RID: 3189
		HasSemantic,
		// Token: 0x04000C76 RID: 3190
		MethodDefOrRef,
		// Token: 0x04000C77 RID: 3191
		MemberForwarded,
		// Token: 0x04000C78 RID: 3192
		Implementation,
		// Token: 0x04000C79 RID: 3193
		CustomAttributeType,
		// Token: 0x04000C7A RID: 3194
		ResolutionScope,
		// Token: 0x04000C7B RID: 3195
		TypeOrMethodDef,
		// Token: 0x04000C7C RID: 3196
		CodedTokenMax = 95,
		// Token: 0x04000C7D RID: 3197
		Short,
		// Token: 0x04000C7E RID: 3198
		UShort,
		// Token: 0x04000C7F RID: 3199
		Long,
		// Token: 0x04000C80 RID: 3200
		ULong,
		// Token: 0x04000C81 RID: 3201
		Byte,
		// Token: 0x04000C82 RID: 3202
		StringHeap,
		// Token: 0x04000C83 RID: 3203
		GuidHeap,
		// Token: 0x04000C84 RID: 3204
		BlobHeap
	}
}
