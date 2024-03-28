CREATE VIEW InfoProduct AS
SELECT        dbo.Product.ID_Product, dbo.Product.Name, dbo.Product.DescriptionProduct, dbo.ManufactureProduct.Name AS Manufacture, dbo.Product.Cost, dbo.Product.QuantityStock, dbo.Product.Photo
FROM            dbo.ManufactureProduct INNER JOIN
                         dbo.Product ON dbo.ManufactureProduct.ID_Manufacturer = dbo.Product.ID_Manufacture