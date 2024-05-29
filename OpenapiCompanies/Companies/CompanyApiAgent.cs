using OpenapiCompanies.Config;
using System.Xml.Linq;

namespace OpenapiCompanies.Companies
{

    public interface ICompanyApiAgent
    {
        Task<Company> GetCompanyAsync(int companyId);
    }

    public class CompanyApiAgent : ICompanyApiAgent
    {

        public CompanyApiAgent(CompanyXMLApiOption companyXMLApiOption, ILogger<CompanyApiAgent> logger) 
        {
            _companyXMLApiOption = companyXMLApiOption;
            _logger = logger;
        }

        private readonly CompanyXMLApiOption _companyXMLApiOption;
        private readonly ILogger<CompanyApiAgent> _logger;

        /// <summary>
        /// GetCompanyAsync
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>Company</returns>
        public async Task<Company> GetCompanyAsync(int companyId)
        {
            string xmlUrl = _companyXMLApiOption.ApiUrl + companyId.ToString() + ".xml";
            XDocument xmlDoc = await GetXmlFromUrl(xmlUrl);

            if (xmlDoc != null)
            {
                
                var company = new Company();

                ParseXmlToCompany(xmlDoc, company);

                if(company != null)
                {
                    if(company.Id == companyId)
                    {
                        return company;
                    }
                    else
                    {
                        _logger.LogError($"Company id doesn't match while fetching company by id= {companyId}");
                    }
                }

                return null;
            }
            else
            {
                _logger.LogError($"Failed to fetch XML from the URL: {xmlUrl}");
                
            }
            return null;
        }

        /// <summary>
        /// GetXmlFromUrl
        /// </summary>
        /// <param name="url"></param>
        /// <returns>XDocument</returns>
        private async Task<XDocument> GetXmlFromUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string xmlString = await response.Content.ReadAsStringAsync();
                        return XDocument.Parse(xmlString);
                    }
                    else
                    {
                        _logger.LogError($"Failed to fetch XML from URL. Status code: {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred while fetching XML from URL: {ex.Message}");
                    return null;
                }
            }
        }

        /// <summary>
        /// ParseXmlToCompany
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="company"></param>
        private void ParseXmlToCompany(XDocument xmlDoc, Company company)
        {
            if (xmlDoc == null)
            {
                return;
            }
            XElement dataElement = xmlDoc.Root;
            if (dataElement != null && dataElement.Name == "Data")
            {
                string? IdString = dataElement.Element("id")?.Value;
                if(!string.IsNullOrEmpty(IdString))
                {
                    if(int.TryParse(IdString, out int id))
                    {
                        company.Id = id;
                    }
                }
                
                company.Name = dataElement.Element("name")?.Value;
                company.Description = dataElement.Element("description")?.Value;
            }
        }
    }
}
