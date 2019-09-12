using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace MediaShareBot.Clients.Discord.Attributes {

    public class AdministratorsOnly : PreconditionAttribute {

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services) {
            if (context.User is SocketGuildUser user) {

                if (user.Roles.Any(x => x.Permissions.Administrator)) {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }

                return Task.FromResult(PreconditionResult.FromError($"{user.Mention} you must be an administrator to run this command."));
            }

            return Task.FromResult(PreconditionResult.FromError($"{context.User.Mention} you must be in a guild to run this command."));
        }

    }

}
