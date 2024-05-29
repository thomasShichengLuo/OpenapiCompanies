using Microsoft.AspNetCore.Mvc;
using OpenapiCompanies.Companies;

namespace OpenapiCompanies.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CompaniesController : ControllerBase
    {

        private readonly ILogger<CompaniesController> _logger;
        private readonly ICompanyApiAgent _companyApiAgent;

        public CompaniesController(ILogger<CompaniesController> logger, ICompanyApiAgent companyApiAgent)
        {
            _logger = logger;
            _companyApiAgent = companyApiAgent;
        }

        /// <summary>
        /// v1/companies/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("companies/{id}")]
        public async Task<ActionResult<Company>> Get(int id)
        {
            try
            {
                if(id >= 0) 
                {
                    var company = await _companyApiAgent.GetCompanyAsync(id);
                    if(company != null)
                    {
                        return Ok(company);
                    }
                    return NotFound(new ErrorMessage() { Error= "Not Found", Error_description=$"Can not found the company {id}" });
                }
                else
                {
                    return NotFound(new ErrorMessage() { Error = "Wrong Id", Error_description = $"Can not found the company {id}" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCompany | Exception while getting company. companyId={id}");
                _logger.LogError($"\t{ex.Message}");
                _logger.LogError($"\t{ex.StackTrace}");
                return NotFound(new ErrorMessage() { Error = "Exception", Error_description = $"Can not found the company {id}" });
            }
            
        }
    }
}
