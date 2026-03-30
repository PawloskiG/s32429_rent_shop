using s32429_rent_shop.DOMAIN;
using s32429_rent_shop.SERVICE;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var service = new RentService();

        // Przykładowe dodanie sprzętu do systemu                                                            #1
        try
        {
            service.AddEquipment(new Laptop(
                "Dell",
                "XPS 15",
                "JSI89S0",
                8192,
                "Intel Core i7-8900K",
                16
            ));

            service.AddEquipment(new Projector(
                "Epson",
                "EB-U05",
                "PRJ12345",
                "1920x1080",
                3200,
                new List<Projector_Interface> { Projector_Interface.HDMI, Projector_Interface.VGA }
            ));

            service.AddEquipment(new Camera(
                "Canon",
                "EOS 250D",
                "CAM98765",
                24,
                Camera_Interface.USB_C
            ));
        }
        catch (Exception ex)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Error during adding an equipment: {ex.Message}");
            Console.ForegroundColor = prevColor;
        }

        // Przykładowe dodanie użytkowników do systemu                                                      # 2
        try
        {
            service.AddUser(new Student("Jan", "Kowalski", "90030354345"));
            service.AddUser(new Employee("Anna", "Nowak", "90030354321"));
        }
        catch (Exception ex)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Error during adding an user: {ex.Message}");
            Console.ForegroundColor = prevColor;
        }
        

        // Przykładowe wypożyczenie sprzętu                                                                 # 3
        var userForRent = service.FindUsers(u => u.PESEL.Equals("90030354345", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        var equipmentForRent = service.FindEquipment(e => e.Serial_Number.Equals("JSI89S0", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

        try
        {
            service.RentEquipment(equipmentForRent, userForRent, 5);

            equipmentForRent = service.FindEquipment(e => e.Serial_Number.Equals("PRJ12345", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            service.RentEquipment(equipmentForRent, userForRent, 2);

            // Przykładowe zwrócenie sprzętu w terminie                                                         # 5
            service.ReturnEquipment(equipmentForRent);

            // Próba zwrotu opóźnionego sprzętu i naliczenie kary                                               # 6
            userForRent = service.FindUsers(u => u.PESEL.Equals("90030354321", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            equipmentForRent = service.FindEquipment(e => e.Serial_Number.Equals("CAM98765", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            service.RentEquipment(equipmentForRent, userForRent, -5);
        }
        catch (Exception ex)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Error during renting/returning an equipment: {ex.Message}");
            Console.ForegroundColor = prevColor;
        }

        try
        {
            // Próba wypożyczenia tego samego sprzętu ponownie i przekroczenie limitu wypożyczeń przez studenta # 4
            service.RentEquipment(equipmentForRent, userForRent, 2); 
        }
        catch(Exception ex)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Error during renting/returning an equipment: {ex.Message}");
            Console.ForegroundColor = prevColor;
        }

        // Generowanie raportów                                                                             # 7
        service.GenerateReportUser();
        service.GenerateReportEquipment();
        service.GenerateRaportRent();
        service.GenerateRaportSummary();
    }
}
