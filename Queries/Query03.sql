-- Escreva uma query que atualize o e-mail de um cliente com base no seu ID

DECLARE @NewEmail NVARCHAR(100) = 'joliveira@teste.com';
DECLARE @CustomerId INT = 1;

UPDATE Customers
SET Email = @NewEmail
WHERE Id = @CustomerId;