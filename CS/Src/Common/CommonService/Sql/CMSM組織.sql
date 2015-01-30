SELECT
A.組織CD,
A.組織名,
A.組織階層区分,
H1.基準値名 組織階層区分名,
A.上位組織CD,
S2.組織名 上位組織名
-- [共通項目]
FROM CMSM組織 A
LEFT JOIN CMSM汎用基準値 H1 ON H1.分類CD = 'M001' AND H1.基準値CD = A.組織階層区分
LEFT JOIN CMSM組織 S2 ON S2.組織CD = A.上位組織CD
-- [共通JOIN]
-- WHERE [検索条件]
