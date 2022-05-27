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

			var paginatedSelectsEx = ctx.Client.GetPaginatedSelects();

			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent("This is the content!")
				.AddEmbed(new DiscordEmbedBuilder()
					.WithColor(DiscordColor.Blurple)
					.WithTitle("Emoji List")
					.WithAuthor($"{Program.pselect.Options.Count} emojis")
					.WithDescription("These are all the emojis!")
					.WithFooter("Taken from DSharpPlus.DiscordEmoji"))
				.AddComponents(paginatedSelectsEx.BuildSelect("theSelect"))
				.AddComponents(new DiscordButtonComponent(ButtonStyle.Success, "__", "Test Button", true),
					new DiscordLinkButtonComponent("https://denvis.glitch.me/", "testetstetst")));
		}
	}
}
