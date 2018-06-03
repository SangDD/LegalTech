﻿CREATE SEQUENCE SEQ_APP_CLASS_INFO
  INCREMENT BY 1
  START WITH 6521
  MINVALUE 1
  MAXVALUE 9999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 5
/

DROP TABLE APP_CLASS_INFO;

CREATE TABLE APP_CLASS_INFO
    (ID                             NUMBER(20,0) DEFAULT 0,
    CODE                           VARCHAR2(20 CHAR),
    NAME_VI                        VARCHAR2(500 CHAR),
    NAME_EN                        VARCHAR2(500 CHAR),
    NAME_CN                        VARCHAR2(500 CHAR),
    DELETED                        NUMBER(1,0) DEFAULT 0,
    CREATED_BY                     VARCHAR2(50 CHAR),
    CREATED_DATE                   DATE,
    MODIFIED_BY                    VARCHAR2(50 CHAR),
    MODIFIED_DATE                  DATE)
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

CREATE OR REPLACE 
PACKAGE PKG_APP_CLASS
 IS TYPE TCURSOR IS REF CURSOR;

PROCEDURE PROC_CLASS_ADD
(
P_ID IN NUMBER,
P_CODE IN VARCHAR2,
P_NAME_VI IN VARCHAR2,
P_NAME_EN IN VARCHAR2,
P_NAME_CN IN VARCHAR2,
P_CREATED_BY IN VARCHAR2,
P_CREATED_DATE IN DATE,
P_RETURN OUT NUMBER
);

PROCEDURE PROC_CLASS_UPDATE
(
P_ID IN NUMBER,
P_CODE IN VARCHAR2,
P_NAME_VI IN VARCHAR2,
P_NAME_EN IN VARCHAR2,
P_NAME_CN IN VARCHAR2,
P_MODIFIED_BY IN VARCHAR2,
P_MODIFIED_DATE IN DATE,
P_RETURN OUT NUMBER
);


PROCEDURE PROC_CLASS_DELETE
(
P_ID IN NUMBER,
P_MODIFIED_BY IN VARCHAR2,
P_MODIFIED_DATE IN DATE,
P_RETURN OUT NUMBER
);

PROCEDURE PROC_LAWER_SEARCH
(
    P_SEARCH_KEY IN VARCHAR2,
    P_FROM IN VARCHAR2,
    P_TO IN VARCHAR2,
    P_SORT_TYPE IN VARCHAR2,
    P_TOTAL_RECORD OUT NUMBER,
    P_CURSOR OUT TCURSOR
);


PROCEDURE PROC_LAWER_GET_BY_ID
(
    P_ID IN NUMBER,
    P_CURSOR OUT TCURSOR
);


END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY PKG_APP_CLASS
IS

PROCEDURE PROC_CLASS_ADD
(
P_ID IN NUMBER,
P_CODE IN VARCHAR2,
P_NAME_VI IN VARCHAR2,
P_NAME_EN IN VARCHAR2,
P_NAME_CN IN VARCHAR2,
P_CREATED_BY IN VARCHAR2,
P_CREATED_DATE IN DATE,
P_RETURN OUT NUMBER
)
IS
V_ID NUMBER;
V_COUNT NUMBER;
BEGIN
P_RETURN := PKG_COMMON.SUCESS ;

SELECT COUNT(0) INTO V_COUNT FROM APP_CLASS_INFO A WHERE UPPER(A.CODE) = UPPER(P_CODE) AND DELETED = 0;

IF V_COUNT > 0 THEN
P_RETURN := -2;
RETURN;
END IF;

SELECT SEQ_APP_CLASS_INFO.NEXTVAL INTO V_ID FROM DUAL;

INSERT INTO APP_CLASS_INFO (ID, CODE, NAME_VI, NAME_EN, NAME_CN, CREATED_BY, CREATED_DATE)
VALUES( V_ID, P_CODE,  P_NAME_VI, P_NAME_EN, P_NAME_CN,  P_CREATED_BY, P_CREATED_DATE);
P_RETURN:= V_ID;
 COMMIT;

EXCEPTION
WHEN OTHERS THEN
P_RETURN:= PKG_COMMON.ERROR ;
RAISE;
ROLLBACK;
END;




PROCEDURE PROC_CLASS_UPDATE
(
P_ID IN NUMBER,
P_CODE IN VARCHAR2,
P_NAME_VI IN VARCHAR2,
P_NAME_EN IN VARCHAR2,
P_NAME_CN IN VARCHAR2,
P_MODIFIED_BY IN VARCHAR2,
P_MODIFIED_DATE IN DATE,
P_RETURN OUT NUMBER
)
IS
V_ID NUMBER;
V_COUNT NUMBER;
BEGIN
P_RETURN := PKG_COMMON.SUCESS ;

SELECT COUNT(0) INTO V_COUNT FROM APP_CLASS_INFO A WHERE UPPER(A.CODE) = UPPER(P_CODE) AND DELETED = 0 AND ID <> P_ID;

IF V_COUNT > 0 THEN
P_RETURN := -2;
RETURN;
END IF;

UPDATE  APP_CLASS_INFO SET CODE = P_CODE, NAME_VI = P_NAME_VI, NAME_EN = P_NAME_EN, NAME_CN =  P_NAME_CN,
  MODIFIED_BY = P_MODIFIED_BY, MODIFIED_DATE = P_MODIFIED_DATE WHERE ID = V_ID;
 COMMIT;


EXCEPTION
WHEN OTHERS THEN
P_RETURN:= PKG_COMMON.ERROR ;
RAISE;
ROLLBACK;
END;



PROCEDURE PROC_CLASS_DELETE
(
P_ID IN NUMBER,
P_MODIFIED_BY IN VARCHAR2,
P_MODIFIED_DATE IN DATE,
P_RETURN OUT NUMBER
)
IS
V_ID NUMBER;
V_COUNT NUMBER;
BEGIN
P_RETURN := PKG_COMMON.SUCESS ;

UPDATE  APP_CLASS_INFO SET DELETED = 1,
  MODIFIED_BY = P_MODIFIED_BY, MODIFIED_DATE = P_MODIFIED_DATE WHERE ID = V_ID;
 COMMIT;


EXCEPTION
WHEN OTHERS THEN
P_RETURN:= PKG_COMMON.ERROR ;
RAISE;
ROLLBACK;
END;



PROCEDURE PROC_LAWER_SEARCH
(
    P_SEARCH_KEY IN VARCHAR2,
    P_FROM IN VARCHAR2,
    P_TO IN VARCHAR2,
    P_SORT_TYPE IN VARCHAR2,
    P_TOTAL_RECORD OUT NUMBER,
    P_CURSOR OUT TCURSOR
)
IS
    V_SQL VARCHAR2(32000) DEFAULT '';
    V_CONDITION VARCHAR2(4000) DEFAULT '';
    V_TOTAL_COUNT VARCHAR2(32000) DEFAULT '';

    V_INDEX NUMBER DEFAULT 0;
    CURSOR V_CUR_CONDITION IS
        SELECT COLUMN_VALUE AS CONDITION FROM TABLE(FN_PARSER(P_SEARCH_KEY,'|'));

    V_STATUS VARCHAR2(3) DEFAULT 'ALL';
    V_CLASS_CODE VARCHAR2(100) DEFAULT 'ALL';
    V_CLASS_NAME_VI VARCHAR2(100) DEFAULT 'ALL';
    V_CLASS_NAME_EN VARCHAR2(100) DEFAULT 'ALL';
    V_CLASS_NAME_CN VARCHAR2(100) DEFAULT 'ALL';


    V_FROM NUMBER ;
    V_TO NUMBER;
BEGIN
    -- LAY RA CAC DK TIM KIEM
    FOR ITEM IN V_CUR_CONDITION LOOP

        IF V_INDEX = 0 THEN
            V_CLASS_CODE := ITEM.CONDITION;
        END IF;
         IF V_INDEX = 1 THEN
            V_CLASS_NAME_VI := ITEM.CONDITION;
        END IF;
         IF V_INDEX = 2 THEN
            V_CLASS_NAME_EN := ITEM.CONDITION;
        END IF;
         IF V_INDEX = 3 THEN
            V_CLASS_NAME_CN := ITEM.CONDITION;
        END IF;

        V_INDEX := V_INDEX + 1;
    END LOOP;

    IF V_CLASS_CODE <> 'ALL' THEN
        V_CONDITION :=  V_CONDITION || ' AND UPPER(A.CODE) LIKE ''%' || UPPER(V_CLASS_CODE) || '%'' ';
    END IF;
    IF V_CLASS_NAME_VI <> 'ALL' THEN
        V_CONDITION :=  V_CONDITION || ' AND UPPER(A.NAME_VI) LIKE ''%' || UPPER(V_CLASS_NAME_VI) || '%'' ';
    END IF;
    IF V_CLASS_NAME_EN <> 'ALL' THEN
        V_CONDITION :=  V_CONDITION || ' AND UPPER(A.NAME_EN) LIKE ''%' || UPPER(V_CLASS_NAME_EN) || '%'' ';
    END IF;
    IF V_CLASS_NAME_CN <> 'ALL' THEN
        V_CONDITION :=  V_CONDITION || ' AND UPPER(A.NAME_CN) LIKE ''%' || UPPER(V_CLASS_NAME_CN) || '%'' ';
    END IF;


    V_SQL := 'SELECT * FROM APP_CLASS_INFO A WHERE 1 = 1 ';

    V_SQL :=  V_SQL || V_CONDITION;

    IF P_SORT_TYPE <> 'ALL' THEN
        V_SQL :=  V_SQL || P_SORT_TYPE;
    ELSE
        V_SQL :=  V_SQL || ' ORDER BY A.CODE';
    END IF;

   -- DBMS_OUTPUT.PUT_LINE(V_SQL);


    V_TOTAL_COUNT := 'SELECT COUNT(*) FROM (' || V_SQL || ')';
    EXECUTE IMMEDIATE V_TOTAL_COUNT INTO V_TOTAL_COUNT;

    V_FROM := P_FROM ;
    V_TO := P_TO;

    IF P_FROM = '0' AND P_TO = '0' THEN
        V_FROM := '1' ;
        V_TO := V_TOTAL_COUNT;
    END IF;

    V_SQL := 'SELECT * FROM ( SELECT ROWNUM STT, V.* FROM( ' || V_SQL || ') V ) A WHERE STT BETWEEN ' || V_FROM || ' AND ' || V_TO;
    OPEN P_CURSOR FOR V_SQL;
    --PKG_LOG.LOGMSG(V_SQL);


    P_TOTAL_RECORD := V_TOTAL_COUNT;

EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;



PROCEDURE PROC_LAWER_GET_BY_ID
(
    P_ID IN NUMBER,
    P_CURSOR OUT TCURSOR
)
IS
 BEGIN
  OPEN P_CURSOR FOR SELECT * FROM APP_CLASS_INFO WHERE ID = P_ID;
 EXCEPTION
WHEN OTHERS THEN
    RAISE;
END;

END;
/

