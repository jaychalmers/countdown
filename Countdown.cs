using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Minnoq.Countdown
{
    public class Countdown
    {
        private readonly DateTime WeddingDate = new DateTime(2023, 08, 08);

        [FunctionName("Countdown")]
        public void Run([TimerTrigger("0 0 7 * * *")]TimerInfo myTimer, ILogger log)
        {
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            string fromNumber = Environment.GetEnvironmentVariable("FROM_NUMBER");
            string toNumber = Environment.GetEnvironmentVariable("TO_NUMBER");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: GetBody(),
                from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(toNumber)
            );
        }

        private string GetBody()
        {
            var now = DateTime.UtcNow;
            return $"It's {WeddingDate.Subtract(now).Days} days til our wedding!";
        }
    }
}
