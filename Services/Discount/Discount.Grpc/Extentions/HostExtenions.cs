using Npgsql;

namespace Discount.Grpc.Extentions
{
    public static class HostExtenions
    {   
        // این متد یک افزونه برای رابط IHost ایجاد می‌کند و پارامتر retry به صورت اختیاری با مقدار پیش‌فرض 0 دارد.
        // تلاش‌های تکراری برای مهاجرت نیز با استفاده از پارامتر retry انجام می‌شود.
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            // ابتدا مقدار retry را در یک متغیر جداگانه ذخیره می‌کنیم.
            int retryForAvailability = retry.Value;
            // با استفاده از CreateScope()، یک Scope ایجاد می‌کنیم تا به سرویس‌های مورد نیاز دسترسی داشته باشیم.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                // migrate database
                try
                {
                    // اطلاعات مربوط به مهاجرت پایگاه داده را به لاگ می‌نویسیم.
                    logger.LogInformation("migrating PostgreSql database");

                    // اتصال به پایگاه داده PostgreSQL برقرار می‌کنیم.
                    using var connection = 
                        new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    // با استفاده از command.CommandText، دستورات SQL مورد نیاز برای مهاجرت و تغییرات را اجرا می‌کنیم.
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    // دستورات SQL برای حذف و ایجاد جدول Coupon.
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    // دستورات SQL برای افزودن داده‌های نمونه به جدول.
                    command.CommandText = @"CREATE TABLE Coupon (ID SERIAL PRIMARY KEY,
                                                                 ProductName VARCHAR(200) NOT NULL,
                                                                 Description TEXT,
                                                                 Amount INT)";
                    command.ExecuteNonQuery();

                    // seed data

                    command.CommandText = 
                        "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('IPhone x', 'iphone discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = 
                        "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Samsong 10', 'samsong discount', 150);";
                    command.ExecuteNonQuery();

                    // این پیام بعد از انجام مهاجرت به لاگ ثبت شود.
                    logger.LogInformation("an error has been occured");

                }
                catch (Exception ex)
                {
                    // در صورت بروز خطا، اطلاعات مربوط به خطا به لاگ اضافه می‌کنیم.
                    logger.LogError("an error has been occured");

                    // اگر تعداد تلاش‌های تکراری کم‌تر از 50 باشد، تاخیر داریم و مجدداً تابع MigrateDatabase را با تلاش‌های بیشتر فراخوانی می‌کنیم.
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }
            
            return host;
        }
    }
}
