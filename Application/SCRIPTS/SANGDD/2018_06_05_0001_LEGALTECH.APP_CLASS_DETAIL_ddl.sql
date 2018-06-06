-- Start of DDL Script for Table LEGALTECH.APP_CLASS_DETAIL
-- Generated 05/06/2018 10:43:26 PM from LEGALTECH@LOCAL

-- Start of DDL Script for Table LEGALTECH.APP_CLASS_DETAIL
-- Generated 05/06/2018 11:05:29 PM from LEGALTECH@LOCAL

CREATE TABLE app_class_detail
    (id                             NUMBER,
    textinput                      VARCHAR2(200 CHAR),
    code                           VARCHAR2(30 CHAR),
    app_header_id                  NUMBER(20,0))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
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





-- Comments for APP_CLASS_DETAIL

COMMENT ON COLUMN app_class_detail.code IS 'M� Nh�m class'
/
COMMENT ON COLUMN app_class_detail.id IS 'ID t? tang'
/
COMMENT ON COLUMN app_class_detail.textinput IS 'N?i dung ngu?i d�ng nh?p'
/

-- End of DDL Script for Table LEGALTECH.APP_CLASS_DETAIL


-- End of DDL Script for Table LEGALTECH.APP_CLASS_DETAIL



-- Start of DDL Script for Sequence LEGALTECH.SEQ_APP_CLASS_DETAIL
-- Generated 05/06/2018 10:57:42 PM from LEGALTECH@LOCAL

CREATE SEQUENCE seq_app_class_detail
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20
/


-- End of DDL Script for Sequence LEGALTECH.SEQ_APP_CLASS_DETAIL
/

-- Start of DDL Script for Package LEGALTECH.PKG_APPCLASS_DETAIL
-- Generated 05/06/2018 11:08:02 PM from LEGALTECH@LOCAL
-- Start of DDL Script for Package LEGALTECH.PKG_APPCLASS_DETAIL
-- Generated 05/06/2018 11:30:15 PM from LEGALTECH@LOCAL

CREATE OR REPLACE 
PACKAGE pkg_appclass_detail
  IS

PROCEDURE PROC_APP_CLASS_DETAIL_INSERT
(

    P_TEXTINPUT IN APP_CLASS_DETAIL.TEXTINPUT % TYPE,
    P_CODE IN APP_CLASS_DETAIL.CODE % TYPE ,
    P_APP_HEADER_ID IN NUMBER ,
    P_RETURN OUT NUMBER
);
PROCEDURE PROC_APP_CLASS_DETAIL_DELETE
(

    P_APP_HEADER_ID IN NUMBER ,
    P_RETURN OUT NUMBER
);

END; -- PACKAGE SPEC
/


CREATE OR REPLACE 
PACKAGE BODY pkg_appclass_detail
IS

PROCEDURE PROC_APP_CLASS_DETAIL_INSERT
(

    P_TEXTINPUT IN APP_CLASS_DETAIL.TEXTINPUT % TYPE,
    P_CODE IN APP_CLASS_DETAIL.CODE % TYPE ,
    P_APP_HEADER_ID IN NUMBER ,
    P_RETURN OUT NUMBER
)
IS
V_ID NUMBER := 0 ;

BEGIN
    P_RETURN:= PKG_COMMON.SUCESS ;
    SELECT SEQ_APP_CLASS_DETAIL.NEXTVAL INTO V_ID FROM DUAL;
    INSERT INTO APP_CLASS_DETAIL ( ID, TEXTINPUT, CODE ,  APP_HEADER_ID  )
    VALUES ( V_ID, P_TEXTINPUT, P_CODE ,P_APP_HEADER_ID);

--KHONG COMMIT INSERT BATH
EXCEPTION WHEN OTHERS THEN
RAISE;
  P_RETURN:= PKG_COMMON.ERROR ;
END;



PROCEDURE PROC_APP_CLASS_DETAIL_DELETE
(

    P_APP_HEADER_ID IN NUMBER ,
    P_RETURN OUT NUMBER
)
IS


BEGIN
    P_RETURN:= PKG_COMMON.SUCESS ;
     DELETE APP_CLASS_DETAIL WHERE  APP_HEADER_ID = P_APP_HEADER_ID ;

--KHONG COMMIT INSERT BATH
EXCEPTION WHEN OTHERS THEN
RAISE;
  P_RETURN:= PKG_COMMON.ERROR ;
END;

   -- ENTER FURTHER CODE BELOW AS SPECIFIED IN THE PACKAGE SPEC.
END;
/


-- End of DDL Script for Package LEGALTECH.PKG_APPCLASS_DETAIL



-- End of DDL Script for Package LEGALTECH.PKG_APPCLASS_DETAIL

