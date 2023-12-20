using System.Data.SqlClient;

namespace PKKMB_API.Model
{
    public class RuanganRepository
    {
        private readonly string _connectionString;

        private readonly SqlConnection _connection;
        ResponseModel responseModel = new ResponseModel();

        public RuanganRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }
        public List<RuanganModel> getAllData()
        {
            List<RuanganModel> ruanganList = new List<RuanganModel>();
            try
            {
                string query = "select * from pkm_msruangan";
                SqlCommand command = new SqlCommand(query, _connection);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RuanganModel ruangan = new RuanganModel
                    {
                        rng_idruangan = reader["rng_idruangan"].ToString(),
                        rng_namaruangan = reader["rng_namaruangan"].ToString(),
                        rng_status = reader["rng_status"].ToString(),
                    };
                    ruanganList.Add(ruangan);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ruanganList;
        }

        public RuanganModel getData(string rng_idruangan)
        {
            RuanganModel ruanganModel = new RuanganModel();
            try
            {
                string query = "select * from pkm_msruangan where rng_idruangan= @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", rng_idruangan);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                ruanganModel.rng_idruangan = reader["rng_idruangan"].ToString();
                ruanganModel.rng_namaruangan = reader["rng_namaruangan"].ToString();
                ruanganModel.rng_status = reader["rng_status"].ToString();
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ruanganModel;
        }

        public ResponseModel insertRuangan(RuanganModel ruanganModel)
        {
            try
            {
                string query = "insert into pkm_msruangan values(@p1,@p2, @p3)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", ruanganModel.rng_idruangan);
                command.Parameters.AddWithValue("@p2", ruanganModel.rng_namaruangan);
                command.Parameters.AddWithValue("@p3", ruanganModel.rng_status);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseModel.status = 500;
                responseModel.messages = "Failed, " + ex.Message;
                return responseModel;
            }
            responseModel.status = 200;
            responseModel.messages = "Success";
            responseModel.data = null;

            return responseModel;
        }

        public ResponseModel updateRuangan(RuanganModel ruanganModel)
        {
            try
            {
                string query = "UPDATE pkm_msruangan " +
                            "SET rng_namaruangan = @p2, " +
                            "rng_status = @p3 " +
                            "WHERE rng_idruangan = @p1";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", ruanganModel.rng_idruangan);
                command.Parameters.AddWithValue("@p2", ruanganModel.rng_namaruangan);
                command.Parameters.AddWithValue("@p3", ruanganModel.rng_status);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                responseModel.status = 500;
                responseModel.messages = "Failed, " + ex.Message;
                return responseModel;
            }
            responseModel.status = 200;
            responseModel.messages = "Success";
            responseModel.data = null;

            return responseModel;

        }
    }
}
