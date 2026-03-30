using s32429_rent_shop.DOMAIN;
using s32429_rent_shop.SERVICE;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var service = new RentService();

        // Przykładowe dodanie sprzętu do systemu
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

        // Przykładowe dodanie użytkowników do systemu
        service.AddUser(new Student("Jan", "Kowalski", "85010212345"));
        service.AddUser(new Employee("Anna", "Nowak", "90030354321"));


    }
}
