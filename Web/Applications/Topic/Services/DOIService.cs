using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace SpecialTopic.Topic.Services
{
    public class DOIService
    {
        private string pubmedSearchUrl = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term=";
        public string GetPaperFromDOI(string doi)
        {            
            doi=doi.ToLower().Replace("<br />"," or ");
            WebClient webclient = new WebClient();
            string pubmedIdList=webclient.DownloadString(pubmedSearchUrl + doi);

            return null;
        }
    }
}