using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Minnoq.Countdown
{
    public class Countdown
    {
        private readonly DateTime WeddingDate = new DateTime(2023, 08, 08, 14, 0, 0); //doublee check this is correct time

        [FunctionName("Countdown")]
        public void Run([TimerTrigger("0 0 7 * * *")]TimerInfo myTimer, ILogger log)
        {
            string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            string fromNumber = Environment.GetEnvironmentVariable("NUMBER_FROM");
            string jayNumber = Environment.GetEnvironmentVariable("NUMBER_JAY");
            string monNumber = Environment.GetEnvironmentVariable("NUMBER_MON");

            var recipients = new List<string>
            {
                jayNumber
            };

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: GetBody(),
                from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(jayNumber)
            );
        }

        private string GetBody()
        {
            var sb = new StringBuilder();

            var dayMessage = DayOfWeekMessage();
            sb.Append(dayMessage);
            sb.Append(" ");

            var nextSentenceShouldBeUpperCase = NextSentenceShouldBeUpperCase(dayMessage);

            var span = WeddingDate.Subtract(DateTime.Now);
            sb.Append($"{(nextSentenceShouldBeUpperCase ? 'I' : 'i')}t's {span.Days} days til our wedding!");
            sb.Append(" ");

            sb.Append(RandomGreeting());

            if (DateTime.Now > new DateTime(2022,04,01))
            {
                sb.Append(" You should see this first on April 1");
            }

            return sb.ToString();
        }

        private bool NextSentenceShouldBeUpperCase(string currentSentence)
        {
            var sentenceEnders = new char[]{'.', '!'};
            return sentenceEnders.Any(c => currentSentence.EndsWith(c));
        }

        //no leap year support
        private string DayOfWeekMessage()
        {
            if (DateTime.Now.DayOfYear == 89)
            {
                return "Test - today should be March 30th";
            }

            if (DateTime.Now.DayOfYear == 97)
            {
                return "Happy birthday!!!";
            }

            if (DateTime.Now.DayOfYear == 261)
            {
                return "Happy anniversary!!!";
            }

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                return "It's Monday! Boo! But...";
            }

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                return "It's Friday! Woohoo! And guess what,";
            }

            return $"Happy {DateTime.Now.ToString("dddd")}!";
        }

        private string RandomGreeting()
        {
            var greetings = new string[]
            {
                "",
                "I love you!",
                "You're my favourite!"
            };
            return greetings[new Random().Next(greetings.Length)];
        }
    }
}
