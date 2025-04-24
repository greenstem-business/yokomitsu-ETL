using ETL_API.Helper;
using ETL_API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//IF WANT TO KNOW THE ENCYPTED STRING OF YOUR CREDENTIALS
//USE ENCRYPT FUNCTION TO PROCESS YOUR STRING THEN PRINT OUT TO SEE
//EXAMPLE AS BELOW:
//Console.WriteLine(AesEncryptionHelper.Encrypt("Hello World"));

string decrypted_server = AesEncryptionHelper.Decrypt(builder.Configuration.GetConnectionString("Server"));
string decrypted_name = AesEncryptionHelper.Decrypt(builder.Configuration.GetConnectionString("Name"));
string decrypted_user = AesEncryptionHelper.Decrypt(builder.Configuration.GetConnectionString("User"));
string decrypted_pwd = AesEncryptionHelper.Decrypt(builder.Configuration.GetConnectionString("Pwd"));
string connectionString = $@"Server={decrypted_server};Database={decrypted_name};
                            User Id={decrypted_user};Password={decrypted_pwd};TrustServerCertificate=True;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() 
        || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger", permanent: false);
    return Task.CompletedTask;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();
app.UseDefaultFiles();
app.MapControllers();
app.Run();
