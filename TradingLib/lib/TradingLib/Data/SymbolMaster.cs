using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Data
{
    public partial class SymbolMaster
    {
        private string _symbol;
        private string _issue_name;
        private float _first_date;
        private float _last_date;
        private string _country;
        private string _exchange;
        private byte _num_fields;
        private string _sec_type;
        private short _fn;


        public string Symbol { get { return _symbol; } set { _symbol = value; } }
        public string IssueName { get { return _issue_name; } set { _issue_name = value; } }
        public float FirstDate { get { return _first_date; } set { _first_date = value; } }
        public float LastDate { get { return _last_date; } set { _last_date = value; } }
        public string Country { get { return _country; } set { _country = value; } }
        public string Exchange { get { return _exchange; } set { _exchange = value; } }
        public byte Fields { get { return _num_fields; } set { _num_fields = value; } }
        public string SecType { get { return _sec_type; } set { _sec_type = value; } }
        public short FN {get{return _fn;} set{_fn = value;}}


        

        // Methods
        public SymbolMaster()
        {

        }
        


    }
}
