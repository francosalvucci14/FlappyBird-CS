-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Creato il: Set 23, 2020 alle 17:31
-- Versione del server: 10.4.11-MariaDB
-- Versione PHP: 7.4.5

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `flappy-bird`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `recordxuser`
--

CREATE TABLE `recordxuser` (
  `RID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `RecordN` int(11) NOT NULL,
  `Username` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dump dei dati per la tabella `recordxuser`
--

INSERT INTO `recordxuser` (`RID`, `UserID`, `RecordN`, `Username`) VALUES
(4, 3, 75, 'admin'),
(6, 2, 25, 'Franco'),
(11, 14, 25, 'user'),
(15, 17, 18, 'super');

-- --------------------------------------------------------

--
-- Struttura della tabella `userflappy`
--

CREATE TABLE `userflappy` (
  `UID` int(11) NOT NULL,
  `Name` text NOT NULL,
  `Password` text NOT NULL,
  `email` text NOT NULL,
  `Record` int(11) NOT NULL,
  `Admin` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dump dei dati per la tabella `userflappy`
--

INSERT INTO `userflappy` (`UID`, `Name`, `Password`, `email`, `Record`, `Admin`) VALUES
(2, 'Franco', 'Franco', 'testFranco@test.com', 25, 1),
(3, 'admin', 'secret', 'francosalvucci@yahoo.com', 75, 1),
(14, 'user', 'User', 'user@user.com', 25, 1),
(17, 'super', 'Superv1sor', 'francosalvucci14@gmail.com', 18, 1);

--
-- Indici per le tabelle scaricate
--

--
-- Indici per le tabelle `recordxuser`
--
ALTER TABLE `recordxuser`
  ADD PRIMARY KEY (`RID`),
  ADD KEY `UserID` (`UserID`);

--
-- Indici per le tabelle `userflappy`
--
ALTER TABLE `userflappy`
  ADD PRIMARY KEY (`UID`);

--
-- AUTO_INCREMENT per le tabelle scaricate
--

--
-- AUTO_INCREMENT per la tabella `recordxuser`
--
ALTER TABLE `recordxuser`
  MODIFY `RID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT per la tabella `userflappy`
--
ALTER TABLE `userflappy`
  MODIFY `UID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- Limiti per le tabelle scaricate
--

--
-- Limiti per la tabella `recordxuser`
--
ALTER TABLE `recordxuser`
  ADD CONSTRAINT `recordxuser_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `userflappy` (`UID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
