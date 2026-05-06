using DeliveryService.Domain.Entities;

namespace DeliveryService.Domain.Exceptions;

/// <summary>
/// Исключение: дата выдачи паспорта должна быть не раньше 14 лет после даты рождения
/// </summary>
public class InvalidPassportDateException : InvalidOperationException
{
    public DateTime BirthDate { get; }
    public DateTime IssuedDate { get; }

    public InvalidPassportDateException(DateTime birthDate, DateTime issuedDate)
        : base($"Дата выдачи паспорта {issuedDate:dd.MM.yyyy} должна быть не раньше 14 лет после даты рождения {birthDate:dd.MM.yyyy}. Минимальная дата выдачи: {birthDate.AddYears(14):dd.MM.yyyy}")
    {
        BirthDate = birthDate;
        IssuedDate = issuedDate;
    }
}
