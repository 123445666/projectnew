namespace PHBC.DAO
{
    using System.Data.Entity.Core.Objects.DataClasses;
    using System;
    using System.Data.Entity;
    public partial class DB_PHBCEntities : DbContext
    {

        [DbFunction("DB_PHBCModel.Store", "sosanhstring")]
        public static bool sosanhstring(string txt, string txt2)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        internal object getDistrictByProvince()
        {
            throw new NotImplementedException();
        }
    }
}