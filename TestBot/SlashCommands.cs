using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.PaginatedSelects;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
	public class SlashCommands : ApplicationCommandModule
	{
		[SlashCommand("test", "Test the paginated selects!")]
		public async Task TestCommand(InteractionContext ctx)
		{
			await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
				new DiscordInteractionResponseBuilder().AsEphemeral());

			Console.WriteLine("Loading options...");

			List<DiscordSelectComponentOption> options = new()
			{
				new DiscordSelectComponentOption("Skip to 289th page lmao", "#289", "funny")
			};
			var prop = typeof(DiscordEmoji).GetProperty("UnicodeEmojis", BindingFlags.NonPublic | BindingFlags.Static);
			if(prop == null) throw new ArgumentNullException(nameof(prop));
			int i = 1;
			var dict = (Dictionary<string, string>)prop.GetValue(null) ?? new();
			foreach (KeyValuePair<string, string> kvp in dict)
			{
				options.Add(new DiscordSelectComponentOption(kvp.Key, kvp.Key.Replace(":", ""), $"emoji #{i}", false, new DiscordComponentEmoji(kvp.Value)));
				i++;
			}

			Console.WriteLine("Options loaded!");

			var paginatedSelects = ctx.Client.GetPaginatedSelects();

			var select = paginatedSelects.CreateSelect("theSelect", options);

			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent("This is the content!")
				.AddEmbed(new DiscordEmbedBuilder()
					.WithColor(DiscordColor.Blurple)
					.WithTitle("Emoji List")
					.WithAuthor($"{dict.Count} emojis")
					.WithDescription("These are all the emojis!")
					.WithFooter("Taken from DSharpPlus.DiscordEmoji"))
				.AddComponents(select)
				.AddComponents(new DiscordButtonComponent(ButtonStyle.Success, "__", "Test Button", true),
					new DiscordLinkButtonComponent("https://denvis.glitch.me/", "testetstetst")));
		}
	}
}
