using System.Security.Claims;

namespace TeamTaskManager.Extension
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new Exception("Cannot get userId"));
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value ?? throw new Exception("Cannot get fullname");
        }
    }
}
