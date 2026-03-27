// ============================================================
//  CarFactory.cs — Console Car Lookup
// ============================================================

using System;
using System.Collections.Generic;

enum CarType { Tesla, BMW, Toyota, Ferrari, Lada }

interface IElectric
{
    int BatteryCapacityKwh { get; }
    int RangeKm { get; }
    string ChargerType { get; }
}

interface IMechanical
{
    string FuelType { get; }
    double EngineVolumeLiters { get; }
    int HorsePower { get; }
}

interface IAutomatical
{
    string TransmissionName { get; }
}

interface IManual
{
    int Gears { get; }
}


interface ICar
{
    string Brand { get; }
    string Model { get; }
    int Seats { get; }
    int Year { get; }
    string GetDescription();
}

abstract class ACar : ICar
{
    public abstract string Brand { get; }
    public abstract string Model { get; }
    public abstract int Seats { get; }
    public abstract int Year { get; }

    protected string SeatsLabel => $"{Seats} места";
    protected string BaseInfo => $"{Year} г.в., {SeatsLabel}";

    public abstract string GetDescription();
}


abstract class AutomaticCombustionCar : ACar, IMechanical, IAutomatical
{
    public abstract string FuelType { get; }
    public abstract double EngineVolumeLiters { get; }
    public abstract int HorsePower { get; }
    public virtual string TransmissionName => "Автоматическая КПП";

    protected string MechanicalInfo =>
        $"{FuelType}, {EngineVolumeLiters}л, {HorsePower} л.с., {TransmissionName}";
}

//  ABSTRACT — ManualCombustionCar

abstract class ManualCombustionCar : ACar, IMechanical, IManual
{
    public abstract string FuelType { get; }
    public abstract double EngineVolumeLiters { get; }
    public abstract int HorsePower { get; }
    public abstract int Gears { get; }

    protected string MechanicalInfo =>
        $"{FuelType}, {EngineVolumeLiters}л, {HorsePower} л.с., {Gears}-ст. механика";
}

//  ABSTRACT — ElectricAutoCar

abstract class ElectricAutoCar : ACar, IElectric, IAutomatical
{
    public abstract int BatteryCapacityKwh { get; }
    public abstract int RangeKm { get; }
    public abstract string ChargerType { get; }
    public string TransmissionName => "Автоматическая (одноступенчатая)";

    protected string ElectricInfo =>
        $"Электромотор, {BatteryCapacityKwh} кВт⋅ч, запас хода {RangeKm} км, " +
        $"зарядка {ChargerType}, {TransmissionName}";
}


//  CONCRETE CARS


sealed class TeslaModelS : ElectricAutoCar
{
    public override string Brand => "Tesla";
    public override string Model => "Model S";
    public override int Seats => 5;
    public override int Year => 2023;
    public override int BatteryCapacityKwh => 100;
    public override int RangeKm => 652;
    public override string ChargerType => "Supercharger V3";

    public override string GetDescription() =>
        $"{Brand} {Model}: электрокар, {ElectricInfo}, {SeatsLabel}, Android-система на борту. ({BaseInfo})";
}

sealed class BMW5Series : AutomaticCombustionCar
{
    public override string Brand => "BMW";
    public override string Model => "5 Series";
    public override int Seats => 5;
    public override int Year => 2022;
    public override string FuelType => "Бензин";
    public override double EngineVolumeLiters => 2.0;
    public override int HorsePower => 184;

    public override string GetDescription() =>
        $"{Brand} {Model}: премиум седан, {MechanicalInfo}, {SeatsLabel}, " +
        $"навигация iDrive, подогрев сидений. ({BaseInfo})";
}

sealed class ToyotaCamry : AutomaticCombustionCar
{
    public override string Brand => "Toyota";
    public override string Model => "Camry";
    public override int Seats => 5;
    public override int Year => 2023;
    public override string FuelType => "Бензин";
    public override double EngineVolumeLiters => 2.5;
    public override int HorsePower => 181;

    public override string GetDescription() =>
        $"{Brand} {Model}: надёжный семейный седан, {MechanicalInfo}, {SeatsLabel}, " +
        $"Toyota Safety Sense, CarPlay/Android Auto. ({BaseInfo})";
}

sealed class Ferrari488 : AutomaticCombustionCar
{
    public override string Brand => "Ferrari";
    public override string Model => "488 GTB";
    public override int Seats => 2;
    public override int Year => 2021;
    public override string FuelType => "Бензин";
    public override double EngineVolumeLiters => 3.9;
    public override int HorsePower => 670;
    public override string TransmissionName => "7-ст. робот (PDK)";

    public override string GetDescription() =>
        $"{Brand} {Model}: спорткар, {MechanicalInfo}, {SeatsLabel}, " +
        $"карбоновый кузов, разгон 0–100 за 3.0 с. ({BaseInfo})";
}

sealed class LadaGranta : ManualCombustionCar
{
    public override string Brand => "Lada";
    public override string Model => "Granta";
    public override int Seats => 5;
    public override int Year => 2022;
    public override string FuelType => "Бензин";
    public override double EngineVolumeLiters => 1.6;
    public override int HorsePower => 106;
    public override int Gears => 5;

    public override string GetDescription() =>
        $"{Brand} {Model}: доступный отечественный седан, {MechanicalInfo}, {SeatsLabel}, " +
        $"кондиционер, ABS, подушки безопасности. ({BaseInfo})";
}

static class CarFactory
{
    public static ICar Create(CarType type) => type switch
    {
        CarType.Tesla => new TeslaModelS(),
        CarType.BMW => new BMW5Series(),
        CarType.Toyota => new ToyotaCamry(),
        CarType.Ferrari => new Ferrari488(),
        CarType.Lada => new LadaGranta(),
        _ => throw new ArgumentException($"Неизвестный тип: {type}")
    };
}


class Program
{
    private static readonly Dictionary<string, CarType> Aliases =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "tesla",   CarType.Tesla   },
            { "bmw",     CarType.BMW     },
            { "toyota",  CarType.Toyota  },
            { "ferrari", CarType.Ferrari },
            { "lada",    CarType.Lada    },
        };

    static void Main()
    {
        Console.WriteLine("=== Справочник автомобилей ===");
        Console.WriteLine($"Доступные марки: {string.Join(", ", Aliases.Keys)}");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Введите марку автомобиля или 'done' для выхода: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (input.Equals("done", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Готово!");
                break;
            }

            if (Aliases.TryGetValue(input, out CarType carType))
            {
                ICar car = CarFactory.Create(carType);
                Console.WriteLine($"\n«{car.GetDescription()}»\n");
            }
            else
            {
                Console.WriteLine($"Марка '{input}' не найдена. Попробуйте ещё раз.\n");
            }
        }
    }
}
