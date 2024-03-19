
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<SecuritySchemeCommand>("security-scheme")
        .WithDescription("Create a new security scheme")
        .WithExample(new[] { "security-scheme -i input.yaml -o output.yaml" });
});

return app.Run(args);