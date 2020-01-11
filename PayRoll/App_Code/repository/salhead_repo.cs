using System;
using System.Diagnostics.CodeAnalysis;

namespace repository
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Salhead_repo
    {
        public string _code, _name, _group, _operatorcode, _formula, _oldname;
        public DateTime _startdate;
        public int _id;

        public Salhead_repo()
        {
            
        }

        public Salhead_repo(string code, string name, string group, string operatorcode, string formula, DateTime startdate)
        {
            this.Code = code;
            this.Name = name;
            this.Group = group;
            this.OperatorCode = operatorcode;
            this.Formula = formula;
            this.Startdate = startdate;
        }

        public Salhead_repo(int id, string code, string name, string group, string operatorcode, string formula, DateTime startdate)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.Group = group;
            this.OperatorCode = operatorcode;
            this.Formula = formula;
            this.Startdate = startdate;
        }

        public Salhead_repo(int id, string code, string name, string group, string operatorcode, string formula, DateTime startdate, string oldname)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.Group = group;
            this.Formula = formula;
            this.OperatorCode = operatorcode;
            this.Oldname = oldname;
            this.Startdate = startdate;
        }


        public string Oldname
        {
            get { return _oldname; }
            set { _oldname = value; }
        }

        public string OperatorCode
        {
            get { return _operatorcode; }
            set { _operatorcode = value; }
        }
        public string Formula
        {
            get { return _formula; }
            set { _formula = value; }
        }

        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public DateTime Startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
