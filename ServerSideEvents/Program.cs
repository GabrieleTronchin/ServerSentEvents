using ServerSideEvents;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MoqDataRetrieverService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/SSE", async (CancellationToken ct, MoqDataRetrieverService service, HttpContext ctx) =>
{
    ctx.Response.Headers.TryAdd("Content-Type", "text/event-stream");
    while (!ct.IsCancellationRequested)
    {
        var messages = service.GetNewMessages();
        foreach (var msg in messages)
        {

            await ctx.Response.WriteAsync($"data: ");
            await JsonSerializer.SerializeAsync(ctx.Response.Body, msg);
            await ctx.Response.WriteAsync($"\n\n");


            await ctx.Response.Body.FlushAsync();
        }
    }

})
.WithName("ServerSideEvents")
.WithOpenApi();

app.Run();

