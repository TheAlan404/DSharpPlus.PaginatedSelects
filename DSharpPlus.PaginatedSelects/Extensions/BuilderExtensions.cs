using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSharpPlus.PaginatedSelects
{
	public static class BuilderExtensions
	{
		internal static DiscordSelectComponent GetSelect(string customId, PaginatedSelect paginated)
		{
			if (PaginatedSelectsExtension._lastInstance == null) throw new NullReferenceException("Couldn't find the PaginatedSelectsExtension");
			var select = PaginatedSelectsExtension._lastInstance.AddPaginatedSelect(customId, paginated);
			if (select == null) throw new NullReferenceException("PaginatedSelectsExtension gave null");
			return select;
		}

		internal static DiscordSelectComponent GetSelect(string customId)
		{
			if (PaginatedSelectsExtension._lastInstance == null) throw new NullReferenceException("Couldn't find the PaginatedSelectsExtension");
			var select = PaginatedSelectsExtension._lastInstance.BuildSelect(customId);
			if (select == null) throw new NullReferenceException("PaginatedSelectsExtension gave null");
			return select;
		}

		#region DiscordInteractionResponseBuilder

		/// <summary>
		/// Create a paginated select and add the first page as a select component
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <param name="paginated"></param>
		/// <returns></returns>
		public static DiscordInteractionResponseBuilder AddPaginatedSelect(this DiscordInteractionResponseBuilder builder,
			DiscordSelectComponent select)
			=> builder.AddPaginatedSelect(select.CustomId, select.ToPaginatedSelect());

		/// <summary>
		/// Create a paginated select and add the first page as a select component
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <param name="paginated"></param>
		/// <returns></returns>
		public static DiscordInteractionResponseBuilder AddPaginatedSelect(this DiscordInteractionResponseBuilder builder,
			string customId, PaginatedSelect paginated)
			=> builder.AddComponents(GetSelect(customId, paginated));

		/// <summary>
		/// Add a pre-created paginated select
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <returns></returns>
		public static DiscordInteractionResponseBuilder AddPaginatedSelect(this DiscordInteractionResponseBuilder builder,
			string customId)
			=> builder.AddComponents(GetSelect(customId));

		#endregion

		#region DiscordFollowupMessageBuilder

		/// <summary>
		/// Create a paginated select and add the first page as a select component
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <param name="paginated"></param>
		/// <returns></returns>
		public static DiscordFollowupMessageBuilder AddPaginatedSelect(this DiscordFollowupMessageBuilder builder,
			DiscordSelectComponent select)
			=> builder.AddPaginatedSelect(select.CustomId, select.ToPaginatedSelect());

		/// <summary>
		/// Create a paginated select and add the first page as a select component
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <param name="paginated"></param>
		/// <returns></returns>
		public static DiscordFollowupMessageBuilder AddPaginatedSelect(this DiscordFollowupMessageBuilder builder,
			string customId, PaginatedSelect paginated)
			=> builder.AddComponents(GetSelect(customId, paginated));

		/// <summary>
		/// Add a pre-created paginated select
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <returns></returns>
		public static DiscordFollowupMessageBuilder AddPaginatedSelect(this DiscordFollowupMessageBuilder builder,
			string customId)
			=> builder.AddComponents(GetSelect(customId));

		#endregion

		#region DiscordWebhookBuilder

		/// <summary>
		/// Create a paginated select and add the first page as a select component
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <param name="paginated"></param>
		/// <returns></returns>
		public static DiscordWebhookBuilder AddPaginatedSelect(this DiscordWebhookBuilder builder,
			DiscordSelectComponent select)
			=> builder.AddPaginatedSelect(select.CustomId, select.ToPaginatedSelect());

		/// <summary>
		/// Create a paginated select and add the first page as a select component
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <param name="paginated"></param>
		/// <returns></returns>
		public static DiscordWebhookBuilder AddPaginatedSelect(this DiscordWebhookBuilder builder,
			string customId, PaginatedSelect paginated)
			=> builder.AddComponents(GetSelect(customId, paginated));

		/// <summary>
		/// Add a pre-created paginated select
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="customId"></param>
		/// <returns></returns>
		public static DiscordWebhookBuilder AddPaginatedSelect(this DiscordWebhookBuilder builder,
			string customId)
			=> builder.AddComponents(GetSelect(customId));

		#endregion
	}
}
