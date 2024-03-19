using Spectre.Console.Cli;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;

public class SecuritySchemeCommand : AsyncCommand<SecuritySchemeCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-i|--input <INPUT>")]
        public string? Input { get; set; }

        [CommandOption("-o|--output <OUTPUT>")]
        public string? Output { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Input is null || settings.Output is null)
        {
            return 1;
        }

        var reader = new OpenApiStreamReader();
        OpenApiDocument document;

        // Read the OpenAPI document from a stream
        using (var stream = File.OpenRead(settings.Input))
        {
            var readResult = await reader.ReadAsync(stream);
            if (readResult.OpenApiDiagnostic.Errors.Count > 0)
            {
                foreach (var error in readResult.OpenApiDiagnostic.Errors)
                {
                    Console.WriteLine(error.Message);
                }
                return 1;
            }

            document = readResult.OpenApiDocument;

            // Create a new security scheme
            var securityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri("https://login.microsoftonline.com/microsft.onmicrosoft.com/v2.0/.well-known/openid-configuration"),
            };

            // Add the security scheme to the document
            document.Components.SecuritySchemes.Add("openIdConnect", securityScheme);
        }

        using (var stream = File.Create(settings.Output))
        {
            document.SerializeAsV3(new OpenApiYamlWriter(new StreamWriter(stream)));
        }

        return 0;
    }
}