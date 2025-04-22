-- Crie uma view que exiba o nome do cliente, o total de pedidos e o valor total gasto

CREATE VIEW CustomersSummary AS
SELECT	c.Id,
		c.Name,
		COUNT(DISTINCT op.OrderId) AS 'OrdersCount',
		ISNULL(SUM(op.Quantity * op.UnitPrice), 0) AS TotalAmountSpent
FROM Customers c
LEFT JOIN Orders o
	ON c.Id = o.CustomerId
LEFT JOIN OrderProducts op
	ON o.Id = op.OrderId
GROUP BY c.Id, c.Name;