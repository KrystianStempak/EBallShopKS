# EBallShop

Projekt sklepu z piłkami.

Zrobiony na ocenę 5, posiada:

-Produkty (CRUD)
-Koszyk (CRUD)
-Detale klienta

-Testy

-JWT
-Rejestracja klienta
-Logowanie
-Restartowanie hasła
-Edycja konta
-Wdrożenie roli użytkownika
-Procesowanie koszyka

-Wdrożone jako mikrousługi (produkty, koszyk, klient)

-Docker-compose i MediatR (użyty do stworzenia koszyka)

INFO:
Docker-compose opdala się bardzo długo, do kilku minut, dlatego zalecamy odpalić program a potem ręcznie wpisać url do swaggera nawet jak strona api nie odpaliła się jeszcze sama z siebie.

ADMIN:
Aby zalogować się na konto administratora należy wpisać:
username: "admin"
password: "password"

URL:
https://localhost:5002/swagger/index.html - EBallShop - api z produktami
https://localhost:5004/swagger/index.html - UserService - api z logowaniem itp.
https://localhost:5006/swagger/index.html - ShoppingCart - api z koszykiem

Co znajduje się w projekcie?

----EBallShop----

--api/ball--
-get - pobiera wszystkie piłki z bazy
-get{id} - pobiera piłke po id z bazy
-post - tworzy nową piłkę i zapiuje do bazy (ADMIN wymagany)
-delete - usuwa piłkę z bazy (ADMIN wymagany)
-put - edytuje dane piłki w bazie (ADMIN wymagany)



----UserService----

--api/login--
-post - pozwala na zalogowanie się
-get - zwraca string, używać można tego do sprawdzania czy ma się role administratora (ADMIN wymagany)

--api/user--
-get - zwraca informacje o koncie użytkownika aktualnie zalogowanego i po autoryzacji
-post(register) - pozwala stworzyć konto i zapiać je w bazie danych
-post(reset-password) - pozwala zresetować hasło
-put - pozwala edytować dane konta w bazie danych użytkownika zalogowanego i po autoryzacji
-delete - pozwala usunąć użytkownika z bazy danych (ADMIN wymagane)



----ShoppingCart----

-get - zwraca wszystkie koszyki z bazy danych
-get{id} - zwraca koszyk po id z bazy dancyh
-post(add-product) - dodaje produkt do koszyka
-post(remove-product) - usuwa produkt z koszyka


Dodane do projektu jest również zapisywanie zmian (CRUD) bazy danych z piłkami (EBallShop) w postaci zapiywania logów przy użyciu NLog.







