using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public class Laptop : Equipment
    {
        private int _ram_mb;
        public int RAM_MB
        {
            get => _ram_mb;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Pamięć RAM musi być większa od zera");
                _ram_mb = value;
            }
        }

        private string _cpu_name;
        public string CPU_Name
        {
            get => _cpu_name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa CPU nie może być pusta");
                _cpu_name = value;
            }
        }

        private float _size_inch;
        public float Size_Inch
        {
            get => _size_inch;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Rozmiar w calach musi być większy od zera");
                _size_inch = value;
            }
        }

        public Laptop(string Vendor, string Model, string Serial_Number, int RAM_MB, string CPU_Name, float Size_Inch) : base(Vendor, Model, Serial_Number)
        {
            this.RAM_MB = RAM_MB;
            this.CPU_Name = CPU_Name;
            this.Size_Inch = Size_Inch;
        }

        public override void PrintInfo()
        {
            PrintBaseInfo();
            Console.WriteLine($"RAM (MB): {RAM_MB}");
            Console.WriteLine($"CPU Name: {CPU_Name}");
            Console.WriteLine($"Size (inch): {Size_Inch}");
            Console.WriteLine($"    Equipment type: Laptop");
        }
    }
}
