using Microsoft.EntityFrameworkCore;
using TransactionsApi.Context;
using TransactionsApi.Interfaces;
using TransactionsApi.Services;

namespace TransactionsApi.Settings
{
    public static class ServicesExtentions
    {
        public static void ConfigureDateBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("StringPadrao"));
            });
        }


        public static void ConfigureCustomServices(this IServiceCollection services)
        {
            services.AddScoped<SecurityServices>();
            services.AddScoped<IClientServices, ClientServices>();
            services.AddScoped<ITransactionsServices, TransactionsServices>();
            services.AddScoped<ITransactionPdfService, TransactionPdfService>();

        }

    }
}
