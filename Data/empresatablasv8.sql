/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE DATABASE IF NOT EXISTS `agenda_empresa_user4` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `agenda_empresa_user4`;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `ActualizarUsuario`(
    IN p_IdUsuario INT,
    IN p_NuevoNombreUsuario VARCHAR(50),
    IN p_NuevaClave VARCHAR(255),
    IN p_NuevoIdPersona INT,
    IN p_NuevoIdRol INT,
    IN p_NuevoActivo VARCHAR(2)
)
BEGIN
    -- Verificar si el nuevo nombre de usuario ya existe
    IF EXISTS (SELECT 1 FROM usuario WHERE NombreUsuario = p_NuevoNombreUsuario AND IdUsuario <> p_IdUsuario) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El nuevo nombre de usuario ya está en uso.';
    ELSE
        -- Actualizar información del usuario
        UPDATE usuario
        SET
            NombreUsuario = p_NuevoNombreUsuario,
            ClaveHash = SHA2(p_NuevaClave, 256),
            IdPersona = p_NuevoIdPersona,
            IdRol = p_NuevoIdRol,
            Activo = p_NuevoActivo
        WHERE IdUsuario = p_IdUsuario;
    END IF;
END//
DELIMITER ;

CREATE TABLE IF NOT EXISTS `agendamientos` (
  `IdAgendamiento` int NOT NULL AUTO_INCREMENT,
  `IdCliente` int DEFAULT NULL,
  `Estado` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT 'V',
  `FechaHora` datetime DEFAULT NULL,
  PRIMARY KEY (`IdAgendamiento`),
  KEY `Agendamientos_ibfk_1` (`IdCliente`),
  CONSTRAINT `Agendamientos_ibfk_1` FOREIGN KEY (`IdCliente`) REFERENCES `personas` (`IdPersona`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `agendamientos` (`IdAgendamiento`, `IdCliente`, `Estado`, `FechaHora`) VALUES
	(1, 2, 'v', '2023-12-03 20:16:12');

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `AgendarCita`(
    IN p_IdProfesional INT,
    IN p_IdHorario INT,
    IN p_IdCliente INT,
    IN p_IdSedeAgendada INT,
    IN p_IdServicioAgendado INT
)
BEGIN
    -- Verificar disponibilidad del horario
    DECLARE disponibilidadHorario CHAR(3);
    SELECT Disponibilidad INTO disponibilidadHorario FROM horarios WHERE IdHorario = p_IdHorario;

    IF disponibilidadHorario = 'v' THEN
        -- Insertar en la tabla agendas si el horario está disponible
        INSERT INTO agendas (IdProfesional, IdHorario, IdCliente, IdSedeAgendada, IdServicioAgendado)
        VALUES (p_IdProfesional, p_IdHorario, p_IdCliente, p_IdSedeAgendada, p_IdServicioAgendado);
    ELSE
        -- Puedes manejar la lógica para horario no disponible, por ejemplo, lanzar un error o devolver un código específico.
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El horario no está disponible.';
    END IF;
END//
DELIMITER ;

CREATE TABLE IF NOT EXISTS `agendas` (
  `IdAgenda` int NOT NULL AUTO_INCREMENT,
  `IdSede` int DEFAULT NULL,
  `IdServicio` int DEFAULT NULL,
  `IdHorario` int DEFAULT NULL,
  `IdProfesional` int DEFAULT NULL,
  `IdSedeAgendada` int DEFAULT NULL,
  `IdServicioAgendado` int DEFAULT NULL,
  PRIMARY KEY (`IdAgenda`),
  KEY `Agendas_ibfk_1` (`IdSede`),
  KEY `Agendas_ibfk_2` (`IdServicio`),
  KEY `Agendas_ibfk_3` (`IdHorario`),
  KEY `Agendas_ibfk_4` (`IdProfesional`),
  CONSTRAINT `Agendas_ibfk_1` FOREIGN KEY (`IdSede`) REFERENCES `sedes` (`IdSede`),
  CONSTRAINT `Agendas_ibfk_2` FOREIGN KEY (`IdServicio`) REFERENCES `servicios` (`IdServicio`),
  CONSTRAINT `Agendas_ibfk_3` FOREIGN KEY (`IdHorario`) REFERENCES `horarios` (`IdHorario`),
  CONSTRAINT `Agendas_ibfk_4` FOREIGN KEY (`IdProfesional`) REFERENCES `personas` (`IdPersona`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `agendas` (`IdAgenda`, `IdSede`, `IdServicio`, `IdHorario`, `IdProfesional`, `IdSedeAgendada`, `IdServicioAgendado`) VALUES
	(1, 5, 13, 1, 2, 5, 13);

CREATE TABLE IF NOT EXISTS `agenda_agendamiento` (
  `IdAgenda` int NOT NULL,
  `IdAgendamiento` int NOT NULL,
  PRIMARY KEY (`IdAgenda`,`IdAgendamiento`),
  KEY `FK_AgendaAgendamiento_Agendamiento` (`IdAgendamiento`),
  CONSTRAINT `FK_AgendaAgendamiento_Agenda` FOREIGN KEY (`IdAgenda`) REFERENCES `agendas` (`IdAgenda`),
  CONSTRAINT `FK_AgendaAgendamiento_Agendamiento` FOREIGN KEY (`IdAgendamiento`) REFERENCES `agendamientos` (`IdAgendamiento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `agenda_agendamiento` (`IdAgenda`, `IdAgendamiento`) VALUES
	(1, 1);

CREATE TABLE IF NOT EXISTS `asistencias` (
  `IdAsistencia` int NOT NULL AUTO_INCREMENT,
  `EstadoAsistencia` varchar(1) DEFAULT NULL,
  `IdAgendamiento` int DEFAULT NULL,
  PRIMARY KEY (`IdAsistencia`),
  KEY `Asistencias_ibfk_1` (`IdAgendamiento`),
  CONSTRAINT `Asistencias_ibfk_1` FOREIGN KEY (`IdAgendamiento`) REFERENCES `agendamientos` (`IdAgendamiento`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE IF NOT EXISTS `cancelaciones` (
  `IdCancelacion` int NOT NULL AUTO_INCREMENT,
  `FechaHora` datetime DEFAULT NULL,
  `Motivo` varchar(200) DEFAULT NULL,
  `IdAgendamiento` int DEFAULT NULL,
  PRIMARY KEY (`IdCancelacion`),
  KEY `Cancelaciones_ibfk_1` (`IdAgendamiento`),
  CONSTRAINT `Cancelaciones_ibfk_1` FOREIGN KEY (`IdAgendamiento`) REFERENCES `agendamientos` (`IdAgendamiento`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `CancelarCita`(IN p_IdAgenda INT, IN p_Motivo VARCHAR(200))
BEGIN
    -- Verificar el estado actual de la agenda antes de la cancelación
    DECLARE estadoActual VARCHAR(50);
    SELECT Estado INTO estadoActual FROM agendas WHERE IdAgenda = p_IdAgenda;

    IF estadoActual <> 'Cancelada' THEN
        -- Actualizar el estado de la agenda
        UPDATE agendas
        SET Estado = 'Cancelada'
        WHERE IdAgenda = p_IdAgenda;

        -- Registrar la cancelación
        INSERT INTO cancelaciones (FechaHora, Motivo, IdAgenda)
        VALUES (NOW(), p_Motivo, p_IdAgenda);
    ELSE
        -- Puedes manejar la lógica para la cancelación de una cita ya cancelada, por ejemplo, lanzar un error o devolver un código específico.
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La cita ya está cancelada.';
    END IF;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `EliminarUsuario`(
    IN p_IdUsuario INT
)
BEGIN
    -- Eliminar usuario
    DELETE FROM usuario WHERE IdUsuario = p_IdUsuario;
END//
DELIMITER ;

CREATE TABLE IF NOT EXISTS `horarios` (
  `IdHorario` int NOT NULL AUTO_INCREMENT,
  `HoraInicio` time DEFAULT NULL,
  `HoraFin` time DEFAULT NULL,
  `Disponibilidad` char(3) DEFAULT 'v',
  `FechaInicio` date DEFAULT NULL,
  `FechaFin` date DEFAULT NULL,
  PRIMARY KEY (`IdHorario`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `horarios` (`IdHorario`, `HoraInicio`, `HoraFin`, `Disponibilidad`, `FechaInicio`, `FechaFin`) VALUES
	(1, '09:00:00', '10:00:00', 'v', '2023-12-01', '2023-12-01'),
	(3, '11:30:00', '12:30:00', 'v', '2023-12-01', '2023-12-01'),
	(4, '09:00:00', '10:00:00', 'v', '2023-12-01', '2023-12-15'),
	(5, '14:00:00', '15:00:00', 'v', '2023-12-01', '2023-12-15'),
	(6, '14:00:00', '15:30:00', 'v', '2023-12-01', '2023-12-01'),
	(8, '16:30:00', '18:00:00', 'v', '2023-12-01', '2023-12-15'),
	(9, '09:00:00', '10:00:00', 'v', '2023-12-01', '2023-12-01'),
	(10, '14:00:00', '15:30:00', 'v', '2023-12-01', '2023-12-01'),
	(11, '11:30:00', '12:30:00', 'v', '2023-12-01', '2023-12-01');

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `IniciarSesion`(IN InNombreUsuario VARCHAR(50), IN InClave VARCHAR(255))
BEGIN
    DECLARE hashedPassword VARCHAR(255);
    SELECT ClaveHash INTO hashedPassword FROM usuario WHERE NombreUsuario = InNombreUsuario;

    -- Verificar la contraseña hash
    IF hashedPassword IS NOT NULL AND hashedPassword = SHA2(InClave, 256) THEN
        -- La contraseña es válida
        SELECT * FROM usuario WHERE NombreUsuario = InNombreUsuario;
    ELSE
        -- La contraseña no es válida o el usuario no existe
        SELECT 'Error' AS Status;
    END IF;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `MostrarCitasCliente`(IN p_IdCliente INT)
BEGIN
    SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdProfesional
    FROM agendas A
    INNER JOIN horarios H ON A.IdHorario = H.IdHorario
    INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio
    WHERE A.IdCliente = p_IdCliente;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `MostrarCitasProfesional`(IN p_IdProfesional INT)
BEGIN
    SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdCliente
    FROM agendas A
    INNER JOIN horarios H ON A.IdHorario = H.IdHorario
    INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio
    WHERE A.IdProfesional = p_IdProfesional;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `MostrarDisponibilidadHorarios`(IN p_IdServicio INT, IN p_IdSede INT)
BEGIN
    SELECT H.IdHorario, H.HoraInicio, H.HoraFin
    FROM horarios H
    LEFT JOIN agendas A ON H.IdHorario = A.IdHorario
    WHERE A.IdAgenda IS NULL
    AND H.Disponibilidad = 'v'
    AND A.IdServicioAgendado = p_IdServicio
    AND A.IdSedeAgendada = p_IdSede;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `MostrarDisponibilidadHorariosSede`(IN p_IdSede INT)
BEGIN
    SELECT H.IdHorario, H.HoraInicio, H.HoraFin, H.FechaInicio, H.FechaFin
    FROM horarios H
    LEFT JOIN agendas A ON H.IdHorario = A.IdHorario
    WHERE A.IdAgenda IS NULL
    AND H.Disponibilidad = 'v'
    AND A.IdSedeAgendada = p_IdSede;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `MostrarDisponibilidadHorariosServicio`(IN p_IdServicio INT)
BEGIN
    SELECT H.IdHorario, H.HoraInicio, H.HoraFin, H.FechaInicio, H.FechaFin
    FROM horarios H
    LEFT JOIN agendas A ON H.IdHorario = A.IdHorario
    WHERE A.IdAgenda IS NULL
    AND H.Disponibilidad = 'v'
    AND A.IdServicioAgendado = p_IdServicio;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `ObtenerDetallesAgenda`(IN p_IdAgenda INT)
BEGIN
    SELECT
        A.IdAgenda,
        A.Estado,
        P.PrimerNombre AS ProfesionalNombre,
        S.Direccion AS SedeDireccion,
        Se.Nombre AS ServicioNombre,
        H.HoraInicio AS HorarioInicio,
        H.HoraFin AS HorarioFin
    FROM
        agendas A
    INNER JOIN profesionales P ON A.IdProfesional = P.IdPersona
    INNER JOIN sedes S ON A.IdSede = S.IdSede
    INNER JOIN servicios Se ON A.IdServicio = Se.IdServicio
    INNER JOIN horarios H ON A.IdHorario = H.IdHorario
    WHERE
        A.IdAgenda = p_IdAgenda;
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `ObtenerDetallesAgendaPorId`(IN p_IdAgenda INT)
BEGIN
    SELECT
        A.IdAgenda,
        A.Estado,
        CONCAT(P.PrimerNombre, ' ', P.PrimerApellido) AS ProfesionalNombre,
        S.Nombre AS SedeNombre,
        Se.Nombre AS ServicioNombre,
        CONCAT(C.PrimerNombre, ' ', C.PrimerApellido) AS ClienteNombre,
        H.FechaInicio AS FechaInicio,
        H.HoraInicio AS HoraInicio
    FROM
        agendas A
    INNER JOIN profesionales P ON A.IdProfesional = P.IdPersona
    INNER JOIN sedes S ON A.IdSede = S.IdSede
    INNER JOIN servicios Se ON A.IdServicio = Se.IdServicio
    INNER JOIN horarios H ON A.IdHorario = H.IdHorario
    LEFT JOIN personas C ON A.IdCliente = C.IdPersona
    WHERE
        A.IdAgenda = p_IdAgenda;
END//
DELIMITER ;

CREATE TABLE IF NOT EXISTS `permisos` (
  `IdPermiso` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`IdPermiso`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `permisos` (`IdPermiso`, `Nombre`) VALUES
	(11, 'Crear Cita'),
	(12, 'Cancelar Cita'),
	(13, 'Ver Agenda');

CREATE TABLE IF NOT EXISTS `personas` (
  `IdPersona` int NOT NULL AUTO_INCREMENT,
  `Identificacion` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PrimerNombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SegundoNombre` varchar(50) DEFAULT NULL,
  `PrimerApellido` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SegundoApellido` varchar(50) DEFAULT NULL,
  `Sexo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Correo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT 'Correo@mail.com',
  `Telefono` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT '00000000',
  `Estado` varchar(50) DEFAULT 'v',
  PRIMARY KEY (`IdPersona`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `personas` (`IdPersona`, `Identificacion`, `PrimerNombre`, `SegundoNombre`, `PrimerApellido`, `SegundoApellido`, `Sexo`, `Correo`, `Telefono`, `Estado`) VALUES
	(2, '987654321', 'Ana', NULL, 'Gómez', NULL, 'Femenino', 'ana@example.com', '555-5678', 'Activo'),
	(4, '123456789', 'Juan', NULL, 'Pérez', NULL, 'Masculino', 'juan@example.com', '555-1234', 'Activo'),
	(5, '987654321', 'María', NULL, 'Gómez', NULL, 'F', 'maria.gomez@mail.com', '987654321', 'v'),
	(6, '111222333', 'Carlos', NULL, 'Martínez', NULL, 'Masculino', 'carlos@example.com', '555-9876', 'Inactivo'),
	(7, '123456789', 'Juan', NULL, 'Pérez', NULL, 'M', 'juan.perez@mail.com', '123456789', 'v'),
	(9, '111222333', 'Carlos', NULL, 'Rodríguez', NULL, 'M', 'carlos.rodriguez@mail.com', '111222333', 'v'),
	(10, '123456789', 'Juan', 'Pablo', 'Gómez', 'Pérez', 'Masculino', 'juan@gmail.com', '123456789', 'v'),
	(11, '123456789', 'Juan', 'Pablo', 'Gómez', 'Pérez', 'Masculino', 'juan@gmail.com', '123456789', 'v'),
	(12, '111111111', 'Juan', NULL, 'Pérez', NULL, 'Masculino', 'juan@gmail.com', '123456789', 'v'),
	(13, '222222222', 'María', NULL, 'López', NULL, 'Femenino', 'maria@gmail.com', '987654321', 'v'),
	(14, '333333333', 'Carlos', NULL, 'Rodríguez', NULL, 'Masculino', 'carlos@gmail.com', '555555555', 'v');

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `RegistrarPersonaPorAdmin`(
    IN p_PrimerNombre VARCHAR(50),
    IN p_SegundoNombre VARCHAR(50),
    IN p_PrimerApellido VARCHAR(50),
    IN p_SegundoApellido VARCHAR(50),
    IN p_Sexo VARCHAR(50),
    IN p_Correo VARCHAR(50),
    IN p_Telefono VARCHAR(50),
    IN p_Estado VARCHAR(50)
)
BEGIN
    -- Insertar nueva persona
    INSERT INTO personas (
        PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido,
        Sexo, Correo, Telefono, Estado
    )
    VALUES (
        p_PrimerNombre, p_SegundoNombre, p_PrimerApellido, p_SegundoApellido,
        p_Sexo, p_Correo, p_Telefono, p_Estado
    );
END//
DELIMITER ;

DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `RegistrarUsuario`(
    IN p_NombreUsuario VARCHAR(50),
    IN p_Clave VARCHAR(255),
    IN p_IdPersona INT,
    IN p_IdRol INT,
    IN p_Activo VARCHAR(2)
)
BEGIN
    -- Verificar si el nombre de usuario ya existe
    IF EXISTS (SELECT 1 FROM usuario WHERE NombreUsuario = p_NombreUsuario) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El nombre de usuario ya está en uso.';
    ELSE
        -- Insertar nuevo usuario
        INSERT INTO usuario (NombreUsuario, ClaveHash, IdPersona, IdRol, Activo)
        VALUES (p_NombreUsuario, SHA2(p_Clave, 256), p_IdPersona, p_IdRol, p_Activo);
    END IF;
END//
DELIMITER ;

CREATE TABLE IF NOT EXISTS `roles` (
  `IdRol` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT 'N',
  PRIMARY KEY (`IdRol`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `roles` (`IdRol`, `Nombre`) VALUES
	(1, 'Cliente'),
	(2, 'Profesional'),
	(4, 'Administrador');

CREATE TABLE IF NOT EXISTS `rolespermisos` (
  `IdRol` int NOT NULL,
  `IdPermiso` int NOT NULL,
  PRIMARY KEY (`IdRol`,`IdPermiso`),
  KEY `RolesPermisos_ibfk_2` (`IdPermiso`),
  CONSTRAINT `RolesPermisos_ibfk_1` FOREIGN KEY (`IdRol`) REFERENCES `roles` (`IdRol`),
  CONSTRAINT `RolesPermisos_ibfk_2` FOREIGN KEY (`IdPermiso`) REFERENCES `permisos` (`IdPermiso`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `rolespermisos` (`IdRol`, `IdPermiso`) VALUES
	(1, 11),
	(1, 12),
	(1, 13);

CREATE TABLE IF NOT EXISTS `sedes` (
  `IdSede` int NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(200) DEFAULT NULL,
  `Nombre` varchar(50) DEFAULT NULL,
  `Telefono` varchar(50) DEFAULT NULL,
  `Duracion` time DEFAULT NULL,
  `Estado` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT 'Activo',
  PRIMARY KEY (`IdSede`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `sedes` (`IdSede`, `Direccion`, `Nombre`, `Telefono`, `Duracion`, `Estado`) VALUES
	(4, 'Calle 123, Ciudad A', 'Sede Principal', '555-1111', '08:00:00', 'Activo'),
	(5, 'Avenida XYZ, Ciudad B', 'Sucursal Norte', '555-2222', '09:00:00', 'Inactivo'),
	(6, 'Calle 456, Ciudad C', 'Sucursal Sur', '555-3333', '07:00:00', 'Activo'),
	(7, 'Calle 1', 'Sede A', '555-1234', '01:00:00', 'v'),
	(8, 'Calle 2', 'Sede B', '555-5678', '00:45:00', 'v'),
	(9, 'Calle 3', 'Sede C', '555-9876', '01:30:00', 'v');

CREATE TABLE IF NOT EXISTS `servicios` (
  `IdServicio` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) DEFAULT NULL,
  `Descripcion` varchar(200) DEFAULT NULL,
  `Duracion` time DEFAULT NULL,
  `Estado` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT 'Activo',
  PRIMARY KEY (`IdServicio`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `servicios` (`IdServicio`, `Nombre`, `Descripcion`, `Duracion`, `Estado`) VALUES
	(10, 'Corte de Cabello', 'Servicio básico de corte de cabello', '00:30:00', 'Activo'),
	(11, 'Manicura', 'Manicura y esmaltado de uñas', '01:00:00', 'Inactivo'),
	(12, 'Masaje Relajante', 'Masaje de cuerpo completo', '01:30:00', 'Activo'),
	(13, 'Corte de Pelo', 'Corte de pelo y peinado', '00:30:00', 'v'),
	(14, 'Masaje Relajante', 'Masaje de cuerpo completo', '01:00:00', 'v'),
	(15, 'Manicura', 'Manicura y esmaltado', '00:45:00', 'v'),
	(16, 'Natacion', 'Nuevo servicio de diciembre', '11:37:00', 'V'),
	(19, 'Karate', 'Nuevo servicio de diciembre', '13:52:00', 'Activo');

CREATE TABLE IF NOT EXISTS `usuario` (
  `IdUsuario` int NOT NULL AUTO_INCREMENT,
  `NombreUsuario` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClaveHash` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IdPersona` int NOT NULL,
  `IdRol` int DEFAULT NULL,
  `Activo` varchar(2) DEFAULT 'v',
  PRIMARY KEY (`IdUsuario`) USING BTREE,
  UNIQUE KEY `UserName_UNIQUE` (`NombreUsuario`) USING BTREE,
  KEY `Usuario_ibfk_1` (`IdPersona`),
  KEY `Usuario_ibfk_2` (`IdRol`),
  CONSTRAINT `Usuario_ibfk_1` FOREIGN KEY (`IdPersona`) REFERENCES `personas` (`IdPersona`),
  CONSTRAINT `Usuario_ibfk_2` FOREIGN KEY (`IdRol`) REFERENCES `roles` (`IdRol`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `usuario` (`IdUsuario`, `NombreUsuario`, `ClaveHash`, `IdPersona`, `IdRol`, `Activo`) VALUES
	(1, 'admin', '085213edaf3eb4e3e58688f890ce23357616bb5a8297828881daab5ca68c59b7', 5, 4, 'v'),
	(2, 'cliente', '085213edaf3eb4e3e58688f890ce23357616bb5a8297828881daab5ca68c59b7', 5, 4, 'v'),
	(3, 'empleado', '085213edaf3eb4e3e58688f890ce23357616bb5a8297828881daab5ca68c59b7', 5, 4, 'v'),
	(7, 'usuario1', 'b5c496aa9fd7e0f53f8773f7f00b48951f24f512b49188534d2f6fdeebaeeb07', 4, 1, 'v'),
	(8, 'usuario2', '1000788eb0beb7d809d0b9b04f50b0f019c025eef0ef200f2f2f827420c059e3', 2, 4, 'v'),
	(9, 'usuario3', '085213edaf3eb4e3e58688f890ce23357616bb5a8297828881daab5ca68c59b7', 5, 2, 'v');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
