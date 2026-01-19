using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace Infrastructure.Email
{
    public class EmailTemplateService
    {
        private readonly Dictionary<string, EmailTemplateModel> _templates;
        public EmailTemplateService(IWebHostEnvironment environment)
        {
            var path = Path.Combine(environment.ContentRootPath, "EmailTemplates", "EmailTemplates.json");
            var json = File.ReadAllText(path);
            _templates = JsonSerializer.Deserialize
            <Dictionary<string, EmailTemplateModel>>(json)!;
        }
        public EmailTemplateModel Get(string key)
        {
            return _templates[key];
        }
    }
   
}
