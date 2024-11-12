using CLDV6212_ST10381071_POEPart1;
using CLDV6212_ST10381071_POEPart1.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Services
{
    public class OrderProcessService
    {
		private readonly IConfiguration _configuration;

		public OrderProcessService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<bool> ProcessOrderAsync(string orderID)
		{
			var connectionString = _configuration.GetConnectionString("DefaultConnection");
			var query = "INSERT INTO OrdersProcessed (OrderID, DateProcessed) VALUES (@OrderID, @DateProcessed)";

			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@OrderID", orderID);
					command.Parameters.AddWithValue("@DateProcessed", DateTime.Now); // Get the current date and time

					connection.Open();
					int rowsAffected = await command.ExecuteNonQueryAsync();

					// If rowsAffected > 0, the order was successfully processed
					return rowsAffected > 0;
				}
			}
			catch (Exception)
			{
				return false; // Something went wrong with the database operation
			}
		}
	}
}
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//