using WebApplication1.Models.Entity;

namespace WebApplication2.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = default!;
        public string EmployeeAge { get; set; } = default!;
    }
}
