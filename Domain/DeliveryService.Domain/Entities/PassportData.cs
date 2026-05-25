using DeliveryService.Domain.Base;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Паспортные данные (passport_datas).
/// </summary>
public class PassportData : Entity<Guid>
{
    public PassportSeries Series { get; private set; } = default!;
    public PassportNumber Number { get; private set; } = default!;
    public Name IssuedBy { get; private set; } = default!;
    public DateTime? IssuedDate { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public Name BirthPlace { get; private set; } = default!;
    public Address RegistrationAddress { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected PassportData()
    {
    }

    public PassportData(
        PassportSeries series,
        PassportNumber number,
        Name issuedBy,
        DateTime? issuedDate,
        DateTime? birthDate,
        Name birthPlace,
        Address registrationAddress
    ) : this(Guid.NewGuid(), series, number, issuedBy, issuedDate, birthDate, birthPlace, registrationAddress, DateTime.UtcNow, DateTime.UtcNow)
    {
    }

    protected PassportData(
        Guid id,
        PassportSeries series,
        PassportNumber number,
        Name issuedBy,
        DateTime? issuedDate,
        DateTime? birthDate,
        Name birthPlace,
        Address registrationAddress,
        DateTime createdAt,
        DateTime updatedAt
    ) : base(id)
    {
        Series = series ?? throw new ArgumentNullValueException(nameof(series));
        Number = number ?? throw new ArgumentNullValueException(nameof(number));
        IssuedBy = issuedBy ?? throw new ArgumentNullValueException(nameof(issuedBy));
        IssuedDate = issuedDate;
        BirthDate = birthDate;
        BirthPlace = birthPlace ?? throw new ArgumentNullValueException(nameof(birthPlace));
        RegistrationAddress = registrationAddress ?? throw new ArgumentNullValueException(nameof(registrationAddress));
        CreatedAt = createdAt.Kind == DateTimeKind.Utc ? createdAt : DateTime.SpecifyKind(createdAt, DateTimeKind.Utc);
        UpdatedAt = updatedAt.Kind == DateTimeKind.Utc ? updatedAt : DateTime.SpecifyKind(updatedAt, DateTimeKind.Utc);
    }

    /// <summary>
    /// Обновляет паспортные данные.
    /// </summary>
    public void Update(
        PassportSeries series,
        PassportNumber number,
        Name issuedBy,
        DateTime? issuedDate,
        DateTime? birthDate,
        Name birthPlace,
        Address registrationAddress
    )
    {
        Series = series ?? throw new ArgumentNullValueException(nameof(series));
        Number = number ?? throw new ArgumentNullValueException(nameof(number));
        IssuedBy = issuedBy ?? throw new ArgumentNullValueException(nameof(issuedBy));
        IssuedDate = issuedDate;
        BirthDate = birthDate;
        BirthPlace = birthPlace ?? throw new ArgumentNullValueException(nameof(birthPlace));
        RegistrationAddress = registrationAddress ?? throw new ArgumentNullValueException(nameof(registrationAddress));
        UpdatedAt = DateTime.UtcNow;
    }
}
