﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalManagementApp_Api.Gateway;
using HospitalManagementApp_Api.Models;

namespace HospitalManagementApp_Api.Controllers
{
    public class ClinicalChartController : Controller
    {
        readonly ClinicalChartGateway _chartGateway=new ClinicalChartGateway();
        public ActionResult Index()
        {            
            return View();
        }
	}
}