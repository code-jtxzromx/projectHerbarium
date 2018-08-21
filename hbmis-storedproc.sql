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
	@scientificName	VARCHAR(100),
	@author			VARCHAR(100),
	@alternateName	VARCHAR(MAX),
	@isVerified		BIT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION
	
	BEGIN TRY
		DECLARE @genusID INT;
		SET @genusID = (SELECT intGenusID
						FROM tblGenus
						WHERE strGenusName = @genusName);

		IF NOT EXISTS (SELECT	intSpeciesID
					   FROM		tblSpecies
					   WHERE	intGenusID = @genusID AND strSpeciesName = @speciesName)
		BEGIN
			INSERT INTO tblSpecies (intGenusID, strSpeciesName, strCommonName, strScientificName, 
				strSpeciesAuthor, strSpeciesAlternateName, boolSpeciesIdentified)
			VALUES (@genusID, @speciesName, @commonName, @scientificName, @author, @alternateName, @isVerified)
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
	@scientificName	VARCHAR(100),
	@author			VARCHAR(100),
	@alternateName	VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION

	BEGIN TRY
		DECLARE @speciesID INT
		DECLARE @genusID INT

		SET @speciesID = (SELECT intSpeciesID
						  FROM viewTaxonSpecies
						  WHERE strSpeciesNo = @speciesNo)
		SET @genusID = (SELECT intGenusID
						FROM tblGenus
						WHERE strGenusName = @genusName)

		UPDATE tblSpecies
		SET intGenusID = @genusID,
			strSpeciesName = @speciesName,
			strCommonName = @commonName,
			strScientificName = @scientificName,
			strSpeciesAuthor = @author,
			strSpeciesAlternateName = @alternateName
		WHERE intSpeciesID = @speciesID
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
	@herbariumSheet		VARBINARY(MAX),
	@locality			VARCHAR(255),
	@collector			VARCHAR(255),
	@staff				VARCHAR(255),
	@dateCollected		DATE,
	@description		VARCHAR(MAX),
	@accessionNumber	VARCHAR(50) = NULL,
	@dateDeposited		VARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
	
	BEGIN TRY
		DECLARE @identifier INT;
		DECLARE @localityID INT;
		DECLARE @collectorID INT;
		DECLARE @staffID INT;
		DECLARE @yearCode INT;

		SET @localityID = (SELECT intLocalityID FROM tblLocality WHERE strShortLocation = @locality);
		SET @collectorID = (SELECT intCollectorID FROM viewCollector WHERE strFullName = @collector);
		SET @staffID = (SELECT intStaffID FROM viewHerbariumStaff WHERE strFullName = @staff);

		IF @accessionNumber IS NULL
		BEGIN
			SET @yearCode = YEAR(GETDATE()) % 100;
			SET @identifier = (SELECT TOP 1 CAST(SUBSTRING(strAccessionNumber, 6, 5) AS INT) FROM tblPlantDeposit ORDER BY strAccessionNumber DESC);

			IF @identifier IS NULL
				SET @accessionNumber = 'PUPH-00001-' + FORMAT(@yearCode, '0#');
			ELSE
				SET @accessionNumber = 'PUPH-' + FORMAT(@identifier + 1, '0000#') + '-' + FORMAT(@yearCode, '0#');
		END

		IF @dateDeposited IS NULL
			SET @dateDeposited = (SELECT GETDATE());

		INSERT INTO tblPlantDeposit(strAccessionNumber, picHerbariumSheet, intCollectorID, intLocalityID, intStaffID, 
									dateCollected, dateDeposited, strDescription, strStatus)
		VALUES(@accessionNumber, @herbariumSheet, @collectorID, @localityID, @staffID, @dateCollected, @dateDeposited, @description, 'For Verification')

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

-- Re-Submit Plant Deposit
-- Conflicted Code
IF OBJECT_ID('procResubmitPlantDeposit', 'P') IS NOT NULL
	DROP PROCEDURE procResubmitPlantDeposit
GO

--CREATE PROCEDURE procResubmitPlantDeposit
--	@plantDepositNo	VARCHAR(10),
--	@herbariumSheet	VARBINARY(MAX),
--	@staff			VARCHAR(255),
--	@description	VARCHAR(MAX)
--AS
--BEGIN
--	DECLARE @Result INT = 0

--	BEGIN TRANSACTION
	
--	BEGIN TRY
--		DECLARE @staffID INT;

--		SET @staffID = (SELECT intStaffID FROM viewHerbariumStaff WHERE strFullName = @staff)

--		UPDATE tblPlantDeposit
--		SET picHerbariumSheet = @herbariumSheet,
--			intStaffID = @staffID,
--			dateDeposited = GETDATE(),
--			strDescription = @description,
--			strStatus = 'For Deposit'
--		WHERE strDepositNumber = @plantDepositNo;
--	END TRY
--	BEGIN CATCH
--		ROLLBACK TRANSACTION
--		SET @Result = 1
--	END CATCH

--	IF XACT_STATE() = 1
--		COMMIT TRANSACTION

--	RETURN @Result
--END
--GO

-- Verify Plant Deposit
IF OBJECT_ID('procVerifyPlantDeposit', 'P') IS NOT NULL
	DROP PROCEDURE procVerifyPlantDeposit
GO
CREATE PROCEDURE procVerifyPlantDeposit
	@accessionNo	VARCHAR(50),
	@referenceNo	VARCHAR(50),
	@taxonName		VARCHAR(MAX),
	@validatorName	VARCHAR(MAX),
	@dateVerified	DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0

	BEGIN TRANSACTION
		
	BEGIN TRY
		DECLARE @orgDepositID INT;
		DECLARE @newDepositID INT;
		DECLARE @speciesID INT;
		DECLARE @validatorID INT;

		SET @orgDepositID = (SELECT intPlantDepositID FROM tblPlantDeposit WHERE strAccessionNumber = @accessionNo);
		SET @newDepositID = (SELECT intPlantDepositID FROM tblPlantDeposit WHERE strAccessionNumber = @referenceNo);
		SET @speciesID = (SELECT intSpeciesID FROM viewTaxonSpecies WHERE strScientificName = @taxonName);
		SET @validatorID = (SELECT intValidatorID FROM viewValidator WHERE strFullName = @validatorName);

		UPDATE tblPlantDeposit
		SET strStatus = 'Verified'
		WHERE strAccessionNumber = @accessionNo;

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
	@accessionNumber	VARCHAR(50),
	@referenceNumber	VARCHAR(50),
	@herbariumSheet		VARBINARY(MAX),
	@locality			VARCHAR(255),
	@taxonName			VARCHAR(MAX),
	@collector			VARCHAR(255),
	@validator			VARCHAR(255),
	@staff				VARCHAR(255),
	@dateCollected		DATE,
	@dateDeposited		DATE,
	@dateVerified		DATE,
	@description		VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result INT = 0;

	BEGIN TRANSACTION

	BEGIN TRY
		EXECUTE @Result = procInsertPlantDeposit @herbariumSheet, @locality, @collector, @staff, @dateCollected, @description, @accessionNumber, @dateDeposited
		IF @Result = 0
			EXECUTE @Result = procVerifyPlantDeposit @accessionNumber, @referenceNumber, @taxonName, @validator, @dateVerified
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
	@boxNumber			VARCHAR(MAX)
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
		SET @depositID = (SELECT intPlantDepositID FROM tblPlantDeposit WHERE strAccessionNumber = @accessionNumber)

		UPDATE tblPlantDeposit SET strStatus = 'Stored' WHERE intPlantDepositID = @depositID;

		INSERT INTO tblStoredHerbarium (intHerbariumSheetID, intBoxID, boolLoanAvailable)
		VALUES (@sheetID, @boxID, 0);
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
-- Conflicted Code
IF OBJECT_ID('procProcessLoan', 'P') IS NOT NULL
	DROP PROCEDURE procProcessLoan
GO
--CREATE PROCEDURE procProcessLoan
--	@collectorName	VARCHAR(255),
--	@taxonName		VARCHAR(255),
--	@startDate		DATE,
--	@duration		INT,
--	@durationMode	VARCHAR(10),
--	@copies			INT,
--	@purpose		VARCHAR(255)
--AS
--BEGIN
--	DECLARE @Result INT = 0

--	BEGIN TRANSACTION

--	BEGIN TRY
--		DECLARE @collectorID INT;
--		DECLARE @specieID INT;
--		DECLARE @returningDate DATE;
--		DECLARE @identifier INT;
--		DECLARE @loanNumber VARCHAR(10);

--		SET @collectorID = (SELECT intCollectorID FROM viewCollector WHERE strFullName = @collectorName);
--		SET @specieID = (SELECT intSpeciesID FROM viewTaxonSpecies WHERE strScientificName = @taxonName);
	
--		SET @returningDate = (SELECT CASE
--								  WHEN @durationMode = 'Day/s' THEN CAST(DATEADD(DD, @duration, @startDate) AS DATE)
--								  WHEN @durationMode = 'Month/s' THEN CAST(DATEADD(MM, @duration, @startDate) AS DATE)
--							  END);

--		SET @identifier = (SELECT TOP 1 CAST(SUBSTRING(strLoanNumber, 4, 6) AS INT) FROM tblPlantLoan ORDER BY strLoanNumber DESC);

--		IF @identifier IS NULL
--			SET @loanNumber = 'HL-000001';
--		ELSE
--			SET @loanNumber = 'HL-' + FORMAT(@identifier + 1, '00000#');

--		INSERT INTO tblPlantLoan(strLoanNumber, intCollectorID, intSpeciesID, dateLoan, dateReturning, dateProcessed, intCopies, strPurpose, strStatus)
--		VALUES (@loanNumber, @collectorID, @specieID, @startDate, @returningDate, GETDATE(), @copies, @purpose, 'Pending')
--	END TRY
--	BEGIN CATCH
--		ROLLBACK TRANSACTION
--		SET @Result = 1
--	END CATCH

--	IF XACT_STATE() = 1
--		COMMIT TRANSACTION

--	RETURN @Result
--END
--GO

-------------- SAMPLE DATA INITIALIZATION --------------

--EXECUTE procInsertLocality 'Philippines', 'Luzon', 'National Capital Region', 'Metro Manila', 'Manila', 'Santa Mesa', 'Polytechnic University of the Philippines - Sta. Mesa (Main) Campus', 'PUP Main Campus, Sta. Mesa, Manila', '', '14°35''52.44" N', '121°0''38.88" E'
--EXECUTE procInsertLocality 'Luzon', 'National Capital Region', 'Metro Manila', 'Manila', 'Sampaloc', 'University of Sto. Tomas - Espana', 'UST Espana, Sampaloc, Manila'
--EXECUTE procInsertLocality 'Luzon', 'National Capital Region', 'Metro Manila', 'Quezon City', 'Diliman', 'University of the Philippines - Diliman Campus', 'UP Diliman, Diliman, Quezon City'

--EXECUTE procInsertCollector 'Jake', 'M', 'Magpantay', 'M', '', 'Quezon City', '09991357924', 'jakemagpantay@yahoo.com', 'College of Engineering'
--EXECUTE procInsertCollector 'Anna', 'B', 'Balingit', 'B', '', '09051234567', 'annabalingit@gmail.com', 'College of Science', 'BSCHEM 4-1'
--EXECUTE procInsertCollector 'Rhisiel', 'V', 'Valle', 'V', '', '09361234567', 'rvalle1233@hotmail.com', 'College of Arts and Letters', 'AB-PHILO 4-1' 
--EXECUTE procInsertCollector 'Maica', 'C', 'Opena', 'O', '', '09219876543', 'opena_maica@ymail.com', 'College of Accounting and Finance', 'BSBAMM 4-1'

--EXECUTE procInsertValidator 'Paolo', 'J', 'Labrador', 'J', '', '09881234567', 'paololabrador@gmail.com', 'National Herbarium Center'
--EXECUTE procInsertValidator 'Marie Annthonite', 'N', 'Buena', 'N', '', '09111234567', 'tonetbuena@yahoo.com', 'University of Santo Tomas'
--EXECUTE procInsertValidator 'Michael John', 'D', 'Ducut', 'D', '', '09991234567', 'mjducut@outlook.com', 'University of the Philippines - Los Banos'

--EXECUTE procInsertHerbariumStaff 'Jerome', 'B', 'Casingal', 'B', '', '09161234567', 'jetcasingal@hotmail.com', 'ADMINISTRATOR', 'College of Computer and Infomation Sciences'
--EXECUTE procInsertHerbariumStaff 'Althea Nicole', 'M', 'Cruz', 'M', '', '09181237654', 'theapanda@yahoo.com', 'STUDENT ASSISTANT', 'College of Computer and Infomation Sciences'
--EXECUTE procInsertHerbariumStaff 'Nino Danielle', 'C', 'Escueta', 'C', '', '09989991700', 'ndescueta@ymail.com', 'CURATOR', 'College of Computer and Information Sciences'
--EXECUTE procInsertHerbariumStaff 'Armin','S', 'Coronado', 'S', '', '09123456789', 'armin_coronado@pup.edu.ph', 'ADMINISTRATOR', 'College of Science'
--EXECUTE procInsertHerbariumStaff 'Ma. Eleanor', 'C', 'Salvador', '', '09178482910', 'maria_salvador@pup.edu.ph', 'ADMINISTRATOR', 'College of Science'

--EXECUTE procInsertAccount 'Jerome Casingal ', 'jetcasingal', 'jetcasingal'
--EXECUTE procInsertAccount 'Nino Danielle Escueta ', 'ndescueta', 'ndescueta'
--EXECUTE procInsertAccount 'Althea Nicole Cruz ', 'theacruz', 'theacruz'

--EXECUTE procInsertPhylum 'Eukaryota', 'Plantae', 'Magnoliophyta'

--EXECUTE procInsertClass 'Magnoliophyta', 'Magnoliopsida'

--EXECUTE procInsertOrder 'Magnoliopsida', 'Lamiales'

--EXECUTE procInsertFamily 'Lamiales', 'Lamiaceae'

--EXECUTE procInsertGenus 'Lamiaceae', 'Vitex'

--EXECUTE procInsertSpecies 'Vitex', 'negundo', 'Chinese chastetree', 'Vitex Negundo L.', 'Carl Linneaus', 'Lagundi; Nirgundi;', '1'

--EXECUTE procInsertFamilyBox 'Lamiaceae', '20', 1, 1, 1

--SELECT * FROM tblFamily
--SELECT * FROM tblGenus
--SELECT * FROM tblSpecie
--SELECT * FROM tblLocality
--SELECT * FROM tblPerson
--SELECT * FROM tblCollector
SELECT * FROM tblValidator
SELECT * FROM tblHerbariumStaff

SELECT * FROM viewTaxonPhylum
SELECT * FROM viewTaxonClass
SELECT * FROM viewTaxonOrder
SELECT * FROM viewTaxonFamily
SELECT * FROM viewTaxonGenus
SELECT strSpeciesNo, strGenusName, strSpeciesName, strCommonName, strScientificName, strSpeciesAuthor, strSpeciesAlternateName, boolSpeciesIdentified FROM viewTaxonSpecies

SELECT FB.strBoxNumber, FB.strFamilyName, FB.intBoxLimit, FB.intRackNo, FB.intRackRow, FB.intRackColumn, COUNT(HI.intStoredSheetID) 
FROM viewFamilyBox FB LEFT JOIN viewHerbariumInventory HI ON FB.strBoxNumber = HI.strBoxNumber
GROUP BY FB.strBoxNumber, FB.strFamilyName, FB.intBoxLimit, FB.intRackNo, FB.intRackRow, FB.intRackColumn

SELECT intLocalityID, strCountry, strIsland, strRegion, strProvince, strCity, strArea, strSpecificLocation, strShortLocation, strFullLocality, strLatitude, strLongtitude
FROM viewLocality

SELECT intCollectorID, strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, strHomeAddress, strContactNumber, strEmailAddress, strFullName, strAffiliation 
FROM viewCollector
SELECT * FROM viewAccounts