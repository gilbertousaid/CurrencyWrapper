using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Xml;
using System.Net;


namespace CurrencySearch.BL
{

    public class parseHTML
    {
        private List<string> ParseHtml(string html, string token)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var tokensFound = htmlDoc.DocumentNode.Descendants("li")
                    .Where(node => !node.GetAttributeValue("class", "").Contains(token)).ToList();

            List<string> tokenList = new List<string>();

            foreach (var t in tokensFound)
            {
                if (t.FirstChild.Attributes.Count > 0)
                    tokenList.Add(t.FirstChild.Attributes[0].Value);
            }
            return tokenList;
        }
    }

    public class Utilities
    {
        public string mapCurrency(string currencyIn)
        {
            switch (currencyIn.ToUpper())
            {
                case "USD":
                    return "02";
                    break;
                case "EU":
                    return "24";
                    break;
            }
            return string.Empty;
        }
        public Boolean validDate(string dateToValidate) {
            
            string[] format = { "yyyyMMdd" };
            DateTime date;

            if (DateTime.TryParseExact(dateToValidate,
                                       format,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out date))
            {
                return true;
            }
            return false;
        }

        public Boolean validateInput(string startDate, string endDate, string currency) {
            if (!validDate(startDate)){
                return false;
            }
            if (!validDate(endDate))
            {
                return false;
            }
            if (mapCurrency(currency) != string.Empty){
                return false;
            }
            return true;
        }
        public ArrayList buildTerms(string date1, string date2, string currency)
        {
            var parameters = new ArrayList();
            parameters.Add(date1.Substring(0, 4)); // Year 1 // 0
            parameters.Add(date1.Substring(4, 2)); // Month 1 // 1
            parameters.Add(date1.Substring(6, 2)); // Day 1 // 2

            parameters.Add(date2.Substring(0, 4)); // Year 2 // 3
            parameters.Add(date2.Substring(4, 2)); // Month 2 // 4
            parameters.Add(date2.Substring(6, 2)); // Day 2 // 5

            parameters.Add(mapCurrency(currency)); // 6

            return parameters;
        }

        public string sanitizeHtml(string input) {
            string replacement = input.Replace("\t", "").Replace("\n","").Replace("\r","");
            return replacement;
        }

        public string OnlyCurl(string URLStr)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(URLStr).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;                    
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    return responseString;
                }
            }
            return String.Empty;
        }

        public List<string> ParseHtml(string html, string token)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var tokensFound = htmlDoc.DocumentNode.Descendants("td")
                    .Where(node => node.GetAttributeValue("class", "").Contains(token)).ToList();

            List<string> tokenList = new List<string>();

            foreach (var t in tokensFound)
            {
              tokenList.Add(t.InnerText);
            }
            return tokenList;
        }

        public List<string> meanMaxMin(List<string> exchange) {
            List<string> exchangeList = new List<string>();
            float max = 0.00f;
            float min = 0.00f;
            float mean = 0.00f;
            int counter = 0;
            foreach (var e in exchange) {
                try
                {
                    var fvalue = float.Parse(e.Trim());
                    if (fvalue > max) {
                        max = fvalue;
                    }
                    if (min == 0) {
                        min = max;
                    }
                    if (fvalue < min) {
                        min = fvalue;
                    }
                    mean = mean + fvalue;
                }
                catch { 
                    // invalid value
                }
                counter++;
            }
            if (counter > 0)
            {
                mean /= counter;
                exchangeList.Add(mean.ToString());
                exchangeList.Add(max.ToString());
                exchangeList.Add(min.ToString());
            }
            return exchangeList;
        }

    }
    public class Interface
    {
        Utilities utils = new Utilities();
        private string BanguatCall(ArrayList terms)
        {
            string URLStr = String.Format("https://www.banguat.gob.gt/cambio/historico.asp?kmoneda={0}&ktipo=5&kdia={1}&kmes={2}&kanio={3}&kdia1={4}&kmes1={5}&kanio1={6}&submit1=Consultar", 
                terms[6], terms[2], terms[1], terms[0], terms[5], terms[4], terms[3]);
            return utils.OnlyCurl(URLStr);
        }

        public string singleDate(string singleDate, string currency)
        {
            var newSearchTerm = utils.buildTerms(singleDate, singleDate, currency);            
            var htmlFound = BanguatCall(newSearchTerm);
            var sanitizedHtml = utils.sanitizeHtml(htmlFound);
            return String.Format("The Rate Exchange for the currency {0} at {1} is {2}",currency, singleDate, 
                String.Join(",", utils.ParseHtml(sanitizedHtml, "coltexto")));
        }

        public string rangeOfDates(string startDate, string endDate, string currency)
        {
            var newSearchTerm = utils.buildTerms(startDate, endDate, currency);
            var htmlFound = BanguatCall(newSearchTerm);
            var sanitizedHtml = utils.sanitizeHtml(htmlFound);
            var listToReturn = utils.meanMaxMin(utils.ParseHtml(sanitizedHtml, "coltexto"));
            return String.Format("For the currency {0} in date ranges {1} to {2} has the mean value of {3} and the max of {4} and the min of {5}",
                currency, startDate, endDate, listToReturn[0], listToReturn[1], listToReturn[2]);
        }
    }
}