using System.Configuration;
using System.Data;
using System.Windows;
using CarDealership.Data;

namespace CarDealership
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new ApplicationDbContext())
            {
                context.Database.EnsureCreated();

                if (!context.Brands.Any())
                {
                    DbInitializer.Seed(context);
                }
            }
        }

    }
}
