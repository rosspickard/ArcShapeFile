# ArcShapeFile
A C# library to read and write ESRI Shapefiles

The ESRI Shapefile format is a commonly shared geographic data format. This is my attempt to read and write to it. The set of C# classes that make up the DLL allow you to read and create both the vector/point data and the associated attribute record.  The source code comes with a reasonably comprehensive help file that includes code C# and VB.Net examples.

<h2>Background</h2>
The Shapefile format first made it's appearence back in the 1980's.&nbsp; Despite it's limitations it remains a standard format for moving data between GIS systems. Essentially the format as described in the <a href="https://www.esri.com/library/whitepapers/pdfs/shapefile.pdf">ESRI Shapefile Technical Description</a> consists on three files</p>
<ul><li>.shp - holds the vertice data (x, y and in some cases z and measure values)</li>
    <li>.shx - contains the offset and length of each record in the .shp</li>
    <li>.dbf - contains the attribute data for each record.  This file is based on the old Ashton Tate dBASE III format, though without memo field support so text fields are limited to 254 characters and the field names are limited to 10 characters</li></ul>
<p>A further limitation of the ShapeFile format is that each Shapefile can only contain one type of shape (i.e. all points or all lines or all polygons).  There is one exception and that is you can have a null record to act as a placeholder within the Shapefile structure.  </p>

<p>Additionally I've added support for the .prj file.  This is a WKT file holding the projection information - handy when you want to use this data in different systems as most seem to recognise it.<p>I originally developed this code in VS 2005 so I've keep the style (relatively) simple - no vars, just explicit dimensioning and no LINQ (though that meant I had to write my own query parser) - so you should be able to easily import the code into any VS version and run it as is.
