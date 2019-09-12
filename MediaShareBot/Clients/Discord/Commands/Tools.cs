using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MediaShareBot.Clients.Discord.Attributes;
using MediaShareBot.Extensions;

namespace MediaShareBot.Clients.Discord.Commands {

    [RequireContext(ContextType.Guild)]
    public class Tools : ModuleBase {

        [Command("ping")]
        public Task Ping() {
            DateTime now = DateTime.Now.ToUniversalTime();

            return ReplyAsync($"{now.Subtract(Context.Message.CreatedAt.DateTime).TotalMilliseconds.ToString("N0")}ms" +
                $"```Ping: {Context.Message.CreatedAt.DateTime.ToString(Constants.DateTimeFormatFull)}" +
                $"{Environment.NewLine}" +
                $"Pong: {now.ToString(Constants.DateTimeFormatFull)}```");
        }

        [Command("emotes")]
        [AdministratorsOnly]
        public async Task Emotes() {
            IReadOnlyCollection<GuildEmote> emotes = Context.Guild.Emotes;
            StringBuilder builder = new StringBuilder();

            foreach (GuildEmote emote in emotes) {
                List<string> allowedRoleNames = new List<string>();
                foreach (ulong id in emote.RoleIds) {
                    allowedRoleNames.Add(Context.Guild.Roles.FirstOrDefault(x => x.Id == id).Name);
                }
                string allowedRoles = allowedRoleNames.Count > 0 ? string.Join(", ", allowedRoleNames) : "Everyone";

                List<string> flagsList = new List<string>();
                if (emote.Animated) { flagsList.Add("Animated"); }
                if (emote.IsManaged) { flagsList.Add("Managed"); }

                builder.AppendLine(emote.Name);
                builder.AppendLine($"● {emote.Id}");
                builder.AppendLine($"● {emote.CreatedAt.DateTime.ToString(Constants.DateTimeFormatShort).ToLower()} utc");
                if (flagsList.Count > 0) {
                    builder.AppendLine($"● Flags: {string.Join(", ", flagsList)}");
                }
                builder.AppendLine($"● Usable by: {allowedRoles}");
            }

            List<string> chunks = builder.ToString().SplitIntoChunksPreserveNewLines(1990);

            foreach (string chunk in chunks) {
                await ReplyAsync($"```{chunk}```");
            }
        }

        [Command("roles")]
        [AdministratorsOnly]
        public async Task Roles() {
            List<IRole> roles = Context.Guild.Roles.OrderByDescending(x => x.Position).ToList();
            StringBuilder builder = new StringBuilder();

            foreach (IRole role in roles) {
                builder.AppendLine(role.Name);
                builder.AppendLine($"● {role.Id}");
                builder.AppendLine($"● {role.CreatedAt.DateTime.ToString(Constants.DateTimeFormatShort).ToLower()} utc");
                builder.AppendLine($"● {(role.Color.RawValue == 0 ? "#99AAB5" : role.Color.ToString())}");
            }

            List<string> chunks = builder.ToString().SplitIntoChunksPreserveNewLines(1990);

            foreach (string chunk in chunks) {
                await ReplyAsync($"```{chunk}```");
            }
        }

    }

}
