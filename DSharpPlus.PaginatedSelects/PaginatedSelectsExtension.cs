using System;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.PaginatedSelects
{
	public class PaginatedSelectsExtension : BaseExtension
	{
		private DiscordClient _client;
		private PaginatedSelectsConfiguration _config;
		private Dictionary<string, List<DiscordSelectComponentOption>> _options;

		private const int SELECT_OPTION_LIMIT = 25;
		private int optionsPerPage = SELECT_OPTION_LIMIT - 2;

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
			List<DiscordSelectComponentOption> options = _options[customId];
			if (options.Count <= optionsPerPage) return options;

			int pageCount = (int)Math.Ceiling((double)options.Count / optionsPerPage);
			options = options.Skip(page * optionsPerPage).Take(optionsPerPage).ToList();
			if(options.Skip((page + 1) * optionsPerPage).Count() > 0)
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

		public void SetOptions(string customId, List<DiscordSelectComponentOption> options)
		{
			_options[customId] = options;
		}

		private Task OnComponent(object sender, ComponentInteractionCreateEventArgs e)
		{
			// warning, shitcode. im really sorry. -dennis
			_ = Task.Run(() =>
			{
				if (!_options.ContainsKey(e.Id)) return;
				// lazy method
				bool isValid = int.TryParse(e.Values[0].Replace(_config.ValuePrefix, "").Replace(_config.ValueSuffix, ""), out int pageNum);
				if (!isValid) return;

				var options = GetOptions(e.Id, pageNum);

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
				}

				e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
					new DiscordInteractionResponseBuilder()
					.WithContent(e.Message.Content)
					.AddEmbeds(e.Message.Embeds)
					.AddComponents(rows));
			});
			return Task.CompletedTask;
		}

		public DiscordSelectComponent CreateSelect(List<DiscordSelectComponentOption> options)
			=> CreateSelect(Guid.NewGuid().ToString(), "Select an option", options);
		public DiscordSelectComponent CreateSelect(string customId, string placeholder, List<DiscordSelectComponentOption> options)
		{
			SetOptions(customId, options);
			return new DiscordSelectComponent(customId, placeholder, GetOptions(customId, 0));
		}
	}
}