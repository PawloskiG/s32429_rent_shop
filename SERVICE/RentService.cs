using s32429_rent_shop.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s32429_rent_shop.SERVICE
{
    public class RentService
    {
        private List<Equipment> _equipment = new();
        private List<User> _users = new();
        private List<Rent> _rents = new();

        public void AddEquipment(Equipment equipment) => _equipment.Add(equipment);
        public void AddUser(User user) => _users.Add(user);

        public void RentEquipment(Equipment equipment, User user, int days)
        { 
            if(equipment == null)
                throw new Exception("Equipment not found");
            if (user == null)
                throw new Exception("User not found");

            if (equipment.Status != Equipment_Status.Available)
                throw new Exception("Equipment not available");

            var activeLoans = _rents.Count(r => r.User.Id == user.Id && r.ReturnDate == null);
            if (activeLoans >= user.MaxLoans)
                throw new Exception("User exceeded limit");

            var rental = new Rent(equipment, user, days);
            _rents.Add(rental);
            equipment.Status = Equipment_Status.Rented;
        }

        public void ReturnEquipment(Equipment equipment)
        {
            if (equipment == null)
                throw new ArgumentException("Invalid equipment ID");

            var rental = _rents.First(r => r.Equipment.Id == equipment.Id && r.ReturnDate == null);
            rental.Return();
            rental.Equipment.Status = Equipment_Status.Available;

            var penalty = rental.CalculatePenalty();
            if (penalty > 0)
                Console.WriteLine($"Penalty: {penalty} PLN");
        }

        public IEnumerable<Equipment> GetAllEquipment() => _equipment;

        public IEnumerable<Equipment> FindEquipment(Func<Equipment, bool> predicate) => _equipment.Where(predicate);

        public IEnumerable<Equipment> GetAvailableEquipment() => _equipment.Where(e => e.Status == Equipment_Status.Available);

        public IEnumerable<User> GetAllUser() => _users;

        public IEnumerable<User> FindUsers(Func<User, bool> predicate) => _users.Where(predicate);

        public IEnumerable<Rent> GetAllRent() => _rents;

        public IEnumerable<Rent> FindRents(Func<Rent, bool> predicate) => _rents.Where(predicate);

        public IEnumerable<Rent> GetUserActiveRentals(Guid userId) =>
            _rents.Where(r => r.User.Id == userId && r.ReturnDate == null);

        public IEnumerable<Rent> GetOverdueRentals() =>
            _rents.Where(r => r.IsOverdue());

        public void MarkUnavailable(Guid equipmentId)
        {
            var eq = _equipment.First(e => e.Id == equipmentId);
            eq.Status = Equipment_Status.Unavailable;
        }



        public void GenerateReportEquipment()
        {
            Console.WriteLine("====================     RAPORT EQUIPMENT    ====================");
            Console.WriteLine("");

            foreach (var equipment in _equipment)
            {
                equipment.PrintInfo();
            }

            Console.WriteLine("");
        }

        public void GenerateReportUser()
        {
            Console.WriteLine("====================     RAPORT USERS        ====================");
            Console.WriteLine("");

            foreach (var user in _users)
            {
                user.PrintInfo();
            }
            Console.WriteLine("");
        }

        public void GenerateRaportRent()
        {
            Console.WriteLine("====================     RAPORT RENTS        ====================");
            Console.WriteLine("");

            foreach (var rent in _rents)
            {
                rent.PrintInfo();
            }
            Console.WriteLine("");
        }

        public void GenerateRaportSummary()
        {
            Console.WriteLine("====================     RAPORT SUMMARY      ====================");
            Console.WriteLine("");
            Console.WriteLine($"Total equipment: {_equipment.Count}");
            Console.WriteLine($"Available: {_equipment.Count(e => e.Status == Equipment_Status.Available)}");
            Console.WriteLine($"Rented: {_equipment.Count(e => e.Status == Equipment_Status.Rented)}");
            Console.WriteLine($"Unavailable: {_equipment.Count(e => e.Status == Equipment_Status.Unavailable)}");
            Console.WriteLine($"Active rentals: {_rents.Count(r => r.ReturnDate == null)}");
            Console.WriteLine("");
        }
    }
}