using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UEngineBot.Module.HelperModule;
using UEngineBot.Module.UFinderModule;

namespace UEngineBot
{
    public class UEngineBotCore
    {
        /** 봇 클라이언트 객체를 나타냅니다. */
        private DiscordSocketClient _Client;

        /** 명령어 수신 클라이언트 객체를 나타냅니다. */
        private CommandService _Commands;

        /** 
         * 봇의 진입점을 실행합니다. 
         * Bot 의 웬만한 작업은 비동기적으로 처리되므로 비동기 함수를 작성하여 실행합니다.
         */
        private static void Main(string[] args) => new UEngineBotCore().BotEntry().GetAwaiter().GetResult();
        private async Task BotEntry()
        {

            /** 봇 소켓 기본 설정 */
            DiscordSocketConfig newDiscordSocketConfig = new DiscordSocketConfig();
            /** 로그 레벨을 설정합니다. */
            newDiscordSocketConfig.LogLevel = LogSeverity.Verbose;



            /** 봇 명령어 서비스 기본 설정*/
            CommandServiceConfig newCommandServiceConfig = new CommandServiceConfig();
            /** 로그 레벨을 설정합니다. */
            newCommandServiceConfig.LogLevel = LogSeverity.Verbose;



            /** 디스코드 봇을 초기화합니다. */
            _Client = new DiscordSocketClient(newDiscordSocketConfig);

            /** 명령어 수신 클라이언트 초기화. */
            _Commands = new CommandService(newCommandServiceConfig);



            /** 로그 수신 시 호출될 메서드를 바인딩합니다. */
            _Client.Log += OnClientLogReceived;
            _Commands.Log += OnClientLogReceived;

            /** 봇 토큰을 이용하여 디스코드 서버에 로그인합니다. */
            await _Client.LoginAsync(TokenType.Bot, 
                "ODk0NTQ1MzgxMjAxOTQwNTAy.YVrkTg.Lr3JyhcAfrR_TDlIKpgP5Wvr1bY");

            /** 봇이 이벤트를 수신하도록 합니다. */
            await _Client.StartAsync();

            /** 봇이 메시지를 수신할 경우 호출될 메서드를 바인딩합니다. */
            _Client.MessageReceived += OnClientMessage;

            await _Commands.AddModuleAsync<HelperModule>(null);
            await _Commands.AddModuleAsync<UFinderModule>(null);

            /** 봇이 종료되지 않도록 블로킹 시킵니다. */
            await Task.Delay(-1);
        }

        /** 로그 수신 시 호출되는 메서드입니다. */
        private Task OnClientLogReceived(LogMessage message)
        {
            Console.WriteLine(message.ToString());

            return Task.CompletedTask;
        }

        /** 클라이언트에서 메시지를 수신하는 경우 호출되는 메서드입니다. */
        private async Task OnClientMessage(SocketMessage socketMessage)
        {
            /** 사용자가 보낸 메시지 객체를 얻습니다. */
            SocketUserMessage socketUserMessage = socketMessage as SocketUserMessage;

            /** 사용자가 보낸 메시지가 아닌 경우 함수 호출 종료. */
            if (socketUserMessage == null) return;



            /** 봇 채널에서 수신되었음을 확인하고, 봇 채널이 아니라면 함수 호출 종료. */
            bool receivedOnBotChannel = socketUserMessage.Channel.Name == "봇";
            if (!receivedOnBotChannel) return;



                /** 봇이 작성한 명령어인 경우 함수 호출 종료. */
            bool authorIsBot = socketUserMessage.Author.IsBot;
            if (authorIsBot) return;
            


            /** 명령어 사용 심볼을 나타냅니다. */
            char commandSimnol = '~';

            /** 명령어 심볼의 위치를 나타냅니다. */
            int commandSimbolPosition = 0;

            /** 작성한 메시지 내부에 명령어 심볼이 포함되어있는지 확인하고, 심볼이 포함되지 않은 경우 함수 호출 종료. */
            bool isCommand = socketUserMessage.HasCharPrefix(commandSimnol, ref commandSimbolPosition);

            /** 메시지가 사용자의 멘션 문자열로 시작하지 않는다면 함수 호출 종료. */
            bool hasMentionPrefixed = socketUserMessage.HasMentionPrefix(_Client.CurrentUser, ref commandSimbolPosition);
            if (!isCommand && !hasMentionPrefixed) return;
            



            /** 수신된 메시지에 대한 Context 를 생성합니다. */
            SocketCommandContext socketCommandContext = new SocketCommandContext(
                _Client, socketUserMessage);

            /** 모듈이 명령어를 처리할수 있도록 합니다. */
            var result = await _Commands.ExecuteAsync(
                socketCommandContext, commandSimbolPosition, null);

            /** 수신된 명령어를 재전송합니다. */
            //await socketCommandContext.Channel.SendMessageAsync(
            //    $"[{socketUserMessage.Channel.Name}]Command Received : {socketUserMessage.Content}");

        }
    }

}
