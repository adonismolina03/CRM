using AAMG.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AAMG.AppWebMVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _hhttpClientAAMGAPI;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _hhttpClientAAMGAPI = httpClientFactory.CreateClient("AAMGAPI");
        }
        // GET: CustomerController
        public async Task<ActionResult> Index(SearchQueryCustomerDTO queryCustomerDTO, int CountRow = 0)
        {
            if (queryCustomerDTO.SendRowCount == 0)
                queryCustomerDTO.SendRowCount = 2;
            if (queryCustomerDTO.Take == 0)
                queryCustomerDTO.Take = 10;

            var result = new SearchResultCustomerDTO();

            var response = await _hhttpClientAAMGAPI.PostAsJsonAsync("/customer/search", queryCustomerDTO);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<SearchResultCustomerDTO>();

            result = result != null ? result : new SearchResultCustomerDTO();

            if (result.CountRow == 0 && queryCustomerDTO.SendRowCount == 1)
                result.CountRow = CountRow;

            ViewBag.CountRow = result.CountRow;
            queryCustomerDTO.SendRowCount = 0;
            ViewBag.SearchQuery = queryCustomerDTO;

            return View(result);
        }

        // GET: CustomerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var result = new GetIdResultCustomerDTO();

            var response = await _hhttpClientAAMGAPI.GetAsync("/customer/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

            return View(result ?? new GetIdResultCustomerDTO());
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCustomerDTO createDTO)
        {
            try
            {
                var response = await _hhttpClientAAMGAPI.PostAsJsonAsync("/customer", createDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = "Error al intentar guardar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var result = new GetIdResultCustomerDTO();
            var response = await _hhttpClientAAMGAPI.GetAsync("/customer/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

            return View(new EditCustomerDTO(result ?? new GetIdResultCustomerDTO()));
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditCustomerDTO editDTO)
        {try
            {
                var response = await _hhttpClientAAMGAPI.PutAsJsonAsync("/customer", editDTO);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = "Error al intentar editar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            var result = new GetIdResultCustomerDTO();
            var response = await _hhttpClientAAMGAPI.GetAsync("/customer/" + id);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

            return View(result ?? new GetIdResultCustomerDTO());
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, GetIdResultCustomerDTO getIdResultDTO)
        {
            try
            {
                var response = await _hhttpClientAAMGAPI.DeleteAsync("/customer/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = "Error al intentar editar el registro";
                return View(getIdResultDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultDTO);
            }
        }
    }
}
