using System.Collections.Generic;
using s32429_rent_shop.DOMAIN;

class Program
{
    static void Main()
    {
        var laptop = new Laptop(
            "Dell",
            "XPS 15",
            "JSI89S0",
            8192,
            "Intel Core i7-8900K",
            16
            );

        var projector = new Projector(
            "Epson",
            "EB-U05",
            "PRJ12345",
            "1920x1080",
            3200,
            new List<Projector_Interface> { Projector_Interface.HDMI, Projector_Interface.VGA }
        );

        var camera = new Camera(
            "Canon",
            "EOS 250D",
            "CAM98765",
            24,
            Camera_Interface.USB_C
        );

        var student = new Student("Jan", "Kowalski", "85010212345");
        var employee = new Employee("Anna", "Nowak", "90030354321");
    }
}
