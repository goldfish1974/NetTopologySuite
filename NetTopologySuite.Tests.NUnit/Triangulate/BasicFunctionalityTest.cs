namespace NetTopologySuite.Tests.NUnit.Triangulate
{
    using System;

    using GeoAPI.Geometries;
    using GeoAPI.Operations.Buffer;

    using global::NUnit.Framework;

    using NetTopologySuite.Geometries;
    using NetTopologySuite.Geometries.Utilities;
    using NetTopologySuite.IO;
    using NetTopologySuite.Operation.Buffer;
    using NetTopologySuite.Tests.NUnit.Operation.Buffer;
    using NetTopologySuite.Triangulate;

    public class BasicFunctionalityTest
    {
        private const string NTS = "POLYGON ((0 0, 0 140, 220 140, 220 0, 0 0), "+
            "(10 120, 10.722892984206778 123.73299952016072, "+
            "12.78705708350404 126.92628720768715, 15.894058348765082 129.11818200940593, "+
            "19.594690896674148 129.99178285046074, 23.353922466029093 129.42078574705238, "+
            "26.628248631330855 127.48774465919216, 28.94427190999916 124.47213595499957, "+
            "60 62.36067977499789, 60 120, 60.76120467488713 123.8268343236509, " +
            "62.928932188134524 127.07106781186548, 66.1731656763491 129.23879532511287, " +
            "70 130, 73.8268343236509 129.23879532511287, 75 128.45491108143625, " +
            "76.1731656763491 129.23879532511287, 80 130, 110 130, 140 130, " +
            "143.8268343236509 129.23879532511287, 147.07106781186548 127.07106781186548, " +
            "149.23879532511287 123.8268343236509, 150 120, 149.7563920050868 118.77529990657533, " +
            "151.2698653266467 120.57898717483984, 153.9303097578365 122.81136511581886, " +
            "161.4494964168583 127.15256955749211, 164.713014640189 128.3403930497698, " +
            "173.2635182233307 129.84807753012208, 175 130, 200 130, 203.8268343236509 " +
            "129.23879532511287, 207.07106781186548 127.07106781186548, 209.23879532511287 " +
            "123.8268343236509, 210 120, 209.23879532511287 116.1731656763491, " +
            "207.07106781186548 112.92893218813452, 203.8268343236509 110.76120467488713, " +
            "200 110, 175.87488663525926 110, 169.9224599701969 108.950426578261, " +
            "165.4573462044979 106.37249194367239, 162.14321732110722 102.42286694057128, " +
            "160.3798061746948 97.5779346345886, 160.3798061746948 92.42206536541137, " +
            "162.1432173211072 87.57713305942873, 165.4573462044979 83.62750805632763, " +
            "169.9224599701969 81.049573421739, 176.7364817766693 79.84807753012208, " +
            "185.286985359811 78.3403930497698, 188.55050358314173 77.1525695574921, " +
            "196.06969024216352 72.81136511581883, 198.73013467335326 70.57898717483982, " +
            "204.31107952580075 63.92787609686536, 206.04756130247006 60.92020143325668, " +
            "209.0171200331643 52.76140587492995, 209.62019382530522 49.34120444167325, " +
            "209.62019382530522 40.658795558326744, 209.0171200331643 37.23859412507005, " +
            "206.04756130247006 29.079798566743296, 204.31107952580075 26.07212390313459, " +
            "198.73013467335326 19.421012825160147, 196.06969024216346 17.18863488418115, " +
            "188.55050358314165 12.847430442507887, 185.28698535981098 11.659606950230206, " +
            "176.7364817766693 10.15192246987792, 175 10, 150 10, 146.1731656763491 10.761204674887136, " +
            "142.92893218813452 12.928932188134524, 140.76120467488713 16.173165676349104, 140 20, " +
            "140.76120467488713 23.8268343236509, 142.92893218813452 27.071067811865476, " +
            "146.1731656763491 29.238795325112868, 150 30, 174.12511336474077 30, " +
            "180.0775400298031 31.049573421738998, 184.5426537955021 33.62750805632761, " +
            "187.8567826788928 37.57713305942872, 189.62019382530522 42.4220653654114, " +
            "189.62019382530522 47.57793463458859, 187.85678267889278 52.42286694057128, " +
            "184.54265379550213 56.372491943672365, 180.07754002980312 58.95042657826101, " +
            "173.26351822333072 60.15192246987792, 164.713014640189 61.65960695023021, " +
            "161.4494964168583 62.8474304425079, 153.9303097578365 67.18863488418117, " +
            "151.26986532664674 69.42101282516016, 145.68892047419925 76.07212390313462, " +
            "143.95243869752994 79.07979856674335, 140.9828799668357 87.2385941250701, " +
            "140.3798061746948 90.65879555832676, 140.3798061746948 99.34120444167326, " +
            "140.98287996683572 102.76140587492995, 143.91633489196772 110.82100704271807, " +
            "143.8268343236509 110.76120467488713, 140 110, 120 110, 120 20, 119.23879532511287 " +
            "16.1731656763491, 117.07106781186548 12.928932188134524, 113.8268343236509 10.761204674887132, " +
            "110 10, 106.1731656763491 10.761204674887132, 102.92893218813452 12.928932188134524, " +
            "100.76120467488713 16.1731656763491, 100 20, 100 110, 80 110, 80 20, " +
            "79.27710701579322 16.267000479839275, 77.21294291649596 13.073712792312847, " +
            "74.10594165123493 10.881817990594055, 70.40530910332586 10.008217149539268, " +
            "66.64607753397091 10.579214252947615, 63.37175136866915 12.512255340807839, " +
            "61.05572809000084 15.52786404500042, 30 77.63932022500211, 30 20, " +
            "29.238795325112868 16.1731656763491, 27.071067811865476 12.928932188134524, " +
            "23.8268343236509 10.761204674887132, 20 10, 16.173165676349104 10.761204674887132, " +
            "12.928932188134524 12.928932188134524, 10.761204674887132 16.1731656763491, 10 20, 10 120))";

        private const string NTSConstraint =
            "LINESTRING (34.96311725379413 119.71656675537787, 36.33949290421136 123.21301759477318, 38.833111746463445 126.15913961700457, 42.10104411784998 128.17950514098408, 45.62680512024284 128.97429931913373, 48.86523935166572 128.46699609373036, 51.37305058722211 126.78826491337418, 52.913352510662726 124.07652241981722, 71.51430966977843 62.07333188943154, 71.58557468430577 61.898068584789755, 71.6885749824677 61.739364877120096, 71.81962357298151 61.60290171295524, 71.97402944990398 61.49356391792834, 72.14626551154349 61.41526533990515, 72.3301664081956 61.370808749166606, 72.51914923655872 61.36178551064503, 72.70644918086838 61.388518619539106, 72.8853616657569 61.450051139400976, 73.0494823527523 61.54418045656465, 73.19293638951862 61.667537124746765, 73.31058870564866 61.81570547750397, 73.39822782727582 61.9833816911121, 73.45271663069049 62.16456363985971, 84.96266759044103 119.7143184386124, 86.39267375611483 123.29908111485405, 88.99138032337366 126.29236665973563, 92.379229221886 128.28920586249174, 95.99222721694183 128.98037925968512, 99.24624027434227 128.30629924879958, 100.08027016838044 127.66305832079102, 100.2481994170519 127.5582821767598, 100.43347613107491 127.48863440466162, 100.62884148878229 127.45684368391986, 100.82664141287113 127.46415552006795, 101.01912644393245 127.51028344799377, 101.19875535088681 127.59342025514793, 102.379229221886 128.28920586249174, 106.09479148765456 129, 165.89751124235505 129, 169.24624027434228 128.30629924879958, 171.68248053533338 126.42736246543772, 173.00392016194328 123.60485088634755, 173.0002089080676 120.1926197650456, 172.58262735223013 119.14581954027332, 172.52861267315424 118.95976368170453, 172.51148824660902 118.76678409190568, 172.5318968271233 118.57412413782899, 172.58907239152435 118.38901518910187, 172.68086889113374 118.21840519313767, 172.80384080234822 118.06869788832023, 172.95337245219076 117.94551244373035, 173.12385126467905 117.85347254718532, 173.30887842531814 117.79603285803859, 173.5015090565589 117.77534933872403, 173.69451288938797 117.79219833206844, 173.88064564690634 117.84594742175415, 174.0529209536847 117.9345791696658, 174.20487256495008 118.05476683915518, 176.0274123531918 119.80872741575247, 179.0169380544606 121.95675542672845, 187.27270523871917 126.22981447461491, 190.62722780352024 127.36791397035991, 199.3601372862591 128.85530799283035, 201.04291289743358 129, 225.89751124235505 129, 229.24624027434228 128.30629924879958, 231.68248053533338 126.42736246543772, 233.00392016194328 123.60485088634755, 233.0002089080676 120.1926197650456, 231.6073262438852 116.70091888514597, 229.00861967662635 113.70763334026437, 225.62077077811404 111.71079413750827, 221.90520851234547 111, 197.87488663525926 111, 197.70698401761933 110.9858035864155, 191.54464266820918 109.9362301646765, 191.39498170231658 109.898663558353, 191.25288181566862 109.83851975703315, 186.2721811230519 107.26058512244454, 186.03842401468418 107.09302501109254, 181.93437013067327 103.14340000799143, 181.75676292136703 102.91410068613993, 179.02436531375807 98.06916838015725, 178.95790332750042 97.9259474553801, 178.91481242592158 97.77405076972678, 177.88363857208614 92.61818150054955, 177.86437124731216 92.43950027195835, 177.87739722447986 92.26025592707228, 178.67182190969572 87.41532362108964, 178.72473418227617 87.21962420986532, 178.81602824804017 87.03861768595124, 181.34023213081065 83.08899268285013, 181.4723513200029 82.92380731772346, 181.63625931960553 82.79010670938109, 185.58578615838684 80.21217207479246, 185.76188975660165 80.12073493179787, 185.95258054133055 80.065869158836, 192.52630316947955 78.86437326721908, 200.67990454621784 77.37411894555302, 203.5202351236928 76.25916084787417, 209.9965923430923 72.03191382225073, 212.060772767794 69.95056978668008, 216.19787296247281 63.47724388746478, 217.2608762100313 60.66022390801355, 218.56747165609775 52.69173223686542, 218.49077573277683 49.45192905095087, 216.78734560339095 40.934778404021344, 215.5560168700324 37.66281334044694, 211.02873102214144 29.6352871115467, 208.78019010815055 26.742716023438703, 201.97258764680805 20.191272584247397, 198.98306194553948 18.04324457327161, 190.72729476128077 13.770185525385084, 187.37277219647973 12.632086029640105, 178.63986271374102 11.144692007169667, 176.95708710256628 11, 152.10248875764492 11, 148.75375972565774 11.693700751200426, 146.31751946466662 13.57263753456227, 144.99607983805672 16.395149113652458, 144.9997910919324 19.80738023495439, 146.3926737561148 23.299081114854026, 148.99138032337365 26.292366659735624, 152.379229221886 28.289205862491716, 156.09479148765456 29, 180.12511336474077 29, 180.2930159823807 29.0141964135845, 186.45535733179082 30.063769835323498, 186.60501829768342 30.101336441647003, 186.74711818433138 30.16148024296685, 191.7278188769481 32.73941487755546, 191.9615759853158 32.90697498890746, 196.06562986932673 36.85659999200856, 196.243237078633 37.085899313860075, 198.97563468624193 41.930831619842756, 199.04209667249958 42.07405254461991, 199.08518757407842 42.225949230273216, 200.11636142791386 47.381818499450404, 200.13562875268784 47.56049972804161, 200.12260277552014 47.73974407292768, 199.32817809030425 52.58467637891037, 199.2752658177238 52.7803757901347, 199.1839717519598 52.96138231404877, 196.65976786918938 56.91100731714985, 196.52764867999713 57.07619268227653, 196.3637406803945 57.2098932906189, 192.4142138416132 59.787827925207544, 192.23811024339838 59.87926506820214, 192.04741945866948 59.934130841164006, 185.47369683052048 61.13562673278091, 177.32009545378216 62.62588105444699, 174.47976487630726 63.740839152125815, 168.00340765690765 67.96808617774933, 165.93922723220606 70.04943021331984, 161.80212703752719 76.52275611253522, 160.7391237899687 79.33977609198647, 159.43252834390225 87.30826776313462, 159.5092242672232 90.54807094904913, 161.21265439660908 99.06522159597866, 162.44398312996768 102.33718665955308, 166.95156408836579 110.32977329714943, 167.03219470981728 110.51384866012485, 167.07439257179698 110.71032862555269, 167.07645351211406 110.91127834288316, 167.03829429961885 111.10858245047328, 166.96145599549587 111.29427281476202, 166.8490417174517 111.46085032363402, 166.70559132019375 111.601587738335, 166.53689805323253 111.71080137325363, 166.34977460028972 111.78408063177159, 166.1517779488181 111.81846612836847, 165.95090420078856 111.81256920351358, 165.75526564982104 111.76662800473166, 165.64363383186435 111.71516788323204, 161.90520851234547 111, 142 111, 141.805056240167 110.98081442205046, 141.61759269773412 110.92399386100436, 141.44480256551344 110.83171858746786, 141.2933160006062 110.70752931034748, 141.16894571761108 110.55619131575659, 141.0764639480461 110.38351161747909, 141.01941932430907 110.19611613513818, 123.03733240955896 20.285681561387577, 121.60732624388517 16.70091888514593, 119.0086196766264 13.707633340264396, 115.62077077811402 11.710794137508286, 112.00777278305814 11.019620740314881, 108.75375972565774 11.693700751200423, 106.31751946466663 13.572637534562265, 104.99607983805672 16.39514911365246, 104.9998923089778 19.900442031296276, 122.98058067569092 109.80388386486182, 122.99999956774097 109.99907020546684, 122.98094367549487 110.19429231973143, 122.92414617061327 110.38203907567268, 122.83179232578966 110.55508695423097, 122.7074354368185 110.70677797272795, 122.55586011024687 110.83127584942444, 122.3828981765901 110.92379055329873, 122.19520431181364 110.98076259953638, 122 111, 102 111, 101.80505624016702 110.98081442205046, 101.61759269773414 110.92399386100436, 101.44480256551344 110.83171858746786, 101.2933160006062 110.70752931034748, 101.16894571761108 110.55619131575659, 101.07646394804611 110.38351161747909, 101.01941932430908 110.19611613513818, 83.03688274620588 20.283433244622177, 81.66050709578862 16.786982405226798, 79.1668882535366 13.84086038299547, 75.89895588215005 11.820494859015922, 72.37319487975715 11.025700680866278, 69.13476064833431 11.53300390626963, 66.6269494127779 13.211735086625815, 65.08664748933724 15.923477580182837, 46.48569033022157 77.92666811056846, 46.414425315694224 78.10193141521025, 46.3114250175323 78.2606351228799, 46.18037642701849 78.39709828704476, 46.02597055009601 78.50643608207166, 45.8537344884565 78.58473466009485, 45.6698335918044 78.62919125083339, 45.480850763441275 78.63821448935497, 45.29355081913162 78.61148138046089, 45.11463833424308 78.54994886059902, 44.950517647247686 78.45581954343534, 44.80706361048137 78.33246287525323, 44.689411294351345 78.18429452249603, 44.601772172724175 78.0166183088879, 44.5472833693095 77.83543636014029, 33.03733240955895 20.28568156138755, 31.607326243885176 16.700918885145953, 29.008619676626374 13.707633340264385, 25.620770778114018 11.710794137508286, 22.007772783058144 11.019620740314881, 18.753759725657755 11.69370075120042, 16.31751946466661 13.572637534562277, 14.996079838056724 16.39514911365245, 14.999892308977806 19.90044203129625, 34.96311725379413 119.71656675537787)";

        private WKTReader _wktReader;

        [SetUp]
        public void SetUp()
        {
            _wktReader = new WKTReader();
        }

        [Test /*, ExpectedException() */]
        public void TestInvertedItalicNTS()
        {
            var atb = new AffineTransformationBuilder(
                new Coordinate(0, 0),
                new Coordinate(50, 0),
                new Coordinate(0, 100),
                new Coordinate(0, 0),
                new Coordinate(50, 0),
                new Coordinate(20, 100));

            var geom = _wktReader.Read(NTS);

            //Apply italic effect
            geom = atb.GetTransformation().Transform(geom);
            Console.WriteLine(geom.AsText());

            //Setup 
            var dtb = new DelaunayTriangulationBuilder();
            dtb.SetSites(geom);
            var result = dtb.GetEdges(geom.Factory);
            Console.WriteLine(result.AsText());
        }

        [Test, ExpectedException("LocateFailureException") ]
        [Ignore("Currently failing with LocateFailureException, but why is it taking so long?")]
        public void TestInvertedItalicNTSConforming()
        {
            var atb = new AffineTransformationBuilder(
                new Coordinate(0, 0), new Coordinate(50, 0), new Coordinate(0, 100),
                new Coordinate(0, 0), new Coordinate(50, 0), new Coordinate(20, 100));

            var geom = _wktReader.Read(NTS);
            
            //Apply italic effect
            geom = atb.GetTransformation().Transform(geom);
            Console.WriteLine(geom.AsText());

            var constraint = (IGeometry)((IPolygon)geom).GetInteriorRingN(0);
            constraint = geom.Factory.CreatePolygon((ILinearRing)constraint, null);
            constraint = ((IPolygon)constraint.Buffer(-1)).Shell;
            var coordinates = constraint.Coordinates;
            coordinates[coordinates.Length - 1].X -= 1e-7;
            coordinates[coordinates.Length - 1].Y -= 1e-7;

            constraint = geom.Factory.CreateLineString(coordinates);
            Console.WriteLine(constraint.AsText());

            //Setup 
            var dtb = new ConformingDelaunayTriangulationBuilder { Sites = geom, Constraints = constraint };
            var result = dtb.GetEdges(geom.Factory);
            Console.WriteLine(result.AsText());


        }
    }
}