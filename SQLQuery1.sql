SELECT * FROM Usuarios as U 
	LEFT JOIN Contatos C ON C.UsuarioId = U.Id 
	LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id
WHERE U.Id = 1008;