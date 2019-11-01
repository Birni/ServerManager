using DiscordBot.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArkServer;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    [Group("shop"), Aliases("items")]
    internal class ShopItems
    {
        Dependencies dep;
        public ShopItems(Dependencies d)
        {
            this.dep = d;
        }

        [Command("itemlist")]
        public async Task PostItemListAsync(CommandContext ctx)
        {

            //foreach (KeyValuePair<string, Server> server in ServerCollection.MServerCollection.GetCollection())
            //{

            //}


            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#FF0000"),
                Title = "An exception occured when executing a command",
                Description = "test",
                Timestamp = DateTime.UtcNow
            };

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}