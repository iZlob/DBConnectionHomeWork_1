DROP TABLE  IF EXISTS `orders`;
DROP TABLE IF EXISTS `workers`;
DROP TABLE IF EXISTS `cars`;    

CREATE TABLE `workers`(
    id int PRIMARY KEY AUTO_INCREMENT,
    Name varchar(50)
    );
    
 
 CREATE TABLE `cars`(
    id int PRIMARY KEY AUTO_INCREMENT,
    manufacturer varchar(50),
    model varchar(50)
    );
    
 CREATE TABLE `orders`(
    id int PRIMARY KEY AUTO_INCREMENT,
    accepcerId int,
    repairerId int,
    carId int,
    defect varchar(1000),
    fixed varchar(1000),
    
    FOREIGN KEY (accepcerId) REFERENCES workers(id),
    FOREIGN KEY (repairerId) REFERENCES workers(id),
    FOREIGN KEY (carId) REFERENCES cars(id)
    );
   
   
 INSERT INTO `cars` (manufacturer, model)
 VALUES ("lada", "sedan"), ("lada", "baklazhan"), ("bmv", "m5"), ("uaz", "buhanka");
 
 SELECT * FROM cars;
 
 INSERT INTO `workers` (name)
 VALUES ("john"), ("bill"), ("goblin"), ("petr"), ("ekaterina"), ("anton"), ("snoop dog"), ("vitia");
 
 SELECT * FROM workers;
 
 INSERT INTO orders (accepcerId, repairerId, carId, defect, fixed)
 SELECT wa.id, wr.id, c.id, "Троит двигатель", "Рассыпались свечи зажигания"
 FROM workers AS wa, cars AS c, workers AS wr
 WHERE wa.Name = "john" and wr.Name = "anton" AND c.manufacturer = "bmv";
 
 INSERT INTO orders (accepcerId, repairerId, carId, defect, fixed)
 SELECT wa.id, wr.id, c.id, "Не работает машина", "Нет двигателя"
 FROM workers AS wa, cars AS c, workers AS wr
 WHERE wa.Name = "snoop dog" and wr.Name = "ekaterina" AND c.manufacturer = "uaz";
 
 
SELECT * FROM orders;