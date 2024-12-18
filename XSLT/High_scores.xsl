<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html" encoding="UTF-8" indent="yes" />

    <xsl:template match="/">
        <html>
            <head>
                <!-- Page header -->
                <title>High scores</title>
                <link rel="stylesheet" type="text/css" href="../CSS/Style_scores.css"/>
            </head>
            <body>
                <!-- Page title -->
                <h1>Table of the high scores</h1>

                <!-- Table of high scores -->
                <table border="1">
                    <tr>
                        <!-- Table header -->
                        <th>Player name</th>
                        <th>Date</th>
                        <th>Score</th>
                    </tr>
                    <!-- Loop through each high score -->
                    <xsl:for-each select="//GameSaves/Save">
                        <!-- Sort the high scores by score -->
                        <xsl:sort select="Score" data-type="number" order="descending"/>
                        <tr>
                            <!-- High score values -->
                            <td><xsl:value-of select="Username"/></td>
                            <td><xsl:value-of select="Date"/></td>
                            <td><xsl:value-of select="Score"/></td>
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
