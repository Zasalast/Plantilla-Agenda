CREATE DATABASE IF NOT EXISTS `agenda_empresa_user4`;
USE `agenda_empresa_user4`;

-- Tablas
CREATE TABLE IF NOT EXISTS `personas` (
  `IdPersona` INT NOT NULL AUTO_INCREMENT,
  `Identificacion` VARCHAR(15) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PrimerNombre` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SegundoNombre` VARCHAR(50) DEFAULT NULL,
  `PrimerApellido` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SegundoApellido` VARCHAR(50) DEFAULT NULL,
  `Sexo` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Correo` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT 'Correo@mail.com',
  `Telefono` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT '00000000',
  `Estado` VARCHAR(50) DEFAULT 'v',
  PRIMARY KEY (`IdPersona`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `sedes` (
  `IdSede` INT NOT NULL AUTO_INCREMENT,
  `Direccion` VARCHAR(200) DEFAULT NULL,
  `Nombre` VARCHAR(50) DEFAULT NULL,
  `Telefono` VARCHAR(50) DEFAULT NULL,
  `Duracion` TIME DEFAULT NULL,
  `Estado` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`IdSede`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `servicios` (
  `IdServicio` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(100) DEFAULT NULL,
  `Descripcion` VARCHAR(200) DEFAULT NULL,
  `Duracion` TIME DEFAULT NULL,
  `Estado` VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (`IdServicio`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `roles` (
  `IdRol` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT 'N',
  PRIMARY KEY (`IdRol`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `permisos` (
  `IdPermiso` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`IdPermiso`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `rolespermisos` (
  `IdRol` INT NOT NULL,
  `IdPermiso` INT NOT NULL,
  PRIMARY KEY (`IdRol`, `IdPermiso`),
  KEY `RolesPermisos_ibfk_2` (`IdPermiso`),
  CONSTRAINT `RolesPermisos_ibfk_1` FOREIGN KEY (`IdRol`) REFERENCES `roles` (`IdRol`),
  CONSTRAINT `RolesPermisos_ibfk_2` FOREIGN KEY (`IdPermiso`) REFERENCES `permisos` (`IdPermiso`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `usuario` (
  `IdUsuario` INT NOT NULL AUTO_INCREMENT,
  `NombreUsuario` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClaveHash` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IdPersona` INT NOT NULL,
  `IdRol` INT DEFAULT NULL,
  `Activo` VARCHAR(2) DEFAULT 'v',
  PRIMARY KEY (`IdUsuario`) USING BTREE,
  UNIQUE KEY `UserName_UNIQUE` (`NombreUsuario`) USING BTREE,
  KEY `Usuario_ibfk_1` (`IdPersona`),
  KEY `Usuario_ibfk_2` (`IdRol`),
  CONSTRAINT `Usuario_ibfk_1` FOREIGN KEY (`IdPersona`) REFERENCES `personas` (`IdPersona`),
  CONSTRAINT `Usuario_ibfk_2` FOREIGN KEY (`IdRol`) REFERENCES `roles` (`IdRol`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `horarios` (
  `IdHorario` INT NOT NULL AUTO_INCREMENT,
  `HoraInicio` TIME DEFAULT NULL,
  `HoraFin` TIME DEFAULT NULL,
  `Disponibilidad` CHAR(3) DEFAULT 'v',
  `FechaInicio` DATE DEFAULT NULL,
  `FechaFin` DATE DEFAULT NULL,
  PRIMARY KEY (`IdHorario`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `agendas` (
  `IdAgenda` INT NOT NULL AUTO_INCREMENT,
  `IdSede` INT DEFAULT NULL,
  `IdServicio` INT DEFAULT NULL,
  `IdHorario` INT DEFAULT NULL,
  `IdProfesional` INT DEFAULT NULL,
  `Estado` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IdCliente` INT DEFAULT NULL,
  `IdSedeAgendada` INT DEFAULT NULL,
  `IdServicioAgendado` INT DEFAULT NULL,
  PRIMARY KEY (`IdAgenda`),
  KEY `Agendas_ibfk_1` (`IdSede`),
  KEY `Agendas_ibfk_2` (`IdServicio`),
  KEY `Agendas_ibfk_3` (`IdHorario`),
  KEY `Agendas_ibfk_4` (`IdProfesional`),
  CONSTRAINT `Agendas_ibfk_1` FOREIGN KEY (`IdSede`) REFERENCES `sedes` (`IdSede`),
  CONSTRAINT `Agendas_ibfk_2` FOREIGN KEY (`IdServicio`) REFERENCES `servicios` (`IdServicio`),
  CONSTRAINT `Agendas_ibfk_3` FOREIGN KEY (`IdHorario`) REFERENCES `horarios` (`IdHorario`),
  CONSTRAINT `Agendas_ibfk_4` FOREIGN KEY (`IdProfesional`) REFERENCES `personas` (`IdPersona`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `agendamientos` (
  `IdAgendamiento` INT NOT NULL AUTO_INCREMENT,
  `IdCliente` INT DEFAULT NULL,
  `Estado` VARCHAR(50) COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FechaHora` DATETIME DEFAULT NULL,
  PRIMARY KEY (`IdAgendamiento`),
  KEY `Agendamientos_ibfk_1` (`IdCliente`),
  CONSTRAINT `Agendamientos_ibfk_1` FOREIGN KEY (`IdCliente`) REFERENCES `personas` (`IdPersona`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `asistencias` (
  `IdAsistencia` INT NOT NULL AUTO_INCREMENT,
  `EstadoAsistencia` VARCHAR(1) DEFAULT NULL,
  `IdAgendamiento` INT DEFAULT NULL,
  PRIMARY KEY (`IdAsistencia`),
  KEY `Asistencias_ibfk_1` (`IdAgendamiento`),
  CONSTRAINT `Asistencias_ibfk_1` FOREIGN KEY (`IdAgendamiento`) REFERENCES `agendamientos` (`IdAgendamiento`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `cancelaciones` (
  `IdCancelacion` INT NOT NULL AUTO_INCREMENT,
  `FechaHora` DATETIME DEFAULT NULL,
  `Motivo` VARCHAR(200) DEFAULT NULL,
  `IdAgendamiento` INT DEFAULT NULL,
  PRIMARY KEY (`IdCancelacion`),
  KEY `Cancelaciones_ibfk_1` (`IdAgendamiento`),
  CONSTRAINT `Cancelaciones_ibfk_1` FOREIGN KEY (`IdAgendamiento`) REFERENCES `agendamientos` (`IdAgendamiento`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Procedimientos
DELIMITER //
CREATE PROCEDURE AgendarCita(
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
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE CancelarCita(IN p_IdAgenda INT, IN p_Motivo VARCHAR(200))
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
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE IniciarSesion(IN InNombreUsuario VARCHAR(50), IN InClave VARCHAR(255))
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
END //
DELIMITER ;

 -- Otros procedimientos
DELIMITER //

-- Este procedimiento muestra la lista de citas agendadas para un cliente específico
CREATE PROCEDURE MostrarCitasCliente(IN p_IdCliente INT)
BEGIN
    SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdProfesional
    FROM agendas A
    INNER JOIN horarios H ON A.IdHorario = H.IdHorario
    INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio
    WHERE A.IdCliente = p_IdCliente;
END //
DELIMITER ;


DELIMITER //
-- Este procedimiento muestra la lista de citas agendadas para un profesional específico
CREATE PROCEDURE MostrarCitasProfesional(IN p_IdProfesional INT)
BEGIN
    SELECT A.IdAgenda, A.Estado, S.Nombre AS Servicio, H.HoraInicio, H.HoraFin, A.IdCliente
    FROM agendas A
    INNER JOIN horarios H ON A.IdHorario = H.IdHorario
    INNER JOIN servicios S ON A.IdServicioAgendado = S.IdServicio
    WHERE A.IdProfesional = p_IdProfesional;
END //
DELIMITER ;


DELIMITER //
-- Este procedimiento muestra la disponibilidad de horarios para un servicio y sede específicos
CREATE PROCEDURE MostrarDisponibilidadHorarios(IN p_IdServicio INT, IN p_IdSede INT)
BEGIN
    SELECT H.IdHorario, H.HoraInicio, H.HoraFin
    FROM horarios H
    LEFT JOIN agendas A ON H.IdHorario = A.IdHorario
    WHERE A.IdAgenda IS NULL
    AND H.Disponibilidad = 'v'
    AND A.IdServicioAgendado = p_IdServicio
    AND A.IdSedeAgendada = p_IdSede;
END //

DELIMITER ;


DELIMITER //

-- Este procedimiento muestra la disponibilidad de horarios para un servicio específico
CREATE PROCEDURE MostrarDisponibilidadHorariosServicio(IN p_IdServicio INT)
BEGIN
    SELECT H.IdHorario, H.HoraInicio, H.HoraFin, H.FechaInicio, H.FechaFin
    FROM horarios H
    LEFT JOIN agendas A ON H.IdHorario = A.IdHorario
    WHERE A.IdAgenda IS NULL
    AND H.Disponibilidad = 'v'
    AND A.IdServicioAgendado = p_IdServicio;
END //
DELIMITER ;
-- Este procedimiento muestra la disponibilidad de horarios para una sede específica
DELIMITER //
CREATE PROCEDURE MostrarDisponibilidadHorariosSede(IN p_IdSede INT)
BEGIN
    SELECT H.IdHorario, H.HoraInicio, H.HoraFin, H.FechaInicio, H.FechaFin
    FROM horarios H
    LEFT JOIN agendas A ON H.IdHorario = A.IdHorario
    WHERE A.IdAgenda IS NULL
    AND H.Disponibilidad = 'v'
    AND A.IdSedeAgendada = p_IdSede;
END //

DELIMITER ;
DELIMITER //
CREATE PROCEDURE `RegistrarPersonaPorAdmin`(
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
CREATE PROCEDURE `EliminarUsuario`(
    IN p_IdUsuario INT
)
BEGIN
    -- Eliminar usuario
    DELETE FROM usuario WHERE IdUsuario = p_IdUsuario;
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE `RegistrarUsuario`(
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

DELIMITER //
CREATE PROCEDURE `ActualizarUsuario`(
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

DELIMITER //
CREATE PROCEDURE ObtenerDetallesAgenda(IN p_IdAgenda INT)
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
END //
DELIMITER ;
