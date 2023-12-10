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

-- hur fan kopplar jag kurser och klasser, separat tabell m�ste det nog bli
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
-- OBS funderar p� att ha en tabell av alla kombinationer som till�ter null p� Grade och GradeDate tills de fylls i
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
    c.Id
from Students s, Course c



-- sql queries som kommer beh�vas

-- 1. **H�mta alla elever**
--    Anv�ndaren f�r v�lja om de vill se eleverna sorterade p� f�r- eller efternamn och om det ska vara stigande eller fallande sortering.
select FirstName, LastName
from Students
order by FirstName, LastName
-- eller: order by FirstName desc, LastName desc

--2. **H�mta alla elever i en viss klass**
--    Anv�ndaren ska f�rst f� se en lista med alla klasser som finns, sedan f�r anv�ndaren v�lja en av klasserna och d� skrivs alla elever i den klassen ut.
--    Extra utmaning: l�t anv�ndaren �ven f� v�lja sortering p� eleverna som i punkten ovan.
select distinct ClassName
from Class

select s.FirstName, s.LastName
from Students s, Class c
where s.ClassID=c.ClassName
	and c.ClassName=(!!!!!chosen ClassName)
order by FirstName, LastName
  -- eller: order by FirstName desc, LastName desc
  
--3. **L�gga till ny personal**
--    Anv�ndaren ska ha m�jlighet att mata in uppgifter om en ny anst�lld och den datan sparas d� ner i databasen.
insert into Staff(FirstName, LastName, CategoryId)
values
(!!!! chosen values!!!!)

    
--4. **H�mta personal**
--    Anv�ndaren f�r v�lja om denna vill se alla anst�llda, eller bara inom en av kategorierna s� som ex l�rare.
    
select s.FirstName, s.LastName
from Staff s, StaffCategory sc
where s.CategoryId=sc.Id
	and sc.Category=(!!!!!chosen Category!!)
-- eller alla oavsett category!

--5. **H�mta alla betyg som satts den senaste m�naden**
--    H�r f�r anv�ndaren se en lista med alla betyg som satts senaste m�naden d�r elevens namn, kursens namn och betyget framg�r.
select 
	s.FirstName,
	s.LastName,
	c.CourseName,
	g.Grade,
	g.GradeDate
from Students s 
	left join ClassCourse cc on s.ClassID=cc.ClassId
	left join Course c on c.Id=cc.CourseId
	left join Grades g on g.CourseId=c.Id and g.StudentId=s.Id
where g.GradeDate between convert(date,GETDATE()) and DATEADD(month, -1, convert(date,getdate())
go

--6. **Snittbetyg per kurs**
--    H�mta en lista med alla kurser och det snittbetyg som eleverna f�tt p� den kursen samt det h�gsta och l�gsta betyget som n�gon f�tt i kursen.
--    H�r f�r anv�ndaren se en lista med alla kurser i databasen, snittbetyget samt det h�gsta och l�gsta betyget f�r varje kurs.
--    Tips: Det kan vara sv�rt att g�ra detta med betyg i form av bokst�ver s� du kan v�lja att lagra betygen som siffror i st�llet.
select 
	c.CourseName,
	Count(g.Grade) as NumberOfGrades,
	avg(convert(decimal,g.Grade)) as MeanGrade,
	min(g.Grade) as MinGrade,
	max(g.Grade) as MaxGrade
from Course c  
	left join Grades g on g.CourseId=c.Id
group by 
	c.CourseName

    
--7. **L�gga till nya elever**
--    Anv�ndaren f�r m�jlighet att mata in uppgifter om en ny elev och den datan sparas d� ner i databasen.