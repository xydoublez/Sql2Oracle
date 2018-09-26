using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SfxOracle
{
    public interface IDbHelper
    {
          DataSet Query(string SQLString);
          int ExecuteSql(string SQLString);
    }
}
