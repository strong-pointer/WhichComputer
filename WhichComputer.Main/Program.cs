using Westwind.AspNetCore.LiveReload;
using WhichComputer.Main;

namespace WhichComputer.Main
{
    public class Program
    {
        private static QuestionnaireLoader _loader = new QuestionnaireLoader(QuestionnaireLoader.LocalPath);

        public static IConfigurationRoot Config { get; } = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

        private static ComputerLoader ComputerLoader { get; } = new ComputerLoader(ComputerLoader.LocalPath);

        public static QuestionnaireLoader GetQuestionnaireLoader()
        {
            return _loader;
        }

        public static ComputerLoader GetComputerLoader()
        {
            return ComputerLoader;
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                builder.Services.AddLiveReload();
                app.UseLiveReload();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}