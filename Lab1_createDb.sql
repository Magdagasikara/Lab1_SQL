use master
go

create database School
go

use School
go

create table Class (
Id int primary key identity(1,1),
ClassName nvarchar(25) not null
)
go

create table Course (
Id int primary key identity(1,1),
CourseName nvarchar(25) not null,
)
go

create table StaffCategory (
Id int primary key identity(1,1),
Category nvarchar(25)
)
go

create table Students ( 
Id int primary key identity(1,1),
FirstName nvarchar(25) not null,
LastName nvarchar(30) not null,
ClassId int not null,
foreign key (ClassId) references Class(Id)
)
go

create table ClassCourse (
Id int primary key identity(1,1),
ClassId int not null,
CourseId int not null,
foreign key (ClassId) references Class(Id),
foreign key (CourseId) references Course(Id)
-- skulle kunna koppla h�r till l�rare ocks� men det beh�vs inte nu
)
go

create table Grades (
Id int primary key identity(1,1),
Grade int not null,
-- OBS funderade p� att ha en tabell av alla kombinationer som till�ter null p� Grade och GradeDate tills de fyllts i
-- men eftersom vi ska kunna l�gga till nya studenter men vi beh�ver inte ge betyg s� kommer jag inte skapa nya rader h�r vid skapande av elev
GradeDate date not null,
CourseId int not null,
StudentId int not null
foreign key (CourseId) references Course(Id)
)
go

create table Staff (
Id int primary key identity(1,1),
FirstName nvarchar(25) not null,
LastName nvarchar(30) not null,
CategoryId int not null
)
go

insert into Class(ClassName)
values
('NET23'),
('NET22'),
('JS23')
go

insert into Students(FirstName, LastName, ClassId )
values
('Magda', 'Kb', 1),
('Eva', 'Olsson', 1),
('Lars', 'Eriksson', 3),
('Sara', 'Nilsson', 2),
('Anders', 'Berg', 1),
('Karin', 'Gustavsson', 2),
('Oscar', 'Svensson', 3),
('Emelie', 'Lind', 1),
('Johan', 'Persson', 2),
('Camilla', 'Karlsson', 3),
('Gustav', 'Andersson', 1),
('Frida', 'Johansson', 2),
('Henrik', 'Larsson', 3),
('Malin', 'Eriksson', 1),
('Per', 'Svensson', 2),
('Anna', 'Andersson', 3)
go


insert into Course(CourseName) 
values
('IT Tech and Operations'),
('Programmering med C#'),
('Frontendutveckling del 1'),
('Web Design')
go

insert into ClassCourse (ClassId, CourseId)
values
(1, 1), -- NET23 - IT Tech and Operations
(1, 2), -- NET23 - Programmering med C#
(1, 3), -- NET23 - Frontendutveckling del 1

(2, 1), -- NET22 - IT Tech and Operations
(2, 2), -- NET22 - Programmering med C#
(2, 3), -- NET22 - Frontendutveckling del 1

(3, 3), -- JS23 - Frontendutveckling del 1
(3, 4) -- JS23 - Web Design
go

insert into StaffCategory (Category)
values
('L�rare'),
('Programansvarig'),
('Rektor'),
('Administrat�r')
go

insert into Staff(FirstName, LastName, CategoryId)
values
('Christoffer','Fjellstr�m',1),
('Aldor','Besher',1),
('Alexander','Ekberg',2),
('Alexander','d�r',3)
go

insert into Grades(Grade,GradeDate,StudentId,CourseId)
-- student f�r inte betyg i kurs som hans klass inte g�r!
-- s�tter betyg 1-5
select -- med lite hj�lp av ChatGPT :)
    ROUND(RAND(CHECKSUM(NEWID())) * 4 + 1, 0), -- Slumpm�ssigt betyg mellan 1 och 5
    DATEADD(DAY, -CAST(RAND(CHECKSUM(NEWID())) * 100 AS INT), GETDATE()), -- Slumpm�ssigt datum
    s.Id,
    c.CourseId
from Students s, ClassCourse c
where s.ClassId=c.ClassId 
go