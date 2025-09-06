using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SpearPoint.Domain.Entities;

namespace SpearPoint.Infrastructure.Services;
public static class DedupeHashService
{
    public static string Compute(Question q)
    {
        var sb = new StringBuilder();
        sb.Append(Normalize(q.Stem));


        // Stable order across choices, ignore label ordering
        foreach (var choiceText in q.Choices.Select(c => c.Text).Select(Normalize).OrderBy(t => t))
            sb.Append("|").Append(choiceText);


        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
        return ConvertToHex(bytes);
    }

    private static string Normalize(string s) =>
    string.Join(" ", (s ?? string.Empty).Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));

    private static string ConvertToHex(byte[] bytes)
    {
        var c = new char[bytes.Length * 2];
        int b; int i = 0;
        foreach (var t in bytes)
        {
            b = t >> 4;
            c[i++] = (char)(55 + b + (((b - 10) >> 31) & -7));
            b = t & 0xF;
            c[i++] = (char)(55 + b + (((b - 10) >> 31) & -7));
        }
        return new string(c);
    }

}
