using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSharpPlus.PaginatedSelects
{
	public class PaginatedSelect
	{
		public static int OptionsPerPage => 23;

		public string Placeholder;
		public List<DiscordSelectComponentOption> Options = new();
		public int PageCount => (int)Math.Ceiling((double)Options.Count / OptionsPerPage);

		/*
		public List<DiscordSelectComponentOption> GetPage(int page = 0)
		{
			var options = GetPageRaw(page);

		}

		public List<DiscordSelectComponentOption> GetPageRaw(int page)
			=> Options.Skip(page * OptionsPerPage).Take(OptionsPerPage).ToList();*/
	}
}
