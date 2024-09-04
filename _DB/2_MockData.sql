-- ─────────────────────────────────────────────────────────────────────────────
--    EXTRAS: Insert Mock Data
-- ─────────────────────────────────────────────────────────────────────────────
USE AlsetJournals
-- Insert Users
-- ─────────────────────────────────────────────────────────────────────────────
INSERT INTO Users (FirstName, LastName, Email)
VALUES
('Alice', 'Smith', 'alice.smith@example.com'),
('Bob', 'Johnson', 'bob.johnson@example.com'),
('Charlie', 'Williams', 'charlie.williams@example.com'),
('Diana', 'Brown', 'diana.brown@example.com');

-- Insert Journals
-- ─────────────────────────────────────────────────────────────────────────────
INSERT INTO Journals (Title, Description, PublicationDate, PDFFile, UserId)
VALUES
('The Future of AI', 'A comprehensive study on the future of artificial intelligence.', '2024-08-01', 'ai_future.pdf', 1),
('Climate Change Impacts', 'An analysis of climate change impacts on coastal regions.', '2024-07-15', 'climate_change.pdf', 2),
('Advancements in Quantum Computing', 'Recent advancements and future directions in quantum computing.', '2024-06-20', 'quantum_computing.pdf', 3),
('Innovations in Renewable Energy', 'Exploring new technologies in renewable energy sources.', '2024-05-10', 'renewable_energy.pdf', 4);

-- Insert Followers
-- ─────────────────────────────────────────────────────────────────────────────
INSERT INTO Followers (ResearcherId, FollowerId)
VALUES
(1, 2), -- Bob follows Alice
(1, 3), -- Charlie follows Alice
(2, 3), -- Charlie follows Bob
(3, 4), -- Diana follows Charlie
(4, 1); -- Alice follows Diana

-- ─────────────────────────────────────────────────────────────────────────────