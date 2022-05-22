using DSharpPlus.Entities;
using System;

namespace DSharpPlus.PaginatedSelects
{
	public class PaginatedSelectsConfiguration
	{
		public DiscordSelectComponentOption NextPageOption =
			new DiscordSelectComponentOption("Next Page", "_", "Go to the next page", false, new DiscordComponentEmoji("➡️"));
		public DiscordSelectComponentOption PreviousPageOption =
			new DiscordSelectComponentOption("Previous Page", "_", "Go to the previous page", false, new DiscordComponentEmoji("⬅️"));
		public string ValuePrefix = "#";
		public string ValueSuffix = "";
		public string Placeholder = "Select an option (page {page}/{pagecount})";
	}
}