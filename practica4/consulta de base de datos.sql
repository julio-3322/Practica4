create database practica4

use practica4

create table autor(
	id int primary key identity(1,1),
	nombre nvarchar(100),
	nacionalidad nvarchar(50),
	anionacimiento int,
)

create table libro(
	id int primary key identity(1,1),
	titulo nvarchar(200),
	aniopublicacion int,
	genero nvarchar(50),
	numeropaginas int,
	precio decimal,
	disponibilidad bit,
	autorid int foreign key references autor(id)
)


select * from autor