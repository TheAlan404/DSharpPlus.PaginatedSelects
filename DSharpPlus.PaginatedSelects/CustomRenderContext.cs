using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSharpPlus.PaginatedSelects
{
	public class CustomRenderContext
	{
		public string Id;
		public PaginatedSelect PaginatedSelect;
		public DiscordSelectComponent Select;
		public DiscordInteraction Interaction;
	}
}
