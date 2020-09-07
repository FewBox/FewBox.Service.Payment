using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FewBox.Service.Payment.Domain.Services;
using FewBox.Service.Payment.Model.Configs;
using FewBox.Service.Payment.Model.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FewBox.Service.Payment.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class PaypalController : ControllerBase
    {
        private PaypalConfig PaypalConfig { get; set; }
        private ILogger Logger { get; set; }
        private IPaypalService PaypalService { get; set; }

        public PaypalController(PaypalConfig paypalConfig, ILogger<PaypalController> logger, PaypalServiceBuilder paypalServiceBuilder)
        {
            this.PaypalConfig = paypalConfig;
            this.Logger = logger;
            this.PaypalService = paypalServiceBuilder.Build();
        }

        [HttpPost]
        public async Task<StatusCodeResult> ReceiveAsync()
        {
            var paymentInfo = new PaymentInfo();
            using (StreamReader reader = new StreamReader(this.Request.Body, Encoding.ASCII))
            {
                paymentInfo.Body = await reader.ReadToEndAsync();
            }
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                string content = $"cmd=_notify-validate&{paymentInfo.Body}";
                var httpContent = new StringContent(content);
                HttpResponseMessage response = await httpClient.PostAsync(this.PaypalConfig.Url, httpContent);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();
                if (responseString.Equals("VERIFIED"))
                {
                    // check that Payment_status=Completed
                    // check that Txn_id has not been previously processed
                    // check that Receiver_email is your Primary PayPal email
                    // check that Payment_amount/Payment_currency are correct
                    // process payment
                    NameValueCollection qureyParams = System.Web.HttpUtility.ParseQueryString(WebUtility.UrlDecode(paymentInfo.Body));
                    PaypalContext paypalContext = new PaypalContext(qureyParams);
                    if (paypalContext.PaymentInformation.PaymentStatusType == PaymentStatusType.Completed &&
                    paypalContext.BasicInformation.ReceiverEmail == this.PaypalConfig.ReceiverEmail &&
                    paypalContext.CurrencyAndCurrrencyExchange.MCCurrency == this.PaypalConfig.Currency)
                    {
                        this.PaypalService.HandleIPN(paypalContext);
                    }
                }
                else if (responseString.Equals("INVALID"))
                {
                    this.Logger.LogError(paymentInfo.Body);
                    return Unauthorized();
                }
                else
                {
                    this.Logger.LogError(responseString);
                    return BadRequest();
                }
                this.Logger.LogTrace(@"{0}: {1}", paymentInfo.Body, responseString);
            }
            return Ok();
        }
    }
}