using FluentValidation.TestHelper;
using PoupaGuara.Auth.Models;
using PoupaGuara.Auth.Validators;

namespace PoupaGuara.Auth.UnitTests.Validators;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    private static CreateUserDto ValidDto() =>
        new("Guilherme", "gui@example.com", new DateOnly(1994, 5, 10), "segredo123");

    // ── happy path ──────────────────────────────────────────────────────────

    [Fact]
    public void Valid_input_passes()
    {
        _validator.TestValidate(ValidDto()).ShouldNotHaveAnyValidationErrors();
    }

    // ── name ────────────────────────────────────────────────────────────────

    [Fact]
    public void Name_null_fires_name_null()
    {
        _validator.TestValidate(ValidDto() with { Name = null })
            .ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode("name-null");
    }

    [Fact]
    public void Name_empty_fires_name_null()
    {
        _validator.TestValidate(ValidDto() with { Name = "" })
            .ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode("name-null");
    }

    [Fact]
    public void Name_101_chars_fires_name_too_long()
    {
        _validator.TestValidate(ValidDto() with { Name = new string('A', 101) })
            .ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode("name-too-long");
    }

    [Fact]
    public void Name_100_chars_passes()
    {
        _validator.TestValidate(ValidDto() with { Name = new string('A', 100) })
            .ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    // ── email ────────────────────────────────────────────────────────────────

    [Fact]
    public void Email_null_fires_email_null()
    {
        _validator.TestValidate(ValidDto() with { Email = null })
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorCode("email-null");
    }

    [Fact]
    public void Email_empty_fires_email_null()
    {
        _validator.TestValidate(ValidDto() with { Email = "" })
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorCode("email-null");
    }

    [Theory]
    [InlineData("nao-eh-email")]
    [InlineData("sem-arroba")]
    [InlineData("@semlocal.com")]
    [InlineData("sem-dominio@")]
    public void Email_invalid_pattern_fires_email_invalid_pattern(string bad)
    {
        _validator.TestValidate(ValidDto() with { Email = bad })
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorCode("email-invalid-pattern");
    }

    // ── birthDate ────────────────────────────────────────────────────────────

    [Fact]
    public void BirthDate_null_fires_birthday_null()
    {
        _validator.TestValidate(ValidDto() with { BirthDate = null })
            .ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorCode("birthday-null");
    }

    [Fact]
    public void BirthDate_null_does_not_fire_birthday_invalid()
    {
        // birthday-null and birthday-invalid must be mutually exclusive
        var result = _validator.TestValidate(ValidDto() with { BirthDate = null });
        Assert.DoesNotContain(result.Errors, e => e.ErrorCode == "birthday-invalid");
    }

    [Fact]
    public void BirthDate_today_fires_birthday_invalid()
    {
        _validator.TestValidate(ValidDto() with { BirthDate = DateOnly.FromDateTime(DateTime.Today) })
            .ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorCode("birthday-invalid");
    }

    [Fact]
    public void BirthDate_future_fires_birthday_invalid()
    {
        _validator.TestValidate(ValidDto() with { BirthDate = new DateOnly(2035, 1, 1) })
            .ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorCode("birthday-invalid");
    }

    [Fact]
    public void BirthDate_yesterday_passes()
    {
        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        _validator.TestValidate(ValidDto() with { BirthDate = yesterday })
            .ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }

    // ── password ─────────────────────────────────────────────────────────────

    [Fact]
    public void Password_null_fires_password_null()
    {
        _validator.TestValidate(ValidDto() with { Password = null })
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorCode("password-null");
    }

    [Fact]
    public void Password_empty_fires_password_null()
    {
        _validator.TestValidate(ValidDto() with { Password = "" })
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorCode("password-null");
    }

    [Fact]
    public void Password_7_chars_fires_password_too_short()
    {
        _validator.TestValidate(ValidDto() with { Password = "abc1234" })
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorCode("password-too-short");
    }

    [Fact]
    public void Password_8_chars_passes()
    {
        _validator.TestValidate(ValidDto() with { Password = "abc12345" })
            .ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    // ── combined / multiple errors ────────────────────────────────────────────

    [Fact]
    public void Empty_body_fires_all_null_codes()
    {
        var result = _validator.TestValidate(new CreateUserDto(null, null, null, null));
        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorCode("name-null");
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorCode("email-null");
        result.ShouldHaveValidationErrorFor(x => x.BirthDate).WithErrorCode("birthday-null");
        result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorCode("password-null");
    }

    [Fact]
    public void All_invalid_fires_expected_codes()
    {
        var dto = new CreateUserDto("Guilherme", "bad", new DateOnly(2099, 12, 31), "123");
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorCode("email-invalid-pattern");
        result.ShouldHaveValidationErrorFor(x => x.BirthDate).WithErrorCode("birthday-invalid");
        result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorCode("password-too-short");
    }
}
