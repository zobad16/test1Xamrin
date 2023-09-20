using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xUtilityPCL
{
    public interface platformSpecificxx
    {
    }

    public interface platformSpecific
    {

        Object getHandler(); //20171022



        float getDensity();



        bool isiphone5();




        Task<string> sendEmailNEW(MailParams oMail, string office365_baseURL, string office365_actionPath, Email_Type mode);







        void hidekeyboard();



        string getLocalDatabasePath();





        Boolean savefile(string path_Name, byte[] arrbytes);


        byte[] readfile(string path_Name);

        Boolean FileExists(string filenameWithPath);
        Int64 FileSize(string filenameWithPath);

        DateTime FileDate(string filenameWithPath);

        Boolean DirectoryExists(string directoryName, Boolean lCreateIfNOTExists);

        Boolean DeleteFile(string filenameWithPath);

        SQLite.SQLiteConnection SQLITEGetConnection(string dbNameWithoutPath);
        //20160209
        //20160209		SQLite.Net.SQLiteConnection SQLITEGetConnection (string dbNameWithoutPath);



        Tuple<double, double> GetImageSize(byte[] input);

        byte[] resizeImage(byte[] input, float width, float heightm, Boolean isjpg, string albumPath = "",
                            Boolean setWhiteBackground = false, Boolean lTimeStamp = false, Int32 nQuality = 60);






        string getApplicationVersion();



        string Device_getDeviceID();



    }

    public enum Email_Type
    {
        Normale,
        Office365,
        MailGun
    }



}