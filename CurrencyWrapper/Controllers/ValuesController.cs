using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CurrencySearch.BL;

namespace CurrencyWrapper.Controllers
{
    [RoutePrefix("api/currencies")]
    public class ValuesController : ApiController
    {
        // GET api/values

        [HttpGet]
        [Route("date/{date}/{currency}")]
        public string Get(string date, string currency)
        {
            var myUtil = new Utilities();
            if (myUtil.validDate(date))
            {
                if (myUtil.mapCurrency(currency) != string.Empty)
                {
                    var invoker = new Interface();
                    return invoker.singleDate(date, currency);
                }
                else
                {
                    return "No valid currency, use USD or EU";
                }
            }
            else {
                return "Invalid Date Format, use yyyymmdd";
            }
            return string.Empty;
        }

        [HttpGet]
        [Route("daterange/{date1}/{date2}/{currency}")]
        public string GetInRange(string date1, string date2, string currency)
        {
            var myUtil = new Utilities();
            if (myUtil.validDate(date1) && myUtil.validDate(date2))
            {
                if (myUtil.mapCurrency(currency) != string.Empty)
                {
                    var invoker = new Interface();
                    return invoker.rangeOfDates(date1, date2, currency);
                }
                else
                {
                    return "No valid currency, use USD or EU";
                }
            }
            else
            {
                return "Invalid Date Format, use yyyymmdd";
            }
            return string.Empty;
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
