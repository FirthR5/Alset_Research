-- ─────────────────────────────────────────────────────────────────────────────
--    Create DB
-- ─────────────────────────────────────────────────────────────────────────────
-- USE master;	GO;		DROP DATABASE AlsetJournals
CREATE DATABASE AlsetJournals
GO
USE AlsetJournals
GO
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Journals (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    PublicationDate DATE NOT NULL,
    PDFFile NVARCHAR(255) NOT NULL, 
    UserId INT NOT NULL, 
    CONSTRAINT FK_Journals_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Followers (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    ResearcherId INT NOT NULL,
    FollowerId INT NOT NULL, 
    CONSTRAINT FK_Followers_Researcher FOREIGN KEY (ResearcherId) REFERENCES Users(Id),
    CONSTRAINT FK_Followers_Follower FOREIGN KEY (FollowerId) REFERENCES Users(Id),
    CONSTRAINT UQ_Followers UNIQUE (ResearcherId, FollowerId) 
);
-- ─────────────────────────────────────────────────────────────────────────────