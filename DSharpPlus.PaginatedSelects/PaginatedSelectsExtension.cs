using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

// to anyone who might read this code;
//   i am not paying your hospital fees
//   i am sorry
//   pls make a pr if you can

namespace DSharpPlus.PaginatedSelects
{
	public class PaginatedSelectsExtension : BaseExtension
	{
		private DiscordClient _client;
		private PaginatedSelectsConfiguration _config;
		private Dictionary<string, PaginatedSelect> _options;

		private int OptionsPerPage => PaginatedSelect.OptionsPerPage;

		public PaginatedSelectsExtension(PaginatedSelectsConfiguration? configuration = null)
		{
			_config = configuration ?? new PaginatedSelectsConfiguration();
		}

		protected override void Setup(DiscordClient client)
		{
			if (_client != null)
				throw new InvalidOperationException("DONT RUN SETUP MORE THAN ONCE");

			_client = client;
			_config = new PaginatedSelectsConfiguration();

			_options = new();

			_client.ComponentInteractionCreated += OnComponent;
		}

		public List<DiscordSelectComponentOption> GetOptions(string customId, int page = 0)
		{
			PaginatedSelect paginated = _options[customId];
			if (paginated.Options.Count <= OptionsPerPage) return paginated.Options;

			var options = paginated.Options.Skip(page * OptionsPerPage).Take(OptionsPerPage).ToList();

			if(page + 1 != paginated.PageCount)
			{
				var opt = _config.NextPageOption;
				opt = new DiscordSelectComponentOption(
					Format(opt.Label, page + 1),
					$"{_config.ValuePrefix}{page + 1}{_config.ValuePrefix}",
					Format(opt.Description, page + 1),
					false,
					opt.Emoji);
				options.Add(opt);
			}

			if (page != 0)
			{
				var opt = _config.PreviousPageOption;
				opt = new DiscordSelectComponentOption(
					Format(opt.Label, page - 1),
					$"{_config.ValuePrefix}{page - 1}{_config.ValuePrefix}",
					Format(opt.Description, page - 1),
					false,
					opt.Emoji);
				options.Insert(0, opt);
			}
			return options;

			string Format(string input, int p = 0)
			{
				if (p == 0) p = page;
				return input.Replace("{page}", p.ToString())
					.Replace("{pagecount}", paginated.PageCount.ToString());
			}
		}

		public void AddPaginatedSelect(string customId, PaginatedSelect select)
		{
			if (select.Placeholder == null) select.Placeholder = _config.DefaultPlaceholder;
			_options[customId] = select;
		}

		private Task OnComponent(object sender, ComponentInteractionCreateEventArgs e)
		{
			// warning, shitcode. im really sorry. -dennis
			_ = Task.Run(() =>
			{
				if (!_options.ContainsKey(e.Id)) return;

				PaginatedSelect paginatedSelect = _options[e.Id];

				if (!e.Values[0].StartsWith(_config.ValuePrefix))
				{
					if (_config.AutoRemoveSelects && paginatedSelect.AutoRemoveSelect != false)
						_options.Remove(e.Id);
					return;
				}
				// lazy method
				string val = e.Values[0];
				if (_config.ValuePrefix.Length > 0) val = val.Replace(_config.ValuePrefix, ""); //shitcode
				bool isValid = int.TryParse(val, out int pageNum);
				if (!isValid) return;

				var renderedSelect = BuildSelect(e.Id, pageNum);
				
				if (paginatedSelect.CustomRender != null)
				{
					paginatedSelect.CustomRender(new CustomRenderContext()
					{
						Id = e.Id,
						PaginatedSelect = paginatedSelect,
						Select = renderedSelect,
						Interaction = e.Interaction,
					});
				}
				else
				{
					e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
						new DiscordInteractionResponseBuilder()
						.AddEmbeds(e.Message.Embeds)
						.AddComponents(Utils.ReplaceComponent(e.Message.Components, e.Id, renderedSelect)));
				}
			});
			return Task.CompletedTask;
		}

		public DiscordSelectComponent CreateSelect(List<DiscordSelectComponentOption> options)
			=> CreateSelect("_paginated_" + Guid.NewGuid().ToString(), options);
		public DiscordSelectComponent CreateSelect(string customId, List<DiscordSelectComponentOption> options)
			=> CreateSelect(customId, "", options);
		public DiscordSelectComponent CreateSelect(string customId, string placeholder, List<DiscordSelectComponentOption> options)
		{
			var paginated = new PaginatedSelect()
			{
				Options = options,
			};
			if (placeholder.Length > 0) paginated.Placeholder = placeholder;
			AddPaginatedSelect(customId, paginated);
			return BuildSelect(customId);
		}

		public DiscordSelectComponent BuildSelect(string customId, int page = 0)
		{
			if (!_options.ContainsKey(customId)) throw new KeyNotFoundException($"Paginated select with custom id {customId} doesnt exist");
			var paginatedSelect = _options[customId];

			return new DiscordSelectComponent(customId, Format(paginatedSelect.Placeholder + _config.PlaceholderSuffix, page, paginatedSelect.PageCount), GetOptions(customId, page));
		}

		private static string Format(string input, int page, int pagecount)
		{
			return input.Replace("{page}", (page + 1).ToString())
				.Replace("{pagecount}", pagecount.ToString());
		}

		public static class Utils
		{
			public static List<DiscordActionRowComponent> ReplaceComponent(IReadOnlyCollection<DiscordActionRowComponent> components, string customId, DiscordComponent component)
				=> ReplaceComponent(components.ToList(), customId, component);
			public static List<DiscordActionRowComponent> ReplaceComponent(List<DiscordActionRowComponent> components, string customId, DiscordComponent component)
			{
				List<DiscordActionRowComponent> rows = new();
				foreach (var oldrow in components)
				{
					List<DiscordComponent> comps = new();
					foreach(var oldcomp in oldrow.Components)
						comps.Add(oldcomp.CustomId == customId ? component : oldcomp);
					rows.Add(new(comps));
				}
				return rows;
			}
		}
	}
}