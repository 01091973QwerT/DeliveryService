using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.ValueObjects;

namespace DomainApp;

internal class Program
{
    private static readonly List<Sender> Senders = [];
    private static readonly List<Receiver> Receivers = [];
    private static readonly List<Delivery> Deliveries = [];
    private static readonly List<HistoryItem> History = [];

    private static void Main(string[] args)
    {
        SeedDemoData();

        while (true)
        {
            ShowMainMenu();
            var choice = Console.ReadLine()?.Trim();

            try
            {
                switch (choice)
                {
                    case "1":
                        EnterSenderMode();
                        break;
                    case "2":
                        EnterReceiverMode();
                        break;
                    case "3":
                        CreateSender();
                        break;
                    case "4":
                        CreateReceiver();
                        break;
                    case "5":
                        ShowHistory();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("===== DeliveryService DomainApp =====");
        Console.WriteLine("1  - Войти как отправитель");
        Console.WriteLine("2  - Войти как получатель");
        Console.WriteLine("3  - Зарегистрировать отправителя");
        Console.WriteLine("4  - Зарегистрировать получателя");
        Console.WriteLine("5  - История завершенных/отмененных доставок");
        Console.WriteLine("0  - Выход");
        Console.Write("Выбор: ");
    }

    private static void SeedDemoData()
    {
        // Создаём тестовых отправителей
        var sender1 = new Sender(new Name("Иван Петров"), new Phone("+7-999-123-4567"));
        var sender2 = new Sender(new Name("Мария Сидорова"), new Phone("+7-999-765-4321"));
        Senders.AddRange([sender1, sender2]);

        // Создаём тестовых получателей
        var receiver1 = new Receiver(new Name("Алексей Козлов"), new Phone("+7-999-111-2222"));
        var receiver2 = new Receiver(new Name("Елена Новикова"), new Phone("+7-999-333-4444"));
        Receivers.AddRange([receiver1, receiver2]);
    }

    private static void CreateSender()
    {
        Console.Write("Имя отправителя: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.Write("Телефон отправителя: ");
        var phone = Console.ReadLine() ?? string.Empty;

        var sender = new Sender(new Name(name), new Phone(phone));
        Senders.Add(sender);
        Console.WriteLine($"Отправитель создан: {sender.Id} ({sender.Name})");
    }

    private static void CreateReceiver()
    {
        Console.Write("Имя получателя: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.Write("Телефон получателя: ");
        var phone = Console.ReadLine() ?? string.Empty;

        var receiver = new Receiver(new Name(name), new Phone(phone));
        Receivers.Add(receiver);
        Console.WriteLine($"Получатель создан: {receiver.Id} ({receiver.Name})");
    }

    private static void EnterSenderMode()
    {
        var sender = PickSender();
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Отправитель: {sender.Name} ===");
            Console.WriteLine("1 - Создать доставку (из пункта А в пункт Б)");
            Console.WriteLine("2 - Просмотр моих доставок");
            Console.WriteLine("3 - Повторный заказ");
            Console.WriteLine("4 - Отменить доставку");
            Console.WriteLine("5 - Изменить способ оплаты");
            Console.WriteLine("6 - Указать паспортные данные");
            Console.WriteLine("0 - Назад");
            Console.Write("Выбор: ");

            var c = Console.ReadLine()?.Trim();
            try
            {
                switch (c)
                {
                    case "1":
                        CreateDelivery(sender);
                        break;
                    case "2":
                        ViewSenderDeliveries(sender);
                        break;
                    case "3":
                        RepeatDelivery(sender);
                        break;
                    case "4":
                        CancelDelivery(sender);
                        break;
                    case "5":
                        ChangePaymentMethod(sender);
                        break;
                    case "6":
                        SetSenderPassportData(sender);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите Enter...");
            Console.ReadLine();
        }
    }

    private static void EnterReceiverMode()
    {
        var receiver = PickReceiver();
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Получатель: {receiver.Name} ===");
            Console.WriteLine("1 - Просмотр уведомлений о доставках");
            Console.WriteLine("2 - Указать паспортные данные");
            Console.WriteLine("0 - Назад");
            Console.Write("Выбор: ");

            var c = Console.ReadLine()?.Trim();
            try
            {
                switch (c)
                {
                    case "1":
                        ViewReceiverNotifications(receiver);
                        break;
                    case "2":
                        SetReceiverPassportData(receiver);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите Enter...");
            Console.ReadLine();
        }
    }

    private static Sender PickSender()
    {
        Console.WriteLine("Список отправителей:");
        for (int i = 0; i < Senders.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {Senders[i].Name} ({Senders[i].Phone})");
        }
        Console.Write("Выберите номер: ");
        var idx = int.Parse(Console.ReadLine() ?? "1") - 1;
        if (idx < 0 || idx >= Senders.Count) throw new IndexOutOfRangeException("Неверный номер");
        return Senders[idx];
    }

    private static Receiver PickReceiver()
    {
        Console.WriteLine("Список получателей:");
        for (int i = 0; i < Receivers.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {Receivers[i].Name} ({Receivers[i].Phone})");
        }
        Console.Write("Выберите номер: ");
        var idx = int.Parse(Console.ReadLine() ?? "1") - 1;
        if (idx < 0 || idx >= Receivers.Count) throw new IndexOutOfRangeException("Неверный номер");
        return Receivers[idx];
    }

    /// <summary>
    /// Use case: «Формирует доставку из пункта А в пункт Б».
    /// </summary>
    private static void CreateDelivery(Sender sender)
    {
        Console.WriteLine("=== Создание доставки ===");

        // Выбираем получателя
        var receiver = PickReceiver();

        Console.Write("Адрес забора (пункт А): ");
        var pickupAddr = Console.ReadLine() ?? string.Empty;

        Console.Write("Адрес доставки (пункт Б): ");
        var deliveryAddr = Console.ReadLine() ?? string.Empty;

        // Use case: «Выбор оплаты»
        Console.WriteLine("Выберите способ оплаты:");
        Console.WriteLine("  1 - Наличные");
        Console.WriteLine("  2 - Карта");
        Console.WriteLine("  3 - Онлайн");
        Console.Write("Выбор: ");
        var payChoice = Console.ReadLine()?.Trim();
        var paymentMethod = payChoice switch
        {
            "1" => PaymentMethod.Cash,
            "2" => PaymentMethod.Card,
            "3" => PaymentMethod.Online,
            _ => PaymentMethod.Cash
        };

        Console.Write("Показать паспорт получателя при доставке? (да/нет): ");
        var passportShown = (Console.ReadLine()?.Trim().ToLower() == "да");

        var delivery = sender.CreateDelivery(
            receiver,
            new Address(pickupAddr),
            new Address(deliveryAddr),
            paymentMethod,
            passportShown,
            DateTime.UtcNow
        );

        Deliveries.Add(delivery);
        Console.WriteLine($"Доставка создана: {delivery.Id}");
        Console.WriteLine($"  Из: {delivery.PickupAddress}");
        Console.WriteLine($"  В: {delivery.DeliveryAddress}");
        Console.WriteLine($"  Оплата: {delivery.PaymentMethod}");
        Console.WriteLine($"  Статус: {delivery.Status}");
    }

    /// <summary>
    /// Use case: «Просмотр заказов».
    /// </summary>
    private static void ViewSenderDeliveries(Sender sender)
    {
        Console.WriteLine($"=== Доставки отправителя «{sender.Name}» ===");
        var deliveries = sender.ViewDeliveries();
        if (deliveries.Count == 0)
        {
            Console.WriteLine("Нет доставок.");
            return;
        }

        int i = 1;
        foreach (var d in deliveries)
        {
            Console.WriteLine($"{i++}. [{d.Status}] {d.PickupAddress} -> {d.DeliveryAddress} (оплата: {d.PaymentMethod})");
        }
    }

    /// <summary>
    /// Use case: «Повторный заказ».
    /// </summary>
    private static void RepeatDelivery(Sender sender)
    {
        var deliveries = sender.ViewDeliveries().ToList();
        if (deliveries.Count == 0)
        {
            Console.WriteLine("Нет доставок для повтора.");
            return;
        }

        Console.WriteLine("Выберите доставку для повтора:");
        for (int i = 0; i < deliveries.Count; i++)
        {
            var d = deliveries[i];
            Console.WriteLine($"  {i + 1}. {d.PickupAddress} -> {d.DeliveryAddress}");
        }
        Console.Write("Номер: ");
        var idx = int.Parse(Console.ReadLine() ?? "1") - 1;
        if (idx < 0 || idx >= deliveries.Count) throw new IndexOutOfRangeException("Неверный номер");

        var existingDelivery = deliveries[idx];
        var newDelivery = sender.RepeatDelivery(sender, existingDelivery, DateTime.UtcNow);
        Deliveries.Add(newDelivery);

        Console.WriteLine($"Создан повторный заказ: {newDelivery.Id}");
    }

    private static void CancelDelivery(Sender sender)
    {
        var deliveries = sender.ViewDeliveries().Where(d => d.IsActive).ToList();
        if (deliveries.Count == 0)
        {
            Console.WriteLine("Нет активных доставок для отмены.");
            return;
        }

        Console.WriteLine("Выберите доставку для отмены:");
        for (int i = 0; i < deliveries.Count; i++)
        {
            var d = deliveries[i];
            Console.WriteLine($"  {i + 1}. [{d.Status}] {d.PickupAddress} -> {d.DeliveryAddress}");
        }
        Console.Write("Номер: ");
        var idx = int.Parse(Console.ReadLine() ?? "1") - 1;
        if (idx < 0 || idx >= deliveries.Count) throw new IndexOutOfRangeException("Неверный номер");

        var delivery = deliveries[idx];
        var cancelled = sender.CancelDelivery(sender, delivery);

        if (cancelled)
        {
            History.Add(new HistoryItem(delivery.Id, "Отменена", DateTime.UtcNow));
            Console.WriteLine($"Доставка {delivery.Id} отменена.");
        }
        else
        {
            Console.WriteLine("Доставка уже была отменена.");
        }
    }

    /// <summary>
    /// Use case: «Выбор оплаты» (изменение способа оплаты).
    /// </summary>
    private static void ChangePaymentMethod(Sender sender)
    {
        var deliveries = sender.ViewDeliveries().Where(d => d.IsActive).ToList();
        if (deliveries.Count == 0)
        {
            Console.WriteLine("Нет активных доставок.");
            return;
        }

        Console.WriteLine("Выберите доставку:");
        for (int i = 0; i < deliveries.Count; i++)
        {
            var d = deliveries[i];
            Console.WriteLine($"  {i + 1}. [{d.PaymentMethod}] {d.PickupAddress} -> {d.DeliveryAddress}");
        }
        Console.Write("Номер: ");
        var idx = int.Parse(Console.ReadLine() ?? "1") - 1;
        if (idx < 0 || idx >= deliveries.Count) throw new IndexOutOfRangeException("Неверный номер");

        Console.WriteLine("Выберите новый способ оплаты:");
        Console.WriteLine("  1 - Наличные");
        Console.WriteLine("  2 - Карта");
        Console.WriteLine("  3 - Онлайн");
        Console.Write("Выбор: ");
        var payChoice = Console.ReadLine()?.Trim();
        var newMethod = payChoice switch
        {
            "1" => PaymentMethod.Cash,
            "2" => PaymentMethod.Card,
            "3" => PaymentMethod.Online,
            _ => PaymentMethod.Cash
        };

        var delivery = deliveries[idx];
        var changed = sender.ChangePaymentMethod(sender, delivery, newMethod);

        if (changed)
        {
            Console.WriteLine($"Способ оплаты изменён на {newMethod}.");
        }
        else
        {
            Console.WriteLine("Способ оплаты не изменён (уже такой же).");
        }
    }

    /// <summary>
    /// Use case: «Дает паспортные данные» (отправитель).
    /// </summary>
    private static void SetSenderPassportData(Sender sender)
    {
        Console.WriteLine("=== Указание паспортных данных ===");
        var passportData = CreatePassportData();
        sender.SetPassportData(passportData);
        Console.WriteLine("Паспортные данные сохранены.");
    }

    /// <summary>
    /// Use case: «Дает паспортные данные» (получатель).
    /// </summary>
    private static void SetReceiverPassportData(Receiver receiver)
    {
        Console.WriteLine("=== Указание паспортных данных ===");
        var passportData = CreatePassportData();
        receiver.SetPassportData(passportData);
        Console.WriteLine("Паспортные данные сохранены.");
    }

    private static PassportData CreatePassportData()
    {
        Console.Write("Серия паспорта (4 цифры): ");
        var series = Console.ReadLine() ?? string.Empty;

        Console.Write("Номер паспорта (6 цифр): ");
        var number = Console.ReadLine() ?? string.Empty;

        Console.Write("Кем выдан: ");
        var issuedBy = Console.ReadLine() ?? string.Empty;

        Console.Write("Дата выдачи (yyyy-MM-dd): ");
        DateTime? issuedDate = null;
        var issuedDateStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(issuedDateStr) && DateTime.TryParse(issuedDateStr, out var parsedIssuedDate))
        {
            issuedDate = parsedIssuedDate;
        }

        Console.Write("Дата рождения (yyyy-MM-dd): ");
        DateTime? birthDate = null;
        var birthDateStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(birthDateStr) && DateTime.TryParse(birthDateStr, out var parsedBirthDate))
        {
            birthDate = parsedBirthDate;
        }

        Console.Write("Место рождения: ");
        var birthPlace = Console.ReadLine() ?? string.Empty;

        Console.Write("Адрес регистрации: ");
        var registrationAddress = Console.ReadLine() ?? string.Empty;

        return new PassportData(
            new PassportSeries(series),
            new PassportNumber(number),
            new Name(issuedBy),
            issuedDate,
            birthDate,
            new Name(birthPlace),
            new Address(registrationAddress)
        );
    }

    /// <summary>
    /// Use case: «Уведомление о доставке».
    /// </summary>
    private static void ViewReceiverNotifications(Receiver receiver)
    {
        Console.WriteLine($"=== Уведомления для получателя «{receiver.Name}» ===");

        // Находим все доставки для этого получателя
        var receiverDeliveries = Deliveries.Where(d => d.Receiver.Id == receiver.Id).ToList();

        if (receiverDeliveries.Count == 0)
        {
            Console.WriteLine("Нет доставок для вас.");
            return;
        }

        foreach (var delivery in receiverDeliveries)
        {
            Console.WriteLine($"\nДоставка от «{delivery.Sender.Name}»:");
            Console.WriteLine($"  Из: {delivery.PickupAddress}");
            Console.WriteLine($"  В: {delivery.DeliveryAddress}");
            Console.WriteLine($"  Статус: {delivery.Status}");

            var notifications = delivery.Notifications;
            if (notifications.Count > 0)
            {
                Console.WriteLine("  Уведомления:");
                foreach (var n in notifications)
                {
                    Console.WriteLine($"    [{n.SentAt:yyyy-MM-dd HH:mm}] {n.Message}");
                }
            }
            else
            {
                Console.WriteLine("  Уведомлений пока нет.");
            }
        }
    }

    private static void ShowHistory()
    {
        Console.WriteLine("=== История завершенных/отмененных доставок ===");
        if (History.Count == 0)
        {
            Console.WriteLine("История пуста.");
            return;
        }

        foreach (var item in History)
        {
            Console.WriteLine($"  [{item.ChangedAt:yyyy-MM-dd HH:mm}] Доставка {item.DeliveryId}: {item.Action}");
        }
    }
}

/// <summary>
/// Элемент истории для отслеживания изменений.
/// </summary>
internal record HistoryItem(Guid DeliveryId, string Action, DateTime ChangedAt);
