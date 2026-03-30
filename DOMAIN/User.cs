using System;
using System.Collections.Generic;
using System.Text;

namespace s32429_rent_shop.DOMAIN
{
    public abstract class User
    {
        public Guid Id { get; } = Guid.NewGuid();
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string PESEL { get; set; }

        protected User(string FirstName, string LastName, string PESEL)
        {
            if (PESEL.Length != 11 || !PESEL.All(char.IsDigit))
                throw new ArgumentException("Niepoprawny PESEL");
            this.PESEL = PESEL;
            this.FirstName = FirstName;
            this.LastName = LastName;

        }

        public abstract int MaxLoans { get; }
    }

    public class Student : User
    {
        public Student(string FirstName, string LastName, string PESEL) : base(FirstName, LastName, PESEL) { }
        public override int MaxLoans => 2;
    }

    public class Employee : User
    {
        public Employee(string FirstName, string LastName, string PESEL) : base(FirstName, LastName, PESEL) { }
        public override int MaxLoans => 5;
    }
}
