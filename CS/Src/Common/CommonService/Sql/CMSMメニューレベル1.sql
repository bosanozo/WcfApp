WITH SL (組織CD, 上位組織CD, 組織階層区分) AS
(SELECT 組織CD, 上位組織CD, 組織階層区分 FROM CMSM組織 WHERE 組織CD = @1
UNION ALL
SELECT A.組織CD, A.上位組織CD, A.組織階層区分 FROM CMSM組織 A
  JOIN SL ON SL.上位組織CD = A.組織CD AND SL.組織階層区分 != '1')
SELECT DISTINCT メニューID, 画面名, FIRST_VALUE(許否フラグ)
  OVER (PARTITION BY メニューID ORDER BY 許否フラグ DESC) 許否フラグ
  FROM (SELECT DISTINCT M.メニューID, 画面名, ロールID, FIRST_VALUE(許否フラグ)
  OVER (PARTITION BY M.メニューID, R.ロールID ORDER BY SL.組織階層区分 DESC) 許否フラグ
  FROM CMSMメニュー M
  LEFT JOIN CMSMメニュー管理 R ON R.ロールID IN (SELECT ロールID FROM CMSMユーザロール WHERE ユーザID = @2)
       AND R.メニューID = M.メニューID
  LEFT JOIN SL ON SL.組織CD = R.組織CD
  WHERE M.メニューID = M.上位メニューID AND 空欄フラグ != 1) MR
ORDER BY メニューID