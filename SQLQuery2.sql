SELECT * FROM Usuarios as U 
	LEFT JOIN Contatos C ON C.UsuarioId = U.Id 
    LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id
    LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id
    LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id
                          