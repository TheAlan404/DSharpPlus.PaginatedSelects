using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSharpPlus.PaginatedSelects
{
	public static class DiscordClientExtensions
	{
		public static PaginatedSelectsExtension UsePaginatedSelects(this DiscordClient client,
			PaginatedSelectsConfiguration? configuration = null)
		{
			PaginatedSelectsExtension ext = new(configuration);
			client.AddExtension(ext);
			return ext;
		}

		public static PaginatedSelectsExtension GetPaginatedSelects(this DiscordClient client) => client.GetExtension<PaginatedSelectsExtension>();
	}
}
