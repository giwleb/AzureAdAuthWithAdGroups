using AzureAdAuthWithAdGroups;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

var FimUserRequirement = new GroupAuthorizationRequirement(builder.Configuration["AuthGroups:FimUserGroupIds"].Split(',').ToList());

var FimDisbursementEntryRequirement = new GroupAuthorizationRequirement(builder.Configuration["AuthGroups:FimDisbursementEntryGroupIds"].Split(',').ToList());

var FimCorporateAdminRequirement = new GroupAuthorizationRequirement(builder.Configuration["AuthGroups:FimCorporateAdminGroupIds"].Split(',').ToList());

// Add services to the container.

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FimUserPolicy", policy => policy.Requirements.Add(FimUserRequirement));

    options.AddPolicy("FimDisbursementEntryPolicy", policy => policy.Requirements.Add(FimDisbursementEntryRequirement));

    options.AddPolicy("FimCorporateAdminPolicy", policy => policy.Requirements.Add(FimCorporateAdminRequirement));

    options.AddPolicy("FimAllPermissionsPolicy", policy =>
    {
        policy.Requirements.Add(FimDisbursementEntryRequirement);
        policy.Requirements.Add(FimCorporateAdminRequirement);
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAuthorizationHandler, GroupAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
