-- ─────────────────────────────────────────────────────────────────────────────
-- Scripts
-- ─────────────────────────────────────────────────────────────────────────────
USE AlsetJournals;
BEGIN TRANSACTION;
-- Log In
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @Email NVARCHAR(255) = 'alice.smith@example.com';

SELECT Id, FirstName, LastName
FROM Users
WHERE Email = @Email;

-- Get My Username
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @UserId INT = 1;

SELECT FirstName, LastName
FROM Users
WHERE Id = @UserId;

-- Upload a New Journal
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @Title NVARCHAR(255) = 'Journal Title';
DECLARE @Description NVARCHAR(MAX) = 'Journal Description';
DECLARE @PublicationDate DATE = GETDATE();
DECLARE @PDFFile NVARCHAR(255) = 'path/to/file.pdf';
DECLARE @UserId INT = 1; -- The ID of the user uploading the journal

INSERT INTO Journals (Title, Description, PublicationDate, PDFFile, UserId)
VALUES (@Title, @Description, @PublicationDate, @PDFFile, @UserId);

-- List of Researchers
-- ─────────────────────────────────────────────────────────────────────────────
SELECT DISTINCT U.Id, U.FirstName, U.LastName
FROM Users U
JOIN Journals J ON U.Id = J.UserId;

-- List of Researchers with Follow Status
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @UserId INT = 1; -- ID of the current user

SELECT U.Id AS ResearcherId, U.FirstName, U.LastName,
       CASE 
           WHEN F.FollowerId IS NOT NULL THEN 1
           ELSE 0
       END AS FollowStatus
FROM Users U
LEFT JOIN Followers F ON U.Id = F.ResearcherId AND F.FollowerId = @UserId
WHERE U.Id <> @UserId; -- Exclude my user

-- List of Journals for the Researchers I Follow
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @UserId INT = 1; -- ID of the current user

SELECT J.Id, J.Title, J.Description, J.PublicationDate, J.PDFFile
FROM Journals J
JOIN Followers F ON J.UserId = F.ResearcherId
WHERE F.FollowerId = @UserId;

-- List of Journals for the Researchers I Follow with Researcher Info
-- ─────────────────────────────────────────────────────────────────────────────
DECLARE @UserId INT = 1; -- ID of the current user

SELECT J.Id, J.Title, J.Description, J.PublicationDate, J.PDFFile,
       U.FirstName AS ResearcherFirstName, U.LastName AS ResearcherLastName
FROM Journals J
JOIN Followers F ON J.UserId = F.ResearcherId
JOIN Users U ON J.UserId = U.Id
WHERE F.FollowerId = @UserId;

-- ─────────────────────────────────────────────────────────────────────────────
ROLLBACK TRANSACTION;
-- ─────────────────────────────────────────────────────────────────────────────