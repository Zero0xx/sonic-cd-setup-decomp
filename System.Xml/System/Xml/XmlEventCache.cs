using System;
using System.Collections;
using System.Text;
using System.Xml.Schema;
using System.Xml.Xsl.Runtime;

namespace System.Xml
{
	// Token: 0x0200007A RID: 122
	internal sealed class XmlEventCache : XmlRawWriter
	{
		// Token: 0x0600054C RID: 1356 RVA: 0x0001615A File Offset: 0x0001515A
		public XmlEventCache(string baseUri, bool hasRootNode)
		{
			this.baseUri = baseUri;
			this.hasRootNode = hasRootNode;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00016170 File Offset: 0x00015170
		public void EndEvents()
		{
			if (this.singleText.Count == 0)
			{
				this.AddEvent(XmlEventCache.XmlEventType.Unknown);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00016186 File Offset: 0x00015186
		public string BaseUri
		{
			get
			{
				return this.baseUri;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001618E File Offset: 0x0001518E
		public bool HasRootNode
		{
			get
			{
				return this.hasRootNode;
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00016198 File Offset: 0x00015198
		public void EventsToWriter(XmlWriter writer)
		{
			if (this.singleText.Count != 0)
			{
				writer.WriteString(this.singleText.GetResult());
				return;
			}
			XmlRawWriter xmlRawWriter = writer as XmlRawWriter;
			for (int i = 0; i < this.pages.Count; i++)
			{
				XmlEventCache.XmlEvent[] array = this.pages[i] as XmlEventCache.XmlEvent[];
				for (int j = 0; j < array.Length; j++)
				{
					switch (array[j].EventType)
					{
					case XmlEventCache.XmlEventType.Unknown:
						return;
					case XmlEventCache.XmlEventType.DocType:
						writer.WriteDocType(array[j].String1, array[j].String2, array[j].String3, (string)array[j].Object);
						break;
					case XmlEventCache.XmlEventType.StartElem:
						writer.WriteStartElement(array[j].String1, array[j].String2, array[j].String3);
						break;
					case XmlEventCache.XmlEventType.StartAttr:
						writer.WriteStartAttribute(array[j].String1, array[j].String2, array[j].String3);
						break;
					case XmlEventCache.XmlEventType.EndAttr:
						writer.WriteEndAttribute();
						break;
					case XmlEventCache.XmlEventType.CData:
						writer.WriteCData(array[j].String1);
						break;
					case XmlEventCache.XmlEventType.Comment:
						writer.WriteComment(array[j].String1);
						break;
					case XmlEventCache.XmlEventType.PI:
						writer.WriteProcessingInstruction(array[j].String1, array[j].String2);
						break;
					case XmlEventCache.XmlEventType.Whitespace:
						writer.WriteWhitespace(array[j].String1);
						break;
					case XmlEventCache.XmlEventType.String:
						writer.WriteString(array[j].String1);
						break;
					case XmlEventCache.XmlEventType.Raw:
						writer.WriteRaw(array[j].String1);
						break;
					case XmlEventCache.XmlEventType.EntRef:
						writer.WriteEntityRef(array[j].String1);
						break;
					case XmlEventCache.XmlEventType.CharEnt:
						writer.WriteCharEntity((char)array[j].Object);
						break;
					case XmlEventCache.XmlEventType.SurrCharEnt:
					{
						char[] array2 = (char[])array[j].Object;
						writer.WriteSurrogateCharEntity(array2[0], array2[1]);
						break;
					}
					case XmlEventCache.XmlEventType.Base64:
					{
						byte[] array3 = (byte[])array[j].Object;
						writer.WriteBase64(array3, 0, array3.Length);
						break;
					}
					case XmlEventCache.XmlEventType.BinHex:
					{
						byte[] array3 = (byte[])array[j].Object;
						writer.WriteBinHex(array3, 0, array3.Length);
						break;
					}
					case XmlEventCache.XmlEventType.XmlDecl1:
						if (xmlRawWriter != null)
						{
							xmlRawWriter.WriteXmlDeclaration((XmlStandalone)array[j].Object);
						}
						break;
					case XmlEventCache.XmlEventType.XmlDecl2:
						if (xmlRawWriter != null)
						{
							xmlRawWriter.WriteXmlDeclaration(array[j].String1);
						}
						break;
					case XmlEventCache.XmlEventType.StartContent:
						if (xmlRawWriter != null)
						{
							xmlRawWriter.StartElementContent();
						}
						break;
					case XmlEventCache.XmlEventType.EndElem:
						if (xmlRawWriter != null)
						{
							xmlRawWriter.WriteEndElement(array[j].String1, array[j].String2, array[j].String3);
						}
						else
						{
							writer.WriteEndElement();
						}
						break;
					case XmlEventCache.XmlEventType.FullEndElem:
						if (xmlRawWriter != null)
						{
							xmlRawWriter.WriteFullEndElement(array[j].String1, array[j].String2, array[j].String3);
						}
						else
						{
							writer.WriteFullEndElement();
						}
						break;
					case XmlEventCache.XmlEventType.Nmsp:
						if (xmlRawWriter != null)
						{
							xmlRawWriter.WriteNamespaceDeclaration(array[j].String1, array[j].String2);
						}
						else
						{
							writer.WriteAttributeString("xmlns", array[j].String1, "http://www.w3.org/2000/xmlns/", array[j].String2);
						}
						break;
					case XmlEventCache.XmlEventType.EndBase64:
						if (xmlRawWriter != null)
						{
							xmlRawWriter.WriteEndBase64();
						}
						break;
					case XmlEventCache.XmlEventType.Close:
						writer.Close();
						break;
					case XmlEventCache.XmlEventType.Flush:
						writer.Flush();
						break;
					case XmlEventCache.XmlEventType.Dispose:
						((IDisposable)writer).Dispose();
						break;
					}
				}
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000165B0 File Offset: 0x000155B0
		public string EventsToString()
		{
			if (this.singleText.Count != 0)
			{
				return this.singleText.GetResult();
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (int i = 0; i < this.pages.Count; i++)
			{
				XmlEventCache.XmlEvent[] array = this.pages[i] as XmlEventCache.XmlEvent[];
				for (int j = 0; j < array.Length; j++)
				{
					switch (array[j].EventType)
					{
					case XmlEventCache.XmlEventType.Unknown:
						return stringBuilder.ToString();
					case XmlEventCache.XmlEventType.StartAttr:
						flag = true;
						break;
					case XmlEventCache.XmlEventType.EndAttr:
						flag = false;
						break;
					case XmlEventCache.XmlEventType.CData:
					case XmlEventCache.XmlEventType.Whitespace:
					case XmlEventCache.XmlEventType.String:
					case XmlEventCache.XmlEventType.Raw:
						if (!flag)
						{
							stringBuilder.Append(array[j].String1);
						}
						break;
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001668B File Offset: 0x0001568B
		public override XmlWriterSettings Settings
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001668E File Offset: 0x0001568E
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			this.AddEvent(XmlEventCache.XmlEventType.DocType, name, pubid, sysid, subset);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001669C File Offset: 0x0001569C
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.AddEvent(XmlEventCache.XmlEventType.StartElem, prefix, localName, ns);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000166A8 File Offset: 0x000156A8
		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.AddEvent(XmlEventCache.XmlEventType.StartAttr, prefix, localName, ns);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000166B4 File Offset: 0x000156B4
		public override void WriteEndAttribute()
		{
			this.AddEvent(XmlEventCache.XmlEventType.EndAttr);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000166BD File Offset: 0x000156BD
		public override void WriteCData(string text)
		{
			this.AddEvent(XmlEventCache.XmlEventType.CData, text);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000166C7 File Offset: 0x000156C7
		public override void WriteComment(string text)
		{
			this.AddEvent(XmlEventCache.XmlEventType.Comment, text);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000166D1 File Offset: 0x000156D1
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.AddEvent(XmlEventCache.XmlEventType.PI, name, text);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000166DC File Offset: 0x000156DC
		public override void WriteWhitespace(string ws)
		{
			this.AddEvent(XmlEventCache.XmlEventType.Whitespace, ws);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000166E6 File Offset: 0x000156E6
		public override void WriteString(string text)
		{
			if (this.pages == null)
			{
				this.singleText.ConcatNoDelimiter(text);
				return;
			}
			this.AddEvent(XmlEventCache.XmlEventType.String, text);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00016706 File Offset: 0x00015706
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00016716 File Offset: 0x00015716
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.WriteRaw(new string(buffer, index, count));
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00016726 File Offset: 0x00015726
		public override void WriteRaw(string data)
		{
			this.AddEvent(XmlEventCache.XmlEventType.Raw, data);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00016731 File Offset: 0x00015731
		public override void WriteEntityRef(string name)
		{
			this.AddEvent(XmlEventCache.XmlEventType.EntRef, name);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001673C File Offset: 0x0001573C
		public override void WriteCharEntity(char ch)
		{
			this.AddEvent(XmlEventCache.XmlEventType.CharEnt, ch);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001674C File Offset: 0x0001574C
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			char[] o = new char[]
			{
				lowChar,
				highChar
			};
			this.AddEvent(XmlEventCache.XmlEventType.SurrCharEnt, o);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00016773 File Offset: 0x00015773
		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			this.AddEvent(XmlEventCache.XmlEventType.Base64, XmlEventCache.ToBytes(buffer, index, count));
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00016785 File Offset: 0x00015785
		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			this.AddEvent(XmlEventCache.XmlEventType.BinHex, XmlEventCache.ToBytes(buffer, index, count));
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00016797 File Offset: 0x00015797
		public override void Close()
		{
			this.AddEvent(XmlEventCache.XmlEventType.Close);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000167A1 File Offset: 0x000157A1
		public override void Flush()
		{
			this.AddEvent(XmlEventCache.XmlEventType.Flush);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000167AB File Offset: 0x000157AB
		public override void WriteValue(object value)
		{
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value, this.resolver));
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x000167C4 File Offset: 0x000157C4
		public override void WriteValue(string value)
		{
			this.WriteString(value);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000167D0 File Offset: 0x000157D0
		protected override void Dispose(bool disposing)
		{
			try
			{
				this.AddEvent(XmlEventCache.XmlEventType.Dispose);
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00016800 File Offset: 0x00015800
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
			this.AddEvent(XmlEventCache.XmlEventType.XmlDecl1, standalone);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00016810 File Offset: 0x00015810
		internal override void WriteXmlDeclaration(string xmldecl)
		{
			this.AddEvent(XmlEventCache.XmlEventType.XmlDecl2, xmldecl);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001681B File Offset: 0x0001581B
		internal override void StartElementContent()
		{
			this.AddEvent(XmlEventCache.XmlEventType.StartContent);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00016825 File Offset: 0x00015825
		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
			this.AddEvent(XmlEventCache.XmlEventType.EndElem, prefix, localName, ns);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00016832 File Offset: 0x00015832
		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.AddEvent(XmlEventCache.XmlEventType.FullEndElem, prefix, localName, ns);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001683F File Offset: 0x0001583F
		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
			this.AddEvent(XmlEventCache.XmlEventType.Nmsp, prefix, ns);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001684B File Offset: 0x0001584B
		internal override void WriteEndBase64()
		{
			this.AddEvent(XmlEventCache.XmlEventType.EndBase64);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00016858 File Offset: 0x00015858
		private void AddEvent(XmlEventCache.XmlEventType eventType)
		{
			int num = this.NewEvent();
			this.pageCurr[num].InitEvent(eventType);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00016880 File Offset: 0x00015880
		private void AddEvent(XmlEventCache.XmlEventType eventType, string s1)
		{
			int num = this.NewEvent();
			this.pageCurr[num].InitEvent(eventType, s1);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000168A8 File Offset: 0x000158A8
		private void AddEvent(XmlEventCache.XmlEventType eventType, string s1, string s2)
		{
			int num = this.NewEvent();
			this.pageCurr[num].InitEvent(eventType, s1, s2);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000168D0 File Offset: 0x000158D0
		private void AddEvent(XmlEventCache.XmlEventType eventType, string s1, string s2, string s3)
		{
			int num = this.NewEvent();
			this.pageCurr[num].InitEvent(eventType, s1, s2, s3);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000168FC File Offset: 0x000158FC
		private void AddEvent(XmlEventCache.XmlEventType eventType, string s1, string s2, string s3, object o)
		{
			int num = this.NewEvent();
			this.pageCurr[num].InitEvent(eventType, s1, s2, s3, o);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00016928 File Offset: 0x00015928
		private void AddEvent(XmlEventCache.XmlEventType eventType, object o)
		{
			int num = this.NewEvent();
			this.pageCurr[num].InitEvent(eventType, o);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00016950 File Offset: 0x00015950
		private int NewEvent()
		{
			if (this.pages == null)
			{
				this.pages = new ArrayList();
				this.pageCurr = new XmlEventCache.XmlEvent[32];
				this.pages.Add(this.pageCurr);
				if (this.singleText.Count != 0)
				{
					this.pageCurr[0].InitEvent(XmlEventCache.XmlEventType.String, this.singleText.GetResult());
					this.pageSize++;
					this.singleText.Clear();
				}
			}
			else if (this.pageSize >= this.pageCurr.Length)
			{
				this.pageCurr = new XmlEventCache.XmlEvent[this.pageSize * 2];
				this.pages.Add(this.pageCurr);
				this.pageSize = 0;
			}
			return this.pageSize++;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00016A24 File Offset: 0x00015A24
		private static byte[] ToBytes(byte[] buffer, int index, int count)
		{
			if (index != 0 || count != buffer.Length)
			{
				if (buffer.Length - index > count)
				{
					count = buffer.Length - index;
				}
				byte[] array = new byte[count];
				Array.Copy(buffer, index, array, 0, count);
				return array;
			}
			return buffer;
		}

		// Token: 0x0400060E RID: 1550
		private const int InitialPageSize = 32;

		// Token: 0x0400060F RID: 1551
		private ArrayList pages;

		// Token: 0x04000610 RID: 1552
		private XmlEventCache.XmlEvent[] pageCurr;

		// Token: 0x04000611 RID: 1553
		private int pageSize;

		// Token: 0x04000612 RID: 1554
		private bool hasRootNode;

		// Token: 0x04000613 RID: 1555
		private StringConcat singleText;

		// Token: 0x04000614 RID: 1556
		private string baseUri;

		// Token: 0x0200007B RID: 123
		private enum XmlEventType
		{
			// Token: 0x04000616 RID: 1558
			Unknown,
			// Token: 0x04000617 RID: 1559
			DocType,
			// Token: 0x04000618 RID: 1560
			StartElem,
			// Token: 0x04000619 RID: 1561
			StartAttr,
			// Token: 0x0400061A RID: 1562
			EndAttr,
			// Token: 0x0400061B RID: 1563
			CData,
			// Token: 0x0400061C RID: 1564
			Comment,
			// Token: 0x0400061D RID: 1565
			PI,
			// Token: 0x0400061E RID: 1566
			Whitespace,
			// Token: 0x0400061F RID: 1567
			String,
			// Token: 0x04000620 RID: 1568
			Raw,
			// Token: 0x04000621 RID: 1569
			EntRef,
			// Token: 0x04000622 RID: 1570
			CharEnt,
			// Token: 0x04000623 RID: 1571
			SurrCharEnt,
			// Token: 0x04000624 RID: 1572
			Base64,
			// Token: 0x04000625 RID: 1573
			BinHex,
			// Token: 0x04000626 RID: 1574
			XmlDecl1,
			// Token: 0x04000627 RID: 1575
			XmlDecl2,
			// Token: 0x04000628 RID: 1576
			StartContent,
			// Token: 0x04000629 RID: 1577
			EndElem,
			// Token: 0x0400062A RID: 1578
			FullEndElem,
			// Token: 0x0400062B RID: 1579
			Nmsp,
			// Token: 0x0400062C RID: 1580
			EndBase64,
			// Token: 0x0400062D RID: 1581
			Close,
			// Token: 0x0400062E RID: 1582
			Flush,
			// Token: 0x0400062F RID: 1583
			Dispose
		}

		// Token: 0x0200007C RID: 124
		private struct XmlEvent
		{
			// Token: 0x06000578 RID: 1400 RVA: 0x00016A5D File Offset: 0x00015A5D
			public void InitEvent(XmlEventCache.XmlEventType eventType)
			{
				this.eventType = eventType;
			}

			// Token: 0x06000579 RID: 1401 RVA: 0x00016A66 File Offset: 0x00015A66
			public void InitEvent(XmlEventCache.XmlEventType eventType, string s1)
			{
				this.eventType = eventType;
				this.s1 = s1;
			}

			// Token: 0x0600057A RID: 1402 RVA: 0x00016A76 File Offset: 0x00015A76
			public void InitEvent(XmlEventCache.XmlEventType eventType, string s1, string s2)
			{
				this.eventType = eventType;
				this.s1 = s1;
				this.s2 = s2;
			}

			// Token: 0x0600057B RID: 1403 RVA: 0x00016A8D File Offset: 0x00015A8D
			public void InitEvent(XmlEventCache.XmlEventType eventType, string s1, string s2, string s3)
			{
				this.eventType = eventType;
				this.s1 = s1;
				this.s2 = s2;
				this.s3 = s3;
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x00016AAC File Offset: 0x00015AAC
			public void InitEvent(XmlEventCache.XmlEventType eventType, string s1, string s2, string s3, object o)
			{
				this.eventType = eventType;
				this.s1 = s1;
				this.s2 = s2;
				this.s3 = s3;
				this.o = o;
			}

			// Token: 0x0600057D RID: 1405 RVA: 0x00016AD3 File Offset: 0x00015AD3
			public void InitEvent(XmlEventCache.XmlEventType eventType, object o)
			{
				this.eventType = eventType;
				this.o = o;
			}

			// Token: 0x170000BE RID: 190
			// (get) Token: 0x0600057E RID: 1406 RVA: 0x00016AE3 File Offset: 0x00015AE3
			public XmlEventCache.XmlEventType EventType
			{
				get
				{
					return this.eventType;
				}
			}

			// Token: 0x170000BF RID: 191
			// (get) Token: 0x0600057F RID: 1407 RVA: 0x00016AEB File Offset: 0x00015AEB
			public string String1
			{
				get
				{
					return this.s1;
				}
			}

			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x06000580 RID: 1408 RVA: 0x00016AF3 File Offset: 0x00015AF3
			public string String2
			{
				get
				{
					return this.s2;
				}
			}

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x06000581 RID: 1409 RVA: 0x00016AFB File Offset: 0x00015AFB
			public string String3
			{
				get
				{
					return this.s3;
				}
			}

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x06000582 RID: 1410 RVA: 0x00016B03 File Offset: 0x00015B03
			public object Object
			{
				get
				{
					return this.o;
				}
			}

			// Token: 0x04000630 RID: 1584
			private XmlEventCache.XmlEventType eventType;

			// Token: 0x04000631 RID: 1585
			private string s1;

			// Token: 0x04000632 RID: 1586
			private string s2;

			// Token: 0x04000633 RID: 1587
			private string s3;

			// Token: 0x04000634 RID: 1588
			private object o;
		}
	}
}
