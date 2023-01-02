using System.Security.Claims;
using CSharpFunctionalExtensions;

namespace MvcMovie.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Maybe<long> UserId(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<long>.None;

        var claim = ClaimTypes.NameIdentifier;

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<long>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value.Split("###")[0];

            return value.IsNullOrEmptyOrWhiteSpace()
                ? Maybe<long>.None
                : Convert.ToInt64(value);
        }
        catch
        {
            return Maybe<long>.None;
        }
    }

    public static Maybe<long> TicketId(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<long>.None;

        var claim = ClaimTypes.NameIdentifier;

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<long>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value.Split("###")[1];

            return value.IsNullOrEmptyOrWhiteSpace()
                ? Maybe<long>.None
                : Convert.ToInt64(value);
        }
        catch
        {
            return Maybe<long>.None;
        }
    }

    public static Maybe<string> UserName(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<string>.None;

        var claim = ClaimTypes.Name;

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<string>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value;

            return value.IsNullOrEmptyOrWhiteSpace() ? Maybe<string>.None : Maybe<string>.From(value!);
        }
        catch
        {
            return Maybe<string>.None;
        }
    }

    public static Maybe<string> FirstName(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<string>.None;

        var claim = ClaimTypes.GivenName;

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<string>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value;

            return value.IsNullOrEmptyOrWhiteSpace() ? Maybe<string>.None : Maybe<string>.From(value!);
        }
        catch
        {
            return Maybe<string>.None;
        }
    }

    public static Maybe<string> LastName(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<string>.None;

        var claim = ClaimTypes.Surname;

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<string>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value;

            return value.IsNullOrEmptyOrWhiteSpace() ? Maybe<string>.None : Maybe<string>.From(value!);
        }
        catch
        {
            return Maybe<string>.None;
        }
    }

    public static Maybe<string> Email(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<string>.None;

        var claim = ClaimTypes.Email;

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<string>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value;

            return value.IsNullOrEmptyOrWhiteSpace() ? Maybe<string>.None : Maybe<string>.From(value!);
        }
        catch
        {
            return Maybe<string>.None;
        }
    }

    public static Maybe<string> Uid(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<string>.None;

        var claim = "uid";

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<string>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value;

            return value.IsNullOrEmptyOrWhiteSpace() ? Maybe<string>.None : Maybe<string>.From(value!);
        }
        catch
        {
            return Maybe<string>.None;
        }
    }

    public static Maybe<string> Uti(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            return Maybe<string>.None;

        var claim = "uti";

        if (!principal.HasClaim(_ => _.Type == claim))
            return Maybe<string>.None;

        try
        {
            string? value = principal.FindFirst(claim)?.Value;

            return value.IsNullOrEmptyOrWhiteSpace() ? Maybe<string>.None : Maybe<string>.From(value!);
        }
        catch
        {
            return Maybe<string>.None;
        }
    }
}