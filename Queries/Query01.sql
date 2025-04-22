-- Escreva uma query que retorne os 5 clientes mais recentes da tabela Clientes ordenados por data de cadastro.

SELECT TOP 5 [Id],
			 [Name],
			 [Email],
			 [CreationDate]
FROM Customers 
ORDER BY CreationDate DESC