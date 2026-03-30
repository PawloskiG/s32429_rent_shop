# s32429_rent_shop
# Paweł Gębicki s32429

Krótki opis decyzji projektowych:

- Rozdzielenie warstw: katalog `DOMAIN` zawiera modele i logikę encji (walidacja w właściwościach), a `SERVICE` zawiera serwis zarządzający stanem aplikacji.
- Utworzyłem trzy główne modele Equipment, User i Rent, które reprezentują sprzęt, użytkowników i wypożyczenia. Grupy urządzeń są reprezentowane przez klasy dziedziczące, podobnie jak różne typy użytkowników (Student, Employee). Możliwe, że będą dochodzić inne typy urządzeń i użytkowników.
- Walidacja: pola modeli wykonują podstawową walidację w setterach (np. PESEL, wartości liczbowe).
- Przechowywanie stanu: aplikacja trzyma dane w pamięci (`List<T>`); proste i łatwo dodać później trwałe przechowywanie.
- Logika wypożyczeń: `RentService` odpowiada za reguły biznesowe (limit wypożyczeń, statusy sprzętu, naliczanie kar).
- Eksport: proste serializowanie do JSON-u, nie udało mi się zrobić niestety importu.
- Interfejs użytkownika: dodano osobny plik `InteractiveMenu.cs` z tekstowym menu konsolowym, które pozwala na interaktywne testowanie funkcji.
- Dalej chciałbym rozbudować aplikację o pełną obsługę zdarzeń (usuwanie użytkownika, modyfikacja sprzętu), co przy obecnym podziale na DOMAIN, SERVICE wydaje mi się łatwe do zaimplementowania
- Przygotowałem możliwość filtrowania raportów po przekazanych predykatach (metody takie jak FindUser), ale nie udało mi się tego zaimplementować w menu, ale same metody wykorzystuję w kodzie.
