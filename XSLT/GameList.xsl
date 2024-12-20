<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="xml" encoding="UTF-8" indent="yes" />

    <xsl:template match="/">
        <GameList>
            <!-- For each game save -->
            <xsl:for-each select="//GameHistory/Game">
                <Game>
                    <!-- Collect game values -->
                    <PlayerName><xsl:value-of select="Username"/></PlayerName>
                    <Date><xsl:value-of select="Date"/></Date>
                </Game>
            </xsl:for-each>
        </GameList>
    </xsl:template>
</xsl:stylesheet>