using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Exceptions;
using DeliveryService.ValueObjects;
using System.Numerics;

namespace DomainApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== СЕРВИС ДОСТАВКИ - ДЕМОНСТРАЦИЯ БИЗНЕС-ЛОГИКИ ===\n");

        try
        {
            // ========== 1. СОЗДАНИЕ ОТПРАВИТЕЛЯ И ПОЛУЧАТЕЛЯ ==========
            Console.WriteLine("--- 1. РЕГИСТРАЦИЯ ОТПРАВИТЕЛЯ И ПОЛУЧАТЕЛЯ ---");

            // Паспортные данные отправителя
            var senderPassport = new PassportData(
                new PassportSeries("4010"),
                new PassportNumber("123456"),
                issuedBy: "ОВД Тверской г. Москвы",
                issuedDate: new DateTime(2015, 05, 15),
                birthDate: new DateTime(1990, 01, 01),
                birthPlace: "г. Москва",
                registrationAddress: new AddressText("г. Москва, ул. Тверская, д. 1, кв. 10")
            );

            var sender = new Sender(
                new Name("Иван Петров"),
                new Phone("+79161234567"),
                senderPassport
            );

            // Паспортные данные получателя
            var receiverPassport = new PassportData(
                new PassportSeries("5020"),
                new PassportNumber("654321"),
                issuedBy: "ОВД Арбат г. Москвы",
                issuedDate: new DateTime(2018, 10, 20),
                birthDate: new DateTime(1995, 05, 15),
                birthPlace: "г. Москва",
                registrationAddress: new AddressText("г. Москва, ул. Арбат, д. 10, кв. 25")
            );

            var receiver = new Receiver(
                new Name("Мария Иванова"),
                new Phone("+79167654321"),
                receiverPassport
            );

            Console.WriteLine($"✅ Отправитель: {sender.Name.Value}, тел: {sender.Phone.Value}");
            Console.WriteLine($"   Паспорт: {sender.PassportData?.Series.Value} {sender.PassportData?.Number.Value}");
            Console.WriteLine($"✅ Получатель: {receiver.Name.Value}, тел: {receiver.Phone.Value}");
            Console.WriteLine($"   Паспорт: {receiver.PassportData?.Series.Value} {receiver.PassportData?.Number.Value}\n");

            // ========== 2. ФОРМИРОВАНИЕ ДОСТАВКИ ==========
            Console.WriteLine("--- 2. ФОРМИРОВАНИЕ ДОСТАВКИ ИЗ ПУНКТА А В ПУНКТ Б ---");

            var pickupAddress = new AddressText("г. Москва, ул. Тверская, д. 1");
            var deliveryAddress = new AddressText("г. Москва, ул. Арбат, д. 10");

            var delivery = sender.CreateDelivery(
                receiver,
                pickupAddress,
                deliveryAddress,
                PaymentMethod.Online
            );

            Console.WriteLine($"📦 Создана доставка #{delivery.Id.ToString()[..8]}...");
            Console.WriteLine($"   Откуда: {delivery.PickupAddress.Value}");
            Console.WriteLine($"   Куда: {delivery.DeliveryAddress.Value}");
            Console.WriteLine($"   Статус: {delivery.Status}");
            Console.WriteLine($"   Способ оплаты: {delivery.PaymentMethod}");
            Console.WriteLine($"   Статус оплаты: {delivery.PaymentStatus}\n");

            // ========== 3. ВЫБОР СПОСОБА ОПЛАТЫ ==========
            Console.WriteLine("--- 3. ВЫБОР СПОСОБА ОПЛАТЫ ---");

            delivery.SelectPaymentMethod(PaymentMethod.Online);
            Console.WriteLine($"💳 Выбран способ оплаты: {delivery.PaymentMethod}\n");

            // ========== 4. ОПЛАТА ==========
            Console.WriteLine("--- 4. ОПЛАТА ДОСТАВКИ ---");

            delivery.PayOnline();
            Console.WriteLine($"💰 Доставка оплачена. Статус оплаты: {delivery.PaymentStatus}\n");

            // ========== 5. НАЧАЛО ДОСТАВКИ ==========
            Console.WriteLine("--- 5. НАЧАЛО ДОСТАВКИ ---");

            delivery.StartDelivery();
            Console.WriteLine($"🚚 Доставка начата. Статус: {delivery.Status}\n");

            // ========== 6. ДЕМОНСТРАЦИЯ ПАСПОРТА ==========
            Console.WriteLine("--- 6. ДЕМОНСТРАЦИЯ ПАСПОРТА ПОЛУЧАТЕЛЕМ ---");

            delivery.ShowPassport();
            Console.WriteLine($"🆔 Паспорт показан курьеру: {delivery.ReceiverPassportShown}\n");

            // ========== 7. ПОЛУЧЕНИЕ ДОСТАВКИ ==========
            Console.WriteLine("--- 7. ПОЛУЧЕНИЕ ДОСТАВКИ ---");

            delivery.ReceiveDelivery();
            Console.WriteLine($"✅ Доставка получена! Статус: {delivery.Status}\n");

            // ========== 8. ПРОСМОТР ИСТОРИИ ЗАКАЗОВ ==========
            Console.WriteLine("--- 8. ПРОСМОТР ИСТОРИИ ЗАКАЗОВ ---");

            Console.WriteLine("📋 ИСТОРИЯ ДОСТАВОК ПОЛУЧАТЕЛЯ:");
            var receiverHistory = receiver.GetDeliveriesHistory();
            foreach (var d in receiverHistory)
            {
                Console.WriteLine($"   - Доставка #{d.Id.ToString()[..8]}... | Статус: {d.Status} | Создана: {d.CreatedAt:dd.MM.yyyy}");
            }
            Console.WriteLine();

            // ========== 9. ПРОВЕРКА БИЗНЕС-ПРАВИЛ (ИСКЛЮЧЕНИЯ) ==========
            Console.WriteLine("--- 9. ПРОВЕРКА БИЗНЕС-ПРАВИЛ (ИСКЛЮЧЕНИЯ) ---");

            Console.WriteLine("   Попытка начать уже доставленную доставку...");
            try
            {
                delivery.StartDelivery();
            }
            catch (InvalidDeliveryStatusException ex)
            {
                Console.WriteLine($"   ❌ Ошибка (ожидаемо): {ex.Message}");
            }

            Console.WriteLine("\n=== ДЕМОНСТРАЦИЯ УСПЕШНО ЗАВЕРШЕНА ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
