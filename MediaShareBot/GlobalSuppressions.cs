// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to this project.
// Project-level suppressions either have no target or are given a specific target and scoped to a namespace, type, member, etc.

// Discord.net will complain the user added a parameter 
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Discord.net will complain the user added a parameter ", Scope = "member", Target = "~M:HeadNonSub.Clients.Discord.Commands.Exclamation.Streamlabs.Prices(System.String)~System.Threading.Tasks.Task")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Discord.net will complain the user added a parameter ", Scope = "member", Target = "~M:HeadNonSub.Clients.Discord.Commands.Exclamation.MeeSix.Warn(Discord.WebSocket.SocketUser,System.String)~System.Threading.Tasks.Task")]

// FlowAnalysis bug, https://git.io/fj9gg https://git.io/fj9g2
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0068:Use recommended dispose pattern", Justification = "FlowAnalysis bug")]
