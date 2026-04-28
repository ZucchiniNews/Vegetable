using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class SuccessModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? SessionId { get; set; }

    public void OnGet(string? session_id)
    {
        SessionId = session_id;
    }
}
