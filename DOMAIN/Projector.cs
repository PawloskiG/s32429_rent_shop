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
        private string _resolution;
        public string Resolution
        {
            get => _resolution;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Rozdzielczość nie może być pusta");
                _resolution = value;
            }
        }

        private int _lumens;
        public int Lumens
        {
            get => _lumens;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Lumens musi być większy od zera");
                _lumens = value;
            }
        }

        private List<Projector_Interface> _interfaces;
        public List<Projector_Interface> Interfaces
        {
            get => _interfaces;
            set
            {
                if (value == null || value.Count == 0)
                    throw new ArgumentException("Lista interfejsów nie może być pusta");
                foreach (var i in value)
                {
                    if (!Enum.IsDefined(typeof(Projector_Interface), i))
                        throw new ArgumentException("Niepoprawny interfejs w liście");
                }
                _interfaces = value;
            }
        }

        public Projector(string Vendor, string Model, string Serial_Number, string Resolution, int Lumens, List<Projector_Interface> Interfaces) : base(Vendor, Model, Serial_Number)
        {
            this.Resolution = Resolution;
            this.Lumens = Lumens;
            this.Interfaces = Interfaces;
        }
    }
}
