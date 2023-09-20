
using AnagrafeCaninaMobileFORMS.DROID;
using Android.Content.PM;
using Android.Graphics;
using Android.Views.InputMethods;
using Java.Util.Zip;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using Xamarin.Forms;
using xUtilityPCL;
using Android.App;
using Android.Content;
using static Android.Renderscripts.Sampler;
using static Java.Util.Jar.Attributes;
using Android.OS;
using Plugin.Messaging;

[assembly: Dependency(typeof(platformSpecific_Android))]
namespace AnagrafeCaninaMobileFORMS.DROID
{
    public class platformSpecific_Android : platformSpecific
    {


        //implementare25: riepilogo modifiche da riportare sui singoli progetti
        //1) nel MainActivity aggiungere attributo: LaunchMode = LaunchMode.SingleInstance
        //2) commentare la classe public class RegistrationIntentService : IntentService (parcheggi e manutenzioni)



        public static void NoOp()
        {
            //Nota: metterlo sempre per potere fare funzionare i dependency services
            //      in una class library a parte
        }

        public Object getHandler() //20171022
        {
            return null; //non chiamato
        }

        public SQLite.SQLiteConnection SQLITEGetConnection(string dbNameWithoutPath)
        {
            string path = getLocalDatabasePath();
            string pathName = System.IO.Path.Combine(path, dbNameWithoutPath);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(pathName, false); //20160409
            return conn;
        }
        //20160209


        public string getLocalDatabasePath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }



        public Boolean savefile(string path_Name, byte[] arrbytes)
        {
            FileInfo fi = new FileInfo(path_Name);
            DirectoryInfo di = (fi.Directory);

            if (di.Exists == false)
                di.Create();
            System.IO.File.WriteAllBytes(path_Name, arrbytes);


            if (System.IO.File.Exists(path_Name))
                return true;
            else
                return false;
        }





        public byte[] readfile(string path_Name)
        {
            return System.IO.File.ReadAllBytes(path_Name);

        }

        public Boolean FileExists(string filenameWithPath)
        {
            return File.Exists(filenameWithPath);
        }

        public Int64 FileSize(string filenameWithPath)
        {
            Int64 l = 0;

            using (var f = File.OpenRead(filenameWithPath))
            {
                l = f.Length;
                f.Close();
            }
            return l;
        }
        public DateTime FileDate(string filenameWithPath)
        {
            var d = File.GetCreationTimeUtc(filenameWithPath);
            return d;


        }

        public Boolean DirectoryExists(string directoryName, Boolean lCreateIfNOTExists)
        {
            var l = Directory.Exists(directoryName);

            if (lCreateIfNOTExists && l == false)
            {
                var x = new Java.IO.File(directoryName);
                x.Mkdir();
                return true;
            }
            else
                return Directory.Exists(directoryName);
        }

        public Boolean DeleteFile(string filenameWithPath)
        {
            try
            {
                File.Delete(filenameWithPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        public void hidekeyboard()
        {
            Activity c = Forms.Context as Activity;
            InputMethodManager imm = (InputMethodManager)c.GetSystemService(Context.InputMethodService);
            //imm.ToggleSoftInput (ShowFlags.Implicit, 0);
            Object v = c.CurrentFocus;
            if (v != null)
            {
                //20160605imm.HideSoftInputFromWindow (c.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);  
                imm.HideSoftInputFromWindow(c.CurrentFocus.WindowToken, HideSoftInputFlags.None);  //20160605
            }

        }



        public float getDensity()
        {
            return xUtilityAndroid.Measures.getDensity(Forms.Context);
        }

        public float getWidthDpi()
        {
            return xUtilityAndroid.Measures.getWidthDpi(Forms.Context);
        }



        public bool isiphone5()
        {
            return false;
        }








        public Tuple<double, double> GetImageSize(byte[] input)
        {
            var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray
                    (input, 0, input.Length);


            double imageWidth = bitmap.Width;
            double imageHeight = bitmap.Height;

            return new Tuple<double, double>(imageWidth, imageHeight);
        }


        public byte[] resizeImage(byte[] input, float width, float height, Boolean isjpg, string albumPath = "",
                                   Boolean setWhiteBackground = false, Boolean lTimeStamp = false, Int32 nQuality = 60)
        {
            Activity c = Xamarin.Forms.Forms.Context as Activity;



            try
            {

                var bitmap = Android.Graphics.BitmapFactory.DecodeByteArray
                    (input, 0, input.Length);

                if (bitmap == null)
                    return null;

                Boolean PictureTakenHorizontally = false;
                if (albumPath != "") //20170620 aggiunto IF
                {
                    Android.Media.ExifInterface myexif = new Android.Media.ExifInterface(albumPath);
                    string orient = myexif.GetAttribute(Android.Media.ExifInterface.TagOrientation);
                    //implementare25: il valore di ritorno èempre 0 (undefined) in quanto probabilmente
                    //                il plugin di montemagno NON setta la exitfinterface
                    //https://github.com/jamesmontemagno/MediaPlugin/issues/407
                    //https://github.com/jamesmontemagno/MediaPlugin/blob/master/src/Media.Plugin.Android/MediaImplementation.cs


                    if (Convert.ToInt64(orient) <= 4)
                        PictureTakenHorizontally = true;
                    else
                        PictureTakenHorizontally = false;
                }
                if (albumPath == "")
                    PictureTakenHorizontally = true;


                bool filter = true;

                float ratio = Math.Max((float)width / bitmap.Width,
                                  (float)width / bitmap.Height);
                int Newwidth = (int)Math.Round(ratio * (float)bitmap.Width);
                int Newheight = (int)Math.Round(ratio * (float)bitmap.Height);

                Matrix matrix = new Matrix();
                //PictureTakenHorizontally = true; //implementare25 non importa più decommentarla

                //20180111 montemagno plugin ritorna correttamente quindi forziamo la parte else che non fa la rotazione

                if (PictureTakenHorizontally == false)
                {
                    //baco Montemagno l'immagine arriva sempre orrizontale, occorre ruotarla
                    matrix.SetRotate(90, 0, 0);

                    Android.Graphics.Bitmap newBitmap = Android.Graphics.Bitmap.CreateScaledBitmap
                    (bitmap, Newwidth, Newheight, filter);


                    matrix.PostTranslate(newBitmap.Height, 0);

                    Bitmap bmRotated = Bitmap.CreateBitmap(newBitmap.Height, newBitmap.Width, newBitmap.GetConfig());
                    //20160611 inizio
                    if (setWhiteBackground)
                    {
                        bmRotated.EraseColor(Android.Graphics.Color.White);
                    }
                    //20160611 fine

                    Canvas tmpCanvas = new Canvas(bmRotated);
                    tmpCanvas.DrawBitmap(newBitmap, matrix, null);
                    //20160630 inizio
                    if (lTimeStamp)
                    {
                        string dataAttuale = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        Paint tPaint = new Paint();
                        tPaint.TextSize = 35;
                        tPaint.Color = Android.Graphics.Color.Blue;
                        tPaint.SetStyle(Paint.Style.Fill);
                        float hh = tPaint.MeasureText("yY");
                        tmpCanvas.DrawText(dataAttuale, 20, hh + 15, tPaint);
                    }
                    //20160630 fine


                    tmpCanvas.SetBitmap(null);
                    MemoryStream stream = new MemoryStream();
                    if (isjpg)
                        bmRotated.Compress(Bitmap.CompressFormat.Jpeg, nQuality, stream); //20160929 era 60
                    else
                        bmRotated.Compress(Bitmap.CompressFormat.Png, nQuality, stream); //20160929 era 60

                    byte[] byteArray = stream.ToArray();// .ToByteArray ();

                    newBitmap.Recycle();
                    bitmap.Recycle();
                    bmRotated.Recycle();
                    return byteArray;

                }
                else
                {
                    //l'immagine è stata scattata in posizione orrizontale: non importa ruotarla
                    Android.Graphics.Bitmap newBitmap = Android.Graphics.Bitmap.CreateScaledBitmap
                        (bitmap, Newwidth, Newheight, filter);

                    //20160611 inizio
                    //http://www.scriptscoop2.com/t/63fd5bd73429/java-android-bitmap-convert-transparent-pixels-to-a-color.html
                    if (setWhiteBackground || lTimeStamp)
                    {
                        Bitmap imageWithBG = Bitmap.CreateBitmap(newBitmap.Width, newBitmap.Height, newBitmap.GetConfig());
                        Canvas canvas = new Canvas(imageWithBG);
                        if (setWhiteBackground)
                        {
                            imageWithBG.EraseColor(Android.Graphics.Color.White); //20160611
                            canvas.DrawBitmap(newBitmap, 0f, 0f, null); // draw old image on the background
                        }
                        if (lTimeStamp)
                        {
                            canvas.DrawBitmap(newBitmap, 0f, 0f, null); // draw old image on the background
                            string dataAttuale = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            Paint tPaint = new Paint();
                            tPaint.TextSize = 35;
                            tPaint.Color = Android.Graphics.Color.Blue;
                            tPaint.SetStyle(Paint.Style.Fill);
                            float hh = tPaint.MeasureText("yY");
                            canvas.DrawText(dataAttuale, 20, hh + 15, tPaint);
                        }



                        newBitmap.Recycle();

                        MemoryStream stream2 = new MemoryStream();
                        if (isjpg)
                            imageWithBG.Compress(Bitmap.CompressFormat.Jpeg, nQuality, stream2);//20160929 era 60
                        else
                            imageWithBG.Compress(Bitmap.CompressFormat.Png, nQuality, stream2); //20160929 era 60

                        byte[] byteArray2 = stream2.ToArray();// .ToByteArray ();

                        newBitmap.Recycle();
                        bitmap.Recycle();
                        imageWithBG.Recycle();
                        return byteArray2;
                    }
                    //20160611 fine
                    else
                    {
                        MemoryStream stream = new MemoryStream();
                        if (isjpg)
                            newBitmap.Compress(Bitmap.CompressFormat.Jpeg, nQuality, stream); //20160929 era 60
                        else
                            newBitmap.Compress(Bitmap.CompressFormat.Png, nQuality, stream); //20160929 era 60

                        byte[] byteArray = stream.ToArray();// .ToByteArray ();

                        bitmap.Recycle();
                        return byteArray;
                    }


                    //
                    //http://www.scriptscoop2.com/t/63fd5bd73429/java-android-bitmap-convert-transparent-pixels-to-a-color.html
                    //					Bitmap imageWithBG = Bitmap.createBitmap(image.getWidth(), image.getHeight(),image.getConfig());  // Create another image the same size
                    //					imageWithBG.eraseColor(Color.WHITE);  // set its background to white, or whatever color you want
                    //					Canvas canvas = new Canvas(imageWithBG);  // create a canvas to draw on the new image
                    //					canvas.drawBitmap(image, 0f, 0f, null); // draw old image on the background
                    //					image.recycle();  // clear out old image 


                    //



                }

                //				MediaStore.Images.Media.InsertImage (c.ContentResolver, bmRotated, "Testimage", "");




            }
            catch
            {

                return null;

            }
        }





        public Size GetImageSize(string path)
        {
            //BitmapFactory.Options bmOptions = new BitmapFactory.Options();
            Bitmap bitmap = BitmapFactory.DecodeFile(path);
            Size s = new Size(bitmap.Width, bitmap.Height);
            return s;
        }



        //public async Task<string> sendEmailOffice365(Office365Params o365, string baseURL, string actionPath)
        public async Task<string> sendEmailNEW(MailParams oMail, string office365_baseURL, string office365_actionPath, Email_Type mode)
        {
            //await Task.Delay(1);
            RestClient client = null;
            RestRequest request = null;
            if (mode == Email_Type.Office365)
            {
                client = new RestClient(office365_baseURL);
                string controllerAction = office365_actionPath;// "AnagraficaClientis/UploadFile"
                request = new RestRequest(office365_actionPath, Method.Post);
            }

            if (oMail.Attachements.Count > 0)
            {
                String folder = DependencyService.Get<platformSpecific>().getLocalDatabasePath();
                if (oMail.Attachements.Count == 1)
                {
                    String fullPath = System.IO.Path.Combine(folder, oMail.Attachements[0].NomeFileSenzaPath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        //byte[] buffer = new byte[1024];
                        var newfileCompletePath = "/storage/emulated/0/Documents/" + oMail.Attachements[0].NomeFileSenzaPath;
                        var l = FileSize(fullPath);

                        if (!System.IO.Directory.Exists("/storage/emulated/0/Documents/"))
                        {
                            System.IO.Directory.CreateDirectory("/storage/emulated/0/Documents");
                        }
                        File.Copy(fullPath, newfileCompletePath, true); //20171002
                        using (FileStream fos = new FileStream(newfileCompletePath, FileMode.Open))
                        {
                            var arr = new byte[l];
                            fos.Read(arr, 0, Convert.ToInt32(l));

                            fos.Close();
                            //var s = Convert.ToBase64String(arr);
                            oMail.Attachements[0].FileArrayBytes = arr;
                        }
                    }
                }
                else
                {
                    String fullPath = System.IO.Path.Combine(folder, oMail.Attachements[0].NomeFileSenzaPath);
                    if (System.IO.File.Exists(fullPath))
                    {

                        byte[] buffer = new byte[1024];


                        if (!System.IO.Directory.Exists("/storage/emulated/0/Documents/"))
                        {
                            System.IO.Directory.CreateDirectory("/storage/emulated/0/Documents");
                        }
                        //do il nome al file zip (nome del primo file senza estensione + ".zip"
                        var nomezip = oMail.Attachements[0].NomeFileSenzaPath.Substring(0, oMail.Attachements[0].NomeFileSenzaPath.Length - 4) + ".zip";
                        var newfileCompletePath = "/storage/emulated/0/Documents/" + nomezip;
                        FileStream fos = new FileStream(newfileCompletePath, FileMode.Create);
                        ZipOutputStream zos = new ZipOutputStream(fos);
                        byte[] arrzip = null;

                        foreach (var at in oMail.Attachements)
                        {
                            ZipEntry ze = new ZipEntry(at.NomeAllegato);
                            zos.PutNextEntry(ze);
                            fullPath = System.IO.Path.Combine(folder, at.NomeFileSenzaPath);
                            Java.IO.FileInputStream myin = new Java.IO.FileInputStream(System.IO.Path.Combine(folder, fullPath));
                            int len;
                            while ((len = myin.Read(buffer)) > 0)
                            {
                                zos.Write(buffer, 0, len);
                            }
                            myin.Close();
                        }
                        zos.CloseEntry();
                        zos.Close();
                        fos.Close();
                        //rieleggo lo zip per ricavare l'array di bytes
                        var l = FileSize(newfileCompletePath);
                        using (FileStream myzip = new FileStream(newfileCompletePath, FileMode.Open))
                        {
                            var arr = new byte[l];
                            myzip.Read(arr, 0, Convert.ToInt32(l));
                            myzip.Close();
                            oMail.Attachements.Clear();
                            oMail.Attachements.Add(new Attachement() { FileArrayBytes = arr, NomeAllegato = nomezip, NomeFileSenzaPath = nomezip });
                        }


                    }
                }




            }

            if (mode == Email_Type.Office365)
            {
                var par = JsonConvert.SerializeObject(oMail);
                request.AddParameter( "application/json",par, ParameterType.RequestBody);
                try
                {
                    //var r = client.Execute<List<string>>(request);
                    var r = await client.ExecuteAsync<List<string>>(request);
                    return r.Data[0];
                }
                catch (Exception ex)
                {
                    //await DisplayAlert("Messaggio", "Errore spedizione db.", "OK");
                    return ex.Message;
                }

            }
            if (mode == Email_Type.Normale)
            {
                var emailMessenger = CrossMessaging.Current.EmailMessenger;
                if (emailMessenger.CanSendEmail)
                {

                    if (oMail.Attachements.Count > 0) //sempre 1 file in chiaro o uno zip
                    {
                        String folder = "/storage/emulated/0/Documents/";//DependencyService.Get<platformSpecific>().getLocalDatabasePath();

                        String fullPathInput = System.IO.Path.Combine(folder, oMail.Attachements[0].NomeFileSenzaPath);
                        var email = new EmailMessageBuilder()
                        .To(oMail.Recipients)
                        .Cc(oMail.CCs)
                        .Bcc(oMail.BCCs)
                        //.Cc("cc.plugins@xamarin.com")
                        //.Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
                        .Subject(oMail.Subject)
                        .Body(oMail.Body)
                        // Alternatively use EmailBuilder fluent interface to construct more complex e-mail with multiple recipients, bcc, attachments etc. 
                        .WithAttachment(fullPathInput, "image/jpeg")
                        .Build();
                        emailMessenger.SendEmail(email);
                    }
                    else
                    {
                        // Send simple e-mail to single receiver without attachments, bcc, cc etc.
                        //emailMessenger.SendEmail("to.plugins@xamarin.com", "Xamarin Messaging Plugin", "Well hello there from Xam.Messaging.Plugin");
                        var email = new EmailMessageBuilder()
                        .To(oMail.Recipients)
                        .Cc(oMail.CCs)
                        .Bcc(oMail.BCCs)
                        //.Cc("cc.plugins@xamarin.com")
                        //.Bcc(new[] { "bcc1.plugins@xamarin.com", "bcc2.plugins@xamarin.com" })
                        .Subject(oMail.Subject)
                        .Body(oMail.Body)
                        .Build();
                        emailMessenger.SendEmail(email);
                    }
                }
            }
            if (mode == Email_Type.MailGun)
            {
                /*
				RestClient client3 = new RestClient();
				client3.BaseUrl = new Uri("https://api.mailgun.net/v3");
				client3.Authenticator =
				new HttpBasicAuthenticator("api",
										  "key-254d37b7b2c671845a765e7d0bf8ab50");
				RestRequest request3 = new RestRequest();
				request3.AddParameter("domain", "sandbox4f20cf856f5c437ca07529032964dfdc.mailgun.org", ParameterType.UrlSegment);
				request3.Resource = "{domain}/messages";
				request3.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox4f20cf856f5c437ca07529032964dfdc.mailgun.org>");
				request3.AddParameter("to", "Alessandro Facchini <alfacchini@alice.it>");
				request3.AddParameter("subject", "Hello Alessandro Facchini");
				request3.AddParameter("text", "Congratulations Alessandro Facchini, you just sent an email with Mailgun!  You are truly awesome!");
				request3.Method = Method.POST;
				var cc = client3.Execute(request3);
				*/




                RestResponse resp2 = null;
                var options = new RestClientOptions("https://api.mailgun.net/v3");
                options.Authenticator = new HttpBasicAuthenticator("api", oMail.mailgun_Key);//"key-c813baf3a313eb22a6d63d1dec8c6ba8"); //"key-254d37b7b2c671845a765e7d0bf8ab50");
                RestClient client2 = new RestClient(options);
                           
                RestRequest request2 = new RestRequest();
                request2.AddParameter("domain", oMail.mailgun_Domain, ParameterType.UrlSegment);//"sandboxee693156474e40f889e7494451b846f3.mailgun.org", ParameterType.UrlSegment);
                                                                                                //"sandbox4f20cf856f5c437ca07529032964dfdc.mailgun.org"
                request2.Resource = "{domain}/messages";
                request2.AddParameter("from", "Mailgun Sandbox <postmaster@" + oMail.mailgun_Domain + ">"); // sandboxee693156474e40f889e7494451b846f3.mailgun.org>");
                var rec = "";
                foreach (var s in oMail.Recipients)
                {
                    rec += "alessandro " + "<" + s + ">" + ";";
                }
                request2.AddParameter("to", rec.TrimEnd(";".ToCharArray()));


                //request2.AddParameter("to", "Alessandro Facchini <alfacchini@alice.it>");
                //nota: se non si registrano domini è possibile mandare la mail solo all'utente registrato; pertanto si ignorano eventuali cc e bcc
                //request2.AddParameter("cc", "anagrafe@alterbit.it"); //darebbe errore
                request2.AddParameter("subject", oMail.Subject);//oMail.Subject);
                var myBody = oMail.Body;
                if (myBody == "") myBody = "-";
                request2.AddParameter("html", myBody);
                if (oMail.Attachements.Count > 0) //sempre 1 file in chiaro o uno zip
                {
                    String folder = "/storage/emulated/0/Documents/";//DependencyService.Get<platformSpecific>().getLocalDatabasePath();
                    String fullPathInput = System.IO.Path.Combine(folder, oMail.Attachements[0].NomeFileSenzaPath);
                    request2.AddFile("attachment", fullPathInput);
                }

                request2.Method = Method.Post;
                try
                {
                    resp2 = await client2.ExecuteAsync(request2);
                    //resp2 = client2.Execute(request2);
                    var s = resp2.Content;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            }

            return "";
        }








        public string getApplicationVersion()
        {
            PackageInfo packageInfo = null;
            string pckName = Android.App.Application.Context.PackageName;
            PackageManager manager = Android.App.Application.Context.PackageManager;
            /*if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                packageInfo = manager.GetPackageInfo(pckName, PackageManager..PackageInfoFlags.Of(0));
            else
            {
                #pragma warning disable CS0618 // Type or member is obsolete
                packageInfo = manager.GetPackageInfo(pckName, 0);
                #pragma warning restore CS0618 // Type or member is obsolete

            }*/
        #pragma warning disable CS0618 // Type or member is obsolete
            packageInfo = manager.GetPackageInfo(pckName, 0);
        #pragma warning restore CS0618 // Type or member is obsolete
            return packageInfo.VersionName;
        }







        public string Device_getDeviceID()
        {
            //http://codeworks.it/blog/?p=260
            return Android.Provider.Settings.Secure.GetString(Forms.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }





    }
}
