using SklaDinya_desktop_BL_component.Enums;
using SklaDinya_desktop_BL_component.Services;
using SklaDinya_desktop_BL_tests.Helpers;

namespace SklaDinya_desktop_BL_tests;

public class SessionServiceTests
{
    private readonly SessionService _sut = new();

    // ── SetToken ───────────────────────────────────────────────────────────

    [Fact]
    public void SetToken_ValidClientToken_TokenAndPayloadAreStored()
    {
        var token = JwtTestHelper.ClientToken();

        _sut.SetToken(token);

        Assert.Equal(token,          _sut.Token);
        Assert.NotNull(_sut.Payload);
        Assert.Equal(UserRole.Client, _sut.CurrentRole);
    }

    [Fact]
    public void SetToken_ValidOperatorToken_ParsesStorageIdAndOperatorRole()
    {
        var token = JwtTestHelper.OperatorToken();

        _sut.SetToken(token);

        Assert.Equal(UserRole.StorageOperator, _sut.CurrentRole);
        Assert.NotNull(_sut.Payload!.StorageId);
        Assert.Equal(OperatorRole.MainOperator, _sut.Payload.OperatorRole);
    }

    [Fact]
    public void SetToken_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.SetToken(""));
    }

    [Fact]
    public void SetToken_WhitespaceString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _sut.SetToken("   "));
    }

    // ── Clear ──────────────────────────────────────────────────────────────

    [Fact]
    public void Clear_AfterSetToken_TokenAndPayloadAreNull()
    {
        _sut.SetToken(JwtTestHelper.ClientToken());

        _sut.Clear();

        Assert.Null(_sut.Token);
        Assert.Null(_sut.Payload);
        Assert.Null(_sut.CurrentRole);
    }

    [Fact]
    public void Clear_WithoutPriorSetToken_DoesNotThrow()
    {
        var ex = Record.Exception(() => _sut.Clear());

        Assert.Null(ex);
    }

    // ── IsAuthenticated ────────────────────────────────────────────────────

    [Fact]
    public void IsAuthenticated_AfterSetToken_ReturnsTrue()
    {
        _sut.SetToken(JwtTestHelper.ClientToken());

        Assert.True(_sut.IsAuthenticated());
    }

    [Fact]
    public void IsAuthenticated_BeforeSetToken_ReturnsFalse()
    {
        Assert.False(_sut.IsAuthenticated());
    }

    [Fact]
    public void IsAuthenticated_AfterClear_ReturnsFalse()
    {
        _sut.SetToken(JwtTestHelper.ClientToken());
        _sut.Clear();

        Assert.False(_sut.IsAuthenticated());
    }
}
