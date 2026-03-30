using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public abstract class User
    {
        public Guid Id { get; } = Guid.NewGuid();

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be empty");
                _firstName = value;
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be empty");
                _lastName = value;
            }
        }

        private string _pesel;
        public string PESEL
        {
            get => _pesel;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length != 11 || !value.All(char.IsDigit))
                    throw new ArgumentException("Invalid PESEL");
                _pesel = value;
            }
        }

        protected User(string FirstName, string LastName, string PESEL)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PESEL = PESEL;
        }

        public abstract int MaxLoans { get; }
        public abstract void PrintInfo();

        protected void PrintBaseInfo()
        {
            Console.WriteLine($"==================== GUID: {Id}");
            Console.WriteLine($"First Name: {FirstName}");
            Console.WriteLine($"Last Name: {LastName}");
            Console.WriteLine($"PESEL: {PESEL}");
            Console.WriteLine($"Max Loans: {MaxLoans}");
        }
    }

    public class Student : User
    {
        public Student(string FirstName, string LastName, string PESEL) : base(FirstName, LastName, PESEL) { }
        public override int MaxLoans => 2;
        public override void PrintInfo()
        {
            PrintBaseInfo();
            Console.WriteLine($"    Account Type: Student");
        }
    }

    public class Employee : User
    {
        public Employee(string FirstName, string LastName, string PESEL) : base(FirstName, LastName, PESEL) { }
        public override int MaxLoans => 5;
        public override void PrintInfo()
        {
            PrintBaseInfo();
            Console.WriteLine($"    Account Type: Employee");
        }
    }
}
