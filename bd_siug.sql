-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 16-02-2017 a las 07:06:36
-- Versión del servidor: 10.1.9-MariaDB
-- Versión de PHP: 5.6.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `bd_siug`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_access`
--

CREATE TABLE `tb_access` (
  `acc_id` bigint(20) NOT NULL,
  `acc_us_id` bigint(20) NOT NULL,
  `acc_date` date DEFAULT NULL,
  `acc_final_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_access`
--

INSERT INTO `tb_access` (`acc_id`, `acc_us_id`, `acc_date`, `acc_final_date`) VALUES
(1, 1010, '2015-10-25', '2017-02-07'),
(2, 1907, '2015-10-25', '2015-10-25');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_bill`
--

CREATE TABLE `tb_bill` (
  `bil_id` bigint(20) NOT NULL,
  `bil_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `bil_state` bigint(20) DEFAULT NULL,
  `bil_total` decimal(10,0) NOT NULL,
  `bil_sal_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_bill`
--

INSERT INTO `tb_bill` (`bil_id`, `bil_date`, `bil_state`, `bil_total`, `bil_sal_id`) VALUES
(1, '2017-01-08 21:19:31', 1, '35000', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_category`
--

CREATE TABLE `tb_category` (
  `cat_id` bigint(20) NOT NULL,
  `cat_name` varchar(200) NOT NULL,
  `cat_descript` varchar(200) NOT NULL,
  `cat_state` bigint(20) NOT NULL,
  `cat_createDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_category`
--

INSERT INTO `tb_category` (`cat_id`, `cat_name`, `cat_descript`, `cat_state`, `cat_createDate`) VALUES
(1, 'Repuestos', 'Productos relacionados con automoviles', 1, '2014-11-12 05:10:57');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_clients`
--

CREATE TABLE `tb_clients` (
  `cli_id` bigint(20) NOT NULL,
  `cli_nit` varchar(200) NOT NULL,
  `cli_name_company` varchar(200) NOT NULL,
  `cli_name_contact` varchar(200) NOT NULL,
  `cli_last_name_contact` varchar(200) NOT NULL,
  `cli_phone` varchar(200) NOT NULL,
  `cli_cel_phone` varchar(200) NOT NULL,
  `cli_email` varchar(200) NOT NULL,
  `cli_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `cli_state` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='tabla donde se almacenan los clientes';

--
-- Volcado de datos para la tabla `tb_clients`
--

INSERT INTO `tb_clients` (`cli_id`, `cli_nit`, `cli_name_company`, `cli_name_contact`, `cli_last_name_contact`, `cli_phone`, `cli_cel_phone`, `cli_email`, `cli_date`, `cli_state`) VALUES
(1, '123456-3', 'Grupo prisa', 'Jorge Armando', 'Diaz Mendoza', '9003340', '30090909', 'jorge.diaz@grupoprisa.com', '2016-05-23 04:05:25', 1),
(2, '345345345-6', 'Grupo babaria', 'ximena', 'caro', '4433355', '3003009090', 'ximana.caro@babaria.com', '2017-02-15 04:57:02', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_profile`
--

CREATE TABLE `tb_profile` (
  `prof_id` bigint(20) NOT NULL,
  `prof_name` varchar(100) NOT NULL,
  `prof_status` bigint(1) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_provider`
--

CREATE TABLE `tb_provider` (
  `pro_id` bigint(20) NOT NULL,
  `pro_name` varchar(200) NOT NULL,
  `pro_nit` varchar(200) NOT NULL,
  `pro_state` bigint(20) NOT NULL,
  `pro_contact` varchar(200) NOT NULL,
  `pro_phone` varchar(200) NOT NULL,
  `pro_email` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_provider`
--

INSERT INTO `tb_provider` (`pro_id`, `pro_name`, `pro_nit`, `pro_state`, `pro_contact`, `pro_phone`, `pro_email`) VALUES
(1, 'Chevrolet', '80115448-5', 1, 'Felipe Diaz', '3202174177', 'comercial1@chevrolet.com.co'),
(2, 'Corsa', '87878554', 1, 'Gilberto Gonzalez', '31602486221', 'comercial@corsacolombia.com'),
(3, 'Honda', '34348889-5', 1, 'Carlos Ramirez', '8893307', 'fuerza.comercial@hondacolombia.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_role`
--

CREATE TABLE `tb_role` (
  `role_id` bigint(20) NOT NULL,
  `role_descript` varchar(100) NOT NULL,
  `role_status` bigint(1) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_role`
--

INSERT INTO `tb_role` (`role_id`, `role_descript`, `role_status`) VALUES
(1, 'Administrador', 1),
(9, 'Empleado', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_role_user`
--

CREATE TABLE `tb_role_user` (
  `rolu_id` bigint(20) NOT NULL,
  `rolu_role_id` bigint(20) NOT NULL,
  `rolu_user_id` bigint(20) NOT NULL,
  `rolu_lastUpdate` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `rolu_status` bigint(1) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_role_user`
--

INSERT INTO `tb_role_user` (`rolu_id`, `rolu_role_id`, `rolu_user_id`, `rolu_lastUpdate`, `rolu_status`) VALUES
(1, 1, 1010, '2014-11-12 05:39:00', 1),
(2, 9, 1907, '2015-03-12 02:39:45', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_sale`
--

CREATE TABLE `tb_sale` (
  `sal_id` bigint(20) NOT NULL,
  `sal_userId` bigint(20) NOT NULL,
  `sal_cli_id` bigint(20) NOT NULL,
  `sal_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `sal_state` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_sale`
--

INSERT INTO `tb_sale` (`sal_id`, `sal_userId`, `sal_cli_id`, `sal_date`, `sal_state`) VALUES
(1, 1010, 1, '2017-01-08 21:19:31', 1),
(3, 1010, 2, '0000-00-00 00:00:00', 1),
(4, 1010, 1, '0000-00-00 00:00:00', 1),
(5, 1010, 1, '0000-00-00 00:00:00', 1),
(6, 1010, 1, '2017-02-15 19:40:52', 1),
(7, 1010, 2, '2017-02-16 03:52:21', 1),
(8, 1010, 1, '2017-02-16 04:46:20', 1),
(9, 1010, 1, '2017-02-16 05:11:20', 1),
(10, 1010, 1, '2017-02-16 05:11:40', 1),
(11, 1010, 1, '2017-02-16 05:46:09', 1),
(12, 1010, 1, '2017-02-16 05:52:36', 1),
(13, 1010, 1, '2017-02-16 05:56:40', 1),
(14, 1010, 1, '2017-02-16 06:04:13', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_sale_detail`
--

CREATE TABLE `tb_sale_detail` (
  `sade_id` bigint(20) NOT NULL,
  `sade_sale_id` bigint(20) NOT NULL,
  `sade_stock_id` bigint(20) NOT NULL,
  `sade_quantity` bigint(20) NOT NULL,
  `sade_subtotal` decimal(20,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

--
-- Volcado de datos para la tabla `tb_sale_detail`
--

INSERT INTO `tb_sale_detail` (`sade_id`, `sade_sale_id`, `sade_stock_id`, `sade_quantity`, `sade_subtotal`) VALUES
(1, 1, 1, 1, '35000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_state`
--

CREATE TABLE `tb_state` (
  `sta_id` bigint(20) NOT NULL,
  `sta_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_state`
--

INSERT INTO `tb_state` (`sta_id`, `sta_name`) VALUES
(1, 'Activo'),
(2, 'Inactivo'),
(3, 'N/A');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_stock`
--

CREATE TABLE `tb_stock` (
  `sto_id` bigint(20) NOT NULL,
  `sto_descript` varchar(200) NOT NULL,
  `sto_state` bigint(20) NOT NULL,
  `sto_avaible` bigint(20) NOT NULL,
  `sto_buyPrice` decimal(10,0) NOT NULL,
  `sto_salePrice` decimal(20,0) NOT NULL,
  `sto_url` varchar(200) DEFAULT NULL,
  `sto_provId` bigint(20) NOT NULL,
  `sto_category` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_stock`
--

INSERT INTO `tb_stock` (`sto_id`, `sto_descript`, `sto_state`, `sto_avaible`, `sto_buyPrice`, `sto_salePrice`, `sto_url`, `sto_provId`, `sto_category`) VALUES
(1, 'Parabrisas', 1, 33, '25000', '35000', '', 2, 1),
(2, 'Suspensión', 1, 25, '160000', '200000', 'ddd', 1, 1),
(3, 'Rines de lujo', 1, 12, '80000', '100000', 'pendiente', 3, 1),
(112, 'Turbo', 1, 30, '89000', '120000', 'Array', 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tb_users`
--

CREATE TABLE `tb_users` (
  `us_id` bigint(20) NOT NULL,
  `us_name` varchar(200) NOT NULL,
  `us_lastname` varchar(200) NOT NULL,
  `us_phone` varchar(200) NOT NULL,
  `us_email` varchar(200) NOT NULL,
  `us_pass` varchar(200) NOT NULL,
  `us_cityId` bigint(20) NOT NULL,
  `us_createDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `us_state` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Volcado de datos para la tabla `tb_users`
--

INSERT INTO `tb_users` (`us_id`, `us_name`, `us_lastname`, `us_phone`, `us_email`, `us_pass`, `us_cityId`, `us_createDate`, `us_state`) VALUES
(1010, 'Usuario', 'Administrador', '3003003000', 'administradorn@miapp.com', '1e48c4420b7073bc11916c6c1de226bb', 1, '2014-11-12 03:16:12', 1),
(1907, 'Usuario', 'Empleado', '2003040', 'prueba@miapp.com.co', '77369e37b2aa1404f416275183ab055f', 1, '2015-03-12 02:07:52', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `tb_access`
--
ALTER TABLE `tb_access`
  ADD PRIMARY KEY (`acc_id`),
  ADD KEY `acc_us_id` (`acc_us_id`);

--
-- Indices de la tabla `tb_bill`
--
ALTER TABLE `tb_bill`
  ADD PRIMARY KEY (`bil_id`),
  ADD KEY `bil_sal_Id` (`bil_sal_id`),
  ADD KEY `bil_state` (`bil_state`);

--
-- Indices de la tabla `tb_category`
--
ALTER TABLE `tb_category`
  ADD PRIMARY KEY (`cat_id`),
  ADD KEY `cat_state` (`cat_state`);

--
-- Indices de la tabla `tb_clients`
--
ALTER TABLE `tb_clients`
  ADD PRIMARY KEY (`cli_id`),
  ADD KEY `cli_state` (`cli_state`);

--
-- Indices de la tabla `tb_profile`
--
ALTER TABLE `tb_profile`
  ADD PRIMARY KEY (`prof_id`),
  ADD KEY `prof_status` (`prof_status`);

--
-- Indices de la tabla `tb_provider`
--
ALTER TABLE `tb_provider`
  ADD PRIMARY KEY (`pro_id`),
  ADD KEY `pro_state` (`pro_state`);

--
-- Indices de la tabla `tb_role`
--
ALTER TABLE `tb_role`
  ADD PRIMARY KEY (`role_id`),
  ADD KEY `role_status` (`role_status`);

--
-- Indices de la tabla `tb_role_user`
--
ALTER TABLE `tb_role_user`
  ADD PRIMARY KEY (`rolu_id`),
  ADD UNIQUE KEY `rolu_user_id` (`rolu_user_id`),
  ADD KEY `rolu_role_id` (`rolu_role_id`),
  ADD KEY `rolu_status` (`rolu_status`);

--
-- Indices de la tabla `tb_sale`
--
ALTER TABLE `tb_sale`
  ADD PRIMARY KEY (`sal_id`),
  ADD KEY `sal_userId` (`sal_userId`),
  ADD KEY `sal_state` (`sal_state`),
  ADD KEY `sal_cli_id` (`sal_cli_id`);

--
-- Indices de la tabla `tb_sale_detail`
--
ALTER TABLE `tb_sale_detail`
  ADD PRIMARY KEY (`sade_id`),
  ADD KEY `sade_sale_id` (`sade_sale_id`),
  ADD KEY `sade_stock_id` (`sade_stock_id`),
  ADD KEY `sade_quantity` (`sade_quantity`);

--
-- Indices de la tabla `tb_state`
--
ALTER TABLE `tb_state`
  ADD PRIMARY KEY (`sta_id`);

--
-- Indices de la tabla `tb_stock`
--
ALTER TABLE `tb_stock`
  ADD PRIMARY KEY (`sto_id`),
  ADD KEY `sto_provId` (`sto_provId`),
  ADD KEY `sto_category` (`sto_category`),
  ADD KEY `sto_state` (`sto_state`),
  ADD KEY `sto_avaible` (`sto_avaible`);

--
-- Indices de la tabla `tb_users`
--
ALTER TABLE `tb_users`
  ADD PRIMARY KEY (`us_id`),
  ADD KEY `us_state` (`us_state`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `tb_access`
--
ALTER TABLE `tb_access`
  MODIFY `acc_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT de la tabla `tb_bill`
--
ALTER TABLE `tb_bill`
  MODIFY `bil_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT de la tabla `tb_category`
--
ALTER TABLE `tb_category`
  MODIFY `cat_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT de la tabla `tb_clients`
--
ALTER TABLE `tb_clients`
  MODIFY `cli_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT de la tabla `tb_profile`
--
ALTER TABLE `tb_profile`
  MODIFY `prof_id` bigint(20) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT de la tabla `tb_provider`
--
ALTER TABLE `tb_provider`
  MODIFY `pro_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT de la tabla `tb_role`
--
ALTER TABLE `tb_role`
  MODIFY `role_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
--
-- AUTO_INCREMENT de la tabla `tb_role_user`
--
ALTER TABLE `tb_role_user`
  MODIFY `rolu_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT de la tabla `tb_sale`
--
ALTER TABLE `tb_sale`
  MODIFY `sal_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;
--
-- AUTO_INCREMENT de la tabla `tb_sale_detail`
--
ALTER TABLE `tb_sale_detail`
  MODIFY `sade_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT de la tabla `tb_state`
--
ALTER TABLE `tb_state`
  MODIFY `sta_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT de la tabla `tb_stock`
--
ALTER TABLE `tb_stock`
  MODIFY `sto_id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=113;
--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `tb_access`
--
ALTER TABLE `tb_access`
  ADD CONSTRAINT `tb_access_ibfk_1` FOREIGN KEY (`acc_us_id`) REFERENCES `tb_users` (`us_id`);

--
-- Filtros para la tabla `tb_bill`
--
ALTER TABLE `tb_bill`
  ADD CONSTRAINT `tb_bill_ibfk_1` FOREIGN KEY (`bil_state`) REFERENCES `tb_state` (`sta_id`),
  ADD CONSTRAINT `tb_bill_ibfk_2` FOREIGN KEY (`bil_sal_id`) REFERENCES `tb_sale` (`sal_id`);

--
-- Filtros para la tabla `tb_category`
--
ALTER TABLE `tb_category`
  ADD CONSTRAINT `tb_category_ibfk_1` FOREIGN KEY (`cat_state`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_clients`
--
ALTER TABLE `tb_clients`
  ADD CONSTRAINT `fk_tb_clients_tb_state` FOREIGN KEY (`cli_state`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_profile`
--
ALTER TABLE `tb_profile`
  ADD CONSTRAINT `tb_profile_ibfk_1` FOREIGN KEY (`prof_status`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_provider`
--
ALTER TABLE `tb_provider`
  ADD CONSTRAINT `tb_provider_ibfk_1` FOREIGN KEY (`pro_state`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_role`
--
ALTER TABLE `tb_role`
  ADD CONSTRAINT `tb_role_ibfk_1` FOREIGN KEY (`role_status`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_role_user`
--
ALTER TABLE `tb_role_user`
  ADD CONSTRAINT `tb_role_user_ibfk_1` FOREIGN KEY (`rolu_role_id`) REFERENCES `tb_role` (`role_id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `tb_role_user_ibfk_2` FOREIGN KEY (`rolu_user_id`) REFERENCES `tb_users` (`us_id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `tb_role_user_ibfk_3` FOREIGN KEY (`rolu_status`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_sale`
--
ALTER TABLE `tb_sale`
  ADD CONSTRAINT `fk_tb_sale_tb_clients` FOREIGN KEY (`sal_cli_id`) REFERENCES `tb_clients` (`cli_id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `tb_sale_ibfk_1` FOREIGN KEY (`sal_userId`) REFERENCES `tb_users` (`us_id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `tb_sale_ibfk_2` FOREIGN KEY (`sal_state`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_sale_detail`
--
ALTER TABLE `tb_sale_detail`
  ADD CONSTRAINT `tb_sale_detail_ibfk_1` FOREIGN KEY (`sade_sale_id`) REFERENCES `tb_sale` (`sal_id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `tb_sale_detail_ibfk_2` FOREIGN KEY (`sade_stock_id`) REFERENCES `tb_stock` (`sto_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_stock`
--
ALTER TABLE `tb_stock`
  ADD CONSTRAINT `tb_stock_ibfk_1` FOREIGN KEY (`sto_state`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `tb_stock_ibfk_2` FOREIGN KEY (`sto_provId`) REFERENCES `tb_provider` (`pro_id`) ON DELETE NO ACTION ON UPDATE CASCADE,
  ADD CONSTRAINT `tb_stock_ibfk_3` FOREIGN KEY (`sto_category`) REFERENCES `tb_category` (`cat_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Filtros para la tabla `tb_users`
--
ALTER TABLE `tb_users`
  ADD CONSTRAINT `tb_users_ibfk_1` FOREIGN KEY (`us_state`) REFERENCES `tb_state` (`sta_id`) ON DELETE NO ACTION ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
