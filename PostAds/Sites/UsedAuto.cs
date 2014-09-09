namespace Motorcycle.Sites
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Config.Data;
    using HTTP;
    using POST;
    using Interfaces;

    internal class UsedAuto : IPostOnSite
    {
        public async Task PostMoto(DicHolder data)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;

                    const string url = "http://usedauto.com.ua/add/add.php";
                    const string urlFile = "http://usedauto.com.ua/add/modules/picturesUpload.php";
                    const string referer = "http://usedauto.com.ua/add/sale.php";

                    var cookieContainer = Cookies.GetCookiesContainer(referer);

                    //Upload fotos
                    var photoId = string.Empty;

                    foreach (
                        var requestFile in
                            fileDictionary.Select(fotoPath => Request.POSTRequest(urlFile, cookieContainer,
                                new Dictionary<string, string> {{"photos", ""}},
                                new Dictionary<string, string> {{fotoPath.Key, fotoPath.Value}})))
                    {
                        requestFile.Referer = referer;
                        var responseFileString = Response.GetResponseString(requestFile);
                        requestFile.Abort();

                        //Get file id
                        var start = responseFileString.IndexOf("value=\"") + "value=\"".Length;
                        var end = responseFileString.IndexOf("\"", start);
                        if (photoId == string.Empty)
                        {
                            photoId = responseFileString.Substring(start, end - start);
                            dataDictionary["main_photo"] = photoId;
                        }
                        photoId += "," + responseFileString.Substring(start, end - start);
                    }
                    dataDictionary["photos"] = photoId;
                    //==============End upload fotos==============//

                    //Dictionary to NameValueCollection
                    var valueCollection = new NameValueCollection();
                    foreach (var value in dataDictionary)
                        valueCollection.Add(value.Key, value.Value);

                    //Post advert's data            
                    var responseByte = new WebClient().UploadValues(url, "POST", valueCollection);
                    var responseString = Encoding.Default.GetString(responseByte);
                });
        }

        public async Task PostSpare(DicHolder data)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;
                });
        }

        public async Task PostEquip(DicHolder data)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var dataDictionary = data.DataDictionary;
                    var fileDictionary = data.FileDictionary;
                });
        }
    }
}