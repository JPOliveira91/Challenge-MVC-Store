-- Crie uma stored procedure que receba um ID de cliente e retorne os pedidos realizados por ele

CREATE PROCEDURE GetOrdersByCustomer
    @CustomerId INT
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM Customers WHERE Id = @CustomerId)
	BEGIN
		PRINT 'Usuário Não Encontrado';
	END
	ELSE
	BEGIN
		SELECT	o.Id,
				o.Date,
				SUM(op.Quantity * op.UnitPrice) AS TotalAmount,
				STRING_AGG(CAST(op.Quantity AS VARCHAR(MAX)) + ' ' + p.Name, ', ') WITHIN GROUP (ORDER BY p.Name) AS Products
		FROM Orders o
		LEFT JOIN OrderProducts op
			ON o.Id = op.OrderId
		LEFT JOIN Products p
			ON op.ProductId = p.Id
		WHERE o.CustomerId = @CustomerId
		GROUP BY o.Id, o.Date
		ORDER BY o.Date DESC;
	END    
END