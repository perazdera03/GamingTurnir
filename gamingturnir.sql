-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: gamingturnir_db
-- ------------------------------------------------------
-- Server version	9.7.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'abd156c3-7a18-11f1-a65a-74563c9c9f75:1-277';

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20260626210657_InitialMigration','9.0.0');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clanovitima`
--

DROP TABLE IF EXISTS `clanovitima`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clanovitima` (
  `ClanTimaId` int NOT NULL AUTO_INCREMENT,
  `KorisnikId` int NOT NULL,
  `TimId` int NOT NULL,
  `Uloga` longtext COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`ClanTimaId`),
  KEY `IX_ClanoviTima_KorisnikId` (`KorisnikId`),
  KEY `IX_ClanoviTima_TimId` (`TimId`),
  CONSTRAINT `FK_ClanoviTima_Korisnici_KorisnikId` FOREIGN KEY (`KorisnikId`) REFERENCES `korisnici` (`KorisnikId`) ON DELETE CASCADE,
  CONSTRAINT `FK_ClanoviTima_Timovi_TimId` FOREIGN KEY (`TimId`) REFERENCES `timovi` (`TimId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clanovitima`
--

LOCK TABLES `clanovitima` WRITE;
/*!40000 ALTER TABLE `clanovitima` DISABLE KEYS */;
INSERT INTO `clanovitima` VALUES (1,3,1,'Defanziva'),(3,1,1,'Kapiten'),(12,8,5,'Igrac'),(13,6,4,'Kapiten'),(14,2,2,'Majstor');
/*!40000 ALTER TABLE `clanovitima` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `korisnici`
--

DROP TABLE IF EXISTS `korisnici`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `korisnici` (
  `KorisnikId` int NOT NULL AUTO_INCREMENT,
  `Username` longtext COLLATE utf8mb4_general_ci NOT NULL,
  `PasswordHash` longtext COLLATE utf8mb4_general_ci NOT NULL,
  `Rola` int NOT NULL,
  `DatumRegisdtracije` datetime(6) NOT NULL,
  PRIMARY KEY (`KorisnikId`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `korisnici`
--

LOCK TABLES `korisnici` WRITE;
/*!40000 ALTER TABLE `korisnici` DISABLE KEYS */;
INSERT INTO `korisnici` VALUES (1,'stefan','AQAAAAIAAYagAAAAEOAU1KOz0ZXfhVI+ZFqtjm9RQCY4ik21spAVrjhhCxSfIfaQNBQdyA53g158y/TZww==',1,'2026-06-27 13:56:34.332908'),(2,'petar','AQAAAAIAAYagAAAAEAK8+wneJXURz2P0MqSpMwWlYeUsklY1WCfi3ab4Q2uDO16Wlq7NS4/XmMXi/Coy2A==',0,'2026-06-27 13:59:35.917766'),(3,'vanja','AQAAAAIAAYagAAAAELwkrrZrJOl8eAJC6vmL8PMIxWVSMT11kt10IcxaXOvR9pQTkJh0DakvHY9DClKiwQ==',2,'2026-06-27 14:02:13.941069'),(6,'marko','AQAAAAIAAYagAAAAEJG5WYtiiD73yJyP9RdCbfYq2GWiCAGt4pg8vGwcoBkx4v7sP6rJpmshtjcWN3nVmQ==',1,'2026-06-27 15:31:13.646221'),(8,'aleksa','AQAAAAIAAYagAAAAEDAafrb+66U9OtfrkkzFvXTWpXlOeXm5nkm7SQOz6vQ3/D18ayxZc2UVgmF+sji8Mg==',0,'2026-07-06 21:20:07.930346'),(10,'test','AQAAAAIAAYagAAAAEEptclUsbRB3+YunMZ1ysKa/EEpi2Go63CVNmI32NFLbYG9OqFGq6a0SFMZw9psuRQ==',0,'2026-08-14 17:48:44.905168'),(11,'test1','AQAAAAIAAYagAAAAEMVanE7DY44URolvR3DdI7d/QOj98yhJPz2WnpW+f4wXZCoKpNfNQw+Jf4pl4laoOw==',1,'2026-08-14 18:19:08.976909'),(12,'test2','AQAAAAIAAYagAAAAENpsY2sGuGSzaPIq++tTCbihy+0O1bFmNRtv5tDTKB8O1I/m1jNfe2TMUN4do+6pPw==',2,'2026-08-14 18:21:52.547498');
/*!40000 ALTER TABLE `korisnici` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mecevi`
--

DROP TABLE IF EXISTS `mecevi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mecevi` (
  `MecId` int NOT NULL AUTO_INCREMENT,
  `TurnirId` int NOT NULL,
  `Tim1Id` int NOT NULL,
  `Tim2Id` int NOT NULL,
  `RezultatTim1` int DEFAULT NULL,
  `RezultatTim2` int DEFAULT NULL,
  `DatumMeca` datetime(6) NOT NULL,
  PRIMARY KEY (`MecId`),
  KEY `IX_Mecevi_Tim1Id` (`Tim1Id`),
  KEY `IX_Mecevi_Tim2Id` (`Tim2Id`),
  KEY `IX_Mecevi_TurnirId` (`TurnirId`),
  CONSTRAINT `FK_Mecevi_Timovi_Tim1Id` FOREIGN KEY (`Tim1Id`) REFERENCES `timovi` (`TimId`),
  CONSTRAINT `FK_Mecevi_Timovi_Tim2Id` FOREIGN KEY (`Tim2Id`) REFERENCES `timovi` (`TimId`),
  CONSTRAINT `FK_Mecevi_Turniri_TurnirId` FOREIGN KEY (`TurnirId`) REFERENCES `turniri` (`TurnirId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mecevi`
--

LOCK TABLES `mecevi` WRITE;
/*!40000 ALTER TABLE `mecevi` DISABLE KEYS */;
INSERT INTO `mecevi` VALUES (1,1,1,2,13,10,'2024-03-05 00:00:00.000000'),(2,1,4,5,16,14,'2024-03-06 00:00:00.000000'),(3,2,1,9,13,10,'2024-05-16 00:00:00.000000'),(4,5,2,3,2,0,'2025-06-12 00:00:00.000000'),(5,6,4,5,3,2,'2025-11-03 00:00:00.000000');
/*!40000 ALTER TABLE `mecevi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `timovi`
--

DROP TABLE IF EXISTS `timovi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `timovi` (
  `TimId` int NOT NULL AUTO_INCREMENT,
  `Naziv` longtext COLLATE utf8mb4_general_ci NOT NULL,
  `Opis` longtext COLLATE utf8mb4_general_ci NOT NULL,
  `DatumOsnivanja` datetime(6) NOT NULL,
  PRIMARY KEY (`TimId`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `timovi`
--

LOCK TABLES `timovi` WRITE;
/*!40000 ALTER TABLE `timovi` DISABLE KEYS */;
INSERT INTO `timovi` VALUES (1,'Vitality','Profesionalni tim iz Francuske','2013-08-05 00:00:00.000000'),(2,'FaZe Clan','Americki Tim za CS2 Esports','2010-05-30 00:00:00.000000'),(3,'Team Liquid','Holandski team za MOBA igrice.','2000-11-01 00:00:00.000000'),(4,'Invictus Gaming','Iskusni veterani gaming scene iz Kine','2011-08-02 00:00:00.000000'),(5,'Virtus.pro','Ruski tim sa iskustvom u vise igara.','2003-11-01 00:00:00.000000'),(9,'NaVi','Ukrajinska esports grupa','2009-12-17 00:00:00.000000');
/*!40000 ALTER TABLE `timovi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `turniri`
--

DROP TABLE IF EXISTS `turniri`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `turniri` (
  `TurnirId` int NOT NULL AUTO_INCREMENT,
  `Naziv` longtext COLLATE utf8mb4_general_ci NOT NULL,
  `Igrica` longtext COLLATE utf8mb4_general_ci NOT NULL,
  `DatumPocetka` datetime(6) NOT NULL,
  PRIMARY KEY (`TurnirId`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `turniri`
--

LOCK TABLES `turniri` WRITE;
/*!40000 ALTER TABLE `turniri` DISABLE KEYS */;
INSERT INTO `turniri` VALUES (1,'CS2 Open 2024','CS2','2024-03-01 00:00:00.000000'),(2,'Valorant Championship','Valorant','2024-05-15 00:00:00.000000'),(4,'League of Legends Cup','League of Legends','2025-07-20 00:00:00.000000'),(5,'Dota 2 Masters','Dota 2','2025-06-10 00:00:00.000000'),(6,'FIFA Pro League','FIFA 25','2025-11-01 00:00:00.000000'),(10,'R6S Championship','Rainbow Six Siege','2025-06-21 00:00:00.000000'),(11,'Marvel Rivals Cup','Marvel Rivals','2025-12-05 00:00:00.000000');
/*!40000 ALTER TABLE `turniri` ENABLE KEYS */;
UNLOCK TABLES;
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-08-16 19:23:25
