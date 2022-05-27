using DSharpPlus.Entities;

namespace DSharpPlus.PaginatedSelects
{
	public class PaginatedSelectsConfiguration
	{
		/// <summary>
		/// Select option for the next page option
		/// </summary>
		public DiscordSelectComponentOption NextPageOption =
			new DiscordSelectComponentOption("Next Page", "_", "Go to the next page", false, new DiscordComponentEmoji("➡️"));

		/// <summary>
		/// Select option for the previous page option
		/// </summary>
		public DiscordSelectComponentOption PreviousPageOption =
			new DiscordSelectComponentOption("Previous Page", "_", "Go to the previous page", false, new DiscordComponentEmoji("⬅️"));
		
		/// <summary>
		/// Prefix for the next/previous page value
		/// </summary>
		public string ValuePrefix = "#";

		/// <summary>
		/// The default placeholder for the select components. Can be overriden by specifying placeholders per-paginated-select
		/// </summary>
		public string DefaultPlaceholder = "Select an option";

		/// <summary>
		/// Suffix for select components. Used to show the page.
		/// </summary>
		public string PlaceholderSuffix = " (page {page}/{pagecount})";

		/// <summary>
		/// Wheather to automatically remove paginated selects from the list when they are selected.
		/// Can be overridden per-pagineted-select.
		/// </summary>
		public bool AutoRemoveSelects = true;
	}
}