using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Settings;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class RaidEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public RaidEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            string icon = Parser.Raiders >= SettingsManager.Configuration.LargeRaid ? ":bell: " : "";

            // Raid message
            await DiscordClient.SendRaidMessageAsync($"{icon}**{Parser.FromUser}** raided with **{Parser.Raiders}** viewers");

        }

    }

}
