-- Create the database
CREATE DATABASE CDNFreelancerDB;
GO

-- Use the database
USE CDNFreelancerDB;
GO

-- Create the Freelancers table
CREATE TABLE Freelancers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(15) NOT NULL,
    IsArchived BIT DEFAULT 0
);
GO

-- Create the Skillsets table
CREATE TABLE Skillsets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FreelancerId INT NOT NULL FOREIGN KEY REFERENCES Freelancers(Id) ON DELETE CASCADE,
    Skill NVARCHAR(100) NOT NULL
);
GO

-- Create the Hobbies table
CREATE TABLE Hobbies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FreelancerId INT NOT NULL FOREIGN KEY REFERENCES Freelancers(Id) ON DELETE CASCADE,
    Hobby NVARCHAR(100) NOT NULL
);
GO
