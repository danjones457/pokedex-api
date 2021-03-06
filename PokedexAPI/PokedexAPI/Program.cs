using PokedexAPI.Handlers;
using PokedexAPI.Helpers;
using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Interfaces.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPokemonHandler, PokemonHandler>();
builder.Services.AddScoped<IPokeApiToPokemonHelper, PokeApiToPokemonHelper>();
builder.Services.AddScoped<IPokeApiHelper, PokeApiHelper>();
builder.Services.AddScoped<IPokemonTranslatedHandler, PokemonTranslatedHandler>();
builder.Services.AddScoped<IPokemonTranslatedHelper, PokemonTranslatedHelper>();
builder.Services.AddScoped<ITranslationsHelper, TranslationsHelper>();

builder.Services.AddHttpClient<IPokeApiHelper, PokeApiHelper>();
builder.Services.AddHttpClient<ITranslationsHelper, TranslationsHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
