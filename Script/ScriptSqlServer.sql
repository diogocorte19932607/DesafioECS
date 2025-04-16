--1
SELECT TOP 5 *
FROM Clientes
ORDER BY DataCadastro DESC;


--2
CREATE PROCEDURE GetPedidosPorCliente
    @ClienteId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT 
        P.Id,
        P.Code,
        P.Name,
        P.Price
    FROM Products P
    WHERE P.ClienteId = '14D76DB8-8E86-4B74-BF6B-200C21E17483';
END
exec GetPedidosPorCliente '14D76DB8-8E86-4B74-BF6B-200C21E17483'



--3
UPDATE Clientes
SET Email = 'novoemail@example.com'
WHERE Id = '14D76DB8-8E86-4B74-BF6B-200C21E17483';



--4
CREATE VIEW vw_ResumoClientes
AS
SELECT 
    C.Nome,
    COUNT(P.Id) AS TotalPedidos,
    ISNULL(SUM(P.Price), 0) AS ValorTotalGasto
FROM Clientes C
LEFT JOIN Products P ON C.Id = P.ClienteId
GROUP BY C.Nome;

select * from  vw_ResumoClientes
