# NextStop

## Projektmitglieder

- Andreas Neubauer
- Jessica Erli


## Verwendete Datenbank
mssql

## Überlegungen

### Routen
Siehe INSERTS.sql, Beispiele auf Region Amstetten bezogen, sowie auch die Feiertage und Ferien basieren auf Niederösterreich.
überschneidungen bei:
1. Haltestelle "Amstetten Bahnhof" -> Alle 3 Routen beginnen hier
2. Haltestelle "Amstetten Hauptplatz" -> Alle 3 Routen kommen hier vorbei (spiegelt eine zentrale Lage wieder)
3. Haltestelle "Amstetten Wasserturm" -> Route 1 und 2 nutzen diese Haltestelle
4. Haltestelle "Preinsbach" -> Route 1 und 3 nutzen diese Haltestelle
5. Haltestelle "Amstetten Krankenhaus" -> Route 1 und 3 nutzen diese Haltestelle


| RouteName                                     | Name                          | StopSequence | Scheduled |
|-----------------------------------------------|-------------------------------|--------------|-----------|
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Amstetten Bahnhof             | 1            | 0         |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Amstetten Hauptplatz          | 2            | 3         |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Amstetten Krankenhaus         | 3            | 5         |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Amstetten Wasserturm          | 4            | 2         |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Greinsfurth                   | 5            | 10        |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Preinsbach                    | 6            | 8         |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Euratsfeld                    | 7            | 12        |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Amstetten Schloss Edla        | 8            | 15        |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Amstetten Rathaus             | 9            | 3         |
| Route 1: Bahnhof - Greinsfurth - Euratsfeld   | Amstetten Bahnhof             | 10           | 5         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Amstetten Bahnhof             | 1            | 0         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Amstetten Hauptplatz          | 2            | 3         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Amstetten Rathaus             | 3            | 2         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Amstetten Wasserturm          | 4            | 5         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Amstetten Landesberufsschule  | 5            | 8         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Mauer-Öhling                  | 6            | 15        |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Neuhofen an der Ybbs          | 7            | 10        |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Amstetten Krankenhaus         | 8            | 7         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Preinsbach                    | 9            | 6         |
| Route 2: Bahnhof - Hauptplatz - Neuhofen      | Amstetten Bahnhof             | 10           | 5         |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Amstetten Bahnhof             | 1            | 0         |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Amstetten Hauptplatz          | 2            | 3         |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Amstetten Rathaus             | 3            | 2         |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Amstetten Schloss Edla        | 4            | 4         |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Ulmerfeld-Hausmening          | 5            | 15        |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Preinsbach                    | 6            | 7         |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Kollmitzberg                  | 7            | 12        |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Zeillern                      | 8            | 10        |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Amstetten Krankenhaus         | 9            | 15        |
| Route 3: Bahnhof - Kollmitzberg - Zeillern    | Amstetten Bahnhof             | 10           | 5         |



### Beziehung zwischen Route und Stop
- 0..* zu 0..* Beziehung
- Attribut StopSequence: Hier steht drinnen, welche Reihenfolge die Haltestelle in der Sequenz hat
- Attribut Scheduled: Hier wird im Endeffekt vom Sequenz-Start weg (ist immer auf die Row davor bezogen) angegeben, nach wievielen Minuten das Verkehrsmittel an der Haltestelle sein sollte.

### Tabelle Trip
Hierbei handelt es sich um die konkrete Fahrt, wo auch die aktuelle Verspätung in Minuten abgebildet ist.

### Generierung der ID
Entweder von Client-Seite oder Server-Seite (bei Serverseite muss man die Generierung der DB übergeben, Domain muss id Optional gemacht werden und die DAOs und Unit-Tests angepasst werden)