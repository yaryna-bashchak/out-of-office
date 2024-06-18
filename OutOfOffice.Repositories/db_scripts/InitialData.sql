USE OutOfOfficeDB;
GO

-- Insert data into Employees and related tables if they are empty
IF NOT EXISTS (SELECT * FROM Subdivisions)
INSERT INTO Subdivisions (Name) VALUES 
    ('Recruiting'),
    ('Adaptation'),
    ('Employee assessment'),
    ('Individual development plans'),
    ('Employee training');
GO

IF NOT EXISTS (SELECT * FROM Positions)
INSERT INTO Positions (Name) VALUES 
    ('Employee'),
    ('Project Manager'),
    ('HR Manager'),
    ('Administrator');
GO

IF NOT EXISTS (SELECT * FROM EmployeeStatuses)
INSERT INTO EmployeeStatuses (Name) VALUES 
    ('Active'),
    ('Inactive');
GO

IF NOT EXISTS (SELECT * FROM Employees)
INSERT INTO Employees (FullName, SubdivisionID, PositionID, StatusID, PeoplePartnerID, OutOfOfficeBalance)
BEGIN
    VALUES ('Kylo Silva',		1, 4, 1, NULL, 15.0),
            ('Carolina Zhang',	1, 3, 1, NULL, 10.0),
            ('Dion Sheppard',	1, 3, 1, 2, 12.5),
            ('Erin Thomas',		1, 3, 1, 2, 17.0),
            ('Mack Hurst',		2, 3, 1, 3, 13.0),
            ('Desmond Small',	2, 3, 1, 4, 7.5),
            ('Matilda Boyd',	1, 2, 1, 3, 5.0),
            ('Dean Payne',		1, 2, 1, 3, 8.5),
            ('London Ali',		4, 2, 1, 6, 10.0),
            ('Arjun Schroeder',	4, 2, 1, 1, 11.0),
            ('Cameron Harvey',	5, 2, 1, 2, 17.25),
            ('Jane Smith',		1, 1, 1, 6, 11.0),
            ('Serena Wilkerson',1, 1, 1, 6, 18.0),
            ('Carmelo Horn',	2, 1, 2, 2, 13.75),
            ('Avah Swanson',	2, 1, 1, 4, 14.0),
            ('Hugo Vaughan',	4, 1, 1, 4, 15.625),
            ('Nancy Salinas',	4, 1, 1, 3, 9.0),
            ('Edgar Lowe',		4, 1, 2, 5, 4.5),
            ('Amari Cox',		5, 1, 1, 5, 10.0),
            ('Connor Ortiz',	5, 1, 1, 2, 11.0);

    UPDATE Employees SET PeoplePartnerID = 2 WHERE ID IN (1, 2);
END
GO

ALTER TABLE Employees
ALTER COLUMN PeoplePartnerID INT NOT NULL;
GO

-- Insert data into Leave Requests and related tables if they are empty
IF NOT EXISTS (SELECT * FROM AbsenceReasons)
INSERT INTO AbsenceReasons (Name) VALUES 
    ('Sick'),
    ('Vacation');
GO

IF NOT EXISTS (SELECT * FROM RequestTypes)
INSERT INTO RequestTypes (Name) VALUES 
    ('Full days'),
    ('Partial day');
GO

IF NOT EXISTS (SELECT * FROM LeaveRequestStatuses)
INSERT INTO LeaveRequestStatuses (Name) VALUES 
    ('New'),
    ('Submitted'),
    ('Cancelled'),
    ('Approved'),
    ('Rejected');
GO

IF NOT EXISTS (SELECT * FROM LeaveRequests)
INSERT INTO LeaveRequests (EmployeeID, AbsenceReasonID, StartDate, EndDate, RequestTypeID, Hours, StatusID, Comment)
    VALUES
    (11, 2, '2024-01-01', '2024-01-01', 1, NULL, 2, 'New Year Day off'),
    (12, 2, '2024-01-02', '2024-01-03', 1, NULL, 4, NULL),
    (13, 1, '2024-01-04', '2024-01-04', 1, NULL, 2, 'Medical appointment'),
    (14, 1, '2024-01-15', '2024-01-15', 2, 4,	 4, 'Dentist in the morning'),
    (15, 2, '2024-01-10', '2024-01-12', 1, NULL, 3, 'Ski trip'),
    (16, 1, '2024-02-01', '2024-02-01', 1, NULL, 4, 'Personal day'),
    (17, 2, '2024-03-05', '2024-03-05', 1, NULL, 2, 'Child�s school event'),
    (18, 2, '2024-04-07', '2024-04-08', 1, NULL, 5, 'Short vacation'),
    (19, 1, '2024-05-01', '2024-05-01', 2, 3,	 4, 'Doctor visit'),
    (20, 2, '2024-06-14', '2024-06-20', 1, NULL, 1, 'Long vacation'),
    (20, 1, '2024-06-22', '2024-06-22', 1, NULL, 4, 'Doctor visit');
GO

-- Insert data into Approval Requests and related tables if they are empty
IF NOT EXISTS (SELECT * FROM ApprovalRequestStatuses)
INSERT INTO ApprovalRequestStatuses (Name) VALUES 
    ('New'),
    ('Approved'),
    ('Rejected'),
    ('Cancelled');
GO

IF NOT EXISTS (SELECT * FROM ApprovalRequests)
INSERT INTO ApprovalRequests (ApproverID, LeaveRequestID, StatusID, Comment)
    VALUES
	(2, 1, 1, NULL),
    (6, 2, 2, NULL),
    (6, 3, 1, NULL),
    (2, 4, 2, NULL),
    (4, 5,	4, NULL),
    (4, 6, 2, NULL),
    (3, 7, 1, NULL),
    (5, 8, 3, 'Please choose other days'),
    (5, 9, 2, NULL),
    (2, 11, 2, NULL);
GO

-- Insert data into Projects and related tables if they are empty
IF NOT EXISTS (SELECT * FROM ProjectTypes)
INSERT INTO ProjectTypes (Name) VALUES 
    ('Software Development'), 
    ('Research and Development'),
    ('Product Launch'),
    ('Marketing Campaign');
GO

IF NOT EXISTS (SELECT * FROM ProjectStatuses)
INSERT INTO ProjectStatuses (Name) VALUES 
    ('Active'), 
    ('Inactive'),
    ('Completed');
GO

IF NOT EXISTS (SELECT * FROM Projects)
INSERT INTO Projects (ProjectTypeID, StartDate, EndDate, ProjectManagerID, StatusID, Comment)
	VALUES
    (1, '2024-01-01', '2024-06-01', 7, 1, 'Development of the new platform.'),
    (1, '2024-02-01', NULL,			8, 1, 'Ongoing maintenance of the existing systems.'),
    (2, '2024-03-01', '2024-12-01', 9, 2, 'Researching new technologies for integration.'),
    (3, '2024-01-15', '2024-04-15', 10, 3, 'Launching the new product line.'),
    (4, '2024-01-20', '2024-03-20', 11, 1, 'New marketing strategy for Eastern Europe.');
GO

-- Insert data into ProjectEmployees if they are empty
IF NOT EXISTS (SELECT * FROM ProjectEmployees)
INSERT INTO ProjectEmployees (ProjectID, EmployeeID, StartDate, EndDate)
    VALUES
    (1, 12, '2024-01-01', '2024-06-01'),
    (1, 13, '2024-01-01', '2024-06-01'),
    (2, 14, '2024-02-01', NULL),
    (2, 15, '2024-02-01', NULL),
    (3, 16, '2024-03-01', '2024-12-01'),
    (4, 17, '2024-01-15', '2024-04-15'),
    (4, 18, '2024-01-15', '2024-04-15'),
    (5, 19, '2024-01-20', '2024-03-20'),
    (5, 20, '2024-01-20', '2024-03-20');
GO
