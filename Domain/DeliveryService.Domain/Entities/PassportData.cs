using DeliveryService.Domain.Base;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;

namespace DeliveryService.Domain.Entities;

public class PassportData : Entity<Guid>
{
    private PassportSeries? _series;
    private PassportNumber? _number;

    public PassportSeries Series
    {
        get => _series!;
        private set => _series = value;
    }

    public PassportNumber Number
    {
        get => _number!;
        private set => _number = value;
    }

    public string? IssuedBy { get; private set; }
    public DateTime? IssuedDate { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? BirthPlace { get; private set; }
    public AddressText? RegistrationAddress { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Sender? Sender { get; private set; }
    public Receiver? Receiver { get; private set; }

    protected PassportData() { }

    public PassportData(
        PassportSeries series,
        PassportNumber number,
        string? issuedBy = null,
        DateTime? issuedDate = null,
        DateTime? birthDate = null,
        string? birthPlace = null,
        AddressText? registrationAddress = null)
        : base(Guid.NewGuid())
    {
        Series = series ?? throw new ArgumentNullException(nameof(series));
        Number = number ?? throw new ArgumentNullException(nameof(number));
        IssuedBy = issuedBy;

        if (birthDate.HasValue && issuedDate.HasValue)
        {
            var minIssuedDate = birthDate.Value.AddYears(14);
            if (issuedDate.Value < minIssuedDate)
                throw new InvalidPassportDateException(birthDate.Value, issuedDate.Value);
        }

        IssuedDate = issuedDate;
        BirthDate = birthDate;
        BirthPlace = birthPlace;
        RegistrationAddress = registrationAddress;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool Update(
        PassportSeries? series = null,
        PassportNumber? number = null,
        string? issuedBy = null,
        DateTime? issuedDate = null,
        DateTime? birthDate = null,
        string? birthPlace = null,
        AddressText? registrationAddress = null)
    {
        bool updated = false;

        if (series != null && Series != series)
        {
            Series = series;
            updated = true;
        }

        if (number != null && Number != number)
        {
            Number = number;
            updated = true;
        }

        if (issuedBy != null && IssuedBy != issuedBy)
        {
            IssuedBy = issuedBy;
            updated = true;
        }

        if (issuedDate.HasValue && IssuedDate != issuedDate)
        {
            if (BirthDate.HasValue && issuedDate.Value < BirthDate.Value.AddYears(14))
                throw new InvalidPassportDateException(BirthDate.Value, issuedDate.Value);

            IssuedDate = issuedDate;
            updated = true;
        }

        if (birthDate.HasValue && BirthDate != birthDate)
        {
            if (IssuedDate.HasValue && IssuedDate.Value < birthDate.Value.AddYears(14))
                throw new InvalidPassportDateException(birthDate.Value, IssuedDate.Value);

            BirthDate = birthDate;
            updated = true;
        }

        if (birthPlace != null && BirthPlace != birthPlace)
        {
            BirthPlace = birthPlace;
            updated = true;
        }

        if (registrationAddress != null && RegistrationAddress != registrationAddress)
        {
            RegistrationAddress = registrationAddress;
            updated = true;
        }

        if (updated)
            UpdatedAt = DateTime.UtcNow;

        return updated;
    }
}