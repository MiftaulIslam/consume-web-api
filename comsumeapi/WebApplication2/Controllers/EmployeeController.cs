using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class EmployeeController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5153/api");
        public readonly HttpClient _http;
        public EmployeeController()
        {
            _http = new HttpClient();
            _http.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<EmployeeViewModel> Employees = new List<EmployeeViewModel>();
            HttpResponseMessage response = _http.GetAsync(baseAddress+"/Employee/GetEmployees").Result;

            if (!response.IsSuccessStatusCode)
            {
                return Json("Something went wrong");
            }

            string data = response.Content.ReadAsStringAsync().Result;
            Employees = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(data);
            return View(Employees);


        }
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddEmployee(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(employee);
                StringContent content = new StringContent(data,System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = _http.PostAsync(baseAddress + "/Employee/AddEmployee", content).Result;
                if (!response.IsSuccessStatusCode) return Json("Something went wrong");
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            EmployeeViewModel employee = new EmployeeViewModel();

            HttpResponseMessage response = _http.GetAsync(baseAddress + $"/Employee/GetEmployee/{id}").Result;
            if (!response.IsSuccessStatusCode) return Json("Something went wrong, while getting the employee");
            

            string data = response.Content.ReadAsStringAsync().Result;

            employee = JsonConvert.DeserializeObject<EmployeeViewModel>(data);
            return View(employee);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {

                string data = JsonConvert.SerializeObject(employee);
                StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = _http.PutAsync(baseAddress + $"/Employee/UpdateEmployee/{employee.Id}", content).Result;

                if (!response.IsSuccessStatusCode) return Json($"Something went wrong, while Updating employee {employee.Id}");

                return RedirectToAction("Index");

            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {

            HttpResponseMessage response = _http.DeleteAsync(baseAddress + $"/Employee/DeleteEmployee/{id}").Result;

            if (!response.IsSuccessStatusCode) return Json($"Something went wrong, while deleting employee");
            return RedirectToAction("Index");
        }
    }
}
