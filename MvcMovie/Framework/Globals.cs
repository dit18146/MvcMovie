using System.Globalization;

namespace MvcMovie.Framework;

/// <summary>
///     Global configuration constants
/// </summary>
public static class Globals
{
    public static class HostingInfo
    {
        /// <summary>
        ///     The  HTTP_X_FORWARDED_PROTO header
        /// </summary>
        public static string XForwardedProtoHeader => "X-Forwarded-Proto";

        /// <summary>
        ///     The X-FORWARDED-FOR header
        /// </summary>
        public static string XForwardedForHeader => "X-FORWARDED-FOR";

        /// <summary>
        ///     The Clouflare header
        /// </summary>
        public static string CloudflareHeader => "CF-Connecting-IP";
    }

    public static class Cdns
    {
        /// <summary>
        ///     Azure Blob Storage: https://assets.b2bgamingservices.com
        /// </summary>
        public static string AzureCdn => "https://assets.b2bgamingservices.com";

        /// <summary>
        ///     Legacy, used for teasers some site texts: https://content.b2bgamingservices.com
        /// </summary>
        public static string LegacyCdn => "https://content.b2bgamingservices.com";

        /// <summary>
        ///     Fav icon url
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static string FavIconsCdn(long siteId, string imageName) =>
            $"{AzureCdn}/sites/{siteId}/assets/favicons/{imageName}";
    }

    /// <summary>
    ///     Localization related configuration
    /// </summary>
    public static class Localization
    {
        /// <summary>
        ///     DefaultLanguageId  = 1 English
        /// </summary>
        public static long DefaultLanguageId => 1;

        /// <summary>
        ///     FallBackLanguageId  = 1 English
        /// </summary>
        public static long FallBackLanguageId => 1;
    }

    public static class Cookies
    {
        /// <summary>
        ///     Holds the bo authentication cookie (a break for legacy name)
        /// </summary>
        public static string SiteAuthenticationCookie => "__ex_ac_bo_v1";

        /// <summary>
        ///     Holds the languageId - legacy name
        /// </summary>
        public static string LanguageCookie => "bo.lang";
    }

    public static class Authentication
    {
        /// <summary>
        ///     Key for PreventSlidingExpirationAttibute
        /// </summary>
        public static string PreventSlidingExpiration => "PreventAuthSlidingRenewal";

        /// <summary>
        ///     Who Am I
        /// </summary>
        public static string MasterAdminRole => "MasterAdmin".ToLower(CultureInfo.InvariantCulture);

        /// <summary>
        ///     ServiceStack admin password
        /// </summary>
        public static string ServiceStackAdminPassword => "b0_pr0d1gy";
    }

    /// <summary>
    ///     Kendo related
    /// </summary>
    public static class Kendo
    {
        public static string Version => "2022.3.1109";
    }
}