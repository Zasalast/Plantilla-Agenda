
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