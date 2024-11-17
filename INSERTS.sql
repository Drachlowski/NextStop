-- Schulferien (Wurde basierend auf Niederösterreich angenommen)
INSERT INTO Holiday (Name, Date, EndDate, IsSchoolHoliday) VALUES
   ('Herbstferien'      , '2024-10-28', '2024-10-31', 1),
   ('Weihnachtsferien'  , '2024-12-23', '2025-01-06', 1),
   ('Semesterferien'    , '2025-02-03', '2025-02-08', 1),
   ('Osterferien'       , '2025-04-12', '2025-04-21', 1),
   ('Pfingsferien'      , '2025-06-07', '2025-06-09', 1),
   ('Sommerferien'      , '2025-06-28', '2025-08-31', 1);


-- Feiertage (Basierend auf Niederösterreich)
INSERT INTO Holiday (Name, Date) VALUES
   ('Nationalfeiertag'  , '2024-10-28'),
   ('Allerheiligen'     , '2024-11-01'),
   (N'Mariä Empfängnis' , '2024-12-08'),
   ('Weihnachten'       , '2024-12-25'),
   ('Stefanitag'        , '2024-12-26'),
   ('Neujahr'           , '2025-01-01'),
   (N'Heilige Drei Könige', '2025-01-06'),
   ('Ostersonntag'      , '2025-04-20'),
   ('Ostermontag'       , '2025-04-21'),
   ('Staatsfeiertag'    , '2025-05-01'),
   (N'Christi Himmelfahrt', '2025-05-29'),
   ('Pfingstsonntag'    , '2025-06-08'),
   ('Pfingstmontag'     , '2025-06-09'),
   ('Fronleichnam'      , '2025-06-19'),
   (N'Mariä Himmelfahrt', '2025-08-15'),
   ('Nationalfeiertag'  , '2025-10-26'),
   ('Allerheiligen'     , '2025-11-01'),
   (N'Mariä Empfängnis' , '2025-12-08'),
   ('Weihnachten'       , '2025-12-25'),
   ('Stefanitag'        , '2025-12-26');


-- Haltestellen
INSERT INTO Stop (Name, ShortName, Latitude, Longitude) VALUES
    ('Amstetten Bahnhof', 'ABH', 48.121667, 14.878056),
    ('Greinsfurth', 'GRF', 48.108457, 14.839032),
    ('Ulmerfeld-Hausmening', 'UHM', 48.074854, 14.816269),
    ('Mauer-Öhling', 'MOE', 48.092731, 14.804470),
    ('Amstetten Krankenhaus', 'AKH', 48.128034, 14.882498),
    ('Amstetten Hauptplatz', 'AHP', 48.123410, 14.871478),
    ('Amstetten Landesberufsschule', 'ALB', 48.116979, 14.883463),
    ('Amstetten Wasserturm', 'AWT', 48.120798, 14.879083),
    ('Amstetten Rathaus', 'ARH', 48.122990, 14.871240),
    ('Amstetten Schloss Edla', 'ASE', 48.126990, 14.864820),
    ('Euratsfeld', 'EUR', 48.055841, 14.871121),
    ('Neuhofen an der Ybbs', 'NHY', 48.050848, 14.904151),
    ('Kollmitzberg', 'KMB', 48.181523, 14.849801),
    ('Preinsbach', 'PRB', 48.140621, 14.862583),
    ('Zeillern', 'ZLN', 48.099463, 14.762264);


-- Routen
INSERT INTO Route (RouteName, ValidityStartDate, ValidityEndDate, DaysOfOperation) VALUES
    ('Route 1: Bahnhof - Greinsfurth - Euratsfeld', '2024-11-17', NULL, 'Mo-Fr'),
    ('Route 2: Bahnhof - Hauptplatz - Neuhofen', '2024-11-17', NULL, 'Mo-So'),
    ('Route 3: Bahnhof - Kollmitzberg - Zeillern', '2024-11-17', NULL, 'Sa-So');


-- Route 1: Bahnhof - Greinsfurth - Euratsfeld
INSERT INTO RouteStop (RouteId, StopId, StopSequence, Scheduled) VALUES
    (1, 1, 1, 0),    -- Amstetten Bahnhof
    (1, 6, 2, 3),    -- Amstetten Hauptplatz
    (1, 5, 3, 5),    -- Amstetten Krankenhaus
    (1, 8, 4, 2),    -- Amstetten Wasserturm
    (1, 2, 5, 10),   -- Greinsfurth
    (1, 14, 6, 8),   -- Preinsbach
    (1, 11, 7, 12),  -- Euratsfeld
    (1, 10, 8, 15),  -- Schloss Edla
    (1, 9, 9, 3),    -- Amstetten Rathaus
    (1, 1, 10, 5);   -- Rückkehr Bahnhof

-- Route 2: Bahnhof - Hauptplatz - Neuhofen
INSERT INTO RouteStop (RouteId, StopId, StopSequence, Scheduled) VALUES
    (2, 1, 1, 0),    -- Amstetten Bahnhof
    (2, 6, 2, 3),    -- Amstetten Hauptplatz
    (2, 9, 3, 2),    -- Amstetten Rathaus
    (2, 8, 4, 5),    -- Amstetten Wasserturm
    (2, 7, 5, 8),    -- Amstetten Landesberufsschule
    (2, 4, 6, 15),   -- Mauer-Öhling
    (2, 12, 7, 10),  -- Neuhofen an der Ybbs
    (2, 5, 8, 7),    -- Amstetten Krankenhaus
    (2, 14, 9, 6),   -- Preinsbach
    (2, 1, 10, 5);   -- Rückkehr Bahnhof

-- Route 3: Bahnhof - Kollmitzberg - Zeillern
INSERT INTO RouteStop (RouteId, StopId, StopSequence, Scheduled) VALUES
    (3, 1, 1, 0),    -- Amstetten Bahnhof
    (3, 6, 2, 3),    -- Amstetten Hauptplatz
    (3, 9, 3, 2),    -- Amstetten Rathaus
    (3, 10, 4, 4),   -- Schloss Edla
    (3, 3, 5, 15),   -- Ulmerfeld-Hausmening
    (3, 14, 6, 7),   -- Preinsbach
    (3, 13, 7, 12),  -- Kollmitzberg
    (3, 15, 8, 10),  -- Zeillern
    (3, 5, 9, 15),   -- Amstetten Krankenhaus
    (3, 1, 10, 5);   -- Rückkehr Bahnhof
