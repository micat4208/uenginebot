using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UEngineBot.Module.HelperModule
{
    /** 도우미 모듈입니다. */
    public class HelperModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("도움", "?", "Command", "Commands")]
        public async Task HelpCommand()
        {
            //await Context.Channel.SendMessageAsync("Help Test");

            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder.Title = "명령어";
            embedBuilder.Description = "명령어는 '~' 심볼로 시작합니다.";

            embedBuilder.AddField(
                "\nHelp / 도움 / ? / Command / Commands", 
                "도움말을 표시합니다.");

            embedBuilder.AddField(
                "\nUnity keyword",
                "유니티 문서 내에서 keyword 에 대한 내용을 찾습니다.");

            embedBuilder.AddField(
                "\nUnreal keyword",
                "언리얼 문서 내에서 keyword 에 대한 내용을 찾습니다.");

            await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }


    }
}
