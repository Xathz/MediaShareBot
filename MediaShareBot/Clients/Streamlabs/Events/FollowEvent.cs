using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class FollowEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public FollowEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            // Follow message
            await DiscordClient.SendFollowMessageAsync($"**{Parser.FromUser}** followed");

        }

    }

}
