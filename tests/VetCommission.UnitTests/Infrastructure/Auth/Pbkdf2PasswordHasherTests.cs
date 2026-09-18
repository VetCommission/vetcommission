using FluentAssertions;
using VetCommission.Infrastructure.Auth;

namespace VetCommission.UnitTests.Infrastructure.Auth;

public sealed class Pbkdf2PasswordHasherTests
{
    [Fact]
    public void Hash_returns_verifiable_hash_without_exposing_plain_password()
    {
        var hasher = new Pbkdf2PasswordHasher();

        var hash = hasher.Hash("Admin@123");

        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotContain("Admin@123");
        hasher.Verify(hash, "Admin@123").Should().BeTrue();
        hasher.Verify(hash, "wrong-password").Should().BeFalse();
    }
}
