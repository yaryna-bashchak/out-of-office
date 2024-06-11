USE OutOfOfficeDB;
GO

-- Employees
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Subdivisions')
CREATE TABLE Subdivisions ( -- 'Recruiting', 'Adaptation', 'Employee assessment', 'Individual development plans', 'Employee training'
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Positions')
CREATE TABLE Positions ( -- 'Employee', 'Project Manager', 'HR Manager', 'Administrator'
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EmployeeStatuses')
CREATE TABLE EmployeeStatuses ( -- 'Active', 'Inactive'
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees')
CREATE TABLE Employees (
    ID INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(255) NOT NULL,
    SubdivisionID INT NOT NULL,
    PositionID INT NOT NULL,
    StatusID INT NOT NULL,
    PeoplePartnerID INT, -- from the “Employee” table with “HR Manager” position
    OutOfOfficeBalance DECIMAL(5,2) NOT NULL,
    Photo VARBINARY(MAX),
	CONSTRAINT FK_SubdivisionID FOREIGN KEY (SubdivisionID) REFERENCES Subdivisions(ID),
	CONSTRAINT FK_PositionID FOREIGN KEY (PositionID) REFERENCES Positions(ID),
	CONSTRAINT FK_EmployeeStatusID FOREIGN KEY (StatusID) REFERENCES EmployeeStatuses(ID)
);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Employees_PeoplePartnerID')
ALTER TABLE Employees
ADD CONSTRAINT FK_Employees_PeoplePartnerID
FOREIGN KEY (PeoplePartnerID) REFERENCES Employees(ID);
GO

-- Leave Requests
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AbsenceReasons')
CREATE TABLE AbsenceReasons ( -- sick, vacation
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RequestTypes')
CREATE TABLE RequestTypes ( -- full day, multiple days, partial day
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LeaveRequestStatuses')
CREATE TABLE LeaveRequestStatuses ( -- 'New', 'Approved', 'Cancelled'
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LeaveRequests')
CREATE TABLE LeaveRequests (
    ID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    AbsenceReasonID INT NOT NULL,
	StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
	RequestTypeID INT NOT NULL,
	Hours INT CHECK (Hours <= 8 AND Hours > 0),
	StatusID INT NOT NULL,
	Comment VARCHAR(MAX),
	CONSTRAINT FK_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Employees(ID),
	CONSTRAINT FK_AbsenceReasonID FOREIGN KEY (AbsenceReasonID) REFERENCES AbsenceReasons(ID),
	CONSTRAINT FK_RequestTypeID FOREIGN KEY (RequestTypeID) REFERENCES RequestTypes(ID),
	CONSTRAINT FK_LeaveRequestStatusID FOREIGN KEY (StatusID) REFERENCES LeaveRequestStatuses(ID),
	CONSTRAINT CHK_LeaveRequests_Date_Check CHECK (EndDate >= StartDate)
);
GO

-- Approval Requests
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ApprovalRequestStatuses')
CREATE TABLE ApprovalRequestStatuses ( -- 'New', 'Approved', 'Rejected'
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ApprovalRequests')
CREATE TABLE ApprovalRequests (
	ID INT PRIMARY KEY IDENTITY(1,1),
	ApproverID INT NOT NULL,
	LeaveRequestID INT NOT NULL,
	StatusID INT NOT NULL,
	Comment VARCHAR(MAX),
	CONSTRAINT FK_ApproverID FOREIGN KEY (ApproverID) REFERENCES Employees(ID),
	CONSTRAINT FK_LeaveRequestID FOREIGN KEY (LeaveRequestID) REFERENCES LeaveRequests(ID),
	CONSTRAINT FK_ApprovalRequestStatusID FOREIGN KEY (StatusID) REFERENCES ApprovalRequestStatuses(ID)
);
GO

-- Projects
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectTypes')
CREATE TABLE ProjectTypes (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectStatuses')
CREATE TABLE ProjectStatuses ( -- 'Active', 'Inactive'
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL
);
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Projects')
CREATE TABLE Projects (
	ID INT PRIMARY KEY IDENTITY(1,1),
	ProjectTypeID INT NOT NULL,
	StartDate DATE NOT NULL,
    EndDate DATE,
	ProjectManagerID INT NOT NULL, -- from the “Employee” table with “Project Manager” position
	StatusID INT NOT NULL,
	Comment VARCHAR(MAX),
	CONSTRAINT FK_ProjectTypeID FOREIGN KEY (ProjectTypeID) REFERENCES ProjectTypes(ID),
	CONSTRAINT FK_ProjectManagerID FOREIGN KEY (ProjectManagerID) REFERENCES Employees(ID),
	CONSTRAINT FK_ProjectStatusID FOREIGN KEY (StatusID) REFERENCES ProjectStatuses(ID),
	CONSTRAINT CHK_Projects_Date_Check CHECK (EndDate IS NULL OR EndDate > StartDate)
);
GO

-- ProjectEmployees
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectEmployees')
CREATE TABLE ProjectEmployees (
	ID INT PRIMARY KEY IDENTITY(1,1),
	ProjectID INT NOT NULL,
	EmployeeID INT NOT NULL,
	StartDate DATE NOT NULL,
    EndDate DATE,
	CONSTRAINT FK_ProjectEmployees_ProjectID FOREIGN KEY (ProjectID) REFERENCES Projects(ID),
	CONSTRAINT FK_ProjectEmployees_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Employees(ID),
	CONSTRAINT CHK_ProjectEmployees_Date_Check CHECK (EndDate IS NULL OR EndDate > StartDate)
);
GO
