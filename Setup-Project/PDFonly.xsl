<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:wi="http://schemas.microsoft.com/wix/2006/wi" 
    xmlns="http://schemas.microsoft.com/wix/2006/wi">

    <!-- <xsl:template match="wi:Component[not( -->
        <!-- contains(concat(wi:File/@Source,'|'), '.pdf|'))]"> -->
     <!-- </xsl:template> -->

    <xsl:template match="wi:Component[not(contains(concat(wi:File/@Source,'|'), '.pdf|'))]"/>

    <!-- <xsl:template match="wi:ComponentRef[not(contains(concat(wi:File/@Source,'|'), '.pdf|'))]"/> -->

     <xsl:template match="node()|@*">
       <xsl:copy>
       <xsl:apply-templates select="@*"/>
       <xsl:apply-templates/>
       </xsl:copy>
     </xsl:template>
 </xsl:stylesheet>