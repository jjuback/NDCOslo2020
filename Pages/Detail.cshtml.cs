using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace Processes.Pages
{
    public class DetailModel : PageModel
    {
        private readonly ILogger<DetailModel> _logger;

        public Process Process { get; set; }

        public string[] PropertyList { get; set; }

        public DetailModel(ILogger<DetailModel> logger)
        {
            _logger = logger;

            PropertyList = new string[] {
                "Id",
                "ProcessName",
                "PriorityClass",
                "WorkingSet64"
            };
        }

        public void OnGet(int? id)
        {
            if (id.HasValue)
            {
                Process = Process.GetProcessById(id.Value);
            }
            
            if (Process == null)
            {
                NotFound();
            }
        }
        
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id.HasValue)
            {
                Process = Process.GetProcessById(id.Value);
            }
            
            if (Process == null)
            {
                return NotFound();
            }

            MessageBoxOptions options = new MessageBoxOptions("Are you sure you want to kill this process?");
            options.Type = MessageBoxType.question;
            options.Buttons = new string[] {"No", "Yes"};
            options.DefaultId = 1;
            options.CancelId = 0;
            MessageBoxResult result = await Electron.Dialog.ShowMessageBoxAsync(options);

            if (result.Response == 1)
            {
                Process.Kill();
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
