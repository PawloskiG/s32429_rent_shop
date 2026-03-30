using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public enum Equipment_Status
    {
        Available,
        Rented,
        Unavailable
    }
    public abstract class Equipment
    {
        public Guid Id { get; } = Guid.NewGuid();

        private Equipment_Status _status = Equipment_Status.Available;
        public Equipment_Status Status
        {
            get => _status;
            set
            {
                if (!Enum.IsDefined(typeof(Equipment_Status), value))
                    throw new ArgumentException("Invalid equipment status");
                _status = value;
            }
        }

        private string _vendor;
        public string Vendor
        {
            get => _vendor;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Vendor cannot be empty");
                _vendor = value;
            }
        }

        private string _model;
        public string Model
        {
            get => _model;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Model cannot be empty");
                _model = value;
            }
        }

        private string _serial_Number;
        public string Serial_Number
        {
            get => _serial_Number;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Serial number cannot be empty");
                _serial_Number = value;
            }
        }

        protected Equipment(string Vendor, string Model, string Serial_Number)
        {
            this.Vendor = Vendor;
            this.Model = Model;
            this.Serial_Number = Serial_Number;
        }

        public void ChangeStatus(Equipment_Status newStatus)
        {
            this.Status = newStatus;
        }

        public abstract void PrintInfo();

        protected void PrintBaseInfo()
        {
            Console.WriteLine($"==================== GUID: {Id}");
            Console.WriteLine($"Status: {Status}");
            Console.WriteLine($"Vendor: {Vendor}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Serial Number: {Serial_Number}");
        }
    }
}
