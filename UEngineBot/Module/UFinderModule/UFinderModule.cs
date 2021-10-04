using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace UEngineBot.Module.UFinderModule
{
    public class UFinderModule : ModuleBase<SocketCommandContext>
    {

        public UFinderModule()
        {
            Console.WriteLine("wqasd");

        }

        [Command("Unity")]
        public async Task FindOfUnityReference(string keyword)
        {

            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder.Title = $"유니티 문서 내에서 다음 내용을 찾습니다. [{keyword}]";
            embedBuilder.Description = $"https://docs.unity3d.com/ScriptReference/30_search.html?q={keyword}";

            await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
            
        }

        [Command("Unreal")]
        public async Task FindOfUnrealReference(string keyword)
        {

            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder.Title = $"언리얼 문서 내에서 다음 내용을 찾습니다. [{keyword}]";
            embedBuilder.Description = $"https://www.unrealengine.com/ko/bing-search?keyword={keyword}";

            await Context.Channel.SendMessageAsync("", false, embedBuilder.Build());
        }
    }
}
