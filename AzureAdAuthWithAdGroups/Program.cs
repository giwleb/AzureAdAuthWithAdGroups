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

var FimUserGroupRequirement = new GroupAuthorizationRequirement(builder.Configuration["AuthGroups:FimUserGroupIds"].Split(',').ToList());

var FimDisbursementEntryGroupRequirement = new GroupAuthorizationRequirement(builder.Configuration["AuthGroups:FimDisbursementEntryGroupIds"].Split(',').ToList());

var FimCorporateAdminGroupRequirement = new GroupAuthorizationRequirement(builder.Configuration["AuthGroups:FimCorporateAdminGroupIds"].Split(',').ToList());

// Add services to the container.

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FimUserGroupPolicy", policy => policy.Requirements.Add(FimUserGroupRequirement));

    options.AddPolicy("FimDisbursementEntryGroupPolicy", policy => policy.Requirements.Add(FimDisbursementEntryGroupRequirement));

    options.AddPolicy("FimCorporateAdminGroupPolicy", policy => policy.Requirements.Add(FimCorporateAdminGroupRequirement));

    options.AddPolicy("FimAllPermissionsGroupPolicy", policy =>
    {
        policy.Requirements.Add(FimDisbursementEntryGroupRequirement);
        policy.Requirements.Add(FimCorporateAdminGroupRequirement);
    });
     
    options.AddPolicy("RequireDaemonRole", policy => policy.RequireRole("DaemonAppRole"));
});

builder.Services.AddControllers();

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
