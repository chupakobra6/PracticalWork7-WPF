using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Laboratory_work_No._5
{
    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Table> Tables { get; set; }
    }

    public class Table
    {
        public string Name { get; set; }
        public DataTable DataTable { get; set; }
        public ObservableCollection<Column> Columns { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public DataColumn DataColumn { get; set; }
        public DataTable DataTable { get; set; }
    }
}
