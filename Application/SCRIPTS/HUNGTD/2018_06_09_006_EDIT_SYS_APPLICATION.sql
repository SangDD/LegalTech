﻿--SELECT * FROM SYS_APPLICATION;
DELETE FROM SYS_APPLICATION a WHERE a.id IN (3, 99);

INSERT INTO SYS_APPLICATION 
VALUES(99,'VI_VN','REQUEST \n FOR INTERNATIONAL TRADEMARK \n REGISTRATION \n BASED ON VIETNAM TRADEMARK APPLICATION   ','TM06DKQT','Request for international trademark registration(Vietnamese)',NULL,NULL,3,NULL,NULL,NULL,1,NULL,'/trade-mark-01/sua-doi-don-dang-ky/TM06DKQT');
INSERT INTO SYS_APPLICATION 
VALUES(99,'EN_US','TỜ KHAI \n YÊU CẦU ĐĂNG KÝ QUỐC TẾ NHÃN HIỆU \n CÓ NGUỒN GỐC VIỆT NAM  ','TM06DKQT','Request for international trademark registration',NULL,NULL,4,NULL,NULL,NULL,1,NULL,'/trade-mark-01/sua-doi-don-dang-ky/TM06DKQT');

COMMIT;