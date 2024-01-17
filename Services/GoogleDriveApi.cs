using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BiDegree.Models;
using BiDegree.Shared;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static MudBlazor.Colors;

namespace BiDegree.Services
{
    public class GoogleDriveAPI : IGoogleDriveApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public GoogleDriveAPI(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<DriveFileList> GetDriveFileList(string folderId)
        {
            //await Task.Yield();
            //return GetFileList();

            var gApiKey = await _localStorage.GetItemAsync<string>(Constants.KeyName_DriveApiKey);

            // see https://developers.google.com/drive/api/v2/reference/files/list

            var url = $"https://www.googleapis.com/drive/v2/files?q='{folderId}'+in+parents&maxResults=1000&key={gApiKey}";
            var driveFileList = await _httpClient.GetFromJsonAsync<DriveFileList>(url);

            return driveFileList;
        }

        public DriveFileList GetFileList()
        {
            var files = new List<string> { "DSC03735.JPG", "DSC03762.JPG", "DSC03764.JPG", "DSC02428.JPG", "DSC01145.JPG", "bche_006.jpg", "img_112.jpg", "img_331.jpg", "20160111_110423.jpg", "IMG_0440.JPG", "IMG_0495.JPG", "20190105_121013.jpg", "20191231_130405.jpg", "20210524_203508.jpg", "Copy of usa95045.jpg", "Copy of usa95131.jpg", "Copy of usa95294.jpg", "Copy of J&M.jpg", "Copy of Jennie23-111Retouched.jpg", "us in galveston - 2.jpg", "Lady bird lake.jpg", "20180207_123647.jpg", "20171029_124634.jpg", "20180826_194529.jpg", "20180830_181517.jpg", "20191118_143930.jpg", "20191123_160831.jpg", "20191110_050534.jpg", "20191112_072951.jpg", "20191126_122213.jpg", "20191117_165319.jpg", "20191109_101050.jpg", "20191126_171440.jpg", "DSC_7509.jpg", "20191113_055355.jpg", "DSC_7514.jpg", "20191109_054548.jpg", "20191109_054053.jpg", "DSC_7516.jpg", "20191109_054828.jpg", "DSC_7524.jpg", "DSC_7661.jpg", "IMG_5114.jpg", "DSC_7569.jpg", "IMG_5630.jpg", "IMG_5679.jpg", "20191231_123831.jpg", "Vera Dad Me Marcelo at Butterfly center.jpg", "20200105_103855.jpg", "Scan_0004.jpg", "Scan_0005.jpg", "Scan_0007.jpg", "Scan_0020.jpg", "20190312_135133.jpg", "IMG_4208.jpg", "DSC01103.jpg", "escanear0103.jpg", "escanear0057.jpg", "20140109_211349.jpg", "20160923_224642.jpg", "20180623_081636.jpg", "20180620_142525.jpg", "20171103_164656.jpg", "DSCN0494.jpg", "DSCN0453.jpg", "DSCN0475.jpg", "IMG_7497.jpg", "IMG_7499.jpg", "20191128_165312.jpg", "20191129_124737.jpg", "IMG-20191128-WA0005.jpg", "P5010026.jpg", "20180210_173143.jpg", "20180106_165018.jpg", "20180106_174431.jpg", "20180620_142331.jpg", "20180618_181116.jpg", "IMG_3184.jpg", "a332432a-efb4-4e4f-accc-75a0addf4a68.jpg", "IMG_3125.jpg", "20180208_150831.jpg", "20180208_144523.jpg", "20180210_173158.jpg", "20180208_144845.jpg", "20180209_002833.jpg", "20210601_170046.mp4", "20210601_170656.jpg", "20210523_100324.jpg", "20160819_172547.jpg", "20160819_140633.jpg", "20180208_144419.jpg", "IMG_2265.jpg", "IMG_2237.jpg", "IMG_2263.jpg", "20180211_162914.jpg", "IMG_2177.jpg", "WhatsApp Image 2018-02-08 at 21.38.30.jpg", "20170720_175929.jpg", "IMG_0528.jpg", "IMG_0249.jpg", "20170718_123408.jpg", "20170717_113315.jpg", "IMG_0407.jpg", "20210606_082906.jpg", "20171104_125029.jpg", "20171209_151916.jpg", "20160918_193929.jpg", "20171126_092211.jpg", "20160918_193701.jpg", "20180325_183545.jpg", "20180402_180758.jpg", "20190216_101000.jpg", "20190216_103644.jpg", "20191110_055531.jpg", "20191116_220204.jpg", "20181227_091423.jpg", "20180216_160146.jpg", "20181229_124158.jpg", "20190216_100822.jpg", "20180829_161424.jpg", "20180211_145445.jpg", "20180826_130722.jpg", "20191109_101054.jpg", "20180825_105347.jpg", "20191110_052433.jpg", "20190216_103012.jpg", "20191116_150926.jpg", "20201006_112451.jpg", "20201223_165630.jpg", "20201006_115908.jpg", "DSC_8335.jpg", "DSC_8394.jpg", "20191118_174806.jpg", "20210124_102059.jpg", "20191231_094456.jpg", "20210214_084141.jpg", "20200209_161543.jpg", "20210606_113420.jpg", "20210124_113747.jpg", "20210531_113017.jpg", "20201005_122651.jpg", "20160815_203146.jpg", "IMG-20171104-WA0014.jpg", "FullSizeRender.jpg", "IMG-20160815-WA0005.jpg", "IMG_3136.jpg", "IMG-20160814-WA0003.jpg", "Vera-Can 2016-12-21 at 14.03.35.jpg", "9A1F6769-C707-4F18-A40A-58E9381EF299.jpg", "MarceloMate.png", "usa95416.jpg", "DSCN0579.jpg", "IMG-20171104-WA0002.jpg", "DSCN0011.jpg", "20160816_191920.jpg", "IMG_5624.jpg", "20191231_124557.jpg", "20191231_130306(0).jpg", "20191225_142457.jpg", "usa95428.jpg", "20171103_164338.jpg", "img075.jpg", "20160816_211520.jpg", "20180206_121815.jpg", "20180620_145811.jpg", "20180211_162847.jpg", "20160815_210543.jpg", "20180621_195055.jpg", "20190216_101042.jpg", "20180826_084846.jpg", "20170704_194755.jpg", "DSC_7564.jpg", "20191109_054835.jpg", "20191126_164851.jpg", "img001.jpg", "IMG_3167.MOV", "DSC_8375.jpg", "usa95113.jpg", "DSC_8402.jpg", "DSC_7614.jpg", "20201006_112947.jpg", "20201006_115424.jpg", "DSCN0378.jpg", "20201223_141201.jpg", "20191124_154331.jpg", "20191110_101056.jpg", "20191124_190812.jpg", "20201005_143257.jpg", "20201005_122326.jpg", "20210620_151926.jpg", "20160815_182946.jpg", "20210531_114558.jpg", "20210531_113243.mp4", "20210616_172640.jpg", "IMG-20210625-WA0004.jpg", "IMG_3463.jpg", "IMG_5518.jpg", "DSCN1108.jpg", "DSC02305.jpg", "DSC02272.jpg", "20200120_130400.jpg", "20191117_172231.jpg", "20191115_113633.jpg", "20201006_091816.jpg", "Scan_0001.jpg", "20191123_134733.jpg", "20191118_145949.jpg", "20171029_152411.jpg", "20210711_111345.jpg", "20180205_103518.jpg", "20201020_180728.jpg", "20201005_123244.jpg", "20171126_105516.jpg", "IMG_2200.jpg", "20190406_173947.jpg", "20160612_152650.jpg", "20160104_221949.jpg", "DSC03779.jpg", "bche 002.jpg", "bche 028.jpg", "20180211_134413.jpg", "DSC01173.jpg", "DSC01237 copia 2.jpg", "bche_042.jpg", "20180210_145749.jpg", "20180210_142244.jpg", "DSC01228.jpg", "DSC01184.jpg", "20171020_131745.jpg", "20170718_154806.jpg", "20170716_115425.jpg", "usa95246.jpg", "20171020_130903.jpg", "Scan_0006.jpg", "20210411_154542.jpg", "20181229_124353.jpg", "IMG_3186.jpg", "20171209_151823.jpg", "20170720_180125.jpg", "20201005_161236.jpg", "DSCN0297.jpg", "20210814_164153.jpg", "20210814_172550.jpg", "20210814_174305.mp4", "20210814_193634.jpg", "20210815_125555.jpg", "20210814_134119.mp4", "20210814_134214.jpg", "20210814_155358.jpg", "20210814_163233.jpg", "20210814_163724.jpg", "20210814_182630.jpg", "IMG_8471.jpg", "img542.jpg", "escanear0112.jpg", "Bariloche_Night.jpg", "img057.jpg", "DSC00165.JPG", "Chicos 001.jpg", "20210925_145041.jpg", "20210925_163419.jpg", "20210925_181714.jpg", "20210925_143916.jpg", "20210925_144031.jpg", "20210925_143250.jpg", "20210925_144943.jpg", "20210925_144033.jpg", "20211003_124603.jpg", "20211003_122320.jpg", "20210926_111355.jpg", "20211003_132933.jpg", "20211003_130535.jpg", "20211003_132513.jpg", "20211003_130318.jpg", "20211003_124519.jpg", "20211003_124329.jpg", "20160822_080755.jpg", "20190515_143250.jpg", "004.jpg", "013.jpg", "115.jpg", "bche 001.jpg", "DSCN0961.jpg", "escanear0003.jpg", "escanear0021.jpg", "escanear0029.jpg", "escanear0098.jpg", "Familia Mama (39).jpg", "feb 2011 062.jpg", "P5210075.jpg", "Potrero20080105.jpg", "20210606_092445.jpg", "20211030_153520.jpg", "Scan_0021.jpg", "Horacio.jpg", "img067.jpg", "usa95102.jpg", "usa95131.jpg", "20180825_110613.jpg", "20180826_083026.jpg", "20180207_145132.jpg", "20180630_115152.jpg", "20180825_172817.jpg", "20181229_124312.jpg", "20181229_170441.jpg", "20181230_150316.jpg", "20190312_140741.jpg", "20190829_055625.jpg", "20191126_170816.jpg", "20191129_124707.jpg", "20191130_150936.jpg", "20201005_121904.jpg", "20201005_133902.jpg", "20201005_162512.jpg", "20201005_190347.jpg", "20201006_115918.jpg", "20210516_085604.jpg", "20210523_100249.jpg", "DSC_8371.jpg", "20211126_183206.jpg", "20211126_182123.jpg", "20211128_154019.jpg", "20211128_153945.jpg", "20211128_153813.jpg", "3D7D59F6-FC76-4D87-90F3-DC82DC67F70D.JPG", "20211207_171256.jpg", "20211231_100051.jpg", "20211230_080436.jpg", "20211230_075434.jpg", "20211210_185427.jpg", "20211230_080114.jpg", "20211229_162529.jpg", "20211231_100327.jpg", "20211230_080336.jpg", "20211204_144801.jpg", "20211204_144547.jpg", "IMG_9387.jpg", "IMG_9403.jpg", "20211230_101608.jpg", "20220104_172022.jpg", "E28281C2-1A3A-428D-833A-1A11BED403BD.jpg", "IMG_9492.jpg", "DSC02522.JPG", "verita_blue.jpg", "20220418_131510.jpg", "20220420_113407.jpg", "20220418_124001.jpg", "20220421_114124.jpg", "20220422_170340.jpg", "20191113_055553.jpg", "20201224_083231.jpg", "20220420_113608.jpg", "WhatsApp Image 2022-06-16 at 8.29.50 AM.jpeg", "20220703_100246.mp4", "20220702_212823.jpg", "20220703_095405.jpg", "2ABC70AF-EB4A-434C-8B33-7EE84B7697A8.jpg", "0D2978DA-540E-4EFF-A251-25A3F7FBE949.jpg", "20210925_143421.jpg", "20220820_153057.mp4", "Walker.jpg", "20220820_102637.jpg", "IMG_0784.jpg", "IMG_0779.jpg", "IMG_0753.jpg", "IMG_0765.jpg", "IMG_1092.jpg", "20221002_181033.mp4", "20221003_111154.mp4", "imagejpeg_0.jpg", "0c897438-1e69-4c0b-a4e9-a1f0c24b036f.jpg", "828cbbd4-9c6c-4e12-bc31-4a0f4c7bed60.jpg", "IMG_1454.jpg", "IMG_1461.jpg", "IMG_1466.jpg", "IMG_1280.jpg", "IMG_1365.jpg", "IMG_1182.jpg", "IMG_1138.jpg", "IMG_1095.jpg", "IMG_1036.jpg", "IMG_1110.jpg", "IMG_1005.jpg", "IMG_1023.jpg", "IMG_1009.jpg", "IMG_0996.jpg", "IMG_1008.jpg", "IMG_1902.jpg", "IMG_1840.jpg", "IMG_1449.jpg", "IMG_1445.jpg", "IMG_1631.jpg", "20221207_170429.jpg", "20221207_170448.jpg", "IMG_1543.jpg", "IMG_1573.jpg", "IMG_1571.jpg", "IMG_1591.jpg", "20221204_104322.jpg", "20221204_104813.jpg", "IMG_1603.jpg", "20221210_115940.jpg", "IMG_1630.jpg", "20221120_123345.jpg", "20221120_124440.jpg", "20221210_122527.jpg", "430150401_0_102_9.jpg", "20221210_115350.jpg", "20221207_170429 (1).jpg", "20191222_112442.jpg", "20210124_143926.jpg", "20200105_103815.jpg", "20220820_153304.jpg", "20180616_104944.jpg", "20180826_154648.jpg", "20190111_203226.jpg", "20190110_162920.jpg", "img046.jpg", "DSC01668.JPG", "DSC01615.jpg", "20220212_114841.jpg", "20210207_100551.jpg", "IMG_3105.JPG", "20161130_223408.jpg", "20200105_151611.jpg", "20160517_181128.jpg", "20160111_111524.jpg", "20230225_114013.mp4", "20230225_115602.jpg", "IMG_1943.jpg", "20191124_153736.jpg", "20210502_142404.jpg", "20191109_095845.jpg", "20191113_055304.jpg", "20221218_171830.jpg", "20230610_105714.jpg", "20180620_152359.jpg", "20180620_140423.jpg", "20160819_174202.jpg", "20171126_133110.jpg", "20191112_065218.jpg", "IMG_0167 (1).jpg", "IMG_0167.jpg", "10593045_10204858785177208_1700442117893737783_n.jpg", "20211230_112205.jpg", "20211229_164834.jpg", "20211231_094458.jpg", "20211229_152746.jpg", "20211229_162454.jpg", "20200104_175948.jpg" };
            HashSet<Item> randomFiles = new HashSet<Item>();

            int maxRandomNumber = files.Count;

            for (int i = 0; i < maxRandomNumber; i++)
            {
                Random rnd = new();
                int rndPosition = rnd.Next(1, maxRandomNumber);

                randomFiles.Add(new Item
                {
                    downloadUrl = $"/localphotos/PhotoFrame/{files[rndPosition]}"
                });
            }


            DriveFileList driveFileList = new()
            {
                items = randomFiles.ToArray()
            };

            return driveFileList;
        }

    }
}
