using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FewBox.Core.Utility.Net;
using FewBox.Core.Web.Config;
using FewBox.Core.Web.Dto;
using FewBox.Core.Web.Error;
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
        private IExceptionProcessorService ExceptionProcessorService { get; set; }
        private NotificationConfig NotificationConfig { get; set; }

        public PaypalController(PaypalConfig paypalConfig, IExceptionProcessorService exceptionProcessorService, NotificationConfig notificationConfig)
        {
            this.PaypalConfig = paypalConfig;
            this.ExceptionProcessorService = exceptionProcessorService;
            this.NotificationConfig = notificationConfig;
        }

        [HttpPost]
        [Trace]
        public StatusCodeResult Receive()
        {
            var paymentInfo = new PaymentInfo();
            using (StreamReader reader = new StreamReader(this.Request.Body, Encoding.ASCII))
            {
                paymentInfo.Body = reader.ReadToEnd();
            }
            using (var httpClient = new HttpClient())
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
                this.SendNotification(responseString,"");
            }
            return Ok();
        }

        private void SendNotification(string name, string param)
        {
            Task.Run(() =>
            {
                this.ExceptionProcessorService.TryCatchInConsole(() =>
                {
                    RestfulUtility.Post<NotificationRequestDto, NotificationResponseDto>($"{this.NotificationConfig.Protocol}://{this.NotificationConfig.Host}:{this.NotificationConfig.Port}/api/notification", new Package<NotificationRequestDto>
                    {
                        Headers = new List<Header> { },
                        Body = new NotificationRequestDto
                        {
                            Name = name,
                            Param = param
                        }
                    });
                });
            });
        }
    }
}