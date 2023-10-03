-- Se crea la base de datos KJMC
create database AAMG
go

-- Usa la base de datos KJMC
use AAMG
go

-- Se crea la tabla Customers ( Clients)
create table Customers
(
	Id int identity(1,1) primary key,
	Name varchar(50) not null,
	LastName varchar(50) not null,
	Address varchar(255)
)
go