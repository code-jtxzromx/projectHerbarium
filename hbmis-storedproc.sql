USE HerbariumDatabase
GO

-------------- STORED PROCEDURE CREATION --------------

-- Insert Phylum
IF OBJECT_ID('procInsertPhylum', 'P') IS NOT NULL
	DROP PROCEDURE procInsertPhylum
GO
CREATE PROCEDURE procInsertPhylum
	@domainName		VARCHAR(50),
	@kingdomName	VARCHAR(50),
	@phylumName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION

	BEGIN TRY
		IF NOT EXISTS (SELECT intPhylumID
					   FROM tblPhylum
					   WHERE strDomainName = @domainName AND strKingdomName = @kingdomName AND strPhylumName = @phylumName)
		BEGIN
			INSERT INTO tblPhylum (strDomainName, strKingdomName, strPhylumName)
			VALUES (@domainName, @kingdomName, @phylumName)
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1;
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result;
END
GO

-- Update Phylum
IF OBJECT_ID('procUpdatePhylum', 'P') IS NOT NULL
	DROP PROCEDURE procUpdatePhylum
GO
CREATE PROCEDURE procUpdatePhylum
	@phylumNo		VARCHAR(50),
	@domainName		VARCHAR(50),
	@kingdomName	VARCHAR(50),
	@phylumName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @phylumID INT

		SET @phylumID = (SELECT intPhylumID
						 FROM viewTaxonPhylum
						 WHERE strPhylumNo = @phylumNo)

		UPDATE tblPhylum
		SET strDomainName = @domainName,
			strKingdomName = @kingdomName,
			strPhylumName = @phylumName
		WHERE intPhylumID = @phylumID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1;
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Class
IF OBJECT_ID('procInsertClass', 'P') IS NOT NULL
	DROP PROCEDURE procInsertClass
GO
CREATE PROCEDURE procInsertClass
	@phylumName		VARCHAR(50),
	@className		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @phylumID INT;
		SET @phylumID = (SELECT intPhylumID
						 FROM tblPhylum
						 WHERE strPhylumName = @phylumName);

		IF NOT EXISTS (SELECT	intClassID
					   FROM		tblClass
					   WHERE	intPhylumID = @phylumID AND strClassName = @className)
		BEGIN
			INSERT INTO tblClass (intPhylumID, strClassName)
			VALUES (@phylumID, @className)
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1;
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result;
END
GO

-- Update Class
IF OBJECT_ID('procUpdateClass', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateClass
GO
CREATE PROCEDURE procUpdateClass
	@classNo		VARCHAR(50),
	@phylumName		VARCHAR(50),
	@className		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @classID INT
		DECLARE @phylumID INT

		SET @classID = (SELECT intClassID
						FROM viewTaxonClass
						WHERE strClassNo = @classNo)
		SET @phylumID = (SELECT intPhylumID
						 FROM tblPhylum
						 WHERE strPhylumName = @phylumName)
		
		UPDATE tblClass
		SET intPhylumID = @phylumID,
			strClassName = @className
		WHERE intClassID = @classID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Order
IF OBJECT_ID('procInsertOrder', 'P') IS NOT NULL
	DROP PROCEDURE procInsertOrder
GO
CREATE PROCEDURE procInsertOrder
	@className		VARCHAR(50),
	@orderName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @classID INT;
		SET @classID = (SELECT intClassID
						FROM tblClass
						WHERE strClassName = @className);

		IF NOT EXISTS (SELECT	intOrderID
					   FROM		tblOrder
					   WHERE	intClassID = @classID AND strOrderName = @orderName)
		BEGIN
			INSERT INTO tblOrder (intClassID, strOrderName)
			VALUES (@classID, @orderName)
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1;
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result;
END
GO

-- Update Order
IF OBJECT_ID('procUpdateOrder', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateOrder
GO
CREATE PROCEDURE procUpdateOrder
	@orderNo		VARCHAR(50),
	@className		VARCHAR(50),
	@orderName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @orderID INT
		DECLARE @classID INT

		SET @orderID = (SELECT intOrderID
						FROM viewTaxonOrder
						WHERE strOrderNo = @orderNo)
		SET @classID = (SELECT intClassID
						FROM tblClass
						WHERE strClassName = @className)

		UPDATE tblOrder
		SET intClassID = @classID,
			strOrderName = @orderName
		WHERE intOrderID = @orderID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Family
IF OBJECT_ID('procInsertFamily', 'P') IS NOT NULL
	DROP PROCEDURE procInsertFamily
GO
CREATE PROCEDURE procInsertFamily
	@orderName		VARCHAR(50),
	@familyName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION
	
	BEGIN TRY
		DECLARE @orderID INT;
		SET @orderID = (SELECT intOrderID
						FROM tblOrder
						WHERE strOrderName = @orderName);

		IF NOT EXISTS (SELECT	intFamilyID
					   FROM		tblFamily
					   WHERE	intOrderID = @orderID AND strFamilyName = @familyName)
		BEGIN
			INSERT INTO tblFamily (intOrderID, strFamilyName)
			VALUES (@orderID, @familyName)
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1;
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result;
END
GO

-- Update Family
IF OBJECT_ID('procUpdateFamily', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateFamily
GO
CREATE PROCEDURE procUpdateFamily
	@familyNo		VARCHAR(50),
	@orderName		VARCHAR(50),
	@familyName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @familyID INT
		DECLARE @orderID INT

		SET @familyID = (SELECT intFamilyID
						 FROM viewTaxonFamily
						 WHERE strFamilyNo = @familyNo)
		SET @orderID = (SELECT intOrderID
						FROM tblOrder
						WHERE strOrderName = @orderName)

		UPDATE tblFamily
		SET intOrderID = @orderID,
			strFamilyName = @familyName
		WHERE intFamilyID = @familyID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Genus
IF OBJECT_ID('procInsertGenus', 'P') IS NOT NULL
	DROP PROCEDURE procInsertGenus
GO
CREATE PROCEDURE procInsertGenus
	@familyName		VARCHAR(50) = NULL,
	@genusName		VARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION
	
	BEGIN TRY
		DECLARE @familyID INT;
		SET @familyID = (SELECT intFamilyID
						FROM tblFamily
						WHERE strFamilyName = @familyName);

		IF NOT EXISTS (SELECT	intGenusID
					   FROM		tblGenus
					   WHERE	intFamilyID = @familyID AND strGenusName = @genusName)
		BEGIN
			INSERT INTO tblGenus (intFamilyID, strGenusName)
			VALUES (@familyID, @genusName)
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1;
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result;
END
GO

-- Update Genus
IF OBJECT_ID('procUpdateGenus', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateGenus
GO
CREATE PROCEDURE procUpdateGenus
	@genusNo		VARCHAR(50),
	@familyName		VARCHAR(50),
	@genusName		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @genusID INT
		DECLARE @familyID INT

		SET @genusID = (SELECT intGenusID
						FROM viewTaxonGenus
						WHERE strGenusNo = @genusNo)
		SET @familyID = (SELECT intFamilyID
						 FROM tblFamily
						 WHERE strFamilyName = @familyName)

		UPDATE tblGenus
		SET intFamilyID = @familyID,
			strGenusName = @genusName
		WHERE intGenusID = @genusID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Species
IF OBJECT_ID('procInsertSpecies', 'P') IS NOT NULL
	DROP PROCEDURE procInsertSpecies
GO
CREATE PROCEDURE procInsertSpecies
	@genusName		VARCHAR(50),
	@speciesName	VARCHAR(50),
	@commonName		VARCHAR(100),
	@author			VARCHAR(100) = NULL,
	@isVerified		BIT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION
	
	BEGIN TRY
		DECLARE @genusID INT;
		DECLARE @speciesID INT;
		DECLARE @authorID INT;

		SET @genusID = (SELECT intGenusID
						FROM tblGenus
						WHERE strGenusName = @genusName);

		IF NOT EXISTS (SELECT	intSpeciesID
					   FROM		tblSpecies
					   WHERE	intGenusID = @genusID AND strSpeciesName = @speciesName)
		BEGIN
			INSERT INTO tblSpecies (intGenusID, strSpeciesName, strCommonName)
			VALUES (@genusID, @speciesName, @commonName)

			IF @isVerified = 1
			BEGIN
				SET @speciesID = (SELECT intSpeciesID FROM tblSpecies WHERE strSpeciesName = @speciesName);
				SET @authorID = (SELECT intAuthorID FROM tblAuthor WHERE strAuthorsName = @author)

				INSERT INTO tblSpeciesAuthor (intSpeciesID, intAuthorID)
				VALUES (@speciesID, @authorID)
			END
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1;
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result;
END
GO

-- Update Species
IF OBJECT_ID('procUpdateSpecies', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateSpecies
GO
CREATE PROCEDURE procUpdateSpecies
	@speciesNo		VARCHAR(50),
	@genusName		VARCHAR(50),
	@speciesName	VARCHAR(50),
	@commonName		VARCHAR(100),
	@author			VARCHAR(100) = NULL,
	@isVerified		BIT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @speciesID INT
		DECLARE @genusID INT
		DECLARE @authorID INT

		SET @speciesID = (SELECT intSpeciesID
						  FROM viewTaxonSpecies
						  WHERE strSpeciesNo = @speciesNo)
		SET @genusID = (SELECT intGenusID
						FROM tblGenus
						WHERE strGenusName = @genusName)

		UPDATE tblSpecies
		SET intGenusID = @genusID,
			strSpeciesName = @speciesName,
			strCommonName = @commonName
		WHERE intSpeciesID = @speciesID

		IF @isVerified = 1
		BEGIN
			SET @authorID = (SELECT intAuthorID
							 FROM tblAuthor
							 WHERE strAuthorsName = @author)

			UPDATE tblSpeciesAuthor
			SET intAuthorID = @authorID
			WHERE intSpeciesID = @speciesID
		END

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Species Author
IF OBJECT_ID('procInsertSpeciesAuthor', 'P') IS NOT NULL
	DROP PROCEDURE procInsertSpeciesAuthor
GO
CREATE PROCEDURE procInsertSpeciesAuthor
	@author			VARCHAR(255),
	@speciesSuffix	VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		IF NOT EXISTS (SELECT *
					   FROM tblAuthor
					   WHERE strAuthorsName = @author AND strSpeciesSuffix = @speciesSuffix)
		BEGIN
			INSERT INTO tblAuthor(strAuthorsName, strSpeciesSuffix)
			VALUES (@author, @speciesSuffix)
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Species Author
IF OBJECT_ID('procUpdateSpeciesAuthor', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateSpeciesAuthor
GO
CREATE PROCEDURE procUpdateSpeciesAuthor
	@authorID		INT,
	@author			VARCHAR(255),
	@speciesSuffix	VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		UPDATE tblAuthor
		SET strAuthorsName = @author,
			strSpeciesSuffix = @speciesSuffix
		WHERE intAuthorID = @authorID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Species Alternate Name
IF OBJECT_ID('procInsertSpeciesAlternate', 'P') IS NOT NULL
	DROP PROCEDURE procInsertSpeciesAlternate
GO
CREATE PROCEDURE procInsertSpeciesAlternate
	@taxonName		VARCHAR(255),
	@language		VARCHAR(255),
	@alternateName	VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @speciesID INT
		
		SET @speciesID = (SELECT intSpeciesID FROM viewTaxonSpecies WHERE strScientificName = @taxonName)

		IF NOT EXISTS (SELECT *
					   FROM tblSpeciesAlternateName
					   WHERE intSpeciesID = @speciesID AND strLanguage = @language AND strAlternateName = @alternateName)
		BEGIN
			INSERT INTO tblSpeciesAlternateName (intSpeciesID, strLanguage, strAlternateName)
			VALUES (@speciesID, @language, @alternateName)
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result

END
GO

-- Update Species Alternate Name
IF OBJECT_ID('procUpdateSpeciesAlternate', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateSpeciesAlternate
GO
CREATE PROCEDURE procUpdateSpeciesAlternate
	@altNameID		INT,
	@taxonName		VARCHAR(255),
	@language		VARCHAR(255),
	@alternateName	VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @speciesID INT

		SET @speciesID = (SELECT intSpeciesID FROM viewTaxonSpecies WHERE strScientificName = @taxonName)

		UPDATE tblSpeciesAlternateName
		SET intSpeciesID = @speciesID,
			strLanguage = @language,
			strAlternateName = @alternateName
		WHERE intAltNameID = @altNameID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Plant Type
IF OBJECT_ID('procInsertPlantType', 'P') IS NOT NULL
	DROP PROCEDURE procInsertPlantType
GO
CREATE PROCEDURE procInsertPlantType
	@plantCode		VARCHAR(255),
	@plantType		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM tblPlantType WHERE strPlantTypeCode = @plantCode)
		BEGIN
			INSERT INTO tblPlantType(strPlantTypeCode, strPlantTypeName)
			VALUES (@plantCode, @plantType)
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Plant Type
IF OBJECT_ID('procUpdatePlantType', 'P') IS NOT NULL
	DROP PROCEDURE procUpdatePlantType
GO
CREATE PROCEDURE procUpdatePlantType
	@plantTypeID	INT,
	@plantCode		VARCHAR(255),
	@plantType		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		UPDATE tblPlantType
		SET strPlantTypeCode = @plantCode,
			strPlantTypeName = @plantType
		WHERE intPlantTypeID = @plantTypeID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Family Box
IF OBJECT_ID('procInsertFamilyBox', 'P') IS NOT NULL
	DROP PROCEDURE procInsertFamilyBox
GO
CREATE PROCEDURE procInsertFamilyBox
	@familyName		VARCHAR(50),
	@boxLimit		INT,
	@rack			INT,
	@row			INT,
	@column			INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @familyID INT
		DECLARE @identifier INT
		DECLARE @boxNumber VARCHAR(10)

		SET @familyID = (SELECT intFamilyID FROM tblFamily WHERE strFamilyName = @familyName)
		SET @identifier = (SELECT TOP 1 CAST(SUBSTRING(strBoxNumber, 5, 3) AS INT) FROM tblFamilyBox ORDER BY strBoxNumber DESC)

		IF @identifier IS NULL
			SET @boxNumber = 'BOX-001'
		ELSE 
			SET @boxNumber = 'BOX-' + FORMAT(@identifier + 1, '00#')

		IF NOT EXISTS (SELECT intBoxID
					   FROM tblFamilyBox
					   WHERE intRackNo = @rack AND intRackRow = @row AND intRackColumn = @column)
		BEGIN
			INSERT INTO tblFamilyBox (strBoxNumber, intFamilyID, intBoxLimit, intRackNo, intRackRow, intRackColumn)
			VALUES (@boxNumber, @familyID, @boxLimit, @rack, @row, @column);
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Family Box
IF OBJECT_ID('procUpdateFamilyBox', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateFamilyBox
GO
CREATE PROCEDURE procUpdateFamilyBox
	@boxNumber		VARCHAR(10),
	@familyName		VARCHAR(50),
	@boxLimit		INT,
	@rack			INT,
	@row			INT,
	@column			INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @familyID INT
		DECLARE @currentStoredSheets INT

		SET @familyID = (SELECT intFamilyID FROM tblFamily WHERE strFamilyName = @familyName)
		SET @currentStoredSheets = (SELECT COUNT(intStoredSheetID) FROM viewHerbariumInventory WHERE strBoxNumber = @boxNumber);

		IF NOT EXISTS (SELECT intBoxID
					   FROM tblFamilyBox
					   WHERE intRackNo = @rack AND intRackRow = @row AND intRackColumn = @column)
		BEGIN
			IF @currentStoredSheets < @boxLimit
			BEGIN
				UPDATE tblFamilyBox
				SET intFamilyID = @familyID,
					intBoxLimit = @boxLimit,
					intRackNo = @rack,
					intRackRow = @row,
					intRackColumn = @column
				WHERE strBoxNumber = @boxNumber
			END
			ELSE
			BEGIN
				SET @Result = 3;
			END
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Locality
IF OBJECT_ID('procInsertLocality', 'P') IS NOT NULL
	DROP PROCEDURE procInsertLocality
GO
CREATE PROCEDURE procInsertLocality
	@country			VARCHAR(50),
	@island				VARCHAR(50) = NULL,
	@region				VARCHAR(50) = NULL,
	@province			VARCHAR(50) = NULL,
	@city				VARCHAR(50) = NULL,
	@area				VARCHAR(50) = NULL,
	@specificLocation	VARCHAR(255) = NULL,
	@shortLocation		VARCHAR(255) = NULL,
	@fullLocation		VARCHAR(MAX) = NULL,
	@latitude			VARCHAR(20) = NULL,
	@longtitude			VARCHAR(20) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		IF NOT EXISTS (SELECT intLocalityID
					   FROM tblLocality
					   wHERE strCountry = @country AND strIsland = @island AND strProvince = @province
							AND strCity = @city AND strArea = @area AND strSpecificLocation = @specificLocation
							AND strShortLocation = @shortLocation AND strFullLocality = @fullLocation 
							AND strLatitude = @latitude AND strLongtitude = @longtitude)
		BEGIN
			INSERT INTO tblLocality(strCountry, strIsland, strRegion, strProvince, strCity, strArea, strSpecificLocation,
									strFullLocality, strShortLocation, strLatitude, strLongtitude)
			VALUES (@country, @island, @region, @province, @city, @area, @shortLocation, @fullLocation, @shortLocation, @latitude, @longtitude)
		END
		ELSE
		BEGIN
			SET @Result = 2;
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Locality
IF OBJECT_ID('procUpdateLocality', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateLocality
GO
CREATE PROCEDURE procUpdateLocality
	@localityID			INT,
	@country			VARCHAR(50),
	@island				VARCHAR(50),
	@region				VARCHAR(50),
	@province			VARCHAR(50),
	@city				VARCHAR(50),
	@area				VARCHAR(50),
	@specificLocation	VARCHAR(255),
	@shortLocation		VARCHAR(255),
	@fullLocation		VARCHAR(MAX) = NULL,
	@latitude			VARCHAR(20) = NULL,
	@longtitude			VARCHAR(20) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		
		UPDATE tblLocality
		SET strCountry = @country, 
			strIsland = @island,
			strRegion = @region,
			strProvince = @province,
			strCity = @city,
			strArea = @area,
			strSpecificLocation = @specificLocation,
			strFullLocality = @fullLocation,
			strShortLocation = @shortLocation,
			strLatitude = @latitude,
			strLongtitude = @longtitude
		WHERE intLocalityID = @localityID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Collector
IF OBJECT_ID('procInsertCollector', 'P') IS NOT NULL
	DROP PROCEDURE procInsertCollector
GO
CREATE PROCEDURE procInsertCollector
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50) = NULL,
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3) = NULL,
	@namesuffix		VARCHAR(5) = NULL,
	@address		VARCHAR(MAX),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@affiliation	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		IF NOT EXISTS (SELECT intCollectorID
					   FROM tblCollector
					   WHERE strFirstname = @firstname AND strMiddlename = @middlename AND strLastname = @lastname
							AND strMiddleInitial = @middleinitial AND strNameSuffix = @namesuffix AND strHomeAddress = @address
							AND strContactNumber = @contactno AND strEmailAddress = @email AND strAffiliation = @affiliation)
		BEGIN
			INSERT INTO tblCollector (strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, strHomeAddress, 
										strContactNumber, strEmailAddress, strAffiliation)
			VALUES (@firstname, @middlename, @lastname, @middleinitial, @namesuffix, @address, @contactno, @email, @affiliation);
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Collector
IF OBJECT_ID('procUpdateCollector', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateCollector
GO
CREATE PROCEDURE procUpdateCollector
	@collectorID	INT,
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50) = NULL,
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3) = NULL,
	@namesuffix		VARCHAR(5) = NULL,
	@address		VARCHAR(MAX),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@affiliation	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		UPDATE tblCollector
		SET strFirstname = @firstname,
			strMiddlename = @middlename,
			strLastname = @lastname,
			strMiddleInitial = @middleinitial,
			strNameSuffix = @namesuffix,
			strHomeAddress = @address,
			strContactNumber = @contactno,
			strEmailAddress = @email,
			strAffiliation = @affiliation
		WHERE intCollectorID = @collectorID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Borrower
IF OBJECT_ID('procInsertBorrower', 'P') IS NOT NULL
	DROP PROCEDURE procInsertBorrower
GO
CREATE PROCEDURE procInsertBorrower
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50) = NULL,
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3) = NULL,
	@namesuffix		VARCHAR(5) = NULL,
	@address		VARCHAR(MAX),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@affiliation	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		IF NOT EXISTS (SELECT intBorrowerID
					   FROM tblBorrower
					   WHERE strFirstname = @firstname AND strMiddlename = @middlename AND strLastname = @lastname
							AND strMiddleInitial = @middleinitial AND strNameSuffix = @namesuffix AND strHomeAddress = @address
							AND strContactNumber = @contactno AND strEmailAddress = @email AND strAffiliation = @affiliation)
		BEGIN
			INSERT INTO tblBorrower(strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, strHomeAddress, 
										strContactNumber, strEmailAddress, strAffiliation)
			VALUES (@firstname, @middlename, @lastname, @middleinitial, @namesuffix, @address, @contactno, @email, @affiliation);
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Borrower
IF OBJECT_ID('procUpdateBorrower', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateBorrower
GO
CREATE PROCEDURE procUpdateBorrower
	@borrowerID		INT,
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50) = NULL,
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3) = NULL,
	@namesuffix		VARCHAR(5) = NULL,
	@address		VARCHAR(MAX),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@affiliation	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		UPDATE tblBorrower
		SET strFirstname = @firstname,
			strMiddlename = @middlename,
			strLastname = @lastname,
			strMiddleInitial = @middleinitial,
			strNameSuffix = @namesuffix,
			strHomeAddress = @address,
			strContactNumber = @contactno,
			strEmailAddress = @email,
			strAffiliation = @affiliation
		WHERE intBorrowerID = @borrowerID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Validator
IF OBJECT_ID('procInsertValidator', 'P') IS NOT NULL
	DROP PROCEDURE procInsertValidator
GO
CREATE PROCEDURE procInsertValidator
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50),
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3),
	@namesuffix		VARCHAR(5),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@institution	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0
	
	BEGIN TRANSACTION
	
	BEGIN TRY
		IF NOT EXISTS (SELECT intValidatorID
					   FROM tblValidator
					   WHERE strFirstname = @firstname AND strMiddlename = @middlename AND strLastname = @lastname
							AND strMiddleInitial = @middleinitial AND strNameSuffix = @namesuffix
							AND strContactNumber = @contactno AND strEmailAddress = @email AND strInstitution = @institution)
		BEGIN
			INSERT INTO tblValidator(strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, 
											strContactNumber, strEmailAddress, strInstitution, strValidatorType)
			VALUES (@firstname, @middlename, @lastname, @middleinitial, @namesuffix, @contactno, @email, @institution, 'External');
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Validator
IF OBJECT_ID('procUpdateValidator', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateValidator
GO
CREATE PROCEDURE procUpdateValidator
	@validatorID	INT,
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50),
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3),
	@namesuffix		VARCHAR(5),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@institution	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		UPDATE tblValidator
		SET strFirstname = @firstname,
			strMiddlename = @middlename,
			strLastname = @lastname,
			strMiddleInitial = @middleinitial,
			strNameSuffix = @namesuffix,
			strContactNumber = @contactno,
			strEmailAddress = @email,
			strInstitution = @institution
		WHERE intValidatorID = @validatorID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Herbarium Staff
IF OBJECT_ID('procInsertHerbariumStaff', 'P') IS NOT NULL
	DROP PROCEDURE procInsertHerbariumStaff
GO
CREATE PROCEDURE procInsertHerbariumStaff
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50),
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3),
	@namesuffix		VARCHAR(5),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@role			VARCHAR(50),
	@department		VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		IF NOT EXISTS (SELECT intStaffID
					   FROM tblHerbariumStaff
					   WHERE strFirstname = @firstname AND strMiddlename = @middlename AND strLastname = @lastname
							AND strMiddleInitial = @middleinitial AND strNameSuffix = @namesuffix
							AND strContactNumber = @contactno AND strEmailAddress = @email 
							AND strRole = @role AND strCollegeDepartment = @department)
		BEGIN
			INSERT INTO tblHerbariumStaff(strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, 
										  strContactNumber, strEmailAddress, strRole, strCollegeDepartment)
			VALUES (@firstname, @middlename, @lastname, @middleinitial, @namesuffix, @contactno, @email, @role, @department);

			IF @role IN ('CURATOR', 'ADMINISTRATOR') AND 
				NOT EXISTS(SELECT intValidatorID FROM tblValidator 
						   WHERE strFirstname = @firstname AND strMiddlename = @middlename AND strLastname = @lastname
								AND strMiddleInitial = @middleinitial AND strNameSuffix = @namesuffix
								AND strContactNumber = @contactno AND strEmailAddress = @email)
			BEGIN
				INSERT INTO tblValidator(strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, strContactNumber, strEmailAddress, strInstitution, strValidatorType)
				VALUES (@firstname, @middlename, @lastname, @middleinitial, @namesuffix, @contactno, @email, 'Polytechnic University of the Philippines', 'Internal')
			END
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Herbarium Staff
IF OBJECT_ID('procUpdateHerbariumStaff', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateHerbariumStaff
GO
CREATE PROCEDURE procUpdateHerbariumStaff
	@staffID		INT,
	@firstname		VARCHAR(50),
	@middlename		VARCHAR(50),
	@lastname		VARCHAR(50),
	@middleinitial	VARCHAR(3),
	@namesuffix		VARCHAR(5),
	@contactno		VARCHAR(15),
	@email			VARCHAR(255),
	@role			VARCHAR(50),
	@department		VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		DECLARE @prevRole VARCHAR(50)
		DECLARE @validatorID INT

		SET @prevRole = (SELECT strRole FROM tblHerbariumStaff WHERE intStaffID = @staffID)
		SET @validatorID = (SELECT intValidatorID 
							FROM tblValidator 
							WHERE strFirstname = @firstname AND strMiddlename = @middlename AND strLastname = @lastname
									AND strMiddleInitial = @middleinitial AND strNameSuffix = @namesuffix)

		UPDATE tblHerbariumStaff
		SET strFirstname = @firstname,
			strMiddlename = @middlename,
			strLastname = @lastname,
			strMiddleInitial = @middleinitial,
			strNameSuffix = @namesuffix,
			strContactNumber = @contactno,
			strEmailAddress = @email,
			strRole = @role,
			strCollegeDepartment = @department
		WHERE intStaffID = @staffID
		
		IF (@prevRole <> @role) AND (@role IN ('CURATOR', 'ADMINISTRATOR'))
		BEGIN 
			SET @validatorID = (SELECT intValidatorID FROM tblValidator 
								WHERE strFirstname = @firstname AND
										strMiddlename = @middlename AND
										strLastname = @lastname AND
										strMiddleInitial = @middleinitial AND
										strNameSuffix = @namesuffix);

			IF @validatorID IS NULL
			BEGIN
				INSERT INTO tblValidator (strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, strContactNumber, strEmailAddress, strInstitution, strValidatorType)
				VALUES (@firstname, @middlename, @lastname, @middleinitial, @namesuffix, @contactno, @email, 'Polytechnic University of the Philippines', 'Internal');
			END
		END

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Access Account
IF OBJECT_ID('procInsertAccount', 'P') IS NOT NULL
	DROP PROCEDURE procInsertAccount
GO
CREATE PROCEDURE procInsertAccount
	@staffName		VARCHAR(MAX),
	@username		VARCHAR(50),
	@password		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @staffID INT

		SET @staffID = (SELECT intStaffID FROM viewHerbariumStaff WHERE strFullName = @staffName)

		IF NOT EXISTS (SELECT intAccountID
					   FROM tblAccounts
					   WHERE intStaffID = @staffID AND strUsername = @username AND strPassword = @password)
		BEGIN
			INSERT INTO tblAccounts(intStaffID, strUsername, strPassword)
			VALUES (@staffID, @username, @password)
		END
		ELSE
		BEGIN
			SET @Result = 2
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Update Access Account
IF OBJECT_ID('procUpdateAccount', 'P') IS NOT NULL
	DROP PROCEDURE procUpdateAccount
GO
CREATE PROCEDURE procUpdateAccount
	@accountID		INT,
	@staffName		VARCHAR(MAX),
	@username		VARCHAR(50),
	@password		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @staffID INT;

		SET @staffID = (SELECT intStaffID FROM viewHerbariumStaff WHERE strFullName = @staffName);

		UPDATE tblAccounts
		SET intStaffID = @staffID,
			strUsername = @username,
			strPassword = @password
		WHERE intAccountID = @accountID;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert New Plant Deposit 
IF OBJECT_ID('procInsertPlantDeposit', 'P') IS NOT NULL
	DROP PROCEDURE procInsertPlantDeposit
GO
CREATE PROCEDURE procInsertPlantDeposit
	@herbariumSheet		VARBINARY(MAX) = NULL,
	@localityID			INT,
	@collectorID		INT,
	@staffID			INT,
	@dateCollected		DATE,
	@description		VARCHAR(MAX),
	@plantTypeID		INT,
	@accessionDigits	INT = NULL,
	@dateDeposited		VARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		IF @accessionDigits IS NULL
		BEGIN
			INSERT INTO tblReceivedDeposits (intPlantTypeID, picHerbariumSheet, intCollectorID, intLocalityID, intStaffID, dateCollected, dateDeposited, strDescription, strStatus)
			VALUES (@plantTypeID, @herbariumSheet, @collectorID, @localityID, @staffID, @dateCollected, GETDATE(), @description, 'New Deposit')
		END
		ELSE
		BEGIN
			INSERT INTO tblPlantDeposit(intAccessionNumber, intPlantTypeID, intFormatID, picHerbariumSheet, intCollectorID, intLocalityID, 
										intStaffID, dateCollected, dateDeposited, strDescription, strStatus)
			VALUES(@accessionDigits, @plantTypeID, 1, @herbariumSheet, @collectorID, @localityID, @staffID, @dateCollected, @dateDeposited, @description, 'For Verification')
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Accept/Reject Plant Deposit
IF OBJECT_ID('procConfirmDeposit', 'P') IS NOT NULL
	DROP PROCEDURE procConfirmDeposit
GO
CREATE PROCEDURE procConfirmDeposit
	@depositID			VARCHAR(50),
	@receiveStatus		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @accessionNumber INT
		SET @accessionNumber = (SELECT TOP 1 intAccessionNumber FROM tblPlantDeposit ORDER BY intAccessionNumber DESC)

		UPDATE tblReceivedDeposits
		SET strStatus = @receiveStatus
		WHERE intDepositID = @depositID

		IF @receiveStatus = 'Accepted'
		BEGIN
			IF @accessionNumber IS NULL
				SET @accessionNumber = 1
			ELSE
				SET @accessionNumber += 1

			INSERT INTO tblPlantDeposit(intAccessionNumber, intPlantTypeID, intFormatID, picHerbariumSheet, intCollectorID, intLocalityID, 
										intStaffID, dateCollected, dateDeposited, strDescription, strStatus)
			SELECT @accessionNumber, intPlantTypeID, 1, picHerbariumSheet, intCollectorID, intLocalityID, intStaffID, dateCollected, dateDeposited, strDescription, 'For Verification'
			FROM tblReceivedDeposits
			WHERE intDepositID = @depositID	
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION		
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Resubmit Plant Deposit
IF OBJECT_ID('procPlantResubmission', 'P') IS NOT NULL
	DROP PROCEDURE procPlantResubmission
GO
CREATE PROCEDURE procPlantResubmission
	@depositID			INT,
	@herbariumSheet		VARBINARY(MAX) = NULL,
	@localityID			INT,
	@staffID			INT,
	@description		VARCHAR(MAX),
	@plantTypeID		INT
AS
BEGIN 
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		UPDATE tblReceivedDeposits
		SET picHerbariumSheet = @herbariumSheet,
			intLocalityID = @localityID,
			intStaffID = @staffID,
			dateDeposited = GETDATE(),
			strDescription = @description,
			intPlantTypeID = @plantTypeID,
			strStatus = 'Resubmitted'
		WHERE intDepositID = @depositID
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION		
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Further Verify Plant Deposit
IF OBJECT_ID('procExternalVerifyDeposit', 'P') IS NOT NULL
	DROP PROCEDURE procExternalVerifyDeposit
GO
CREATE PROCEDURE procExternalVerifyDeposit
	@orgDepositID	INT,
	@newDepositID	INT = NULL,
	@speciesID		INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		UPDATE tblPlantDeposit
		SET strStatus = 'Further Verification'
		WHERE intPlantDepositID = @orgDepositID

		IF @speciesID IS NOT NULL
		BEGIN
			INSERT INTO tblVerifyingDeposit (intPlantDepositID, intReferenceDepositID, intSpeciesID)
			VALUES (@orgDepositID, @newDepositID, @speciesID)
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION		
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Verify Plant Deposit
IF OBJECT_ID('procVerifyPlantDeposit', 'P') IS NOT NULL
	DROP PROCEDURE procVerifyPlantDeposit
GO
CREATE PROCEDURE procVerifyPlantDeposit
	@orgDepositID	INT,
	@newDepositID	INT,
	@speciesID		INT,
	@validatorID	INT,
	@dateVerified	DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
		
	BEGIN TRY
		UPDATE tblPlantDeposit
		SET strStatus = 'Verified'
		WHERE intPlantDepositID = @orgDepositID;

		IF @dateVerified IS NULL
			SET @dateVerified = GETDATE()

		INSERT INTO tblHerbariumSheet(intPlantDepositID, intPlantReferenceID, intSpeciesID, intValidatorID, dateVerified)
		VALUES ( @orgDepositID, @newDepositID, @speciesID, @validatorID, @dateVerified)
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION		
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Insert Existing Verified Plant Deposit
IF OBJECT_ID('procInsertVerifiedDeposit', 'P') IS NOT NULL
	DROP PROCEDURE procInsertVerifiedDeposit
GO
CREATE PROCEDURE procInsertVerifiedDeposit
	@accessionDigits	INT,
	@newDepositID		INT = NULL,
	@sameAccession		BIT,
	@herbariumSheet		VARBINARY(MAX) = NULL,
	@localityID			INT,
	@speciesID			INT,
	@collectorID		INT,
	@validatorID		INT,
	@staffID			INT,
	@dateCollected		DATE,
	@dateDeposited		DATE,
	@dateVerified		DATE,
	@description		VARCHAR(MAX),
	@plantTypeID		INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION

	BEGIN TRY
		EXECUTE @Result = procInsertPlantDeposit @herbariumSheet, @localityID, @collectorID, @staffID, @dateCollected, @description, @plantTypeID, @accessionDigits, @dateDeposited
		IF @Result = 0
		BEGIN
			DECLARE @orgDepositID VARCHAR(50);
			SET @orgDepositID = (SELECT intPlantDepositID FROM viewPlantDeposit WHERE intAccessionNumber = @accessionDigits)

			IF @sameAccession = 1
				SET @newDepositID = @orgDepositID

			EXECUTE @Result = procVerifyPlantDeposit @orgDepositID, @newDepositID, @speciesID, @validatorID, @dateVerified
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1		
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result;
END
GO

-- Store Herbarium Sheet
IF OBJECT_ID('procStoreHerbariumSheet', 'P') IS NOT NULL
	DROP PROCEDURE procStoreHerbariumSheet
GO
CREATE PROCEDURE procStoreHerbariumSheet
	@accessionNumber	VARCHAR(50),
	@boxNumber			VARCHAR(MAX),
	@loanAvailable		BIT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		DECLARE @boxID INT;
		DECLARE @sheetID INT;
		DECLARE @depositID INT;

		SET @boxID = (SELECT intBoxID FROM viewFamilyBox WHERE strBoxNumber = @boxNumber);
		SET @sheetID = (SELECT intHerbariumSheetID FROM viewHerbariumSheet WHERE strAccessionNumber = @accessionNumber);
		SET @depositID = (SELECT intPlantDepositID FROM viewPlantDeposit WHERE strAccessionNumber = @accessionNumber)

		UPDATE tblPlantDeposit SET strStatus = 'Stored' WHERE intPlantDepositID = @depositID;

		INSERT INTO tblStoredHerbarium (intHerbariumSheetID, intBoxID, boolLoanAvailable)
		VALUES (@sheetID, @boxID, @loanAvailable);
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1		
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Process Loan
IF OBJECT_ID('procProcessLoan', 'P') IS NOT NULL
	DROP PROCEDURE procProcessLoan
GO
CREATE PROCEDURE procProcessLoan
	@borrowerName	VARCHAR(255),
	@startDate		DATE,
	@endDate		DATE,
	@purpose		VARCHAR(255),
	@status			VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @borrowerID INT;
		SET @borrowerID = (SELECT intBorrowerID FROM viewBorrower WHERE strFullName = @borrowerName);
	
		INSERT INTO tblPlantLoanTransaction(intBorrowerID, dateLoan, dateReturning, dateProcessed, strPurpose, strStatus)
		VALUES (@borrowerID, @startDate, @endDate, GETDATE(), @purpose, @status)
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION

	RETURN @Result
END
GO

-- Process Loaning Plants
IF OBJECT_ID('procLoanPlants', 'P') IS NOT NULL
	DROP PROCEDURE procLoanPlants
GO
CREATE PROCEDURE procLoanPlants
	@loanNumber		VARCHAR(50),
	@taxonName		VARCHAR(100),
	@copies			INT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
		
	BEGIN TRY
		DECLARE @loanID INT
		DECLARE @speciesID INT
		DECLARE @sheetID INT
		DECLARE @status VARCHAR(50);

		SET @loanID = (SELECT intLoanID FROM viewPlantLoans WHERE strLoanNumber = @loanNumber)
		SET @status = (SELECT strStatus FROM viewPlantLoans WHERE strLoanNumber = @loanNumber)
		SET @speciesID = (SELECT intSpeciesID FROM viewTaxonSpecies WHERE strScientificName = @taxonName)

		INSERT INTO tblLoaningSpecies(intLoanID, intSpeciesID, intCopies)
		VALUES (@loanID, @speciesID, @copies)

		IF @status = 'Approved'
		BEGIN
			DECLARE myCursor CURSOR FOR
				SELECT TOP (@copies) intHerbariumSheetID
				FROM tblStoredHerbarium
				WHERE boolLoanAvailable = 1
				ORDER BY intStoredSheetID ASC;

			OPEN myCursor
			FETCH NEXT FROM myCursor INTO @sheetID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO tblLoaningPlants(intLoanID, intHerbariumSheetID)
				VALUES (@loanID, @sheetID)

				UPDATE tblPlantDeposit
				SET strStatus = 'Loaned'
				WHERE intPlantDepositID = (SELECT intPlantDepositID
										   FROM tblHerbariumSheet
										   WHERE intHerbariumSheetID = @sheetID)

				UPDATE tblStoredHerbarium
				SET boolLoanAvailable = 0
				WHERE intHerbariumSheetID = @sheetID

				FETCH NEXT FROM myCursor INTO @sheetID
			END

			CLOSE myCursor
			DEALLOCATE myCursor
		END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @Result = 1
	END CATCH

	IF XACT_STATE() = 1
		COMMIT TRANSACTION
END
GO

-------------- SAMPLE DATA INITIALIZATION --------------

EXECUTE procInsertLocality 'Philippines', 'Luzon', 'National Capital Region', 'Metro Manila', 'Manila', 'Santa Mesa', 'Polytechnic University of the Philippines - Sta. Mesa (Main) Campus', 'PUP Main Campus, Sta. Mesa, Manila', '', '14°35''52.44" N', '121°0''38.88" E'
EXECUTE procInsertLocality 'Philippines', 'Luzon', 'National Capital Region', 'Metro Manila', 'Manila', 'Sampaloc', 'University of Sto. Tomas - Espana', 'UST Espana, Sampaloc, Manila', '', '', ''
EXECUTE procInsertLocality 'Philippines', 'Luzon', 'National Capital Region', 'Metro Manila', 'Quezon City', 'Diliman', 'University of the Philippines - Diliman Campus', 'UP Diliman, Diliman, Quezon City', '', '', ''

EXECUTE procInsertCollector 'Jake', 'M', 'Magpantay', 'M', '', 'Quezon City', '09991357924', 'jakemagpantay@yahoo.com', 'College of Engineering'
EXECUTE procInsertCollector 'Anna', 'B', 'Balingit', 'B', '', 'Manila', '09051234567', 'annabalingit@gmail.com', 'College of Science'
EXECUTE procInsertCollector 'Rhisiel', 'V', 'Valle', 'V', '', 'Manila', '09361234567', 'rvalle1233@hotmail.com', 'College of Arts and Letters'
EXECUTE procInsertCollector 'Maica', 'C', 'Opena', 'O', '', 'Muntilupa City', '09219876543', 'opena_maica@ymail.com', 'College of Accounting and Finance'

EXECUTE procInsertBorrower 'David Adrienne', 'J', 'Fernando', 'J', '', 'Manila', '09997654321', 'vidfernando@yandex.com', 'College of Business Administration'
EXECUTE procInsertBorrower 'Guiller', 'G', 'Pascual', 'G', '', 'Manila', '09001234567', 'sensei_guiller@gmail.com', 'College of Computer and Information Sciences'

EXECUTE procInsertValidator 'Paolo', 'J', 'Labrador', 'J', '', '09881234567', 'paololabrador@gmail.com', 'National Herbarium Center'
EXECUTE procInsertValidator 'Marie Annthonite', 'N', 'Buena', 'N', '', '09111234567', 'tonetbuena@yahoo.com', 'University of Santo Tomas'
EXECUTE procInsertValidator 'Michael John', 'D', 'Ducut', 'D', '', '09991234567', 'mjducut@outlook.com', 'University of the Philippines - Los Banos'

EXECUTE procInsertHerbariumStaff 'Jerome', 'B', 'Casingal', 'B', '', '09161234567', 'jetcasingal@hotmail.com', 'ADMINISTRATOR', 'College of Computer and Infomation Sciences'
EXECUTE procInsertHerbariumStaff 'Althea Nicole', 'M', 'Cruz', 'M', '', '09181237654', 'theapanda@yahoo.com', 'STUDENT ASSISTANT', 'College of Computer and Infomation Sciences'
EXECUTE procInsertHerbariumStaff 'Nino Danielle', 'C', 'Escueta', 'C', '', '09989991700', 'ndescueta@ymail.com', 'CURATOR', 'College of Computer and Information Sciences'
EXECUTE procInsertHerbariumStaff 'Christine Joy', 'V', 'Leynes', 'V', '', '09991234567', 'tineleynes@gmail.com', 'STUDENT ASSISTANT', 'College of Computer and Information Sciences'
EXECUTE procInsertHerbariumStaff 'Armin','S', 'Coronado', 'S', '', '09123456789', 'armin_coronado@pup.edu.ph', 'ADMINISTRATOR', 'College of Science'
EXECUTE procInsertHerbariumStaff 'Ma. Eleanor', 'C', 'Salvador', 'C', '', '09178482910', 'maria_salvador@pup.edu.ph', 'ADMINISTRATOR', 'College of Science'

EXECUTE procInsertAccount 'Jerome Casingal', 'jetcasingal', 'jetcasingal'
EXECUTE procInsertAccount 'Nino Danielle Escueta', 'ndescueta', 'ndescueta'
EXECUTE procInsertAccount 'Althea Nicole Cruz', 'theacruz', 'theacruz'
EXECUTE procInsertAccount 'Christine Joy Leynes', 'tineleynes', 'tineleynes'

EXECUTE procInsertSpeciesAuthor 'Carl Linneaus', 'L.'

EXECUTE procInsertPhylum 'Eukaryota', 'Plantae', 'Magnoliophyta'
EXECUTE procInsertClass 'Magnoliophyta', 'Magnoliopsida'
EXECUTE procInsertOrder 'Magnoliopsida', 'Lamiales'
EXECUTE procInsertFamily 'Lamiales', 'Lamiaceae'
EXECUTE procInsertGenus 'Lamiaceae', 'Vitex'
EXECUTE procInsertSpecies 'Vitex', 'negundo', 'Chinese chastetree', 'Carl Linneaus', 1

EXECUTE procInsertSpeciesAlternate 'Vitex negundo L.', 'Filipino', 'Lagundi'
EXECUTE procInsertFamilyBox 'Lamiaceae', 20, 1, 1, 1

EXECUTE procInsertPlantType 'V', 'Vascular'
EXECUTE procInsertPlantType 'B', 'Bryophytes'
EXECUTE procInsertPlantType 'F', 'Flowering'
EXECUTE procInsertPlantType 'A', 'Algae'

INSERT INTO tblAccessionFormat (strInstitutionCode, strAccessionFormat, strYearFormat) VALUES ('PUPH', '0000#', '0#')

--SELECT * FROM tblLocality
--SELECT * FROM tblPerson
--SELECT * FROM tblCollector
--SELECT * FROM tblBorrower
--SELECT * FROM tblValidator
--SELECT * FROM tblHerbariumStaff

--SELECT * FROM viewTaxonPhylum
--SELECT * FROM viewTaxonClass
--SELECT * FROM viewTaxonOrder
--SELECT * FROM viewTaxonFamily
--SELECT * FROM viewTaxonGenus
--SELECT * FROM viewTaxonSpecies

--SELECT * FROM tblAuthor
--SELECT * FROM tblSpeciesAuthor
--SELECT * FROM tblSpeciesAlternateName

--SELECT FB.strBoxNumber, FB.strFamilyName, FB.intBoxLimit, FB.intRackNo, FB.intRackRow, FB.intRackColumn, COUNT(HI.intStoredSheetID) 
--FROM viewFamilyBox FB LEFT JOIN viewHerbariumInventory HI ON FB.strBoxNumber = HI.strBoxNumber
--GROUP BY FB.strBoxNumber, FB.strFamilyName, FB.intBoxLimit, FB.intRackNo, FB.intRackRow, FB.intRackColumn

--SELECT intLocalityID, strCountry, strIsland, strRegion, strProvince, strCity, strArea, strSpecificLocation, strShortLocation, strFullLocality, strLatitude, strLongtitude
--FROM viewLocality

--SELECT intCollectorID, strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, strHomeAddress, strContactNumber, strEmailAddress, strFullName, strAffiliation 
--FROM viewCollector
--SELECT * FROM viewAccounts

