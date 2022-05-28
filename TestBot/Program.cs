using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.PaginatedSelects;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace TestBot
{
	public static class Program
	{
		private static DiscordClient _client;
		public static PaginatedSelect pselect;

		private static async Task Main()
		{
			string token = Environment.GetEnvironmentVariable("TOKEN");
			if (token == null)
			{
				Console.WriteLine(
					"Please add a valid Discord bot token in the TOKEN environment variable, and then run this executable again");
				Environment.Exit(1);
			}

			_client = new DiscordClient(new DiscordConfiguration
			{
				Token = token,
				MinimumLogLevel = LogLevel.Trace
			});

			PaginatedSelectsExtension paginatedSelects = _client.UsePaginatedSelects(new PaginatedSelectsConfiguration() { });

			SlashCommandsExtension slash = _client.UseSlashCommands();

			slash.RegisterCommands<SlashCommands>(705114431721570366);

			slash.SlashCommandErrored += (sender, args) =>
			{
				if (args.Exception is SlashExecutionChecksFailedException fail)
					args.Context.CreateResponseAsync(string.Join("\n", fail.FailedChecks.Select(x => x.GetType().Name)));
				else
					args.Context.CreateResponseAsync(Formatter.Sanitize(args.Exception.ToString()));
				return Task.CompletedTask;
			};

			slash.ApplicationCommandRegistered += (sender, args) =>
			{
				_client.Logger.LogInformation($"Registered {args.Commands.Count()} commands for {args.GuildId} ({(args.GuildId is null or 0 ? "global" : _client.Guilds[args.GuildId.Value].Name)})");
				return Task.CompletedTask;
			};

			slash.ApplicationCommandRegisterFailed += (sender, args) =>
			{
				_client.Logger.LogError(args.Exception, $"Failed to register application commands for {args.GuildId} ({(args.GuildId is null or 0 ? "global" : _client.Guilds[args.GuildId.Value].Name)})");
				return Task.CompletedTask;
			};

			_client.ClientErrored += (_, args) =>
			{
				if (args.Exception is BadRequestException exception)
					_client.Logger.LogCritical(exception.Errors);
				return Task.CompletedTask;
			};

			SetupSelect();

			await _client.ConnectAsync();
			await Task.Delay(-1);
		}

		private static void SetupSelect()
		{
			List<DiscordSelectComponentOption> options = new();
			var prop = typeof(DiscordEmoji).GetProperty("UnicodeEmojis", BindingFlags.NonPublic | BindingFlags.Static);
			if (prop == null) throw new ArgumentNullException(nameof(prop));
			int i = 1;
			var dict = (Dictionary<string, string>)prop.GetValue(null) ?? new();
			foreach (KeyValuePair<string, string> kvp in dict)
			{
				options.Add(new DiscordSelectComponentOption(kvp.Key, kvp.Key.Replace(":", ""), $"emoji #{i}", false, new DiscordComponentEmoji(kvp.Value)));
				i++;
			}

			Console.WriteLine("Paginated select loaded!");

			pselect = new PaginatedSelect(options);

			var paginatedSelects = _client.GetPaginatedSelects();
			paginatedSelects.AddPaginatedSelect("theSelect", pselect);
		}
	}
}