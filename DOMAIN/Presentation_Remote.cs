using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public enum Presentation_Remote
    {
        USB_A,
        USB_C,
        Bluetooth
    }

    public class Prezentation_Remote : Equipment
    {
        private Presentation_Remote _interface;
        public Presentation_Remote Interface
        {
            get => _interface;
            set
            {
                if (!Enum.IsDefined(typeof(Presentation_Remote), value))
                    throw new ArgumentException("Niepoprawny interfejs pilota prezentacji");
                _interface = value;
            }
        }

        public Prezentation_Remote(string Vendor, string Model, string Serial_Number, Presentation_Remote Interface) : base(Vendor, Model, Serial_Number)
        {
            this.Interface = Interface;
        }
    }
}
