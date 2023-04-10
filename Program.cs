
using LoanManagementSystemAPI.Data;
using LoanManagementSystemAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IGadgetLoanRepository, GadgetLoanRepository>();

/*
var issuer = builder.Configuration["JWT:Issuer"];
var audience = builder.Configuration["JWT:Audience"];
var key = builder.Configuration["JWT:Key"];
var app = builder.Build();
*/

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // endpoints
    app.UseSwaggerUI(); // swagger editor to show the action urls for each operation
}

app.UseAuthentication(); // token validation

app.UseAuthorization(); // which endpoints [URL] is allowed to be accessed 

// maps you contrllers actions as urls
app.MapControllers();

app.Run();
