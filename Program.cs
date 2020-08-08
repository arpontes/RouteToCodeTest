using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build().Run();

public class Startup
{
    public void Configure(IApplicationBuilder app)
        => app.UseRouting()
              .UseEndpoints(endpoints =>
              {
                  endpoints.MapGet("/{id:int?}", async context =>
                  {
                      await context.Response.WriteAsJsonAsync(new { Result = "Hello World", Received = context.Request.Path.Value });
                  });
                  endpoints.MapPost("/", receiveData);
              });

    public record User(int id, string name);
    private async Task receiveData(HttpContext context)
    {
        var user = await context.Request.ReadFromJsonAsync<User>();
        var changedUser = user with { name = user.name.ToUpper() };
        await context.Response.WriteAsJsonAsync(new { Result = "Changed", Data = changedUser });
    }
}
