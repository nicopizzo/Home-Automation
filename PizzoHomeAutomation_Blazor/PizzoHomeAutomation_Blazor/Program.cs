using PizzoHomeAutomation_Blazor.Components;
using PizzoHomeAutomation_Blazor.Client;
using LettuceEncrypt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var lettuceSection = builder.Configuration.GetSection("LettuceEncrypt");
var directoryInfo = new DirectoryInfo(lettuceSection.GetValue<string>("CertDirectory")!);

builder.Services.AddBlazorClientServices();
builder.Services.AddLettuceEncrypt()
    .PersistDataToDirectory(directoryInfo, lettuceSection.GetValue<string>("CertPassword")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(PizzoHomeAutomation_Blazor.Client._Imports).Assembly);

app.Run();
