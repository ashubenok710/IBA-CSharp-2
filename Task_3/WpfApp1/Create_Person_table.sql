USE [DBPersons]
GO

UPDATE [dbo].[Person]
   SET [date] =  DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 3650), '2015-01-01')
 
GO


