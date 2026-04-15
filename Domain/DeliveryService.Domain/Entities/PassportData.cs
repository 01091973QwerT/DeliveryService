using DeliveryService.Domain.Base;
using DeliveryService.ValueObjects;
using DeliveryService.ValueObjects;
using System.Reflection;

namespace DeliveryService.Domain.Entities;

/// <summary>
/// Паспортные данные (общая таблица для отправителя и получателя)
/// Связь: один PassportData может принадлежать либо Sender, либо Receiver
/// </summary>
public class PassportData : Entity<Guid>
{
    // Паспортные данные (Value Objects)
    public PassportSeries Series { get; private set; }
    public PassportNumber Number { get; private set; }
    public string? IssuedBy { get; private set; }
    public DateTime? IssuedDate { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? BirthPlace { get; private set; }
    public AddressText? RegistrationAddress { get; private set; }

    // Временные метки
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Навигационные свойства (обратные ссылки)
    // PassportData может быть связан либо с Sender, либо с Receiver
    public Sender? Sender { get; private set; }
    public Receiver? Receiver { get; private set; }

    // Конструктор для EF Core
    private PassportData() { }

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
        IssuedDate = issuedDate;
        BirthDate = birthDate;
        BirthPlace = birthPlace;
        RegistrationAddress = registrationAddress;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(
        PassportSeries? series = null,
        PassportNumber? number = null,
        string? issuedBy = null,
        DateTime? issuedDate = null,
        DateTime? birthDate = null,
        string? birthPlace = null,
        AddressText? registrationAddress = null)
    {
        if (series != null) Series = series;
        if (number != null) Number = number;
        if (issuedBy != null) IssuedBy = issuedBy;
        if (issuedDate.HasValue) IssuedDate = issuedDate;
        if (birthDate.HasValue) BirthDate = birthDate;
        if (birthPlace != null) BirthPlace = birthPlace;
        if (registrationAddress != null) RegistrationAddress = registrationAddress;
        UpdatedAt = DateTime.UtcNow;
    }
}