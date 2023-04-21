# ASP .NET Core Web API "Klinika"



Główne narzędzia użyte w projekcie: MS SQL Server, .NET Core 6 Web API, Entity Framework, MediatR.





## Dokumentacja

Projekt składa się z 5 warstw (Domain, Application, Infrastracture, API, Tests).  

Aplikacja korzysta z EntityFramework do łączenia z bazą Microsoft SQL Server (hostowana na Microsoft Azure) i mapowania obiektowego. Zastosowałem podejście Database first.

Do walidacji użyłem biblioteki FluentValidation, która pozwoliła mi odseparować walidację od klas biznesowych.

Użyłem Automappera do mapowania DTO.

Hasła w bazie danych są zapisywane są pomocą zakodowanego ciągu znaków. Hasła są kodowane za pomocą algorytmu Pbkdf2 HMACSHA_512. 

Autoryzacja została zaimplementowana przy użyciu JWT. 
W systemie istnieją 3 role: Administrator, Klient, Weterynarz. Do JWT dołączana jest informacja o ID użytkownika (w formie zakodowanej), co pozwaliło na bezpieczne uwierzytelniania użytkowników.

W warswie Tests znajdują się testy jednostkowe serwisów i handler-ów.




## Uruchomienie (w Visual Studio)

- uruchom aplikację za pomocą IIS Express

![third step](https://github.com/MichalOstrowskiSolbeg/Task6/blob/main/3.png?raw=true)
## Dane logowania

Dane logowania do konta Administratora: (Nazwa użytkowanika: "Adm1n", Hasło: "Adm1n")
Dane logowania do konta Weterynarza: (Nazwa użytkowanika: "Ewa12", Hasło: "Ewa12")

## Baza danych

Baza danych jest hostowana na platformie Microsoft Azure.



![Database](https://github.com/s20783/Klinika_backend/blob/master/Klinika_database.png?raw=true)
