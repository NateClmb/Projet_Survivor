<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html" encoding="UTF-8" indent="yes" />

    <xsl:template match="/">
        <html>
            <head>
                <!-- Page header -->
                <title>High scores</title>
                <link rel="stylesheet" type="text/css" href="../CSS/StyleScores.css"/>
            </head>
            <body>
                <!-- Page title -->
                <h1>High scores</h1>

                <!-- Table of high scores -->
                <table border="1">
                    <tr>
                        <!-- Table header -->
                        <th>Username</th>
                        <th>Date</th>
                        <th>Enemies killed</th>
                        <th>Time played</th>
                    </tr>
                    <!-- Loop through each game in the history -->
                    <xsl:for-each select="//GameHistory/Game">
                        <!-- Sort the games by the number of Killed (descending) -->
                        <xsl:sort select="Killed" data-type="number" order="descending"/>
                        <tr>
                            <!-- High score values -->
                            <td><xsl:value-of select="Username"/></td>
                            <td><xsl:value-of select="Date"/></td>
                            <td><xsl:value-of select="Killed"/></td>
                            <td><xsl:value-of select="Time"/></td>
                        </tr>
                    </xsl:for-each>
                </table>
            </body>
            <footer>
                <!-- Page footer including copyrights -->
                <p>© 2024 | COLOMBAN N. - DELEUZE-DORDRON A. - YAHA S. | All rights reserved.</p>
            </footer>
        </html>
    </xsl:template>
</xsl:stylesheet>
