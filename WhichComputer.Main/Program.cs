using Westwind.AspNetCore.LiveReload;
using WhichComputer.Main;

namespace WhichComputer
{
    public class Program
    {
        private static QuestionnaireLoader _loader = new QuestionnaireLoader(QuestionnaireLoader.LocalPath);
        private static ComputerLoader _computerLoader = new ComputerLoader(ComputerLoader.LocalPath);

        public static void Main(string[] args)
        {
            Console.WriteLine(_loader);
            Console.WriteLine(_computerLoader);
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