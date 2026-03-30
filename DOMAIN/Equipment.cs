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
        private Guid Id { get; } = Guid.NewGuid();
        private Equipment_Status Status { get; set; } = Equipment_Status.Available;
        private string Vendor { get; set; }
        private string Model { get; set; }
        private string Serial_Number { get; set; }

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
    }
}
