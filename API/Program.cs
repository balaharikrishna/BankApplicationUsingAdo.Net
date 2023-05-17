using API.Mappings;
using BankApplicationRepository.IRepository;
using BankApplicationRepository.Repository;
using BankApplicationServices.IServices;
using BankApplicationServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using API.AuthorizationPolicies.CustomerOnly;
using API.AuthorizationPolicies.StaffOnly;
using API.AuthorizationPolicies.ManagerOnly;
using API.AuthorizationPolicies.HeadManagerOnly;
using API.AuthorizationPolicies.ReserveBankManagerOnly;
using API.AuthorizationPolicies.BranchMembersOnly;
using API.AuthorizationPolicies.MinimumHeadManager;
using API.AuthorizationPolicies.ManagerStaffOnly;
using API.AuthorizationPolicies.ManagerHeadManagerOnly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomerOnly", policy => policy.Requirements.Add(new CustomerOnlyRequirement()));
    options.AddPolicy("StaffOnly", policy => policy.Requirements.Add(new StaffOnlyRequirement()));
    options.AddPolicy("ManagerOnly", policy => policy.Requirements.Add(new ManagerOnlyRequirement()));
    options.AddPolicy("HeadManagerOnly", policy => policy.Requirements.Add(new HeadManagerOnlyRequirement()));
    options.AddPolicy("ReserveBankManagerOnly", policy => policy.Requirements.Add(new ReserveBankManagerOnlyRequirement()));
    options.AddPolicy("BranchMembersOnly", policy => policy.Requirements.Add(new BranchMembersOnlyRequirement()));
    options.AddPolicy("MinimumHeadManager", policy => policy.Requirements.Add(new MinimumHeadManagerRequirement()));
    options.AddPolicy("ManagerStaffOnly", policy => policy.Requirements.Add(new ManagerStaffOnlyRequirement()));
    options.AddPolicy("ManagerHeadManagerOnly", policy => policy.Requirements.Add(new ManagerHeadManagerOnlyRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, CustomerOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, StaffOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ManagerOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HeadManagerOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ReserveBankManagerOnlyHeadler>();
builder.Services.AddSingleton<IAuthorizationHandler, BranchMembersOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HeadManagerOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ManagerStaffOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ManagerHeadManagerOnlyHandler>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddScoped<SqlConnection>(sp => new SqlConnection(builder.Configuration.GetConnectionString("MyDbConnection")));
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IHeadManagerService, HeadManagerService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IReserveBankManagerService, ReserveBankManagerService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<ITransactionChargeService, TransactionChargeService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IHeadManagerRepository, HeadManagerRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IReserveBankManagerRepository, ReserveBankManagerRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ITransactionChargeRepository, TransactionChargeRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITokenIssueService, TokenIssueService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<AuthenticationMiddleware>();
//app.UseMiddleware<AuthorizationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
