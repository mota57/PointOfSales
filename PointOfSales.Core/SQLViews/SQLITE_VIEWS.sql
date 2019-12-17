CREATE VIEW v_product_merchant
as
SELECT p.Id, p.Name, c.Name as CategoryName, p.ProductCode, p.Price, p.MainImage, p.IsProductForRent, p.Note
FROM Product as p
INNER JOIN Category as c on p.CategoryId =  c.Id
