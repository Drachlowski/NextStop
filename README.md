# NextStop

## Projektmitglieder

Andreas Neubauer
Jessica Erli


## Verwendete Datenbank
mssql

## Überlegungen

### Beziehung zwischen Route und Stop
- 0..* zu 0..* Beziehung
- Attribut StopSequence: Hier steht drinnen, welche Reihenfolge die Haltestelle in der Sequenz hat
- Attribut Scheduled: Hier wird im Endeffekt vom Sequenz-Start weg angegeben, nach wievielen Minuten das Verkehrsmittel an der Haltestelle sein sollte.

### Tabelle Trip
Hierbei handelt es sich um die konkrete Fahrt, wo auch die aktuelle Verspätung in Minuten abgebildet ist.