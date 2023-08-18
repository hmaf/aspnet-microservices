using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        #region Constractor

        private readonly IConfiguration _configuration;
        NpgsqlConnection connection = new NpgsqlConnection();
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        #endregion

        #region  Get

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if(coupon == null) 
                return new Coupon { ProductName = "NO DISCOUNT", Amount = 0, Description = "No Discount" };

            return coupon;   
        }

        #endregion

        #region Create
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES(@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if(affected == 0) return false;

            return true;
        }

        #endregion

        #region Update
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if(affected == 0) return false;

            return true;
        }

        #endregion

        #region Delete
        public async Task<bool> DeleteDiscount(string productName)
        {
            var affected = await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName=@ProductName",
                new { ProductName = productName });

            if(affected == 0) return false;

            return true;
        }

        #endregion
    }
}
