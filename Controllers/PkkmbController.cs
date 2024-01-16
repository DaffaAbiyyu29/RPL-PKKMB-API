using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
    public class PkkmbController : Controller
    {
        private readonly PkkmbRepository _pkkmbRepo;
        private readonly IConfiguration _configuration;

        public PkkmbController(IConfiguration configuration)
        {
            _pkkmbRepo = new PkkmbRepository(configuration);
            _configuration = configuration;
        }

        [HttpGet("/GetAllPkkmb", Name = "GetAllPkkmb")]
        public IActionResult GetAllPkkmb()
        {
            try
            {
                var pkm = _pkkmbRepo.getAllData();

                if (pkm.Count > 0)
                {
                    return Ok(new { Status = 200, Messages = "Berhasil Menampilkan PKKMB", Data = pkm });
                }
                else
                {
                    return StatusCode(404, new { Status = 404, Messages = "Data PKKMB Tidak Tersedia", Data = pkm });
                }
            }
            catch (Exception ex)
            {
                // Tangani kesalahan umum
                return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data PKKMB", Data = ex.Message });
            }
        }

        [HttpGet("/GetPkkmbAktif", Name = "GetPkkmbAktif")]
        public IActionResult GetPkkmbAktif()
        {
            PkkmbModel pkm = _pkkmbRepo.getPkkmbAktif();
            try
            {
                if (pkm != null)
                {
                    return Ok(new { Status = 200, Messages = "Berhasil Menampilkan PKKMB Aktif", Data = pkm });

                }
                else
                {
                    // Account not found
                    return NotFound(new { Status = 404, Messages = "Data PKKMB Aktif Tidak Tersedia", Data = new Object() });
                }
            }
            catch (Exception ex)
            {
                // General error
                return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan PKKMB = " + ex.Message, Data = new Object() });
            }
        }

        [HttpGet("/GetPkkmb", Name = "GetPkkmb")]
        public IActionResult GetPkkmb(string pkm_idPkkmb)
        {
            PkkmbModel pkm = _pkkmbRepo.getData(pkm_idPkkmb);
            try
            {
                if (pkm != null)
                {
                    return Ok(new { Status = 200, Messages = "Berhasil Menampilkan PKKMB", Data = pkm });

                }
                else
                {
                    // Account not found
                    return NotFound(new { Status = 404, Messages = "Data PKKMB Tidak Tersedia", Data = new Object() });
                }
            }
            catch (Exception ex)
            {
                // General error
                return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan PKKMB = " + ex.Message, Data = new Object() });
            }
        }

        [HttpPost("/TambahPkkmb", Name = "TambahPkkmb")]
        public IActionResult TambahPkkmb([FromBody] PkkmbModel pkm)
        {
            var result = _pkkmbRepo.tambahPkkmb(pkm);
            return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
        }

        [HttpPut("/UpdatePkkmb", Name = "UpdatePkkmb")]
        public IActionResult UpdatePkkmb([FromBody] PkkmbModel pkm)
        {
            var result = _pkkmbRepo.updatePkkmb(pkm);
            return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
        }
        
        [HttpPut("/AktifkanPkkmb", Name = "AktifkanPkkmb")]
        public IActionResult AktifkanPkkmb(string pkm_idPkkmb)
        {
            var result = _pkkmbRepo.aktifkanPkkmb(pkm_idPkkmb);
            return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
        }
        
        [HttpPut("/NonAktifkanPkkmb", Name = "NonAktifkanPkkmb")]
        public IActionResult NonAktifkanPkkmb(string pkm_idPkkmb)
        {
            var result = _pkkmbRepo.nonaktifkanPkkmb(pkm_idPkkmb);
            return StatusCode(result.status, new { Status = result.status, Messages = result.messages });
        }
    }
}
