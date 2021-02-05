using JobsFeedScrapper.EventHub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace JobsFeedScrapper.Service
{
    public class TelegramBotService : BackgroundService
    {
        private readonly ILogger<TelegramBotService> _logger;

        private readonly IConfiguration _configuration;

        private readonly IJobsFeedEventHub _eventHub;

        private TelegramBotClient _bot;

        private readonly int _chatId;

        private readonly AsyncLock _mutex = new AsyncLock();

        public TelegramBotService(
            ILogger<TelegramBotService> logger,
            IConfiguration configuration,
            IJobsFeedEventHub eventHub)
        {
            _logger = logger;
            _configuration = configuration;
            _eventHub = eventHub;

            _eventHub.NewJobs += _eventHub_NewJobs;

            _chatId = configuration.GetSection("bot").GetValue<Int32>("chatId");
        }

        private async void _eventHub_NewJobs(object sender, NewJobsEventArgs e)
        {
            using(await _mutex.LockAsync())
            { 
                try
                {
                    foreach (var job in e.Jobs)
                    {
                        await Task.Delay(1000);

                        _logger.LogInformation($"{DateTime.UtcNow} - Sending message");                        

                        var content = job.Content
                            .Replace("<br />", "\n")
                            .Replace("&nbsp;", " ")
                            .Replace("&bull;", "•");

                        var sb = new StringBuilder();
                        sb.Append($"(<b>{e.Feed.Name}) {job.Title}</b>\n\n");
                        sb.Append(content);

                        var message = sb.ToString();
                        message = sb.ToString().Substring(0, Math.Min(4096, message.Length));

                        await _bot.SendTextMessageAsync(
                            new Telegram.Bot.Types.ChatId(_chatId),
                            text: message,
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    }                    
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Telegram Bot Service");

            _bot = new TelegramBotClient(_configuration.GetSection("bot").GetValue<string>("token"));

            _bot.OnMessage += _bot_OnMessage;

            _bot.StartReceiving();

            return Task.CompletedTask;
        }

        private void _bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            _logger.LogInformation($"New message from {e.Message.Chat.Id}");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _bot.StopReceiving();
            _bot = null;

            return Task.CompletedTask;
        }
    }
}
