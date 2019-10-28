--select * from [dbo].[ProductsImage]
--truncate table ProductsImage
--alter table ProductsImage add constraint ct_UniqueFileName unique(FileName);

ALTER TABLE ProductsImage
ADD CONSTRAINT UC_FileName UNIQUE (FileName);

--ALTER TABLE ProductsImage
--DROP CONSTRAINT UC_Person UNIQUE (NameFile);

 