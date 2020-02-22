-- phpMyAdmin SQL Dump
-- version 5.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 22, 2020 at 06:52 AM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `exidnumbers`
--

-- --------------------------------------------------------

--
-- Stand-in structure for view `latest_syncs`
-- (See below for the actual view)
--
CREATE TABLE `latest_syncs` (
`sync_id` int(11)
,`new_count` int(11)
,`their_id` varchar(255)
,`site` varchar(50)
,`type` varchar(50)
,`title` varchar(255)
,`url` varchar(255)
,`property_id` int(11)
,`property` varchar(50)
,`current_count` int(11)
,`current_daily` int(11)
,`new_daily` decimal(17,4)
);

-- --------------------------------------------------------

--
-- Table structure for table `milestones`
--

CREATE TABLE `milestones` (
  `id` int(11) NOT NULL,
  `site` varchar(50) NOT NULL,
  `property` varchar(50) NOT NULL,
  `count` int(11) NOT NULL,
  `min_delta` int(11) NOT NULL,
  `max_delta` int(11) NOT NULL,
  `template` varchar(160) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `monitored_items`
--

CREATE TABLE `monitored_items` (
  `id` int(11) NOT NULL,
  `site` varchar(50) NOT NULL,
  `their_id` varchar(255) NOT NULL,
  `type` varchar(50) NOT NULL,
  `title` varchar(255) NOT NULL,
  `url` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `monitored_properties`
--

CREATE TABLE `monitored_properties` (
  `id` int(11) NOT NULL,
  `updated` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `monitored_id` int(11) NOT NULL,
  `property` varchar(50) NOT NULL,
  `count` int(11) DEFAULT NULL,
  `daily` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `queued_tweets`
--

CREATE TABLE `queued_tweets` (
  `id` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  `text` varchar(280) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `sync_history`
--

CREATE TABLE `sync_history` (
  `id` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  `property_id` int(11) NOT NULL,
  `count` int(11) NOT NULL,
  `processed` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Structure for view `latest_syncs`
--
DROP TABLE IF EXISTS `latest_syncs`;

CREATE VIEW `latest_syncs`  AS  select `s`.`id` AS `sync_id`,`s`.`count` AS `new_count`,`i`.`their_id` AS `their_id`,`i`.`site` AS `site`,`i`.`type` AS `type`,`i`.`title` AS `title`,`i`.`url` AS `url`,`p`.`id` AS `property_id`,`p`.`property` AS `property`,`p`.`count` AS `current_count`,`p`.`daily` AS `current_daily`,(select (`s`.`count` - `sync_history`.`count`) / timestampdiff(HOUR,`sync_history`.`timestamp`,`s`.`timestamp`) * 24 from `sync_history` where `s`.`property_id` = `sync_history`.`property_id` and `sync_history`.`timestamp` > cast(current_timestamp() - interval 30 day as datetime) order by `sync_history`.`timestamp` limit 1) AS `new_daily` from ((`sync_history` `s` left join `monitored_properties` `p` on(`p`.`id` = `s`.`property_id`)) left join `monitored_items` `i` on(`i`.`id` = `p`.`monitored_id`)) where `s`.`processed` = 0 ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `milestones`
--
ALTER TABLE `milestones`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `monitored_items`
--
ALTER TABLE `monitored_items`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `monitored_properties`
--
ALTER TABLE `monitored_properties`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `queued_tweets`
--
ALTER TABLE `queued_tweets`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sync_history`
--
ALTER TABLE `sync_history`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `milestones`
--
ALTER TABLE `milestones`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `monitored_items`
--
ALTER TABLE `monitored_items`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `monitored_properties`
--
ALTER TABLE `monitored_properties`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `queued_tweets`
--
ALTER TABLE `queued_tweets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `sync_history`
--
ALTER TABLE `sync_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
