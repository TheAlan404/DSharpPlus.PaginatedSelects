# Paginated Selects

This extension adds paginated select components to DSharpPlus.

### How to use

First, activate the extension on your discord client
```cs
using DSharpPlus.PaginatedSelects;

var paginatedExtension = client.UsePaginatedSelects();
```

__**A) Pre-registering a paginated select**__

1. register it
```cs
var paginatedSelect = new PaginatedSelect(new() {
	new DiscordSelectComponentOption("Yes", "true"),
	new DiscordSelectComponentOption("No", "false"),
});

paginatedExtension.AddPaginatedSelect("yesorno", paginatedSelect);
```

2. use it
```cs
var select = paginatedExtension.BuildSelect("yesorno");
```

__**B) Registering a dynamic paginated select**__
aka register temperoraly

```cs
var select = paginatedExtension.CreateSelect(new(){
	new DiscordSelectComponentOption("Yes", "true"),
	new DiscordSelectComponentOption("No", "false"),
});
```

### Want more?

This extension is highly configurable. You can set a default placeholder text for your selects (`PaginatedSelectsConfiguration`)
and override them per-select. (`PaginatedSelect`)

You can also change the next and previous page select options in the configuration.

Pull requests and issues are welcome as f-

### Formatting

The label, description and the placeholder/suffix in the configuration allow for custom formatting.

- `{page}` will turn into the page number
- `{pagecount}` will turn into the total number of pages