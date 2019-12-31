CREATE VIEW v_product_merchant
as
SELECT p.Id, p.Name, c.Name as CategoryName, p.ProductCode, p.Price, p.MainImage, p.IsProductForRent, p.Note
FROM Product as p
INNER JOIN Category as c on p.CategoryId =  c.Id


CREATE VIEW v_product_controller
AS
select 
p.Id,p.Name, p.Price, p.ProductCode, c.Name as CategoryName
from Product p left join Category as c on p.CategoryId = c.Id;
