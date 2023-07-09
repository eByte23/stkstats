using HtmlAgilityPack;

namespace HtmlAgilityPack
{

    public static class HtmlAgilityPackExtensions
    {
        public const string ELEMENT_CONTAINS_PATH = ".//{0}[contains({1}, '{2}')]";
        public const string ELEMENT_EQUALS_PATH = ".//{0}[{1}='{2}']";
        public static HtmlNode SelectById(this HtmlNode node, string elementId)
        {
            string xpath = string.Format(
               ELEMENT_CONTAINS_PATH,
               /* All elements */
               "*",
               "@id",
               elementId
           );

            return node.SelectSingleNode(xpath);
        }

        public static HtmlNodeCollection SelectNodesContainingClass(this HtmlNode node, string className, string nodeType = "*")
        {
            string xpath = string.Format(
                ELEMENT_CONTAINS_PATH,
                /* All elements */
                nodeType,
                "@class",
                className
            );

            return node.SelectNodes(xpath);
        }
    }
}