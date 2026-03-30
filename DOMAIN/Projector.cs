using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public enum Projector_Interface
    {
        VGA,
        HDMI,
        DisplayPort,
        WIFI
    }

    public class Projector : Equipment
    {
        private string Resolution { get; set; }
        private int Lumens { get; set; }
        private List<Projector_Interface> Interfaces { get; }

        public Projector(string Vendor, string Model, string Serial_Number, string Resolution, int Lumens, List<Projector_Interface> Interfaces) : base(Vendor, Model, Serial_Number)
        {
            this.Resolution = Resolution;
            this.Lumens = Lumens;
            this.Interfaces = Interfaces;
        }
    }
}
