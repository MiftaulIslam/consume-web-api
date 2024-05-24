

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;

        }
        //Get: api/employee
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeModel>> GetEmployees()
        {
            if (_context.Employee == null) return NotFound();
            return _context.Employee.ToList();
        }


        //Get:api/employee/id
        [HttpGet("{id}")]

        public ActionResult<EmployeeModel> GetEmployee(int id)
        {

            if (_context.Employee == null) return NotFound();
            if (id < 0) return NotFound();
            var employee = _context.Employee.Find(id);
            if (employee == null) return NotFound();
            return employee;
        }

        //Post:api/Employee
        [HttpPost]
        public ActionResult AddEmployee(EmployeeModel employee)
        {
            if (_context.Employee.Any(x => x.EmployeeName == employee.EmployeeName))
            {
                return Conflict("Employee already exists");
            }

            _context.Employee.Add(employee);
            _context.SaveChanges();
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateEmployee(int id,  EmployeeModel employee)
        {
            if (id != employee.Id) return BadRequest("Miss match Employee");

            _context.Entry(employee).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }catch(DbUpdateConcurrencyException  ex){
                if (!_context.Employee.Any(x => x.Id == employee.Id)) return NotFound("Not found");
            }

            return Ok("Status ok");
        }  
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            if (id < 0) return BadRequest("Invalid Id");
            var employee = _context.Employee.Find(id);
            if (employee == null) return NotFound("Employee Not found");
            _context.Employee.Remove(employee);
            _context.SaveChanges();
            return Ok("Successfully delete employee");
        }
    }
}
