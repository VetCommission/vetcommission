using System.Security.Cryptography;
using VetCommission.Application.Features.Auth;

namespace VetCommission.Infrastructure.Auth;

public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 210_000;
    private const char Separator = '.';
    private const string Version = "v1";
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return string.Join(
            Separator,
            Version,
            Iterations.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(key));
    }

    public bool Verify(string passwordHash, string password)
    {
        if (string.IsNullOrWhiteSpace(passwordHash) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        var parts = passwordHash.Split(Separator);
        if (parts.Length != 4 || parts[0] != Version)
        {
            return false;
        }

        if (!int.TryParse(parts[1], System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var iterations))
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(parts[2]);
            var expectedKey = Convert.FromBase64String(parts[3]);
            var actualKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, expectedKey.Length);

            return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
