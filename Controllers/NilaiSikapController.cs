using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
    public class NilaiSikapController : Controller
    {
        private readonly NilaiSikapRepository nilaiSikapRepository;
        ResponseModel response = new ResponseModel();

        public NilaiSikapController(IConfiguration configuration)
        {
            nilaiSikapRepository = new NilaiSikapRepository(configuration);
        }

        [HttpGet("/GetAllNilaiSikap", Name = "GetAllNilaiSikap")]
        public IActionResult GetAllNilaiSikap()
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = nilaiSikapRepository.getAllData();
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed";
            }
            return Ok(response);
        }

        [HttpGet("/GetNilaiSikap", Name = "GetNilaiSikap")]
        public IActionResult GetNilaiSikap(string nls_idnilaisikap)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = nilaiSikapRepository.getData(nls_idnilaisikap);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

        [HttpPost("/InsertNilaiSikap", Name = "InsertNilaiSikap")]
        public IActionResult InsertNilaiSikap([FromBody] NilaiSikapModel nilaiSikapModel)
        {

            try
            {
                var result = nilaiSikapRepository.insertNilaiSikap(nilaiSikapModel);
                response.status = 200;
                response.messages = "Success";
                response.data = result;
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;

            }
            return Ok(response);
        }

        [HttpPut("/UpdateNilaiSikap", Name = "UpdateNilaiSikap")]
        public IActionResult UpdateNilaiSikap([FromBody] NilaiSikapModel nilaiSikapModel)
        {
            NilaiSikapModel nilaiSikap = new NilaiSikapModel();
            nilaiSikap.nls_idnilaisikap = nilaiSikapModel.nls_idnilaisikap;
            nilaiSikap.nls_nopendaftaran = nilaiSikapModel.nls_nopendaftaran;
            nilaiSikap.nls_nim = nilaiSikapModel.nls_nim;
            nilaiSikap.nls_sikap = nilaiSikapModel.nls_sikap;
            nilaiSikap.nls_jamplus = nilaiSikapModel.nls_jamplus;
            nilaiSikap.nls_jamminus = nilaiSikapModel.nls_jamminus;
            nilaiSikap.nls_tanggal = nilaiSikapModel.nls_tanggal;
            nilaiSikap.nls_status = nilaiSikapModel.nls_status;
            try
            {
                var result = nilaiSikapRepository.updateNilaiSikap(nilaiSikap);
                response.status = 200;
                response.messages = "Success";
                response.data = result;
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;

            }
            return Ok(response);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
