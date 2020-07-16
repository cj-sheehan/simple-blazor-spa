# simple-blazor-spa

This is a simple Blazor WASM application with a WebAPI backend.

## Usage

- A database backup can be found in the NameDB project.
- Requires .NET Core 3.1.
- May be necessary to update `NameApi/appsettings.json` with connection string to the database.
- Solution has been tested using IIS with multiple start up projects (NameApi + BlazorApp).
- Additional configuration may be required depending on your hosting solution in `BlazorApp/wwwroot/appsettings.json`.
