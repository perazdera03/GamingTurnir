-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 07, 2026 at 10:46 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `gamingturnir`
--

-- --------------------------------------------------------

--
-- Table structure for table `clanovitima`
--

CREATE TABLE `clanovitima` (
  `ClanTimaId` int(11) NOT NULL,
  `KorisnikId` int(11) NOT NULL,
  `TimId` int(11) NOT NULL,
  `Uloga` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `korisnici`
--

CREATE TABLE `korisnici` (
  `KorisnikId` int(11) NOT NULL,
  `Username` longtext NOT NULL,
  `PasswordHash` longtext NOT NULL,
  `Rola` int(11) NOT NULL,
  `DatumRegisdtracije` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `korisnici`
--

INSERT INTO `korisnici` (`KorisnikId`, `Username`, `PasswordHash`, `Rola`, `DatumRegisdtracije`) VALUES
(1, 'stefan', 'AQAAAAIAAYagAAAAEOAU1KOz0ZXfhVI+ZFqtjm9RQCY4ik21spAVrjhhCxSfIfaQNBQdyA53g158y/TZww==', 1, '2026-06-27 13:56:34.332908'),
(2, 'petar', 'AQAAAAIAAYagAAAAEAK8+wneJXURz2P0MqSpMwWlYeUsklY1WCfi3ab4Q2uDO16Wlq7NS4/XmMXi/Coy2A==', 0, '2026-06-27 13:59:35.917766'),
(3, 'vanja', 'AQAAAAIAAYagAAAAELwkrrZrJOl8eAJC6vmL8PMIxWVSMT11kt10IcxaXOvR9pQTkJh0DakvHY9DClKiwQ==', 2, '2026-06-27 14:02:13.941069'),
(6, 'marko', 'AQAAAAIAAYagAAAAEJG5WYtiiD73yJyP9RdCbfYq2GWiCAGt4pg8vGwcoBkx4v7sP6rJpmshtjcWN3nVmQ==', 1, '2026-06-27 15:31:13.646221'),
(8, 'aleksa', 'AQAAAAIAAYagAAAAEDAafrb+66U9OtfrkkzFvXTWpXlOeXm5nkm7SQOz6vQ3/D18ayxZc2UVgmF+sji8Mg==', 0, '2026-07-06 21:20:07.930346');

-- --------------------------------------------------------

--
-- Table structure for table `mecevi`
--

CREATE TABLE `mecevi` (
  `MecId` int(11) NOT NULL,
  `TurnirId` int(11) NOT NULL,
  `Tim1Id` int(11) NOT NULL,
  `Tim2Id` int(11) NOT NULL,
  `RezultatTim1` int(11) DEFAULT NULL,
  `RezultatTim2` int(11) DEFAULT NULL,
  `DatumMeca` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `mecevi`
--

INSERT INTO `mecevi` (`MecId`, `TurnirId`, `Tim1Id`, `Tim2Id`, `RezultatTim1`, `RezultatTim2`, `DatumMeca`) VALUES
(1, 1, 1, 2, 13, 10, '2024-03-05 00:00:00.000000'),
(2, 1, 3, 4, 16, 14, '2024-03-06 00:00:00.000000'),
(3, 2, 1, 5, 13, 10, '2024-05-16 00:00:00.000000'),
(4, 5, 2, 3, 2, 0, '2025-06-12 00:00:00.000000'),
(5, 6, 4, 5, 3, 2, '2025-11-03 00:00:00.000000');

-- --------------------------------------------------------

--
-- Table structure for table `timovi`
--

CREATE TABLE `timovi` (
  `TimId` int(11) NOT NULL,
  `Naziv` longtext NOT NULL,
  `Opis` longtext NOT NULL,
  `DatumOsnivanja` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `timovi`
--

INSERT INTO `timovi` (`TimId`, `Naziv`, `Opis`, `DatumOsnivanja`) VALUES
(1, 'Team Alpha', 'Profesionalni tim iz Srbije', '2022-01-15 00:00:00.000000'),
(2, 'Dark Force', 'Tim specijalizovan za FPS igrice', '2021-06-10 00:00:00.000000'),
(3, 'Storm Riders', 'Mladi talenti iz regiona', '2023-03-20 00:00:00.000000'),
(4, 'Iron Wolf', 'Iskusni veterani gaming scene', '2020-11-05 00:00:00.000000'),
(5, 'Nova Squad', 'Novi tim sa velikim potencijalom', '2024-02-28 00:00:00.000000');

-- --------------------------------------------------------

--
-- Table structure for table `turniri`
--

CREATE TABLE `turniri` (
  `TurnirId` int(11) NOT NULL,
  `Naziv` longtext NOT NULL,
  `Igrica` longtext NOT NULL,
  `DatumPocetka` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `turniri`
--

INSERT INTO `turniri` (`TurnirId`, `Naziv`, `Igrica`, `DatumPocetka`) VALUES
(1, 'CS2 Open 2024', 'CS2', '2024-03-01 00:00:00.000000'),
(2, 'Valorant Championship', 'Valorant', '2024-05-15 00:00:00.000000'),
(4, 'League of Legends Cup', 'League of Legends', '2025-07-20 00:00:00.000000'),
(5, 'Dota 2 Masters', 'Dota 2', '2025-06-10 00:00:00.000000'),
(6, 'FIFA Pro League', 'FIFA 25', '2025-11-01 00:00:00.000000');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20260626210657_InitialMigration', '9.0.0');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `clanovitima`
--
ALTER TABLE `clanovitima`
  ADD PRIMARY KEY (`ClanTimaId`),
  ADD KEY `IX_ClanoviTima_KorisnikId` (`KorisnikId`),
  ADD KEY `IX_ClanoviTima_TimId` (`TimId`);

--
-- Indexes for table `korisnici`
--
ALTER TABLE `korisnici`
  ADD PRIMARY KEY (`KorisnikId`);

--
-- Indexes for table `mecevi`
--
ALTER TABLE `mecevi`
  ADD PRIMARY KEY (`MecId`),
  ADD KEY `IX_Mecevi_Tim1Id` (`Tim1Id`),
  ADD KEY `IX_Mecevi_Tim2Id` (`Tim2Id`),
  ADD KEY `IX_Mecevi_TurnirId` (`TurnirId`);

--
-- Indexes for table `timovi`
--
ALTER TABLE `timovi`
  ADD PRIMARY KEY (`TimId`);

--
-- Indexes for table `turniri`
--
ALTER TABLE `turniri`
  ADD PRIMARY KEY (`TurnirId`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `clanovitima`
--
ALTER TABLE `clanovitima`
  MODIFY `ClanTimaId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `korisnici`
--
ALTER TABLE `korisnici`
  MODIFY `KorisnikId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `mecevi`
--
ALTER TABLE `mecevi`
  MODIFY `MecId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `timovi`
--
ALTER TABLE `timovi`
  MODIFY `TimId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `turniri`
--
ALTER TABLE `turniri`
  MODIFY `TurnirId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `clanovitima`
--
ALTER TABLE `clanovitima`
  ADD CONSTRAINT `FK_ClanoviTima_Korisnici_KorisnikId` FOREIGN KEY (`KorisnikId`) REFERENCES `korisnici` (`KorisnikId`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_ClanoviTima_Timovi_TimId` FOREIGN KEY (`TimId`) REFERENCES `timovi` (`TimId`) ON DELETE CASCADE;

--
-- Constraints for table `mecevi`
--
ALTER TABLE `mecevi`
  ADD CONSTRAINT `FK_Mecevi_Timovi_Tim1Id` FOREIGN KEY (`Tim1Id`) REFERENCES `timovi` (`TimId`),
  ADD CONSTRAINT `FK_Mecevi_Timovi_Tim2Id` FOREIGN KEY (`Tim2Id`) REFERENCES `timovi` (`TimId`),
  ADD CONSTRAINT `FK_Mecevi_Turniri_TurnirId` FOREIGN KEY (`TurnirId`) REFERENCES `turniri` (`TurnirId`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
