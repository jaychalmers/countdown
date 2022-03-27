using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Twilio;

namespace Minnoq.Countdown
{
    public class Countdown
    {
        [FunctionName("Countdown")]
        public void Run([TimerTrigger("0 0 7 * * *")]TimerInfo myTimer, ILogger log)
        {
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            string fromNumber = Environment.GetEnvironmentVariable("FROM_NUMBER");
            string toNumber = Environment.GetEnvironmentVariable("TO_NUMBER");

            TwilioClient.init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "This is a test message from the app",
                from: new Twilio.Types.PhoneNumber(fromNumber),
                toNumber: new Twilio.Types.PhoneNumber(toNumber)
            );
        }
    }
}
