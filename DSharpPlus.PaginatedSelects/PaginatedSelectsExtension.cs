using System;
using System.Diagnostics;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

// to anyone who might read this code;
//   i am not paying your hospital fees
//   i am sorry

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

			int pageCount = (int)Math.Ceiling((double)paginated.Options.Count / OptionsPerPage);
			var options = paginated.Options.Skip(page * OptionsPerPage).Take(OptionsPerPage).ToList();

			if(page + 1 != pageCount)
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
					.Replace("{pagecount}", pageCount.ToString());
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
					if (_config.AutoRemoveOptions)
					{
						_options.Remove(e.Id);
					}
					return;
				}
				// lazy method
				string val = e.Values[0];
				if (_config.ValuePrefix.Length > 0) val = val.Replace(_config.ValuePrefix, ""); //shitcode
				if (_config.ValueSuffix.Length > 0) val = val.Replace(_config.ValueSuffix, "");
				bool isValid = int.TryParse(val, out int pageNum);
				if (!isValid) return;

				var options = GetOptions(e.Id, pageNum);

				Debug.Write(options);
				Debug.Write(options.Count);

				/*
				List<DiscordActionRowComponent> rows = new();

				var oldrows = e.Message.Components.ToList();
				foreach (var oldrow in rows)
				{
					List<DiscordComponent> comps = new();
					if (oldrow.Components.First().CustomId == e.Id)
					{
						DiscordSelectComponent oldselect = (DiscordSelectComponent)oldrow.Components.First();
						comps.Add(new DiscordSelectComponent(e.Id,
							oldselect.Placeholder,
							options));
					}
					else
					{
						comps.AddRange(oldrow.Components);
					}
					rows.Add(new(comps));
				}*/

				e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
					new DiscordInteractionResponseBuilder()
					.WithContent(e.Message.Content)
					.AddEmbeds(e.Message.Embeds)
					//.AddComponents(rows)
					.AddComponents(new DiscordSelectComponent(e.Id, Format(paginatedSelect.Placeholder, pageNum, paginatedSelect.PageCount), options)));
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
			return new DiscordSelectComponent(customId, Format(placeholder, 0, paginated.PageCount), GetOptions(customId, 0));
		}

		private static string Format(string input, int page, int pagecount)
		{
			return input.Replace("{page}", (page + 1).ToString())
				.Replace("{pagecount}", pagecount.ToString());
		}
	}
}