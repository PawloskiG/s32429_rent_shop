using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public class Laptop : Equipment
    {
        private int RAM_MB { get; set; }
        private string CPU_Name { get; set; }
        private int Size_Inch { get; set; }

        public Laptop(string Vendor, string Model, string Serial_Number, int RAM_MB, string CPU_Name, int Size_Inch) : base(Vendor, Model, Serial_Number)
        {
            this.RAM_MB = RAM_MB;
            this.CPU_Name = CPU_Name;
            this.Size_Inch = Size_Inch;
        }
    }
}
