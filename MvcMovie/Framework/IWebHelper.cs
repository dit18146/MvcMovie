namespace MvcMovie.Framework;

public interface IWebHelper
    {
        /// <summary>
        ///     Gets whether the request is made with AJAX
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>True if the request is via AJAX</returns>
        bool IsAjaxRequest(HttpRequest request);

        /// <summary>
        ///     Get Url scheme
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetScheme();

        /// <summary>
        ///     Get UserAgent
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetUserAgent();

        /// <summary>
        ///     Get URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetUrlReferrer();

        /// <summary>
        ///     Get current host header
        /// </summary>
        /// <returns></returns>
        string GetCurrentHostHeader();

        /// <summary>
        ///     Get current host header with url scheme
        /// </summary>
        /// <returns></returns>
        string GetCurrentHostHeaderWithScheme();

        /// <summary>
        ///     Get current domain name
        /// </summary>
        /// <returns></returns>
        string GetDomainName();

        /// <summary>
        ///     Get IP address of the user taking under consideration extra headers
        /// </summary>
        /// <returns>String of IP address</returns>
        string GetCurrentIpAddress();

        /// <summary>
        ///     Get IP address of the user as reported by .net
        /// </summary>
        /// <returns></returns>
        string GetRemoteIpAddress();

        /// <summary>
        ///     Get IP address of the user as reported by .net
        /// </summary>
        /// <returns></returns>
        string GetRemoteIpAddressPort();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        string GetLocalIpAddress();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        string GetLocalIpAddressPort();

        /// <summary>
        ///     Returns the request path
        /// </summary>
        /// <returns></returns>
        string GetRequestPath();

        /// <summary>
        ///     Returns the request path with host header and scheme
        /// </summary>
        /// <returns></returns>
        string GetRequestPathWithHostHeaderAndScheme();

        /// <summary>
        ///     Returns true if the requested resource is an image, font, etc. Used to exlude request from certain pipelines
        /// </summary>
        /// <returns>True if the request targets a static resource file.</returns>
        bool IsStaticResource();

        /// <summary>
        ///     Returns true if the requested resource is an image, font, etc. Used to exlude request from certain pipelines
        /// </summary>
        /// <returns>True if the request targets a static resource file.</returns>
        bool IsStaticResource(PathString path);

        /// <summary>
        ///     Detects if this is a mobile device
        /// </summary>
        /// <param name="forceDesktopVersion"></param>
        /// <returns></returns>
        bool IsMobileDevice(bool forceDesktopVersion = false);

        /// <summary>
        ///     Detects if this is a mobile device
        /// </summary>
        /// <param name="userAgent"></param>
        /// <param name="forceDesktopVersion"></param>
        /// <returns></returns>
        bool IsMobileDevice(string userAgent,
            bool forceDesktopVersion = false);

        /// <summary>
        ///     Detects if hostname is a "mobile" hostname
        /// </summary>
        /// <param name="forceDesktopVersion"></param>
        /// <returns></returns>
        bool IsMobileUrl(bool forceDesktopVersion = false);

        /// <summary>
        ///     Returns all request headers
        /// </summary>
        /// <returns>a dictionary with all headers</returns>
        Dictionary<string, string> GetRequestHeaders();
    }