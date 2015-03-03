using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using SpecialTopic.Topic;

namespace spt.Utils
{
    public class PubMedService
    {
             /// <summary>
        /// ESearch:  Searches and retrieves primary IDs 
        /// (for use in EFetch, ELink and ESummary) and term translations, 
        /// and optionally retains results for future use in 
        /// the user's environment. 
        /// </summary>
        /// <param name="term">search terms</param>
        /// <returns>xml file</returns>
        public string ESearch(string term)
        {
            if(string.IsNullOrEmpty(term))
                throw new Exception("search item is null");
            string xml = "";
            string server = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?"
                        + "db=pubmed&term={0}&datetype=xml&sort=pub+date&retmax=100";//&field=titl&retmax=100
            server = string.Format(server, term);

            WebClient client=new WebClient();
            xml=client.DownloadString(server);

            return xml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pmid">逗号分隔的pmids</param>
        /// <returns> xml file </returns>
        public string EFetch(string pmid)
        {
            string xml = "";
            string server = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?"
                        + "db=pubmed&id={0}&retmode=xml&rettype=abstract";
                        //&retmax=100
            server = string.Format(server, pmid);

            WebClient client=new WebClient();
            xml=client.DownloadString(server);

            return xml;
        }

        public List<string> ParsePMIDs(string xml)
        {
            if(string.IsNullOrEmpty(xml))
                return null;
            //Debug.Print(xml.Length);
            //Debug.WriteLine(xml);
            List<string> list = new List<string>();
            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(xml);
            }
            catch //(WebException ex)
            {
                xdoc.LoadXml(xml);
            }
            XmlNode cnode = xdoc.SelectSingleNode("/eSearchResult/Count");
            if(cnode ==null)
                return null;
            int total = int.Parse(cnode.InnerText);
            if (total == 0)
            {
                list.Add("-1");

            }
            else
            {
                XmlNodeList nodelist = xdoc.SelectNodes("/eSearchResult/IdList/Id");
                foreach (XmlNode node in nodelist)
                {
                    list.Add(node.InnerText);
                }
            }
            return list;
        }

        public List<PubMedItem> Parse(string xml)
        {
            List<PubMedItem> pmItemList = new List<PubMedItem>();

            if (string.IsNullOrEmpty(xml))
                return pmItemList;
            if (xml.IndexOf("XML not found for id") > -1)
                return pmItemList;
            xml = xml.Replace("http://www.ncbi.nlm.nih.gov/corehtml/query/DTD/pubmed_140101.dtd", "");
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            XmlNodeList articleNodeList = xdoc.SelectNodes("/PubmedArticleSet/PubmedArticle");
            if (articleNodeList.Count < 1)
                return pmItemList;

            foreach (XmlNode articleNode in articleNodeList)
            {
                PubMedItem it = new PubMedItem();
                it.Pmid = articleNode.SelectSingleNode("MedlineCitation/PMID").InnerText;
                it.Title = articleNode.SelectSingleNode("MedlineCitation/Article/ArticleTitle").InnerText;

                XmlNode node = articleNode.SelectSingleNode("MedlineCitation/Article/Abstract/AbstractText");
                if (node != null)
                {
                    it.Abstract = node.InnerText;
                }
                else
                    it.Abstract = "";
                node = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/ISOAbbreviation");
                if (node != null)
                {
                    it.Journal = node.InnerText;
                }
                else
                {
                    it.Journal = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/Title").InnerText;
                }

                node = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/ISSN");
                if (node != null)
                {
                    it.JIssn = node.InnerText;
                }
                else
                {
                    it.JIssn = "";
                }

                node = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/Volume");
                if (node != null)
                {
                    it.Volume = node.InnerText;
                }
                else
                    it.Volume = "";

                node = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/Issue");
                if (node != null)
                {
                    it.Issue = node.InnerText;
                }
                else
                {
                    it.Issue = "";
                }
                //if (it.Volssue.Length > 98)
                //    it.Volssue = it.Volssue.Substring(0, 90);

                node = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/PubDate/Year");
                if (node != null)
                    it.Pubdate = node.InnerText;

                node = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/PubDate/Month");
                if (node != null)
                    it.Pubdate = it.Pubdate + "," + node.InnerText;
                node = articleNode.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/PubDate/Day");
                if (node != null)
                    it.Pubdate += " " + node.InnerText;

                XmlNodeList authorNodelist = articleNode.SelectNodes("MedlineCitation/Article/AuthorList/Author");
                if (authorNodelist == null)
                {
                    it.Authors = "";
                }
                else if (authorNodelist.Count == 0)
                {
                    it.Authors = "";
                }
                else
                {
                    string authors = "";
                    foreach (XmlNode authorNode in authorNodelist)
                    {
                        node=authorNode.SelectSingleNode("./LastName");
                        string lastname = "",forename="";
                        if (node != null)
                            lastname = node.InnerText;
                        node = authorNode.SelectSingleNode("./ForeName");
                        if (node != null)
                            forename= node.InnerText;

                        authors += forename+" "+lastname+",";
                    }
                    authors = authors.Substring(0,authors.Length-1);
                    it.Authors = authors;
                    /*string author = "";
                    node = nodelist[0].SelectSingleNode("./LastName");
                    if (node != null)
                        author = node.InnerText;
                    node = nodelist[0].SelectSingleNode("./Initials");
                    if (node != null)
                        author = author + ", " + node.InnerText;

                    it.Authors = author;

                    if (nodelist.Count > 1)
                    {
                        author = " and ";
                        node = nodelist[1].SelectSingleNode("./LastName");
                        if (node != null)
                            author += node.InnerText;
                        node = nodelist[1].SelectSingleNode("./Initials");

                        if (node != null)
                            author = author + ", " + node.InnerText;

                        it.Authors += author;
                    }*/
                }

                node = articleNode.SelectSingleNode("MedlineCitation/Article/Pagination/MedlinePgn");
                if (node != null)
                {
                    it.Pagination = node.InnerText;

                }
                else
                    it.Pagination = "";

                node=articleNode.SelectSingleNode("PubmedData/ArticleIdList/ArticleId[@IdType='doi']");
                if (node != null)
                {
                    it.Doi = node.InnerText;
                }

                XmlNodeList pubTypeNodeList = articleNode.SelectNodes("MedlineCitation/Article/PublicationTypeList/PublicationType");
                if(pubTypeNodeList==null)
                {
                    it.PublicationType = "";
                }
                else if (pubTypeNodeList.Count < 1)
                {
                    it.PublicationType = "";
                }
                else 
                {
                    string pubtype = "";
                    foreach (XmlNode pubTypeNode in pubTypeNodeList)
                    {
                        pubtype+=pubTypeNode.InnerText+"|";
                    }
                    pubtype = pubtype.Substring(0, pubtype.Length - 1);
                    it.PublicationType = pubtype;
                }
                pmItemList.Add(it);

            }
            return pmItemList;
        }

        public List<PubMedItem> GetCitationsFromDOIs(string dois)
        {
            string pmidxml = ESearch(dois);
            List<string> pmidlist = ParsePMIDs(pmidxml);

            string querypmidlist = "";
            foreach (string pmid in pmidlist)
            {
                querypmidlist += pmid + ",";
            }
            querypmidlist = querypmidlist.Substring(0, querypmidlist.Length - 1);

            string resultxml =  EFetch(querypmidlist);

            List<PubMedItem> list =  Parse(resultxml);

            return list;
        }


    }


    public class PubMedItem
    {
        public string Pmid{get;set;}

        public string Doi { get;set; }

        public string Title{get;set;}

        public string Authors{get;set;}

        public string Abstract{get;set;}

        public string Journal{get;set;}
        public string JIssn { get; set; }

        public string Volume { get; set; }
        public string Issue { get; set; }

        public string Pagination { get; set; }

        public string Pubdate{get;set;}

        public string PublicationType { get; set; }

        public PaperEntity AsPaperEntity()
        {
            return new PaperEntity()
            {
                Doi = this.Doi,
                PmId = this.Pmid,
                Title = this.Title,
                Authors = this.Authors,
                Journal = this.Journal,
                JIssn = this.JIssn,
                Volume = this.Volume,
                Issue = this.Issue,
                Pagination = this.Pagination,
                Pubdate = this.Pubdate,
                PublicationType = this.PublicationType,
                Abstract = this.Abstract,
            };
        }

    }


}
