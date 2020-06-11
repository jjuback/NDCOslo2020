using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Processes.Pages
{
    public class ChartModel : PageModel
    {
        public List<object> Processes { get; set; }
        private readonly ILogger<ChartModel> _logger;

        public ChartModel(ILogger<ChartModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var items = Process.GetProcesses()
                .Where(p => !String.IsNullOrEmpty(p.ProcessName))
                .OrderByDescending(p => p.WorkingSet64)
                .Take(10);

            Processes = items.Select(p => new {
                Id = p.Id,
                Name = p.ProcessName,
                Memory = p.WorkingSet64
            }).ToList<object>();
        }
    }
}
