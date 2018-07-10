﻿--SELECT * FROM S_FUNCTIONS;
DELETE FROM S_FUNCTIONS a WHERE a.FUNCTIONNAME IN (
'WikiQLDMBaiViet',
'WikiListLuuTam',
'WikiListPeding',
'WikiListApproved',
'WikiAddNew'
);

INSERT INTO S_FUNCTIONS 
VALUES(173,'WikiQLDMBaiViet','Quản lý danh mục bài viết',1,'/vi-vn/wiki-manage/wiki-doc/catalogue-list','/vi-vn/wiki-manage/wiki-doc/do-delete-catalogue',9,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(183,'WikiListLuuTam','(2) Quản lý bài viết lưu tạm',1,'/vi-vn/wiki-manage/wiki-doc/list','/vi-vn/wiki-manage/wiki-doc/do-delete-doc',2,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(184,'WikiListPeding','(3) Danh sách bài viết chờ xử lý',1,'/vi-vn/wiki-manage/wiki-doc/pending-list','/vi-vn/wiki-manage/wiki-doc/pending-list',3,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(185,'WikiListApproved','(4) Danh sách bài viết đã đăng',1,'/vi-vn/wiki-manage/wiki-doc/approved-list','/vi-vn/wiki-manage/wiki-doc/approved-list',4,0,0,15,NULL,NULL,0);
INSERT INTO S_FUNCTIONS 
VALUES(190,'WikiAddNew','(1)Thêm mới bài viết',1,'/vi-vn/wiki-manage/wiki-doc/add','/vi-vn/wiki-manage/wiki-doc/add',1,0,0,15,NULL,NULL,0);