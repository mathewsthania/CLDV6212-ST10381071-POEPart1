using Azure.Storage.Blobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;


//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace CLDV6212_ST10381071_POEPart1.Services
{
	public class ImageService
	{
		private readonly IConfiguration _configuration;

		public ImageService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task InsertBlobAsync(byte[] imageData)
		{
			var connectionString = _configuration.GetConnectionString("DefaultConnection");
			var query = @"INSERT INTO UploadImage (BlobImage) VALUES (@BlobImage)";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@BlobImage", imageData);

				connection.Open();
				await command.ExecuteNonQueryAsync();
			}
		}
	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//