using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public static class Global
    {

        public static Boolean WorkAlwaysOnlineIfInternetAvailable { get; set; }

        public static string BaseURL { get; set; }

        public static string LoginDatabaseName = "alterbit.db";


        public static Boolean IsPasswordEncrypted { get; set; }

        public static Boolean IsLoggedOffline { get; set; }

        public static Boolean IsLogged { get; set; }

        public static Int32 IDUserLogged { get; set; }

        public static Int32? ID1 { get; set; }

        public static Int32? ID2 { get; set; }

        public static Int32? ID3 { get; set; }

        public static double k_leadingspace = 1.50;

        //sintassi cs6 public static Int32 ggValiditaLogin{ get; set; }= 30;

        //private static double _fontSizeLabel = 12;

        //public static double FontSizeLabel{ get { return _fontSizeLabel; } set { _fontSizeLabel = value; } }

        private static Int32 _ggValiditaLogin = 5; //20170921 30;

        public static Int32 ggValiditaLogin { get { return _ggValiditaLogin; } set { _ggValiditaLogin = value; } }


        private static Color _k_coloreBackgroundPagina = Color.FromRgb(211, 215, 221);

        public static Color k_coloreBackgroundPagina { get { return _k_coloreBackgroundPagina; } set { _k_coloreBackgroundPagina = value; } }

        private static Color _k_coloreForeground = Color.FromRgb(113, 113, 113);

        public static Color k_coloreForeground { get { return _k_coloreForeground; } set { _k_coloreForeground = value; } }

        public static Color k_grigioLight = Color.FromRgb(240, 242, 244);

        public static string k_nessunValore = "Nessun valore";
        public static Int32 k_maxNumberForListView = 50000;
        //20150515


        public static Boolean LastFormIsMenu;
        //20150719

    }
}
