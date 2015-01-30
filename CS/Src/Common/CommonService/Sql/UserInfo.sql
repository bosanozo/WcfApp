SELECT 
U.ユーザID Id,
U.ユーザ名 Name,
'' Roles,
U.組織CD SoshikiCd,
S1.組織名 SoshikiName,
S1.組織階層区分 SoshikiKaisoKbn
FROM CMSMユーザ U 
JOIN CMSM組織 S1 ON S1.組織CD = U.組織CD 
WHERE U.ユーザID = {0}
