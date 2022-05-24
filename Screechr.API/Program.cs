using Microsoft.EntityFrameworkCore;
using Screechr.Repository;
using Screechr.Service;
using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Auth token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
});
builder.Services.AddDbContext<ScreechrContext>(options => options.UseInMemoryDatabase("Screechr"));

RegisterDefaultInterfacesFromAssembly(builder.Services, typeof(UserService).GetTypeInfo().Assembly);
RegisterDefaultInterfacesFromAssembly(builder.Services, typeof(UserRepository).GetTypeInfo().Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterDefaultInterfacesFromAssembly(IServiceCollection services, Assembly assembly)
{
    var types = GetConcreteTypes(assembly);

    foreach (var type in types)
    {
        var interfaceType = GetDefaultInterfaceType(type);

        if (interfaceType != null && type.GetGenericArguments().Length == 0)
            services.AddTransient(interfaceType, type);
    }
}

static IEnumerable<Type> GetConcreteTypes(Assembly assembly)
{
    return assembly.GetTypes().Where(t => !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface);
}

static Type GetDefaultInterfaceType(System.Type type)
{
    return type.GetInterfaces().FirstOrDefault(i => i.Name.Equals("I" + type.Name, StringComparison.OrdinalIgnoreCase));
}