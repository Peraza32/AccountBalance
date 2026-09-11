using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace AccountDashboard.Web.Common
{
    public static class SessionExtensions
    {
        private const string SelectedCardKey = "SelectedCard";

        public static void SetSelectedCard(this ISession session, SelectedCardContext context)
        {
            session.SetString(SelectedCardKey, JsonSerializer.Serialize(context));
        }

        public static SelectedCardContext? GetSelectedCard(this ISession session)
        {
            var raw = session.GetString(SelectedCardKey);
            return string.IsNullOrEmpty(raw) ? null : JsonSerializer.Deserialize<SelectedCardContext>(raw);
        }

        public static void ClearSelectedCard(this ISession session)
        {
            session.Remove(SelectedCardKey);
        }
    }
}
