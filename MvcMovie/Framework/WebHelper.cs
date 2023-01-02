#nullable disable
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using MvcMovie.Extensions;

// ReSharper disable PossibleNullReferenceException

namespace MvcMovie.Framework;

public class WebHelper : IWebHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebHelper(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    /// <summary>
    ///     Gets whether the request is made with AJAX
    /// </summary>
    /// <param name="request">HTTP request</param>
    /// <returns>True if the request is via AJAX</returns>
    public bool IsAjaxRequest(HttpRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    public string GetScheme()
    {
        string result = _httpContextAccessor.HttpContext.Request.Scheme;

        //cloudflare fills this
        var xForwardedProtoHeader =
            _httpContextAccessor.HttpContext.Request.Headers[Globals.HostingInfo.XForwardedProtoHeader];

        if (!string.IsNullOrEmpty(xForwardedProtoHeader))
            result = xForwardedProtoHeader.FirstOrDefault();

        return result;
    }

    /// <summary>
    ///     Get UserAgent
    /// </summary>
    /// <returns>URL referrer</returns>
    public string GetUserAgent()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
    }

    /// <summary>
    ///     Get URL referrer
    /// </summary>
    /// <returns>URL referrer</returns>
    public string GetUrlReferrer()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
    }

    /// <summary>
    ///     Get current host header
    /// </summary>
    /// <returns></returns>
    public string GetCurrentHostHeader()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Request.Host.Value;
    }

    /// <summary>
    ///     Get current host header with url scheme
    /// </summary>
    /// <returns></returns>
    public string GetCurrentHostHeaderWithScheme()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return GetScheme() + Uri.SchemeDelimiter + _httpContextAccessor.HttpContext.Request.Host.Value;
    }

    /// <summary>
    ///     Get current domain name
    /// </summary>
    /// <returns></returns>
    public string GetDomainName()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        string host = GetCurrentHostHeader();

        string[] parts = host.Split('.');

        int count = parts.Length;

        switch (count)
        {
            case < 2: //localhost
                return host;
            case 2:
                return $"{parts[0]}.{parts[1]}";
            case > 3:
                return $"{parts[count - 3]}.{parts[count - 2]}.{parts[count - 1]}";
            default:
                return $"{parts[count - 2]}.{parts[count - 1]}";
        }
    }

    /// <summary>
    ///     Get IP address of the user
    /// </summary>
    /// <returns>String of IP address</returns>
    public string GetCurrentIpAddress()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        var result = string.Empty;

        try
        {
            //TODO: clear steps needed depending on which proxy we are behind - now check with cloudflare
            if (_httpContextAccessor.HttpContext.Request.Headers != null)
            {
                var forwardedHttpHeaderKey = Globals.HostingInfo.CloudflareHeader;

                var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];

                if (!string.IsNullOrEmpty(forwardedHeader))
                    result = forwardedHeader.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        catch
        {
            return string.Empty;
        }

        if (result != null && result.Equals(IPAddress.IPv6Loopback.ToString(),
                StringComparison.InvariantCultureIgnoreCase))
            result = IPAddress.Loopback.ToString();

        if (IPAddress.TryParse(result ?? string.Empty, out var ip))
            //IP address is valid 
            result = ip.ToString();
        else if (!string.IsNullOrEmpty(result))
            //remove port
            result = result.Split(':').FirstOrDefault();

        return result;
    }

    /// <summary>
    ///     Get IP address of the user as reported by .net
    /// </summary>
    /// <returns></returns>
    public string GetRemoteIpAddress()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
    }

    /// <summary>
    ///     Get IP address of the user as reported by .net
    /// </summary>
    /// <returns></returns>
    public string GetRemoteIpAddressPort()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Connection.RemotePort.ToString();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public string GetLocalIpAddress()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public string GetLocalIpAddressPort()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Connection.LocalPort.ToString();
    }

    /// <inheritdoc />
    public string GetRequestPath()
    {
        if (!IsRequestAvailable())
            return string.Empty;

        return _httpContextAccessor.HttpContext.Request.Path;
    }

    /// <inheritdoc />
    public string GetRequestPathWithHostHeaderAndScheme() => $"{GetCurrentHostHeaderWithScheme()}{GetRequestPath()}";

    /// <summary>
    ///     Returns true if the requested resource is an image, font, etc. Used to exlude request from certain pipelines
    /// </summary>
    /// <returns>True if the request targets a static resource file.</returns>
    public bool IsStaticResource()
    {
        if (!IsRequestAvailable())
            return false;

        string path = _httpContextAccessor.HttpContext.Request.Path;

        return IsStaticResource(path);
    }

    public bool IsStaticResource(PathString path)
    {
        if (!path.HasValue)
            return false;

        var contentTypeProvider = new FileExtensionContentTypeProvider();

        return contentTypeProvider.TryGetContentType(path.Value, out string _);
    }

    /// <inheritdoc />
    public bool IsMobileDevice(bool forceDesktopVersion = false)
    {
        if (!IsRequestAvailable())
            return false;

        if (forceDesktopVersion) return false;

        string userAgent = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        return IsMobileDevice(userAgent, forceDesktopVersion);
    }

    /// <inheritdoc />
    public bool IsMobileDevice(string userAgent, bool forceDesktopVersion = false)
    {
        if (userAgent.IsNullOrEmptyOrWhiteSpace())
            return false;

        if (forceDesktopVersion) return false;

        const string mobileRegExCheckMajor =
            @"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|fban|fbav|playbook|tablet|mobile|android+";

        const string mobileRegExCheckVariation =
            @"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-";


        return Regex.IsMatch(userAgent, mobileRegExCheckMajor, RegexOptions.IgnoreCase | RegexOptions.Multiline) ||
               Regex.IsMatch(userAgent.Substring(0, 4), mobileRegExCheckVariation,
                   RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }

    /// <summary>
    ///     Detects if hostname is a "mobile" hostname
    /// </summary>
    /// <param name="forceDesktopVersion"></param>
    /// <returns></returns>
    public bool IsMobileUrl(bool forceDesktopVersion = false)
    {
        if (!IsRequestAvailable())
            return false;

        if (forceDesktopVersion) return false;

        string hostName = GetCurrentHostHeaderWithScheme();

        return hostName.Contains("://m.") || hostName.Contains("://dev-m.");
    }

    /// <summary>
    ///     Returns all request headers
    /// </summary>
    /// <returns>a dictionary with all headers</returns>
    public Dictionary<string, string> GetRequestHeaders()
    {
        return _httpContextAccessor.HttpContext.Request.Headers
            .ToDictionary<KeyValuePair<string, StringValues>, string, string>(item => item.Key,
                item => item.Value);
    }

    #region private

    /// <summary>
    ///     Check whether current HTTP request is available
    /// </summary>
    /// <returns>True if available; otherwise false</returns>
    protected virtual bool IsRequestAvailable()
    {
        if (_httpContextAccessor?.HttpContext == null)
            return false;

        try
        {
            if (_httpContextAccessor.HttpContext.Request == null)
                return false;
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    #endregion
}
#nullable enable