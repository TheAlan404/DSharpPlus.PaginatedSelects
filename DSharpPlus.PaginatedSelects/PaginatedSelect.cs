using DSharpPlus.Entities;
using System;
using System.Collections.Generic;

namespace DSharpPlus.PaginatedSelects
{
	public class PaginatedSelect
	{
		public static int OptionsPerPage => 20;

		public string Placeholder;
		public List<DiscordSelectComponentOption> Options = new();
		public int PageCount => (int)Math.Ceiling((double)Options.Count / OptionsPerPage);

		/// <summary>
		/// If set to true, will remove itself when user selects the option.
		/// If set to false, will keep itself on the paginated selects list.
		/// Defaults to the config value <see cref="PaginatedSelectsConfiguration.AutoRemoveSelects"/> if null.
		/// </summary>
		public bool? AutoRemoveSelect = null;

		public Action<CustomRenderContext> CustomRender;

		public PaginatedSelect() {}

		public PaginatedSelect(List<DiscordSelectComponentOption> options)
		{
			Options = options;
		}
	}
}
