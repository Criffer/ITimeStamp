// <auto-generated />
//
// This file was automatically generated by PocosGenerator.csx, inspired from the PetaPoco T4 Template
// Do not make changes directly to this file - edit the PocosGenerator.GenerateClass() method in the PocosGenerator.Core.csx file instead
// 

using System;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace com.apthai.APTimeStamp.Model.APFamilyModel
{
  

   [Table("EmpProfile")]
    public partial class EmpProfile
    {
        public int? EmpID { get; set; }
        public string EmpCode { get; set; }
        public string EmpDeviceID { get; set; }
        public string EmpName { get; set; }
        public string EmpLastName { get; set; }
        public string PositionName { get; set; }
        public string EMail { get; set; }
    }
}