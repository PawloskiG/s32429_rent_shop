using s32429_rent_shop.SERVICE;
using s32429_rent_shop.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;

// Interactive text menu for performing operations without changing existing files.
// To run the menu call: `InteractiveMenu.Run()` from your `Program.Main()` (or use it manually from debugger).
public static class InteractiveMenu
{
    public static void Run(RentService service)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("========== RENT SHOP - MENU ==========");
            Console.WriteLine("1) Add equipment");
            Console.WriteLine("2) Add user");
            Console.WriteLine("3) List all equipment");
            Console.WriteLine("4) List available equipment");
            Console.WriteLine("5) List users");
            Console.WriteLine("6) Rent equipment");
            Console.WriteLine("7) Return equipment");
            Console.WriteLine("8) Mark equipment unavailable");
            Console.WriteLine("9) Reports");
            Console.WriteLine("10) Export all to JSON");
            Console.WriteLine("0) Exit");
            Console.Write("Choose option: ");

            var input = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (input)
                {
                    case "1":
                        AddEquipmentFlow(service);
                        break;
                    case "2":
                        AddUserFlow(service);
                        break;
                    case "3":
                        service.GenerateReportEquipment();
                        break;
                    case "4":
                        var avail = service.FindEquipment(e => e.Status == Equipment_Status.Available);
                        Console.WriteLine("--- Available equipment ---");
                        foreach (var e in avail) e.PrintInfo();
                        break;
                    case "5":
                        service.GenerateReportUser();
                        break;
                    case "6":
                        RentEquipmentFlow(service);
                        break;
                    case "7":
                        ReturnEquipmentFlow(service);
                        break;
                    case "8":
                        MarkUnavailableFlow(service);
                        break;
                    case "9":
                        ReportsFlow(service);
                        break;
                    case "10":
                        service.ExportAllToJson();
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
            catch (Exception ex)
            {
                var prev = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ForegroundColor = prev;
            }
        }
    }

    static void AddEquipmentFlow(RentService service)
    {
        Console.WriteLine("Choose equipment type: 1) Laptop  2) Projector  3) Camera");
        var t = Console.ReadLine();
        var vendor = ReadNonEmpty("Vendor: ");
        var model = ReadNonEmpty("Model: ");
        var serial = ReadNonEmpty("Serial number: ");

        switch (t)
        {
            case "1":
                var ram = ReadInt("RAM (MB): ");
                var cpu = ReadNonEmpty("CPU name: ");
                var size = ReadFloat("Size (inch): ");
                service.AddEquipment(new Laptop(vendor, model, serial, ram, cpu, size));
                break;
            case "2":
                var res = ReadNonEmpty("Resolution: ");
                var lum = ReadInt("Lumens: ");
                var interfaces = ReadProjectorInterfaces();
                service.AddEquipment(new Projector(vendor, model, serial, res, lum, interfaces));
                break;
            case "3":
                var mp = ReadInt("Megapixels: ");
                var camInterface = ReadCameraInterface();
                service.AddEquipment(new Camera(vendor, model, serial, mp, camInterface));
                break;
            default:
                Console.WriteLine("Unknown equipment type");
                break;
        }
    }

    static void AddUserFlow(RentService service)
    {
        Console.WriteLine("Choose user type: 1) Student  2) Employee");
        var t = Console.ReadLine();
        var first = ReadNonEmpty("First name: ");
        var last = ReadNonEmpty("Last name: ");
        var pesel = ReadNonEmpty("PESEL (11 digits): ");

        switch (t)
        {
            case "1":
                service.AddUser(new Student(first, last, pesel));
                break;
            case "2":
                service.AddUser(new Employee(first, last, pesel));
                break;
            default:
                Console.WriteLine("Unknown user type");
                break;
        }
    }

    static void RentEquipmentFlow(RentService service)
    {
        var serial = ReadNonEmpty("Equipment serial number: ");
        var userPesel = ReadNonEmpty("User PESEL: ");
        var days = ReadInt("Days to rent: ");

        var equipment = service.FindEquipment(e => e.Serial_Number.Equals(serial, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        var user = service.FindUsers(u => u.PESEL.Equals(userPesel, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

        service.RentEquipment(equipment, user, days);
    }

    static void ReturnEquipmentFlow(RentService service)
    {
        var serial = ReadNonEmpty("Equipment serial number to return: ");
        var equipment = service.FindEquipment(e => e.Serial_Number.Equals(serial, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        service.ReturnEquipment(equipment);
    }

    static void MarkUnavailableFlow(RentService service)
    {
        var serial = ReadNonEmpty("Equipment serial number to mark unavailable: ");
        var equipment = service.FindEquipment(e => e.Serial_Number.Equals(serial, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        if (equipment == null) throw new Exception("Equipment not found");
        service.MarkUnavailable(equipment.Id);
        Console.WriteLine("Equipment marked as unavailable");
    }

    static void ReportsFlow(RentService service)
    {
        Console.WriteLine("Reports: 1) Equipment  2) Users  3) Rents  4) Summary");
        var r = Console.ReadLine();
        switch (r)
        {
            case "1":
            {
                Func<Equipment, bool> predicate = null;
                Console.Write("Do you want to filter equipment report? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    Console.WriteLine("Equipment filters: 1) Vendor contains  2) Model contains  3) Status  4) Serial equals");
                    var f = Console.ReadLine();
                    switch (f)
                    {
                        case "1":
                            var vendor = ReadNonEmpty("Vendor contains: ");
                            predicate = e => e.Vendor != null && e.Vendor.IndexOf(vendor, StringComparison.OrdinalIgnoreCase) >= 0;
                            break;
                        case "2":
                            var model = ReadNonEmpty("Model contains: ");
                            predicate = e => e.Model != null && e.Model.IndexOf(model, StringComparison.OrdinalIgnoreCase) >= 0;
                            break;
                        case "3":
                            Console.WriteLine("Status options: Available, Rented, Unavailable");
                            var st = ReadNonEmpty("Status: ");
                            if (Enum.TryParse<Equipment_Status>(st, true, out var status))
                                predicate = e => e.Status == status;
                            else
                                Console.WriteLine("Unknown status - skipping filter");
                            break;
                        case "4":
                            var serial = ReadNonEmpty("Serial equals: ");
                            predicate = e => string.Equals(e.Serial_Number, serial, StringComparison.OrdinalIgnoreCase);
                            break;
                        default:
                            Console.WriteLine("Unknown filter option - no filter applied");
                            break;
                    }
                }

                service.GenerateReportEquipment(predicate);
                break;
            }
            case "2":
            {
                Func<User, bool> predicate = null;
                Console.Write("Do you want to filter users report? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    Console.WriteLine("User filters: 1) First name contains  2) Last name contains  3) PESEL equals");
                    var f = Console.ReadLine();
                    switch (f)
                    {
                        case "1":
                            var first = ReadNonEmpty("First name contains: ");
                            predicate = u => u.FirstName != null && u.FirstName.IndexOf(first, StringComparison.OrdinalIgnoreCase) >= 0;
                            break;
                        case "2":
                            var last = ReadNonEmpty("Last name contains: ");
                            predicate = u => u.LastName != null && u.LastName.IndexOf(last, StringComparison.OrdinalIgnoreCase) >= 0;
                            break;
                        case "3":
                            var pesel = ReadNonEmpty("PESEL equals: ");
                            predicate = u => string.Equals(u.PESEL, pesel, StringComparison.OrdinalIgnoreCase);
                            break;
                        default:
                            Console.WriteLine("Unknown filter option - no filter applied");
                            break;
                    }
                }

                service.GenerateReportUser(predicate);
                break;
            }
            case "3":
            {
                Func<Rent, bool> predicate = null;
                Console.Write("Do you want to filter rents report? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    Console.WriteLine("Rent filters: 1) Overdue only  2) User PESEL  3) Equipment serial  4) Active (not returned)");
                    var f = Console.ReadLine();
                    switch (f)
                    {
                        case "1":
                            predicate = rnt => rnt.IsOverdue();
                            break;
                        case "2":
                            var pesel = ReadNonEmpty("User PESEL: ");
                            predicate = rnt => string.Equals(rnt.User?.PESEL, pesel, StringComparison.OrdinalIgnoreCase);
                            break;
                        case "3":
                            var serial = ReadNonEmpty("Equipment serial: ");
                            predicate = rnt => string.Equals(rnt.Equipment?.Serial_Number, serial, StringComparison.OrdinalIgnoreCase);
                            break;
                        case "4":
                            predicate = rnt => rnt.ReturnDate == null;
                            break;
                        default:
                            Console.WriteLine("Unknown filter option - no filter applied");
                            break;
                    }
                }

                service.GenerateRaportRent(predicate);
                break;
            }
            case "4":
                service.GenerateRaportSummary();
                break;
            default:
                Console.WriteLine("Unknown report option");
                break;
        }
    }

    static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
            Console.WriteLine("Value cannot be empty");
        }
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (int.TryParse(s, out var v)) return v;
            Console.WriteLine("Invalid integer");
        }
    }

    static float ReadFloat(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (float.TryParse(s, out var v)) return v;
            Console.WriteLine("Invalid number");
        }
    }

    static List<Projector_Interface> ReadProjectorInterfaces()
    {
        var list = new List<Projector_Interface>();
        Console.WriteLine("Available interfaces: ");
        foreach (var name in Enum.GetNames(typeof(Projector_Interface)))
            Console.WriteLine($" - {name}");
        Console.WriteLine("Enter interfaces separated by comma (e.g. HDMI,VGA): ");
        var raw = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(raw)) throw new Exception("Interfaces cannot be empty");
        var parts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var p in parts)
        {
            if (Enum.TryParse<Projector_Interface>(p, true, out var val))
                list.Add(val);
            else
                Console.WriteLine($"Unknown interface ignored: {p}");
        }
        if (list.Count == 0) throw new Exception("No valid interfaces provided");
        return list;
    }

    static Camera_Interface ReadCameraInterface()
    {
        Console.WriteLine("Available camera interfaces: ");
        foreach (var name in Enum.GetNames(typeof(Camera_Interface)))
            Console.WriteLine($" - {name}");
        while (true)
        {
            Console.Write("Choose camera interface: ");
            var s = Console.ReadLine();
            if (Enum.TryParse<Camera_Interface>(s, true, out var val)) return val;
            Console.WriteLine("Invalid camera interface");
        }
    }
}
