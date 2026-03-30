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
        private Presentation_Remote Interface { get; set; }

        public Prezentation_Remote(string Vendor, string Model, string Serial_Number, Presentation_Remote Interface) : base(Vendor, Model, Serial_Number)
        {
            this.Interface = Interface;
        }
    }
}
