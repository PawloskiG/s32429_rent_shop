using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public enum Presentation_Remote_Interface
    {
        USB_A,
        USB_C,
        Bluetooth
    }

    public class Presentation_Remote : Equipment
    {
        private Presentation_Remote_Interface _interface;
        public Presentation_Remote_Interface Interface
        {
            get => _interface;
            set
            {
                if (!Enum.IsDefined(typeof(Presentation_Remote), value))
                    throw new ArgumentException("Niepoprawny interfejs pilota prezentacji");
                _interface = value;
            }
        }

        private bool _hasPointer;
        public bool HasPointer
        {
            get => _hasPointer;
            set
            {
                _hasPointer = value;
            }
        }

        public Presentation_Remote(string Vendor, string Model, string Serial_Number, Presentation_Remote_Interface Interface, bool HasPointer) : base(Vendor, Model, Serial_Number)
        {
            this.Interface = Interface;
            this.HasPointer = HasPointer;
        }

        public override void PrintInfo()
        {
            PrintBaseInfo();
            Console.WriteLine($"Presentation Remote Interface: {Interface}");
            Console.WriteLine($"Has Pointer: {HasPointer}");
            Console.WriteLine($"    Equipment type: Presentation Remote");
        }
    }
}
