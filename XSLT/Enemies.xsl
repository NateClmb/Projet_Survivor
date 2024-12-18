<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html" encoding="UTF-8" indent="yes" />

    <xsl:template match="/">
        <html>
            <head>
                <!-- Page header -->
                <title>Enemies list</title>
                <link rel="stylesheet" type="text/css" href="../CSS/Style_enemies.css"/>
            </head>
            <body>
                <h1>Enemies list</h1>

                <!-- Table of enemies -->
                <table border="1" cellpadding="5" cellspacing="0">
                    <tr>
                        <!-- Table header -->
                        <th>Enemy name</th>
                        <th>Attack type</th>
                        <th>HP</th>
                        <th>Damage</th>
                        <th>Speed</th>
                        <th>XP Value</th>
                    </tr>

                    <!-- Loop through each enemy -->
                    <xsl:for-each select="/Enemies/Enemy">
                        <tr>
                            <!-- Enemy values -->
                            <td><xsl:value-of select="Name"/></td>
                            <td><xsl:value-of select="Type"/></td>
                            <td><xsl:value-of select="HP"/></td>
                            <td><xsl:value-of select="AttackDamage"/></td>
                            <td><xsl:value-of select="Speed"/></td>
                            <td><xsl:value-of select="XPValue"/></td>
                        </tr>
                    </xsl:for-each>
                </table>
                <footer>
                    <!-- Page footer including copyrights -->
                    <p>© 2024 | COLOMBAN N. - DELEUZE-DORDRON A. - YAHA S. | All rights reserved.</p>
                </footer>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>