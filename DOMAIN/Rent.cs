using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public class Rent
    {
        private Equipment Equipment { get; }
        private User User { get; }
        private DateTime StartDate { get; }
        private DateTime DueDate { get; }
        private DateTime? ReturnDate { get; set; }

        public Rent(Equipment equipment, User user, int days)
        {
            Equipment = equipment;
            User = user;
            StartDate = DateTime.Now;
            DueDate = StartDate.AddDays(days);
        }

        public void Return()
        {
            ReturnDate = DateTime.Now;
        }

        public bool IsOverdue()
        {
            return !ReturnDate.HasValue && DateTime.Now > DueDate;
        }

        public decimal CalculatePenalty()
        {
            if (!ReturnDate.HasValue || ReturnDate <= DueDate)
                return 0;

            var daysLate = (ReturnDate.Value - DueDate).Days;
            return daysLate * 10; // 10 PLN per day
        }
    }
}
