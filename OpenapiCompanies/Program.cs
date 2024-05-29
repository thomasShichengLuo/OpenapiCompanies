
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using OpenapiCompanies.Companies;

namespace OpenapiCompanies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var companyOptions = builder.Configuration.GetSection("CompanyApi").Get<Config.CompanyXMLApiOption>();
            if (companyOptions != null)
            {
                builder.Services.AddSingleton(provider => companyOptions);
            }
            builder.Services.AddTransient<ICompanyApiAgent, CompanyApiAgent>();

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
        }

    }
}
