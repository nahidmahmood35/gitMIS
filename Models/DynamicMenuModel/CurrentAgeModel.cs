﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalManagementApp_Api.Models.DynamicMenuModel
{
    public class CurrentAgeModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string DateOfBirth { get; set; }
    }
}