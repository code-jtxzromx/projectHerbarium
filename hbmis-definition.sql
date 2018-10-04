-------------- DATABASE CREATION --------------

USE master
GO
IF DB_ID('HerbariumDatabase') IS NOT NULL
	DROP DATABASE HerbariumDataBase
GO

CREATE DATABASE HerbariumDatabase
GO
USE HerbariumDataBase
GO

-- [Ctrl+K][Ctrl+U] To Uncomment the Selected Lines

-------------- TABLE CREATION --------------

-- Herbarium Menu
IF OBJECT_ID('tblSystemMenu', 'U') IS NOT NULL
	DROP TABLE tblSystemMenu
GO
CREATE TABLE tblSystemMenu
(
	intMenuID INT IDENTITY(1, 1) NOT NULL,
	strLevel CHAR NOT NULL,
	strMainMenu VARCHAR(255) NOT NULL,
	strSubMenu VARCHAR(255),
	strPageLocation VARCHAR(255) NOT NULL,
	strAccessLevel VARCHAR(255) NOT NULL
	CONSTRAINT pk_tblSystemMenu PRIMARY KEY(intMenuID)
)
GO

-- Taxonomic Hierarchy : Phylum
IF OBJECT_ID('tblPhylum', 'U') IS NOT NULL
	DROP TABLE tblPhylum
GO
CREATE TABLE tblPhylum
(
	intPhylumID INT IDENTITY(1, 1) NOT NULL,
	strDomainName VARCHAR(50) NOT NULL,
	strKingdomName VARCHAR(50) NOT NULL,
	strPhylumName VARCHAR(50) NOT NULL
	CONSTRAINT pk_tblPhylum PRIMARY KEY(intPhylumID)
)
GO

ALTER TABLE tblPhylum ADD CONSTRAINT df_tblPhylum_strDomainName DEFAULT 'Eukaryota' FOR strDomainName
GO
ALTER TABLE tblPhylum ADD CONSTRAINT df_tblPhylum_strKingdomName DEFAULT 'Plantae' FOR strKingdomName
GO

-- Taxonomic Hierarchy : Class
IF OBJECT_ID('tblClass', 'U') IS NOT NULL
	DROP TABLE tblClass
GO
CREATE TABLE tblClass
(
	intClassID INT IDENTITY(1, 1) NOT NULL,
	intPhylumID INT NOT NULL,
	strClassName VARCHAR(50) NOT NULL
	CONSTRAINT pk_tblClass PRIMARY KEY(intClassID),
	CONSTRAINT fk_tblClass_tblPhylum FOREIGN KEY(intPhylumID)
		REFERENCES tblPhylum(intPhylumID)
)
GO

-- Taxonomic Hierarchy : Order
IF OBJECT_ID('tblOrder', 'U') IS NOT NULL
	DROP TABLE tblOrder
GO
CREATE TABLE tblOrder
(
	intOrderID INT IDENTITY(1, 1) NOT NULL,
	intClassID INT NOT NULL,
	strOrderName VARCHAR(50) NOT NULL
	CONSTRAINT pk_tblOrder PRIMARY KEY(intOrderID),
	CONSTRAINT fk_tblOrder_tblClass FOREIGN KEY(intClassID)
		REFERENCES tblClass(intClassID)
)
GO

-- Taxonomic Hierarchy : Family
IF OBJECT_ID('tblFamily', 'U') IS NOT NULL
	DROP TABLE tblFamily
GO
CREATE TABLE tblFamily
(
	intFamilyID INT IDENTITY(1, 1) NOT NULL,
	intOrderID INt NOT NULL,
	strFamilyName VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tblFamily PRIMARY KEY(intFamilyID),
	CONSTRAINT fk_tblFamily_tblOrder FOREIGN KEY(intOrderID)
		REFERENCES tblOrder(intOrderID)
)
GO

-- Taxonomic Hierarchy : Genus
IF OBJECT_ID('tblGenus', 'U') IS NOT NULL
	DROP TABLE tblGenus
GO
CREATE TABLE tblGenus
(
	intGenusID INT IDENTITY(1, 1) NOT NULL,
	intFamilyID INT NOT NULL,
	strGenusName VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tblGenus PRIMARY KEY(intGenusID),
	CONSTRAINT fk_tblGenus_tblFamily FOREIGN KEY (intFamilyID)
		REFERENCES tblFamily(intFamilyID)
)
GO

-- Taxonomic Hierarchy : Species
IF OBJECT_ID('tblSpecies', 'U') IS NOT NULL
	DROP TABLE tblSpecies
GO
CREATE TABLE tblSpecies
(
	intSpeciesID INT IDENTITY(1, 1) NOT NULL,
	intGenusID INT NOT NULL,
	strSpeciesName VARCHAR(50) NOT NULL,
	strCommonName VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tblSpecies PRIMARY KEY(intSpeciesID),
	CONSTRAINT fk_tblSpecies_tblGenus FOREIGN KEY(intGenusID)
		REFERENCES tblGenus(intGenusID)
)
GO

-- Author
IF OBJECT_ID('tblAuthor', 'U') IS NOT NULL
	DROP TABLE tblAuthor
GO
CREATE TABLE tblAuthor
(
	intAuthorID INT IDENTITY(1, 1) NOT NULL,
	strAuthorsName VARCHAR(255) NOT NULL,
	strSpeciesSuffix VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tblAuthor PRIMARY KEY(intAuthorID)
)

-- Species Author
IF OBJECT_ID('tblSpeciesAuthor', 'U') IS NOT NULL
	DROP TABLE tblSpeciesAuthor
GO
CREATE TABLE tblSpeciesAuthor
(
	intSpeciesID INT NOT NULL,
	intAuthorID INT NOT NULL,
	CONSTRAINT pk_tblSpeciesAuthor PRIMARY KEY(intSpeciesID),
	CONSTRAINT fk_tblSpeciesAuthor_tblSpecies FOREIGN KEY(intSpeciesID)
		REFERENCES tblSpecies(intSpeciesID),
	CONSTRAINT fk_tblSpeciesAuthor_tblAuthor FOREIGN KEY(intAuthorID)
		REFERENCES tblAuthor(intAuthorID)
)
GO

-- Species Alternate Name
IF OBJECT_ID('tblSpeciesAlternateName', 'U') IS NOT NULL
	DROP TABLE tblSpeciesAlternateName 
GO
CREATE TABLE tblSpeciesAlternateName
(
	intAltNameID INT IDENTITY(1, 1) NOT NULL,
	intSpeciesID INT NOT NULL,
	strLanguage VARCHAR(255) NOT NULL,
	strAlternateName VARCHAR(255) NOT NULL,
	CONSTRAINT pk_tblSpeciesAlternateName PRIMARY KEY(intAltNameID),
	CONSTRAINT fk_tblSpeciesAlternateName_tblSpecies FOREIGN KEY(intSpeciesID)
		REFERENCES tblSpecies(intSpeciesID)
)
GO

-- Plant Types
IF OBJECT_ID('tblPlantType', 'U') IS NOT NULL
	DROP TABLE tblPlantType
GO
CREATE TABLE tblPlantType
(
	intPlantTypeID INT IDENTITY(1, 1) NOT NULL,
	strPlantTypeCode VARCHAR(5) NOT NULL,
	strPlantTypeName VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tblPlantType PRIMARY KEY(intPlantTypeID)
)
GO

-- Family Box
IF OBJECT_ID('tblFamilyBox', 'U') IS NOT NULL
	DROP TABLE tblFamilyBox
GO
CREATE TABLE tblFamilyBox
(
	intBoxID INT IDENTITY(1, 1) NOT NULL,
	intFamilyID INT NOT NULL,
	intBoxLimit INT NOT NULL,
	intRackNo INT NOT NULL,
	intRackRow INT NOT NULL,
	intRackColumn INT NOT NULL,
	CONSTRAINT pk_tblFamilyBox PRIMARY KEY (intBoxID),
	CONSTRAINT fk_tblFamilyBox_tblFamily FOREIGN KEY (intFamilyID)
		REFERENCES tblFamily(intFamilyID)
)
GO

-- Country
IF OBJECT_ID('tblCountry', 'U') IS NOT NULL
	DROP TABLE tblCountry
GO
CREATE TABLE tblCountry
(
	intCountryID INT IDENTITY(1, 1) NOT NULL,
	strCountry VARCHAR(255) NOT NULL
	CONSTRAINT pk_tblCountry PRIMARY KEY(intCountryID)
)
GO

-- Region
IF OBJECT_ID('tblRegion', 'U') IS NOT NULL
	DROP TABLE tblRegion
GO
CREATE TABLE tblRegion
(
	intRegionID INT IDENTITY(1, 1) NOT NULL,
	strIsland VARCHAR(50) NOT NULL,
	strRegionCode VARCHAR(10) NOT NULL,
	strRegion VARCHAR(255) NOT NULL
	CONSTRAINT pk_tblRegion PRIMARY KEY(intRegionID)
)
GO

-- Provinces
IF OBJECT_ID('tblProvince', 'U') IS NOT NULL
	DROP TABLE tblProvince
GO
CREATE TABLE tblProvince
(
	intProvinceID INT IDENTITY(1, 1) NOT NULL,
	intRegionID INT NOT NULL,
	strProvince VARCHAR(255) NOT NULL
	CONSTRAINT pk_tblProvince PRIMARY KEY(intProvinceID),
	CONSTRAINT fk_tblProvince_tblRegion FOREIGN KEY(intRegionID)
		REFERENCES tblRegion(intRegionID)
)
GO

-- City
IF OBJECT_ID('tblCity', 'U') IS NOT NULL
	DROP TABLE tblCity
GO
CREATE TABLE tblCity
(
	intCityID INT IDENTITY(1, 1) NOT NULL,
	intProvinceID INT NOT NULL,
	strCity VARCHAR(255) NOT NULL
	CONSTRAINT pk_tblCity PRIMARY KEY(intCityID),
	CONSTRAINT fk_tblCity_tblProvince FOREIGN KEY(intProvinceID)
		REFERENCES tblProvince(intProvinceID)
)
GO

-- Locality
IF OBJECT_ID('tblLocality', 'U') IS NOT NULL
	DROP TABLE tblLocality
GO
CREATE TABLE tblLocality
(
	intLocalityID INT IDENTITY(1, 1) NOT NULL,
	intCountryID INT NOT NULL,
	strSpecificLocation VARCHAR(255),
	strShortLocation VARCHAR(255),
	strFullLocality VARCHAR(50),
	strLatitude VARCHAR(20),
	strLongtitude VARCHAR(20),
	CONSTRAINT pk_tblLocality PRIMARY KEY(intLocalityID),
	CONSTRAINT fk_tblLocality_tblCountry FOREIGN KEY(intCountryID)
		REFERENCES tblCountry(intCountryID)
)
GO

-- Philippine Locality
IF OBJECT_ID('tblPHLocality', 'U') IS NOT NULL
	DROP TABLE tblPHLocality
GO
CREATE TABLE tblPHLocality
(
	intLocalityID INT NOT NULL,
	intCityID INT NOT NULL,
	CONSTRAINT pk_tblPHLocality PRIMARY KEY(intLocalityID),
	CONSTRAINT fk_tblPHLocality_tblLocality FOREIGN KEY(intLocalityID)
		REFERENCES tblLocality(intLocalityID),
	CONSTRAINT fk_tblPHLocality_tblCity FOREIGN KEY(intCityID)
		REFERENCES tblCity(intCityID),
)
GO

-- Collector
IF OBJECT_ID('tblCollector', 'U') IS NOT NULL
	DROP TABLE tblCollector
GO
CREATE TABLE tblCollector
(
	intCollectorID INT IDENTITY(1001, 1) NOT NULL,
	strFirstname VARCHAR(50) NOT NULL,
	strMiddlename VARCHAR(50),
	strLastname VARCHAR(50) NOT NULL,
	strMiddleInitial VARCHAR(3),
	strNameSuffix VARCHAR(5),
	strHomeAddress VARCHAR(MAX) NOT NULL,
	strContactNumber VARCHAR(15) NOT NULL,
	strEmailAddress VARCHAR(255) NOT NULL,
	strAffiliation VARCHAR(100) NOT NULL,
	CONSTRAINT pk_tblCollector PRIMARY KEY(intCollectorID)
)
GO

-- Borrower
IF OBJECT_ID('tblBorrower', 'U') IS NOT NULL
	DROP TABLE tblBorrower
GO
CREATE TABLE tblBorrower 
(
	intBorrowerID INT IDENTITY(1001, 1) NOT NULL,
	strFirstname VARCHAR(50) NOT NULL,
	strMiddlename VARCHAR(50),
	strLastname VARCHAR(50) NOT NULL,
	strMiddleInitial VARCHAR(3),
	strNameSuffix VARCHAR(5),
	strHomeAddress VARCHAR(MAX) NOT NULL,
	strContactNumber VARCHAR(15) NOT NULL,
	strEmailAddress VARCHAR(255) NOT NULL,
	strAffiliation VARCHAR(100) NOT NULL,
	boolIsCollector BIT NOT NULL
	CONSTRAINT pk_tblBorrower PRIMARY KEY(intBorrowerID)
)
GO

-- Validator
IF OBJECT_ID('tblValidator', 'U') IS NOT NULL       
	DROP TABLE tblValidator
GO
CREATE TABLE tblValidator
(
	intValidatorID INT IDENTITY(1001, 1) NOT NULL,
	strFirstname VARCHAR(50) NOT NULL,
	strMiddlename VARCHAR(50),
	strLastname VARCHAR(50) NOT NULL,
	strMiddleInitial VARCHAR(3),
	strNameSuffix VARCHAR(5),
	strContactNumber VARCHAR(15) NOT NULL,
	strEmailAddress VARCHAR(255) NOT NULL,
	strInstitution VARCHAR(255) NOT NULL,
	strValidatorType VARCHAR(10) NOT NULL
	CONSTRAINT pk_tblValidator PRIMARY KEY(intValidatorID)
)
GO

-- Herbarium Staff
IF OBJECT_ID('tblHerbariumStaff', 'U') IS NOT NULL
	DROP TABLE tblHerbariumStaff
GO
CREATE TABLE tblHerbariumStaff
(
	intStaffID INT IDENTITY(1001, 1) NOT NULL,
	strFirstname VARCHAR(50) NOT NULL,
	strMiddlename VARCHAR(50),
	strLastname VARCHAR(50) NOT NULL,
	strMiddleInitial VARCHAR(3),
	strNameSuffix VARCHAR(5),
	strContactNumber VARCHAR(15) NOT NULL,
	strEmailAddress VARCHAR(255) NOT NULL,
	strRole VARCHAR(50) NOT NULL,
	strCollegeDepartment VARCHAR(100) NOT NULL,
	boolActive BIT NOT NULL
	CONSTRAINT pk_tblHerbariumStaff PRIMARY KEY(intStaffID)
)
GO

ALTER TABLE tblHerbariumStaff ADD CONSTRAINT df_tblHerbariumStaff_boolActive DEFAULT 1 FOR boolActive
GO

-- Access Accounts
IF OBJECT_ID('tblAccounts', 'U') IS NOT NULL
	DROP TABLE tblAccounts
GO
CREATE TABLE tblAccounts
(
	intStaffID INT NOT NULL,
	strUsername VARCHAR(50) NOT NULL,
	strPassword VARCHAR(50) NOT NULL,
	boolActive BIT NOT NULL
	CONSTRAINT pk_tblAccounts PRIMARY KEY(intStaffID),
	CONSTRAINT fk_tblAccounts_tblHerbariumStaff FOREIGN KEY(intStaffID)
		REFERENCES tblHerbariumStaff(intStaffID)
)

ALTER TABLE tblAccounts ADD CONSTRAINT df_tblAccounts_boolActive DEFAULT 1 FOR boolActive
GO

-- Accession Number Format
IF OBJECT_ID('tblAccessionFormat', 'U') IS NOT NULL
	DROP TABLE tblAccessionFormat
GO
CREATE TABLE tblAccessionFormat
(
	intFormatID INT IDENTITY(1, 1) NOT NULL,
	strInstitutionCode VARCHAR(10) NOT NULL,
	strAccessionFormat VARCHAR(10) NOT NULL,
	strYearFormat VARCHAR(10) NOT NULL,
	CONSTRAINT pk_tblAccessionFormat PRIMARY KEY(intFormatID)
)

-- New Plant Deposit
IF OBJECT_ID('tblReceivedDeposits' ,'U') IS NOT NULL
	DROP TABLE tblReceivedDeposits
GO
CREATE TABLE tblReceivedDeposits
(
	intDepositID INT IDENTITY(1, 1) NOT NULL,
	intPlantTypeID INT NOT NULL,
	picHerbariumSheet VARBINARY(MAX),
	intCollectorID INT NOT NULL,
	intLocalityID INT NOT NULL,
	intStaffID INT NOT NULL,
	dateCollected DATE NOT NULL,
	dateDeposited DATE NOT NULL,
	strDescription VARCHAR(MAX) NOT NULL,
	strStatus VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tblReceivedDeposit PRIMARY KEY(intDepositID),
	CONSTRAINT fk_tblReceivedDeposit_tblPlantType FOREIGN KEY(intPlantTypeID)
		REFERENCES tblPlantType(intPlantTypeID),
	CONSTRAINT fk_tblReceivedDeposit_tblCollector FOREIGN KEY(intCollectorID)
		REFERENCES tblCollector(intCollectorID),
	CONSTRAINT fk_tblReceivedDeposit_tblLocality FOREIGN KEY(intLocalityID)
		REFERENCES tblLocality(intLocalityID),
	CONSTRAINT fk_tblReceivedDeposit_tblHerbariumStaff FOREIGN KEY(intStaffID)
		REFERENCES tblHerbariumStaff(intStaffID)
)
GO

-- Plant Deposit
IF OBJECT_ID('tblPlantDeposit', 'U') IS NOT NULL
	DROP TABLE tblPlantDeposit
GO
CREATE TABLE tblPlantDeposit
(
	intPlantDepositID INT IDENTITY(1, 1) NOT NULL,
	intAccessionNumber INT NOT NULL,
	intPlantTypeID INT NOT NULL,
	intFormatID INT NOT NULL,
	intCollectorID INT NOT NULL,
	intLocalityID INT NOT NULL,
	intStaffID INT NOT NULL,
	dateCollected DATE NOT NULL,
	dateDeposited DATE NOT NULL,
	strDescription VARCHAR(MAX) NOT NULL,
	strStatus VARCHAR(50) NOT NULL,
	CONSTRAINT pk_tblPlantDeposit PRIMARY KEY(intPlantDepositID),
	CONSTRAINT fk_tblPlantDeposit_tblPlantType FOREIGN KEY(intPlantTypeID)
		REFERENCES tblPlantType(intPlantTypeID),
	CONSTRAINT fk_tblPlantDeposit_tblAccessionFormat FOREIGN KEY(intFormatID)
		REFERENCES tblAccessionFormat(intFormatID),
	CONSTRAINT fk_tblPlantDeposit_tblCollector FOREIGN KEY(intCollectorID)
		REFERENCES tblCollector(intCollectorID),
	CONSTRAINT fk_tblPlantDeposit_tblLocality FOREIGN KEY(intLocalityID)
		REFERENCES tblLocality(intLocalityID),
	CONSTRAINT fk_tblPlantDeposit_tblHerbariumStaff FOREIGN KEY(intStaffID)
		REFERENCES tblHerbariumStaff(intStaffID)
)
GO

-- Herbarium Sheet Picture
IF OBJECT_ID('tblHerbariumSheetPicture', 'U') IS NOT NULL
	DROP TABLE tblHerbariumSheetPicture
GO
CREATE TABLE tblHerbariumSheetPicture
(
	intPictureID INT IDENTITY(1, 1) NOT NULL,
	intPlantDepositID INT NOT NULL,
	picHerbariumSheet VARBINARY(MAX),
	strTagDescription VARCHAR(255),
	CONSTRAINT pk_tblHerbariumSheetPicture PRIMARY KEY(intPictureID),
	CONSTRAINT fk_tblHerbariumSheetPicture_tblPlantDeposit FOREIGN KEY(intPlantDepositID)
		REFERENCES tblPlantDeposit(intPlantDepositID)
)
GO

-- Further Verification Plant Deposit
IF OBJECT_ID('tblVerifyingDeposit', 'U') IS NOT NULL
	DROP TABLE tblVerifyingDeposit
GO
CREATE TABLE tblVerifyingDeposit
(
	intPlantDepositID INT NOT NULL,
	intSpeciesID INT NOT NULL,
	intReferenceDepositID INT NOT NULL,
	CONSTRAINT pk_tblVerifyingDeposit PRIMARY KEY(intPlantDepositID),
	CONSTRAINT fk_tblVerifyingDeposit_tblPlantDeposit_Org FOREIGN KEY(intPlantDepositID)
		REFERENCES tblPlantDeposit(intPlantDepositID),
	CONSTRAINT fk_tblVerifyingDeposit_tblSpecies FOREIGN KEY(intSpeciesID)
		REFERENCES tblSpecies(intSpeciesID),	
	CONSTRAINT fk_tblVerifyingDeposit_tblPlantDeposit_Dup FOREIGN KEY(intPlantDepositID)
		REFERENCES tblPlantDeposit(intPlantDepositID)
)
GO

-- Herbarium Sheet
IF OBJECT_ID('tblHerbariumSheet', 'U') IS NOT NULL
	DROP TABLE tblHerbariumSheet
GO
CREATE TABLE tblHerbariumSheet
(
	intHerbariumSheetID INT IDENTITY(1000, 1) NOT NULL,
	intPlantDepositID INT NOT NULL,
	intPlantReferenceID INT NOT NULL,
	intSpeciesID INT NOT NULL,
	intValidatorID INT NOT NULL,
	dateVerified DATE NOT NULL,
	CONSTRAINT pk_tblHerbariumSheet PRIMARY KEY(intHerbariumSheetID),
	CONSTRAINT fk_tblHerbariumSheet_tblPlantDeposit_Org FOREIGN KEY(intPlantDepositID)
		REFERENCES tblPlantDeposit(intPlantDepositID),
	CONSTRAINT fk_tblHerbariumSheet_tblPlantDeposit_Ref FOREIGN KEY(intPlantReferenceID)
		REFERENCES tblPlantDeposit(intPlantDepositID),
	CONSTRAINT fk_tblHerbariumSheet_tblSpecies FOREIGN KEY(intSpeciesID)
		REFERENCES tblSpecies(intSpeciesID),
	CONSTRAINT fk_tblHerbariumSheet_tblValidator FOREIGN KEY(intValidatorID)
		REFERENCES tblValidator(intValidatorID)
)
GO

-- Stored Herbarium Sheet
IF OBJECT_ID('tblStoredHerbarium', 'U') IS NOT NULL
	DROP TABLE tblStoredHerbarium
GO
CREATE TABLE tblStoredHerbarium
(
	intStoredSheetID INT IDENTITY(1000, 1) NOT NULL,
	intHerbariumSheetID INT NOT NULL,
	intBoxID INT NOT NULL,
	boolLoanAvailable BIT NOT NULL,
	CONSTRAINT pk_tblStoredHerbarium PRIMARY KEY(intStoredSheetID),
	CONSTRAINT fk_tblStoredHerbarium_tblHerbariumSheet FOREIGN KEY(intHerbariumSheetID)
		REFERENCES tblHerbariumSheet(intHerbariumSheetID),
	CONSTRAINT fk_tblStoredHerbarium_tblFamilyBox FOREIGN KEY(intBoxID)
		REFERENCES tblFamilyBox(intBoxID)
)

-- Loan Transaction
IF OBJECT_ID('tblPlantLoanTransaction', 'U') IS NOT NULL
	DROP TABLE tblPlantLoanTransaction
GO
CREATE TABLE tblPlantLoanTransaction
(
	intLoanID INT IDENTITY(1, 1) NOT NULL,
	intBorrowerID INT NOT NULL,
	dateLoan DATE NOT NULL,
	dateReturning DATE NOT NULL,
	dateProcessed DATETIME NOT NULL,
	strPurpose VARCHAR(255) NOT NULL,
	strStatus VARCHAR(10) NOT NULL,
	CONSTRAINT pk_tblPlantLoanTransaction PRIMARY KEY(intLoanID),
	CONSTRAINT fk_tblPlantLoanTransaction_tblBorrower FOREIGN KEY(intBorrowerID)
		REFERENCES tblBorrower(intBorrowerID)
)
GO

-- Plants Deposits in Loan
IF OBJECT_ID('tblLoaningPlants', 'U') IS NOT NULL
	DROP TABLE tblLoaningPlants
GO
CREATE TABLE tblLoaningPlants
(
	intPlantLoanID INT IDENTITY(1, 1) NOT NULL,
	intLoanID INT NOT NULL,
	intHerbariumSheetID INT NOT NULL,
	CONSTRAINT pk_tblLoaningPlants PRIMARY KEY(intPlantLoanID),
	CONSTRAINT fk_tblLoaningPlants_tblPlantLoanTransaction FOREIGN KEY(intLoanID)
		REFERENCES tblPlantLoanTransaction(intLoanID),
	CONSTRAINT fk_tblLoaningPlants_tblHerbariumSheet FOREIGN KEY(intHerbariumSheetID)
		REFERENCES tblHerbariumSheet(intHerbariumSheetID)
)

-- Species in Loan
IF OBJECT_ID('tblLoaningSpecies', 'U') IS NOT NULL
	DROP TABLE tblLoaningSpecies
GO
CREATE TABLE tblLoaningSpecies
(
	intSpeciesLoanID INT IDENTITY(1, 1) NOT NULL,
	intLoanID INT NOT NULL,
	intSpeciesID INT NOT NULL,
	intCopies INT NOT NULL,
	CONSTRAINT pk_tblLoaningSpecies PRIMARY KEY(intSpeciesLoanID),
	CONSTRAINT fk_tblLoaningSpecies_tblPlantLoanTransaction FOREIGN KEY(intLoanID)
		REFERENCES tblPlantLoanTransaction(intLoanID),
	CONSTRAINT fk_tblLoaningSpecies_tblSpecies FOREIGN KEY(intSpeciesID)
		REFERENCES tblSpecies(intSpeciesID)
)

-------------- VIEWS CREATION --------------

-- Phylum View
IF OBJECT_ID('viewTaxonPhylum', 'V') IS NOT NULL
	DROP VIEW viewTaxonPhylum
GO
CREATE VIEW viewTaxonPhylum
AS
(
	SELECT	TP.intPhylumID, 
			CONCAT('P', FORMAT(TP.intPhylumID, '0#')) strPhylumNo, 
			TP.strDomainName, TP.strKingdomName, TP.strPhylumName
	FROM tblPhylum TP
)
GO

-- Class View
IF OBJECT_ID('viewTaxonClass', 'V') IS NOT NULL
	DROP VIEW viewTaxonClass
GO
CREATE VIEW viewTaxonClass
AS
(
	SELECT	TC.intClassID,
			CONCAT('P', FORMAT(TP.intPhylumID, '0#'), 
				   'C', FORMAT(TC.intClassID, '0#')) strClassNo,
			TP.strDomainName, TP.strKingdomName, TP.strPhylumName, TC.strClassName 
	FROM tblClass TC
		INNER JOIN tblPhylum TP ON TC.intPhylumID = TP.intPhylumID
)
GO

-- Order View
IF OBJECT_ID('viewTaxonOrder', 'V') IS NOT NULL
	DROP VIEW viewTaxonOrder
GO
CREATE VIEW viewTaxonOrder
AS
(
	SELECT	TD.intOrderID,
			CONCAT('P', FORMAT(TP.intPhylumID, '0#'), 
				   'C', FORMAT(TC.intClassID, '0#'), 
				   'O', FORMAT(TD.intOrderID, '0#')) strOrderNo,
			TP.strDomainName, TP.strKingdomName, TP.strPhylumName, TC.strClassName, TD.strOrderName
	FROM tblOrder TD
		INNER JOIN tblClass TC ON TD.intClassID = TC.intClassID
		INNER JOIN tblPhylum TP ON TC.intPhylumID = TP.intPhylumID
)
GO

-- Family View
IF OBJECT_ID('viewTaxonFamily', 'V') IS NOT NULL
	DROP VIEW viewTaxonFamily
GO
CREATE VIEW viewTaxonFamily
AS
(
	SELECT	TF.intFamilyID,
			CONCAT('P', FORMAT(TP.intPhylumID, '0#'), 
				   'C', FORMAT(TC.intClassID, '0#'), 
				   'O', FORMAT(TD.intOrderID, '0#'), 
				   'F', FORMAT(TF.intFamilyID, '0#')) strFamilyNo,
			TP.strDomainName, TP.strKingdomName, TP.strPhylumName, TC.strClassName, TD.strOrderName, TF.strFamilyName
	FROM tblFamily TF
		INNER JOIN tblOrder TD ON TF.intOrderID = TD.intOrderID
		INNER JOIN tblClass TC ON TD.intClassID = TC.intClassID
		INNER JOIN tblPhylum TP ON TC.intPhylumID = TP.intPhylumID
)
GO

-- Genus View
IF OBJECT_ID('viewTaxonGenus', 'V') IS NOT NULL
	DROP VIEW viewTaxonGenus
GO
CREATE VIEW viewTaxonGenus
AS
(
	SELECT	TG.intGenusID,
			CONCAT('P', FORMAT(TP.intPhylumID, '0#'), 
				   'C', FORMAT(TC.intClassID, '0#'), 
				   'O', FORMAT(TD.intOrderID, '0#'), 
				   'F', FORMAT(TF.intFamilyID, '0#'), 
				   'G', FORMAT(TG.intGenusID, '0#')) strGenusNo,
			TP.strDomainName, TP.strKingdomName, TP.strPhylumName, TC.strClassName, TD.strOrderName, TF.strFamilyName, TG.strGenusName
	FROM tblGenus TG 
		INNER JOIN tblFamily TF ON TG.intFamilyID = TF.intFamilyID
		INNER JOIN tblOrder TD ON TF.intOrderID = TD.intOrderID
		INNER JOIN tblClass TC ON TD.intClassID = TC.intClassID
		INNER JOIN tblPhylum TP ON TC.intPhylumID = TP.intPhylumID
)
GO

-- Species Hierarchy View
IF OBJECT_ID('viewSpeciesHierarchy', 'V') IS NOT NULL
	DROP VIEW viewSpeciesHierarchy
GO
CREATE VIEW viewSpeciesHierarchy
AS
(
	SELECT	TS.intSpeciesID,
			CONCAT('P', FORMAT(TP.intPhylumID, '0#'), 
				   'C', FORMAT(TC.intClassID, '0#'), 
				   'O', FORMAT(TD.intOrderID, '0#'), 
				   'F', FORMAT(TF.intFamilyID, '0#'), 
				   'G', FORMAT(TG.intGenusID, '0#'), 
				   'S', FORMAT(TS.intSpeciesID, '0#')) strSpeciesNo,
			TP.strDomainName, TP.strKingdomName, TP.strPhylumName, TC.strClassName, TD.strOrderName, TF.strFamilyName, TG.strGenusName, 
			TS.strSpeciesName, TS.strCommonName
	FROM tblSpecies TS
		INNER JOIN tblGenus TG ON TS.intGenusID = TG.intGenusID
		INNER JOIN tblFamily TF ON TG.intFamilyID = TF.intFamilyID
		INNER JOIN tblOrder TD ON TF.intOrderID = TD.intOrderID
		INNER JOIN tblClass TC ON TD.intClassID = TC.intClassID
		INNER JOIN tblPhylum TP ON TC.intPhylumID = TP.intPhylumID
)
GO

-- Full Species View
IF OBJECT_ID('viewTaxonSpecies', 'V') IS NOT NULL
	DROP VIEW viewTaxonSpecies
GO
CREATE VIEW viewTaxonSpecies
AS
(
	SELECT Sp.intSpeciesID, Sp.strSpeciesNo, Sp.strDomainName, Sp.strKingdomName, Sp.strPhylumName, Sp.strClassName, Sp.strOrderName, Sp.strFamilyName, 
			Sp.strGenusName, Sp.strSpeciesName, Sp.strCommonName, Au.strAuthorsName, 
			RTRIM(CONCAT(Sp.strGenusName, ' ', Sp.strSpeciesName, ' ', ISNULL(Au.strSpeciesSuffix, ''))) strScientificName,
			CAST((CASE WHEN Au.strSpeciesSuffix IS NULL THEN 0 ELSE 1 END) AS BIT) boolSpeciesIdentified
	FROM tblSpeciesAuthor SA  
		INNER JOIN tblAuthor Au ON SA.intAuthorID = Au.intAuthorID
		RIGHT JOIN viewSpeciesHierarchy Sp ON SA.intSpeciesID = Sp.intSpeciesID
)
GO

-- Species Alternate Name
IF OBJECT_ID('viewSpeciesAlternate', 'V') IS NOT NULL
	DROP VIEW viewSpeciesAlternate
GO
CREATE VIEW viewSpeciesAlternate
AS
(
	SELECT SAN.intAltNameID, TS.strScientificName, SAN.strLanguage, SAN.strAlternateName
	FROM tblSpeciesAlternateName SAN
		INNER JOIN viewTaxonSpecies TS ON SAN.intSpeciesID = TS.intSpeciesID 
)
GO

-- Family Box View
IF OBJECT_ID('viewFamilyBox', 'V') IS NOT NULL
	DROP VIEW viewFamilyBox
GO
CREATE VIEW viewFamilyBox
AS
(
	SELECT FB.intBoxID, CONCAT('BOX-', FORMAT(FB.intBoxID, '00#')) strBoxNumber, TF.strFamilyName, FB.intBoxLimit, FB.intRackNo, FB.intRackRow, FB.intRackColumn
	FROM tblFamilyBox FB
		INNER JOIN tblFamily TF ON FB.intFamilyID = TF.intFamilyID
) 
GO

-- Locality View
IF OBJECT_ID('viewLocality', 'V') IS NOT NULL
	DROP VIEW viewLocality
GO
CREATE VIEW viewLocality
AS
(
	SELECT Lo.intLocalityID, Co.strCountry, Re.strIsland, Re.strRegion, Pr.strProvince, Ci.strCity, Lo.strSpecificLocation, Lo.strShortLocation,
		CASE
			WHEN Co.strCountry = 'Philippines' THEN CONCAT(Lo.strSpecificLocation, ', ', Ci.strCity, ', ', Pr.strProvince, ', ', Re.strRegion, ', ', Re.strIsland)
			ELSE Lo.strFullLocality
		END strFullLocality, Lo.strLatitude, Lo.strLongtitude
	FROM tblLocality Lo
		LEFT JOIN tblPHLocality PL ON Lo.intLocalityID = PL.intLocalityID
		INNER JOIN tblCity Ci ON PL.intCityID = Ci.intCityID
		INNER JOIN tblProvince Pr ON Ci.intProvinceID = Pr.intProvinceID
		INNER JOIN tblRegion Re ON Pr.intRegionID = Re.intRegionID
		INNER JOIN tblCountry Co ON Lo.intCountryID = Co.intCountryID
)
GO

-- Collector View
IF OBJECT_ID('viewCollector', 'V') IS NOT NULL
	DROP VIEW viewCollector
GO
CREATE VIEW viewCollector
AS
(
	SELECT Co.intCollectorID, Co.strFirstname, Co.strMiddlename, Co.strLastname, Co.strMiddleInitial, Co.strNameSuffix, Co.strHomeAddress, Co.strContactNumber,
		Co.strEmailAddress, RTRIM(LTRIM(CONCAT(Co.strFirstname, ' ', Co.strLastname, ' ', Co.strNameSuffix))) strFullName, Co.strAffiliation
	FROM tblCollector Co
)
GO

-- Borrower VIew
IF OBJECT_ID('viewBorrower', 'V') IS NOT NULL
	DROP VIEW viewBorrower
GO
CREATE VIEW viewBorrower
AS
(
	SELECT Bo.intBorrowerID, Bo.strFirstname, Bo.strMiddlename, Bo.strLastname, Bo.strMiddleInitial, Bo.strNameSuffix, Bo.strHomeAddress, Bo.strContactNumber,
		Bo.strEmailAddress, RTRIM(LTRIM(CONCAT(Bo.strFirstname, ' ', Bo.strLastname, ' ', Bo.strNameSuffix))) strFullName, Bo.strAffiliation
	FROM tblBorrower Bo
)
GO

-- Validator View
IF OBJECT_ID('viewValidator', 'V') IS NOT NULL
	DROP VIEW viewValidator
GO
CREATE VIEW viewValidator
AS
(
	SELECT Va.intValidatorID, Va.strFirstname, Va.strMiddlename, Va.strLastname, Va.strMiddleInitial, Va.strNameSuffix, Va.strContactNumber,
		Va.strEmailAddress, RTRIM(LTRIM(CONCAT(Va.strFirstname, ' ', Va.strLastname, ' ', Va.strNameSuffix))) strFullName, Va.strInstitution, Va.strValidatorType
	FROM tblValidator Va
)
GO

-- Herbarium Staff View
IF OBJECT_ID('viewHerbariumStaff', 'V') IS NOT NULL
	DROP VIEW viewHerbariumStaff
GO
CREATE VIEW viewHerbariumStaff
AS
(
	SELECT St.intStaffID, St.strFirstname, St.strMiddlename, St.strLastname, St.strMiddleInitial, St.strNameSuffix, St.strContactNumber,
		St.strEmailAddress, RTRIM(LTRIM(CONCAT(St.strFirstname, ' ', St.strLastname, ' ', St.strNameSuffix))) strFullName, 
		St.strRole, St.strCollegeDepartment, St.boolActive
	FROM tblHerbariumStaff St
	WHERE St.boolActive = 1
)
GO

-- Access Accounts
IF OBJECT_ID('viewAccounts', 'V') IS NOT NULL
	DROP VIEW viewAccounts
GO
CREATE VIEW viewAccounts
AS
(
	SELECT HS.intStaffID, HS.strFullName, Ac.strUsername, Ac.strPassword, HS.strRole, Ac.boolActive
	FROM tblAccounts Ac	
		INNER JOIN viewHerbariumStaff HS ON Ac.intStaffID = HS.intStaffID
	WHERE Ac.boolActive = 1
)
GO

-- Received Deposit View
IF OBJECT_ID('viewReceivedDeposit', 'V') IS NOT NULL
	DROP VIEW viewReceivedDeposit
GO
CREATE VIEW viewReceivedDeposit
AS
(
	SELECT RD.intDepositID, CONCAT('DEP-', FORMAT(RD.intDepositID, '00000#')) strDepositNumber, RD.intPlantTypeID, 
			PT.strPlantTypeName, RD.picHerbariumSheet, Co.strFullName AS strCollector, Lo.strFullLocality, Lo.strShortLocation,
			St.strFullName AS strStaff, RD.dateCollected, RD.dateDeposited, RD.strDescription, RD.strStatus
	FROM tblReceivedDeposits RD
		INNER JOIN tblPlantType PT ON RD.intPlantTypeID = PT.intPlantTypeID
		INNER JOIN viewCollector Co ON RD.intCollectorID = Co.intCollectorID
		INNER JOIN viewLocality Lo ON RD.intLocalityID = Lo.intLocalityID
		INNER JOIN viewHerbariumStaff St ON RD.intStaffID = St.intStaffID
)
GO

-- Plant Deposit View
IF OBJECT_ID('viewPlantDeposit', 'V') IS NOT NULL
	DROP VIEW viewPlantDeposit
GO
CREATE VIEW viewPlantDeposit
AS
(
	SELECT PD.intPlantDepositID, 
			CONCAT(AF.strInstitutionCode,'-', PT.strPlantTypeCode, '-', FORMAT(PD.intAccessionNumber, AF.strAccessionFormat), '-', 
			FORMAT(YEAR(PD.dateDeposited) % POWER(10, LEN(AF.strYearFormat)), AF.strYearFormat)) strAccessionNumber, 
			PD.intAccessionNumber, Co.strFullName AS strCollector, Lo.strFullLocality,
			St.strFullName AS strStaff, PD.dateCollected, PD.dateDeposited, PD.strDescription, PD.strStatus
	FROM tblPlantDeposit PD
		INNER JOIN tblPlantType PT ON PD.intPlantTypeID = PT.intPlantTypeID
		INNER JOIN tblAccessionFormat AF ON PD.intFormatID = AF.intFormatID
		INNER JOIN viewCollector Co ON PD.intCollectorID = Co.intCollectorID
		INNER JOIN viewLocality Lo ON PD.intLocalityID = Lo.intLocalityID
		INNER JOIN viewHerbariumStaff St ON PD.intStaffID = St.intStaffID
)
GO

-- Herbarium Sheet Picture View
IF OBJECT_ID('viewHerbariumPicture', 'V') IS NOT NULL
	DROP VIEW viewHerbariumPicture
GO
CREATE VIEW viewHerbariumPicture
AS
(
	SELECT HP.intPictureID, PD.strAccessionNumber, HP.picHerbariumSheet, HP.strTagDescription
	FROM tblHerbariumSheetPicture HP
		INNER JOIN viewPlantDeposit PD ON HP.intPlantDepositID = PD.intPlantDepositID
)
GO

-- Externally Verifying Plant Deposit View
IF OBJECT_ID('viewVerifyingDeposit', 'V') IS NOT NULL
	DROP VIEW viewVerifyingDeposit
GO
CREATE VIEW viewVerifyingDeposit
AS
(
	SELECT PD_A.intPlantDepositID, PD_A.strAccessionNumber, PD_B.strAccessionNumber AS strReferenceAccession, 
		TS.strFamilyName, TS.strScientificName, TS.strCommonName, PD_A.strCollector, PD_A.strFullLocality, PD_A.strStaff,  
		PD_A.dateCollected, PD_A.dateDeposited, PD_A.strDescription, PD_A.strStatus
	FROM tblVerifyingDeposit VD
		RIGHT JOIN viewPlantDeposit PD_A ON VD.intPlantDepositID = PD_A.intPlantDepositID
		INNER JOIN viewPlantDeposit PD_B ON VD.intReferenceDepositID = PD_B.intPlantDepositID
		INNER JOIN viewTaxonSpecies TS ON VD.intSpeciesID = TS.intSpeciesID
	WHERE PD_A.strStatus = 'Further Verification'
)
GO

-- Herbarium Sheet
IF OBJECT_ID('viewHerbariumSheet', 'V') IS NOT NULL
	DROP VIEW viewHerbariumSheet
GO
CREATE VIEW viewHerbariumSheet
AS
(
	SELECT HS.intHerbariumSheetID, HS.intPlantDepositID, PD_A.strAccessionNumber, 
			HS.intPlantReferenceID, PD_B.strAccessionNumber AS strReferenceAccession, 
			TS.strFamilyName, TS.strScientificName, TS.strCommonName, PD_A.strCollector, 
			PD_A.strFullLocality, PD_A.strStaff, PD_A.dateCollected, PD_A.dateDeposited, HS.dateVerified,
			PD_A.strDescription, Va.strFullName AS strValidator, PD_A.strStatus
	FROM tblHerbariumSheet HS
		INNER JOIN viewPlantDeposit PD_A ON HS.intPlantDepositID = PD_A.intPlantDepositID
		INNER JOIN viewPlantDeposit PD_B ON HS.intPlantReferenceID = PD_B.intPlantDepositID
		INNER JOIN viewTaxonSpecies TS ON HS.intSpeciesID = TS.intSpeciesID
		INNER JOIN viewValidator Va ON HS.intValidatorID = Va.intValidatorID
)
GO

-- Stored Herbarium
IF OBJECT_ID('viewHerbariumInventory', 'V') IS NOT NULL
	DROP VIEW viewHerbariumInventory
GO
CREATE VIEW viewHerbariumInventory
AS
(
	SELECT SH.intStoredSheetID, HS.strAccessionNumber, HS.strReferenceAccession, 
		HS.strFamilyName, HS.strScientificName, HS.strCommonName, HS.strCollector, HS.strFullLocality, HS.strStaff, 
		HS.dateCollected, HS.dateDeposited, HS.strDescription, HS.strStatus, HS.strValidator, 
		HS.dateVerified, FB.strBoxNumber, SH.boolLoanAvailable
	FROM tblStoredHerbarium SH
		INNER JOIN viewHerbariumSheet HS ON SH.intHerbariumSheetID = HS.intHerbariumSheetID
		INNER JOIN viewFamilyBox FB ON SH.intBoxID = FB.intBoxID
)
GO

-- Plant Loans
IF OBJECT_ID('viewPlantLoans', 'V') IS NOT NULL
	DROP VIEW viewPlantLoans
GO
CREATE VIEW viewPlantLoans
AS
(
	SELECT PL.intLoanID, CONCAT('HB-', FORMAT(PL.intLoanID, '00000#')) as strLoanNumber, Bo.strFullName AS strBorrower, 
		PL.dateLoan, PL.dateReturning, CONCAT(CONVERT(VARCHAR, PL.dateLoan, 107), ' - ', CONVERT(VARCHAR, PL.dateReturning, 107)) as strDuration, 
		PL.dateProcessed, PL.strPurpose, PL.strStatus
	FROM tblPlantLoanTransaction PL
		INNER JOIN viewBorrower Bo ON PL.intBorrowerID = Bo.intBorrowerID
)
GO

IF OBJECT_ID('viewSpeciesInventory', 'V') IS NOT NULL
	DROP VIEW viewSpeciesInventory
GO
CREATE VIEW viewSpeciesInventory
AS
(
	SELECT TS.strFamilyName, TS.strGenusName, TS.strScientificName, COUNT(HI.intStoredSheetID) AS intSpeciesCount
	FROM viewTaxonSpecies TS 
		LEFT JOIN viewHerbariumInventory HI ON TS.strScientificName = HI.strScientificName AND HI.boolLoanAvailable = 1
	GROUP BY TS.strFamilyName, TS.strGenusName, TS.strScientificName 
)
GO

IF OBJECT_ID('viewSpeciesLoanCount', 'V') IS NOT NULL
	DROP VIEW viewSpeciesLoanCount
GO
CREATE VIEW viewSpeciesLoanCount
AS
(
	SELECT TS.strScientificName, SUM(LS.intCopies) AS intBorrowedCount
	FROM viewTaxonSpecies TS  
		JOIN tblLoaningSpecies LS ON TS.intSpeciesID = LS.intSpeciesID 
		JOIN tblPlantLoanTransaction LT ON LT.intLoanID = LS.intLoanID AND LT.strStatus = 'Requesting'
	GROUP BY TS.strFamilyName, TS.strScientificName 
)
GO

IF OBJECT_ID('viewLoanedSpecies', 'V') IS NOT NULL
	DROP VIEW viewLoanedSpecies
GO
CREATE VIEW viewLoanedSpecies
AS
(
	SELECT LS.intSpeciesLoanID, PL.strLoanNumber, TS.strFamilyName, TS.strScientificName, LS.intCopies 
	FROM tblLoaningSpecies LS
		JOIN viewPlantLoans PL ON LS.intLoanID = PL.intLoanID
		JOIN viewTaxonSpecies TS ON LS.intSpeciesID = TS.intSpeciesID
)
GO

--------- STATIC DATA INSERTION ---------
-- Do not Overwrite!

SET NOCOUNT ON

-- System Menu
INSERT INTO tblSystemMenu(strLevel, strMainMenu, strSubMenu, strPageLocation, strAccessLevel)
VALUES ('A', 'Home',				NULL, 'HomePage',			'STUDENT ASSISTANT'),
	   ('A', 'Maintenance',			NULL, 'MaintenancePage',	'STUDENT ASSISTANT'),
	   ('A', 'Transaction',			NULL, 'TransactionPage',	'STUDENT ASSISTANT'),
	   ('A', 'Management Tools',	NULL, 'UtilitiesPage',		'STUDENT ASSISTANT'),
	   ('A', 'Queries',				NULL, 'QueriesPage',		'CURATOR'),
	   ('A', 'Reports',				NULL, 'ReportsPage',		'CURATOR')
GO
INSERT INTO tblSystemMenu(strLevel, strMainMenu, strSubMenu, strPageLocation, strAccessLevel)
VALUES ('B', 'Maintenance',			'Taxonomic Hierarchy',				'TaxonomicHierarchyPage',	'CURATOR'),
       ('B', 'Maintenance',			'Species Author',					'SpeciesAuthorPage',		'CURATOR'),
       ('B', 'Maintenance',			'Species Nomenclature',				'SpeciesNomenclaturePage',	'CURATOR'),
       ('B', 'Maintenance',			'Plant Types',						'PlantTypePage',			'CURATOR'),
       ('B', 'Maintenance',			'Herbarium Boxes',					'HerbariumBoxPage',			'STUDENT ASSISTANT'),
       ('B', 'Maintenance',			'Plant Locality',					'PlantLocalityPage',		'STUDENT ASSISTANT'),
       ('B', 'Maintenance',			'Collector',						'CollectorPage',			'STUDENT ASSISTANT'),
       ('B', 'Maintenance',			'Borrower',							'BorrowerPage',				'STUDENT ASSISTANT'),
       ('B', 'Maintenance',			'External Validator',				'ExternalValidatorPage',	'CURATOR'),
       ('B', 'Maintenance',			'Herbarium Staff',					'HerbariumStaffPage',		'ADMINISTRATOR'),
       ('B', 'Maintenance',			'Access Accounts',					'AccessAccountsPage',		'ADMINISTRATOR'),
       ('B', 'Transaction',			'Plant Deposit',					'PlantDepositPage',			'STUDENT ASSISTANT'),
       ('B', 'Transaction',			'Plant Receiving',					'PlantReceivingPage',		'CURATOR'),
       ('B', 'Transaction',			'Plant Resubmission',				'PlantResubmissionPage',	'CURATOR'),
       ('B', 'Transaction',			'Plant Verification',				'PlantVerificationPage',	'CURATOR'),
       ('B', 'Transaction',			'Plant Classification and Storage',	'PlantClassificationPage',	'CURATOR'),
       ('B', 'Transaction',			'Plant Loaning',					'PlantLoaningPage',			'STUDENT ASSISTANT'),
       ('B', 'Transaction',			'Plant Loan Returns',				'PlantLoanReturnPage',		'CURATOR'),
       ('B', 'Management Tools',	'Herbarium Inventory',				'HerbariumInventoryPage',	'STUDENT ASSISTANT'),
       ('B', 'Management Tools',	'Sheet Tracking',					'SheetTrackingPage',		'CURATOR'),
       ('B', 'Management Tools',	'Audit Trailing',					'AuditTrailingPage',		'CURATOR')
GO

-- Countries
INSERT INTO tblCountry(strCountry)
VALUES ('Afghanistan'), ('Aland Islands'), ('ALbania'), ('Algeria'), ('American Samoa'), 
	   ('Andorra'), ('Angola'), ('Anguilla'), ('Antarctica'), ('Antigua and Barbuda'), 
	   ('Argentina'), ('Armenia'), ('Aruba'), ('Australia'), ('Austria'), 
	   ('Azerbaijan'), ('Bahamas'), ('Bahrain'), ('Bangladesh'), ('Barbados'), 
	   ('Belarus'), ('Belgium'), ('Belize'), ('Benin'), ('Bermuda'),
	   ('Bhutan'), ('Bolivia'), ('Bosnia and Herzegovina'), ('Botswana'), ('Bouvet Island'),
	   ('Brazil'), ('British Indian Ocean Territory'), ('Brunei Darussalam'), ('Bulgaria'), ('Burkina Paso'),
	   ('Burundi'), ('Cambodia'), ('Cameroon'), ('Canada'), ('Cape Verde'),
	   ('Cayman Islands'), ('Central African Republic'), ('Chad'), ('Chile'), ('China'),
	   ('Christmas Island'), ('Cocos (Keeling) Island'), ('Colombia'), ('Comoros'), ('Congo'),
	   ('Congo, Democratic Republic of'), ('Cook Islands'), ('Costa Rica'), ('Cote D''Ivoire'), ('Croatia'),
	   ('Cuba'), ('Cyprus'), ('Czech Republic'), ('Denmark'), ('Djibouti'),
	   ('Dominica'), ('Dominican Republic'), ('Ecuador'), ('Egypt'), ('El Salvador'),
	   ('Equatorial Guinea'), ('Eritrea'), ('Estonia'), ('Ethiopia'), ('Falkland Islands (Malvinas)'),
	   ('Faroe Islands'), ('Fiji'), ('Finland'), ('France'), ('French Guiana'),
	   ('French Polynesia'), ('French Southern Territories'), ('Gabon'), ('Gambia'), ('Georgia'),
	   ('Germany'), ('Ghana'), ('Gibraltar'), ('Greece'), ('Greenland'),
	   ('Grenada'), ('Guadaloupe'), ('Guam'), ('Guatemala'), ('Guernsey'),
	   ('Guinea'), ('Guinea-Bissau'), ('Guyana'), ('Haiti'), ('Heard Island and McDonald Islands'),
	   ('Holy See (Vatican City State)'), ('Honduras'), ('Hong Kong'), ('Hungary'), ('Iceland'),
	   ('India'), ('Indonesia'), ('Iran'), ('Iraq'), ('Ireland'),
	   ('Isle of Man'), ('Israel'), ('Italy'), ('Jamaica'), ('Japan'),
	   ('Jersey'), ('Jordan'), ('Kazakhstan'), ('Kenya'), ('Kiribati'),
	   ('Korea, North'), ('Korea, South'), ('Kuwait'), ('Kyrgyzstan'), ('Laos'),
	   ('Latvia'), ('Lebanon'), ('Lesotho'), ('Liberia'), ('Libya'),
	   ('Liechtenstein'), ('Lithuania'), ('Luxemberg'), ('Macao'), ('Macedonia'),
	   ('Madagascar'), ('Malawi'), ('Malaysia'), ('Maldives'), ('Mali'),
	   ('Malta'), ('Marshall Islands'), ('Martinique'), ('Mauritania'), ('Mauritius'),
	   ('Mayotte'), ('Mexico'), ('Micronesia'), ('Moldova'), ('Monaco'),
	   ('Mongolia'), ('Montenegro'), ('Montserrat'), ('Morocco'), ('Mozambique'),
	   ('Myanmar'), ('Namibia'), ('Nauru'), ('Nepal'), ('Netherlands'),
	   ('Netherlands Antille'), ('New Caledonia'), ('New Zealand'), ('Nicaragua'), ('Niger'),
	   ('Nigeria'), ('Niue'), ('Norfolk Island'), ('Northern Mariana Islands'), ('Norway'),
	   ('Oman'), ('Pakistan'), ('Palau'), ('Palestine'), ('Panama'), 
	   ('Papua New Guinea'), ('Paraguay'), ('Peru'), ('Philippines'), ('Pitcairn'),
	   ('Poland'), ('Portugal'), ('Puerto Rico'), ('Qatar'), ('Reunion'),
	   ('Romania'), ('Russia'), ('Rwanda'), ('Saint Barthelemy'), ('Saint Helena, Ascension and Tristan Da Cunha'),
	   ('Saint Kitts'), ('Saint Lucia'), ('Saint Martin'), ('Saint Pierre and Miquelon'), ('Saint Vincent and the Grenadines'),
	   ('Samoa'), ('San Marino'), ('Sao Tome and Principe'), ('Saudi Arabia'), ('Senegal'),
	   ('Serbia'), ('Seychelles'), ('Sierra Leone'), ('Singapore'), ('Slovakia'),
	   ('Slovenia'), ('Solomon Islands'), ('Somalia'), ('South Africa'), ('South Georgia and the South Sandwich Islands'),
	   ('Spain'), ('Sri Lanka'), ('Sudan'), ('Suriname'), ('Svalbard and Jan Mayen'),
	   ('Swaziland'), ('Sweden'), ('Switzerland'), ('Syria'), ('Taiwan'),
	   ('Tajikistan'), ('Tanzania'), ('Thailand'), ('Timor-Leste'), ('Togo'),
	   ('Tokelau'), ('Tonga'), ('Trinidad and Tobago'), ('Tunisia'), ('Turkey'),
	   ('Turkmenistan'), ('Turks and Caicos Islands'), ('Tuvalu'), ('Uganda'), ('Ukraine'),
	   ('United Arab Emirates'), ('United Kingdom'), ('United States'), ('United States Minor Outlying Islands'), ('Uruguay'),
	   ('Uzbekistan'), ('Vanuatu'), ('Vatican City'), ('Venezuela'), ('Vietnam'),
	   ('Virgin Islands (British)'), ('Virgin Islands (US)'), ('Wallis and Fortuna'), ('Western Sahara'), ('Yemen'),
	   ('Yugoslavia'), ('Zambia'), ('Zimbabwe')

-- Regions
INSERT INTO tblRegion (strIsland, strRegionCode, strRegion)
VALUES ('Luzon', 'NCR', 'National Capital Region'),
	   ('Luzon', 'CAR', 'Cordillera Administrative Region'),
	   ('Luzon', 'I', 'Ilocos Region'),
	   ('Luzon', 'II', 'Cagayan Valley'),
	   ('Luzon', 'III', 'Central Luzon'),
	   ('Luzon', 'IV-A', 'CALABARZON'),
	   ('Luzon', 'IV-B', 'MIMAROPA'),
	   ('Luzon', 'V', 'Bicol Region'),
	   ('Visayas', 'VI', 'Western Visayas'),
	   ('Visayas', 'VII', 'Central Visayas'),
	   ('Visayas', 'VIII', 'Eastern Visayas'),
	   ('Mindanao', 'IX', 'Zamboanga Peninsula'),
	   ('Mindanao', 'X', 'Northern Mindanao'),
	   ('Mindanao', 'XI', 'Davao Region'),
	   ('Mindanao', 'XII', 'SOCCSKSARGEN'),
	   ('Mindanao', 'XIII', 'Caraga Region'),
	   ('Mindanao', 'ARMM', 'Autonomous Region in Muslim Mindanao')
GO

-- Provinces
INSERT INTO tblProvince (intRegionID, strProvince)
VALUES (1, 'Metro Manila'),
	   (2, 'Abra'), (2, 'Apayao'), (2, 'Benguet'), (2, 'Ifugao'), (2, 'Kalinga'), (2, 'Mountain Province'),
	   (3, 'Ilocos Norte'), (3, 'Ilocos Sur'), (3, 'La Union'), (3, 'Pangasinan'),
	   (4, 'Batanes'), (4, 'Cagayan'), (4, 'Isabela'), (4, 'Nueva Vizcaya'), (4, 'Quirino'),
	   (5, 'Aurora'), (5, 'Bataan'), (5, 'Bulacan'), (5, 'Nueva Ecija'), (5, 'Pampanga'), (5, 'Tarlac'), (5, 'Zambales'),
	   (6, 'Batangas'), (6, 'Cavite'), (6, 'Laguna'), (6, 'Quezon'), (6, 'Rizal'),
	   (7, 'Marinduque'), (7, 'Occidental Mindoro'), (7, 'Oriental Mindoro'), (7, 'Palawan'), (7, 'Romblon'),
	   (8, 'Albay'), (8, 'Camarines Norte'), (8, 'Camarines Sur'), (8, 'Catanduanes'), (8, 'Masbate'), (8, 'Sorsogon'),
	   (9, 'Aklan'), (9, 'Antique'), (9, 'Capiz'), (9, 'Guimaras'), (9, 'Negros Occidental'), (9, 'Iloilo'),
	   (10, 'Bohol'), (10, 'Cebu'), (10, 'Negros Oriental'), (10, 'Siquijor'),
	   (11, 'Biliran'), (11, 'Eastern Samar'), (11, 'Leyte'), (11, 'Northern Samar'), (11, 'Southern Leyte'),
	   (12, 'Zamboanga del Norte'), (12, 'Zamboanga del Sur'), (12, 'Zamboanga Sibugay'),
	   (13, 'Bukidnon'), (13, 'Camiguin'), (13, 'Lanao del Norte'), (13, 'Misamis Occidental'), (13, 'Misamis Oriental'),
	   (14, 'Compostela Valley'), (14, 'Davao del Norte'), (14, 'Davao del Sur'), (14, 'Davao Occidental'), (14, 'Davao Oriental'),
	   (15, 'Cotabato'), (15, 'Saranggani'), (15, 'South Cotabato'), (15, 'Sultan Kudarat'),
	   (16, 'Agusan del Norte'), (16, 'Agusan del Sur'), (16, 'Dinagat Islands'), (16, 'Surigao del Norte'), (16, 'Surigao del Sur'),
	   (17, 'Basilan'), (17, 'Lanao del Sur'), (17, 'Maguindanao'), (17, 'Sulu'), (17, 'Tawi-Tawi')
GO

-- Cities
INSERT INTO tblCity (intProvinceID, strCity)
VALUES (1, 'Caloocan City'), (1, 'Las Piñas, City of'), (1, 'Makati, City of'), (1, 'Malabon, City of'), (1, 'Mandaluyong, City of'), 
	   (1, 'Manila, City of '), (1, 'Marikina, City of '), (1, 'Muntilupa, City of '), (1, 'Navotas, City of'), (1, 'Parañaque, City of'),
	   (1, 'Pasay City'), (1, 'Pasig, City of'), (1, 'Pateros'), (1, 'Quezon City'), (1, 'San Juan, City of '), 
	   (1, 'Taguig City'), (1, 'Valenzuela, City of ')

-- Others
INSERT INTO tblAccessionFormat (strInstitutionCode, strAccessionFormat, strYearFormat) VALUES ('PUPH', '0000#', '0#')

-------------- END OF FILE --------------
