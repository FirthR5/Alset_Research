# Alset_Research

How to run this system:

1. Go to "_DB" Folder
   
   1. Run 1_Create_DB.sql to create the DB
   
   2. Run 2_MockData.sql to create the Users Data

2. Open the ASP.NET Project Solution "Alset_Research.sln"
   
   1. Open "appsettings.json" and change the **Server** to your server, after that save the settings.
   
   2. Open Package Manager Console and execute this: `Scaffold-DbContext "name=DevDB" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force` 
   
   3. Open Models/AlseJournalsContext and add again this piece of code (below OnModelCreatingPartial):
      
      ```csharp
          public DbSet<ResearchDTO> ResearchDTOs { get; set; }
          public DbSet<JournalDTO> JournalsDTOs { get; set; }
      ```

3. Run, Execute and Debug