using DiscordBot.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    [Group("troll"), Aliases("asdf")]
    internal class Lol
    {
        Dependencies dep;
        public Lol(Dependencies d)
        {
            this.dep = d;
        }

        [Command("lol")]
        public async Task ConfirmationAsync(CommandContext ctx)
        {
           await ctx.RespondAsync("▒▒▒▒▒▒▒▒▒▄▄▄▄▒▒▒▒▒▒▒");
            await ctx.RespondAsync("▒▒▒▒▒▒▄▀▀▓▓▓▀█▒▒▒▒▒▒");
            await ctx.RespondAsync("▒▒▒▒▄▀▓▓▄██████▄▒▒▒▒");
            await ctx.RespondAsync("▒▒▒▄█▄█▀░░▄░▄░█▀▒▒▒▒");
            await ctx.RespondAsync("▒▒▄▀░██▄░░▀░▀░▀▄▒▒▒▒");
            await ctx.RespondAsync("▒▒▀▄░░▀░▄█▄▄░░▄█▄▒▒▒");
            await ctx.RespondAsync("▒▒▒▒▀█▄▄░░▀▀▀█▀▒▒▒▒▒");
            await ctx.RespondAsync("▒▒▒▄▀▓▓▓▀██▀▀█▄▀▀▄▒▒");
            await ctx.RespondAsync("▒▒█▓▓▄▀▀▀▄█▄▓▓▀█░█▒▒");
            await ctx.RespondAsync("▒▒▀▄█░░░░░█▀▀▄▄▀█▒▒▒");
            await ctx.RespondAsync("▒▒▒▄▀▀▄▄▄██▄▄█▀▓▓█▒▒");
            await ctx.RespondAsync("▒▒█▀▓█████████▓▓▓█▒▒");
            await ctx.RespondAsync("▒▒█▓▓██▀▀▀▒▒▒▀▄▄█▀▒▒");
            await ctx.RespondAsync("▒▒▒▀▀▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒");
        }
    }
}