#region Using
using System.Xml;
#endregion

namespace OpenLDR.Dashboard.API.Utils
{
    public class XmlWriterEE : XmlWriter
    {
        #region Variables
        private XmlWriter baseWriter;
        #endregion

        #region Constructor
        public XmlWriterEE(XmlWriter w)
        {
            baseWriter = w;
        }
        #endregion

        #region WriteEndElement
        //Force WriteEndElement to use WriteFullEndElement
        public override void WriteEndElement() { baseWriter.WriteFullEndElement(); }
        #endregion

        #region WriteFullEndElement
        public override void WriteFullEndElement()
        {
            baseWriter.WriteFullEndElement();
        }
        #endregion

        #region Close
        public override void Close()
        {
            baseWriter.Close();
        }
        #endregion

        #region Flush
        public override void Flush()
        {
            baseWriter.Flush();
        }
        #endregion

        #region LookupPrefix
        public override string LookupPrefix(string ns)
        {
            return (baseWriter.LookupPrefix(ns));
        }
        #endregion

        #region WriteBase64
        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            baseWriter.WriteBase64(buffer, index, count);
        }
        #endregion

        #region WriteCData
        public override void WriteCData(string text)
        {
            baseWriter.WriteCData(text);
        }
        #endregion

        #region WriteCharEntity
        public override void WriteCharEntity(char ch)
        {
            baseWriter.WriteCharEntity(ch);
        }
        #endregion

        #region WriteChars
        public override void WriteChars(char[] buffer, int index, int count)
        {
            baseWriter.WriteChars(buffer, index, count);
        }
        #endregion

        #region WriteComment
        public override void WriteComment(string text)
        {
            baseWriter.WriteComment(text);
        }
        #endregion

        #region WriteDocType
        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            baseWriter.WriteDocType(name, pubid, sysid, subset);
        }
        #endregion

        #region WriteEndAttribute
        public override void WriteEndAttribute()
        {
            baseWriter.WriteEndAttribute();
        }
        #endregion

        #region WriteEndDocument
        public override void WriteEndDocument()
        {
            baseWriter.WriteEndDocument();
        }
        #endregion

        #region WriteEntityRef
        public override void WriteEntityRef(string name)
        {
            baseWriter.WriteEntityRef(name);
        }
        #endregion

        #region WriteProcessingInstruction
        public override void WriteProcessingInstruction(string name, string text)
        {
            baseWriter.WriteProcessingInstruction(name, text);
        }
        #endregion

        #region WriteRaw
        public override void WriteRaw(string data)
        {
            baseWriter.WriteRaw(data);
        }
        #endregion

        #region WriteRaw
        public override void WriteRaw(char[] buffer, int index, int count)
        {
            baseWriter.WriteRaw(buffer, index, count);
        }
        #endregion

        #region WriteStartAttribute
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            baseWriter.WriteStartAttribute(prefix, localName, ns);
        }
        #endregion

        #region WriteStartDocument
        public override void WriteStartDocument(bool standalone)
        {
            baseWriter.WriteStartDocument(standalone);
        }
        #endregion

        #region WriteStartDocument
        public override void WriteStartDocument()
        {
            baseWriter.WriteStartDocument();
        }
        #endregion

        #region WriteStartElement
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            baseWriter.WriteStartElement(prefix, localName, ns);
        }
        #endregion

        #region WriteState
        public override WriteState WriteState
        {
            get { return baseWriter.WriteState; }
        }
        #endregion

        #region WriteString
        public override void WriteString(string text)
        {
            baseWriter.WriteString(text);
        }
        #endregion

        #region WriteSurrogateCharEntity
        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            baseWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }
        #endregion

        #region WriteWhitespace
        public override void WriteWhitespace(string ws)
        {
            baseWriter.WriteWhitespace(ws);
        }
        #endregion

    }
}
