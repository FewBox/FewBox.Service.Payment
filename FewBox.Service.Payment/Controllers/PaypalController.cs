using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FewBox.Core.Web.Filter;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.AspNetCore.Mvc;

namespace FewBox.Service.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaypalController : ControllerBase
    {
        private PaypalConfig PaypalConfig { get; set; }

        public PaypalController(PaypalConfig paypalConfig)
        {
            this.PaypalConfig = paypalConfig;
        }

        [HttpPost]
        [Trace]
        public StatusCodeResult Receive()
        {
            var paymentInfo = new PaymentInfo();
            using(StreamReader reader = new StreamReader(this.Request.Body, Encoding.ASCII))
	        {  
		        paymentInfo.Body = reader.ReadToEnd();
	        }
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                string content = $"cmd=_notify-validate&{paymentInfo.Body}";
                var httpContent = new StringContent(content);
                HttpResponseMessage response = httpClient.PostAsync(this.PaypalConfig.Url, httpContent).Result;
                response.EnsureSuccessStatusCode();
                string responseString = response.Content.ReadAsStringAsync().Result;
                if (responseString.Equals("VERIFIED"))
                {
                    // check that Payment_status=Completed
                    // check that Txn_id has not been previously processed
                    // check that Receiver_email is your Primary PayPal email
                    // check that Payment_amount/Payment_currency are correct
                    // process payment
                }
                else if (responseString.Equals("INVALID"))
                {
                    //Log for manual investigation
                }
                else
                {
                    //Log error
                }
            }
            return Ok();
        }
    }
}