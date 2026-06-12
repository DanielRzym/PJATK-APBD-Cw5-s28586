using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw5_s28586.Models;
namespace PJATK_APBD_Cw5_s28586.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public PatientsController(HospitalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery] string? search)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = $"%{search}%";
                query = query.Where(p => 
                    EF.Functions.Like(p.FirstName, searchTerm) || 
                    EF.Functions.Like(p.LastName, searchTerm));
            }

           
            var patients = await query.Select(p => new
            {
                pesel = p.Pesel,
                firstName = p.FirstName,
                lastName = p.LastName,
                age = p.Age,
                
                sex = p.Sex == true ? "Male" : "Female", 
                
                admissions = p.Admissions.Select(a => new
                {
                    id = a.Id,
                    admissionDate = a.AdmissionDate,
                    dischargeDate = a.DischargeDate,
                    ward = new
                    {
                        id = a.Ward.Id,
                        name = a.Ward.Name,
                        description = a.Ward.Description
                    }
                }).ToList(),

                bedAssignments = p.BedAssignments.Select(ba => new
                {
                    id = ba.Id,
                    from = ba.From,
                    to = ba.To,
                    bed = new
                    {
                        id = ba.Bed.Id,
                        bedType = new
                        {
                            id = ba.Bed.BedType.Id,
                            name = ba.Bed.BedType.Name,
                            description = ba.Bed.BedType.Description
                        },
                        room = new
                        {
                            id = ba.Bed.Room.Id,
                            hasTv = ba.Bed.Room.HasTv,
                            ward = new
                            {
                                id = ba.Bed.Room.Ward.Id,
                                name = ba.Bed.Room.Ward.Name,
                                description = ba.Bed.Room.Ward.Description
                            }
                        }
                    }
                }).ToList()
            }).ToListAsync();

            return Ok(patients);
        }
    }
}
