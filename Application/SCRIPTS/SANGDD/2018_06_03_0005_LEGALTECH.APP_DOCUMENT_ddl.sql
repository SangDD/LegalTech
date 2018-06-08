-- Start of DDL Script for Table LEGALTECH.APP_DOCUMENT
-- Generated 03/06/2018 3:21:43 PM from LEGALTECH@LOCAL
-- Start of DDL Script for Sequence LEGALTECH.SEQ_APP_DOCUMENT
-- Generated 03/06/2018 3:20:59 PM from LEGALTECH@LOCAL

CREATE SEQUENCE seq_app_document
  INCREMENT BY 1
  START WITH 1
  MINVALUE 1
  MAXVALUE 9999999999999999999999999999
  NOCYCLE
  NOORDER
  CACHE 20
/


-- End of DDL Script for Sequence LEGALTECH.SEQ_APP_DOCUMENT


CREATE TABLE app_document
    (id                             NUMBER,
    language_code                  VARCHAR2(5 CHAR),
    app_header_id                  NUMBER,
    document_id                    VARCHAR2(10 CHAR),
    isuse                          NUMBER,
    note                           VARCHAR2(250 CHAR),
    status                         NUMBER DEFAULT 0,
    document_filing_date           DATE,
    filename                       VARCHAR2(250 CHAR),
    url_hardcopy                   VARCHAR2(250 CHAR),
    char01                         VARCHAR2(100 CHAR),
    char02                         VARCHAR2(100 CHAR),
    char03                         VARCHAR2(100 CHAR),
    char04                         VARCHAR2(100 CHAR),
    char05                         VARCHAR2(100 CHAR))
  SEGMENT CREATION IMMEDIATE
  PCTFREE     10
  INITRANS    1
  MAXTRANS    255
  TABLESPACE  users
  STORAGE   (
    INITIAL     131072
    NEXT        1048576
    MINEXTENTS  1
    MAXEXTENTS  2147483645
  )
  NOCACHE
  MONITORING
  NOPARALLEL
  LOGGING
/





-- Comments for APP_DOCUMENT

COMMENT ON COLUMN app_document.document_id IS 'TAM THOI FIXED CUNG DI '
/
COMMENT ON COLUMN app_document.filename IS 'TEN FILE TAI LEN'
/
COMMENT ON COLUMN app_document.note IS 'NOI DUNG, SO TRANG, TIENG VIET ,TIENG ANH'
/
COMMENT ON COLUMN app_document.status IS '1:DA NOP BAN CUNG,0:CHUA NOP BAN CUNG'
/

-- End of DDL Script for Table LEGALTECH.APP_DOCUMENT


/

-- Start of DDL Script for Package LEGALTECH.PKG_APP_DOCUMENT
-- Generated 03/06/2018 3:31:15 PM from LEGALTECH@LOCAL
-- Start of DDL Script for Package LEGALTECH.PKG_APP_DETAIL_04NH
-- Generated 05/06/2018 10:32:45 PM from LEGALTECH@LOCAL

CREATE OR REPLACE 
PACKAGE pkg_app_detail_04nh
  IS
 PROCEDURE PROC_APP_DETAIL_04NH_INSERT
(
    P_APP_HEADER_ID IN APP_DETAIL_04NH.APP_HEADER_ID % TYPE,
    P_APPCODE IN APP_DETAIL_04NH.APPCODE % TYPE,
    P_LANGUAGE_CODE IN APP_DETAIL_04NH.LANGUAGE_CODE % TYPE,
    P_APPNO IN APP_DETAIL_04NH.APPNO % TYPE,
    P_DUADATE IN APP_DETAIL_04NH.DUADATE % TYPE,
    P_LOGOURL IN APP_DETAIL_04NH.LOGOURL % TYPE,
    P_DACTICHHANGHOA IN APP_DETAIL_04NH.DACTICHHANGHOA % TYPE,
    P_COLOR IN APP_DETAIL_04NH.COLOR % TYPE,
    P_DESCRIPTION IN APP_DETAIL_04NH.DESCRIPTION % TYPE,
    P_HUONGQUYENUUTIEN IN APP_DETAIL_04NH.HUONGQUYENUUTIEN % TYPE,
    P_SODON_UT IN APP_DETAIL_04NH.SODON_UT % TYPE,
    P_NGAYNOPDON_UT IN APP_DETAIL_04NH.NGAYNOPDON_UT % TYPE,
    P_NUOCNOPDON_UT IN APP_DETAIL_04NH.NUOCNOPDON_UT % TYPE,
    P_NGUONGOCDIALY IN APP_DETAIL_04NH.NGUONGOCDIALY % TYPE,
    P_CHATLUONG IN APP_DETAIL_04NH.CHATLUONG % TYPE,
    P_DACTINHKHAC IN APP_DETAIL_04NH.DACTINHKHAC % TYPE,
    P_CDK_NAME_1 IN APP_DETAIL_04NH.CDK_NAME_1 % TYPE,
    P_CDK_ADDRESS_1 IN APP_DETAIL_04NH.CDK_ADDRESS_1 % TYPE,
    P_CDK_PHONE_1 IN APP_DETAIL_04NH.CDK_PHONE_1 % TYPE,
    P_CDK_FAX_1 IN APP_DETAIL_04NH.CDK_FAX_1 % TYPE,
    P_CDK_EMAIL_1 IN APP_DETAIL_04NH.CDK_EMAIL_1 % TYPE,
    P_CDK_NAME_2 IN APP_DETAIL_04NH.CDK_NAME_2 % TYPE,
    P_CDK_ADDRESS_2 IN APP_DETAIL_04NH.CDK_ADDRESS_2 % TYPE,
    P_CDK_PHONE_2 IN APP_DETAIL_04NH.CDK_PHONE_2 % TYPE,
    P_CDK_FAX_2 IN APP_DETAIL_04NH.CDK_FAX_2 % TYPE,
    P_CDK_EMAIL_2 IN APP_DETAIL_04NH.CDK_EMAIL_2 % TYPE,
    P_CDK_NAME_3 IN APP_DETAIL_04NH.CDK_NAME_3 % TYPE,
    P_CDK_ADDRESS_3 IN APP_DETAIL_04NH.CDK_ADDRESS_3 % TYPE,
    P_CDK_PHONE_3 IN APP_DETAIL_04NH.CDK_PHONE_3 % TYPE,
    P_CDK_FAX_3 IN APP_DETAIL_04NH.CDK_FAX_3 % TYPE,
    P_CDK_EMAIL_3 IN APP_DETAIL_04NH.CDK_EMAIL_3 % TYPE,
    P_CDK_NAME_4 IN APP_DETAIL_04NH.CDK_NAME_4 % TYPE,
    P_CDK_ADDRESS_4 IN APP_DETAIL_04NH.CDK_ADDRESS_4 % TYPE,
    P_CDK_PHONE_4 IN APP_DETAIL_04NH.CDK_PHONE_4 % TYPE,
    P_CDK_FAX_4 IN APP_DETAIL_04NH.CDK_FAX_4 % TYPE,
    P_CDK_EMAIL_4 IN APP_DETAIL_04NH.CDK_EMAIL_4 % TYPE,
    P_USED_SPECIAL IN APP_DETAIL_04NH.USED_SPECIAL % TYPE ,
    P_RETURN OUT NUMBER
);

PROCEDURE PROC_APP_DETAIL_04NH_UPDATE
(
    P_ID IN APP_DETAIL_04NH.ID % TYPE,
    P_APP_HEADER_ID IN APP_DETAIL_04NH.APP_HEADER_ID % TYPE,
    P_APPCODE IN APP_DETAIL_04NH.APPCODE % TYPE,
    P_LANGUAGE_CODE IN APP_DETAIL_04NH.LANGUAGE_CODE % TYPE,
    P_APPNO IN APP_DETAIL_04NH.APPNO % TYPE,
    P_DUADATE IN APP_DETAIL_04NH.DUADATE % TYPE,
    P_LOGOURL IN APP_DETAIL_04NH.LOGOURL % TYPE,
    P_DACTICHHANGHOA IN APP_DETAIL_04NH.DACTICHHANGHOA % TYPE,
    P_COLOR IN APP_DETAIL_04NH.COLOR % TYPE,
    P_DESCRIPTION IN APP_DETAIL_04NH.DESCRIPTION % TYPE,
    P_HUONGQUYENUUTIEN IN APP_DETAIL_04NH.HUONGQUYENUUTIEN % TYPE,
    P_SODON_UT IN APP_DETAIL_04NH.SODON_UT % TYPE,
    P_NGAYNOPDON_UT IN APP_DETAIL_04NH.NGAYNOPDON_UT % TYPE,
    P_NUOCNOPDON_UT IN APP_DETAIL_04NH.NUOCNOPDON_UT % TYPE,
    P_NGUONGOCDIALY IN APP_DETAIL_04NH.NGUONGOCDIALY % TYPE,
    P_CHATLUONG IN APP_DETAIL_04NH.CHATLUONG % TYPE,
    P_DACTINHKHAC IN APP_DETAIL_04NH.DACTINHKHAC % TYPE,
    P_CDK_NAME_1 IN APP_DETAIL_04NH.CDK_NAME_1 % TYPE,
    P_CDK_ADDRESS_1 IN APP_DETAIL_04NH.CDK_ADDRESS_1 % TYPE,
    P_CDK_PHONE_1 IN APP_DETAIL_04NH.CDK_PHONE_1 % TYPE,
    P_CDK_FAX_1 IN APP_DETAIL_04NH.CDK_FAX_1 % TYPE,
    P_CDK_EMAIL_1 IN APP_DETAIL_04NH.CDK_EMAIL_1 % TYPE,
    P_CDK_NAME_2 IN APP_DETAIL_04NH.CDK_NAME_2 % TYPE,
    P_CDK_ADDRESS_2 IN APP_DETAIL_04NH.CDK_ADDRESS_2 % TYPE,
    P_CDK_PHONE_2 IN APP_DETAIL_04NH.CDK_PHONE_2 % TYPE,
    P_CDK_FAX_2 IN APP_DETAIL_04NH.CDK_FAX_2 % TYPE,
    P_CDK_EMAIL_2 IN APP_DETAIL_04NH.CDK_EMAIL_2 % TYPE,
    P_CDK_NAME_3 IN APP_DETAIL_04NH.CDK_NAME_3 % TYPE,
    P_CDK_ADDRESS_3 IN APP_DETAIL_04NH.CDK_ADDRESS_3 % TYPE,
    P_CDK_PHONE_3 IN APP_DETAIL_04NH.CDK_PHONE_3 % TYPE,
    P_CDK_FAX_3 IN APP_DETAIL_04NH.CDK_FAX_3 % TYPE,
    P_CDK_EMAIL_3 IN APP_DETAIL_04NH.CDK_EMAIL_3 % TYPE,
    P_CDK_NAME_4 IN APP_DETAIL_04NH.CDK_NAME_4 % TYPE,
    P_CDK_ADDRESS_4 IN APP_DETAIL_04NH.CDK_ADDRESS_4 % TYPE,
    P_CDK_PHONE_4 IN APP_DETAIL_04NH.CDK_PHONE_4 % TYPE,
    P_CDK_FAX_4 IN APP_DETAIL_04NH.CDK_FAX_4 % TYPE,
    P_CDK_EMAIL_4 IN APP_DETAIL_04NH.CDK_EMAIL_4 % TYPE,
    P_USED_SPECIAL IN APP_DETAIL_04NH.USED_SPECIAL % TYPE ,
    P_RETURN OUT NUMBER
);



PROCEDURE PROC_APP_DETAIL_04NH_DELETE (

    P_APP_HEADER_ID IN APP_DETAIL_04NH.APP_HEADER_ID % TYPE,
    P_APPCODE IN APP_DETAIL_04NH.APPCODE % TYPE,
    P_LANGUAGE_CODE IN APP_DETAIL_04NH.LANGUAGE_CODE % TYPE,
    P_RETURN OUT NUMBER
);

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_app_detail_04nh
IS

PROCEDURE PROC_APP_DETAIL_04NH_INSERT
(
    P_APP_HEADER_ID IN APP_DETAIL_04NH.APP_HEADER_ID % TYPE,
    P_APPCODE IN APP_DETAIL_04NH.APPCODE % TYPE,
    P_LANGUAGE_CODE IN APP_DETAIL_04NH.LANGUAGE_CODE % TYPE,
    P_APPNO IN APP_DETAIL_04NH.APPNO % TYPE,
    P_DUADATE IN APP_DETAIL_04NH.DUADATE % TYPE,
    P_LOGOURL IN APP_DETAIL_04NH.LOGOURL % TYPE,
    P_DACTICHHANGHOA IN APP_DETAIL_04NH.DACTICHHANGHOA % TYPE,
    P_COLOR IN APP_DETAIL_04NH.COLOR % TYPE,
    P_DESCRIPTION IN APP_DETAIL_04NH.DESCRIPTION % TYPE,
    P_HUONGQUYENUUTIEN IN APP_DETAIL_04NH.HUONGQUYENUUTIEN % TYPE,
    P_SODON_UT IN APP_DETAIL_04NH.SODON_UT % TYPE,
    P_NGAYNOPDON_UT IN APP_DETAIL_04NH.NGAYNOPDON_UT % TYPE,
    P_NUOCNOPDON_UT IN APP_DETAIL_04NH.NUOCNOPDON_UT % TYPE,
    P_NGUONGOCDIALY IN APP_DETAIL_04NH.NGUONGOCDIALY % TYPE,
    P_CHATLUONG IN APP_DETAIL_04NH.CHATLUONG % TYPE,
    P_DACTINHKHAC IN APP_DETAIL_04NH.DACTINHKHAC % TYPE,
    P_CDK_NAME_1 IN APP_DETAIL_04NH.CDK_NAME_1 % TYPE,
    P_CDK_ADDRESS_1 IN APP_DETAIL_04NH.CDK_ADDRESS_1 % TYPE,
    P_CDK_PHONE_1 IN APP_DETAIL_04NH.CDK_PHONE_1 % TYPE,
    P_CDK_FAX_1 IN APP_DETAIL_04NH.CDK_FAX_1 % TYPE,
    P_CDK_EMAIL_1 IN APP_DETAIL_04NH.CDK_EMAIL_1 % TYPE,
    P_CDK_NAME_2 IN APP_DETAIL_04NH.CDK_NAME_2 % TYPE,
    P_CDK_ADDRESS_2 IN APP_DETAIL_04NH.CDK_ADDRESS_2 % TYPE,
    P_CDK_PHONE_2 IN APP_DETAIL_04NH.CDK_PHONE_2 % TYPE,
    P_CDK_FAX_2 IN APP_DETAIL_04NH.CDK_FAX_2 % TYPE,
    P_CDK_EMAIL_2 IN APP_DETAIL_04NH.CDK_EMAIL_2 % TYPE,
    P_CDK_NAME_3 IN APP_DETAIL_04NH.CDK_NAME_3 % TYPE,
    P_CDK_ADDRESS_3 IN APP_DETAIL_04NH.CDK_ADDRESS_3 % TYPE,
    P_CDK_PHONE_3 IN APP_DETAIL_04NH.CDK_PHONE_3 % TYPE,
    P_CDK_FAX_3 IN APP_DETAIL_04NH.CDK_FAX_3 % TYPE,
    P_CDK_EMAIL_3 IN APP_DETAIL_04NH.CDK_EMAIL_3 % TYPE,
    P_CDK_NAME_4 IN APP_DETAIL_04NH.CDK_NAME_4 % TYPE,
    P_CDK_ADDRESS_4 IN APP_DETAIL_04NH.CDK_ADDRESS_4 % TYPE,
    P_CDK_PHONE_4 IN APP_DETAIL_04NH.CDK_PHONE_4 % TYPE,
    P_CDK_FAX_4 IN APP_DETAIL_04NH.CDK_FAX_4 % TYPE,
    P_CDK_EMAIL_4 IN APP_DETAIL_04NH.CDK_EMAIL_4 % TYPE,
    P_USED_SPECIAL IN APP_DETAIL_04NH.USED_SPECIAL % TYPE ,
    P_RETURN OUT NUMBER
)
IS
V_ID NUMBER  ;

BEGIN

    P_RETURN:= PKG_COMMON.SUCESS ;
    SELECT SEQ_APP_DETAIL_01.NEXTVAL INTO V_ID FROM DUAL;

    INSERT INTO APP_DETAIL_04NH
    (
            ID, APP_HEADER_ID, APPCODE,  LANGUAGE_CODE, APPNO, DUADATE, LOGOURL, DACTICHHANGHOA,
            COLOR, DESCRIPTION, HUONGQUYENUUTIEN, SODON_UT, NGAYNOPDON_UT, NUOCNOPDON_UT, NGUONGOCDIALY, CHATLUONG,
            DACTINHKHAC, CDK_NAME_1, CDK_ADDRESS_1, CDK_PHONE_1, CDK_FAX_1, CDK_EMAIL_1, CDK_NAME_2, CDK_ADDRESS_2,
            CDK_PHONE_2, CDK_FAX_2, CDK_EMAIL_2, CDK_NAME_3, CDK_ADDRESS_3, CDK_PHONE_3, CDK_FAX_3, CDK_EMAIL_3,
            CDK_NAME_4, CDK_ADDRESS_4, CDK_PHONE_4, CDK_FAX_4, CDK_EMAIL_4, USED_SPECIAL
    )
    VALUES
    (
        V_ID, P_APP_HEADER_ID, P_APPCODE, P_LANGUAGE_CODE, P_APPNO, TRUNC(P_DUADATE), P_LOGOURL, P_DACTICHHANGHOA,
        P_COLOR, P_DESCRIPTION, P_HUONGQUYENUUTIEN, P_SODON_UT, TRUNC(P_NGAYNOPDON_UT), P_NUOCNOPDON_UT, P_NGUONGOCDIALY, P_CHATLUONG,
        P_DACTINHKHAC, P_CDK_NAME_1, P_CDK_ADDRESS_1, P_CDK_PHONE_1, P_CDK_FAX_1, P_CDK_EMAIL_1, P_CDK_NAME_2, P_CDK_ADDRESS_2,
        P_CDK_PHONE_2, P_CDK_FAX_2, P_CDK_EMAIL_2, P_CDK_NAME_3, P_CDK_ADDRESS_3, P_CDK_PHONE_3, P_CDK_FAX_3, P_CDK_EMAIL_3,
        P_CDK_NAME_4, P_CDK_ADDRESS_4, P_CDK_PHONE_4, P_CDK_FAX_4, P_CDK_EMAIL_4, P_USED_SPECIAL
    );

--COMMIT; KHONG COMMIT DANG DUNG TRACSACTION
EXCEPTION WHEN OTHERS THEN
RAISE;
  P_RETURN:= PKG_COMMON.ERROR ;
END;




PROCEDURE PROC_APP_DETAIL_04NH_UPDATE
(
    P_ID IN APP_DETAIL_04NH.ID % TYPE,
    P_APP_HEADER_ID IN APP_DETAIL_04NH.APP_HEADER_ID % TYPE,
    P_APPCODE IN APP_DETAIL_04NH.APPCODE % TYPE,
    P_LANGUAGE_CODE IN APP_DETAIL_04NH.LANGUAGE_CODE % TYPE,
    P_APPNO IN APP_DETAIL_04NH.APPNO % TYPE,
    P_DUADATE IN APP_DETAIL_04NH.DUADATE % TYPE,
    P_LOGOURL IN APP_DETAIL_04NH.LOGOURL % TYPE,
    P_DACTICHHANGHOA IN APP_DETAIL_04NH.DACTICHHANGHOA % TYPE,
    P_COLOR IN APP_DETAIL_04NH.COLOR % TYPE,
    P_DESCRIPTION IN APP_DETAIL_04NH.DESCRIPTION % TYPE,
    P_HUONGQUYENUUTIEN IN APP_DETAIL_04NH.HUONGQUYENUUTIEN % TYPE,
    P_SODON_UT IN APP_DETAIL_04NH.SODON_UT % TYPE,
    P_NGAYNOPDON_UT IN APP_DETAIL_04NH.NGAYNOPDON_UT % TYPE,
    P_NUOCNOPDON_UT IN APP_DETAIL_04NH.NUOCNOPDON_UT % TYPE,
    P_NGUONGOCDIALY IN APP_DETAIL_04NH.NGUONGOCDIALY % TYPE,
    P_CHATLUONG IN APP_DETAIL_04NH.CHATLUONG % TYPE,
    P_DACTINHKHAC IN APP_DETAIL_04NH.DACTINHKHAC % TYPE,
    P_CDK_NAME_1 IN APP_DETAIL_04NH.CDK_NAME_1 % TYPE,
    P_CDK_ADDRESS_1 IN APP_DETAIL_04NH.CDK_ADDRESS_1 % TYPE,
    P_CDK_PHONE_1 IN APP_DETAIL_04NH.CDK_PHONE_1 % TYPE,
    P_CDK_FAX_1 IN APP_DETAIL_04NH.CDK_FAX_1 % TYPE,
    P_CDK_EMAIL_1 IN APP_DETAIL_04NH.CDK_EMAIL_1 % TYPE,
    P_CDK_NAME_2 IN APP_DETAIL_04NH.CDK_NAME_2 % TYPE,
    P_CDK_ADDRESS_2 IN APP_DETAIL_04NH.CDK_ADDRESS_2 % TYPE,
    P_CDK_PHONE_2 IN APP_DETAIL_04NH.CDK_PHONE_2 % TYPE,
    P_CDK_FAX_2 IN APP_DETAIL_04NH.CDK_FAX_2 % TYPE,
    P_CDK_EMAIL_2 IN APP_DETAIL_04NH.CDK_EMAIL_2 % TYPE,
    P_CDK_NAME_3 IN APP_DETAIL_04NH.CDK_NAME_3 % TYPE,
    P_CDK_ADDRESS_3 IN APP_DETAIL_04NH.CDK_ADDRESS_3 % TYPE,
    P_CDK_PHONE_3 IN APP_DETAIL_04NH.CDK_PHONE_3 % TYPE,
    P_CDK_FAX_3 IN APP_DETAIL_04NH.CDK_FAX_3 % TYPE,
    P_CDK_EMAIL_3 IN APP_DETAIL_04NH.CDK_EMAIL_3 % TYPE,
    P_CDK_NAME_4 IN APP_DETAIL_04NH.CDK_NAME_4 % TYPE,
    P_CDK_ADDRESS_4 IN APP_DETAIL_04NH.CDK_ADDRESS_4 % TYPE,
    P_CDK_PHONE_4 IN APP_DETAIL_04NH.CDK_PHONE_4 % TYPE,
    P_CDK_FAX_4 IN APP_DETAIL_04NH.CDK_FAX_4 % TYPE,
    P_CDK_EMAIL_4 IN APP_DETAIL_04NH.CDK_EMAIL_4 % TYPE,
    P_USED_SPECIAL IN APP_DETAIL_04NH.USED_SPECIAL % TYPE ,
    P_RETURN OUT NUMBER
)
IS
BEGIN
    P_RETURN:= PKG_COMMON.SUCESS ;
    UPDATE APP_DETAIL_04NH SET
    APPNO = P_APPNO,   DUADATE = TRUNC(P_DUADATE), LOGOURL = P_LOGOURL,
    DACTICHHANGHOA = P_DACTICHHANGHOA,   COLOR = P_COLOR,
    DESCRIPTION = P_DESCRIPTION,   HUONGQUYENUUTIEN = P_HUONGQUYENUUTIEN,  SODON_UT = P_SODON_UT,
    NGAYNOPDON_UT = TRUNC(P_NGAYNOPDON_UT), NUOCNOPDON_UT = P_NUOCNOPDON_UT,
    NGUONGOCDIALY = P_NGUONGOCDIALY,   CHATLUONG = P_CHATLUONG,
    DACTINHKHAC = P_DACTINHKHAC,  CDK_NAME_1 = P_CDK_NAME_1,
    CDK_ADDRESS_1 = P_CDK_ADDRESS_1,   CDK_PHONE_1 = P_CDK_PHONE_1,
    CDK_FAX_1 = P_CDK_FAX_1,  CDK_EMAIL_1 = P_CDK_EMAIL_1,
    CDK_NAME_2 = P_CDK_NAME_2,   CDK_ADDRESS_2 = P_CDK_ADDRESS_2,
    CDK_PHONE_2 = P_CDK_PHONE_2,  CDK_FAX_2 = P_CDK_FAX_2,
    CDK_EMAIL_2 = P_CDK_EMAIL_2,  CDK_NAME_3 = P_CDK_NAME_3,
    CDK_ADDRESS_3 = P_CDK_ADDRESS_3,  CDK_PHONE_3 = P_CDK_PHONE_3,
    CDK_FAX_3 = P_CDK_FAX_3,  CDK_EMAIL_3 = P_CDK_EMAIL_3,
    CDK_NAME_4 = P_CDK_NAME_4,  CDK_ADDRESS_4 = P_CDK_ADDRESS_4,
    CDK_PHONE_4 = P_CDK_PHONE_4,   CDK_FAX_4 = P_CDK_FAX_4,
    CDK_EMAIL_4 = P_CDK_EMAIL_4,  USED_SPECIAL = P_USED_SPECIAL  WHERE ID = P_ID;
--COMMIT; KHONG COMMIT DANG DUNG TRACSACTION
EXCEPTION WHEN OTHERS THEN
RAISE;
  P_RETURN:= PKG_COMMON.ERROR ;
END;


PROCEDURE PROC_APP_DETAIL_04NH_DELETE (

    P_APP_HEADER_ID IN APP_DETAIL_04NH.APP_HEADER_ID % TYPE,
    P_APPCODE IN APP_DETAIL_04NH.APPCODE % TYPE,
    P_LANGUAGE_CODE IN APP_DETAIL_04NH.LANGUAGE_CODE % TYPE,
    P_RETURN OUT NUMBER
)
IS
BEGIN
 P_RETURN:= PKG_COMMON.SUCESS ;
 DELETE  APP_DETAIL_04NH WHERE   APP_HEADER_ID = P_APP_HEADER_ID AND  APPCODE = P_APPCODE AND LANGUAGE_CODE = P_LANGUAGE_CODE ;

EXCEPTION WHEN OTHERS THEN
RAISE;
  P_RETURN:= PKG_COMMON.ERROR ;
END  ;



   -- ENTER FURTHER CODE BELOW AS SPECIFIED IN THE PACKAGE SPEC.
END;
/


-- End of DDL Script for Package LEGALTECH.PKG_APP_DETAIL_04NH




-- End of DDL Script for Package LEGALTECH.PKG_APP_DOCUMENT


