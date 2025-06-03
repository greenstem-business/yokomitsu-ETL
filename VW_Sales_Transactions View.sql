CREATE VIEW [dbo].[VW_Sales_Transactions] AS
SELECT 
	[Document No],
	[Transaction Type],
	'SALES' AS [Transaction Description],
	[Stock] AS [Stock Code],
	[Quantity],
	[Invoice Amount] AS [Total Amount],
	[Unit Cost],
	ar.[Location],
	[Document Date] AS [Issue Date],
	[Customer] AS [Customer Code],
	[Customer Name],
	[Salesman] AS [Salesman Code],
	sm.[Salesman Name] AS [Salesman Name],
	[Nett Qty]
FROM view_AR_Invoice_Tran ar WITH(NOLOCK) 
INNER JOIN Stock st WITH(NOLOCK)
ON ar.[Stock] = st.[Stock Code]
LEFT JOIN Salesman_tbl sm
ON ar.[Salesman] = sm.[Salesman Code]

UNION ALL

SELECT 
	[Document No],
	[Transaction Type],
	'RETURN' AS [Transaction Description],
	[Stock] AS [Stock Code],
	[Quantity],
	[CN Amount] AS [Total Amount],
	[Unit Cost],
	ar.[Location],
	[Document Date],
	[Customer] AS [Customer Code],
	[Customer Name],
	[Salesman] AS [Salesman Code],
	sm.[Salesman Name] AS [Salesman Name],
	[Nett Qty]
FROM view_AR_CN_Tran ar WITH(NOLOCK) 
INNER JOIN Stock st WITH(NOLOCK)
ON ar.[Stock] = st.[Stock Code]
LEFT JOIN Salesman_tbl sm
ON ar.[Salesman] = sm.[Salesman Code];

--API tokens table

CREATE TABLE API_TOKENS
(
	[ID] INT IDENTITY PRIMARY KEY,
	[User Name] VARCHAR(50),
	[Token] NVARCHAR(500),
	[Created Date Time] DATETIME,
	[Is Active] BIT DEFAULT(1)
)

select * from API_TOKENS;
