# Paginated Selects

This extension adds paginated select components to DSharpPlus. (badly)

### How to use

First, activate the extension on your discord client

```cs
using DSharpPlus.PaginatedSelects;

var paginatedExtension = client.UsePaginatedSelects();
```

__**Creating a paginated select**__

1) Using AddPaginatedSelect

```cs
PaginatedSelect paginated = new PaginatedSelect(/* List<DiscordSelectComponentOption> here */);
// OR
var normalSelect = new DiscordSelectComponent()
{
	// bla bla
};
var paginated = normalSelect.ToPaginatedSelect();

DiscordSelectComponent firstPage = paginatedExtension.AddPaginatedSelect("myselect", paginated);
```

2) Using the extension methods on builders (`DiscordInteractionResponseBuilder`, `DiscordFollowupMessageBuilder`, `DiscordWebhookBuilder`)

```cs
// For paginated selects that was already created:
await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent("Hi! Where are you from?")
				.AddPaginatedSelect("myselect"));

// To create a new one:
await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent("Hi! Where are you from?")
				.AddPaginatedSelect("myselect", new PaginatedSelect(new(){
					new("America", "us"),
					new("France", "fr"),
					new("Germany", "gb"),
					new("Turkey", "shit"),
					// ...
				})));
// Note: you can also make a DiscordSelectComponent and supply it, the extension will call ToPaginatedSelect() on it.
```



__**I want it gone after its used**__

```cs
// For this paginated select:
new PaginatedSelect()
{
	AutoRemoveSelect = true,
};

// For all of the selects (that doesnt set PaginatedSelect#AutoRemoveSelect to false)
new PaginatedSelectsConfiguration()
{
	AutoRemoveSelect = true,
};
```



__**I want the other contents of the message changed after the page changes**__

Sure! (do it yourself)
```cs
new PaginatedSelect()
{
	CustomRender = (CustomRenderContext ctx) => {
		ctx.Interaction.DoStuff();
		// Note: you must supply the select component too
		// You can use pre-made ctx.Select and PaginatedSelectsExtension.Utils.ReplaceComponent ;)
	}
};
```

### Want more?

This extension is highly configurable. You can set a default placeholder text for your selects (`PaginatedSelectsConfiguration`)
and override them per-select. (`PaginatedSelect`)

You can also change the next and previous page select options in the configuration.

```cs
new PaginatedSelectsConfiguration()
{
	NextPageOption = new("NEXTTTT!!", "_", "next page uwu"),
	PreviousPageOption = new("fuck go back", "_", "oh god did i go too far"),
};
```

Pull requests and issues are welcome (please)

### Formatting

The label, description and the placeholder/suffix in the configuration allow for custom formatting.

- `{page}` will turn into the page number
- `{pagecount}` will turn into the total number of pages
