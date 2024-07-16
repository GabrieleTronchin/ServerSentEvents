using Bogus;

namespace ServerSideEvents
{
    public class MoqDataRetrieverService
    {

        public List<NotificationMessage> GetNewMessages() {

            Task.Delay(1000).Wait();
          
           return new Faker<NotificationMessage>()
                .RuleFor("Title", f => f.Random.String())
                .RuleFor("Message", f => f.Random.String())
                .GenerateBetween(5, 15);
        }

    }
}
