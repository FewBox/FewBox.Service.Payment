using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

        public PaypalController(PaypalConfig paypalConfig, ILogger<PaypalController> logger)
        {
            this.PaypalConfig = paypalConfig;
            this.Logger = logger;
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
                    this.Logger.LogWarning(paymentInfo.Body);
                    // check that Payment_status=Completed
                    // check that Txn_id has not been previously processed
                    // check that Receiver_email is your Primary PayPal email
                    // check that Payment_amount/Payment_currency are correct
                    // process payment
                    if (this.Request.Form["payment_status"] == "Completed")
                    {
                        if (this.Request.Form["receiver_email"] == this.PaypalConfig.Email && this.Request.Form["mc_currency"] == "USD")
                        {
                            // payer_email
                        }
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