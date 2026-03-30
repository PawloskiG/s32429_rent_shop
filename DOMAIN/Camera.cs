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
        private int _megapixels;
        public int Megapixels
        {
            get => _megapixels;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Megapiksele muszą być większe od zera");
                _megapixels = value;
            }
        }

        private Camera_Interface _cameraInterface;
        public Camera_Interface Camera_Interface
        {
            get => _cameraInterface;
            set
            {
                if (!Enum.IsDefined(typeof(Camera_Interface), value))
                    throw new ArgumentException("Niepoprawny interfejs kamery");
                _cameraInterface = value;
            }
        }

        public Camera(string Vendor, string Model, string Serial_Number, int Megapixels, Camera_Interface Camera_Interface) : base(Vendor, Model, Serial_Number)
        {
            this.Megapixels = Megapixels;
            this.Camera_Interface = Camera_Interface;
        }

        public override void PrintInfo()
        {
            PrintBaseInfo();
            Console.WriteLine($"Megapixels: {Megapixels}");
            Console.WriteLine($"Camera Interface: {Camera_Interface}");
            Console.WriteLine($"    Equipment type: Camera");
        }
    }
}
