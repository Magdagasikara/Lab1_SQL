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
-- skulle kunna koppla här till lärare också men det behövs inte nu
)
go

create table Grades (
Id int primary key identity(1,1),
Grade int not null,
-- OBS funderade på att ha en tabell av alla kombinationer som tillåter null på Grade och GradeDate tills de fyllts i
-- men eftersom vi ska kunna lägga till nya studenter men vi behöver inte ge betyg så kommer jag inte skapa nya rader här vid skapande av elev
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
('Lärare'),
('Programansvarig'),
('Rektor'),
('Administratör')
go

insert into Staff(FirstName, LastName, CategoryId)
values
('Christoffer','Fjellström',1),
('Aldor','Besher',1),
('Alexander','Ekberg',2),
('Alexander','där',3)
go

insert into Grades(Grade,GradeDate,StudentId,CourseId)
-- student får inte betyg i kurs som hans klass inte går!
-- sätter betyg 1-5
select -- med lite hjälp av ChatGPT :)
    ROUND(RAND(CHECKSUM(NEWID())) * 4 + 1, 0), -- Slumpmässigt betyg mellan 1 och 5
    DATEADD(DAY, -CAST(RAND(CHECKSUM(NEWID())) * 100 AS INT), GETDATE()), -- Slumpmässigt datum
    s.Id,
    c.CourseId
from Students s, ClassCourse c
where s.ClassId=c.ClassId 
go