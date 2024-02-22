<xsl:stylesheet version="1.0"
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
        exclude-result-prefixes="msxsl"
        xmlns:wix="http://wixtoolset.org/schemas/v4/wxs">

	<xsl:output method="xml" indent="yes" />

	<xsl:strip-space elements="*"/>

	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

	<!--Match and ignore .exe files-->
	<xsl:key name="exe-search" match="wix:Component[contains(wix:File/@Source, 'ConsoleContainer.WorkerService.exe')]" use="@Id"/>
	<xsl:template match="wix:Component[key('exe-search', @Id)]"/>
	<xsl:template match="wix:ComponentRef[key('exe-search', @Id)]"/>
</xsl:stylesheet>