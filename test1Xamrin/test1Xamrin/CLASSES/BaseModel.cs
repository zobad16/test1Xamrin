using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace xUtilityPCL
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseModel()
        {
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual BaseModel Clone()
        {
            Object myClone = Activator.CreateInstance(this.GetType());
            var r = this.GetType().GetRuntimeProperties();
            //PropertyInfo p = null;
            try
            {
                foreach (PropertyInfo pi in r)
                {
                    if (pi.Name.ToLower().Contains("original")) //20180129
                        continue;
                    pi.SetValue(myClone, pi.GetValue(this));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return (BaseModel)myClone;
        }

        public virtual void RejectChanges(BaseModel b)
        {
            var r = this.GetType().GetRuntimeProperties();
            //PropertyInfo p = null;
            foreach (PropertyInfo pi in r)
            {
                pi.SetValue(this, pi.GetValue(b));
            }
        }


        public enum RowStateEnum
        {
            Modified,
            Added,
            Deleted,
            Unchanged
        }

        public RowStateEnum RowState
        {
            get { return m_RowState; }
            set { m_RowState = value; }
        }

        private RowStateEnum m_RowState = RowStateEnum.Unchanged;

        #region "gps Coordinates"

        private double _map_latitude = 0;

        public double map_latitude
        {
            get { return _map_latitude; }
            set { _map_latitude = value; }
        }

        public double map_distanceFromCurrentPosition { get; set; }

        public double map_longitude { get; set; }

        public string map_Label { get; set; }

        public string map_indirizzo { get; set; }

        public string map_comune { get; set; }

        public string map_cap { get; set; }


        public string map_pk { get; set; }



        #endregion

        #region "proprietà di gestione sqlite"

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //public string StatusIMC { get; set; }

        #endregion


    }
}
