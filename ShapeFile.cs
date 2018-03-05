/******************************************************************************
 *
 * Project:  ShapeFile Access Library
 * Purpose:  To Read and Write ArcShape access code.
 * Author:   Ross Pickard, cr_pickard@hotmail.com
 *
 ******************************************************************************
 * Copyright (c) 2014, Ross Pickard
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 ******************************************************************************
 * 
 * Revision 3.00  22 Sep 2014 rpickard
 * Conversion from VB.Net libraries
 * 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ArcShapeFile
{

    #region **********          eNums                         **********

    internal struct SQLLevels
    {
        public bool Result;
        public int SQLOperator;
        public int Level;
    }

    /// <summary>
    /// Database Language ENum
    /// </summary>
    public enum eLanguage
    {
        ///<summary>OEM</summary>
        OEM = 0,
        ///<summary>US MSDOS</summary>
        Codepage_437_US_MSDOS = 0x1,
        ///<summary>International MSDOS</summary>
        Codepage_850_International_MSDOS = 0x2,
        ///<summary>Windows ANSI</summary>
        Codepage_1252_Windows_ANSI = 0x3,
        ///<summary>ANSI</summary>
        ANSI = 0x57,
        ///<summary>Greek MSDOS</summary>
        Codepage_737_Greek_MSDOS = 0x6a,
        ///<summary>EasernEuropean MSDOS</summary>
        Codepage_852_EasernEuropean_MSDOS = 0x64,
        ///<summary>Turkish MSDOS</summary>
        Codepage_857_Turkish_MSDOS = 0x6b,
        ///<summary>Icelandic MSDOS</summary>
        Codepage_861_Icelandic_MSDOS = 0x67,
        ///<summary>Nordic MSDOS</summary>
        Codepage_865_Nordic_MSDOS = 0x66,
        ///<summary>Russian MSDOS</summary>
        Codepage_866_Russian_MSDOS = 0x65,
        ///<summary>Chinese Windows</summary>
        Codepage_950_Chinese_Windows = 0x78,
        ///<summary>Chinese Windows</summary>
        Codepage_936_Chinese_Windows = 0x7a,
        ///<summary>Japanese Windows</summary>
        Codepage_932_Japanese_Windows = 0x7b,
        ///<summary>Hebrew Windows</summary>
        Codepage_1255_Hebrew_Windows = 0x7d,
        ///<summary>Arabic Windows</summary>
        Codepage_1256_Arabic_Windows = 0x7e,
        ///<summary>Eastern European Windows</summary>
        Codepage_1250_Eastern_European_Windows = 0xc8,
        ///<summary>Russian Windows</summary>
        Codepage_1251_Russian_Windows = 0xc9,
        ///<summary>Turkish Windows</summary>
        Codepage_1254_Turkish_Windows = 0xca,
        ///<summary>Greek Windows</summary>
        Codepage_1253_Greek_Windows = 0xcb
    }

    /// <summary>
    /// MultiPatch part types ENum
    /// </summary>
    public enum ePartType
    {
        /// <summary>
        /// No part type
        /// </summary>
        none = -1,
        /// <summary>
        /// Traingle Strip
        /// </summary>
        shpTriangleStrip = 0,
        /// <summary>
        /// Triangle Fan
        /// </summary>
        shpTriangleFan = 1,
        /// <summary>
        /// Outer ring
        /// </summary>
        shpOuterRing = 2,
        /// <summary>
        /// Inner ring
        /// </summary>
        shpInnerRing = 3,
        /// <summary>
        /// First Ring
        /// </summary>
        shpFirstRing = 4,
        /// <summary>
        /// Ring
        /// </summary>
        shpRing = 5
    }

    /// <summary>
    /// Shape type ENums
    /// </summary>
    public enum eShapeType
    {
        ///<summary>Null Shape</summary>
        shpNull = 0,
        ///<summary>Point</summary>
        shpPoint = 1,
        ///<summary>PolyLine</summary>
        shpPolyLine = 3,
        ///<summary>Polygon</summary>
        shpPolygon = 5,
        ///<summary>MultiPoint</summary>
        shpMultiPoint = 8,
        ///<summary>Point with Z values</summary>
        shpPointZ = 11,
        ///<summary>PolyLine with Z values</summary>
        shpPolyLineZ = 13,
        ///<summary>Polygon with Z values</summary>
        shpPolygonZ = 15,
        ///<summary>MultiPoints with Z values</summary>
        shpMultiPointZ = 18,
        ///<summary>Point with Measures</summary>
        shpPointM = 21,
        ///<summary>PolyLine with Measures</summary>
        shpPolyLineM = 23,
        ///<summary>Polygon with Measures</summary>
        shpPolygonM = 25,
        ///<summary>MultiPoint with Measures</summary>
        shpMultiPointM = 28,
        ///<summary>Multi Patch</summary>
        shpMultiPatch = 31
    }

    /// <summary>
    /// Database Field DataType Enum
    /// </summary>
    public enum eFieldType
    {
        ///<summary>Numeric</summary>
        shpNumeric = 19,
        ///<summary>Date</summary>
        shpDate = 8,
        ///<summary>Text</summary>
        shpText = 10,
        ///<summary>Boolean</summary>
        shpBoolean = 1,
        ///<summary>Double</summary>
        shpDouble = 7,
        ///<summary>Long</summary>
        shpLong = 4,
        ///<summary>Integer</summary>
        shpInteger = 3,
        ///<summary>Single</summary>
        shpSingle = 6,
        ///<summary>Float</summary>
        shpFloat = 20
    }

    /// <summary>
    /// Polygon digitised direction ENum
    /// </summary>
    public enum eDirection
    {
        ///<summary>Polygon is draw in a clockwise direction</summary>
        Clockwise = 0,
        ///<summary>Polygon is draw in a anti-clockwise direction</summary>
        AntiClockwise = 1
    }

    /// <summary>
    /// ShapeFile data reading action ENum
    /// </summary>
    public enum eReadMode
    {
        ///<summary>On each move read all the data into the collections (Vertices, Fields)</summary>
        FullRead = -1,
        ///<summary>On each move read just the data header information into the collections (Vertices, Fields)</summary>
        HeaderOnly = 0,
        ///<summary>On each move read all the data into memory and don't populate the collections (Vertices, Fields)</summary>
        FastRead = 1
    }

    /// <summary>
    /// Geocentric Datum list Enum
    /// </summary>
    public enum eGeocentricDatums
    {
        ///<summary>Greek</summary>
        Greek = 4120,
        ///<summary>GGRS87</summary>
        GGRS87 = 4121,
        ///<summary>ATS77</summary>
        ATS77 = 4122,
        ///<summary>KKJ</summary>
        KKJ = 4123,
        ///<summary>RT90</summary>
        RT90 = 4124,
        ///<summary>Samboja</summary>
        Samboja = 4125,
        ///<summary>LKS94 ETRS89</summary>
        LKS94_ETRS89 = 4126,
        ///<summary>Tete</summary>
        Tete = 4127,
        ///<summary>Madzansua</summary>
        Madzansua = 4128,
        ///<summary>Observatario</summary>
        Observatario = 4129,
        ///<summary>Moznet</summary>
        Moznet = 4130,
        ///<summary>Indian 1960</summary>
        Indian_1960 = 4131,
        ///<summary>FD58</summary>
        FD58 = 4132,
        ///<summary>EST92</summary>
        EST92 = 4133,
        ///<summary>PDO Survey Datum 1993</summary>
        PDO_Survey_Datum_1993 = 4134,
        ///<summary>Old Hawaiian</summary>
        Old_Hawaiian = 4135,
        ///<summary>St Lawrence Island</summary>
        St_Lawrence_Island = 4136,
        ///<summary>St Paul Island</summary>
        St_Paul_Island = 4137,
        ///<summary>St George Island</summary>
        St_George_Island = 4138,
        ///<summary>Puerto Rico</summary>
        Puerto_Rico = 4139,
        ///<summary>NAD83CSRS98</summary>
        NAD83CSRS98 = 4140,
        ///<summary>Israel</summary>
        Israel = 4141,
        ///<summary>Locodjo 1965</summary>
        Locodjo_1965 = 4142,
        ///<summary>Abidjan 1987</summary>
        Abidjan_1987 = 4143,
        ///<summary>Kalianpur 1937</summary>
        Kalianpur_1937 = 4144,
        ///<summary>Kalianpur 1962</summary>
        Kalianpur_1962 = 4145,
        ///<summary>Kalianpur 1975</summary>
        Kalianpur_1975 = 4146,
        ///<summary>Hanoi 1972</summary>
        Hanoi_1972 = 4147,
        ///<summary>Hartebeesthoek94</summary>
        Hartebeesthoek94 = 4148,
        ///<summary>CH1903</summary>
        CH1903 = 4149,
        ///<summary>CH1903 Plus</summary>
        CH1903_Plus = 4150,
        ///<summary>CHTRF95</summary>
        CHTRF95 = 4151,
        ///<summary>NAD83HARN</summary>
        NAD83HARN = 4152,
        ///<summary>Rassadiran</summary>
        Rassadiran = 4153,
        ///<summary>ED50ED77</summary>
        ED50ED77 = 4154,
        ///<summary>Mount Dillon</summary>
        Mount_Dillon = 4157,
        ///<summary>Naparima 1955</summary>
        Naparima_1955 = 4158,
        ///<summary>ELD79</summary>
        ELD79 = 4159,
        ///<summary>Chos Malal 1914</summary>
        Chos_Malal_1914 = 4160,
        ///<summary>Pampa del Castillo</summary>
        Pampa_del_Castillo = 4161,
        ///<summary>Korean 1985</summary>
        Korean_1985 = 4162,
        ///<summary>Yemen NGN96</summary>
        Yemen_NGN96 = 4163,
        ///<summary>South Yemen</summary>
        South_Yemen = 4164,
        ///<summary>Korean 1995</summary>
        Korean_1995 = 4166,
        ///<summary>NZGD2000</summary>
        NZGD2000 = 4167,
        ///<summary>Accra</summary>
        Accra = 4168,
        ///<summary>SIRGAS</summary>
        SIRGAS = 4170,
        ///<summary>RGF93</summary>
        RGF93 = 4171,
        ///<summary>POSGAR</summary>
        POSGAR = 4172,
        ///<summary>IRENET95</summary>
        IRENET95 = 4173,
        ///<summary>Sierra Leone 1924</summary>
        Sierra_Leone_1924 = 4174,
        ///<summary>Sierra Leone 1968</summary>
        Sierra_Leone_1968 = 4175,
        ///<summary>Australian Antarctic</summary>
        Australian_Antarctic = 4176,
        ///<summary>Pulkovo 194283</summary>
        Pulkovo_194283 = 4178,
        ///<summary>Pulkovo 194258</summary>
        Pulkovo_194258 = 4179,
        ///<summary>EST97</summary>
        EST97 = 4180,
        ///<summary>Luxembourg 1930</summary>
        Luxembourg_1930 = 4181,
        ///<summary>Azores Occidental 1939</summary>
        Azores_Occidental_1939 = 4182,
        ///<summary>Azores Central 1948</summary>
        Azores_Central_1948 = 4183,
        ///<summary>Azores Oriental 1940</summary>
        Azores_Oriental_1940 = 4184,
        ///<summary>Madeira 1936</summary>
        Madeira_1936 = 4185,
        ///<summary>OSNI 1952</summary>
        OSNI_1952 = 4188,
        ///<summary>REGVEN</summary>
        REGVEN = 4189,
        ///<summary>POSGAR 98</summary>
        POSGAR_98 = 4190,
        ///<summary>Albanian 1987</summary>
        Albanian_1987 = 4191,
        ///<summary>Douala 1948</summary>
        Douala_1948 = 4192,
        ///<summary>Manoca 1962</summary>
        Manoca_1962 = 4193,
        ///<summary>Qornoq 1927</summary>
        Qornoq_1927 = 4194,
        ///<summary>Scoresbysund 1952</summary>
        Scoresbysund_1952 = 4195,
        ///<summary>Ammassalik 1958</summary>
        Ammassalik_1958 = 4196,
        ///<summary>Kousseri</summary>
        Kousseri = 4198,
        ///<summary>Egypt 1930</summary>
        Egypt_1930 = 4199,
        ///<summary>Pulkovo 1995</summary>
        Pulkovo_1995 = 4200,
        ///<summary>Adindan</summary>
        Adindan = 4201,
        ///<summary>AGD66</summary>
        AGD66 = 4202,
        ///<summary>AGD84</summary>
        AGD84 = 4203,
        ///<summary>Ain el Abd</summary>
        Ain_el_Abd = 4204,
        ///<summary>Afgooye</summary>
        Afgooye = 4205,
        ///<summary>Agadez</summary>
        Agadez = 4206,
        ///<summary>Lisbon</summary>
        Lisbon = 4207,
        ///<summary>Aratu</summary>
        Aratu = 4208,
        ///<summary>Arc 1950</summary>
        Arc_1950 = 4209,
        ///<summary>Arc 1960</summary>
        Arc_1960 = 4210,
        ///<summary>Batavia</summary>
        Batavia = 4211,
        ///<summary>Barbados 1938</summary>
        Barbados_1938 = 4212,
        ///<summary>Beduaram</summary>
        Beduaram = 4213,
        ///<summary>Beijing 1954</summary>
        Beijing_1954 = 4214,
        ///<summary>Belge 1950</summary>
        Belge_1950 = 4215,
        ///<summary>Bermuda 1957</summary>
        Bermuda_1957 = 4216,
        ///<summary>Bogota 1975</summary>
        Bogota_1975 = 4218,
        ///<summary>Bukit Rimpah</summary>
        Bukit_Rimpah = 4219,
        ///<summary>Camacupa</summary>
        Camacupa = 4220,
        ///<summary>Campo Inchauspe</summary>
        Campo_Inchauspe = 4221,
        ///<summary>Cape</summary>
        Cape = 4222,
        ///<summary>Carthage</summary>
        Carthage = 4223,
        ///<summary>Chua</summary>
        Chua = 4224,
        ///<summary>Corrego Alegre</summary>
        Corrego_Alegre = 4225,
        ///<summary>Cote dIvoire</summary>
        Cote_dIvoire = 4226,
        ///<summary>Deir ez Zor</summary>
        Deir_ez_Zor = 4227,
        ///<summary>Douala</summary>
        Douala = 4228,
        ///<summary>Egypt 1907</summary>
        Egypt_1907 = 4229,
        ///<summary>ED50</summary>
        ED50 = 4230,
        ///<summary>ED87</summary>
        ED87 = 4231,
        ///<summary>Fahud</summary>
        Fahud = 4232,
        ///<summary>Gandajika 1970</summary>
        Gandajika_1970 = 4233,
        ///<summary>Garoua</summary>
        Garoua = 4234,
        ///<summary>Guyane Francaise</summary>
        Guyane_Francaise = 4235,
        ///<summary>Hu Tzu Shan</summary>
        Hu_Tzu_Shan = 4236,
        ///<summary>HD72</summary>
        HD72 = 4237,
        ///<summary>ID74</summary>
        ID74 = 4238,
        ///<summary>Indian 1954</summary>
        Indian_1954 = 4239,
        ///<summary>Indian 1975</summary>
        Indian_1975 = 4240,
        ///<summary>Jamaica 1875</summary>
        Jamaica_1875 = 4241,
        ///<summary>JAD69</summary>
        JAD69 = 4242,
        ///<summary>Kalianpur 1880</summary>
        Kalianpur_1880 = 4243,
        ///<summary>Kandawala</summary>
        Kandawala = 4244,
        ///<summary>Kertau 1968</summary>
        Kertau_1968 = 4245,
        ///<summary>KOC</summary>
        KOC = 4246,
        ///<summary>La Canoa</summary>
        La_Canoa = 4247,
        ///<summary>PSAD56</summary>
        PSAD56 = 4248,
        ///<summary>Lake</summary>
        Lake = 4249,
        ///<summary>Leigon</summary>
        Leigon = 4250,
        ///<summary>Liberia 1964</summary>
        Liberia_1964 = 4251,
        ///<summary>Lome</summary>
        Lome = 4252,
        ///<summary>Luzon 1911</summary>
        Luzon_1911 = 4253,
        ///<summary>Hito XVIII 1963</summary>
        Hito_XVIII_1963 = 4254,
        ///<summary>Herat North</summary>
        Herat_North = 4255,
        ///<summary>Mahe 1971</summary>
        Mahe_1971 = 4256,
        ///<summary>Makassar</summary>
        Makassar = 4257,
        ///<summary>ETRS89</summary>
        ETRS89 = 4258,
        ///<summary>Malongo 1987</summary>
        Malongo_1987 = 4259,
        ///<summary>Manoca</summary>
        Manoca = 4260,
        ///<summary>Merchich</summary>
        Merchich = 4261,
        ///<summary>Massawa</summary>
        Massawa = 4262,
        ///<summary>Minna</summary>
        Minna = 4263,
        ///<summary>Mhast</summary>
        Mhast = 4264,
        ///<summary>Monte Mario</summary>
        Monte_Mario = 4265,
        ///<summary>Mporaloko</summary>
        Mporaloko = 4266,
        ///<summary>North American 1927</summary>
        GCS_North_American_1927 = 4267,
        ///<summary>NAD27 Michigan</summary>
        NAD27_Michigan = 4268,
        ///<summary>North American 1983</summary>
        GCS_North_American_1983 = 4269,
        ///<summary>Nahrwan 1967</summary>
        Nahrwan_1967 = 4270,
        ///<summary>Naparima 1972</summary>
        Naparima_1972 = 4271,
        ///<summary>NZGD49</summary>
        NZGD49 = 4272,
        ///<summary>NGO 1948</summary>
        NGO_1948 = 4273,
        ///<summary>Datum 73</summary>
        Datum_73 = 4274,
        ///<summary>NTF</summary>
        NTF = 4275,
        ///<summary>NSWC 9Z2</summary>
        NSWC_9Z2 = 4276,
        ///<summary>OSGB 1936</summary>
        OSGB_1936 = 4277,
        ///<summary>OSGB70</summary>
        OSGB70 = 4278,
        ///<summary>OSSN80</summary>
        OSSN80 = 4279,
        ///<summary>Padang</summary>
        Padang = 4280,
        ///<summary>Palestine 1923</summary>
        Palestine_1923 = 4281,
        ///<summary>Pointe Noire</summary>
        Pointe_Noire = 4282,
        ///<summary>GDA94</summary>
        GDA94 = 4283,
        ///<summary>Pulkovo 1942</summary>
        Pulkovo_1942 = 4284,
        ///<summary>Qatar 1974</summary>
        Qatar_1974 = 4285,
        ///<summary>Qatar 1948</summary>
        Qatar_1948 = 4286,
        ///<summary>Qornoq</summary>
        Qornoq = 4287,
        ///<summary>Loma Quintana</summary>
        Loma_Quintana = 4288,
        ///<summary>Amersfoort</summary>
        Amersfoort = 4289,
        ///<summary>SAD69</summary>
        SAD69 = 4291,
        ///<summary>Sapper Hill 1943</summary>
        Sapper_Hill_1943 = 4292,
        ///<summary>Schwarzeck</summary>
        Schwarzeck = 4293,
        ///<summary>Segora</summary>
        Segora = 4294,
        ///<summary>Serindung</summary>
        Serindung = 4295,
        ///<summary>Sudan</summary>
        Sudan = 4296,
        ///<summary>Tananarive</summary>
        Tananarive = 4297,
        ///<summary>Timbalai 1948</summary>
        Timbalai_1948 = 4298,
        ///<summary>TM65</summary>
        TM65 = 4299,
        ///<summary>TM75</summary>
        TM75 = 4300,
        ///<summary>Tokyo</summary>
        Tokyo = 4301,
        ///<summary>Trinidad 1903</summary>
        Trinidad_1903 = 4302,
        ///<summary>TC1948</summary>
        TC1948 = 4303,
        ///<summary>Voirol 1875</summary>
        Voirol_1875 = 4304,
        ///<summary>Bern 1938</summary>
        Bern_1938 = 4306,
        ///<summary>Nord Sahara 1959</summary>
        Nord_Sahara_1959 = 4307,
        ///<summary>RT38</summary>
        RT38 = 4308,
        ///<summary>Yacare</summary>
        Yacare = 4309,
        ///<summary>Yoff</summary>
        Yoff = 4310,
        ///<summary>Zanderij</summary>
        Zanderij = 4311,
        ///<summary>MGI</summary>
        MGI = 4312,
        ///<summary>Belge 1972</summary>
        Belge_1972 = 4313,
        ///<summary>DHDN</summary>
        DHDN = 4314,
        ///<summary>Conakry 1905</summary>
        Conakry_1905 = 4315,
        ///<summary>Dealul Piscului 1933</summary>
        Dealul_Piscului_1933 = 4316,
        ///<summary>Dealul Piscului 1970</summary>
        Dealul_Piscului_1970 = 4317,
        ///<summary>NGN</summary>
        NGN = 4318,
        ///<summary>KUDAMS</summary>
        KUDAMS = 4319,
        ///<summary>WGS 72</summary>
        WGS_72 = 4322,
        ///<summary>WGS 72BE</summary>
        WGS_72BE = 4324,
        ///<summary>WGS 1984</summary>
        GCS_WGS_1984 = 4326,
        ///<summary>Anguilla 1957</summary>
        Anguilla_1957 = 4600,
        ///<summary>Antigua 1943</summary>
        Antigua_1943 = 4601,
        ///<summary>Dominica 1945</summary>
        Dominica_1945 = 4602,
        ///<summary>Grenada 1953</summary>
        Grenada_1953 = 4603,
        ///<summary>St Kitts 1955</summary>
        St_Kitts_1955 = 4605,
        ///<summary>St Lucia 1955</summary>
        St_Lucia_1955 = 4606,
        ///<summary>St Vincent 1945</summary>
        St_Vincent_1945 = 4607,
        ///<summary>NAD2776</summary>
        NAD2776 = 4608,
        ///<summary>NAD27CGQ77</summary>
        NAD27CGQ77 = 4609,
        ///<summary>Xian 1980</summary>
        Xian_1980 = 4610,
        ///<summary>Hong Kong 1980</summary>
        Hong_Kong_1980 = 4611,
        ///<summary>JGD2000</summary>
        JGD2000 = 4612,
        ///<summary>Segara</summary>
        Segara = 4613,
        ///<summary>QND95</summary>
        QND95 = 4614,
        ///<summary>Porto Santo</summary>
        Porto_Santo = 4615,
        ///<summary>SWEREF99</summary>
        SWEREF99 = 4619,
        ///<summary>Point 58</summary>
        Point_58 = 4620,
        ///<summary>Fort Marigot</summary>
        Fort_Marigot = 4621,
        ///<summary>Guadeloupe 1948</summary>
        Guadeloupe_1948 = 4622,
        ///<summary>CSG67</summary>
        CSG67 = 4623,
        ///<summary>RGFG95</summary>
        RGFG95 = 4624,
        ///<summary>Martinique 1938</summary>
        Martinique_1938 = 4625,
        ///<summary>Reunion 1947</summary>
        Reunion_1947 = 4626,
        ///<summary>RGR92</summary>
        RGR92 = 4627,
        ///<summary>Tahiti 52</summary>
        Tahiti_52 = 4628,
        ///<summary>Tahaa 54</summary>
        Tahaa_54 = 4629,
        ///<summary>IGN72 Nuku Hiva</summary>
        IGN72_Nuku_Hiva = 4630,
        ///<summary>K0 1949</summary>
        K0_1949 = 4631,
        ///<summary>Combani 1950</summary>
        Combani_1950 = 4632,
        ///<summary>IGN56 Lifou</summary>
        IGN56_Lifou = 4633,
        ///<summary>ST87 Ouvea</summary>
        ST87_Ouvea = 4635,
        ///<summary>Petrels 1972</summary>
        Petrels_1972 = 4636,
        ///<summary>Perroud 1950</summary>
        Perroud_1950 = 4637,
        ///<summary>Saint Pierre et Miquelon 1950</summary>
        Saint_Pierre_et_Miquelon_1950 = 4638,
        ///<summary>MOP78</summary>
        MOP78 = 4639,
        ///<summary>RRAF 1991</summary>
        RRAF_1991 = 4640,
        ///<summary>IGN53 Mare</summary>
        IGN53_Mare = 4641,
        ///<summary>ST84 Ile des Pins</summary>
        ST84_Ile_des_Pins = 4642,
        ///<summary>ST71 Belep</summary>
        ST71_Belep = 4643,
        ///<summary>NEA74 Noumea</summary>
        NEA74_Noumea = 4644,
        ///<summary>RGNC 1991</summary>
        RGNC_1991 = 4645,
        ///<summary>Grand Comoros</summary>
        Grand_Comoros = 4646,
        ///<summary>Reykjavik 1900</summary>
        Reykjavik_1900 = 4657,
        ///<summary>Hjorsey 1955</summary>
        Hjorsey_1955 = 4658,
        ///<summary>ISN93</summary>
        ISN93 = 4659,
        ///<summary>Helle 1954</summary>
        Helle_1954 = 4660,
        ///<summary>LKS92</summary>
        LKS92 = 4661,
        ///<summary>IGN72 Grande Terre</summary>
        IGN72_Grande_Terre = 4662,
        ///<summary>Porto Santo 1995</summary>
        Porto_Santo_1995 = 4663,
        ///<summary>Azores Oriental 1995</summary>
        Azores_Oriental_1995 = 4664,
        ///<summary>Azores Central 1995</summary>
        Azores_Central_1995 = 4665,
        ///<summary>Lisbon 1890</summary>
        Lisbon_1890 = 4666,
        ///<summary>IKBD92</summary>
        IKBD92 = 4667,
        ///<summary>ED79</summary>
        ED79 = 4668,
        ///<summary>IGM95</summary>
        IGM95 = 4670,
        ///<summary>Bern 1898 Bern</summary>
        Bern_1898_Bern = 4801,
        ///<summary>Bogota 1975 Bogota</summary>
        Bogota_1975_Bogota = 4802,
        ///<summary>Lisbon Lisbon</summary>
        Lisbon_Lisbon = 4803,
        ///<summary>Makassar Jakarta</summary>
        Makassar_Jakarta = 4804,
        ///<summary>MGI Ferro</summary>
        MGI_Ferro = 4805,
        ///<summary>Monte Mario Rome</summary>
        Monte_Mario_Rome = 4806,
        ///<summary>NTF Paris</summary>
        NTF_Paris = 4807,
        ///<summary>Padang Jakarta</summary>
        Padang_Jakarta = 4808,
        ///<summary>Belge 1950 Brussels</summary>
        Belge_1950_Brussels = 4809,
        ///<summary>Tananarive Paris</summary>
        Tananarive_Paris = 4810,
        ///<summary>Voirol 1875 Paris</summary>
        Voirol_1875_Paris = 4811,
        ///<summary>Batavia Jakarta</summary>
        Batavia_Jakarta = 4813,
        ///<summary>RT38 Stockholm</summary>
        RT38_Stockholm = 4814,
        ///<summary>Carthage Paris</summary>
        Carthage_Paris = 4816,
        ///<summary>NGO 1948 Oslo</summary>
        NGO_1948_Oslo = 4817,
        ///<summary>Nord Sahara 1959 Paris</summary>
        Nord_Sahara_1959_Paris = 4819,
        ///<summary>Segara Jakarta</summary>
        Segara_Jakarta = 4820,
        ///<summary>unnamed ellipse</summary>
        unnamed_ellipse = 4901,
        ///<summary>NDG Paris</summary>
        NDG_Paris = 4902,
        ///<summary>Madrid 1870 Madrid</summary>
        Madrid_1870_Madrid = 4903,
        ///<summary>Lisbon 1890 Lisbon</summary>
        Lisbon_1890_Lisbon = 4904,
        ///<summary>Everest Bangladesh</summary>
        GCS_Everest_Bangladesh = 37202,
        ///<summary>Everest India Nepal</summary>
        GCS_Everest_India_Nepal = 37203,
        ///<summary>Hong Kong 1963</summary>
        GCS_Hong_Kong_1963 = 37205,
        ///<summary>Oman</summary>
        GCS_Oman = 37206,
        ///<summary>South Asia Singapore</summary>
        GCS_South_Asia_Singapore = 37207,
        ///<summary>Ayabelle</summary>
        GCS_Ayabelle = 37208,
        ///<summary>Beacon E 1945</summary>
        GCS_Beacon_E_1945 = 37212,
        ///<summary>Tern Island 1961</summary>
        GCS_Tern_Island_1961 = 37213,
        ///<summary>Astro 1952</summary>
        GCS_Astro_1952 = 37214,
        ///<summary>Bellevue IGN</summary>
        GCS_Bellevue_IGN = 37215,
        ///<summary>Canton 1966</summary>
        GCS_Canton_1966 = 37216,
        ///<summary>Chatham Island 1971</summary>
        GCS_Chatham_Island_1971 = 37217,
        ///<summary>DOS 1968</summary>
        GCS_DOS_1968 = 37218,
        ///<summary>Easter Island 1967</summary>
        GCS_Easter_Island_1967 = 37219,
        ///<summary>Guam 1963</summary>
        GCS_Guam_1963 = 37220,
        ///<summary>GUX 1</summary>
        GCS_GUX_1 = 37221,
        ///<summary>Johnston Island 1961</summary>
        GCS_Johnston_Island_1961 = 37222,
        ///<summary>Carthage Degree</summary>
        GCS_Carthage_Degree = 37223,
        ///<summary>Midway 1961</summary>
        GCS_Midway_1961 = 37224,
        ///<summary>Pitcairn 1967</summary>
        GCS_Pitcairn_1967 = 37226,
        ///<summary>Santo DOS 1965</summary>
        GCS_Santo_DOS_1965 = 37227,
        ///<summary>Viti Levu 1916</summary>
        GCS_Viti_Levu_1916 = 37228,
        ///<summary>Wake Eniwetok 1960</summary>
        GCS_Wake_Eniwetok_1960 = 37229,
        ///<summary>Wake Island 1952</summary>
        GCS_Wake_Island_1952 = 37230,
        ///<summary>Anna 1 1965</summary>
        GCS_Anna_1_1965 = 37231,
        ///<summary>Gan 1970</summary>
        GCS_Gan_1970 = 37232,
        ///<summary>ISTS 073 1969</summary>
        GCS_ISTS_073_1969 = 37233,
        ///<summary>Kerguelen Island 1949</summary>
        GCS_Kerguelen_Island_1949 = 37234,
        ///<summary>Reunion</summary>
        GCS_Reunion = 37235,
        ///<summary>Ascension Island 1958</summary>
        GCS_Ascension_Island_1958 = 37237,
        ///<summary>DOS 71 4</summary>
        GCS_DOS_71_4 = 37238,
        ///<summary>Cape Canaveral</summary>
        GCS_Cape_Canaveral = 37239,
        ///<summary>Fort Thomas 1955</summary>
        GCS_Fort_Thomas_1955 = 37240,
        ///<summary>Graciosa Base SW 1948</summary>
        GCS_Graciosa_Base_SW_1948 = 37241,
        ///<summary>ISTS 061 1968</summary>
        GCS_ISTS_061_1968 = 37242,
        ///<summary>LC5 1961</summary>
        GCS_LC5_1961 = 37243,
        ///<summary>Observ Meteorologico 1939</summary>
        GCS_Observ_Meteorologico_1939 = 37245,
        ///<summary>Pico de Las Nieves</summary>
        GCS_Pico_de_Las_Nieves = 37246,
        ///<summary>Sao Braz</summary>
        GCS_Sao_Braz = 37249,
        ///<summary>Selvagem Grande 1938</summary>
        GCS_Selvagem_Grande_1938 = 37250,
        ///<summary>Tristan 1968</summary>
        GCS_Tristan_1968 = 37251,
        ///<summary>Samoa 1962</summary>
        GCS_Samoa_1962 = 37252,
        ///<summary>Camp Area</summary>
        GCS_Camp_Area = 37253,
        ///<summary>Deception Island</summary>
        GCS_Deception_Island = 37254,
        ///<summary>S42 Hungary</summary>
        GCS_S42_Hungary = 37257,
        ///<summary>Kusaie 1951</summary>
        GCS_Kusaie_1951 = 37259,
        ///<summary>Alaskan Islands</summary>
        GCS_Alaskan_Islands = 37260,
        ///<summary>Hermannskogel</summary>
        GCS_Hermannskogel = 104102,
        ///<summary>Sierra Leone 1960</summary>
        GCS_Sierra_Leone_1960 = 104103,
        ///<summary>Datum Lisboa Bessel</summary>
        GCS_Datum_Lisboa_Bessel = 104105,
        ///<summary>Datum Lisboa Hayford</summary>
        GCS_Datum_Lisboa_Hayford = 104106,
        ///<summary>Merchich Degree</summary>
        GCS_Merchich_Degree = 104261
    }

    /// <summary>
    /// Geographic projection Enums
    /// </summary>
    public enum eGeographicDatums
    {
        ///<summary>Anguilla 1957 British West Indies Grid</summary>
        Anguilla_1957_British_West_Indies_Grid = 2000,
        ///<summary>Antigua 1943 British West Indies Grid</summary>
        Antigua_1943_British_West_Indies_Grid = 2001,
        ///<summary>Dominica 1945 British West Indies Grid</summary>
        Dominica_1945_British_West_Indies_Grid = 2002,
        ///<summary>Grenada 1953 British West Indies Grid</summary>
        Grenada_1953_British_West_Indies_Grid = 2003,
        ///<summary>Montserrat 1958 British West Indies Grid</summary>
        Montserrat_1958_British_West_Indies_Grid = 2004,
        ///<summary>St Kitts 1955 British West Indies Grid</summary>
        St_Kitts_1955_British_West_Indies_Grid = 2005,
        ///<summary>St Lucia 1955 British West Indies Grid</summary>
        St_Lucia_1955_British_West_Indies_Grid = 2006,
        ///<summary>St Vincent 45 British West Indies Grid</summary>
        St_Vincent_45_British_West_Indies_Grid = 2007,
        ///<summary>NAD27CGQ77 SCoPQ zone 2</summary>
        NAD27CGQ77_SCoPQ_zone_2 = 2008,
        ///<summary>NAD27CGQ77 SCoPQ zone 3</summary>
        NAD27CGQ77_SCoPQ_zone_3 = 2009,
        ///<summary>NAD27CGQ77 SCoPQ zone 4</summary>
        NAD27CGQ77_SCoPQ_zone_4 = 2010,
        ///<summary>NAD27CGQ77 SCoPQ zone 5</summary>
        NAD27CGQ77_SCoPQ_zone_5 = 2011,
        ///<summary>NAD27CGQ77 SCoPQ zone 6</summary>
        NAD27CGQ77_SCoPQ_zone_6 = 2012,
        ///<summary>NAD27CGQ77 SCoPQ zone 7</summary>
        NAD27CGQ77_SCoPQ_zone_7 = 2013,
        ///<summary>NAD27CGQ77 SCoPQ zone 8</summary>
        NAD27CGQ77_SCoPQ_zone_8 = 2014,
        ///<summary>NAD27CGQ77 SCoPQ zone 9</summary>
        NAD27CGQ77_SCoPQ_zone_9 = 2015,
        ///<summary>NAD27CGQ77 SCoPQ zone 10</summary>
        NAD27CGQ77_SCoPQ_zone_10 = 2016,
        ///<summary>NAD2776 MTM zone 8</summary>
        NAD2776_MTM_zone_8 = 2017,
        ///<summary>NAD2776 MTM zone 9</summary>
        NAD2776_MTM_zone_9 = 2018,
        ///<summary>NAD2776 MTM zone 10</summary>
        NAD2776_MTM_zone_10 = 2019,
        ///<summary>NAD2776 MTM zone 11</summary>
        NAD2776_MTM_zone_11 = 2020,
        ///<summary>NAD2776 MTM zone 12</summary>
        NAD2776_MTM_zone_12 = 2021,
        ///<summary>NAD2776 MTM zone 13</summary>
        NAD2776_MTM_zone_13 = 2022,
        ///<summary>NAD2776 MTM zone 14</summary>
        NAD2776_MTM_zone_14 = 2023,
        ///<summary>NAD2776 MTM zone 15</summary>
        NAD2776_MTM_zone_15 = 2024,
        ///<summary>NAD2776 MTM zone 16</summary>
        NAD2776_MTM_zone_16 = 2025,
        ///<summary>NAD2776 MTM zone 17</summary>
        NAD2776_MTM_zone_17 = 2026,
        ///<summary>NAD2776 UTM zone 15N</summary>
        NAD2776_UTM_zone_15N = 2027,
        ///<summary>NAD2776 UTM zone 16N</summary>
        NAD2776_UTM_zone_16N = 2028,
        ///<summary>NAD2776 UTM zone 17N</summary>
        NAD2776_UTM_zone_17N = 2029,
        ///<summary>NAD2776 UTM zone 18N</summary>
        NAD2776_UTM_zone_18N = 2030,
        ///<summary>NAD27CGQ77 UTM zone 17N</summary>
        NAD27CGQ77_UTM_zone_17N = 2031,
        ///<summary>NAD27CGQ77 UTM zone 18N</summary>
        NAD27CGQ77_UTM_zone_18N = 2032,
        ///<summary>NAD27CGQ77 UTM zone 19N</summary>
        NAD27CGQ77_UTM_zone_19N = 2033,
        ///<summary>NAD27CGQ77 UTM zone 20N</summary>
        NAD27CGQ77_UTM_zone_20N = 2034,
        ///<summary>NAD27CGQ77 UTM zone 21N</summary>
        NAD27CGQ77_UTM_zone_21N = 2035,
        ///<summary>NAD83CSRS98 New Brunswick Stereo deprecated</summary>
        NAD83CSRS98_New_Brunswick_Stereo_deprecated = 2036,
        ///<summary>NAD83CSRS98 UTM zone 19N deprecated</summary>
        NAD83CSRS98_UTM_zone_19N_deprecated = 2037,
        ///<summary>NAD83CSRS98 UTM zone 20N deprecated</summary>
        NAD83CSRS98_UTM_zone_20N_deprecated = 2038,
        ///<summary>Israel Israeli TM Grid</summary>
        Israel_Israeli_TM_Grid = 2039,
        ///<summary>Locodjo 1965 UTM zone 30N</summary>
        Locodjo_1965_UTM_zone_30N = 2040,
        ///<summary>Abidjan 1987 UTM zone 30N</summary>
        Abidjan_1987_UTM_zone_30N = 2041,
        ///<summary>Locodjo 1965 UTM zone 29N</summary>
        Locodjo_1965_UTM_zone_29N = 2042,
        ///<summary>Abidjan 1987 UTM zone 29N</summary>
        Abidjan_1987_UTM_zone_29N = 2043,
        ///<summary>Hanoi 1972 GaussKruger zone 18</summary>
        Hanoi_1972_GaussKruger_zone_18 = 2044,
        ///<summary>Hanoi 1972 GaussKruger zone 19</summary>
        Hanoi_1972_GaussKruger_zone_19 = 2045,
        ///<summary>Hartebeesthoek94 Lo15</summary>
        Hartebeesthoek94_Lo15 = 2046,
        ///<summary>Hartebeesthoek94 Lo17</summary>
        Hartebeesthoek94_Lo17 = 2047,
        ///<summary>Hartebeesthoek94 Lo19</summary>
        Hartebeesthoek94_Lo19 = 2048,
        ///<summary>Hartebeesthoek94 Lo21</summary>
        Hartebeesthoek94_Lo21 = 2049,
        ///<summary>Hartebeesthoek94 Lo23</summary>
        Hartebeesthoek94_Lo23 = 2050,
        ///<summary>Hartebeesthoek94 Lo25</summary>
        Hartebeesthoek94_Lo25 = 2051,
        ///<summary>Hartebeesthoek94 Lo27</summary>
        Hartebeesthoek94_Lo27 = 2052,
        ///<summary>Hartebeesthoek94 Lo29</summary>
        Hartebeesthoek94_Lo29 = 2053,
        ///<summary>Hartebeesthoek94 Lo31</summary>
        Hartebeesthoek94_Lo31 = 2054,
        ///<summary>Hartebeesthoek94 Lo33</summary>
        Hartebeesthoek94_Lo33 = 2055,
        ///<summary>CH1903 plus LV95</summary>
        CH1903_plus_LV95 = 2056,
        ///<summary>Rassadiran Nakhl e Taqi</summary>
        Rassadiran_Nakhl_e_Taqi = 2057,
        ///<summary>ED50ED77 UTM zone 38N</summary>
        ED50ED77_UTM_zone_38N = 2058,
        ///<summary>ED50ED77 UTM zone 39N</summary>
        ED50ED77_UTM_zone_39N = 2059,
        ///<summary>ED50ED77 UTM zone 40N</summary>
        ED50ED77_UTM_zone_40N = 2060,
        ///<summary>ED50ED77 UTM zone 41N</summary>
        ED50ED77_UTM_zone_41N = 2061,
        ///<summary>Madrid 1870 Madrid Spain</summary>
        Madrid_1870_Madrid_Spain = 2062,
        ///<summary>Dabola 1981 UTM zone 28N deprecated</summary>
        Dabola_1981_UTM_zone_28N_deprecated = 2063,
        ///<summary>Dabola 1981 UTM zone 29N deprecated</summary>
        Dabola_1981_UTM_zone_29N_deprecated = 2064,
        ///<summary>SJTSK Ferro Krovak</summary>
        SJTSK_Ferro_Krovak = 2065,
        ///<summary>Mount Dillon Tobago Grid</summary>
        Mount_Dillon_Tobago_Grid = 2066,
        ///<summary>Naparima 1955 UTM zone 20N</summary>
        Naparima_1955_UTM_zone_20N = 2067,
        ///<summary>ELD79 Libya zone 5</summary>
        ELD79_Libya_zone_5 = 2068,
        ///<summary>ELD79 Libya zone 6</summary>
        ELD79_Libya_zone_6 = 2069,
        ///<summary>ELD79 Libya zone 7</summary>
        ELD79_Libya_zone_7 = 2070,
        ///<summary>ELD79 Libya zone 8</summary>
        ELD79_Libya_zone_8 = 2071,
        ///<summary>ELD79 Libya zone 9</summary>
        ELD79_Libya_zone_9 = 2072,
        ///<summary>ELD79 Libya zone 10</summary>
        ELD79_Libya_zone_10 = 2073,
        ///<summary>ELD79 Libya zone 11</summary>
        ELD79_Libya_zone_11 = 2074,
        ///<summary>ELD79 Libya zone 12</summary>
        ELD79_Libya_zone_12 = 2075,
        ///<summary>ELD79 Libya zone 13</summary>
        ELD79_Libya_zone_13 = 2076,
        ///<summary>ELD79 UTM zone 32N</summary>
        ELD79_UTM_zone_32N = 2077,
        ///<summary>ELD79 UTM zone 33N</summary>
        ELD79_UTM_zone_33N = 2078,
        ///<summary>ELD79 UTM zone 34N</summary>
        ELD79_UTM_zone_34N = 2079,
        ///<summary>ELD79 UTM zone 35N</summary>
        ELD79_UTM_zone_35N = 2080,
        ///<summary>Chos Malal 1914 Argentina zone 2</summary>
        Chos_Malal_1914_Argentina_zone_2 = 2081,
        ///<summary>Pampa del Castillo Argentina zone 2</summary>
        Pampa_del_Castillo_Argentina_zone_2 = 2082,
        ///<summary>Hito XVIII 1963 Argentina zone 2</summary>
        Hito_XVIII_1963_Argentina_zone_2 = 2083,
        ///<summary>Hito XVIII 1963 UTM zone 19S</summary>
        Hito_XVIII_1963_UTM_zone_19S = 2084,
        ///<summary>NAD27 Cuba Norte deprecated</summary>
        NAD27_Cuba_Norte_deprecated = 2085,
        ///<summary>NAD27 Cuba Sur deprecated</summary>
        NAD27_Cuba_Sur_deprecated = 2086,
        ///<summary>ELD79 TM 12 NE</summary>
        ELD79_TM_12_NE = 2087,
        ///<summary>Carthage TM 11 NE</summary>
        Carthage_TM_11_NE = 2088,
        ///<summary>Yemen NGN96 UTM zone 38N</summary>
        Yemen_NGN96_UTM_zone_38N = 2089,
        ///<summary>Yemen NGN96 UTM zone 39N</summary>
        Yemen_NGN96_UTM_zone_39N = 2090,
        ///<summary>South Yemen Gauss Kruger zone 8 deprecated</summary>
        South_Yemen_Gauss_Kruger_zone_8_deprecated = 2091,
        ///<summary>South Yemen Gauss Kruger zone 9 deprecated</summary>
        South_Yemen_Gauss_Kruger_zone_9_deprecated = 2092,
        ///<summary>Hanoi 1972 GK 106 NE</summary>
        Hanoi_1972_GK_106_NE = 2093,
        ///<summary>WGS 72BE TM 106 NE</summary>
        WGS_72BE_TM_106_NE = 2094,
        ///<summary>Bissau UTM zone 28N</summary>
        Bissau_UTM_zone_28N = 2095,
        ///<summary>Korean 1985 Korea East Belt</summary>
        Korean_1985_Korea_East_Belt = 2096,
        ///<summary>Korean 1985 Korea Central Belt</summary>
        Korean_1985_Korea_Central_Belt = 2097,
        ///<summary>Korean 1985 Korea West Belt</summary>
        Korean_1985_Korea_West_Belt = 2098,
        ///<summary>Qatar 1948 Qatar Grid</summary>
        Qatar_1948_Qatar_Grid = 2099,
        ///<summary>GGRS87 Greek Grid</summary>
        GGRS87_Greek_Grid = 2100,
        ///<summary>Lake Maracaibo Grid M1</summary>
        Lake_Maracaibo_Grid_M1 = 2101,
        ///<summary>Lake Maracaibo Grid</summary>
        Lake_Maracaibo_Grid = 2102,
        ///<summary>Lake Maracaibo Grid M3</summary>
        Lake_Maracaibo_Grid_M3 = 2103,
        ///<summary>Lake Maracaibo La Rosa Grid</summary>
        Lake_Maracaibo_La_Rosa_Grid = 2104,
        ///<summary>NZGD2000 Mount Eden 2000</summary>
        NZGD2000_Mount_Eden_2000 = 2105,
        ///<summary>NZGD2000 Bay of Plenty 2000</summary>
        NZGD2000_Bay_of_Plenty_2000 = 2106,
        ///<summary>NZGD2000 Poverty Bay 2000</summary>
        NZGD2000_Poverty_Bay_2000 = 2107,
        ///<summary>NZGD2000 Hawkes Bay 2000</summary>
        NZGD2000_Hawkes_Bay_2000 = 2108,
        ///<summary>NZGD2000 Taranaki 2000</summary>
        NZGD2000_Taranaki_2000 = 2109,
        ///<summary>NZGD2000 Tuhirangi 2000</summary>
        NZGD2000_Tuhirangi_2000 = 2110,
        ///<summary>NZGD2000 Wanganui 2000</summary>
        NZGD2000_Wanganui_2000 = 2111,
        ///<summary>NZGD2000 Wairarapa 2000</summary>
        NZGD2000_Wairarapa_2000 = 2112,
        ///<summary>NZGD2000 Wellington 2000</summary>
        NZGD2000_Wellington_2000 = 2113,
        ///<summary>NZGD2000 Collingwood 2000</summary>
        NZGD2000_Collingwood_2000 = 2114,
        ///<summary>NZGD2000 Nelson 2000</summary>
        NZGD2000_Nelson_2000 = 2115,
        ///<summary>NZGD2000 Karamea 2000</summary>
        NZGD2000_Karamea_2000 = 2116,
        ///<summary>NZGD2000 Buller 2000</summary>
        NZGD2000_Buller_2000 = 2117,
        ///<summary>NZGD2000 Grey 2000</summary>
        NZGD2000_Grey_2000 = 2118,
        ///<summary>NZGD2000 Amuri 2000</summary>
        NZGD2000_Amuri_2000 = 2119,
        ///<summary>NZGD2000 Marlborough 2000</summary>
        NZGD2000_Marlborough_2000 = 2120,
        ///<summary>NZGD2000 Hokitika 2000</summary>
        NZGD2000_Hokitika_2000 = 2121,
        ///<summary>NZGD2000 Okarito 2000</summary>
        NZGD2000_Okarito_2000 = 2122,
        ///<summary>NZGD2000 Jacksons Bay 2000</summary>
        NZGD2000_Jacksons_Bay_2000 = 2123,
        ///<summary>NZGD2000 Mount Pleasant 2000</summary>
        NZGD2000_Mount_Pleasant_2000 = 2124,
        ///<summary>NZGD2000 Gawler 2000</summary>
        NZGD2000_Gawler_2000 = 2125,
        ///<summary>NZGD2000 Timaru 2000</summary>
        NZGD2000_Timaru_2000 = 2126,
        ///<summary>NZGD2000 Lindis Peak 2000</summary>
        NZGD2000_Lindis_Peak_2000 = 2127,
        ///<summary>NZGD2000 Mount Nicholas 2000</summary>
        NZGD2000_Mount_Nicholas_2000 = 2128,
        ///<summary>NZGD2000 Mount York 2000</summary>
        NZGD2000_Mount_York_2000 = 2129,
        ///<summary>NZGD2000 Observation Point 2000</summary>
        NZGD2000_Observation_Point_2000 = 2130,
        ///<summary>NZGD2000 North Taieri 2000</summary>
        NZGD2000_North_Taieri_2000 = 2131,
        ///<summary>NZGD2000 Bluff 2000</summary>
        NZGD2000_Bluff_2000 = 2132,
        ///<summary>NZGD2000 UTM zone 58S</summary>
        NZGD2000_UTM_zone_58S = 2133,
        ///<summary>NZGD2000 UTM zone 59S</summary>
        NZGD2000_UTM_zone_59S = 2134,
        ///<summary>NZGD2000 UTM zone 60S</summary>
        NZGD2000_UTM_zone_60S = 2135,
        ///<summary>Accra Ghana National Grid</summary>
        Accra_Ghana_National_Grid = 2136,
        ///<summary>Accra TM 1 NW</summary>
        Accra_TM_1_NW = 2137,
        ///<summary>NAD27CGQ77 Quebec Lambert</summary>
        NAD27CGQ77_Quebec_Lambert = 2138,
        ///<summary>NAD83CSRS98 SCoPQ zone 2 deprecated</summary>
        NAD83CSRS98_SCoPQ_zone_2_deprecated = 2139,
        ///<summary>NAD83CSRS98 MTM zone 3 deprecated</summary>
        NAD83CSRS98_MTM_zone_3_deprecated = 2140,
        ///<summary>NAD83CSRS98 MTM zone 4 deprecated</summary>
        NAD83CSRS98_MTM_zone_4_deprecated = 2141,
        ///<summary>NAD83CSRS98 MTM zone 5 deprecated</summary>
        NAD83CSRS98_MTM_zone_5_deprecated = 2142,
        ///<summary>NAD83CSRS98 MTM zone 6 deprecated</summary>
        NAD83CSRS98_MTM_zone_6_deprecated = 2143,
        ///<summary>NAD83CSRS98 MTM zone 7 deprecated</summary>
        NAD83CSRS98_MTM_zone_7_deprecated = 2144,
        ///<summary>NAD83CSRS98 MTM zone 8 deprecated</summary>
        NAD83CSRS98_MTM_zone_8_deprecated = 2145,
        ///<summary>NAD83CSRS98 MTM zone 9 deprecated</summary>
        NAD83CSRS98_MTM_zone_9_deprecated = 2146,
        ///<summary>NAD83CSRS98 MTM zone 10 deprecated</summary>
        NAD83CSRS98_MTM_zone_10_deprecated = 2147,
        ///<summary>NAD83CSRS98 UTM zone 21N deprecated</summary>
        NAD83CSRS98_UTM_zone_21N_deprecated = 2148,
        ///<summary>NAD83CSRS98 UTM zone 18N deprecated</summary>
        NAD83CSRS98_UTM_zone_18N_deprecated = 2149,
        ///<summary>NAD83CSRS98 UTM zone 17N deprecated</summary>
        NAD83CSRS98_UTM_zone_17N_deprecated = 2150,
        ///<summary>NAD83CSRS98 UTM zone 13N deprecated</summary>
        NAD83CSRS98_UTM_zone_13N_deprecated = 2151,
        ///<summary>NAD83CSRS98 UTM zone 12N deprecated</summary>
        NAD83CSRS98_UTM_zone_12N_deprecated = 2152,
        ///<summary>NAD83CSRS98 UTM zone 11N deprecated</summary>
        NAD83CSRS98_UTM_zone_11N_deprecated = 2153,
        ///<summary>RGF93 Lambert93</summary>
        RGF93_Lambert93 = 2154,
        ///<summary>NAD83HARN UTM zone 59S deprecated</summary>
        NAD83HARN_UTM_zone_59S_deprecated = 2156,
        ///<summary>IRENET95 Irish Transverse Mercator</summary>
        IRENET95_Irish_Transverse_Mercator = 2157,
        ///<summary>IRENET95 UTM zone 29N</summary>
        IRENET95_UTM_zone_29N = 2158,
        ///<summary>Sierra Leone 1924 New Colony Grid</summary>
        Sierra_Leone_1924_New_Colony_Grid = 2159,
        ///<summary>Sierra Leone 1924 New War Office Grid</summary>
        Sierra_Leone_1924_New_War_Office_Grid = 2160,
        ///<summary>Sierra Leone 1968 UTM zone 28N</summary>
        Sierra_Leone_1968_UTM_zone_28N = 2161,
        ///<summary>Sierra Leone 1968 UTM zone 29N</summary>
        Sierra_Leone_1968_UTM_zone_29N = 2162,
        ///<summary>US National Atlas Equal Area</summary>
        US_National_Atlas_Equal_Area = 2163,
        ///<summary>Locodjo 1965 TM 5 NW</summary>
        Locodjo_1965_TM_5_NW = 2164,
        ///<summary>Abidjan 1987 TM 5 NW</summary>
        Abidjan_1987_TM_5_NW = 2165,
        ///<summary>Pulkovo 194283 Gauss Kruger zone 3 deprecated</summary>
        Pulkovo_194283_Gauss_Kruger_zone_3_deprecated = 2166,
        ///<summary>Pulkovo 194283 Gauss Kruger zone 4 deprecated</summary>
        Pulkovo_194283_Gauss_Kruger_zone_4_deprecated = 2167,
        ///<summary>Pulkovo 194283 Gauss Kruger zone 5 deprecated</summary>
        Pulkovo_194283_Gauss_Kruger_zone_5_deprecated = 2168,
        ///<summary>Luxembourg 1930 Gauss</summary>
        Luxembourg_1930_Gauss = 2169,
        ///<summary>MGI Slovenia Grid</summary>
        MGI_Slovenia_Grid = 2170,
        ///<summary>Pulkovo 194258 Poland zone I deprecated</summary>
        Pulkovo_194258_Poland_zone_I_deprecated = 2171,
        ///<summary>Pulkovo 194258 Poland zone II</summary>
        Pulkovo_194258_Poland_zone_II = 2172,
        ///<summary>Pulkovo 194258 Poland zone III</summary>
        Pulkovo_194258_Poland_zone_III = 2173,
        ///<summary>Pulkovo 194258 Poland zone IV</summary>
        Pulkovo_194258_Poland_zone_IV = 2174,
        ///<summary>Pulkovo 194258 Poland zone V</summary>
        Pulkovo_194258_Poland_zone_V = 2175,
        ///<summary>ETRS89 Poland CS2000 zone 5</summary>
        ETRS89_Poland_CS2000_zone_5 = 2176,
        ///<summary>ETRS89 Poland CS2000 zone 6</summary>
        ETRS89_Poland_CS2000_zone_6 = 2177,
        ///<summary>ETRS89 Poland CS2000 zone 7</summary>
        ETRS89_Poland_CS2000_zone_7 = 2178,
        ///<summary>ETRS89 Poland CS2000 zone 8</summary>
        ETRS89_Poland_CS2000_zone_8 = 2179,
        ///<summary>ETRS89 Poland CS92</summary>
        ETRS89_Poland_CS92 = 2180,
        ///<summary>Azores Occidental 1939 UTM zone 25N</summary>
        Azores_Occidental_1939_UTM_zone_25N = 2188,
        ///<summary>Azores Central 1948 UTM zone 26N</summary>
        Azores_Central_1948_UTM_zone_26N = 2189,
        ///<summary>Azores Oriental 1940 UTM zone 26N</summary>
        Azores_Oriental_1940_UTM_zone_26N = 2190,
        ///<summary>Madeira 1936 UTM zone 28N deprecated</summary>
        Madeira_1936_UTM_zone_28N_deprecated = 2191,
        ///<summary>ED50 France EuroLambert</summary>
        ED50_France_EuroLambert = 2192,
        ///<summary>NZGD2000 New Zealand Transverse Mercator 2000</summary>
        NZGD2000_New_Zealand_Transverse_Mercator_2000 = 2193,
        ///<summary>American Samoa 1962 American Samoa Lambert deprecated</summary>
        American_Samoa_1962_American_Samoa_Lambert_deprecated = 2194,
        ///<summary>NAD83HARN UTM zone 2S</summary>
        NAD83HARN_UTM_zone_2S = 2195,
        ///<summary>ETRS89 Kp2000 Jutland</summary>
        ETRS89_Kp2000_Jutland = 2196,
        ///<summary>ETRS89 Kp2000 Zealand</summary>
        ETRS89_Kp2000_Zealand = 2197,
        ///<summary>ETRS89 Kp2000 Bornholm</summary>
        ETRS89_Kp2000_Bornholm = 2198,
        ///<summary>ATS77 New Brunswick Stereographic ATS77</summary>
        ATS77_New_Brunswick_Stereographic_ATS77 = 2200,
        ///<summary>REGVEN UTM zone 18N</summary>
        REGVEN_UTM_zone_18N = 2201,
        ///<summary>REGVEN UTM zone 19N</summary>
        REGVEN_UTM_zone_19N = 2202,
        ///<summary>REGVEN UTM zone 20N</summary>
        REGVEN_UTM_zone_20N = 2203,
        ///<summary>NAD27 Tennessee</summary>
        NAD27_Tennessee = 2204,
        ///<summary>NAD83 Kentucky North</summary>
        NAD83_Kentucky_North = 2205,
        ///<summary>ED50 3degree GaussKruger zone 9</summary>
        ED50_3degree_GaussKruger_zone_9 = 2206,
        ///<summary>ED50 3degree GaussKruger zone 10</summary>
        ED50_3degree_GaussKruger_zone_10 = 2207,
        ///<summary>ED50 3degree GaussKruger zone 11</summary>
        ED50_3degree_GaussKruger_zone_11 = 2208,
        ///<summary>ED50 3degree GaussKruger zone 12</summary>
        ED50_3degree_GaussKruger_zone_12 = 2209,
        ///<summary>ED50 3degree GaussKruger zone 13</summary>
        ED50_3degree_GaussKruger_zone_13 = 2210,
        ///<summary>ED50 3degree GaussKruger zone 14</summary>
        ED50_3degree_GaussKruger_zone_14 = 2211,
        ///<summary>ED50 3degree GaussKruger zone 15</summary>
        ED50_3degree_GaussKruger_zone_15 = 2212,
        ///<summary>ETRS89 TM 30 NE</summary>
        ETRS89_TM_30_NE = 2213,
        ///<summary>Douala 1948 AOF west deprecated</summary>
        Douala_1948_AOF_west_deprecated = 2214,
        ///<summary>Manoca 1962 UTM zone 32N</summary>
        Manoca_1962_UTM_zone_32N = 2215,
        ///<summary>Qornoq 1927 UTM zone 22N</summary>
        Qornoq_1927_UTM_zone_22N = 2216,
        ///<summary>Qornoq 1927 UTM zone 23N</summary>
        Qornoq_1927_UTM_zone_23N = 2217,
        ///<summary>Scoresbysund 1952 Greenland zone 5 east</summary>
        Scoresbysund_1952_Greenland_zone_5_east = 2218,
        ///<summary>ATS77 UTM zone 19N</summary>
        ATS77_UTM_zone_19N = 2219,
        ///<summary>ATS77 UTM zone 20N</summary>
        ATS77_UTM_zone_20N = 2220,
        ///<summary>Scoresbysund 1952 Greenland zone 6 east</summary>
        Scoresbysund_1952_Greenland_zone_6_east = 2221,
        ///<summary>NAD83 Arizona East ft</summary>
        NAD83_Arizona_East_ft = 2222,
        ///<summary>NAD83 Arizona Central ft</summary>
        NAD83_Arizona_Central_ft = 2223,
        ///<summary>NAD83 Arizona West ft</summary>
        NAD83_Arizona_West_ft = 2224,
        ///<summary>NAD83 California zone 1 ftUS</summary>
        NAD83_California_zone_1_ftUS = 2225,
        ///<summary>NAD83 California zone 2 ftUS</summary>
        NAD83_California_zone_2_ftUS = 2226,
        ///<summary>NAD83 California zone 3 ftUS</summary>
        NAD83_California_zone_3_ftUS = 2227,
        ///<summary>NAD83 California zone 4 ftUS</summary>
        NAD83_California_zone_4_ftUS = 2228,
        ///<summary>NAD83 California zone 5 ftUS</summary>
        NAD83_California_zone_5_ftUS = 2229,
        ///<summary>NAD83 California zone 6 ftUS</summary>
        NAD83_California_zone_6_ftUS = 2230,
        ///<summary>NAD83 Colorado North ftUS</summary>
        NAD83_Colorado_North_ftUS = 2231,
        ///<summary>NAD83 Colorado Central ftUS</summary>
        NAD83_Colorado_Central_ftUS = 2232,
        ///<summary>NAD83 Colorado South ftUS</summary>
        NAD83_Colorado_South_ftUS = 2233,
        ///<summary>NAD83 Connecticut ftUS</summary>
        NAD83_Connecticut_ftUS = 2234,
        ///<summary>NAD83 Delaware ftUS</summary>
        NAD83_Delaware_ftUS = 2235,
        ///<summary>NAD83 Florida East ftUS</summary>
        NAD83_Florida_East_ftUS = 2236,
        ///<summary>NAD83 Florida West ftUS</summary>
        NAD83_Florida_West_ftUS = 2237,
        ///<summary>NAD83 Florida North ftUS</summary>
        NAD83_Florida_North_ftUS = 2238,
        ///<summary>NAD83 Georgia East ftUS</summary>
        NAD83_Georgia_East_ftUS = 2239,
        ///<summary>NAD83 Georgia West ftUS</summary>
        NAD83_Georgia_West_ftUS = 2240,
        ///<summary>NAD83 Idaho East ftUS</summary>
        NAD83_Idaho_East_ftUS = 2241,
        ///<summary>NAD83 Idaho Central ftUS</summary>
        NAD83_Idaho_Central_ftUS = 2242,
        ///<summary>NAD83 Idaho West ftUS</summary>
        NAD83_Idaho_West_ftUS = 2243,
        ///<summary>NAD83 Indiana East ftUS deprecated</summary>
        NAD83_Indiana_East_ftUS_deprecated = 2244,
        ///<summary>NAD83 Indiana West ftUS deprecated</summary>
        NAD83_Indiana_West_ftUS_deprecated = 2245,
        ///<summary>NAD83 Kentucky North ftUS</summary>
        NAD83_Kentucky_North_ftUS = 2246,
        ///<summary>NAD83 Kentucky South ftUS</summary>
        NAD83_Kentucky_South_ftUS = 2247,
        ///<summary>NAD83 Maryland ftUS</summary>
        NAD83_Maryland_ftUS = 2248,
        ///<summary>NAD83 Massachusetts Mainland ftUS</summary>
        NAD83_Massachusetts_Mainland_ftUS = 2249,
        ///<summary>NAD83 Massachusetts Island ftUS</summary>
        NAD83_Massachusetts_Island_ftUS = 2250,
        ///<summary>NAD83 Michigan North ft</summary>
        NAD83_Michigan_North_ft = 2251,
        ///<summary>NAD83 Michigan Central ft</summary>
        NAD83_Michigan_Central_ft = 2252,
        ///<summary>NAD83 Michigan South ft</summary>
        NAD83_Michigan_South_ft = 2253,
        ///<summary>NAD83 Mississippi East ftUS</summary>
        NAD83_Mississippi_East_ftUS = 2254,
        ///<summary>NAD83 Mississippi West ftUS</summary>
        NAD83_Mississippi_West_ftUS = 2255,
        ///<summary>NAD83 Montana ft</summary>
        NAD83_Montana_ft = 2256,
        ///<summary>NAD83 New Mexico East ftUS</summary>
        NAD83_New_Mexico_East_ftUS = 2257,
        ///<summary>NAD83 New Mexico Central ftUS</summary>
        NAD83_New_Mexico_Central_ftUS = 2258,
        ///<summary>NAD83 New Mexico West ftUS</summary>
        NAD83_New_Mexico_West_ftUS = 2259,
        ///<summary>NAD83 New York East ftUS</summary>
        NAD83_New_York_East_ftUS = 2260,
        ///<summary>NAD83 New York Central ftUS</summary>
        NAD83_New_York_Central_ftUS = 2261,
        ///<summary>NAD83 New York West ftUS</summary>
        NAD83_New_York_West_ftUS = 2262,
        ///<summary>NAD83 New York Long Island ftUS</summary>
        NAD83_New_York_Long_Island_ftUS = 2263,
        ///<summary>NAD83 North Carolina ftUS</summary>
        NAD83_North_Carolina_ftUS = 2264,
        ///<summary>NAD83 North Dakota North ft</summary>
        NAD83_North_Dakota_North_ft = 2265,
        ///<summary>NAD83 North Dakota South ft</summary>
        NAD83_North_Dakota_South_ft = 2266,
        ///<summary>NAD83 Oklahoma North ftUS</summary>
        NAD83_Oklahoma_North_ftUS = 2267,
        ///<summary>NAD83 Oklahoma South ftUS</summary>
        NAD83_Oklahoma_South_ftUS = 2268,
        ///<summary>NAD83 Oregon North ft</summary>
        NAD83_Oregon_North_ft = 2269,
        ///<summary>NAD83 Oregon South ft</summary>
        NAD83_Oregon_South_ft = 2270,
        ///<summary>NAD83 Pennsylvania North ftUS</summary>
        NAD83_Pennsylvania_North_ftUS = 2271,
        ///<summary>NAD83 Pennsylvania South ftUS</summary>
        NAD83_Pennsylvania_South_ftUS = 2272,
        ///<summary>NAD83 South Carolina ft</summary>
        NAD83_South_Carolina_ft = 2273,
        ///<summary>NAD83 Tennessee ftUS</summary>
        NAD83_Tennessee_ftUS = 2274,
        ///<summary>NAD83 Texas North ftUS</summary>
        NAD83_Texas_North_ftUS = 2275,
        ///<summary>NAD83 Texas North Central ftUS</summary>
        NAD83_Texas_North_Central_ftUS = 2276,
        ///<summary>NAD83 Texas Central ftUS</summary>
        NAD83_Texas_Central_ftUS = 2277,
        ///<summary>NAD83 Texas South Central ftUS</summary>
        NAD83_Texas_South_Central_ftUS = 2278,
        ///<summary>NAD83 Texas South ftUS</summary>
        NAD83_Texas_South_ftUS = 2279,
        ///<summary>NAD83 Utah North ft</summary>
        NAD83_Utah_North_ft = 2280,
        ///<summary>NAD83 Utah Central ft</summary>
        NAD83_Utah_Central_ft = 2281,
        ///<summary>NAD83 Utah South ft</summary>
        NAD83_Utah_South_ft = 2282,
        ///<summary>NAD83 Virginia North ftUS</summary>
        NAD83_Virginia_North_ftUS = 2283,
        ///<summary>NAD83 Virginia South ftUS</summary>
        NAD83_Virginia_South_ftUS = 2284,
        ///<summary>NAD83 Washington North ftUS</summary>
        NAD83_Washington_North_ftUS = 2285,
        ///<summary>NAD83 Washington South ftUS</summary>
        NAD83_Washington_South_ftUS = 2286,
        ///<summary>NAD83 Wisconsin North ftUS</summary>
        NAD83_Wisconsin_North_ftUS = 2287,
        ///<summary>NAD83 Wisconsin Central ftUS</summary>
        NAD83_Wisconsin_Central_ftUS = 2288,
        ///<summary>NAD83 Wisconsin South ftUS</summary>
        NAD83_Wisconsin_South_ftUS = 2289,
        ///<summary>ATS77 Prince Edward Isl Stereographic ATS77</summary>
        ATS77_Prince_Edward_Isl_Stereographic_ATS77 = 2290,
        ///<summary>NAD83CSRS98 Prince Edward Isl Stereographic NAD83 deprecated</summary>
        NAD83CSRS98_Prince_Edward_Isl_Stereographic_NAD83_deprecated = 2291,
        ///<summary>ATS77 MTM Nova Scotia zone 4</summary>
        ATS77_MTM_Nova_Scotia_zone_4 = 2294,
        ///<summary>ATS77 MTM Nova Scotia zone 5</summary>
        ATS77_MTM_Nova_Scotia_zone_5 = 2295,
        ///<summary>Qornoq 1927 Greenland zone 3 west</summary>
        Qornoq_1927_Greenland_zone_3_west = 2301,
        ///<summary>Qornoq 1927 Greenland zone 4 east deprecated</summary>
        Qornoq_1927_Greenland_zone_4_east_deprecated = 2302,
        ///<summary>Qornoq 1927 Greenland zone 4 west</summary>
        Qornoq_1927_Greenland_zone_4_west = 2303,
        ///<summary>Qornoq 1927 Greenland zone 5 west</summary>
        Qornoq_1927_Greenland_zone_5_west = 2304,
        ///<summary>Qornoq 1927 Greenland zone 6 west</summary>
        Qornoq_1927_Greenland_zone_6_west = 2305,
        ///<summary>Qornoq 1927 Greenland zone 7 west</summary>
        Qornoq_1927_Greenland_zone_7_west = 2306,
        ///<summary>Qornoq 1927 Greenland zone 8 east</summary>
        Qornoq_1927_Greenland_zone_8_east = 2307,
        ///<summary>Batavia TM 109 SE</summary>
        Batavia_TM_109_SE = 2308,
        ///<summary>WGS 84 TM 116 SE</summary>
        WGS_84_TM_116_SE = 2309,
        ///<summary>WGS 84 TM 132 SE</summary>
        WGS_84_TM_132_SE = 2310,
        ///<summary>WGS 84 TM 6 NE</summary>
        WGS_84_TM_6_NE = 2311,
        ///<summary>Garoua UTM zone 33N</summary>
        Garoua_UTM_zone_33N = 2312,
        ///<summary>Kousseri UTM zone 33N</summary>
        Kousseri_UTM_zone_33N = 2313,
        ///<summary>Trinidad 1903 Trinidad Grid ftCla</summary>
        Trinidad_1903_Trinidad_Grid_ftCla = 2314,
        ///<summary>Campo Inchauspe UTM zone 19S</summary>
        Campo_Inchauspe_UTM_zone_19S = 2315,
        ///<summary>Campo Inchauspe UTM zone 20S</summary>
        Campo_Inchauspe_UTM_zone_20S = 2316,
        ///<summary>PSAD56 ICN Regional</summary>
        PSAD56_ICN_Regional = 2317,
        ///<summary>Ain el Abd Aramco Lambert</summary>
        Ain_el_Abd_Aramco_Lambert = 2318,
        ///<summary>ED50 TM27</summary>
        ED50_TM27 = 2319,
        ///<summary>ED50 TM30</summary>
        ED50_TM30 = 2320,
        ///<summary>ED50 TM33</summary>
        ED50_TM33 = 2321,
        ///<summary>ED50 TM36</summary>
        ED50_TM36 = 2322,
        ///<summary>ED50 TM39</summary>
        ED50_TM39 = 2323,
        ///<summary>ED50 TM42</summary>
        ED50_TM42 = 2324,
        ///<summary>ED50 TM45</summary>
        ED50_TM45 = 2325,
        ///<summary>Hong Kong 1980 Grid System</summary>
        Hong_Kong_1980_Grid_System = 2326,
        ///<summary>Xian 1980 GaussKruger zone 13</summary>
        Xian_1980_GaussKruger_zone_13 = 2327,
        ///<summary>Xian 1980 GaussKruger zone 14</summary>
        Xian_1980_GaussKruger_zone_14 = 2328,
        ///<summary>Xian 1980 GaussKruger zone 15</summary>
        Xian_1980_GaussKruger_zone_15 = 2329,
        ///<summary>Xian 1980 GaussKruger zone 16</summary>
        Xian_1980_GaussKruger_zone_16 = 2330,
        ///<summary>Xian 1980 GaussKruger zone 17</summary>
        Xian_1980_GaussKruger_zone_17 = 2331,
        ///<summary>Xian 1980 GaussKruger zone 18</summary>
        Xian_1980_GaussKruger_zone_18 = 2332,
        ///<summary>Xian 1980 GaussKruger zone 19</summary>
        Xian_1980_GaussKruger_zone_19 = 2333,
        ///<summary>Xian 1980 GaussKruger zone 20</summary>
        Xian_1980_GaussKruger_zone_20 = 2334,
        ///<summary>Xian 1980 GaussKruger zone 21</summary>
        Xian_1980_GaussKruger_zone_21 = 2335,
        ///<summary>Xian 1980 GaussKruger zone 22</summary>
        Xian_1980_GaussKruger_zone_22 = 2336,
        ///<summary>Xian 1980 GaussKruger zone 23</summary>
        Xian_1980_GaussKruger_zone_23 = 2337,
        ///<summary>Xian 1980 GaussKruger CM 75E</summary>
        Xian_1980_GaussKruger_CM_75E = 2338,
        ///<summary>Xian 1980 GaussKruger CM 81E</summary>
        Xian_1980_GaussKruger_CM_81E = 2339,
        ///<summary>Xian 1980 GaussKruger CM 87E</summary>
        Xian_1980_GaussKruger_CM_87E = 2340,
        ///<summary>Xian 1980 GaussKruger CM 93E</summary>
        Xian_1980_GaussKruger_CM_93E = 2341,
        ///<summary>Xian 1980 GaussKruger CM 99E</summary>
        Xian_1980_GaussKruger_CM_99E = 2342,
        ///<summary>Xian 1980 GaussKruger CM 105E</summary>
        Xian_1980_GaussKruger_CM_105E = 2343,
        ///<summary>Xian 1980 GaussKruger CM 111E</summary>
        Xian_1980_GaussKruger_CM_111E = 2344,
        ///<summary>Xian 1980 GaussKruger CM 117E</summary>
        Xian_1980_GaussKruger_CM_117E = 2345,
        ///<summary>Xian 1980 GaussKruger CM 123E</summary>
        Xian_1980_GaussKruger_CM_123E = 2346,
        ///<summary>Xian 1980 GaussKruger CM 129E</summary>
        Xian_1980_GaussKruger_CM_129E = 2347,
        ///<summary>Xian 1980 GaussKruger CM 135E</summary>
        Xian_1980_GaussKruger_CM_135E = 2348,
        ///<summary>Xian 1980 3degree GaussKruger zone 25</summary>
        Xian_1980_3degree_GaussKruger_zone_25 = 2349,
        ///<summary>Xian 1980 3degree GaussKruger zone 26</summary>
        Xian_1980_3degree_GaussKruger_zone_26 = 2350,
        ///<summary>Xian 1980 3degree GaussKruger zone 27</summary>
        Xian_1980_3degree_GaussKruger_zone_27 = 2351,
        ///<summary>Xian 1980 3degree GaussKruger zone 28</summary>
        Xian_1980_3degree_GaussKruger_zone_28 = 2352,
        ///<summary>Xian 1980 3degree GaussKruger zone 29</summary>
        Xian_1980_3degree_GaussKruger_zone_29 = 2353,
        ///<summary>Xian 1980 3degree GaussKruger zone 30</summary>
        Xian_1980_3degree_GaussKruger_zone_30 = 2354,
        ///<summary>Xian 1980 3degree GaussKruger zone 31</summary>
        Xian_1980_3degree_GaussKruger_zone_31 = 2355,
        ///<summary>Xian 1980 3degree GaussKruger zone 32</summary>
        Xian_1980_3degree_GaussKruger_zone_32 = 2356,
        ///<summary>Xian 1980 3degree GaussKruger zone 33</summary>
        Xian_1980_3degree_GaussKruger_zone_33 = 2357,
        ///<summary>Xian 1980 3degree GaussKruger zone 34</summary>
        Xian_1980_3degree_GaussKruger_zone_34 = 2358,
        ///<summary>Xian 1980 3degree GaussKruger zone 35</summary>
        Xian_1980_3degree_GaussKruger_zone_35 = 2359,
        ///<summary>Xian 1980 3degree GaussKruger zone 36</summary>
        Xian_1980_3degree_GaussKruger_zone_36 = 2360,
        ///<summary>Xian 1980 3degree GaussKruger zone 39</summary>
        Xian_1980_3degree_GaussKruger_zone_39 = 2363,
        ///<summary>KKJ Finland zone 1</summary>
        KKJ_Finland_zone_1 = 2391,
        ///<summary>KKJ Finland zone 2</summary>
        KKJ_Finland_zone_2 = 2392,
        ///<summary>KKJ Finland Uniform Coordinate System</summary>
        KKJ_Finland_Uniform_Coordinate_System = 2393,
        ///<summary>KKJ Finland zone 4</summary>
        KKJ_Finland_zone_4 = 2394,
        ///<summary>RT90 25 gon W deprecated</summary>
        RT90_25_gon_W_deprecated = 2400,
        ///<summary>Beijing 1954 3degree GaussKruger zone 25</summary>
        Beijing_1954_3degree_GaussKruger_zone_25 = 2401,
        ///<summary>Beijing 1954 3degree GaussKruger zone 26</summary>
        Beijing_1954_3degree_GaussKruger_zone_26 = 2402,
        ///<summary>Beijing 1954 3degree GaussKruger zone 27</summary>
        Beijing_1954_3degree_GaussKruger_zone_27 = 2403,
        ///<summary>JGD2000 Japan Plane Rectangular CS I</summary>
        JGD2000_Japan_Plane_Rectangular_CS_I = 2443,
        ///<summary>JGD2000 Japan Plane Rectangular CS II</summary>
        JGD2000_Japan_Plane_Rectangular_CS_II = 2444,
        ///<summary>JGD2000 Japan Plane Rectangular CS III</summary>
        JGD2000_Japan_Plane_Rectangular_CS_III = 2445,
        ///<summary>JGD2000 Japan Plane Rectangular CS IV</summary>
        JGD2000_Japan_Plane_Rectangular_CS_IV = 2446,
        ///<summary>JGD2000 Japan Plane Rectangular CS V</summary>
        JGD2000_Japan_Plane_Rectangular_CS_V = 2447,
        ///<summary>JGD2000 Japan Plane Rectangular CS VI</summary>
        JGD2000_Japan_Plane_Rectangular_CS_VI = 2448,
        ///<summary>JGD2000 Japan Plane Rectangular CS VII</summary>
        JGD2000_Japan_Plane_Rectangular_CS_VII = 2449,
        ///<summary>JGD2000 Japan Plane Rectangular CS VIII</summary>
        JGD2000_Japan_Plane_Rectangular_CS_VIII = 2450,
        ///<summary>JGD2000 Japan Plane Rectangular CS IX</summary>
        JGD2000_Japan_Plane_Rectangular_CS_IX = 2451,
        ///<summary>JGD2000 Japan Plane Rectangular CS X</summary>
        JGD2000_Japan_Plane_Rectangular_CS_X = 2452,
        ///<summary>JGD2000 Japan Plane Rectangular CS XI</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XI = 2453,
        ///<summary>JGD2000 Japan Plane Rectangular CS XII</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XII = 2454,
        ///<summary>JGD2000 Japan Plane Rectangular CS XIII</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XIII = 2455,
        ///<summary>JGD2000 Japan Plane Rectangular CS XIV</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XIV = 2456,
        ///<summary>JGD2000 Japan Plane Rectangular CS XV</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XV = 2457,
        ///<summary>JGD2000 Japan Plane Rectangular CS XVI</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XVI = 2458,
        ///<summary>JGD2000 Japan Plane Rectangular CS XVII</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XVII = 2459,
        ///<summary>JGD2000 Japan Plane Rectangular CS XVIII</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XVIII = 2460,
        ///<summary>JGD2000 Japan Plane Rectangular CS XIX</summary>
        JGD2000_Japan_Plane_Rectangular_CS_XIX = 2461,
        ///<summary>Albanian 1987 GaussKruger zone 4</summary>
        Albanian_1987_GaussKruger_zone_4 = 2462,
        ///<summary>Pulkovo 1995 GaussKruger CM 21E</summary>
        Pulkovo_1995_GaussKruger_CM_21E = 2463,
        ///<summary>Pulkovo 1995 GaussKruger CM 27E</summary>
        Pulkovo_1995_GaussKruger_CM_27E = 2464,
        ///<summary>Pulkovo 1995 GaussKruger CM 33E</summary>
        Pulkovo_1995_GaussKruger_CM_33E = 2465,
        ///<summary>Pulkovo 1995 GaussKruger CM 171W</summary>
        Pulkovo_1995_GaussKruger_CM_171W = 2491,
        ///<summary>Pulkovo 1942 GaussKruger CM 9E</summary>
        Pulkovo_1942_GaussKruger_CM_9E = 2492,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 7</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_7 = 2523,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 8</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_8 = 2524,
        ///<summary>Samboja UTM zone 50S deprecated</summary>
        Samboja_UTM_zone_50S_deprecated = 2550,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 53</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_53 = 2570,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 54</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_54 = 2571,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 55</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_55 = 2572,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 56</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_56 = 2573,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 57</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_57 = 2574,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 58</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_58 = 2575,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 59</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_59 = 2576,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 60 deprecated</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_60_deprecated = 2577,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 61</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_61 = 2578,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 62</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_62 = 2579,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 63</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_63 = 2580,
        ///<summary>Pulkovo 1942 3degree GaussKruger zone 64</summary>
        Pulkovo_1942_3degree_GaussKruger_zone_64 = 2581,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 21E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_21E = 2582,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 24E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_24E = 2583,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 27E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_27E = 2584,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 48E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_48E = 2591,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 51E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_51E = 2592,
        ///<summary>Lietuvos Koordinoei Sistema 1994 deprecated</summary>
        Lietuvos_Koordinoei_Sistema_1994_deprecated = 2600,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 75E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_75E = 2601,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 78E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_78E = 2602,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 81E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_81E = 2603,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 84E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_84E = 2604,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 87E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_87E = 2605,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 90E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_90E = 2606,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 93E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_93E = 2607,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 96E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_96E = 2608,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 159E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_159E = 2629,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 162E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_162E = 2630,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 165E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_165E = 2631,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 168E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_168E = 2632,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 171E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_171E = 2633,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 174E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_174E = 2634,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 177E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_177E = 2635,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 180E</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_180E = 2636,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 177W</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_177W = 2637,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 174W</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_174W = 2638,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 171W</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_171W = 2639,
        ///<summary>Pulkovo 1942 3degree GaussKruger CM 168W</summary>
        Pulkovo_1942_3degree_GaussKruger_CM_168W = 2640,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 14</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_14 = 2648,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 15</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_15 = 2649,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 16</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_16 = 2650,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 17</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_17 = 2651,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 18</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_18 = 2652,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 27</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_27 = 2661,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 28</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_28 = 2662,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 29</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_29 = 2663,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 30</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_30 = 2664,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 31</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_31 = 2665,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 37</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_37 = 2671,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 38</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_38 = 2672,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 41</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_41 = 2675,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 42</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_42 = 2676,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 43</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_43 = 2677,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 44</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_44 = 2678,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 47</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_47 = 2681,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 48</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_48 = 2682,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 49</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_49 = 2683,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 50</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_50 = 2684,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 54</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_54 = 2688,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 55</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_55 = 2689,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 56</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_56 = 2690,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 57</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_57 = 2691,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 58</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_58 = 2692,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 59</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_59 = 2693,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 62</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_62 = 2696,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 63</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_63 = 2697,
        ///<summary>Pulkovo 1995 3degree GaussKruger zone 64</summary>
        Pulkovo_1995_3degree_GaussKruger_zone_64 = 2698,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 24E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_24E = 2700,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 36E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_36E = 2704,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 45E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_45E = 2707,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 48E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_48E = 2708,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 51E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_51E = 2709,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 54E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_54E = 2710,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 57E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_57E = 2711,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 84E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_84E = 2720,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 87E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_87E = 2721,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 90E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_90E = 2722,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 93E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_93E = 2723,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 102E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_102E = 2726,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 105E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_105E = 2727,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 114E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_114E = 2730,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 123E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_123E = 2733,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 126E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_126E = 2734,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 129E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_129E = 2735,
        ///<summary>Tete UTM zone 36S</summary>
        Tete_UTM_zone_36S = 2736,
        ///<summary>Tete UTM zone 37S</summary>
        Tete_UTM_zone_37S = 2737,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 144E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_144E = 2742,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 147E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_147E = 2743,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 150E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_150E = 2744,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 153E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_153E = 2745,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 168E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_168E = 2750,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 171E</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_171E = 2751,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 177W</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_177W = 2755,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 174W</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_174W = 2756,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 171W</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_171W = 2757,
        ///<summary>Pulkovo 1995 3degree GaussKruger CM 168W</summary>
        Pulkovo_1995_3degree_GaussKruger_CM_168W = 2758,
        ///<summary>NAD83HARN Alabama East</summary>
        NAD83HARN_Alabama_East = 2759,
        ///<summary>NAD83HARN Alabama West</summary>
        NAD83HARN_Alabama_West = 2760,
        ///<summary>NAD83HARN Arizona East</summary>
        NAD83HARN_Arizona_East = 2761,
        ///<summary>NAD83HARN Arizona Central</summary>
        NAD83HARN_Arizona_Central = 2762,
        ///<summary>NAD83HARN Arizona West</summary>
        NAD83HARN_Arizona_West = 2763,
        ///<summary>NAD83HARN California zone 1</summary>
        NAD83HARN_California_zone_1 = 2766,
        ///<summary>NAD83HARN California zone 2</summary>
        NAD83HARN_California_zone_2 = 2767,
        ///<summary>NAD83HARN California zone 3</summary>
        NAD83HARN_California_zone_3 = 2768,
        ///<summary>NAD83HARN California zone 4</summary>
        NAD83HARN_California_zone_4 = 2769,
        ///<summary>NAD83HARN California zone 5</summary>
        NAD83HARN_California_zone_5 = 2770,
        ///<summary>NAD83HARN California zone 6</summary>
        NAD83HARN_California_zone_6 = 2771,
        ///<summary>NAD83HARN Colorado North</summary>
        NAD83HARN_Colorado_North = 2772,
        ///<summary>NAD83HARN Colorado Central</summary>
        NAD83HARN_Colorado_Central = 2773,
        ///<summary>NAD83HARN Colorado South</summary>
        NAD83HARN_Colorado_South = 2774,
        ///<summary>NAD83HARN Connecticut</summary>
        NAD83HARN_Connecticut = 2775,
        ///<summary>NAD83HARN Delaware</summary>
        NAD83HARN_Delaware = 2776,
        ///<summary>NAD83HARN Florida East</summary>
        NAD83HARN_Florida_East = 2777,
        ///<summary>NAD83HARN Florida West</summary>
        NAD83HARN_Florida_West = 2778,
        ///<summary>NAD83HARN Florida North</summary>
        NAD83HARN_Florida_North = 2779,
        ///<summary>NAD83HARN Georgia East</summary>
        NAD83HARN_Georgia_East = 2780,
        ///<summary>NAD83HARN Georgia West</summary>
        NAD83HARN_Georgia_West = 2781,
        ///<summary>NAD83HARN Hawaii zone 1</summary>
        NAD83HARN_Hawaii_zone_1 = 2782,
        ///<summary>NAD83HARN Hawaii zone 2</summary>
        NAD83HARN_Hawaii_zone_2 = 2783,
        ///<summary>NAD83HARN Hawaii zone 3</summary>
        NAD83HARN_Hawaii_zone_3 = 2784,
        ///<summary>NAD83HARN Hawaii zone 4</summary>
        NAD83HARN_Hawaii_zone_4 = 2785,
        ///<summary>NAD83HARN Hawaii zone 5</summary>
        NAD83HARN_Hawaii_zone_5 = 2786,
        ///<summary>NAD83HARN Idaho East</summary>
        NAD83HARN_Idaho_East = 2787,
        ///<summary>NAD83HARN Idaho Central</summary>
        NAD83HARN_Idaho_Central = 2788,
        ///<summary>NAD83HARN Idaho West</summary>
        NAD83HARN_Idaho_West = 2789,
        ///<summary>NAD83HARN Illinois East</summary>
        NAD83HARN_Illinois_East = 2790,
        ///<summary>NAD83HARN Illinois West</summary>
        NAD83HARN_Illinois_West = 2791,
        ///<summary>NAD83HARN Indiana East</summary>
        NAD83HARN_Indiana_East = 2792,
        ///<summary>NAD83HARN Indiana West</summary>
        NAD83HARN_Indiana_West = 2793,
        ///<summary>NAD83HARN Iowa North</summary>
        NAD83HARN_Iowa_North = 2794,
        ///<summary>NAD83HARN Iowa South</summary>
        NAD83HARN_Iowa_South = 2795,
        ///<summary>NAD83HARN Kansas North</summary>
        NAD83HARN_Kansas_North = 2796,
        ///<summary>NAD83HARN Kansas South</summary>
        NAD83HARN_Kansas_South = 2797,
        ///<summary>NAD83HARN Kentucky North</summary>
        NAD83HARN_Kentucky_North = 2798,
        ///<summary>NAD83HARN Kentucky South</summary>
        NAD83HARN_Kentucky_South = 2799,
        ///<summary>NAD83HARN Louisiana North</summary>
        NAD83HARN_Louisiana_North = 2800,
        ///<summary>NAD83HARN Louisiana South</summary>
        NAD83HARN_Louisiana_South = 2801,
        ///<summary>NAD83HARN Maine East</summary>
        NAD83HARN_Maine_East = 2802,
        ///<summary>NAD83HARN Maine West</summary>
        NAD83HARN_Maine_West = 2803,
        ///<summary>NAD83HARN Maryland</summary>
        NAD83HARN_Maryland = 2804,
        ///<summary>NAD83HARN Massachusetts Mainland</summary>
        NAD83HARN_Massachusetts_Mainland = 2805,
        ///<summary>NAD83HARN Massachusetts Island</summary>
        NAD83HARN_Massachusetts_Island = 2806,
        ///<summary>NAD83HARN Michigan North</summary>
        NAD83HARN_Michigan_North = 2807,
        ///<summary>NAD83HARN Michigan Central</summary>
        NAD83HARN_Michigan_Central = 2808,
        ///<summary>NAD83HARN Michigan South</summary>
        NAD83HARN_Michigan_South = 2809,
        ///<summary>NAD83HARN Minnesota North</summary>
        NAD83HARN_Minnesota_North = 2810,
        ///<summary>NAD83HARN Minnesota Central</summary>
        NAD83HARN_Minnesota_Central = 2811,
        ///<summary>NAD83HARN Minnesota South</summary>
        NAD83HARN_Minnesota_South = 2812,
        ///<summary>NAD83HARN Mississippi East</summary>
        NAD83HARN_Mississippi_East = 2813,
        ///<summary>NAD83HARN Mississippi West</summary>
        NAD83HARN_Mississippi_West = 2814,
        ///<summary>NAD83HARN Missouri East</summary>
        NAD83HARN_Missouri_East = 2815,
        ///<summary>NAD83HARN Missouri Central</summary>
        NAD83HARN_Missouri_Central = 2816,
        ///<summary>NAD83HARN Missouri West</summary>
        NAD83HARN_Missouri_West = 2817,
        ///<summary>NAD83HARN Montana</summary>
        NAD83HARN_Montana = 2818,
        ///<summary>NAD83HARN Nebraska</summary>
        NAD83HARN_Nebraska = 2819,
        ///<summary>NAD83HARN Nevada East</summary>
        NAD83HARN_Nevada_East = 2820,
        ///<summary>NAD83HARN Nevada Central</summary>
        NAD83HARN_Nevada_Central = 2821,
        ///<summary>NAD83HARN Nevada West</summary>
        NAD83HARN_Nevada_West = 2822,
        ///<summary>NAD83HARN New Hampshire</summary>
        NAD83HARN_New_Hampshire = 2823,
        ///<summary>NAD83HARN New Jersey</summary>
        NAD83HARN_New_Jersey = 2824,
        ///<summary>NAD83HARN New Mexico East</summary>
        NAD83HARN_New_Mexico_East = 2825,
        ///<summary>NAD83HARN New Mexico Central</summary>
        NAD83HARN_New_Mexico_Central = 2826,
        ///<summary>NAD83HARN New Mexico West</summary>
        NAD83HARN_New_Mexico_West = 2827,
        ///<summary>NAD83HARN New York East</summary>
        NAD83HARN_New_York_East = 2828,
        ///<summary>NAD83HARN New York Central</summary>
        NAD83HARN_New_York_Central = 2829,
        ///<summary>NAD83HARN New York West</summary>
        NAD83HARN_New_York_West = 2830,
        ///<summary>NAD83HARN New York Long Island</summary>
        NAD83HARN_New_York_Long_Island = 2831,
        ///<summary>NAD83HARN North Dakota North</summary>
        NAD83HARN_North_Dakota_North = 2832,
        ///<summary>NAD83HARN North Dakota South</summary>
        NAD83HARN_North_Dakota_South = 2833,
        ///<summary>NAD83HARN Ohio North</summary>
        NAD83HARN_Ohio_North = 2834,
        ///<summary>NAD83HARN Ohio South</summary>
        NAD83HARN_Ohio_South = 2835,
        ///<summary>NAD83HARN Oklahoma North</summary>
        NAD83HARN_Oklahoma_North = 2836,
        ///<summary>NAD83HARN Oklahoma South</summary>
        NAD83HARN_Oklahoma_South = 2837,
        ///<summary>NAD83HARN Oregon North</summary>
        NAD83HARN_Oregon_North = 2838,
        ///<summary>NAD83HARN Oregon South</summary>
        NAD83HARN_Oregon_South = 2839,
        ///<summary>NAD83HARN Rhode Island</summary>
        NAD83HARN_Rhode_Island = 2840,
        ///<summary>NAD83HARN South Dakota North</summary>
        NAD83HARN_South_Dakota_North = 2841,
        ///<summary>NAD83HARN South Dakota South</summary>
        NAD83HARN_South_Dakota_South = 2842,
        ///<summary>NAD83HARN Tennessee</summary>
        NAD83HARN_Tennessee = 2843,
        ///<summary>NAD83HARN Texas North</summary>
        NAD83HARN_Texas_North = 2844,
        ///<summary>NAD83HARN Texas North Central</summary>
        NAD83HARN_Texas_North_Central = 2845,
        ///<summary>NAD83HARN Texas Central</summary>
        NAD83HARN_Texas_Central = 2846,
        ///<summary>NAD83HARN Texas South Central</summary>
        NAD83HARN_Texas_South_Central = 2847,
        ///<summary>NAD83HARN Texas South</summary>
        NAD83HARN_Texas_South = 2848,
        ///<summary>NAD83HARN Utah North</summary>
        NAD83HARN_Utah_North = 2849,
        ///<summary>NAD83HARN Utah Central</summary>
        NAD83HARN_Utah_Central = 2850,
        ///<summary>NAD83HARN Utah South</summary>
        NAD83HARN_Utah_South = 2851,
        ///<summary>NAD83HARN Vermont</summary>
        NAD83HARN_Vermont = 2852,
        ///<summary>NAD83HARN Virginia North</summary>
        NAD83HARN_Virginia_North = 2853,
        ///<summary>NAD83HARN Virginia South</summary>
        NAD83HARN_Virginia_South = 2854,
        ///<summary>NAD83HARN Washington North</summary>
        NAD83HARN_Washington_North = 2855,
        ///<summary>NAD83HARN Washington South</summary>
        NAD83HARN_Washington_South = 2856,
        ///<summary>NAD83HARN West Virginia North</summary>
        NAD83HARN_West_Virginia_North = 2857,
        ///<summary>NAD83HARN West Virginia South</summary>
        NAD83HARN_West_Virginia_South = 2858,
        ///<summary>NAD83HARN Wisconsin North</summary>
        NAD83HARN_Wisconsin_North = 2859,
        ///<summary>NAD83HARN Wisconsin Central</summary>
        NAD83HARN_Wisconsin_Central = 2860,
        ///<summary>NAD83HARN Wisconsin South</summary>
        NAD83HARN_Wisconsin_South = 2861,
        ///<summary>NAD83HARN Wyoming East</summary>
        NAD83HARN_Wyoming_East = 2862,
        ///<summary>NAD83HARN Wyoming East Central</summary>
        NAD83HARN_Wyoming_East_Central = 2863,
        ///<summary>NAD83HARN Wyoming West Central</summary>
        NAD83HARN_Wyoming_West_Central = 2864,
        ///<summary>NAD83HARN Wyoming West</summary>
        NAD83HARN_Wyoming_West = 2865,
        ///<summary>NAD83HARN Puerto Rico Virgin Is</summary>
        NAD83HARN_Puerto_Rico_Virgin_Is = 2866,
        ///<summary>NAD83HARN Arizona East ft</summary>
        NAD83HARN_Arizona_East_ft = 2867,
        ///<summary>NAD83HARN Arizona Central ft</summary>
        NAD83HARN_Arizona_Central_ft = 2868,
        ///<summary>NAD83HARN Arizona West ft</summary>
        NAD83HARN_Arizona_West_ft = 2869,
        ///<summary>NAD83HARN California zone 1 ftUS</summary>
        NAD83HARN_California_zone_1_ftUS = 2870,
        ///<summary>NAD83HARN California zone 2 ftUS</summary>
        NAD83HARN_California_zone_2_ftUS = 2871,
        ///<summary>NAD83HARN California zone 3 ftUS</summary>
        NAD83HARN_California_zone_3_ftUS = 2872,
        ///<summary>NAD83HARN California zone 4 ftUS</summary>
        NAD83HARN_California_zone_4_ftUS = 2873,
        ///<summary>NAD83HARN California zone 5 ftUS</summary>
        NAD83HARN_California_zone_5_ftUS = 2874,
        ///<summary>NAD83HARN California zone 6 ftUS</summary>
        NAD83HARN_California_zone_6_ftUS = 2875,
        ///<summary>NAD83HARN Colorado North ftUS</summary>
        NAD83HARN_Colorado_North_ftUS = 2876,
        ///<summary>NAD83HARN Colorado Central ftUS</summary>
        NAD83HARN_Colorado_Central_ftUS = 2877,
        ///<summary>NAD83HARN Colorado South ftUS</summary>
        NAD83HARN_Colorado_South_ftUS = 2878,
        ///<summary>NAD83HARN Connecticut ftUS</summary>
        NAD83HARN_Connecticut_ftUS = 2879,
        ///<summary>NAD83HARN Delaware ftUS</summary>
        NAD83HARN_Delaware_ftUS = 2880,
        ///<summary>NAD83HARN Florida East ftUS</summary>
        NAD83HARN_Florida_East_ftUS = 2881,
        ///<summary>NAD83HARN Florida West ftUS</summary>
        NAD83HARN_Florida_West_ftUS = 2882,
        ///<summary>NAD83HARN Florida North ftUS</summary>
        NAD83HARN_Florida_North_ftUS = 2883,
        ///<summary>NAD83HARN Georgia East ftUS</summary>
        NAD83HARN_Georgia_East_ftUS = 2884,
        ///<summary>NAD83HARN Georgia West ftUS</summary>
        NAD83HARN_Georgia_West_ftUS = 2885,
        ///<summary>NAD83HARN Idaho East ftUS</summary>
        NAD83HARN_Idaho_East_ftUS = 2886,
        ///<summary>NAD83HARN Idaho Central ftUS</summary>
        NAD83HARN_Idaho_Central_ftUS = 2887,
        ///<summary>NAD83HARN Idaho West ftUS</summary>
        NAD83HARN_Idaho_West_ftUS = 2888,
        ///<summary>NAD83HARN Kentucky North ftUS</summary>
        NAD83HARN_Kentucky_North_ftUS = 2891,
        ///<summary>NAD83HARN Kentucky South ftUS</summary>
        NAD83HARN_Kentucky_South_ftUS = 2892,
        ///<summary>NAD83HARN Maryland ftUS</summary>
        NAD83HARN_Maryland_ftUS = 2893,
        ///<summary>NAD83HARN Massachusetts Mainland ftUS</summary>
        NAD83HARN_Massachusetts_Mainland_ftUS = 2894,
        ///<summary>NAD83HARN Massachusetts Island ftUS</summary>
        NAD83HARN_Massachusetts_Island_ftUS = 2895,
        ///<summary>NAD83HARN Michigan North ft</summary>
        NAD83HARN_Michigan_North_ft = 2896,
        ///<summary>NAD83HARN Michigan Central ft</summary>
        NAD83HARN_Michigan_Central_ft = 2897,
        ///<summary>NAD83HARN Michigan South ft</summary>
        NAD83HARN_Michigan_South_ft = 2898,
        ///<summary>NAD83HARN Mississippi East ftUS</summary>
        NAD83HARN_Mississippi_East_ftUS = 2899,
        ///<summary>NAD83HARN Mississippi West ftUS</summary>
        NAD83HARN_Mississippi_West_ftUS = 2900,
        ///<summary>NAD83HARN Montana ft</summary>
        NAD83HARN_Montana_ft = 2901,
        ///<summary>NAD83HARN New Mexico East ftUS</summary>
        NAD83HARN_New_Mexico_East_ftUS = 2902,
        ///<summary>NAD83HARN New Mexico Central ftUS</summary>
        NAD83HARN_New_Mexico_Central_ftUS = 2903,
        ///<summary>NAD83HARN New Mexico West ftUS</summary>
        NAD83HARN_New_Mexico_West_ftUS = 2904,
        ///<summary>NAD83HARN New York East ftUS</summary>
        NAD83HARN_New_York_East_ftUS = 2905,
        ///<summary>NAD83HARN New York Central ftUS</summary>
        NAD83HARN_New_York_Central_ftUS = 2906,
        ///<summary>NAD83HARN New York West ftUS</summary>
        NAD83HARN_New_York_West_ftUS = 2907,
        ///<summary>NAD83HARN New York Long Island ftUS</summary>
        NAD83HARN_New_York_Long_Island_ftUS = 2908,
        ///<summary>NAD83HARN North Dakota North ft</summary>
        NAD83HARN_North_Dakota_North_ft = 2909,
        ///<summary>NAD83HARN North Dakota South ft</summary>
        NAD83HARN_North_Dakota_South_ft = 2910,
        ///<summary>NAD83HARN Oklahoma North ftUS</summary>
        NAD83HARN_Oklahoma_North_ftUS = 2911,
        ///<summary>NAD83HARN Oklahoma South ftUS</summary>
        NAD83HARN_Oklahoma_South_ftUS = 2912,
        ///<summary>NAD83HARN Oregon North ft</summary>
        NAD83HARN_Oregon_North_ft = 2913,
        ///<summary>NAD83HARN Oregon South ft</summary>
        NAD83HARN_Oregon_South_ft = 2914,
        ///<summary>NAD83HARN Tennessee ftUS</summary>
        NAD83HARN_Tennessee_ftUS = 2915,
        ///<summary>NAD83HARN Texas North ftUS</summary>
        NAD83HARN_Texas_North_ftUS = 2916,
        ///<summary>NAD83HARN Texas North Central ftUS</summary>
        NAD83HARN_Texas_North_Central_ftUS = 2917,
        ///<summary>NAD83HARN Texas Central ftUS</summary>
        NAD83HARN_Texas_Central_ftUS = 2918,
        ///<summary>NAD83HARN Texas South Central ftUS</summary>
        NAD83HARN_Texas_South_Central_ftUS = 2919,
        ///<summary>NAD83HARN Texas South ftUS</summary>
        NAD83HARN_Texas_South_ftUS = 2920,
        ///<summary>NAD83HARN Utah North ft</summary>
        NAD83HARN_Utah_North_ft = 2921,
        ///<summary>NAD83HARN Utah Central ft</summary>
        NAD83HARN_Utah_Central_ft = 2922,
        ///<summary>NAD83HARN Utah South ft</summary>
        NAD83HARN_Utah_South_ft = 2923,
        ///<summary>NAD83HARN Virginia North ftUS</summary>
        NAD83HARN_Virginia_North_ftUS = 2924,
        ///<summary>NAD83HARN Virginia South ftUS</summary>
        NAD83HARN_Virginia_South_ftUS = 2925,
        ///<summary>NAD83HARN Washington North ftUS</summary>
        NAD83HARN_Washington_North_ftUS = 2926,
        ///<summary>NAD83HARN Washington South ftUS</summary>
        NAD83HARN_Washington_South_ftUS = 2927,
        ///<summary>NAD83HARN Wisconsin North ftUS</summary>
        NAD83HARN_Wisconsin_North_ftUS = 2928,
        ///<summary>NAD83HARN Wisconsin Central ftUS</summary>
        NAD83HARN_Wisconsin_Central_ftUS = 2929,
        ///<summary>NAD83HARN Wisconsin South ftUS</summary>
        NAD83HARN_Wisconsin_South_ftUS = 2930,
        ///<summary>Porto Santo UTM zone 28N</summary>
        Porto_Santo_UTM_zone_28N = 2942,
        ///<summary>NAD27 Alaska Albers</summary>
        NAD27_Alaska_Albers = 2964,
        ///<summary>NAD83 Indiana East ftUS</summary>
        NAD83_Indiana_East_ftUS = 2965,
        ///<summary>NAD83 Indiana West ftUS</summary>
        NAD83_Indiana_West_ftUS = 2966,
        ///<summary>NAD83HARN Indiana East ftUS</summary>
        NAD83HARN_Indiana_East_ftUS = 2967,
        ///<summary>NAD83HARN Indiana West ftUS</summary>
        NAD83HARN_Indiana_West_ftUS = 2968,
        ///<summary>Fort Marigot UTM zone 20N</summary>
        Fort_Marigot_UTM_zone_20N = 2969,
        ///<summary>Guadeloupe 1948 UTM zone 20N</summary>
        Guadeloupe_1948_UTM_zone_20N = 2970,
        ///<summary>CSG67 UTM zone 22N</summary>
        CSG67_UTM_zone_22N = 2971,
        ///<summary>RGFG95 UTM zone 22N</summary>
        RGFG95_UTM_zone_22N = 2972,
        ///<summary>Martinique 1938 UTM zone 20N</summary>
        Martinique_1938_UTM_zone_20N = 2973,
        ///<summary>RGR92 UTM zone 40S</summary>
        RGR92_UTM_zone_40S = 2975,
        ///<summary>Tahiti 52 UTM zone 6S</summary>
        Tahiti_52_UTM_zone_6S = 2976,
        ///<summary>Tahaa 54 UTM zone 5S</summary>
        Tahaa_54_UTM_zone_5S = 2977,
        ///<summary>IGN72 Nuku Hiva UTM zone 7S</summary>
        IGN72_Nuku_Hiva_UTM_zone_7S = 2978,
        ///<summary>K0 1949 UTM zone 42S deprecated</summary>
        K0_1949_UTM_zone_42S_deprecated = 2979,
        ///<summary>Combani 1950 UTM zone 38S</summary>
        Combani_1950_UTM_zone_38S = 2980,
        ///<summary>IGN56 Lifou UTM zone 58S</summary>
        IGN56_Lifou_UTM_zone_58S = 2981,
        ///<summary>IGN72 Grand Terre UTM zone 58S deprecated</summary>
        IGN72_Grand_Terre_UTM_zone_58S_deprecated = 2982,
        ///<summary>ST87 Ouvea UTM zone 58S deprecated</summary>
        ST87_Ouvea_UTM_zone_58S_deprecated = 2983,
        ///<summary>RGNC 1991 Lambert New Caledonia deprecated</summary>
        RGNC_1991_Lambert_New_Caledonia_deprecated = 2984,
        ///<summary>Petrels 1972 Terre Adelie Polar Stereographic</summary>
        Petrels_1972_Terre_Adelie_Polar_Stereographic = 2985,
        ///<summary>Perroud 1950 Terre Adelie Polar Stereographic</summary>
        Perroud_1950_Terre_Adelie_Polar_Stereographic = 2986,
        ///<summary>Saint Pierre et Miquelon 1950 UTM zone 21N</summary>
        Saint_Pierre_et_Miquelon_1950_UTM_zone_21N = 2987,
        ///<summary>MOP78 UTM zone 1S</summary>
        MOP78_UTM_zone_1S = 2988,
        ///<summary>RRAF 1991 UTM zone 20N</summary>
        RRAF_1991_UTM_zone_20N = 2989,
        ///<summary>Reunion 1947 TM Reunion deprecated</summary>
        Reunion_1947_TM_Reunion_deprecated = 2990,
        ///<summary>NAD83 Oregon Lambert</summary>
        NAD83_Oregon_Lambert = 2991,
        ///<summary>NAD83 Oregon Lambert ft</summary>
        NAD83_Oregon_Lambert_ft = 2992,
        ///<summary>NAD83HARN Oregon Lambert</summary>
        NAD83HARN_Oregon_Lambert = 2993,
        ///<summary>NAD83HARN Oregon Lambert ft</summary>
        NAD83HARN_Oregon_Lambert_ft = 2994,
        ///<summary>IGN53 Mare UTM zone 58S</summary>
        IGN53_Mare_UTM_zone_58S = 2995,
        ///<summary>ST84 Ile des Pins UTM zone 58S</summary>
        ST84_Ile_des_Pins_UTM_zone_58S = 2996,
        ///<summary>ST71 Belep UTM zone 58S</summary>
        ST71_Belep_UTM_zone_58S = 2997,
        ///<summary>NEA74 Noumea UTM zone 58S</summary>
        NEA74_Noumea_UTM_zone_58S = 2998,
        ///<summary>Grand Comoros UTM zone 38S</summary>
        Grand_Comoros_UTM_zone_38S = 2999,
        ///<summary>Segara NEIEZ</summary>
        Segara_NEIEZ = 3000,
        ///<summary>Batavia NEIEZ</summary>
        Batavia_NEIEZ = 3001,
        ///<summary>Makassar NEIEZ</summary>
        Makassar_NEIEZ = 3002,
        ///<summary>Monte Mario Italy zone 1</summary>
        Monte_Mario_Italy_zone_1 = 3003,
        ///<summary>Monte Mario Italy zone 2</summary>
        Monte_Mario_Italy_zone_2 = 3004,
        ///<summary>NAD83 BC Albers</summary>
        NAD83_BC_Albers = 3005,
        ///<summary>SWEREF99 TM</summary>
        SWEREF99_TM = 3006,
        ///<summary>SWEREF99 12 00</summary>
        SWEREF99_12_00 = 3007,
        ///<summary>SWEREF99 13 30</summary>
        SWEREF99_13_30 = 3008,
        ///<summary>SWEREF99 15 00</summary>
        SWEREF99_15_00 = 3009,
        ///<summary>SWEREF99 16 30</summary>
        SWEREF99_16_30 = 3010,
        ///<summary>SWEREF99 18 00</summary>
        SWEREF99_18_00 = 3011,
        ///<summary>SWEREF99 14 15</summary>
        SWEREF99_14_15 = 3012,
        ///<summary>SWEREF99 15 45</summary>
        SWEREF99_15_45 = 3013,
        ///<summary>SWEREF99 17 15</summary>
        SWEREF99_17_15 = 3014,
        ///<summary>SWEREF99 18 45</summary>
        SWEREF99_18_45 = 3015,
        ///<summary>SWEREF99 20 15</summary>
        SWEREF99_20_15 = 3016,
        ///<summary>SWEREF99 21 45</summary>
        SWEREF99_21_45 = 3017,
        ///<summary>SWEREF99 23 15</summary>
        SWEREF99_23_15 = 3018,
        ///<summary>RT90 75 gon V</summary>
        RT90_75_gon_V = 3019,
        ///<summary>RT90 5 gon V</summary>
        RT90_5_gon_V = 3020,
        ///<summary>RT90 25 gon V</summary>
        RT90_25_gon_V = 3021,
        ///<summary>RT90 0 gon</summary>
        RT90_0_gon = 3022,
        ///<summary>RT90 25 gon O</summary>
        RT90_25_gon_O = 3023,
        ///<summary>RT90 5 gon O</summary>
        RT90_5_gon_O = 3024,
        ///<summary>RT38 75 gon V</summary>
        RT38_75_gon_V = 3025,
        ///<summary>RT38 5 gon V</summary>
        RT38_5_gon_V = 3026,
        ///<summary>RT38 25 gon V</summary>
        RT38_25_gon_V = 3027,
        ///<summary>RT38 0 gon</summary>
        RT38_0_gon = 3028,
        ///<summary>RT38 25 gon O</summary>
        RT38_25_gon_O = 3029,
        ///<summary>RT38 5 gon O</summary>
        RT38_5_gon_O = 3030,
        ///<summary>WGS 84 Antarctic Polar Stereographic</summary>
        WGS_84_Antarctic_Polar_Stereographic = 3031,
        ///<summary>WGS 84 Australian Antarctic Polar Stereographic</summary>
        WGS_84_Australian_Antarctic_Polar_Stereographic = 3032,
        ///<summary>WGS 84 Australian Antarctic Lambert</summary>
        WGS_84_Australian_Antarctic_Lambert = 3033,
        ///<summary>ETRS89 ETRSLCC</summary>
        ETRS89_ETRSLCC = 3034,
        ///<summary>ETRS89 ETRSLAEA</summary>
        ETRS89_ETRSLAEA = 3035,
        ///<summary>Moznet UTM zone 36S</summary>
        Moznet_UTM_zone_36S = 3036,
        ///<summary>Moznet UTM zone 37S</summary>
        Moznet_UTM_zone_37S = 3037,
        ///<summary>ETRS89 ETRSTM26</summary>
        ETRS89_ETRSTM26 = 3038,
        ///<summary>ETRS89 ETRSTM30</summary>
        ETRS89_ETRSTM30 = 3042,
        ///<summary>ETRS89 ETRSTM31</summary>
        ETRS89_ETRSTM31 = 3043,
        ///<summary>ETRS89 ETRSTM32</summary>
        ETRS89_ETRSTM32 = 3044,
        ///<summary>ETRS89 ETRSTM33</summary>
        ETRS89_ETRSTM33 = 3045,
        ///<summary>ETRS89 ETRSTM34</summary>
        ETRS89_ETRSTM34 = 3046,
        ///<summary>ETRS89 ETRSTM36</summary>
        ETRS89_ETRSTM36 = 3048,
        ///<summary>ETRS89 ETRSTM37</summary>
        ETRS89_ETRSTM37 = 3049,
        ///<summary>Hjorsey 1955 UTM zone 26N</summary>
        Hjorsey_1955_UTM_zone_26N = 3054,
        ///<summary>Hjorsey 1955 UTM zone 27N</summary>
        Hjorsey_1955_UTM_zone_27N = 3055,
        ///<summary>Hjorsey 1955 UTM zone 28N</summary>
        Hjorsey_1955_UTM_zone_28N = 3056,
        ///<summary>ISN93 Lambert 1993</summary>
        ISN93_Lambert_1993 = 3057,
        ///<summary>Helle 1954 Jan Mayen Grid</summary>
        Helle_1954_Jan_Mayen_Grid = 3058,
        ///<summary>LKS92 Latvia TM</summary>
        LKS92_Latvia_TM = 3059,
        ///<summary>Porto Santo 1995 UTM zone 28N</summary>
        Porto_Santo_1995_UTM_zone_28N = 3061,
        ///<summary>Azores Oriental 1995 UTM zone 26N</summary>
        Azores_Oriental_1995_UTM_zone_26N = 3062,
        ///<summary>Azores Central 1995 UTM zone 26N</summary>
        Azores_Central_1995_UTM_zone_26N = 3063,
        ///<summary>IGM95 UTM zone 32N</summary>
        IGM95_UTM_zone_32N = 3064,
        ///<summary>IGM95 UTM zone 33N</summary>
        IGM95_UTM_zone_33N = 3065,
        ///<summary>NAD83HARN Kentucky Single Zone</summary>
        NAD83HARN_Kentucky_Single_Zone = 3090,
        ///<summary>Tokyo UTM zone 54N</summary>
        Tokyo_UTM_zone_54N = 3095,
        ///<summary>JGD2000 UTM zone 55N</summary>
        JGD2000_UTM_zone_55N = 3101,
        ///<summary>American Samoa 1962 American Samoa Lambert</summary>
        American_Samoa_1962_American_Samoa_Lambert = 3102,
        ///<summary>Mauritania 1999 UTM zone 28N deprecated</summary>
        Mauritania_1999_UTM_zone_28N_deprecated = 3103,
        ///<summary>Mauritania 1999 UTM zone 29N deprecated</summary>
        Mauritania_1999_UTM_zone_29N_deprecated = 3104,
        ///<summary>Indian 1960 UTM zone 48N</summary>
        Indian_1960_UTM_zone_48N = 3148,
        ///<summary>Indian 1960 UTM zone 49N</summary>
        Indian_1960_UTM_zone_49N = 3149,
        ///<summary>Indian 1960 TM 106 NE</summary>
        Indian_1960_TM_106_NE = 3176,
        ///<summary>FD58 Iraq zone</summary>
        FD58_Iraq_zone = 3200,
        ///<summary>WGS 84 SCAR IMW SR5556</summary>
        WGS_84_SCAR_IMW_SR5556 = 3239,
        ///<summary>WGS 84 SCAR IMW SR5758</summary>
        WGS_84_SCAR_IMW_SR5758 = 3240,
        ///<summary>Estonian Coordinate System of 1992</summary>
        Estonian_Coordinate_System_of_1992 = 3300,
        ///<summary>Estonian Coordinate System of 1997</summary>
        Estonian_Coordinate_System_of_1997 = 3301,
        ///<summary>IGN63 Hiva Oa UTM zone 7S</summary>
        IGN63_Hiva_Oa_UTM_zone_7S = 3302,
        ///<summary>NAD83 Arkansas North ftUS</summary>
        NAD83_Arkansas_North_ftUS = 3433,
        ///<summary>PSD93 UTM zone 39N</summary>
        PSD93_UTM_zone_39N = 3439,
        ///<summary>PSD93 UTM zone 40N</summary>
        PSD93_UTM_zone_40N = 3440,
        ///<summary>Old Hawaiian Hawaii zone 1</summary>
        Old_Hawaiian_Hawaii_zone_1 = 3561,
        ///<summary>Old Hawaiian Hawaii zone 2</summary>
        Old_Hawaiian_Hawaii_zone_2 = 3562,
        ///<summary>Old Hawaiian Hawaii zone 3</summary>
        Old_Hawaiian_Hawaii_zone_3 = 3563,
        ///<summary>Old Hawaiian Hawaii zone 4</summary>
        Old_Hawaiian_Hawaii_zone_4 = 3564,
        ///<summary>Old Hawaiian Hawaii zone 5</summary>
        Old_Hawaiian_Hawaii_zone_5 = 3565,
        ///<summary>NAD83NSRS2007 Wisconsin South ftUS</summary>
        NAD83NSRS2007_Wisconsin_South_ftUS = 3700,
        ///<summary>SWEREF99 RT90 5 gon V emulation</summary>
        SWEREF99_RT90_5_gon_V_emulation = 3846,
        ///<summary>SWEREF99 RT90 25 gon V emulation</summary>
        SWEREF99_RT90_25_gon_V_emulation = 3847,
        ///<summary>SWEREF99 RT90 0 gon emulation</summary>
        SWEREF99_RT90_0_gon_emulation = 3848,
        ///<summary>SWEREF99 RT90 25 gon O emulation</summary>
        SWEREF99_RT90_25_gon_O_emulation = 3849,
        ///<summary>SWEREF99 RT90 5 gon O emulation</summary>
        SWEREF99_RT90_5_gon_O_emulation = 3850,
        ///<summary>Puerto Rico UTM zone 20N</summary>
        Puerto_Rico_UTM_zone_20N = 3920,
        ///<summary>RGF93 CC46</summary>
        RGF93_CC46 = 3946,
        ///<summary>RGF93 CC47</summary>
        RGF93_CC47 = 3947,
        ///<summary>RGF93 CC48</summary>
        RGF93_CC48 = 3948,
        ///<summary>Puerto Rico State Plane CS of 1927</summary>
        Puerto_Rico_State_Plane_CS_of_1927 = 3991,
        ///<summary>Puerto Rico St Croix</summary>
        Puerto_Rico_St_Croix = 3992,
        ///<summary>Pulkovo 1995 GaussKruger zone 4</summary>
        Pulkovo_1995_GaussKruger_zone_4 = 20004,
        ///<summary>Pulkovo 1995 GaussKruger zone 5</summary>
        Pulkovo_1995_GaussKruger_zone_5 = 20005,
        ///<summary>Pulkovo 1995 GaussKruger zone 6</summary>
        Pulkovo_1995_GaussKruger_zone_6 = 20006,
        ///<summary>Pulkovo 1995 GaussKruger zone 7</summary>
        Pulkovo_1995_GaussKruger_zone_7 = 20007,
        ///<summary>Pulkovo 1995 GaussKruger zone 8</summary>
        Pulkovo_1995_GaussKruger_zone_8 = 20008,
        ///<summary>Pulkovo 1995 GaussKruger zone 9</summary>
        Pulkovo_1995_GaussKruger_zone_9 = 20009,
        ///<summary>Pulkovo 1995 GaussKruger zone 10</summary>
        Pulkovo_1995_GaussKruger_zone_10 = 20010,
        ///<summary>Pulkovo 1995 GaussKruger zone 11</summary>
        Pulkovo_1995_GaussKruger_zone_11 = 20011,
        ///<summary>Pulkovo 1995 GaussKruger zone 12</summary>
        Pulkovo_1995_GaussKruger_zone_12 = 20012,
        ///<summary>Pulkovo 1995 GaussKruger zone 13</summary>
        Pulkovo_1995_GaussKruger_zone_13 = 20013,
        ///<summary>Pulkovo 1995 GaussKruger zone 14</summary>
        Pulkovo_1995_GaussKruger_zone_14 = 20014,
        ///<summary>Pulkovo 1995 GaussKruger zone 15</summary>
        Pulkovo_1995_GaussKruger_zone_15 = 20015,
        ///<summary>Pulkovo 1995 GaussKruger zone 16</summary>
        Pulkovo_1995_GaussKruger_zone_16 = 20016,
        ///<summary>Pulkovo 1995 GaussKruger zone 17</summary>
        Pulkovo_1995_GaussKruger_zone_17 = 20017,
        ///<summary>Pulkovo 1995 GaussKruger zone 18</summary>
        Pulkovo_1995_GaussKruger_zone_18 = 20018,
        ///<summary>Pulkovo 1995 GaussKruger zone 19</summary>
        Pulkovo_1995_GaussKruger_zone_19 = 20019,
        ///<summary>Pulkovo 1995 GaussKruger zone 20</summary>
        Pulkovo_1995_GaussKruger_zone_20 = 20020,
        ///<summary>Pulkovo 1995 GaussKruger zone 21</summary>
        Pulkovo_1995_GaussKruger_zone_21 = 20021,
        ///<summary>Pulkovo 1995 GaussKruger zone 22</summary>
        Pulkovo_1995_GaussKruger_zone_22 = 20022,
        ///<summary>Pulkovo 1995 GaussKruger zone 23</summary>
        Pulkovo_1995_GaussKruger_zone_23 = 20023,
        ///<summary>Pulkovo 1995 GaussKruger zone 24</summary>
        Pulkovo_1995_GaussKruger_zone_24 = 20024,
        ///<summary>Pulkovo 1995 GaussKruger zone 25</summary>
        Pulkovo_1995_GaussKruger_zone_25 = 20025,
        ///<summary>Pulkovo 1995 GaussKruger zone 26</summary>
        Pulkovo_1995_GaussKruger_zone_26 = 20026,
        ///<summary>Pulkovo 1995 GaussKruger zone 27</summary>
        Pulkovo_1995_GaussKruger_zone_27 = 20027,
        ///<summary>Pulkovo 1995 GaussKruger zone 28</summary>
        Pulkovo_1995_GaussKruger_zone_28 = 20028,
        ///<summary>Pulkovo 1995 GaussKruger zone 29</summary>
        Pulkovo_1995_GaussKruger_zone_29 = 20029,
        ///<summary>Pulkovo 1995 GaussKruger zone 30</summary>
        Pulkovo_1995_GaussKruger_zone_30 = 20030,
        ///<summary>Pulkovo 1995 GaussKruger zone 31</summary>
        Pulkovo_1995_GaussKruger_zone_31 = 20031,
        ///<summary>Pulkovo 1995 GaussKruger zone 32</summary>
        Pulkovo_1995_GaussKruger_zone_32 = 20032,
        ///<summary>Pulkovo 1995 GaussKruger 4N deprecated</summary>
        Pulkovo_1995_GaussKruger_4N_deprecated = 20064,
        ///<summary>Pulkovo 1995 GaussKruger 5N deprecated</summary>
        Pulkovo_1995_GaussKruger_5N_deprecated = 20065,
        ///<summary>Pulkovo 1995 GaussKruger 6N deprecated</summary>
        Pulkovo_1995_GaussKruger_6N_deprecated = 20066,
        ///<summary>Pulkovo 1995 GaussKruger 7N deprecated</summary>
        Pulkovo_1995_GaussKruger_7N_deprecated = 20067,
        ///<summary>Pulkovo 1995 GaussKruger 8N deprecated</summary>
        Pulkovo_1995_GaussKruger_8N_deprecated = 20068,
        ///<summary>Pulkovo 1995 GaussKruger 9N deprecated</summary>
        Pulkovo_1995_GaussKruger_9N_deprecated = 20069,
        ///<summary>Pulkovo 1995 GaussKruger 10N deprecated</summary>
        Pulkovo_1995_GaussKruger_10N_deprecated = 20070,
        ///<summary>Pulkovo 1995 GaussKruger 11N deprecated</summary>
        Pulkovo_1995_GaussKruger_11N_deprecated = 20071,
        ///<summary>Pulkovo 1995 GaussKruger 12N deprecated</summary>
        Pulkovo_1995_GaussKruger_12N_deprecated = 20072,
        ///<summary>Pulkovo 1995 GaussKruger 13N deprecated</summary>
        Pulkovo_1995_GaussKruger_13N_deprecated = 20073,
        ///<summary>Pulkovo 1995 GaussKruger 14N deprecated</summary>
        Pulkovo_1995_GaussKruger_14N_deprecated = 20074,
        ///<summary>Pulkovo 1995 GaussKruger 15N deprecated</summary>
        Pulkovo_1995_GaussKruger_15N_deprecated = 20075,
        ///<summary>Pulkovo 1995 GaussKruger 16N deprecated</summary>
        Pulkovo_1995_GaussKruger_16N_deprecated = 20076,
        ///<summary>Pulkovo 1995 GaussKruger 17N deprecated</summary>
        Pulkovo_1995_GaussKruger_17N_deprecated = 20077,
        ///<summary>Pulkovo 1995 GaussKruger 18N deprecated</summary>
        Pulkovo_1995_GaussKruger_18N_deprecated = 20078,
        ///<summary>Pulkovo 1995 GaussKruger 19N deprecated</summary>
        Pulkovo_1995_GaussKruger_19N_deprecated = 20079,
        ///<summary>Pulkovo 1995 GaussKruger 20N deprecated</summary>
        Pulkovo_1995_GaussKruger_20N_deprecated = 20080,
        ///<summary>Pulkovo 1995 GaussKruger 21N deprecated</summary>
        Pulkovo_1995_GaussKruger_21N_deprecated = 20081,
        ///<summary>Pulkovo 1995 GaussKruger 22N deprecated</summary>
        Pulkovo_1995_GaussKruger_22N_deprecated = 20082,
        ///<summary>Pulkovo 1995 GaussKruger 23N deprecated</summary>
        Pulkovo_1995_GaussKruger_23N_deprecated = 20083,
        ///<summary>Pulkovo 1995 GaussKruger 24N deprecated</summary>
        Pulkovo_1995_GaussKruger_24N_deprecated = 20084,
        ///<summary>Pulkovo 1995 GaussKruger 25N deprecated</summary>
        Pulkovo_1995_GaussKruger_25N_deprecated = 20085,
        ///<summary>Pulkovo 1995 GaussKruger 26N deprecated</summary>
        Pulkovo_1995_GaussKruger_26N_deprecated = 20086,
        ///<summary>Pulkovo 1995 GaussKruger 27N deprecated</summary>
        Pulkovo_1995_GaussKruger_27N_deprecated = 20087,
        ///<summary>Pulkovo 1995 GaussKruger 28N deprecated</summary>
        Pulkovo_1995_GaussKruger_28N_deprecated = 20088,
        ///<summary>Pulkovo 1995 GaussKruger 29N deprecated</summary>
        Pulkovo_1995_GaussKruger_29N_deprecated = 20089,
        ///<summary>Pulkovo 1995 GaussKruger 30N deprecated</summary>
        Pulkovo_1995_GaussKruger_30N_deprecated = 20090,
        ///<summary>Pulkovo 1995 GaussKruger 31N deprecated</summary>
        Pulkovo_1995_GaussKruger_31N_deprecated = 20091,
        ///<summary>Pulkovo 1995 GaussKruger 32N deprecated</summary>
        Pulkovo_1995_GaussKruger_32N_deprecated = 20092,
        ///<summary>Adindan UTM zone 37N</summary>
        Adindan_UTM_zone_37N = 20137,
        ///<summary>Adindan UTM zone 38N</summary>
        Adindan_UTM_zone_38N = 20138,
        ///<summary>AGD66 AMG zone 48</summary>
        AGD66_AMG_zone_48 = 20248,
        ///<summary>AGD66 AMG zone 49</summary>
        AGD66_AMG_zone_49 = 20249,
        ///<summary>AGD66 AMG zone 50</summary>
        AGD66_AMG_zone_50 = 20250,
        ///<summary>AGD66 AMG zone 51</summary>
        AGD66_AMG_zone_51 = 20251,
        ///<summary>AGD66 AMG zone 52</summary>
        AGD66_AMG_zone_52 = 20252,
        ///<summary>AGD66 AMG zone 53</summary>
        AGD66_AMG_zone_53 = 20253,
        ///<summary>AGD66 AMG zone 54</summary>
        AGD66_AMG_zone_54 = 20254,
        ///<summary>AGD66 AMG zone 55</summary>
        AGD66_AMG_zone_55 = 20255,
        ///<summary>AGD66 AMG zone 56</summary>
        AGD66_AMG_zone_56 = 20256,
        ///<summary>AGD66 AMG zone 57</summary>
        AGD66_AMG_zone_57 = 20257,
        ///<summary>AGD66 AMG zone 58</summary>
        AGD66_AMG_zone_58 = 20258,
        ///<summary>AGD84 AMG zone 48</summary>
        AGD84_AMG_zone_48 = 20348,
        ///<summary>AGD84 AMG zone 49</summary>
        AGD84_AMG_zone_49 = 20349,
        ///<summary>AGD84 AMG zone 50</summary>
        AGD84_AMG_zone_50 = 20350,
        ///<summary>AGD84 AMG zone 51</summary>
        AGD84_AMG_zone_51 = 20351,
        ///<summary>AGD84 AMG zone 52</summary>
        AGD84_AMG_zone_52 = 20352,
        ///<summary>AGD84 AMG zone 53</summary>
        AGD84_AMG_zone_53 = 20353,
        ///<summary>AGD84 AMG zone 54</summary>
        AGD84_AMG_zone_54 = 20354,
        ///<summary>AGD84 AMG zone 55</summary>
        AGD84_AMG_zone_55 = 20355,
        ///<summary>AGD84 AMG zone 56</summary>
        AGD84_AMG_zone_56 = 20356,
        ///<summary>AGD84 AMG zone 57</summary>
        AGD84_AMG_zone_57 = 20357,
        ///<summary>AGD84 AMG zone 58</summary>
        AGD84_AMG_zone_58 = 20358,
        ///<summary>Ain el Abd UTM zone 37N</summary>
        Ain_el_Abd_UTM_zone_37N = 20437,
        ///<summary>Ain el Abd UTM zone 38N</summary>
        Ain_el_Abd_UTM_zone_38N = 20438,
        ///<summary>Ain el Abd UTM zone 39N</summary>
        Ain_el_Abd_UTM_zone_39N = 20439,
        ///<summary>Ain el Abd Bahrain Grid</summary>
        Ain_el_Abd_Bahrain_Grid = 20499,
        ///<summary>Afgooye UTM zone 38N</summary>
        Afgooye_UTM_zone_38N = 20538,
        ///<summary>Afgooye UTM zone 39N</summary>
        Afgooye_UTM_zone_39N = 20539,
        ///<summary>Lisbon Lisbon Portuguese National Grid</summary>
        Lisbon_Lisbon_Portuguese_National_Grid = 20790,
        ///<summary>Aratu UTM zone 22S</summary>
        Aratu_UTM_zone_22S = 20822,
        ///<summary>Aratu UTM zone 23S</summary>
        Aratu_UTM_zone_23S = 20823,
        ///<summary>Aratu UTM zone 24S</summary>
        Aratu_UTM_zone_24S = 20824,
        ///<summary>Arc 1950 UTM zone 34S</summary>
        Arc_1950_UTM_zone_34S = 20934,
        ///<summary>Arc 1950 UTM zone 35S</summary>
        Arc_1950_UTM_zone_35S = 20935,
        ///<summary>Arc 1950 UTM zone 36S</summary>
        Arc_1950_UTM_zone_36S = 20936,
        ///<summary>Arc 1960 UTM zone 35S</summary>
        Arc_1960_UTM_zone_35S = 21035,
        ///<summary>Arc 1960 UTM zone 36S</summary>
        Arc_1960_UTM_zone_36S = 21036,
        ///<summary>Arc 1960 UTM zone 37S</summary>
        Arc_1960_UTM_zone_37S = 21037,
        ///<summary>Arc 1960 UTM zone 35N</summary>
        Arc_1960_UTM_zone_35N = 21095,
        ///<summary>Arc 1960 UTM zone 36N</summary>
        Arc_1960_UTM_zone_36N = 21096,
        ///<summary>Arc 1960 UTM zone 37N</summary>
        Arc_1960_UTM_zone_37N = 21097,
        ///<summary>Batavia UTM zone 48S</summary>
        Batavia_UTM_zone_48S = 21148,
        ///<summary>Batavia UTM zone 49S</summary>
        Batavia_UTM_zone_49S = 21149,
        ///<summary>Batavia UTM zone 50S</summary>
        Batavia_UTM_zone_50S = 21150,
        ///<summary>Barbados 1938 British West Indies Grid</summary>
        Barbados_1938_British_West_Indies_Grid = 21291,
        ///<summary>Barbados 1938 Barbados National Grid</summary>
        Barbados_1938_Barbados_National_Grid = 21292,
        ///<summary>Beijing 1954 GaussKruger zone 13</summary>
        Beijing_1954_GaussKruger_zone_13 = 21413,
        ///<summary>Beijing 1954 GaussKruger zone 14</summary>
        Beijing_1954_GaussKruger_zone_14 = 21414,
        ///<summary>Beijing 1954 GaussKruger zone 15</summary>
        Beijing_1954_GaussKruger_zone_15 = 21415,
        ///<summary>Beijing 1954 GaussKruger zone 16</summary>
        Beijing_1954_GaussKruger_zone_16 = 21416,
        ///<summary>Beijing 1954 GaussKruger zone 17</summary>
        Beijing_1954_GaussKruger_zone_17 = 21417,
        ///<summary>Beijing 1954 GaussKruger zone 18</summary>
        Beijing_1954_GaussKruger_zone_18 = 21418,
        ///<summary>Beijing 1954 GaussKruger zone 19</summary>
        Beijing_1954_GaussKruger_zone_19 = 21419,
        ///<summary>Beijing 1954 GaussKruger zone 20</summary>
        Beijing_1954_GaussKruger_zone_20 = 21420,
        ///<summary>Beijing 1954 GaussKruger zone 21</summary>
        Beijing_1954_GaussKruger_zone_21 = 21421,
        ///<summary>Beijing 1954 GaussKruger zone 22</summary>
        Beijing_1954_GaussKruger_zone_22 = 21422,
        ///<summary>Beijing 1954 GaussKruger zone 23</summary>
        Beijing_1954_GaussKruger_zone_23 = 21423,
        ///<summary>Beijing 1954 GaussKruger 13N deprecated</summary>
        Beijing_1954_GaussKruger_13N_deprecated = 21473,
        ///<summary>Beijing 1954 GaussKruger 14N deprecated</summary>
        Beijing_1954_GaussKruger_14N_deprecated = 21474,
        ///<summary>Beijing 1954 GaussKruger 15N deprecated</summary>
        Beijing_1954_GaussKruger_15N_deprecated = 21475,
        ///<summary>Beijing 1954 GaussKruger 16N deprecated</summary>
        Beijing_1954_GaussKruger_16N_deprecated = 21476,
        ///<summary>Beijing 1954 GaussKruger 17N deprecated</summary>
        Beijing_1954_GaussKruger_17N_deprecated = 21477,
        ///<summary>Beijing 1954 GaussKruger 18N deprecated</summary>
        Beijing_1954_GaussKruger_18N_deprecated = 21478,
        ///<summary>Beijing 1954 GaussKruger 19N deprecated</summary>
        Beijing_1954_GaussKruger_19N_deprecated = 21479,
        ///<summary>Beijing 1954 GaussKruger 20N deprecated</summary>
        Beijing_1954_GaussKruger_20N_deprecated = 21480,
        ///<summary>Beijing 1954 GaussKruger 21N deprecated</summary>
        Beijing_1954_GaussKruger_21N_deprecated = 21481,
        ///<summary>Beijing 1954 GaussKruger 22N deprecated</summary>
        Beijing_1954_GaussKruger_22N_deprecated = 21482,
        ///<summary>Beijing 1954 GaussKruger 23N deprecated</summary>
        Beijing_1954_GaussKruger_23N_deprecated = 21483,
        ///<summary>Belge 1950 Brussels Belge Lambert 50</summary>
        Belge_1950_Brussels_Belge_Lambert_50 = 21500,
        ///<summary>Bern 1898 Bern LV03C</summary>
        Bern_1898_Bern_LV03C = 21780,
        ///<summary>CH1903 LV03</summary>
        CH1903_LV03 = 21781,
        ///<summary>Bogota 1975 UTM zone 17N deprecated</summary>
        Bogota_1975_UTM_zone_17N_deprecated = 21817,
        ///<summary>Bogota 1975 UTM zone 18N</summary>
        Bogota_1975_UTM_zone_18N = 21818,
        ///<summary>Bogota 1975 Colombia West zone deprecated</summary>
        Bogota_1975_Colombia_West_zone_deprecated = 21891,
        ///<summary>Bogota 1975 Colombia Bogota zone deprecated</summary>
        Bogota_1975_Colombia_Bogota_zone_deprecated = 21892,
        ///<summary>Bogota 1975 Colombia East Central zone deprecated</summary>
        Bogota_1975_Colombia_East_Central_zone_deprecated = 21893,
        ///<summary>Bogota 1975 Colombia East deprecated</summary>
        Bogota_1975_Colombia_East_deprecated = 21894,
        ///<summary>Camacupa UTM zone 32S</summary>
        Camacupa_UTM_zone_32S = 22032,
        ///<summary>Camacupa UTM zone 33S</summary>
        Camacupa_UTM_zone_33S = 22033,
        ///<summary>Camacupa TM 1130 SE</summary>
        Camacupa_TM_1130_SE = 22091,
        ///<summary>Camacupa TM 12 SE</summary>
        Camacupa_TM_12_SE = 22092,
        ///<summary>Campo Inchauspe Argentina 1</summary>
        Campo_Inchauspe_Argentina_1 = 22191,
        ///<summary>Campo Inchauspe Argentina 2</summary>
        Campo_Inchauspe_Argentina_2 = 22192,
        ///<summary>Campo Inchauspe Argentina 3</summary>
        Campo_Inchauspe_Argentina_3 = 22193,
        ///<summary>Campo Inchauspe Argentina 4</summary>
        Campo_Inchauspe_Argentina_4 = 22194,
        ///<summary>Campo Inchauspe Argentina 5</summary>
        Campo_Inchauspe_Argentina_5 = 22195,
        ///<summary>Campo Inchauspe Argentina 6</summary>
        Campo_Inchauspe_Argentina_6 = 22196,
        ///<summary>Campo Inchauspe Argentina 7</summary>
        Campo_Inchauspe_Argentina_7 = 22197,
        ///<summary>Cape UTM zone 34S</summary>
        Cape_UTM_zone_34S = 22234,
        ///<summary>Cape UTM zone 35S</summary>
        Cape_UTM_zone_35S = 22235,
        ///<summary>Cape UTM zone 36S</summary>
        Cape_UTM_zone_36S = 22236,
        ///<summary>Carthage UTM zone 32N</summary>
        Carthage_UTM_zone_32N = 22332,
        ///<summary>Carthage Nord Tunisie</summary>
        Carthage_Nord_Tunisie = 22391,
        ///<summary>Carthage Sud Tunisie</summary>
        Carthage_Sud_Tunisie = 22392,
        ///<summary>Corrego Alegre UTM zone 23S</summary>
        Corrego_Alegre_UTM_zone_23S = 22523,
        ///<summary>Corrego Alegre UTM zone 24S</summary>
        Corrego_Alegre_UTM_zone_24S = 22524,
        ///<summary>Deir ez Zor Levant Zone</summary>
        Deir_ez_Zor_Levant_Zone = 22700,
        ///<summary>Deir ez Zor Syria Lambert</summary>
        Deir_ez_Zor_Syria_Lambert = 22770,
        ///<summary>Deir ez Zor Levant Stereographic</summary>
        Deir_ez_Zor_Levant_Stereographic = 22780,
        ///<summary>Douala UTM zone 32N deprecated</summary>
        Douala_UTM_zone_32N_deprecated = 22832,
        ///<summary>Egypt 1907 Blue Belt</summary>
        Egypt_1907_Blue_Belt = 22991,
        ///<summary>Egypt 1907 Red Belt</summary>
        Egypt_1907_Red_Belt = 22992,
        ///<summary>Egypt 1907 Purple Belt</summary>
        Egypt_1907_Purple_Belt = 22993,
        ///<summary>Egypt 1907 Extended Purple Belt</summary>
        Egypt_1907_Extended_Purple_Belt = 22994,
        ///<summary>ED50 UTM zone 28N</summary>
        ED50_UTM_zone_28N = 23028,
        ///<summary>ED50 UTM zone 29N</summary>
        ED50_UTM_zone_29N = 23029,
        ///<summary>ED50 UTM zone 30N</summary>
        ED50_UTM_zone_30N = 23030,
        ///<summary>ED50 UTM zone 31N</summary>
        ED50_UTM_zone_31N = 23031,
        ///<summary>ED50 UTM zone 32N</summary>
        ED50_UTM_zone_32N = 23032,
        ///<summary>ED50 UTM zone 33N</summary>
        ED50_UTM_zone_33N = 23033,
        ///<summary>ED50 UTM zone 34N</summary>
        ED50_UTM_zone_34N = 23034,
        ///<summary>ED50 UTM zone 35N</summary>
        ED50_UTM_zone_35N = 23035,
        ///<summary>ED50 UTM zone 36N</summary>
        ED50_UTM_zone_36N = 23036,
        ///<summary>ED50 UTM zone 37N</summary>
        ED50_UTM_zone_37N = 23037,
        ///<summary>ED50 UTM zone 38N</summary>
        ED50_UTM_zone_38N = 23038,
        ///<summary>ED50 TM 0 N</summary>
        ED50_TM_0_N = 23090,
        ///<summary>ED50 TM 5 NE</summary>
        ED50_TM_5_NE = 23095,
        ///<summary>Fahud UTM zone 39N</summary>
        Fahud_UTM_zone_39N = 23239,
        ///<summary>Fahud UTM zone 40N</summary>
        Fahud_UTM_zone_40N = 23240,
        ///<summary>Garoua UTM zone 33N deprecated</summary>
        Garoua_UTM_zone_33N_deprecated = 23433,
        ///<summary>HD72 EOV</summary>
        HD72_EOV = 23700,
        ///<summary>ID74 UTM zone 46N</summary>
        ID74_UTM_zone_46N = 23846,
        ///<summary>ID74 UTM zone 47N</summary>
        ID74_UTM_zone_47N = 23847,
        ///<summary>ID74 UTM zone 48N</summary>
        ID74_UTM_zone_48N = 23848,
        ///<summary>ID74 UTM zone 49N</summary>
        ID74_UTM_zone_49N = 23849,
        ///<summary>ID74 UTM zone 50N</summary>
        ID74_UTM_zone_50N = 23850,
        ///<summary>ID74 UTM zone 51N</summary>
        ID74_UTM_zone_51N = 23851,
        ///<summary>ID74 UTM zone 52N</summary>
        ID74_UTM_zone_52N = 23852,
        ///<summary>ID74 UTM zone 53N deprecated</summary>
        ID74_UTM_zone_53N_deprecated = 23853,
        ///<summary>ID74 UTM zone 46S deprecated</summary>
        ID74_UTM_zone_46S_deprecated = 23886,
        ///<summary>ID74 UTM zone 47S</summary>
        ID74_UTM_zone_47S = 23887,
        ///<summary>ID74 UTM zone 48S</summary>
        ID74_UTM_zone_48S = 23888,
        ///<summary>ID74 UTM zone 49S</summary>
        ID74_UTM_zone_49S = 23889,
        ///<summary>ID74 UTM zone 50S</summary>
        ID74_UTM_zone_50S = 23890,
        ///<summary>ID74 UTM zone 51S</summary>
        ID74_UTM_zone_51S = 23891,
        ///<summary>ID74 UTM zone 52S</summary>
        ID74_UTM_zone_52S = 23892,
        ///<summary>ID74 UTM zone 53S</summary>
        ID74_UTM_zone_53S = 23893,
        ///<summary>ID74 UTM zone 54S</summary>
        ID74_UTM_zone_54S = 23894,
        ///<summary>Indian 1954 UTM zone 46N</summary>
        Indian_1954_UTM_zone_46N = 23946,
        ///<summary>Indian 1954 UTM zone 47N</summary>
        Indian_1954_UTM_zone_47N = 23947,
        ///<summary>Indian 1954 UTM zone 48N</summary>
        Indian_1954_UTM_zone_48N = 23948,
        ///<summary>Indian 1975 UTM zone 47N</summary>
        Indian_1975_UTM_zone_47N = 24047,
        ///<summary>Indian 1975 UTM zone 48N</summary>
        Indian_1975_UTM_zone_48N = 24048,
        ///<summary>Jamaica 1875 Jamaica Old Grid</summary>
        Jamaica_1875_Jamaica_Old_Grid = 24100,
        ///<summary>JAD69 Jamaica National Grid</summary>
        JAD69_Jamaica_National_Grid = 24200,
        ///<summary>Kalianpur 1937 UTM zone 45N</summary>
        Kalianpur_1937_UTM_zone_45N = 24305,
        ///<summary>Kalianpur 1937 UTM zone 46N</summary>
        Kalianpur_1937_UTM_zone_46N = 24306,
        ///<summary>Kalianpur 1962 UTM zone 41N</summary>
        Kalianpur_1962_UTM_zone_41N = 24311,
        ///<summary>Kalianpur 1962 UTM zone 42N</summary>
        Kalianpur_1962_UTM_zone_42N = 24312,
        ///<summary>Kalianpur 1962 UTM zone 43N</summary>
        Kalianpur_1962_UTM_zone_43N = 24313,
        ///<summary>Kalianpur 1975 UTM zone 42N</summary>
        Kalianpur_1975_UTM_zone_42N = 24342,
        ///<summary>Kalianpur 1975 UTM zone 43N</summary>
        Kalianpur_1975_UTM_zone_43N = 24343,
        ///<summary>Kalianpur 1975 UTM zone 44N</summary>
        Kalianpur_1975_UTM_zone_44N = 24344,
        ///<summary>Kalianpur 1975 UTM zone 45N</summary>
        Kalianpur_1975_UTM_zone_45N = 24345,
        ///<summary>Kalianpur 1975 UTM zone 46N</summary>
        Kalianpur_1975_UTM_zone_46N = 24346,
        ///<summary>Kalianpur 1975 UTM zone 47N</summary>
        Kalianpur_1975_UTM_zone_47N = 24347,
        ///<summary>Kalianpur 1880 India zone 0</summary>
        Kalianpur_1880_India_zone_0 = 24370,
        ///<summary>Kalianpur 1880 India zone I</summary>
        Kalianpur_1880_India_zone_I = 24371,
        ///<summary>Kalianpur 1880 India zone IIa</summary>
        Kalianpur_1880_India_zone_IIa = 24372,
        ///<summary>Kalianpur 1880 India zone III</summary>
        Kalianpur_1880_India_zone_III = 24373,
        ///<summary>Kalianpur 1880 India zone IV</summary>
        Kalianpur_1880_India_zone_IV = 24374,
        ///<summary>Kalianpur 1937 India zone IIb</summary>
        Kalianpur_1937_India_zone_IIb = 24375,
        ///<summary>Kalianpur 1962 India zone I</summary>
        Kalianpur_1962_India_zone_I = 24376,
        ///<summary>Kalianpur 1962 India zone IIa</summary>
        Kalianpur_1962_India_zone_IIa = 24377,
        ///<summary>Kalianpur 1975 India zone I</summary>
        Kalianpur_1975_India_zone_I = 24378,
        ///<summary>Kalianpur 1975 India zone IIa</summary>
        Kalianpur_1975_India_zone_IIa = 24379,
        ///<summary>Kalianpur 1975 India zone IIb</summary>
        Kalianpur_1975_India_zone_IIb = 24380,
        ///<summary>Kalianpur 1975 India zone III</summary>
        Kalianpur_1975_India_zone_III = 24381,
        ///<summary>Kalianpur 1880 India zone IIb</summary>
        Kalianpur_1880_India_zone_IIb = 24382,
        ///<summary>Kertau 1968 Singapore Grid</summary>
        Kertau_1968_Singapore_Grid = 24500,
        ///<summary>Kertau 1968 UTM zone 47N</summary>
        Kertau_1968_UTM_zone_47N = 24547,
        ///<summary>Kertau 1968 UTM zone 48N</summary>
        Kertau_1968_UTM_zone_48N = 24548,
        ///<summary>Kertau RSO Malaya ch deprecated</summary>
        Kertau_RSO_Malaya_ch_deprecated = 24571,
        ///<summary>KOC Lambert</summary>
        KOC_Lambert = 24600,
        ///<summary>La Canoa UTM zone 18N</summary>
        La_Canoa_UTM_zone_18N = 24718,
        ///<summary>La Canoa UTM zone 19N</summary>
        La_Canoa_UTM_zone_19N = 24719,
        ///<summary>La Canoa UTM zone 20N</summary>
        La_Canoa_UTM_zone_20N = 24720,
        ///<summary>PSAD56 UTM zone 18N</summary>
        PSAD56_UTM_zone_18N = 24818,
        ///<summary>PSAD56 UTM zone 19N</summary>
        PSAD56_UTM_zone_19N = 24819,
        ///<summary>PSAD56 UTM zone 20N</summary>
        PSAD56_UTM_zone_20N = 24820,
        ///<summary>PSAD56 UTM zone 21N</summary>
        PSAD56_UTM_zone_21N = 24821,
        ///<summary>PSAD56 UTM zone 17S</summary>
        PSAD56_UTM_zone_17S = 24877,
        ///<summary>PSAD56 UTM zone 18S</summary>
        PSAD56_UTM_zone_18S = 24878,
        ///<summary>PSAD56 UTM zone 19S</summary>
        PSAD56_UTM_zone_19S = 24879,
        ///<summary>PSAD56 UTM zone 20S</summary>
        PSAD56_UTM_zone_20S = 24880,
        ///<summary>PSAD56 UTM zone 22S</summary>
        PSAD56_UTM_zone_22S = 24882,
        ///<summary>PSAD56 Peru west zone</summary>
        PSAD56_Peru_west_zone = 24891,
        ///<summary>PSAD56 Peru central zone</summary>
        PSAD56_Peru_central_zone = 24892,
        ///<summary>PSAD56 Peru east zone</summary>
        PSAD56_Peru_east_zone = 24893,
        ///<summary>Leigon Ghana Metre Grid</summary>
        Leigon_Ghana_Metre_Grid = 25000,
        ///<summary>Lome UTM zone 31N</summary>
        Lome_UTM_zone_31N = 25231,
        ///<summary>Luzon 1911 Philippines zone I</summary>
        Luzon_1911_Philippines_zone_I = 25391,
        ///<summary>Luzon 1911 Philippines zone II</summary>
        Luzon_1911_Philippines_zone_II = 25392,
        ///<summary>Luzon 1911 Philippines zone III</summary>
        Luzon_1911_Philippines_zone_III = 25393,
        ///<summary>Luzon 1911 Philippines zone IV</summary>
        Luzon_1911_Philippines_zone_IV = 25394,
        ///<summary>Luzon 1911 Philippines zone V</summary>
        Luzon_1911_Philippines_zone_V = 25395,
        ///<summary>ETRS89 TM Baltic93</summary>
        ETRS89_TM_Baltic93 = 25884,
        ///<summary>Malongo 1987 UTM zone 32S</summary>
        Malongo_1987_UTM_zone_32S = 25932,
        ///<summary>Merchich Nord Maroc</summary>
        Merchich_Nord_Maroc = 26191,
        ///<summary>Merchich Sud Maroc</summary>
        Merchich_Sud_Maroc = 26192,
        ///<summary>Massawa UTM zone 37N</summary>
        Massawa_UTM_zone_37N = 26237,
        ///<summary>Minna UTM zone 31N</summary>
        Minna_UTM_zone_31N = 26331,
        ///<summary>Minna UTM zone 32N</summary>
        Minna_UTM_zone_32N = 26332,
        ///<summary>Minna Nigeria West Belt</summary>
        Minna_Nigeria_West_Belt = 26391,
        ///<summary>Minna Nigeria Mid Belt</summary>
        Minna_Nigeria_Mid_Belt = 26392,
        ///<summary>Minna Nigeria East Belt</summary>
        Minna_Nigeria_East_Belt = 26393,
        ///<summary>Mhast UTM zone 32S deprecated</summary>
        Mhast_UTM_zone_32S_deprecated = 26432,
        ///<summary>Monte Mario Rome Italy zone 1 deprecated</summary>
        Monte_Mario_Rome_Italy_zone_1_deprecated = 26591,
        ///<summary>Monte Mario Rome Italy zone 2 deprecated</summary>
        Monte_Mario_Rome_Italy_zone_2_deprecated = 26592,
        ///<summary>Mporaloko UTM zone 32N</summary>
        Mporaloko_UTM_zone_32N = 26632,
        ///<summary>Mporaloko UTM zone 32S</summary>
        Mporaloko_UTM_zone_32S = 26692,
        ///<summary>NAD 1927 UTM Zone 3N</summary>
        NAD_1927_UTM_Zone_3N = 26703,
        ///<summary>NAD 1927 UTM Zone 4N</summary>
        NAD_1927_UTM_Zone_4N = 26704,
        ///<summary>NAD 1927 UTM Zone 5N</summary>
        NAD_1927_UTM_Zone_5N = 26705,
        ///<summary>NAD 1927 UTM Zone 6N</summary>
        NAD_1927_UTM_Zone_6N = 26706,
        ///<summary>NAD 1927 UTM Zone 7N</summary>
        NAD_1927_UTM_Zone_7N = 26707,
        ///<summary>NAD 1927 UTM Zone 8N</summary>
        NAD_1927_UTM_Zone_8N = 26708,
        ///<summary>NAD 1927 UTM Zone 9N</summary>
        NAD_1927_UTM_Zone_9N = 26709,
        ///<summary>NAD 1927 UTM Zone 10N</summary>
        NAD_1927_UTM_Zone_10N = 26710,
        ///<summary>NAD 1927 UTM Zone 11N</summary>
        NAD_1927_UTM_Zone_11N = 26711,
        ///<summary>NAD 1927 UTM Zone 12N</summary>
        NAD_1927_UTM_Zone_12N = 26712,
        ///<summary>NAD 1927 UTM Zone 13N</summary>
        NAD_1927_UTM_Zone_13N = 26713,
        ///<summary>NAD 1927 UTM Zone 14N</summary>
        NAD_1927_UTM_Zone_14N = 26714,
        ///<summary>NAD 1927 UTM Zone 15N</summary>
        NAD_1927_UTM_Zone_15N = 26715,
        ///<summary>NAD 1927 UTM Zone 16N</summary>
        NAD_1927_UTM_Zone_16N = 26716,
        ///<summary>NAD 1927 UTM Zone 17N</summary>
        NAD_1927_UTM_Zone_17N = 26717,
        ///<summary>NAD 1927 UTM Zone 18N</summary>
        NAD_1927_UTM_Zone_18N = 26718,
        ///<summary>NAD 1927 UTM Zone 19N</summary>
        NAD_1927_UTM_Zone_19N = 26719,
        ///<summary>NAD 1927 UTM Zone 20N</summary>
        NAD_1927_UTM_Zone_20N = 26720,
        ///<summary>NAD 1927 UTM Zone 21N</summary>
        NAD_1927_UTM_Zone_21N = 26721,
        ///<summary>NAD 1927 UTM Zone 22N</summary>
        NAD_1927_UTM_Zone_22N = 26722,
        ///<summary>NAD27 Alabama East</summary>
        NAD27_Alabama_East = 26729,
        ///<summary>NAD27 Alabama West</summary>
        NAD27_Alabama_West = 26730,
        ///<summary>NAD27 Alaska zone 2</summary>
        NAD27_Alaska_zone_2 = 26732,
        ///<summary>NAD27 Alaska zone 3</summary>
        NAD27_Alaska_zone_3 = 26733,
        ///<summary>NAD27 Alaska zone 4</summary>
        NAD27_Alaska_zone_4 = 26734,
        ///<summary>NAD27 Alaska zone 5</summary>
        NAD27_Alaska_zone_5 = 26735,
        ///<summary>NAD27 Alaska zone 6</summary>
        NAD27_Alaska_zone_6 = 26736,
        ///<summary>NAD27 Alaska zone 7</summary>
        NAD27_Alaska_zone_7 = 26737,
        ///<summary>NAD27 Alaska zone 8</summary>
        NAD27_Alaska_zone_8 = 26738,
        ///<summary>NAD27 Alaska zone 9</summary>
        NAD27_Alaska_zone_9 = 26739,
        ///<summary>NAD27 Alaska zone 10</summary>
        NAD27_Alaska_zone_10 = 26740,
        ///<summary>NAD27 California zone I</summary>
        NAD27_California_zone_I = 26741,
        ///<summary>NAD27 California zone II</summary>
        NAD27_California_zone_II = 26742,
        ///<summary>NAD27 California zone III</summary>
        NAD27_California_zone_III = 26743,
        ///<summary>NAD27 California zone IV</summary>
        NAD27_California_zone_IV = 26744,
        ///<summary>NAD27 California zone V</summary>
        NAD27_California_zone_V = 26745,
        ///<summary>NAD27 California zone VI</summary>
        NAD27_California_zone_VI = 26746,
        ///<summary>NAD27 California zone VII deprecated</summary>
        NAD27_California_zone_VII_deprecated = 26747,
        ///<summary>NAD27 Arizona East</summary>
        NAD27_Arizona_East = 26748,
        ///<summary>NAD27 Arizona Central</summary>
        NAD27_Arizona_Central = 26749,
        ///<summary>NAD27 Arizona West</summary>
        NAD27_Arizona_West = 26750,
        ///<summary>NAD27 Arkansas North</summary>
        NAD27_Arkansas_North = 26751,
        ///<summary>NAD27 Arkansas South</summary>
        NAD27_Arkansas_South = 26752,
        ///<summary>NAD27 Colorado North</summary>
        NAD27_Colorado_North = 26753,
        ///<summary>NAD27 Colorado Central</summary>
        NAD27_Colorado_Central = 26754,
        ///<summary>NAD27 Colorado South</summary>
        NAD27_Colorado_South = 26755,
        ///<summary>NAD27 Connecticut</summary>
        NAD27_Connecticut = 26756,
        ///<summary>NAD27 Delaware</summary>
        NAD27_Delaware = 26757,
        ///<summary>NAD27 Florida East</summary>
        NAD27_Florida_East = 26758,
        ///<summary>NAD27 Florida West</summary>
        NAD27_Florida_West = 26759,
        ///<summary>NAD27 Florida North</summary>
        NAD27_Florida_North = 26760,
        ///<summary>NAD27 Georgia East</summary>
        NAD27_Georgia_East = 26766,
        ///<summary>NAD27 Georgia West</summary>
        NAD27_Georgia_West = 26767,
        ///<summary>NAD27 Idaho East</summary>
        NAD27_Idaho_East = 26768,
        ///<summary>NAD27 Idaho Central</summary>
        NAD27_Idaho_Central = 26769,
        ///<summary>NAD27 Idaho West</summary>
        NAD27_Idaho_West = 26770,
        ///<summary>NAD27 Illinois East</summary>
        NAD27_Illinois_East = 26771,
        ///<summary>NAD27 Illinois West</summary>
        NAD27_Illinois_West = 26772,
        ///<summary>NAD27 Indiana East</summary>
        NAD27_Indiana_East = 26773,
        ///<summary>NAD27 Indiana West</summary>
        NAD27_Indiana_West = 26774,
        ///<summary>NAD27 Iowa North</summary>
        NAD27_Iowa_North = 26775,
        ///<summary>NAD27 Iowa South</summary>
        NAD27_Iowa_South = 26776,
        ///<summary>NAD27 Kansas North</summary>
        NAD27_Kansas_North = 26777,
        ///<summary>NAD27 Kansas South</summary>
        NAD27_Kansas_South = 26778,
        ///<summary>NAD27 Kentucky North</summary>
        NAD27_Kentucky_North = 26779,
        ///<summary>NAD27 Kentucky South</summary>
        NAD27_Kentucky_South = 26780,
        ///<summary>NAD27 Louisiana North</summary>
        NAD27_Louisiana_North = 26781,
        ///<summary>NAD27 Louisiana South</summary>
        NAD27_Louisiana_South = 26782,
        ///<summary>NAD27 Maine East</summary>
        NAD27_Maine_East = 26783,
        ///<summary>NAD27 Maine West</summary>
        NAD27_Maine_West = 26784,
        ///<summary>NAD27 Maryland</summary>
        NAD27_Maryland = 26785,
        ///<summary>NAD27 Massachusetts Mainland</summary>
        NAD27_Massachusetts_Mainland = 26786,
        ///<summary>NAD27 Massachusetts Island</summary>
        NAD27_Massachusetts_Island = 26787,
        ///<summary>NAD27 Minnesota North</summary>
        NAD27_Minnesota_North = 26791,
        ///<summary>NAD27 Minnesota Central</summary>
        NAD27_Minnesota_Central = 26792,
        ///<summary>NAD27 Minnesota South</summary>
        NAD27_Minnesota_South = 26793,
        ///<summary>NAD27 Mississippi East</summary>
        NAD27_Mississippi_East = 26794,
        ///<summary>NAD27 Mississippi West</summary>
        NAD27_Mississippi_West = 26795,
        ///<summary>NAD27 Missouri East</summary>
        NAD27_Missouri_East = 26796,
        ///<summary>NAD27 Missouri Central</summary>
        NAD27_Missouri_Central = 26797,
        ///<summary>NAD27 Missouri West</summary>
        NAD27_Missouri_West = 26798,
        ///<summary>NAD Michigan Michigan East</summary>
        NAD_Michigan_Michigan_East = 26801,
        ///<summary>NAD Michigan Michigan Old Central</summary>
        NAD_Michigan_Michigan_Old_Central = 26802,
        ///<summary>NAD Michigan Michigan West</summary>
        NAD_Michigan_Michigan_West = 26803,
        ///<summary>NAD Michigan Michigan North</summary>
        NAD_Michigan_Michigan_North = 26811,
        ///<summary>NAD Michigan Michigan Central</summary>
        NAD_Michigan_Michigan_Central = 26812,
        ///<summary>NAD Michigan Michigan South</summary>
        NAD_Michigan_Michigan_South = 26813,
        ///<summary>NAD 1983 UTM Zone 3N</summary>
        NAD_1983_UTM_Zone_3N = 26903,
        ///<summary>NAD 1983 UTM Zone 4N</summary>
        NAD_1983_UTM_Zone_4N = 26904,
        ///<summary>NAD 1983 UTM Zone 5N</summary>
        NAD_1983_UTM_Zone_5N = 26905,
        ///<summary>NAD 1983 UTM Zone 6N</summary>
        NAD_1983_UTM_Zone_6N = 26906,
        ///<summary>NAD 1983 UTM Zone 7N</summary>
        NAD_1983_UTM_Zone_7N = 26907,
        ///<summary>NAD 1983 UTM Zone 8N</summary>
        NAD_1983_UTM_Zone_8N = 26908,
        ///<summary>NAD 1983 UTM Zone 9N</summary>
        NAD_1983_UTM_Zone_9N = 26909,
        ///<summary>NAD 1983 UTM Zone 10N</summary>
        NAD_1983_UTM_Zone_10N = 26910,
        ///<summary>NAD 1983 UTM Zone 11N</summary>
        NAD_1983_UTM_Zone_11N = 26911,
        ///<summary>NAD 1983 UTM Zone 12N</summary>
        NAD_1983_UTM_Zone_12N = 26912,
        ///<summary>NAD 1983 UTM Zone 13N</summary>
        NAD_1983_UTM_Zone_13N = 26913,
        ///<summary>NAD 1983 UTM Zone 14N</summary>
        NAD_1983_UTM_Zone_14N = 26914,
        ///<summary>NAD 1983 UTM Zone 15N</summary>
        NAD_1983_UTM_Zone_15N = 26915,
        ///<summary>NAD 1983 UTM Zone 16N</summary>
        NAD_1983_UTM_Zone_16N = 26916,
        ///<summary>NAD 1983 UTM Zone 17N</summary>
        NAD_1983_UTM_Zone_17N = 26917,
        ///<summary>NAD 1983 UTM Zone 18N</summary>
        NAD_1983_UTM_Zone_18N = 26918,
        ///<summary>NAD 1983 UTM Zone 19N</summary>
        NAD_1983_UTM_Zone_19N = 26919,
        ///<summary>NAD 1983 UTM Zone 20N</summary>
        NAD_1983_UTM_Zone_20N = 26920,
        ///<summary>NAD 1983 UTM Zone 21N</summary>
        NAD_1983_UTM_Zone_21N = 26921,
        ///<summary>NAD 1983 UTM Zone 22N</summary>
        NAD_1983_UTM_Zone_22N = 26922,
        ///<summary>NAD 1983 UTM Zone 23N</summary>
        NAD_1983_UTM_Zone_23N = 26923,
        ///<summary>NAD83 Alabama East</summary>
        NAD83_Alabama_East = 26929,
        ///<summary>NAD83 Alabama West</summary>
        NAD83_Alabama_West = 26930,
        ///<summary>NAD83 Alaska zone 1</summary>
        NAD83_Alaska_zone_1 = 26931,
        ///<summary>NAD83 Alaska zone 2</summary>
        NAD83_Alaska_zone_2 = 26932,
        ///<summary>NAD83 Alaska zone 3</summary>
        NAD83_Alaska_zone_3 = 26933,
        ///<summary>NAD83 Alaska zone 4</summary>
        NAD83_Alaska_zone_4 = 26934,
        ///<summary>NAD83 Alaska zone 5</summary>
        NAD83_Alaska_zone_5 = 26935,
        ///<summary>NAD83 Alaska zone 6</summary>
        NAD83_Alaska_zone_6 = 26936,
        ///<summary>NAD83 Alaska zone 7</summary>
        NAD83_Alaska_zone_7 = 26937,
        ///<summary>NAD83 Alaska zone 8</summary>
        NAD83_Alaska_zone_8 = 26938,
        ///<summary>NAD83 Alaska zone 9</summary>
        NAD83_Alaska_zone_9 = 26939,
        ///<summary>NAD83 Alaska zone 10</summary>
        NAD83_Alaska_zone_10 = 26940,
        ///<summary>NAD83 California zone 1</summary>
        NAD83_California_zone_1 = 26941,
        ///<summary>NAD83 California zone 2</summary>
        NAD83_California_zone_2 = 26942,
        ///<summary>NAD83 California zone 3</summary>
        NAD83_California_zone_3 = 26943,
        ///<summary>NAD83 California zone 4</summary>
        NAD83_California_zone_4 = 26944,
        ///<summary>NAD83 California zone 5</summary>
        NAD83_California_zone_5 = 26945,
        ///<summary>NAD83 California zone 6</summary>
        NAD83_California_zone_6 = 26946,
        ///<summary>NAD83 Arizona East</summary>
        NAD83_Arizona_East = 26948,
        ///<summary>NAD83 Arizona Central</summary>
        NAD83_Arizona_Central = 26949,
        ///<summary>NAD83 Arizona West</summary>
        NAD83_Arizona_West = 26950,
        ///<summary>NAD83 Arkansas North</summary>
        NAD83_Arkansas_North = 26951,
        ///<summary>NAD83 Arkansas South</summary>
        NAD83_Arkansas_South = 26952,
        ///<summary>NAD83 Colorado North</summary>
        NAD83_Colorado_North = 26953,
        ///<summary>NAD83 Colorado Central</summary>
        NAD83_Colorado_Central = 26954,
        ///<summary>NAD83 Colorado South</summary>
        NAD83_Colorado_South = 26955,
        ///<summary>NAD83 Connecticut</summary>
        NAD83_Connecticut = 26956,
        ///<summary>NAD83 Delaware</summary>
        NAD83_Delaware = 26957,
        ///<summary>NAD83 Florida East</summary>
        NAD83_Florida_East = 26958,
        ///<summary>NAD83 Florida West</summary>
        NAD83_Florida_West = 26959,
        ///<summary>NAD83 Florida North</summary>
        NAD83_Florida_North = 26960,
        ///<summary>NAD83 Hawaii zone 1</summary>
        NAD83_Hawaii_zone_1 = 26961,
        ///<summary>NAD83 Hawaii zone 2</summary>
        NAD83_Hawaii_zone_2 = 26962,
        ///<summary>NAD83 Hawaii zone 3</summary>
        NAD83_Hawaii_zone_3 = 26963,
        ///<summary>NAD83 Hawaii zone 4</summary>
        NAD83_Hawaii_zone_4 = 26964,
        ///<summary>NAD83 Hawaii zone 5</summary>
        NAD83_Hawaii_zone_5 = 26965,
        ///<summary>NAD83 Georgia East</summary>
        NAD83_Georgia_East = 26966,
        ///<summary>NAD83 Georgia West</summary>
        NAD83_Georgia_West = 26967,
        ///<summary>NAD83 Idaho East</summary>
        NAD83_Idaho_East = 26968,
        ///<summary>NAD83 Idaho Central</summary>
        NAD83_Idaho_Central = 26969,
        ///<summary>NAD83 Idaho West</summary>
        NAD83_Idaho_West = 26970,
        ///<summary>NAD83 Illinois East</summary>
        NAD83_Illinois_East = 26971,
        ///<summary>NAD83 Illinois West</summary>
        NAD83_Illinois_West = 26972,
        ///<summary>NAD83 Indiana East</summary>
        NAD83_Indiana_East = 26973,
        ///<summary>NAD83 Indiana West</summary>
        NAD83_Indiana_West = 26974,
        ///<summary>NAD83 Iowa North</summary>
        NAD83_Iowa_North = 26975,
        ///<summary>NAD83 Iowa South</summary>
        NAD83_Iowa_South = 26976,
        ///<summary>NAD83 Kansas North</summary>
        NAD83_Kansas_North = 26977,
        ///<summary>NAD83 Kansas South</summary>
        NAD83_Kansas_South = 26978,
        ///<summary>NAD83 Kentucky North deprecated</summary>
        NAD83_Kentucky_North_deprecated = 26979,
        ///<summary>NAD83 Kentucky South</summary>
        NAD83_Kentucky_South = 26980,
        ///<summary>NAD83 Louisiana North</summary>
        NAD83_Louisiana_North = 26981,
        ///<summary>NAD83 Louisiana South</summary>
        NAD83_Louisiana_South = 26982,
        ///<summary>NAD83 Maine East</summary>
        NAD83_Maine_East = 26983,
        ///<summary>NAD83 Maine West</summary>
        NAD83_Maine_West = 26984,
        ///<summary>NAD83 Maryland</summary>
        NAD83_Maryland = 26985,
        ///<summary>NAD83 Massachusetts Mainland</summary>
        NAD83_Massachusetts_Mainland = 26986,
        ///<summary>NAD83 Massachusetts Island</summary>
        NAD83_Massachusetts_Island = 26987,
        ///<summary>NAD83 Michigan North</summary>
        NAD83_Michigan_North = 26988,
        ///<summary>NAD83 Michigan Central</summary>
        NAD83_Michigan_Central = 26989,
        ///<summary>NAD83 Michigan South</summary>
        NAD83_Michigan_South = 26990,
        ///<summary>NAD83 Minnesota North</summary>
        NAD83_Minnesota_North = 26991,
        ///<summary>NAD83 Minnesota Central</summary>
        NAD83_Minnesota_Central = 26992,
        ///<summary>NAD83 Minnesota South</summary>
        NAD83_Minnesota_South = 26993,
        ///<summary>NAD83 Mississippi East</summary>
        NAD83_Mississippi_East = 26994,
        ///<summary>NAD83 Mississippi West</summary>
        NAD83_Mississippi_West = 26995,
        ///<summary>NAD83 Missouri East</summary>
        NAD83_Missouri_East = 26996,
        ///<summary>NAD83 Missouri Central</summary>
        NAD83_Missouri_Central = 26997,
        ///<summary>NAD83 Missouri West</summary>
        NAD83_Missouri_West = 26998,
        ///<summary>Nahrwan 1967 UTM zone 38N</summary>
        Nahrwan_1967_UTM_zone_38N = 27038,
        ///<summary>Nahrwan 1967 UTM zone 39N</summary>
        Nahrwan_1967_UTM_zone_39N = 27039,
        ///<summary>Nahrwan 1967 UTM zone 40N</summary>
        Nahrwan_1967_UTM_zone_40N = 27040,
        ///<summary>Naparima 1972 UTM zone 20N</summary>
        Naparima_1972_UTM_zone_20N = 27120,
        ///<summary>NZGD49 New Zealand Map Grid</summary>
        NZGD49_New_Zealand_Map_Grid = 27200,
        ///<summary>NZGD49 Mount Eden Circuit</summary>
        NZGD49_Mount_Eden_Circuit = 27205,
        ///<summary>NZGD49 Bay of Plenty Circuit</summary>
        NZGD49_Bay_of_Plenty_Circuit = 27206,
        ///<summary>NZGD49 Poverty Bay Circuit</summary>
        NZGD49_Poverty_Bay_Circuit = 27207,
        ///<summary>NZGD49 Hawkes Bay Circuit</summary>
        NZGD49_Hawkes_Bay_Circuit = 27208,
        ///<summary>NZGD49 Taranaki Circuit</summary>
        NZGD49_Taranaki_Circuit = 27209,
        ///<summary>NZGD49 Tuhirangi Circuit</summary>
        NZGD49_Tuhirangi_Circuit = 27210,
        ///<summary>NZGD49 Wanganui Circuit</summary>
        NZGD49_Wanganui_Circuit = 27211,
        ///<summary>NZGD49 Wairarapa Circuit</summary>
        NZGD49_Wairarapa_Circuit = 27212,
        ///<summary>NZGD49 Wellington Circuit</summary>
        NZGD49_Wellington_Circuit = 27213,
        ///<summary>NZGD49 Collingwood Circuit</summary>
        NZGD49_Collingwood_Circuit = 27214,
        ///<summary>NZGD49 Nelson Circuit</summary>
        NZGD49_Nelson_Circuit = 27215,
        ///<summary>NZGD49 Karamea Circuit</summary>
        NZGD49_Karamea_Circuit = 27216,
        ///<summary>NZGD49 Buller Circuit</summary>
        NZGD49_Buller_Circuit = 27217,
        ///<summary>NZGD49 Grey Circuit</summary>
        NZGD49_Grey_Circuit = 27218,
        ///<summary>NZGD49 Amuri Circuit</summary>
        NZGD49_Amuri_Circuit = 27219,
        ///<summary>NZGD49 Marlborough Circuit</summary>
        NZGD49_Marlborough_Circuit = 27220,
        ///<summary>NZGD49 Hokitika Circuit</summary>
        NZGD49_Hokitika_Circuit = 27221,
        ///<summary>NZGD49 Okarito Circuit</summary>
        NZGD49_Okarito_Circuit = 27222,
        ///<summary>NZGD49 Jacksons Bay Circuit</summary>
        NZGD49_Jacksons_Bay_Circuit = 27223,
        ///<summary>NZGD49 Mount Pleasant Circuit</summary>
        NZGD49_Mount_Pleasant_Circuit = 27224,
        ///<summary>NZGD49 Gawler Circuit</summary>
        NZGD49_Gawler_Circuit = 27225,
        ///<summary>NZGD49 Timaru Circuit</summary>
        NZGD49_Timaru_Circuit = 27226,
        ///<summary>NZGD49 Lindis Peak Circuit</summary>
        NZGD49_Lindis_Peak_Circuit = 27227,
        ///<summary>NZGD49 Mount Nicholas Circuit</summary>
        NZGD49_Mount_Nicholas_Circuit = 27228,
        ///<summary>NZGD49 Mount York Circuit</summary>
        NZGD49_Mount_York_Circuit = 27229,
        ///<summary>NZGD49 Observation Point Circuit</summary>
        NZGD49_Observation_Point_Circuit = 27230,
        ///<summary>NZGD49 North Taieri Circuit</summary>
        NZGD49_North_Taieri_Circuit = 27231,
        ///<summary>NZGD49 Bluff Circuit</summary>
        NZGD49_Bluff_Circuit = 27232,
        ///<summary>NZGD49 UTM zone 58S</summary>
        NZGD49_UTM_zone_58S = 27258,
        ///<summary>NZGD49 UTM zone 59S</summary>
        NZGD49_UTM_zone_59S = 27259,
        ///<summary>NZGD49 UTM zone 60S</summary>
        NZGD49_UTM_zone_60S = 27260,
        ///<summary>NZGD49 North Island Grid</summary>
        NZGD49_North_Island_Grid = 27291,
        ///<summary>NZGD49 South Island Grid</summary>
        NZGD49_South_Island_Grid = 27292,
        ///<summary>NGO 1948 Oslo NGO zone I</summary>
        NGO_1948_Oslo_NGO_zone_I = 27391,
        ///<summary>NGO 1948 Oslo NGO zone II</summary>
        NGO_1948_Oslo_NGO_zone_II = 27392,
        ///<summary>NGO 1948 Oslo NGO zone III</summary>
        NGO_1948_Oslo_NGO_zone_III = 27393,
        ///<summary>NGO 1948 Oslo NGO zone IV</summary>
        NGO_1948_Oslo_NGO_zone_IV = 27394,
        ///<summary>NGO 1948 Oslo NGO zone V</summary>
        NGO_1948_Oslo_NGO_zone_V = 27395,
        ///<summary>NGO 1948 Oslo NGO zone VI</summary>
        NGO_1948_Oslo_NGO_zone_VI = 27396,
        ///<summary>NGO 1948 Oslo NGO zone VII</summary>
        NGO_1948_Oslo_NGO_zone_VII = 27397,
        ///<summary>NGO 1948 Oslo NGO zone VIII</summary>
        NGO_1948_Oslo_NGO_zone_VIII = 27398,
        ///<summary>Datum 73 UTM zone 29N</summary>
        Datum_73_UTM_zone_29N = 27429,
        ///<summary>Lambert Conformal Conic</summary>
        Lambert_Conformal_Conic = 27500,
        ///<summary>NTF Paris France I deprecated</summary>
        NTF_Paris_France_I_deprecated = 27581,
        ///<summary>NTF Paris France II deprecated</summary>
        NTF_Paris_France_II_deprecated = 27582,
        ///<summary>NTF Paris France III deprecated</summary>
        NTF_Paris_France_III_deprecated = 27583,
        ///<summary>NTF Paris France IV deprecated</summary>
        NTF_Paris_France_IV_deprecated = 27584,
        ///<summary>NTF Paris Nord France deprecated</summary>
        NTF_Paris_Nord_France_deprecated = 27591,
        ///<summary>NTF Paris Centre France deprecated</summary>
        NTF_Paris_Centre_France_deprecated = 27592,
        ///<summary>NTF Paris Sud France deprecated</summary>
        NTF_Paris_Sud_France_deprecated = 27593,
        ///<summary>OSGB 1936 British National Grid</summary>
        OSGB_1936_British_National_Grid = 27700,
        ///<summary>Palestine 1923 Palestine Grid</summary>
        Palestine_1923_Palestine_Grid = 28191,
        ///<summary>Palestine 1923 Palestine Belt</summary>
        Palestine_1923_Palestine_Belt = 28192,
        ///<summary>Palestine 1923 Israeli CS Grid</summary>
        Palestine_1923_Israeli_CS_Grid = 28193,
        ///<summary>Pointe Noire UTM zone 32S</summary>
        Pointe_Noire_UTM_zone_32S = 28232,
        ///<summary>GDA94 MGA zone 49</summary>
        GDA94_MGA_zone_49 = 28349,
        ///<summary>GDA94 MGA zone 50</summary>
        GDA94_MGA_zone_50 = 28350,
        ///<summary>GDA94 MGA zone 51</summary>
        GDA94_MGA_zone_51 = 28351,
        ///<summary>GDA94 MGA zone 52</summary>
        GDA94_MGA_zone_52 = 28352,
        ///<summary>GDA94 MGA zone 53</summary>
        GDA94_MGA_zone_53 = 28353,
        ///<summary>GDA94 MGA zone 54</summary>
        GDA94_MGA_zone_54 = 28354,
        ///<summary>GDA94 MGA zone 55</summary>
        GDA94_MGA_zone_55 = 28355,
        ///<summary>GDA94 MGA zone 56</summary>
        GDA94_MGA_zone_56 = 28356,
        ///<summary>GDA94 MGA zone 57</summary>
        GDA94_MGA_zone_57 = 28357,
        ///<summary>GDA94 MGA zone 58</summary>
        GDA94_MGA_zone_58 = 28358,
        ///<summary>Pulkovo 1942 GaussKruger zone 2</summary>
        Pulkovo_1942_GaussKruger_zone_2 = 28402,
        ///<summary>Pulkovo 1942 GaussKruger zone 3</summary>
        Pulkovo_1942_GaussKruger_zone_3 = 28403,
        ///<summary>Pulkovo 1942 GaussKruger zone 4</summary>
        Pulkovo_1942_GaussKruger_zone_4 = 28404,
        ///<summary>Pulkovo 1942 GaussKruger zone 5</summary>
        Pulkovo_1942_GaussKruger_zone_5 = 28405,
        ///<summary>Pulkovo 1942 GaussKruger zone 6</summary>
        Pulkovo_1942_GaussKruger_zone_6 = 28406,
        ///<summary>Pulkovo 1942 GaussKruger zone 7</summary>
        Pulkovo_1942_GaussKruger_zone_7 = 28407,
        ///<summary>Pulkovo 1942 GaussKruger zone 8</summary>
        Pulkovo_1942_GaussKruger_zone_8 = 28408,
        ///<summary>Pulkovo 1942 GaussKruger zone 9</summary>
        Pulkovo_1942_GaussKruger_zone_9 = 28409,
        ///<summary>Pulkovo 1942 GaussKruger zone 10</summary>
        Pulkovo_1942_GaussKruger_zone_10 = 28410,
        ///<summary>Pulkovo 1942 GaussKruger zone 11</summary>
        Pulkovo_1942_GaussKruger_zone_11 = 28411,
        ///<summary>Pulkovo 1942 GaussKruger zone 12</summary>
        Pulkovo_1942_GaussKruger_zone_12 = 28412,
        ///<summary>Pulkovo 1942 GaussKruger zone 13</summary>
        Pulkovo_1942_GaussKruger_zone_13 = 28413,
        ///<summary>Pulkovo 1942 GaussKruger zone 14</summary>
        Pulkovo_1942_GaussKruger_zone_14 = 28414,
        ///<summary>Pulkovo 1942 GaussKruger zone 15</summary>
        Pulkovo_1942_GaussKruger_zone_15 = 28415,
        ///<summary>Pulkovo 1942 GaussKruger zone 16</summary>
        Pulkovo_1942_GaussKruger_zone_16 = 28416,
        ///<summary>Pulkovo 1942 GaussKruger zone 17</summary>
        Pulkovo_1942_GaussKruger_zone_17 = 28417,
        ///<summary>Pulkovo 1942 GaussKruger zone 18</summary>
        Pulkovo_1942_GaussKruger_zone_18 = 28418,
        ///<summary>Pulkovo 1942 GaussKruger zone 19</summary>
        Pulkovo_1942_GaussKruger_zone_19 = 28419,
        ///<summary>Pulkovo 1942 GaussKruger zone 20</summary>
        Pulkovo_1942_GaussKruger_zone_20 = 28420,
        ///<summary>Pulkovo 1942 GaussKruger zone 21</summary>
        Pulkovo_1942_GaussKruger_zone_21 = 28421,
        ///<summary>Pulkovo 1942 GaussKruger zone 22</summary>
        Pulkovo_1942_GaussKruger_zone_22 = 28422,
        ///<summary>Pulkovo 1942 GaussKruger zone 23</summary>
        Pulkovo_1942_GaussKruger_zone_23 = 28423,
        ///<summary>Pulkovo 1942 GaussKruger zone 24</summary>
        Pulkovo_1942_GaussKruger_zone_24 = 28424,
        ///<summary>Pulkovo 1942 GaussKruger zone 25</summary>
        Pulkovo_1942_GaussKruger_zone_25 = 28425,
        ///<summary>Pulkovo 1942 GaussKruger zone 26</summary>
        Pulkovo_1942_GaussKruger_zone_26 = 28426,
        ///<summary>Pulkovo 1942 GaussKruger zone 27</summary>
        Pulkovo_1942_GaussKruger_zone_27 = 28427,
        ///<summary>Pulkovo 1942 GaussKruger zone 28</summary>
        Pulkovo_1942_GaussKruger_zone_28 = 28428,
        ///<summary>Pulkovo 1942 GaussKruger zone 29</summary>
        Pulkovo_1942_GaussKruger_zone_29 = 28429,
        ///<summary>Pulkovo 1942 GaussKruger zone 30</summary>
        Pulkovo_1942_GaussKruger_zone_30 = 28430,
        ///<summary>Pulkovo 1942 GaussKruger zone 31</summary>
        Pulkovo_1942_GaussKruger_zone_31 = 28431,
        ///<summary>Pulkovo 1942 GaussKruger zone 32</summary>
        Pulkovo_1942_GaussKruger_zone_32 = 28432,
        ///<summary>Pulkovo 1942 GaussKruger 2N deprecated</summary>
        Pulkovo_1942_GaussKruger_2N_deprecated = 28462,
        ///<summary>Pulkovo 1942 GaussKruger 3N deprecated</summary>
        Pulkovo_1942_GaussKruger_3N_deprecated = 28463,
        ///<summary>Pulkovo 1942 GaussKruger 4N deprecated</summary>
        Pulkovo_1942_GaussKruger_4N_deprecated = 28464,
        ///<summary>Pulkovo 1942 GaussKruger 5N deprecated</summary>
        Pulkovo_1942_GaussKruger_5N_deprecated = 28465,
        ///<summary>Pulkovo 1942 GaussKruger 6N deprecated</summary>
        Pulkovo_1942_GaussKruger_6N_deprecated = 28466,
        ///<summary>Pulkovo 1942 GaussKruger 7N deprecated</summary>
        Pulkovo_1942_GaussKruger_7N_deprecated = 28467,
        ///<summary>Pulkovo 1942 GaussKruger 8N deprecated</summary>
        Pulkovo_1942_GaussKruger_8N_deprecated = 28468,
        ///<summary>Pulkovo 1942 GaussKruger 9N deprecated</summary>
        Pulkovo_1942_GaussKruger_9N_deprecated = 28469,
        ///<summary>Pulkovo 1942 GaussKruger 10N deprecated</summary>
        Pulkovo_1942_GaussKruger_10N_deprecated = 28470,
        ///<summary>Pulkovo 1942 GaussKruger 11N deprecated</summary>
        Pulkovo_1942_GaussKruger_11N_deprecated = 28471,
        ///<summary>Pulkovo 1942 GaussKruger 12N deprecated</summary>
        Pulkovo_1942_GaussKruger_12N_deprecated = 28472,
        ///<summary>Pulkovo 1942 GaussKruger 13N deprecated</summary>
        Pulkovo_1942_GaussKruger_13N_deprecated = 28473,
        ///<summary>Pulkovo 1942 GaussKruger 14N deprecated</summary>
        Pulkovo_1942_GaussKruger_14N_deprecated = 28474,
        ///<summary>Pulkovo 1942 GaussKruger 15N deprecated</summary>
        Pulkovo_1942_GaussKruger_15N_deprecated = 28475,
        ///<summary>Pulkovo 1942 GaussKruger 16N deprecated</summary>
        Pulkovo_1942_GaussKruger_16N_deprecated = 28476,
        ///<summary>Pulkovo 1942 GaussKruger 17N deprecated</summary>
        Pulkovo_1942_GaussKruger_17N_deprecated = 28477,
        ///<summary>Pulkovo 1942 GaussKruger 18N deprecated</summary>
        Pulkovo_1942_GaussKruger_18N_deprecated = 28478,
        ///<summary>Pulkovo 1942 GaussKruger 19N deprecated</summary>
        Pulkovo_1942_GaussKruger_19N_deprecated = 28479,
        ///<summary>Pulkovo 1942 GaussKruger 20N deprecated</summary>
        Pulkovo_1942_GaussKruger_20N_deprecated = 28480,
        ///<summary>Pulkovo 1942 GaussKruger 21N deprecated</summary>
        Pulkovo_1942_GaussKruger_21N_deprecated = 28481,
        ///<summary>Pulkovo 1942 GaussKruger 22N deprecated</summary>
        Pulkovo_1942_GaussKruger_22N_deprecated = 28482,
        ///<summary>Pulkovo 1942 GaussKruger 23N deprecated</summary>
        Pulkovo_1942_GaussKruger_23N_deprecated = 28483,
        ///<summary>Pulkovo 1942 GaussKruger 24N deprecated</summary>
        Pulkovo_1942_GaussKruger_24N_deprecated = 28484,
        ///<summary>Pulkovo 1942 GaussKruger 25N deprecated</summary>
        Pulkovo_1942_GaussKruger_25N_deprecated = 28485,
        ///<summary>Pulkovo 1942 GaussKruger 26N deprecated</summary>
        Pulkovo_1942_GaussKruger_26N_deprecated = 28486,
        ///<summary>Pulkovo 1942 GaussKruger 27N deprecated</summary>
        Pulkovo_1942_GaussKruger_27N_deprecated = 28487,
        ///<summary>Pulkovo 1942 GaussKruger 28N deprecated</summary>
        Pulkovo_1942_GaussKruger_28N_deprecated = 28488,
        ///<summary>Pulkovo 1942 GaussKruger 29N deprecated</summary>
        Pulkovo_1942_GaussKruger_29N_deprecated = 28489,
        ///<summary>Pulkovo 1942 GaussKruger 30N deprecated</summary>
        Pulkovo_1942_GaussKruger_30N_deprecated = 28490,
        ///<summary>Pulkovo 1942 GaussKruger 31N deprecated</summary>
        Pulkovo_1942_GaussKruger_31N_deprecated = 28491,
        ///<summary>Pulkovo 1942 GaussKruger 32N deprecated</summary>
        Pulkovo_1942_GaussKruger_32N_deprecated = 28492,
        ///<summary>Qatar 1974 Qatar National Grid</summary>
        Qatar_1974_Qatar_National_Grid = 28600,
        ///<summary>Amersfoort RD Old</summary>
        Amersfoort_RD_Old = 28991,
        ///<summary>SAD69 Brazil Polyconic deprecated</summary>
        SAD69_Brazil_Polyconic_deprecated = 29100,
        ///<summary>SAD69 UTM zone 18N deprecated</summary>
        SAD69_UTM_zone_18N_deprecated = 29118,
        ///<summary>SAD69 UTM zone 19N deprecated</summary>
        SAD69_UTM_zone_19N_deprecated = 29119,
        ///<summary>SAD69 UTM zone 20N deprecated</summary>
        SAD69_UTM_zone_20N_deprecated = 29120,
        ///<summary>SAD69 UTM zone 21N deprecated</summary>
        SAD69_UTM_zone_21N_deprecated = 29121,
        ///<summary>SAD69 UTM zone 22N deprecated</summary>
        SAD69_UTM_zone_22N_deprecated = 29122,
        ///<summary>SAD69 UTM zone 17S deprecated</summary>
        SAD69_UTM_zone_17S_deprecated = 29177,
        ///<summary>SAD69 UTM zone 18S deprecated</summary>
        SAD69_UTM_zone_18S_deprecated = 29178,
        ///<summary>SAD69 UTM zone 19S deprecated</summary>
        SAD69_UTM_zone_19S_deprecated = 29179,
        ///<summary>SAD69 UTM zone 20S deprecated</summary>
        SAD69_UTM_zone_20S_deprecated = 29180,
        ///<summary>SAD69 UTM zone 21S deprecated</summary>
        SAD69_UTM_zone_21S_deprecated = 29181,
        ///<summary>SAD69 UTM zone 22S deprecated</summary>
        SAD69_UTM_zone_22S_deprecated = 29182,
        ///<summary>SAD69 UTM zone 23S deprecated</summary>
        SAD69_UTM_zone_23S_deprecated = 29183,
        ///<summary>SAD69 UTM zone 24S deprecated</summary>
        SAD69_UTM_zone_24S_deprecated = 29184,
        ///<summary>SAD69 UTM zone 25S deprecated</summary>
        SAD69_UTM_zone_25S_deprecated = 29185,
        ///<summary>Sapper Hill 1943 UTM zone 20S</summary>
        Sapper_Hill_1943_UTM_zone_20S = 29220,
        ///<summary>Sapper Hill 1943 UTM zone 21S</summary>
        Sapper_Hill_1943_UTM_zone_21S = 29221,
        ///<summary>Schwarzeck UTM zone 33S</summary>
        Schwarzeck_UTM_zone_33S = 29333,
        ///<summary>Sudan UTM zone 35N deprecated</summary>
        Sudan_UTM_zone_35N_deprecated = 29635,
        ///<summary>Sudan UTM zone 36N deprecated</summary>
        Sudan_UTM_zone_36N_deprecated = 29636,
        ///<summary>Tananarive UTM zone 38S</summary>
        Tananarive_UTM_zone_38S = 29738,
        ///<summary>Tananarive UTM zone 39S</summary>
        Tananarive_UTM_zone_39S = 29739,
        ///<summary>Timbalai 1948 UTM zone 49N</summary>
        Timbalai_1948_UTM_zone_49N = 29849,
        ///<summary>Timbalai 1948 UTM zone 50N</summary>
        Timbalai_1948_UTM_zone_50N = 29850,
        ///<summary>Timbalai 1948 RSO Borneo ch</summary>
        Timbalai_1948_RSO_Borneo_ch = 29871,
        ///<summary>Timbalai 1948 RSO Borneo ft</summary>
        Timbalai_1948_RSO_Borneo_ft = 29872,
        ///<summary>Timbalai 1948 RSO Borneo m</summary>
        Timbalai_1948_RSO_Borneo_m = 29873,
        ///<summary>TM65 Irish National Grid deprecated</summary>
        TM65_Irish_National_Grid_deprecated = 29900,
        ///<summary>OSNI 1952 Irish National Grid</summary>
        OSNI_1952_Irish_National_Grid = 29901,
        ///<summary>TM75 Irish Grid</summary>
        TM75_Irish_Grid = 29903,
        ///<summary>Tokyo Japan Plane Rectangular CS I</summary>
        Tokyo_Japan_Plane_Rectangular_CS_I = 30161,
        ///<summary>Tokyo Japan Plane Rectangular CS II</summary>
        Tokyo_Japan_Plane_Rectangular_CS_II = 30162,
        ///<summary>Tokyo Japan Plane Rectangular CS III</summary>
        Tokyo_Japan_Plane_Rectangular_CS_III = 30163,
        ///<summary>Tokyo Japan Plane Rectangular CS IV</summary>
        Tokyo_Japan_Plane_Rectangular_CS_IV = 30164,
        ///<summary>Tokyo Japan Plane Rectangular CS V</summary>
        Tokyo_Japan_Plane_Rectangular_CS_V = 30165,
        ///<summary>Tokyo Japan Plane Rectangular CS VI</summary>
        Tokyo_Japan_Plane_Rectangular_CS_VI = 30166,
        ///<summary>Tokyo Japan Plane Rectangular CS VII</summary>
        Tokyo_Japan_Plane_Rectangular_CS_VII = 30167,
        ///<summary>Tokyo Japan Plane Rectangular CS VIII</summary>
        Tokyo_Japan_Plane_Rectangular_CS_VIII = 30168,
        ///<summary>Tokyo Japan Plane Rectangular CS IX</summary>
        Tokyo_Japan_Plane_Rectangular_CS_IX = 30169,
        ///<summary>Tokyo Japan Plane Rectangular CS X</summary>
        Tokyo_Japan_Plane_Rectangular_CS_X = 30170,
        ///<summary>Tokyo Japan Plane Rectangular CS XI</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XI = 30171,
        ///<summary>Tokyo Japan Plane Rectangular CS XII</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XII = 30172,
        ///<summary>Tokyo Japan Plane Rectangular CS XIII</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XIII = 30173,
        ///<summary>Tokyo Japan Plane Rectangular CS XIV</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XIV = 30174,
        ///<summary>Tokyo Japan Plane Rectangular CS XV</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XV = 30175,
        ///<summary>Tokyo Japan Plane Rectangular CS XVI</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XVI = 30176,
        ///<summary>Tokyo Japan Plane Rectangular CS XVII</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XVII = 30177,
        ///<summary>Tokyo Japan Plane Rectangular CS XVIII</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XVIII = 30178,
        ///<summary>Tokyo Japan Plane Rectangular CS XIX</summary>
        Tokyo_Japan_Plane_Rectangular_CS_XIX = 30179,
        ///<summary>Trinidad 1903 Trinidad Grid</summary>
        Trinidad_1903_Trinidad_Grid = 30200,
        ///<summary>TC1948 UTM zone 39N</summary>
        TC1948_UTM_zone_39N = 30339,
        ///<summary>TC1948 UTM zone 40N</summary>
        TC1948_UTM_zone_40N = 30340,
        ///<summary>Voirol 1875 Nord Algerie ancienne</summary>
        Voirol_1875_Nord_Algerie_ancienne = 30491,
        ///<summary>Voirol 1875 Sud Algerie ancienne</summary>
        Voirol_1875_Sud_Algerie_ancienne = 30492,
        ///<summary>Nord Sahara 1959 UTM zone 29N</summary>
        Nord_Sahara_1959_UTM_zone_29N = 30729,
        ///<summary>Nord Sahara 1959 UTM zone 30N</summary>
        Nord_Sahara_1959_UTM_zone_30N = 30730,
        ///<summary>Nord Sahara 1959 UTM zone 31N</summary>
        Nord_Sahara_1959_UTM_zone_31N = 30731,
        ///<summary>Nord Sahara 1959 UTM zone 32N</summary>
        Nord_Sahara_1959_UTM_zone_32N = 30732,
        ///<summary>RT38 25 gon W deprecated</summary>
        RT38_25_gon_W_deprecated = 30800,
        ///<summary>Yoff UTM zone 28N</summary>
        Yoff_UTM_zone_28N = 31028,
        ///<summary>Zanderij UTM zone 21N</summary>
        Zanderij_UTM_zone_21N = 31121,
        ///<summary>Zanderij TM 54 NW</summary>
        Zanderij_TM_54_NW = 31154,
        ///<summary>Zanderij Suriname Old TM</summary>
        Zanderij_Suriname_Old_TM = 31170,
        ///<summary>Zanderij Suriname TM</summary>
        Zanderij_Suriname_TM = 31171,
        ///<summary>MGI 3degree Gauss zone 5 deprecated</summary>
        MGI_3degree_Gauss_zone_5_deprecated = 31265,
        ///<summary>MGI 3degree Gauss zone 6 deprecated</summary>
        MGI_3degree_Gauss_zone_6_deprecated = 31266,
        ///<summary>MGI 3degree Gauss zone 7 deprecated</summary>
        MGI_3degree_Gauss_zone_7_deprecated = 31267,
        ///<summary>MGI 3degree Gauss zone 8 deprecated</summary>
        MGI_3degree_Gauss_zone_8_deprecated = 31268,
        ///<summary>MGI Ferro Austria West Zone</summary>
        MGI_Ferro_Austria_West_Zone = 31281,
        ///<summary>MGI Ferro Austria Central Zone</summary>
        MGI_Ferro_Austria_Central_Zone = 31282,
        ///<summary>MGI Ferro Austria East Zone</summary>
        MGI_Ferro_Austria_East_Zone = 31283,
        ///<summary>MGI Austria M28</summary>
        MGI_Austria_M28 = 31284,
        ///<summary>MGI Austria M31</summary>
        MGI_Austria_M31 = 31285,
        ///<summary>MGI Austria M34</summary>
        MGI_Austria_M34 = 31286,
        ///<summary>MGI Austria Lambert deprecated</summary>
        MGI_Austria_Lambert_deprecated = 31297,
        ///<summary>Belge 1972 Belgian Lambert 72</summary>
        Belge_1972_Belgian_Lambert_72 = 31370,
        ///<summary>DHDN 3degree Gauss zone 1 deprecated</summary>
        DHDN_3degree_Gauss_zone_1_deprecated = 31461,
        ///<summary>DHDN 3degree Gauss zone 2 deprecated</summary>
        DHDN_3degree_Gauss_zone_2_deprecated = 31462,
        ///<summary>DHDN 3degree Gauss zone 3 deprecated</summary>
        DHDN_3degree_Gauss_zone_3_deprecated = 31463,
        ///<summary>DHDN 3degree Gauss zone 4 deprecated</summary>
        DHDN_3degree_Gauss_zone_4_deprecated = 31464,
        ///<summary>DHDN 3degree Gauss zone 5 deprecated</summary>
        DHDN_3degree_Gauss_zone_5_deprecated = 31465,
        ///<summary>Conakry 1905 UTM zone 28N</summary>
        Conakry_1905_UTM_zone_28N = 31528,
        ///<summary>Conakry 1905 UTM zone 29N</summary>
        Conakry_1905_UTM_zone_29N = 31529,
        ///<summary>Dealul Piscului 1933  Stereo 33</summary>
        Dealul_Piscului_1933__Stereo_33 = 31600,
        ///<summary>Dealul Piscului 1970  Stereo 70</summary>
        Dealul_Piscului_1970__Stereo_70 = 31700,
        ///<summary>NGN UTM zone 38N</summary>
        NGN_UTM_zone_38N = 31838,
        ///<summary>NGN UTM zone 39N</summary>
        NGN_UTM_zone_39N = 31839,
        ///<summary>KUDAMS KTM deprecated</summary>
        KUDAMS_KTM_deprecated = 31900,
        ///<summary>SIRGAS 2000 UTM zone 17S</summary>
        SIRGAS_2000_UTM_zone_17S = 31977,
        ///<summary>SIRGAS 2000 UTM zone 18S</summary>
        SIRGAS_2000_UTM_zone_18S = 31978,
        ///<summary>SIRGAS 2000 UTM zone 19S</summary>
        SIRGAS_2000_UTM_zone_19S = 31979,
        ///<summary>SIRGAS 2000 UTM zone 20S</summary>
        SIRGAS_2000_UTM_zone_20S = 31980,
        ///<summary>SIRGAS 2000 UTM zone 21S</summary>
        SIRGAS_2000_UTM_zone_21S = 31981,
        ///<summary>SIRGAS 2000 UTM zone 22S</summary>
        SIRGAS_2000_UTM_zone_22S = 31982,
        ///<summary>SIRGAS 2000 UTM zone 23S</summary>
        SIRGAS_2000_UTM_zone_23S = 31983,
        ///<summary>SIRGAS 2000 UTM zone 24S</summary>
        SIRGAS_2000_UTM_zone_24S = 31984,
        ///<summary>SIRGAS 2000 UTM zone 25S</summary>
        SIRGAS_2000_UTM_zone_25S = 31985,
        ///<summary>NAD27 Montana North</summary>
        NAD27_Montana_North = 32001,
        ///<summary>NAD27 Montana Central</summary>
        NAD27_Montana_Central = 32002,
        ///<summary>NAD27 Montana South</summary>
        NAD27_Montana_South = 32003,
        ///<summary>NAD27 Nebraska North</summary>
        NAD27_Nebraska_North = 32005,
        ///<summary>NAD27 Nebraska South</summary>
        NAD27_Nebraska_South = 32006,
        ///<summary>NAD27 Nevada East</summary>
        NAD27_Nevada_East = 32007,
        ///<summary>NAD27 Nevada Central</summary>
        NAD27_Nevada_Central = 32008,
        ///<summary>NAD27 Nevada West</summary>
        NAD27_Nevada_West = 32009,
        ///<summary>NAD27 New Hampshire</summary>
        NAD27_New_Hampshire = 32010,
        ///<summary>NAD27 New Jersey</summary>
        NAD27_New_Jersey = 32011,
        ///<summary>NAD27 New Mexico East</summary>
        NAD27_New_Mexico_East = 32012,
        ///<summary>NAD27 New Mexico Central</summary>
        NAD27_New_Mexico_Central = 32013,
        ///<summary>NAD27 New Mexico West</summary>
        NAD27_New_Mexico_West = 32014,
        ///<summary>NAD27 New York East</summary>
        NAD27_New_York_East = 32015,
        ///<summary>NAD27 New York Central</summary>
        NAD27_New_York_Central = 32016,
        ///<summary>NAD27 New York West</summary>
        NAD27_New_York_West = 32017,
        ///<summary>NAD27 New York Long Island</summary>
        NAD27_New_York_Long_Island = 32018,
        ///<summary>NAD27 North Carolina</summary>
        NAD27_North_Carolina = 32019,
        ///<summary>NAD27 North Dakota North</summary>
        NAD27_North_Dakota_North = 32020,
        ///<summary>NAD27 North Dakota South</summary>
        NAD27_North_Dakota_South = 32021,
        ///<summary>NAD27 Ohio North</summary>
        NAD27_Ohio_North = 32022,
        ///<summary>NAD27 Ohio South</summary>
        NAD27_Ohio_South = 32023,
        ///<summary>NAD27 Oklahoma North</summary>
        NAD27_Oklahoma_North = 32024,
        ///<summary>NAD27 Oklahoma South</summary>
        NAD27_Oklahoma_South = 32025,
        ///<summary>NAD27 Oregon North</summary>
        NAD27_Oregon_North = 32026,
        ///<summary>NAD27 Oregon South</summary>
        NAD27_Oregon_South = 32027,
        ///<summary>NAD27 Pennsylvania North</summary>
        NAD27_Pennsylvania_North = 32028,
        ///<summary>NAD27 Pennsylvania South</summary>
        NAD27_Pennsylvania_South = 32029,
        ///<summary>NAD27 Rhode Island</summary>
        NAD27_Rhode_Island = 32030,
        ///<summary>NAD27 South Carolina North</summary>
        NAD27_South_Carolina_North = 32031,
        ///<summary>NAD27 South Carolina South</summary>
        NAD27_South_Carolina_South = 32033,
        ///<summary>NAD27 South Dakota North</summary>
        NAD27_South_Dakota_North = 32034,
        ///<summary>NAD27 South Dakota South</summary>
        NAD27_South_Dakota_South = 32035,
        ///<summary>NAD27 Tennessee deprecated</summary>
        NAD27_Tennessee_deprecated = 32036,
        ///<summary>NAD27 Texas North</summary>
        NAD27_Texas_North = 32037,
        ///<summary>NAD27 Texas North Central</summary>
        NAD27_Texas_North_Central = 32038,
        ///<summary>NAD27 Texas Central</summary>
        NAD27_Texas_Central = 32039,
        ///<summary>NAD27 Texas South Central</summary>
        NAD27_Texas_South_Central = 32040,
        ///<summary>NAD27 Texas South</summary>
        NAD27_Texas_South = 32041,
        ///<summary>NAD27 Utah North</summary>
        NAD27_Utah_North = 32042,
        ///<summary>NAD27 Utah Central</summary>
        NAD27_Utah_Central = 32043,
        ///<summary>NAD27 Utah South</summary>
        NAD27_Utah_South = 32044,
        ///<summary>NAD27 Vermont</summary>
        NAD27_Vermont = 32045,
        ///<summary>NAD27 Virginia North</summary>
        NAD27_Virginia_North = 32046,
        ///<summary>NAD27 Virginia South</summary>
        NAD27_Virginia_South = 32047,
        ///<summary>NAD27 Washington North</summary>
        NAD27_Washington_North = 32048,
        ///<summary>NAD27 Washington South</summary>
        NAD27_Washington_South = 32049,
        ///<summary>NAD27 West Virginia North</summary>
        NAD27_West_Virginia_North = 32050,
        ///<summary>NAD27 West Virginia South</summary>
        NAD27_West_Virginia_South = 32051,
        ///<summary>NAD27 Wisconsin North</summary>
        NAD27_Wisconsin_North = 32052,
        ///<summary>NAD27 Wisconsin Central</summary>
        NAD27_Wisconsin_Central = 32053,
        ///<summary>NAD27 Wisconsin South</summary>
        NAD27_Wisconsin_South = 32054,
        ///<summary>NAD27 Wyoming East</summary>
        NAD27_Wyoming_East = 32055,
        ///<summary>NAD27 Wyoming East Central</summary>
        NAD27_Wyoming_East_Central = 32056,
        ///<summary>NAD27 Wyoming West Central</summary>
        NAD27_Wyoming_West_Central = 32057,
        ///<summary>NAD27 Wyoming West</summary>
        NAD27_Wyoming_West = 32058,
        ///<summary>NAD27 Guatemala Norte</summary>
        NAD27_Guatemala_Norte = 32061,
        ///<summary>NAD27 Guatemala Sur</summary>
        NAD27_Guatemala_Sur = 32062,
        ///<summary>NAD27 BLM 14N feet deprecated</summary>
        NAD27_BLM_14N_feet_deprecated = 32074,
        ///<summary>NAD27 BLM 15N feet deprecated</summary>
        NAD27_BLM_15N_feet_deprecated = 32075,
        ///<summary>NAD27 BLM 16N feet deprecated</summary>
        NAD27_BLM_16N_feet_deprecated = 32076,
        ///<summary>NAD27 BLM 17N feet deprecated</summary>
        NAD27_BLM_17N_feet_deprecated = 32077,
        ///<summary>NAD27 MTM zone 1</summary>
        NAD27_MTM_zone_1 = 32081,
        ///<summary>NAD27 MTM zone 2</summary>
        NAD27_MTM_zone_2 = 32082,
        ///<summary>NAD27 MTM zone 3</summary>
        NAD27_MTM_zone_3 = 32083,
        ///<summary>NAD27 MTM zone 4</summary>
        NAD27_MTM_zone_4 = 32084,
        ///<summary>NAD27 MTM zone 5</summary>
        NAD27_MTM_zone_5 = 32085,
        ///<summary>NAD27 MTM zone 6</summary>
        NAD27_MTM_zone_6 = 32086,
        ///<summary>NAD27 Quebec Lambert</summary>
        NAD27_Quebec_Lambert = 32098,
        ///<summary>NAD83 Montana</summary>
        NAD83_Montana = 32100,
        ///<summary>NAD83 Nebraska</summary>
        NAD83_Nebraska = 32104,
        ///<summary>NAD83 Nevada East</summary>
        NAD83_Nevada_East = 32107,
        ///<summary>NAD83 Nevada Central</summary>
        NAD83_Nevada_Central = 32108,
        ///<summary>NAD83 Nevada West</summary>
        NAD83_Nevada_West = 32109,
        ///<summary>NAD83 New Hampshire</summary>
        NAD83_New_Hampshire = 32110,
        ///<summary>NAD83 New Jersey</summary>
        NAD83_New_Jersey = 32111,
        ///<summary>NAD83 New Mexico East</summary>
        NAD83_New_Mexico_East = 32112,
        ///<summary>NAD83 New Mexico Central</summary>
        NAD83_New_Mexico_Central = 32113,
        ///<summary>NAD83 New Mexico West</summary>
        NAD83_New_Mexico_West = 32114,
        ///<summary>NAD83 New York East</summary>
        NAD83_New_York_East = 32115,
        ///<summary>NAD83 New York Central</summary>
        NAD83_New_York_Central = 32116,
        ///<summary>NAD83 New York West</summary>
        NAD83_New_York_West = 32117,
        ///<summary>NAD83 New York Long Island</summary>
        NAD83_New_York_Long_Island = 32118,
        ///<summary>NAD83 North Carolina</summary>
        NAD83_North_Carolina = 32119,
        ///<summary>NAD83 North Dakota North</summary>
        NAD83_North_Dakota_North = 32120,
        ///<summary>NAD83 North Dakota South</summary>
        NAD83_North_Dakota_South = 32121,
        ///<summary>NAD83 Ohio North</summary>
        NAD83_Ohio_North = 32122,
        ///<summary>NAD83 Ohio South</summary>
        NAD83_Ohio_South = 32123,
        ///<summary>NAD83 Oklahoma North</summary>
        NAD83_Oklahoma_North = 32124,
        ///<summary>NAD83 Oklahoma South</summary>
        NAD83_Oklahoma_South = 32125,
        ///<summary>NAD83 Oregon North</summary>
        NAD83_Oregon_North = 32126,
        ///<summary>NAD83 Oregon South</summary>
        NAD83_Oregon_South = 32127,
        ///<summary>NAD83 Pennsylvania North</summary>
        NAD83_Pennsylvania_North = 32128,
        ///<summary>NAD83 Pennsylvania South</summary>
        NAD83_Pennsylvania_South = 32129,
        ///<summary>NAD83 Rhode Island</summary>
        NAD83_Rhode_Island = 32130,
        ///<summary>NAD83 South Carolina</summary>
        NAD83_South_Carolina = 32133,
        ///<summary>NAD83 South Dakota North</summary>
        NAD83_South_Dakota_North = 32134,
        ///<summary>NAD83 South Dakota South</summary>
        NAD83_South_Dakota_South = 32135,
        ///<summary>NAD83 Tennessee</summary>
        NAD83_Tennessee = 32136,
        ///<summary>NAD83 Texas North</summary>
        NAD83_Texas_North = 32137,
        ///<summary>NAD83 Texas North Central</summary>
        NAD83_Texas_North_Central = 32138,
        ///<summary>NAD83 Texas Central</summary>
        NAD83_Texas_Central = 32139,
        ///<summary>NAD83 Texas South Central</summary>
        NAD83_Texas_South_Central = 32140,
        ///<summary>NAD83 Texas South</summary>
        NAD83_Texas_South = 32141,
        ///<summary>NAD83 Utah North</summary>
        NAD83_Utah_North = 32142,
        ///<summary>NAD83 Utah Central</summary>
        NAD83_Utah_Central = 32143,
        ///<summary>NAD83 Utah South</summary>
        NAD83_Utah_South = 32144,
        ///<summary>NAD83 Vermont</summary>
        NAD83_Vermont = 32145,
        ///<summary>NAD83 Virginia North</summary>
        NAD83_Virginia_North = 32146,
        ///<summary>NAD83 Virginia South</summary>
        NAD83_Virginia_South = 32147,
        ///<summary>NAD83 Washington North</summary>
        NAD83_Washington_North = 32148,
        ///<summary>NAD83 Washington South</summary>
        NAD83_Washington_South = 32149,
        ///<summary>NAD83 West Virginia North</summary>
        NAD83_West_Virginia_North = 32150,
        ///<summary>NAD83 West Virginia South</summary>
        NAD83_West_Virginia_South = 32151,
        ///<summary>NAD83 Wisconsin North</summary>
        NAD83_Wisconsin_North = 32152,
        ///<summary>NAD83 Wisconsin Central</summary>
        NAD83_Wisconsin_Central = 32153,
        ///<summary>NAD83 Wisconsin South</summary>
        NAD83_Wisconsin_South = 32154,
        ///<summary>NAD83 Wyoming East</summary>
        NAD83_Wyoming_East = 32155,
        ///<summary>NAD83 Wyoming East Central</summary>
        NAD83_Wyoming_East_Central = 32156,
        ///<summary>NAD83 Wyoming West Central</summary>
        NAD83_Wyoming_West_Central = 32157,
        ///<summary>NAD83 Wyoming West</summary>
        NAD83_Wyoming_West = 32158,
        ///<summary>NAD83 Puerto Rico Virgin Is</summary>
        NAD83_Puerto_Rico_Virgin_Is = 32161,
        ///<summary>NAD83 SCoPQ zone 2</summary>
        NAD83_SCoPQ_zone_2 = 32180,
        ///<summary>NAD83 MTM zone 1</summary>
        NAD83_MTM_zone_1 = 32181,
        ///<summary>NAD83 MTM zone 2</summary>
        NAD83_MTM_zone_2 = 32182,
        ///<summary>NAD83 MTM zone 3</summary>
        NAD83_MTM_zone_3 = 32183,
        ///<summary>NAD83 MTM zone 4</summary>
        NAD83_MTM_zone_4 = 32184,
        ///<summary>NAD83 MTM zone 5</summary>
        NAD83_MTM_zone_5 = 32185,
        ///<summary>NAD83 MTM zone 6</summary>
        NAD83_MTM_zone_6 = 32186,
        ///<summary>NAD83 MTM zone 7</summary>
        NAD83_MTM_zone_7 = 32187,
        ///<summary>NAD83 MTM zone 8</summary>
        NAD83_MTM_zone_8 = 32188,
        ///<summary>NAD83 MTM zone 9</summary>
        NAD83_MTM_zone_9 = 32189,
        ///<summary>NAD83 MTM zone 10</summary>
        NAD83_MTM_zone_10 = 32190,
        ///<summary>NAD83 MTM zone 11</summary>
        NAD83_MTM_zone_11 = 32191,
        ///<summary>NAD83 MTM zone 12</summary>
        NAD83_MTM_zone_12 = 32192,
        ///<summary>NAD83 MTM zone 13</summary>
        NAD83_MTM_zone_13 = 32193,
        ///<summary>NAD83 MTM zone 14</summary>
        NAD83_MTM_zone_14 = 32194,
        ///<summary>NAD83 MTM zone 15</summary>
        NAD83_MTM_zone_15 = 32195,
        ///<summary>NAD83 MTM zone 16</summary>
        NAD83_MTM_zone_16 = 32196,
        ///<summary>NAD83 MTM zone 17</summary>
        NAD83_MTM_zone_17 = 32197,
        ///<summary>NAD83 Quebec Lambert</summary>
        NAD83_Quebec_Lambert = 32198,
        ///<summary>WGS 72 UTM zone 1N</summary>
        WGS_72_UTM_zone_1N = 32201,
        ///<summary>WGS 72 UTM zone 2N</summary>
        WGS_72_UTM_zone_2N = 32202,
        ///<summary>WGS 72 UTM zone 3N</summary>
        WGS_72_UTM_zone_3N = 32203,
        ///<summary>WGS 72 UTM zone 4N</summary>
        WGS_72_UTM_zone_4N = 32204,
        ///<summary>WGS 72 UTM zone 5N</summary>
        WGS_72_UTM_zone_5N = 32205,
        ///<summary>WGS 72 UTM zone 6N</summary>
        WGS_72_UTM_zone_6N = 32206,
        ///<summary>WGS 72 UTM zone 7N</summary>
        WGS_72_UTM_zone_7N = 32207,
        ///<summary>WGS 72 UTM zone 8N</summary>
        WGS_72_UTM_zone_8N = 32208,
        ///<summary>WGS 72 UTM zone 9N</summary>
        WGS_72_UTM_zone_9N = 32209,
        ///<summary>WGS 72 UTM zone 29N</summary>
        WGS_72_UTM_zone_29N = 32229,
        ///<summary>WGS 72 UTM zone 30N</summary>
        WGS_72_UTM_zone_30N = 32230,
        ///<summary>WGS 72 UTM zone 31N</summary>
        WGS_72_UTM_zone_31N = 32231,
        ///<summary>WGS 72 UTM zone 32N</summary>
        WGS_72_UTM_zone_32N = 32232,
        ///<summary>WGS 72 UTM zone 33N</summary>
        WGS_72_UTM_zone_33N = 32233,
        ///<summary>WGS 72 UTM zone 34N</summary>
        WGS_72_UTM_zone_34N = 32234,
        ///<summary>WGS 72 UTM zone 35N</summary>
        WGS_72_UTM_zone_35N = 32235,
        ///<summary>WGS 72 UTM zone 36N</summary>
        WGS_72_UTM_zone_36N = 32236,
        ///<summary>WGS 72 UTM zone 37N</summary>
        WGS_72_UTM_zone_37N = 32237,
        ///<summary>WGS 72 UTM zone 38N</summary>
        WGS_72_UTM_zone_38N = 32238,
        ///<summary>WGS 72 UTM zone 39N</summary>
        WGS_72_UTM_zone_39N = 32239,
        ///<summary>WGS 72 UTM zone 40N</summary>
        WGS_72_UTM_zone_40N = 32240,
        ///<summary>WGS 72 UTM zone 41N</summary>
        WGS_72_UTM_zone_41N = 32241,
        ///<summary>WGS 72 UTM zone 42N</summary>
        WGS_72_UTM_zone_42N = 32242,
        ///<summary>WGS 72 UTM zone 43N</summary>
        WGS_72_UTM_zone_43N = 32243,
        ///<summary>WGS 72 UTM zone 44N</summary>
        WGS_72_UTM_zone_44N = 32244,
        ///<summary>WGS 72 UTM zone 45N</summary>
        WGS_72_UTM_zone_45N = 32245,
        ///<summary>WGS 72 UTM zone 46N</summary>
        WGS_72_UTM_zone_46N = 32246,
        ///<summary>WGS 72 UTM zone 47N</summary>
        WGS_72_UTM_zone_47N = 32247,
        ///<summary>WGS 72 UTM zone 48N</summary>
        WGS_72_UTM_zone_48N = 32248,
        ///<summary>WGS 72 UTM zone 49N</summary>
        WGS_72_UTM_zone_49N = 32249,
        ///<summary>WGS 72 UTM zone 50N</summary>
        WGS_72_UTM_zone_50N = 32250,
        ///<summary>WGS 72 UTM zone 51N</summary>
        WGS_72_UTM_zone_51N = 32251,
        ///<summary>WGS 72 UTM zone 52N</summary>
        WGS_72_UTM_zone_52N = 32252,
        ///<summary>WGS 72 UTM zone 53N</summary>
        WGS_72_UTM_zone_53N = 32253,
        ///<summary>WGS 72 UTM zone 54N</summary>
        WGS_72_UTM_zone_54N = 32254,
        ///<summary>WGS 72 UTM zone 55N</summary>
        WGS_72_UTM_zone_55N = 32255,
        ///<summary>WGS 72 UTM zone 56N</summary>
        WGS_72_UTM_zone_56N = 32256,
        ///<summary>WGS 72 UTM zone 57N</summary>
        WGS_72_UTM_zone_57N = 32257,
        ///<summary>WGS 72 UTM zone 58N</summary>
        WGS_72_UTM_zone_58N = 32258,
        ///<summary>WGS 72 UTM zone 59N</summary>
        WGS_72_UTM_zone_59N = 32259,
        ///<summary>WGS 72 UTM zone 60N</summary>
        WGS_72_UTM_zone_60N = 32260,
        ///<summary>WGS 72 UTM zone 1S</summary>
        WGS_72_UTM_zone_1S = 32301,
        ///<summary>WGS 72 UTM zone 2S</summary>
        WGS_72_UTM_zone_2S = 32302,
        ///<summary>WGS 72 UTM zone 3S</summary>
        WGS_72_UTM_zone_3S = 32303,
        ///<summary>WGS 72 UTM zone 4S</summary>
        WGS_72_UTM_zone_4S = 32304,
        ///<summary>WGS 72 UTM zone 5S</summary>
        WGS_72_UTM_zone_5S = 32305,
        ///<summary>WGS 72 UTM zone 6S</summary>
        WGS_72_UTM_zone_6S = 32306,
        ///<summary>WGS 72 UTM zone 7S</summary>
        WGS_72_UTM_zone_7S = 32307,
        ///<summary>WGS 72 UTM zone 8S</summary>
        WGS_72_UTM_zone_8S = 32308,
        ///<summary>WGS 72 UTM zone 9S</summary>
        WGS_72_UTM_zone_9S = 32309,
        ///<summary>WGS 72 UTM zone 10S</summary>
        WGS_72_UTM_zone_10S = 32310,
        ///<summary>WGS 72 UTM zone 11S</summary>
        WGS_72_UTM_zone_11S = 32311,
        ///<summary>WGS 72 UTM zone 12S</summary>
        WGS_72_UTM_zone_12S = 32312,
        ///<summary>WGS 72 UTM zone 13S</summary>
        WGS_72_UTM_zone_13S = 32313,
        ///<summary>WGS 72 UTM zone 14S</summary>
        WGS_72_UTM_zone_14S = 32314,
        ///<summary>WGS 72 UTM zone 15S</summary>
        WGS_72_UTM_zone_15S = 32315,
        ///<summary>WGS 72 UTM zone 16S</summary>
        WGS_72_UTM_zone_16S = 32316,
        ///<summary>WGS 72 UTM zone 17S</summary>
        WGS_72_UTM_zone_17S = 32317,
        ///<summary>WGS 72 UTM zone 18S</summary>
        WGS_72_UTM_zone_18S = 32318,
        ///<summary>WGS 72 UTM zone 19S</summary>
        WGS_72_UTM_zone_19S = 32319,
        ///<summary>WGS 72 UTM zone 20S</summary>
        WGS_72_UTM_zone_20S = 32320,
        ///<summary>WGS 72 UTM zone 21S</summary>
        WGS_72_UTM_zone_21S = 32321,
        ///<summary>WGS 72 UTM zone 22S</summary>
        WGS_72_UTM_zone_22S = 32322,
        ///<summary>WGS 72 UTM zone 23S</summary>
        WGS_72_UTM_zone_23S = 32323,
        ///<summary>WGS 72 UTM zone 24S</summary>
        WGS_72_UTM_zone_24S = 32324,
        ///<summary>WGS 72 UTM zone 25S</summary>
        WGS_72_UTM_zone_25S = 32325,
        ///<summary>WGS 72 UTM zone 26S</summary>
        WGS_72_UTM_zone_26S = 32326,
        ///<summary>WGS 72 UTM zone 27S</summary>
        WGS_72_UTM_zone_27S = 32327,
        ///<summary>WGS 72 UTM zone 28S</summary>
        WGS_72_UTM_zone_28S = 32328,
        ///<summary>WGS 72 UTM zone 29S</summary>
        WGS_72_UTM_zone_29S = 32329,
        ///<summary>WGS 72 UTM zone 30S</summary>
        WGS_72_UTM_zone_30S = 32330,
        ///<summary>WGS 72 UTM zone 31S</summary>
        WGS_72_UTM_zone_31S = 32331,
        ///<summary>WGS 72 UTM zone 32S</summary>
        WGS_72_UTM_zone_32S = 32332,
        ///<summary>WGS 72 UTM zone 33S</summary>
        WGS_72_UTM_zone_33S = 32333,
        ///<summary>WGS 72 UTM zone 34S</summary>
        WGS_72_UTM_zone_34S = 32334,
        ///<summary>WGS 72 UTM zone 35S</summary>
        WGS_72_UTM_zone_35S = 32335,
        ///<summary>WGS 72 UTM zone 36S</summary>
        WGS_72_UTM_zone_36S = 32336,
        ///<summary>WGS 72 UTM zone 37S</summary>
        WGS_72_UTM_zone_37S = 32337,
        ///<summary>WGS 72 UTM zone 38S</summary>
        WGS_72_UTM_zone_38S = 32338,
        ///<summary>WGS 72 UTM zone 39S</summary>
        WGS_72_UTM_zone_39S = 32339,
        ///<summary>WGS 72 UTM zone 40S</summary>
        WGS_72_UTM_zone_40S = 32340,
        ///<summary>WGS 72 UTM zone 41S</summary>
        WGS_72_UTM_zone_41S = 32341,
        ///<summary>WGS 72 UTM zone 42S</summary>
        WGS_72_UTM_zone_42S = 32342,
        ///<summary>WGS 72 UTM zone 43S</summary>
        WGS_72_UTM_zone_43S = 32343,
        ///<summary>WGS 72 UTM zone 44S</summary>
        WGS_72_UTM_zone_44S = 32344,
        ///<summary>WGS 72 UTM zone 45S</summary>
        WGS_72_UTM_zone_45S = 32345,
        ///<summary>WGS 72 UTM zone 46S</summary>
        WGS_72_UTM_zone_46S = 32346,
        ///<summary>WGS 72 UTM zone 47S</summary>
        WGS_72_UTM_zone_47S = 32347,
        ///<summary>WGS 72 UTM zone 48S</summary>
        WGS_72_UTM_zone_48S = 32348,
        ///<summary>WGS 72 UTM zone 49S</summary>
        WGS_72_UTM_zone_49S = 32349,
        ///<summary>WGS 72 UTM zone 50S</summary>
        WGS_72_UTM_zone_50S = 32350,
        ///<summary>WGS 72 UTM zone 51S</summary>
        WGS_72_UTM_zone_51S = 32351,
        ///<summary>WGS 72 UTM zone 52S</summary>
        WGS_72_UTM_zone_52S = 32352,
        ///<summary>WGS 72 UTM zone 53S</summary>
        WGS_72_UTM_zone_53S = 32353,
        ///<summary>WGS 72 UTM zone 54S</summary>
        WGS_72_UTM_zone_54S = 32354,
        ///<summary>WGS 72 UTM zone 55S</summary>
        WGS_72_UTM_zone_55S = 32355,
        ///<summary>WGS 72 UTM zone 56S</summary>
        WGS_72_UTM_zone_56S = 32356,
        ///<summary>WGS 72 UTM zone 57S</summary>
        WGS_72_UTM_zone_57S = 32357,
        ///<summary>WGS 72 UTM zone 58S</summary>
        WGS_72_UTM_zone_58S = 32358,
        ///<summary>WGS 72 UTM zone 59S</summary>
        WGS_72_UTM_zone_59S = 32359,
        ///<summary>WGS 72 UTM zone 60S</summary>
        WGS_72_UTM_zone_60S = 32360,
        ///<summary>WGS 84 UPS North</summary>
        WGS_84_UPS_North = 32661,
        ///<summary>WGS 84 TM 36 SE</summary>
        WGS_84_TM_36_SE = 32766,
        ///<summary>Canada Albers Equal Area Conic</summary>
        Canada_Albers_Equal_Area_Conic = 102001,
        ///<summary>Canada Lambert Conformal Conic</summary>
        Canada_Lambert_Conformal_Conic = 102002,
        ///<summary>USA Contiguous Albers Equal Area Conic</summary>
        USA_Contiguous_Albers_Equal_Area_Conic = 102003,
        ///<summary>USA Contiguous Lambert Conformal Conic</summary>
        USA_Contiguous_Lambert_Conformal_Conic = 102004,
        ///<summary>USA Contiguous Equidistant Conic</summary>
        USA_Contiguous_Equidistant_Conic = 102005,
        ///<summary>Alaska Albers Equal Area Conic</summary>
        Alaska_Albers_Equal_Area_Conic = 102006,
        ///<summary>Hawaii Albers Equal Area Conic</summary>
        Hawaii_Albers_Equal_Area_Conic = 102007,
        ///<summary>North America Albers Equal Area Conic</summary>
        North_America_Albers_Equal_Area_Conic = 102008,
        ///<summary>North America Lambert Conformal Conic</summary>
        North_America_Lambert_Conformal_Conic = 102009,
        ///<summary>North America Equidistant Conic</summary>
        North_America_Equidistant_Conic = 102010,
        ///<summary>Africa Sinusoidal</summary>
        Africa_Sinusoidal = 102011,
        ///<summary>Asia Lambert Conformal Conic</summary>
        Asia_Lambert_Conformal_Conic = 102012,
        ///<summary>Europe Albers Equal Area Conic</summary>
        Europe_Albers_Equal_Area_Conic = 102013,
        ///<summary>Europe Lambert Conformal Conic</summary>
        Europe_Lambert_Conformal_Conic = 102014,
        ///<summary>South America Lambert Conformal Conic</summary>
        South_America_Lambert_Conformal_Conic = 102015,
        ///<summary>North Pole Azimuthal Equidistant</summary>
        North_Pole_Azimuthal_Equidistant = 102016,
        ///<summary>North Pole Lambert Azimuthal Equal Area</summary>
        North_Pole_Lambert_Azimuthal_Equal_Area = 102017,
        ///<summary>North Pole Stereographic</summary>
        North_Pole_Stereographic = 102018,
        ///<summary>South Pole Azimuthal Equidistant</summary>
        South_Pole_Azimuthal_Equidistant = 102019,
        ///<summary>South Pole Lambert Azimuthal Equal Area</summary>
        South_Pole_Lambert_Azimuthal_Equal_Area = 102020,
        ///<summary>South Pole Stereographic</summary>
        South_Pole_Stereographic = 102021,
        ///<summary>Africa Albers Equal Area Conic</summary>
        Africa_Albers_Equal_Area_Conic = 102022,
        ///<summary>Africa Equidistant Conic</summary>
        Africa_Equidistant_Conic = 102023,
        ///<summary>Africa Lambert Conformal Conic</summary>
        Africa_Lambert_Conformal_Conic = 102024,
        ///<summary>Asia North Albers Equal Area Conic</summary>
        Asia_North_Albers_Equal_Area_Conic = 102025,
        ///<summary>Asia North Equidistant Conic</summary>
        Asia_North_Equidistant_Conic = 102026,
        ///<summary>Asia North Lambert Conformal Conic</summary>
        Asia_North_Lambert_Conformal_Conic = 102027,
        ///<summary>Asia South Albers Equal Area Conic</summary>
        Asia_South_Albers_Equal_Area_Conic = 102028,
        ///<summary>Asia South Equidistant Conic</summary>
        Asia_South_Equidistant_Conic = 102029,
        ///<summary>Asia South Lambert Conformal Conic</summary>
        Asia_South_Lambert_Conformal_Conic = 102030,
        ///<summary>Europe Equidistant Conic</summary>
        Europe_Equidistant_Conic = 102031,
        ///<summary>South America Equidistant Conic</summary>
        South_America_Equidistant_Conic = 102032,
        ///<summary>South America Albers Equal Area Conic</summary>
        South_America_Albers_Equal_Area_Conic = 102033,
        ///<summary>SJTSK Krovak</summary>
        SJTSK_Krovak = 102065,
        ///<summary>SJTSK Ferro Krovak East North</summary>
        SJTSK_Ferro_Krovak_East_North = 102066,
        ///<summary>SJTSK Krovak East North</summary>
        SJTSK_Krovak_East_North = 102067,
        ///<summary>NGO 1948 Norway Zone 1</summary>
        NGO_1948_Norway_Zone_1 = 102101,
        ///<summary>NGO 1948 Norway Zone 2</summary>
        NGO_1948_Norway_Zone_2 = 102102,
        ///<summary>NGO 1948 Norway Zone 3</summary>
        NGO_1948_Norway_Zone_3 = 102103,
        ///<summary>NGO 1948 Norway Zone 4</summary>
        NGO_1948_Norway_Zone_4 = 102104,
        ///<summary>NGO 1948 Norway Zone 5</summary>
        NGO_1948_Norway_Zone_5 = 102105,
        ///<summary>NGO 1948 Norway Zone 6</summary>
        NGO_1948_Norway_Zone_6 = 102106,
        ///<summary>NGO 1948 Norway Zone 7</summary>
        NGO_1948_Norway_Zone_7 = 102107,
        ///<summary>NGO 1948 Norway Zone 8</summary>
        NGO_1948_Norway_Zone_8 = 102108,
        ///<summary>Old Hawaiian UTM Zone 4N</summary>
        Old_Hawaiian_UTM_Zone_4N = 102114,
        ///<summary>Old Hawaiian UTM Zone 5N</summary>
        Old_Hawaiian_UTM_Zone_5N = 102115,
        ///<summary>NAD 1927 Michigan GeoRef Feet US</summary>
        NAD_1927_Michigan_GeoRef_Feet_US = 102120,
        ///<summary>NAD 1983 Michigan GeoRef Feet US</summary>
        NAD_1983_Michigan_GeoRef_Feet_US = 102121,
        ///<summary>NAD 1927 Michigan GeoRef Meters</summary>
        NAD_1927_Michigan_GeoRef_Meters = 102122,
        ///<summary>NAD 1983 Michigan GeoRef Meters</summary>
        NAD_1983_Michigan_GeoRef_Meters = 102123,
        ///<summary>NGO 1948 UTM Zone 32N</summary>
        NGO_1948_UTM_Zone_32N = 102132,
        ///<summary>NGO 1948 UTM Zone 33N</summary>
        NGO_1948_UTM_Zone_33N = 102133,
        ///<summary>NGO 1948 UTM Zone 34N</summary>
        NGO_1948_UTM_Zone_34N = 102134,
        ///<summary>NGO 1948 UTM Zone 35N</summary>
        NGO_1948_UTM_Zone_35N = 102135,
        ///<summary>Hong Kong 1980 UTM Zone 49N</summary>
        Hong_Kong_1980_UTM_Zone_49N = 102141,
        ///<summary>Hong Kong 1980 UTM Zone 50N</summary>
        Hong_Kong_1980_UTM_Zone_50N = 102142,
        ///<summary>Tokyo UTM Zone 51N</summary>
        Tokyo_UTM_Zone_51N_ESRI = 102151,
        ///<summary>Tokyo UTM Zone 52N</summary>
        Tokyo_UTM_Zone_52N_ESRI = 102152,
        ///<summary>Tokyo UTM Zone 53N</summary>
        Tokyo_UTM_Zone_53N_ESRI = 102153,
        ///<summary>Tokyo UTM Zone 54N</summary>
        Tokyo_UTM_Zone_54N_ESRI = 102154,
        ///<summary>Tokyo UTM Zone 55N</summary>
        Tokyo_UTM_Zone_55N_ESRI = 102155,
        ///<summary>Tokyo UTM Zone 56N</summary>
        Tokyo_UTM_Zone_56N_ESRI = 102156,
        ///<summary>Datum 73 Hayford Gauss IGeoE</summary>
        Datum_73_Hayford_Gauss_IGeoE = 102160,
        ///<summary>Datum 73 Hayford Gauss IPCC</summary>
        Datum_73_Hayford_Gauss_IPCC = 102161,
        ///<summary>Graciosa Base SW 1948 UTM Zone 26N</summary>
        Graciosa_Base_SW_1948_UTM_Zone_26N = 102162,
        ///<summary>Lisboa Bessel Bonne</summary>
        Lisboa_Bessel_Bonne = 102163,
        ///<summary>Lisboa Hayford Gauss IGeoE</summary>
        Lisboa_Hayford_Gauss_IGeoE = 102164,
        ///<summary>Lisboa Hayford Gauss IPCC</summary>
        Lisboa_Hayford_Gauss_IPCC = 102165,
        ///<summary>Observ Meteorologico 1939 UTM Zone 25N</summary>
        Observ_Meteorologico_1939_UTM_Zone_25N = 102166,
        ///<summary>Sao Braz UTM Zone 26N</summary>
        Sao_Braz_UTM_Zone_26N = 102168,
        ///<summary>Selvagem Grande 1938 UTM Zone 28N</summary>
        Selvagem_Grande_1938_UTM_Zone_28N = 102169,
        ///<summary>Nord Maroc Degree</summary>
        Nord_Maroc_Degree = 102191,
        ///<summary>Sud Maroc Degree</summary>
        Sud_Maroc_Degree = 102192,
        ///<summary>Sahara Degree</summary>
        Sahara_Degree = 102193,
        ///<summary>Nord Algerie Ancienne Degree</summary>
        Nord_Algerie_Ancienne_Degree = 102491,
        ///<summary>Sud Algerie Ancienne Degree</summary>
        Sud_Algerie_Ancienne_Degree = 102492,
        ///<summary>NTF France I degrees</summary>
        NTF_France_I_degrees = 102581,
        ///<summary>NTF France II degrees</summary>
        NTF_France_II_degrees = 102582,
        ///<summary>NTF France III degrees</summary>
        NTF_France_III_degrees = 102583,
        ///<summary>NTF France IV degrees</summary>
        NTF_France_IV_degrees = 102584,
        ///<summary>Nord Algerie Degree</summary>
        Nord_Algerie_Degree = 102591,
        ///<summary>Sud Algerie Degree</summary>
        Sud_Algerie_Degree = 102592,
        ///<summary>NAD 1983 StatePlane Alabama East FIPS 0101 Feet</summary>
        NAD_1983_StatePlane_Alabama_East_FIPS_0101_Feet = 102629,
        ///<summary>NAD 1983 StatePlane Alabama West FIPS 0102 Feet</summary>
        NAD_1983_StatePlane_Alabama_West_FIPS_0102_Feet = 102630,
        ///<summary>NAD 1983 StatePlane Alaska 1 FIPS 5001 Feet</summary>
        NAD_1983_StatePlane_Alaska_1_FIPS_5001_Feet = 102631,
        ///<summary>NAD 1983 StatePlane Alaska 2 FIPS 5002 Feet</summary>
        NAD_1983_StatePlane_Alaska_2_FIPS_5002_Feet = 102632,
        ///<summary>NAD 1983 StatePlane Alaska 3 FIPS 5003 Feet</summary>
        NAD_1983_StatePlane_Alaska_3_FIPS_5003_Feet = 102633,
        ///<summary>NAD 1983 StatePlane Alaska 4 FIPS 5004 Feet</summary>
        NAD_1983_StatePlane_Alaska_4_FIPS_5004_Feet = 102634,
        ///<summary>NAD 1983 StatePlane Alaska 5 FIPS 5005 Feet</summary>
        NAD_1983_StatePlane_Alaska_5_FIPS_5005_Feet = 102635,
        ///<summary>NAD 1983 StatePlane Alaska 6 FIPS 5006 Feet</summary>
        NAD_1983_StatePlane_Alaska_6_FIPS_5006_Feet = 102636,
        ///<summary>NAD 1983 StatePlane Alaska 7 FIPS 5007 Feet</summary>
        NAD_1983_StatePlane_Alaska_7_FIPS_5007_Feet = 102637,
        ///<summary>NAD 1983 StatePlane Alaska 8 FIPS 5008 Feet</summary>
        NAD_1983_StatePlane_Alaska_8_FIPS_5008_Feet = 102638,
        ///<summary>NAD 1983 StatePlane Alaska 9 FIPS 5009 Feet</summary>
        NAD_1983_StatePlane_Alaska_9_FIPS_5009_Feet = 102639,
        ///<summary>NAD 1983 StatePlane Alaska 10 FIPS 5010 Feet</summary>
        NAD_1983_StatePlane_Alaska_10_FIPS_5010_Feet = 102640,
        ///<summary>NAD 1983 StatePlane Arizona East FIPS 0201 Feet</summary>
        NAD_1983_StatePlane_Arizona_East_FIPS_0201_Feet = 102648,
        ///<summary>NAD 1983 StatePlane Arizona Central FIPS 0202 Feet</summary>
        NAD_1983_StatePlane_Arizona_Central_FIPS_0202_Feet = 102649,
        ///<summary>NAD 1983 StatePlane Arizona West FIPS 0203 Feet</summary>
        NAD_1983_StatePlane_Arizona_West_FIPS_0203_Feet = 102650,
        ///<summary>NAD 1983 StatePlane Arkansas North FIPS 0301 Feet</summary>
        NAD_1983_StatePlane_Arkansas_North_FIPS_0301_Feet = 102651,
        ///<summary>NAD 1983 StatePlane Arkansas South FIPS 0302 Feet</summary>
        NAD_1983_StatePlane_Arkansas_South_FIPS_0302_Feet = 102652,
        ///<summary>NAD 1983 StatePlane Hawaii 1 FIPS 5101 Feet</summary>
        NAD_1983_StatePlane_Hawaii_1_FIPS_5101_Feet = 102661,
        ///<summary>NAD 1983 StatePlane Hawaii 2 FIPS 5102 Feet</summary>
        NAD_1983_StatePlane_Hawaii_2_FIPS_5102_Feet = 102662,
        ///<summary>NAD 1983 StatePlane Hawaii 3 FIPS 5103 Feet</summary>
        NAD_1983_StatePlane_Hawaii_3_FIPS_5103_Feet = 102663,
        ///<summary>NAD 1983 StatePlane Hawaii 4 FIPS 5104 Feet</summary>
        NAD_1983_StatePlane_Hawaii_4_FIPS_5104_Feet = 102664,
        ///<summary>NAD 1983 StatePlane Hawaii 5 FIPS 5105 Feet</summary>
        NAD_1983_StatePlane_Hawaii_5_FIPS_5105_Feet = 102665,
        ///<summary>NAD 1983 StatePlane Illinois East FIPS 1201 Feet</summary>
        NAD_1983_StatePlane_Illinois_East_FIPS_1201_Feet = 102671,
        ///<summary>NAD 1983 StatePlane Illinois West FIPS 1202 Feet</summary>
        NAD_1983_StatePlane_Illinois_West_FIPS_1202_Feet = 102672,
        ///<summary>NAD 1983 StatePlane Iowa North FIPS 1401 Feet</summary>
        NAD_1983_StatePlane_Iowa_North_FIPS_1401_Feet = 102675,
        ///<summary>NAD 1983 StatePlane Iowa South FIPS 1402 Feet</summary>
        NAD_1983_StatePlane_Iowa_South_FIPS_1402_Feet = 102676,
        ///<summary>NAD 1983 StatePlane Kansas North FIPS 1501 Feet</summary>
        NAD_1983_StatePlane_Kansas_North_FIPS_1501_Feet = 102677,
        ///<summary>NAD 1983 StatePlane Kansas South FIPS 1502 Feet</summary>
        NAD_1983_StatePlane_Kansas_South_FIPS_1502_Feet = 102678,
        ///<summary>NAD 1983 StatePlane Louisiana North FIPS 1701 Feet</summary>
        NAD_1983_StatePlane_Louisiana_North_FIPS_1701_Feet = 102681,
        ///<summary>NAD 1983 StatePlane Louisiana South FIPS 1702 Feet</summary>
        NAD_1983_StatePlane_Louisiana_South_FIPS_1702_Feet = 102682,
        ///<summary>NAD 1983 StatePlane Maine East FIPS 1801 Feet</summary>
        NAD_1983_StatePlane_Maine_East_FIPS_1801_Feet = 102683,
        ///<summary>NAD 1983 StatePlane Maine West FIPS 1802 Feet</summary>
        NAD_1983_StatePlane_Maine_West_FIPS_1802_Feet = 102684,
        ///<summary>NAD 1983 StatePlane Michigan North FIPS 2111 Feet</summary>
        NAD_1983_StatePlane_Michigan_North_FIPS_2111_Feet = 102688,
        ///<summary>NAD 1983 StatePlane Michigan Central FIPS 2112 Feet</summary>
        NAD_1983_StatePlane_Michigan_Central_FIPS_2112_Feet = 102689,
        ///<summary>NAD 1983 StatePlane Michigan South FIPS 2113 Feet</summary>
        NAD_1983_StatePlane_Michigan_South_FIPS_2113_Feet = 102690,
        ///<summary>NAD 1983 StatePlane Minnesota North FIPS 2201 Feet</summary>
        NAD_1983_StatePlane_Minnesota_North_FIPS_2201_Feet = 102691,
        ///<summary>NAD 1983 StatePlane Minnesota Central FIPS 2202 Feet</summary>
        NAD_1983_StatePlane_Minnesota_Central_FIPS_2202_Feet = 102692,
        ///<summary>NAD 1983 StatePlane Minnesota South FIPS 2203 Feet</summary>
        NAD_1983_StatePlane_Minnesota_South_FIPS_2203_Feet = 102693,
        ///<summary>NAD 1983 StatePlane Missouri East FIPS 2401 Feet</summary>
        NAD_1983_StatePlane_Missouri_East_FIPS_2401_Feet = 102696,
        ///<summary>NAD 1983 StatePlane Missouri Central FIPS 2402 Feet</summary>
        NAD_1983_StatePlane_Missouri_Central_FIPS_2402_Feet = 102697,
        ///<summary>NAD 1983 StatePlane Missouri West FIPS 2403 Feet</summary>
        NAD_1983_StatePlane_Missouri_West_FIPS_2403_Feet = 102698,
        ///<summary>NAD 1983 StatePlane Montana FIPS 2500 Feet</summary>
        NAD_1983_StatePlane_Montana_FIPS_2500_Feet = 102700,
        ///<summary>NAD 1983 StatePlane Nebraska FIPS 2600 Feet</summary>
        NAD_1983_StatePlane_Nebraska_FIPS_2600_Feet = 102704,
        ///<summary>NAD 1983 StatePlane Nevada East FIPS 2701 Feet</summary>
        NAD_1983_StatePlane_Nevada_East_FIPS_2701_Feet = 102707,
        ///<summary>NAD 1983 StatePlane Nevada Central FIPS 2702 Feet</summary>
        NAD_1983_StatePlane_Nevada_Central_FIPS_2702_Feet = 102708,
        ///<summary>NAD 1983 StatePlane Nevada West FIPS 2703 Feet</summary>
        NAD_1983_StatePlane_Nevada_West_FIPS_2703_Feet = 102709,
        ///<summary>NAD 1983 StatePlane New Hampshire FIPS 2800 Feet</summary>
        NAD_1983_StatePlane_New_Hampshire_FIPS_2800_Feet = 102710,
        ///<summary>NAD 1983 StatePlane New Jersey FIPS 2900 Feet</summary>
        NAD_1983_StatePlane_New_Jersey_FIPS_2900_Feet = 102711,
        ///<summary>NAD 1983 StatePlane North Dakota North FIPS 3301 Feet</summary>
        NAD_1983_StatePlane_North_Dakota_North_FIPS_3301_Feet = 102720,
        ///<summary>NAD 1983 StatePlane North Dakota South FIPS 3302 Feet</summary>
        NAD_1983_StatePlane_North_Dakota_South_FIPS_3302_Feet = 102721,
        ///<summary>NAD 1983 StatePlane Ohio North FIPS 3401 Feet</summary>
        NAD_1983_StatePlane_Ohio_North_FIPS_3401_Feet = 102722,
        ///<summary>NAD 1983 StatePlane Ohio South FIPS 3402 Feet</summary>
        NAD_1983_StatePlane_Ohio_South_FIPS_3402_Feet = 102723,
        ///<summary>NAD 1983 StatePlane Oregon North FIPS 3601 Feet</summary>
        NAD_1983_StatePlane_Oregon_North_FIPS_3601_Feet = 102726,
        ///<summary>NAD 1983 StatePlane Oregon South FIPS 3602 Feet</summary>
        NAD_1983_StatePlane_Oregon_South_FIPS_3602_Feet = 102727,
        ///<summary>NAD 1983 StatePlane Rhode Island FIPS 3800 Feet</summary>
        NAD_1983_StatePlane_Rhode_Island_FIPS_3800_Feet = 102730,
        ///<summary>NAD 1983 StatePlane South Carolina FIPS 3900 Feet</summary>
        NAD_1983_StatePlane_South_Carolina_FIPS_3900_Feet = 102733,
        ///<summary>NAD 1983 StatePlane South Dakota North FIPS 4001 Feet</summary>
        NAD_1983_StatePlane_South_Dakota_North_FIPS_4001_Feet = 102734,
        ///<summary>NAD 1983 StatePlane South Dakota South FIPS 4002 Feet</summary>
        NAD_1983_StatePlane_South_Dakota_South_FIPS_4002_Feet = 102735,
        ///<summary>NAD 1983 StatePlane Utah North FIPS 4301 Feet</summary>
        NAD_1983_StatePlane_Utah_North_FIPS_4301_Feet = 102742,
        ///<summary>NAD 1983 StatePlane Utah Central FIPS 4302 Feet</summary>
        NAD_1983_StatePlane_Utah_Central_FIPS_4302_Feet = 102743,
        ///<summary>NAD 1983 StatePlane Utah South FIPS 4303 Feet</summary>
        NAD_1983_StatePlane_Utah_South_FIPS_4303_Feet = 102744,
        ///<summary>NAD 1983 StatePlane Vermont FIPS 4400 Feet</summary>
        NAD_1983_StatePlane_Vermont_FIPS_4400_Feet = 102745,
        ///<summary>NAD 1983 StatePlane West Virginia North FIPS 4701 Feet</summary>
        NAD_1983_StatePlane_West_Virginia_North_FIPS_4701_Feet = 102750,
        ///<summary>NAD 1983 StatePlane West Virginia South FIPS 4702 Feet</summary>
        NAD_1983_StatePlane_West_Virginia_South_FIPS_4702_Feet = 102751,
        ///<summary>NAD 1983 StatePlane Wyoming East FIPS 4901 Feet</summary>
        NAD_1983_StatePlane_Wyoming_East_FIPS_4901_Feet = 102755,
        ///<summary>NAD 1983 StatePlane Wyoming East Central FIPS 4902 Feet</summary>
        NAD_1983_StatePlane_Wyoming_East_Central_FIPS_4902_Feet = 102756,
        ///<summary>NAD 1983 StatePlane Wyoming West Central FIPS 4903 Feet</summary>
        NAD_1983_StatePlane_Wyoming_West_Central_FIPS_4903_Feet = 102757,
        ///<summary>NAD 1983 StatePlane Wyoming West FIPS 4904 Feet</summary>
        NAD_1983_StatePlane_Wyoming_West_FIPS_4904_Feet = 102758,
        ///<summary>NAD 1983 StatePlane Puerto Rico Virgin Islands FIPS 5200 Feet</summary>
        NAD_1983_StatePlane_Puerto_Rico_Virgin_Islands_FIPS_5200_Feet = 102761,
        ///<summary>NAD 1983 StatePlane Guam FIPS 5400 Feet</summary>
        NAD_1983_StatePlane_Guam_FIPS_5400_Feet = 102766
    }

    /// <summary>
    /// Prime Meridian Enums
    /// </summary>
    public enum ePrimeMeridian
    {
        ///<summary>Greenwich Prime Meridian</summary>
        Greenwich = 8901,
        ///<summary>Athens Prime Meridian 23°25'33.17" E</summary>
        Athens = 8912,
        ///<summary>Bern Prime Meridian 7°15'44.1" E</summary>
        Bern = 8907,
        ///<summary>Bogota Prime Meridian 74°2'42.47" W</summary>
        Bogota = 8904,
        ///<summary>Brussels Prime Meridian 4°13'13.7" E</summary>
        Brussels = 8910,
        ///<summary>Ferro Prime Meridian 17°24'0" W </summary>
        Ferro = 8909,
        ///<summary>Jakarta Prime Meridian 106°28'58" E</summary>
        Jakarta = 8908,
        ///<summary>Lisbon Prime Meridian 9°4'31.75" W</summary>
        Lisbon = 8902,
        ///<summary>Madrid Prime Meridian 3°24'41.97" W</summary>
        Madrid = 8905,
        ///<summary>Oslo Prime Meridian 10°25'56.1" E </summary>
        Oslo = 8913,
        ///<summary>Paris Prime Meridian 2°20'14.025" W</summary>
        Paris = 8903,
        ///<summary>Rome Prime Meridian 2°12'5.02" E" E</summary>
        Rome = 8906,
        ///<summary>Stockholm Prime Meridian 18°1'58.73" E </summary>
        Stockholm = 8911
    }

    #endregion

    /// <summary>
    /// The ShapeFile class represents a .shp ArcGIS shapefile and contains all the function necessary to read and write to it.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("1D1C4791-CD7C-450c-B64C-913637424267")]
    public class ShapeFile : ArcShapeFile.IShapeFile, IDisposable
    {

        #region **********          Local Variables               **********

        private Parts mvarParts;
        private Fields mvarFields;
        private Vertices mvarVertices;
        private Projection mvarDatum;


        // Private Variables for OCX & DLL
        private string mvarShapeFile;
        private string mvarShapeIndex;
        private string mvarShapeDBF;

        private int mvarShapeType;
        private int mvarRecordShapeType;
        private int mvarShapeCount;
        private int mvarCurrentRecord;
        private bool mvarBOF = true;
        private bool mvarEOF = false;
        private bool mvarAddShapeID = false;
        private bool mvarTestForHole = false;
        private bool mvarRetainData = false;
        private eReadMode mvarReadmode = eReadMode.FullRead;
        private bool mvarStartEmpty;
        private bool mvarLockFile = false;
        private bool mvarIsNull = false;

        // Language settings
        private eLanguage mvarLanguage = eLanguage.OEM;
        private string mvarsysDelimiter;
        private string mvardbfDelimiter;
        private bool mvarYYYYMMDD = true;


        // Area and Centriod
        private double? mvarShapeArea;
        private double mvarCentroidX;
        private double mvarCentroidY;
        private double mvarPerimeter;

        // Parts Variables
        //Private mvarParts As Parts
        private double mvarPartXMin;
        private double mvarPartXMax;
        private double mvarPartYMin;
        private double mvarPartYMax;
        private int mvarNoOfParts;
        private int mvarNoOfPoints;

        // For an individual Shape Record
        private double mvarShapeXMin;
        private double mvarShapeXMax;
        private double mvarShapeYMin;
        private double mvarShapeYMax;
        private double mvarShapeZMin;
        private double mvarShapeZMax;
        private double? mvarShapeMMin;
        private double? mvarShapeMMax;

        // For the entire Shape File
        private double mvarShapeFileXMin;
        private double mvarShapeFileXMax;
        private double mvarShapeFileYMin;
        private double mvarShapeFileYMax;
        private double mvarShapeFileZMin;
        private double mvarShapeFileZMax;
        private double? mvarShapeFileMMin;
        private double? mvarShapeFileMMax;

        // Find Variables
        private bool mvarNoMatch = false;
        private string mvarFindQuery;
        private bool mvarFindXY = false;
        private double mvarFindX;
        private double mvarFindY;
        private double mvarFindTolerance = 0;

        // Project Variables
        private System.Data.DataTable mvarProjTable;
        private bool disposed = false; // to detect redundant calls

        // File Streams
        FileStream fsShapeFile;
        FileStream fsShapeIndex;
        FileStream fsDataFile;

        #endregion

        #region **********          String Functions              **********

        private string SetDelimiter(eLanguage CodePage)
        {
            string DelimterValue = null;
            // *********************************************************
            // * Set the decimal delimiter based on the  DBF code page *
            // *********************************************************

            switch (CodePage)
            {
                case eLanguage.Codepage_437_US_MSDOS:
                    DelimterValue = ".";
                    break;
                case eLanguage.Codepage_737_Greek_MSDOS:
                case eLanguage.Codepage_852_EasernEuropean_MSDOS:
                case eLanguage.Codepage_1253_Greek_Windows:
                case eLanguage.Codepage_857_Turkish_MSDOS:
                case eLanguage.Codepage_861_Icelandic_MSDOS:
                case eLanguage.Codepage_865_Nordic_MSDOS:
                case eLanguage.Codepage_866_Russian_MSDOS:
                case eLanguage.Codepage_1250_Eastern_European_Windows:
                case eLanguage.Codepage_1254_Turkish_Windows:
                    DelimterValue = ",";
                    break;
                case eLanguage.Codepage_932_Japanese_Windows:
                case eLanguage.Codepage_936_Chinese_Windows:
                case eLanguage.Codepage_950_Chinese_Windows:
                    DelimterValue = ".";
                    break;
                case eLanguage.Codepage_1252_Windows_ANSI:
                case eLanguage.Codepage_1255_Hebrew_Windows:
                case eLanguage.Codepage_850_International_MSDOS:
                case eLanguage.Codepage_1256_Arabic_Windows:
                    DelimterValue = ".";
                    break;
                case eLanguage.Codepage_1251_Russian_Windows:
                    DelimterValue = " ";
                    break;
                default:
                    // OEM = 0  ANSI = &H57
                    System.Globalization.NumberFormatInfo decInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                    DelimterValue = decInfo.NumberDecimalSeparator;
                    break;
            }
            return DelimterValue;

        }

        private string GetCodePageName(eLanguage Language)
        {
            string functionReturnValue = null;
            // ****************************************************
            // * Resolve the code page name from the DBF LCID     *
            // ****************************************************

            switch (Language)
            {
                case eLanguage.Codepage_437_US_MSDOS:
                    functionReturnValue = "IBM437";
                    break;
                case eLanguage.Codepage_737_Greek_MSDOS:
                    functionReturnValue = "ibm737";
                    break;
                case eLanguage.Codepage_850_International_MSDOS:
                    functionReturnValue = "ibm850";
                    break;
                case eLanguage.Codepage_852_EasernEuropean_MSDOS:
                    functionReturnValue = "ibm852";
                    break;
                case eLanguage.Codepage_857_Turkish_MSDOS:
                    functionReturnValue = "ibm857";
                    break;
                case eLanguage.Codepage_861_Icelandic_MSDOS:
                    functionReturnValue = "ibm861";
                    break;
                case eLanguage.Codepage_865_Nordic_MSDOS:
                    functionReturnValue = "IBM865";
                    break;
                case eLanguage.Codepage_866_Russian_MSDOS:
                    functionReturnValue = "cp866";
                    break;
                case eLanguage.Codepage_932_Japanese_Windows:
                    functionReturnValue = "shift_jis";
                    break;
                case eLanguage.Codepage_936_Chinese_Windows:
                    functionReturnValue = "gb2312";
                    break;
                case eLanguage.Codepage_950_Chinese_Windows:
                    functionReturnValue = "big5";
                    break;
                case eLanguage.Codepage_1250_Eastern_European_Windows:
                    functionReturnValue = "windows-1250";
                    break;
                case eLanguage.Codepage_1251_Russian_Windows:
                    functionReturnValue = "windows-1251";
                    break;
                case eLanguage.Codepage_1252_Windows_ANSI:
                    functionReturnValue = "windows-1252";
                    break;
                case eLanguage.Codepage_1253_Greek_Windows:
                    functionReturnValue = "windows-1253";
                    break;
                case eLanguage.Codepage_1254_Turkish_Windows:
                    functionReturnValue = "windows-1254";
                    break;
                case eLanguage.Codepage_1255_Hebrew_Windows:
                    functionReturnValue = "windows-1255";
                    break;
                case eLanguage.Codepage_1256_Arabic_Windows:
                    functionReturnValue = "windows-1256";
                    break;
                default:
                    functionReturnValue = Encoding.Default.HeaderName;
                    break;
            }
            return functionReturnValue;

        }

        private byte[] StrToByteArray(string str)
        {
            // ****************************************************
            // * Convert a String to a Byte Array                 *
            // ****************************************************
            byte[] DataBytes = System.Text.Encoding.GetEncoding(GetCodePageName(mvarLanguage)).GetBytes(str);
            return DataBytes;
        }

        private string ByteArrayToString(byte[] ByteArray)
        {
            // ****************************************************
            // * Convert the Byte array to it's string value      *
            // ****************************************************
            string sAns = Encoding.GetEncoding(GetCodePageName(mvarLanguage)).GetString(ByteArray);
            int iPos = sAns.IndexOf((char)0);
            if (iPos > 0)
            { sAns = sAns.Substring(0, iPos); }
            return sAns;
        }

        #endregion

        #region **********          Initialisation                **********
        /// <summary>
        /// The ShapeFile Base class
        /// </summary>
        public ShapeFile()
        {

            // Load the projection info into a DataTable
            mvarProjTable = new System.Data.DataTable();
            mvarProjTable.Columns.Add("EPSG", System.Type.GetType("System.Int32"));
            mvarProjTable.Columns.Add("Type", System.Type.GetType("System.String"));
            mvarProjTable.Columns.Add("Name", System.Type.GetType("System.String"));
            mvarProjTable.Columns.Add("WKT", System.Type.GetType("System.String"));

            System.Reflection.Assembly AppAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream nStream = default(Stream);

            nStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ArcShapeFile.ESRIProj.txt");
            using (nStream)
            {
                if (nStream != null)
                {
                    using (StreamReader reader = new StreamReader(nStream))
                    {
                        string projData = reader.ReadToEnd();
                        string[] projLines = projData.Split('\n');
                        foreach (string projLine in projLines)
                        {
                            string[] data = projLine.Split('\t');
                            System.Data.DataRow nRow = mvarProjTable.NewRow();
                            nRow[0] = Convert.ToInt32(data[0]);
                            nRow[1] = "Projcs";
                            nRow[2] = data[1];
                            nRow[3] = data[2];
                            mvarProjTable.Rows.Add(nRow);
                        }
                    }
                }
            }

            nStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ArcShapeFile.ESRIGeo.txt");
            using (nStream)
            {
                if (nStream != null)
                {
                    using (StreamReader reader = new StreamReader(nStream))
                    {
                        string projData = reader.ReadToEnd();
                        string[] projLines = projData.Split('\n');
                        foreach (string projLine in projLines)
                        {
                            string[] data = projLine.Split('\t');
                            System.Data.DataRow nRow = mvarProjTable.NewRow();
                            nRow[0] = Convert.ToInt32(data[0]);
                            nRow[1] = "Geocs";
                            nRow[2] = data[1];
                            nRow[3] = data[2];
                            mvarProjTable.Rows.Add(nRow);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Releases all resources used by ArcShapeFile
        /// </summary>
        ///<remarks>
        ///This method makes sure that the file handles have been released and the garbage collection has been fired.  Always a good idea.
        ///</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by ArcShapeFile
        /// </summary>
        /// <param name="disposing"></param>
        ///<remarks>
        ///This method makes sure that the file handles have been released and the garbage collection has been fired.  Always a good idea.
        ///</remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    if (fsShapeFile != null)
                    {
                        Close();
                    }

                    if (mvarParts != null)
                    {
                        mvarParts.Clear();
                        mvarParts = null;
                    }
                    if (mvarFields != null)
                    {
                        mvarFields.Clear();
                        mvarFields = null;
                    }
                    if (mvarVertices != null)
                    {
                        mvarVertices.Clear();
                        mvarVertices = null;
                    }
                }
            }
            disposed = true;

        }

        #endregion

        #region **********          ShapeFile Properties          **********


        /// <summary>
        /// Sets/Returns the codepage of the Fields database
        /// </summary>
        /// <value>The Language Codepage. The setting or return value is an ENum constant that indicates the LCID of the language code page.</value>
        /// <remarks>
        /// Language support in the .DBF format is pretty limited but normally you can get away with the default of OEM which turns character recognition over to the host computer.</remarks>
        /// <seealso cref="eLanguage"/>
        public eLanguage Language
        {
            get { return mvarLanguage; }
            set { mvarLanguage = value; }
        }

        /// <summary>
        /// Returns the ordinal position of the current record within the <c>ShapeFile</c>
        /// </summary>
        /// <remarks>Sometimes you want to know how far through the file you are.  This property will tell you what record number you've reached.  Note: the first record will return a CurrentRecord of 1 rather than 0
        /// as this property is meant to indicate the relative position within the ShapeFile from 1 to RecordCount.  The MoveTo methods and CopyFrom methods rely on a 0 based ordinal (0 to RecordCount - 1).</remarks>
        /// <seealso cref="RecordCount"/>
        public int CurrentRecord
        {
            // Shape CurrentCount Properties

            get { return mvarCurrentRecord; }
        }

        /// <summary>
        /// Returns the total number of records (including NULL shapes and deleted records) in a <c>ShapeFile</c>
        /// </summary>
        /// <remarks>If the <B>RecordCount</B> value is 0, there are no objects in the collection.  You can use this property to easily	iterate through the records in the file i.e.
        /// <example>
        /// <code lang="C#">
        /// for (int i = 0; i &lt; shp.RecordCount; i++}
        /// {
        ///     // Do some stuff here
        ///     shp.MoveNext();
        /// }
        /// </code>
        /// <code lang="VB">
        /// For i as Integer = 0 To shp.RecordCount - 1 
        ///     ' Do some stuff here
        ///     shp.MoveNext()
        /// Next i
        /// </code>
        /// </example>
        /// </remarks>
        public int RecordCount
        {
            // Shape CurrentCount Properties

            get { return mvarShapeCount; }
        }

        ///<summary>
        ///Returns the total area of the current polygon shape.
        ///</summary>
        ///<value>The total area of the current polygon shape</value>
        ///<remarks>
        ///The value returned by this object represents the total area of the ShapeFile. So ... if you have a multipart polygon shape that includes donuts then the Area reports will be the area of all non-donut parts minus the area of all donut parts. For the area of each part refer to the Parts <see cref="Part.Area"/> property.
        ///This variable is a Nullable double so if you aren't reading a polygon ShapeFile the value will be null.
        ///</remarks>
        public double? Area
        {
            get
            {
                double? functionReturnValue = null;
                // Area of Polygon Shape File
                if (mvarShapeArea != null)
                {
                    functionReturnValue = System.Math.Abs(Convert.ToDouble(mvarShapeArea));
                }
                else
                {
                    functionReturnValue = null;
                }
                return functionReturnValue;
            }
        }

        /// <summary>
        /// Returns the X centre of gravity of a ShapeFile polygon shape
        /// </summary>
        /// <value>
        /// The centroid x.
        /// </value>
        /// <remarks>
        ///There are several methods for finding the centre of mass of a polygon.  In this case I'm using weighted sum of the centroids of the polygon partitioned into triangles (the code can be found <see href="http://math.stackexchange.com/questions/90463/how-can-i-calculate-the-centroid-of-polygon">here</see>).  For polyline features the average mid point is used.  In either case refer to the <see cref="Part.CentroidX">Part.CentroidX</see> property for the individual centroids of multipart shapes.
        /// </remarks>
        /// <seealso cref="CentroidY"/>
        /// <seealso cref="Part.CentroidX"/>
        /// <seealso cref="Part.CentroidY"/>
        public double CentroidX
        {
            // Centroids of Polygon Shape
            get { return mvarCentroidX; }
        }

        /// <summary>
        /// Returns the Y centre of gravity of a ShapeFile polygon shape
        /// </summary>
        /// <value>
        /// The centroid y.
        /// </value>
        /// <remarks>
        ///There are several methods for finding the centre of mass of a polygon.  In this case I'm using weighted sum of the centroids of the polygon partitioned into triangles (the code can be found <see href="http://math.stackexchange.com/questions/90463/how-can-i-calculate-the-centroid-of-polygon">here</see>).  For polyline features the average mid point is used.  In either case refer to the <see cref="Part.CentroidX">Part.CentroidY</see> property for the individual centroids of multipart shapes.
        /// </remarks>
        /// <seealso cref="CentroidX"/>
        /// <seealso cref="Part.CentroidX"/>
        /// <seealso cref="Part.CentroidY"/>
        public double CentroidY
        {
            get { return mvarCentroidY; }
        }

        /// <summary>
        /// Returns the total length of the perimeter around a ShapeFile polygon shape or the length a Shapefile line shape
        /// </summary>
        /// <value>The total perimeter or length</value>
        ///<remarks>
        ///This property is really designed to cater for polygons and lines.
        ///If you want the perimeter length of each shape part use the ShapeFile <see cref="Part.Perimeter"/> property
        ///</remarks>
        public double Perimeter
        {
            get { return mvarPerimeter; }
        }

        /// <summary>
        /// Returns the maximum Y value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<remarks>
        ///<value>The maximum Y value of the current shape</value>
        ///Part of the entire ShapeFile record minimum bounding values, this value and is popoulated for all shape types.
        ///</remarks>
        public double yMax
        {
            // MBR of Shape File
            get { return mvarShapeFileYMax; }
        }

        /// <summary>
        /// Returns the maximum X value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<value>The maximum X value of the current shape</value>
        ///<remarks>
        ///Part of the entire ShapeFile record minimum bounding values, this value and is popoulated for all shape types.
        ///</remarks>
        public double xMax
        {
            get { return mvarShapeFileXMax; }
        }

        /// <summary>
        /// Returns the minimum Y value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<value>The minimum Y value of the current shape</value>
        /// <remarks>
        /// Part of the entire ShapeFile record minimum bounding values, this value and is popoulated for all shape types.
        /// </remarks>
        public double yMin
        {
            get { return mvarShapeFileYMin; }
        }

        /// <summary>
        /// Returns the minimum X value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<value>The minimum Y value of the current shape</value>
        ///<remarks>
        ///Part of the entire ShapeFile record minimum bounding values, this value and is popoulated for all shape types.
        ///</remarks>
        public double xMin
        {
            get { return mvarShapeFileXMin; }
        }

        /// <summary>
        /// Returns the minimum non-null measure value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<value>The minimum measure value of the current shape</value>
        ///<remarks>
        ///Part of the minimum bounding values for the entire ShapeFile,  this value may be null and populated only for shapes in X,Y,Z space or Measured X,Y space.
        ///</remarks>
        public double? mMin
        {
            get { return mvarShapeFileMMin; }
        }

        /// <summary>
        /// Returns the maximum non-null measure value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<value>The maximum measure value of the current shape</value>
        ///<remarks>
        ///Part of the minimum bounding values for the entire ShapeFile,  this value may be null and populated only for shapes in X,Y,Z space or Measured X,Y space.
        ///</remarks>
        public double? mMax
        {
            get { return mvarShapeFileMMax; }
        }

        /// <summary>
        /// Returns the minimum Z value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<value>The minimum Z value of the current shape</value>
        ///<remarks>
        ///Part of the entire ShapeFile minimum bounding values, this value is populated only for shapes in X,Y,Z space and will be null otherwise
        ///</remarks>
        public double? zMin
        {
            get { return mvarShapeFileZMin; }
        }

        /// <summary>
        /// Returns the maximum Z value of the minimum bounding rectangle describing each 
        /// outer bounds of the entire ShapeFile
        /// </summary>
        ///<value>The maximum Z value of the current shape</value>
        ///<remarks>
        ///Part of the entire ShapeFile minimum bounding values, this value is populated only for shapes in X,Y,Z space and will be null otherwise
        ///</remarks>
        public double? zMax
        {
            get { return mvarShapeFileZMax; }
        }

        /// <summary>
        /// Returns a value that indicates whether the current record position is before the 
        /// first record in a ShapeFile 
        /// </summary>
        /// <value>Beginning of File condition</value>
        /// <returns>True if you try and move above the first record</returns>
        /// <remarks>Useful when you're moving backwards through the ShapeFile records</remarks>
        /// <example>
        /// <code lang="C#">
        /// shp.MoveLast();
        /// While(!shp.BOF)
        /// {
        ///     // Do some stuff here
        ///     shp.MovePrevious();
        /// }
        /// </code>
        ///<code lang="VB">
        /// shp.MoveLast()
        /// While Not shp.BOF
        ///     '' Do some stuff here
        ///     shp.MovePrevious()
        /// End While
        /// </code>        
        /// </example>
        /// <seealso cref="EOF"/>
        public bool BOF
        {
            // Shape Beginning and End of File Properties

            get { return mvarBOF; }
        }

        /// <summary>
        /// Returns a value that indicates whether the current record position is after the 
        /// last record in a ShapeFile 
        /// </summary>
        /// <value>End of File condition</value>
        /// <returns>True if you try and move beyond the last record</returns>
        /// <remarks>This property can be handy if you want to cycle through the ShapeFile records without referencing a for 0 to RecordCount loop. ???? I sense puzzlement.  Check out the example
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// While(!shp.EOF)
        /// {
        ///     // Do some stuff here
        ///     shp.MoveNext();
        /// }
        /// </code>
        ///<code lang="VB">
        /// While Not shp.EOF
        ///     '' Do some stuff here
        ///     shp.MoveNext()
        /// End While
        /// </code>        
        /// </example>
        ///<seealso cref="BOF"/>

        public bool EOF
        {

            get { return mvarEOF; }
        }

        /// <summary>
        /// Indicates whether a Find search has located any records for the given criteria
        /// </summary>
        /// <value>Is there a match in the search condition</value>
        ///<remarks>
        ///To locate a record, use one of the <see cref="O:ArcShapeFile.ShapeFile.FindFirst">FindFirst</see>  methods. Check the NoMatch property to see whether a record was found.
        ///If the find method is unsuccessful and NoMatch is True and the current record will no longer be valid.  Note: Using any of the Move methods will also reset the NoMatch property to True.
        ///</remarks>
        ///<example><code lang="C#">
        ///     myShape.FindFirst("[FloatField] &gt; 1 and [DateField] &lt; 10 July 2000");
        ///     Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString());
        ///     
        ///     while (!myShape.NoMatch)
        ///     {
        ///         myShape.FindNext();
        ///         if (!myShape.NoMatch)
        ///         {
        ///            Console.WriteLine(&quot;Next value found at Record {0}&quot;, myShape.CurrentRecord.ToString());
        ///            foreach (Field mF in myShape.Fields)
        ///                Console.WriteLine(mF.Name + "   " + mF.Value.ToString());
        ///        }
        ///    }
        /// </code>
        /// <code lang="VB">
        /// myShape.FindFirst("[FloatField] &gt; 1 and [DateField] &lt; 10 July 2000")
        /// Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 
        /// While Not myShape.NoMatch
        /// 	myShape.FindNext()
        /// 	If Not myShape.NoMatch Then
        /// 		Console.WriteLine("Next value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 		For Each mF As Field In myShape.Fields
        ///             Console.WriteLine(mF.Name + "   " + mF.Value.ToString())
        /// 		Next
        /// 	End If
        /// End While
        /// </code>
        /// </example>
        ///<seealso cref="FindFirst(System.String)"/>
        ///<seealso cref="FindFirst(System.Double,System.Double)"/>
        ///<seealso cref="FindFirst(System.Double,System.Double,System.Double)"/>
        ///<seealso cref="FindNext"/>
        public bool NoMatch
        {
            // ****************************
            // * Has Find been Successful *
            // ****************************


            get { return mvarNoMatch; }
        }

        /// <summary>
        /// The TestForHoles property allows you to turn on or off the ShapeFile librarys' ability to check for holes within polygons by using a point in polygon method.
        /// If it is set to False the digitised orientataion of the polygon will be used to determine holes.
        /// </summary>
        /// <value>Should the TestForHoles action be done</value>
        ///<remarks>
        ///By default the library relies on the orientation of the polygon parts to denote whether the part is a hole.  Clockwise means no, anti-clockwise means yes.
        ///If you suspect that the shapes delivered to you haven't followed this rule then set the TestForHoles property to True.  This will force a point in polygon test before the <see cref="Part.IsHole"/> property of the Parts collection is set.  The downside of this is that it slows the read speed.
        ///</remarks>
        public bool TestForHoles
        {
            get { return mvarTestForHole; }
            set { mvarTestForHole = value; }
        }

        /// <summary>
        /// The RetainData Method allows you to keep existing data values when you are creating ShapeFile records
        /// </summary>
        /// <value>Should the attribute data be retained between writes</value>
        ///<remarks>
        ///Why would you want to do this?  Well I've sometimes found that I wanted to retain most of the data values between record inserts and only change the values on one or two fields.
        ///That's why I dreamed up the property.  The default is False meaning that you have to freshly populate all your defined fields with values between inserts.
        ///<para>OK I'm teaching everyone to suck eggs here but ... If you do want to retain the data - make sure you set the RetainData property to True BEFORE you write out the record.</para>
        ///</remarks>
        ///<example>
        ///<code lang="C#">
        ///using (ShapeFile myShape as new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", eShapeType.shpPoint);
        ///    myShape.Fields.Add("TextField", eFieldType.shpText);
        ///    myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2);
        ///    myShape.Fields.Add("DateField", eFieldType.shpDate);
        ///    myShape.Fields.Add("FloatField", eFieldType.shpFloat);
        ///    myShape.WriteFieldDefs();
        ///
        ///    myShape.RetainData = true;
        ///    myShape.Vertices.Add(1, 1);
        ///    myShape.Fields[0].Value = "First";
        ///    myShape.Fields[1].Value = 1.23;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.Fields[3].Value = 1.23E-5;
        ///    myShape.WriteShape();
        ///
        ///    myShape.Vertices.Add(1, 10);
        ///    myShape.Fields[0].Value = "Second;
        ///    myShape.WriteShape();
        ///
        ///    myShape.RetainData = false;
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Fields[0].Value = "Third";
        ///    myShape.Fields[1].Value = 2.34;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.Fields[3].Value = 2.34E-5;
        ///    myShape.WriteShape();
        ///}
        ///</code>
        ///<code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open("C:\Shapes\Point.shp", eShapeType.shpPoint)
        ///    myShape.Fields.Add("TextField", eFieldType.shpText)
        ///    myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2)
        ///    myShape.Fields.Add("DateField", eFieldType.shpDate)
        ///    myShape.Fields.Add("FloatField", eFieldType.shpFloat)
        ///    myShape.WriteFieldDefs()
        ///
        ///    myShape.RetainData = True
        ///    myShape.Vertices.Add(1, 1)
        ///    myShape.Fields(0).Value = "First"
        ///    myShape.Fields(1).Value = 1.23
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.Fields(3).Value = 0.0000123
        ///    myShape.WriteShape()
        ///
        ///    myShape.Vertices.Add(1, 10)
        ///    myShape.Fields(0).Value = "Second"
        ///    myShape.WriteShape()
        ///
        ///    myShape.RetainData = False
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Fields(0).Value = "Third"
        ///    myShape.Fields(1).Value = 2.34
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.Fields(3).Value = 0.0000234
        ///    myShape.WriteShape()
        ///End Using
        ///</code>
        ///</example>
        public bool RetainData
        {
            get { return mvarRetainData; }
            set { mvarRetainData = value; }
        }

        /// <summary>
        /// The ReadMode property allows you to turn on or off the automatic loading of ShapeFile 
        /// vertice and database information. Setting this to HeaderOnly can be handy when you don't 
        /// want the overhead of reading all the shape information, but want to test the bounds or database 
        /// information separately
        /// </summary>
        /// <value>How do you want the record to be read</value>
        ///<remarks>
        ///<para>Set this property before any move action to change the way the DLL loads the data.  The question really is - When to use what method.
        ///Let's see.  FullRead loads all the data into the Vertices and Fields collections. The upside of this is that it is easy to then manipulate all the data for the ShapeFile. The downside is that it does take time to load the data into the collections. This may be a real headache if you are reading a record with a large number of vertices.
        ///So ... If you're loading a ShapeFile and want to scan through to find a particular condition then HeaderOnly might be the one to use.</para>
        ///<para>HeaderOnly mode reads in the MBR values of the Vertices (including the how many vertices there are) but doesn't load any of the data into the collections.  This makes it fast to scan through all the records.
        ///When you want to see the contents of a particular record then you can load the Vertice data using the <see cref="LoadShapeData"/> method and/or the database values into Fields using the <see cref="LoadDBFData"/> method.</para>
        ///<para>If you want the best of both worlds use my personal favourite - FastRead.  This method loads all the record metadata (number of points, number of parts, the MBRs), populates the Fields collection with the database attributes, but holds all the vertice data in an internal byte array.  Everytime you read a vertice the DLL scans the array for the correct byte offset.  
        ///What it doesn't do is caluclate centroids, areas and test for holes.</para>
        ///<para>So how fast are the various methods?  Here are some results reading the vertices from a polyline shapefile with 688,354 records.
        ///<list type="table">
        ///    <listheader>
        ///        <term>ReadMode</term>
        ///        <description>Description</description>
        ///        <unlock>Files Unlocked Speed</unlock>
        ///        <lock>Files Locked Speed</lock>
        ///    </listheader>
        ///    <item>
        ///        <term>FullRead</term>
        ///        <description>The Field and Vertice collections, MBR, Number of points, centroids etc. are populated</description>
        ///        <unlock>394,279ms</unlock>
        ///        <lock>14,264ms</lock>
        ///    </item>
        ///    <item>
        ///        <term>FastRead</term>
        ///        <description>The Field collections, MBR and Number of points are populated</description>
        ///        <unlock>380,155ms</unlock>
        ///        <lock>9,919ms</lock>
        ///    </item>
        ///    <item>
        ///        <term>HeaderOnly</term>
        ///        <description>The MBR and Number of points are populated.  A separate call is required to load the Fields or Vertices</description>
        ///        <unlock>153,254ms</unlock>
        ///        <lock>8,112ms</lock>
        ///    </item>
        /// </list>
        /// Note that the best results are achieved when you <see cref="O:ArcShapeFile.ShapeFile.Open">Open</see> your ShapeFile with the Lock parameter set to True.
        ///</para>
        ///</remarks>
        /// <seealso cref="LoadDBFData"/>
        /// <seealso cref="LoadShapeData"/>
        public eReadMode ReadMode
        {
            get { return mvarReadmode; }
            set { mvarReadmode = value; }
        }

        /// <summary>
        /// Returns the pathname of the currently opened ShapeFile
        /// </summary>
        ///<value>The name of the ShapeFile</value>
        ///<remarks>
        ///This property is read only and is set by using the <see cref="O:ArcShapeFile.ShapeFile.Open"/> method.
        ///Just a pointer here: ShapeFiles really consist of a geometry file (.SHP) and index file (.SHX) and a database file (.DBF).  Other extensions are used in the ARCGIS envirnoment that may be included in a delivery, but these aren't used by this DLL.
        ///</remarks>
        public string ShapeFileName
        {
            get { return mvarShapeFile; }
        }

        /// <summary>
        /// Returns the type of the current record of the opened ShapeFile
        /// </summary>
        /// <value>The eNum of the ShapeType</value>
        ///<remarks>
        ///There can only be one ShapeType per ShapeFile. Well ... not strictly true ... You can also have Null Shapes. ESRI have yet to implement multiple ShapeTypes within a Shapefile. The White Paper on Shapefiles suggests that this is a future implementation, but nothing has happened in the last 25 years so I wouldn't hold my breath.
        ///</remarks>
        public eShapeType ShapeType
        {
            get { return (eShapeType)mvarRecordShapeType; }
        }

        /// <summary>
        /// Gets or Sets the ability of the Open command to create a NULL record when a new ShapeFile is created.
        /// This acts as a place holder and allows the empty Shapefile to be read by other applications.
        /// </summary>
        /// <value>Will the ShapeFile be created empty (Default = True)</value>
        ///<remarks>
        ///This property was added because a user of the VB DLL was creating ShapeFiles and then loading them in other apps to add data.
        ///Because the programs were reading the number of records from the DBF file the app threw a wobbly when it encounter no records (go figure) ... so ...
        ///This property allows you to create a new ShapeFile that either has a null shape added to the bottom ( value = False ) or completely empty ( default value = True).
        ///</remarks>
        public bool CreateEmpty
        {
            get { return mvarStartEmpty; }
            set { mvarStartEmpty = value; }
        }

        /// <summary>
        ///  Returns the Vertices collection associated with current ShapeFile Record
        /// </summary>
        public Vertices Vertices
        {
            get { return (Vertices)mvarVertices; }
        }

        /// <summary>
        ///  Returns the Parts collection associated with current ShapeFile Record
        /// </summary>
        public Parts Parts
        {
            get { return (Parts)mvarParts; }
        }

        /// <summary>
        ///  Returns the Fields collection associated with current ShapeFile Record
        /// </summary>
        /// <remarks>The Fields collection represents the attributes held within the DBF file associated with the ShapeFile.</remarks>
        public Fields Fields
        {
            get { return (Fields)mvarFields; }
        }

        /// <summary>
        /// The Projection and Datum info of the ShapeFile
        /// </summary>
        /// <remarks>I created the Projection class to hold all the details found within the .PRJ file.  To write a projection file use <see cref="O:ArcShapeFile.ShapeFile.WriteProjection">WriteProjection</see>.  </remarks>
        public Projection Projection
        {
            get { return mvarDatum; }
            set { mvarDatum = value; }
        }

        /// <summary>
        /// Returns true if the current record is a null shape
        /// </summary>
        /// <value>Is the current record null</value>
        ///<remarks>
        ///NULL shape records are often used as place holders in the ShapeFile or, in the case of this DLL, as an indicator that the current record has been marked for deletion.
        ///This is the only time that you can have more than one shape type within one ShapeFile.  NULL shapes take up space in the file, but their contents are skip over.  To really remove these beasties you should <see cref="Pack"/> the file.
        ///</remarks>
        public bool IsRecordNull
        { get { return mvarIsNull; } }

        #endregion

        #region **********          ShapeFile Methods             **********

        #region **********          Open / Close Methods          **********

        /// <summary>
        /// Creates a ShapeFile of a particular ShapeType with the file handles locked open ready for you to play with 
        /// </summary>
        /// <param name="filename">The name of the ShapeFile to create</param>
        /// <param name="shapetype">The type of ShapeFile to be created</param>
        /// <param name="lockfile">Sets the lock condition of the ShapeFile handle.  If True
        /// the ShapeFiles will remain open until the Close() command is given otherwise the ShapeFiles
        /// are opened and closed with each Read and Write.</param>
        /// <remarks>
        /// <para>This overload of the Open method creates a new ShapeFile or overwrites an existing ShapeFile.  Don't forget to define a field definition using the <see cref="O:ArcShapeFile.Fields.Add">Field.Add</see> method AND write them out with <see cref="WriteFieldDefs"/>.  If you don't then this beast will automatically create a integer field definition called SHAPE_ID to prevent your .DBF file from being corrupted.
        /// Set to the <paramref name="lockfile"/> status to True and the file locks on the .SHP, .SHX and .DBF file streams are held open between actions. This should make write opertations faster. The lock is maintained until cancelled with the <see cref="Close"/> command.  If the lock isn't set then every move or update operation will create and release locks on the 3 file parts.  What this does do is allow other apps to update the ShapeFile between actions.  Your choice if you want this to happen.</para>
        /// <para>For a speed test of the various Open methods refer to the table in <see cref="ReadMode"/></para>
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", eShapeType.shpPoint, true);
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType);
        ///
        ///    // Add a new record
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Fields["TextField"].Value = "New Record";
        ///    myShape.Fields["NumField"].Value = 3.45;
        ///    myShape.Fields["DateField"].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///
        ///    // Wait a while and nobody else can touch it while you have go for a cup of tea ...
        ///    System.Threading.Thread.Sleep(10000);
        ///
        ///    // Add a another new record
        ///    myShape.Vertices.Add(10, 15);
        ///    myShape.Fields[0].Value = "New Record 2";
        ///    myShape.Fields[1].Value = 4.45;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", True)
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType)
        ///
        ///    ' Add a new record
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Fields("TextField").Value = "New Record"
        ///    myShape.Fields("NumField").Value = 3.45
        ///    myShape.Fields("DateField").Value = DateTime.Now
        ///    myShape.WriteShape()
        ///
        ///    ' Wait a while and nobody else can touch it while you have go for a cup of tea ...
        ///    System.Threading.Thread.Sleep(10000)
        ///
        ///    ' Add a another new record
        ///    myShape.Vertices.Add(10, 15)
        ///    myShape.Fields(0).Value = "New Record 2"
        ///    myShape.Fields(1).Value = 4.45
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.WriteShape()
        ///End Using
        /// </code>
        /// </example>
        /// <seealso cref="Open(System.String)"/>
        /// <seealso cref="Open(System.String, System.Boolean)"/>
        /// <seealso cref="Open(System.String, eShapeType)"/>
        /// <seealso cref="Close"/>
        /// <seealso cref="ReadMode"/>
        public void Open(string filename, eShapeType shapetype, bool lockfile)
        { OpenShape(filename, true, shapetype, lockfile); }
        /// <summary>
        /// Opens an existing ShapeFile ready for viewing, adding or editing.
        /// </summary>
        /// <param name="filename">The name of the ShapeFile to open for reading or editing</param>
        /// <remarks>
        /// <para>This overload of the Open method opens an existing ShapeFile for reading or editing only.  The type is read from the file itself and doesn't have to be defined.  Remember when editing that you cannot mix <see cref="ShapeType">ShapeTypes</see> within a ShapeFile.</para>
        /// <para>As the Lockfile parameter hasn't been defined the file handles on the .SHP, .SHX and .DBF files will be released after any action.  This allows other applications to use the ShapeFile between edits etc. but slows the DLL read times drastically. </para> 
        /// <para>For a speed test of the various Open methods refer to the table in <see cref="ReadMode"/> </para>
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp");
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType);
        ///
        ///    // Add a new record
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Fields["TextField"].Value = "New Record";
        ///    myShape.Fields["NumField"].Value = 3.45;
        ///    myShape.Fields["DateField"].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///
        ///    // Wait a while as someone else is playing with the shape ...
        ///    System.Threading.Thread.Sleep(10000);
        ///
        ///    // Add a another new record
        ///    myShape.Vertices.Add(10, 15);
        ///    myShape.Fields[0].Value = "New Record 2";
        ///    myShape.Fields[1].Value = 4.45;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp")
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType)
        ///
        ///    ' Add a new record
        ///    myShape.Fields("TextField").Value = "New Record";
        ///    myShape.Fields("NumField").Value = 3.45;
        ///    myShape.Fields("DateField").Value = DateTime.Now;
        ///    myShape.WriteShape()
        ///
        ///    ' Wait a while as someone else is playing with the shape ...
        ///    System.Threading.Thread.Sleep(10000)
        ///
        ///    ' Add a another new record
        ///    myShape.Vertices.Add(10, 15)
        ///    myShape.Fields(0).Value = "New Record 2"
        ///    myShape.Fields(1).Value = 4.45
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.Fields(3).Value = 44500000000.0
        ///    myShape.WriteShape()
        ///End Using
        /// </code>
        /// </example>
        /// <seealso cref="Open(System.String, System.Boolean)"/>
        /// <seealso cref="Open(System.String, eShapeType)"/>
        /// <seealso cref="Open(System.String, eShapeType, System.Boolean)"/>
        /// <seealso cref="Close"/>
        /// <seealso cref="ReadMode"/>
        public void Open(string filename)
        { OpenShape(filename, false, eShapeType.shpNull, false); }
        /// <summary>
        /// Creates a ShapeFile of a particular ShapeType ready for you to play with 
        /// </summary>
        /// <param name="filename">The name of the ShapeFile to create</param>
        /// <param name="shapetype">The type of ShapeFile to be created</param>
        /// <remarks>
        /// <para>This overload of the Open method creates a new ShapeFile or overwrites an existing ShapeFile.  Don't forget to define a field definition using the <see cref="O:ArcShapeFile.Fields.Add">Field.Add</see> method AND write them out with <see cref="WriteFieldDefs"/>.  If you don't then this beast will automatically create a integer field definition called SHAPE_ID to prevent you .DBF file from being corrupted.</para>
        /// <para>For a speed test of the various Open methods refer to the table in <see cref="ReadMode"/></para>
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", eShapeType.shpPoint);
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType);
        ///
        ///    // Add a new record
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Fields["TextField"].Value = "New Record";
        ///    myShape.Fields["NumField"].Value = 3.45;
        ///    myShape.Fields["DateField"].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///
        ///    // Wait a while as someone else is playing with the shape ...
        ///    System.Threading.Thread.Sleep(10000);
        ///
        ///    // Add a another new record
        ///    myShape.Vertices.Add(10, 15);
        ///    myShape.Fields[0].Value = "New Record 2";
        ///    myShape.Fields[1].Value = 4.45;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", eShapeType.shpPoint)
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType)
        ///
        ///    ' Add a new record
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Fields("TextField").Value = "New Record";
        ///    myShape.Fields("NumField").Value = 3.45;
        ///    myShape.Fields("DateField").Value = DateTime.Now;
        ///    myShape.WriteShape()
        ///
        ///    ' Wait a while as someone else is playing with the shape ...
        ///    System.Threading.Thread.Sleep(10000)
        ///
        ///    ' Add a another new record
        ///    myShape.Vertices.Add(10, 15)
        ///    myShape.Fields(0).Value = "New Record 2"
        ///    myShape.Fields(1).Value = 4.45
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.WriteShape()
        ///End Using
        /// </code>
        /// </example>
        /// <seealso cref="Open(System.String)"/>
        /// <seealso cref="Open(System.String, System.Boolean)"/>
        /// <seealso cref="Open(System.String, eShapeType, System.Boolean)"/>
        /// <seealso cref="Close"/>
        /// <seealso cref="ReadMode"/>
        public void Open(string filename, eShapeType shapetype)
        { OpenShape(filename, true, shapetype, false); }
        /// <summary>
        /// Opens an existing ShapeFile ready for viewing, adding or editing, the file handles of which are locked open.
        /// </summary>
        /// <param name="filename">The name of the ShapeFile to open for reading or editing</param>
        /// <param name="lockfile">Sets the lock condition of the ShapeFile handle.  If True
        /// the ShapeFiles will remain open until the Close() command is given otherwise the ShapeFiles
        /// are opened and closed with each Read and Write.</param>
        /// <remarks>
        /// <para>This overload of the Open method opens an existing ShapeFile for reading or editing.
        /// Set to the <paramref name="lockfile"/> status to True and the file locks on the .SHP, .SHX and .DBF file streams are held open between actions. This makes read and write opertations significantly faster. The lock is maintained until cancelled with the <see cref="Close"/> command.  If the lock isn't set then every move or update operation will create and release locks on the 3 file parts.  What this does do is allow other apps to update the ShapeFile between actions.  Your choice if you want this to happen.</para>
        /// <para>For a speed test of the various Open methods refer to the table in <see cref="ReadMode"/> </para>
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", true);
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType);
        ///
        ///    // Add a new record
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Fields["TextField"].Value = "New Record";
        ///    myShape.Fields["NumField"].Value = 3.45;
        ///    myShape.Fields["DateField"].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///
        ///    // Wait a while and nobody else can touch it while you have go for a cup of tea ...
        ///    System.Threading.Thread.Sleep(10000);
        ///
        ///    // Add a another new record
        ///    myShape.Vertices.Add(10, 15);
        ///    myShape.Fields[0].Value = "New Record 2";
        ///    myShape.Fields[1].Value = 4.45;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", True)
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType)
        ///
        ///    ' Add a new record
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Fields("TextField").Value = "New Record";
        ///    myShape.Fields("NumField").Value = 3.45;
        ///    myShape.Fields("DateField").Value = DateTime.Now;
        ///    myShape.WriteShape()
        ///
        ///    ' Wait a while and nobody else can touch it while you have go for a cup of tea ...
        ///    System.Threading.Thread.Sleep(10000)
        ///
        ///    ' Add a another new record
        ///    myShape.Vertices.Add(10, 15)
        ///    myShape.Fields(0).Value = "New Record 2"
        ///    myShape.Fields(1).Value = 4.45
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.WriteShape()
        ///End Using
        /// </code>
        /// </example>
        /// <seealso cref="Open(System.String)"/>
        /// <seealso cref="Open(System.String, eShapeType)"/>
        /// <seealso cref="Open(System.String, eShapeType, System.Boolean)"/>
        /// <seealso cref="Close"/>
        /// <seealso cref="ReadMode"/>
        public void Open(string filename, bool lockfile)
        { OpenShape(filename, false, eShapeType.shpNull, lockfile); }

        private void OpenShape(string filename, bool isNew, eShapeType shapetype, bool lockfile)
        {
            mvarParts = new Parts();
            mvarFields = new Fields();
            mvarVertices = new Vertices();
            mvarVertices.VerticeAdded += new Vertices.AddVertHandler(mvarVertices_VerticeAdded);
            mvarVertices.VerticeDeleted += new Vertices.DelVertHandler(mvarVertices_VerticeDeleted);
            mvarVertices.PartAdded += new Vertices.AddPartHandler(mvarVertices_PartAdded);
            mvarVertices.VerticesCleared += mvarVertices_VerticesCleared;

            mvarShapeArea = null;
            mvarCentroidX = 0;
            mvarCentroidY = 0;
            mvarPerimeter = 0;
            mvarPartXMin = 0;
            mvarPartXMax = 0;
            mvarPartYMin = 0;
            mvarPartYMax = 0;
            mvarShapeFileXMin = 0;
            mvarShapeFileXMax = 0;
            mvarShapeFileYMin = 0;
            mvarShapeFileYMax = 0;
            mvarShapeFileZMin = 0;
            mvarShapeFileZMax = 0;
            mvarShapeFileMMin = null;
            mvarShapeFileMMax = null;

            mvarAddShapeID = false;                         // parameter to check if an ID needs to be added to the DBF
            mvarNoMatch = true;                             // Find operation success parameter
            mvarsysDelimiter = SetDelimiter(eLanguage.OEM); // System Delimiter
            mvarYYYYMMDD = true;                            // YYYYMMDD Date Format
            mvarLockFile = lockfile;
            //mvarMemoFound = false;
            //mvarMemoNumber = 0;
            mvarShapeCount = 0;


            mvarShapeFile = filename;
            mvarShapeIndex = filename.Substring(0, filename.Length - 3) + "shx";
            mvarShapeDBF = filename.Substring(0, filename.Length - 3) + "dbf";
            LoadDatum(filename.Substring(0, filename.Length - 3) + "prj");


            if (!isNew)
            {
                if (!File.Exists(mvarShapeFile) | !File.Exists(mvarShapeIndex) | !File.Exists(mvarShapeDBF))
                { throw new Exception("You are trying to read a ShapeFile that does not exist or is missing a .shp, .shx or .dbf part"); }
            }
            else
            {
                mvarShapeType = Convert.ToInt32(shapetype);
                mvarRecordShapeType = mvarShapeType;
                try
                {
                    if (File.Exists(mvarShapeFile)) { File.Delete(mvarShapeFile); }
                    if (File.Exists(mvarShapeIndex)) { File.Delete(mvarShapeIndex); }
                    if (File.Exists(mvarShapeDBF)) { File.Delete(mvarShapeDBF); }
                }
                catch
                { throw new Exception("Cannot create the ShapeFile - Another application has locked it"); }

            }
            // ************************************************
            // * Create the Shape, Index and DBF File Streams *
            // ************************************************
            try
            { fsShapeFile = File.Open(mvarShapeFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite); }
            catch
            { throw new Exception("The Shape File " + mvarShapeFile + " has been locked by another application"); }
            try
            { fsShapeIndex = File.Open(mvarShapeIndex, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite); }
            catch
            { throw new Exception("The Shape Index File " + mvarShapeIndex + " has been locked by another application"); }
            try
            { fsDataFile = File.Open(mvarShapeDBF, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite); }
            catch
            { throw new Exception("The Database File " + mvarShapeDBF + " has been locked by another application"); }

            // **************************************
            // * Create the Shape and Index Headers * 
            // **************************************
            if (isNew)
            {
                WriteShapeHeader(fsShapeFile);
                WriteShapeHeader(fsShapeIndex);
                mvarShapeCount = 0;
            }
            else
            {
                ReadShapeHeader(fsShapeFile);
                mvarFields = ReadDBFHeader(fsDataFile);
                // Reset the status of the record
                foreach (Field mF in mvarFields)
                    mF.Status = null;
                mvarShapeCount = mvarFields.RecordCount;
            }

            //Close the filename handles
            if (!mvarLockFile) Close();

            // Raise event to show that the shapefile has been opened
            onShapeFileOpened(new ShapeFileOpenEventArgs(filename, mvarReadmode.ToString(), mvarLockFile));

            // Move to the first record
            if (!isNew)
            { MoveFirst(); }

        } //End Open

        /// <summary>
        /// Closes the handles to the opened ShapeFile
        /// </summary>
        /// <remarks>
        /// This method releases any locks on the .SHP, .SHX and .DBF files.
        /// If you've <see cref="O:ArcShapeFile.ShapeFile.Open">opened</see> your ShapeFile and set the lockfile parameter to True then you must use the Close method to release the file locks.
        /// To release the memory used by the internal variables and collections you will need to use <see cref="Dispose()"/>.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// ShapeFile myShape = new ShapeFile();
        /// myShape.Open(@"C:\Shapes\Point.shp", true);
        ///
        /// // Add a new record
        /// myShape.Vertices.Add(10, 10);
        /// myShape.Fields[0].Value = "New Record";
        /// myShape.Fields[1].Value = 3.45;
        /// myShape.Fields[2].Value = DateTime.Now;
        /// myShape.WriteShape();
        /// myShape.Close();
        /// myShape.Dispose();
        /// </code>
        /// <code lang="VB">
        /// myShape.Open(@"C:\Shapes\Point.shp", True)
        /// 
        /// ' Add a new record
        /// myShape.Vertices.Add(10, 10)
        /// myShape.Fields(0).Value = "New Record"
        /// myShape.Fields(1).Value = 3.45
        /// myShape.Fields(2).Value = DateTime.Now
        /// myShape.WriteShape()
        /// myShape.Close()
        /// myShape.Dispose()
        /// </code>
        /// </example>
        /// <seealso cref="O:ArcShapeFile.ShapeFile.Dispose"/>
        /// <seealso cref="O:ArcShapeFile.ShapeFile.Open"/>
        public void Close()
        {
            try { fsShapeFile.Close(); }
            catch { }
            try { fsShapeIndex.Close(); }
            catch { }
            try { fsDataFile.Close(); }
            catch { }
            try { fsShapeFile.Dispose(); }
            catch { }
            try { fsShapeIndex.Dispose(); }
            catch { }
            try { fsDataFile.Dispose(); }
            catch { }

            fsShapeFile = null;
            fsShapeIndex = null;
            fsDataFile = null;

        }
        #endregion

        #region **********          Create / Modify Methods       **********

        /// <summary>
        /// Writes out all data - both Vertice and Field - to the physical files
        /// </summary>
        /// <remarks>
        /// The WriteShape method does the actual writing of the ShapeFile record to disk. Until you use this command the entire Vertice and Field data are held in memory, so use it or loose it.  One thing
        /// to note is that after the WriteShape command is given the Vertices collection is emptied and the Values are stripped out of the Fields collection.  If you choose to discard the record before writing
        /// then it's a good idea to invoke the <see cref="M:ArcShapeFile.Vertices.Clear"/> command.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Polygon.shp", true);
        ///
        ///    // Add a new record
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Vertices.Add(10, 20);
        ///    myShape.Vertices.Add(20, 20);
        ///    myShape.Vertices.Add(20, 10);
        ///    myShape.Vertices.Add(10, 10);
        ///    myShape.Fields[0].Value = "New Record";
        ///    myShape.Fields[1].Value = 3.45;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///
        ///    // Ooops ... let's clear the vertices and try again
        ///    myShape.Vertices.Clear();
        ///    
        ///    // try again
        ///    myShape.Vertices.Add(15, 10);
        ///    myShape.Vertices.Add(15, 20);
        ///    myShape.Vertices.Add(20, 20);
        ///    myShape.Vertices.Add(20, 10);
        ///    myShape.Vertices.Add(15, 10);
        ///    myShape.WriteShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Polygon.shp", True)
        ///
        ///    ' Add a new record
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Vertices.Add(10, 20)
        ///    myShape.Vertices.Add(20, 20)
        ///    myShape.Vertices.Add(20, 10)
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Fields(0).Value = "New Record"
        ///    myShape.Fields(1).Value = 3.45
        ///    myShape.Fields(2).Value = DateTime.Now
        ///
        ///    ' Ooops ... let's clear the vertices and try again
        ///    myShape.Vertices.Clear();
        ///    
        ///    // try again
        ///    myShape.Vertices.Add(15, 10)
        ///    myShape.Vertices.Add(15, 20)
        ///    myShape.Vertices.Add(20, 20)
        ///    myShape.Vertices.Add(20, 10)
        ///    myShape.Vertices.Add(15, 10)
        ///    myShape.WriteShape()
        ///End Using
        /// </code>
        /// </example>
        /// <seealso cref="M:ArcShapeFile.Vertices.Clear"/>
        public void WriteShape()
        {
            // **********************************************************************
            // * Write a new ShapeFile Record to File based on the Collections Data *
            // **********************************************************************

            mvarShapeCount++;

            if (mvarFields.Count == 0)
            {
                //Add a shape ID to the collection if no fields have been defined
                mvarFields.Add("shape_id", eFieldType.shpNumeric, 6, 0);
                AppendFieldDefs(ref mvarFields, fsDataFile);
                mvarAddShapeID = true;
                mvarShapeCount = 1;
            }

            if (mvarAddShapeID == true)
            {
                mvarFields[0].Value = mvarShapeCount;
            }

            // I'll get to this bit in a moment
            WriteShapeRecord(mvarShapeCount);
            WriteDBFRecord(mvarShapeCount);
            mvarCurrentRecord = mvarShapeCount;
            // Shape record is written - create a new instance
            // Remove the old vertices
            mvarVertices.Clear();
            mvarParts.Clear();
            if (mvarRetainData == false)
            {
                mvarFields.Strip();
            }
            onShapeRecordCreated(new ShapeFileEventArgs(mvarShapeCount));
        }
        
        /// <summary>
        /// Appends a NULL shape record to the opened ShapeFile
        /// </summary>
        /// <remarks>
        /// Null Shape records are often used as placeholders and there are going to be times when you want to create an empty ShapeFile for use in another application.
        /// Well you are lucky 'cos when the WriteFieldDefs statement is given a Null Shape is automatically created (this record will be overwritten the first time you create a record).  Otherwise you can append a Null Shape record at anytime by issuing this command
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", true);
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType);
        ///
        ///    // Add a new record
        ///    myShape.Fields["TextField"].Value = "New Record";
        ///    myShape.Fields["NumField"].Value = 3.45;
        ///    myShape.Fields["DateField"].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///
        ///    // Add a placeholder record to fill in later
        ///    myShape.AddNullShape();
        ///
        ///    // Add a another new record
        ///    myShape.Vertices.Add(10, 15);
        ///    myShape.Fields[0].Value = "New Record 2";
        ///    myShape.Fields[1].Value = 4.45;
        ///    myShape.Fields[2].Value = DateTime.Now;
        ///    myShape.WriteShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", True)
        ///    Console.WriteLine("Shape Type: {0}", myShape.ShapeType)
        ///
        ///    ' Add a new record
        ///    myShape.Vertices.Add(10, 10)
        ///    myShape.Fields("TextField").Value = "New Record";
        ///    myShape.Fields("NumField").Value = 3.45;
        ///    myShape.Fields("DateField").Value = DateTime.Now;
        ///    myShape.WriteShape()
        ///
        ///    ' Add a placeholder record to fill in later
        ///    myShape.AddNullShape()
        ///
        ///    ' Add a another new record
        ///    myShape.Vertices.Add(10, 15)
        ///    myShape.Fields(0).Value = "New Record 2"
        ///    myShape.Fields(1).Value = 4.45
        ///    myShape.Fields(2).Value = DateTime.Now
        ///    myShape.WriteShape()
        ///End Using
        /// </code>
        /// </example>
        public void AddNullShape()
        {
            // *******************************
            // * Creates a NULL shape record *
            // *******************************
            int holdShapeType = mvarShapeType;
            mvarShapeType = Convert.ToInt32(eShapeType.shpNull);
            WriteShape();
            mvarShapeType = holdShapeType;
        }

        /// <summary>
        /// Copies the Vertice and Field values of a record defined by its ordinal position to another ShapeFile
        /// </summary>
        /// <param name="ShapeFileName">The name of the shapefile (.shp) that will recieve the record.</param>
        /// <param name="RecordNumber">The number of the record to be copied.</param>
        ///<remarks>
        ///The CopyTo method allows you to copy an entire record into a new or existing ShapeFile.  If the ShapeFile already exists than it must have the same ShapeType as the open one.  In
        ///this overload you get to define which record you want copied.  
        ///Any database fields that share the same names as those in your open ShapeFile will have the values copied across ... all others will be ignored.
        ///</remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", true);
        ///
        ///    // Copy the first record to another file
        ///    myShape.CopyTo(@"C:\Shapes\Point_Copy.shp", 0);
        ///    
        ///    // Copy the third record to another file
        ///    myShape.CopyTo(@"C:\Shapes\Point_Copy.shp", 2);
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", True)
        ///
        ///    ' Copy the first record to another file
        ///    myShape.CopyTo(@"C:\Shapes\Point_Copy.shp", 0);
        ///    
        ///    ' Copy the third record to another file
        ///    myShape.CopyTo(@"C:\Shapes\Point_Copy.shp", 2);
        ///    
        ///End Using
        /// </code>
        /// </example>
        public void CopyTo(string ShapeFileName, int RecordNumber)
        { CopyRecordTo(ShapeFileName, RecordNumber); }

        /// <summary>
        /// Copies the Vertice and Field values of the current record to another ShapeFile
        /// </summary>
        /// <param name="ShapeFileName">The name of the shapefile (.shp) that will recieve the record.</param>
        ///<remarks>
        ///The CopyTo method allows you to copy an entire record into a new or existing ShapeFile.  If the ShapeFile already exists than it must have the same ShapeType as the open one.  This is a sneeky way of creating a new ShapeFile huh?
        ///Any database fields that share the same names as those in your open ShapeFile will have the values copied across ... all others will be ignored.
        ///</remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", true);
        ///
        ///    for (int i=0; i &lt; myShape.RecordCount; i++)
        ///    {
        ///        // Copy the record to another file
        ///        myShape.CopyTo(@"C:\Shapes\Point_Copy.shp");
        ///    }
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", True)
        ///
        ///    For i as Integer = 0 To myShape.RecordCount - 1
        ///        ' Copy the record to another file
        ///        myShape.CopyTo(@"C:\Shapes\Point_Copy.shp");
        ///    Next i
        ///End Using
        /// </code>
        /// </example>
        ///<seealso cref="CopyFrom"/>
        public void CopyTo(string ShapeFileName)
        {
            CopyRecordTo(ShapeFileName, mvarCurrentRecord);
        }

        /// <summary>
        /// Copies the Vertice and Field values of records found by an SQL query to another ShapeFile
        /// </summary>
        /// <param name="ShapeFileName">The name of the shapefile (.shp) that will recieve the record.</param>
        /// <param name="Where">A Where clause to filter the input ShapeFile</param>
        ///<remarks>
        ///The CopyTo method allows you to copy an entire record into a new or existing ShapeFile.  If the ShapeFile already exists than it must have the same ShapeType as the open one.  In
        ///this overload you can define which record you want copied by using a where clause with the same comparisons shown in  the <see cref="FindFirst(System.String)">FindFirst</see> method.  
        ///Any database fields that share the same names as those in your open ShapeFile will have the values copied across ... all others will be ignored.
        ///</remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", true);
        ///
        ///    // Copy a selection of records to another file
        ///    myShape.CopyTo(@"C:\Shapes\Point_Copy.shp", "[TextField] like 'Record%' and [DateField] > 10 July 2010");
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", True)
        ///
        ///    ' Copy a selection of records to another file
        ///    myShape.CopyTo(@"C:\Shapes\Point_Copy.shp", "[TextField] like 'Record%' and [DateField] > 10 July 2010")
        ///    
        ///End Using
        /// </code>
        /// </example>
        ///<seealso cref="CopyFrom"/>
        public void CopyTo(string ShapeFileName, string Where)
        {
            FindFirst(Where);
            while (!mvarNoMatch)
            {
                CopyTo(ShapeFileName);
                FindNext();
            }
        }

        private void CopyRecordTo(string ShapeFileName, int RecordNumber)
        {
            // *******************************************
            // * Copy a record To another ShapeFile      *
            // *******************************************


            // For the entire Shape File
            double lvarShapeRecXMin;
            double lvarShapeRecXMax;
            double lvarShapeRecYMin;
            double lvarShapeRecYMax;
            double lvarShapeRecZMin;
            double lvarShapeRecZMax;
            double lvarShapeRecMMin;
            double lvarShapeRecMMax;
            int outRecordNumber;
            int outTotalRecords;
            int FilePos;
            eShapeType outShapeType;
            byte[] ByteArray = new byte[4];

            FileStream outShapeFile;
            FileStream outIndexFile;
            FileStream outDataFile;

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);
            OpenStream(mvarShapeDBF, ref fsDataFile);

            try
            {
                outShapeFile = new FileStream(ShapeFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                outIndexFile = new FileStream(ShapeFileName.Substring(0, ShapeFileName.Length - 3) + "shx", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                outDataFile = new FileStream(ShapeFileName.Substring(0, ShapeFileName.Length - 3) + "dbf", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            catch
            { throw new Exception("The output ShapeFile is locked by another application"); }

            // Ensure that the vertice data is loaded into the byte array
            LoadShapeRecord(RecordNumber);
            ReadShapeRecordHeader(mvarVertices.vertData);   // Read the MBR detrails from the array
            PopulateVerticeHeader(ref mvarVertices);        // mvarVertices has the data but not content length

            // Read the MBR of the output file
            if (outShapeFile.Length == 0)
            { // New Shape File
                WriteShapeHeader(outShapeFile);
                WriteShapeHeader(outIndexFile);
                bool _startEmpty = mvarStartEmpty;
                mvarStartEmpty = true;
                AppendFieldDefs(ref mvarFields, outDataFile);
                mvarStartEmpty = _startEmpty;
                outShapeType = (eShapeType)mvarShapeType;
                outTotalRecords = 0;
                outRecordNumber = 1;

                lvarShapeRecXMin = mvarShapeXMin;
                lvarShapeRecYMin = mvarShapeYMin;
                lvarShapeRecXMax = mvarShapeXMax;
                lvarShapeRecYMax = mvarShapeYMax;
                lvarShapeRecZMin = Convert.ToDouble(mvarShapeZMin);
                lvarShapeRecZMax = Convert.ToDouble(mvarShapeZMax);
                lvarShapeRecMMin = Convert.ToDouble(mvarShapeMMin);
                lvarShapeRecMMax = Convert.ToDouble(mvarShapeMMax);

            }
            else
            {
                // Read the header to ensure that the ShapeType is correct
                byte[] HeaderArray = new byte[100];
                // Read the initial 32 byte header
                outShapeFile.Seek(0, SeekOrigin.Begin);
                outShapeFile.Read(HeaderArray, 0, 100);
                outShapeType = (eShapeType)BitConverter.ToInt32(HeaderArray, 32);
                lvarShapeRecXMin = Math.Min(BitConverter.ToDouble(HeaderArray, 36), mvarShapeXMin);
                lvarShapeRecYMin = Math.Min(BitConverter.ToDouble(HeaderArray, 44), mvarShapeYMin);
                lvarShapeRecXMax = Math.Max(BitConverter.ToDouble(HeaderArray, 52), mvarShapeXMax);
                lvarShapeRecYMax = Math.Max(BitConverter.ToDouble(HeaderArray, 60), mvarShapeYMax);
                lvarShapeRecZMin = Math.Min(BitConverter.ToDouble(HeaderArray, 68), Convert.ToDouble(mvarShapeZMin));
                lvarShapeRecZMax = Math.Max(BitConverter.ToDouble(HeaderArray, 76), Convert.ToDouble(mvarShapeZMax));
                lvarShapeRecMMin = Math.Min(BitConverter.ToDouble(HeaderArray, 84), Convert.ToDouble(mvarShapeMMin));
                lvarShapeRecMMax = Math.Max(BitConverter.ToDouble(HeaderArray, 92), Convert.ToDouble(mvarShapeMMax));
                // Read the number of records int the DB
                outDataFile.Seek(4, SeekOrigin.Begin);
                outDataFile.Read(ByteArray, 0, 4);
                outTotalRecords = BitConverter.ToInt32(ByteArray, 0);
                outRecordNumber = outTotalRecords + 1;
            }

            if (outShapeType == (eShapeType)mvarShapeType)
            {

                // **********************************************
                // * Output Index Record                        *
                // **********************************************
                if (outRecordNumber == 1)
                {
                    FilePos = 46;
                    outIndexFile.Seek(100, SeekOrigin.Begin);
                }
                else
                {
                    // Read existing start position in Index File
                    FilePos = 100 + ((outRecordNumber - 1) * 8) - 8;
                    outIndexFile.Seek(FilePos, SeekOrigin.Begin);
                    // Start position of previous record
                    outIndexFile.Read(ByteArray, 0, 4);
                    Array.Reverse(ByteArray);
                    FilePos = BitConverter.ToInt32(ByteArray, 0);
                    // Read Content Length of previous record
                    outIndexFile.Read(ByteArray, 0, 4);
                    Array.Reverse(ByteArray);
                    // Combine to find Start position of current record
                    FilePos += BitConverter.ToInt32(ByteArray, 0);
                    // Start position of Shape File Write in DWORDs
                }

                // File Position

                ByteArray = BitConverter.GetBytes(FilePos + 4);
                Array.Reverse(ByteArray);
                outIndexFile.Write(ByteArray, 0, 4);


                // Content Length
                int ContentLength = (mvarVertices.vertData.Length) / 2;
                ByteArray = BitConverter.GetBytes(ContentLength);
                Array.Reverse(ByteArray);
                // Write new index record
                outIndexFile.Write(ByteArray, 0, 4);

                // Goto the start of the output record
                outShapeFile.Seek(GetShapeRecordStart(outRecordNumber, outIndexFile) - 4, SeekOrigin.Begin);
                // update the record number
                //outShapeFile.Seek(FilePos, SeekOrigin.Begin);
                ByteArray = BitConverter.GetBytes(outRecordNumber);
                Array.Reverse(ByteArray);
                outShapeFile.Write(ByteArray, 0, 4);
                ByteArray = BitConverter.GetBytes(ContentLength);
                Array.Reverse(ByteArray);
                outShapeFile.Write(ByteArray, 0, 4);

                // Write out Shape Record
                outShapeFile.Write(mvarVertices.vertData, 0, mvarVertices.vertData.Length);


                // ********************************
                // * Update the Shape file header *
                // ********************************

                // File Length of ShapeFile in WORDS (2 bytes)
                long FileLength = Convert.ToInt32(outShapeFile.Length);
                ByteArray = BitConverter.GetBytes(Convert.ToInt32(FileLength / 2));
                Array.Reverse(ByteArray);
                outShapeFile.Seek(24, SeekOrigin.Begin);
                outShapeFile.Write(ByteArray, 0, 4);

                // File Length of fsShapeIndex in WORDS (2 bytes)
                FileLength = Convert.ToInt32(outIndexFile.Length);
                ByteArray = BitConverter.GetBytes(Convert.ToInt32(FileLength / 2));
                Array.Reverse(ByteArray);
                outIndexFile.Seek(24, SeekOrigin.Begin);
                outIndexFile.Write(ByteArray, 0, 4);

                // Update the MBRs of the headers
                outShapeFile.Seek(36, SeekOrigin.Begin);
                outIndexFile.Seek(36, SeekOrigin.Begin);
                byte[] DblBytes = new byte[8];

                DblBytes = BitConverter.GetBytes(lvarShapeRecXMin);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);

                DblBytes = BitConverter.GetBytes(lvarShapeRecYMin);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);

                DblBytes = BitConverter.GetBytes(lvarShapeRecXMax);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);

                DblBytes = BitConverter.GetBytes(lvarShapeRecYMax);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);

                DblBytes = BitConverter.GetBytes(lvarShapeRecZMin);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);

                DblBytes = BitConverter.GetBytes(lvarShapeRecZMax);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);

                DblBytes = BitConverter.GetBytes(lvarShapeRecMMin);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);

                DblBytes = BitConverter.GetBytes(lvarShapeRecMMax);
                outShapeFile.Write(DblBytes, 0, 8);
                outIndexFile.Write(DblBytes, 0, 8);


                // ***********************************************************
                // * Output the Database Record - Match fields where you can *
                // ***********************************************************


                // compare the two dbfHeaders
                outDataFile.Seek(0, SeekOrigin.Begin);
                byte[] dbfHeader = new byte[32];
                outDataFile.Read(dbfHeader, 0, 32);

                // Header Length
                Int16 outHeaderLength = BitConverter.ToInt16(dbfHeader, 8);

                bool dbfIsSame = false;
                if (mvarFields.HeaderLength == outHeaderLength)
                {
                    // header lengths are the same ... are the contents?
                    fsDataFile.Seek(32, SeekOrigin.Begin);
                    byte[] hdrComp1 = new byte[mvarFields.HeaderLength - 32];
                    fsDataFile.Read(hdrComp1, 0, hdrComp1.Length);
                    byte[] hdrComp2 = new byte[outHeaderLength - 32];
                    outDataFile.Seek(32, SeekOrigin.Begin);
                    outDataFile.Read(hdrComp2, 0, hdrComp2.Length);

                    // Compare the converted strings
                    if (BitConverter.ToString(hdrComp1, 0, hdrComp1.Length) == BitConverter.ToString(hdrComp2, 0, hdrComp2.Length))
                    { dbfIsSame = true; }
                }

                if (dbfIsSame)
                {
                    // Database is the same format simply read the dataline and write it out
                    FilePos = mvarFields.HeaderLength + ((mvarFields.Recordlength * (RecordNumber - 1)));
                    byte[] DataArray = new byte[mvarFields.Recordlength];
                    fsDataFile.Seek(FilePos, SeekOrigin.Begin);
                    fsDataFile.Read(DataArray, 0, DataArray.Length);
                    FilePos = outHeaderLength + ((mvarFields.Recordlength * (outRecordNumber - 1)));
                    outDataFile.Seek(FilePos, SeekOrigin.Begin);
                    outDataFile.Write(DataArray, 0, DataArray.Length);
                    // Finish off the record
                    if (outTotalRecords < outRecordNumber)
                    {
                        DataArray[0] = 26;
                        outDataFile.Write(DataArray, 0, 1);
                        outDataFile.Seek(4, SeekOrigin.Begin);
                        ByteArray = BitConverter.GetBytes(outRecordNumber);
                        outDataFile.Write(ByteArray, 0, 4);

                        // Update the number of records int the DB
                        ByteArray = BitConverter.GetBytes(outRecordNumber);
                        outDataFile.Seek(4, SeekOrigin.Begin);
                        outDataFile.Write(ByteArray, 0, 4);

                    }
                }
                else
                {
                    // Ensure that the values have been populated into the Fields collection
                    if (mvarReadmode == eReadMode.HeaderOnly)
                        LoadDBFData();

                    // Copy across the fields that match by field name
                    Fields outFields = ReadDBFHeader(outDataFile);
                    foreach (Field oField in outFields)
                    {
                        for (int i = 0; i < mvarFields.Count; i++)
                        {
                            if (oField.Name == mvarFields[i].Name)
                            {
                                string DataValue = "";
                                if (mvarFields[i].Value != null)
                                {
                                    if (mvarFields[i].Type == eFieldType.shpDate)
                                    {
                                        DateTime dtTime = Convert.ToDateTime(mvarFields[i].Value);
                                        DataValue = dtTime.ToString("yyyyMMdd");
                                    }
                                    else
                                    { DataValue = mvarFields[i].Value.ToString(); }

                                    switch ((eFieldType)oField.Type)
                                    {
                                        case eFieldType.shpBoolean:
                                            oField.Value = Convert.ToBoolean(DataValue);
                                            break;

                                        case eFieldType.shpDate:
                                            oField.Value = Convert.ToDateTime(DataValue);
                                            break;

                                        case eFieldType.shpNumeric:
                                        case eFieldType.shpDouble:
                                        case eFieldType.shpFloat:
                                            oField.Value = Convert.ToDouble(DataValue);
                                            break;

                                        case eFieldType.shpInteger:
                                            oField.Value = Convert.ToInt32(DataValue);
                                            break;

                                        case eFieldType.shpLong:
                                            oField.Value = Convert.ToInt64(DataValue);
                                            break;

                                        case eFieldType.shpSingle:
                                            oField.Value = Convert.ToSingle(DataValue);
                                            break;

                                        default:
                                            oField.Value = DataValue;
                                            break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    WriteDBFRecord(outRecordNumber, outFields, outDataFile);

                }


                outShapeFile.Close();
                outShapeFile.Dispose();
                outIndexFile.Close();
                outIndexFile.Dispose();
                outDataFile.Close();
                outDataFile.Dispose();
                CloseStream(ref fsShapeFile);
                CloseStream(ref fsShapeIndex);
                CloseStream(ref fsDataFile);

            }
            else
            {
                outShapeFile.Close();
                outShapeFile.Dispose();
                outIndexFile.Close();
                outIndexFile.Dispose();
                outDataFile.Close();
                outDataFile.Dispose();
                CloseStream(ref fsShapeFile);
                CloseStream(ref fsShapeIndex);
                CloseStream(ref fsDataFile);
                throw new Exception("Invalid Operation:  The ShapeType of output ShapeFile does not match that of the input ShapeFile");
            }

        }

        /// <summary>
        /// Copies the Vertice and Field values from another ShapeFile into the current open ShapeFile
        /// </summary>
        /// <param name="ShapeFileName">The name of the shapefile (.shp) that has the record.</param>
        /// <param name="RecordNumber">The number of the record to be copied.</param>
        ///<remarks>
        ///The CopyFrom method allows you to copy either a Null Shape or a shape of the same ShapeType to your currently opened ShapeFile.
        ///Any database fields that share the same names as those in your open ShapeFile will have the values copied across ... all others will be ignored.
        ///</remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Point.shp", true);
        ///
        ///    // Copy the third record from another file
        ///    myShape.CopyFrom(@"C:\Shapes\Point_Extra.shp", 2);
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open(@"C:\Shapes\Point.shp", True)
        ///
        ///    ' Copy the third record from another file
        ///    myShape.CopyTo(@"C:\Shapes\Point_Extra.shp", 2);
        ///    
        ///End Using
        /// </code>
        /// </example>
        ///<seealso cref="O:ArcShapeFile.ShapeFile.CopyTo"/>
        public void CopyFrom(string ShapeFileName, int RecordNumber)
        {
            byte[] ByteArray = new byte[4];
            FileStream inShapeFile;
            FileStream inIndexFile;
            FileStream inDataFile;

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);
            OpenStream(mvarShapeDBF, ref fsDataFile);

            try
            {
                inShapeFile = new FileStream(ShapeFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                inIndexFile = new FileStream(ShapeFileName.Substring(0, ShapeFileName.Length - 3) + "shx", FileMode.Open, FileAccess.Read, FileShare.Read);
                inDataFile = new FileStream(ShapeFileName.Substring(0, ShapeFileName.Length - 3) + "dbf", FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch
            { throw new Exception("The input ShapeFile is locked by another application"); }


            //Ensure that the Shapefile types are the same
            inShapeFile.Seek(32, SeekOrigin.Begin);
            inShapeFile.Read(ByteArray, 0, 4);
            int inShapeType = BitConverter.ToInt32(ByteArray, 0);

            if (inShapeType != mvarShapeType)
            {
                inShapeFile.Close();
                inShapeFile.Dispose();
                inIndexFile.Close();
                inIndexFile.Dispose();
                inDataFile.Close();
                inDataFile.Dispose();
                throw new Exception("The input ShapeFile has a different type from the open ShapeFile");
            }

            // Ensure that the Record number is correct
            inDataFile.Seek(4, SeekOrigin.Begin);
            inDataFile.Read(ByteArray, 0, 4);
            int inTotalRecords = BitConverter.ToInt32(ByteArray, 0);
            if (RecordNumber > inTotalRecords | RecordNumber < 1)
            {
                inShapeFile.Close();
                inShapeFile.Dispose();
                inIndexFile.Close();
                inIndexFile.Dispose();
                inDataFile.Close();
                inDataFile.Dispose();
                throw new Exception("The Record number given lays outside the record bounds of the input ShapeFile");
            }

            // *****************************
            // * ShapeFile & Index Section *
            // *****************************

            // Load the input ShapeFile Vertice Data
            long FilePos = GetShapeRecordStart(RecordNumber, inIndexFile);
            inShapeFile.Seek(FilePos, SeekOrigin.Begin);
            inShapeFile.Read(ByteArray, 0, 4);
            Array.Reverse(ByteArray);
            int ContentLength = BitConverter.ToInt32(ByteArray, 0);
            byte[] inVertData = new byte[ContentLength * 2];
            inShapeFile.Read(inVertData, 0, inVertData.Length);

            inShapeFile.Close();
            inShapeFile.Dispose();
            inIndexFile.Close();
            inIndexFile.Dispose();

            // Get starting position of the new record
            mvarShapeCount++;
            //FilePos = GetShapeRecordStart(mvarShapeCount, fsShapeIndex);

            // **********************************************
            // * Index Record                               *
            // **********************************************
            // File Position
            FilePos = 50;
            fsShapeIndex.Seek(100, SeekOrigin.Begin);
            if (mvarShapeCount > 1)
            {
                fsShapeIndex.Seek(100 + ((mvarShapeCount - 2) * 8), SeekOrigin.Begin);
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                FilePos = BitConverter.ToInt32(ByteArray, 0);
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                FilePos += BitConverter.ToInt32(ByteArray, 0) + 4;
            }

            fsShapeFile.Seek(FilePos * 2, SeekOrigin.Begin);
            ByteArray = BitConverter.GetBytes(mvarShapeCount);
            Array.Reverse(ByteArray);
            fsShapeFile.Write(ByteArray, 0, 4);

            ByteArray = BitConverter.GetBytes(Convert.ToInt32(FilePos));
            Array.Reverse(ByteArray);
            fsShapeIndex.Write(ByteArray, 0, 4);

            // Insert the Shape and Index Record Headers
            ByteArray = BitConverter.GetBytes(ContentLength);
            Array.Reverse(ByteArray);
            fsShapeFile.Write(ByteArray, 0, 4);
            fsShapeIndex.Write(ByteArray, 0, 4);


            // Insert the Vertice Data
            fsShapeFile.Write(inVertData, 0, inVertData.Length);

            // File Length of Shape File in WORDS (2 bytes)
            long FileLength = Convert.ToInt32(fsShapeFile.Length);
            ByteArray = BitConverter.GetBytes(Convert.ToInt32(FileLength / 2));
            Array.Reverse(ByteArray);
            fsShapeFile.Seek(24, SeekOrigin.Begin);
            fsShapeFile.Write(ByteArray, 0, 4);

            // File Length of Index File in WORDS (2 bytes)
            FileLength = Convert.ToInt32(fsShapeIndex.Length);
            ByteArray = BitConverter.GetBytes(Convert.ToInt32(FileLength / 2));
            Array.Reverse(ByteArray);
            fsShapeIndex.Seek(24, SeekOrigin.Begin);
            fsShapeIndex.Write(ByteArray, 0, 4);

            // Update the ShapeFile MBR Data
            ReadShapeRecordHeader(inVertData);
            if (mvarShapeCount > 1)
            {
                mvarShapeFileXMin = Math.Min(mvarShapeFileXMin, mvarShapeXMin);
                mvarShapeFileXMax = Math.Max(mvarShapeFileXMax, mvarShapeXMax);
                mvarShapeFileYMin = Math.Min(mvarShapeFileYMin, mvarShapeYMin);
                mvarShapeFileYMax = Math.Max(mvarShapeFileYMax, mvarShapeYMax);
                mvarShapeFileZMin = Math.Min(Convert.ToDouble(mvarShapeFileZMin), Convert.ToDouble(mvarShapeZMin));
                mvarShapeFileZMax = Math.Max(Convert.ToDouble(mvarShapeFileZMax), Convert.ToDouble(mvarShapeZMax));

                if (mvarShapeMMin != null)
                {
                    if (mvarShapeMMin > -1E+38)
                    {
                        if (mvarShapeFileMMin != null)
                        { mvarShapeFileMMin = Math.Min(Convert.ToDouble(mvarShapeFileMMin), Convert.ToDouble(mvarShapeMMin)); }
                        else
                        { mvarShapeFileMMin = mvarShapeMMin; }
                    }
                }
                if (mvarShapeMMax != null)
                {
                    mvarShapeFileMMax = Math.Max(Convert.ToDouble(mvarShapeFileMMin), Convert.ToDouble(mvarShapeMMin));
                    if (mvarShapeFileMMax <= -1E+38)
                    { mvarShapeFileMMax = null; }
                }
            }
            else
            {
                mvarShapeFileXMin = mvarShapeXMin;
                mvarShapeFileXMax = mvarShapeXMax;
                mvarShapeFileYMin = mvarShapeYMin;
                mvarShapeFileYMax = mvarShapeYMax;
                mvarShapeFileZMin = mvarShapeZMin;
                mvarShapeFileZMax = mvarShapeZMax;
                mvarShapeFileMMin = mvarShapeMMin;
                mvarShapeFileMMax = mvarShapeMMax;
            }

            byte[] DblBytes = new byte[8];
            fsShapeFile.Seek(36, SeekOrigin.Begin);
            fsShapeIndex.Seek(36, SeekOrigin.Begin);
            DblBytes = BitConverter.GetBytes(mvarShapeFileXMin);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileYMin);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileXMax);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileYMax);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileZMin));
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileZMax));
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileMMin));
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileMMax));
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);


            // ********************
            // * Database Section *
            // ********************
            Int16 inHeaderLength = 0;
            Int16 inRecordLength = 0;
            byte[] hdrComp2 = null;
            bool dbfIsSame = false;

            // Field Defs haven't been created yet ... load the one from the input database
            if (mvarFields.Count == 0)
            {
                mvarFields = ReadDBFHeader(inDataFile);
                byte[] HeaderArray = new byte[mvarFields.HeaderLength];
                inDataFile.Seek(0, SeekOrigin.Begin);
                inDataFile.Read(HeaderArray, 0, HeaderArray.Length);
                fsDataFile.Seek(0, SeekOrigin.Begin);
                fsDataFile.Write(HeaderArray, 0, HeaderArray.Length);
                inHeaderLength = mvarFields.HeaderLength;
                dbfIsSame = true;
            }
            else
            {

                // compare the two dbfHeaders
                inDataFile.Seek(0, SeekOrigin.Begin);
                byte[] dbfHeader = new byte[32];
                inDataFile.Read(dbfHeader, 0, 32);

                // Header Length
                inHeaderLength = BitConverter.ToInt16(dbfHeader, 8);
                inRecordLength = BitConverter.ToInt16(dbfHeader, 10);

                // header lengths are the same ... are the contents?
                fsDataFile.Seek(32, SeekOrigin.Begin);
                byte[] hdrComp1 = new byte[mvarFields.HeaderLength - 32];
                fsDataFile.Read(hdrComp1, 0, hdrComp1.Length);
                hdrComp2 = new byte[inHeaderLength - 32];
                inDataFile.Seek(32, SeekOrigin.Begin);
                inDataFile.Read(hdrComp2, 0, hdrComp2.Length);

                // Compare the converted strings
                if (BitConverter.ToString(hdrComp1, 0, hdrComp1.Length) == BitConverter.ToString(hdrComp2, 0, hdrComp2.Length))
                { dbfIsSame = true; }
            }

            // Read the input Database Record
            FilePos = inHeaderLength + ((inRecordLength * (RecordNumber - 1)));
            byte[] DataArray = new byte[mvarFields.Recordlength];
            // Fill the output line with spaces
            inDataFile.Seek(FilePos, SeekOrigin.Begin);
            inDataFile.Read(DataArray, 0, DataArray.Length);

            FilePos = mvarFields.HeaderLength + ((mvarFields.Recordlength * (mvarShapeCount - 1)));
            fsDataFile.Seek(FilePos, SeekOrigin.Begin);

            if (dbfIsSame)
            {
                // Database is the same format simply read the dataline and write it out
                fsDataFile.Write(DataArray, 0, DataArray.Length);
            }
            else
            {

                int inFieldCount = (inHeaderLength / 32) - 1;
                string[] inFieldName = new string[inFieldCount];
                int[] inFieldSize = new int[inFieldCount];
                int dbfFilePos = 0;
                byte[] lvarFieldName = new byte[10];

                for (int i = 0; i < inFieldCount; i++)
                {
                    //Fieldname is null terminated
                    Buffer.BlockCopy(hdrComp2, dbfFilePos, lvarFieldName, 0, 10);
                    // Assumes input and output codepages are the same
                    inFieldName[i] = ByteArrayToString(lvarFieldName);
                    inFieldSize[i] = hdrComp2[dbfFilePos + 16];
                    dbfFilePos += 32;
                }


                // Copy the fields that match

                byte[] outDBFRecord = new byte[mvarFields.Recordlength];
                int outDBFPos = 1;
                int outFilePos;
                outDBFRecord[0] = DataArray[0];  // Delete or Current Flag
                for (int i = 1; i < outDBFRecord.Length; i++)
                { outDBFRecord[i] = 32; }

                for (int i = 0; i < mvarFields.Count; i++)
                {
                    Field mField = mvarFields[i];
                    outFilePos = 1;
                    for (int j = 0; j < inFieldCount; j++)
                    {
                        if (inFieldName[j] == mField.Name)
                        {
                            byte[] sFieldValue = new byte[mField.Size];
                            // Copy the data value into a string
                            if (mField.Size >= inFieldSize[i])
                            {
                                Buffer.BlockCopy(DataArray, outFilePos, sFieldValue, 0, inFieldSize[i]);
                            }
                            else
                            {
                                Buffer.BlockCopy(DataArray, outFilePos, sFieldValue, 0, mField.Size);
                            }
                            // Copy resized values to output array
                            Buffer.BlockCopy(sFieldValue, 0, outDBFRecord, outDBFPos, sFieldValue.Length);
                            break;
                        }
                        outFilePos += inFieldSize[j];
                    }
                    outDBFPos += mField.Size;
                }
                fsDataFile.Write(outDBFRecord, 0, mvarFields.Recordlength);


            }

            // Finish off the record
            ByteArray[0] = 26;
            fsDataFile.Write(ByteArray, 0, 1);
            fsDataFile.Seek(0, SeekOrigin.Begin);
            ByteArray[0] = 3;
            ByteArray[1] = Convert.ToByte(DateTime.Now.Year - 1900);
            ByteArray[2] = Convert.ToByte(DateTime.Now.Month);
            ByteArray[3] = Convert.ToByte(DateTime.Now.Day);
            fsDataFile.Write(ByteArray, 0, 4);
            ByteArray = BitConverter.GetBytes(mvarShapeCount);
            fsDataFile.Write(ByteArray, 0, 4);

            inDataFile.Close();
            inDataFile.Dispose();
            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);
            CloseStream(ref fsDataFile);

        }

        ///<summary>
        ///Writes out any changed Vertice or Field data to the respective files.
        ///</summary>
        ///<remarks>
        ///Obviously, you would only use this method if you've modified the data in your ShapeFile record.  With this command the fun really begins. If you have deleted or added vertices to your shape record then the .SHP and .SHX files will be overwritten.   Similarly if you've modified the attribute values or characteristics then the .DBF needs to be re-writen. This isn't as scary as it sounds, but for big files this may be memory and IO intensive so may take a bit of time. As always, altering files is a risky business so backing up your ShapeFile before you play with it would be a good idea.
        ///</remarks>
        /// <example>
        /// <code lang="C#">
        ///using (ShapeFile myShape = new ShapeFile())
        ///{
        ///    myShape.Open(@"C:\Shapes\Polygon.shp", true);
        ///    // Add a vertice
        ///    myShape.MoveTo(2);
        ///    // insert a vertice at ordinal position 6
        ///    myShape.Vertices.Add(16, 8, (int)6);
        ///    myShape.WriteShape();
        ///
        ///    // Modify a database value
        ///    myShape.Fields[0].Value = "Changed Value";
        ///    myShape.ModifyShape();
        ///
        ///    // Remove the vertice at the ordinal position 7
        ///    myShape.Vertices.RemoveAt(7);
        ///    myShape.ModifyShape();
        ///}
        /// </code>
        /// <code lang="VB">
        ///Using myShape As New ShapeFile()
        ///    myShape.Open("C:\Shapes\Polygon.shp", true)
        ///    ' Add a vertice
        ///    myShape.MoveTo(2)
        ///    ' insert a vertice at ordinal position 6
        ///    myShape.Vertices.Add(16, 8, (int)6)
        ///    myShape.WriteShape()
        ///
        ///    ' Modify a database value
        ///    myShape.Fields(0).Value = "Changed Value"
        ///    myShape.ModifyShape()
        ///
        ///    ' Remove the vertice at the ordinal position 7
        ///    myShape.Vertices.RemoveAt(7)
        ///    myShape.ModifyShape()
        ///
        ///End Using
        /// </code>
        /// </example>
        public void ModifyShape()
        {

            // Write any database record changes
            if (Globals.mvarFieldChange)
            {
                WriteDBFRecord(mvarCurrentRecord);
                onShapeRecordModified(new ShapeFileModifyEventArgs(mvarCurrentRecord, "Fields"));

            }

            if (Globals.mvarVerticeChange)
            {

                if (mvarCurrentRecord < mvarShapeCount)
                {
                    OpenStream(mvarShapeFile, ref fsShapeFile);
                    OpenStream(mvarShapeIndex, ref fsShapeIndex);

                    // Get current content length of this Shape Record
                    long FilePos = GetShapeRecordStart(mvarCurrentRecord, fsShapeIndex);
                    long NextPos = GetShapeRecordStart(mvarCurrentRecord + 1, fsShapeIndex);
                    byte[] ByteArray = new byte[4];

                    fsShapeFile.Seek(FilePos, SeekOrigin.Begin);
                    fsShapeFile.Read(ByteArray, 0, 4);
                    Array.Reverse(ByteArray);
                    int ContentLength = BitConverter.ToInt32(ByteArray, 0) * 2;

                    // Calculate length of file and addition factor
                    long LengthOfFile = fsShapeFile.Length;
                    int NewContentLength = CalcContentLength() * 2;
                    int AddFactor = NewContentLength - ContentLength;

                    if (AddFactor != 0)
                    {
                        // Read in the rest of the file
                        fsShapeFile.Seek(NextPos, SeekOrigin.Begin);
                        byte[] moveData = new byte[(LengthOfFile - NextPos)];
                        fsShapeFile.Read(moveData, 0, moveData.Length);

                        // Write the held data allowing for the difference in data size
                        NextPos += AddFactor;
                        fsShapeFile.Seek(NextPos, SeekOrigin.Begin);
                        fsShapeFile.Write(moveData, 0, moveData.Length);

                        // Re-write the Index Records
                        FilePos = 100 + ((mvarCurrentRecord - 1) * 8);
                        fsShapeIndex.Seek(FilePos, SeekOrigin.Begin);

                        ByteArray = BitConverter.GetBytes((int)(NewContentLength / 2));
                        Array.Reverse(ByteArray);
                        fsShapeIndex.Write(ByteArray, 0, 4);

                        for (int i = mvarCurrentRecord; i < mvarShapeCount; i++)
                        {
                            FilePos += 8;
                            fsShapeIndex.Seek(FilePos, SeekOrigin.Begin);
                            fsShapeIndex.Read(ByteArray, 0, 4);
                            Array.Reverse(ByteArray);
                            int iTempVal = BitConverter.ToInt32(ByteArray, 0);
                            iTempVal += AddFactor / 2;
                            ByteArray = BitConverter.GetBytes(iTempVal);
                            Array.Reverse(ByteArray);
                            fsShapeIndex.Seek(FilePos, SeekOrigin.Begin);
                            fsShapeIndex.Write(ByteArray, 0, 4);
                        }
                    }

                    if (AddFactor < 0)
                    { fsShapeFile.SetLength(LengthOfFile + AddFactor); }

                    CloseStream(ref fsShapeIndex);
                    CloseStream(ref fsShapeFile);
                }

                WriteShapeRecord(mvarCurrentRecord);
                // re-read the record to ensure that any part info is correctly displayed
                MoveTo(mvarCurrentRecord);
                onShapeRecordModified(new ShapeFileModifyEventArgs(mvarCurrentRecord, "Vertices"));

            }
            Globals.mvarFieldChange = false;
            Globals.mvarVerticeChange = false;

        }

        /// <summary>
        /// Rewrites the ShapeFile parts removing all deleted and Null records
        /// </summary>
        /// <remarks>
        /// <para>The Pack method physically removes any data marked by the <see cref="DeleteShape"/> method from the .SHP and .DBF files.
        /// Any record that has a record shape type set to 0 (i.e. a Null record) or has the database record flagged as being deleted will be ignored during the re-write process.</para>
        /// <para>Why do you need this method?  If you've been using this DLL to delete records then the record is physically still in the file structure it's just that the ShapeType has been set to a Null and the delete flag on the DBF record has been set.  Pack completly rewrites the .SHP, .SHX and .DBF 
        /// files and skips the Nulls.</para>
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///
        ///     // Remove a new record
        ///     myShape.MoveTo(2);
        ///     myShape.DeleteShape();
        ///
        ///     // Make the changes permanent
        ///     myShape.Pack();
        /// }
        ///</code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        ///
        ///     myShape.Open("C:\Shapes\Point.shp")
        ///
        ///     ' Remove a new record
        ///     myShape.MoveTo(2)
        ///     myShape.DeleteShape()
        ///
        ///     ' Make the changes permanent
        ///     myShape.Pack()
        ///
        /// End Using
        ///</code></example>
        ///<seealso cref="DeleteShape"/>
        public void Pack()
        {
            // *********************************************************
            // * Pack down the shapefile and dbf, regenerate the index *
            // *********************************************************

            int i = 0;
            int FilePos = 0;
            int DBFPos = 0;
            int RecCount = 0;
            int IndexOffset = 0;
            int ContentLength = 0;
            byte[] ByteArray = new byte[4];
            bool UseRecord = false;
            Int16 dbfHeaderLength = mvarFields.HeaderLength;
            Int16 dbfRecordLength = mvarFields.Recordlength;
            byte[] lvarDBFData = new byte[dbfRecordLength];
            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);
            OpenStream(mvarShapeDBF, ref fsDataFile);

            FileStream TempSHPFile = default(FileStream);
            FileStream TempSHXFile = default(FileStream);
            FileStream TempDBFFile = default(FileStream);
            string sDBFTemp = null;
            string sSHXTemp = null;
            string sSHPTemp = null;
            long LengthofFile = 0;

            // *******************************************
            // * Define input files                      *
            // *******************************************

            // Define Temporary Output files

            sSHPTemp = Path.GetTempFileName();
            TempSHPFile = File.Open(sSHPTemp, FileMode.Create, FileAccess.ReadWrite);

            sSHXTemp = Path.GetTempFileName();
            TempSHXFile = File.Open(sSHXTemp, FileMode.Create, FileAccess.ReadWrite);

            sDBFTemp = Path.GetTempFileName();
            TempDBFFile = File.Open(sDBFTemp, FileMode.Create, FileAccess.ReadWrite);


            // *******************************************
            // * Create the Headers                      *
            // *******************************************
            fsShapeFile.Seek(32, SeekOrigin.Begin);
            fsShapeFile.Read(ByteArray, 0, 4);
            mvarShapeType = BitConverter.ToInt32(ByteArray, 0);
            WriteShapeHeader(TempSHXFile);
            WriteShapeHeader(TempSHPFile);
            byte[] dbfHeader = new byte[dbfHeaderLength];
            fsDataFile.Seek(0, SeekOrigin.Begin);
            fsDataFile.Read(dbfHeader, 0, dbfHeaderLength);
            TempDBFFile.Write(dbfHeader, 0, dbfHeaderLength);
            TempSHXFile.Seek(100, SeekOrigin.Begin);
            TempSHPFile.Seek(100, SeekOrigin.Begin);

            // *******************************************
            // * Copy each not NULL and UNDELETED record *
            // *******************************************
            IndexOffset = 50;
            for (i = 1; i <= mvarShapeCount; i++)
            {
                // Read in the Index Position for the record
                FilePos = 100 + ((i - 1) * 8);
                fsShapeIndex.Seek(FilePos, SeekOrigin.Begin);

                // Find start of this Shape Record
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                FilePos = BitConverter.ToInt32(ByteArray, 0);

                // Get current content length of this Shape Record
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                ContentLength = BitConverter.ToInt32(ByteArray, 0);

                // Read the Shapetype of this record from the ShapeFile
                fsShapeFile.Seek((FilePos * 2) + 8, SeekOrigin.Begin);
                fsShapeFile.Read(ByteArray, 0, 4);
                mvarRecordShapeType = BitConverter.ToInt32(ByteArray, 0);

                // Goto start of DBF record
                DBFPos = dbfHeaderLength + ((dbfRecordLength * (i - 1)));
                fsDataFile.Seek(DBFPos, SeekOrigin.Begin);
                fsDataFile.Read(lvarDBFData, 0, dbfRecordLength);

                if (mvarRecordShapeType == 0)
                {
                    UseRecord = false;
                }
                else
                {
                    UseRecord = true;
                    // Read the Delete point of the DBF record
                    if (lvarDBFData[0] == 42)
                    {
                        UseRecord = false;
                    }
                }


                if (UseRecord)
                {
                    RecCount = RecCount + 1;

                    // Write the DBF record
                    TempDBFFile.Write(lvarDBFData, 0, dbfRecordLength);

                    // Read the Shapefile content
                    fsShapeFile.Seek(FilePos * 2, SeekOrigin.Begin);
                    byte[] lvarShapeData = new byte[(ContentLength * 2) + 8];
                    fsShapeFile.Read(lvarShapeData, 0, (ContentLength * 2) + 8);

                    // Update the Record number
                    ByteArray = BitConverter.GetBytes(RecCount);
                    Array.Reverse(ByteArray);
                    Buffer.BlockCopy(ByteArray, 0, lvarShapeData, 0, 4);


                    TempSHPFile.Write(lvarShapeData, 0, (ContentLength * 2) + 8);

                    // Write then Indexfile content
                    ByteArray = BitConverter.GetBytes(IndexOffset);
                    Array.Reverse(ByteArray);
                    TempSHXFile.Write(ByteArray, 0, 4);
                    ByteArray = BitConverter.GetBytes(ContentLength);
                    Array.Reverse(ByteArray);
                    TempSHXFile.Write(ByteArray, 0, 4);
                    IndexOffset = IndexOffset + ContentLength + 4;

                }
            }

            // ********************************************
            // * Update the Filelengths and record counts *
            // ********************************************
            TempSHPFile.Seek(24, SeekOrigin.Begin);
            LengthofFile = TempSHPFile.Length / 2;
            ByteArray = BitConverter.GetBytes(Convert.ToInt32(LengthofFile));
            Array.Reverse(ByteArray);
            TempSHPFile.Write(ByteArray, 0, 4);

            TempSHXFile.Seek(24, SeekOrigin.Begin);
            LengthofFile = TempSHXFile.Length / 2;
            ByteArray = BitConverter.GetBytes(Convert.ToInt32(LengthofFile));
            Array.Reverse(ByteArray);
            TempSHXFile.Write(ByteArray, 0, 4);

            TempDBFFile.Seek(4, SeekOrigin.Begin);
            ByteArray = BitConverter.GetBytes(RecCount);
            TempDBFFile.Write(ByteArray, 0, 4);

            ByteArray[0] = 26;
            // End of File Character (1Ah)
            TempDBFFile.Seek(0, SeekOrigin.End);
            TempDBFFile.Write(ByteArray, 0, 1);

            TempSHPFile.Close();
            TempSHXFile.Close();
            TempDBFFile.Close();
            fsShapeFile.Close();
            fsShapeIndex.Close();
            fsDataFile.Close();

            // ********************************************
            // * Copy the temporary files over            *
            // ********************************************
            File.Delete(mvarShapeFile);
            File.Move(sSHPTemp, mvarShapeFile);

            File.Delete(mvarShapeIndex);
            File.Move(sSHXTemp, mvarShapeIndex);

            File.Delete(mvarShapeDBF);
            File.Move(sDBFTemp, mvarShapeDBF);

            onShapePacked();
            //ShapeFilePacked();

            // ********************************************
            // * Reopen the shapefile to populate the MBRs*
            // ********************************************
            OpenShape(mvarShapeFile, false, eShapeType.shpNull, mvarLockFile);

        }

        /// <summary>
        /// Deletes a part including the vertices in a multipart shape.
        /// </summary>
        /// <param name="PartNo">The part number to be deleted</param>
        /// <remarks>Want to delete a whole within a polygon shape or a particular part of a shape record?  Then this method is for you.  Each ring (inner or outer) of a multipart shape is prepresented by a
        /// new <see cref="Part"/>.  This method removes the part from the collections memory.  To make this permanent the <see cref="ModifyShape"/> method must be used.  Remember the collections use a zero based ordinal to reference them.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     // Create a polygon with a donut
        ///     myShape.Open(@"C:\Shapes\Polygon.shp", eShapeType.shpPolygon);
        ///     myShape.Fields.Add("TextField", eFieldType.shpText);
        ///     myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2);
        ///     myShape.WriteFieldDefs();
        ///
        ///     myShape.Fields["TextField"].Value = "Polygon";
        ///     myShape.Fields[1].Value = 1.23;
        ///     // Outer shape (note the orientation is clockwise)
        ///     myShape.Vertices.Add(10, 1);
        ///     myShape.Vertices.Add(10, 10);
        ///     myShape.Vertices.Add(20, 10);
        ///     myShape.Vertices.Add(20, 1);
        ///     myShape.Vertices.Add(10, 1);
        ///     // inner shape (note the orientation is anti-clockwise)
        ///     myShape.Vertices.NewPart();
        ///     myShape.Vertices.Add(14, 4);
        ///     myShape.Vertices.Add(14, 6);
        ///     myShape.Vertices.Add(16, 6);
        ///     myShape.Vertices.Add(16, 4);
        ///     myShape.Vertices.Add(14, 4);
        ///     myShape.WriteShape();
        ///
        ///     // Remove the hole
        ///     myShape.DeletePart(1);
        ///     // Write the changes
        ///     myShape.ModifyShape();
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        ///    	' Create a polygon with a donut
        ///    	myShape.Open("C:\Shapes\Polygon.shp", eShapeType.shpPolygon)
        ///    	myShape.Fields.Add("TextField", eFieldType.shpText)
        ///    	myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2)
        ///    	myShape.WriteFieldDefs()
        ///
        ///    	myShape.Fields("TextField").Value = "Polygon"
        ///    	myShape.Fields(1).Value = 1.23
        ///    	' Outer shape (note the orientation is clockwise)
        ///    	myShape.Vertices.Add(10, 1)
        ///    	myShape.Vertices.Add(10, 10)
        ///    	myShape.Vertices.Add(20, 10)
        ///    	myShape.Vertices.Add(20, 1)
        ///    	myShape.Vertices.Add(10, 1)
        ///    	' inner shape (noote the orientation is anti-clockwise)
        ///    	myShape.Vertices.NewPart()
        ///    	myShape.Vertices.Add(14, 4)
        ///    	myShape.Vertices.Add(14, 6)
        ///    	myShape.Vertices.Add(16, 6)
        ///    	myShape.Vertices.Add(16, 4)
        ///    	myShape.Vertices.Add(14, 4)
        ///    	myShape.WriteShape()
        ///
        ///    	' Remove the hole
        ///    	myShape.DeletePart(1)
        ///    	' Write the changes
        ///    	myShape.ModifyShape()
        /// End Using
        /// </code>
        /// </example>
        ///<seealso cref="DeleteShape"/>
        ///<seealso cref="ModifyShape"/>
        public void DeletePart(int PartNo)
        {
            if (PartNo < 0 | PartNo > mvarParts.Count - 1)
            {
                throw new Exception("You are trying to delete a part that does not exist");
            }
            else if (mvarNoOfParts == 0)
            {
                // only one part so remove the entire record
                DeleteShape();
            }

            // Remove all vertices belonging to this part
            int noOfVertsDeleted = (mvarParts[PartNo].Ends - mvarParts[PartNo].Begins) + 1;
            for (int i = mvarParts[PartNo].Ends; i <= mvarParts[PartNo].Begins; i--)
                mvarVertices.RemoveAt(i);
            mvarNoOfPoints = mvarVertices.Count;

            // Remove all parts belonging to this part
            bool recalcMBR = !mvarParts[PartNo].IsHole;
            mvarParts.RemoveAt(PartNo);

            for (int i = PartNo; i < mvarParts.Count; i++)
            {
                mvarParts[i].Begins -= noOfVertsDeleted;
                mvarParts[i].Ends -= noOfVertsDeleted;
            }
            mvarNoOfParts -= 1;
            mvarVertices.NoOfPoints = mvarVertices.Count;

            if (recalcMBR)
            {
                // Recalculate the shape MBR
                double lvarShapeXMin = mvarParts[0].MBRXMin;
                double lvarShapeXMax = mvarParts[0].MBRXMax;
                double lvarShapeYMin = mvarParts[0].MBRYMin;
                double lvarShapeYMax = mvarParts[0].MBRYMax;
                double? lvarShapeZMin = mvarParts[0].zMin;
                double? lvarShapeZMax = mvarParts[0].zMax;
                double? lvarShapeMMin = mvarParts[0].MeasureMin;
                double? lvarShapeMMax = mvarParts[0].MeasureMax;
                for (int i = 1; i < mvarParts.Count; i++)
                {
                    lvarShapeXMin = Math.Min(lvarShapeXMin, mvarParts[i].MBRXMin);
                    lvarShapeXMax = Math.Max(lvarShapeXMax, mvarParts[i].MBRXMax);
                    lvarShapeYMin = Math.Min(lvarShapeYMin, mvarParts[i].MBRYMin);
                    lvarShapeYMax = Math.Max(lvarShapeYMax, mvarParts[i].MBRYMax);
                    if (mvarParts[i].zMin != null)
                    {
                        if (lvarShapeZMin == null)
                            lvarShapeZMin = mvarParts[i].zMin;
                        else
                            lvarShapeZMin = Math.Min((double)lvarShapeZMin, (double)mvarParts[i].zMin);
                    }
                    if (mvarParts[i].MeasureMax != null)
                    {
                        if (lvarShapeZMax == null)
                            lvarShapeZMax = mvarParts[i].MeasureMax;
                        else
                            lvarShapeZMax = Math.Max((double)lvarShapeZMax, (double)mvarParts[i].MeasureMax);
                    }
                    if (mvarParts[i].zMax != null)
                    {
                        if (lvarShapeMMin == null)
                            lvarShapeMMin = mvarParts[i].zMax;
                        else
                            lvarShapeMMin = Math.Min((double)lvarShapeMMin, (double)mvarParts[i].zMax);
                    }
                    if (mvarParts[i].MeasureMax != null)
                    {
                        if (lvarShapeMMax == null)
                            lvarShapeMMax = mvarParts[i].MeasureMax;
                        else
                            lvarShapeMMax = Math.Max((double)lvarShapeMMax, (double)mvarParts[i].MeasureMax);
                    }
                }
                mvarVertices.xMin = lvarShapeXMin;
                mvarVertices.xMax = lvarShapeXMax;
                mvarVertices.yMin = lvarShapeYMin;
                mvarVertices.yMax = lvarShapeYMax;
                mvarVertices.zMin = lvarShapeZMin;
                mvarVertices.zMax = lvarShapeZMax;
                mvarVertices.mMin = lvarShapeMMin;
                mvarVertices.mMax = lvarShapeMMax;
            }
        }

        /// <summary>
        /// Removes all Vertice and Field database information associated with a particular part from the current ShapeFile record
        /// </summary>
        ///<remarks>
        /// Every now and again you find that you need to completely delete a record from your ShapeFile.  This is part 1 of that process.  The
        /// DeleteShape method changes the ShapeType of the record to that of a Null Shape and sets the delete flag at the start of the associated DBF record.  To complete the process and permanently 
        /// remove the record you must use the <see cref="Pack"/> method. As we are only changing two flags with the DeleteShape method you can undo the delete by using the <see cref="UnDeleteShape"/> method anytime prior to a Pack.
        ///</remarks>
        ///<example>
        ///<code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///
        ///     // Add some records to an existing shape
        ///     myShape.Vertices.Add(10, 10);
        ///     myShape.Fields[0].Value = "New Record 1";
        ///     myShape.WriteShape();
        ///
        ///     myShape.Vertices.Add(10, 15);
        ///     myShape.Fields[0].Value = "New Record 2";
        ///     myShape.WriteShape();
        ///
        ///     myShape.Vertices.Add(10, 15);
        ///     myShape.Fields[0].Value = "New Record 3";
        ///     myShape.WriteShape();
        ///
        ///     // Get rid of Record 2 - it's a duplicate
        ///     myShape.MoveTo(1);
        ///     myShape.DeleteShape();
        ///
        ///     // Make the changes permanent
        ///     myShape.Pack();
        /// }
        ///</code>
        ///<code lang="VB">
        /// Using myShape As New ShapeFile()
        ///     myShape.Open("C:\Shapes\Point.shp")
        ///
        ///     ' Add some records to an existing shape
        ///     myShape.Vertices.Add(10, 10)
        ///     myShape.Fields(0).Value = "New Record 1"
        ///     myShape.WriteShape()
        ///
        ///     myShape.Vertices.Add(10, 15)
        ///     myShape.Fields(0).Value = "New Record 2"
        ///     myShape.WriteShape()
        ///
        ///     myShape.Vertices.Add(10, 15)
        ///     myShape.Fields(0).Value = "New Record 3"
        ///     myShape.WriteShape()
        ///
        ///     ' Get rid of Record 2 - it's a duplicate
        ///     myShape.MoveTo(1)
        ///     myShape.DeleteShape()
        ///
        ///     ' Make the changes permanent
        ///     myShape.Pack()
        /// End Using
        ///</code>
        ///</example>
        ///<seealso cref="UnDeleteShape"/>
        ///<seealso cref="Pack"/>
        public void DeleteShape()
        {
            // ***********************************************************
            // * Mark the DBF file for Delete and set Shape Type to NULL *
            // ***********************************************************

            byte[] ByteArray = new byte[4];

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);
            OpenStream(mvarShapeDBF, ref fsDataFile);

            // **********************************************
            // * Change the Shape Type to NULL              *
            // **********************************************
            long FilePos = GetShapeRecordStart(mvarCurrentRecord, fsShapeIndex);
            fsShapeFile.Seek(FilePos + 4, SeekOrigin.Begin);

            // Write a null shape record
            fsShapeFile.Write(ByteArray, 0, 4);

            // **********************************************
            // * Mark the DBF record                        *
            // **********************************************
            FilePos = mvarFields.HeaderLength + (mvarFields.Recordlength * (mvarCurrentRecord - 1));
            // The first record is an extra bit out from the headerlength - an astrix indicates the deleted record
            fsDataFile.Seek(FilePos, SeekOrigin.Begin);
            ByteArray[0] = 42;
            // Astrix character (2Ah)
            fsDataFile.Write(ByteArray, 0, 1);

            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);
            CloseStream(ref fsDataFile);

            mvarFields.isDeleted = true;
            onShapeRecordDeleted(new ShapeFileEventArgs(mvarCurrentRecord));

        }

        /// <summary>
        /// Restores all Vertice and Field database information for the ShapeFiles record that had been previously deleted with the DeleteShape command 
        /// </summary>
        ///<remarks>
        ///The <see cref="DeleteShape"/> method does not physically remove the data from the .SHP and .DBF files, it only toggles the shape record type to NULL and marks the DBF record for deletion. The UnDeleteShape method resets these markers.
        /// Can you undelete a record once you've Packed the ShapeFile?  The answer is no.  <see cref="Pack"/> perminantly removes the features.
        ///</remarks>
        ///<example>
        ///<code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///
        ///     // Add some records to an existing shape
        ///     myShape.Vertices.Add(10, 10);
        ///     myShape.Fields[0].Value = "New Record 1";
        ///     myShape.WriteShape();
        ///
        ///     myShape.Vertices.Add(10, 15);
        ///     myShape.Fields[0].Value = "New Record 2";
        ///     myShape.WriteShape();
        ///
        ///     myShape.Vertices.Add(10, 15);
        ///     myShape.Fields[0].Value = "New Record 3";
        ///     myShape.WriteShape();
        ///
        ///     // Get rid of Record 2 - it's a duplicate
        ///     myShape.MoveTo(1);
        ///     myShape.DeleteShape();
        ///
        ///     // Ooops bring it back
        ///     myShape.UnDeleteShape();
        /// }
        ///</code>
        ///<code lang="VB">
        /// Using myShape As New ShapeFile()
        ///     myShape.Open("C:\Shapes\Point.shp")
        ///
        ///     ' Add some records to an existing shape
        ///     myShape.Vertices.Add(10, 10)
        ///     myShape.Fields(0).Value = "New Record 1"
        ///     myShape.WriteShape()
        ///
        ///     myShape.Vertices.Add(10, 15)
        ///     myShape.Fields(0).Value = "New Record 2"
        ///     myShape.WriteShape()
        ///
        ///     myShape.Vertices.Add(10, 15)
        ///     myShape.Fields(0).Value = "New Record 3"
        ///     myShape.WriteShape()
        ///
        ///     ' Get rid of Record 2 - it's a duplicate
        ///     myShape.MoveTo(1)
        ///     myShape.DeleteShape()
        ///
        ///     ' Ooops bring it back
        ///     myShape.UnDeleteShape()
        /// End Using
        ///</code>
        ///</example>
        ///<seealso cref="DeleteShape"/>
        ///<seealso cref="Pack"/>
        public void UnDeleteShape()
        {
            // ************************************************************
            // * Reset DBF Delete Mark and set Shape Type to current type *
            // ************************************************************

            byte[] ByteArray = new byte[4];
            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);
            OpenStream(mvarShapeDBF, ref fsDataFile);

            // **********************************************
            // * Change the Shape Type to the current       *
            // **********************************************
            long FilePos = GetShapeRecordStart(mvarCurrentRecord, fsShapeIndex);
            fsShapeFile.Seek(FilePos + 4, SeekOrigin.Begin);

            // Write a null shape record
            ByteArray = BitConverter.GetBytes(mvarShapeType);
            fsShapeFile.Write(ByteArray, 0, 4);

            // **********************************************
            // * Unmark the DBF record                      *
            // **********************************************
            FilePos = (mvarFields.HeaderLength - 1) + (mvarFields.Recordlength * (mvarCurrentRecord - 1));
            // The first record is an extra bit out from the headerlength - an astrix indicates the deleted record
            fsDataFile.Seek(FilePos, SeekOrigin.Begin);
            ByteArray[0] = 32;
            // Space character (20h)
            fsDataFile.Write(ByteArray, 0, 1);


            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);
            CloseStream(ref fsDataFile);

            mvarFields.isDeleted = true;

        }

        /// <summary>
        /// Removes all Vertice and Field database values for the entire ShapeFiles object. The Field definitions remain in place.
        /// </summary>
        /// <remarks>Sometimes you just want to start again.  This method allows you to take an existing ShapeFile and strip eveything out of it - only leaving the database definition.</remarks>
        public void EmptyShape()
        {
            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);
            OpenStream(mvarShapeDBF, ref fsDataFile);

            WriteShapeHeader(fsShapeFile);
            WriteShapeHeader(fsShapeIndex);
            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);

            AppendFieldDefs(ref mvarFields, fsDataFile);

            // Update the record number in the DB to 0
            fsDataFile.Seek(4, SeekOrigin.Begin);
            fsDataFile.WriteByte(0);
            fsDataFile.WriteByte(0);
            fsDataFile.WriteByte(0);
            CloseStream(ref fsDataFile);

            mvarShapeCount = 0;
            mvarVertices.Clear();
            mvarParts.Clear();
            mvarFields.Strip();

        }

        /// <summary>
        /// Recalculates the MBR of each shape and updates the X,Y,Z and Measure Min/Max values of both the shape record and the entire file
        /// </summary>
        ///<remarks>
        ///If you have been doing a lot of editing to your Vertices then the minimum and maximum values of your shape record may be out of whack. This means that the maximum zoom extents of your ShapeFile will not be correctly displayed in products like ArcGIS. This method scans your ShapeFile and recalculates the minimum and maximum values for: 
        ///<li>X Coordinates</li> 
        ///<li>Y Coordinates</li> 
        ///<li>Z Coordinates</li> 
        ///<li>Measures</li> 
        ///<para> The min/max values for added or edited records are automatically recalculated when you invoke the <see cref="ModifyShape"/> method.  Deleted vertices ... not.</para>
        ///</remarks>
        public void UpdateMBR()
        {

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);

            byte[] dblBytes = new byte[8];
            for (int recordnumber = 0; recordnumber < mvarShapeCount; recordnumber++)
            {
                // Populate the raw byte data into the Vertice VertData array
                byte[] LongBytes = new byte[4];
                long FilePos = 0;

                if (recordnumber > 1)
                {
                    //Read the location out out the index file
                    int Offset = 100 + ((recordnumber - 1) * 8);
                    fsShapeIndex.Seek(Offset, SeekOrigin.Begin);
                    fsShapeIndex.Read(LongBytes, 0, 4);
                    Array.Reverse(LongBytes);
                    FilePos = (BitConverter.ToInt32(LongBytes, 0) * 2) + 4;
                }
                else
                {
                    FilePos = 104;
                }

                if (FilePos > fsShapeFile.Length)
                { return; }

                // Move to start of the Shape Record
                FilePos = fsShapeFile.Seek(FilePos, SeekOrigin.Begin);
                // Ignore the record number
                if (FilePos == 0)
                {
                    throw new Exception("The ShapeFile is corrupted at record number " + recordnumber.ToString());
                }

                fsShapeFile.Read(LongBytes, 0, 4);
                Array.Reverse(LongBytes);
                int ContentLength = BitConverter.ToInt32(LongBytes, 0);

                // Check to avoid silly errors
                if (ContentLength == 0)
                {
                    throw new Exception("The ShapeFile is corrupted at record number " + recordnumber.ToString());
                }

                byte[] vertData = new byte[ContentLength * 2];
                fsShapeFile.Read(vertData, 0, ContentLength * 2);

                // *****************************************************************
                // *  The shape record now is held in verData = Read the vertices  *
                // *****************************************************************
                int ArrayPos = 0;
                double lvarXMin = 1E+38;
                double lvarYMin = 1E+38;
                double lvarZMin = -1E+38;
                double lvarMMin = -1E+38;
                double lvarXMax = -1E+38;
                double lvarYMax = -1E+38;
                double lvarZMax = -1E+38;
                double lvarMMax = -1E+38;
                double lvarM = 0;
                int lvarNoOfPoints = 0;
                int lvarNoOfParts = 0;
                eShapeType lvarShapeType = (eShapeType)BitConverter.ToInt32(vertData, 0);

                double XVal;
                double YVal;
                double ZVal;

                // X & Y Values
                switch ((eShapeType)lvarShapeType)
                {

                    // *******************************
                    // * Point Shapes                *
                    // *******************************
                    case eShapeType.shpPoint:
                    case eShapeType.shpPointM:
                    case eShapeType.shpPointZ:
                        ArrayPos = 4;
                        lvarXMin = BitConverter.ToDouble(vertData, ArrayPos);
                        lvarYMin = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (lvarShapeType == eShapeType.shpPointZ)
                        {
                            lvarZMin = BitConverter.ToDouble(vertData, 20);
                            lvarMMin = BitConverter.ToDouble(vertData, 28);
                        }
                        if (lvarShapeType == eShapeType.shpPointM)
                        {
                            lvarM = BitConverter.ToDouble(vertData, 20);
                            if (lvarM > -1E+38)
                            {
                                lvarMMin = lvarM;
                            }
                        }
                        lvarXMax = lvarXMin;
                        lvarYMax = lvarYMin;
                        lvarZMax = lvarZMin;
                        lvarMMax = lvarMMin;

                        break;
                    // *******************************
                    // * MultiPoint Shapes           *
                    // *******************************
                    case eShapeType.shpMultiPoint:
                    case eShapeType.shpMultiPointZ:
                    case eShapeType.shpMultiPointM:
                        lvarNoOfPoints = BitConverter.ToInt32(vertData, 36);
                        ArrayPos = 40;
                        for (int i = 0; i < lvarNoOfPoints; i++)
                        {
                            XVal = BitConverter.ToDouble(vertData, ArrayPos);
                            YVal = BitConverter.ToDouble(vertData, ArrayPos + 8);
                            lvarXMin = Math.Min(lvarXMin, XVal);
                            lvarYMin = Math.Min(lvarYMin, YVal);
                            lvarXMax = Math.Max(lvarXMax, XVal);
                            lvarYMax = Math.Max(lvarYMax, YVal);
                            ArrayPos += 16;
                        }

                        if ((eShapeType)mvarShapeType == eShapeType.shpMultiPointZ)
                        {
                            ArrayPos += 16;
                            for (int i = 0; i < lvarNoOfPoints; i++)
                            {
                                ZVal = BitConverter.ToDouble(vertData, ArrayPos);
                                lvarZMin = Math.Min(lvarZMin, ZVal);
                                lvarZMax = Math.Max(lvarZMax, ZVal);
                                ArrayPos += 8;
                            }
                        }
                        if ((eShapeType)mvarShapeType == eShapeType.shpMultiPointM | (eShapeType)mvarShapeType == eShapeType.shpMultiPointZ)
                        {
                            ArrayPos += 16;
                            for (int i = 0; i < lvarNoOfPoints; i++)
                            {
                                lvarM = BitConverter.ToDouble(vertData, ArrayPos);
                                if (lvarM > -1E+38)
                                {
                                    if (lvarMMin > -1E+38 & lvarM > -1E+38)
                                        lvarMMin = Math.Min(lvarMMin, lvarM);
                                    else
                                        lvarMMin = lvarM;
                                    if (lvarMMax > -1E+38)
                                        lvarMMax = Math.Max(lvarMMax, lvarM);
                                    else
                                        lvarMMax = lvarM;
                                }
                                ArrayPos += 8;
                            }
                        }
                        break;
                    // *******************************
                    // * PolyLine and Polygon Shapes *
                    // *******************************
                    case eShapeType.shpPolyLine:
                    case eShapeType.shpPolygon:
                    case eShapeType.shpPolyLineZ:
                    case eShapeType.shpPolygonZ:
                    case eShapeType.shpPolyLineM:
                    case eShapeType.shpPolygonM:
                        //Arc, Polygon
                        lvarNoOfParts = BitConverter.ToInt32(vertData, 36);
                        lvarNoOfPoints = BitConverter.ToInt32(vertData, 40);
                        ArrayPos = 44 + (lvarNoOfParts * 4);
                        for (int i = 0; i < lvarNoOfPoints; i++)
                        {
                            XVal = BitConverter.ToDouble(vertData, ArrayPos);
                            YVal = BitConverter.ToDouble(vertData, ArrayPos + 8);
                            lvarXMin = Math.Min(lvarXMin, XVal);
                            lvarYMin = Math.Min(lvarYMin, YVal);
                            lvarXMax = Math.Max(lvarXMax, XVal);
                            lvarYMax = Math.Max(lvarYMax, YVal);
                            ArrayPos += 16;
                        }

                        if (lvarShapeType == eShapeType.shpPolyLineZ | lvarShapeType == eShapeType.shpPolygonZ)
                        {
                            ArrayPos += 16;
                            for (int i = 0; i < lvarNoOfPoints; i++)
                            {
                                ZVal = BitConverter.ToDouble(vertData, ArrayPos);
                                lvarZMin = Math.Min(lvarZMin, ZVal);
                                lvarZMax = Math.Max(lvarZMax, ZVal);
                                ArrayPos += 8;
                            }
                        }

                        if (lvarShapeType == eShapeType.shpPolyLineM | lvarShapeType == eShapeType.shpPolygonM)
                        {
                            ArrayPos += 16;
                            for (int i = 0; i < lvarNoOfPoints; i++)
                            {
                                lvarM = BitConverter.ToDouble(vertData, ArrayPos);
                                if (lvarM > -1E+38)
                                {
                                    if (lvarMMin > -1E+38 & lvarM > -1E+38)
                                        lvarMMin = Math.Min(lvarMMin, lvarM);
                                    else
                                        lvarMMin = lvarM;
                                    if (lvarMMax > -1E+38)
                                        lvarMMax = Math.Max(lvarMMax, lvarM);
                                    else
                                        lvarMMax = lvarM;
                                }
                                ArrayPos += 8;
                            }
                        }

                        break;
                    // *******************************
                    // * MultiPatch Shapes           *
                    // *******************************
                    case eShapeType.shpMultiPatch:
                        lvarNoOfParts = BitConverter.ToInt32(vertData, 36);
                        lvarNoOfPoints = BitConverter.ToInt32(vertData, 40);
                        ArrayPos = 44 + (lvarNoOfParts * 8);
                        for (int i = 0; i < lvarNoOfPoints; i++)
                        {
                            XVal = BitConverter.ToDouble(vertData, ArrayPos);
                            YVal = BitConverter.ToDouble(vertData, ArrayPos + 8);
                            lvarXMin = Math.Min(lvarXMin, XVal);
                            lvarYMin = Math.Min(lvarYMin, YVal);
                            lvarXMax = Math.Max(lvarXMax, XVal);
                            lvarYMax = Math.Max(lvarYMax, YVal);
                            ArrayPos += 16;
                        }
                        ArrayPos += 16;
                        for (int i = 0; i < lvarNoOfPoints; i++)
                        {
                            ZVal = BitConverter.ToDouble(vertData, ArrayPos);
                            lvarZMin = Math.Min(lvarZMin, ZVal);
                            lvarZMax = Math.Max(lvarZMax, ZVal);
                            ArrayPos += 8;
                        }
                        ArrayPos += 16;
                        for (int i = 0; i < lvarNoOfPoints; i++)
                        {
                            lvarM = BitConverter.ToDouble(vertData, ArrayPos);
                            if (lvarM > -1E+38)
                            {
                                if (lvarMMin > -1E+38 & lvarM > -1E+38)
                                    lvarMMin = Math.Min(lvarMMin, lvarM);
                                else
                                    lvarMMin = lvarM;
                                if (lvarMMax > -1E+38)
                                    lvarMMax = Math.Max(lvarMMax, lvarM);
                                else
                                    lvarMMax = lvarM;
                            }
                            ArrayPos += 8;
                        }

                        break;
                }


                // * Update the individual Record MBR

                if ((eShapeType)lvarShapeType != eShapeType.shpPoint & (eShapeType)lvarShapeType != eShapeType.shpPointM & (eShapeType)lvarShapeType != eShapeType.shpPointZ)
                {
                    fsShapeFile.Seek(FilePos + 8, SeekOrigin.Begin);
                    dblBytes = BitConverter.GetBytes(lvarXMin);
                    fsShapeFile.Write(dblBytes, 0, 8);
                    dblBytes = BitConverter.GetBytes(lvarYMin);
                    fsShapeFile.Write(dblBytes, 0, 8);
                    dblBytes = BitConverter.GetBytes(lvarXMax);
                    fsShapeFile.Write(dblBytes, 0, 8);
                    dblBytes = BitConverter.GetBytes(lvarYMax);
                    fsShapeFile.Write(dblBytes, 0, 8);

                    switch ((eShapeType)lvarShapeType)
                    {

                        // *******************************
                        // * Multi Point Shapes          *
                        // *******************************
                        case eShapeType.shpMultiPointZ:
                            ArrayPos = 44 + (lvarNoOfPoints * 16);
                            dblBytes = BitConverter.GetBytes(lvarZMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarZMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            ArrayPos = 44 + (lvarNoOfPoints * 24) + 16;
                            dblBytes = BitConverter.GetBytes(lvarMMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarMMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            break;

                        case eShapeType.shpMultiPointM:
                            ArrayPos = 44 + (lvarNoOfPoints * 16);
                            dblBytes = BitConverter.GetBytes(lvarMMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarMMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            break;

                        // *******************************
                        // * PolyLine and Polygon Shapes *
                        // *******************************
                        case eShapeType.shpPolyLineZ:
                        case eShapeType.shpPolygonZ:
                            ArrayPos = 48 + (lvarNoOfPoints * 16) + (lvarNoOfParts * 4);
                            dblBytes = BitConverter.GetBytes(lvarZMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarZMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            ArrayPos = 48 + (lvarNoOfPoints * 24) + (lvarNoOfParts * 4);
                            dblBytes = BitConverter.GetBytes(lvarMMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarMMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            break;

                        case eShapeType.shpPolyLineM:
                        case eShapeType.shpPolygonM:
                            ArrayPos = 48 + (lvarNoOfPoints * 16) + (lvarNoOfParts * 4);
                            dblBytes = BitConverter.GetBytes(lvarMMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarMMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            break;

                        // *******************************
                        // * MultiPatch Shapes           *
                        // *******************************
                        case eShapeType.shpMultiPatch:
                            ArrayPos = 48 + (lvarNoOfPoints * 16) + (lvarNoOfParts * 8);
                            dblBytes = BitConverter.GetBytes(lvarZMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarZMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            ArrayPos = 48 + (lvarNoOfPoints * 24) + (lvarNoOfParts * 8);
                            dblBytes = BitConverter.GetBytes(lvarMMin);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            dblBytes = BitConverter.GetBytes(lvarMMax);
                            fsShapeFile.Write(dblBytes, 0, 8);
                            break;


                    }
                }

                // *******************************************
                // * Update the MBR of the entire shape file *
                // *******************************************
                if (recordnumber == 0)
                {
                    // One record so use current record values
                    mvarShapeFileXMin = lvarXMin;
                    mvarShapeFileXMax = lvarXMax;
                    mvarShapeFileYMin = lvarYMin;
                    mvarShapeFileYMax = lvarYMax;
                    mvarShapeFileZMin = lvarZMin;
                    mvarShapeFileZMax = lvarZMax;
                    mvarShapeFileMMin = lvarMMin;
                    mvarShapeFileMMax = lvarMMax;
                }
                else
                {
                    // Find Min/Max values
                    if ((eShapeType)mvarShapeType != eShapeType.shpNull)
                    {
                        mvarShapeFileXMin = Math.Min(lvarXMin, mvarShapeFileXMin);
                        mvarShapeFileXMax = Math.Max(lvarXMax, mvarShapeFileXMax);
                        mvarShapeFileYMin = Math.Min(lvarYMin, mvarShapeFileYMin);
                        mvarShapeFileYMax = Math.Max(lvarYMax, mvarShapeFileYMax);
                        mvarShapeFileZMin = Math.Min(lvarZMin, mvarShapeFileZMin);
                        mvarShapeFileZMax = Math.Max(lvarZMax, mvarShapeFileZMax);
                        if (mvarShapeFileMMin > -1E+38 & lvarMMin > -1E+38)
                            mvarShapeFileMMin = Math.Min(lvarMMin, Convert.ToDouble(mvarShapeFileMMin));
                        else
                            mvarShapeFileMMin = lvarMMin;
                        if (lvarMMax > -1E+38)
                            mvarShapeFileMMax = Math.Max(lvarMMax, Convert.ToDouble(mvarShapeFileMMax));
                        else
                            mvarShapeFileMMax = lvarMMax;

                        mvarShapeFileMMin = Math.Min(lvarMMin, Convert.ToDouble(mvarShapeFileMMin));
                        mvarShapeFileMMax = Math.Max(lvarMMax, Convert.ToDouble(mvarShapeFileMMax));
                    }
                }
            }


            fsShapeFile.Seek(36, SeekOrigin.Begin);
            dblBytes = BitConverter.GetBytes(mvarShapeFileXMin);
            fsShapeFile.Write(dblBytes, 0, 8);
            dblBytes = BitConverter.GetBytes(mvarShapeFileYMin);
            fsShapeFile.Write(dblBytes, 0, 8);
            dblBytes = BitConverter.GetBytes(mvarShapeFileXMax);
            fsShapeFile.Write(dblBytes, 0, 8);
            dblBytes = BitConverter.GetBytes(mvarShapeFileYMax);
            fsShapeFile.Write(dblBytes, 0, 8);
            dblBytes = BitConverter.GetBytes(mvarShapeFileZMin);
            fsShapeFile.Write(dblBytes, 0, 8);
            dblBytes = BitConverter.GetBytes(mvarShapeFileZMax);
            fsShapeFile.Write(dblBytes, 0, 8);
            dblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileMMin));
            fsShapeFile.Write(dblBytes, 0, 8);
            dblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileMMax));
            fsShapeFile.Write(dblBytes, 0, 8);

            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);



        }

        /// <summary>
        /// Forces the part orientation of a polygon ring
        /// </summary>
        /// <param name="PartNo">The number of the part</param>
        /// <param name="Orientation">Direction to be set</param>
        /// <remarks>
        /// When you load in the vertices of a multi-part polygon you sometimes aren't too sure whether the vertice direction represents an inner or outer shape.  The ShapeFile format specifies that outside shapes have a clockwise vertice orientation, and a hole has an anti-clockwise orientation.
        /// Use this method to explicitly set the orientation of a part before you write it out.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     // Create a polygon with a donut
        ///     myShape.Open(@"C:\Shapes\Polygon.shp", eShapeType.shpPolygon);
        ///     myShape.Fields.Add("TextField", eFieldType.shpText);
        ///     myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2);
        ///     myShape.WriteFieldDefs();
        ///
        ///     myShape.Fields["TextField"].Value = "Polygon";
        ///     myShape.Fields[1].Value = 1.23;
        ///     // Outer shape 
        ///     myShape.Vertices.Add(10, 1);
        ///     myShape.Vertices.Add(10, 10);
        ///     myShape.Vertices.Add(20, 10);
        ///     myShape.Vertices.Add(20, 1);
        ///     myShape.Vertices.Add(10, 1);
        ///     // inner shape 
        ///     myShape.Vertices.NewPart();
        ///     myShape.Vertices.Add(14, 4);
        ///     myShape.Vertices.Add(14, 6);
        ///     myShape.Vertices.Add(16, 6);
        ///     myShape.Vertices.Add(16, 4);
        ///     myShape.Vertices.Add(14, 4);
        ///
        ///     // Make sure the inner shape has the correct orientation
        ///     myShape.SetPartDirection(1, eDirection.AntiClockwise);
        ///
        ///     // Display the vertices to make sure
        ///     for (int i = myShape.Parts[1].Begins; i &lt;= myShape.Parts[1].Ends; i++)
        ///         Console.WriteLine("Vertice {0}  X: {1}  Y: {2}", i, myShape.Vertices[i].X_Cord, myShape.Vertices[i].Y_Cord);
        ///
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        ///     ' Create a polygon with a donut
        ///     myShape.Open("C:\Shapes\Polygon.shp", eShapeType.shpPolygon)
        ///     myShape.Fields.Add("TextField", eFieldType.shpText)
        ///     myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2)
        ///     myShape.WriteFieldDefs()
        ///
        ///     myShape.Fields("TextField").Value = "Polygon"
        ///     myShape.Fields(1).Value = 1.23
        ///     ' Outer shape 
        ///     myShape.Vertices.Add(10, 1)
        ///     myShape.Vertices.Add(10, 10)
        ///     myShape.Vertices.Add(20, 10)
        ///     myShape.Vertices.Add(20, 1)
        ///     myShape.Vertices.Add(10, 1)
        ///     ' inner shape 
        ///     myShape.Vertices.NewPart()
        ///     myShape.Vertices.Add(14, 4)
        ///     myShape.Vertices.Add(14, 6)
        ///     myShape.Vertices.Add(16, 6)
        ///     myShape.Vertices.Add(16, 4)
        ///     myShape.Vertices.Add(14, 4)
        ///
        ///     ' Make sure the inner shape has the correct orientation
        ///     myShape.SetPartDirection(1, eDirection.AntiClockwise)
        ///
        ///     ' Display the vertices to make sure
        ///     For i As Integer = myShape.Parts(1).Begins To myShape.Parts(1).Ends
        ///         Console.WriteLine("Vertice {0}  X: {1}  Y: {2}", i, myShape.Vertices(i).X_Cord, myShape.Vertices(i).Y_Cord)
        ///     Next i
        ///
        /// End Using 
        /// </code>
        /// </example>
        public void SetPartDirection(int PartNo, eDirection Orientation)
        {
            if (PartNo < 0 || PartNo > mvarParts.Count - 1)
                return;

            // Find the direction of the current part
            double aSum = 0;
            double AreaFactor = 0;
            eDirection currentDir = eDirection.AntiClockwise;

            for (int i = mvarParts[PartNo].Begins + 1; i <= mvarParts[PartNo].Ends; i++)
            {
                AreaFactor = ((mvarVertices[i - 1].X_Cord * mvarVertices[i].Y_Cord) - (mvarVertices[i].X_Cord * mvarVertices[i - 1].Y_Cord));
                aSum += AreaFactor;
            }

            if (aSum > 0)
                currentDir = eDirection.Clockwise;

            if (currentDir != Orientation)
            {
                // Reverse the order of the segments
                Vertices tVerts = new Vertices();
                for (int i = mvarParts[PartNo].Ends; i >= mvarParts[PartNo].Begins; i--)
                {
                    Vertice thisVert = mvarVertices[i];
                    tVerts.Add(thisVert);
                }

                for (int i = mvarParts[PartNo].Begins; i <= mvarParts[PartNo].Ends; i++)
                    mvarVertices[i] = tVerts[i - mvarParts[PartNo].Begins];

                mvarParts[PartNo].Area = 0 - mvarParts[PartNo].Area;
                mvarParts[PartNo].Direction = Orientation;
                if (Orientation == eDirection.AntiClockwise)
                    mvarParts[PartNo].IsHole = true;
                else
                    mvarParts[PartNo].IsHole = false;

            }


        }

        #endregion

        #region **********          Move Methods                  **********

        ///<summary>
        ///Moves to the first record in the currently opened ShapeFile.
        ///</summary>
        ///<remarks>
        ///This step happens by default when you first open a ShapeFile.  Whether or not the data is immediately read depends on the <see cref="ReadMode"/>.  Moving to the top record will set the <see cref="BOF"/> property to True.
        ///</remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///     myShape.MoveFirst();
        ///     while(!myShape.EOF)
        ///     {
        ///         // Show me what record this is
        ///         Console.WriteLine("Record: {0} Name: {1}", myShape.CurrentRecord, myShape.Fields["TextField"].Value);
        ///         // Move to the previous record
        ///         myShape.MoveNext()
        ///     }
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	myShape.Open("C:\Shapes\Point.shp")
        /// 	myShape.MoveFirst();
        /// 	While Not myShape.EOF
        /// 		' Show me what record this is
        /// 		Console.WriteLine("Record: {0} Name: {1}", myShape.CurrentRecord, myShape.Fields("TextField").Value)
        /// 		' Got to the previous record
        /// 		myShape.MoveNext(i)
        /// 	End While
        /// End Using
        /// </code>
        /// </example>
        /// <seealso cref="MoveLast"/>
        /// <seealso cref="MoveNext"/>
        /// <seealso cref="MovePrevious"/>
        /// <seealso cref="MoveTo"/>
        public void MoveFirst()
        {
            // ***********************************************
            // * Move to the first record of the ShapeFile   *
            // ***********************************************

            // Remove the old vertices & Parts
            if (mvarVertices == null)
                mvarVertices = new Vertices();
            else
                mvarVertices.Clear();
            if (mvarParts == null)
                mvarParts = new Parts();
            else
                mvarParts.Clear();

            // go to the first database record and shape vertice record
            mvarCurrentRecord = 1;
            LoadShapeRecord(mvarCurrentRecord);
            if (mvarReadmode != eReadMode.HeaderOnly)
            {
                // read the shape and DBF data
                ReadShapeRecordHeader(mvarVertices.vertData);
                ReadShapeRecord();
                ReadDBFRecord(mvarCurrentRecord);
            }
            else
            {
                // read the shape header info only
                ReadShapeRecordHeader(mvarVertices.vertData);
                PopulateVerticeHeader(ref mvarVertices);
                if (mvarFields != null)
                    mvarFields.Strip();
            }
            mvarBOF = true;
            if (mvarShapeCount >= mvarCurrentRecord)
            { mvarEOF = false; }
            else
            { mvarEOF = true; }
            mvarNoMatch = true;
            Globals.mvarVerticeChange = false;
            Globals.mvarFieldChange = false;
        }

        /// <summary>
        /// Moves to the last record in the currently opened ShapeFile.
        /// </summary>
        /// <remarks>
        /// This method takes you to the last record in the opened ShapeFile.  Whether or not the data is immediately read depends on the <see cref="ReadMode"/>.  Moving to the last record will set the <see cref="EOF"/> property to True.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///     myShape.MoveLast();
        ///     while(!myShape.BOF)
        ///     {
        ///         // Show me what record this is
        ///         Console.WriteLine("Record: {0} Name: {1}", myShape.CurrentRecord, myShape.Fields["TextField"].Value);
        ///         // Move to the previous record
        ///         myShape.MovePrevious()
        ///     }
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	myShape.Open("C:\Shapes\Point.shp")
        /// 	myShape.MoveLast();
        /// 	While Not myShape.BOF
        /// 		' Show me what record this is
        /// 		Console.WriteLine("Record: {0} Name: {1}", myShape.CurrentRecord, myShape.Fields("TextField").Value)
        /// 		' Got to the previous record
        /// 		myShape.MovePrevious(i)
        /// 	End While
        /// End Using
        /// </code>
        /// </example>
        /// <seealso cref="MoveFirst"/>
        /// <seealso cref="MoveNext"/>
        /// <seealso cref="MovePrevious"/>
        /// <seealso cref="MoveTo"/>
        public void MoveLast()
        {
            // ***********************************************
            // * Move to the last record of the ShapeFile    *
            // ***********************************************

            // Remove the old vertices & Parts
            if (mvarVertices == null)
                mvarVertices = new Vertices();
            else
                mvarVertices.Clear();
            if (mvarParts == null)
                mvarParts = new Parts();
            else
                mvarParts.Clear();

            // go to the first database record and shape vertice record
            mvarCurrentRecord = mvarShapeCount;
            LoadShapeRecord(mvarCurrentRecord);
            if (mvarReadmode != eReadMode.HeaderOnly)
            {
                // read the shape and DBF data
                ReadShapeRecordHeader(mvarVertices.vertData);
                ReadShapeRecord();
                ReadDBFRecord(mvarCurrentRecord);
            }
            else
            {
                // read the shape header info only
                ReadShapeRecordHeader(mvarVertices.vertData);
                PopulateVerticeHeader(ref mvarVertices);
                if (mvarFields != null)
                    mvarFields.Strip();
            }
            if (mvarCurrentRecord > 1)
            {
                mvarBOF = false;
            }
            else
            {
                mvarBOF = true;
            }
            mvarEOF = true;
            mvarNoMatch = true;
            Globals.mvarVerticeChange = false;
            Globals.mvarFieldChange = false;
        }

        /// <summary>
        /// Moves to the next record in the currently opened ShapeFile.
        /// </summary>
        /// <remarks>
        /// MoveNext moves the file position pointer to the next record in the ShapeFile.  Once again, whether or not the data is immediately read depends on the <see cref="ReadMode"/>.  Moving to the last record will set the <see cref="EOF"/> property to True.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///     for (int i = 0; i &lt; myShape.RecordCount; i++)
        ///     {
        ///         // Show me what record this is
        ///         Console.WriteLine("Record: {0} Name: {1}", i, myShape.Fields["TextField"].Value);
        ///         // Move to the next record
        ///         myShape.MoveNext()
        ///     }
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	myShape.Open("C:\Shapes\Point.shp")
        /// 	For i As Integer = 0 To myShape.RecordCount - 1
        /// 		' Show me what record this is
        /// 		Console.WriteLine("Record: {0} Name: {1}", i, myShape.Fields("TextField").Value)
        /// 		' Got to the next record
        /// 		myShape.MoveNext(i)
       /// 	Next i
        /// End Using
        /// </code>
        /// </example>
        /// <seealso cref="MoveFirst"/>
        /// <seealso cref="MoveLast"/>
        /// <seealso cref="MovePrevious"/>
        /// <seealso cref="MoveTo"/>
        public void MoveNext()
        {
            // ***********************************************
            // * Move to the first record of the ShapeFile   *
            // ***********************************************

            // Remove the old vertices & Parts
            if (mvarVertices == null)
                mvarVertices = new Vertices();
            else
                mvarVertices.Clear();
            if (mvarParts == null)
                mvarParts = new Parts();
            else
                mvarParts.Clear();

            // go to the first database record and shape vertice record
            mvarCurrentRecord++;
            if (mvarCurrentRecord > mvarShapeCount)
            {
                mvarCurrentRecord = mvarShapeCount;
                mvarEOF = true;
                mvarBOF = false;
                return;
            }
            LoadShapeRecord(mvarCurrentRecord);
            if (mvarReadmode != eReadMode.HeaderOnly)
            {
                // read the shape and DBF data
                ReadShapeRecordHeader(mvarVertices.vertData);
                PopulateVerticeHeader(ref mvarVertices);
                ReadShapeRecord();
                ReadDBFRecord(mvarCurrentRecord);
            }
            else
            {
                // read the shape header info only
                ReadShapeRecordHeader(mvarVertices.vertData);
                PopulateVerticeHeader(ref mvarVertices);
                if (mvarFields != null)
                    mvarFields.Strip();
            }
            if (mvarCurrentRecord > 1)
            { mvarBOF = false; }
            else
            { mvarBOF = true; }
            if (mvarShapeCount >= mvarCurrentRecord)
            { mvarEOF = false; }
            else
            { mvarEOF = true; }
            mvarNoMatch = true;
            Globals.mvarVerticeChange = false;
            Globals.mvarFieldChange = false;
        }

        /// <summary>
        /// Moves to the previous record in the currently opened ShapeFile.
        /// </summary>
        /// <remarks>
        /// MovePrevious moves the file position pointer to the current record minus one in the ShapeFile.  Once again, whether or not the data is immediately read depends on the <see cref="ReadMode"/>.  Moving to the first record will set the <see cref="BOF"/> property to True.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///     myShape.MoveLast();
        ///     for (int i = myShape.RecordCount; i &gt; 0; i--)
        ///     {
        ///         // Show me what record this is
        ///         Console.WriteLine("Record: {0} Name: {1}", i, myShape.Fields["TextField"].Value);
        ///         // Move to the previous record
        ///         myShape.MovePrevious()
        ///     }
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	myShape.Open("C:\Shapes\Point.shp")
        /// 	myShape.MoveLast();
        /// 	For i As Integer = myShape.RecordCount To 1 Step -1
        /// 		' Show me what record this is
        /// 		Console.WriteLine("Record: {0} Name: {1}", i, myShape.Fields("TextField").Value)
        /// 		' Got to the previous record
        /// 		myShape.MovePrevious(i)
        /// 	Next i
        /// End Using
        /// </code>
        /// </example>
        /// <seealso cref="MoveFirst"/>
        /// <seealso cref="MoveLast"/>
        /// <seealso cref="MoveNext"/>
        /// <seealso cref="MoveTo"/>
        public void MovePrevious()
        {
            // ***********************************************
            // * Move to the first record of the ShapeFile   *
            // ***********************************************

            // Remove the old vertices & Parts
            if (mvarVertices == null)
                mvarVertices = new Vertices();
            else
                mvarVertices.Clear();
            if (mvarParts == null)
                mvarParts = new Parts();
            else
                mvarParts.Clear();

            // go to the first database record and shape vertice record
            mvarCurrentRecord--;
            if (mvarCurrentRecord < 1)
            {
                mvarCurrentRecord = 1;
                mvarEOF = false;
                mvarBOF = true;
                return;
            }

            LoadShapeRecord(mvarCurrentRecord);
            if (mvarReadmode != eReadMode.HeaderOnly)
            {
                // read the shape and DBF data
                ReadShapeRecordHeader(mvarVertices.vertData);
                ReadShapeRecord();
                ReadDBFRecord(mvarCurrentRecord);
            }
            else
            {
                // read the shape header info only
                ReadShapeRecordHeader(mvarVertices.vertData);
                PopulateVerticeHeader(ref mvarVertices);
                if (mvarFields != null)
                    mvarFields.Strip();
            }
            if (mvarCurrentRecord > 1)
            { mvarBOF = false; }
            else
            { mvarBOF = true; }
            if (mvarShapeCount >= mvarCurrentRecord)
            { mvarEOF = false; }
            else
            { mvarEOF = true; }
            mvarNoMatch = true;
            Globals.mvarVerticeChange = false;
            Globals.mvarFieldChange = false;
        }

        /// <summary>
        /// Moves to a given record number in the currently opened ShapeFile.
        /// </summary>
        /// <param name="Index">The record number to move to - between 0 and .RecordNumber-1 </param>
        /// <remarks>
        /// What to move to record number x?  This is the method for you. One thing to note is this is a zero based indicator i.e. the valid parsed number extends from 0 to ShapeFile.RecordCount - 1.  Once again, whether or not the data is immediately read depends on the <see cref="ReadMode"/>.  Moving to the last record will set the <see cref="EOF"/> property to True and moving to the first record will set the <see cref="BOF"/> property to True.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///     for (int i = 0; i &lt; myShape.RecordCount; i++)
        ///     {
        ///         // Got to the ith record
        ///         myShape.MoveTo(i);
        ///         // Show me what record this is
        ///         Console.WriteLine("Record: {0} Name: {1}", i, myShape.Fields["TextField"].Value);
        ///     }
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	myShape.Open("C:\Shapes\Point.shp")
        /// 	For i As Integer = 0 To myShape.RecordCount - 1
        /// 		' Got to the ith record
        /// 		myShape.MoveTo(i)
        /// 		' Show me what record this is
        /// 		Console.WriteLine("Record: {0} Name: {1}", i, myShape.Fields("TextField").Value)
        /// 	Next i
        /// End Using
        /// </code>
        /// </example>
        /// <seealso cref="MoveFirst"/>
        /// <seealso cref="MoveLast"/>
        /// <seealso cref="MoveNext"/>
        /// <seealso cref="MovePrevious"/>
        public void MoveTo(int Index)
        {
            // ***********************************************
            // * Move to the first record of the ShapeFile   *
            // ***********************************************

            // Remove the old vertices & Parts
            if (mvarVertices == null)
                mvarVertices = new Vertices();
            else
                mvarVertices.Clear();
            if (mvarParts == null)
                mvarParts = new Parts();
            else
                mvarParts.Clear();

            // go to the first database record and shape vertice record
            mvarCurrentRecord = Index + 1;
            if (mvarCurrentRecord > mvarShapeCount)
            {
                mvarCurrentRecord = mvarShapeCount;
                mvarEOF = true;
                mvarBOF = false;
                return;
            }
            if (mvarCurrentRecord < 1)
            {
                mvarCurrentRecord = 1;
                mvarEOF = false;
                mvarBOF = true;
                return;
            }

            LoadShapeRecord(mvarCurrentRecord);
            if (mvarReadmode != eReadMode.HeaderOnly)
            {
                // read the shape and DBF data
                ReadShapeRecordHeader(mvarVertices.vertData);
                ReadShapeRecord();
                ReadDBFRecord(mvarCurrentRecord);
            }
            else
            {
                // read the shape header info only
                ReadShapeRecordHeader(mvarVertices.vertData);
                PopulateVerticeHeader(ref mvarVertices);
                if(mvarFields!=null)
                    mvarFields.Strip();
            }
            if (mvarCurrentRecord > 1)
            { mvarBOF = false; }
            else
            { mvarBOF = true; }
            if (mvarShapeCount >= mvarCurrentRecord)
            { mvarEOF = false; }
            else
            { mvarEOF = true; }
            mvarNoMatch = true;
            Globals.mvarVerticeChange = false;
            Globals.mvarFieldChange = false;
        }

        #endregion

        #region **********          Database Methods              **********


        /// <summary>
        /// Loads the Database field definitions from an existing dBASE(.dbf) file or ShapeFile into the Fields Collection
        /// </summary>
        /// <param name="DBFFileName">The name of the Database file or ShapeFile whose data definiton you want to copy</param>
        ///<remarks>
        ///Want to use the data definition from an existing database file in your ShapeFile?  Well this is the method for you.
        ///All were doing here is copying all the data schema into the Fields collection.  You still have to write the definition out using <see cref="WriteFieldDefs"/> though.
        ///</remarks>
        ///<example>
        ///<code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     // Create a new point shapefile
        ///     myShape.Open(@"C:\Shapes\Point.shp", eShapeType.shpPoint);
        ///
        ///     // Pull in the database definitions from a polygon file
        ///     myShape.LoadFieldDefs(@"C:\Shapes\Polygon.shp");
        ///     // Add an extra field
        ///     myShape.Fields.Add("FloatField", eFieldType.shpFloat);
        ///
        ///     // Write out all the field definitions
        ///     myShape.WriteFieldDefs();
        ///     // Write out a record
        /// }
        ///</code>
        ///<code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	' Create a new point shapefile
        /// 	myShape.Open("C:\Shapes\Point.shp", eShapeType.shpPoint)
        ///
        /// 	' Pull in the database definitions from a polygon file
        /// 	myShape.LoadFieldDefs("C:\Shapes\Polygon.shp")
        /// 	' Add an extra field
        /// 	myShape.Fields.Add("FloatField", eFieldType.shpFloat)
        ///
        /// 	' Write out all the field definitions
        /// 	myShape.WriteFieldDefs()
        /// 	' Write out a record
        /// End Using
        ///</code>
        ///</example>
        ///<seealso cref="WriteFieldDefs"/>
        public void LoadFieldDefs(string DBFFileName)
        {
            FileStream lvarDataFile;
            if (!File.Exists(DBFFileName))
            { throw new Exception("You are trying to load the data definition from a database file that does not exist"); }

            try
            {
                if (DBFFileName.ToLower().EndsWith("shp"))
                    DBFFileName = DBFFileName.Substring(0, DBFFileName.Length - 3) + "dbf";
                lvarDataFile = File.Open(DBFFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            catch
            { throw new Exception("The Database File " + DBFFileName + " has been locked by another application"); }

            // Load the Field Definition into a new List
            Fields lvarFields = new Fields();
            lvarFields = ReadDBFHeader(lvarDataFile);

            lvarDataFile.Close();
            lvarDataFile.Dispose();

            // Ensure that field duplication does not occur
            mvarFields.FixFieldNames = true;

            // Load the fields
            foreach (Field addField in lvarFields)
            {
                mvarFields.Add(addField.Name, addField.Type, addField.Size, addField.Decimal);
            }
        }

        /// <summary>
        /// Writes the database field definitions in the Fields collection out to the .DBF file. All modified definitions are updated
        /// </summary>
        /// <remarks>
        /// Can you use this method to add extra fields to an existing database table?  Sure can!  The WriteFieldDefs method takes care of both new and modified field structures in one go.
        /// If you're not storing any info along with your shapes then you can skip this step however it's implicitly fired on the first <see cref="WriteShape"/> of a newly created ShapeFile and if there are no fields defined then an integer field called SHAPE_ID is added.
        /// </remarks>
        /// <example>
        /// <code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     // Create a polygon with a donut
        ///     myShape.Open(@"C:\Shapes\Point.shp", eShapeType.shpPoint);
        ///     myShape.Fields.Add("TextField", eFieldType.shpText); // default text size is 10 Chars
        ///     myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2);
        ///     myShape.Fields.Add("DateField", eFieldType.shpDate);
        ///     myShape.WriteFieldDefs();
        ///     // Write out a record
        ///     myShape.Fields["TextField"].Value = "Record 1";
        ///     myShape.Fields[1].Value = 1.23;
        ///     myShape.Fields[2].Value = Convert.ToDateTime("30 Nov 2015");
        ///     myShape.Vertices.Add(10, 1);
        ///     myShape.WriteShape();
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	' Create a polygon with a donut
        /// 	myShape.Open("C:\Shapes\Point.shp", eShapeType.shpPoint)
        /// 	myShape.Fields.Add("TextField", eFieldType.shpText) ' default text size is 10 Chars
        /// 	myShape.Fields.Add("NumField", eFieldType.shpNumeric, 5, 2)
        /// 	myShape.Fields.Add("DateField", eFieldType.shpDate)
        /// 	myShape.WriteFieldDefs()
        /// 	' Write out a record
        /// 	myShape.Fields("TextField").Value = "Record 1"
        /// 	myShape.Fields(1).Value = 1.23
        /// 	myShape.Fields(2).Value = Convert.ToDateTime("30 Nov 2015")
        /// 	myShape.Vertices.Add(10, 1)
        /// 	myShape.WriteShape()
        /// End Using
        /// </code>
        /// </example>
        /// <seealso cref="O:ArcShapeFile.Fields.Add"/>
        public void WriteFieldDefs()
        {
            // Get the size of the DBF file
            long fileLen;
            if (mvarLockFile)
            {
                fileLen = fsDataFile.Length;
            }
            else
            {
                FileInfo fi = new FileInfo(mvarShapeDBF);
                fileLen = fi.Length;
            }

            if (fileLen == 0)
            {
                OpenStream(mvarShapeDBF, ref fsDataFile);
                AppendFieldDefs(ref mvarFields, fsDataFile);
                CloseStream(ref fsDataFile);
                if (mvarStartEmpty & mvarShapeCount == 0)
                {
                    AddNullShape();
                    mvarShapeCount = 0;
                }
            }
            else
            { ModifyFieldDefs(); }

            foreach (Field mF in mvarFields)
                mF.Status = null;

        }

        private void AppendFieldDefs(ref Fields fieldDef, FileStream fsDataFile)
        {
            // ***************************************************************
            // * Reads the the Field Collection and populates DBF field info *
            // ***************************************************************
            Int16 dbfHeaderLength;
            Int16 dbfRecordlength;
            byte[] IntBytes = new byte[2];

            int lvarFieldCount = 0;

            if (fieldDef.Count == 0)
            { 
                // Force in the ShapeID field as no data definitions have been created
                fieldDef.Add("shape_id", eFieldType.shpNumeric, 10, 0);
                mvarAddShapeID = true;
            }

            // Set Record Length
            dbfRecordlength = 1;
            for (int i = 0; i < fieldDef.Count; i++)
            {
                if (fieldDef[i].Status == null)
                { dbfRecordlength += Convert.ToInt16(fieldDef[i].Size); lvarFieldCount++; }
                else
                { if (!fieldDef[i].Status.Contains("D")) { dbfRecordlength += Convert.ToInt16(fieldDef[i].Size); lvarFieldCount++; } }
            }

            // Set Header size
            dbfHeaderLength = Convert.ToInt16(((lvarFieldCount + 1) * 32) + 1);

            fieldDef.HeaderLength = dbfHeaderLength;
            fieldDef.Recordlength = dbfRecordlength;
            fieldDef.Language = mvarLanguage;
            fieldDef.Delimiter = mvardbfDelimiter;

            byte[] lvarDataBytes = new byte[dbfHeaderLength + 1];

            // File Identification bytes
            lvarDataBytes[0] = 3;

            // Date of last update
            lvarDataBytes[1] = Convert.ToByte(DateTime.Now.Year - 1900);
            lvarDataBytes[2] = Convert.ToByte(DateTime.Now.Month);
            lvarDataBytes[3] = Convert.ToByte(DateTime.Now.Day);

            // Write the new header length
            IntBytes = BitConverter.GetBytes(dbfHeaderLength);
            Buffer.BlockCopy(IntBytes, 0, lvarDataBytes, 8, 2);
            IntBytes = BitConverter.GetBytes(dbfRecordlength);
            Buffer.BlockCopy(IntBytes, 0, lvarDataBytes, 10, 2);

            // *********************************************
            // * Language Driver ID                        *
            // *********************************************
            lvarDataBytes[29] = Convert.ToByte(mvarLanguage);

            //Sum the field Lengths
            int k = 31;
            // Set start point of field list - each field section is 32 bits long


            for (int i = 0; i < fieldDef.Count; i++)
            {
                int deleteStatus = -1;
                Field mField = (Field)fieldDef[i];
                if (mField.Status != null)
                    deleteStatus = mField.Status.IndexOf("D");
                if (deleteStatus < 0)
                {
                    // Name
                    byte[] oFieldName = new byte[11];
                    oFieldName = StrToByteArray(mField.Name);
                    Buffer.BlockCopy(oFieldName, 0, lvarDataBytes, k + 1, oFieldName.Length);

                    // Type
                    switch ((eFieldType)mField.Type)
                    {
                        case eFieldType.shpText:
                            lvarDataBytes[k + 12] = 67;
                            //"C"
                            break;
                        case eFieldType.shpNumeric:
                        case eFieldType.shpInteger:
                        case eFieldType.shpDouble:
                        case eFieldType.shpLong:
                        case eFieldType.shpSingle:
                            lvarDataBytes[k + 12] = 78;
                            //"N"
                            break;
                        case eFieldType.shpFloat:
                            lvarDataBytes[k + 12] = 70;
                            //"F"
                            break;
                        case eFieldType.shpDate:
                            lvarDataBytes[k + 12] = 68;
                            //"D"
                            break;
                        case eFieldType.shpBoolean:
                            lvarDataBytes[k + 12] = 76;
                            //"L"
                            break;
                    }

                    // Size
                    lvarDataBytes[k + 17] = Convert.ToByte(mField.Size);

                    //Decimal
                    lvarDataBytes[k + 18] = Convert.ToByte(mField.Decimal);

                    k = k + 32;

                    // Reset the create/modify status
                    mField.Status = null;
                }

            }

            // Remove any deleted definitions from the Fields collection
            for (int i = fieldDef.Count - 1; i >= 0; i--)
            {
                if (fieldDef[i].Status == "D")
                { fieldDef.RemoveAt(i); }
            }

            // Finish off each record
            lvarDataBytes[dbfHeaderLength - 1] = 13;
            // End of Header terminator (0Dh)

            // Write out the DBF Header and Create a NEW file
            fsDataFile.Seek(0, SeekOrigin.Begin);
            fsDataFile.Write(lvarDataBytes, 0, dbfHeaderLength);


        }

        private void CreateEmptyShape()
        {
            // **********************************************
            // * Add a blank record to the shape files so   *
            // * that it can be used straight away          *
            // **********************************************
            byte[] ByteArray = new byte[4];

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);

            if (!mvarStartEmpty)
            {
                int FilePos = 46;
                fsShapeIndex.Seek(100, SeekOrigin.Begin);

                // **********************************************
                // * Index Record                               *
                // **********************************************
                // File Position

                ByteArray = BitConverter.GetBytes(Convert.ToInt32(FilePos + 4));
                Array.Reverse(ByteArray);
                fsShapeIndex.Write(ByteArray, 0, 4);

                // Build an array with all elements
                byte[] lvarDataArray = new byte[12];

                ByteArray = BitConverter.GetBytes(Convert.ToInt32(2));
                // Content length of NULL shape
                Array.Reverse(ByteArray);
                fsShapeIndex.Write(ByteArray, 0, 4);

                // ***********************************************
                // *  ShapeFile Record Header - first 8 bytes    *
                // ***********************************************

                // put contentlength into data hold array
                Buffer.BlockCopy(ByteArray, 0, lvarDataArray, 4, 4);

                // store the record number
                ByteArray = BitConverter.GetBytes(Convert.ToInt32(1));
                Array.Reverse(ByteArray);
                fsShapeIndex.Write(ByteArray, 0, 4);

                // Shape type of NULL
                ByteArray = BitConverter.GetBytes(Convert.ToInt32(0));
                Buffer.BlockCopy(ByteArray, 0, lvarDataArray, 8, 4);

                // ***********************************************
                // *  Write NULL ShapeFile Record                *
                // ***********************************************

                fsShapeFile.Seek(100, SeekOrigin.Begin);
                fsShapeFile.Write(lvarDataArray, 0, 12);

                // Update the file lengths
                ByteArray = BitConverter.GetBytes(Convert.ToInt32(56));
                Array.Reverse(ByteArray);
                fsShapeFile.Seek(24, SeekOrigin.Begin);
                fsShapeIndex.Seek(24, SeekOrigin.Begin);
                fsShapeFile.Write(ByteArray, 0, 4);
                fsShapeIndex.Write(ByteArray, 0, 4);
            }
            else
            {
                // Update the file lengths
                ByteArray = BitConverter.GetBytes(Convert.ToInt32(50));
                Array.Reverse(ByteArray);
                fsShapeFile.Seek(24, SeekOrigin.Begin);
                fsShapeIndex.Seek(24, SeekOrigin.Begin);
                fsShapeFile.Write(ByteArray, 0, 4);
                fsShapeIndex.Write(ByteArray, 0, 4);

            }

            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);

            mvarShapeCount = 0;

        }

        private void ModifyFieldDefs()
        {
            // ***********************************************************************
            // * Create a new DBF definition and populates any old DBF data into it. *
            // ***********************************************************************

            bool IsDeleted = false;
            bool isSizeChange = false;
            bool isTypeChange = false;
            bool isNameChange = false;
            bool isNew = false;
            int FilePos = 0;

            // Temporary variables to read in old data

            string deletedFields = null;

            // Does a new definition need to be created?
            for (int i = 0; i < mvarFields.Count; i++)
            {
                Field mField = mvarFields[i];
                if (mField.Status != null)
                {
                    if (mField.Status == "D")
                    {
                        IsDeleted = true;
                        deletedFields += "|" + i.ToString() + "|";
                    }
                    if (mField.Status.Contains("A"))
                        isNew = true;
                    if (mField.Status.Contains("N"))
                        isNameChange = true;
                    if (mField.Status.Contains("S"))
                        isSizeChange = true;
                    if (mField.Status.Contains("T"))
                        isTypeChange = true;
                    if (mField.Status.Contains("."))
                        isTypeChange = true;
                }
            }

            // **********************************************
            // * Make sure that there is something to do    *
            // **********************************************
            if (!(IsDeleted | isSizeChange | isTypeChange | isNameChange | isNew))
            {
                return;
            }

            OpenStream(mvarShapeDBF, ref fsDataFile);

            // **********************************************
            // * Name change only ... update header only    *
            // **********************************************
            if (!(IsDeleted | isSizeChange | isTypeChange | isNew))
            {
                FilePos = 32;
                for (int i = 0; i < mvarFields.Count; i++)
                {
                    fsDataFile.Seek(FilePos, SeekOrigin.Begin);
                    byte[] oFieldName = new byte[10];
                    // Reset array to 0 bytes
                    oFieldName = StrToByteArray(mvarFields[i].Name);
                    fsDataFile.Write(oFieldName, 0, oFieldName.Length);
                    FilePos += 32;
                }
                CloseStream(ref fsDataFile);
                return;
            }

            // ****************************************
            // * Make a copy of the existing DBF File *
            // ****************************************
            string tempOldFile = Path.GetTempFileName();

            File.Copy(mvarShapeDBF, tempOldFile, true);
            FileStream fsOldDataFile = File.Open(tempOldFile, FileMode.Open, FileAccess.Read, FileShare.Read);


            // ***********************************************
            // * Create New DBF definition from Shape Fields *
            // ***********************************************
            AppendFieldDefs(ref mvarFields, fsDataFile);
            mvarFields = ReadDBFHeader(fsDataFile);
            CloseStream(ref fsDataFile);


            // *********************************************************
            // * Read in the old data and copy it to the new data file *
            // *********************************************************
            Fields oldFields = new Fields();
            oldFields = ReadDBFHeader(fsOldDataFile);
            for (int i = 1; i <= oldFields.RecordCount; i++)
            {
                LoadDBFRecord(i, fsOldDataFile, ref oldFields);

                // Compare the new vs old field defs
                int newFieldNo = -1;
                for (int oldFieldNo = 0; oldFieldNo < oldFields.Count; oldFieldNo++)
                {
                    bool useField = true;
                    if (deletedFields != null)
                    {
                        if (deletedFields.Contains("|" + oldFieldNo.ToString() + "|")) useField = false;
                    }
                    if (useField)
                    {
                        newFieldNo++;
                        Field nField = mvarFields[newFieldNo];
                        // Do any conversions
                        Field mField = oldFields[oldFieldNo];
                        string DataValue = "";
                        if (mField.Type == eFieldType.shpDate)
                        {
                            DateTime dtTime = Convert.ToDateTime(mField.Value);
                            DataValue = dtTime.ToString("yyyyMMdd");
                        }
                        else
                        { DataValue = mField.Value.ToString(); }
                        if (DataValue != null)
                        {
                            try
                            {
                                switch ((eFieldType)nField.Type)
                                {
                                    case eFieldType.shpNumeric:
                                    case eFieldType.shpDouble:
                                    case eFieldType.shpFloat:
                                        nField.Value = Convert.ToDouble(DataValue);

                                        break;
                                    case eFieldType.shpSingle:
                                        nField.Value = Convert.ToSingle(DataValue);

                                        break;
                                    case eFieldType.shpLong:
                                        nField.Value = Convert.ToInt64(DataValue);

                                        break;
                                    case eFieldType.shpInteger:
                                        nField.Value = Convert.ToInt32(DataValue);

                                        break;
                                    case eFieldType.shpBoolean:
                                        string TestString = ",TRUE,T,YES,Y,";
                                        if (TestString.Contains(DataValue.ToUpper())) nField.Value = true;
                                        if (Convert.ToDouble(DataValue) > 0) nField.Value = true;

                                        break;
                                    case eFieldType.shpDate:
                                        nField.Value = Convert.ToDateTime(DataValue);

                                        break;
                                    default:
                                        nField.Value = DataValue;

                                        break;
                                }
                            }
                            catch
                            { }
                        }
                    }
                } // Next Field

                WriteDBFRecord(i);

            } // Next record

            fsOldDataFile.Close();
            fsOldDataFile.Dispose();

            // Delete the old DBF and rename the temporary one
            File.Delete(tempOldFile);

            // Reset the status of all fields
            for (int i = 0; i < mvarFields.Count; i++)
            {
                mvarFields[i].Status = null;
            }

        }

        #endregion

        #endregion

        #region **********          Data Read Methods             **********

        ///<summary>
        ///Loads all the coordinate data for the current record into the Vertices collection.
        ///</summary>
        ///<remarks>
        ///The LoadShapeData method only applies when the <see cref="ReadMode"/> property is set to HeaderOnly.  This is a handy
        ///way of moving through files that have a large number of vertices.
        ///</remarks>
        ///<example>
        ///<code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     // Set the read mode so that only the meta data is read
        ///     myShape.ReadMode = eReadMode.HeaderOnly;
        ///     myShape.Open(@"C:\Shapes\Polygon.shp");
        ///     for (int i = 0; i &lt; myShape.RecordCount; i++)
        ///     {
        ///         if (myShape.xMin &gt; 10 &amp;&amp; myShape.xMax &lt; 15)
        ///         {
        ///             // Show me what record this is
        ///             myShape.LoadShapeData();
        ///             foreach (Vertice v in myShape.Vertices)
        ///             { Console.WriteLine("X: {0} Y: {1}", v.X_Cord, v.Y_Cord); }
        ///         }
        ///         myShape.MoveNext();
        ///     }
        /// }
        ///</code>
        ///<code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	' Set the read mode so that only the meta data is read
        /// 	myShape.ReadMode = eReadMode.HeaderOnly
        /// 	myShape.Open("C:\Shapes\Polygon.shp")
        /// 	For i As Integer = 0 To myShape.RecordCount - 1
        /// 		If myShape.xMin &gt; 10 AndAlso myShape.xMax &lt; 15 Then
        /// 			' Show me what record this is
        /// 			myShape.LoadShapeData()
        /// 			For Each v As Vertice In myShape.Vertices
        /// 				Console.WriteLine("X: {0} Y: {1}", v.X_Cord, v.Y_Cord)
        /// 			Next v
        /// 		End If
        /// 		myShape.MoveNext()
        /// 	Next i
        /// End Using
        ///</code></example>
        ///<seealso cref="ReadMode"/>
        ///<seealso cref="LoadDBFData"/>
        public void LoadShapeData()
        { ReadShapeRecord(); }

        ///<summary>
        ///Loads all database values of the current record to a Fields collection and makes them available
        ///</summary>
        ///<remarks>
        ///The LoadDBFData method only applies when the <see cref="ReadMode"/> property is set to HeaderOnly, otherwise the reading of the database data is implicit with every move (<see cref="MoveFirst"/>, <see cref="MoveNext"/>, <see cref="MovePrevious"/>, <see cref="MoveLast"/>, <see cref="MoveTo"/>) given.
        ///</remarks>
        ///<example>
        ///<code lang="C#">
        /// using (ShapeFile myShape = new ShapeFile())
        /// {
        ///     // Set the read mode so that only the meta data is read
        ///     myShape.ReadMode = eReadMode.HeaderOnly;
        ///     myShape.Open(@"C:\Shapes\Point.shp");
        ///     for (int i = 0; i &lt; myShape.RecordCount; i++)
        ///     {
        ///         if (myShape.xMin &gt; 10 &amp;&amp; myShape.xMax &lt; 15)
        ///         {
        ///             // Show me what record this is
        ///             myShape.LoadDBFData();
        ///             Console.WriteLine("Record: {0} Name: {1}", myShape.CurrentRecord, myShape.Fields["TextField"].Value);
        ///         }
        ///         myShape.MoveNext();
        ///     }
        /// }
        ///</code>
        ///<code lang="VB">
        /// Using myShape As New ShapeFile()
        /// 	' Set the read mode so that only the meta data is read
        /// 	myShape.ReadMode = eReadMode.HeaderOnly
        /// 	myShape.Open("C:\Shapes\Point.shp")
        /// 	For i As Integer = 0 To myShape.RecordCount - 1
        /// 		If myShape.xMin &gt; 10 AndAlso myShape.xMax &lt; 15 Then
        /// 			' Show me what record this is
        /// 			myShape.LoadDBFData()
        /// 			Console.WriteLine("Record: {0} Name: {1}", myShape.CurrentRecord, myShape.Fields("TextField").Value)
        /// 		End If
        /// 		myShape.MoveNext()
        /// 	Next i
        /// End Using
        ///</code>
        ///</example>
        ///<seealso cref="ReadMode"/>
        ///<seealso cref="LoadShapeData"/>
        public void LoadDBFData()
        { ReadDBFRecord(mvarCurrentRecord); }

        private void LoadDatum(string wktFile)
        {
            // Loads the projection info into the Projection collection
            if (!File.Exists(wktFile))
            { return; }

            mvarDatum = new Projection();
            WKTReader mvarReader = new WKTReader();
            mvarReader.Read(wktFile);

            if (mvarReader.Nodes[0].TagName == "PROJCS")
            {
                mvarDatum.Type = "Projection";
                mvarDatum.ProjCoordSystem = mvarReader.GetAttribName("PROJCS");
            }
            else if (mvarReader.Nodes[0].TagName == "PROJCS")
            {
                mvarDatum.Type = "Geographic";
                mvarDatum.GeoCoordSystem = mvarReader.GetAttribName("GEOGCS");
            }
            else
            {
                mvarDatum.Type = "Geocentric";
                mvarDatum.GeoCoordSystem = mvarReader.GetAttribName("GEOCCS");
            }

            mvarDatum.Datum = mvarReader.GetAttribName("DATUM");
            mvarDatum.SpheroidName = mvarReader.GetAttribName("SPHEROID");
            mvarDatum.EquitorialRadius = Convert.ToDouble(mvarReader.GetAttribValuebyName("SPHEROID", mvarDatum.SpheroidName, 0));
            mvarDatum.FlatteningInverse = Convert.ToDouble(mvarReader.GetAttribValuebyName("SPHEROID", mvarDatum.SpheroidName, 1));
            mvarDatum.PrimeMeridianName = mvarReader.GetAttribName("PRIMEM");
            mvarDatum.PrimeMeridian = Convert.ToDouble(mvarReader.GetAttribValuebyName("PRIMEM", mvarDatum.PrimeMeridianName, 0));

            mvarDatum.ProjectionName = mvarReader.GetAttribName("PROJECTION");
            mvarDatum.CentralMeridian = Convert.ToDouble(mvarReader.GetAttribValuebyName("PARAMETER", "CENTRAL_MERIDIAN", 0));
            mvarDatum.FalseEast = Convert.ToDouble(mvarReader.GetAttribValuebyName("PARAMETER", "FALSE_EASTING", 0));
            mvarDatum.FalseNorth = Convert.ToDouble(mvarReader.GetAttribValuebyName("PARAMETER", "FALSE_NORTHING", 0));
            mvarDatum.LatitudeOrigin = Convert.ToDouble(mvarReader.GetAttribValuebyName("PARAMETER", "LATITUDE_OF_ORIGIN", 0));
            mvarDatum.LongitudeOrigin = Convert.ToDouble(mvarReader.GetAttribValuebyName("PARAMETER", "LONGITUDE_OF_ORIGIN", 0));
            mvarDatum.ScaleFactor = Convert.ToDouble(mvarReader.GetAttribValuebyName("PARAMETER", "SCALE_FACTOR", 0));
            mvarDatum.ProjectionUnitName = mvarReader.GetAttribName("GEOGCS", "UNIT");
            mvarDatum.ProjectionUnitSize = Convert.ToDouble(mvarReader.GetAttribValuebyName("GEOGCS", "UNIT", mvarDatum.ProjectionUnitName, 0));
        }

        private int CalcContentLength()
        {
            int contentlength = 0;
            // ******************************************
            // * Calculate the Content Length in DWORDS *
            // ******************************************

            switch ((eShapeType)mvarShapeType)
            {

                case eShapeType.shpNull:
                    // Null
                    contentlength = 2;

                    break;
                case eShapeType.shpPoint:
                    //Points
                    contentlength = 10;

                    break;
                case eShapeType.shpMultiPoint:
                    //Multi Points
                    contentlength = 20 + (mvarVertices.Count * 8);

                    break;
                case eShapeType.shpPolygon:
                case eShapeType.shpPolyLine:
                    //Polygons and Lines
                    contentlength = 24 + (mvarVertices.Count * 8) + ((mvarParts.Count - 1) * 2);

                    break;
                case eShapeType.shpPointM:
                    //Points with Measures
                    contentlength = 14;

                    break;
                case eShapeType.shpMultiPointM:
                    //Multi Points with Measures
                    contentlength = 28 + (mvarVertices.Count * 12);

                    break;
                case eShapeType.shpPolygonM:
                case eShapeType.shpPolyLineM:
                    //Polygons and Lines with Measures
                    contentlength = 32 + (mvarVertices.Count * 12) + ((mvarParts.Count - 1) * 2);

                    break;
                case eShapeType.shpPointZ:
                    //Points with Z Value and Measures
                    contentlength = 18;

                    break;
                case eShapeType.shpMultiPointZ:
                    //Multi Points with Z Value and Measures
                    contentlength = 36 + (mvarVertices.Count * 16);

                    break;
                case eShapeType.shpPolygonZ:
                case eShapeType.shpPolyLineZ:
                    //Polygons and Lines with Z Value and Measures
                    contentlength = 40 + (mvarVertices.Count * 16) + ((mvarParts.Count - 1) * 2);

                    break;
                case eShapeType.shpMultiPatch:
                    //Multi Patch with Z Value and Measures
                    contentlength = 36 + (mvarVertices.Count * 16) + ((mvarParts.Count - 1) * 6);

                    break;
            }
            return contentlength;

        } // Calculate the Content Length of the ShapeFile Record in DWORDS

        private void ReadShapeHeader(FileStream fsShapeFile)
        {
            // *********************************************************
            // * Read the Shape File Header to get MBRs and Shape Type *
            // *********************************************************

            byte[] ByteArray = new byte[100];


            // Read the initial 32 byte header
            fsShapeFile.Seek(0, SeekOrigin.Begin);
            fsShapeFile.Read(ByteArray, 0, 100);
            mvarShapeType = BitConverter.ToInt32(ByteArray, 32);
            mvarShapeFileXMin = BitConverter.ToDouble(ByteArray, 36);
            mvarShapeFileYMin = BitConverter.ToDouble(ByteArray, 44);
            mvarShapeFileXMax = BitConverter.ToDouble(ByteArray, 52);
            mvarShapeFileYMax = BitConverter.ToDouble(ByteArray, 60);
            mvarShapeFileZMin = BitConverter.ToDouble(ByteArray, 68);
            mvarShapeFileZMax = BitConverter.ToDouble(ByteArray, 76);
            mvarShapeFileMMin = BitConverter.ToDouble(ByteArray, 84);
            mvarShapeFileMMax = BitConverter.ToDouble(ByteArray, 92);
            if (mvarShapeFileMMin.ToString() == "NaN")
            { mvarShapeFileMMin = null; }
            else
            {
                if (mvarShapeFileMMin < -1E+38)
                { mvarShapeFileMMin = null; }
            }
            if (mvarShapeFileMMax.ToString() == "NaN")
            { mvarShapeFileMMax = null; }
            else
            {
                if (mvarShapeFileMMax < -1E+38)
                { mvarShapeFileMMax = null; }
            }
            if (mvarShapeType <= 8)
            {
                // 2D shapes
                mvarShapeFileZMin = 0;
                mvarShapeFileZMax = 0;
                mvarShapeFileMMin = null;
                mvarShapeFileMMax = null;
            }
            else if (mvarShapeType >= 21)
            {
                if (mvarShapeType <= 28)
                {
                    // 2D shapes with Measures
                    mvarShapeFileZMin = 0;
                    mvarShapeFileZMax = 0;
                }
            }
        } //Read the Shape File Header to get MBRs and Shape Type

        private void ReadShapeRecordHeader(byte[] vertData)
        {
            // ******************************************************************
            // * Reads the shape MBR of the shape Vertice into public variables *
            // ******************************************************************

            int ArrayPos;

            mvarShapeArea = null;
            mvarCentroidX = 0;
            mvarCentroidY = 0;
            mvarPerimeter = 0;
            mvarNoOfParts = 0;
            mvarNoOfPoints = 0;
            mvarShapeXMin = 0;
            mvarShapeYMin = 0;
            mvarShapeXMax = 0;
            mvarShapeYMax = 0;
            mvarShapeZMin = 0;
            mvarShapeZMax = 0;
            mvarShapeMMin = null;
            mvarShapeMMax = null;

            if (vertData.Length == 0)
            { throw new Exception("The Vertice data has not been read correctly"); }

            // Read in Shape Type of record
            mvarRecordShapeType = BitConverter.ToInt32(vertData, 0);
            mvarIsNull = false;

            if (mvarRecordShapeType == Convert.ToInt32(eShapeType.shpNull))
            {
                mvarIsNull = true;
                return;
            }

            // Read base XY info
            mvarShapeXMin = BitConverter.ToDouble(vertData, 4);
            mvarShapeYMin = BitConverter.ToDouble(vertData, 12);

            switch ((eShapeType)mvarRecordShapeType)
            {

                case eShapeType.shpPoint:
                    mvarShapeXMax = mvarShapeXMin;
                    mvarShapeYMax = mvarShapeYMin;
                    mvarNoOfPoints = 1;

                    break;
                case eShapeType.shpPointM:
                    mvarShapeMMin = BitConverter.ToDouble(vertData, 20);
                    if (mvarShapeMMin.ToString() == "NaN")
                        mvarShapeMMin = -1.1E+38;
                    mvarShapeXMax = mvarShapeXMin;
                    mvarShapeYMax = mvarShapeYMin;
                    mvarShapeMMax = mvarShapeMMin;
                    mvarNoOfPoints = 1;

                    break;
                case eShapeType.shpPointZ:
                    mvarShapeZMin = BitConverter.ToDouble(vertData, 20);
                    mvarShapeMMin = BitConverter.ToDouble(vertData, 28);
                    if (mvarShapeMMin.ToString() == "NaN")
                        mvarShapeMMin = null;
                    mvarShapeXMax = mvarShapeXMin;
                    mvarShapeYMax = mvarShapeYMin;
                    mvarShapeMMax = mvarShapeMMin;
                    mvarShapeZMax = mvarShapeZMin;
                    mvarNoOfPoints = 1;

                    break;
                case eShapeType.shpMultiPoint:
                    mvarShapeXMax = BitConverter.ToDouble(vertData, 20);
                    mvarShapeYMax = BitConverter.ToDouble(vertData, 28);
                    mvarNoOfPoints = BitConverter.ToInt32(vertData, 36);

                    break;
                case eShapeType.shpMultiPointZ:
                    mvarShapeXMax = BitConverter.ToDouble(vertData, 20);
                    mvarShapeYMax = BitConverter.ToDouble(vertData, 28);
                    mvarNoOfPoints = BitConverter.ToInt32(vertData, 36);
                    mvarShapeZMin = BitConverter.ToDouble(vertData, 40 + (mvarNoOfPoints * 16));
                    mvarShapeZMax = BitConverter.ToDouble(vertData, 48 + (mvarNoOfPoints * 16));
                    ArrayPos = 56 + (mvarNoOfPoints * 24);
                    if (vertData.Length > ArrayPos)
                    {
                        mvarShapeMMin = BitConverter.ToDouble(vertData, ArrayPos);
                        mvarShapeMMax = BitConverter.ToDouble(vertData, ArrayPos + 8);
                    }

                    break;
                case eShapeType.shpMultiPointM:
                    mvarShapeXMax = BitConverter.ToDouble(vertData, 20);
                    mvarShapeYMax = BitConverter.ToDouble(vertData, 28);
                    mvarNoOfPoints = BitConverter.ToInt32(vertData, 36);
                    ArrayPos = 40 + (16 * mvarNoOfPoints);
                    if (vertData.Length > ArrayPos)
                    {
                        mvarShapeMMin = BitConverter.ToDouble(vertData, ArrayPos);
                        mvarShapeMMax = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (mvarShapeMMin.ToString() == "NaN")
                            mvarShapeMMin = null;
                        if (mvarShapeMMax.ToString() == "NaN")
                            mvarShapeMMax = null;
                    }


                    break;

                case eShapeType.shpPolyLine:
                case eShapeType.shpPolygon:
                    mvarShapeXMax = BitConverter.ToDouble(vertData, 20);
                    mvarShapeYMax = BitConverter.ToDouble(vertData, 28);
                    mvarNoOfParts = BitConverter.ToInt32(vertData, 36);
                    mvarNoOfPoints = BitConverter.ToInt32(vertData, 40);

                    break;
                case eShapeType.shpPolyLineM:
                case eShapeType.shpPolygonM:
                    mvarShapeXMax = BitConverter.ToDouble(vertData, 20);
                    mvarShapeYMax = BitConverter.ToDouble(vertData, 28);
                    mvarNoOfParts = BitConverter.ToInt32(vertData, 36);
                    mvarNoOfPoints = BitConverter.ToInt32(vertData, 40);
                    ArrayPos = 44 + (16 * mvarNoOfPoints) + (4 * mvarNoOfParts); ;
                    if (vertData.Length > ArrayPos)
                    {
                        mvarShapeMMin = BitConverter.ToDouble(vertData, ArrayPos);
                        mvarShapeMMax = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (mvarShapeMMin.ToString() == "NaN")
                            mvarShapeMMin = null;
                        if (mvarShapeMMax.ToString() == "NaN")
                            mvarShapeMMax = null;
                    }

                    break;
                case eShapeType.shpPolyLineZ:
                case eShapeType.shpPolygonZ:
                    mvarShapeXMax = BitConverter.ToDouble(vertData, 20);
                    mvarShapeYMax = BitConverter.ToDouble(vertData, 28);
                    mvarNoOfParts = BitConverter.ToInt32(vertData, 36);
                    mvarNoOfPoints = BitConverter.ToInt32(vertData, 40);
                    ArrayPos = 44 + (16 * mvarNoOfPoints) + (4 * mvarNoOfParts); ;
                    mvarShapeZMin = BitConverter.ToDouble(vertData, ArrayPos);
                    mvarShapeZMax = BitConverter.ToDouble(vertData, ArrayPos + 8);
                    ArrayPos += 16 + (8 * mvarNoOfPoints);
                    if (vertData.Length > ArrayPos)
                    {
                        mvarShapeMMin = BitConverter.ToDouble(vertData, ArrayPos);
                        mvarShapeMMax = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (mvarShapeMMin.ToString() == "NaN")
                            mvarShapeMMin = null;
                        if (mvarShapeMMax.ToString() == "NaN")
                            mvarShapeMMax = null;
                    }

                    break;
                case eShapeType.shpMultiPatch:
                    mvarShapeXMax = BitConverter.ToDouble(vertData, 20);
                    mvarShapeYMax = BitConverter.ToDouble(vertData, 28);
                    mvarNoOfParts = BitConverter.ToInt32(vertData, 36);
                    mvarNoOfPoints = BitConverter.ToInt32(vertData, 40);
                    ArrayPos = 44 + (16 * mvarNoOfPoints) + (8 * mvarNoOfParts); ;
                    mvarShapeZMin = BitConverter.ToDouble(vertData, ArrayPos);
                    mvarShapeZMax = BitConverter.ToDouble(vertData, ArrayPos + 8);
                    ArrayPos += 16 + (8 * mvarNoOfPoints);
                    if (vertData.Length > ArrayPos)
                    {
                        mvarShapeMMin = BitConverter.ToDouble(vertData, ArrayPos);
                        mvarShapeMMax = BitConverter.ToDouble(vertData, ArrayPos + 8);
                        if (mvarShapeMMin.ToString() == "NaN")
                            mvarShapeMMin = null;
                        if (mvarShapeMMax.ToString() == "NaN")
                            mvarShapeMMax = null;
                    }

                    break;
            }
            if (mvarNoOfPoints == 0)
            {
                mvarIsNull = true;
                mvarRecordShapeType = 0;
            }

            if (mvarNoOfParts == 0) mvarNoOfParts = 1;
            if (mvarShapeMMin != null)
            {
                if (mvarShapeMMin <= -1E+38)
                {
                    mvarShapeMMin = null;
                }
            }
            if (mvarShapeMMax != null)
            {
                if (mvarShapeMMax <= -1E+38)
                {
                    mvarShapeMMax = null;
                }
            }

            mvarVertices.xMin = mvarShapeXMin;
            mvarVertices.yMin = mvarShapeYMin;
            mvarVertices.xMax = mvarShapeXMax;
            mvarVertices.yMax = mvarShapeYMax;
            mvarVertices.zMin = mvarShapeZMin;
            mvarVertices.zMin = mvarShapeZMax;
            mvarVertices.mMin = mvarShapeMMin;
            mvarVertices.mMin = mvarShapeMMax;
            mvarVertices.NoOfPoints = mvarNoOfPoints;

        }

        private Fields ReadDBFHeader(FileStream fsDataFile)
        {
            // ********************************************************************
            // * Reads the DBF field info and populates the ShapeField collection *
            // ********************************************************************
            Fields outFields = new Fields();
            byte[] lvarFieldName = new byte[10];
            byte[] lvarHeader = new byte[32];
            int lvarFieldPos = 0;
            eFieldType lvarFieldType = default(eFieldType);
            short lvarFieldSize = 0;
            short lvarFieldDec = 0;
            short i = 0;
            string svarFieldname = null;
            Int16 dbfHeaderLength;
            Int16 dbfRecordlength;
            int lvarRecordCount = 0;


            // Read the initial 32 byte header
            fsDataFile.Seek(0, SeekOrigin.Begin);
            fsDataFile.Read(lvarHeader, 0, 32);

            // Record count
            lvarRecordCount = BitConverter.ToInt32(lvarHeader, 4);

            // Header Length
            dbfHeaderLength = BitConverter.ToInt16(lvarHeader, 8);

            // Record Length
            dbfRecordlength = BitConverter.ToInt16(lvarHeader, 10);

            int lvarFieldCount = Convert.ToInt32(dbfHeaderLength / 32) - 1;

            // International character
            eLanguage lvarLanguage = (eLanguage)lvarHeader[29];
            string lvardbfDelimiter = SetDelimiter(lvarLanguage);

            outFields.HeaderLength = dbfHeaderLength;
            outFields.Recordlength = dbfRecordlength;
            outFields.Language = lvarLanguage;
            outFields.Delimiter = lvardbfDelimiter;
            outFields.RecordCount = lvarRecordCount;
            // Populate the Fields
            byte[] bFields = new byte[(32 * lvarFieldCount)];
            fsDataFile.Read(bFields, 0, 32 * lvarFieldCount);

            // Add record to the shape fields
            lvarFieldPos = 0;
            for (i = 0; i < lvarFieldCount; i++)
            {
                // Fieldname is null terminated
                Buffer.BlockCopy(bFields, lvarFieldPos, lvarFieldName, 0, 10);
                svarFieldname = ByteArrayToString(lvarFieldName);

                // "N"
                if (bFields[lvarFieldPos + 11] == 78)
                {
                    lvarFieldType = eFieldType.shpNumeric;
                    // "F"
                }
                else if (bFields[lvarFieldPos + 11] == 70)
                {
                    lvarFieldType = eFieldType.shpFloat;
                    // "D"
                }
                else if (bFields[lvarFieldPos + 11] == 68)
                {
                    lvarFieldType = eFieldType.shpDate;
                    // "C"
                }
                else if (bFields[lvarFieldPos + 11] == 67)
                {
                    lvarFieldType = eFieldType.shpText;
                    // "L"
                }
                else if (bFields[lvarFieldPos + 11] == 76)
                {
                    lvarFieldType = eFieldType.shpBoolean;
                    // "M"
                }
                else
                {
                    lvarFieldType = eFieldType.shpText;
                }

                lvarFieldSize = bFields[lvarFieldPos + 16];
                lvarFieldDec = bFields[lvarFieldPos + 17];

                // create the field
                outFields.Add(svarFieldname, lvarFieldType, lvarFieldSize, lvarFieldDec);

                lvarFieldPos = lvarFieldPos + 32;

            }
            return outFields;

        } // Reads the DBF field info and creates the Field collection

        private void ReadDBFRecord(int RecordNumber)
        {
            // ******************************************************************
            // * Read in the DBF record and populate the ShapeFields Collection *
            // ******************************************************************

            int FilePos = 0;
            short Exponent = 0;
            string DataValue = null;

            // Read in entire record
            try
            { OpenStream(mvarShapeDBF, ref fsDataFile); }
            catch
            { throw new Exception("The Database File " + mvarShapeDBF + " has been locked by another application"); }

            if (mvarFields == null)
                mvarFields = ReadDBFHeader(fsDataFile);
            long FileLoc = mvarFields.HeaderLength + (mvarFields.Recordlength * (RecordNumber - 1));

            byte[] lvarDBFData = new byte[mvarFields.Recordlength];
            fsDataFile.Seek(FileLoc, SeekOrigin.Begin);
            fsDataFile.Read(lvarDBFData, 0, lvarDBFData.Length);
            CloseStream(ref fsDataFile);

            // Debugging 
            //Console.WriteLine(ByteArrayToString(lvarDBFData));


            // Reset the default field value to NULL
            mvarFields.Strip();

            if (lvarDBFData[0] == 0x2a)
            {
                mvarFields.isDeleted = true;
            }
            else
            {
                mvarFields.isDeleted = false;
            }

            FilePos = 1;
            for (int i = 0; i < mvarFields.Count; i++)
            {
                Field thisField = (Field)mvarFields[i];
                byte[] bDataValue = new byte[thisField.Size + 1];
                Buffer.BlockCopy(lvarDBFData, FilePos, bDataValue, 0, thisField.Size);
                
                //Special case if all the chars are 2a then the result is  a null
                DataValue = "";
                for (int bTest = 0; bTest < bDataValue.Length - 1; bTest++)
                {
                    if (bDataValue[bTest] != 42)
                    {
                        DataValue = ByteArrayToString(bDataValue);
                        break;
                    }
                }

                // trim nulls
                DataValue = DataValue.Replace("\0", "");

                if (!String.IsNullOrEmpty(DataValue.Trim()))
                {
                    switch (thisField.Type)
                    {
                        case eFieldType.shpText:
                            thisField.Value = DataValue.TrimEnd();
                            break;
                        case eFieldType.shpNumeric:
                            if (mvarsysDelimiter != ".")
                            { DataValue = DataValue.Replace(mvardbfDelimiter, mvarsysDelimiter); }
                            thisField.Value = Convert.ToDouble(DataValue);
                            break;
                        case eFieldType.shpFloat:
                            // Floating values are represented by values and expontent e.g. 1.23425e+004
                            //Check the delimiter
                            if (!DataValue.Contains("FINITY"))
                            {
                                if (mvarsysDelimiter != ".")
                                { DataValue = DataValue.Replace(mvardbfDelimiter, mvarsysDelimiter); }
                                if (DataValue.ToUpper().Contains("E"))
                                {
                                    Exponent = Convert.ToInt16(DataValue.Substring(DataValue.ToUpper().IndexOf("E") + 1));
                                    DataValue = DataValue.Substring(0, DataValue.ToUpper().IndexOf("E"));
                                    thisField.Value = Convert.ToDouble(DataValue) * (Math.Pow(10, Exponent));
                                }
                                else
                                {
                                    thisField.Value = Convert.ToDouble(DataValue);
                                }
                            }
                            break;
                        case eFieldType.shpBoolean:
                            if (DataValue.ToUpper() == "Y" | DataValue.ToUpper() == "T")
                            { thisField.Value = true; }
                            if (DataValue.ToUpper() == "N" | DataValue.ToUpper() == "F")
                            { thisField.Value = false; }
                            break;
                        case eFieldType.shpDate:
                            if (Convert.ToInt32(DataValue) > 0)
                            {
                                try
                                {
                                    // Read in YYYYMMDD and convert to generic date for CDate
                                    if (mvarYYYYMMDD) //Format "ddmmyyyy" )
                                    { thisField.Value = System.DateTime.ParseExact(DataValue, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); }
                                    else // Format "yyyymmdd"
                                    { thisField.Value = System.DateTime.ParseExact(DataValue, "ddMMyyyy", System.Globalization.CultureInfo.CurrentCulture); }
                                }
                                catch
                                {
                                    thisField.Value = null;
                                }

                            }
                            break;
                        default:    // String
                            thisField.Value = DataValue;
                            break;
                    }
                }
                FilePos += thisField.Size;
            }
            Globals.mvarFieldChange = false;
        } // Read in the DBF record and populate the Fields Collection

        private string ReadDBFToString(int RecordNumber)
        {
            // ****************************************************************
            // * Reads in a DBF record and outputs the whole line to a string *
            // * This function is used by FindBySQL for easy data parsing     *
            // ****************************************************************
            // Read in entire record
            long FileLoc = mvarFields.HeaderLength + (mvarFields.Recordlength * (RecordNumber - 1));
            try
            { OpenStream(mvarShapeDBF, ref fsDataFile); }
            catch
            { throw new Exception("The Database File " + mvarShapeDBF + " has been locked by another application"); }

            byte[] lvarDBFData = new byte[mvarFields.Recordlength];
            fsDataFile.Seek(FileLoc, SeekOrigin.Begin);
            fsDataFile.Read(lvarDBFData, 0, lvarDBFData.Length);
            CloseStream(ref fsDataFile);

            if (lvarDBFData[0] == 0x2a)
            {
                return null;
            }
            else
            {
                string DataString = ByteArrayToString(lvarDBFData);
                return DataString.Substring(1);
            }
        }

        private void ReadShapeRecord()
        {
            // *********************************************************************
            // * Reads the Shape Record data and populates the Vertices collection *
            // *********************************************************************

            int PartNo = 0;
            int ArrayPos;

            double XVal = 0;
            double YVal = 0;
            object MVal = null;
            double mVal = 0;
            double ZVal = 0;

            // Area and Centroid Variables
            double xSum = 0;
            double ySum = 0;
            double aSum = 0;
            double AreaFactor = 0;
            double HoldXVal = 0;
            double HoldYVal = 0;

            mvarShapeArea = null;
            mvarCentroidX = 0;
            mvarCentroidY = 0;
            mvarPerimeter = 0;


            // Read in Shape Type of record
            mvarRecordShapeType = BitConverter.ToInt32(mvarVertices.vertData, 0);

            if (mvarRecordShapeType == Convert.ToInt32(eShapeType.shpNull) || mvarIsNull )
            {
                // Nothing to do
                mvarRecordShapeType = 0;
                return;
            }

            // Fast read only - populate the Vertice copy of the data array
            if (mvarReadmode == eReadMode.FastRead)
            {
                mvarVertices.ReadMode = eReadMode.FastRead;
            }
            else
            {
                mvarVertices.Strip();
            }


            switch ((eShapeType)mvarRecordShapeType)
            {
                #region Points
                case eShapeType.shpPoint:
                    if (mvarReadmode != eReadMode.FastRead)
                    {   mvarVertices.Add(mvarVertices.xMin, mvarVertices.yMin); }
                    break;
                case eShapeType.shpPointM:
                    if (mvarReadmode != eReadMode.FastRead)
                    {
                        MVal = mvarVertices.mMin;
                        mvarVertices.Add(mvarVertices.xMin, mvarVertices.yMin, MVal);
                    }

                    break;
                case eShapeType.shpPointZ:
                    if (mvarReadmode != eReadMode.FastRead)
                    {
                        MVal = mvarVertices.mMin;
                        mvarVertices.Add(mvarVertices.xMin, mvarVertices.yMin, MVal, Convert.ToDouble(mvarVertices.zMin));
                    }

                    break;
                case eShapeType.shpMultiPoint:
                case eShapeType.shpMultiPointZ:
                case eShapeType.shpMultiPointM:
                    ArrayPos = 40;
                    xSum = 0;
                    ySum = 0;
                    aSum = 0;

                    for (int i = 0; i < mvarNoOfPoints; i++)
                    {
                        XVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                        YVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                        if (i == 0)
                        {
                            mvarPartXMin = XVal;
                            mvarPartXMax = XVal;
                            mvarPartYMin = YVal;
                            mvarPartYMax = YVal;
                        }
                        else
                        {
                            mvarPartXMin = Math.Min(mvarPartXMin, XVal);
                            mvarPartYMin = Math.Min(mvarPartYMin, YVal);
                            mvarPartXMax = Math.Max(mvarPartXMax, XVal);
                            mvarPartYMax = Math.Max(mvarPartYMax, YVal);
                        }
                        mvarParts.Add(i);
                        Part mPart = (Part)mvarParts[i];
                        mPart.Ends = i;
                        mPart.MBRXMin = XVal;
                        mPart.MBRXMax = XVal;
                        mPart.MBRYMin = YVal;
                        mPart.MBRYMax = YVal;

                        if (mvarReadmode != eReadMode.FastRead)
                        { mvarVertices.Add(XVal, YVal); }
                        ArrayPos += 16;
                        if (i > 0)
                        {
                            AreaFactor = ((HoldXVal * YVal) - (XVal * HoldYVal));
                            xSum = xSum + (HoldXVal + XVal) * AreaFactor;
                            ySum = ySum + (HoldYVal + YVal) * AreaFactor;
                            aSum = aSum + AreaFactor;
                        }
                        HoldXVal = XVal;
                        HoldYVal = YVal;
                    }
                    aSum = aSum / 2;
                    if (xSum != 0 & aSum != 0 & ySum != 0)
                    {
                        mvarCentroidX = (xSum / (6 * aSum));
                        mvarCentroidY = (ySum / (6 * aSum));
                    }

                    if ((eShapeType)mvarShapeType == eShapeType.shpMultiPointZ)
                    {
                        ArrayPos = ArrayPos + 16;
                        if (mvarReadmode != eReadMode.FastRead)
                        {
                            for (int i = 0; i < mvarNoOfParts; i++)
                            {
                                ZVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                mvarVertices[i].Z_Cord = ZVal;
                                Part mPart = (Part)mvarParts[i];
                                mPart.zMin = ZVal;
                                mPart.zMax = ZVal;
                                ArrayPos += 8;
                            }
                        }
                        else
                        { ArrayPos += (mvarNoOfPoints * 8); }
                    }

                    if ((eShapeType)mvarShapeType == eShapeType.shpMultiPointZ | (eShapeType)mvarShapeType == eShapeType.shpMultiPointM)
                    {
                        if (mvarVertices.vertData.Length > ArrayPos)
                        {
                            if (mvarReadmode != eReadMode.FastRead)
                            {
                                for (int i = 0; i < mvarNoOfParts; i++)
                                {
                                    MVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    if (!DBNull.Value.Equals(MVal))
                                    {
                                        if (MVal.ToString() != "NaN")
                                        {
                                            if (Convert.ToDouble(MVal) > -1E+38)
                                            {
                                                mvarVertices[i].Measure = Convert.ToDouble(MVal);
                                                Part mPart = (Part)mvarParts[i];
                                                mPart.MeasureMin = Convert.ToDouble(MVal);
                                                mPart.MeasureMax = Convert.ToDouble(MVal);
                                            }
                                        }
                                    }
                                    ArrayPos += 8;
                                }
                            }
                        }
                    }

                    break;
                #endregion

                #region Poly Lines
                case eShapeType.shpPolyLine:
                case eShapeType.shpPolyLineZ:
                case eShapeType.shpPolyLineM:
                    {
                        mvarShapeArea = null;
                        ArrayPos = 44;
                        for (int i = 0; i < mvarNoOfParts; i++)
                        {
                            int vertBegin = BitConverter.ToInt32(mvarVertices.vertData, ArrayPos);
                            // create a new part
                            mvarParts.Add(vertBegin);
                            if (i > 0)
                                mvarParts[i - 1].Ends = vertBegin - 1;
                            ArrayPos += 4;
                        }
                        mvarParts[mvarParts.Count - 1].Ends = mvarNoOfPoints - 1;

                        mvarPerimeter = 0;
                        if (mvarReadmode != eReadMode.FastRead)
                        {
                            for (int i = 0; i < mvarNoOfParts; i++)
                            {
                                double SumPerimeter = 0.0;
                                int startArrayPos = ArrayPos;
                                Part mPart = (Part)mvarParts[i];

                                HoldXVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                HoldYVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                mvarPartXMin = HoldXVal;
                                mvarPartYMin = HoldYVal;
                                mvarPartXMax = HoldXVal;
                                mvarPartYMax = HoldYVal;
                                ArrayPos = ArrayPos + 16;

                                if (mvarReadmode != eReadMode.FastRead)
                                    mvarVertices.Add(HoldXVal, HoldYVal);

                                for (int j = mPart.Begins + 1; j <= mPart.Ends; j++)
                                {
                                    XVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    YVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                    // Add vertices to collection
                                    if (mvarReadmode != eReadMode.FastRead)
                                        mvarVertices.Add(XVal, YVal);
                                    ArrayPos = ArrayPos + 16;
                                    // Part Min/Max
                                    mvarPartXMin = Math.Min(mvarPartXMin, XVal);
                                    mvarPartYMin = Math.Min(mvarPartYMin, YVal);
                                    mvarPartXMax = Math.Max(mvarPartXMax, XVal);
                                    mvarPartYMax = Math.Max(mvarPartYMax, YVal);

                                    // Calculate Area of Part
                                    SumPerimeter += Math.Sqrt(Math.Pow((HoldXVal - XVal), 2) + Math.Pow((YVal - HoldYVal), 2));
                                    mvarPerimeter += Math.Sqrt(Math.Pow((HoldXVal - XVal), 2) + Math.Pow((YVal - HoldYVal), 2));
                                    HoldXVal = XVal;
                                    HoldYVal = YVal;
                                }
                                mPart.MBRXMax = mvarPartXMax;
                                mPart.MBRXMin = mvarPartXMin;
                                mPart.MBRYMax = mvarPartYMax;
                                mPart.MBRYMin = mvarPartYMin;
                                mPart.Perimeter = SumPerimeter;
                                mPart.IsHole = false;
                                mPart.Area = 0.0;
                                int endArrayPos = ArrayPos;

                                double midDistance = SumPerimeter / 2.0;
                                double segLength = 0.0;
                                ArrayPos = startArrayPos;

                                for (int j = mPart.Begins; j < mPart.Ends; j++)
                                {
                                    HoldXVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    HoldYVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                    ArrayPos = ArrayPos + 16;

                                    XVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    YVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                    double eleLength = Math.Sqrt(Math.Pow((HoldXVal - XVal), 2) + Math.Pow((YVal - HoldYVal), 2));
                                    segLength += eleLength;
                                    if (segLength > midDistance)
                                    {
                                        double Angle = GetVerticeAngle(HoldXVal, HoldYVal, XVal, YVal);
                                        ExtendLine(eleLength - (segLength - midDistance), Angle, ref HoldXVal, ref HoldYVal);
                                        mPart.CentroidX = HoldXVal;
                                        mPart.CentroidY = HoldYVal;
                                        break;
                                    }
                                }
                                ArrayPos = endArrayPos;
                            }

                            // Use the average centroid for multipart lines
                            mvarCentroidX = 0.0;
                            mvarCentroidY = 0.0;
                            for (int i = 0; i < mvarParts.Count; i++)
                            {
                                mvarCentroidX += (double)mvarParts[i].CentroidX;
                                mvarCentroidY += (double)mvarParts[i].CentroidY;
                            }
                            mvarCentroidX /= mvarParts.Count;
                            mvarCentroidY /= mvarParts.Count;

                            // Z - Coordinate data
                            if ((eShapeType)mvarRecordShapeType == eShapeType.shpPolyLineZ)
                            {
                                mvarShapeZMin = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                mvarShapeZMax = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                ArrayPos += 16;
                                PartNo = 0;
                                for (int i = 0; i < mvarNoOfPoints; i++)
                                {
                                    ZVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    if (mvarReadmode != eReadMode.FastRead) { mvarVertices[i].Z_Cord = ZVal; }
                                    ArrayPos += 8;

                                    Part mPart = mvarParts[PartNo];
                                    if (mPart.Begins == i)
                                    {
                                        HoldXVal = ZVal;
                                        HoldYVal = ZVal;
                                    }
                                    else
                                    {
                                        HoldXVal = Math.Min(HoldXVal, ZVal);
                                        HoldYVal = Math.Max(HoldXVal, ZVal);
                                    }

                                    if (mPart.Ends == i)
                                    {
                                        mPart.zMin = HoldXVal;
                                        mPart.zMax = HoldYVal;
                                        PartNo++;
                                    }

                                }
                            }


                            // Measures data
                            if (ArrayPos < mvarVertices.vertData.Length)
                            {
                                if ((eShapeType)mvarRecordShapeType == eShapeType.shpPolyLineM || (eShapeType)mvarRecordShapeType == eShapeType.shpPolyLineZ)
                                {
                                    mvarShapeMMin = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    mvarShapeMMax = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                    ArrayPos += 16;
                                    PartNo = 0;
                                    for (int i = 0; i < mvarNoOfPoints; i++)
                                    {
                                        mVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                        if (!DBNull.Value.Equals(MVal))
                                            mVal = -1.1E+38;
                                        else if (mVal.ToString() == "NaN")
                                            mVal = -1.1E+38;
                                        if (mvarReadmode != eReadMode.FastRead)
                                        {
                                            // If less than this then value is null
                                            if (mVal > -1E+38)
                                                mvarVertices[i].Measure = mVal;
                                        }
                                        ArrayPos += 8;

                                        Part mPart = (Part)mvarParts[PartNo];
                                        if (mPart.Begins == i)
                                        {
                                            HoldXVal = mVal;
                                            HoldYVal = mVal;
                                        }
                                        else
                                        {
                                            HoldXVal = Math.Min(HoldXVal, mVal);
                                            HoldYVal = Math.Max(HoldXVal, mVal);
                                        }

                                        if (mPart.Ends == i)
                                        {
                                            if (HoldXVal > -1E+38)
                                                mPart.MeasureMin = HoldXVal;
                                            if (HoldYVal > -1E+38)
                                                mPart.MeasureMax = HoldYVal;
                                            PartNo++;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                #endregion

                #region Polygons
                case eShapeType.shpPolygon:
                case eShapeType.shpPolygonZ:
                case eShapeType.shpPolygonM:
                case eShapeType.shpMultiPatch:
                    {
                        ArrayPos = 44;
                        for (int i = 0; i < mvarNoOfParts; i++)
                        {
                            int vertBegin = BitConverter.ToInt32(mvarVertices.vertData, ArrayPos);
                            // create a new part
                            mvarParts.Add(vertBegin);
                            if (i > 0)
                                mvarParts[i - 1].Ends = vertBegin - 1;
                            ArrayPos += 4;
                        }
                        mvarParts[mvarParts.Count - 1].Ends = mvarNoOfPoints - 1;

                        if ((eShapeType)mvarShapeType == eShapeType.shpMultiPatch)
                        {
                            //MultiPatch part types
                            for (int i = 0; i < mvarNoOfParts; i++)
                            {
                                int patchType = BitConverter.ToInt32(mvarVertices.vertData, ArrayPos);
                                ArrayPos += 4;
                                mvarParts[i].PartType = (ePartType)patchType;
                            }
                        }

                        // set up variables for area calculation
                        xSum = 0;
                        ySum = 0;
                        aSum = 0;
                        mvarPerimeter = 0;
                        if (mvarReadmode != eReadMode.FastRead)
                        {
                            for (int i = 0; i < mvarNoOfParts; i++)
                            {

                                Part mPart = (Part)mvarParts[i];
                                double partialArea;
                                double SumArea2 = 0;
                                double SumXCentroid = 0.0;
                                double SumYCentroid = 0.0;
                                double SumPerimeter = 0.0;

                                double firstX = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                double firstY = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                double lastX = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                double lastY = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                // Add vertices to collection
                                mvarVertices.Add(firstX, firstY);

                                //System.Diagnostics.Debug.Print("Part: {0}", i);
                                //System.Diagnostics.Debug.Print("ArrayPos: {0}   X: {1}  Y: {2}", ArrayPos, firstX, firstY);

                                mvarPartXMin = firstX;
                                mvarPartYMin = firstY;
                                mvarPartXMax = firstX;
                                mvarPartYMax = firstY;

                                // Find the area by dividing the shape into triangles
                                for (int j = mPart.Begins + 1; j < mPart.Ends; j++)
                                {
                                    ArrayPos += 16;
                                    double midX = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    double midY = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                    SumPerimeter += Math.Sqrt(Math.Pow((midX - firstX), 2) + Math.Pow((midY - firstY), 2));
                                    // Add vertices to collection
                                    mvarVertices.Add(midX, midY);

                                    //System.Diagnostics.Debug.Print("ArrayPos: {0}   X: {1}  Y: {2}", ArrayPos, midX, midY);

                                    mvarPartXMin = Math.Min(mvarPartXMin, midX);
                                    mvarPartYMin = Math.Min(mvarPartYMin, midY);
                                    mvarPartXMax = Math.Max(mvarPartXMax, midX);
                                    mvarPartYMax = Math.Max(mvarPartYMax, midY);
                                    ArrayPos += 16;
                                    lastX = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    lastY = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                    // Add vertices to collection
                                    //if (mvarReadmode != eReadMode.FastRead)
                                    //    mvarVertices.Add(lastX, lastY);
                                    mvarPartXMin = Math.Min(mvarPartXMin, lastX);
                                    mvarPartYMin = Math.Min(mvarPartYMin, lastY);
                                    mvarPartXMax = Math.Max(mvarPartXMax, lastX);
                                    mvarPartYMax = Math.Max(mvarPartYMax, lastY);
                                    ArrayPos -= 16;

                                    double triCentroidX = firstX + midX + lastX;
                                    double triCentroidY = firstY + midY + lastY;
                                    partialArea = (midX - firstX) * (lastY - firstY) - (lastX - firstX) * (midY - firstY);
                                    SumXCentroid += partialArea * triCentroidX;
                                    SumYCentroid += partialArea * triCentroidY;

                                    SumArea2 += partialArea;
                                    SumPerimeter += Math.Sqrt(Math.Pow((midX - lastX), 2) + Math.Pow((midY - lastY), 2));
                                }
                                mvarVertices.Add(lastX, lastY);
                                ArrayPos += 32;
                                //System.Diagnostics.Debug.Print("ArrayPos: {0}   X: {1}  Y: {2}", ArrayPos - 16, lastX, lastY);

                                mvarPerimeter += SumPerimeter;
                                // Just changed this to rolling addition of these 3 terms
                                xSum += SumXCentroid;
                                ySum += SumYCentroid;
                                aSum += SumArea2;

                                SumXCentroid /= 3 * SumArea2;
                                SumYCentroid /= 3 * SumArea2;

                                mPart.MBRXMax = mvarPartXMax;
                                mPart.MBRXMin = mvarPartXMin;
                                mPart.MBRYMax = mvarPartYMax;
                                mPart.MBRYMin = mvarPartYMin;
                                mPart.Perimeter = SumPerimeter;
                                mPart.Area = (SumArea2 * 0.5) * -1;
                                mPart.CentroidX = SumXCentroid;
                                mPart.CentroidY = SumYCentroid;

                                if (mvarShapeArea == null)
                                { mvarShapeArea = mPart.Area; }
                                else
                                { mvarShapeArea += mPart.Area; }
                                if (SumArea2 > 0)
                                {
                                    mPart.Direction = eDirection.AntiClockwise;
                                    mPart.IsHole = true;
                                }
                                else
                                {
                                    mPart.Direction = eDirection.Clockwise;
                                    mPart.IsHole = false;
                                }

                                if (mvarNoOfParts == 1)
                                {
                                    mPart.IsHole = false;
                                }

                                System.Diagnostics.Debug.Print("Part {0}, Begins {1}, Ends {2}, Vertice Count {3}", i, mPart.Begins, mPart.Ends, mvarVertices.Count);

                            }

                            //if asum>0 then
                            //aSum = aSum / 2;
                            if (xSum != 0 & aSum != 0 & ySum != 0)
                            {
                                if (mvarNoOfParts == 1)
                                {
                                    mvarCentroidX = Convert.ToDouble(mvarParts[0].CentroidX);
                                    mvarCentroidY = Convert.ToDouble(mvarParts[0].CentroidY);
                                }
                                else
                                {
                                    mvarCentroidX = (xSum / (3 * aSum));
                                    mvarCentroidY = (ySum / (3 * aSum));
                                }

                            }

                            // Z - Coordinate data
                            if ((eShapeType)mvarRecordShapeType == eShapeType.shpPolygonZ || (eShapeType)mvarRecordShapeType == eShapeType.shpMultiPatch)
                            {
                                mvarShapeZMin = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                mvarShapeZMax = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                ArrayPos += 16;
                                PartNo = 0;
                                for (int i = 0; i < mvarNoOfPoints; i++)
                                {
                                    ZVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    if (mvarReadmode != eReadMode.FastRead) { mvarVertices[i].Z_Cord = ZVal; }
                                    ArrayPos += 8;

                                    Part mPart = mvarParts[PartNo];
                                    if (mPart.Begins == i)
                                    {
                                        HoldXVal = ZVal;
                                        HoldYVal = ZVal;
                                    }
                                    else
                                    {
                                        HoldXVal = Math.Min(HoldXVal, ZVal);
                                        HoldYVal = Math.Max(HoldXVal, ZVal);
                                    }

                                    if (mPart.Ends == i)
                                    {
                                        mPart.zMin = HoldXVal;
                                        mPart.zMax = HoldYVal;
                                        PartNo++;
                                    }

                                }
                            }


                            // Measures data
                            if (ArrayPos < mvarVertices.vertData.Length)
                            {
                                if ((eShapeType)mvarRecordShapeType == eShapeType.shpPolyLineM | (eShapeType)mvarRecordShapeType == eShapeType.shpPolygonM | (eShapeType)mvarRecordShapeType == eShapeType.shpPolyLineZ | (eShapeType)mvarRecordShapeType == eShapeType.shpPolygonZ | (eShapeType)mvarRecordShapeType == eShapeType.shpMultiPatch)
                                {
                                    mvarShapeMMin = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                    mvarShapeMMax = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                    ArrayPos += 16;
                                    PartNo = 0;
                                    if (ArrayPos + (mvarNoOfPoints * 16) <= mvarVertices.vertData.Length)
                                    {
                                        for (int i = 0; i < mvarNoOfPoints; i++)
                                        {
                                            mVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                            if (!DBNull.Value.Equals(MVal))
                                                mVal = -1.1E+38;
                                            else if (mVal.ToString() == "NaN")
                                                mVal = -1.1E+38;
                                            if (mvarReadmode != eReadMode.FastRead)
                                            {
                                                // If less than this then value is null
                                                if (mVal > -1E+38)
                                                    mvarVertices[i].Measure = mVal;
                                            }
                                            ArrayPos += 8;

                                            Part mPart = (Part)mvarParts[PartNo];
                                            if (mPart.Begins == i)
                                            {
                                                HoldXVal = mVal;
                                                HoldYVal = mVal;
                                            }
                                            else
                                            {
                                                HoldXVal = Math.Min(HoldXVal, mVal);
                                                HoldYVal = Math.Max(HoldXVal, mVal);
                                            }

                                            if (mPart.Ends == i)
                                            {
                                                if (HoldXVal > -1E+38)
                                                    mPart.MeasureMin = HoldXVal;
                                                if (HoldYVal > -1E+38)
                                                    mPart.MeasureMax = HoldYVal;
                                                PartNo++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (mvarTestForHole == true)
                        {
                            // **************************************************************
                            // * For multi Part shapes a whole exists when a line in the    *
                            // * part shape crosses a even number of times in one direction *
                            // **************************************************************
                            if (mvarNoOfParts > 1)
                            {

                                mvarShapeArea = 0;
                                for (int i = 0; i < mvarNoOfParts; i++)
                                {
                                    mvarParts[i].IsHole = false;
                                    for (int j = 0; j < mvarNoOfParts; j++)
                                    {
                                        if (i != j)
                                        {
                                            if (PointInPolygon(mvarVertices[mvarParts[i].Begins].X_Cord, mvarVertices[mvarParts[i].Begins].Y_Cord, mvarParts[j].Begins, mvarParts[j].Ends))
                                            {
                                                mvarParts[i].IsHole = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (mvarParts[i].IsHole)
                                        mvarShapeArea -= Math.Abs(Convert.ToDouble(mvarParts[i].Area));
                                    else
                                        mvarShapeArea += Math.Abs(Convert.ToDouble(mvarParts[i].Area));
                                }
                            }
                        }
                        break;
                    }
                #endregion
            }

            Globals.mvarVerticeChange = false;

        } // Reads the Shape Record data and populates the Vertices collection

        private void LoadShapeRecord(int RecordNumber)
        {

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);
            mvarVertices.Populate(fsShapeFile, fsShapeIndex, RecordNumber);
            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);

        }

        private void LoadDBFRecord(int RecordNumber, FileStream fsDataFile, ref Fields DataFields)
        {
            // ******************************************************************
            // * Read in the DBF record and populate the ShapeFields Collection *
            // ******************************************************************

            int FilePos = 0;
            short Exponent = 0;
            string DataValue = null;

            // Read in entire record
            long FileLoc = DataFields.HeaderLength + (DataFields.Recordlength * (RecordNumber - 1));

            byte[] lvarDBFData = new byte[DataFields.Recordlength];
            fsDataFile.Seek(FileLoc, SeekOrigin.Begin);
            fsDataFile.Read(lvarDBFData, 0, lvarDBFData.Length);


            // Reset the default field value to NULL
            DataFields.Strip();

            if (lvarDBFData[0] == 0x2a)
            {
                DataFields.isDeleted = true;
            }
            else
            {
                DataFields.isDeleted = false;
            }

            FilePos = 1;
            for (int i = 0; i < DataFields.Count; i++)
            {
                Field thisField = (Field)DataFields[i];
                byte[] bDataValue = new byte[thisField.Size + 1];
                Buffer.BlockCopy(lvarDBFData, FilePos, bDataValue, 0, thisField.Size);
                DataValue = ByteArrayToString(bDataValue);

                if (DataValue.Trim() != null)
                {
                    switch (thisField.Type)
                    {
                        case eFieldType.shpText:
                            thisField.Value = DataValue.TrimEnd();
                            break;
                        case eFieldType.shpNumeric:
                            if (mvarsysDelimiter != ".")
                            { DataValue = DataValue.Replace(mvardbfDelimiter, mvarsysDelimiter); }
                            thisField.Value = Convert.ToDouble(DataValue);
                            break;
                        case eFieldType.shpFloat:
                            // Floating values are represented by values and expontent e.g. 1.23425e+004
                            //Check the delimiter
                            if (mvarsysDelimiter != ".")
                            { DataValue = DataValue.Replace(mvardbfDelimiter, mvarsysDelimiter); }
                            if (DataValue.ToUpper().Contains("E"))
                            {
                                Exponent = Convert.ToInt16(DataValue.Substring(DataValue.ToUpper().IndexOf("E") + 1));
                                DataValue = DataValue.Substring(0, DataValue.ToUpper().IndexOf("E"));
                                thisField.Value = Convert.ToDouble(DataValue) * (Math.Pow(10, Exponent));
                            }
                            else
                            {
                                thisField.Value = Convert.ToDouble(DataValue);
                            }
                            break;
                        case eFieldType.shpBoolean:
                            if (DataValue.ToUpper() == "Y" | DataValue.ToUpper() == "T")
                            { thisField.Value = true; }
                            if (DataValue.ToUpper() == "N" | DataValue.ToUpper() == "F")
                            { thisField.Value = false; }
                            break;
                        case eFieldType.shpDate:
                            if (Convert.ToInt32(DataValue) > 0)
                            {
                                // Read in YYYYMMDD and convert to generic date for CDate
                                if (mvarYYYYMMDD) //Format "ddmmyyyy" )
                                { thisField.Value = System.DateTime.ParseExact(DataValue, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); }
                                else // Format "yyyymmdd"
                                { thisField.Value = System.DateTime.ParseExact(DataValue, "ddMMyyyy", System.Globalization.CultureInfo.CurrentCulture); }
                            }
                            break;
                        default:    // String
                            thisField.Value = DataValue;
                            break;
                    }
                }
                FilePos += thisField.Size;
            }
        } // Read in the DBF record and populate the Fields Collection

        private void PopulateVerticeHeader(ref Vertices verts)
        {
            // Loads the values read from RecordShapeHeader into the Vertices
            verts.xMin = mvarShapeXMin;
            verts.xMax = mvarShapeXMax;
            verts.yMin = mvarShapeYMin;
            verts.yMax = mvarShapeYMax;
            verts.zMin = mvarShapeZMin;
            verts.zMax = mvarShapeZMax;
            verts.mMin = mvarShapeMMin;
            verts.mMax = mvarShapeMMax;
            verts.NoOfPoints = mvarNoOfPoints;
        }

        #endregion

        #region **********          Data Write Methods            **********

        private void WriteShapeHeader(FileStream fs)
        {
            // ***************************************************
            // * Writes the first 100 bytes of a header file     *
            // * File is opened & closed outside of this routine *
            // ***************************************************

            byte[] ByteArray = new byte[4];
            byte[] outArray = new byte[100];
            //creating a new StreamWriter and passing the filestream object fs as argument

            ByteArray = BitConverter.GetBytes(9994);
            // File Code
            Array.Reverse(ByteArray);
            // Convert to big-endian order
            Buffer.BlockCopy(ByteArray, 0, outArray, 0, 4);

            ByteArray = BitConverter.GetBytes(1000);
            // Version
            Buffer.BlockCopy(ByteArray, 0, outArray, 28, 4);

            ByteArray = BitConverter.GetBytes(mvarShapeType);
            // Shape Type
            Buffer.BlockCopy(ByteArray, 0, outArray, 32, 4);

            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(outArray, 0, 100);
        }

        private void WriteShapeRecordHeader(int RecordNumber)
        {
            // ****************************************************
            // * Write the Shape Record Header - first 8 bytes    *
            // * Consists of the Record Number and Content Length *
            // ****************************************************

            int ContentLength = 0;
            int FilePos = 0;

            int iTempVal = 0;
            byte[] ByteArray = new byte[4];

            if (!mvarLockFile)
            {
                try
                {
                    fsShapeFile = File.Open(mvarShapeFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fsShapeIndex = File.Open(mvarShapeIndex, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                catch
                { throw new Exception("The ShapeFile " + mvarShapeFile + " has been locked by another application"); }
            }

            if (mvarShapeCount == 1)
            {
                FilePos = 100;
            }
            else
            {
                // Locate start position in index file
                FilePos = 100 + ((RecordNumber - 1) * 8) - 8;
                fsShapeIndex.Seek(FilePos, SeekOrigin.Begin);

                // Read previous record Startpos
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                iTempVal = BitConverter.ToInt32(ByteArray, 0);
                // Read previous record ContentLength
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                FilePos = BitConverter.ToInt32(ByteArray, 0);
                FilePos = (FilePos + iTempVal + 4) * 2;
            }

            // Move to start of ShapeFile record (may be at EOF)
            fsShapeFile.Seek(FilePos, SeekOrigin.Begin);

            // Write the record number (Big Endian)
            ByteArray = BitConverter.GetBytes(RecordNumber);
            Array.Reverse(ByteArray);
            fsShapeFile.Write(ByteArray, 0, 4);
            // Write the Content Length (Big Endian)
            ContentLength = CalcContentLength();
            ByteArray = BitConverter.GetBytes(ContentLength);
            Array.Reverse(ByteArray);
            fsShapeFile.Write(ByteArray, 0, 4);

            if (!mvarLockFile)
            {
                fsShapeFile.Close();
                fsShapeIndex.Close();
                fsShapeFile.Dispose();
                fsShapeIndex.Dispose();
            }

        }

        private void WriteShapeRecord(int RecordNumber)
        {
            // **********************************************************************
            // * Write the contents of the Vertices Collection out to the .SHP File *
            // * and update the .SHX index                                          *
            // **********************************************************************

            int i = 0;
            int FilePos = 0;
            int ContentLength = 0;
            int ArrayPos = 0;

            byte[] ByteArray = new byte[4];
            byte[] DblBytes = new byte[8];
            long FileLength = 0;

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);

            if (RecordNumber == 1)
            {
                FilePos = 46;
                fsShapeIndex.Seek(100, SeekOrigin.Begin);
            }
            else
            {
                // Read existing start position in Index File
                FilePos = 100 + ((RecordNumber - 1) * 8) - 8;
                fsShapeIndex.Seek(FilePos, SeekOrigin.Begin);
                // Start position of previous record
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                FilePos = BitConverter.ToInt32(ByteArray, 0);
                // Read Content Length of previous record
                fsShapeIndex.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                // Combine to find Start position of current record
                FilePos += BitConverter.ToInt32(ByteArray, 0);
                // Start position of Shape File Write in DWORDs
            }

            // **********************************************
            // * Index Record                               *
            // **********************************************
            // File Position

            ByteArray = BitConverter.GetBytes(FilePos + 4);
            Array.Reverse(ByteArray);
            fsShapeIndex.Write(ByteArray, 0, 4);

            // Content Length
            ContentLength = CalcContentLength();
            // Build an array with all elements
            byte[] lvarDataArray = new byte[(ContentLength * 2) + 8];

            ByteArray = BitConverter.GetBytes(ContentLength);
            Array.Reverse(ByteArray);
            // Write new index record
            fsShapeIndex.Write(ByteArray, 0, 4);


            // ***********************************************
            // *  ShapeFile Record Header - first 8 bytes    *
            // ***********************************************

            // put contentlength into data hold array
            Buffer.BlockCopy(ByteArray, 0, lvarDataArray, 4, 4);

            // store the record number
            ByteArray = BitConverter.GetBytes(RecordNumber);
            Array.Reverse(ByteArray);
            Buffer.BlockCopy(ByteArray, 0, lvarDataArray, 0, 4);

            // ***********************************************
            // *  ShapeFile Record                           *
            // ***********************************************

            // Shapetype
            ByteArray = BitConverter.GetBytes(mvarShapeType);
            Buffer.BlockCopy(ByteArray, 0, lvarDataArray, 8, 4);

            ArrayPos = 12;
            switch ((eShapeType)mvarShapeType)
            {

                case eShapeType.shpPoint:
                    // *********************************************
                    // * Point                                     *
                    // * X & Y Cords only                          *
                    // *********************************************

                    DblBytes = BitConverter.GetBytes(mvarVertices[0].X_Cord);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices[0].Y_Cord);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);

                    break;
                case eShapeType.shpPointM:
                    // *********************************************
                    // * PointM                                    *
                    // * X, Y Cords and Measure                    *
                    // *********************************************

                    DblBytes = BitConverter.GetBytes(mvarVertices[0].X_Cord);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices[0].Y_Cord);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    if (mvarVertices[0].Measure != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices[0].Measure));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);

                    break;
                case eShapeType.shpPointZ:
                    // *********************************************
                    // * PointZ                                    *
                    // * X, Y, Z Cords and Measure                 *
                    // *********************************************

                    DblBytes = BitConverter.GetBytes(mvarVertices[0].X_Cord);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices[0].Y_Cord);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices[0].Z_Cord));
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    if (mvarVertices[0].Measure != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices[0].Measure));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);


                    break;
                case eShapeType.shpMultiPoint:
                    // *********************************************
                    // * MultiPoint                                *
                    // * MBR, X, Y Cords                           *
                    // *********************************************


                    DblBytes = BitConverter.GetBytes(mvarVertices.xMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);
                    ByteArray = BitConverter.GetBytes(mvarVertices.Count);
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 32, 4);
                    ArrayPos = ArrayPos + 36;

                    // Multipoint X & Y s
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(mVert.X_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        DblBytes = BitConverter.GetBytes(mVert.Y_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                        ArrayPos = ArrayPos + 16;
                    }



                    break;
                case eShapeType.shpMultiPointM:
                    // *********************************************
                    // * MultiPointM                               *
                    // * MBR, X, Y Cords and Measure               *
                    // *********************************************

                    // MultipointM MBR
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);
                    ByteArray = BitConverter.GetBytes(mvarVertices.Count);
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 32, 4);

                    // MultipointM X & Y s
                    ArrayPos = 48;
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(mVert.X_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        DblBytes = BitConverter.GetBytes(mVert.Y_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                        ArrayPos = ArrayPos + 16;
                    }


                    // MultipointM Min & Max of Measures
                    if (mvarVertices.mMin != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMin));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    if (mvarVertices.mMax != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMax));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    ArrayPos = ArrayPos + 16;

                    // Array of Measures
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        if (mVert.Measure != null)
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mVert.Measure));
                        }
                        else
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                        }
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos = ArrayPos + 8;
                    }



                    break;
                case eShapeType.shpMultiPointZ:
                    // *********************************************
                    // * MultiPointZ                               *
                    // * MBR, X, Y, Z Cords and Measure            *
                    // *********************************************

                    // MultipointZ MBR
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);
                    ByteArray = BitConverter.GetBytes(mvarVertices.Count);
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 32, 4);

                    // MultipointZ X & Y s
                    ArrayPos = 48;
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(mVert.X_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        DblBytes = BitConverter.GetBytes(mVert.Y_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                        ArrayPos = ArrayPos + 16;
                    }


                    // MultipointZ Z Min & Max
                    DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.zMin));
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.zMax));
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    ArrayPos = ArrayPos + 16;

                    // MultipointZ Z values
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mVert.Z_Cord));
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos = ArrayPos + 8;
                    }


                    // Multipoint Min & Max of Measures
                    if (mvarVertices.mMin != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMin));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    if (mvarVertices.mMax != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMax));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    ArrayPos = ArrayPos + 16;

                    // Array of Measures
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        if (mVert.Measure != null)
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mVert.Measure));
                        }
                        else
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                        }
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos = ArrayPos + 8;
                    }


                    break;
                case eShapeType.shpPolygon:
                case eShapeType.shpPolyLine:
                    // *********************************************
                    // * Polygon and PolyLine                      *
                    // * MBR, Parts, X, Y Cords                    *
                    // *********************************************


                    // Polygon / Polyline MBR
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);
                    ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts.Count));
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 32, 4);
                    ByteArray = BitConverter.GetBytes(mvarVertices.Count);
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 36, 4);
                    ArrayPos += 40;

                    // Polygon / Polyline Parts start
                    for (i = 0; i < mvarParts.Count; i++)
                    {
                        ByteArray = BitConverter.GetBytes(mvarParts[i].Begins);
                        Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos, 4);
                        ArrayPos += 4;
                    }

                    // Polygon / Polyline X & Y s
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(mVert.X_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        DblBytes = BitConverter.GetBytes(mVert.Y_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                        ArrayPos += 16;
                    }



                    break;
                case eShapeType.shpPolygonM:
                case eShapeType.shpPolyLineM:
                    // *********************************************
                    // * PolygonM and PolyLineM                    *
                    // * MBR, Parts, X, Y Cords and Measures       *
                    // *********************************************

                    // PolygonM / PolylineM MBR
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);
                    ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts.Count));
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 32, 4);
                    ByteArray = BitConverter.GetBytes(mvarVertices.Count);
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 36, 4);
                    ArrayPos += 40;

                    // PolygonM / PolylineM Parts start
                    for (i = 0; i < mvarParts.Count; i++)
                    {
                        ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts[i].Begins));
                        Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos, 4);
                        ArrayPos += 4;
                    }
                    if (mvarParts.Count == 0) ArrayPos += 4;


                    // PolygonM / PolylineM X & Y s
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(mVert.X_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        DblBytes = BitConverter.GetBytes(mVert.Y_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                        ArrayPos += 16;
                    }


                    // PolygonM / PolylineM Min & Max of Measures
                    if (mvarVertices.mMin != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMin));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    if (mvarVertices.mMax != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMax));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    ArrayPos += 16;

                    // PolygonM / PolylineM Array of Measures
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        if (mVert.Measure != null)
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mVert.Measure));
                        }
                        else
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                        }
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos += 8;
                    }



                    break;
                case eShapeType.shpPolygonZ:
                case eShapeType.shpPolyLineZ:
                    // *********************************************
                    // * PolygonZ and PolyLineZ                    *
                    // * MBR, Parts, X, Y, Z Cords and Measures    *
                    // *********************************************

                    // PolygonZ / PolylineZ MBR
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);
                    ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts.Count));
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 32, 4);
                    ByteArray = BitConverter.GetBytes(mvarVertices.Count);
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 36, 4);
                    ArrayPos += 40;

                    // PolygonZ / PolylineZ Parts start
                    for (i = 0; i < mvarParts.Count; i++)
                    {
                        ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts[i].Begins));
                        Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos, 4);
                        ArrayPos += 4;
                    }
                    if (mvarParts.Count == 0) ArrayPos += 4;


                    // PolygonZ / PolylineZ X & Y s
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(mVert.X_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        DblBytes = BitConverter.GetBytes(mVert.Y_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                        ArrayPos += 16;
                    }


                    // PolygonZ / PolylineZ Z Min & Max
                    DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.zMin));
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.zMax));
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    ArrayPos += 16;

                    // PolygonZ / PolylineZ Z values
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        double ZVal = Convert.ToDouble(mvarVertices[i].Z_Cord);
                        DblBytes = BitConverter.GetBytes(ZVal);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos += 8;
                    }


                    // PolygonZ / PolylineZ Min & Max of Measures
                    if (mvarVertices.mMin != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMin));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    if (mvarVertices.mMax != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMax));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    ArrayPos += 16;

                    // PolygonZ / PolylineZ Array of Measures
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        if (mVert.Measure != null)
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mVert.Measure));
                        }
                        else
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                        }
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos += 8;
                    }



                    break;
                case eShapeType.shpMultiPatch:
                    // *************************************************
                    // * MultiPatch                                    *
                    // * MBR, Parts, Types, X, Y, Z Cords and Measures *
                    // *************************************************

                    // Multipatch MBR
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMin);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.xMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    DblBytes = BitConverter.GetBytes(mvarVertices.yMax);
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 24, 8);
                    ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts.Count));
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 32, 4);
                    ByteArray = BitConverter.GetBytes(mvarVertices.Count);
                    Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos + 36, 4);
                    ArrayPos += 40;

                    // Multipatch Parts start
                    for (i = 0; i < mvarParts.Count; i++)
                    {
                        ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts[i].Begins));
                        Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos, 4);
                        ArrayPos += 4;
                    }
                    if (mvarParts.Count == 0) ArrayPos += 4;


                    // Multipatch Parttypes
                    for (i = 0; i < mvarParts.Count; i++)
                    {
                        ByteArray = BitConverter.GetBytes(Convert.ToInt32(mvarParts[i].PartType));
                        Buffer.BlockCopy(ByteArray, 0, lvarDataArray, ArrayPos, 4);
                        ArrayPos += 4;
                    }


                    // Multipatch X & Y s
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        DblBytes = BitConverter.GetBytes(mVert.X_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        DblBytes = BitConverter.GetBytes(mVert.Y_Cord);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                        ArrayPos += 16;
                    }


                    // MultiPatch Z Min & Max
                    DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.zMin));
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.zMax));
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 8, 8);
                    ArrayPos += 16;

                    // MultiPatch Z values
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Double ZVal = Convert.ToDouble(mvarVertices[i].Z_Cord);
                        DblBytes = BitConverter.GetBytes(ZVal);
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos += 8;
                    }


                    // MultiPatch Z Min & Max of Measures
                    if (mvarVertices.mMin != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMin));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                    if (mvarVertices.mMax != null)
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarVertices.mMax));
                    }
                    else
                    {
                        DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                    }
                    Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos + 16, 8);
                    ArrayPos += 16;

                    // MultiPatch Array of Measures
                    for (i = 0; i < mvarVertices.Count; i++)
                    {
                        Vertice mVert = mvarVertices[i];
                        if (mVert.Measure != null)
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(mVert.Measure));
                        }
                        else
                        {
                            DblBytes = BitConverter.GetBytes(Convert.ToDouble(-1.1E+38));
                        }
                        Buffer.BlockCopy(DblBytes, 0, lvarDataArray, ArrayPos, 8);
                        ArrayPos += 8;
                    }



                    break;
            }

            // *******************************************
            // * Update the MBR of the entire shape file *
            // *******************************************
            if (mvarShapeCount == 1)
            {
                // One record so use current record values
                mvarShapeFileXMin = mvarVertices.xMin;
                mvarShapeFileXMax = mvarVertices.xMax;
                mvarShapeFileYMin = mvarVertices.yMin;
                mvarShapeFileYMax = mvarVertices.yMax;
                mvarShapeFileZMin = Convert.ToDouble(mvarVertices.zMin);
                mvarShapeFileZMax = Convert.ToDouble(mvarVertices.zMax);
                if (mvarVertices.mMin != null)
                {
                    if (mvarVertices.mMin < -1E+38)
                    {
                        mvarShapeFileMMin = null;
                    }
                    else
                    {
                        mvarShapeFileMMin = mvarVertices.mMin;
                    }
                }
                if (mvarVertices.mMax != null)
                {
                    if (mvarVertices.mMax < -1E+38)
                    {
                        mvarShapeFileMMax = null;
                    }
                    else
                    {
                        mvarShapeFileMMax = mvarVertices.mMax;
                    }
                }
            }
            else
            {
                // Find Min/Max values
                if ((eShapeType)mvarShapeType != eShapeType.shpNull)
                {
                    mvarShapeFileXMin = Math.Min(mvarVertices.xMin, mvarShapeFileXMin);
                    mvarShapeFileXMax = Math.Max(mvarVertices.xMax, mvarShapeFileXMax);
                    mvarShapeFileYMin = Math.Min(mvarVertices.yMin, mvarShapeFileYMin);
                    mvarShapeFileYMax = Math.Max(mvarVertices.yMax, mvarShapeFileYMax);
                    mvarShapeFileZMin = Math.Min(Convert.ToDouble(mvarVertices.zMin), mvarShapeFileZMin);
                    mvarShapeFileZMax = Math.Max(Convert.ToDouble(mvarVertices.zMax), mvarShapeFileZMax);
                    if (mvarVertices.mMin != null)
                    {
                        if (mvarVertices.mMin > -1E+38)
                        {
                            mvarShapeFileMMin = Math.Max(Convert.ToDouble(mvarVertices.mMin), Convert.ToDouble(mvarVertices.mMin));
                        }
                    }
                    if (mvarVertices.mMax != null)
                    {
                        if (mvarVertices.mMax > -1E+38)
                        {
                            mvarShapeFileMMin = Math.Max(Convert.ToDouble(mvarVertices.mMax), Convert.ToDouble(mvarVertices.mMax));
                        }
                    }
                }
            }

            // ********************************
            // * Output data block to file    *
            // ********************************
            fsShapeFile.Seek((FilePos + 4) * 2, SeekOrigin.Begin);
            fsShapeFile.Write(lvarDataArray, 0, lvarDataArray.Length);

            // ********************************
            // * Update the Shape file header *
            // ********************************

            // File Length of ShapeFile in WORDS (2 bytes)
            FileLength = fsShapeFile.Length;
            ByteArray = BitConverter.GetBytes(Convert.ToInt32(FileLength / 2));
            Array.Reverse(ByteArray);
            fsShapeFile.Seek(24, SeekOrigin.Begin);
            fsShapeFile.Write(ByteArray, 0, 4);

            // File Length of fsShapeIndex in WORDS (2 bytes)
            FileLength = fsShapeIndex.Length;
            ByteArray = BitConverter.GetBytes(Convert.ToInt32(FileLength / 2));
            Array.Reverse(ByteArray);
            fsShapeIndex.Seek(24, SeekOrigin.Begin);
            fsShapeIndex.Write(ByteArray, 0, 4);

            // Update the MBRs of the headers
            fsShapeFile.Seek(36, SeekOrigin.Begin);
            fsShapeIndex.Seek(36, SeekOrigin.Begin);

            DblBytes = BitConverter.GetBytes(mvarShapeFileXMin);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileYMin);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileXMax);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileYMax);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileZMin);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            DblBytes = BitConverter.GetBytes(mvarShapeFileZMax);
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            if (mvarShapeFileMMin == null)
            {
                DblBytes = BitConverter.GetBytes(Convert.ToDouble(0));
            }
            else
            {
                DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileMMin));
            }
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            if (mvarShapeFileMMax == null)
            {
                DblBytes = BitConverter.GetBytes(Convert.ToDouble(0));
            }
            else
            {
                DblBytes = BitConverter.GetBytes(Convert.ToDouble(mvarShapeFileMMax));
            }
            fsShapeFile.Write(DblBytes, 0, 8);
            fsShapeIndex.Write(DblBytes, 0, 8);

            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);

            // Reset the change indicator
            Globals.mvarVerticeChange = false;


        }

        private void WriteDBFRecord(int RecordNumber)
        {
            // ****************************************************************
            // * Write the contents of the Shape Fields Collection to the DBF *
            // ****************************************************************

            int FilePos = 0;
            byte[] LongBytes = new byte[4];
            byte[] SpaceByte = new byte[1];
            int i = 0;
            string DataValue = null;
            int DecimalLength = 0;
            string Exponent = null;

            // Create an output byte array of spaces
            byte[] outBytes = StrToByteArray(new string(' ', mvarFields.Recordlength));
            FilePos = 1;

            OpenStream(mvarShapeDBF, ref fsDataFile);

            mvardbfDelimiter = ".";
            for (i = 0; i < mvarFields.Count; i++)
            {
                // Write out the field as a string
                Field mField = mvarFields[i];

                // Null data values are represented in the file as spaces

                if ((mField.Value == null) | Convert.IsDBNull(mField.Value))
                {
                    DataValue = new string(' ', mField.Size);
                    if ((eFieldType)mField.Type == eFieldType.shpBoolean)
                    { DataValue = "?"; }
                }
                else
                {
                    switch ((eFieldType)mField.Type)
                    {

                        case eFieldType.shpText:
                            // Text - pad the right with spaces upto the field size
                            DataValue = mField.Value.ToString().PadRight(mField.Size);

                            break;
                        case eFieldType.shpNumeric:
                        case eFieldType.shpInteger:
                        case eFieldType.shpLong:
                        case eFieldType.shpSingle:
                        case eFieldType.shpDouble:
                            if (mField.Decimal > 0)
                            {
                                // The rules for DBF numeric fields are that decimals are optional as
                                // long as the field size is not exceeded (e.g. 4,2 may have the values 1111, 11.1, 1.11)
                                // all are 4 characters long with a maximum of 2 significant decimal places)

                                DataValue = mField.Value.ToString().ToUpper();

                                // **************************************************************
                                // * Check this assumption that the decimal separator is stored *
                                // * as locale specific mvardbfdelimiter                        *
                                // **************************************************************
                                if (mvardbfDelimiter != mvarsysDelimiter)
                                {
                                    DataValue = DataValue.Replace(mvarsysDelimiter, mvardbfDelimiter);
                                }

                                // Find the number of decimal points in the data value
                                if (DataValue.Contains(mvardbfDelimiter))
                                {
                                    DecimalLength = mField.Decimal;
                                }
                                else
                                {
                                    DecimalLength = DataValue.Length - (DataValue.IndexOf(mvardbfDelimiter));
                                }
                                DecimalLength = mField.Decimal - DecimalLength;

                                // trim off any extra characters
                                if (DecimalLength < 0)
                                {
                                    DataValue = DataValue.Substring(0, DataValue.Length + DecimalLength);
                                }

                                // Store all decimals as "."
                                if (mvarsysDelimiter != ".")
                                {
                                    DataValue = DataValue.Replace(mvarsysDelimiter, ".");
                                }

                                // Check for exponential component
                                if (DataValue.Contains("E"))
                                {
                                    //DataValue = "5.67e5"
                                    Exponent = DataValue.Substring(DataValue.IndexOf("E") + 1);
                                    DataValue = DataValue.Substring(0, DataValue.IndexOf("E"));
                                    // Strip out the existing decimal
                                    DataValue = DataValue.Replace(mvardbfDelimiter, "");
                                    i = Convert.ToInt32(Exponent);
                                    if (i < 0)
                                    {
                                        // move the decimal to the left
                                        DataValue = "0" + mvardbfDelimiter + new string('0', (System.Math.Abs(i) - 1)) + DataValue;
                                    }
                                    else
                                    {
                                        // move the decimal to the right
                                        // this assumes that the mantica does not exceed the
                                        // number of the exponent -otherwise truncation occurs
                                        DataValue = DataValue.PadRight(i, '0');
                                    }
                                }
                            }
                            else
                            {
                                if (mField.Size < 10)
                                {
                                    DataValue = Convert.ToInt32(mField.Value).ToString();
                                }
                                else
                                {
                                    DataValue = Convert.ToInt64(mField.Value).ToString();
                                }

                            }
                            // Check that the data will fit the field definition
                            if (DataValue.Length > mField.Size)
                            {
                                //Raise an Error
                                throw new Exception("Data in Field " + mField.Name + " is larger than defined in the field definition");
                            }
                            DataValue = DataValue.PadLeft(mField.Size);

                            break;
                        case eFieldType.shpFloat:
                            DataValue = mField.Value.ToString().ToUpper();

                            // Store all decimals as "."
                            if (mvarsysDelimiter != ".")
                            {
                                DataValue = DataValue.Replace(mvarsysDelimiter, ".");
                            }

                            // Find the number of decimal points in the data value
                            if (DataValue.Contains(mvardbfDelimiter))
                            {
                                DecimalLength = DataValue.IndexOf(mvardbfDelimiter) - 1;
                            }
                            else
                            {
                                DecimalLength = DataValue.Length - 1;
                            }

                            if (DecimalLength != 0)
                            {
                                // convert to 1.xxxxx
                                DataValue = DataValue.Replace(mvardbfDelimiter, "");
                                if (DataValue.StartsWith("-"))
                                {
                                    DataValue = DataValue.Substring(0, 2) + mvardbfDelimiter + DataValue.Substring(2);
                                }
                                else
                                {
                                    DataValue = DataValue.Substring(0, 1) + mvardbfDelimiter + DataValue.Substring(1);
                                }
                            }

                            // Check for exponential component
                            if (DataValue.Contains("E"))
                            {
                                //DataValue = "5.67e5"
                                Exponent = DataValue.Substring(DataValue.IndexOf("E") + 1);
                                DataValue = DataValue.Substring(0, DataValue.IndexOf("E"));
                                DecimalLength = Convert.ToInt32(Exponent) + (DecimalLength);
                            }
                            if (DataValue.Length < mField.Size - 5)
                            {
                                DataValue = " " + DataValue.PadRight(mField.Size - 6, '0') + "E";
                            }
                            else
                            {
                                DataValue = " " + DataValue.Substring(0, mField.Size - 6) + "E";
                            }
                            if (Math.Sign(DecimalLength) == 1)
                                DataValue = DataValue + "+";
                            else
                                DataValue = DataValue + "-";
                            DataValue = DataValue + (System.Math.Abs(DecimalLength)).ToString("000");
                            DataValue = DataValue.PadLeft(mField.Size, ' ');

                            break;
                        case eFieldType.shpDate:
                            // **************************************************************
                            // * Check this assumption that the date order is stored        *
                            // * as locale specific mvardbfdelimiter                        *
                            // **************************************************************
                            DateTime mDate = (DateTime)mField.Value;
                            if (!mvarYYYYMMDD)
                            {
                                DataValue = mDate.ToString("ddMMyyyy");
                            }
                            else
                            {
                                DataValue = mDate.ToString("yyyyMMdd");
                            }

                            break;
                        case eFieldType.shpBoolean:
                            if (mField.Value.ToString() == "True")
                            {
                                DataValue = "T";
                            }
                            else if (mField.Value.ToString() == "False")
                            {
                                DataValue = "F";
                            }
                            else
                            {
                                DataValue = "?";
                            }

                            break;
                        default:
                            DataValue = mField.Value.ToString().PadRight(mField.Size, ' ');

                            break;
                    }

                }

                // *******************************************************
                // * Convert output string into necessary codepage bytes *
                // * .NET implementation - COM is forced to Unicode      *
                // *******************************************************
                byte[] tBytes = StrToByteArray(DataValue);
                Buffer.BlockCopy(tBytes, 0, outBytes, FilePos, Math.Min(tBytes.Length, mField.Size));
                FilePos += mField.Size;

                // reset the data value just in case
                DataValue = null;

                // Reset the status monitor
                mField.Status = null;

            }

            // *************************************
            // *   Date of update and RecordCount  *
            // *************************************
            fsDataFile.Seek(0, SeekOrigin.Begin);
            LongBytes[0] = 3;
            LongBytes[1] = Convert.ToByte(DateTime.Now.Year - 1900);
            LongBytes[2] = Convert.ToByte(DateTime.Now.Month);
            LongBytes[3] = Convert.ToByte(DateTime.Now.Day);
            fsDataFile.Write(LongBytes, 0, 4);

            // Update the record number
            fsDataFile.Seek(4, SeekOrigin.Begin);
            fsDataFile.Read(LongBytes, 0, 4);
            // Use Filepos to hold the current DBF record count
            FilePos = BitConverter.ToInt32(LongBytes, 0);
            if (FilePos < RecordNumber)
            {
                // Write the updated record count
                fsDataFile.Seek(4, SeekOrigin.Begin);
                LongBytes = BitConverter.GetBytes(RecordNumber);
                fsDataFile.Write(LongBytes, 0, 4);
            }

            // Workout position of record to be written
            long FileLoc = mvarFields.HeaderLength + (mvarFields.Recordlength * (RecordNumber - 1));
            fsDataFile.Seek(FileLoc, SeekOrigin.Begin);
            fsDataFile.Write(outBytes, 0, mvarFields.Recordlength);



            // *****************************************
            // * Finish the record with the terminator *
            // *****************************************
            if (RecordNumber == mvarShapeCount)
            {
                SpaceByte[0] = 26;
                fsDataFile.Write(SpaceByte, 0, 1);
            }

            CloseStream(ref fsDataFile);

        }

        private void WriteDBFRecord(int RecordNumber, Fields ShapeFields, FileStream fsDataStream)
        {
            // ****************************************************************
            // * Write the contents of the Shape Fields Collection to the DBF *
            // ****************************************************************

            int FilePos = 0;
            byte[] LongBytes = new byte[4];
            byte[] SpaceByte = new byte[1];
            int i = 0;
            string DataValue = null;
            int DecimalLength = 0;
            string Exponent = null;

            // Create an output byte array of spaces
            byte[] outBytes = StrToByteArray(new string(' ', ShapeFields.Recordlength));
            FilePos = 1;


            mvardbfDelimiter = ".";
            for (i = 0; i < ShapeFields.Count; i++)
            {
                // Write out the field as a string
                Field mField = ShapeFields[i];

                // Null data values are represented in the file as spaces

                if (mField.Value == null)
                {
                    DataValue = new string(' ', mField.Size);
                    if ((eFieldType)mField.Type == eFieldType.shpBoolean)
                    { DataValue = "?"; }
                }
                else
                {
                    switch ((eFieldType)mField.Type)
                    {

                        case eFieldType.shpText:
                            // Text - pad the right with spaces upto the field size
                            DataValue = mField.Value.ToString().PadRight(mField.Size);

                            break;
                        case eFieldType.shpNumeric:
                        case eFieldType.shpInteger:
                        case eFieldType.shpLong:
                        case eFieldType.shpSingle:
                        case eFieldType.shpDouble:
                            if (mField.Decimal > 0)
                            {
                                // The rules for DBF numeric fields are that decimals are optional as
                                // long as the field size is not exceeded (e.g. 4,2 may have the values 1111, 11.1, 1.11)
                                // all are 4 characters long with a maximum of 2 significant decimal places)

                                DataValue = mField.Value.ToString().ToUpper();

                                // **************************************************************
                                // * Check this assumption that the decimal separator is stored *
                                // * as locale specific mvardbfdelimiter                        *
                                // **************************************************************
                                if (mvardbfDelimiter != mvarsysDelimiter)
                                {
                                    DataValue = DataValue.Replace(mvarsysDelimiter, mvardbfDelimiter);
                                }

                                // Find the number of decimal points in the data value
                                if (DataValue.Contains(mvardbfDelimiter))
                                {
                                    DecimalLength = mField.Decimal;
                                }
                                else
                                {
                                    DecimalLength = DataValue.Length - (DataValue.IndexOf(mvardbfDelimiter));
                                }
                                DecimalLength = mField.Decimal - DecimalLength;

                                // trim off any extra characters
                                if (DecimalLength < 0)
                                {
                                    DataValue = DataValue.Substring(0, DataValue.Length + DecimalLength);
                                }

                                // Store all decimals as "."
                                if (mvarsysDelimiter != ".")
                                {
                                    DataValue = DataValue.Replace(mvarsysDelimiter, ".");
                                }

                                // Check for exponential component
                                if (DataValue.Contains("E"))
                                {
                                    //DataValue = "5.67e5"
                                    Exponent = DataValue.Substring(DataValue.IndexOf("E") + 1);
                                    DataValue = DataValue.Substring(0, DataValue.IndexOf("E"));
                                    // Strip out the existing decimal
                                    DataValue = DataValue.Replace(mvardbfDelimiter, "");
                                    i = Convert.ToInt32(Exponent);
                                    if (i < 0)
                                    {
                                        // move the decimal to the left
                                        DataValue = "0" + mvardbfDelimiter + new string('0', (System.Math.Abs(i) - 1)) + DataValue;
                                    }
                                    else
                                    {
                                        // move the decimal to the right
                                        // this assumes that the mantica does not exceed the
                                        // number of the exponent -otherwise truncation occurs
                                        DataValue = DataValue.PadRight(i, '0');
                                    }
                                }
                            }
                            else
                            {
                                if (mField.Size < 10)
                                {
                                    DataValue = Convert.ToInt32(mField.Value).ToString();
                                }
                                else
                                {
                                    DataValue = Convert.ToInt64(mField.Value).ToString();
                                }
                            }
                            // Check that the data will fit the field definition
                            if (DataValue.Length > mField.Size)
                            {
                                //Raise an Error
                                throw new Exception("Data in Field " + mField.Name + " is larger than defined in the field definition");
                            }
                            DataValue = DataValue.PadLeft(mField.Size);

                            break;
                        case eFieldType.shpFloat:
                            DataValue = mField.Value.ToString().ToUpper();

                            // Store all decimals as "."
                            if (mvarsysDelimiter != ".")
                            {
                                DataValue = DataValue.Replace(mvarsysDelimiter, ".");
                            }

                            // Find the number of decimal points in the data value
                            if (DataValue.Contains(mvardbfDelimiter))
                            {
                                DecimalLength = DataValue.IndexOf(mvardbfDelimiter) - 1;
                            }
                            else
                            {
                                DecimalLength = DataValue.Length - 1;
                            }

                            if (DecimalLength != 0)
                            {
                                // convert to 1.xxxxx
                                DataValue = DataValue.Replace(mvardbfDelimiter, ".");
                                if (DataValue.StartsWith("-"))
                                {
                                    DataValue = DataValue.Substring(0, 2) + mvardbfDelimiter + DataValue.Substring(2);
                                }
                                else
                                {
                                    DataValue = DataValue.Substring(0, 1) + mvardbfDelimiter + DataValue.Substring(1);
                                }
                            }

                            // Check for exponential component
                            if (DataValue.Contains("E"))
                            {
                                //DataValue = "5.67e5"
                                Exponent = DataValue.Substring(DataValue.IndexOf("E") + 1);
                                DataValue = DataValue.Substring(0, DataValue.IndexOf("E"));
                                DecimalLength = Convert.ToInt32(Exponent) + (DecimalLength);
                            }
                            if (DataValue.Length < mField.Size - 5)
                            {
                                DataValue = " " + DataValue.PadRight(mField.Size - 6, '0') + "E";
                            }
                            else
                            {
                                DataValue = " " + DataValue.Substring(0, mField.Size - 6) + "E";
                            }
                            if (Math.Sign(DecimalLength) == 1)
                                DataValue = DataValue + "+";
                            else
                                DataValue = DataValue + "-";
                            DataValue = DataValue + (System.Math.Abs(DecimalLength)).ToString("000");
                            DataValue = DataValue.PadLeft(mField.Size, ' ');

                            break;
                        case eFieldType.shpDate:
                            // **************************************************************
                            // * Check this assumption that the date order is stored        *
                            // * as locale specific mvardbfdelimiter                        *
                            // **************************************************************
                            DateTime mDate = (DateTime)mField.Value;
                            if (!mvarYYYYMMDD)
                            {
                                DataValue = mDate.ToString("ddMMyyyy");
                            }
                            else
                            {
                                DataValue = mDate.ToString("yyyyMMdd");
                            }

                            break;
                        case eFieldType.shpBoolean:
                            if (mField.Value.ToString() == "True")
                            {
                                DataValue = "T";
                            }
                            else if (mField.Value.ToString() == "False")
                            {
                                DataValue = "F";
                            }
                            else
                            {
                                DataValue = "?";
                            }

                            break;
                        default:
                            DataValue = mField.Value.ToString().PadRight(mField.Size, ' ');

                            break;
                    }

                }

                // *******************************************************
                // * Convert output string into necessary codepage bytes *
                // * .NET implementation - COM is forced to Unicode      *
                // *******************************************************
                byte[] tBytes = StrToByteArray(DataValue);
                Buffer.BlockCopy(tBytes, 0, outBytes, FilePos, Math.Min(tBytes.Length, mField.Size));
                FilePos += mField.Size;

                // reset the data value just in case
                DataValue = null;

                // Reset the status monitor
                mField.Status = null;

            }

            // *************************************
            // *   Date of update and RecordCount  *
            // *************************************
            fsDataStream.Seek(0, SeekOrigin.Begin);
            LongBytes[0] = 3;
            LongBytes[1] = Convert.ToByte(DateTime.Now.Year - 1900);
            LongBytes[2] = Convert.ToByte(DateTime.Now.Month);
            LongBytes[3] = Convert.ToByte(DateTime.Now.Day);
            fsDataStream.Write(LongBytes, 0, 4);

            // Update the record number
            fsDataStream.Seek(4, SeekOrigin.Begin);
            fsDataStream.Read(LongBytes, 0, 4);
            // Use Filepos to hold the current DBF record count
            FilePos = BitConverter.ToInt32(LongBytes, 0);
            if (FilePos < RecordNumber)
            {
                // Write the updated record count
                fsDataStream.Seek(4, SeekOrigin.Begin);
                LongBytes = BitConverter.GetBytes(RecordNumber);
                fsDataStream.Write(LongBytes, 0, 4);
            }

            // Workout position of record to be written
            long FileLoc = ShapeFields.HeaderLength + (ShapeFields.Recordlength * (RecordNumber - 1));
            fsDataStream.Seek(FileLoc, SeekOrigin.Begin);
            fsDataStream.Write(outBytes, 0, ShapeFields.Recordlength);



            // *****************************************
            // * Finish the record with the terminator *
            // *****************************************
            if (RecordNumber == mvarShapeCount)
            {
                SpaceByte[0] = 26;
                fsDataStream.Write(SpaceByte, 0, 1);
            }

        }

        #endregion

        #region **********          Find Functions                **********

        private long GetShapeRecordStart(int RecordNumber, FileStream IndexStream)
        {
            // Finds the start position of a record in the ShapeFile by reading the 
            // values in the Index File.  Note the first 8 bytes contain the record header
            byte[] ByteArray = new byte[4];
            long FilePos = 104;

            if (RecordNumber > 1)
            {
                //Read the location out out the index file
                int Offset = 100 + ((RecordNumber - 1) * 8);
                if (Offset < IndexStream.Length)
                {
                    IndexStream.Seek(Offset, SeekOrigin.Begin);
                    IndexStream.Read(ByteArray, 0, 4);
                    Array.Reverse(ByteArray);
                    FilePos = (BitConverter.ToInt32(ByteArray, 0) * 2) + 4;
                }
                else if (Offset == IndexStream.Length)
                {
                    // Go to end of the file
                    FilePos = 100 + ((RecordNumber - 2) * 8);
                    IndexStream.Seek(FilePos, SeekOrigin.Begin);
                    // Start position of previous record
                    IndexStream.Read(ByteArray, 0, 4);
                    Array.Reverse(ByteArray);
                    FilePos = BitConverter.ToInt32(ByteArray, 0);
                    // Read Content Length of previous record
                    IndexStream.Read(ByteArray, 0, 4);
                    Array.Reverse(ByteArray);
                    // Combine to find Start position of current record
                    FilePos += BitConverter.ToInt32(ByteArray, 0);
                    // Start position of Shape File Write in DWORDs
                    FilePos = (FilePos * 2) + 8;
                }
            }
            return FilePos;
        }

        private bool PointInPolygon(double PointX, double PointY, int vertStart, int vertEnd)
        {
            long Counter = 0;
            int i = 0;
            double XInters = 0;
            double polyX1 = mvarVertices[vertStart].X_Cord;
            double polyY1 = mvarVertices[vertStart].Y_Cord;
            double polyX2;
            double polyY2;

            for (i = vertStart + 1; i <= vertEnd; i += 1)
            {
                polyX2 = mvarVertices[i % vertEnd].X_Cord;
                polyY2 = mvarVertices[i % vertEnd].Y_Cord;

                if ((PointY > Math.Min(polyY1, polyY2)))
                {
                    if ((PointY <= Math.Max(polyY1, polyY2)))
                    {
                        if ((PointX <= Math.Max(polyX1, polyX2)))
                        {
                            if ((polyY1 != polyY2))
                            {
                                XInters = Convert.ToDouble(PointY - polyY1) * Convert.ToDouble(polyX2 - polyX1) / Convert.ToDouble(polyY2 - polyY1) + polyX1;
                                if (((polyX1 == polyX2) | (PointX <= Convert.ToInt32(XInters))))
                                {
                                    Counter++;
                                }
                            }
                        }
                    }
                }
                polyX1 = polyX2;
                polyY1 = polyY2;
            }

            if ((Counter % 2 == 0))
            { return false; }
            else
            { return true; }
        }

        private double PointToLineDist(double Px, double Py, double X1, double Y1, double X2, double Y2)
        {
            // Uses cross product to find the closest distance between a point and a line segment
            double A = Px - X1;
            double B = Py - Y1;
            double C = X2 - X1;
            double D = Y2 - Y1;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            double xx = 0;
            double yy = 0;

            if ((param < 0))
            {
                // Use the start vertice
                xx = X1;
                yy = Y1;
            }
            else if ((param > 1))
            {
                // Use the End Vertice
                xx = X2;
                yy = Y2;
            }
            else
            {
                // Nearest point is somewhere between the start and end vertices
                xx = X1 + param * C;
                yy = Y1 + param * D;
            }

            double dist = Math.Sqrt(Math.Pow(Px - xx, 2) + Math.Pow(Py - yy, 2));
            return dist;

        }

        private bool PointInPolygonData(double PointX, double PointY, byte[] vertData)
        {
            int lvarNoOfParts = BitConverter.ToInt32(vertData, 36);
            int lvarNoOfPoints = BitConverter.ToInt32(vertData, 40);
            long Counter = 0;
            double XInters = 0;
            double polyX1;
            double polyY1;
            double polyX2;
            double polyY2;

            int[] partStart = new int[lvarNoOfParts + 1];
            int ArrayPos = 44;

            for (int i = 1; i < partStart.Length; i++)
            {
                int vertBegin = BitConverter.ToInt32(vertData, ArrayPos);
                partStart[i - 1] = vertBegin;
                ArrayPos += 4;
            }
            partStart[partStart.Length - 1] = lvarNoOfPoints;


            for (int i = 1; i < partStart.Length; i++)
            {
                polyX1 = BitConverter.ToDouble(vertData, ArrayPos);
                polyY1 = BitConverter.ToDouble(vertData, ArrayPos + 8);
                ArrayPos += 16;

                for (int vertNo = partStart[i - 1] + 1; vertNo < partStart[i]; vertNo++)
                {
                    polyX2 = BitConverter.ToDouble(vertData, ArrayPos);
                    polyY2 = BitConverter.ToDouble(vertData, ArrayPos + 8);
                    ArrayPos += 16;
                    if ((PointY > Math.Min(polyY1, polyY2)))
                    {
                        if ((PointY <= Math.Max(polyY1, polyY2)))
                        {
                            if ((PointX <= Math.Max(polyX1, polyX2)))
                            {
                                if ((polyY1 != polyY2))
                                {
                                    XInters = Convert.ToDouble(PointY - polyY1) * Convert.ToDouble(polyX2 - polyX1) / Convert.ToDouble(polyY2 - polyY1) + polyX1;
                                    if (((polyX1 == polyX2) | (PointX <= Convert.ToInt32(XInters))))
                                    {
                                        Counter++;
                                    }
                                }
                            }
                        }
                    }
                    polyX1 = polyX2;
                    polyY1 = polyY2;

                }

            }

            if ((Counter % 2 == 0))
            { return false; }
            else
            { return true; }
        }

        private bool PointOnPolylineData(double PointX, double PointY, double Tolerance, byte[] vertData)
        {
            int lvarNoOfParts = BitConverter.ToInt32(vertData, 36);
            int lvarNoOfPoints = BitConverter.ToInt32(vertData, 40);
            double polyX1;
            double polyY1;
            double polyX2;
            double polyY2;

            int[] partStart = new int[lvarNoOfParts + 1];
            int ArrayPos = 44;

            for (int i = 0; i < partStart.Length; i++)
            {
                int vertBegin = BitConverter.ToInt32(vertData, ArrayPos);
                partStart[i] = vertBegin;
                ArrayPos += 4;
            }
            partStart[partStart.Length - 1] = lvarNoOfPoints;


            for (int i = 1; i < partStart.Length; i++)
            {
                polyX1 = BitConverter.ToDouble(vertData, ArrayPos);
                polyY1 = BitConverter.ToDouble(vertData, ArrayPos + 8);
                ArrayPos += 16;

                for (int vertNo = partStart[i] + 1; vertNo < partStart[i]; vertNo++)
                {
                    polyX2 = BitConverter.ToDouble(vertData, ArrayPos);
                    polyY2 = BitConverter.ToDouble(vertData, ArrayPos + 8);
                    ArrayPos += 16;

                    if (PointToLineDist(PointX, PointY, polyX1, polyY1, polyX2, polyY2) <= Tolerance)
                    { return true; }

                    polyX1 = polyX2;
                    polyY1 = polyY2;

                }

            }

            return false;
        }


        private bool ResolveSQLFromText(string vString, string vData)
        {
            // *********************************************************************************
            // * A string is parsed that contains the field, condition, criteria match triplet *
            // * e.g. Name = 'Me'                                                              *
            // *********************************************************************************

            string FieldName = null;
            object FindValue = null;
            object FoundValue = null;
            string DataValue = null;
            short Offset = 0;
            short Exponent = 0;

            string Condition = null;
            bool varNoMatch = false;
            string TestString = null;
            string SubString = null;
            string TestValue = null;
            short k = 0;

            varNoMatch = true;
            if (vString.Contains("!="))
            { Condition = "!="; }
            else if (vString.Contains("<>"))
            { Condition = "<>"; }
            else if (vString.Contains("<="))
            { Condition = "<="; }
            else if (vString.Contains(">="))
            { Condition = ">="; }
            else if (vString.Contains("="))
            { Condition = "="; }
            else if (vString.Contains("<"))
            { Condition = "<"; }
            else if (vString.Contains(">"))
            { Condition = ">"; }
            else if (vString.ToUpper().Contains(" NOT IN "))
            { Condition = "NOT IN"; }
            else if (vString.ToUpper().Contains(" NOT LIKE "))
            { Condition = "NOT LIKE"; }
            else if (vString.ToUpper().Contains(" NOT IS NULL"))
            { Condition = "NOT IS NULL"; }
            else if (vString.ToUpper().Contains(" IN "))
            { Condition = "IN"; }
            else if (vString.ToUpper().Contains(" LIKE "))
            { Condition = "LIKE"; }
            else if (vString.ToUpper().Contains(" IS NULL"))
            { Condition = "IS NULL"; }
            else
            {
                throw new Exception("Incomplete SQL statement");
            }

            TestValue = vString.Substring(vString.IndexOf(Condition) + Condition.Length).Trim();
            FieldName = vString.Substring(0, vString.IndexOf(Condition)).Trim().ToUpper();
            // remove wrapper from fieldname
            if (FieldName.StartsWith("["))
            {
                if (FieldName.EndsWith("]"))
                { FieldName = FieldName.Substring(1, FieldName.Length - 2); }
            }


            switch (Condition)
            {
                case "IS NULL":
                    return TestValue == null;
                case "IS NOT NULL":
                    return TestValue != null;
                default:

                    // Now compare the values

                    for (k = 0; k < mvarFields.Count; k++)
                    {
                        if (mvarFields[k].Name == FieldName)
                        {
                            break;
                        }
                        Offset += mvarFields[k].Size;
                    }
                    DataValue = vData.Substring(Offset, mvarFields[k].Size).Trim();

                    // convert value to type

                    switch (mvarFields[FieldName].Type)
                    {
                        case eFieldType.shpText:
                            FindValue = TestValue;
                            // remove the quotes
                            if (TestValue.ToString().StartsWith("'") | TestValue.ToString().StartsWith("\""))
                            { FindValue = TestValue.Substring(1, TestValue.Length - 2); }
                            FoundValue = DataValue.TrimEnd();

                            break;
                        case eFieldType.shpNumeric:
                            if (Condition != "IN" & Condition != "LIKE")
                            {
                                FindValue = Convert.ToDouble(TestValue);
                            }
                            else
                            {
                                FindValue = TestValue;
                            }
                            if (DataValue != null)
                            {
                                //Check the Delimiter
                                if (DataValue.Contains(",") | DataValue.Contains(" ") | DataValue.Contains("."))
                                {
                                    DataValue = DataValue.Replace(" ", mvardbfDelimiter);
                                    DataValue = DataValue.Replace(",", mvardbfDelimiter);
                                    DataValue = DataValue.Replace(".", mvardbfDelimiter);
                                }
                                FoundValue = Convert.ToDouble(DataValue);
                            }
                            else
                            {
                                FoundValue = null;
                            }

                            break;
                        case eFieldType.shpBoolean:
                            switch (TestValue.ToUpper())
                            {
                                case "TRUE":
                                    FindValue = true;
                                    break;
                                case "T":
                                    FindValue = true;
                                    break;
                                case "YES":
                                    FindValue = true;
                                    break;
                                case "Y":
                                    FindValue = true;
                                    break;
                                default:
                                    FindValue = false;
                                    break;
                            }
                            if (DataValue.ToUpper() == "Y" | DataValue.ToUpper() == "T")
                            { FoundValue = true; }
                            else
                            { FoundValue = false; }

                            break;
                        case eFieldType.shpDate:
                            FindValue = Convert.ToDateTime(TestValue);
                            if (DataValue != null)
                            {
                                if (Convert.ToDouble(DataValue) == 0)
                                {
                                    FoundValue = null;
                                }
                                else
                                {
                                    // Read in YYYYMMDD and convert to generic date for CDate
                                    if (mvarYYYYMMDD) //Format "ddmmyyyy" )
                                    { FoundValue = System.DateTime.ParseExact(DataValue, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); }
                                    else // Format "yyyymmdd"
                                    { FoundValue = System.DateTime.ParseExact(DataValue, "ddMMyyyy", System.Globalization.CultureInfo.CurrentCulture); }
                                }
                            }
                            else
                            {
                                FoundValue = null;
                            }
                            break;
                        case eFieldType.shpFloat:
                            if (DataValue != null)
                            {
                                // Floating values are represented by values and expontent e.g. 1.23425e+004
                                //Check the mvardbfDelimiter
                                if (mvarsysDelimiter != ".")
                                {
                                    TestValue = TestValue.Replace(mvardbfDelimiter, mvarsysDelimiter);
                                    DataValue = DataValue.Replace(mvardbfDelimiter, mvarsysDelimiter);
                                }

                                if (TestValue.ToUpper().Contains("E"))
                                {
                                    Exponent = Convert.ToInt16(TestValue.Substring(TestValue.ToUpper().IndexOf("E") + 1));
                                    FindValue = TestValue.Substring(0, TestValue.ToUpper().IndexOf("E"));
                                    FindValue = Convert.ToDouble(FindValue) * (Math.Pow(10, Exponent));
                                }
                                else
                                {
                                    FindValue = Convert.ToDouble(TestValue);
                                }

                                if (DataValue.ToUpper().Contains("E"))
                                {
                                    Exponent = Convert.ToInt16(DataValue.Substring(DataValue.ToUpper().IndexOf("E") + 1));
                                    FoundValue = DataValue.Substring(0, DataValue.ToUpper().IndexOf("E"));
                                    FoundValue = Convert.ToDouble(FoundValue) * (Math.Pow(10, Exponent));
                                }
                                else
                                {
                                    FoundValue = Convert.ToDouble(DataValue);
                                }

                            }
                            else
                            {
                                FoundValue = null;
                            }
                            break;
                    }


                    break;
            }

            switch (Condition)
            {
                case "=":
                    if (ObjectComparison(FoundValue, FindValue) == 0)
                    {
                        varNoMatch = false;
                        //Exit For
                    }
                    break;
                case "<":
                    if (ObjectComparison(FoundValue, FindValue) == -1)
                    {
                        varNoMatch = false;
                        //Exit For
                    }
                    break;
                case "<=":
                    if (ObjectComparison(FoundValue, FindValue) == 0 | ObjectComparison(FoundValue, FindValue) == -1)
                    {
                        varNoMatch = false;
                        //Exit For
                    }
                    break;
                case ">":
                    if (ObjectComparison(FoundValue, FindValue) == 1)
                    {
                        varNoMatch = false;
                        //Exit For
                    }
                    break;
                case ">=":
                    if (ObjectComparison(FoundValue, FindValue) == 0 | ObjectComparison(FoundValue, FindValue) == 1)
                    {
                        varNoMatch = false;
                        //Exit For
                    }
                    break;
                case "<>":
                case "!=":
                    if (FoundValue != FindValue)
                    {
                        varNoMatch = false;
                        //Exit For
                    }
                    break;
                case "LIKE":
                    if (FindValue.ToString().StartsWith("%") | FindValue.ToString().StartsWith("*"))
                    {
                        if (FindValue.ToString().EndsWith("%") | FindValue.ToString().EndsWith("*"))
                        {
                            // Like '%xxx%'
                            if (FoundValue.ToString().Contains(FindValue.ToString().Substring(1, FindValue.ToString().Length - 2)))
                            { varNoMatch = false; }
                        }
                        else
                        {
                            // Like '%xxx'
                            if (FoundValue.ToString().EndsWith(FindValue.ToString().Substring(1)))
                            { varNoMatch = false; }
                        }
                    }
                    else if (FindValue.ToString().EndsWith("%") | FindValue.ToString().EndsWith("*"))
                    {
                        // Like 'xxx%'
                        if (FoundValue.ToString().StartsWith(FindValue.ToString().Substring(0, FindValue.ToString().Length - 1)))
                        { varNoMatch = false; }
                    }
                    else
                    {
                        // Like 'xxx'
                        if (FoundValue == FindValue)
                        { varNoMatch = false; }
                    }
                    break;
                case "NOT LIKE":
                    varNoMatch = false;
                    if (FindValue.ToString().StartsWith("%") | FindValue.ToString().StartsWith("*"))
                    {
                        if (FindValue.ToString().EndsWith("%") | FindValue.ToString().EndsWith("*"))
                        {
                            // Like '%xxx%'
                            if (FoundValue.ToString().Contains(FindValue.ToString().Substring(1, FindValue.ToString().Length - 2)))
                            { varNoMatch = true; }
                        }
                        else
                        {
                            // Like '%xxx'
                            if (FoundValue.ToString().EndsWith(FindValue.ToString().Substring(1)))
                            { varNoMatch = true; }
                        }
                    }
                    else if (FindValue.ToString().EndsWith("%") | FindValue.ToString().EndsWith("*"))
                    {
                        // Like 'xxx%'
                        if (FoundValue.ToString().StartsWith(FindValue.ToString().Substring(0, FindValue.ToString().Length - 1)))
                        { varNoMatch = true; }
                    }
                    else
                    {
                        // Like 'xxx'
                        if (FoundValue == FindValue)
                        { varNoMatch = true; }
                    }
                    break;
                case "IN":
                    // Remove the brackets
                    TestString = FindValue.ToString().Substring(1, FindValue.ToString().Length - 2);
                    while (TestString.Contains(","))
                    {
                        // subset the leftmost instance of the in values
                        SubString = TestString.Substring(0, TestString.IndexOf(","));
                        TestString = TestString.Substring(TestString.IndexOf(",") + 1);

                        // remove any quotes
                        if (SubString.StartsWith("'") | SubString.StartsWith("\""))
                        { SubString = SubString.Substring(1, SubString.Length - 1); }
                        if (SubString.EndsWith("'") | SubString.EndsWith("\""))
                        { SubString = SubString.Substring(0, SubString.Length - 1); }

                        if (FoundValue.ToString() == SubString)
                        {
                            varNoMatch = false;
                            break;
                        }
                    }
                    if (varNoMatch == true)
                    {
                        TestString = TestString.TrimEnd();
                        // remove any quotes
                        if (TestString.StartsWith("'") | TestString.StartsWith("\""))
                        { TestString = TestString.Substring(1, SubString.Length - 1); }
                        if (TestString.EndsWith("'") | TestString.EndsWith("\""))
                        { TestString = TestString.Substring(0, TestString.Length - 1); }
                        if (FoundValue.ToString() == TestString)
                        { varNoMatch = false; }
                    }
                    break;
                case "NOT IN":
                    varNoMatch = false;
                    TestString = FindValue.ToString().Substring(1, FindValue.ToString().Length - 2);
                    while (TestString.Contains(","))
                    {
                        // subset the leftmost instance of the in values
                        SubString = TestString.Substring(0, TestString.IndexOf(","));
                        TestString = TestString.Substring(TestString.IndexOf(",") + 1);

                        // remove any quotes
                        if (SubString.StartsWith("'") | SubString.StartsWith("\""))
                        { SubString = SubString.Substring(1, SubString.Length - 1); }
                        if (SubString.EndsWith("'") | SubString.EndsWith("\""))
                        { SubString = SubString.Substring(0, SubString.Length - 1); }

                        if (FoundValue.ToString() == SubString)
                        {
                            varNoMatch = true;
                            break;
                        }
                    }
                    if (varNoMatch == false)
                    {
                        TestString = TestString.TrimEnd();
                        // remove any quotes
                        if (TestString.StartsWith("'") | TestString.StartsWith("\""))
                        { TestString = TestString.Substring(1, SubString.Length - 1); }
                        if (TestString.EndsWith("'") | TestString.EndsWith("\""))
                        { TestString = TestString.Substring(0, TestString.Length - 1); }

                        if (FoundValue.ToString() == TestString)
                        { varNoMatch = true; }
                    }
                    break;
            }

            return !varNoMatch;

        }

        private int ObjectComparison(object obj1, object obj2)
        {
            // Compares between two objects of same type
            int compValue = -2;

            try
            {
                double dbValue1;
                double dbValue2;
                DateTime dtValue1;
                DateTime dtValue2;
                bool bValue1;
                bool bValue2;
                string dsValue1 = obj1.ToString();
                string dsValue2 = obj2.ToString();
                if (Double.TryParse(dsValue1, out dbValue1) && Double.TryParse(dsValue2, out dbValue2))
                { compValue = dbValue1.CompareTo(dbValue2); }
                else if (DateTime.TryParse(dsValue1, out dtValue1) && DateTime.TryParse(dsValue2, out dtValue2))
                { compValue = dtValue1.CompareTo(dtValue2); }
                else if (Boolean.TryParse(dsValue1, out bValue1) && Boolean.TryParse(dsValue2, out bValue2))
                { compValue = bValue1.CompareTo(bValue2); }
                else
                { compValue = dsValue1.CompareTo(dsValue2); }
            }
            catch { }
            return compValue;

        }

        /// <summary>
        /// Locates the first record in a ShapeFiles object that satisfies the specified SQL query string criteria and 
        /// makes that record the current record.
        /// </summary>
        /// <param name="QueryString">A SQL query string.  Field names can be surrounded by square brackets.  Use single quotes to denote text strings.</param>
        /// <remarks>The FindFirst method searches through each shape in the <B>ShapeFile</B> and locates the first record that statisfies either the criteria listed in the query string, or a point in polygon match for polygon themes, a point on line for line themes and a point on point for point theme. Point in polygon searches ignore any records where the match is a polygon hole. Once found, it makes the found record current (moving the to the record and loading the various data necessary) and sets the <see cref="NoMatch"/> property to <I>False</I>.  If the method fails to locate a match, the <I>NoMatch</I> property is set to <I>True</I>, and the current record is undefined.  To find subsequent records that match your criteria, use the <see cref="FindNext"/> method.  I haven't used LINQ ... I've written my own query parser (which, strangely enough was quite fun)... but you could, you could.  If you want to recode this section feel free.
        /// <para>So what comparisons have been implemented in the SQL parser?</para>
        ///<list type="table">
        ///    <listheader>
        ///        <term>Comparison</term>
        ///        <description>Description</description>
        ///    </listheader>
        ///    <item>
        ///        <term> &lt;</term>
        ///        <description>The field value is less than that of the criteria</description>
        ///    </item>
        ///  <item>
        ///    <term> =&lt;</term>
        ///    <description>The field value is less than or equal to that of the criteria</description>
        ///  </item>
        ///  <item>
        ///    <term> = </term>
        ///    <description>The field value is equal to that of the criteria</description>
        ///  </item>
        ///  <item>
        ///    <term> &gt;=</term>
        ///    <description>The field value is greater than or equal to that of the criteria</description>
        ///  </item>
        ///  <item>
        ///    <term> &gt;</term>
        ///    <description>The field value is greater than that of the criteria</description>
        ///  </item>
        ///  <item>
        ///    <term> &lt;&gt;, !=</term>
        ///    <description>The field value is not equal to that of the criteria.</description>
        ///  </item>
        ///  <item>
        ///    <term> Like </term>
        ///    <description>The field value is like that of the criteria. Use the % to denote the wild card (e.g. LIKE 'Find Me%').  This is to be used against a string values only. </description>
        ///  </item>
        ///  <item>
        ///    <term> In</term>
        ///    <description>The field value is exists in the criteria list of items. Each criteria value is separated by a comma and held within brackets e.g. IN (1,2,23)</description>
        ///  </item>
        ///  <item>
        ///    <term> Is Null</term>
        ///    <description>The field value is NULL</description>
        ///  </item>
        ///  <item>
        ///    <term> Not Like</term>
        ///    <description>The field value is not like that of the criteria. Use the % to denote the wild card (e.g. NOT LIKE 'Find Me%').  This is to be used against a string values only. </description>
        ///  </item>
        ///  <item>
        ///    <term> Not In</term>
        ///    <description>The field value does not exist in the criteria list of items. Each criteria value is separated by a comma and held within brackets e.g. NOT IN (1,2,23)</description>
        ///  </item>
        ///  <item>
        ///    <term> Not Is Null</term>
        ///    <description>The field value is not NULL</description>
        ///  </item>
        ///</list>
        ///<para>You can also
        ///<li>Join any conditions together using the AND and OR operators</li> 
        ///<li>group conditions together using brackets</li> 
        ///Note: String comparisons are made using single quotes (e.g. "[TextField] = 'My Name'")</para>
        ///</remarks>
        ///<example><code lang="C#">
        ///     myShape.FindFirst("[FloatField] &gt; 1 and ([DateField] &lt; 10 July 2000 OR [DateField] &gt; 10 July 2010)")
        ///     Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString());
        ///     
        ///     while (!myShape.NoMatch)
        ///     {
        ///         myShape.FindNext();
        ///         if (!myShape.NoMatch)
        ///         {
        ///            Console.WriteLine(&quot;Next value found at Record {0}&quot;, myShape.CurrentRecord.ToString());
        ///            foreach (Field mF in myShape.Fields)
        ///                Console.WriteLine(mF.Name + "   " + mF.Value.ToString());
        ///        }
        ///    }
        /// </code>
        /// <code lang="VB">
        /// myShape.FindFirst("[FloatField] &gt; 1 and ([DateField] &lt; 10 July 2000 OR [DateField] &gt; 10 July 2010)")
        /// Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 
        /// While Not myShape.NoMatch
        /// 	myShape.FindNext()
        /// 	If Not myShape.NoMatch Then
        /// 		Console.WriteLine("Next value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 		For Each mF As Field In myShape.Fields
        ///             Console.WriteLine(mF.Name + "   " + mF.Value.ToString())
        /// 		Next
        /// 	End If
        /// End While
        /// </code>
        /// </example>
        /// <seealso cref="FindNext"/>
        /// <seealso cref="NoMatch"/>
        public void FindFirst(string QueryString)
        {
            // *******************************************************
            // * Find the first instance of a query match in the DBF *
            // *******************************************************

            string DataLine = null;

            // Remove the old vertices & Parts
            mvarEOF = false;
            mvarBOF = false;
            mvarNoMatch = true;
            mvarFindQuery = QueryString;
            mvarFindXY = false;
            for (int i = 0; i < mvarShapeCount; i++)
            {
                DataLine = ReadDBFToString(i + 1);
                if (FindFromQuery(QueryString, DataLine) == true)
                {
                    MoveTo(i);
                    mvarNoMatch = false;
                    break;
                }
            }

        }

        /// <summary>
        /// Locates the first record in a ShapeFiles object that satisfies a Point on Line or Point on Point test.   
        /// The tolerance factor allows you a bit of leaway in finding the nearest line or point to your provided point.
        /// </summary>
        /// <param name="FindX">The X Coordinate of the find point.</param>
        /// <param name="FindY">The Y Coordinate of the find point.</param>
        /// <param name="Tolerance">For Point and Polyline Shapes - How close must the test point be</param>
        /// <remarks><para>This method tests whether any of the opened <B>ShapeFile</B> record geometries lay on the provided point.  Polygon shapes will do a point in polygon test sensitive to holes.  In other words it won't find any shapes where the point sits inside the donut.</para>
        /// <para>For polyline and point features the method tests whether the point provided is within the tolerance distance of the feature.</para>
        /// <para>If a record has been found by your query then the <see cref="NoMatch"/> property will be set to <I>False</I>, otherwise it will be set to <I>True</I>.</para></remarks>
        ///<example><code lang="C#">
        ///     myShape.FindFirst(12.0, 9.0 , 2.5);
        ///     Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString());
        ///     
        ///     while (!myShape.NoMatch)
        ///     {
        ///         myShape.FindNext();
        ///         if (!myShape.NoMatch)
        ///         {
        ///            Console.WriteLine(&quot;Next value found at Record {0}&quot;, myShape.CurrentRecord.ToString());
        ///            foreach (Field mF in myShape.Fields)
        ///                Console.WriteLine(mF.Name + "   " + mF.Value.ToString());
        ///        }
        ///    }
        /// </code>
        /// <code lang="VB">
        /// myShape.FindFirst(12.0, 9.0, 2.5)
        /// Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 
        /// While Not myShape.NoMatch
        /// 	myShape.FindNext()
        /// 	If Not myShape.NoMatch Then
        /// 		Console.WriteLine("Next value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 		For Each mF As Field In myShape.Fields
        ///             Console.WriteLine(mF.Name + "   " + mF.Value.ToString())
        /// 		Next
        /// 	End If
        /// End While
        /// </code>
        /// </example>
        /// <seealso cref="FindNext"/>
        /// <seealso cref="NoMatch"/>
        public void FindFirst(double FindX, double FindY, double Tolerance)
        {
            // ********************************************************************
            // * Finds the first shape based on XY                                *
            // * Polygons do a point in poly, otherwise the closest shape is used *
            // * Calls FindByCoord to do the actual work, parsing the first record*
            // * number to set the ball rolling                                   *
            // ********************************************************************
            FindbyCoord(FindX, FindY, 1, Tolerance);
            mvarFindX = FindX;
            mvarFindY = FindY;
            mvarFindTolerance = Tolerance;
        }

        /// <summary>
        /// Locates the first record in a ShapeFiles object that satisfies a Point in Polygon, Point on Line or Point on Point test againts the provided coordinates.
        /// </summary>
        /// <param name="FindX">The X Coordinate of the find point.</param>
        /// <param name="FindY">The Y Coordinate of the find point.</param>
        /// <remarks><para>This method tests whether any of the opened <B>ShapeFile</B> record geometries lay on the provided point.  Polygon shapes will do a point in polygon test sensitive to holes.  In other words it won't find any shapes where the point sits inside the donut.</para>
        /// <para>If a record has been found by your query then the <see cref="NoMatch"/> property will be set to <I>False</I>, otherwise it will be set to <I>True</I>.</para></remarks>
        ///<example><code lang="C#">
        ///     myShape.FindFirst(12.0, 9.0);
        ///     Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString());
        ///     
        ///     while (!myShape.NoMatch)
        ///     {
        ///         myShape.FindNext();
        ///         if (!myShape.NoMatch)
        ///         {
        ///            Console.WriteLine(&quot;Next value found at Record {0}&quot;, myShape.CurrentRecord.ToString());
        ///            foreach (Field mF in myShape.Fields)
        ///                Console.WriteLine(mF.Name + "   " + mF.Value.ToString());
        ///        }
        ///    }
        /// </code>
        /// <code lang="VB">
        /// myShape.FindFirst(12.0, 9.0)
        /// Console.WriteLine("Value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 
        /// While Not myShape.NoMatch
        /// 	myShape.FindNext()
        /// 	If Not myShape.NoMatch Then
        /// 		Console.WriteLine("Next value found at Record {0}", myShape.CurrentRecord.ToString())
        /// 		For Each mF As Field In myShape.Fields
        ///             Console.WriteLine(mF.Name + "   " + mF.Value.ToString())
        /// 		Next
        /// 	End If
        /// End While
        /// </code>
        /// </example>
        /// <seealso cref="FindNext"/>
        /// <seealso cref="NoMatch"/>
        public void FindFirst(double FindX, double FindY)
        {
            // ********************************************************************
            // * Finds the first shape based on XY                                *
            // * Polygons do a point in poly, otherwise the closest shape is used *
            // * Calls FindByCoord to do the actual work, parsing the first record*
            // * number to set the ball rolling                                   *
            // ********************************************************************
            FindbyCoord(FindX, FindY, 1, 0);
            mvarFindX = FindX;
            mvarFindY = FindY;
            mvarFindTolerance = 0;
        }

        /// <summary>
        /// Locates the next record in the ShapeFile that satisfies the specified criteria 
        /// and makes that record the current record
        /// </summary>
        ///<remarks>
        ///Quite simple really. Define your search criteria using a SQL strutured query or XY coordinate and do a <see cref="O:ArcShapeFile.ShapeFile.FindFirst">FindFirst</see>. The FindNext method searches through each ShapeFile record in turn to find the next record that satisfies the criteria. Once found, it makes that record current and sets the <see cref="NoMatch">NoMatch</see> property to False. If the FindNext methods fail to locate a match, the NoMatch property is set to True, and the current record is undefined.
        ///</remarks>
        ///<seealso cref="NoMatch"/>
        ///<seealso cref="FindFirst(System.String)"/>
        ///<seealso cref="FindFirst(System.Double,System.Double)"/>
        ///<seealso cref="FindFirst(System.Double,System.Double,System.Double)"/>
        public void FindNext()
        {
            // *******************************************************
            // * Find the next instance of a query match in the DBF  *
            // *******************************************************

            string DataLine = null;

            mvarNoMatch = true;
            mvarEOF = false;
            mvarBOF = false;
            for (int i = mvarCurrentRecord + 1; i <= mvarShapeCount; i++)
            {
                if (!mvarFindXY)
                {
                    DataLine = ReadDBFToString(i);
                    if (FindFromQuery(mvarFindQuery, DataLine) == true)
                    {
                        MoveTo(i - 1);
                        mvarNoMatch = false;
                        return;
                    }
                }
                else
                {
                    FindbyCoord(mvarFindX, mvarFindY, i, mvarFindTolerance);
                    return;
                }
            }
        }

        /// <summary>
        /// Locates the first record in a ShapeFiles object that satisfies the specified query 
        /// and makes that record the current record
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="InputLine"></param>
        private bool FindFromQuery(string Query, string InputLine)
        {
            // ***********************************************************
            // * Test an input text string for validity against a filter *
            // ***********************************************************

            string SQL_String = null;

            string TempString = null;
            string ParseString = null;
            ArrayList SQLResults = new ArrayList();
            SQLLevels Result;

            int SQLLevel = 1;
            bool EndResult = false;
            bool NextResult = false;
            int ThisLevel = 1;
            int LastOperator = 0;

            // Avoid null queries
            if (InputLine == null | Query == null)
                return false;



            // Replace any strings using " to '
            SQL_String = Query.Replace("\"", "'");
            // Force everything outside of single quotes to upper case
            // This ensures a match against DBF field names

            // Break the SQL string into its compontent parts

            // Look along the string for an AND, OR or (
            TempString = SQL_String;
            while (TempString.Trim().Length > 0)
            {
                ParseString = null;
                while (!TempString.ToUpper().StartsWith(" AND ") & !TempString.ToUpper().StartsWith(" OR ") & !TempString.StartsWith("(") & !TempString.StartsWith(")") & TempString.Length > 0)
                {
                    // Peel off each character at a time
                    if (TempString.Substring(0, 1) == "'")
                    {
                        // Special case for quoted strings
                        int quotepos = TempString.IndexOf("'", 1);
                        ParseString += TempString.Substring(0, quotepos + 1);
                        TempString = TempString.Substring(quotepos + 1);
                    }
                    else
                    {
                        ParseString += TempString.Substring(0, 1);
                        TempString = TempString.Substring(1);
                    }
                }

                // Check for an IN condition
                if (ParseString.EndsWith(" IN "))
                {
                    do
                    {
                        // Peel off each character at a time
                        ParseString += TempString.Substring(0, 1);
                        TempString = TempString.Substring(1);
                    }
                    while (!ParseString.EndsWith(")"));
                }

                // Test the component
                if (ParseString.Length > 0)
                {
                    Result = new SQLLevels();
                    Result.Result = ResolveSQLFromText(ParseString, InputLine);

                    // What is the conditional logic
                    if (TempString.ToUpper().StartsWith(" AND "))
                    {
                        TempString = TempString.Substring(5).TrimStart();
                        // add AND to the next condition list
                        Result.SQLOperator = 1;
                        Result.Level = SQLLevel;
                    }
                    else if (TempString.ToUpper().StartsWith(" OR "))
                    {
                        TempString = TempString.Substring(3).TrimStart();
                        // add OR to the next condition list
                        Result.SQLOperator = 2;
                        Result.Level = SQLLevel;
                    }
                    else if (TempString.StartsWith("("))
                    {
                        TempString = TempString.Substring(1).TrimStart();
                        SQLLevel++;
                        // add ( to the next priority list
                    }
                    else if (TempString.StartsWith(")"))
                    {
                        TempString = TempString.Substring(1).TrimStart();
                        SQLLevel--;
                        // add ) to the next priority list
                    }
                    else
                    {
                        Result.Level = SQLLevel;
                    }
                    SQLResults.Add(Result);
                }
            }
            // All the results are now in the arrays

            Result = (SQLLevels)SQLResults[0];
            EndResult = Result.Result;
            for (int i = 1; i < SQLResults.Count; i++)
            {
                SQLLevels thisRec = (SQLLevels)SQLResults[i];
                SQLLevels prevRec = (SQLLevels)SQLResults[i - 1];

                // compare the levels
                if (thisRec.Level == prevRec.Level)
                {
                    if (EndResult == true)
                    {
                        if (thisRec.Result == false)
                        {
                            // Set to False if the AND is also false
                            if (prevRec.SQLOperator == 1)
                                EndResult = false;
                        }
                    }
                    else
                    {
                        if (thisRec.Result == true)
                        {
                            // Set to True if the OR value is true
                            if (prevRec.SQLOperator == 2)
                                EndResult = true;
                        }
                    }
                    LastOperator = prevRec.SQLOperator;
                }
                else
                {
                    // The levels have changed deal with these then compare against the endresult
                    NextResult = thisRec.Result;
                    ThisLevel = thisRec.Level;
                    i++;
                    thisRec = (SQLLevels)SQLResults[i];
                    prevRec = (SQLLevels)SQLResults[i - 1];
                    while (i <= SQLResults.Count)
                    {
                        if (thisRec.Level == ThisLevel)
                        {
                            if (NextResult == true)
                            {
                                if (thisRec.Result == false)
                                {
                                    // Set to False if the AND is also false
                                    if (prevRec.SQLOperator == 1)
                                        NextResult = false;
                                }
                            }
                            else
                            {
                                if (thisRec.Result == true)
                                {
                                    // Set to True if the OR value is true
                                    if (prevRec.SQLOperator == 2)
                                        NextResult = true;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                        i++;
                    }
                    // Resolve end vs next
                    if (EndResult == false)
                    {
                        if (NextResult == true)
                        {
                            if (LastOperator == 1)
                                EndResult = true;
                        }
                    }
                    else
                    {
                        if (LastOperator == 1)
                        {
                            EndResult = NextResult;
                        }
                    }
                }

            }
            return EndResult;

        }

        private void FindbyCoord(double InX, double InY, int StartRecord, double Tolerance)
        {
            // ********************************************************************
            // * Finds the shape based on XY                                      *
            // * Polygons do a point in poly, otherwise the closest shape is used *
            // ********************************************************************

            int Record = 0;
            int FoundRecord = 0;
            int ArrayPos;
            byte[] ByteArray = new byte[4];


            // Maximum double values

            OpenStream(mvarShapeFile, ref fsShapeFile);
            OpenStream(mvarShapeIndex, ref fsShapeIndex);

            for (Record = StartRecord; Record <= mvarShapeCount; Record++)
            {
                // Read the data into the array shpPts

                long FilePos = GetShapeRecordStart(Record, fsShapeIndex);
                fsShapeFile.Seek(FilePos, SeekOrigin.Begin);
                fsShapeFile.Read(ByteArray, 0, 4);
                Array.Reverse(ByteArray);
                int ContentLength = BitConverter.ToInt32(ByteArray, 0);
                byte[] inVertData = new byte[ContentLength * 2];
                fsShapeFile.Read(inVertData, 0, inVertData.Length);

                // Populate the mvarShape<mbr> values from the intput data array
                ReadShapeRecordHeader(inVertData);
                switch ((eShapeType)mvarRecordShapeType)
                {

                    case eShapeType.shpPoint:
                    case eShapeType.shpPointM:
                    case eShapeType.shpPointZ:
                        // *******************************************
                        // * Ppoint Search                           *
                        // * Use Nearest distance method             *
                        // *******************************************

                        if (System.Math.Sqrt(Math.Pow((InX - mvarShapeXMin), 2) + Math.Pow((InY - mvarShapeYMin), 2)) <= Tolerance)
                        {
                            // The point is very close so display it
                            // The xy is in the mbr, so no need to re-read it
                            FoundRecord = Record;
                        }

                        break;
                    case eShapeType.shpMultiPoint:
                    case eShapeType.shpMultiPointZ:
                    case eShapeType.shpMultiPointM:
                        // *******************************************
                        // * Multipoint Search                       *
                        // * Use Nearest distance method             *
                        // *******************************************

                        if (InX >= mvarShapeXMin)
                        {
                            if (InX <= mvarShapeXMax)
                            {
                                if (InY >= mvarShapeYMin)
                                {
                                    if (InY <= mvarShapeYMax)
                                    {
                                        // The found point is inside the mbr of the shape
                                        ArrayPos = 40;
                                        for (int i = 0; i < mvarNoOfPoints; i++)
                                        {
                                            double XVal = BitConverter.ToDouble(inVertData, ArrayPos);
                                            double YVal = BitConverter.ToDouble(inVertData, ArrayPos + 8);
                                            double dist = System.Math.Sqrt((Math.Pow((InX - XVal), 2)) + (Math.Pow((InY - YVal), 2)));
                                            if (dist < Tolerance)
                                            {
                                                //The point is very close so display it
                                                FoundRecord = Record;
                                                break;
                                            }
                                            ArrayPos += 16;
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    case eShapeType.shpPolygon:
                    case eShapeType.shpPolygonZ:
                    case eShapeType.shpPolygonM:
                    case eShapeType.shpMultiPatch:
                        // Polygon
                        // *******************************************
                        // * Polygon Search                          *
                        // * Use Point In Polygon method             *
                        // *******************************************

                        if (InX >= mvarShapeXMin)
                        {
                            if (InX <= mvarShapeXMax)
                            {
                                if (InY >= mvarShapeYMin)
                                {
                                    if (InY <= mvarShapeYMax)
                                    {
                                        // The found point is inside the mbr of the shape
                                        // Load the shape and test point
                                        if (PointInPolygonData(InX, InY, inVertData))
                                        {
                                            FoundRecord = Record;
                                        }
                                    }
                                }
                            }
                        }

                        break;

                    case eShapeType.shpPolyLine:
                    case eShapeType.shpPolyLineZ:
                    case eShapeType.shpPolyLineM:
                        //Arc
                        // *******************************************
                        // * PolyLine Search                         *
                        // * Use Nearest distance method             *
                        // *******************************************

                        if (InX >= mvarShapeXMin)
                        {
                            if (InX <= mvarShapeXMax)
                            {
                                if (InY >= mvarShapeYMin)
                                {
                                    if (InY <= mvarShapeYMax)
                                    {
                                        // The found point is inside the mbr of the shape
                                        if (PointOnPolylineData(InX, InY, Tolerance, inVertData))
                                        { FoundRecord = Record; }
                                    }
                                }
                            }
                        }

                        break;
                }

                // Exit loop if a match has occurred
                if (FoundRecord > 0)
                    break;
            } // Next record

            CloseStream(ref fsShapeFile);
            CloseStream(ref fsShapeIndex);

            if (FoundRecord > 0)
            {
                MoveTo(FoundRecord);
                mvarNoMatch = false;
            }
            else
            {
                mvarNoMatch = true;
            }
            mvarFindXY = true;

        }

        /// <summary>
        /// Tests whether the entered coordinates are within the polygon or on the point or line of the current shape record 
        /// </summary>
        /// <returns>True if the point given lays within or is on the current polygon shape</returns>
        /// <param name="InX">The X Coordinate of the find point.</param>
        /// <param name="InY">The Y Coordinate of the find point.</param>
        ///<value>Does the point given sit in or on the current shape</value>
        ///<remarks>
        ///Just like the FindFirst method ... but only dealing with the current shape record.  If the point provided is within the polygon shape (not in a hole) or on the current point or line then you will get a True returned.
        ///</remarks>
        ///<seealso cref="FindFirst(System.String)"/>
        ///<seealso cref="FindFirst(System.Double,System.Double)"/>
        ///<seealso cref="FindFirst(System.Double,System.Double,System.Double)"/>
        public bool IsVisible(double InX, double InY)
        {
            return IsPointVisible(InX, InY, 0);
        }

        
        /// <summary>
        /// Tests whether the entered coordinates are on the point or line of the current shape record, give or take your tolerance value
        /// </summary>
        /// <param name="InX">The X Coordinate of the find point.</param>
        /// <param name="InY">The Y Coordinate of the find point.</param>
        /// <param name="Tolerance">For Point and Polyline Shapes - How close must the test point be</param>
        /// <returns>True if the point given lays within or is on the current polygon shape</returns>
        ///<value>Does the point given sit in or on the current shape</value>
        ///<remarks>
        ///Just like the FindFirst method ... but only dealing with the current shape record.  If the point provided is within the polygon shape (not in a hole) or within the tolerance distance from the current point or line then you will get a True returned.
        ///</remarks>
        public bool IsVisible(double InX, double InY, double Tolerance)
        {
            return IsPointVisible(InX, InY, Tolerance);
        }

        private bool IsPointVisible(double InX, double InY, double Tolerance)
        {
            // ********************************************************************
            // * Tests if the current shape contains the XY                       *
            // * Polygons do a point in poly, otherwise the closest shape is used *
            // ********************************************************************

            int ArrayPos;
            byte[] ByteArray = new byte[4];
            bool isVisible = false;

            // Populate the mvarShape<mbr> values from the intput data array
            ReadShapeRecordHeader(mvarVertices.vertData);
            switch ((eShapeType)mvarRecordShapeType)
            {

                case eShapeType.shpPoint:
                case eShapeType.shpPointM:
                case eShapeType.shpPointZ:
                    // *******************************************
                    // * Point Search                            *
                    // * Use Nearest distance method             *
                    // *******************************************

                    if (System.Math.Sqrt(Math.Pow((InX - mvarShapeXMin), 2) + Math.Pow((InY - mvarShapeYMin), 2)) <= Tolerance)
                    {
                        // The point is very close so display it
                        // The xy is in the mbr, so no need to re-read it
                        isVisible = true;
                    }

                    break;
                case eShapeType.shpMultiPoint:
                case eShapeType.shpMultiPointZ:
                case eShapeType.shpMultiPointM:
                    // *******************************************
                    // * Multipoint Search                       *
                    // * Use Nearest distance method             *
                    // *******************************************

                    if (InX >= mvarShapeXMin)
                    {
                        if (InX <= mvarShapeXMax)
                        {
                            if (InY >= mvarShapeYMin)
                            {
                                if (InY <= mvarShapeYMax)
                                {
                                    // The found point is inside the mbr of the shape
                                    ArrayPos = 40;
                                    for (int i = 0; i < mvarNoOfPoints; i++)
                                    {
                                        double XVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos);
                                        double YVal = BitConverter.ToDouble(mvarVertices.vertData, ArrayPos + 8);
                                        double dist = System.Math.Sqrt((Math.Pow((InX - XVal), 2)) + (Math.Pow((InY - YVal), 2)));
                                        if (dist < Tolerance)
                                        {
                                            //The point is very close so display it
                                            isVisible = true;
                                            break;
                                        }
                                        ArrayPos += 16;
                                    }
                                }
                            }
                        }
                    }

                    break;
                case eShapeType.shpPolygon:
                case eShapeType.shpPolygonZ:
                case eShapeType.shpPolygonM:
                case eShapeType.shpMultiPatch:
                    // Polygon
                    // *******************************************
                    // * Polygon Search                          *
                    // * Use Point In Polygon method             *
                    // *******************************************

                    if (InX >= mvarShapeXMin)
                    {
                        if (InX <= mvarShapeXMax)
                        {
                            if (InY >= mvarShapeYMin)
                            {
                                if (InY <= mvarShapeYMax)
                                {
                                    // The found point is inside the mbr of the shape
                                    // Load the shape and test point
                                    if (PointInPolygonData(InX, InY, mvarVertices.vertData))
                                    {
                                        isVisible = true;
                                    }
                                }
                            }
                        }
                    }

                    break;

                case eShapeType.shpPolyLine:
                case eShapeType.shpPolyLineZ:
                case eShapeType.shpPolyLineM:
                    //Arc
                    // *******************************************
                    // * PolyLine Search                         *
                    // * Use Nearest distance method             *
                    // *******************************************

                    if (InX >= mvarShapeXMin)
                    {
                        if (InX <= mvarShapeXMax)
                        {
                            if (InY >= mvarShapeYMin)
                            {
                                if (InY <= mvarShapeYMax)
                                {
                                    // The found point is inside the mbr of the shape
                                    if (PointOnPolylineData(InX, InY, Tolerance, mvarVertices.vertData))
                                    {
                                        isVisible = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    break;
            }

            return isVisible;
        }

        #endregion

        #region **********          Events                        **********

        /// <summary>
        /// All vertices have been clear - reset the parts too
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mvarVertices_VerticesCleared(object sender, EventArgs e)
        {
            mvarParts = new Parts();
        }

        /// <summary>
        /// Add a new part to the collection
        /// </summary>
        /// <param name="e"></param>
        private void mvarVertices_PartAdded(AddPartArgs e)
        {
            if (mvarParts.Count == 0)
                mvarParts.Add(0);
            else
                mvarParts.Add(mvarVertices.Count);
            mvarParts[mvarParts.Count - 1].Ends = mvarVertices.Count - 1;// mvarParts[mvarParts.Count - 1].Begins;
            mvarParts[mvarParts.Count - 1].PartType = e.PartType;

        }

        /// <summary>
        /// When a vertice is added make sure that the Parts structure reflects the correct beginning and end
        /// </summary>
        /// <param name="e"></param>
        private void mvarVertices_VerticeAdded(AddVertArgs e)
        {
            if (mvarParts.Count == 0)
                mvarParts.Add(0);

            int PartNo = mvarParts.Count - 1;
            //Find the current part
            if (e.Insert)
            {
                for (int i = 0; i < mvarParts.Count; i++)
                {
                    if (e.VerticeNo >= mvarParts[i].Begins && e.VerticeNo <= mvarParts[i].Ends)
                    {
                        PartNo = 0;
                        break;
                    }
                }
            }
            Part mPart = mvarParts[PartNo];
            mPart.Ends = Math.Max(mPart.Ends, e.VerticeNo);

            if (e.Insert)
            {
                // The record has been inserted - adjust the beginning and end of subsequent parts
                for (int i = PartNo + 1; i < mvarParts.Count; i++)
                {
                    mvarParts[i].Begins += 1;
                    mvarParts[i].Ends += 1;
                }
            }

        }

        /// <summary>
        /// Update the Part beginning and end point when a vertice is deleted
        /// </summary>
        /// <param name="e"></param>
        private void mvarVertices_VerticeDeleted(DelVertArgs e)
        {
            // Find the correct part
            int PartNo = mvarParts.Count - 1;
            for (int i = 0; i < mvarParts.Count; i++)
            {
                if (e.VerticeNo >= mvarParts[i].Begins && e.VerticeNo <= mvarParts[i].Ends)
                {
                    PartNo = 0;
                    break;
                }
            }

            Part mPart = mvarParts[PartNo];
            mPart.Ends--;

            for (int i = PartNo - 1; i > -1; i++)
            {
                mvarParts[i].Begins--;
                mvarParts[i].Ends--;
            }
            if (mvarParts[0].Ends < 0)
                mvarParts[0].Ends = 0;

        }

        /// <summary>
        /// Event raised when a new record is added to the ShapeFile
        /// </summary>
        /// <remarks>
        /// Fired during a <see cref="WriteShape"/> command, this event will tell you when a record has been added to the ShapeFile.
        /// </remarks>
        public event ShapeCreatedEventHandler ShapeRecordCreated;
        /// <summary>
        /// Raised when the ShapeFile record has been created
        /// </summary>
        /// <param name="e">The record number of the created record</param>
        /// <remarks>
        /// Fired during a <see cref="WriteShape"/> command, this event will tell you when a record has been added to the ShapeFile using this DLL.  I envisaged using this
        /// event to log when a user adds a new record.
        /// </remarks>
        public delegate void ShapeCreatedEventHandler(ShapeFileEventArgs e);
        internal virtual void onShapeRecordCreated(ShapeFileEventArgs e) { if (ShapeRecordCreated != null) ShapeRecordCreated(e); }

        /// <summary>
        /// Event raised when a record is deleted from the ShapeFile
        /// </summary>
        /// <remarks>
        /// Fired during a <see cref="DeleteShape"/> command, this event will tell you when a record has been marked for deletion in the ShapeFile.
        /// </remarks>
        public event ShapeDeletedEventHandler ShapeRecordDeleted;
        /// <summary>
        /// Raised when the ShapeFile record has been deleted
        /// </summary>
        /// <param name="e">The record number of the deleted record</param>
        /// <remarks>
        /// Fired during a <see cref="DeleteShape"/> command, this event will tell you when a record has been marked for deletion in the ShapeFile.
        /// </remarks>
        public delegate void ShapeDeletedEventHandler(ShapeFileEventArgs e);
        internal virtual void onShapeRecordDeleted(ShapeFileEventArgs e) { if (ShapeRecordDeleted != null) ShapeRecordDeleted(e); }

        /// <summary>
        /// Event raised when a ShapeFile has been opened
        /// </summary>
        /// <remarks>
        /// OK ... Paranoia rules ... but sometimes you might want to log what ShapeFile has been opened and how.  This event is fired by the <see cref="Open(System.String)"/> method and will tell you just that.
        /// </remarks>
        public event ShapeOpenedEventHandler ShapeFileOpened;
        /// <summary>
        /// Raised when the ShapeFile has been opened
        /// </summary>
        /// <param name="e">The name of the opened ShapeFile, the Reading Mode  (Header Only, Fast Read) and whether the files are locked</param>
        public delegate void ShapeOpenedEventHandler(ShapeFileOpenEventArgs e);
        /// <summary>
        /// Event raised when a record in the ShapeFile has been modified
        /// </summary>
        internal virtual void onShapeFileOpened(ShapeFileOpenEventArgs e) { if (ShapeFileOpened != null) ShapeFileOpened(e); }

        /// <summary>
        /// Event raised when a ShapeFile has been modified
        /// </summary>
        /// <remarks>
        /// Fired during a <see cref="ModifyShape"/> command, this event will tell you what part of the ShapeFile has been changed - the fields or the vertices.
        /// It gets fired when the Vertices and/or Fields have been updated
        /// </remarks>
        public event ShapeModifiedEventHandler ShapeRecordModified;
        /// <summary>
        /// Raised when the ShapeFile record has been modified
        /// </summary>
        /// <param name="e">The record number of the modified record and the modified scope - Vertice or Field change</param>
        /// <remarks>
        /// Fired during a <see cref="ModifyShape"/> command, this event will tell you what part of the ShapeFile has been changed - the fields or the vertices.
        /// It gets fired when the Vertices and/or Fields have been updated
        /// </remarks>
        public delegate void ShapeModifiedEventHandler(ShapeFileModifyEventArgs e);
        internal virtual void onShapeRecordModified(ShapeFileModifyEventArgs e) { if (ShapeRecordModified != null) ShapeRecordModified(e); }

        /// <summary>
        /// Event raised when the ShapeFile has been packed
        /// </summary>
        /// <remarks>
        /// No event arguments with this one ... It just fires any time the <see cref="Pack"/> command is given.
        /// </remarks>
        public event ShapePackedEventHandler ShapeFilePacked;
        /// <summary>
        /// Raised when the ShapeFile has been packed
        /// </summary>
        /// <remarks>
        /// No event arguments with this one ... It just fires any time the <see cref="Pack"/> command is given.
        /// </remarks>
        public delegate void ShapePackedEventHandler();
        internal virtual void onShapePacked()
        {
            if (ShapeFilePacked != null)
                ShapeFilePacked();
        }

        #endregion

        #region **********          Open / Close FileStream       **********

        private void OpenStream(string Filename, ref FileStream WriteStream)
        {
            // Test to see if the FileStream is already open or is locked
            try
            {
                if (WriteStream == null)
                { WriteStream = new FileStream(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite); }
                else
                {
                    if (WriteStream.Name.ToUpper() != Filename.ToUpper())
                    {
                        WriteStream.Close();
                        WriteStream = new FileStream(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                }
            }
            catch
            { throw new Exception("The file " + Filename + " has been locked by another application"); }
        }

        private void CloseStream(ref FileStream WriteStream)
        {
            if (!mvarLockFile & WriteStream != null)
            {
                WriteStream.Close();
                WriteStream.Dispose();
                WriteStream = null;
            }
        }

        #endregion

        #region **********          Public Projection Methods     **********

        /// <summary>
        /// Writes out a projection PRJ file for use in ESRI applications.  Not all projections are supported,
        /// but included in the enums are those that are used in ArcGis 9
        /// </summary>
        /// <param name="Datum">The EPSG number of the Geo Coordinate System</param>
        public void WriteProjection(eGeocentricDatums Datum)
        {
            int EPSGNo = Convert.ToInt32(Datum);
            WritePrj(EPSGNo);
        }

        /// <summary>
        /// Writes out a projection PRJ file for use in ESRI applications.  Not all projections are supported,
        /// but included in the enums are those that are used in ArcGis 9
        /// </summary>
        /// <param name="Projection">The EPSG number of the projection</param>
        /// <remarks>
        /// ESRI have implemented a WKT version of the standard projection and datum codes.  This file has the same name as the shape file but has a .PRJ extension.  A complete list of projections and datums can be found at the <see href="http://www.SpatialReference.org">Spatial Reference</see> organisation website.  I've implemented all the listed codes in the Datum and Projection pulldowns.
        /// The simplist way is just to parse the EPSG number (if you know it) e.g. WriteProjection(2193).
        /// </remarks>
        public void WriteProjection(eGeographicDatums Projection)
        {
            int EPSGNo = Convert.ToInt32(Projection);
            WritePrj(EPSGNo);
        }

        /// <summary>
        /// Writes out a projection PRJ file for use in ESRI applications.  Not all projections are supported,
        /// but included in the enums are those that are used in ArcGis 9
        /// </summary>
        /// <param name="Projection">The EPSG number of the projection</param>
        public void WriteProjection(int Projection)
        {
            WritePrj(Projection);
        }

        private void WritePrj(int EPSGNo)
        {
            try
            {
                // Pull the data out of the DataTable
                System.Data.DataRow[] prjRow = mvarProjTable.Select("EPSG=" + EPSGNo.ToString());

                if (prjRow.Length == 1)
                {
                    // This will return 1 row that can be used to write the PRJ file
                    string filename = mvarShapeFile;
                    filename = filename.Substring(0, filename.Length - 3) + "prj";
                    StreamWriter prjWriter = new StreamWriter(filename);
                    prjWriter.Write(prjRow[0][3]);
                    prjWriter.Flush();
                    prjWriter.Close();
                    prjWriter.Dispose();
                    // Load the data back into the Projection class
                    LoadDatum(filename);
                }
            }
            catch { }
        }

        #endregion

        #region **********          Internal Functions            **********

        /// <summary>
        /// Gets the vertice angle (in degrees) of a line represented by two vertice points.
        /// </summary>
        /// <param name="FirstVerticeX">The first vertice x.</param>
        /// <param name="FirstVerticeY">The first vertice y.</param>
        /// <param name="LastVerticeX">The last vertice x.</param>
        /// <param name="LastVerticeY">The last vertice y.</param>
        /// <returns></returns>
        private double GetVerticeAngle(double FirstVerticeX, double FirstVerticeY, double LastVerticeX, double LastVerticeY)
        {
            double DiffE = 0;
            double DiffN = 0;
            double ZValue = 0;
            double b = 0;
            double CalcAngle = 0;

            // What is the angle of the vertice line
            // This is the StartAngle
            DiffN = LastVerticeY - FirstVerticeY;
            DiffE = LastVerticeX - FirstVerticeX;

            if (DiffE != 0 & DiffN != 0)
            {
                ZValue = DiffN / DiffE;
                b = System.Math.Atan(ZValue);
                if (DiffE < 0 & DiffN < 0)
                    CalcAngle = Convert.ToDouble((Math.PI + b));
                else if (DiffE < 0 & DiffN > 0)
                    CalcAngle = Convert.ToDouble((Math.PI + b));
                else if (DiffE > 0 & DiffN < 0)
                    CalcAngle = Convert.ToDouble(((Math.PI * 2) + b));
                else
                    CalcAngle = Convert.ToDouble(b);
            }
            else if (DiffN == 0)
            {
                if (DiffE > 0)
                    CalcAngle = 0;
                else
                    CalcAngle = Math.PI;
            }
            else
            {
                if (DiffN > 0)
                    CalcAngle = Math.PI / 2;
                else
                    CalcAngle = Math.PI * 1.5;
            }

            return CalcAngle;
        }

        private void ExtendLine(double AddLength, double VertAngle, ref double StartPointX, ref double StartPointY)
        {
                // Where along the line is the point to be placed
                StartPointX += System.Math.Cos(VertAngle) * AddLength;
                StartPointY += System.Math.Sin(VertAngle) * AddLength;
        }

        #endregion

    } //End Class

    #region Event Argument Classes

    /// <summary>
    /// ShapeFile Event Argument
    /// </summary>
    public class ShapeFileEventArgs : EventArgs
    {
        private int recno;

        /// <summary>
        /// Create a new event with parameters
        /// </summary>
        /// <param name="RecordNumber">The number of the record</param>
        public ShapeFileEventArgs(int RecordNumber)
        { recno = RecordNumber; }

        /// <summary>
        /// The number of the affected ShapeFile Record
        /// </summary>
        public int RecordNumber
        { get { return recno; } set { recno = value; } }

    }

    /// <summary>
    /// ShapeFile File Open Event Arguments
    /// </summary>
    public class ShapeFileOpenEventArgs : EventArgs
    {
        private string filename, readmode;
        private bool locked;
        /// <summary>
        /// Create a new event with parameters
        /// </summary>
        /// <param name="Filename">The name of the opened ShapeFile (.shp)</param>
        /// <param name="Mode">The ReadMode used to open the ShapeFile (HeaderOnly,FullRead or FastRead)</param>
        /// <param name="Locked">Has the file handle lock been set</param>
        public ShapeFileOpenEventArgs(string Filename, string Mode, bool Locked)
        {
            filename = Filename;
            readmode = Mode;
            locked = Locked;
        }

        /// <summary>
        /// The name and path of the opened ShapeFile
        /// </summary>
        public string Filename
        { get { return filename; } set { filename = value; } }
        /// <summary>
        /// The read mode of the opened ShapeFile
        /// </summary>
        public string Mode
        { get { return readmode; } set { readmode = value; } }
        /// <summary>
        /// Has the file handles to opened ShapeFile been locked open
        /// </summary>
        public bool Locked
        { get { return locked; } set { locked = value; } }

    }

    /// <summary>
    /// ShapeFile Modify Event Argument
    /// </summary>
    public class ShapeFileModifyEventArgs : EventArgs
    {
        private int recno;
        private string mod;
        /// <summary>
        /// This event fires when the shape file has been modified using the ModifyShape method
        /// </summary>
        /// <param name="RecordNumber">The record number of the modified record.  Thisis it's ordinal position in the file.</param>
        /// <param name="Modified">What collection has been modified - Fields or Vertices</param>
        public ShapeFileModifyEventArgs(int RecordNumber, string Modified)
        {
            recno = RecordNumber;
            mod = Modified;
        }

        /// <summary>
        /// The number of the affected ShapeFile Record
        /// </summary>
        public int RecordNumber
        { get { return recno; } set { recno = value; } }
        /// <summary>
        /// The name of the collection modified (Field or Vertice)
        /// </summary>
        public string Modified
        { get { return mod; } set { mod = value; } }

    }


    #endregion

} // End NameSpace
