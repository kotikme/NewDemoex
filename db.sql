create table roles(
id serial primary key,
name varchar(150) not null
);

create table users(
id serial primary key,
login varchar(150) not null,
password varchar(150) not null,
fullname varchar(150) not null,
roleid serial not null,
FOREIGN KEY (roleid) REFERENCES roles (Id)
);

create table categories(
id serial primary key,
name varchar(150)
);

create table manufactures(
id serial primary key,
name varchar(150)
);

create table suppliers(
id serial primary key,
name varchar(150)
);

create table products(
id serial primary key,
name varchar(150) not null,
photo bytea,
description varchar(150),
manufacterid int not null,
suppliersid int not null,
price money not null,
unit text not null,
stock int not null,
categoryid int not null,
discount int not null,
FOREIGN KEY (manufacterid) REFERENCES manufactures (Id),
FOREIGN KEY (suppliersid) REFERENCES suppliers (Id),
FOREIGN KEY (categoryid) REFERENCES categories (Id)
);
ALTER TABLE products
ADD COLUMN squ varchar(150);
ALTER TABLE products ALTER COLUMN photo TYPE text;

insert into suppliers(id, name) values (0, 'Kari');
insert into suppliers(id, name) values (1, 'Shoes for You');

insert into manufactures(id, name) values (0, 'Kari');
insert into manufactures(id, name) values (1, 'Marco Tozzi');
insert into manufactures(id, name) values (2, 'Ros');
insert into manufactures(id, name) values (3, 'Rieker');
insert into manufactures(id, name) values (4, 'Alessio Nesca');
insert into manufactures(id, name) values (5, 'CROSBY');

insert into categories(id, name) values (0, 'Woman shoes');
insert into categories(id, name) values (1, 'Man shoes');

insert into roles(id, name) values (0, 'Client');
insert into roles(id, name) values (1, 'Manager');
insert into roles(id, name) values (2, 'Admin');
