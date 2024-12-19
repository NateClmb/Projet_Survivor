<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="xml" encoding="UTF-8" indent="yes" />

    <xsl:template match="/">
        <GameList>
            <!-- Loop through each game save -->
            <xsl:for-each select="//GameSaves/Save">
                <Game>
                    <!-- Game values -->
                    <PlayerName><xsl:value-of select="//Username"/></PlayerName>
                    <Date><xsl:value-of select="Date"/></Date>
                    <Score><xsl:value-of select="Score"/></Score>
                </Game>
            </xsl:for-each>
        </GameList>
    </xsl:template>
</xsl:stylesheet>
