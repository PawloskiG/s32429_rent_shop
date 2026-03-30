using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public class Rent
    {
        public Guid Id { get; } = Guid.NewGuid();
        private Equipment _equipment;
        public Equipment Equipment
        {
            get => _equipment;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Equipment), "Equipment nie może być null");
                _equipment = value;
            }
        }

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(User), "User nie może być null");
                _user = value;
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value == default)
                    throw new ArgumentException("StartDate musi być prawidłową datą");
                if (_dueDate != default && value > _dueDate)
                    throw new ArgumentException("StartDate nie może być późniejsza niż DueDate");
                _startDate = value;
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (value == default)
                    throw new ArgumentException("DueDate musi być prawidłową datą");
                // wykomentowane ze względu na punkt 4 i 6 ze scenariusza demonstracyjnego, muszę jakoś wpisać datę do zwrotu starszą niż bieżący dzień
                //if (_startDate != default && value < _startDate)
                //throw new ArgumentException("DueDate nie może być wcześniejsza niż StartDate");
                _dueDate = value;
            }
        }

        private DateTime? _returnDate;
        public DateTime? ReturnDate
        {
            get => _returnDate;
            set
            {
                if (value.HasValue && _startDate != default && value.Value < _startDate)
                    throw new ArgumentException("ReturnDate nie może być wcześniejsza niż StartDate");
                _returnDate = value;
            }
        }

        public Rent(Equipment equipment, User user, int days)
        {
            // wykomentowane ze względu na punkt 4 i 6 ze scenariusza demonstracyjnego, muszę jakoś wpisać datę do zwrotu starszą niż bieżący dzień
            //if (days <= 0)
                //throw new ArgumentException("Liczba dni musi być większa od zera", nameof(days));

            this.Equipment = equipment;
            this.User = user;
            this.StartDate = DateTime.Now;
            this.DueDate = this.StartDate.AddDays(days);
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
            if (ReturnDate.HasValue)
            {
                if (ReturnDate.Value <= DueDate)
                    return 0;

                var daysLate = (ReturnDate.Value - DueDate).Days;
                return daysLate * 10;
            }

            if (DateTime.Now <= DueDate)
                return 0;

            var daysOverdue = (DateTime.Now - DueDate).Days;
            return daysOverdue * 10;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"==================== GUID: {Id}");
            Console.WriteLine($"Equipment: {Equipment.Vendor} {Equipment.Model} {Equipment.Serial_Number} (ID: {Equipment.Id})");
            Console.WriteLine($"User: {User.FirstName} {User.LastName} (ID: {User.Id})");
            Console.WriteLine($"Start Date: {StartDate}");
            Console.WriteLine($"Due Date: {DueDate}");
            Console.WriteLine($"Return Date: {(ReturnDate.HasValue ? ReturnDate.Value.ToString() : "Not returned")}");
            Console.WriteLine($"Overdue: {(IsOverdue() ? "Yes" : "No")}");
            Console.WriteLine($"Penalty: {CalculatePenalty()} PLN");
        }
    }
}
