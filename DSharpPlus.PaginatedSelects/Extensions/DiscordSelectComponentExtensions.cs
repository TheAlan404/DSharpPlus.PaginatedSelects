using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSharpPlus.PaginatedSelects
{
	public static class DiscordSelectComponentExtensions
	{
		public static PaginatedSelect ToPaginatedSelect(this DiscordSelectComponent select)
		{
			return new PaginatedSelect()
			{
				Options = select.Options.ToList(),
				Placeholder = select.Placeholder,
			};
		}
	}
}
