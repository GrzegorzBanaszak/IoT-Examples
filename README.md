# IoT-Examples

Repozytorium zawierające testy i przykłady obsługi poszczególnych czujników oraz modułów wykonawczych przy użyciu **Raspberry Pi** oraz platformy **.NET 9**.

## Cel projektu

Głównym celem jest stworzenie bazy gotowych do użycia przykładów kodu (PoC - Proof of Concept) dla różnych komponentów elektronicznych. Każdy czujnik jest odizolowany w osobnym folderze, co ułatwia naukę i przenoszenie kodu do większych projektów.

## Struktura repozytorium

Każdy podfolder zawiera osobny projekt dedykowany konkretnemu układowi:

- `28BYJ-48/` – Silnik krokowy ze sterownikiem ULN2003 (test pełnego obrotu i resetu pozycji).
- _(Wkrótce kolejne czujniki...)_

## Technologie

- **Hardware:** Raspberry Pi (model 3/4/5/Zero 2W)
- **Software:** .NET 9 SDK
- **Biblioteki:** [Iot.Device.Bindings](https://www.nuget.org/packages/Iot.Device.Bindings) (oficjalne wsparcie Microsoft dla IoT)
