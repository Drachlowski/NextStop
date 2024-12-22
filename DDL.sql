-- DB Erstellen
CREATE DATABASE NextStop;
GO

USE NextStop;
GO

-- Feiertage / Schulferien
CREATE TABLE Holiday (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Date DATE NOT NULL,
    EndDate DATE NULL,
    IsSchoolHoliday BIT DEFAULT 0
);

-- Haltestellen
CREATE TABLE Stop (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ShortName NVARCHAR(10) NOT NULL UNIQUE,
    Latitude FLOAT NOT NULL,
    Longitude FLOAT NOT NULL
);

-- Routen
CREATE TABLE Route (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RouteName NVARCHAR(50) NOT NULL,
    ValidityStartDate DATE NOT NULL,
    ValidityEndDate DATE NULL,
    DaysOfOperation NVARCHAR(50) NULL
);

-- RouteStop: Verknüpfung von Haltestellen mit einer Route in fester Reihenfolge
CREATE TABLE RouteStop (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RouteId INT NOT NULL,
    StopId INT NOT NULL,
    StopSequence INT NOT NULL,             -- Reihenfolge der Haltestellen auf der Route
    Scheduled INT DEFAULT 0,               -- Zeitverzögerung zur nächsten Haltestelle in Minuten

    FOREIGN KEY (RouteId) REFERENCES Route(Id) ON DELETE CASCADE,
    FOREIGN KEY (StopId) REFERENCES Stop(Id) ON DELETE CASCADE
);

-- Trip: Eine konkrete Fahrt auf einer Route zu einer bestimmten Startzeit
CREATE TABLE Trip (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RouteId INT NOT NULL,                  -- Die Route, die befahren wird
    StartTime DATETIME NOT NULL,           -- Geplante Startzeit der Fahrt
    CurrentDelay INT DEFAULT 0,            -- Verspätung in Minuten (wird in Echtzeit angepasst)
    
    FOREIGN KEY (RouteId) REFERENCES Route(Id) ON DELETE CASCADE
);

CREATE TABLE TripCheckIn (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TripId INT NOT NULL,                                -- Verweis auf den Trip
    RouteStopId INT NOT NULL,                           -- Haltestelle, an der eingecheckt wurde
    CheckInTime DATETIME NOT NULL DEFAULT GETDATE(),    -- Zeitpunkt des Eincheckens
    CurrentDelay INT NOT NULL,                          -- Aktuelle Verspätung in Minuten

    FOREIGN KEY (TripId) REFERENCES Trip(Id) ON DELETE NO ACTION,
    FOREIGN KEY (RouteStopId) REFERENCES RouteStop(Id) ON DELETE NO ACTION
);

CREATE INDEX IDX_TripCheckIn_TripId ON TripCheckIn(TripId);
CREATE INDEX IDX_TripCheckIn_StopId ON TripCheckIn(RouteStopId);


CREATE INDEX IDX_Stop_Name ON Stop(Name);
CREATE INDEX IDX_Route_RouteName ON Route(RouteName);
CREATE INDEX IDX_Holiday_Date ON Holiday(Date);