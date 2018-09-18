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
	intBoxID INT IDENTITY(1000, 1) NOT NULL,
	strBoxNumber VARCHAR(10) NOT NULL,
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

-- Locality
IF OBJECT_ID('tblLocality', 'U') IS NOT NULL
	DROP TABLE tblLocality
GO
CREATE TABLE tblLocality
(
	intLocalityID INT IDENTITY(1000, 1) NOT NULL,
	strCountry VARCHAR(50) NOT NULL,
	strIsland VARCHAR(50),
	strRegion VARCHAR(50),
	strProvince VARCHAR(50),
	strCity VARCHAR(50),
	strArea VARCHAR(50),
	strSpecificLocation VARCHAR(255),
	strShortLocation VARCHAR(255),
	strFullLocality VARCHAR(50),
	strLatitude VARCHAR(20),
	strLongtitude VARCHAR(20),
	CONSTRAINT pk_tblLocality PRIMARY KEY(intLocalityID)
)
GO

-- Collector
IF OBJECT_ID('tblCollector', 'U') IS NOT NULL
	DROP TABLE tblCollector
GO
CREATE TABLE tblCollector
(
	intCollectorID INT IDENTITY(1000, 1) NOT NULL,
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
	intBorrowerID INT IDENTITY(1000, 1) NOT NULL,
	strFirstname VARCHAR(50) NOT NULL,
	strMiddlename VARCHAR(50),
	strLastname VARCHAR(50) NOT NULL,
	strMiddleInitial VARCHAR(3),
	strNameSuffix VARCHAR(5),
	strHomeAddress VARCHAR(MAX) NOT NULL,
	strContactNumber VARCHAR(15) NOT NULL,
	strEmailAddress VARCHAR(255) NOT NULL,
	strAffiliation VARCHAR(100) NOT NULL,
	CONSTRAINT pk_tblBorrower PRIMARY KEY(intBorrowerID)
)
GO

-- Validator
IF OBJECT_ID('tblValidator', 'U') IS NOT NULL       
	DROP TABLE tblValidator
GO
CREATE TABLE tblValidator
(
	intValidatorID INT IDENTITY(1000, 1) NOT NULL,
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
	intStaffID INT IDENTITY(1000, 1) NOT NULL,
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
	intAccountID INT IDENTITY(1000, 1) NOT NULL,
	intStaffID INT NOT NULL,
	strUsername VARCHAR(50) NOT NULL,
	strPassword VARCHAR(50) NOT NULL,
	boolActive BIT NOT NULL
	CONSTRAINT pk_tblAccounts PRIMARY KEY(intAccountID),
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
	picHerbariumSheet VARBINARY(MAX),
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
	FROM viewSpeciesHierarchy Sp 
		LEFT JOIN tblSpeciesAuthor SA ON SA.intSpeciesID = Sp.intSpeciesID
		INNER JOIN tblAuthor Au ON SA.intAuthorID = Au.intAuthorID
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
	SELECT FB.intBoxID, FB.strBoxNumber, TF.strFamilyName, FB.intBoxLimit, FB.intRackNo, FB.intRackRow, FB.intRackColumn
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
	SELECT intLocalityID, strCountry, strIsland, strRegion, strProvince, strCity, strArea, strSpecificLocation, strShortLocation,
		CASE
			WHEN strCountry = 'Philippines' THEN CONCAT(strSpecificLocation, ', ', strArea, ', ', strCity, ', ', strProvince, ', ', strRegion, ', ', strIsland)
			ELSE strFullLocality
		END strFullLocality, strLatitude, strLongtitude
	FROM tblLocality
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
	SELECT Ac.intAccountID, HS.intStaffID, HS.strFullName, Ac.strUsername, Ac.strPassword, HS.strRole, Ac.boolActive
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
			FORMAT(YEAR(PD.dateDeposited) % POWER(10, LEN(AF.strYearFormat)), AF.strYearFormat)) strAccessionNumber, PD.intAccessionNumber,
			PD.picHerbariumSheet, Co.strFullName AS strCollector, Lo.strFullLocality,
			St.strFullName AS strStaff, PD.dateCollected, PD.dateDeposited, PD.strDescription, PD.strStatus
	FROM tblPlantDeposit PD
		INNER JOIN tblPlantType PT ON PD.intPlantTypeID = PT.intPlantTypeID
		INNER JOIN tblAccessionFormat AF ON PD.intFormatID = AF.intFormatID
		INNER JOIN viewCollector Co ON PD.intCollectorID = Co.intCollectorID
		INNER JOIN viewLocality Lo ON PD.intLocalityID = Lo.intLocalityID
		INNER JOIN viewHerbariumStaff St ON PD.intStaffID = St.intStaffID
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
			PD_A.picHerbariumSheet, TS.strFamilyName, TS.strScientificName, TS.strCommonName, PD_A.strCollector, 
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
	SELECT SH.intStoredSheetID, HS.strAccessionNumber, HS.strReferenceAccession, HS.picHerbariumSheet, 
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

-------------- END OF FILE --------------
