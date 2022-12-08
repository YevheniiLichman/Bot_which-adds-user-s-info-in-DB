using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace dark
{
    class Program
    {
        static ITelegramBotClient
            bot = new TelegramBotClient(""); // token

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var name = update.Message.From.FirstName;
                if (message.Text.ToLower() == "/start")
                {
                    Console.WriteLine($"\nid_user = {update.Id} \t name_user = {update.Message.From.Username} \t lang_user = {update.Message.From.LanguageCode}\n");
                    OperationDatabase operationDatabase = new OperationDatabase();
                    // operationDatabase.DeleteUser(name_user); // видаляємо користувача
                    operationDatabase.insertUser(update.Id, update.Message.From.Username, update.Message.From.LanguageCode);
                    
                    await botClient.SendTextMessageAsync(message.Chat,
                        $"Привіт, {name}! Це бот для пошуку інформації!.\n\nНапишіть своє запитання та ми постараємось відповісти на нього якомога швидше :)\n\n" +
                        $"Для того, щоб побачити найчастіші запитання, тицяй на /menu.");
                    return;
                }


                await MenuButton(botClient, update, cancellationToken);
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            
        }

        public static async Task MenuButton(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                var name = update.Message.From.FirstName;

                if (message == null || message.Type != MessageType.Text) return; // перевіряємо на null

                if (message.Text != null && message.Text.ToLower() == "/menu")
                {
                    InlineKeyboardMarkup replyKeyboardMarkup = new(new[]
                    {
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithUrl(
                                text: "Wikipedia",
                                url: "https://en.wikipedia.org/wiki/Main_Page"
                            )
                        },
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithUrl(
                                text: "Codewars",
                                url: "https://www.codewars.com/"
                            )
                        },
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithUrl(
                                text: "GitHub Yevhenii Lichman",
                                url: "https://github.com/YevheniiLichman"
                            )
                        },
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithUrl(
                                text: "СР ФККПІ",
                                url: "http://sr-fkkpi.nau.edu.ua/#/"
                            )
                        },
                    });

                    await botClient.SendTextMessageAsync(message.Chat, "Оберіть, що вам потрібно:)",
                        replyMarkup: replyKeyboardMarkup);
                }
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine($"Let's go {bot.GetMeAsync().Result.FirstName}!");
            Update update = new Update();
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // отримати всі типи оновлень
            };

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}