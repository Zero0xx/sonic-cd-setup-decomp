using System;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x0200008E RID: 142
	internal interface IDtdParserAdapter
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060007A3 RID: 1955
		XmlNameTable NameTable { get; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060007A4 RID: 1956
		XmlNamespaceManager NamespaceManager { get; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060007A5 RID: 1957
		bool DtdValidation { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060007A6 RID: 1958
		bool Normalization { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060007A7 RID: 1959
		bool Namespaces { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060007A8 RID: 1960
		bool V1CompatibilityMode { get; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060007A9 RID: 1961
		Uri BaseUri { get; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060007AA RID: 1962
		char[] ParsingBuffer { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060007AB RID: 1963
		int ParsingBufferLength { get; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060007AC RID: 1964
		// (set) Token: 0x060007AD RID: 1965
		int CurrentPosition { get; set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007AE RID: 1966
		int LineNo { get; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007AF RID: 1967
		int LineStartPosition { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007B0 RID: 1968
		bool IsEof { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060007B1 RID: 1969
		int EntityStackLength { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007B2 RID: 1970
		bool IsEntityEolNormalized { get; }

		// Token: 0x060007B3 RID: 1971
		int ReadData();

		// Token: 0x060007B4 RID: 1972
		void OnNewLine(int pos);

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007B5 RID: 1973
		// (set) Token: 0x060007B6 RID: 1974
		ValidationEventHandler EventHandler { get; set; }

		// Token: 0x060007B7 RID: 1975
		void SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception);

		// Token: 0x060007B8 RID: 1976
		int ParseNumericCharRef(BufferBuilder internalSubsetBuilder);

		// Token: 0x060007B9 RID: 1977
		int ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder);

		// Token: 0x060007BA RID: 1978
		void ParsePI(BufferBuilder sb);

		// Token: 0x060007BB RID: 1979
		void ParseComment(BufferBuilder sb);

		// Token: 0x060007BC RID: 1980
		bool PushEntity(SchemaEntity entity, int entityId);

		// Token: 0x060007BD RID: 1981
		bool PopEntity(out SchemaEntity oldEntity, out int newEntityId);

		// Token: 0x060007BE RID: 1982
		bool PushExternalSubset(string systemId, string publicId);

		// Token: 0x060007BF RID: 1983
		void PushInternalDtd(string baseUri, string internalDtd);

		// Token: 0x060007C0 RID: 1984
		void OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo);

		// Token: 0x060007C1 RID: 1985
		void OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo);

		// Token: 0x060007C2 RID: 1986
		void Throw(Exception e);
	}
}
