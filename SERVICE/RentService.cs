using s32429_rent_shop.DOMAIN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace s32429_rent_shop.SERVICE
{
    public class RentService
    {
        private List<Equipment> _equipment = new();
        private List<User> _users = new();
        private List<Rent> _rents = new();

        public void AddEquipment(Equipment equipment)
        {
            _equipment.Add(equipment);
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Added equipment: {equipment.Vendor} {equipment.Model} (SN: {equipment.Serial_Number})");
            Console.ForegroundColor = prev;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Added user: {user.FirstName} {user.LastName} (PESEL: {user.PESEL})");
            Console.ForegroundColor = prev;
        }

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

            var rent = new Rent(equipment, user, days);
            _rents.Add(rent);
            equipment.Status = Equipment_Status.Rented;

            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Rented: {user.FirstName} {user.LastName} (SN: {equipment.Serial_Number})");
            Console.ForegroundColor = prev;
        }

        public void ReturnEquipment(Equipment equipment)
        {
            if (equipment == null)
                throw new ArgumentException("Invalid equipment");

            var rent = _rents.First(r => r.Equipment.Id == equipment.Id && r.ReturnDate == null);
            rent.Return();
            rent.Equipment.Status = Equipment_Status.Available;

            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Returned: (SN: {equipment.Serial_Number})");
            Console.ForegroundColor = prev;

            var penalty = rent.CalculatePenalty();
            if (penalty > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Penalty: {penalty} PLN");
                Console.ForegroundColor = prev;
            }
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

        public void GenerateReportEquipment(Func<Equipment, bool> predicate = null, string jsonFile = null)
        {
            var list = predicate == null ? GetAllEquipment() : FindEquipment(predicate);
            Console.WriteLine("====================     RAPORT EQUIPMENT    ====================");
            Console.WriteLine("");

            foreach (var equipment in list)
            {
                equipment.PrintInfo();
            }

            Console.WriteLine("");
        }

        public void GenerateReportUser(Func<User, bool> predicate = null, string jsonFile = null)
        {
            var list = predicate == null ? GetAllUser() : FindUsers(predicate);


            Console.WriteLine("====================     RAPORT USERS        ====================");
            Console.WriteLine("");

            foreach (var user in list)
            {
                user.PrintInfo();
            }
            Console.WriteLine("");
        }

        public void GenerateRaportRent(Func<Rent, bool> predicate = null, string jsonFile = null)
        {
            var list = predicate == null ? GetAllRent() : FindRents(predicate);


            Console.WriteLine("====================     RAPORT RENTS        ====================");
            Console.WriteLine("");

            foreach (var rent in list)
            {
                rent.PrintInfo();
            }
            Console.WriteLine("");
        }

        private void ExportToJson<T>(IEnumerable<T> list, string jsonFile, string entityName)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(list, options);
                File.WriteAllText(jsonFile, json);
                var prev = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Exported {entityName} report to JSON: {jsonFile}");
                Console.ForegroundColor = prev;
            }
            catch (Exception ex)
            {
                var prev = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Error exporting {entityName} report to JSON: {ex.Message}");
                Console.ForegroundColor = prev;
            }
        }

        public void ExportAllToJson()
        {
            var equipmentFileName = $"equipment_{DateTime.Now:dd-MM-yyyy}.json";

            var usersFileName = $"users_{DateTime.Now:dd-MM-yyyy}.json";

            var rentsFileName = $"rent_{DateTime.Now:dd-MM-yyyy}.json";

            ExportToJson(_equipment, equipmentFileName, "equipment");
            ExportToJson(_users, usersFileName, "users");
            ExportToJson(_rents, rentsFileName, "rents");
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