DROP TABLE WIKI_CATALOGUES;
CREATE TABLE WIKI_CATALOGUES
    (ID                             NUMBER(20,0),
    NAME                           VARCHAR2(200 CHAR),
    CATA_LEVEL                     NUMBER(2,0) DEFAULT 0,
    CREATED_BY                     VARCHAR2(50 CHAR),
    CREATED_DATE                   DATE,
    MODIFIED_BY                    VARCHAR2(50 CHAR),
    MODIFIED_DATE                  DATE,
    DELETED                        NUMBER(1,0) DEFAULT 0,
    NAME_ENG                       VARCHAR2(200 CHAR),
    PARENT_ID                      NUMBER(20,0))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  USERS
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/


DROP TABLE WIKI_DOCS;

CREATE TABLE WIKI_DOCS
    (ID                             NUMBER(20,0),
    TITLE                          VARCHAR2(500 CHAR),
    CONTENT                        CLOB,
    VIEW_NUMBER                    NUMBER(20,0) DEFAULT 0,
    CREATED_BY                     VARCHAR2(50 CHAR),
    CREATED_DATE                   DATE,
    MODIFIED_BY                    VARCHAR2(50 CHAR),
    MODIFIED_DATE                  DATE,
    DELETED                        NUMBER(1,0) DEFAULT 0,
    LANGUAGE_CODE                  VARCHAR2(5 CHAR),
    HASHTAG                        VARCHAR2(1000 CHAR),
    FILE_URL01                     VARCHAR2(200 CHAR),
    FILE_URL02                     VARCHAR2(200 CHAR),
    FILE_URL03                     VARCHAR2(200 CHAR),
    STATUS                         NUMBER(1,0),
    REFUSE_REASON                  VARCHAR2(500 CHAR))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  USERS
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  LOB ("CONTENT") STORE AS SYS_LOB0000092411C00003$$
  (
  TABLESPACE  USERS
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
   NOCACHE LOGGING
   CHUNK 8192
  )
  NOPARALLEL
  LOGGING
/



DROP TABLE  WIKI_DOCS_CATALOGUES;
CREATE TABLE WIKI_DOCS_CATALOGUES
    (ID                             NUMBER(20,0),
    DOC_ID                         NUMBER(20,0),
    CATALOGUE_ID                   NUMBER(20,0))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  USERS
  STORAGE   (
    INITIAL     65536
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/




COMMENT ON COLUMN WIKI_DOCS_CATALOGUES.CATALOGUE_ID IS 'ID DANH MUC BAI VIET'
/
COMMENT ON COLUMN WIKI_DOCS_CATALOGUES.DOC_ID IS 'ID BAI VIET'
/
drop SEQUENCE SEQ_WIKI_CATAS;
CREATE SEQUENCE SEQ_WIKI_CATAS
  INCREMENT BY 1
  START WITH 61
  MINVALUE 1
  MAXVALUE 9999999999999
  NOCYCLE
  NOORDER
  CACHE 5
/
DROP SEQUENCE SEQ_WIKI_DOC_CATAS;
CREATE SEQUENCE SEQ_WIKI_DOC_CATAS
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999
  NOCYCLE
  NOORDER
  CACHE 5
/

DROP SEQUENCE SEQ_WIKI_DOCS;
CREATE SEQUENCE SEQ_WIKI_DOCS
  INCREMENT BY 1
  START WITH 41
  MINVALUE 1
  MAXVALUE 999999999999999999
  NOCYCLE
  NOORDER
  CACHE 5
/

