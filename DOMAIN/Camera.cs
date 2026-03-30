using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public enum Camera_Interface
    {
        USB_C,
        USB_A,
        WIFI,
        Bluetooth
    }

    public class Camera : Equipment
    {
        private int Megapixels { get; set; }
        private Camera_Interface Camera_Interface { get; set; }

        public Camera(string Vendor, string Model, string Serial_Number, int Megapixels, Camera_Interface Camera_Interface) : base(Vendor, Model, Serial_Number)
        {
            this.Megapixels = Megapixels;
            this.Camera_Interface = Camera_Interface;
        }
    }
}
