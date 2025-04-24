using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HttpWebRequestWebAPI.Controller;

[ApiController]
[Route("api/communication")]
public class CommunicationController : ControllerBase
{
    
    private HttpWebRequest _httpWebRequest;

    [HttpGet("call")]
    public IActionResult Call([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("Please provide a valid `url` query parameter.");

        try
        {
            // initialize the request
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            _httpWebRequest.Method = "GET";

            using var response = (HttpWebResponse)_httpWebRequest.GetResponse();
            using var stream   = response.GetResponseStream();
            using var reader   = new StreamReader(stream!);
            var body           = reader.ReadToEnd();
            var contentType    = response.ContentType;

            return Content(body, contentType);
        }
        catch (WebException we)
        {
            var errorDetail = (we.Response as HttpWebResponse) is HttpWebResponse errResp
                ? $"{(int)errResp.StatusCode} {errResp.StatusDescription}"
                : we.Message;
            return BadRequest($"Error calling `{url}`: {errorDetail}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Unexpected error calling `{url}`: {ex.Message}");
        }
    }
    
    [HttpGet("call-dummy")]
    public IActionResult CallDummy()
    {
        const string dummyUrl = "https://jsonplaceholder.typicode.com/todos/1";
        return Call(dummyUrl);
    }
    
    [HttpGet("status")]
    public IActionResult Status()
    {
        return Ok("returning from HttpWebRequestWebAPI");
    }
}
