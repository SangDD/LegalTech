﻿ALTER TABLE APP_DETAIL_A03 
 ADD (
  SO_PA NUMBER (20) DEFAULT 0,
  SO_HA NUMBER (20) DEFAULT 0
 )
 MODIFY (
  SOHINHPHATSINH NUMBER (20) DEFAULT 0

 )
/
COMMENT ON COLUMN APP_DETAIL_A03.SO_PA IS 'so phuong an'
/
COMMENT ON COLUMN APP_DETAIL_A03.SO_HA IS 'so hinh anh'
/