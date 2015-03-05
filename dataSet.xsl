<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output indent="yes"/>

<!-- アプリケーション -->
<xsl:template match="アプリケーション" >
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:msprop="urn:schemas-microsoft-com:xml-msprop" attributeFormDefault="qualified" elementFormDefault="qualified">
  <xsl:attribute name="id"><xsl:value-of select="ApID"/>DataSet</xsl:attribute>
  <xsl:attribute name="targetNamespace">http://tempuri.org/<xsl:value-of select="ApID"/>DataSet.xsd</xsl:attribute>
  <!--
  <xsl:attribute name="xmlns:mstns">"http://tempuri.org/<xsl:value-of select="ApID"/>DataSet.xsd</xsl:attribute>
  <xsl:attribute name="xmlns">"http://tempuri.org/<xsl:value-of select="ApID"/>DataSet.xsd</xsl:attribute>
  -->

  <!-- データセット -->
  <xs:element msdata:IsDataSet="true" msdata:UseCurrentLocale="true" msprop:EnableTableAdapterManager="true">
    <xsl:attribute name="name"><xsl:value-of select="ApID"/>DataSet</xsl:attribute>

    <!-- データテーブル -->
    <xs:complexType>
      <xs:choice minOccurs="0" maxOccurs="unbounded">
        <xsl:for-each select="データ">
          <xs:element>
            <xsl:attribute name="name">
              <xsl:call-template name="getItemName">
                <xsl:with-param name="ItemName"><xsl:value-of select="テーブル名"/></xsl:with-param>
              </xsl:call-template>
            </xsl:attribute>
            <xs:complexType>
              <xs:sequence>
                <!-- 項目 -->
                <xsl:for-each select="項目">
                  <xs:element>
                    <xsl:attribute name="name">
                      <xsl:call-template name="getItemName">
                        <xsl:with-param name="ItemName"><xsl:value-of select="項目名"/></xsl:with-param>
                      </xsl:call-template>
                    </xsl:attribute>
                    <!-- 入力 -->
                    <xsl:if test="string-length(入力) = 0">
                        <xsl:attribute name="minOccurs">0</xsl:attribute>
                    </xsl:if>
                    <!-- 最大文字数 -->
                    <xsl:choose>
                      <xsl:when test="最大文字数 > 0 and 項目型 != 'Number' and 項目型 != 'Date'">
                        <xs:simpleType>
                          <xs:restriction base="xs:string">
                            <xs:maxLength>
                              <xsl:attribute name="value"><xsl:value-of select="最大文字数"/></xsl:attribute>
                            </xs:maxLength>
                          </xs:restriction>
                        </xs:simpleType>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:attribute name="type">xs:<xsl:call-template name="getItemType">
                            <xsl:with-param name="ItemType"><xsl:value-of select="項目型"/></xsl:with-param>
                          </xsl:call-template>
                        </xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xs:element>
                </xsl:for-each>
                <xs:element name="更新者_xFF29__xFF24_" minOccurs="0">
                  <xs:simpleType>
                    <xs:restriction base="xs:string">
                      <xs:maxLength value="8" />
                    </xs:restriction>
                  </xs:simpleType>
                </xs:element>
                <xs:element name="更新年月日時間" type="xs:dateTime" minOccurs="0" />
                <xs:element name="更新_xFF21__xFF30_" type="xs:string" minOccurs="0" />
                <xs:element name="更新端末_xFF29__xFF24_" type="xs:string" minOccurs="0" />
                <xs:element name="ROWID" type="xs:string" minOccurs="0" />
              </xs:sequence>
            </xs:complexType>
          </xs:element>
        </xsl:for-each>
      </xs:choice>
    </xs:complexType>
  </xs:element>

</xs:schema>

</xsl:template>

<!-- 項目名変換 -->
<xsl:template name="getItemName">
    <xsl:param name="ItemName"/>
    <!-- 1バイト目 -->
    <xsl:variable name="name1">
        <xsl:value-of select="translate($ItemName,
        '０１２３４５６７８９ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ＿ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ',
        '111111111122222222222222233333333333344444444444444455555555555')"/>
    </xsl:variable>
    <!-- 2バイト目 -->
    <xsl:variable name="name2">
        <xsl:value-of select="translate($ItemName,
        '０１２３４５６７８９ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ＿ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ',
        '0123456789123456789ABCDEF0123456789AF123456789ABCDEF0123456789A')"/>
    </xsl:variable>

    <!-- テンプレート呼び出し -->
    <xsl:call-template name="getChar">
        <xsl:with-param name="ItemName"><xsl:value-of select="$ItemName"/></xsl:with-param>
        <xsl:with-param name="name1"><xsl:value-of select="$name1"/></xsl:with-param>
        <xsl:with-param name="name2"><xsl:value-of select="$name2"/></xsl:with-param>
    </xsl:call-template>

</xsl:template>

<!-- 文字取得 -->
<xsl:template name="getChar">
    <xsl:param name="ItemName"/>
    <xsl:param name="name1"/>
    <xsl:param name="name2"/>
    <xsl:param name="i">1</xsl:param>

    <xsl:variable name="c">
        <xsl:value-of select="substring($name1, $i, 1)"/>
    </xsl:variable>

    <xsl:choose>
        <xsl:when test="substring($ItemName, $i, 1) != $c"
        >_xFF<xsl:value-of select="concat($c, substring($name2, $i, 1))"/>_</xsl:when>
        <xsl:otherwise><xsl:value-of select="$c"/></xsl:otherwise>
    </xsl:choose>

    <xsl:if test="$i &lt; string-length($ItemName)">
        <!-- 再帰呼び出し -->
        <xsl:call-template name="getChar">
            <xsl:with-param name="ItemName"><xsl:value-of select="$ItemName"/></xsl:with-param>
            <xsl:with-param name="name1"><xsl:value-of select="$name1"/></xsl:with-param>
            <xsl:with-param name="name2"><xsl:value-of select="$name2"/></xsl:with-param>
            <xsl:with-param name="i"><xsl:value-of select="$i + 1"/></xsl:with-param>
        </xsl:call-template>
    </xsl:if>
</xsl:template>

<!-- 項目型変換 -->
<xsl:template name="getItemType">
    <xsl:param name="ItemType"/>
    <xsl:choose>
        <xsl:when test="$ItemType='Number'">decimal</xsl:when>
        <xsl:when test="$ItemType='Date'">dateTime</xsl:when>
        <xsl:otherwise>string</xsl:otherwise>
    </xsl:choose>
</xsl:template>

</xsl:stylesheet>
