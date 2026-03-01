# Hot Desking API 🏢💻

Zaawansowany backend systemu rezerwacji biurek (Hot Desking) stworzony w środowisku .NET. Aplikacja umożliwia zarządzanie użytkownikami, biurkami oraz procesem rezerwacji (check-in / check-out), wykorzystując architekturę opartą o sprawdzone wzorce projektowe.

## 🛠 Technologie i Architektura

* **Framework:** .NET (C#)
* **Baza danych:** PostgreSQL (uruchamiana w kontenerze Docker)
* **Autoryzacja:** JWT (JSON Web Tokens) z podziałem na role (Admin / User)
* **Wzorce projektowe:**
    * **State (Stan):** Zarządza cyklem życia biurka (np. `Dostępne`, `Zajęte`, `Sprzątanie`). Zapobiega wykonaniu akcji (np. *check-in*) na biurku, które nie jest w odpowiednim stanie.
    * **Strategy (Strategia):** Wykorzystana do elastycznego zarządzania regułami rezerwacji.
    * **Observer (Obserwator):** **Strategy (Strategia):** Wykorzystana do elastycznego zarządzania regułami rezerwacji (np. różne limity czasowe w zależności od roli użytkownika).
  * **Observer (Obserwator):** Służy do reagowania na zdarzenia w systemie. Kiedy użytkownik zwalnia biurko (wykonuje *checkout*), system automatycznie wysyła powiadomienie e-mail do ekipy sprzątającej z informacją, że dane biurko wymaga sprzątania.

---

## 📡 Dokumentacja API (Endpointy)

Autoryzacja odbywa się poprzez przekazanie tokenu JWT w nagłówku żądania: `Authorization: Bearer <twój_token>`.

### 🔐 Autoryzacja
| Metoda | Endpoint | Opis                                          |
| :--- | :--- |:----------------------------------------------|
| **POST** | `/Login` | Logowanie, Zwraca token JWT (Admin lub User). |


### 👤 Użytkownicy (User)
*Operacje modyfikujące wymagają uprawnień administratora.*

| Metoda | Endpoint | Opis |
| :--- | :--- | :--- |
| **GET** | `/User` | Pobiera listę wszystkich użytkowników. |
| **GET** | `/User/{id}` | Pobiera szczegóły użytkownika o podanym ID. |
| **POST** | `/User` | Tworzy nowe konto użytkownika. |
| **PUT** | `/User/{id}` | Aktualizuje dane użytkownika. |
| **DELETE** | `/User/{id}` | Usuwa konto użytkownika. |

### 🪑 Biurka (Desk)
| Metoda | Endpoint | Opis |
| :--- | :--- | :--- |
| **GET** | `/Desk` | Pobiera listę wszystkich biurek i ich status. |
| **GET** | `/Desk/availabledesks` | Pobiera listę dostępnych biurek. |
| **GET** | `/Desk/{id}` | Pobiera szczegóły konkretnego biurka. |
| **POST** | `/Desk` | Dodaje nowe biurko (Admin). |
| **PUT** | `/Desk/{id}` | Aktualizuje dane biurka (Admin). |
| **DELETE** | `/Desk/{id}` | Usuwa biurko z systemu (Admin). |

### ✅ Rezerwacje
| Metoda | Endpoint | Opis |
| :--- | :--- | :--- |
| **POST** | `/Desk/{id}/checkin` | Rozpoczyna rezerwację, zmienia status na "Zajęte". |
| **POST** | `/Desk/{id}/checkout` | Kończy rezerwację, zmienia status na "Dostępne". |
