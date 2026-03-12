# Hot Desking API 🏢💻

**Live Demo (Swagger):** [Otwórz Dokumentację API](https://b3nympfxi9.eu-central-1.awsapprunner.com)

Zaawansowany backend systemu rezerwacji biurek (Hot Desking) stworzony w środowisku .NET 8. Aplikacja demonstruje pełny cykl życia oprogramowania — od implementacji złożonych wzorców projektowych, przez konteneryzację, aż po wdrożenie w chmurze AWS.

---

## 🚀 Infrastruktura i Chmura (Cloud-Native)

Projekt został wdrożony w architekturze rozproszonej z wykorzystaniem usług **Amazon Web Services**:

* **AWS App Runner:** Hosting aplikacji w modelu bezserwerowym (Fully Managed), zapewniający automatyczne skalowanie i obsługę HTTPS.
* **Amazon RDS (PostgreSQL):** Produkcyjna, zarządzana baza danych zapewniająca trwałość i bezpieczeństwo danych.
* **Amazon ECR:** Prywatne repozytorium obrazów kontenerowych.
* **Docker:** Aplikacja jest w pełni skonteneryzowana, co zapewnia spójność środowiska lokalnego i produkcyjnego.

---

## 🛠 Technologie i Architektura

* **Framework:** .NET 8 (ASP.NET Core)
* **Baza danych:** PostgreSQL (EF Core)
* **Autoryzacja:** JWT (JSON Web Tokens) z autoryzacją opartą na rolach (**RBAC**).
* **Wzorce projektowe:**
    * **State (Stan):** Zarządza cyklem życia biurka (`Available`, `Occupied`, `Cleaning`). Logika biznesowa wymusza poprawne przejścia między stanami (np. brak możliwości rezerwacji biurka w trakcie sprzątania).
    * **Strategy (Strategia):** Umożliwia elastyczne definiowanie reguł rezerwacji w zależności od typu użytkownika (np. różne limity czasowe).
    * **Observer (Obserwator):** System automatycznie wykrywa zakończenie rezerwacji i wysyła powiadomienia e-mail (SMTP/Gmail) do serwisu sprzątającego.

---

## ✉️ Kontakt
Kamil Janik – LinkedIn | GitHub
