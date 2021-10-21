# Battleship HappyGame

## Demo
![me](https://github.com/Freeskier/BattleshipGame/blob/master/demo.gif)

## Uwaga!
Przy próbie uruchomienia aplikacji automatycznie zostanie utworzona baza danych (potrzebna do rejestracji i walidacji kont).
Do przetestowania wymagane są dwie różne przeglądarki (np. Firefox i Chrome).

## Działanie aplikacji
Po rejestracji i zalogowaniu się mamy możlwiość wyzwania użytkownika z lobby na pojedynek klikając na jego nazwę.
Na pojawiającym się modalu możemy zaznaczyć checkbox'a Auto play. Jeżeli po obu stronach wybierzemy opcje Auto play, walczyć będzie komputer z komputerem.
Możemy też pograć samemu :)
Okna gry są responsywne, dopasowują się do rozmairu przeglądarki.

## Założenia
Założyłem, że cała logika gry wykonywana będzie po stronie serwera.
Zaimplementowany bot jest dość głupi, ale daje radę :)
Statki generowane są automatycznie.

## Zastosowane technologie
* .NET 
* SignalR
* EntityFramework Core
* React
