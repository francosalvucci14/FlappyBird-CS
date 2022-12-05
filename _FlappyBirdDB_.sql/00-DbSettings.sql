-- phpMyAdmin SQL Dump
    -- version 5.0.2
    -- https://www.phpmyadmin.net/
    --

    -- Host: 127.0.0.1
    -- Creato il: Set 23, 2020 alle 17:31
    -- Versione del server: 10.4.11-MariaDB
    -- Versione PHP: 7.4.5
SET
    SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION
    ;
SET
    time_zone = "+00:00";
    /*!40101
SET
    @OLD_CHARACTER_SET_CLIENT = @@CHARACTER_SET_CLIENT */;
    /*!40101
SET
    @OLD_CHARACTER_SET_RESULTS = @@CHARACTER_SET_RESULTS */;
    /*!40101
SET
    @OLD_COLLATION_CONNECTION = @@COLLATION_CONNECTION */;
    /*!40101
SET NAMES
    utf8mb4 */;
CREATE DATABASE : flappy - bird
-- --------------------------------------------------------
--

-- Struttura della tabella recordxuser
--

CREATE TABLE recordxuser(
    RID INT(11) NOT NULL,
    UserID INT(11) NOT NULL,
    RecordN INT(11) NOT NULL,
    Username TEXT NOT NULL
) ENGINE = INNODB DEFAULT CHARSET = utf8mb4;
-- --------------------------------------------------------
--

-- Struttura della tabella userflappy
--

CREATE TABLE userflappy(
    UID INT(11) NOT NULL,
    Name TEXT NOT NULL,
    Password TEXT NOT NULL,
    email TEXT NOT NULL,
    Record INT(11) NOT NULL,
    Admin INT(11) NOT NULL
) ENGINE = INNODB DEFAULT CHARSET = utf8mb4;
--

-- Struttura della tabella version
--

CREATE TABLE version(
    version1 INT(11) NOT NULL,
    version2 INT(11) NOT NULL,
    build1 INT(11) NOT NULL,
    build2 INT(11) NOT NULL
) ENGINE = INNODB DEFAULT CHARSET = utf8mb4 COMMENT = tabella per verificare la versione del software;
--

-- Dump dei dati per la tabella version
--

INSERT INTO version(
    version1,
    version2,
    build1,
    build2
)
VALUES(2, 1, 1, 0);
--

-- Indici per le tabelle recordxuser
--

ALTER TABLE
    recordxuser ADD PRIMARY KEY(RID),
    ADD KEY UserID(UserID);
    --

    -- Indici per le tabelle userflappy
    --

ALTER TABLE
    userflappy ADD PRIMARY KEY(UID);
    --

    -- AUTO_INCREMENT per la tabella recordxuser
    --

ALTER TABLE
    recordxuser MODIFY RID INT(11) NOT NULL AUTO_INCREMENT,
    AUTO_INCREMENT = 16;
    --

    -- AUTO_INCREMENT per la tabella userflappy
    --

ALTER TABLE
    userflappy MODIFY UID INT(11) NOT NULL AUTO_INCREMENT,
    AUTO_INCREMENT = 21;
    --

    -- Limiti per la tabella recordxuser
    --

ALTER TABLE
    recordxuser ADD CONSTRAINT recordxuser_ibfk_1 FOREIGN KEY(UserID) REFERENCES userflappy(UID);
COMMIT
    ;
    /*!40101
SET
    CHARACTER_SET_CLIENT = @OLD_CHARACTER_SET_CLIENT */;
    /*!40101
SET
    CHARACTER_SET_RESULTS = @OLD_CHARACTER_SET_RESULTS */;
    /*!40101
SET
    COLLATION_CONNECTION = @OLD_COLLATION_CONNECTION */;
    --

    -- Indici per le tabelle version
    --

ALTER TABLE
    version ADD UNIQUE KEY version1(version1);
COMMIT
    ;