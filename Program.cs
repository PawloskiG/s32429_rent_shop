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

        // Przykładowe dodanie użytkowników do systemu                                                      # 2
        service.AddUser(new Student("Jan", "Kowalski", "90030354345"));
        service.AddUser(new Employee("Anna", "Nowak", "90030354321"));

        // Przykładowe wypożyczenie sprzętu                                                                 # 3
        var userForRent = service.GetAllUser().FirstOrDefault(u => u.PESEL.Equals("90030354345", StringComparison.OrdinalIgnoreCase));
        var equipmentForRent = service.GetAllEquipment().FirstOrDefault(e => e.Serial_Number.Equals("JSI89S0", StringComparison.OrdinalIgnoreCase));

        service.RentEquipment(equipmentForRent.Id, userForRent.Id, 5);

        equipmentForRent = service.GetAllEquipment().FirstOrDefault(e => e.Serial_Number.Equals("PRJ12345", StringComparison.OrdinalIgnoreCase));

        service.RentEquipment(equipmentForRent.Id, userForRent.Id, 2);

        // Próba wypożyczenia tego samego sprzętu ponownie i przekroczenie limitu wypożyczeń przez studenta # 4
        //service.RentEquipment(equipmentForRent.Id, userForRent.Id, 2); 

        // Przykładowe zwrócenie sprzętu w terminie                                                         # 5
        service.ReturnEquipment(equipmentForRent.Id);

        // Próba zwrotu opóźnionego sprzętu i naliczenie kary                                               # 6
        userForRent = service.GetAllUser().FirstOrDefault(u => u.PESEL.Equals("90030354321", StringComparison.OrdinalIgnoreCase));
        equipmentForRent = service.GetAllEquipment().FirstOrDefault(e => e.Serial_Number.Equals("CAM98765", StringComparison.OrdinalIgnoreCase));
        service.RentEquipment(equipmentForRent.Id, userForRent.Id, -5);


        // Generowanie raportów                                                                             # 7
        service.GenerateReportUser();
        service.GenerateReportEquipment();
        service.GenerateRaportRent();
        service.GenerateRaportSummary();
    }
}
