CREATE TABLE `employee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) DEFAULT NULL,
  `password` varchar(25) DEFAULT NULL,
  `fname` varchar(45) NOT NULL,
  `mname` varchar(45) DEFAULT NULL,
  `lname` varchar(45) NOT NULL,
  `gender` varchar(10) DEFAULT NULL,
  `religion` varchar(15) DEFAULT NULL,
  `address1` varchar(200) NOT NULL,
  `address2` varchar(200) DEFAULT NULL,
  `address3` varchar(200) DEFAULT NULL,
  `district` varchar(45) NOT NULL,
  `pin` int(11) NOT NULL,
  `state` varchar(45) NOT NULL,
  `country` varchar(45) NOT NULL,
  `role` varchar(45) DEFAULT NULL,
  `aadhar_no` int(16) DEFAULT NULL,
  `pan_no` varchar(10) DEFAULT NULL,
  `employee_pic` blob,
  `employee_type` varchar(45) DEFAULT NULL,
  `join_date` date DEFAULT NULL,
  `active` bit(1) DEFAULT b'0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `username_UNIQUE` (`username`),
  KEY `username` (`id`,`username`),
  KEY `role_idx` (`role`),
  CONSTRAINT `role` FOREIGN KEY (`role`) REFERENCES `post` (`role`) ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE `salary_info` (
  `id` int(11) NOT NULL,
  `username` varchar(45) DEFAULT NULL,
  `name` varchar(140) NOT NULL,
  `role` varchar(45) NOT NULL,
  `bank_name` varchar(100) DEFAULT NULL,
  `branch_name` varchar(50) DEFAULT NULL,
  `account_no` varchar(25) DEFAULT NULL,
  `ifsc_code` varchar(20) DEFAULT NULL,
  `micr_code` varchar(20) DEFAULT NULL,
  `swift_code` varchar(20) DEFAULT NULL,
  `payscale_code` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username_UNIQUE` (`username`),
  KEY `salary_role_to_employee_salary_role_idx_idx` (`role`) /*!80000 INVISIBLE */,
  KEY `name` (`name`),
  KEY `employee_id_esi_idx` (`id`,`username`,`name`,`role`),
  CONSTRAINT `employee_salary_id_to_employee_id` FOREIGN KEY (`id`) REFERENCES `employee` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `employee_salary_id_to_employee_username` FOREIGN KEY (`username`) REFERENCES `employee` (`username`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `salary_role_to_employee_salary_role_idx` FOREIGN KEY (`role`) REFERENCES `employee` (`role`) ON DELETE RESTRICT ON UPDATE CASCADE
);

DROP trigger IF EXISTS employee_salary_info_BEFORE_INSERT;
CREATE DEFINER=`devc100`@`%` TRIGGER `employee_salary_info_BEFORE_INSERT` BEFORE INSERT ON salary_info FOR EACH ROW BEGIN
IF((SELECT mname FROM employee WHERE `id`=NEW.`id`) IS NULL AND (SELECT lname FROM employee WHERE `id`=NEW.`id`) IS NULL) THEN
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`));
ELSEIF ((SELECT mname FROM employee WHERE `id`=NEW.`id`) IS NULL) THEN
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT lname FROM employee WHERE `id`=NEW.`id`));
ELSEIF ((SELECT lname FROM employee WHERE `id`=NEW.`id`) IS NULL) THEN
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT mname FROM employee WHERE `id`=NEW.`id`));
ELSE
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT mname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT lname FROM employee WHERE `id`=NEW.`id`));
END IF;

SET new.`username` = CONCAT((SELECT username FROM employee WHERE `id`=NEW.`id`));

SET new.`role` = CONCAT((SELECT `role` FROM employee WHERE `id`=NEW.`id`));

END;

DROP trigger IF EXISTS employee_salary_info_BEFORE_UPDATE;
CREATE DEFINER=`devc100`@`%` TRIGGER `employee_salary_info_BEFORE_UPDATE` BEFORE UPDATE ON salary_info FOR EACH ROW BEGIN
IF((SELECT mname FROM employee WHERE `id`=NEW.`id`) IS NULL AND (SELECT lname FROM employee WHERE `id`=NEW.`id`) IS NULL) THEN
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`));
ELSEIF ((SELECT mname FROM employee WHERE `id`=NEW.`id`) IS NULL) THEN
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT lname FROM employee WHERE `id`=NEW.`id`));
ELSEIF ((SELECT lname FROM employee WHERE `id`=NEW.`id`) IS NULL) THEN
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT mname FROM employee WHERE `id`=NEW.`id`));
ELSE
SET new.`name` = CONCAT((SELECT fname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT mname FROM employee WHERE `id`=NEW.`id`),' ',(SELECT lname FROM employee WHERE `id`=NEW.`id`));
END IF;

SET new.`username` = CONCAT((SELECT username FROM employee WHERE `id`=NEW.`id`));

SET new.`role` = CONCAT((SELECT `role` FROM employee WHERE `id`=NEW.`id`));
END;

CREATE TABLE `salary_info` (
  `id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `employee_salary_id_to_employee_id` FOREIGN KEY (`id`) REFERENCES `employee` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
);

CREATE TABLE `salary_head` (
  `salhead_id` int(11) NOT NULL AUTO_INCREMENT,
  `salhead_code` varchar(10) DEFAULT NULL,
  `salhead_name` varchar(100) DEFAULT NULL,
  `salhead_group` varchar(45) DEFAULT NULL,
  `salhead_operator` varchar(30) DEFAULT NULL,
  `salhead_formula` varchar(5000) DEFAULT NULL,
  `salhead_start_date` datetime DEFAULT NULL,
  `salhead_creation_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`salhead_id`),
  UNIQUE KEY `salary_head_salhead_code_uindex` (`salhead_code`),
  UNIQUE KEY `salary_head_salhead_name_uindex` (`salhead_name`),
  UNIQUE KEY `salary_head_code` (`salhead_code`),
  UNIQUE KEY `salary_head_name` (`salhead_name`),
  KEY `salhead_operator_fk_idx` (`salhead_operator`),
  CONSTRAINT `salhead_operator_fk` FOREIGN KEY (`salhead_operator`) REFERENCES `head_operator` (`operator_name`) ON DELETE RESTRICT ON UPDATE CASCADE
);

DROP table if exists head_operator;
CREATE TABLE `head_operator` (
  `operator_id` int(11) NOT NULL AUTO_INCREMENT,
  `operator_code` varchar(1) DEFAULT NULL,
  `operator_name` varchar(30) DEFAULT NULL UNIQUE,
  `operator_creation_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`operator_id`),
  UNIQUE KEY `sal_head_operator_name_uindex` (`operator_name`),
  UNIQUE KEY `salary_head_operator_code` (`operator_code`)
);

INSERT INTO `head_operator` (`operator_code`, `operator_name`) VALUES ('+', 'Addition');
INSERT INTO `head_operator` (`operator_code`, `operator_name`) VALUES ('-', 'Substraction');
INSERT INTO `head_operator` (`operator_code`, `operator_name`) VALUES ('*', 'Multiplication');
INSERT INTO `head_operator` (`operator_code`, `operator_name`) VALUES ('/', 'Division');
INSERT INTO `head_operator` (`operator_id`,`operator_code`, `operator_name`) VALUES ('1',NULL, '-Select Operator-');

CREATE DEFINER=`devc100`@`%` TRIGGER `employee_AFTER_UPDATE` AFTER UPDATE ON `employee` FOR EACH ROW BEGIN
IF(NEW.mname IS NULL AND NEW.lname IS NULL) THEN
    UPDATE salary_info SET
                           salary_info.name=CONCAT(NEW.fname),
                           salary_info.username = NEW.username,
                           salary_info.role=NEW.role
    WHERE `id`=NEW.`id`;
ELSEIF (NEW.mname IS NULL AND (NOT NEW.lname IS NULL)) THEN
    UPDATE salary_info SET
                           salary_info.name=CONCAT(NEW.fname,' ',NEW.lname),
                           salary_info.username = NEW.username,
                           salary_info.role=NEW.role
    WHERE `id`=NEW.`id`;
ELSEIF (NEW.lname IS NULL AND (NOT NEW.mname IS NULL)) THEN
    UPDATE salary_info SET
                           salary_info.name=CONCAT(NEW.fname,' ',NEW.mname),
                           salary_info.username = NEW.username,
                           salary_info.role=NEW.role
    WHERE `id`=NEW.`id`;
ELSE
    UPDATE salary_info SET
                           salary_info.name=CONCAT(NEW.fname,' ',NEW.mname,' ',NEW.lname),
                           salary_info.username = NEW.username,
                           salary_info.role=NEW.role
    WHERE `id`=NEW.`id`;
END IF;
END;

DROP procedure if exists salary_head_insert;
CREATE DEFINER=`devc100`@`%` PROCEDURE salary_head_insert
    (_code VARCHAR(10), _name VARCHAR(100),_group VARCHAR(45), _operator VARCHAR(30), _formula VARCHAR(5000),_start_date VARCHAR(30))
BEGIN
   INSERT INTO salary_head (salhead_code, salhead_name, salhead_group, salhead_operator, salhead_formula,salhead_start_date)
   VALUES (_code,_name,_group,_operator,_formula,_start_date);
END;

DROP procedure if exists salary_head_update;
CREATE DEFINER=`devc100`@`%` PROCEDURE salary_head_update
    (_id int, _code VARCHAR(10), _name VARCHAR(100),_group VARCHAR(45),_operator VARCHAR(30), _formula VARCHAR(5000),_start_date VARCHAR(30))
BEGIN
   UPDATE salary_head
   SET salhead_code=_code, salhead_name=_name, salhead_group=_group, salhead_operator = _operator, salhead_formula=_formula,salhead_start_date=_start_date
   WHERE salhead_id=_id;
END;

DROP procedure if exists salary_head_delete;
CREATE DEFINER=`devc100`@`%` PROCEDURE salary_head_delete
    (_id int)
BEGIN
   DELETE FROM salary_head WHERE salhead_id=_id;
END;

select * from salary_info;
call salary_head_insert('basic','basicpay','basic','addition','basic*1','2019-11-30');
call salary_head_update(25,`basic`,`basicpay`,`basic`,`addition`,`basic*1`,`2019-11-30`);
call salary_head_delete(28);

select * from employee;
select * from salary_head;
select * from head_operator;
select * from salary_info;

ALTER TABLE salary_info ADD COLUMN `basicpay` DECIMAL(20,2);
ALTER TABLE salary_info CHANGE COLUMN `x` `y` DECIMAL(20,2);
ALTER TABLE salary_info DROP COLUMN `basicpay`;
call salary_head_delete(29);
SELECT salhead_code,salhead_name,salhead_group,salhead_operator,salhead_formula,salhead_start_date FROM salary_head WHERE salhead_id = 21;