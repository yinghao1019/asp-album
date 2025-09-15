using System.Net;
using System.Net.Sockets;

namespace asp_album.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    /// <summary>
    /// HTTP 狀態碼
    /// </summary>
    public int StatusCode { get; set; } = (int)(HttpStatusCode.OK);

    /// <summary>
    /// HTTP 狀態碼列舉名稱
    /// </summary>
    public string StatusCodeName { get; set; } = HttpStatusCode.OK.ToString();

    /// <summary>
    /// 訊息說明
    /// </summary>
    public string Message { get; set; } = string.Empty;

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
