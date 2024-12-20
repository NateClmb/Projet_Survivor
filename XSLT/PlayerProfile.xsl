<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html" encoding="UTF-8" indent="yes" />

    <xsl:template match="/">
        <html>
            <head>
                <!-- Page header -->
                <title>Player profile</title>
                <link rel="stylesheet" type="text/css" href="../CSS/StyleProfile.css"/>
            </head>
            <body>
                <!-- Page title -->
                <h1>Player profile</h1>

                <!-- Player values -->
                <xsl:for-each select="/PlayerProfile/Player">
                    <h2>Username: <xsl:value-of select="@Username"/></h2> 
                    <p>Games Played: <xsl:value-of select="GamesPlayed"/></p>
                </xsl:for-each>
            </body>
            <footer>
                <!-- Page footer -->
                <p>© 2024 | COLOMBAN N. - DELEUZE-DORDRON A. - YAHA S. | All rights reserved.</p>
            </footer>
        </html>
    </xsl:template>

</xsl:stylesheet>
