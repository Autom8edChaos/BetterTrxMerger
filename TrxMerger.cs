using System.IO;
using System.Xml;

namespace BetterTrxMerger
{
    public static class TrxMerger
    {
        /// <summary>
        /// Extension method to merge a trx file into another. It merges the following nodes:
        /// /TestRun/TestDefinitions, /TestRun/TestEntries, /TestRun/Results
        /// </summary>
        /// <param name="receivingDocument">The receiving document</param>
        /// <param name="donorDocument">Document with nodes to merge into the receiving document</param>
        public static void MergeTrxFile(this XmlDocument receivingDocument, XmlDocument donorDocument)
        {
            string[] copyList = { @"/ns:TestRun/ns:TestDefinitions",
                                  @"/ns:TestRun/ns:TestEntries",
                                  @"/ns:TestRun/ns:Results" };

            foreach (var xPath in copyList)
            {
                CopyNodes(receivingDocument, donorDocument, xPath);
            }
        }

        /// <summary>
        /// Copys the same nodes under a parent node from one document to a second
        /// </summary>
        /// <param name="oDocDonor">Xml Doc to copy the nodes from</param>
        /// <param name="oDocReceiver">Xml Doc to copy the nodes to</param>
        /// <param name="xPath">Generic namespaces are automatically applied. use ns: as namespace for each node.</param>
        /// <returns></returns>
        private static int CopyNodes(XmlDocument oDocReceiver, XmlDocument oDocDonor,  string xPath)
        {
            var namespaceManagerReceiver = new XmlNamespaceManager(oDocReceiver.NameTable);
            namespaceManagerReceiver.AddNamespace("ns", oDocReceiver.DocumentElement.NamespaceURI);
            var namespaceManagerDonor = new XmlNamespaceManager(oDocDonor.NameTable);
            namespaceManagerDonor.AddNamespace("ns", oDocDonor.DocumentElement.NamespaceURI);

            var testDefinitionNode = oDocDonor.SelectSingleNode(xPath, namespaceManagerDonor);
            if (testDefinitionNode == null)
            {
                throw new InvalidDataException("Donor document misses node for xpath " + xPath);
            }
            var testDefinitionNodeReceiver = oDocReceiver.SelectSingleNode(xPath, namespaceManagerReceiver);
            if (testDefinitionNodeReceiver == null)
            {
                throw new InvalidDataException("Receiver document misses node for xpath " + xPath);
            }

            int copied = 0;
            foreach (XmlNode node in testDefinitionNode.ChildNodes)
            {
                XmlNode newChildNode = oDocReceiver.ImportNode(node, true);
                testDefinitionNodeReceiver.AppendChild(newChildNode);
                copied++;
            }
            return copied;
        }
    }
}
