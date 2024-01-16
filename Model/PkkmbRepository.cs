using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace PKKMB_API.Model
{
    public class PkkmbRepository
    {
        private readonly string _connectingString;
        private readonly SqlConnection _connection;
        ResponseModel response = new ResponseModel();

        public PkkmbRepository(IConfiguration configuration)
        {
            _connectingString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectingString);
        }

        public List<PkkmbModel> getAllData()
        {
            List<PkkmbModel> pkmList = new List<PkkmbModel>();
            try
            {
                string query = "select * from pkm_mspkkmb";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PkkmbModel pkm = new PkkmbModel
                    {
                        pkm_idPkkmb = reader["pkm_idPkkmb"].ToString(),
                        pkm_tahunPkkmb = int.Parse(reader["pkm_tahunPkkmb"].ToString()),
                        pkm_status = reader["pkm_status"].ToString(),
                    };
                    pkmList.Add(pkm);
                }
                reader.Close();
                _connection.Close();
                return pkmList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return null;
            }
        }

        public PkkmbModel getData(string pkm_idPkkmb)
        {
            PkkmbModel pkm = new PkkmbModel();
            try
            {
                string query = "SELECT * FROM pkm_mspkkmb WHERE pkm_idPkkmb = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", pkm_idPkkmb);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pkm = new PkkmbModel
                    {
                        pkm_idPkkmb = reader["pkm_idPkkmb"].ToString(),
                        pkm_tahunPkkmb = int.Parse(reader["pkm_tahunPkkmb"].ToString()),
                        pkm_status = reader["pkm_status"].ToString(),
                    };

                    reader.Close();
                    _connection.Close();
                    return pkm;
                }
                else
                {
                    // User not found
                    reader.Close();
                    _connection.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        
        public PkkmbModel getPkkmbAktif()
        {
            PkkmbModel pkm = new PkkmbModel();
            try
            {
                string query = "SELECT * FROM [dbo].[view_PkkmbAktif]";

				SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pkm = new PkkmbModel
                    {
                        pkm_idPkkmb = reader["pkm_idPkkmb"].ToString(),
                        pkm_tahunPkkmb = int.Parse(reader["pkm_tahunPkkmb"].ToString()),
                        pkm_status = reader["pkm_status"].ToString(),
                    };

                    reader.Close();
                    _connection.Close();
                    return pkm;
                }
                else
                {
                    // User not found
                    reader.Close();
                    _connection.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public ResponseModel tambahPkkmb([FromBody] PkkmbModel pkm)
        {
            try
            {
                SqlCommand command = new SqlCommand("sp_TambahPkkmb", _connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pkm_tahunPkkmb", pkm.pkm_tahunPkkmb);
                command.Parameters.AddWithValue("@pkm_status", "Tidak Aktif");

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                response.status = 200;
                response.messages = "Data PKKMB Berhasil Ditambahkan";
                response.data = pkm;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.status = 500;
                response.messages = "Terjadi Kesalahan Saat Menambahkan Data PKKMB = " + ex.Message;
                response.data = null;
            }

            return response;
        }

        public ResponseModel updatePkkmb([FromBody] PkkmbModel pkm)
        {
            try
            {
                SqlCommand command = new SqlCommand("sp_UpdatePkkmb", _connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pkm_idPkkmb", pkm.pkm_idPkkmb);
                command.Parameters.AddWithValue("@pkm_tahunPkkmb", pkm.pkm_tahunPkkmb);


                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                response.status = 200;
                response.messages = "Data PKKMB Berhasil Diubah";
                response.data = pkm;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.status = 500;
                response.messages = "Terjadi Kesalahan Saat Mengubah Data PKKMB = " + ex.Message;
                response.data = null;
            }

            return response;
        }
        
        public ResponseModel aktifkanPkkmb(string pkm_idPkkmb)
        {
            try
            {
                SqlCommand command = new SqlCommand("sp_AktifkanPkkmb", _connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pkm_idPkkmb", pkm_idPkkmb);


                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                response.status = 200;
                response.messages = "Data PKKMB Berhasil Aktif";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.status = 500;
                response.messages = "Terjadi Kesalahan Saat Mengaktifkan Data PKKMB = " + ex.Message;
            }

            return response;
        }
        
        public ResponseModel nonaktifkanPkkmb(string pkm_idPkkmb)
        {
            try
            {
                SqlCommand command = new SqlCommand("sp_NonaktifkanPkkmb", _connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pkm_idPkkmb", pkm_idPkkmb);


                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                response.status = 200;
                response.messages = "Data PKKMB Berhasil Non Aktif";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.status = 500;
                response.messages = "Terjadi Kesalahan Saat Non Aktifkan Data PKKMB = " + ex.Message;
            }

            return response;
        }
    }
}
