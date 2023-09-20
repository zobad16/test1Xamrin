using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace xUtilityPCL
{
    public class sqliteHelper
    {
        public sqliteHelper()
        {
        }

        internal async static Task<Boolean> sqliteTableExist(SQLiteConnection conn, string tableName)
        {
            await Task.Delay(1);
            try
            {
                Int32 x = conn.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='"
                          + tableName + "'");
                //				conn.Dispose (); //20160428
                if (x == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        internal static Boolean sqliteTableExistSYNC(SQLiteConnection conn, string tableName)
        {

            try
            {
                Int32 x = conn.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='"
                          + tableName + "'");
                //				conn.Dispose ();//20160428
                if (x == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }


        public async static Task<Boolean> sqlite_TableExist<T>(string databasename)
        {
            Type BaseModelType = typeof(T);
            string tableName = BaseModelType.Name;
            using (SQLiteConnection cn = await creaDataBaseORGetConnection(databasename))
            {
                await Task.Delay(1);
                try
                {
                    Int32 x = cn.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='"
                              + tableName + "'");
                    cn.Close(); //20160428
                    if (x == 0)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public static Boolean sqlite_TableExistSYNC<T>(string databasename)
        {
            Type BaseModelType = typeof(T);
            string tableName = BaseModelType.Name;
            using (SQLiteConnection cn = creaDataBaseORGetConnectionSYNC(databasename))
            {
                try
                {
                    Int32 x = cn.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='"
                              + tableName + "'");
                    cn.Close();//20160428
                    if (x == 0)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        //20161106 inizio
        public static Int32 sqlite_ColumnExistSYNC<T>(string databasename, string columnName, string alterTableCommand = "")
            where T : BaseModel, new()
        {
            Type BaseModelType = typeof(T);
            var strT = BaseModelType.Name;
            string tableName = BaseModelType.Name;
            using (SQLiteConnection cn = creaDataBaseORGetConnectionSYNC(databasename))
            {
                try
                {
                    //var x = cn.Table<T> ().Table.FindColumn (columnName);
                    var x = cn.GetTableInfo(strT).ToList().FirstOrDefault(y => y.Name == columnName);
                    if (x == null)
                    {
                        if (alterTableCommand == "")
                        {
                            cn.Close();//20160428
                            return 0;
                        }
                        else
                        {
                            cn.Execute(alterTableCommand);
                            cn.Close();//20160428
                            return 0;
                        }
                    }
                    else
                    {
                        cn.Close();//20160428
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return -1;
                }
            }
        }
        //20161106 fine



        public static async Task<SQLiteConnection> creaDataBaseORGetConnection(string databasename)
        {
            await Task.Delay(1);
            String folder = DependencyService.Get<platformSpecific>().getLocalDatabasePath();
            //String nomeFile = "alterbit_" + Global.IDUserLogged.ToString () + ".db";
            String nomeFile = databasename;
            String fullPath = System.IO.Path.Combine(folder, nomeFile);
            SQLiteConnection cn = DependencyService.Get<platformSpecific>().SQLITEGetConnection(nomeFile);
            cn.BusyTimeout = new TimeSpan(0, 0, 60); //20161030
            return cn;
        }

        public static SQLiteConnection creaDataBaseORGetConnectionSYNC(string databasename)
        {

            String folder = DependencyService.Get<platformSpecific>().getLocalDatabasePath();
            //String nomeFile = "alterbit_" + Global.IDUserLogged.ToString () + ".db";
            String nomeFile = databasename;
            String fullPath = System.IO.Path.Combine(folder, nomeFile);
            SQLiteConnection cn = DependencyService.Get<platformSpecific>().SQLITEGetConnection(nomeFile);
            cn.BusyTimeout = new TimeSpan(0, 0, 60); //20161030
            return cn;
        }

        public static async Task<Boolean> sqlite_AggiornaLocalTableFromCollection<T>(
            string databasename, MyObservableCollection<T> l)
            where T : BaseModel, new()
        {
            return await _sqlite_AggiornaLocalTable<T>(databasename, l, true);
        }

        public static async Task<Boolean> sqlite_AggiornaLocalTableFromCollection<T>(
            string databasename, List<T> l)
            where T : BaseModel, new()
        {
            MyObservableCollection<T> o = new MyObservableCollection<T>(l);
            return await _sqlite_AggiornaLocalTable<T>(databasename, o, true);
        }

        private static async Task<Boolean> _sqlite_AggiornaLocalTable<T>(
            string databasename, MyObservableCollection<T> l, Boolean lPreserveChanges)
            where T : BaseModel, new()
        {
            try
            {
                using (SQLiteConnection cn = await creaDataBaseORGetConnection(databasename))
                {
                    cn.BeginTransaction();
                    foreach (var o in l)
                    {
                        var oFound = cn.Find<T>(x => x.Id == o.Id);
                        if (oFound != null)
                        {
                            if (lPreserveChanges == false)
                                o.RowState = BaseModel.RowStateEnum.Unchanged;
                            if (oFound.RowState == BaseModel.RowStateEnum.Added && o.RowState == BaseModel.RowStateEnum.Deleted)
                            { //20141230 introdotto IF
                              //riga presente su db locale in added da portare in stato di deleted
                                cn.Delete(o);
                            }
                            else
                            {
                                cn.Update(o);
                            }
                        }
                        else
                            cn.Insert(o);
                    }
                    cn.Commit();
                    cn.Close(); //20160427
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

        }


        public static Boolean sqlite_AggiornaLocalTableFromCollectionSYNC<T>(
            string databasename, List<T> l)
            where T : BaseModel, new()
        {
            MyObservableCollection<T> o = new MyObservableCollection<T>(l);
            return _sqlite_AggiornaLocalTableSYNC<T>(databasename, o, true);
        }

        private static Boolean _sqlite_AggiornaLocalTableSYNC<T>(
            string databasename, MyObservableCollection<T> l, Boolean lPreserveChanges)
            where T : BaseModel, new()
        {
            try
            {
                using (SQLiteConnection cn = creaDataBaseORGetConnectionSYNC(databasename))
                {
                    cn.BeginTransaction();
                    foreach (var o in l)
                    {
                        var oFound = cn.Find<T>(x => x.Id == o.Id);
                        if (oFound != null)
                        {
                            if (lPreserveChanges == false)
                                o.RowState = BaseModel.RowStateEnum.Unchanged;
                            if (oFound.RowState == BaseModel.RowStateEnum.Added && o.RowState == BaseModel.RowStateEnum.Deleted)
                            { //20141230 introdotto IF
                              //riga presente su db locale in added da portare in stato di deleted
                                cn.Delete(o);
                            }
                            else
                            {
                                cn.Update(o);
                            }
                        }
                        else
                            cn.Insert(o);
                    }

                    cn.Commit();
                    cn.Close(); //20160427
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ", ex.ToString());
                return false;
            }
        }


        public static Boolean sqlite_CreaLocaTableFromCollectionSYNC_ServerSide<T>(List<T> l, SQLiteConnection cn)
            where T : BaseModel, new()
        {
            MyObservableCollection<T> oarr = new MyObservableCollection<T>(l);
            cn.CreateTable<T>();
            try
            {
                cn.BeginTransaction();

                foreach (var o in oarr)
                {
                    cn.Insert(o);

                }
                cn.Commit();
                //cn.Dispose (); //20160427
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }
        }



        private static async Task<Boolean> _sqlite_DeleteLocalTable<T>(
            string databasename, MyObservableCollection<T> l)
            where T : BaseModel, new()
        {
            try
            {


                using (SQLiteConnection cn = await creaDataBaseORGetConnection(databasename))
                {
                    foreach (var o in l)
                    {
                        var oFound = cn.Find<T>(x => x.Id == o.Id);
                        if (oFound != null)
                            cn.Delete(o);
                    }
                    cn.Close();//20160428
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ", ex.ToString());
                return false;
            }
        }

        public static async Task<MyObservableCollection<T>> sqlite_ReadFromExistingLocalTable<T>(
            string databasename)
            where T : BaseModel, new()
        {
            using (SQLiteConnection cn = await creaDataBaseORGetConnection(databasename))
            {
                System.Diagnostics.Debug.WriteLine("db path: " + cn.DatabasePath);
                Type BaseModelType = typeof(T);
                await Task.Delay(1);
                var lTable = await sqliteHelper.sqliteTableExist(cn, BaseModelType.Name);
                if (lTable == false)
                {
                    cn.CreateTable<T>();
                }
                var tq = cn.Table<T>().Where(a => 1 == 1);

                MyObservableCollection<T> o = new MyObservableCollection<T>(tq);
                cn.Close(); //20160428
                return o;
            }


        }

        public static async Task<TableQuery<T>> sqlite_GetTableQueryFromExistingLocalTable<T>(
            string databasename)
            where T : BaseModel, new()
        {
            //20160508 non è possibile usare Using
            SQLiteConnection cn = await creaDataBaseORGetConnection(databasename);
            System.Diagnostics.Debug.WriteLine("db path: " + cn.DatabasePath);
            Type BaseModelType = typeof(T);
            await Task.Delay(1);
            var lTable = await sqliteHelper.sqliteTableExist(cn, BaseModelType.Name);
            if (lTable == false)
            {
                cn.CreateTable<T>();
            }
            var tq = cn.Table<T>(); //20150305 .Where (a => 1 == 1);
                                    //cn.Close (); //20160428
            return tq;


        }

        public static TableQuery<T> sqlite_GetTableQueryFromExistingLocalTableSYNC<T>(
            string databasename)
            where T : BaseModel, new()
        {
            //20160508 non è possibile usare Using
            SQLiteConnection cn = creaDataBaseORGetConnectionSYNC(databasename);
            System.Diagnostics.Debug.WriteLine("db path: " + cn.DatabasePath);
            Type BaseModelType = typeof(T);

            var lTable = sqliteHelper.sqliteTableExistSYNC(cn, BaseModelType.Name);
            if (lTable == false)
            {
                cn.CreateTable<T>();
            }
            var tq = cn.Table<T>().Where(a => 1 == 1);
            //cn.Close ();//20160428
            return tq;

        }


        public static async Task<Int32> sqlite_GetCountFromExistingLocalTable<T>(
            string databasename)
            where T : BaseModel, new()
        {
            using (SQLiteConnection cn = await creaDataBaseORGetConnection(databasename))
            {
                ;
                System.Diagnostics.Debug.WriteLine("db path: " + cn.DatabasePath);
                Type BaseModelType = typeof(T);
                await Task.Delay(1);
                var lTable = await sqliteHelper.sqliteTableExist(cn, BaseModelType.Name);
                if (lTable == false)
                {
                    cn.CreateTable<T>();
                }
                var ii = cn.Table<T>().Count();
                cn.Close();//20160428
                return ii;
            }
        }

        public static MyObservableCollection<T> sqlite_ReadFromExistingLocalTableSYNC<T>(
            string databasename)
            where T : BaseModel, new()
        {
            using (SQLiteConnection cn = creaDataBaseORGetConnectionSYNC(databasename))
            {
                System.Diagnostics.Debug.WriteLine("db path: " + cn.DatabasePath);
                Type BaseModelType = typeof(T);

                var lTable = sqliteHelper.sqliteTableExistSYNC(cn, BaseModelType.Name);
                if (lTable == false)
                {
                    cn.CreateTable<T>();
                }
                var tq = cn.Table<T>().Where(a => 1 == 1);
                MyObservableCollection<T> o = new MyObservableCollection<T>(tq);
                cn.Close();//20160428
                return o;
            }
        }



        public static async Task<Int32> sqlite_SincronizzaExistingLocalTable<T>(
            string databasename, string baseURL, string controllerAction, BaseModel bmPerTest, Boolean lDropTableAfterSync = true,
            Action<MyObservableCollection<T>> myCommandBefore = null, string valoreOK = null,
            Action<MyObservableCollection<T>> myCommandAfter = null,
            Action<RestRequest> myrequest = null
        )//20180621 aggiunto parametro myrestRequest

            //20170531 //20171101 aggiunto parametroValoreOK //20171112 aggiunto parametro myCommandAfter
            where T : BaseModel, new()
        {
            using (SQLiteConnection cn = await creaDataBaseORGetConnection(databasename))
            {
                System.Diagnostics.Debug.WriteLine("db path: " + cn.DatabasePath);
                Type BaseModelType = typeof(T);
                await Task.Delay(1);
                var lTable = await sqliteHelper.sqliteTableExist(cn, BaseModelType.Name);
                var nRecDaSincronizzare = 0;
                if (lTable == true)
                {
                    if (bmPerTest != null)
                        cn.Insert(bmPerTest);
                    nRecDaSincronizzare = cn.Table<T>().Where(a => a.RowState != BaseModel.RowStateEnum.Unchanged).Count();
                    //bool salvataggioOK = true;//Not used
                    MyObservableCollection<T> o = null;
                    if (nRecDaSincronizzare > 0)
                    {
                        var tq = cn.Table<T>().Where(a => a.RowState != BaseModel.RowStateEnum.Unchanged);
                        o = new MyObservableCollection<T>(tq);
                        //20170531 inizio
                        if (myCommandBefore != null)
                        {
                            myCommandBefore(o);
                        }
                        //20170531 fine
                        nRecDaSincronizzare = o.Count; //20171120
                        if (o.Count > 0) //20170531
                        {
                            var lOK = await sqlite_PerformPOSTFromCollection(o, baseURL, controllerAction, valoreOK, myrequest);

                            //20171101 passato parametro valoreOK
                            //if (lErrore == false) {
                            //	lErrore = await _sqlite_DeleteLocalTable (databasename, o);
                            //}
                            if (lOK == false)
                                return -1;
                        }
                    }
                    if (lDropTableAfterSync == true)
                    {
                        cn.DropTable<T>();
                        cn.CreateTable<T>();
                    }
                    else
                    {
                        //20171120var tq = cn.Table<T>().Where(a => a.RowState != BaseModel.RowStateEnum.Unchanged);
                        //20171120MyObservableCollection<T> o = new MyObservableCollection<T>(tq);
                        if (o != null)
                        {
                            foreach (BaseModel bm in o) //riscorro la stessa lista
                            {
                                if (bm.RowState == BaseModel.RowStateEnum.Deleted)
                                    cn.Delete(bm);
                                else
                                {
                                    bm.RowState = BaseModel.RowStateEnum.Unchanged;
                                    cn.Update(bm);
                                }
                            }
                        }

                        //20171112 inizio
                        if (myCommandAfter != null)
                        {
                            myCommandAfter(o);
                        }
                        //20171112 fine

                    }
                }
                else
                {
                    //creo la tabella T
                    cn.CreateTable<T>();
                }
                cn.Close();//20160428
                return nRecDaSincronizzare;
            }
        }

        public static async Task<Boolean> sqlite_SincronizzaLocalTableFromCollection<T>(
            string databasename, string baseURL, string controllerAction,
            MyObservableCollection<T> o, Boolean lPreserveChanges)
            where T : BaseModel, new()
        {
            //20160428 SQLiteConnection cn = await creaDataBaseORGetConnection (databasename);
            //System.Diagnostics.Debug.WriteLine ("db path: " + cn.DatabasePath);
            Type BaseModelType = typeof(T);
            await Task.Delay(1);
            var lOK = await sqlite_PerformPOSTFromCollection(o, baseURL, controllerAction);
            if (lOK)
            {
                lOK = await _sqlite_AggiornaLocalTable(databasename, o, lPreserveChanges);
            }
            else
            {
                lOK = await _sqlite_AggiornaLocalTable(databasename, o, true); //20150610
            }
            return lOK;
        }


        public static async Task<Boolean> sqlite_PerformPOSTFromCollection<T>
        (MyObservableCollection<T> o, string baseURL, string controllerAction,
          string valoreOK = null,
          Action<RestRequest> myrequest = null
        ) //20171101 //20180621 aggiunto parametro myrestRequest
            where T : BaseModel, new()
        {

            //List<MyObservableCollection<T>> r = new List<MyObservableCollection<T>> (){ o };

            //string url = "http://appserver.anagrafecaninarer.it/xDaswebapics/api/";
            var client = new RestClient(baseURL);
            client.HttpClientFactory = new HttpClientFactoryCustom(); //20171022

            RestRequest request = null;
            //request = new RestRequest ("tblATInterventis/postInterventiBindableGeneric", HttpMethod.Post);
            request = new RestRequest(controllerAction, Method.POST); //20160208
                                                                      //20160208 request = new RestRequest (controllerAction, HttpMethod.Post);
                                                                      //AnagraficaClientis
                                                                      //20180621 inizio
            if (myrequest != null)
            {
                myrequest(request);
            }
            //20180621 fine
            Parameter p1 = new Parameter();
            p1.Name = "application/json";
            p1.Value = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            p1.Type = ParameterType.RequestBody;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization-Token",
                                        "57,46,60,70,93,230,85,33,98,19,10,46,84,91,218,43,207,42,159,167,5,25,157,4,224,142,235,8,160,199,123,100,107,58,37,204,133,81,138,196,237,190,56,119,158,7,224,89,84,85,208,169,44,179,102,218,55,60,76,134,144,22,208,230,165,179,83,125,86,57,224,42,29,58,188,45,73,33,160,87,165,105,131,139,132,137,209,67,92,36,168,73,176,205,251,48,240,228,14,39,197,36,42,21,216,242,172,4,160,234,138,77,156,28,191,63,111,207,221,31,103,213,58,62,186,123,221,230"); //20171211

            request.AddParameter(p1);
            //MyObservableCollection<T> r = Newtonsoft.Json.JsonConvert.DeserializeObject<MyObservableCollection<T>> (p1.Value.ToString ());




            try
            {
                //var n = await client.Execute<Int32> (request);
                var n = await client.Execute<string>(request);
                if (n.StatusCode != System.Net.HttpStatusCode.OK)
                { //20161204
                    return false; //20161204
                }
                //20171101 inizio
                if (valoreOK != null)
                {
                    if (n.Data != valoreOK)
                        return false;
                    else
                        return true;
                }
                //20171101 fine
                Debug.WriteLine("Terminato");
                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ",ex.ToString());
                return false;
            }




        }


        public static async Task<Int32> sqlite_PerformPOSTFromByteArray(ByteArrayJson o, string baseURL, string controllerAction)
        {
            var client = new RestClient(baseURL);
            client.HttpClientFactory = new HttpClientFactoryCustom(); //20171022
            RestRequest request = null;
            request = new RestRequest(controllerAction, Method.POST);
            Parameter p1 = new Parameter();
            p1.Name = "application/json";
            p1.Value = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            p1.Type = ParameterType.RequestBody;
            request.AddHeader("Accept", "application/json");
            request.AddParameter(p1);


            try
            {
                var n = await client.Execute<string>(request);
                if (n.StatusCode != System.Net.HttpStatusCode.OK)
                { //20161204
                    return -1; //20161204
                }
                Debug.WriteLine("Terminato");
                return o.Images.Count;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore: ", ex.ToString());

                return -1;
            }




        }


        public static void DuplicaFile(string Path_Name_Orig, string Path_Name_Dest, string Path_Dest)
        {
            try
            {


                var K_currentPlatform = DependencyService.Get<platformSpecific>();

                var l = K_currentPlatform.DirectoryExists(Path_Dest, false);

                if (l)
                {
                    if (K_currentPlatform.FileExists(Path_Name_Orig))
                    {
                        var b1 = K_currentPlatform.readfile(Path_Name_Orig);

                        K_currentPlatform.savefile(Path_Name_Dest, b1);
                        b1 = null;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}

